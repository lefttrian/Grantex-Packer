Imports System.Data.SqlClient
Imports System.Configuration

Public Class SmallSearchBox
    Dim xloc, yloc As Integer
    Dim maintable As String
    Dim fields As New Dictionary(Of String, List(Of String))
    Dim notfirstload As Boolean = False
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)

    Public Sub New(ByVal x As Integer, y As Integer, main_table As String, fieldlist As Dictionary(Of String, List(Of String))) '(FIELD_TITLE,(RELATIONSHIP1FOREIGNTABLE,R1FT_FKEY,R1FT_PKEY,RELATIONSHIP2FOREIGNTABLE,R2FT_FKEY,R2PRIMARYTABLE,R2FT_PKEY,WHEREFIELD))
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        xloc = x
        yloc = y
        fields = fieldlist
        maintable = main_table

    End Sub
    Private Sub SmallSearchBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetLocation(xloc, yloc)
        ComboBox1.DisplayMember = "Key"
        ComboBox1.ValueMember = "Value"
        ComboBox1.DataSource = New BindingSource(fields, Nothing)
        notfirstload = True
    End Sub

    Private Sub SetLocation(ByVal x As Integer, y As Integer)
        Dim formTopLeft As New Point(x, y)
        Dim formTopRight As New Point(x + Me.Width, y)
        Dim formBottomleft As New Point(x, y + Me.Height)
        Dim formBottomRight As New Point(x + Me.Width, y + Me.Height)
        Dim l As New List(Of Point) From {formTopLeft, formTopRight, formBottomleft, formBottomRight}
        Dim ActiveScreen As Screen = Screen.FromControl(Me)
        Dim xloc As Integer = x
        Dim yloc As Integer = y
        For Each p As Point In l
            If Not ActiveScreen.WorkingArea.Contains(p) Then
                If ActiveScreen.WorkingArea.Right < p.X Then
                    xloc = ActiveScreen.WorkingArea.Right - Me.Width
                ElseIf ActiveScreen.WorkingArea.Left > p.X Then
                    xloc = ActiveScreen.WorkingArea.Left
                ElseIf ActiveScreen.WorkingArea.Bottom < p.Y Then
                    yloc = ActiveScreen.WorkingArea.Bottom - Me.Height
                ElseIf ActiveScreen.WorkingArea.Top > p.Y Then
                    yloc = ActiveScreen.WorkingArea.Top
                End If
            End If
        Next
        Me.SetDesktopLocation(xloc, yloc)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            PictureBox1.Visible = False
            Dim r1table As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(0)
            Dim r1tablekey As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(1)
            Dim r1primarykey As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(2)
            Dim r2table As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(3)
            Dim r2tablekey As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(4)
            Dim r2jointablekey As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(5)
            Dim field As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(6)
            Dim EXTRAWHERE As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(7)
            Dim selection As String = TryCast(ComboBox1.SelectedValue, List(Of String)).Item(8)
            If selection = "" Then
                selection = "*"
            End If
            Dim EXTRATEXT1 As String = ""
            Dim EXTRATEXT2 As String = ""
            If r2table <> "" Then
                EXTRATEXT2 = " INNER JOIN " + r2table + " ON " + r2table + "." + r2tablekey + "=" + r2jointablekey
            End If
            If r1table <> "" Then
                EXTRATEXT1 = " INNER JOIN " + r1table + " ON " + r1table + "." + r1tablekey + "=" + maintable + "." + r1primarykey
            End If
            Dim s As String = "SELECT " + selection + " FROM " + maintable + EXTRATEXT1 + EXTRATEXT2 + " WHERE " + field + " LIKE '%'+@TEXT+'%' " + EXTRAWHERE

            Using sc As New SqlCommand(s, conn)
                sc.Parameters.AddWithValue("@text", TextBox1.Text.Replace("*", "%"))
                Using datatable As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = sc.ExecuteReader
                        datatable.Load(reader)
                        conn.Close()
                    End Using
                    If datatable.Rows.Count = 0 Then
                        PictureBox1.Visible = True
                    Else
                        Dim f As New InformationPanelGenericDialog("Αποτελέσματα αναζήτησης:", datatable, MousePosition.X, MousePosition.Y)
                        f.Owner = Me.Owner
                        f.Show()
                    End If
                End Using
            End Using
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub SmallSearchBox_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        Me.Close()
        Me.Dispose()
    End Sub
End Class