Imports System.Data.Objects
Imports System.Data.SqlClient
Imports System.Configuration
Imports DataGridViewAutoFilter
Imports System.Text.RegularExpressions
Imports System.Deployment.Application

Public Class Order
    Private Sub Label2_Click(sender As Object, e As EventArgs)

    End Sub
    Dim p = GetType(Zuby.ADGV.AdvancedDataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim p2 = GetType(Zuby.ADGV.AdvancedDataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim p3 = GetType(Zuby.ADGV.AdvancedDataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim items As New List(Of palletlistitem)
    Dim docs As New List(Of Integer)
    Dim first_load_completed As Boolean = False
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            AdvancedDataGridView1.Dispose()
            AdvancedDataGridView2.Dispose()
            AdvancedDataGridView3.Dispose()
            'For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
            '    Dim ctrl As Control = Me.Controls(i)
            '    ctrl.Dispose()

            'Next
            updconn.Dispose()
            conn.Dispose()
            stlidwithdata = Nothing
            stlidtoadd = Nothing
            stlidtodel = Nothing
        Catch
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Public Sub New(ByVal value As Integer)
        ftrid = value
        InitializeComponent()
    End Sub
    Dim stlidwithdata As New List(Of String)
    Dim orderraw As Integer
    Dim ftrid As Integer
    Dim prodcheck As Integer = 0
    Dim prodcheck1 As Integer = 0
    Dim warecheck As Integer = 0
    Dim warecheck1 As Integer = 0
    Dim dgv1firstload As Boolean = False
    Dim dgv2firstload As Boolean = False
    Dim dgv3firstload As Boolean = False
    Dim ynTable As New DataTable()
    Dim cmd As String = ""
    Dim cmd2 As String = ""
    Dim cmd3 As String = ""
    Dim cmd4 As String = ""
    Dim cmd5 As String = ""
    Dim cmd6 As String = ""
    Dim cmd7 As String = ""
    Dim cmd10 As String = ""
    Dim cmd14 As String = ""
    Dim Status As Integer = 0
    Dim cmd15 As String = ""
    Dim UserHasRights As Boolean = False
    Dim UserHasRecEditRights As Boolean = False
    Dim dptrights As Integer = 0
    Dim userrights As Integer = 0
    Dim f As New Font("Microsoft Sans Serif", 15,
                    FontStyle.Bold)

    Private Sub Order_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Cursor.Current = ExtCursor1.Cursor
            orderraw = Form1.orderraw
            'Using S As New SqlCommand("SELECT ID, NAME,CODEID FROM TBL_RECIPIENTS", conn)
            '    Using dt As New DataTable()
            '        conn.Open()
            '        Using reader As SqlDataReader = S.ExecuteReader
            '            dt.Load(reader)
            '        End Using
            '        conn.Close()
            '        For Each r As DataRow In dt.Rows
            '            items.Add(New palletlistitem(r.Item("NAME"), r.Item("ID"), r.Item("CODEID")))
            '        Next
            '    End Using
            'End Using
            'items.Add(New palletlistitem(" ", 0, " "))
            'TrackBar1.Minimum = 0
            ' TrackBar1.Maximum = 13
            'Dim cmd As String = "select stl.id,mtr.code AS 'ΚΩΔΙΚΟΣ',mtr.description AS 'ΕΙΔΟΣ',stl.primaryqty AS 'ΠΟΣ ΠΑΡ',SC1.LSUMQTY AS 'ΕΛ ΠΟΣ ΜΑΝΤΙΣ',SC2.LSUMQTY AS 'ΔΕΣΜ ΠΟΣ ΜΑΝΤΙΣ',stl.secjustification AS 'ΣΧΟΛΙΑ ΕΞΑΓΩΓΩΝ', TBL.WARECOMMENTS AS 'ΣΧΟΛΙΑ ΑΠΟΘΗΚΗΣ',TBL.PRODCOMMENTS AS 'ΣΧΟΛΙΑ ΠΑΡΑΓΩΓΗΣ' from 
            'storetradelines stl left join material mtr on mtr.id=stl.iteid LEFT JOIN
            'SC_QTY_MANTIS SC1 ON SC1.ITEID=STL.ITEID LEFT JOIN
            'SC_QTY_MANTIS_RETURNS SC2 ON SC2.ITEID=STL.ITEID LEFT JOIN
            'TBL_PACKERORDCLINES TBL ON TBL.STLID=STL.ID where stl.ftrid=" + ftrid.ToString

            'Dim cmd As String = "select TBL.WARECOMMENTS As 'ΣΧΟΛΙΑ ΑΠΟΘΗΚΗΣ', TBL.PRODCOMMENTS AS 'ΣΧΟΛΙΑ ΠΑΡΑΓΩΓΗΣ'
            cmd = "select s.name from FINTRADE ftr left join CUSTOMER cus on ftr.CUSID=cus.ID left join SALESMAN s on s.ID=cus.COLIDSALESMAN where ftr.ID=" + ftrid.ToString
            cmd2 = "Select cus.code+', '+isnull(cus.fathername,'???')+',  '+f.tradecode+', '+CASE WHEN F.SC_RELFTRID IS NOT NULL THEN '('+right(f2.tradecode,7)+f.M_RELFTRIDINC+'), ' ELSE '' END+cast(F.ftrdate as varchar(20)) from fintrade f left join fintrade f2 on f2.id=f.sc_relftrid left join customer cus on cus.id=f.cusid where f.id=" + ftrid.ToString
            cmd3 = "select isnull(cast(fintrade.justification as varchar(max)),'')+' '+isnull(cast(st.secjustification as varchar(max)),'') from fintrade left join storetrade st on st.ftrid=fintrade.id where fintrade.id=" + ftrid.ToString
            cmd4 = "select isnull(CAST(warecomments as varchar(max)),'') from tbl_packerordercheck where ftrid=" + ftrid.ToString
            cmd5 = "select isnull(CAST(prodcomments as varchar(max)),'') from tbl_packerordercheck where ftrid=" + ftrid.ToString
            cmd6 = "select isnull(CAST(packcomments as varchar(max)),'') from tbl_packerordercheck where ftrid=" + ftrid.ToString
            cmd7 = "SELECT z.[id]		,[iteid] ,[BASICCODE] as 'Βασικός Κωδικός' ,[M_PARTNO] as 'Part Number',z.[CODE] as 'Κωδικός',z.subcode1 as 'ΕΝΑΛ ΚΩΔ', mnf as 'ΜΑΡΚΑ',[M_WVA1] as 'WVA',[QTY] as 'Ποιότητα',[MU] as 'M/M',[PRIMARYQTY] as 'Ποσότητα' ,[FLDFLOAT1] as 'OS',[SPECIAL] as 'ΕΙΔ ΧΑΡ',[semi1] as 'Ημιέτοιμο1' ,[semi2]  as 'Ημιέτοιμο2'
,[SEMI3] as 'Ημιέτοιμο3',[spring1] as 'Ελατήριο1' ,[SPRING2] as 'Ελατήριο2',[kit1] as 'Κιτ1',[kit2] as 'Κιτ2' ,[KIT3]  as 'Κιτ3',[sensor1] as 'Αισθητήρας1',[SENSOR2]  as 'Αισθητήρας2',[RIVET] as 'Πριτσίνια',[box1] as 'Κουτί1',[BOX2] as 'Κουτί2' ,[label1] as 'Ετικέτα1' ,[LABEL2] as 'Ετικέτα2' ,[comments] as 'ΣΧ ΕΞΑΓΩΓΩΝ',isnull([PRODCOMMENTS],'') as 'ΣΧ ΠΑΡΑΓΩΓΗΣ'
,isnull([WARECOMMENTS],'') as 'ΣΧ ΑΠΟΘΗΚΗΣ' ,isnull([PACKCOMMENTS],'') as 'ΣΧ ΣΥΣΚΕΥ',[sc_recipient] as scr FROM [Z_PACKER_BPORDER] z left join [TBL_PACKERORDCLINES] t on t.stlid=z.id where z.id in (select id from storetradelines where ftrid=" + ftrid.ToString + ") order by 3"
            cmd10 = "select isnull(sum(isnull(primaryqty,0)),0) from storetradelines s left join material m on s.iteid=m.id where ftrid=" + ftrid.ToString + " and substring(m.code,1,3) in ('102','202')"
            cmd14 = "select count(id) from storetradelines where ftrid=" + ftrid.ToString
            Using s As New SqlCommand("select poc.status,pos.name statusname from tbl_packerordercheck poc inner join TBL_PACKERORDERSTATUS pos on pos.id=poc.status where ftrid=" + ftrid.ToString, conn)
                Using dt As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                    End Using
                    Status = dt(0).Item("status")
                    If dt(0).Item("status") = 12 Then
                        TrackBar1.Value = 13
                    ElseIf dt(0).Item("status") = 13 Then
                        TrackBar1.Value = 12
                    Else
                        TrackBar1.Value = dt(0).Item("status")
                    End If
                    TrackBar1.Label = dt(0).Item("statusname")
                    conn.Close()
                End Using
            End Using
            Using s As New SqlCommand("select  1 from pkrtbl_dptordcrights docr left join PKRTBL_USERDEPARTMENTS ud on ud.id=docr.dptid left join tbl_packeruserdata pud on pud.DEPARTMENT=ud.code
                                    where dptid=" + Form1.activeuserdptid.ToString + " and orderstatus=(select status from tbl_packerordercheck where ftrid=" + ftrid.ToString + ") and pud.ORDCUSER=1 and pud.id=" + Form1.activeuserid.ToString, conn)
                conn.Open()
                If s.ExecuteScalar = 1 Then dptrights = 1 Else dptrights = 0
                conn.Close()
            End Using
            Using s As New SqlCommand("Select 1 from pkrtbl_userordcrights where userid=" + Form1.activeuserid.ToString + " And orderstatus=(Select status from tbl_packerordercheck where ftrid=" + ftrid.ToString + ") and userid in (select id from tbl_packeruserdata where ORDCUSER=1) ", conn)
                conn.Open()
                If s.ExecuteScalar = 1 Then userrights = 1 Else userrights = 0
                conn.Close()
            End Using
            If dptrights = 1 Or userrights = 1 Or Form1.activeuserdpt = "SA" Then
                UserHasRights = True
            End If
            Using s As New SqlCommand("Select 1 from pkrtbl_userordcrights where userid=" + Form1.activeuserid.ToString + "  and orderstatus=(Select status from tbl_packerordercheck where ftrid=" + ftrid.ToString + ") and add_recipient=1", conn)
                conn.Open()
                If s.ExecuteScalar = 1 Or Form1.activeuserdpt = "SA" Then UserHasRecEditRights = True Else UserHasRecEditRights = False
                conn.Close()
            End Using

            Using comm As New SqlCommand(cmd, conn)
                Using comm2 As New SqlCommand(cmd2, conn)
                    Using comm3 As New SqlCommand(cmd3, conn)
                        Using comm4 As New SqlCommand(cmd4, conn)
                            Using comm5 As New SqlCommand(cmd5, conn)
                                Using comm6 As New SqlCommand(cmd6, conn)
                                    AdvancedDataGridView1.Visible = False
                                    Label8.Visible = False
                                    AdvancedDataGridView2.Visible = False
                                    Label9.Visible = False
                                    AdvancedDataGridView3.Visible = False
                                    Label10.Visible = False
                                    dgv1bw.RunWorkerAsync()
                                    dgv2bw.RunWorkerAsync()
                                    dgv3bw.RunWorkerAsync()
                                    variousbw.RunWorkerAsync()
                                    variousbw2.RunWorkerAsync()
                                    variousbw3.RunWorkerAsync()
                                    variousbw4.RunWorkerAsync()
                                    variousbw5.RunWorkerAsync()
                                    Using comm10 As New SqlCommand(cmd10, conn)
                                        Using comm14 As New SqlCommand(cmd14, conn)
                                            If conn.State = ConnectionState.Open Then
                                                conn.Close()
                                            End If
                                            conn.Open()
                                            Label5.Text = "Υπέυθυνος πωλητής " + comm.ExecuteScalar()
                                            Label1.Text = comm2.ExecuteScalar()
                                            Label2.Text = "Είδη στη παραγγελία " + comm14.ExecuteScalar().ToString
                                            TextBox5.Text = comm3.ExecuteScalar()
                                            TextBox2.Text = comm4.ExecuteScalar()
                                            TextBox3.Text = comm5.ExecuteScalar()
                                            TextBox4.Text = comm6.ExecuteScalar()
                                            Using comm16 As New SqlCommand("select isnull(comments,'') from tbl_packeruserordercomments where ftrid=" + ftrid.ToString + " and userid=" + Form1.activeuserid.ToString, conn)
                                                TextBox1.Text = comm16.ExecuteScalar()
                                            End Using
                                            Dim bpquant As Double = comm10.ExecuteScalar()
                                            conn.Close()
                                            With ynTable
                                                .Columns.Add("name", GetType(String))
                                                .Columns.Add("value", GetType(Integer))
                                                Using S As New SqlCommand("SELECT ID, NAME,CODEID FROM TBL_RECIPIENTS", conn)
                                                    Using dt As New DataTable()
                                                        conn.Open()
                                                        Using reader As SqlDataReader = S.ExecuteReader
                                                            dt.Load(reader)
                                                        End Using
                                                        conn.Close()
                                                        For Each r As DataRow In dt.Rows
                                                            .Rows.Add(r.Item("NAME"), r.Item("ID"))
                                                        Next
                                                    End Using
                                                End Using
                                                .Rows.Add("  ", DBNull.Value)
                                                .AcceptChanges()
                                            End With
                                            Label8.Text = "Sets δισκοφρένων: " + bpquant.ToString
                                            Cursor.Current = Cursors.Default
                                            If UserHasRights Then
                                                Button1.Enabled = True  'ΠΡΟΣΟΧΗ ΤΟ ΙΔΙΟ ΝΑ ΓΙΝΕΙ ΚΑΙ ΣΤΟ SELECTEDINDEXCHANGED ΤΟΥ TABCONTROL
                                                Button2.Enabled = True
                                            End If
                                        End Using
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using
                End Using
            End Using
            If Form1.activeuserdpt <> "SA" Then
                Dim dv As DataView = New DataView(Form1.DptAccess)
                dv.RowFilter = "FORM='" + Me.Name + "'"
                For Each r As DataRowView In dv
                    Dim tbxs As Control() = Me.Controls.Find(r.Item("CONTROL"), True)
                    If (Not IsNothing(tbxs) And tbxs.Length > 0) Then
                        If r.Item("LOCKED") = 1 Then
                            tbxs(0).Enabled = False
                        Else
                            tbxs(0).Enabled = True
                        End If
                        If r.Item("NOTVISIBLE") = 1 Then
                            tbxs(0).Visible = False
                        Else
                            tbxs(0).Visible = True
                        End If
                    End If
                Next
                Dim dv2 As DataView = New DataView(Form1.UserAccess)
                dv2.RowFilter = "FORM='" + Me.Name + "'"
                For Each r As DataRowView In dv2
                    Dim tbxs As Control() = Me.Controls.Find(r.Item("CONTROL"), True)
                    If (Not IsNothing(tbxs) And tbxs.Length > 0) Then
                        If r.Item("LOCKED") = 1 Then
                            tbxs(0).Enabled = False
                        Else
                            tbxs(0).Enabled = True
                        End If
                        If r.Item("NOTVISIBLE") = 1 Then
                            tbxs(0).Visible = False
                        Else
                            tbxs(0).Visible = True
                        End If
                    End If
                Next
            End If
            TabControl1.SelectedIndex = 1
            TabControl1.SelectedIndex = 0
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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Me.Close()

    End Sub


    Private Sub AdvancedDataGridView3_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim ostatus As Integer = -1
            Using sqlcmd As New SqlCommand("select isnull(status,-1) from tbl_packerordercheck where ftrid= " + ftrid.ToString, conn)
                conn.Open()
                ostatus = sqlcmd.ExecuteScalar
                conn.Close()

            End Using
            'ΕΝΗΜΕΡΩΣΗ ΠΙΝΑΚΩΝ ΣΧΟΛΙΑΣΜΟΥ ΓΡΑΜΜΩΝ
            If (ostatus <> 13 And ostatus <> 12) Then
                If UserHasRights Then

                    Dim success As Integer = update_order_status(2, ostatus)
                    If success = 0 Then
                        Label12.Text = "Επιτυχής αποθήκευση!"
                        Label12.ForeColor = Color.Green
                        Label12.Visible = True
                    Else
                        Label12.Text = "Παρουσιάστηκε κάποιο πρόβλημα κατά την αποθήκευση!"
                        Label12.ForeColor = Color.Red
                        Label12.Visible = True
                    End If
                    Form1.orderdgv_refresh()
                Else
                    Label12.Text = "Δεν επιτράπηκε!"
                    Label12.ForeColor = Color.Red
                    Label12.Visible = True
                End If
            Else
                Label12.Text = "Δεν επιτράπηκε!"
                Label12.ForeColor = Color.Red
                Label12.Visible = True
            End If

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
    Dim oldvalue As String
    Private Sub AdvancedDataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs)
        If IsDBNull(AdvancedDataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
            oldvalue = ""
        Else
            oldvalue = AdvancedDataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        End If
    End Sub
    Dim newvalue As String
    Dim stlidtoadd As New List(Of String)
    Dim stlidtodel As New List(Of String)
    Private Sub AdvancedDataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView1.CellEndEdit
        With AdvancedDataGridView1
            '    If IsDBNull(.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
            '        newvalue = ""
            '    Else
            '        newvalue = .Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            '    End If
            '    With AdvancedDataGridView1

            '        If .Columns(e.ColumnIndex).Name = "Αποδέκτης" And Not IsNothing(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) _
            '    And Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) Then
            '            Dim val As String = .Rows(e.RowIndex).Cells("Αποδέκτης").Value
            '            If .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then

            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Style.BackColor = Color.Yellow
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = False
            '                '.Rows(e.RowIndex).Cells("Αποδέκτης").Value = val
            '            ElseIf .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 2 Then

            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = .Rows(e.RowIndex).Cells("Ποσότητα").Value
            '                '.Rows(e.RowIndex).Cells("Αποδέκτης").Value = val
            '            Else
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = True
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
            '                '.Rows(e.RowIndex).Cells("Αποδέκτης").Value = val
            '            End If
            '        End If
            '    End With

            stlidtoadd.Add(.Rows(e.RowIndex).Cells("id").Value.ToString + "/1")

            If String.IsNullOrWhiteSpace(newvalue) And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) _
            And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) _
            And IsDBNull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) Then
                stlidtodel.Add(.Rows(e.RowIndex).Cells("id").Value.ToString + "/1")
                stlidtoadd.Remove(.Rows(e.RowIndex).Cells("id").Value.ToString + "/1")
            End If
        End With
    End Sub

    Private Sub AdvancedDataGridView2_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles AdvancedDataGridView2.CellBeginEdit
        If IsDBNull(AdvancedDataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
            oldvalue = ""
        Else
            oldvalue = AdvancedDataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        End If
    End Sub

    Dim lastcomboclmn As Integer
    Dim lastcomborow As Integer
    Private Sub AdvancedDataGridView1_EditingControlShowing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles AdvancedDataGridView1.EditingControlShowing
        lastcomborow = AdvancedDataGridView1.CurrentCell.RowIndex
        lastcomboclmn = AdvancedDataGridView1.CurrentCell.ColumnIndex
        If AdvancedDataGridView1.Columns(lastcomboclmn).HeaderText = "Αποδέκτης" Then
            Dim combo As ComboBox = CType(e.Control, ComboBox)
            If (combo IsNot Nothing) Then
                ' Remove an existing event-handler, if present, to avoid 
                ' adding multiple handlers when the editing control is reused.
                RemoveHandler combo.SelectionChangeCommitted, New EventHandler(AddressOf adgv1_ComboBox_SelectionChangeCommitted)

                ' Add the event handler. 
                AddHandler combo.SelectionChangeCommitted, New EventHandler(AddressOf adgv1_ComboBox_SelectionChangeCommitted)
            End If
        End If
    End Sub

    Private Sub adgv1_ComboBox_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs)
        stlidtoadd.Add(AdvancedDataGridView1.Rows(lastcomborow).Cells("id").Value.ToString + "/1")
    End Sub

    Private Sub AdvancedDataGridView2_EditingControlShowing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles AdvancedDataGridView2.EditingControlShowing
        lastcomborow = AdvancedDataGridView2.CurrentCell.RowIndex
        lastcomboclmn = AdvancedDataGridView2.CurrentCell.ColumnIndex
        If AdvancedDataGridView1.Columns(lastcomboclmn).HeaderText = "Αποδέκτης" Then
            Dim combo As ComboBox = CType(e.Control, ComboBox)
            If (combo IsNot Nothing) Then
                ' Remove an existing event-handler, if present, to avoid 
                ' adding multiple handlers when the editing control is reused.
                RemoveHandler combo.SelectionChangeCommitted, New EventHandler(AddressOf adgv2_ComboBox_SelectionChangeCommitted)

                ' Add the event handler. 
                AddHandler combo.SelectionChangeCommitted, New EventHandler(AddressOf adgv2_ComboBox_SelectionChangeCommitted)
            End If
        End If
    End Sub

    Private Sub adgv2_ComboBox_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs)
        stlidtoadd.Add(AdvancedDataGridView2.Rows(lastcomborow).Cells("id").Value.ToString + "/" + AdvancedDataGridView2.Rows(lastcomborow).Cells("Μέρος").Value.ToString)
    End Sub

    Private Sub AdvancedDataGridView3_EditingControlShowing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles AdvancedDataGridView3.EditingControlShowing
        lastcomborow = AdvancedDataGridView3.CurrentCell.RowIndex
        lastcomboclmn = AdvancedDataGridView3.CurrentCell.ColumnIndex
        If AdvancedDataGridView3.Columns(lastcomboclmn).HeaderText = "Αποδέκτης" Then
            Dim combo As ComboBox = CType(e.Control, ComboBox)
            If (combo IsNot Nothing) Then
                ' Remove an existing event-handler, if present, to avoid 
                ' adding multiple handlers when the editing control is reused.
                RemoveHandler combo.SelectionChangeCommitted, New EventHandler(AddressOf adgv3_ComboBox_SelectionChangeCommitted)

                ' Add the event handler. 
                AddHandler combo.SelectionChangeCommitted, New EventHandler(AddressOf adgv3_ComboBox_SelectionChangeCommitted)
            End If
        End If
    End Sub

    Private Sub adgv3_ComboBox_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs)
        stlidtoadd.Add(AdvancedDataGridView3.Rows(lastcomborow).Cells("id").Value.ToString + "/1")
    End Sub

    Private Sub AdvancedDataGridView2_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView2.CellEndEdit
        With AdvancedDataGridView2
            '    If IsDBNull(.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
            '        newvalue = ""
            '    Else
            '        newvalue = .Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            '    End If
            '    With AdvancedDataGridView2

            '        If .Columns(e.ColumnIndex).Name = "Αποδέκτης" And Not IsNothing(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) _
            '    And Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) Then
            '            Dim val As String = .Rows(e.RowIndex).Cells("Αποδέκτης").Value
            '            If .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then

            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Style.BackColor = Color.Yellow
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = False

            '            ElseIf .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 2 Then

            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = .Rows(e.RowIndex).Cells("Ποσότητα").Value

            '            Else
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = True
            '                .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value

            '            End If
            '        End If
            '    End With
            '    'If .Columns(e.ColumnIndex).Name = "Αποδέκτης" And Not IsNothing(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) _
            '    '    And Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) Then
            '    '    If .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then
            '    '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = False
            '    '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
            '    '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Style.BackColor = Color.Yellow
            '    '    ElseIf .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 2 Then
            '    '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = .Rows(e.RowIndex).Cells("Ποσότητα").Value
            '    '    Else
            '    '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = True
            '    '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
            '    '    End If
            '    'End If

            stlidtoadd.Add(.Rows(e.RowIndex).Cells("id").Value.ToString + "/" + .Rows(e.RowIndex).Cells("Μέρος").Value.ToString)

            If String.IsNullOrWhiteSpace(newvalue) And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) _
            And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) _
            And IsDBNull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) Then
                stlidtodel.Add(.Rows(e.RowIndex).Cells("id").Value.ToString + "/" + .Rows(e.RowIndex).Cells("Μέρος").Value.ToString)
                stlidtoadd.Remove(.Rows(e.RowIndex).Cells("id").Value.ToString + "/" + .Rows(e.RowIndex).Cells("Μέρος").Value.ToString)
            End If
        End With
    End Sub

    Private Sub AdvancedDataGridView3_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles AdvancedDataGridView3.CellBeginEdit
        If IsDBNull(AdvancedDataGridView3.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
            oldvalue = ""
        Else
            oldvalue = AdvancedDataGridView3.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        End If
    End Sub

    Private Sub AdvancedDataGridView3_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView3.CellEndEdit
        With AdvancedDataGridView3
            If IsDBNull(.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                newvalue = ""
            Else
                newvalue = .Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            End If

            'If .Columns(e.ColumnIndex).Name = "Αποδέκτης" And Not IsNothing(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) _
            '    And Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) Then
            '    If .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then

            '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
            '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Style.BackColor = Color.Yellow
            '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = False
            '    ElseIf .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 2 Then
            '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = .Rows(e.RowIndex).Cells("Ποσότητα").Value
            '    Else
            '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").ReadOnly = True
            '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
            '    End If
            'End If




            stlidtoadd.Add(.Rows(e.RowIndex).Cells("id").Value.ToString + "/1")

            If String.IsNullOrWhiteSpace(newvalue) And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) _
                And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) And String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) _
                And IsDBNull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) Then
                stlidtodel.Add(.Rows(e.RowIndex).Cells("id").Value.ToString + "/1")
                stlidtoadd.Remove(.Rows(e.RowIndex).Cells("id").Value.ToString + "/1")
            End If
        End With
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim ostatus As Integer = -1
            Using sqlcmd As New SqlCommand("Select isnull(status,-1) from tbl_packerordercheck where ftrid=" + ftrid.ToString, conn)
                conn.Open()
                ostatus = sqlcmd.ExecuteScalar
                conn.Close()

            End Using
            'ΕΝΗΜΕΡΩΣΗ ΠΙΝΑΚΩΝ ΣΧΟΛΙΑΣΜΟΥ ΓΡΑΜΜΩΝ
            If (ostatus <> 13 And ostatus <> 12) Then
                If UserHasRights Then
                    If Form1.activeuserdpt = "PRD" And TabControl1.SelectedIndex = 0 Then

                        For i As Integer = 0 To AdvancedDataGridView1.Rows.Count - 1
                            If IsDBNull(AdvancedDataGridView1.Rows(i).Cells("Αποδέκτης").Value) Then
                                Throw New System.Exception("Υπάρχει δισκόφρενο χωρίς να έχει συμπληρωθεί αποδέκτης. Παρακαλώ συμπληρώστε αποδέκτη σε όλα τα είδη προτού ολοκληρώσετε τον έλεγχο της παραγγελίας.")
                            End If
                        Next
                    ElseIf Form1.activeuserdpt = "PRD" And TabControl1.SelectedIndex = 1 Then

                        For i As Integer = 0 To AdvancedDataGridView2.Rows.Count - 1
                            If IsDBNull(AdvancedDataGridView2.Rows(i).Cells("Αποδέκτης").Value) Then
                                Throw New System.Exception("Υπάρχει φερμουίτ χωρίς να έχει συμπληρωθεί αποδέκτης. Παρακαλώ συμπληρώστε αποδέκτη σε όλα τα είδη προτού ολοκληρώσετε τον έλεγχο της παραγγελίας.")
                            End If
                        Next
                    End If
                    Dim success As Integer = update_order_status(1, ostatus)
                    If success = 0 Then
                        Label11.Text = "Επιτυχής αποθήκευση!"
                        Label11.ForeColor = Color.Green
                        Label11.Visible = True
                        If TabControl1.SelectedIndex = 0 Then
                            Me.TabPage1.BackColor = Color.Green
                        ElseIf TabControl1.SelectedIndex = 1 Then
                            Me.TabPage3.BackColor = Color.Green
                        End If
                    Else
                        Label11.Text = "Παρουσιάστηκε κάποιο πρόβλημα κατά την αποθήκευση!"
                        Label11.ForeColor = Color.Red
                        Label11.Visible = True
                        If TabControl1.SelectedIndex = 0 Then
                            Me.TabPage1.BackColor = SystemColors.Control
                        ElseIf TabControl1.SelectedIndex = 1 Then
                            Me.TabPage3.BackColor = SystemColors.Control
                        End If
                    End If
                    Form1.orderdgv_refresh()
                Else
                    Label11.Text = "Δεν επιτράπηκε!"
                    Label11.ForeColor = Color.Red
                    Label11.Visible = True
                End If
            Else
                Label11.Text = "Δεν επιτράπηκε!"
                Label11.ForeColor = Color.Red
                Label11.Visible = True
            End If

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

    Private Function update_order_status(ByVal type As Integer, ByVal ostatus As Integer) 'type=1 για τελική αποθήκευση, type=2 για προσωρινή αποθήκευση
        Dim transaction As SqlTransaction
        updconn.Open()
        transaction = updconn.BeginTransaction("SampleTransaction")
        Try
            Dim subtype As Integer = 0
            If TabControl1.SelectedIndex = 0 Then
                subtype = 101
            ElseIf TabControl1.SelectedIndex = 1 Then
                subtype = 102
            ElseIf TabControl1.SelectedIndex = 2 Then
                subtype = 2
            End If
            Dim CancelPreviousUserC As New SqlCommand
            CancelPreviousUserC.Connection = updconn
            Dim usercmd As New SqlCommand
            usercmd.Connection = updconn
            Using s As New SqlCommand("select status,isnull(userid,0) userid, isnull(dptid,0) dptid, isnull(subtype,0) subtype from tbl_packerordercheck poc left join pkrtbl_statusrequirements sr on sr.statusid=poc.status  where ftrid=" + ftrid.ToString, conn)
                Using dt As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                    End Using
                    conn.Close()
                    If Status <> dt.Rows(0).Item("status") Then
                        Throw New Exception("Ασυμφωνία κατάστασης παραγγελίας. Δοκιμάστε να ανοίξετε εκ νέου τη παραγγελία!")
                    End If
                    Dim SubtypeFound As Boolean = False
                    If dt.Rows.Count > 1 Then 'το στάδιο παραγγελίας απαιτεί πολλαπλές εγκρίσεις
                        For Each r As DataRow In dt.Rows
                            If r.Item("subtype") = TabControl1.SelectedIndex Then
                                CancelPreviousUserC.CommandText = "update PKRTBL_USERORDERCHECK set active=0 where userid=@userid and ftrid=@ftrid and subtype=@subtype"
                                CancelPreviousUserC.Parameters.AddWithValue("@subtype", TabControl1.SelectedIndex)
                                usercmd.CommandText = "insert into PKRTBL_USERORDERCHECK (ftrid,userid,checkdate,type,active,subtype) values (@ftrid,@userid,getdate(),@type,1,@subtype)"
                                usercmd.Parameters.AddWithValue("@subtype", TabControl1.SelectedIndex)
                                SubtypeFound = True
                                Exit For
                            End If
                        Next
                    End If
                    If Not SubtypeFound Then
                        CancelPreviousUserC.CommandText = "update PKRTBL_USERORDERCHECK set active=0 where userid=@userid and ftrid=@ftrid"
                        usercmd.CommandText = "insert into PKRTBL_USERORDERCHECK (ftrid,userid,checkdate,type,active) values (@ftrid,@userid,getdate(),@type,1)"
                    End If
                End Using
            End Using
            CancelPreviousUserC.Parameters.AddWithValue("@ftrid", ftrid)
            CancelPreviousUserC.Parameters.AddWithValue("@userid", Form1.activeuserid)
            CancelPreviousUserC.Transaction = transaction
            usercmd.Parameters.AddWithValue("@ftrid", ftrid)
            usercmd.Parameters.AddWithValue("@userid", Form1.activeuserid)
            usercmd.Parameters.AddWithValue("@type", type)
            usercmd.Transaction = transaction
            stlidtoadd = stlidtoadd.Distinct.ToList
            stlidtodel = stlidtodel.Distinct.ToList
            stlidwithdata = stlidwithdata.Distinct.ToList
            Dim mastercmd As String = ""
            Dim addcmd As String = ""
            Dim cmd1 As String = ""
            Dim cmd2 As String = ""
            Dim cmd3 As String = ""
            Dim success As Integer = -1
            Dim tabindex As String = ""
            Dim comm2 As New SqlCommand, comm1 As New SqlCommand, comm4 As New SqlCommand, comm3 As New SqlCommand, mastercomm As New SqlCommand
            'ΕΝΗΜΕΡΩΣΗ ΠΙΝΑΚΩΝ ΣΧΟΛΙΑΣΜΟΥ ΓΡΑΜΜΩΝ
            If TabControl1.SelectedIndex = 0 Then
                With AdvancedDataGridView1
                    For i As Integer = 0 To .Rows.Count - 1
                        Dim screc As String = "NULL"
                        Dim wquant As Double = 0
                        Dim m As String = ""
                        Dim ins As String = ""
                        Dim ins2 As String = ""
                        If Not IsDBNull(.Rows(i).Cells("Αποδέκτης").Value) AndAlso .Rows(i).Cells("Αποδέκτης").Value > 0 Then
                            screc = .Rows(i).Cells("Αποδέκτης").Value.ToString
                        End If
                        If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Then
                            m = " packcomments=@packcomments1"
                            ins = ",packcomments"
                            ins2 = ",@packcomments1"
                        ElseIf Form1.activeuserdpt = "SP" Then
                            m = "warecomments=@warecomments1"
                            ins = ",warecomments"
                            ins2 = ",@warecomments1"
                        ElseIf Form1.activeuserdpt = "PRD" Then
                            m = " prodcomments=@prodcomments1"
                            ins = ",prodcomments"
                            ins2 = ",@prodcomments1"
                        ElseIf Form1.activeuserdpt = "SA" Then
                            m = " prodcomments=@prodcomments1,warecomments=@warecomments1,packcomments=@packcomments1"
                            ins = ",prodcomments,warecomments,packcomments"
                            ins2 = ",@prodcomments1,@warecomments1,@packcomments1"
                        End If
                        If UserHasRecEditRights Then
                            m = m + ",sc_recipient=" + screc
                            ins = ins + ",sc_recipient"
                            ins2 = ins2 + "," + screc
                        End If

                        If stlidwithdata.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                            If stlidtodel.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                                cmd1 = "delete from tbl_packerordclines where stlid=" + .Rows(i).Cells("id").Value.ToString
                            End If
                            If stlidtoadd.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                                cmd1 = "update tbl_packerordclines Set " + m + " where  stlid=" + .Rows(i).Cells("id").Value.ToString


                            End If
                        Else
                            If stlidtoadd.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                                cmd1 = "insert into  tbl_packerordclines (stlid,ftrid,line" + ins + ",code,ware_quantity) values (" + .Rows(i).Cells("id").Value.ToString + "," + ftrid.ToString + ",1" + ins2 + ",' " + .Rows(i).Cells("id").Value.ToString + "',0)"
                            End If

                        End If
                        If cmd1 <> "" Then
                            success = success + 1
                            comm1.CommandText = cmd1
                            comm1.Connection = updconn
                            comm1.Transaction = transaction
                            If Form1.activeuserdpt = "PRD" Or Form1.activeuserdpt = "SA" Then
                                comm1.Parameters.AddWithValue("@prodcomments1", .Rows(i).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString)
                            End If
                            If Form1.activeuserdpt = "SP" Or Form1.activeuserdpt = "SA" Then
                                comm1.Parameters.AddWithValue("@warecomments1", .Rows(i).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString)
                            End If
                            If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "SA" Then
                                comm1.Parameters.AddWithValue("@packcomments1", .Rows(i).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString)
                            End If
                            If comm1.ExecuteNonQuery() > 0 Then
                                success = success - 1
                                comm1.Parameters.Clear()
                                cmd1 = ""
                            Else
                                Throw New System.Exception("ΣΦΑΛΜΑ Ενημερώστε διαχειριστή: " + comm1.CommandText)
                            End If

                        End If
                    Next
                End With
            ElseIf TabControl1.SelectedIndex = 1 Then
                With AdvancedDataGridView2
                    For i As Integer = 0 To .Rows.Count - 1
                        Dim screc As String = "NULL"
                        Dim wquant As Double = 0
                        Dim m As String = ""
                        Dim ins As String = ""
                        Dim ins2 As String = ""
                        If Not IsDBNull(.Rows(i).Cells("Αποδέκτης").Value) AndAlso .Rows(i).Cells("Αποδέκτης").Value > 0 Then
                            screc = .Rows(i).Cells("Αποδέκτης").Value.ToString
                        End If
                        If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Then
                            m = " packcomments=@packcomments1"
                            ins = ",packcomments"
                            ins2 = ",@packcomments1"
                        ElseIf Form1.activeuserdpt = "SP" Then
                            m = "warecomments=@warecomments1"
                            ins = ",warecomments"
                            ins2 = ",@warecomments1"
                        ElseIf Form1.activeuserdpt = "PRD" Then
                            m = " prodcomments=@prodcomments1"
                            ins = ",prodcomments"
                            ins2 = ",@prodcomments1"
                        ElseIf Form1.activeuserdpt = "SA" Then
                            m = " prodcomments=@prodcomments1,warecomments=@warecomments1,packcomments=@packcomments1"
                            ins = ",prodcomments,warecomments,packcomments"
                            ins2 = ",@prodcomments1,@warecomments1,@packcomments1"
                        End If
                        If UserHasRecEditRights Then
                            m = m + ",sc_recipient=" + screc
                            ins = ins + ",sc_recipient"
                            ins2 = ins2 + "," + screc
                        End If
                        If stlidwithdata.Contains(.Rows(i).Cells("id").Value.ToString + "/" + .Rows(i).Cells("Μέρος").Value.ToString) Then
                            If stlidtodel.Contains(.Rows(i).Cells("id").Value.ToString + "/" + .Rows(i).Cells("Μέρος").Value.ToString) Then
                                cmd1 = "delete from tbl_packerordclines where stlid=" + .Rows(i).Cells("id").Value.ToString + " and line=" + .Rows(i).Cells("Μέρος").Value.ToString
                            End If
                            If stlidtoadd.Contains(.Rows(i).Cells("id").Value.ToString + "/" + .Rows(i).Cells("Μέρος").Value.ToString) Then
                                cmd1 = "update tbl_packerordclines Set " + m + " where  stlid=" + .Rows(i).Cells("id").Value.ToString + " and line=" + .Rows(i).Cells("Μέρος").Value.ToString
                            End If
                        Else
                            If stlidtoadd.Contains(.Rows(i).Cells("id").Value.ToString + "/" + .Rows(i).Cells("Μέρος").Value.ToString) Then
                                cmd1 = "insert into  tbl_packerordclines (stlid,ftrid,line" + ins + ",code,ware_quantity) values (" + .Rows(i).Cells("id").Value.ToString + "," + ftrid.ToString + "," + .Rows(i).Cells("Μέρος").Value.ToString + ins2 + ",' " + .Rows(i).Cells("id").Value.ToString + "/" + .Rows(i).Cells("Μέρος").Value.ToString + "',0)"
                            End If
                        End If
                        If cmd1 <> "" Then
                            success = success + 1
                            comm2.CommandText = cmd1
                            comm2.Connection = updconn
                            comm2.Transaction = transaction
                            If Form1.activeuserdpt = "PRD" Or Form1.activeuserdpt = "SA" Then
                                comm2.Parameters.AddWithValue("@prodcomments1", .Rows(i).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString)
                            End If
                            If Form1.activeuserdpt = "SP" Or Form1.activeuserdpt = "SA" Then
                                comm2.Parameters.AddWithValue("@warecomments1", .Rows(i).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString)
                            End If
                            If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "SA" Then
                                comm2.Parameters.AddWithValue("@packcomments1", .Rows(i).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString)
                            End If
                            If comm2.ExecuteNonQuery() > 0 Then
                                success = success - 1
                                comm2.Parameters.Clear()
                                cmd1 = ""
                            Else
                                Throw New System.Exception("ΣΦΑΛΜΑ Ενημερώστε διαχειριστή: " + comm2.CommandText)
                            End If
                        End If
                    Next
                End With
            ElseIf TabControl1.SelectedIndex = 2 Then
                With AdvancedDataGridView3
                    For i As Integer = 0 To .Rows.Count - 1
                        Dim screc As String = "NULL"
                        Dim wquant As Double = 0
                        Dim m As String = ""
                        Dim ins As String = ""
                        Dim ins2 As String = ""
                        If Not IsDBNull(.Rows(i).Cells("Αποδέκτης").Value) AndAlso .Rows(i).Cells("Αποδέκτης").Value > 0 Then
                            screc = .Rows(i).Cells("Αποδέκτης").Value.ToString
                        End If
                        If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Then
                            m = " packcomments=@packcomments1"
                            ins = ",packcomments"
                            ins2 = ",@packcomments1"
                        ElseIf Form1.activeuserdpt = "SP" Then
                            m = "warecomments=@warecomments1"
                            ins = ",warecomments"
                            ins2 = ",@warecomments1"
                        ElseIf Form1.activeuserdpt = "PRD" Then
                            m = " prodcomments=@prodcomments1"
                            ins = ",prodcomments"
                            ins2 = ",@prodcomments1"
                        ElseIf Form1.activeuserdpt = "SA" Then
                            m = " prodcomments=@prodcomments1,warecomments=@warecomments1,packcomments=@packcomments1"
                            ins = ",prodcomments,warecomments,packcomments"
                            ins2 = ",@prodcomments1,@warecomments1,@packcomments1"
                        End If
                        If stlidwithdata.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                            If stlidtodel.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                                cmd1 = "delete from tbl_packerordclines where stlid=" + .Rows(i).Cells("id").Value.ToString
                            End If
                            If stlidtoadd.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                                cmd1 = "update tbl_packerordclines set sc_recipient=" + screc + m + " where  stlid=" + .Rows(i).Cells("id").Value.ToString
                            End If
                        Else
                            If stlidtoadd.Contains(.Rows(i).Cells("id").Value.ToString + "/1") Then
                                cmd1 = "insert into  tbl_packerordclines (stlid,ftrid,line" + ins + ",code,sc_recipient,ware_quantity) values (" + .Rows(i).Cells("id").Value.ToString + "," + ftrid.ToString + ",1" + ins2 + ",' " + .Rows(i).Cells("id").Value.ToString + "'," + screc + ",0)"
                            End If

                        End If
                        If cmd1 <> "" Then
                            success = success + 1
                            comm3.Connection = updconn
                            comm3.CommandText = cmd1

                            comm3.Transaction = transaction
                            If Form1.activeuserdpt = "PRD" Or Form1.activeuserdpt = "SA" Then
                                comm3.Parameters.AddWithValue("@prodcomments1", .Rows(i).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString)
                            End If
                            If Form1.activeuserdpt = "SP" Or Form1.activeuserdpt = "SA" Then
                                comm3.Parameters.AddWithValue("@warecomments1", .Rows(i).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString)
                            End If
                            If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "SA" Then
                                comm3.Parameters.AddWithValue("@packcomments1", .Rows(i).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString)
                            End If

                            If comm3.ExecuteNonQuery() > 0 Then
                                success = success - 1
                                Dim typetxt As String = ""
                                comm3.Parameters.Clear()
                            Else
                                Throw New System.Exception("ΣΦΑΛΜΑ Ενημερώστε διαχειριστή: " + comm3.CommandText)
                            End If

                            cmd1 = ""

                        End If
                    Next
                End With
            End If
            'ΕΝΗΜΕΡΩΣΗ ΠΙΝΑΚΩΝ ΣΧΟΛΙΑΣΜΟΥ ΠΑΡΑΓΓΕΛΙΑΣ
            Dim m2 As String = ""
            If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Then
                m2 = " packcomments=@packcomments4"
            ElseIf Form1.activeuserdpt = "SP" Then
                m2 = "warecomments=@warecomments4"
            ElseIf Form1.activeuserdpt = "PRD" Then
                m2 = " prodcomments=@prodcomments4"
            ElseIf Form1.activeuserdpt = "SA" Then
                m2 = " prodcomments=@prodcomments4,warecomments=@warecomments4,packcomments=@packcomments4"
            End If
            mastercmd = "update tbl_packerordercheck set " + m2
            mastercmd = mastercmd + " where ftrid=" + ftrid.ToString
            Dim check As Integer
            Dim check2 As Integer
            mastercomm.Connection = updconn
            mastercomm.CommandText = mastercmd
            mastercomm.Transaction = transaction
            If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "SA" Or Form1.activeuserdpt = "BP" Then
                mastercomm.Parameters.AddWithValue("@packcomments4", TextBox4.Text)
            End If
            If Form1.activeuserdpt = "SP" Or Form1.activeuserdpt = "SA" Then
                mastercomm.Parameters.AddWithValue("@warecomments4", TextBox2.Text)
            End If
            If Form1.activeuserdpt = "PRD" Or Form1.activeuserdpt = "SA" Then
                mastercomm.Parameters.AddWithValue("@prodcomments4", TextBox3.Text)
            End If
            If Form1.activeuserdpt = "PRD" Or Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "SP" Or Form1.activeuserdpt = "SA" Then
                check = mastercomm.ExecuteNonQuery() 'σχόλια παραγγελίας τμήματος
            Else check = 1
            End If
            CancelPreviousUserC.ExecuteNonQuery()
            check2 = usercmd.ExecuteNonQuery()
            Dim second As Integer
            comm4.CommandText = "[addcomments]"  'προσωπικά σχόλια παραγγελίας
            comm4.CommandType = CommandType.StoredProcedure
            comm4.Connection = updconn
            comm4.Transaction = transaction
            comm4.Parameters.Add("@FTRID", SqlDbType.Int).Value = ftrid
            comm4.Parameters.Add("@userid", SqlDbType.Int).Value = Form1.activeuserid
            comm4.Parameters.Add("@comments", SqlDbType.VarChar).Value = TextBox1.Text
            second = comm4.ExecuteNonQuery()
            If check > 0 And second > 0 And success = -1 And check2 > 0 Then
                success = 0
                Dim cmd As SqlCommand = New SqlCommand("PKRPRC_UPDATEORDERSTATUS", updconn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@FTRID", ftrid)
                cmd.Transaction = transaction
                cmd.ExecuteNonQuery()
                For i As Integer = stlidtoadd.Count - 1 To 0 Step -1
                    If Not stlidwithdata.Contains(stlidtoadd(i)) Then
                        stlidwithdata.Add(stlidtoadd(i))
                        stlidtoadd.RemoveAt(i)
                    End If
                Next
                transaction.Commit()
            Else
                success = -1
                transaction.Rollback()
            End If
            updconn.Close()
            comm1.Dispose()
            comm2.Dispose()
            comm3.Dispose()
            comm4.Dispose()
            usercmd.Dispose()
            mastercomm.Dispose()
            Return success
        Catch ex As Exception
            Try
                transaction.Rollback()
            Catch ex2 As Exception
                ' This catch block will handle any errors that may have occurred
                ' on the server that would cause the rollback to fail, such as
                ' a closed connection.
                Using errfrm As New errormsgbox(ex2.StackTrace.ToString, ex2.Message, "Rollback error!") : errfrm.ShowDialog() : End Using
            End Try
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Cursor.Current = ExtCursor1.Cursor
        Try
            Dim selecteddgv As Integer = TabControl1.SelectedIndex
            If selecteddgv = 0 Then
                If AdvancedDataGridView1.Rows.Count = 0 Then
                    Throw New System.Exception("Δεν υπάρχουν δισκόφρενα στη συγκεκριμένη παραγγελία!")
                    Return
                End If
            ElseIf selecteddgv = 1 Then
                If AdvancedDataGridView2.Rows.Count = 0 Then
                    Throw New System.Exception("Δεν υπάρχουν φερμουΐτ στη συγκεκριμένη παραγγελία!")
                    Return
                End If
            ElseIf selecteddgv = 2 Then
                If AdvancedDataGridView3.Rows.Count = 0 Then
                    Throw New System.Exception("Δεν υπάρχουν ανταλλακτικά στη συγκεκριμένη παραγγελία!")
                    Return

                End If
            End If

            Using clmn1 As New DataGridViewTextBoxColumn
                clmn1.Name = "Ελεύθερα"
                clmn1.HeaderText = "Ελεύθερα"

                Using clmn2 As New DataGridViewTextBoxColumn
                    clmn2.Name = "Δεσμευμένα"
                    clmn2.HeaderText = "Δεσμευμένα"
                    If Not CheckBox1.Checked Then
                        Dim iteidlist As New List(Of String)

                        If selecteddgv = 0 Then

                            For i As Integer = 0 To AdvancedDataGridView1.Rows.Count - 1
                                iteidlist.Add(AdvancedDataGridView1.Rows(i).Cells("iteid").Value.ToString)
                            Next
                        ElseIf selecteddgv = 1 Then

                            For i As Integer = 0 To AdvancedDataGridView2.Rows.Count - 1
                                iteidlist.Add(AdvancedDataGridView2.Rows(i).Cells("iteid").Value.ToString)
                            Next
                        ElseIf selecteddgv = 2 Then

                            For i As Integer = 0 To AdvancedDataGridView3.Rows.Count - 1
                                iteidlist.Add(AdvancedDataGridView3.Rows(i).Cells("iteid").Value.ToString)
                            Next
                        End If

                        iteidlist = iteidlist.Distinct.ToList
                        Dim str As String = ""
                        For Each item As String In iteidlist
                            If iteidlist.IndexOf(item) = 0 Then
                                str = str + item
                            Else
                                str = str + "," + item
                            End If
                        Next
                        Dim cmd As String = "select m.id as 'iteid',s1.lsumqty as 'free',s2.lsumqty as 'desm' from material m left join sc_qty_mantis s1 on m.id=s1.iteid left join sc_qty_mantis_returns s2 on m.id=s2.iteid where m.id in (" + str + ")"
                        Using sqlcmd As New SqlCommand(cmd, conn)
                            Using dt As New DataTable()
                                conn.Open()
                                Using reader As SqlDataReader = sqlcmd.ExecuteReader
                                    dt.Load(reader)
                                    conn.Close()

                                    If selecteddgv = 0 Then
                                        With AdvancedDataGridView1
                                            .AutoGenerateColumns = True
                                            If Not .Columns.Contains("Ελεύθερα") Then
                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn2)

                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn1)
                                            End If
                                            For i As Integer = 0 To .Rows.Count - 1
                                                For j As Integer = 0 To dt.Rows.Count - 1
                                                    If .Rows(i).Cells("iteid").Value = dt.Rows(j).Item("iteid") Then
                                                        .Rows(i).Cells("Ελεύθερα").Value = dt.Rows(j).Item("free")
                                                        .Rows(i).Cells("Δεσμευμένα").Value = dt.Rows(j).Item("desm")
                                                    End If
                                                Next
                                            Next
                                        End With
                                    ElseIf selecteddgv = 1 Then
                                        With AdvancedDataGridView2
                                            .AutoGenerateColumns = True
                                            If Not .Columns.Contains("Ελεύθερα") Then
                                                .Columns.Insert(.Columns("Barcode").Index - 1, clmn2)
                                                .Columns.Insert(.Columns("Barcode").Index - 1, clmn1)
                                            End If
                                            For i As Integer = 0 To .Rows.Count - 1
                                                For j As Integer = 0 To dt.Rows.Count - 1
                                                    If .Rows(i).Cells("iteid").Value = dt.Rows(j).Item("iteid") Then
                                                        .Rows(i).Cells("Ελεύθερα").Value = dt.Rows(j).Item("free")
                                                        .Rows(i).Cells("Δεσμευμένα").Value = dt.Rows(j).Item("desm")
                                                    End If
                                                Next
                                            Next
                                        End With
                                    ElseIf selecteddgv = 2 Then
                                        With AdvancedDataGridView3
                                            .AutoGenerateColumns = True
                                            If Not .Columns.Contains("Ελεύθερα") Then
                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn2)
                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn1)
                                            End If
                                            For i As Integer = 0 To .Rows.Count - 1
                                                For j As Integer = 0 To dt.Rows.Count - 1
                                                    If .Rows(i).Cells("iteid").Value = dt.Rows(j).Item("iteid") Then
                                                        .Rows(i).Cells("Ελεύθερα").Value = dt.Rows(j).Item("free")
                                                        .Rows(i).Cells("Δεσμευμένα").Value = dt.Rows(j).Item("desm")
                                                    End If
                                                Next
                                            Next
                                        End With
                                    End If
                                End Using
                            End Using
                        End Using

                    Else
                        Dim codesneeded As New List(Of String)

                        If selecteddgv = 0 Then
                            With AdvancedDataGridView1

                                For i As Integer = 0 To .Rows.Count - 1
                                    codesneeded.Add(.Rows(i).Cells("Κωδικός").Value.ToString.Substring(0, 8))
                                Next
                            End With
                        ElseIf selecteddgv = 1 Then
                            With AdvancedDataGridView2

                                For i As Integer = 0 To .Rows.Count - 1
                                    codesneeded.Add(.Rows(i).Cells("Barcode").Value.ToString.Substring(0, 8))
                                Next
                            End With
                        ElseIf selecteddgv = 2 Then
                            With AdvancedDataGridView3

                                For i As Integer = 0 To .Rows.Count - 1
                                    codesneeded.Add(.Rows(i).Cells("Κωδικός").Value.ToString.Substring(0, 8))
                                Next
                            End With
                        End If
                        codesneeded = codesneeded.Distinct.ToList
                        Dim str As String = ""
                        For Each item As String In codesneeded
                            If codesneeded.IndexOf(item) = 0 Then
                                str = str + "'" + item + "'"
                            Else
                                str = str + ",'" + item + "'"
                            End If
                        Next
                        Dim cmd As String = "select substring(m.code,1,8) as 'code',sum(s1.lsumqty) as 'free',sum(s2.lsumqty) as 'desm' from material m left join sc_qty_mantis s1 on m.id=s1.iteid left join sc_qty_mantis_returns s2 on m.id=s2.iteid where substring(m.code,1,8) in (" + str + ") group by substring(m.code,1,8)"
                        Using sqlcmd As New SqlCommand(cmd, conn)
                            Using dt As New DataTable()
                                conn.Open()
                                Using reader As SqlDataReader = sqlcmd.ExecuteReader
                                    dt.Load(reader)
                                    conn.Close()
                                    If selecteddgv = 0 Then
                                        With AdvancedDataGridView1
                                            .AutoGenerateColumns = True
                                            If Not .Columns.Contains("Ελεύθερα") Then
                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn2)
                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn1)
                                            End If
                                            For i As Integer = 0 To .Rows.Count - 1
                                                For j As Integer = 0 To dt.Rows.Count - 1
                                                    If .Rows(i).Cells("Κωδικός").Value.ToString.Substring(0, 8) = dt.Rows(j).Item("code") Then
                                                        .Rows(i).Cells("Ελεύθερα").Value = dt.Rows(j).Item("free")
                                                        .Rows(i).Cells("Δεσμευμένα").Value = dt.Rows(j).Item("desm")
                                                    End If
                                                Next
                                            Next
                                            .Columns("Ελεύθερα").ReadOnly = True
                                            .Columns("Δεσμευμένα").ReadOnly = True
                                        End With
                                    ElseIf selecteddgv = 1 Then
                                        With AdvancedDataGridView2
                                            .AutoGenerateColumns = True
                                            If Not .Columns.Contains("Ελεύθερα") Then
                                                .Columns.Insert(.Columns("Barcode").Index - 1, clmn2)
                                                .Columns.Insert(.Columns("Barcode").Index - 1, clmn1)
                                            End If
                                            For i As Integer = 0 To .Rows.Count - 1
                                                For j As Integer = 0 To dt.Rows.Count - 1
                                                    If .Rows(i).Cells("Barcode").Value.ToString.Substring(0, 8) = dt.Rows(j).Item("code") Then
                                                        .Rows(i).Cells("Ελεύθερα").Value = dt.Rows(j).Item("free")
                                                        .Rows(i).Cells("Δεσμευμένα").Value = dt.Rows(j).Item("desm")
                                                    End If
                                                Next
                                            Next
                                            .Columns("Ελεύθερα").ReadOnly = True
                                            .Columns("Δεσμευμένα").ReadOnly = True
                                        End With
                                    ElseIf selecteddgv = 2 Then
                                        With AdvancedDataGridView3
                                            .AutoGenerateColumns = True
                                            If Not .Columns.Contains("Ελεύθερα") Then
                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn2)
                                                .Columns.Insert(.Columns("Κωδικός").Index - 1, clmn1)
                                            End If
                                            For i As Integer = 0 To .Rows.Count - 1
                                                For j As Integer = 0 To dt.Rows.Count - 1
                                                    If .Rows(i).Cells("Κωδικός").Value.ToString.Substring(0, 8) = dt.Rows(j).Item("code") Then
                                                        .Rows(i).Cells("Ελεύθερα").Value = dt.Rows(j).Item("free")
                                                        .Rows(i).Cells("Δεσμευμένα").Value = dt.Rows(j).Item("desm")
                                                    End If
                                                Next
                                            Next
                                            .Columns("Ελεύθερα").ReadOnly = True
                                            .Columns("Δεσμευμένα").ReadOnly = True
                                        End With
                                    End If
                                End Using
                            End Using
                        End Using

                    End If
                End Using
            End Using
            For Each c As DataGridViewColumn In AdvancedDataGridView1.Columns
                c.DisplayIndex = c.Index
            Next
            For Each c As DataGridViewColumn In AdvancedDataGridView2.Columns
                c.DisplayIndex = c.Index
            Next
            For Each c As DataGridViewColumn In AdvancedDataGridView3.Columns
                c.DisplayIndex = c.Index
            Next
            Try
                AdvancedDataGridView1.Columns("Αποδέκτης").DisplayIndex = 1
                AdvancedDataGridView2.Columns("Αποδέκτης").DisplayIndex = 1
                AdvancedDataGridView3.Columns("Αποδέκτης").DisplayIndex = 1
            Catch
            End Try
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
        Cursor.Current = Cursors.Default
    End Sub


    Dim tabpage3selected As Boolean = False
    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Button2.Enabled = False
        Button1.Enabled = False

        If TabControl1.SelectedIndex = 3 Then
            Button4.Enabled = False
        Else
            Button4.Enabled = True
        End If
        If TabControl1.SelectedIndex = 1 Then
            CheckBox3.Visible = True
        Else
            CheckBox3.Visible = False
            CheckBox3.Checked = False
        End If
        If UserHasRights Or ((Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "BL") And TrackBar1.Value >= 6) Then
            If TabControl1.SelectedIndex = 2 Then
                CheckBox3.Visible = False
            End If
            If TabControl1.SelectedIndex = 1 Then
                CheckBox3.Visible = False
                If AdvancedDataGridView2.Rows.Count = 0 Then
                    Button2.Enabled = False
                    Button1.Enabled = False
                Else
                    CheckBox3.Visible = True
                    Button2.Enabled = True
                    Button1.Enabled = True
                End If
            End If
            If TabControl1.SelectedIndex = 2 Then
                If AdvancedDataGridView3.Rows.Count = 0 Then
                    Button2.Enabled = False
                    Button1.Enabled = False
                Else
                    Button2.Enabled = True
                    Button1.Enabled = True
                End If
            End If
            If TabControl1.SelectedIndex = 0 Then
                If AdvancedDataGridView1.Rows.Count = 0 Then
                    Button2.Enabled = False
                    Button1.Enabled = False
                Else
                    Button2.Enabled = True
                    Button1.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub AdvancedDataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim itemid As String = Me.AdvancedDataGridView1.Rows(e.RowIndex).Cells("iteid").Value.ToString
            Dim cmd As String = ""
            With Me.AdvancedDataGridView1
                If .Columns(e.ColumnIndex).Name = "Ημιέτοιμο1" Then
                    cmd = "select isnull(M_SPECID1,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Ημιέτοιμο2" Then
                    cmd = "select isnull( M_SPECID2,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Ημιέτοιμο3" Then
                    cmd = "select isnull(M_SPECID3,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Ελατήριο1" Then
                    cmd = "select isnull(M_SPECID4,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Ελατήριο2" Then
                    cmd = "select isnull(M_SPECID5,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Κιτ1" Then
                    cmd = "select isnull(M_SPECID6,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Κιτ2" Then
                    cmd = "select isnull(M_SPECID7,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Κιτ3" Then
                    cmd = "select isnull(M_SPECID8,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Αισθητήρας1" Then
                    cmd = "select isnull(M_SPECID9,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Αισθητήρας2" Then
                    cmd = "select isnull(M_SPECID10,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Πριτσίνια" Then
                    cmd = "select isnull(M_SPECID11,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Κουτί1" Then
                    cmd = "select isnull(M_SPECID12,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Κουτί2" Then
                    cmd = "select isnull(M_SPECID13,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Ετικέτα1" Then
                    cmd = "select isnull(M_SPECID14,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                ElseIf .Columns(e.ColumnIndex).Name = "Ετικέτα2" Then
                    cmd = "select isnull(M_SPECID15,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                    'ElseIf .Columns(e.ColumnIndex).Name = "ΠΟΣ ΑΠΟΘ" Or .Columns(e.ColumnIndex).Name = "Αποδέκτης" Then
                    '    Return
                Else
                    itemid = .Rows(e.RowIndex).Cells("iteid").Value.ToString
                End If
            End With
            If Not cmd = "" Then
                Using sqlcmd As New SqlCommand(cmd, conn)
                    conn.Open()
                    itemid = sqlcmd.ExecuteScalar()
                    conn.Close()
                    If itemid <> "0" Then
                        Using frm = New ItemDetails(itemid)
                            frm.ShowDialog()
                        End Using
                    End If
                End Using
            Else
                If Me.AdvancedDataGridView1.Columns(e.ColumnIndex).Name = "Ελεύθερα" Or Me.AdvancedDataGridView1.Columns(e.ColumnIndex).Name = "Δεσμευμένα" Then
                    If CheckBox1.Checked Then
                        Using frm = New Form17(Me.AdvancedDataGridView1.Rows(e.RowIndex).Cells("iteid").Value.ToString, Me.AdvancedDataGridView1.Rows(e.RowIndex).Cells("Κωδικός").Value.ToString)
                            frm.ShowDialog()
                        End Using
                    Else
                        Using frm = New WMSLocationStock(Me.AdvancedDataGridView1.Rows(e.RowIndex).Cells("iteid").Value.ToString, " ", Me.AdvancedDataGridView1.Rows(e.RowIndex).Cells("Κωδικός").Value.ToString)
                            frm.ShowDialog()
                        End Using
                    End If
                ElseIf itemid <> "0" Then
                    Using frm = New ItemDetails(itemid)
                        frm.ShowDialog()
                    End Using
                End If
            End If
        End If
    End Sub

    Private Sub AdvancedDataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView2.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim itemid As String = Me.AdvancedDataGridView2.Rows(e.RowIndex).Cells("iteid").Value.ToString
            Dim cmd As String = ""
            With AdvancedDataGridView2
                If .Columns(e.ColumnIndex).Name = "ΕΙΔ ΧΑΡ" Then
                    If .Rows(e.RowIndex).Cells("ΕΙΔ ΧΑΡ").Value.ToString.Contains("Rivets") Then

                        cmd = "select isnull(M_SPECID11,0) from material where id=" + .Rows(e.RowIndex).Cells("iteid").Value.ToString
                    End If
                ElseIf .Columns(e.ColumnIndex).Name = "Αποδέκτης" Then

                    Return
                Else
                    itemid = .Rows(e.RowIndex).Cells("iteid").Value.ToString

                End If
            End With
            If Not cmd = "" Then
                Using sqlcmd As New SqlCommand(cmd, conn)
                    conn.Open()
                    itemid = sqlcmd.ExecuteScalar()
                    conn.Close()
                End Using
            End If


            If Not cmd = "" Then
                Using sqlcmd As New SqlCommand(cmd, conn)
                    conn.Open()
                    itemid = sqlcmd.ExecuteScalar()
                    conn.Close()
                    If itemid <> "0" Then
                        Using frm = New ItemDetails(itemid)
                            frm.ShowDialog()
                        End Using
                    End If
                End Using
            Else
                If Me.AdvancedDataGridView2.Columns(e.ColumnIndex).Name = "Ελεύθερα" Or Me.AdvancedDataGridView2.Columns(e.ColumnIndex).Name = "Δεσμευμένα" Then
                    If CheckBox1.Checked Then
                        Using frm = New Form17(Me.AdvancedDataGridView2.Rows(e.RowIndex).Cells("iteid").Value.ToString, Me.AdvancedDataGridView2.Rows(e.RowIndex).Cells("Barcode").Value.ToString)
                            frm.ShowDialog()
                        End Using
                    Else
                        Using frm = New WMSLocationStock(Me.AdvancedDataGridView2.Rows(e.RowIndex).Cells("iteid").Value.ToString, " ", Me.AdvancedDataGridView2.Rows(e.RowIndex).Cells("Barcode").Value.ToString)
                            frm.ShowDialog()
                        End Using
                    End If
                ElseIf itemid <> "0" Then


                    Using frm = New ItemDetails(itemid)
                        frm.ShowDialog()
                    End Using
                End If
            End If
        End If
    End Sub

    Private Sub AdvancedDataGridView3_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView3.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim itemid As String = Me.AdvancedDataGridView3.Rows(e.RowIndex).Cells("iteid").Value.ToString
            With AdvancedDataGridView3
                If .Columns(e.ColumnIndex).Name = "Ελεύθερα" Or .Columns(e.ColumnIndex).Name = "Δεσμευμένα" Then
                    If CheckBox1.Checked Then
                        Using frm = New Form17(.Rows(e.RowIndex).Cells("iteid").Value.ToString, .Rows(e.RowIndex).Cells("Κωδικός").Value.ToString)
                            frm.ShowDialog()
                        End Using
                    Else
                        Using frm = New WMSLocationStock(.Rows(e.RowIndex).Cells("iteid").Value.ToString, " ", .Rows(e.RowIndex).Cells("Κωδικός").Value.ToString)
                            frm.ShowDialog()
                        End Using
                    End If

                ElseIf .Columns(e.ColumnIndex).Name = "Αποδέκτης" Then

                    Return

                ElseIf itemid <> "0" Then

                    Using frm = New ItemDetails(.Rows(e.RowIndex).Cells("iteid").Value.ToString)
                        frm.ShowDialog()
                    End Using
                End If
            End With
        End If
    End Sub



    Private Sub AdvancedDataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        'Try
        '    With AdvancedDataGridView1

        '        If .Columns(e.ColumnIndex).Name = "ΠΟΣ ΑΠΟΘ" And Not IsNothing(.Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value) _
        '            And Not IsDBNull(.Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value) Then
        '            Dim val As Double = .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value
        '            If val > .Rows(e.RowIndex).Cells("Ποσότητα").Value Then

        '                Throw New System.Exception("Δεν μπορείτε να ξεπεράσετε τη ποσότητα της παραγγελίας")
        '            End If
        '            If Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) AndAlso .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then
        '                .Rows(e.RowIndex).Cells("Ποσότητα").ReadOnly = False
        '            End If
        '        End If
        '    End With

        'Catch ex As Exception
        '    With AdvancedDataGridView1
        '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
        '        If Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) AndAlso .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then
        '            .Rows(e.RowIndex).Cells("Ποσότητα").ReadOnly = False
        '        End If
        '    End With
        '    If updconn.State = ConnectionState.Open Then
        '        updconn.Close()
        '    End If
        '    If conn.State = ConnectionState.Open Then
        '        conn.Close()
        '    End If
        '    Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

        'End Try
    End Sub

    Private Sub AdvancedDataGridView3_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        'Try
        '    With AdvancedDataGridView3

        '        If .Columns(e.ColumnIndex).Name = "ΠΟΣ ΑΠΟΘ" And Not IsNothing(.Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value) _
        '            And Not IsDBNull(.Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value) Then
        '            Dim val As Double = .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value
        '            If val > .Rows(e.RowIndex).Cells("Ποσότητα").Value Then

        '                Throw New System.Exception("Δεν μπορείτε να ξεπεράσετε τη ποσότητα της παραγγελίας")
        '            End If
        '            If Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) AndAlso .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then
        '                .Rows(e.RowIndex).Cells("Ποσότητα").ReadOnly = False
        '            End If
        '        End If
        '    End With

        'Catch ex As Exception
        '    With AdvancedDataGridView3
        '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
        '        If Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) AndAlso .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then
        '            .Rows(e.RowIndex).Cells("Ποσότητα").ReadOnly = False
        '        End If
        '    End With
        '    If updconn.State = ConnectionState.Open Then
        '        updconn.Close()
        '    End If
        '    If conn.State = ConnectionState.Open Then
        '        conn.Close()
        '    End If
        '    Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

        'End Try
    End Sub

    Private Sub AdvancedDataGridView2_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        'Try
        '    With AdvancedDataGridView2

        '        If .Columns(e.ColumnIndex).Name = "ΠΟΣ ΑΠΟΘ" And Not IsNothing(.Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value) _
        '            And Not IsDBNull(.Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value) Then
        '            Dim val As Double = .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value
        '            If val > .Rows(e.RowIndex).Cells("Ποσότητα").Value Then

        '                Throw New System.Exception("Δεν μπορείτε να ξεπεράσετε τη ποσότητα της παραγγελίας")
        '            End If
        '            If Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) AndAlso .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then
        '                .Rows(e.RowIndex).Cells("Ποσότητα").ReadOnly = False
        '            End If
        '        End If
        '    End With

        'Catch ex As Exception
        '    With AdvancedDataGridView2
        '        .Rows(e.RowIndex).Cells("ΠΟΣ ΑΠΟΘ").Value = DBNull.Value
        '        If Not isdbnull(.Rows(e.RowIndex).Cells("Αποδέκτης").Value) AndAlso .Rows(e.RowIndex).Cells("Αποδέκτης").Value = 3 Then
        '            .Rows(e.RowIndex).Cells("Ποσότητα").ReadOnly = False
        '        End If
        '    End With
        '    If updconn.State = ConnectionState.Open Then
        '        updconn.Close()
        '    End If
        '    If conn.State = ConnectionState.Open Then
        '        conn.Close()
        '    End If
        '    Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

        'End Try

    End Sub

    Private Sub AdvancedDataGridView1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        With AdvancedDataGridView1
            If (Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value)) _
            Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value)) _
             Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΕΞΑΓΩΓΩΝ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΕΞΑΓΩΓΩΝ").Value)) Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value))) Then


                Dim myBitmap As New Bitmap(My.Resources.comment_png_22707)
                Dim myIcon As Icon = Icon.FromHandle(myBitmap.GetHicon())
                Dim graphics As Graphics = e.Graphics
                Dim iconHeight As Integer = 14
                Dim iconWidth As Integer = 14
                Dim xPosition As Integer = e.RowBounds.X + (.RowHeadersWidth \ 2)
                Dim yPosition As Integer = e.RowBounds.Y + ((.Rows(e.RowIndex).Height - iconHeight) \ 2)
                Dim rectangle As New Rectangle(xPosition, yPosition, iconWidth, iconHeight)
                graphics.DrawIcon(myIcon, rectangle)
            End If
        End With
    End Sub

    Private Sub AdvancedDataGridView2_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        With AdvancedDataGridView2
            If (Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value)) _
            Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value)) _
             Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΕΞΑΓΩΓΩΝ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΕΞΑΓΩΓΩΝ").Value)) Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value))) Then

                Dim myBitmap As New Bitmap(My.Resources.comment_png_22707)
                Dim myIcon As Icon = Icon.FromHandle(myBitmap.GetHicon())
                Dim graphics As Graphics = e.Graphics
                Dim iconHeight As Integer = 14
                Dim iconWidth As Integer = 14
                Dim xPosition As Integer = e.RowBounds.X + (.RowHeadersWidth \ 2)
                Dim yPosition As Integer = e.RowBounds.Y + ((.Rows(e.RowIndex).Height - iconHeight) \ 2)
                Dim rectangle As New Rectangle(xPosition, yPosition, iconWidth, iconHeight)
                graphics.DrawIcon(myIcon, rectangle)
            End If
        End With
    End Sub

    Private Sub AdvancedDataGridView3_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        With AdvancedDataGridView3
            If (Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value)) _
            Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value)) _
             Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΕΞΑΓΩΓΩΝ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΕΞΑΓΩΓΩΝ").Value)) Or Not (IsDBNull(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value) OrElse String.IsNullOrWhiteSpace(.Rows(e.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value))) Then

                Dim myBitmap As New Bitmap(My.Resources.comment_png_22707)
                Dim myIcon As Icon = Icon.FromHandle(myBitmap.GetHicon())
                Dim graphics As Graphics = e.Graphics
                Dim iconHeight As Integer = 14
                Dim iconWidth As Integer = 14
                Dim xPosition As Integer = e.RowBounds.X + (.RowHeadersWidth \ 2)
                Dim yPosition As Integer = e.RowBounds.Y + ((.Rows(e.RowIndex).Height - iconHeight) \ 2)
                Dim rectangle As New Rectangle(xPosition, yPosition, iconWidth, iconHeight)
                graphics.DrawIcon(myIcon, rectangle)
            End If
        End With
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim status As Integer
        Using s As New SqlCommand("select status from tbl_packerordercheck where ftrid=" + ftrid.ToString, conn)
            conn.Open()
            status = s.ExecuteScalar
            conn.Close()
        End Using

        Dim p As String = "no"
        If Not (TabControl1.SelectedIndex = 1 And CheckBox3.Checked) Then
            Using frm1 As New whattoprint
                Dim result1 As DialogResult = frm1.ShowDialog
                If result1 = DialogResult.OK Then
                    p = frm1.retvalue
                End If
            End Using
        End If
        If p = "no" And Not CheckBox3.Checked Then
            Return
        End If
        If TabControl1.SelectedIndex = 0 Then
            If (Form1.activeuserdpt = "SP" And status >= 6) Or CheckBox2.Checked Then
                Using frm As New PrintBrakePadsOrderWarehouse(ftrid, status, p)
                    frm.ShowDialog()
                End Using
            Else

                'For i As Integer = 0 To AdvancedDataGridView1.Rows.Count - 1
                '    If AdvancedDataGridView1.Rows(i).Cells("Αποδέκτης").Value <> 1 And AdvancedDataGridView1.Rows(i).Visible Then
                '        p = False
                '        Exit For
                '    End If
                'Next
                Using frm As New PrintBrakePadsOrder(ftrid, status, p)
                    frm.ShowDialog()
                End Using
            End If


        ElseIf TabControl1.SelectedIndex = 1 Then
            If (Form1.activeuserdpt = "SP" And status >= 6) Or CheckBox3.Checked Or CheckBox2.Checked Then
                If CheckBox3.Checked Then
                    Using frm As New PrintBrakeLiningsOrderRivets(ftrid, status)
                        frm.ShowDialog()
                    End Using
                Else
                    Using frm As New PrintBrakeLiningsOrderWarehouse(ftrid, status, p)
                        frm.ShowDialog()
                    End Using
                End If

            Else







                Using frm As New PrintBrakeLiningsOrder(ftrid, status, p)
                    frm.ShowDialog()
                End Using



            End If
        ElseIf TabControl1.SelectedIndex = 2 Then
            Using frm As New SPORDER(ftrid, status)
                frm.ShowDialog()
            End Using
        End If

    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        If Not String.IsNullOrWhiteSpace(TextBox5.Text) Then
            PictureBox1.Visible = True
        Else
            PictureBox1.Visible = False
        End If
    End Sub


    Private Sub AdvancedDataGridView1_FilterStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView1.FilterStringChanged
        TryCast(AdvancedDataGridView1.DataSource, DataTable).DefaultView.RowFilter = AdvancedDataGridView1.FilterString

        'textBox_filter.Text = bindingSource_main.Filter
    End Sub

    Private Sub AdvancedDataGridView1_SortStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView1.SortStringChanged
        TryCast(AdvancedDataGridView1.DataSource, DataTable).DefaultView.Sort = AdvancedDataGridView1.SortString
        'textBox_sort.Text = bindingSource_main.Sort
    End Sub

    Private Sub AdvancedDataGridView2_FilterStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView2.FilterStringChanged
        TryCast(AdvancedDataGridView2.DataSource, DataTable).DefaultView.RowFilter = AdvancedDataGridView2.FilterString

        'textBox_filter.Text = bindingSource_main.Filter
    End Sub

    Private Sub AdvancedDataGridView2_SortStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView2.SortStringChanged
        TryCast(AdvancedDataGridView2.DataSource, DataTable).DefaultView.Sort = AdvancedDataGridView2.SortString
        'textBox_sort.Text = bindingSource_main.Sort
    End Sub

    Private Sub AdvancedDataGridView3_FilterStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView3.FilterStringChanged
        TryCast(AdvancedDataGridView3.DataSource, DataTable).DefaultView.RowFilter = AdvancedDataGridView3.FilterString

        'textBox_filter.Text = bindingSource_main.Filter
    End Sub

    Private Sub AdvancedDataGridView3_SortStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView3.SortStringChanged
        TryCast(AdvancedDataGridView3.DataSource, DataTable).DefaultView.Sort = AdvancedDataGridView3.SortString
        'textBox_sort.Text = bindingSource_main.Sort
    End Sub

    Private Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar1.Search
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = AdvancedDataGridView1.CurrentCell.ColumnIndex + 1 >= AdvancedDataGridView1.ColumnCount
            Dim endrow As Boolean = AdvancedDataGridView1.CurrentCell.RowIndex + 1 >= AdvancedDataGridView1.RowCount

            If endcol AndAlso endrow Then
                startColumn = AdvancedDataGridView1.CurrentCell.ColumnIndex
                startRow = AdvancedDataGridView1.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, AdvancedDataGridView1.CurrentCell.ColumnIndex + 1)
                startRow = AdvancedDataGridView1.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            AdvancedDataGridView1.CurrentCell = c
        End If
    End Sub


    Private Sub AdvancedDataGridViewSearchToolBar2_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar2.Search
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = AdvancedDataGridView2.CurrentCell.ColumnIndex + 1 >= AdvancedDataGridView2.ColumnCount
            Dim endrow As Boolean = AdvancedDataGridView2.CurrentCell.RowIndex + 1 >= AdvancedDataGridView2.RowCount

            If endcol AndAlso endrow Then
                startColumn = AdvancedDataGridView2.CurrentCell.ColumnIndex
                startRow = AdvancedDataGridView2.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, AdvancedDataGridView2.CurrentCell.ColumnIndex + 1)
                startRow = AdvancedDataGridView2.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = AdvancedDataGridView2.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = AdvancedDataGridView2.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            AdvancedDataGridView2.CurrentCell = c
        End If
    End Sub

    Private Sub AdvancedDataGridViewSearchToolBar3_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar3.Search
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = AdvancedDataGridView3.CurrentCell.ColumnIndex + 1 >= AdvancedDataGridView3.ColumnCount
            Dim endrow As Boolean = AdvancedDataGridView3.CurrentCell.RowIndex + 1 >= AdvancedDataGridView3.RowCount

            If endcol AndAlso endrow Then
                startColumn = AdvancedDataGridView3.CurrentCell.ColumnIndex
                startRow = AdvancedDataGridView3.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, AdvancedDataGridView3.CurrentCell.ColumnIndex + 1)
                startRow = AdvancedDataGridView3.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = AdvancedDataGridView3.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = AdvancedDataGridView3.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            AdvancedDataGridView3.CurrentCell = c
        End If
    End Sub

    Private Sub AdvancedDataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles AdvancedDataGridView1.KeyDown
        With AdvancedDataGridView1
            If e.KeyCode = Keys.Delete Then
                For Each c As DataGridViewCell In .SelectedCells
                    If .Columns(c.ColumnIndex).HeaderText = "Αποδέκτης" Then
                        c.Value = DBNull.Value
                        stlidtoadd.Add(.Rows(c.RowIndex).Cells("id").Value.ToString + "/1")

                        If String.IsNullOrWhiteSpace(newvalue) And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) _
                            And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) _
                            And IsDBNull(.Rows(c.RowIndex).Cells("Αποδέκτης").Value) Then
                            stlidtodel.Add(.Rows(c.RowIndex).Cells("id").Value.ToString + "/1")
                            stlidtoadd.Remove(.Rows(c.RowIndex).Cells("id").Value.ToString + "/1")
                        End If
                    End If
                Next
            End If

        End With

    End Sub

    Private Sub AdvancedDataGridView2_KeyDown(sender As Object, e As KeyEventArgs) Handles AdvancedDataGridView2.KeyDown
        With AdvancedDataGridView2
            If e.KeyCode = Keys.Delete Then
                For Each c As DataGridViewCell In .SelectedCells
                    If .Columns(c.ColumnIndex).HeaderText = "Αποδέκτης" Then
                        c.Value = DBNull.Value
                        stlidtoadd.Add(.Rows(c.RowIndex).Cells("id").Value.ToString + "/" + .Rows(c.RowIndex).Cells("Μέρος").Value.ToString)

                        If String.IsNullOrWhiteSpace(newvalue) And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) _
                            And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) _
                            And IsDBNull(.Rows(c.RowIndex).Cells("Αποδέκτης").Value) Then
                            stlidtodel.Add(.Rows(c.RowIndex).Cells("id").Value.ToString + "/" + .Rows(c.RowIndex).Cells("Μέρος").Value.ToString)
                            stlidtoadd.Remove(.Rows(c.RowIndex).Cells("id").Value.ToString + "/" + .Rows(c.RowIndex).Cells("Μέρος").Value.ToString)
                        End If
                    End If
                Next
            End If

        End With
    End Sub
    Private Sub AdvancedDataGridView3_KeyDown(sender As Object, e As KeyEventArgs) Handles AdvancedDataGridView3.KeyDown
        With AdvancedDataGridView3
            If e.KeyCode = Keys.Delete Then
                For Each c As DataGridViewCell In .SelectedCells
                    If .Columns(c.ColumnIndex).HeaderText = "Αποδέκτης" Then
                        c.Value = DBNull.Value
                        stlidtoadd.Add(.Rows(c.RowIndex).Cells("id").Value.ToString + "/1")

                        If String.IsNullOrWhiteSpace(newvalue) And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) _
                            And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) And String.IsNullOrWhiteSpace(.Rows(c.RowIndex).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) _
                            And IsDBNull(.Rows(c.RowIndex).Cells("Αποδέκτης").Value) Then
                            stlidtodel.Add(.Rows(c.RowIndex).Cells("id").Value.ToString + "/1")
                            stlidtoadd.Remove(.Rows(c.RowIndex).Cells("id").Value.ToString + "/1")
                        End If
                    End If
                Next
            End If

        End With
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim m2 As String = ""
        Dim mastercmd As String = ""
        If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Then
            m2 = " packcomments=@packcomments"

        ElseIf Form1.activeuserdpt = "SP" Then
            m2 = "warecomments=@warecomments"


        ElseIf Form1.activeuserdpt = "PRD" Then
            m2 = " prodcomments=@prodcomments"

        ElseIf Form1.activeuserdpt = "SA" Then
            m2 = " prodcomments=@prodcomments,warecomments=@warecomments,packcomments=@packcomments"


        End If
        mastercmd = "update tbl_packerordercheck set " + m2


        mastercmd = mastercmd + " where ftrid=" + ftrid.ToString


        Using mastercomm As New SqlCommand(mastercmd, updconn)
            If Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "SA" Then
                mastercomm.Parameters.Add("@packcomments", SqlDbType.Text).Value = TextBox4.Text
            End If
            If Form1.activeuserdpt = "SP" Or Form1.activeuserdpt = "SA" Then
                mastercomm.Parameters.Add("@warecomments", SqlDbType.Text).Value = TextBox2.Text

            End If
            If Form1.activeuserdpt = "PRD" Or Form1.activeuserdpt = "SA" Then
                mastercomm.Parameters.Add("@prodcomments", SqlDbType.Text).Value = TextBox3.Text
            End If



            updconn.Open()
            Dim first As Integer
            Dim second As Integer
            If m2 <> "" Then
                first = mastercomm.ExecuteNonQuery()
            Else
                first = 1
            End If


            Using comm As New SqlCommand("[addcomments]")
                comm.CommandType = CommandType.StoredProcedure
                comm.Connection = updconn
                comm.Parameters.Add("@FTRID", SqlDbType.Int).Value = ftrid
                comm.Parameters.Add("@userid", SqlDbType.Int).Value = Form1.activeuserid
                comm.Parameters.Add("@comments", SqlDbType.VarChar).Value = TextBox1.Text
                second = comm.ExecuteNonQuery()
            End Using

            If first > 0 And second > 0 Then
                Label28.Text = "Επιτυχής αποθήκευση!"
                Label28.ForeColor = Color.Green
                Label28.Visible = True
                Form1.orderdgv_refresh()
            Else
                Label28.Text = "Κάτι δε πήγε καλά!"
                Label28.ForeColor = Color.Red
                Label28.Visible = True
            End If
            updconn.Close()
        End Using
    End Sub

    Private Sub AdvancedDataGridView1_CellBeginEdit_1(sender As Object, e As DataGridViewCellCancelEventArgs) Handles AdvancedDataGridView1.CellBeginEdit

    End Sub
    Dim stlineid As Integer
    Dim itemid As Integer
    Private Sub AdvancedDataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AdvancedDataGridView1.CellMouseClick
        AdvancedDataGridView1.ClearSelection()

        If e.Button = MouseButtons.Right AndAlso e.RowIndex > -1 And (Form1.activeuserdpt = "EX" Or Form1.activeuserdpt = "SA") Then
            AdvancedDataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Selected = True
            ContextMenuStrip2.Show(MousePosition.X, MousePosition.Y)
            stlineid = AdvancedDataGridView1.Rows(e.RowIndex).Cells("id").Value
            itemid = AdvancedDataGridView1.Rows(e.RowIndex).Cells("iteid").Value
        End If
    End Sub
    Private Sub AdvancedDataGridView2_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AdvancedDataGridView2.CellMouseClick
        AdvancedDataGridView2.ClearSelection()

        If e.Button = MouseButtons.Right AndAlso e.RowIndex > -1 And (Form1.activeuserdpt = "EX" Or Form1.activeuserdpt = "SA") Then
            AdvancedDataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex).Selected = True
            ContextMenuStrip2.Show(MousePosition.X, MousePosition.Y)
            stlineid = AdvancedDataGridView2.Rows(e.RowIndex).Cells("id").Value
            itemid = AdvancedDataGridView2.Rows(e.RowIndex).Cells("iteid").Value
        End If
    End Sub
    Private Sub AdvancedDataGridView3_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AdvancedDataGridView3.CellMouseClick
        AdvancedDataGridView3.ClearSelection()

        If e.Button = MouseButtons.Right AndAlso e.RowIndex > -1 And (Form1.activeuserdpt = "EX" Or Form1.activeuserdpt = "SA") Then
            AdvancedDataGridView3.Rows(e.RowIndex).Cells(e.ColumnIndex).Selected = True
            ContextMenuStrip2.Show(MousePosition.X, MousePosition.Y)
            stlineid = AdvancedDataGridView3.Rows(e.RowIndex).Cells("id").Value
            itemid = AdvancedDataGridView3.Rows(e.RowIndex).Cells("iteid").Value
        End If
    End Sub

    Private Sub ΑφαίρεσηΕίδουςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑφαίρεσηΕίδουςToolStripMenuItem.Click
        Try
            Dim ostatus As Integer = -1
            Using sqlcmd As New SqlCommand("Select isnull(status,-1) from tbl_packerordercheck where ftrid=" + ftrid.ToString, conn)
                conn.Open()
                ostatus = sqlcmd.ExecuteScalar
                conn.Close()

            End Using
            Dim ouser As Integer = -1
            Using sqlcmd As New SqlCommand("Select colidsalesman from fintrade where id=" + ftrid.ToString, conn)
                conn.Open()
                ouser = sqlcmd.ExecuteScalar
                conn.Close()

            End Using
            If Form1.activeuser <> "SUPERVISOR" AndAlso Form1.activeuseraid <> ouser Then
                MessageBox.Show("Δεν μπορείτε να διαγράψετε είδος σε παραγγελία άλλου πωλητή!",
                                                         "ΠΡΟΣΟΧΗ!!!",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Error)
                Return
            End If

            If ostatus = 0 Then
                Dim result As DialogResult = MessageBox.Show("Η παραγγελία δεν έχει ελεγχθεί ακόμα. Μπορείτε να αλλάξετε το είδος απευθείας στο ΠΑΞ εντός του Ατλαντίς, αφου ενημερώσετε τον Θ.Καψάνη",
                                                         "ΠΡΟΣΟΧΗ!!!",
                                                         MessageBoxButtons.OKCancel,
                                                         MessageBoxIcon.Warning)
                Return

            End If
            If ostatus = 12 Then
                Dim result As DialogResult = MessageBox.Show("Η παραγγελία έχει αποσταλεί. Επικοινωνήστε με τον διαχεριστή.",
                                                         "ΠΡΟΣΟΧΗ!!!",
                                                         MessageBoxButtons.OKCancel,
                                                         MessageBoxIcon.Warning)
                Return
            End If
            Using comm1 As New SqlCommand("select m.code,m.description,t.id,t3.code as inpallets 
from material m inner join storetradelines stl on stl.iteid=m.id 
left join  tbl_packerordclines t on t.stlid=stl.id and t.line=1
left join tbl_palletlines t2 on t2.stlid=t.stlid left join tbl_palletheaders t3 on t3.id=t2.palletid where stl.id=" + stlineid.ToString + "   and stl.ftrid  = " + ftrid.ToString, conn)
                Using dt2 = New DataTable()
                    conn.Open()
                    Using reader2 As SqlDataReader = comm1.ExecuteReader

                        dt2.Load(reader2)
                        conn.Close()
                        If IsDBNull(dt2.Rows(0).Item("inpallets")) Then
                            Dim result As DialogResult = MessageBox.Show("Μετά τη διαγραφή του είδους στον PACKER, θα πρέπει να το διαγράψετε χειροκίνητα από το ΠΑΞ στο Ατλαντίς. Είστε σίγουροι;",
                                                         "ΠΡΟΣΟΧΗ!!!",
                                                         MessageBoxButtons.OKCancel,
                                                         MessageBoxIcon.Warning)
                            If result = DialogResult.OK Then
                                Using comm2 As New SqlCommand("delete from tbl_packerordclines where stlid=" + stlineid.ToString + " and ftrid =" + ftrid.ToString, updconn)
                                    updconn.Open()
                                    Dim success = comm2.ExecuteNonQuery
                                    If success > 0 Then
                                        MessageBox.Show("ΣΗΜΑΝΤΙΚΟ!! ΤΩΡΑ Διαγράψτε το είδος " + dt2.Rows(0).Item("code") + " από το ΠΑΞ στο Ατλαντίς. ΣΗΜΑΝΤΙΚΟ!! ΤΩΡΑ ΝΑ ΓΙΝΕΙ Η ΔΙΑΓΡΑΦΗ.",
                                                         "ΠΡΟΣΟΧΗ!!!!",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Warning)
                                        Using cmd As New SqlCommand("dbo.notifier_stlid", updconn)
                                            cmd.CommandType = CommandType.StoredProcedure
                                            cmd.Parameters.AddWithValue("@SID", stlineid)
                                            cmd.Parameters.AddWithValue("@FTRID", ftrid)
                                            cmd.Parameters.AddWithValue("@ITEID", itemid)
                                            cmd.Parameters.AddWithValue("@type", 1)
                                            cmd.Parameters.AddWithValue("@user", Form1.activeuser)

                                            cmd.ExecuteNonQuery()

                                        End Using
                                    Else
                                        MessageBox.Show("Απέτυχε η διαγραφή του είδους " + dt2.Rows(0).Item("code") + " στον PACKER. Ενημερώστε τον διαχειριστή.",
                                                         "Αποτυχία",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Error)
                                    End If

                                    updconn.Close()
                                End Using
                            Else
                                Return
                            End If



                        Else
                            Dim pallets = dt2.AsEnumerable().Select(Function(d) DirectCast(d(3).ToString(), Object)).ToArray()
                            Dim palletlist = String.Join(",", pallets)
                            MessageBox.Show("Το είδος έχει κατανεμηθεί στις παλέτες " + palletlist + ". Δεν επιτρέπεται η διαγραφή του. Πρέπει να διαγραφεί πρώτα από όλες τις παλέτες και έπειτα ξαναπροσπαθήστε.",
                                                     "Δεν επιτρέπεται",
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error)
                            Return
                        End If
                    End Using

                End Using
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

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Using frm As New reldocs(docs)
            frm.ShowDialog()

        End Using
    End Sub

    Private Sub dgv1bw_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles dgv1bw.DoWork
        Dim dgv1bwconn As New SqlConnection(connString)
        Dim dt = New DataTable()
        Try
            Using comm7 As New SqlCommand(cmd7, dgv1bwconn)
                dgv1bwconn.Open()
                Using reader As SqlDataReader = comm7.ExecuteReader
                    dt.Load(reader)
                End Using
            End Using
        Catch

        Finally
            dgv1bwconn.Close()
            dgv1bwconn.Dispose()
            e.Result = dt
            dt.Dispose()
        End Try
    End Sub

    Dim l1() As String = {"PRD", "SA"}
    Dim l2() As String = {"SP", "SA"}
    Dim l3() As String = {"BL", "BP", "SA"}

    Private Sub dgv1_WorkCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles dgv1bw.RunWorkerCompleted
        Dim dt = e.Result

        Try
            For i As Integer = 0 To dt.Columns.Count - 1
                Dim c As DataColumn = dt.Columns(i)
                With dt.Columns(i)
                    c.ReadOnly = False
                    If c.ColumnName = "ΣΧ ΠΑΡΑΓΩΓΗΣ" Or c.ColumnName = "ΣΧ ΑΠΟΘΗΚΗΣ" Or c.ColumnName = "ΣΧ ΣΥΣΚΕΥ" Or c.ColumnName = "Αποδέκτης" Then
                        c.AllowDBNull = True
                    End If
                    If c.ColumnName = "ΠΟΣ ΑΠΟΘ" Or (c.ColumnName = "ΣΧ ΠΑΡΑΓΩΓΗΣ" And (l1.Contains(Form1.activeuserdpt))) _
                                   Or (c.ColumnName = "ΣΧ ΑΠΟΘΗΚΗΣ" And (l2.Contains(Form1.activeuserdpt))) _
                                   Or (c.ColumnName = "ΣΧ ΣΥΣΚΕΥ" And (l3.Contains(Form1.activeuserdpt))) Or c.ColumnName = "scr" Then
                        c.ReadOnly = False
                    Else
                        c.ReadOnly = True
                    End If
                End With
            Next
            If Not dt.Columns.Contains("AA") Then
                dt.Columns.Add("AA")
            End If
            dt.Columns("AA").SetOrdinal(0)
            dt.Columns("ΣΧ ΕΞΑΓΩΓΩΝ").SetOrdinal(2)
            dt.Columns("ΣΧ ΑΠΟΘΗΚΗΣ").SetOrdinal(3)
            dt.Columns("ΣΧ ΠΑΡΑΓΩΓΗΣ").SetOrdinal(4)
            dt.Columns("ΣΧ ΣΥΣΚΕΥ").SetOrdinal(5)
            dt.Columns("Ποσότητα").SetOrdinal(6)
            dt.Columns("AA").ReadOnly = False
            AdvancedDataGridView1.DataSource = dt
            AdvancedDataGridView1.AutoGenerateColumns = False

            With Me.AdvancedDataGridView1


                If .Columns.Count > 0 Then

                    Using aclmn As New DataGridViewTextBoxColumn
                        Using cbclmn2 As New DataGridViewComboBoxColumn
                            With cbclmn2
                                .Name = "Αποδέκτης"
                                .HeaderText = "Αποδέκτης"
                                .SortMode = DataGridViewColumnSortMode.Automatic
                            End With
                            With cbclmn2
                                .DataSource = ynTable
                                .DataPropertyName = "scr"
                                .DisplayMember = "name"
                                .ValueMember = "value"
                            End With
                            If Not .Columns.Contains("Αποδέκτης") Then
                                .Columns.Insert(1, cbclmn2)
                            End If
                            For Each C As DataGridViewColumn In .Columns
                                If Not C.Name = "Αποδέκτης" Then
                                    C.ReadOnly = True
                                End If
                                'If C.Name = "ΣΧ ΕΞΑΓΩΓΩΝ" Then
                                '    C.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                                'Else
                                '    C.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                'End If
                            Next

                            .Columns("id").Visible = False
                            .Columns("scr").Visible = False

                            .Columns("iteid").Visible = False
                            If UserHasRecEditRights Then
                                .Columns("Αποδέκτης").ReadOnly = False
                            End If
                            For j As Integer = 0 To .Rows.Count - 1
                                .Rows(j).Cells("AA").Value = j + 1
                            Next

                            Using boldfont = New Font(DefaultFont, FontStyle.Bold)
                                .Columns("Ποσότητα").DefaultCellStyle.Font = boldfont
                                .Columns("Ποσότητα").DefaultCellStyle.BackColor = Color.LightGray
                                .AllowUserToAddRows = False
                                If Not dgv1firstload Then
                                    For i As Integer = 0 To .Rows.Count - 1
                                        If Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) Or
Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) Or
Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) Or Not IsDBNull(.Rows(i).Cells("scr").Value) Then
                                            stlidwithdata.Add(.Rows(i).Cells("id").Value.ToString + "/1")
                                        End If

                                    Next
                                End If
                                dgv1firstload = True
                                dt.Columns("AA").ReadOnly = True



                            End Using
                        End Using
                    End Using
                End If
            End With

            With dt
                If .Columns.Contains("scr") And (Form1.activeuserocu = 1) And TrackBar1.Value < 6 Then
                    .Columns("scr").ReadOnly = False
                    AdvancedDataGridView1.Columns("Αποδέκτης").ReadOnly = False
                Else
                    .Columns("scr").ReadOnly = True
                    AdvancedDataGridView1.Columns("Αποδέκτης").ReadOnly = True
                End If

            End With
            p.SetValue(Me.AdvancedDataGridView1, True, Nothing)
            Dim lst1 As New DataGridViewColumnCollection(AdvancedDataGridView1)
            With AdvancedDataGridView1
                For Each c As DataGridViewColumn In .Columns
                    If c.Visible Then
                        lst1.Add(c.Clone)
                        If c.HeaderText = "ΣΧ ΕΞΑΓΩΓΩΝ" Then
                            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                        Else
                            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        End If
                    End If

                Next
                If (l1.Contains(Form1.activeuserdpt)) Then
                    .Columns("ΣΧ ΠΑΡΑΓΩΓΗΣ").ReadOnly = False
                    'dt.COLUMNS("ΣΧ ΠΑΡΑΓΩΓΗΣ").ReadOnly = False
                End If
                If (l2.Contains(Form1.activeuserdpt)) Then
                    .Columns("ΣΧ ΑΠΟΘΗΚΗΣ").ReadOnly = False
                    'dt.COLUMNS("ΣΧ ΑΠΟΘΗΚΗΣ").ReadOnly = False
                End If
                If ((l3.Contains(Form1.activeuserdpt))) Then
                    .Columns("ΣΧ ΣΥΣΚΕΥ").ReadOnly = False
                    ' dt.COLUMNS("ΣΧ ΣΥΣΚΕΥ").ReadOnly = False
                End If
            End With

            AdvancedDataGridViewSearchToolBar1.SetColumns(lst1)

            AdvancedDataGridView1.Columns("ΣΧ ΕΞΑΓΩΓΩΝ").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            'AdvancedDataGridView1.Columns("ΣΧ ΕΞΑΓΩΓΩΝ").Width = 50

        Catch

        Finally
            AdvancedDataGridView1.Visible = True
            If AdvancedDataGridView1.Rows.Count > 0 Then
                Label8.Visible = True
            End If
            dt.dispose()
        End Try
    End Sub

    Private Sub dgv2bw_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles dgv2bw.DoWork
        Dim dgv2bwconn As New SqlConnection(connString)
        Dim dt2 As New DataTable()
        Try
            Dim cmd8 As String = "select 00 as 'AA'  ,[CODE2]  as 'Κωδικός'   ,z.[id]      ,[iteid] ,z.[CODE] as 'Barcode',z.subcode1 as 'ΕΝΑΛ ΚΩΔ' ,[special] as 'ΕΙΔ ΧΑΡ'     ,mnf as 'ΜΑΡΚΑ',[PRIMARYQTY]    as 'Ποσότητα'  ,[QTY]  as 'Ποιότητα'    ,[MU]   as 'M/M'         , [part] as 'Μέρος'  ,[WVA]   as 'WVA'  ,[WIDTH]  as 'Πλάτος'    ,[THICKNESS1] as 'Πάχος1'
      ,[THICKNESS2]  as 'Πάχος2'    ,[THICKNESS3]  as 'Πάχος3'    ,[EXLENGTH]   as 'Μήκος εξ'   ,[SETOF]  as 'Σετ των'    ,[MOLD]   as 'Καλούπι'   ,[TEM]      ,[HMR] as 'Ημέρ.',[comments] as 'ΣΧ ΕΞΑΓΩΓΩΝ',isnull([PRODCOMMENTS],'') as 'ΣΧ ΠΑΡΑΓΩΓΗΣ',isnull([WARECOMMENTS],'') as 'ΣΧ ΑΠΟΘΗΚΗΣ' ,isnull([PACKCOMMENTS],'') as 'ΣΧ ΣΥΣΚΕΥ'
  ,[sc_recipient] as scr,[M_PARTNO]  as 'Part Number',z.ftrid FROM [Z_PACKER_BLORDER] z left join [TBL_PACKERORDCLINES] t on t.ftrid=z.ftrid and t.stlid=z.id and t.line=Z.PART where z.ftrid=" + ftrid.ToString + " order by 2" '[PARTQTY]    as 'Ποσότητα Ημ/μου' ,
            Using comm8 As New SqlCommand(cmd8, dgv2bwconn)
                dgv2bwconn.Open()
                Using reader2 As SqlDataReader = comm8.ExecuteReader
                    dt2.Load(reader2)

                End Using
            End Using
        Catch
        Finally
            e.Result = dt2
            dt2.Dispose()
            dgv2bwconn.Close()
            dgv2bwconn.Dispose()
        End Try
    End Sub

    Private Sub dgv2bw_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles dgv2bw.RunWorkerCompleted
        Dim dgv2bwconn As New SqlConnection(connString)
        Dim cmd11 As String = "select isnull(sum(isnull(primaryqty,0)),0) from storetradelines s left join material m on s.iteid=m.id where ftrid=" + ftrid.ToString + " and substring(m.code,1,3) in ('101','118','104','105')"
        Dim DT2 As DataTable = e.Result
        Try
            Using comm11 As New SqlCommand(cmd11, dgv2bwconn)
                dgv2bwconn.Open()
                Dim blquant As Double = comm11.ExecuteScalar()
                Label9.Text = "Sets φερμουΐτ: " + blquant.ToString
                For i As Integer = 0 To DT2.Columns.Count - 1
                    Dim c As DataColumn = DT2.Columns(i)
                    With DT2.Columns(i)
                        c.ReadOnly = False
                        If c.ColumnName = "ΣΧ ΠΑΡΑΓΩΓΗΣ" Or c.ColumnName = "ΣΧ ΑΠΟΘΗΚΗΣ" Or c.ColumnName = "ΣΧ ΣΥΣΚΕΥ" Or c.ColumnName = "Αποδέκτης" Then
                            c.AllowDBNull = True
                        End If
                        If c.ColumnName = "ΠΟΣ ΑΠΟΘ" Or (c.ColumnName = "ΣΧ ΠΑΡΑΓΩΓΗΣ" And (l1.Contains(Form1.activeuserdpt))) _
                                   Or (c.ColumnName = "ΣΧ ΑΠΟΘΗΚΗΣ" And (l2.Contains(Form1.activeuserdpt))) _
                                   Or (c.ColumnName = "ΣΧ ΣΥΣΚΕΥ" And (l3.Contains(Form1.activeuserdpt))) Or c.ColumnName = "scr" Then
                            c.ReadOnly = False

                        Else
                            c.ReadOnly = True
                        End If
                    End With
                Next
                If Not DT2.Columns.Contains("AA") Then
                    DT2.Columns.Add("AA")
                End If
                DT2.Columns("AA").SetOrdinal(0)
                DT2.Columns("ΣΧ ΕΞΑΓΩΓΩΝ").SetOrdinal(2)
                DT2.Columns("ΣΧ ΑΠΟΘΗΚΗΣ").SetOrdinal(3)
                DT2.Columns("ΣΧ ΠΑΡΑΓΩΓΗΣ").SetOrdinal(4)
                DT2.Columns("ΣΧ ΣΥΣΚΕΥ").SetOrdinal(5)
                DT2.Columns("Ποσότητα").SetOrdinal(6)
                DT2.Columns("AA").ReadOnly = False
                AdvancedDataGridView2.DataSource = DT2
                AdvancedDataGridView2.AutoGenerateColumns = False


                With AdvancedDataGridView2


                    If .Columns.Count > 0 Then

                        Using aclmn2 As New DataGridViewTextBoxColumn
                            Using cbclmn2 As New DataGridViewComboBoxColumn
                                With cbclmn2
                                    .Name = "Αποδέκτης"
                                    .HeaderText = "Αποδέκτης"
                                    .SortMode = DataGridViewColumnSortMode.Automatic
                                End With

                                With cbclmn2

                                    .DataSource = ynTable
                                    .DataPropertyName = "scr"
                                    .DisplayMember = "name"
                                    .ValueMember = "value"
                                End With
                                If Not .Columns.Contains("Αποδέκτης") Then


                                    .Columns.Insert(1, cbclmn2)



                                End If
                                For Each C As DataGridViewColumn In .Columns
                                    If Not C.Name = "Αποδέκτης" Then
                                        C.ReadOnly = True
                                    End If
                                Next



                                Dim o As Integer = 1
                                If .DataSource.Rows.Count > 0 Then
                                    .DataSource.Rows(0).item("AA") = 1
                                End If
                                If UserHasRecEditRights Then
                                    .Columns("Αποδέκτης").ReadOnly = False
                                End If
                                For j As Integer = 1 To .DataSource.Rows.Count - 1
                                    If .DataSource.Rows(j).item("Barcode") <> .DataSource.Rows(j - 1).item("Barcode") Then
                                        o += 1
                                        .DataSource.Rows(j).item("AA") = o

                                    Else
                                        .DataSource.Rows(j).item("AA") = o
                                        '.DataSource.Rows(j).item("Barcode") = "   "
                                        '.DataSource.rows(j).item("Ποσότητα") = DBNull.Value
                                    End If
                                Next


                                .Columns("id").Visible = False
                                .Columns("scr").Visible = False

                                .Columns("iteid").Visible = False
                                Using boldfont = New Font(DefaultFont, FontStyle.Bold)

                                    .Columns("Ποσότητα").DefaultCellStyle.Font = boldfont
                                    .Columns("Ποσότητα").DefaultCellStyle.BackColor = Color.LightGray

                                    .Columns("TEM").DefaultCellStyle.BackColor = Color.LightYellow
                                    .Columns("Ημέρ.").DefaultCellStyle.BackColor = Color.LightYellow

                                    .AllowUserToAddRows = False
                                    If Not dgv2firstload Then


                                        For i As Integer = 0 To .Rows.Count - 1
                                            If Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) Or
                Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) Or
                Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) Or Not IsDBNull(.Rows(i).Cells("scr").Value) Then
                                                stlidwithdata.Add(.Rows(i).Cells("id").Value.ToString + "/" + .Rows(i).Cells("Μέρος").Value.ToString)
                                            End If

                                        Next
                                    End If



                                End Using

                            End Using
                        End Using
                    End If

                End With
                With DT2
                    If .Columns.Contains("scr") And (Form1.activeuserocu = 1) And TrackBar1.Value < 6 Then
                        .Columns("scr").ReadOnly = False
                        AdvancedDataGridView2.Columns("Αποδέκτης").ReadOnly = False
                    Else
                        .Columns("scr").ReadOnly = True
                        AdvancedDataGridView2.Columns("Αποδέκτης").ReadOnly = True
                    End If
                End With
                p2.SetValue(Me.AdvancedDataGridView2, True, Nothing)
                Dim lst2 As New DataGridViewColumnCollection(AdvancedDataGridView2)
                With AdvancedDataGridView2
                    For Each c As DataGridViewColumn In .Columns
                        If c.Visible Then
                            lst2.Add(c.Clone)
                            If c.HeaderText = "ΣΧ ΕΞΑΓΩΓΩΝ" Then
                                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                            Else
                                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                            End If
                        End If

                    Next
                    If (l1.Contains(Form1.activeuserdpt)) Then
                        .Columns("ΣΧ ΠΑΡΑΓΩΓΗΣ").ReadOnly = False
                        'dt.COLUMNS("ΣΧ ΠΑΡΑΓΩΓΗΣ").ReadOnly = False
                    End If
                    If (l2.Contains(Form1.activeuserdpt)) Then
                        .Columns("ΣΧ ΑΠΟΘΗΚΗΣ").ReadOnly = False
                        'dt.COLUMNS("ΣΧ ΑΠΟΘΗΚΗΣ").ReadOnly = False
                    End If
                    If ((l3.Contains(Form1.activeuserdpt))) Then
                        .Columns("ΣΧ ΣΥΣΚΕΥ").ReadOnly = False
                        ' dt.COLUMNS("ΣΧ ΣΥΣΚΕΥ").ReadOnly = False
                    End If
                End With
                AdvancedDataGridViewSearchToolBar2.SetColumns(lst2)

                AdvancedDataGridView2.Columns("ΣΧ ΕΞΑΓΩΓΩΝ").DefaultCellStyle.WrapMode = DataGridViewTriState.True
                DT2.Columns("AA").ReadOnly = True
            End Using
        Catch

        Finally
            AdvancedDataGridView2.Visible = True
            If AdvancedDataGridView2.Rows.Count > 0 Then
                Label9.Visible = True
            End If
            DT2.Dispose()
            dgv2bwconn.Close()
            dgv2bwconn.Dispose()
        End Try

    End Sub

    Private Sub dgv3bw_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles dgv3bw.DoWork
        Dim dgv3bwconn As New SqlConnection(connString)
        Dim cmd9 As String = "SELECT z.[id]      ,[iteid]      ,z.[CODE]  as 'Κωδικός' ,z.subcode1 as 'ΕΝΑΛ ΚΩΔ'   ,[SUBCODE2]   as 'Παλ.Κωδικός'   ,[FACTORYCODE]   as 'Κωδ. Εργοστασίου'   ,[DESCR]  as 'Περιγραφή'    ,[PRIMARYQTY]  as 'Ποσότητα'    ,[COMMENTS]  as 'ΣΧ ΕΞΑΓΩΓΩΝ', isnull([PRODCOMMENTS],'') as 'ΣΧ ΠΑΡΑΓΩΓΗΣ',isnull([WARECOMMENTS],'') as 'ΣΧ ΑΠΟΘΗΚΗΣ' ,isnull([PACKCOMMENTS],'') as 'ΣΧ ΣΥΣΚΕΥ'
,[sc_recipient] as scr FROM [dbo].[Z_PACKER_SPORDER] z left join [TBL_PACKERORDCLINES] t on t.stlid=z.id where z.id in (select id from storetradelines where ftrid=" + ftrid.ToString + ") order by 3"
        Dim dt3 = New DataTable()
        Try
            Using comm9 As New SqlCommand(cmd9, dgv3bwconn)
                dgv3bwconn.Open()
                Using reader3 As SqlDataReader = comm9.ExecuteReader
                    dt3.Load(reader3)

                End Using
            End Using

        Catch
        Finally
            e.Result = dt3
            dt3.Dispose()
            dgv3bwconn.Close()
            dgv3bwconn.Dispose()
        End Try


    End Sub

    Private Sub dgv3bw_WorkCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles dgv3bw.RunWorkerCompleted
        Dim dgv3bwconn As New SqlConnection(connString)
        Dim cmd12 As String = "select isnull(sum(isnull(primaryqty,0)),0) from storetradelines s left join material m on s.iteid=m.id where ftrid=" + ftrid.ToString + " and substring(m.code,1,3)<>'202' and  substring(m.code,1,1)='2' "
        Dim dt3 As DataTable = e.Result
        Try
            Using comm12 As New SqlCommand(cmd12, dgv3bwconn)
                dgv3bwconn.Open()
                Dim spquant As Double = comm12.ExecuteScalar()
                Label10.Text = "Συνολική ποσότητα ανταλλακτικών: " + spquant.ToString
            End Using
            For i As Integer = 0 To dt3.Columns.Count - 1
                Dim c As DataColumn = dt3.Columns(i)
                With dt3.Columns(i)
                    c.ReadOnly = False
                    If c.ColumnName = "ΣΧ ΠΑΡΑΓΩΓΗΣ" Or c.ColumnName = "ΣΧ ΑΠΟΘΗΚΗΣ" Or c.ColumnName = "ΣΧ ΣΥΣΚΕΥ" Or c.ColumnName = "Αποδέκτης" Then
                        c.AllowDBNull = True
                    End If
                    If c.ColumnName = "ΠΟΣ ΑΠΟΘ" Or (c.ColumnName = "ΣΧ ΠΑΡΑΓΩΓΗΣ" And (l1.Contains(Form1.activeuserdpt))) _
                                   Or (c.ColumnName = "ΣΧ ΑΠΟΘΗΚΗΣ" And (l2.Contains(Form1.activeuserdpt))) _
                                   Or (c.ColumnName = "ΣΧ ΣΥΣΚΕΥ" And (l3.Contains(Form1.activeuserdpt))) Or c.ColumnName = "scr" Then
                        c.ReadOnly = False

                    Else
                        c.ReadOnly = True
                    End If

                End With

            Next
            If Not dt3.Columns.Contains("AA") Then
                dt3.Columns.Add("AA")
            End If
            dt3.Columns("AA").SetOrdinal(0)
            dt3.Columns("ΣΧ ΕΞΑΓΩΓΩΝ").SetOrdinal(2)
            dt3.Columns("ΣΧ ΑΠΟΘΗΚΗΣ").SetOrdinal(3)
            dt3.Columns("ΣΧ ΠΑΡΑΓΩΓΗΣ").SetOrdinal(4)
            dt3.Columns("ΣΧ ΣΥΣΚΕΥ").SetOrdinal(5)
            dt3.Columns("Ποσότητα").SetOrdinal(6)
            dt3.Columns("AA").ReadOnly = False
            AdvancedDataGridView3.DataSource = dt3
            AdvancedDataGridView3.AutoGenerateColumns = False

            With AdvancedDataGridView3

            End With
            With Me.AdvancedDataGridView3
                If .Columns.Count > 0 Then

                    Using aclmn3 As New DataGridViewTextBoxColumn
                        Using cbclmn2 As New DataGridViewComboBoxColumn
                            With cbclmn2
                                .Name = "Αποδέκτης"
                                .HeaderText = "Αποδέκτης"
                                .SortMode = DataGridViewColumnSortMode.Automatic
                            End With

                            With cbclmn2

                                .DataSource = ynTable
                                .DataPropertyName = "scr"
                                .DisplayMember = "name"
                                .ValueMember = "value"
                            End With
                            If Not .Columns.Contains("Αποδέκτης") Then
                                .Columns.Insert(1, cbclmn2)
                            End If
                            For Each C As DataGridViewColumn In .Columns
                                If Not C.Name = "Αποδέκτης" Then
                                    C.ReadOnly = True
                                End If
                            Next
                            If (l1.Contains(Form1.activeuserdpt)) Then
                                .Columns("ΣΧ ΠΑΡΑΓΩΓΗΣ").ReadOnly = False
                                'dt.COLUMNS("ΣΧ ΠΑΡΑΓΩΓΗΣ").ReadOnly = False
                            End If
                            If (l2.Contains(Form1.activeuserdpt)) Then
                                .Columns("ΣΧ ΑΠΟΘΗΚΗΣ").ReadOnly = False
                                'dt.COLUMNS("ΣΧ ΑΠΟΘΗΚΗΣ").ReadOnly = False
                            End If
                            If ((l3.Contains(Form1.activeuserdpt))) Then
                                .Columns("ΣΧ ΣΥΣΚΕΥ").ReadOnly = False
                                ' dt.COLUMNS("ΣΧ ΣΥΣΚΕΥ").ReadOnly = False
                            End If

                            If UserHasRecEditRights Then
                                .Columns("Αποδέκτης").ReadOnly = False
                            End If


                            For j As Integer = 0 To AdvancedDataGridView3.Rows.Count - 1
                                AdvancedDataGridView3.Rows(j).Cells("AA").Value = (j + 1)

                            Next


                            .Columns("id").Visible = False
                            .Columns("scr").Visible = False
                            .Columns("iteid").Visible = False
                            Using boldfont = New Font(DefaultFont, FontStyle.Bold)
                                .Columns("Ποσότητα").DefaultCellStyle.Font = boldfont
                                .Columns("Ποσότητα").DefaultCellStyle.BackColor = Color.LightGray

                                .AllowUserToAddRows = False

                            End Using
                        End Using
                    End Using
                End If
                p3.SetValue(Me.AdvancedDataGridView3, True, Nothing)
                For i As Integer = 0 To .Rows.Count - 1
                    If Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΠΑΡΑΓΩΓΗΣ").Value.ToString) Or
                                Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΑΠΟΘΗΚΗΣ").Value.ToString) Or
                                Not String.IsNullOrWhiteSpace(.Rows(i).Cells("ΣΧ ΣΥΣΚΕΥ").Value.ToString) Or Not IsDBNull(.Rows(i).Cells("scr").Value) Then
                        stlidwithdata.Add(.Rows(i).Cells("id").Value.ToString + "/1")
                    End If

                Next
            End With
            With dt3
                If .Columns.Contains("scr") And (Form1.activeuserocu = 1) And TrackBar1.Value < 6 Then
                    .Columns("scr").ReadOnly = False
                    AdvancedDataGridView1.Columns("Αποδέκτης").ReadOnly = False
                Else
                    .Columns("scr").ReadOnly = True
                    AdvancedDataGridView1.Columns("Αποδέκτης").ReadOnly = True
                End If
            End With
            dt3.Columns("AA").ReadOnly = True
            Dim lst3 As New DataGridViewColumnCollection(AdvancedDataGridView3)
            With AdvancedDataGridView3
                For Each c As DataGridViewColumn In .Columns
                    If c.Visible Then
                        lst3.Add(c.Clone)
                        If c.HeaderText = "ΣΧ ΕΞΑΓΩΓΩΝ" Then
                            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                        Else
                            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        End If
                    End If
                Next
            End With
            AdvancedDataGridViewSearchToolBar3.SetColumns(lst3)

            AdvancedDataGridView3.Columns("ΣΧ ΕΞΑΓΩΓΩΝ").DefaultCellStyle.WrapMode = DataGridViewTriState.True
        Catch
        Finally
            dt3.Dispose()
            AdvancedDataGridView3.Visible = True
            If AdvancedDataGridView3.Rows.Count > 0 Then
                Label10.Visible = True
            End If
            dgv3bwconn.Close()
            dgv3bwconn.Dispose()
        End Try
    End Sub


    Private Sub variousbw_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles variousbw.DoWork
        Dim variousbwconn As New SqlConnection(connString)
        Dim dt4 = New DataTable()
        Try

            Using comm15 As New SqlCommand("select case t.a when 2 then case uoc.subtype when 1 then pu.greekname+' (ΦΕΡΜΟΥΙΤ)' WHEN 0 THEN pu.greekname+' (ΔΙΣΚΟΦΡΕΝΑ)' ELSE PU.GREEKNAME END else pu.greekname end greekname,

uoc.type,uoc.checkdate, uoc.subtype from [PKRTBL_USERORDERCHECK] uoc inner join tbl_packeruserdata pu on pu.id=uoc.userid left join (select userid,count(id) a from [PKRTBL_USERORDERCHECK] where active=1 and ftrid=" + ftrid.ToString + " group by userid) t on t.userid=pu.id where active=1 and ftrid=" + ftrid.ToString, variousbwconn)
                variousbwconn.Open()
                Using reader4 As SqlDataReader = comm15.ExecuteReader
                    dt4.Load(reader4)
                End Using
            End Using

        Catch


        Finally
            variousbwconn.Close()
            variousbwconn.Dispose()
            e.Result = dt4
            dt4.Dispose()
        End Try

    End Sub

    Private Sub variousbw_WorkCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles variousbw.RunWorkerCompleted

        Try
            Using dt4 As DataTable = e.Result
                If dt4.Rows.Count = 0 Then
                    Dim l As New Label
                    l.Text = "Δεν έχει γίνει κανένας έλεγχος ακόμη!"
                    l.AutoSize = True
                    l.Font = f
                    FlowLayoutPanel3.Controls.Add(l)
                Else
                    Using s As New SqlCommand("select isnull(m_chamacheck,0)  from storetrade where ftrid=" + ftrid.ToString, conn)
                        conn.Open()
                        If s.ExecuteScalar = 1 Then
                            Dim o As New OrderApprovalSeal("EXPRESS ORDER", -1, "")
                            FlowLayoutPanel3.Controls.Add(o)
                        End If
                        conn.Close()
                    End Using
                    For Each r As DataRow In dt4.Rows
                        Dim o As New OrderApprovalSeal(r.Item("greekname"), r.Item("type"), r.Item("checkdate").ToString)
                        FlowLayoutPanel3.Controls.Add(o)
                    Next

                End If


                first_load_completed = True
                End Using
        Catch EX As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
        End Try
    End Sub

    Private Sub Order_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If updconn.State = ConnectionState.Open Then
            updconn.Close()
        End If
        If conn.State = ConnectionState.Open Then
            conn.Close()
        End If
        f.Dispose()
        Me.Dispose()
    End Sub

    Private Sub variousbw2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles variousbw2.DoWork
        Dim variousbw2conn As New SqlConnection(connString)
        Try
            Using SQLCOM As New SqlCommand("select top 1 STUFF(( SELECT distinct ', ' +  mnf2.descr          FROM MANUFACTURER mnf2   
inner join material m2 on m2.MNFID=mnf2.codeid 
inner join  storetradelines s2 on s2.iteid=m2.id   where s2.ftrid=b.ftrid    FOR XML PATH ('') )  , 1, 1, '')  AS descr from
     	(select distinct ftrid,mnfid from storetradelines s inner join material m on m.id=s.iteid
        where ftrid=" + ftrid.ToString + " and substring(m.code,1,3) in ('101','104','118','102','202') group by ftrid,m.mnfid,s.iteid ) b ", variousbw2conn)

                variousbw2conn.Open()
                e.Result = "Μάρκα: " + SQLCOM.ExecuteScalar
                variousbw2conn.Close()
            End Using
        Catch
        Finally

            variousbw2conn.Dispose()

        End Try

    End Sub

    Private Sub various2bw_WorkCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles variousbw2.RunWorkerCompleted
        Label26.Text = e.Result
    End Sub

    Private Sub variousbw3_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles variousbw3.DoWork
        Dim variousbw3conn As New SqlConnection(connString)
        Try
            Using q As New SqlCommand("select isnull(printed,'') from tbl_packerordercheck where ftrid=" + ftrid.ToString, variousbw3conn)
                variousbw3conn.Open()
                e.Result = q.ExecuteScalar
                variousbw3conn.Close()
            End Using
        Catch
        Finally

            variousbw3conn.Dispose()

        End Try
    End Sub
    Private Sub various3bw_WorkCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles variousbw3.RunWorkerCompleted
        TextBox6.Text = e.Result
    End Sub

    Private Sub variousbw4_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles variousbw4.DoWork
        Dim variousbw4conn As New SqlConnection(connString)
        Dim DT = New DataTable()
        Try
            Using SQLCOM As New SqlCommand("SELECT DOCID FROM DOCREL WHERE MASTERID=" + ftrid.ToString, variousbw4conn)

                variousbw4conn.Open()

                Using READER As SqlDataReader = SQLCOM.ExecuteReader
                    DT.Load(READER)
                    e.Result = DT

                End Using
                variousbw4conn.Close()

            End Using

        Catch ex As Exception
        Finally
            DT.Dispose()
            variousbw4conn.Dispose()
        End Try
    End Sub
    Private Sub various4bw_WorkCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles variousbw4.RunWorkerCompleted
        Dim DT As DataTable = e.Result
        Try
            If DT.Rows.Count > 0 Then
                Button6.Visible = True
                For i As Integer = 0 To DT.Rows.Count - 1
                    docs.Add(DT.Rows(i).Item(0))
                Next
            End If
        Catch ex As Exception
        Finally
            DT.Dispose()
        End Try
    End Sub

    Private Sub pentousisok_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub AdvancedDataGridViewSearchToolBar1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles AdvancedDataGridViewSearchToolBar1.ItemClicked

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Try
            Dim VALUE As Integer
            VALUE = TrackBar1.Value
            If VALUE = 12 Then VALUE = 13
            If VALUE = 13 Then VALUE = 12
            Dim res = -1
            Using s As New SqlCommand("UPDATE TBL_PACKERORDERCHECK SET STATUS=" + VALUE.ToString + " WHERE FTRID=" + ftrid.ToString, updconn)
                updconn.Open()
                res = s.ExecuteNonQuery()
                updconn.Close()
            End Using
            If res > 0 Then
                If VALUE = 6 Then
                    Using s As New SqlCommand("notifier")
                        s.Connection = updconn
                        updconn.Open()
                        s.CommandType = CommandType.StoredProcedure
                        s.Parameters.AddWithValue("ID", ftrid)
                        s.Parameters.AddWithValue("phase", -3)
                        s.ExecuteNonQuery()
                        updconn.Close()
                    End Using
                End If
                Using ut As New PackerUserTransaction
                        ut.WriteEntry(Form1.activeuserid, 26, ftrid, TrackBar1.Value, orderstatus.Select("id=" + TrackBar1.Value.ToString)(0).Item("name"))
                    End Using
                End If
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

    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        If first_load_completed Then
            TrackBar1.Label = orderstatus.Select("id=" + TrackBar1.Value.ToString)(0).Item("name")
        End If
    End Sub

    Private Sub variousbw5_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles variousbw5.DoWork
        Dim variousbw4conn As New SqlConnection(connString)
        Dim DT = New DataTable()
        Try
            Using SQLCOM As New SqlCommand("select case id when 12 then 13 when 13 then 12 else id end ID,name from TBL_PACKERORDERSTATUS", variousbw4conn)

                variousbw4conn.Open()

                Using READER As SqlDataReader = SQLCOM.ExecuteReader
                    DT.Load(READER)
                    e.Result = DT

                End Using
                variousbw4conn.Close()

            End Using

        Catch ex As Exception
        Finally
            DT.Dispose()
            variousbw4conn.Dispose()
        End Try
    End Sub

    Dim orderstatus As New DataTable()
    Private Sub various5bw_WorkCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles variousbw5.RunWorkerCompleted
        orderstatus = e.Result
    End Sub

    Private Sub TrackBar1_MouseClick(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseClick
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition.X, MousePosition.Y)
        End If
    End Sub

    Private Sub ΔιαγραφήΠαραγγελίαςToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim salesmanid As Integer
        Using s As New SqlCommand("select colidsalesman from customer c inner join fintrade f on f.cusid=c.id where f.id=" + ftrid.ToString, conn)
            conn.Open()
            salesmanid = s.ExecuteScalar
            conn.Close()
        End Using
        If Form1.activeuser = "SUPERVISOR" Or salesmanid = Form1.activeuseraid Then
            Dim palletlist As New List(Of Integer)
            Using transaction = TransactionUtils.CreateTransactionScope(IsolationLevel.Serializable)
                Using s As New SqlCommand("select distinct palletid from tbl_palletlines where ftrid=" + ftrid.ToString, conn)
                    Using dt As New DataTable()
                        conn.Open()
                        Using reader As SqlDataReader = s.ExecuteReader
                            dt.Load(reader)
                            conn.Close()
                        End Using
                        palletlist = dt.AsEnumerable().[Select](Function(r) r.Field(Of Integer)("palletid")).ToList()
                    End Using
                End Using
                Using s As New SqlCommand("delete from tbl_palletlines where palletid in (" + String.Join(",", palletlist.ToArray) + ")", updconn)
                    updconn.Open()
                    s.ExecuteNonQuery()
                    updconn.Close()
                End Using
                Using s As New SqlCommand("delete from tbl_palletheaders where id in (" + String.Join(",", palletlist.ToArray) + ")", updconn)
                    updconn.Open()
                    s.ExecuteNonQuery()
                    updconn.Close()
                End Using
                Using s As New SqlCommand("delete from TBL_PACKERUSERORDERCOMMENTS where ftrid=" + ftrid.ToString, updconn)
                    updconn.Open()
                    s.ExecuteNonQuery()
                    updconn.Close()
                End Using
                Using s As New SqlCommand("delete from TBL_PACKERUSERORDERLINECOMMENTS where stlid in (select id from storetradelines where ftrid=" + ftrid.ToString + ")", updconn)
                    updconn.Open()
                    s.ExecuteNonQuery()
                    updconn.Close()
                End Using
                Using s As New SqlCommand("delete from tbl_packerordlcines where ftrid=" + ftrid.ToString, updconn)
                    updconn.Open()
                    s.ExecuteNonQuery()
                    updconn.Close()
                End Using
                Using s As New SqlCommand("delete from tbl_packerordercheck where ftrid=" + ftrid.ToString, updconn)
                    updconn.Open()
                    s.ExecuteNonQuery()
                    updconn.Close()
                End Using
            End Using
        Else
            Throw New Exception("Μόνο ο υπεύθυνος πωλητής και ο διαχειριστής επιτρέπεται να διαγράψουν παραγγελία!")
        End If
    End Sub
End Class