Imports System.Configuration
Imports System.Data.SqlClient

Public Class recipientchange
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim ftrid As Integer
    Dim iteid As Integer
    Dim stlid As Integer
    Dim p As Point
    Public Sub New(ByVal i As Integer, f As Integer, s As Integer, c As Point)
        ftrid = f
        stlid = s
        iteid = i
        p = c
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub recipientchange_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            Dim cmd As String = "select distinct id,name from tbl_recipients order by 1"
            Using comm As New SqlCommand(cmd, conn)
                conn.Open()
                Dim dt = New DataTable()
                Dim reader As SqlDataReader = comm.ExecuteReader
                dt.Load(reader)
                conn.Close()
                Dim items = dt.AsEnumerable().Select(Function(d) DirectCast(d(1).ToString(), Object)).ToArray()
                ComboBox1.Items.AddRange(items)
            End Using

            Using comm2 As New SqlCommand("select isnull(t.name,0) from tbl_packerordclines ocl left join tbl_recipients t on t.id=ocl.sc_recipient where ocl.stlid=" + stlid.ToString + " and ocl.line=1", conn)
                Using comm3 As New SqlCommand("select code from material where id=" + iteid.ToString, conn)
                    conn.Open()
                    Label3.Text = comm3.ExecuteScalar
                    Dim pv As String = comm2.ExecuteScalar
                    If pv = "0" Then
                        Label5.Text = "-"
                    Else
                        Label5.Text = pv
                    End If
                    conn.Close()
                End Using
            End Using
            p.Offset(-Me.Width \ 2, -Me.Height \ 2)
            Me.Location = p
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Me.Close()

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If Label5.Text = ComboBox1.Items(ComboBox1.SelectedIndex) Then
                Throw New System.Exception("Δεν αλλάξατε αποδέκτη!")
            End If
            Dim comm_cmdtxt As String = ""
            If Label5.Text = "" Then
                comm_cmdtxt = "insert into tbl_packerordclines (stlid,ftrid,line,code,sc_recipient) values (@stlid,@ftrid,1,@code,@sc_recipient)"
            Else
                comm_cmdtxt = "update tbl_packerordclines set sc_recipient=@sc_recipient where stlid=@stlid"
            End If
            Using comm As New SqlCommand(comm_cmdtxt, updconn)
                Using cmd As New SqlCommand("dbo.notifier_stlid", updconn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@SID", stlid)
                    cmd.Parameters.AddWithValue("@FTRID", ftrid)
                    cmd.Parameters.AddWithValue("@ITEID", iteid)
                    cmd.Parameters.AddWithValue("@prevrcpnt", Label5.Text)
                    cmd.Parameters.AddWithValue("@newrcpnt", ComboBox1.Items(ComboBox1.SelectedIndex))
                    cmd.Parameters.AddWithValue("@user", Form1.activeuser)
                    comm.Parameters.AddWithValue("@stlid", stlid)
                    comm.Parameters.AddWithValue("sc_recipient", (ComboBox1.SelectedIndex + 1))
                    If Label5.Text = "" Then
                        comm.Parameters.AddWithValue("@ftrid", ftrid)
                        comm.Parameters.AddWithValue("@code", stlid.ToString + "/1")
                    End If
                    updconn.Open()
                    Dim success = comm.ExecuteNonQuery()
                    If success <= 0 Then
                        Label6.ForeColor = Color.Red
                        Label6.Text = "Πρόβλημα"
                        Label6.Visible = True
                    Else
                        Label6.ForeColor = Color.Green
                        Label6.Text = "Επιτυχία"
                        Label6.Visible = True
                        cmd.ExecuteNonQuery()
                    End If
                End Using
            End Using
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Me.Close()

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
End Class