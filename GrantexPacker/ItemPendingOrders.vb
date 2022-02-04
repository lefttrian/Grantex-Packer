
Imports System.Configuration
Imports System.Data.SqlClient
Public Class ItemPendingOrders
    Dim p = GetType(Zuby.ADGV.AdvancedDataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim string_parameters As New Dictionary(Of String, String)
    Dim integer_parameters As New Dictionary(Of String, Integer)
    Dim integer_parameters_not_equal As New Dictionary(Of String, Integer)
    Dim list_parameters As New Dictionary(Of String, String)
    Dim x, y As Integer
    Public Sub New(ByVal x_ As Integer, y_ As Integer, Optional string_parameters_ As Dictionary(Of String, String) = Nothing, Optional integer_parameters_equal_ As Dictionary(Of String, Integer) = Nothing, Optional integer_parameters_not_equal_ As Dictionary(Of String, Integer) = Nothing, Optional list_parameters_ As Dictionary(Of String, String) = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        string_parameters = string_parameters_
        integer_parameters = integer_parameters_equal_
        list_parameters = list_parameters_
        integer_parameters_not_equal = integer_parameters_not_equal_
        x = x_
        y = y_
        ' Add any initialization after the InitializeComponent() call.
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

    Private Sub ItemDistribution_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        doWork()
        Timer1.Start()
    End Sub
    Private Sub doublebufferedTableLayoutPanel1_Click(sender As Object, e As EventArgs) Handles DoubleBufferedTableLayoutPanel1.Click, Panel1.Click, Panel2.Click, AdvancedDataGridView1.Click, CustomDGVSearchBox1.Click
        timercounter = 0
    End Sub
    Private Sub doWork()
        AdvancedDataGridView1.Visible = False
        AdvancedDataGridView1.DataSource = Nothing
        SetLocation(x, y)
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        DoubleBufferedTableLayoutPanel1.Controls.Add(pic, 0, 2)
        pic.Dock = DockStyle.Fill
        mainworker.RunWorkerAsync()
    End Sub

    Private Sub CustomDGVSearchBox1_ButtonPressed() Handles CustomDGVSearchBox1.ButtonPressed
        CustomDGVSearchBox1.continue_click()
    End Sub

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted
        AdvancedDataGridView1.DataSource = e.Result
        For Each c As Control In DoubleBufferedTableLayoutPanel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
        Dim col2 As New DatagridviewStackedProgressColumn
        col2.Name = "Status"
        If Not AdvancedDataGridView1.Columns.Contains(col2.Name) Then
            AdvancedDataGridView1.Columns.Add(col2)
            AdvancedDataGridView1.Columns("Status").DisplayIndex = 4
        End If
        For Each r As DataGridViewRow In AdvancedDataGridView1.Rows
            r.Cells("Status").Value = r.Cells("red").Value.ToString + "/" + r.Cells("black").Value.ToString + "/" + r.Cells("blue").Value.ToString + "/" + r.Cells("lightgreen").Value.ToString + "/" + r.Cells("green").Value.ToString + "/" + r.Cells("gold").Value.ToString
        Next
        AdvancedDataGridView1.Visible = True
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
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)
        doWork()
    End Sub

    Dim hiddenCols As New List(Of String) From {"PALLETID", "iteid", "STLID", "FTRID", "spciteid", "red", "blue", "black", "lightgreen", "green", "gold"}
    Dim ColNames As New Dictionary(Of String, String) From {{"PALLETCODE", "ΠΑΛΕΤΑ"}, {"itecode", "ΚΥΡΙΟ ΕΙΔΟΣ"}, {"speccode", "Παρελκόμενο"}, {"specq", "Ποσ Πρλκμν"}, {"apoth", "Α"}, {"parag", "Π"}, {"apothparag", "Α-Π"}, {"m_dispatchdate", "ΗΜ/ΝΙΑ ΑΠ"}, {"apothsusk", "Α-Σ"}, {"paragapoth", "Π-Α"}, {"unknown", "Άγνωστος"}, {"CODE", "ΕΙΔΟΣ"}, {"ORDSTATUS", "ΚΑΤΑΣΤΑΣΗ ΠΑΡ"}, {"q", "ΠΟΣ"}, {"WEEKNUM", "ΕΒΔΟΜΑΔΑ"}, {"YR", "ΕΤΟΣ"}, {"TRADECODE", "ΠΑΡ"}}

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork
        Try
            Dim dt As New DataTable()
            Dim selecttxt As String = ""
            Dim grouptxt As String = ""
            selecttxt = " stl.FTRID,STL.STLID,m.id iteid,m.subcode1 itecode,dbo.get_tradecode(po.ftrid) TRADECODE,st.m_dispatchdate,pos.name ORDSTATUS,z.red,z.blue,z.black,z.lightgreen,z.green,z.gold,
sum(case when pocl.sc_recipient=1 then stl.primaryqty else 0 end) parag,sum(case when pocl.sc_recipient=2 then stl.primaryqty else 0 end) apoth,
sum(case when pocl.sc_recipient=3 then stl.primaryqty else 0 end) apothparag, sum(case when pocl.sc_recipient=4 then stl.primaryqty else 0 end) apothsusk,
sum(case when pocl.sc_recipient=5 then stl.primaryqty else 0 end) paragapoth, sum(case when isnull(pocl.sc_recipient,0)=0 then stl.primaryqty else 0 end) unknown,m2.id spciteid,m2.subcode1 speccode,spcl.primaryqty*mu.edicode*stl.primaryqty specq"
            grouptxt = " group by   stl.ftrid,STL.STLID,po.ftrid,m.id,m.subcode1,pos.name,z.red,z.blue,z.black,z.lightgreen,z.green,z.gold,m_dispatchdate,m2.id,m2.subcode1,spcl.primaryqty*mu.edicode*stl.primaryqty order by st.m_dispatchdate asc"
            Using s As New SqlCommand("Select " + selecttxt + " FROM itemtransest stl inner join tbl_packerordercheck po on po.ftrid=stl.ftrid 
inner join storetrade st on st.ftrid=stl.ftrid inner join TBL_PACKERORDERSTATUS pos on pos.id=po.status 
inner join material m on m.id=stl.iteid 
LEFT JOIN (select row_number() over (partition by iteid order by iteid,id desc) rn,iteid,id,fromdate from specification group by iteid,id,fromdate ) SPC ON SPC.ITEID=STL.ITEID and rn=1
LEFT JOIN SPECIFICATIONLINES SPCL ON SPCL.SPCID=SPC.ID left join material m2 on m2.id=spcl.iteid 
left join mesunit mu on mu.codeid=m2.mu1
left join tbl_packerordclines pocl on pocl.stlid=stl.stlid and pocl.line=1   left join (select distinct stlid,backorder red,black,blue,lightgreen,green, gold from PKRVIW_PALLETITEMSTATUS) z on z.stlid=stl.stlid
where (po.status<12) AND (0=1 ", conn)
                If Not IsNothing(string_parameters) Or Not IsNothing(integer_parameters) Or Not IsNothing(integer_parameters_not_equal) Or Not IsNothing(list_parameters) Then
                    's.CommandText += " WHERE "
                    Dim total_params As Integer = 1
                    'If Not IsNothing(string_parameters) Then

                    '    For Each c As KeyValuePair(Of String, String) In string_parameters
                    '        If total_params > 0 Then
                    '            s.CommandText += " OR "
                    '        End If
                    '        s.CommandText += c.Key.Replace("@", "")
                    '        s.CommandText += " Like '%'+@" + c.Key + "+'%'"
                    '        s.Parameters.AddWithValue("@" + c.Key, c.Value)
                    '        total_params += 1
                    '    Next
                    'End If
                    If Not IsNothing(integer_parameters) Then
                        For Each c As KeyValuePair(Of String, Integer) In integer_parameters
                            If total_params > 0 Then
                                s.CommandText += " OR "
                            End If
                            s.CommandText += c.Key.Replace("@", "")
                            Dim splitkey As String() = c.Key.Split(".")

                            s.CommandText += "=@" + splitkey(1) + total_params.ToString
                            s.Parameters.AddWithValue("@" + splitkey(1) + total_params.ToString, c.Value)
                            total_params += 1
                        Next
                    End If
                    'If Not IsNothing(integer_parameters_not_equal) Then
                    '    For Each c As KeyValuePair(Of String, Integer) In integer_parameters_not_equal
                    '        If total_params > 0 Then
                    '            s.CommandText += " OR "
                    '        End If
                    '        s.CommandText += c.Key.Replace("@", "")
                    '        s.CommandText += "<>@" + c.Key
                    '        s.Parameters.AddWithValue("@" + c.Key, c.Value)
                    '        total_params += 1
                    '    Next
                    'End If
                    'If Not IsNothing(list_parameters) Then
                    '    For Each c As KeyValuePair(Of String, String) In list_parameters
                    '        If total_params > 0 Then
                    '            s.CommandText += " OR "
                    '        End If
                    '        s.CommandText += c.Key.Replace("@", "")
                    '        s.CommandText += " in (select value from dbo.f_split(@" + c.Key + ",',')) "
                    '        s.Parameters.AddWithValue("@" + c.Key, c.Value)
                    '        total_params += 1
                    '    Next
                    'End If
                    s.CommandText += ")"
                    s.CommandText += grouptxt
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                        'dt.Constraints.Clear()
                    End Using
                    conn.Close()
                End If
            End Using
            e.Result = dt
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point

    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseDown, Panel1.MouseDown, DoubleBufferedTableLayoutPanel1.MouseDown, Panel1.MouseDown, Panel2.MouseDown ' Add more handles here (Example: PictureBox1.MouseDown)
        If e.Button = MouseButtons.Left Then
            MoveForm = True
            Me.Cursor = Cursors.NoMove2D
            MoveForm_MousePosition = e.Location
        End If
    End Sub

    Private Sub DatagridviewStackedProgressColumnReportForm_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        If Me.OwnedForms.Count = 0 Then
            Me.Close()
            Me.Dispose()
        End If
    End Sub

    Public Sub MoveForm_MouseMove(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseMove, Panel1.MouseMove, Panel2.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If
    End Sub

    Private Sub DoubleBufferedTableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles DoubleBufferedTableLayoutPanel1.Paint

    End Sub

    Private Sub AdvancedDataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles AdvancedDataGridView1.DataBindingComplete

        For Each c As DataGridViewColumn In AdvancedDataGridView1.Columns
            If hiddenCols.Contains(c.Name) Then
                c.Visible = False
            End If
            If ColNames.ContainsKey(c.Name) Then
                c.HeaderText = ColNames(c.Name)
            End If
        Next

    End Sub

    Private Sub AdvancedDataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView1.CellDoubleClick
        If e.RowIndex > -1 And e.ColumnIndex > -1 Then
            With AdvancedDataGridView1
                If .Columns(e.ColumnIndex).Name = "itecode" Then
                    Using f As New ItemDetails(.Rows(e.RowIndex).Cells("iteid").Value)
                        f.Owner = Me
                        f.ShowDialog()
                    End Using
                ElseIf .Columns(e.ColumnIndex).Name = "speccode" Then
                    Using f As New ItemDetails(.Rows(e.RowIndex).Cells("spciteid").Value)
                        f.Owner = Me
                        f.ShowDialog()
                    End Using
                ElseIf .Columns(e.ColumnIndex).Name = "TRADECODE" Then
                    Using f As New Order(.Rows(e.RowIndex).Cells("ftrid").Value)
                        f.Owner = Me
                        f.ShowDialog()
                    End Using
                ElseIf .Columns(e.ColumnIndex).Name = "Status" Then
                    Dim f As New DatagridviewStackedProgressColumnReportForm(0, AdvancedDataGridView1.Rows(e.RowIndex).Cells("itecode").Value, AdvancedDataGridView1.Rows(e.RowIndex).Cells("TRADECODE").Value, Cursor.Position.X, Cursor.Position.Y, stl_or_ftrid_id:=AdvancedDataGridView1.Rows(e.RowIndex).Cells("STLID").Value)
                    f.Owner = Me
                    f.Show()
                End If
            End With
        End If
    End Sub

    Private Sub TableLayoutPanel1_Click(sender As Object, e As EventArgs)

    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseUp, Panel1.MouseUp, Panel2.MouseUp ' Add more handles here (Example: PictureBox1.MouseUp)
        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If
    End Sub
End Class