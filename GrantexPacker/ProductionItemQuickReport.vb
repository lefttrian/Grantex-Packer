Imports System.Configuration
Imports System.Data.SqlClient

Public Class ProductionItemQuickReport
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub ProductionItemQuickReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each c As Control In Panel1.Controls
            AddHandler c.MouseClick, AddressOf ClickHandler
        Next
        For Each c As Control In Me.Controls
            AddHandler c.MouseClick, AddressOf ClickHandler
        Next
        SetLocation(xloc, yloc)
        Panel1.Hide()
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        Me.Controls.Add(pic)
        pic.Dock = DockStyle.Fill
        Timer1.Interval = 1000
        Label4.Text = "30"
        Timer1.Start()
        LockUIAccess(Me)
        MainWorker.RunWorkerAsync()
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

    Dim ftrid As Integer
    Dim itecode As String
    Dim retvalues As String = ""
    Dim yloc As Integer
    Dim xloc As Integer


    Private Sub ClickHandler(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        timercounter = 0
    End Sub

    Public Sub New(ByVal i As Integer, c As String, x As Integer, y As Integer, r As String)

        ' This call is required by the designer.
        InitializeComponent()
        ftrid = i
        itecode = c
        retvalues = r
        xloc = x
        yloc = y
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub MainWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MainWorker.DoWork
        Try
            Using dt = New DataTable()
                Dim cmd As String = "SELECT isnull(c.fathername,'???')+' '+DBO.GET_TRADECODE(F.ID),S.ID AS STLID,S.ITEID,CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END PRIMARYQTY ,M.SUBCODE1,M.DESCRIPTION, isnull(t2.codeid,'') sc_recipient,ISNULL(z.quantity, 0)  DISTRIBED
                            ,(z2.blue) blue,(z2.black) black ,(z2.lightgreen) lightgreen,(z2.green) green,(z2.gold) gold,(z2.BACKORDER) BACKORDER FROM FINTRADE F INNER JOIN STORETRADELINES S ON S.FTRID=F.ID INNER JOIN MATERIAL M ON M.ID=S.ITEID LEFT JOIN TBL_PACKERORDCLINES T ON T.STLID=S.ID AND T.LINE=1 left join customer c on c.id=f.cusid
                            LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join tbl_recipients t2 on t2.id=t.sc_recipient  LEFT OUTER JOIN
                         dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID inner join z_packer_itemsbrowser z2 on z2.STLID=S.ID
                            WHERE F.ID=" + ftrid.ToString + " AND M.SUBCODE1 LIKE '" + itecode + "%' and t.sc_recipient in (" + retvalues + ") "
                Using command As SqlCommand = New SqlCommand(cmd, conn)
                    conn.Open()
                    Using reader As SqlDataReader = command.ExecuteReader
                        dt.Load(reader)
                    End Using
                    conn.Close()
                End Using
                e.Result = dt
            End Using
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub MainWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MainWorker.RunWorkerCompleted
        Try
            Using DT As DataTable = e.Result

                Label1.Text = itecode
                Label2.Text = DT(0)(0)
                Dim col2 As New DatagridviewStackedProgressColumn
                col2.Name = "Status"
                DataGridView1.Columns.Add(col2)
                For Each r As DataRow In DT.Rows
                    Me.DataGridView1.Rows.Add(New String() {r("SUBCODE1"), r("iteid"), r("stlid"), r("SC_RECIPIENT"), r("PRIMARYQTY"), r("DISTRIBED"), r("BACKORDER").ToString + "/" + r("black").ToString + "/" + r("blue").ToString + "/" + r("LIGHTGREEN").ToString + "/" + r("green").ToString + "/" + r("gold").ToString})
                Next
                Panel1.Visible = True
                For Each c As Control In Me.Controls
                    If Not IsNothing(TryCast(c, PictureBox)) Then
                        c.Dispose()
                    End If
                Next
            End Using
            For Each r As DataGridViewRow In DataGridView1.Rows
                If r.Cells("quantity").Value = r.Cells("distribed").Value Then
                    r.DefaultCellStyle.BackColor = Color.LightGreen
                    r.DefaultCellStyle.SelectionBackColor = Color.LightGreen
                End If
            Next
            Using c As SqlCommand = New SqlCommand("SELECT REMARKS FROM PKRTBL_PRODUCTION WHERE CODE='" + itecode + "' AND FTRID=" + ftrid.ToString, conn)
                conn.Open()
                TextBox1.Text = c.ExecuteScalar
                conn.Close()
            End Using
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Using f As New Order(ftrid)
            f.ShowDialog()

        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

#Region " Move Form "

    ' [ Move Form ]
    '
    ' // By Elektro 
    Private Const cGrip As Integer = 16
    Private Const cCaption As Integer = 32
    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point

    Private Sub ProductionSearchForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not TryCast(Me.Owner, production).Timer1.Enabled Then
            TryCast(Me.Owner, production).Timer1.Start()
        End If
    End Sub



    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles _
    Panel1.MouseDown ' Add more handles here (Example: PictureBox1.MouseDown)

        If e.Button = MouseButtons.Left Then
            MoveForm = True
            Me.Cursor = Cursors.NoMove2D
            MoveForm_MousePosition = e.Location
        End If

    End Sub

    Public Sub MoveForm_MouseMove(sender As Object, e As MouseEventArgs) Handles _
    Panel1.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)

        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If

    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles _
    Panel1.MouseUp ' Add more handles here (Example: PictureBox1.MouseUp)

        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If

    End Sub



    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        If e.ColumnIndex = 0 And e.RowIndex >= 0 Then
            Using f As New ItemDetails(DataGridView1.Rows(e.RowIndex).Cells("iteid").Value)
                f.ShowDialog()
            End Using
        End If
    End Sub


    Private Sub DataGridView1_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellMouseEnter
        If e.ColumnIndex = 0 Then
            Me.DataGridView1.Cursor = Cursors.Hand
        Else
            Me.DataGridView1.Cursor = Cursors.Default
        End If

    End Sub

    Private Sub DataGridView1_CellMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellMouseLeave
        Me.DataGridView1.Cursor = Cursors.Default
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If DataGridView1.Columns(e.ColumnIndex).Name = "distribed" Then
            Dim senderGrid = DirectCast(sender, DataGridView)
            If e.RowIndex >= 0 Then
                Dim lst As New Dictionary(Of String, Integer)
                lst.Add("stlid", CInt(DataGridView1.Rows(e.RowIndex).Cells("stlid").Value))
                Using f As New ItemDistribution(Cursor.Position.X, Cursor.Position.Y, integer_parameters_equal_:=lst)
                    f.ShowDialog()
                End Using
            End If
        End If
    End Sub

    Dim timercounter As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Me.OwnedForms.Count = 0 Then
            timercounter += 1
        End If
        Label4.Text = (30 - timercounter).ToString
        If timercounter = 30 Then
            Me.Close()
            Me.Dispose()
        End If

    End Sub

    Private Sub Label4_TextChanged(sender As Object, e As EventArgs) Handles Label4.TextChanged
        If CInt(Label4.Text) < 10 Then
            Label4.ForeColor = Color.Firebrick
        Else
            Label4.ForeColor = SystemColors.ButtonShadow
        End If
    End Sub

    Private Sub NoPaddingButton1_Click(sender As Object, e As EventArgs) Handles NoPaddingButton1.Click
        Try
            ' If Not String.IsNullOrWhiteSpace(TextBox1.Text) Then
            Dim txt As String = ""
            If String.IsNullOrWhiteSpace(TextBox1.Text) Then
                txt = "| LAST TEXT EDIT: " + Form1.activeuser + ", " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + " |"
            Else
                txt = StrRep(TextBox1.Text) + "| LAST TEXT EDIT: " + Form1.activeuser + ", " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + " |"
            End If

            Using cmd As New SqlCommand("PKRPRC_PRODUCTION_UPDATE", updconn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@R", SqlDbType.Text).Value = txt
                cmd.Parameters.Add("@CODE", SqlDbType.VarChar).Value = itecode
                cmd.Parameters.Add("@FTRID", SqlDbType.Int).Value = ftrid
                updconn.Open()

                If cmd.ExecuteNonQuery() > 0 Then
                    Label5.Text = "Επιτυχής αποθήκευση!"
                    Label5.ForeColor = Color.Green
                    Label5.Visible = True
                    TextBox1.Text = txt
                Else
                    Label5.Text = "Κάτι δεν πήγε καλά!"
                    Label5.ForeColor = Color.Red
                    Label5.Visible = True
                End If
                updconn.Close()
            End Using
            'End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.S AndAlso e.Modifiers = Keys.Control Then
            NoPaddingButton1.PerformClick()
        End If
    End Sub

    Private Function StrRep(ByVal xstr As String)

        Dim xst As Integer = xstr.IndexOf("| LAST")
        Dim xend As Integer = xstr.IndexOf(" |")
        If xst = -1 Then

            Return xstr
        End If

        Dim xsub As String = xstr.Substring(xst, (xend - xst) + 2)
        xstr = xstr.Replace(xsub, String.Empty)

        Return xstr

    End Function

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If DataGridView1.Columns(e.ColumnIndex).Name = "Status" Then
            Dim f As New DatagridviewStackedProgressColumnReportForm(0, DataGridView1.Rows(e.RowIndex).Cells("item").Value, "", Cursor.Position.X, Cursor.Position.Y, DataGridView1.Rows(e.RowIndex).Cells("stlid").Value, retvalues, "stlid")
            f.Owner = Me
            f.Show()
        End If
    End Sub

#End Region
End Class