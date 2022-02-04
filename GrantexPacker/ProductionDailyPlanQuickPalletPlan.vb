Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Transactions

Public Class ProductionDailyPlanQuickPalletPlan
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim dpconn1 As New SqlConnection(connString)
    Dim dpconn2 As New SqlConnection(connString)
    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim stlid As Integer = 0
    Dim iteid As Integer = 0
    Dim ftrid As Integer = 0
    Dim itecode As String
    Dim yloc As Integer
    Dim xloc As Integer
    Dim id As Integer
    Dim type As Integer
    Dim stock As Boolean = False
    Dim quan As Double = 0
    Dim pallettype As String = ""
    Dim sum As Double = 0
    Dim selindex As Integer = 0
    Dim dailyplanid As Integer = 0
    Public numofpallets As Integer = 0
    Public quanperpallet As Double = 0
    Public extrapalletquantity As Double = 0
    Public extrapalletid As Integer = 0
    Public extrapalletfull = True
    Dim ColWidths As New Dictionary(Of String, Integer) From {{"CODE", 90}, {"OPENDATE", 70}, {"STATUS", 49}, {"QUANTITY", 43}, {"weight", 43}, {"netweight", 45}, {"remarks", 79}, {"INF", 28}, {"mixedclmn", 28}}
    Dim ColNames As New Dictionary(Of String, String) From {{"CODE", "ΚΩΔΙΚΟΣ"}, {"INF", "ΗΜΤΛΣ"}, {"OPENDATE", "ΗΜ ΔΗΜ"}, {"STATUS", "STAT"}, {"QUANTITY", "ΠΟΣ"}, {"weight", "ΒΑΡ"}, {"netweight", "ΚΑΘ ΒΑΡ"}, {"remarks", "ΣΧΟΛΙΑ"}}

    Public Sub New(ByVal dailyplan_id As Integer, pertypeid As Integer, typeid As Integer, x As Integer, y As Integer, index As Integer, Optional ite_id As Integer = 0, Optional ftr_id As Integer = 0)
        ' This call is required by the designer.
        InitializeComponent()
        id = pertypeid
        type = typeid
        If typeid = 0 Then
            stlid = pertypeid
        ElseIf typeid = 1 Then
            stock = True
        End If
        xloc = x
        yloc = y
        iteid = ite_id
        ftrid = ftr_id
        selindex = index
        dailyplanid = dailyplan_id
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point
    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseDown, Panel1.MouseDown, Panel3.MouseDown, FlowLayoutPanel1.MouseDown ' Add more handles here (Example: PictureBox1.MouseDown)
        If e.Button = MouseButtons.Left Then
            MoveForm = True
            Me.Cursor = Cursors.NoMove2D
            MoveForm_MousePosition = e.Location
        End If
    End Sub

    Public Sub MoveForm_MouseMove(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseMove, Panel1.MouseMove, Panel3.MouseMove, FlowLayoutPanel1.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If
    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseUp, Panel1.MouseUp, Panel3.MouseUp, FlowLayoutPanel1.MouseUp ' Add more handles here (Example: PictureBox1.MouseUp)
        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub NoPaddingCheckbox1_CheckedChanged(sender As Object, e As EventArgs) Handles NoPaddingCheckbox1.CheckedChanged
        If NoPaddingCheckbox1.Checked Then
            NoPaddingCheckbox1.Text = ""
            Me.Height = 415
            clear_dgv()
            work()
            'dgv.Dock = DockStyle.Fill
        Else
            NoPaddingCheckbox1.Text = ""
            Me.Height = 95
            clear_dgv()
        End If
    End Sub

    Private Sub clear_dgv()
        For Each c As Control In FlowLayoutPanel1.Controls
            If c.GetType = GetType(DataGridView) Then
                RemoveHandler TryCast(c, DataGridView).SelectionChanged, AddressOf dgv_selectionchanged
                RemoveHandler TryCast(c, DataGridView).CellEndEdit, AddressOf dgv_cellendedit
                RemoveHandler TryCast(c, DataGridView).Click, AddressOf TableLayoutPanel1_Click
                RemoveHandler TryCast(c, DataGridView).DataBindingComplete, AddressOf dgv_databindingcomplete
                RemoveHandler TryCast(c, DataGridView).CellDoubleClick, AddressOf dgv_celldoubleclick
                c.Dispose()
            End If
        Next
    End Sub

    Private Sub dgv_cwc(sender As Object, e As DataGridViewColumnEventArgs)
        Label5.Text = e.Column.Width
    End Sub


    Private Sub dgv_databindingcomplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim colwidths As New Dictionary(Of String, Integer) From {{"INF", 15}, {"mixedclmn", 15}}
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        Dim dt As DataTable = dgv.DataSource
        Dim imclmn As New DataGridViewImageColumn
        imclmn.Name = "INF"
        imclmn.HeaderText = "Ημιτελής"
        imclmn.DefaultCellStyle.NullValue = Nothing
        imclmn.Width = Me.ColWidths("INF")
        Dim imclmn2 As New DataGridViewImageColumn
        imclmn2.Name = "mixedclmn"
        imclmn2.HeaderText = "Mixed"
        imclmn2.DefaultCellStyle.NullValue = Nothing
        dgv.Columns.Insert(0, imclmn)
        dgv.Columns.Insert(0, imclmn2)
        imclmn2.Width = Me.ColWidths("mixedclmn")
        For Each dgvr As DataGridViewRow In dgv.Rows
            Dim status As Integer = dt.Rows(dgvr.Index).Item("status")
            If dt.Rows(dgvr.Index).Item("mixed") = "MIXED" Then
                dgvr.Cells("mixedclmn").Value = My.Resources.icons8_diversity_20
            End If
            If dt.Rows(dgvr.Index).Item("isnotfull") = 1 Then
                dgvr.Cells("INF").Value = My.Resources.icons8_flag_filled_16
            End If
            If status = 1 Then
                dgvr.DefaultCellStyle.BackColor = Color.LightGreen
                For Each c As DataGridViewCell In dgvr.Cells
                    c.ToolTipText = "Ολοκληρωμένη - κλεισμένη σε αναμονή για αποστολή"
                Next
            ElseIf status = 0 Then
                If IsDBNull(dt.Rows(dgvr.Index).Item("lockedbydpt")) Then
                    For Each c As DataGridViewCell In dgvr.Cells
                        c.ToolTipText = "Παλέτα ανοιγμένη-σε φάση συμπλήρωσης-ξεκλείδωτη"
                    Next
                    Continue For
                ElseIf dt.Rows(dgvr.Index).Item("lockedbydpt") = Form1.activeuserdpt Then
                    dgvr.DefaultCellStyle.BackColor = Color.LightGray
                    For Each c As DataGridViewCell In dgvr.Cells
                        c.ToolTipText = "Παλέτα ανοιγμένη-σε φάση συμπλήρωσης-κλειδωμένη από το τμήμα σας"
                    Next
                ElseIf dt.Rows(dgvr.Index).Item("lockedbydpt") <> Form1.activeuserdpt Then
                    dgvr.DefaultCellStyle.BackColor = Color.LightSalmon
                    For Each c As DataGridViewCell In dgvr.Cells
                        c.ToolTipText = "Παλέτα ανοιγμένη-σε φάση συμπλήρωσης-κλειδωμένη από άλλο τμήμα"
                    Next
                End If
            ElseIf status < 0 Then
                dgvr.DefaultCellStyle.BackColor = Color.LightBlue
                For Each c As DataGridViewCell In dgvr.Cells
                    c.ToolTipText = "Παλέτα σε φάση σχεδιασμού-ξεκλείδωτη"
                Next
                'If IsDBNull(dt.Rows(dgvr.Index).Item("lockedbydpt")) Then
                '    dgvr.DefaultCellStyle.BackColor = Color.LightCyan
                '    For Each c As DataGridViewCell In dgvr.Cells
                '        c.ToolTipText = "Παλέτα σε φάση σχεδιασμού-ξεκλείδωτη"
                '    Next
                'ElseIf dt.Rows(dgvr.Index).Item("lockedbydpt") = Form1.activeuserdpt Then
                '    dgvr.DefaultCellStyle.BackColor = Color.SkyBlue
                '    For Each c As DataGridViewCell In dgvr.Cells
                '        c.ToolTipText = "Παλέτα σε φάση σχεδιασμού-κλειδωμένη από το τμήμα σας"
                '    Next
                'ElseIf dt.Rows(dgvr.Index).Item("lockedbydpt") <> Form1.activeuserdpt Then
                '    dgvr.DefaultCellStyle.BackColor = Color.Plum
                '    For Each c As DataGridViewCell In dgvr.Cells
                '        c.ToolTipText = "Παλέτα σε φάση σχεδιασμού-κλειδωμένη από άλλο τμήμα"
                '    Next
                'End If
                If dt.Rows(dgvr.Index).Item("isnotfull") = 1 Then
                    dgvr.Cells("INF").Value = My.Resources.icons8_flag_filled_16
                End If
            End If
        Next
        For Each c As DataGridViewColumn In dgv.Columns
            If ColNames.ContainsKey(c.Name) Then
                c.HeaderText = ColNames(c.Name)
            End If
        Next
    End Sub
    Private Sub dgv_selectionchanged(sender As Object, e As EventArgs)
        openbutton.Visible = False
        RemoveSemiFinished.Visible = False
        SetSemiFinished.Visible = False
        closebutton.Visible = False
        printbutton.Visible = False
        deletebutton.Visible = False
        lockbutton.Visible = False
        unlockbutton.Visible = False
        combinebutton.Visible = False
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        Dim dt As DataTable = dgv.DataSource
        Dim PerLineButtonStatus As New DataTable()
        PerLineButtonStatus.Columns.Add("Index", GetType(Integer))
        PerLineButtonStatus.Columns.Add("lockbutton", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("unlockbutton", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("printbutton", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("combinebutton", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("openbutton", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("closebutton", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("deletebutton", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("SetSemiFinished", GetType(Boolean))
        PerLineButtonStatus.Columns.Add("RemoveSemiFinished", GetType(Boolean))
        For Each i As DataGridViewRow In dgv.SelectedRows
            Dim r As DataRow = dt.Rows(i.Index)
            Dim newrow As DataRow = PerLineButtonStatus.NewRow
            newrow.Item("printbutton") = True
            newrow.Item("Index") = i.Index
            With r
                If .Item("STATUS") = -1 Or .Item("STATUS") = -2 Then
                    newrow.Item("openbutton") = True
                    newrow.Item("closebutton") = False
                    newrow.Item("deletebutton") = True
                ElseIf .Item("STATUS") = 0 Then
                    newrow.Item("openbutton") = False
                    newrow.Item("closebutton") = True
                    newrow.Item("deletebutton") = False
                Else
                    newrow.Item("openbutton") = False
                    newrow.Item("closebutton") = False
                    newrow.Item("deletebutton") = False
                End If
                If IsDBNull(.Item("closedbydpt")) Then
                    If .Item("STATUS") >= 0 Then
                        newrow.Item("RemoveSemiFinished") = False
                        newrow.Item("SetSemiFinished") = False
                        newrow.Item("combinebutton") = False
                        If IsDBNull(.Item("lockedbydpt")) Then
                            newrow.Item("lockbutton") = True
                            newrow.Item("unlockbutton") = False
                            If .Item("STATUS") = 0 Then
                                newrow.Item("deletebutton") = True
                            End If
                        Else
                            If Form1.activeuserdpt = .Item("lockedbydpt") Or Form1.activeuserdpt = "SA" Then
                                newrow.Item("unlockbutton") = True
                                newrow.Item("lockbutton") = False
                                If .Item("isnotfull") = 1 Then
                                    newrow.Item("combinebutton") = True
                                Else
                                    newrow.Item("combinebutton") = False
                                End If
                                If .Item("STATUS") = 0 Then
                                    newrow.Item("deletebutton") = True
                                End If
                            Else
                                newrow.Item("lockbutton") = False
                                newrow.Item("unlockbutton") = False
                                newrow.Item("openbutton") = False
                                newrow.Item("closebutton") = False
                                newrow.Item("deletebutton") = False
                            End If
                        End If
                    Else
                        If .Item("createdptid") = Form1.activeuserdptid Or Form1.activeuserdpt = "SA" Then
                            newrow.Item("openbutton") = True
                            newrow.Item("closebutton") = False
                            newrow.Item("deletebutton") = True
                            newrow.Item("lockbutton") = False
                            newrow.Item("unlockbutton") = False
                            newrow.Item("combinebutton") = True
                            If .Item("isnotfull") = 1 Then
                                newrow.Item("combinebutton") = True
                                newrow.Item("SetSemiFinished") = False
                                newrow.Item("RemoveSemiFinished") = True
                            Else
                                newrow.Item("combinebutton") = False
                                newrow.Item("RemoveSemiFinished") = False
                                newrow.Item("SetSemiFinished") = True
                            End If
                        Else
                            newrow.Item("openbutton") = False
                            newrow.Item("closebutton") = False
                            newrow.Item("deletebutton") = False
                            newrow.Item("lockbutton") = False
                            newrow.Item("unlockbutton") = False
                            newrow.Item("combinebutton") = False
                            newrow.Item("RemoveSemiFinished") = False
                            newrow.Item("SetSemiFinished") = False
                        End If
                    End If
                Else
                        newrow.Item("lockbutton") = False
                    newrow.Item("unlockbutton") = False
                    newrow.Item("openbutton") = False
                    newrow.Item("closebutton") = False
                    newrow.Item("deletebutton") = False
                    newrow.Item("combinebutton") = False
                    newrow.Item("RemoveSemiFinished") = False
                    newrow.Item("SetSemiFinished") = False
                End If

            End With
            PerLineButtonStatus.Rows.Add(newrow)
            Dim txt As String = ""
        Next
        For Each c As Button In Panel3.Controls
            Dim visible As Boolean = True
            If PerLineButtonStatus.Columns.Contains(c.Name) Then
                For Each r As DataRow In PerLineButtonStatus.Rows
                    If r.Item(c.Name) = False Then
                        visible = False
                        Exit For
                    End If
                Next
            End If
            c.Visible = visible
        Next
        'Dim HasPlannedStock As Boolean = False
        'Dim HasStatusMinus21 As Boolean = False
        'Dim HasPhysicalPallets As Boolean = False
        'Dim HasLockedORClosedFromOtherDPT As Boolean = False
        'Dim userdpt As String = Form1.activeuserdpt
        'Dim IsCompleted As Boolean = False
        'For Each i As DataGridViewRow In dgv.SelectedRows
        '    If dt.Rows(i.Index).Item("STATUS") < 0 And dt.Rows(i.Index).Item("isstock") <> 1 Then
        '        HasStatusMinus21 = True
        '    ElseIf dt.Rows(i.Index).Item("STATUS") = 0 Then
        '        HasPhysicalPallets = True
        '    ElseIf dt.Rows(i.Index).Item("STATUS") = 1 Then
        '        IsCompleted = True
        '    ElseIf dt.Rows(i.Index).Item("STATUS") < 0 And dt.Rows(i.Index).Item("isstock") = 1 Then
        '        HasPlannedStock = True
        '    End If
        '    If userdpt <> "SA" AndAlso ((Not IsDBNull(dt.Rows(i.Index).Item("closedbydpt")) AndAlso dt.Rows(i.Index).Item("closedbydpt") <> Form1.activeuserdpt) Or (Not IsDBNull(dt.Rows(i.Index).Item("lockedbydpt")) AndAlso dt.Rows(i.Index).Item("lockedbydpt") <> Form1.activeuserdpt)) Then
        '        HasLockedORClosedFromOtherDPT = True
        '    End If
        '    If IsDBNull(dt.Rows(i.Index).Item("closedbydpt")) Then
        '        lockbutton.Visible =
        '    End If
        'Next
        'If (HasStatusMinus21 And HasPlannedStock And HasPhysicalPallets) Or (Not HasStatusMinus21 And Not HasPlannedStock And Not HasPhysicalPallets) Then
        '    openbutton.Visible = False
        '    closebutton.Visible = False
        '    printbutton.Visible = False
        '    deletebutton.Visible = False
        'ElseIf Not HasLockedORClosedFromOtherDPT AndAlso ((HasStatusMinus21 And Not HasPlannedStock And Not HasPhysicalPallets) Or (HasStatusMinus21 And HasPlannedStock And Not HasPhysicalPallets) Or
        '        (Not HasStatusMinus21 And HasPlannedStock And Not HasPhysicalPallets)) Then
        '    openbutton.Visible = True
        '    deletebutton.Visible = True

        'ElseIf Not HasLockedORClosedFromOtherDPT AndAlso (HasPhysicalPallets And Not HasStatusMinus21 And Not HasPlannedStock And Not IsCompleted) Then
        '    closebutton.Visible = True
        '        printbutton.Visible = True
        '    ElseIf IsCompleted Then
        '        printbutton.Visible = True
        '    End If

    End Sub

    Private Sub dgv_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        Try
            Dim dgv As DataGridView = TryCast(sender, DataGridView)
            If e.RowIndex >= 0 Then
                If dgv.Columns(e.ColumnIndex).Name = "weight" Then
                    If IsDBNull(dgv.Rows(e.RowIndex).Cells("weight").Value) Then
                        Return
                    End If
                    Using s As New SqlCommand("update tbl_palletheaders set weight=@w,netweight=@w-23 where id=" + dgv.Rows(e.RowIndex).Cells("ID").Value.ToString, updconn)
                        s.Parameters.Add("@w", SqlDbType.Float).Value = dgv.Rows(e.RowIndex).Cells("weight").Value
                        updconn.Open()
                        s.ExecuteNonQuery()
                        updconn.Close()
                        work()
                    End Using
                ElseIf dgv.Columns(e.ColumnIndex).Name = "netweight" Then
                    If IsDBNull(dgv.Rows(e.RowIndex).Cells("netweight").Value) Then
                        Return
                    End If
                    Using s As New SqlCommand("update tbl_palletheaders set netweight=@w where id=" + dgv.Rows(e.RowIndex).Cells("ID").Value.ToString, updconn)
                        s.Parameters.Add("@w", SqlDbType.Float).Value = dgv.Rows(e.RowIndex).Cells("netweight").Value
                        updconn.Open()
                        s.ExecuteNonQuery()
                        updconn.Close()
                        work()
                    End Using
                End If
            End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub dgv_celldoubleclick(sender As Object, e As DataGridViewCellEventArgs)
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        If e.RowIndex >= 0 Then
            If dgv.Columns(e.ColumnIndex).Name = "remarks" Then
                Dim msg = "Εισάγετε σχόλια:"
                Dim result2 As String = InputBox(msg,
                                                      "Προσθήκη σχολίων", " "
                                                      )
                If Not String.IsNullOrWhiteSpace(result2) Then
                    Dim txt As String = result2 + ", " + Form1.activeuser + ", " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine
                    Using cmd As New SqlCommand("update tbl_palletheaders set remarks=@remarks+cast(isnull(remarks,'') as varchar(7000)) where id=" + dgv.Rows(e.RowIndex).Cells("ID").Value.ToString, updconn)
                        cmd.Parameters.Add("@remarks", SqlDbType.VarChar, 8000).Value = txt
                        updconn.Open()
                        If cmd.ExecuteNonQuery() > 0 Then
                            work()
                        End If
                        updconn.Close()
                    End Using
                End If
            Else
                Using f As New PalletDetails(dgv.Rows(e.RowIndex).Cells("ID").Value)
                    f.ShowDialog()
                End Using
            End If

        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub ProductionDailyPlanQuickPalletPlan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetLocation(xloc, yloc)
        Timer1.Start()
        quan = TryCast(Owner, production).dpdgv2.Rows(selindex).Cells("quantity").Value
        sum = TryCast(Owner, production).dpdgv2.Rows(selindex).Cells("pallets").Value
        Using c As New SqlCommand("select isnull(q1.a,0) planned,isnull(q3.a,0) opened ,isnull(q2.a,0) completed from
(select dailyplanid,count(distinct palletid) a from tbl_palletlines where dailyplanid=" + dailyplanid.ToString + " group by DAILYPLANID) q1
left join
(select pl.dailyplanid,count(ph.id) a from tbl_palletheaders ph inner join tbl_palletlines pl on pl.palletid=ph.id where dailyplanid=" + dailyplanid.ToString + " and status=1 group by dailyplanid) q2 on q2.dailyplanid=q1.dailyplanid 
left join
(select pl.dailyplanid,count(ph.id) a from tbl_palletheaders ph inner join tbl_palletlines pl on pl.palletid=ph.id where dailyplanid=" + dailyplanid.ToString + " and status=0 group by dailyplanid) q3 on q3.dailyplanid=q1.dailyplanid", conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = c.ExecuteReader
                    dt.Load(reader)
                    conn.Close()

                End Using
                If dt.Rows.Count > 0 Then
                    Label5.Text = dt.Rows(0).Item("planned")
                    Label6.Text = dt.Rows(0).Item("completed")
                    Label9.Text = dt.Rows(0).Item("opened")
                Else
                    Label5.Text = "-"
                    Label6.Text = "-"
                    Label9.Text = "-"
                End If
            End Using
        End Using
        If type = 0 Then
            Using s As New SqlCommand("select description+ '('+cast(width as varchar(4))+'x'+cast(length as varchar(4))+'x'+cast(height as varchar(4))+')' as name from pkrtbl_pallettypes where id=(select top 1 isnull(pc.PALLETTYPEID,1) from customer c left join PKRTBL_CUSTOMER pc on pc.cusid=c.id left join fintrade f on f.cusid=c.id 
 left join storetradelines stl on stl.ftrid=f.id left join pkrtbl_dailyplan dp on type=0 and pertypeid1=stl.id 
 where dp.id=" + dailyplanid.ToString + ")  ", conn)
                conn.Open()
                pallettype = s.ExecuteScalar
                conn.Close()
            End Using
        Else
            Using s As New SqlCommand("select description+ '('+cast(width as varchar(4))+'x'+cast(length as varchar(4))+'x'+cast(height as varchar(4))+')' as name from pkrtbl_pallettypes where id=1", conn)
                conn.Open()
                pallettype = s.ExecuteScalar
                conn.Close()
            End Using
        End If
        If type = 0 Then 'παραγγελία
            Label2.Text = "Παραγγελία"
            Label2.ForeColor = Color.DarkGreen
        ElseIf type = 1 Then  'απόθεμα
            Label2.Text = "Απόθεμα"
            Label2.ForeColor = Color.Red
        End If
        LockUIAccess(Me)
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

        Dim f As New AddPalletDialogBox(id, type, Cursor.Position.X, Cursor.Position.Y, oo:=True, limit:=quan - sum, pallet_type:=pallettype)
        f.Owner = Me
        f.Show()

    End Sub

    Public Sub execute()
        Dim result = 0
        Try
            For Each f As Form In Me.OwnedForms
                f.Close()
                f.Dispose()
            Next
            Using transaction = TransactionUtils.CreateTransactionScope(IsolationLevel.ReadUncommitted)
                Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, stock:=stock, stl_id:=stlid, ftr_id:=ftrid)
                    Dim s As Integer = -2
                    If stock Then
                        s = -4
                    End If
                    Dim desiredloc As String = ""

                    If numofpallets > 1 Or (numofpallets = 1 And extrapalletquantity = 0) Then
                        Using newids As DataTable = pm.Create(numofpallets, status:=s, desired_loc:=desiredloc)
                            If IsNothing(newids) Then
                                Throw New Exception("Δεν υπάρχουν ελεύθερες θέσεις αποθήκευσης.")
                            End If
                            For Each r As DataRow In newids.Rows
                                pm.AddItem(r("ID"), iteid, stlid, ftrid, quanperpallet, dailyplan_id:=dailyplanid)
                            Next
                            If extrapalletquantity > 0 Then
                                If extrapalletid = 0 Then
                                    Using newid As DataTable = pm.Create(1, status:=s, isnotfull:=1)
                                        pm.AddItem(newid.Rows(0).Item("ID"), iteid, stlid, ftrid, extrapalletquantity, dailyplan_id:=dailyplanid)
                                    End Using
                                Else
                                    pm.AddItem(extrapalletid, iteid, stlid, ftrid, extrapalletquantity, dailyplan_id:=dailyplanid)
                                    pm.ToggleSemiFinished(extrapalletfull, extrapalletid)
                                End If
                            End If
                        End Using
                    ElseIf numofpallets = 1 And extrapalletquantity > 0 Then
                        If extrapalletid = 0 Then
                            Using newid As DataTable = pm.Create(1, status:=s, isnotfull:=1)
                                pm.AddItem(newid.Rows(0).Item("ID"), iteid, stlid, ftrid, extrapalletquantity, dailyplan_id:=dailyplanid)
                            End Using
                        Else
                            pm.AddItem(extrapalletid, iteid, stlid, ftrid, extrapalletquantity, dailyplan_id:=dailyplanid)
                            pm.ToggleSemiFinished(extrapalletfull, extrapalletid)
                        End If
                    End If
                End Using
                transaction.Complete()
                result = 1
            End Using
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
        If result = 1 Then
            work()
            TryCast(Owner, production).dp2work()
        End If
    End Sub

    Private Sub ProductionDailyPlanQuickPalletPlan_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        'If Me.OwnedForms.Count = 0 Then
        '    Me.Close()
        '    Me.Dispose()
        'End If
    End Sub

    Private Sub NoPaddingButton3_Click(sender As Object, e As EventArgs) Handles NoPaddingButton3.Click
        Dim f As New AddPalletDialogBox(id, type, Cursor.Position.X, Cursor.Position.Y, limit:=quan - sum, pallet_type:=pallettype)
        f.Owner = Me
        f.Show()
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub openbutton_Click(sender As Object, e As EventArgs) Handles openbutton.Click
        Try
            Dim result As Integer = 0
            For Each c As Control In FlowLayoutPanel1.Controls
                If c.GetType = GetType(DataGridView) Then
                    Dim l As New List(Of Integer)
                    For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                        l.Add(r.Cells("ID").Value)
                    Next
                    If l.Count > 0 Then
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=ftrid, stock:=stock)
                            result = pm.Promote(l)
                        End Using
                    End If
                End If
            Next
            If result <> 0 Then
                work()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub deletebutton_Click(sender As Object, e As EventArgs) Handles deletebutton.Click
        Try
            Dim result As Integer = 0
            For Each c As Control In FlowLayoutPanel1.Controls
                If c.GetType = GetType(DataGridView) Then
                    Dim l As New List(Of Integer)
                    For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                        l.Add(r.Cells("ID").Value)
                    Next
                    If l.Count > 0 Then
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=ftrid, stock:=stock)
                            result = pm.Delete(l, skipcheck:=True)
                        End Using
                    End If
                End If
            Next
            If result <> 0 Then
                work()
                TryCast(Owner, production).dp2work()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub closebutton_Click(sender As Object, e As EventArgs) Handles closebutton.Click
        Try
            Dim result As Integer = 0
            For Each c As Control In FlowLayoutPanel1.Controls
                If c.GetType = GetType(DataGridView) Then
                    Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=ftrid, stock:=stock)
                        For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                            Dim success As Integer = -1
                            If IsDBNull(r.Cells("weight").Value) Or IsDBNull(r.Cells("netweight").Value) Then
                                Throw New Exception("Δεν επιτρέπεται κλείσιμο παλέτας χωρίς βάρη.")
                            End If
                            Dim remarks As String = ""
                            remarks = r.Cells("remarks").Value
                            success = pm.Close(r.Cells("ID").Value, r.Cells("remarks").Value, r.Cells("weight").Value, r.Cells("netweight").Value)
                            result += success
                            If success < 0 Then
                                Throw New Exception("Κάτι δε πήγε καλά κατά τη διαγραφή της παλέτας " + r.Cells("CODE").Value)
                            End If
                        Next
                    End Using
                End If
            Next
            If result <> 0 Then
                work()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub TableLayoutPanel1_Click(sender As Object, e As EventArgs) Handles TableLayoutPanel1.Click, Panel1.Click, Panel2.Click, Panel3.Click, FlowLayoutPanel1.Click, Label1.Click, Label2.Click,
            Label3.Click, Label4.Click, Label5.Click, Label6.Click, Label7.Click
        timercounter = 0
    End Sub

    Private Sub unlockbutton_Click(sender As Object, e As EventArgs) Handles unlockbutton.Click
        Try
            Dim result As Integer = 0
            For Each c As Control In FlowLayoutPanel1.Controls
                If c.GetType = GetType(DataGridView) Then
                    Dim l As New List(Of Integer)
                    For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                        l.Add(r.Cells("ID").Value)
                    Next
                    If l.Count > 0 Then
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=ftrid, stock:=stock)
                            result = pm.unlock(l)
                        End Using
                    End If
                End If
            Next
            If result > 0 Then
                work()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub lockbutton_Click(sender As Object, e As EventArgs) Handles lockbutton.Click
        Try
            Dim result As Integer = 0
            For Each c As Control In FlowLayoutPanel1.Controls
                If c.GetType = GetType(DataGridView) Then
                    Dim l As New List(Of Integer)
                    For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                        l.Add(r.Cells("ID").Value)
                    Next
                    If l.Count > 0 Then
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=ftrid, stock:=stock)
                            result = pm.lock(l)
                        End Using
                    End If
                End If
            Next
            If result > 0 Then
                work()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub printbutton_Click(sender As Object, e As EventArgs) Handles printbutton.Click

    End Sub

    Private Sub NoPaddingButton2_Click(sender As Object, e As EventArgs) Handles combinebutton.Click
        For Each c As Control In FlowLayoutPanel1.Controls
            If c.GetType = GetType(DataGridView) Then
                Dim l As New List(Of Integer)
                For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                    If r.Cells("ISNOTFULL").Value = 1 Then
                        l.Add(r.Cells("ID").Value)
                    End If

                Next
                If l.Count > 0 Then

                    Dim f As New ProductionCombineSemifinishedPallets(l, Cursor.Position.X, Cursor.Position.Y)
                    f.Owner = Me
                    If f.ShowDialog = DialogResult.OK Then
                        work()
                    Else
                        Return
                    End If

                End If
            End If
        Next
    End Sub

    Public Sub work()
        clear_dgv()
        quan = TryCast(Owner, production).dpdgv2.Rows(selindex).Cells("quantity").Value
        sum = TryCast(Owner, production).dpdgv2.Rows(selindex).Cells("pallets").Value
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        clear_dgv()
        FlowLayoutPanel1.Controls.Add(pic)
        pic.Dock = DockStyle.Fill
        mainworker.RunWorkerAsync()
    End Sub

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork

        Dim CMD As String = ""
        'If stlid = 0 Then
        CMD = "SELECT ph.ID,ph.createdptid,ph.CODE,dbo.pkrfn_mixedpallet(ph.id) mixed,ph.OPENDATE,ph.STATUS,lockpu.department lockedbydpt, closepu.department closedbydpt,PL.QUANTITY,isnull(ph.isstock,0) isstock,ph.weight,ph.netweight,ph.remarks,ISNULL(PH.ISNOTFULL,0) ISNOTFULL FROM TBL_PALLETHEADERS PH LEFT JOIN TBL_PALLETLINES PL ON PL.PALLETID=PH.ID  left join tbl_packeruserdata closepu on closepu.id=ph.closedbyid  left join tbl_packeruserdata lockpu on lockpu.id=ph.lockedbyid WHERE PL.dailyplanid=" + dailyplanid.ToString
        'Else
        '    CMD = "SELECT ph.ID,ph.CODE,dbo.pkrfn_mixedpallet(ph.id) mixed,ph.OPENDATE,ph.STATUS,lockpu.department lockedbydpt, closepu.department closedbydpt,PL.QUANTITY,isnull(ph.isstock,0) isstock,ph.weight,ph.netweight,ph.remarks,ISNULL(PH.ISNOTFULL,0) ISNOTFULL FROM TBL_PALLETHEADERS PH LEFT JOIN TBL_PALLETLINES PL ON PL.PALLETID=PH.ID  left join tbl_packeruserdata closepu on closepu.id=ph.closedbyid  left join tbl_packeruserdata lockpu on lockpu.id=ph.lockedbyid WHERE PL.dailyplanid=" + dailyplanid.ToString + " or pl.stlid=" + stlid.ToString
        'End If
        Using data = New DataTable()
            Using c As SqlCommand = New SqlCommand(CMD, conn)
                conn.Open()
                Using reader As SqlDataReader = c.ExecuteReader
                    data.Load(reader)
                End Using
                conn.Close()
            End Using
            e.Result = data
        End Using

    End Sub

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted

        Dim dgv As New DataGridView
        dgv.DataSource = e.Result
        dgv.Width = FlowLayoutPanel1.Width - 5
        dgv.Height = 320
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToOrderColumns = False
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        AddHandler dgv.SelectionChanged, AddressOf dgv_selectionchanged
        AddHandler dgv.CellEndEdit, AddressOf dgv_cellendedit
        AddHandler dgv.Click, AddressOf TableLayoutPanel1_Click
        AddHandler dgv.DataBindingComplete, AddressOf dgv_databindingcomplete
        AddHandler dgv.CellDoubleClick, AddressOf dgv_celldoubleclick
        FlowLayoutPanel1.Controls.Add(dgv)
        For Each c As DataGridViewColumn In dgv.Columns
            If Not ColNames.Keys.Contains(c.Name) Then
                c.Visible = False
            End If
            If c.Name = "weight" Or c.Name = "netweight" Then
                Continue For
            Else
                c.ReadOnly = True
            End If
        Next
        dgv.ClearSelection()
        openbutton.Visible = False
        closebutton.Visible = False
        printbutton.Visible = False
        deletebutton.Visible = False
        lockbutton.Visible = False
        unlockbutton.Visible = False
        combinebutton.Visible = False
        SetSemiFinished.Visible = False
        RemoveSemiFinished.Visible = False
    End Sub

    Private Sub SetSemiFinished_Click(sender As Object, e As EventArgs) Handles SetSemiFinished.Click
        Try
            Dim result As Integer = 0
            For Each c As Control In FlowLayoutPanel1.Controls
                If c.GetType = GetType(DataGridView) Then
                    Dim l As New List(Of Integer)
                    For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                        l.Add(r.Cells("ID").Value)
                    Next
                    If l.Count > 0 Then
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=ftrid, stock:=stock)
                            result = pm.ToggleSemiFinished(True, l)
                        End Using
                    End If
                End If
            Next
            If result <> 0 Then
                work()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub RemoveSemiFinished_Click(sender As Object, e As EventArgs) Handles RemoveSemiFinished.Click
        Try
            Dim result As Integer = 0
            For Each c As Control In FlowLayoutPanel1.Controls
                If c.GetType = GetType(DataGridView) Then
                    Dim l As New List(Of Integer)
                    For Each r As DataGridViewRow In TryCast(c, DataGridView).SelectedRows
                        l.Add(r.Cells("ID").Value)
                    Next
                    If l.Count > 0 Then
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=ftrid, stock:=stock)
                            result = pm.ToggleSemiFinished(False, l)
                        End Using
                    End If
                End If
            Next
            If result <> 0 Then
                work()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
End Class