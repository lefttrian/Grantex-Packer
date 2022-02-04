Imports Microsoft.Reporting.WinForms
Imports System.Configuration
Imports System.Data.SqlClient

Public Class DatagridviewStackedProgressColumnReportForm

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)


    Private Sub DatagridviewStackedProgressColumnReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'atlantisDataSet.pkrprc_STATUSPIECHART' table. You can move, or remove it, as needed.
        'Me.pkrprc_STATUSPIECHARTTableAdapter.Fill(Me.atlantisDataSet.pkrprc_STATUSPIECHART, STLID:=pertypeid)
        ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\DatagridviewStackedProgressColumnReport.rdlc"
        SetLocation(xloc, yloc)
        Try
            If IsNothing(data) Then
                ReportViewer1.Visible = False
                Dim pic As New PictureBox
                pic.Image = My.Resources.rolling
                pic.SizeMode = PictureBoxSizeMode.CenterImage
                TableLayoutPanel1.Controls.Add(pic, 0, 1)
                pic.Dock = DockStyle.Fill
                mainworker.RunWorkerAsync()
            Else
                For Each c As DataRow In data.Rows
                    Dim drow As atlantisDataSet.DataTable1Row
                    drow = atlantisDataSet.DataTable1.NewDataTable1Row
                    drow.stlid = CInt(c("stlid"))
                    drow.s = c("s")
                    drow.t = c("t")
                    atlantisDataSet.DataTable1.Rows.Add(drow)
                Next
                prepare_reportviewer()
            End If

        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub prepare_reportviewer()
        ReportViewer1.LocalReport.EnableHyperlinks = True
        DataTable1BindingSource.EndEdit()
        DataTable1BindingSource.ResetBindings(True)
        Dim title As New ReportParameter("title", "Ποσότητες είδους " + itecode + " από " + itepar)
        ReportViewer1.LocalReport.SetParameters(title)
        Dim itecode2 As New ReportParameter("itecode", itecode)
        ReportViewer1.LocalReport.SetParameters(itecode2)
        Timer1.Start()
        Me.ReportViewer1.RefreshReport()
    End Sub

    Dim pertypeid As Integer, xloc As Integer, yloc As Integer, itecode As String, itepar As String, data As DataTable, stlid As Integer = 0, recvalues As String = "", ftrid As Integer = 0, weeknum As Integer = 0, year As Integer = 0

    Public Sub New(ByVal id As Integer, code As String, par As String, x As Integer, y As Integer, dt As DataTable, rec_values As String, Optional stl_or_ftrid_id As Integer = 0, Optional type As String = "stlid", Optional year_ As Integer = 0)
        ' This call is required by the designer.
        InitializeComponent()
        pertypeid = id
        xloc = x
        yloc = y
        itecode = code
        itepar = par
        recvalues = rec_values
        If type = "stlid" Then
            stlid = stl_or_ftrid_id
        ElseIf type = "ftrid" Then
            ftrid = stl_or_ftrid_id
        ElseIf type = "week" Then
            weeknum = stl_or_ftrid_id
            year = year_
        End If
        data = dt
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal id As Integer, code As String, par As String, x As Integer, y As Integer, stl_or_ftrid_id As Integer, Optional rec_values As String = "1,2,3,4,5", Optional type As String = "stlid")
        ' This call is required by the designer.
        InitializeComponent()
        pertypeid = id
        xloc = x
        yloc = y
        itecode = code
        itepar = par
        If type = "stlid" Then
            stlid = stl_or_ftrid_id
        ElseIf type = "ftrid" Then
            ftrid = stl_or_ftrid_id
        End If
        recvalues = rec_values
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        ' Add any initialization after the InitializeComponent() call.
    End Sub



    Private Sub ReportViewer1_Hyperlink(sender As Object, e As HyperlinkEventArgs) Handles ReportViewer1.Hyperlink
        Dim link As Uri = New Uri(e.Hyperlink)
        If link.Authority = "someaction" Then
            e.Cancel = True
            Dim sep As Char() = New Char() {"="c}
            Dim param = link.Query.Split(sep)
            Dim item As String = param(1)
            If item = "" Then
                item = "%"
            End If
            Dim status As String = param(2)
            If status = "LightBlue" Then
                status = "blue"
            ElseIf status = "DarkGreen" Then
                status = "green"
            ElseIf status = "DarkGray" Then
                For Each c As Control In Me.OwnedForms
                    If Not IsNothing(TryCast(c, PopUpNotification)) Then
                        c.Dispose()
                    End If
                Next
                Dim f As New PopUpNotification("Η γκρι κατηγορία είναι τα είδη που δεν έχουν κατανεμηθεί σε παλέτες. Δεν υπάρχουν παλέτες να δείτε!", Cursor.Position.X, Cursor.Position.Y)
                f.Owner = Me
                f.Show()
                Return
                status = "black"
            End If
            If stlid <> 0 Or ftrid <> 0 Or weeknum <> 0 Then
                'Dim cmd As String = ""
                'If stlid <> 0 Then
                '    cmd = "select distinct palletid from PKRVIW_PALLETITEMSTATUS z  left join tbl_packerordclines pocl on pocl.stlid=z.stlid and pocl.line=1 where pocl.sc_recipient in (select value from dbo.f_split('" + recvalues + "',',')) and z.stlid=" + stlid.ToString + " and  z.subcode1 like '" + item + "%' and " + status + "<>0"
                'ElseIf ftrid <> 0 Then
                '    cmd = "select distinct palletid from PKRVIW_PALLETITEMSTATUS z left join tbl_packerordclines pocl on pocl.stlid=z.stlid and pocl.line=1 where pocl.sc_recipient in (select value from dbo.f_split('" + recvalues + "',',')) and z.ftrid=" + ftrid.ToString + " and z.subcode1 like '" + item + "%' and " + status + "<>0"
                'ElseIf weeknum <> 0 Then
                '    cmd = "select distinct palletid from PKRVIW_PALLETITEMSTATUS z  left join tbl_packerordclines pocl on pocl.stlid=z.stlid and pocl.line=1 where pocl.sc_recipient in (select value from dbo.f_split('" + recvalues + "',',')) and  Z.stlid in (select id from storetradelines s inner join storetrade st on st.ftrid=s.ftrid where year(m_dispatchdate)=" + year.ToString + " and datepart(wk,m_dispatchdate)=" + weeknum + ") and z.subcode1 like '" + item + "%' and " + status + "<>0"
                'End If
                'Dim lst As List(Of Integer)
                'Using S As New SqlCommand(cmd, conn)
                '    Using dt As New DataTable()
                '        conn.Open()
                '        Using reader As SqlDataReader = S.ExecuteReader
                '            dt.Load(reader)
                '        End Using
                '        lst = (From r In dt.AsEnumerable() Select r.Field(Of Integer)(0)).ToList()
                '        conn.Close()
                '    End Using
                'End Using
                'Dim filter As String = ""
                'Dim filtertype As String = ""
                'If stlid <> 0 Then
                '    filter = stlid
                '    filtertype = "stlid"
                'Else
                '    filter = ftrid
                '    filtertype = "ftrid"
                'End If
                Dim lst As New Dictionary(Of String, String)
                lst.Add("subcode1", item)
                Dim lst2 As New Dictionary(Of String, Integer)
                lst2.Add(status, 0)
                Dim lst3 As New Dictionary(Of String, String)
                lst3.Add("sc_recipient", recvalues)
                Dim lst4 As New Dictionary(Of String, Integer)
                If stlid <> 0 Then
                    lst4.Add("stlid", stlid)
                ElseIf ftrid <> 0 Then
                    lst4.Add("ftrid", ftrid)
                ElseIf weeknum <> 0 Then
                    lst4.Add("yr", year)
                    lst4.Add("weeknum", weeknum)
                End If
                Using f As New ItemDistribution(Cursor.Position.X, Cursor.Position.Y, string_parameters_:=lst, integer_parameters_not_equal_:=lst2, list_parameters_:=lst3, integer_parameters_equal_:=lst4)
                    f.Owner = Me
                    f.ShowDialog()
                End Using
            Else
                Throw New Exception("Δεν γίνεται να επιστραφούν οι παλέτες χωρίς ftrid ή stlid ή αριθμό εβδομάδας!")
            End If
        End If
    End Sub

    Private Sub TableLayoutPanel1_Click(sender As Object, e As EventArgs) Handles TableLayoutPanel1.Click, Panel1.Click, Panel2.Click, ReportViewer1.Click
        timercounter = 0
    End Sub

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted
        Dim dt As New DataTable()
        dt = e.Result
        For Each r As DataColumn In dt.Columns
            Dim drow As atlantisDataSet.DataTable1Row
            drow = atlantisDataSet.DataTable1.NewDataTable1Row
            drow.stlid = stlid
            drow.s = r.ColumnName
            drow.t = dt.Rows(0).Item(r.ColumnName)
            atlantisDataSet.DataTable1.Rows.Add(drow)
        Next
        For Each c As Control In TableLayoutPanel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
        prepare_reportviewer()
        ReportViewer1.Visible = True
    End Sub

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork
        Dim extracmd As String = ""
        If stlid <> 0 Then
            extracmd = " z2.stlid =" + stlid.ToString
        ElseIf ftrid <> 0 Then
            extracmd = " z2.ftrid=" + ftrid.ToString + " AND z2.SUBCODE1 LIKE '" + itecode + "%' and poc.sc_recipient in (" + recvalues + ") "
        Else
            extracmd = " z2.ftrid=" + ftrid.ToString + " AND z2.SUBCODE1 LIKE '" + itecode + "%' and poc.sc_recipient in (" + recvalues + ") "
        End If
        Using s As New SqlCommand("Select sum((z2.blue)) 'Σε σχεδιασμένες παλέτες',sum((z2.black)) 'Εκκρεμούν' ,sum((z2.lightgreen)) 'Σε παλέτα',sum((z2.green)) 'Σε κλειστές παλέτες',sum((z2.gold)) 'Απεσταλμένα',sum((z2.BACKORDER)) Backorder  from z_packer_itemsbrowser z2 inner join tbl_packerordclines poc on z2.stlid=poc.stlid and poc.line=1 where " + extracmd, conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                End Using
                e.Result = dt
            End Using
        End Using
    End Sub

    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point

    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseDown, Panel1.MouseDown, TableLayoutPanel1.MouseDown, Panel1.MouseDown, Panel2.MouseDown ' Add more handles here (Example: PictureBox1.MouseDown)
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
    TableLayoutPanel1.MouseMove, Panel1.MouseMove, Panel2.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If
    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseUp, Panel1.MouseUp, Panel2.MouseUp, ReportViewer1.MouseMove ' Add more handles here (Example: PictureBox1.MouseUp)
        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
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
End Class