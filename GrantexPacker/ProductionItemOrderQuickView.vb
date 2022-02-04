Imports System.Configuration
Imports System.Data.SqlClient
Public Class ProductionItemOrderQuickView
    Dim p = GetType(Zuby.ADGV.AdvancedDataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Private Sub ProductionItemOrderQuickView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        p.SetValue(Me.DataGridView1, True, Nothing)
        For Each c As Control In Panel1.Controls
            AddHandler c.MouseClick, AddressOf ClickHandler
        Next
        For Each c As Control In Me.Controls
            AddHandler c.MouseClick, AddressOf ClickHandler
        Next
        Timer1.Interval = 1000
        Label4.Text = "30"
        Timer1.Start()
        SetLocation(xloc, yloc)
        Panel1.Hide()
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        Me.Controls.Add(pic)
        pic.Dock = DockStyle.Fill
        If FormIsLoadingType Then
            CheckBox2.Visible = False
            CheckBox1.Visible = False
            DataGridView1.Columns("itemorderdetails").Visible = False
            DataGridView1.Columns("work").HeaderText = "->Daily Plan"
            Dim lbl As New Label
            lbl.Text = "Επιλέξτε κωδικό είδους που θέλετε να εισάγετε στο Daily Plan"
            lbl.ForeColor = Color.Red
            lbl.Location = CheckBox2.Location
            lbl.AutoSize = True
            Panel1.Controls.Add(lbl)
            Label2.Location = CheckBox1.Location
        End If
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
                ElseIf ActiveScreen.WorkingArea.left > p.X Then
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

    Dim itecode As String
    Dim yloc As Integer
    Dim xloc As Integer
    Dim timercounter As Integer = 0
    Dim retvalues As String = ""
    Dim sd As String = ""
    Dim ed As String = ""
    Dim FormIsLoadingType As Boolean = False


    Private Sub ClickHandler(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        timercounter = 0
    End Sub

    Public Sub New(ByVal r As String, ByVal i As String, x As Integer, y As Integer, Optional startdate As String = "", Optional enddate As String = "", Optional LoadingMode As Boolean = False)

        ' This call is required by the designer.
        InitializeComponent()
        itecode = i
        retvalues = r
        xloc = x
        yloc = y
        sd = startdate
        ed = enddate
        FormIsLoadingType = LoadingMode
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        ' Add any initialization after the InitializeComponent() call.

    End Sub

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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub MainWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MainWorker.DoWork
        Try

            Dim DatesStr As String = " AND 1=1 "
            If sd <> "" Then
                DatesStr = " AND ST.M_DISPATCHDATE >=CONVERT(datetime, '" + sd + "', 103) AND ST.M_DISPATCHDATE<=CONVERT(datetime, '" + ed + "', 103) "

            Else

            End If
            Using dt = New DataTable()
                Dim cmd As String = ""
                If CheckBox2.Checked And Not CheckBox1.Checked Then
                    cmd = "SELECT f.id ftrid,isnull(c.fathername,'???')+' '+DBO.GET_TRADECODE(F.ID) tradecode,f.ftrdate,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,sum(CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END) QTY, sum(ISNULL(z.quantity, 0))  DISTRIBED
,sum(z2.blue) blue,sum(z2.black) black ,sum(z2.lightgreen) lightgreen,sum(z2.green) green,sum(z2.gold) gold,sum(z2.BACKORDER) BACKORDER
FROM FINTRADE F INNER JOIN TBL_PACKERORDERCHECK T1 ON T1.FTRID=F.ID INNER JOIN STORETRADELINES S ON S.FTRID=F.ID 
INNER JOIN STORETRADE ST ON ST.FTRID=F.ID
INNER JOIN MATERIAL M ON M.ID=S.ITEID LEFT JOIN TBL_PACKERORDCLINES T ON T.STLID=S.ID AND T.LINE=1
left join customer c on c.id=f.cusid
LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join tbl_recipients t2 on t2.id=t.sc_recipient
left join TBL_PACKERORDERSTATUS t3 on t3.id=t1.status LEFT OUTER JOIN
                         dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID inner join z_packer_itemsbrowser z2 on z2.STLID=S.ID
WHERE  M.SUBCODE1 LIKE '" + itecode + "%' AND F.DSRID=9000 AND T1.STATUS<12 AND T1.STATUS>=6 and t.sc_recipient in (" + retvalues + ") " + DatesStr + "
GROUP BY  f.id,DBO.GET_TRADECODE(F.ID),ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,f.ftrdate,c.fathername
ORDER BY 4 ASC"
                ElseIf Not CheckBox2.Checked And Not CheckBox1.Checked Then
                    cmd = "SELECT f.id ftrid,isnull(c.fathername,'???')+' '+DBO.GET_TRADECODE(F.ID) tradecode,f.ftrdate,m.id iteid,s.id stlid,m.subcode1,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,sum(CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END) QTY, sum(ISNULL(z.quantity, 0))  DISTRIBED
,sum(z2.blue) blue,sum(z2.black) black ,sum(z2.lightgreen) lightgreen,sum(z2.green) green,sum(z2.gold) gold,sum(z2.BACKORDER) BACKORDER
FROM FINTRADE F INNER JOIN TBL_PACKERORDERCHECK T1 ON T1.FTRID=F.ID INNER JOIN STORETRADELINES S ON S.FTRID=F.ID 
INNER JOIN STORETRADE ST ON ST.FTRID=F.ID
INNER JOIN MATERIAL M ON M.ID=S.ITEID LEFT JOIN TBL_PACKERORDCLINES T ON T.STLID=S.ID AND T.LINE=1
left join customer c on c.id=f.cusid
LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join tbl_recipients t2 on t2.id=t.sc_recipient
left join TBL_PACKERORDERSTATUS t3 on t3.id=t1.status  LEFT OUTER JOIN
                         dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID inner join z_packer_itemsbrowser z2 on z2.STLID=S.ID
WHERE  M.SUBCODE1 LIKE '" + itecode + "%' AND F.DSRID=9000 AND T1.STATUS<12 AND T1.STATUS>=6 and t.sc_recipient in (" + retvalues + ") " + DatesStr + "
GROUP BY  f.id,DBO.GET_TRADECODE(F.ID),m.id,s.id,m.subcode1,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,f.ftrdate,c.fathername
ORDER BY 7 ASC"
                ElseIf Not CheckBox2.Checked And CheckBox1.Checked Then
                    cmd = "SELECT f.id ftrid,isnull(c.fathername,'???')+' '+DBO.GET_TRADECODE(F.ID) tradecode,f.ftrdate,m.id iteid,s.id stlid,m.subcode1,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,sum(CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END) QTY, sum(ISNULL(z.quantity, 0))  DISTRIBED
,sum(z2.blue) blue,sum(z2.black) black ,sum(z2.lightgreen) lightgreen,sum(z2.green) green,sum(z2.gold) gold,sum(z2.BACKORDER) BACKORDER
FROM FINTRADE F INNER JOIN TBL_PACKERORDERCHECK T1 ON T1.FTRID=F.ID INNER JOIN STORETRADELINES S ON S.FTRID=F.ID 
INNER JOIN STORETRADE ST ON ST.FTRID=F.ID
INNER JOIN MATERIAL M ON M.ID=S.ITEID LEFT JOIN TBL_PACKERORDCLINES T ON T.STLID=S.ID AND T.LINE=1
left join customer c on c.id=f.cusid
LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join tbl_recipients t2 on t2.id=t.sc_recipient
left join TBL_PACKERORDERSTATUS t3 on t3.id=t1.status  LEFT OUTER JOIN
                         dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID inner join z_packer_itemsbrowser z2 on z2.STLID=S.ID
WHERE  M.SUBCODE1 LIKE '" + itecode + "%' AND F.DSRID=9000 AND T1.STATUS<12 AND T1.STATUS>=6  " + DatesStr + "
GROUP BY  f.id,DBO.GET_TRADECODE(F.ID),m.id,s.id,m.subcode1,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,f.ftrdate,c.fathername
ORDER BY 7 ASC"
                ElseIf CheckBox2.Checked And CheckBox1.Checked Then
                    cmd = "SELECT f.id ftrid,isnull(c.fathername,'???')+' '+DBO.GET_TRADECODE(F.ID) tradecode,f.ftrdate,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,sum(CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END) QTY, sum(ISNULL(z.quantity, 0))  DISTRIBED
,sum(z2.blue) blue,sum(z2.black) black ,sum(z2.lightgreen) lightgreen,sum(z2.green) green,sum(z2.gold) gold,sum(z2.BACKORDER) BACKORDER
FROM FINTRADE F INNER JOIN TBL_PACKERORDERCHECK T1 ON T1.FTRID=F.ID INNER JOIN STORETRADELINES S ON S.FTRID=F.ID 
INNER JOIN STORETRADE ST ON ST.FTRID=F.ID
INNER JOIN MATERIAL M ON M.ID=S.ITEID LEFT JOIN TBL_PACKERORDCLINES T ON T.STLID=S.ID AND T.LINE=1
left join customer c on c.id=f.cusid
LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join tbl_recipients t2 on t2.id=t.sc_recipient
left join TBL_PACKERORDERSTATUS t3 on t3.id=t1.status  LEFT OUTER JOIN
                         dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID inner join z_packer_itemsbrowser z2 on z2.STLID=S.ID
WHERE  M.SUBCODE1 LIKE '" + itecode + "%' AND F.DSRID=9000 AND T1.STATUS<12 AND T1.STATUS>=6  " + DatesStr + "
GROUP BY  f.id,DBO.GET_TRADECODE(F.ID),ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,f.ftrdate,c.fathername
ORDER BY 4 ASC"
                End If
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
    Private Sub ProductionSearchForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            If Not TryCast(Me.Owner, production).Timer1.Enabled Then
                TryCast(Me.Owner, production).Timer1.Start()
            End If
        Catch
        End Try
    End Sub

    Public ResultRow As DataRow
    Dim DT As New DataTable()

    Private Sub MainWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MainWorker.RunWorkerCompleted
        DT = e.Result
        If sd <> "" Then
            Label1.Text = itecode + " Περίοδος " + sd + " έως " + ed
        Else
            Label1.Text = itecode
        End If
        'Label2.Text = DT(0)(0)
        Dim sum As Double = 0
        Dim dsum As Double = 0
        Dim col As New DataGridViewButtonColumn
        col.Name = "distribed"
        col.HeaderText = "Σε Παλέτες"
        If Not DataGridView1.Columns.Contains(col.Name) Then
            DataGridView1.Columns.Add(col)
        End If
        Dim col2 As New DatagridviewStackedProgressColumn
        col2.Name = "Status"
        If Not DataGridView1.Columns.Contains(col2.Name) Then
            DataGridView1.Columns.Add(col2)
        End If
        If CheckBox2.Checked Then
            DataGridView1.Columns("item").Visible = False
            For Each r As DataRow In DT.Rows
                DataGridView1.Rows.Add(New String() {r("ftrid"), r("tradecode"), r("ftrdate"), "", "", "", "", "", r("M_DISPATCHDATE"), r("status"), r("NAME"), r("QTY"), r("DISTRIBED"), r("BACKORDER").ToString + "/" + r("black").ToString + "/" + r("blue").ToString + "/" + r("LIGHTGREEN").ToString + "/" + r("green").ToString + "/" + r("gold").ToString})
                sum += r("QTY")
                dsum += r("DISTRIBED")
                If FormIsLoadingType Then
                    If r("BLACK") = 0 Then
                        DataGridView1.Rows(DT.Rows.IndexOf(r)).Cells("work").Style.ForeColor = Color.LightGray
                        DataGridView1.Rows(DT.Rows.IndexOf(r)).Cells("work").Style.SelectionForeColor = Color.LightGray
                    End If
                End If
            Next
        Else
            DataGridView1.Columns("item").Visible = True
            For Each r As DataRow In DT.Rows
                DataGridView1.Rows.Add(New String() {r("ftrid"), r("tradecode"), r("ftrdate"), r("iteid"), r("stlid"), r("subcode1"), "", "", r("M_DISPATCHDATE"), r("status"), r("NAME"), r("QTY"), r("DISTRIBED"), r("BACKORDER").ToString + "/" + r("black").ToString + "/" + r("blue").ToString + "/" + r("LIGHTGREEN").ToString + "/" + r("green").ToString + "/" + r("gold").ToString})
                sum += r("QTY")
                dsum += r("DISTRIBED")
                If FormIsLoadingType Then
                    If r("BLACK") = 0 Then
                        DataGridView1.Rows(DT.Rows.IndexOf(r)).Cells("work").Style.ForeColor = Color.LightGray
                        DataGridView1.Rows(DT.Rows.IndexOf(r)).Cells("work").Style.SelectionForeColor = Color.LightGray
                    End If
                End If
            Next
        End If
        For Each r As DataGridViewRow In DataGridView1.Rows
            If r.Cells("quantity").Value = r.Cells("distribed").Value Then
                r.DefaultCellStyle.BackColor = Color.LightGreen
                r.DefaultCellStyle.SelectionBackColor = Color.LightGreen
            End If
        Next
        Label2.Text = "Σύνολο:" + (sum + +dsum).ToString + " Σε παλέτες:" + dsum.ToString + " Υπόλοιπο:" + sum.ToString
        Panel1.Visible = True
        DataGridView1.Visible = True
        For Each c As Control In Me.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
        For Each c As Control In Panel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
    End Sub
    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        If DataGridView1.Columns(e.ColumnIndex).Name = "tradecode" And e.RowIndex >= 0 Then
            Using f As New Order(DataGridView1.Rows(e.RowIndex).Cells("ftrid").Value)
                f.ShowDialog()
            End Using
        ElseIf DataGridView1.Columns(e.ColumnIndex).Name = "item" And e.RowIndex >= 0 Then
            Using f As New ItemDetails(DataGridView1.Rows(e.RowIndex).Cells("iteid").Value)
                f.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim senderGrid = DirectCast(sender, DataGridView)

        If DataGridView1.Columns(e.ColumnIndex).Name = "itemorderdetails" AndAlso
           e.RowIndex >= 0 Then
            If String.IsNullOrWhiteSpace((DataGridView1.Rows(e.RowIndex).Cells("item").Value)) Then
                Dim f As New ProductionItemQuickReport(DataGridView1.Rows(e.RowIndex).Cells(0).Value, Label1.Text, Cursor.Position.X, Cursor.Position.Y, retvalues)
                f.Owner = Me.Owner
                f.Show()
            Else
                Dim f As New ProductionItemQuickReport(DataGridView1.Rows(e.RowIndex).Cells(0).Value, DataGridView1.Rows(e.RowIndex).Cells("item").Value, Cursor.Position.X, Cursor.Position.Y, retvalues)
                f.Owner = Me.Owner
                f.Show()
            End If
        ElseIf DataGridView1.Columns(e.ColumnIndex).Name = "work" AndAlso
           e.RowIndex >= 0 Then
            If FormIsLoadingType Then
                If Not DT.Rows(e.RowIndex).Item("BLACK") > 0 Then
                    Return
                End If
                ResultRow = DT.Rows(e.RowIndex)
                Me.DialogResult = DialogResult.OK
            Else
                Form1.Button8.PerformClick()
                Form1.DateTimePicker1.Value = DataGridView1.Rows(e.RowIndex).Cells("ftrdate").Value
                Dim strarr() As String
                strarr = DataGridView1.Rows(e.RowIndex).Cells("tradecode").Value.Split(" "c)
                Form1.TextBox1.Text = strarr(strarr.Length - 1)
                Form1.Button1.PerformClick()
                Form1.TopMost = True
                Form1.TopMost = False
            End If

        ElseIf DataGridView1.Columns(e.ColumnIndex).Name = "distribed" AndAlso DataGridview1.columns(e.columnindex).GetType = GetType(DataGridViewButtonColumn) Then
            If e.RowIndex >= 0 Then
                Dim lst As New Dictionary(Of String, String)
                lst.Add("subcode1", itecode)
                Dim lst2 As New Dictionary(Of String, Integer)
                lst2.Add("ftrid", DataGridView1.Rows(e.RowIndex).Cells("ftrid").Value)
                Using f As New ItemDistribution(Cursor.Position.X, Cursor.Position.Y, string_parameters_:=lst, integer_parameters_equal_:=lst2)
                    f.ShowDialog()
                End Using
            End If
        End If
    End Sub

    Private Const cGrip As Integer = 16
    Private Const cCaption As Integer = 32
    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point





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

    Private Sub DataGridView1_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellMouseEnter
        If DataGridView1.Columns(e.ColumnIndex).Name = "tradecode" Or DataGridView1.Columns(e.ColumnIndex).Name = "item" Then
            Me.DataGridView1.Cursor = Cursors.Hand
        Else
            Me.DataGridView1.Cursor = Cursors.Default
        End If

    End Sub

    Private Sub DataGridView1_CellMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellMouseLeave
        Me.DataGridView1.Cursor = Cursors.Default
    End Sub



    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If Not MainWorker.IsBusy Then
            DataGridView1.Rows.Clear()
            DataGridView1.Columns.Remove("distribed")
            DataGridView1.Columns.Remove("Status")
            DataGridView1.Visible = False
            Dim pic As New PictureBox
            pic.Image = My.Resources.rolling
            pic.SizeMode = PictureBoxSizeMode.CenterImage
            Panel1.Controls.Add(pic)
            pic.Dock = DockStyle.Fill
            MainWorker.RunWorkerAsync()
        End If
    End Sub

    Private Sub Label4_TextChanged(sender As Object, e As EventArgs) Handles Label4.TextChanged
        If CInt(Label4.Text) < 10 Then
            Label4.ForeColor = Color.Firebrick
        Else
            Label4.ForeColor = SystemColors.ButtonShadow
        End If
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If Not MainWorker.IsBusy Then
            DataGridView1.Rows.Clear()
            DataGridView1.Columns.Remove("distribed")
            DataGridView1.Columns.Remove("Status")
            DataGridView1.Visible = False
            Dim pic As New PictureBox
            pic.Image = My.Resources.rolling
            pic.SizeMode = PictureBoxSizeMode.CenterImage
            Panel1.Controls.Add(pic)
            pic.Dock = DockStyle.Fill
            MainWorker.RunWorkerAsync()
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If DataGridView1.Columns(e.ColumnIndex).Name = "Status" Then
            If DataGridView1.Rows(e.RowIndex).Cells("item").Value <> "" Then
                Dim f As New DatagridviewStackedProgressColumnReportForm(0, DataGridView1.Rows(e.RowIndex).Cells("item").Value, "", Cursor.Position.X, Cursor.Position.Y, DataGridView1.Rows(e.RowIndex).Cells("ftrid").Value, retvalues, "ftrid")
                f.Owner = Me
                f.Show()
            Else
                Dim f As New DatagridviewStackedProgressColumnReportForm(0, itecode, "", Cursor.Position.X, Cursor.Position.Y, DataGridView1.Rows(e.RowIndex).Cells("ftrid").Value, retvalues, "ftrid")
                f.Owner = Me
                f.Show()
            End If

        End If
    End Sub
End Class