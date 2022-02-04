Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class PackingList
    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim NotFirstRun As Boolean = False
    Dim InsertDTColumns As String() = {"PALLETID", "STLID", "FTRID", "ITEID", "PALLETID", "PALLETSTATUS", "DPT", "PLORDER", "STATUS", "PLID", "CREATEUSERID", "TRADECODE", "SUBCODE1", "DESCRIPTION", "M_PARTNO", "QUANTITY", "CREATEDATE", "WEIGHT", "NETWEIGHT", "CODE", "LOCCODE", "OPENDATE", "USERNAME", "CLOSEDBYID"}
    Dim InsertDTColumnNames As String() = {"ID", "STLID", "FTRID", "ITEID", "PALLETID", "ΛΕΠΤ ΚΑΤΑΣΤΑΣΗ ΠΑΛΕΤΑΣ", "ΤΜΗΜΑ", "A/A", "STATUS", "PLID", "ΔΗΜ ΑΠΟ", "ΠΑΡ", "ΚΩΔΙΚΟΣ", "ΠΕΡΙΓΡΑΦΗ", "PARTNO/ORGNAL", "ΠΟΣΟΤΗΤΑ", "ΗΜΝΙΑ ΔΗΜ", "WEIGHT", "NETWEIGHT", "ΚΩΔ ΠΑΛ", "ΘΕΣΗ ΑΠΘ", "ΗΜΝΙΑ ΑΝΟΙΓΜ", "ΕΚΛΕΙΣΕ ΑΠΟ", "CLOSEDBYID"}
    Dim InsertDTDatatypes As Type() = {GetType(Integer), GetType(Integer), GetType(Integer), GetType(Integer), GetType(Integer), GetType(String), GetType(String), GetType(Integer), GetType(Integer), GetType(Integer), GetType(Integer), GetType(String), GetType(String), GetType(String), GetType(String), GetType(Double), GetType(Date), GetType(Double), GetType(Double), GetType(String), GetType(String), GetType(String), GetType(String), GetType(Integer)}
    Dim InsertDTColumnVisibility As Boolean() = {False, False, False, False, False, True, True, True, False, False, False, True, True, True, True, True, False, True, True, True, True, True, True, False}
    Dim InsertDTColumnForcedIndex As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer) From {{"SUBCODE1", 6}, {"M_PARTNO", 9}, {"QUANTITY", 10}, {"DESCRIPTION", 8}, {"WEIGHT", 11}, {"NETWEIGHT", 12}, {"PALLETSTATUS", 2}, {"DPT", 3}, {"PLORDER", 4}, {"CODE", 5}, {"TRADECODE", 7}}
    Dim fromexcel As Boolean = False
    Dim PalletStatusPerUserDPT As Integer = -1
    Dim availDPTs As New Dictionary(Of String, Integer)
    Dim availDPTCodes As New Dictionary(Of String, Integer)
    Dim f As New Font("Arial", 10, FontStyle.Bold)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim PalletsPerFTRID As New Dictionary(Of Integer, Integer)
            Dim OKPallets As New List(Of Integer)
            Dim DELPallets As New List(Of Integer)
            For Each r As DataGridViewRow In DataGridView1.Rows 'αν ο αυξων της παλετας εχει υπολογιστει κανει skip, αλλιως υπολογίζει παλέτα για το αντίστοιχο ftrid
                If r.Cells("palletid").Value = 0 And r.Cells("status").Value = -1 Then
                    If Not OKPallets.Contains(r.Cells("plorder").Value) Then
                        OKPallets.Add(r.Cells("plorder").Value)
                    Else
                        Continue For
                    End If
                    If Not PalletsPerFTRID.ContainsKey(r.Cells("ftrid").Value) Then
                        PalletsPerFTRID.Add(r.Cells("ftrid").Value, 1)
                    Else
                        PalletsPerFTRID(r.Cells("ftrid").Value) = PalletsPerFTRID(r.Cells("ftrid").Value) + 1
                    End If
                ElseIf r.Cells("palletid").Value > 0 And r.Cells("status").Value = -1 And r.Cells("Progress").Value = "Προς Διαγραφή" Then
                    If Not DELPallets.Contains(r.Cells("palletid").Value) Then
                        DELPallets.Add(r.Cells("palletid").Value)
                    End If
                End If
            Next
            Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=plcusid)
                If DELPallets.Count > 0 Then pm.Delete(DELPallets) 'σβήνει παλέτες σημασμένες  για διαγραφή
                For Each k As Integer In PalletsPerFTRID.Keys
                    Using newids As DataTable = pm.Create(PalletsPerFTRID(k), status:=PalletStatusPerUserDPT, ftr_id:=k, pl_id:=plid)
                        If IsNothing(newids) Then
                            Throw New Exception("Δεν υπάρχουν ελεύθερες θέσεις αποθήκευσης.")
                        End If
                        For Each r As DataRow In newids.Rows
                            For Each dr As DataGridViewRow In DataGridView1.Rows
                                If dr.Cells("status").Value = PalletStatusPerUserDPT AndAlso newids.Rows.IndexOf(r) + 1 = dr.Cells("plorder").Value Then
                                    pm.AssignCreateDPT(r("ID"), availDPTCodes(dr.Cells("dpt").Value))
                                    pm.AddItem(r("ID"), dr.Cells("iteid").Value, dr.Cells("stlid").Value, dr.Cells("ftrid").Value, dr.Cells("quantity").Value, addorder:=True)
                                End If
                            Next
                        Next
                    End Using
                Next
            End Using
            Button1.Enabled = False
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

        Finally
            updconn.Close()
        End Try

    End Sub

    Dim printed = False


    Private Sub PackingList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            GetType(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, DataGridView1, New Object() {True})
            Using DT As New DataTable()
                Using COM As New SqlCommand
                    COM.Connection = conn
                    COM.CommandText = "SELECT PL.PLCUSID,PL.CODE,pl.plcusid,PL.OPENDATE,PL.CLOSEDATE, PU.USERNAME,cus.code+'-'+isnull(cus.fathername,'???')+'-'+cus.name cusname,isnull(f.tradecode,'Δεν έχει εισαχθεί!') atlantisstatus,PL.STATUS,PL.PRINTDATE,ISNULL(PU2.USERNAME,'-') PRINTUSERNAME
FROM TBL_PACKINGLISTS PL INNER JOIN TBL_PACKERUSERDATA PU ON PU.ID=PL.CREATEUSER inner join customer cus on cus.id=pl.plcusid left join storetrade s on s.sc_plid=pl.id left join fintrade f on f.id=s.ftrid and f.dsrid=9020 left JOIN TBL_PACKERUSERDATA PU2 ON PU2.ID=PL.printuserid
WHERE PL.ID=" + plid.ToString
                    conn.Open()
                    Using reader As SqlDataReader = COM.ExecuteReader
                        DT.Load(reader)
                        conn.Close()
                    End Using
                End Using
                Using s As New SqlCommand("SELECT id,name,code FROM PKRTBL_USERDEPARTMENTS WHERE PKRTBL_ITEMTYPESID IS NOT NULL", conn)
                    conn.Open()
                    Using D As New DataTable()
                        Using READER As SqlDataReader = s.ExecuteReader
                            D.Load(READER)
                        End Using
                        conn.Close()
                        For Each r As DataRow In D.Rows
                            availDPTs.Add(r.Item("name"), r.Item("id"))
                            availDPTCodes.Add(r.Item("code"), r.Item("id"))
                        Next
                    End Using
                End Using
                codelbl.Text = DT.Rows(0).Item("code")
                ToolTip1.SetToolTip(codelbl, plid)
                plcode = codelbl.Text
                cusnamelbl.Text = DT.Rows(0).Item("cusname")
                createdatelbl.Text = DT.Rows(0).Item("opendate")
                Dim printdate = DT.Rows(0).Item("printdate")
                If IsDBNull(printdate) Then
                    printdatelbl.Text = "-"
                Else
                    printdatelbl.Text = printdate
                    printed = True
                End If
                printuserlbl.Text = DT.Rows(0).Item("printusername")
                plcusid = DT.Rows(0).Item("plcusid")
                Dim closedate = DT.Rows(0).Item("closedate")
                If IsDBNull(closedate) Then
                    closedatelbl.Text = "-"
                Else
                    closedatelbl.Text = closedate
                End If
                createuserlbl.Text = DT.Rows(0).Item("username")
                plstatus = DT.Rows(0).Item("status")
                If plstatus = -1 Then
                    statuslbl.Text = "DRAFT - προετοιμασία"
                    statuslbl.ForeColor = Color.Orange
                    ProcessingButton1.state = "draft"
                ElseIf plstatus = 2 Then
                    statuslbl.Text = "Αποθηκευμένο κανονικής μορφής"
                    statuslbl.ForeColor = Color.Blue
                ElseIf plstatus = 1 Then
                    statuslbl.Text = "Ολοκληρωμένο"
                    statuslbl.ForeColor = Color.Green
                    Button1.Visible = False
                    ProcessingButton1.state = "completed"
                End If
                Dim atlantisstatus = DT.Rows(0).Item("atlantisstatus")
                If atlantisstatus = "Δεν έχει εισαχθεί!" Then
                    atlantiscodelbl.Text = atlantisstatus
                    atlantiscodelbl.ForeColor = Color.Red
                    If plstatus = 1 Then
                        InformationPanel1.addwarning(2, "Κλειστό Packing list, σε αναμονή εισαγωγής στο Ατλαντίς")
                    End If
                Else
                    atlantiscodelbl.Text = atlantisstatus
                    atlantiscodelbl.ForeColor = Color.Green
                    InformationPanel1.addwarning(3, "Ολοκληρωμένο Packing list, εισηγμένο στο Ατλαντίς ")
                End If
            End Using
            DataGridView1.Visible = False
            Dim pic As New PictureBox
            pic.Image = My.Resources.rolling
            pic.SizeMode = PictureBoxSizeMode.CenterImage
            TableLayoutPanel1.Controls.Add(pic, 0, 1)
            pic.Dock = DockStyle.Fill
            PalletsWorker.RunWorkerAsync()
            fromexcel = False
            NotFirstRun = True
            LockUIAccess(Me)
            ' AddHandler ProcessingButton1.UC_Button1Click, AddressOf ProcessingButton1_Click
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Return
        End Try
    End Sub


    Dim plcusid As Integer
    Public plstatus As Integer
    Dim plid As Integer
    Dim plcode As String
    Public Sub New(ByVal id As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        plid = id
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub lockdown()
        Dim alloweddpts As New List(Of String) From {"SA", "EX"}
        If Not alloweddpts.Contains(Form1.activeuserdpt) OrElse printed Then
            DataGridView1.ReadOnly = True
            Button3.Enabled = False
            Button1.Enabled = False
            ContextMenuStrip1.Enabled = False
        End If

        If Not alloweddpts.Contains(Form1.activeuserdpt) Then
            ProcessingButton1.Enabled = False
        End If
    End Sub

    Private Sub PalletsWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles PalletsWorker.DoWork
        Dim dt As New DataTable()
        Try
            Using com As New SqlCommand
                com.Connection = conn
                com.CommandText = "select pl.stlid,pl.ftrid,pl.iteid,ph.id palletid,dbo.pkrfn_getpalletstatus(ph.id) palletstatus,pudpt.name dpt,ph.plorder,ph.code,ph.loccode,ph.opendate,pu.username,ph.weight,ph.NETWEIGHT,isnull(ph.status,0) status,ph.closedbyid,
ph.width,ph.length,ph.height,dbo.get_tradecode(pl.ftrid) tradecode,m.subcode1,m.DESCRIPTION,m.m_partno,pl.quantity from TBL_PALLETHEADERS ph inner join TBL_PALLETLINES pl on pl.PALLETID=ph.id left join tbl_packeruserdata pu on pu.id=ph.CLOSEDBYID
inner join material m on m.id=pl.iteid inner join pkrtbl_userdepartments pudpt on pudpt.id=ph.createdptid where plid=" + plid.ToString + " or (isnull(ph.status,0)>=-2 and ph.cusid=" + plcusid.ToString + " and ph.plid is null) order by 4,3"
                conn.Open()
                Dim reader As SqlDataReader = com.ExecuteReader
                dt.Load(reader)
                conn.Close()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            e.Result = dt
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub PalletsWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles PalletsWorker.RunWorkerCompleted
        For Each c As Control In TableLayoutPanel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
        DataGridView1.DataSource = e.Result
        DataGridView1.Visible = True
    End Sub

    Private Sub PalletLinesWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Dim dt As New DataTable()
        Try

            Using com As New SqlCommand
                com.Connection = conn
                com.CommandText = "SELECT pl.iteid,pl.stlid,pl.ftrid,dbo.pkrfn_get_palletstatus palletstatus, dbo.get_tradecode(pl.ftrid) tradecode,m.subcode1,m.description,pl.quantity FROM TBL_PALLETLINES PL inner join material m on m.id=pl.iteid where PALLETID="
                conn.Open()
                Dim reader As SqlDataReader = com.ExecuteReader
                dt.Load(reader)
                conn.Close()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            e.Result = dt
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        With DataGridView1
            If DataGridView1.RowCount > 0 And Not DataGridView1.Columns.Contains("Progress") Then
                Dim col As New DataGridViewProgressColumn
                col.Name = "Progress"
                col.HeaderText = "Πρόοδος"
                DataGridView1.Columns.Insert(0, col)
            End If
            If DataGridView1.RowCount > 0 And Not DataGridView1.Columns.Contains("Lock") Then
                Dim col As New DataGridViewImageColumn
                col.Name = "Lock"
                col.HeaderText = "Προστατευμένη"
                col.DefaultCellStyle.NullValue = Nothing
                DataGridView1.Columns.Insert(1, col)
            End If
            Dim exlines As New List(Of Integer) From {-2, -1}
            For r = 0 To .Rows.Count - 1
                If plstatus = 1 And exlines.Contains(.Rows(r).Cells("status").Value) Then
                    .Rows(r).Visible = False
                    Continue For
                End If
                If .Rows(r).Cells("status").Value = -2 Then
                    InformationPanel1.addwarning(1, "Υπάρχουν παλέτες σχεδιασμένες από τη παραγωγή! Τα δεδομένα που θα επικολλήσετε πρέπει να τις λαμβάνουν υπόψη!")
                    .Rows(r).Cells("Progress").Value = "10,Προγραμματισμένη"
                    .Rows(r).Cells("Lock").Value = My.Resources.icons8_redlight_16
                    For Each cell As DataGridViewCell In .Rows(r).Cells
                        If Not cell.OwningColumn.Name = "plorder" Then
                            cell.ReadOnly = True
                        End If
                        cell.ToolTipText = "Παλέτα προγραμματισμένη από τη παραγωγή!"
                    Next
                ElseIf .Rows(r).Cells("status").Value = 0 Then
                    InformationPanel1.addwarning(1, "Υπάρχουν υλοποιημένες παλέτες! Τα δεδομένα που θα επικολλήσετε πρέπει να τις λαμβάνουν υπόψη!")
                    .Rows(r).Cells("Progress").Value = "50,Γεμίζει"
                    .Rows(r).Cells("Lock").Value = My.Resources.icons8_redlight_16
                    For Each cell As DataGridViewCell In .Rows(r).Cells
                        If Not cell.OwningColumn.Name = "plorder" Then
                            cell.ReadOnly = True
                        End If
                        cell.ToolTipText = "Φυσική παλέτα σε φάση συμπλήρωσης!"
                    Next
                ElseIf .Rows(r).Cells("status").Value = 1 Then
                    InformationPanel1.addwarning(1, "Υπάρχουν υλοποιημένες παλέτες! Τα δεδομένα που θα επικολλήσετε πρέπει να τις λαμβάνουν υπόψη!")
                    .Rows(r).Cells("Progress").Value = "100,Ολοκληρωμένη"
                    .Rows(r).Cells("Lock").Value = My.Resources.icons8_redlight_16
                    For Each cell As DataGridViewCell In .Rows(r).Cells
                        If Not cell.OwningColumn.Name = "plorder" Then
                            cell.ReadOnly = True
                        End If
                        cell.ToolTipText = "Ολοκληρωμένη παλέτα!"
                    Next
                ElseIf .Rows(r).Cells("status").Value = -1 Then
                    .Rows(r).Cells("Progress").Value = "10,Προγραμματισμένη"
                    For Each cell As DataGridViewCell In .Rows(r).Cells
                        If Not cell.OwningColumn.Name = "plorder" Then
                            cell.ReadOnly = True
                        End If
                        cell.ToolTipText = "Παλέτα προγραμματισμένη από εξαγωγές"
                    Next
                End If
            Next
            For r = 1 To .Rows.Count - 1
                If .Rows(r).Cells("palletid").Value <> .Rows(r - 1).Cells("palletid").Value Then
                    If .Rows(r - 1).DefaultCellStyle.BackColor = SystemColors.Window Or .Rows(r - 1).DefaultCellStyle.BackColor = Color.Empty Then
                        .Rows(r).DefaultCellStyle.BackColor = Color.WhiteSmoke
                    Else
                        .Rows(r).DefaultCellStyle.BackColor = SystemColors.Window
                    End If
                Else
                    .Rows(r).DefaultCellStyle.BackColor = .Rows(r - 1).DefaultCellStyle.BackColor
                End If
                .Rows(r).Cells("plorder").Style.BackColor = Color.Yellow
                .Rows(r).Cells("quantity").Style.BackColor = Color.LightGray
            Next

            For Each c As DataGridViewColumn In DataGridView1.Columns
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader
                Dim colw As Integer = c.Width
                If c.Name.ToUpper = "PLORDER" Or c.Name.ToUpper = "QUANTITY" Then
                    colw = 25
                End If
                If c.Name.ToUpper = "PROGRESS" Then
                    colw = 110
                End If
                If c.Name.ToUpper = "CODE" Or c.Name.ToUpper = "SUBCODE1" Then
                    colw = 130
                End If
                If c.Name <> "plorder" Then
                    c.ReadOnly = True
                End If
                If InsertDTColumns.Contains(c.Name.ToUpper) Then
                    c.Visible = InsertDTColumnVisibility(Array.IndexOf(InsertDTColumns, c.Name.ToUpper))
                End If
                If InsertDTColumnForcedIndex.ContainsKey(c.Name.ToUpper) Then
                    c.DisplayIndex = InsertDTColumnForcedIndex(c.Name.ToUpper)
                End If
                If InsertDTColumns.Contains(c.Name.ToUpper) Then
                    c.HeaderText = InsertDTColumnNames(Array.IndexOf(InsertDTColumns, c.Name.ToUpper))
                End If
                If c.Visible Then
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    c.Width = colw
                End If
            Next
            TableLayoutPanel1.RowStyles(1).Height = InformationPanel1.GetRequiredHeight
        End With
        lockdown()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        f.Dispose()
        Me.Dispose()
    End Sub

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If e.RowIndex > -1 And DataGridView1.Columns(e.ColumnIndex).Name = "plorder" Then
            If NotFirstRun And Not fromexcel Then
                Dim newid = DataGridView1.Rows(e.RowIndex).Cells("palletid").Value
                Dim neworder = DataGridView1.Rows(e.RowIndex).Cells("plorder").Value
                Dim oldid As Integer = 0
                For Each r As DataGridViewRow In DataGridView1.Rows
                    If IsDBNull(neworder) Then
                        neworder = DBNull.Value
                    ElseIf Not IsDBNull(r.Cells("plorder").Value) AndAlso (r.Cells("plorder").Value = neworder And r.Cells("palletid").Value <> newid) Then
                        oldid = r.Cells("palletid").Value
                    End If
                Next
                Dim skip As Boolean = False
                If oldid = 0 Then
                    skip = True
                End If
                Using c As New SqlCommand("UPDATE TBL_PALLETHEADERS SET PLORDER=(SELECT PLORDER FROM TBL_PALLETHEADERS WHERE ID=" + newid.ToString + ") WHERE ID=" + oldid.ToString + " ")
                    c.Connection = updconn
                    Dim result As Integer = 0
                    If Not skip Then
                        updconn.Open()
                        result = c.ExecuteNonQuery
                        updconn.Close()
                    End If
                    If skip Or result > 0 Then
                        Dim extra As String = ""
                        If Not IsDBNull(neworder) Then
                            extra = ",plid=" + plid.ToString
                        Else
                            extra = ",plid=null"
                        End If
                        Using c2 As New SqlCommand("UPDATE TBL_PALLETHEADERS SET PLORDER=@o " + extra + " WHERE ID=" + newid.ToString)
                            c2.Connection = updconn
                            c2.Parameters.AddWithValue("@o", neworder)
                            updconn.Open()
                            Dim result2 = c2.ExecuteNonQuery
                            updconn.Close()
                            If result2 > 0 And Not multiple Then
                                DataGridView1.Hide()
                                Dim pic As New PictureBox
                                pic.Image = My.Resources.rolling
                                pic.SizeMode = PictureBoxSizeMode.CenterImage
                                TableLayoutPanel1.Controls.Add(pic, 0, 1)
                                pic.Dock = DockStyle.Fill
                                PalletsWorker.RunWorkerAsync()
                            End If
                        End Using
                    End If
                End Using
                'ElseIf NotFirstRun Then
                '    For Each r As DataGridViewRow In DataGridView1.Rows
                '        If r.Index = e.RowIndex Then
                '            Continue For
                '        End If
                '        If r.Cells("plorder").Value = previousvalue Then
                '            r.Cells("plorder").Value = DataGridView1.Rows(e.RowIndex).Cells("plorder").Value
                '        ElseIf r.Cells("plorder").Value = DataGridView1.Rows(e.RowIndex).Cells("plorder").Value Then
                '            r.Cells("plorder").Value = previousvalue
                '        End If
                '    Next
            End If

        End If
    End Sub


    Dim customers As DataTable = New DataTable()
    Private Function read_excel(ByVal s As String)
        Dim InsertDT As New DataTable(0)
        Dim palletnums As New List(Of Integer)
        Try
            Dim rowData As String() = s.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            Dim dt As New System.Data.DataTable()
            Dim FirstLine As Boolean = True
            Dim ColumnNames As String() = {"tradecode", "plorder", "dpt", "itecode", "quantity", "weight", "netweight"}
            For Each r As String In rowData
                Dim columnData As String() = r.Split(New String() {ControlChars.Tab}, StringSplitOptions.None)
                If Not (columnData.Length = 7) Then
                    Throw New Exception("Οι στήλες πρέπει να είναι 7 (Παρ,Αρ Παλέτας,Τμήμα (BP,BL,SP),Είδος,Ποσότητα,Βάρος,Καθαρό Βάρος)")
                End If
                If FirstLine Then
                    For i As Int32 = 0 To columnData.Length - 1
                        dt.Columns.Add(ColumnNames(i))
                    Next
                    FirstLine = False
                End If
                Dim newRow As DataRow = dt.Rows.Add() ' already added now '
                For col As Int32 = 0 To columnData.Length - 1
                    newRow.SetField(col, columnData(col))
                Next
            Next
            Dim distinctOrders = dt.DefaultView.ToTable(True, "tradecode")
            Dim disordtext As New List(Of String)
            Dim disordcounter As Integer = 0
            For Each r As DataRow In distinctOrders.Rows
                disordtext.Add("@o" + disordcounter.ToString)
                disordcounter += 1
            Next
            Dim OrdersString As String = String.Join(",", disordtext)
            Dim tempdt As New DataTable()
            Using c As SqlCommand = New SqlCommand("select stlid,ftrid,iteid,dbo.get_tradecode(ftrid) tradecode,m.code,m.subcode1,
case left(m.subcode1,1) when '1' then m.m_wva1 when '2' then m.factorycode end m_partno
,m.description,I.CUSID perid,I.SCHREMAINDER primaryqty from Z_PACKER_ITEMSBROWSER i inner join material m on m.id=i.iteid where ftrid in (select id from fintrade where dsrid=9000 and dbo.get_tradecode(id) in (" + OrdersString + "))", conn)
                disordcounter = 0
                For Each r As DataRow In distinctOrders.Rows
                    c.Parameters.Add("@o" + disordcounter.ToString, SqlDbType.NVarChar).Value = String.Join(",", r(0))
                    disordcounter += 1
                Next
                conn.Open()
                Using reader As SqlDataReader = c.ExecuteReader
                    tempdt.Load(reader)
                End Using
                conn.Close()
                Dim OrdersCustomers = tempdt.DefaultView.ToTable(True, "perid")
                If OrdersCustomers.Rows.Count > 1 Then
                    Throw New Exception("Δεν είναι δυνατό μία αποστολή να έχει είδη απο παραγγελίες διαφορετικών πελατών.")
                ElseIf OrdersCustomers.Rows.Count = 0 Then
                    Throw New Exception("Δεν βρέθηκαν εκκρεμείς παραγγελίες που να συμφωνούν με τα δεδομένα που επικολλήσατε.")
                End If
                If OrdersCustomers.Rows(0).Item("perid") <> plcusid Then
                    Throw New Exception("Ο πελάτης που επιλέξατε προηγουμένως δεν είναι ίδιος με τον πελάτη των παραγγελιών που επιχειρείτε να καταχωρήσετε.")
                End If
            End Using


            Dim tempcounter As Integer = 0
            For Each st As String In InsertDTColumns
                If Not InsertDT.Columns.Contains(st) Then
                    InsertDT.Columns.Add(st, InsertDTDatatypes(tempcounter))
                End If
                tempcounter += 1

            Next
            For Each r As DataRow In dt.Rows
                Dim temprow As DataRow() = tempdt.Select("(code='" & r("itecode") & "' or subcode1='" & r("itecode") & "') and tradecode='" & r("tradecode") & "'")
                If Not IsNumeric(r("plorder")) Or Not IsNumeric(r("quantity")) Or Not IsNumeric(r("weight")) Or Not IsNumeric(r("netweight")) Then
                    Throw New Exception("Άκυρη μορφή δεδομένων. Γραμμή " + (dt.Rows.IndexOf(r) + 1).ToString + " του αρχείου εισαγωγής.")
                End If
                If temprow.Length = 0 Then
                    Throw New Exception("Το είδος " + r("itecode").ToString + " δεν βρέθηκε στη παραγγελία " + r("tradecode") + ". Γραμμή " + (dt.Rows.IndexOf(r) + 1).ToString + " του αρχείου εισαγωγής.")
                End If
                If CDbl(r("quantity")) < 0 Then
                    Throw New Exception("Η ποσότητα για το είδος " + r("itecode").ToString + " στη παραγγελία " + r("tradecode") + " είναι αρνητική. Γραμμή " + (dt.Rows.IndexOf(r) + 1).ToString + " του αρχείου εισαγωγής.")
                End If
                If CDbl(r("quantity")) = 0 Then
                    Throw New Exception("Η ποσότητα για το είδος " + r("itecode").ToString + " στη παραγγελία " + r("tradecode") + " είναι μηδενική. Γραμμή " + (dt.Rows.IndexOf(r) + 1).ToString + " του αρχείου εισαγωγής.")
                End If
                If CDbl(temprow(0).Item("primaryqty")) < CDbl(r("quantity")) Then
                    Throw New Exception("Η ποσότητα για το είδος " + r("itecode").ToString + " στη παραγγελία " + r("tradecode") + " είναι μεγαλύτερη του αναμενόμενου. Γραμμή " + (dt.Rows.IndexOf(r) + 1).ToString + " του αρχείου εισαγωγής.")
                End If
                If Not availDPTCodes.ContainsKey(r("dpt").ToString.ToUpper) Then
                    Throw New Exception("Άγνωστο τμήμα στη γραμμή " + (dt.Rows.IndexOf(r) + 1).ToString + ". Πιθανές επιλογές: BL, BP, SP.")
                End If
                Dim InsertRow = InsertDT.Rows.Add()
                InsertRow("CREATEUSERID") = Form1.activeuserid
                'InsertRow("PLCODE") = plcode
                InsertRow("SUBCODE1") = temprow(0).Item("code")
                InsertRow("M_PARTNO") = temprow(0).Item("m_partno")
                InsertRow("DESCRIPTION") = temprow(0).Item("description")
                InsertRow("STLID") = CInt(temprow(0).Item("stlid"))
                InsertRow("ITEID") = CInt(temprow(0).Item("iteid"))
                InsertRow("FTRID") = CInt(temprow(0).Item("ftrid"))
                InsertRow("QUANTITY") = CDbl(r("quantity"))
                InsertRow("plorder") = CInt(r("plorder"))
                InsertRow("WEIGHT") = CInt(r("weight"))
                InsertRow("NETWEIGHT") = CInt(r("netweight"))
                InsertRow("dpt") = (r("dpt"))
                InsertRow("TRADECODE") = temprow(0).Item("tradecode")
                InsertRow("CREATEDATE") = Now()
                InsertRow("palletid") = 0
                InsertRow("STATUS") = -1
                InsertRow("PLID") = plid
                palletnums.Add(CInt(r("plorder")))
            Next
            Return InsertDT
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            Return Nothing
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim dgvds As DataTable = DataGridView1.DataSource
        For i = dgvds.Rows.Count - 1 To 0 Step -1
            If dgvds.Rows(i).Item("status") = -1 Then
                dgvds.Rows.RemoveAt(i)
            End If
        Next
        fromexcel = True
        Dim s As String
        s = Clipboard.GetText
        Dim InsertDT As DataTable = read_excel(s)
        If IsNothing(InsertDT) Then
            Return
        End If
        DataGridView1.ReadOnly = False
        For Each r As DataRow In InsertDT.Rows
            Dim nr = dgvds.NewRow()
            nr("stlid") = r("stlid")
            nr("iteid") = r("iteid")
            nr("ftrid") = r("ftrid")
            nr("palletid") = 0
            nr("plorder") = r("plorder")
            nr("dpt") = r("dpt")
            nr("code") = ""
            nr("loccode") = ""
            nr("username") = Form1.activeuser
            nr("weight") = r("weight")
            nr("netweight") = r("netweight")
            nr("status") = -1
            nr("closedbyid") = 0
            nr("tradecode") = r("tradecode")
            nr("subcode1") = r("subcode1")
            nr("description") = r("description")
            nr("m_partno") = r("m_partno")
            nr("quantity") = r("quantity")
            nr("opendate") = r("createdate")
            dgvds.Rows.Add(nr)
        Next
        DataGridView1.DataSource = dgvds
        If DataGridView1.RowCount > 0 And Not printed Then
            DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells("plorder")
            DataGridView1.Rows(0).Selected = True
            Button1.Enabled = True
        End If
        Dim IDTCcounter As Integer = 0
        'For Each c As DataGridViewColumn In DataGridView1.Columns
        '    c.Visible = InsertDTColumnVisibility(IDTCcounter)
        '    c.HeaderText = InsertDTColumnNames(IDTCcounter)
        '    IDTCcounter += 1
        'Next
        'DataGridView1.ReadOnly = True
        'Button5.Enabled = True
        'Button2.Enabled = True
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        If DataGridView1.RowCount > 0 Then

            If e.Button = MouseButtons.Right Then
                ContextMenuStrip1.Show(MousePosition.X, MousePosition.Y)

            End If
        End If
    End Sub

    Private Sub ΣυμπλήρωσεΜεΤηΤιμήΤουΠρώτουΕπιλεγμένουΚελιούToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim IndexCollection As New List(Of Integer)
        IndexCollection.Add(DataGridView1.SelectedCells.Item(0).RowIndex)
        For i = 1 To DataGridView1.SelectedCells.Count - 1
            If DataGridView1.SelectedCells.Item(i).RowIndex < DataGridView1.SelectedCells.Item(i - 1).RowIndex Then
                IndexCollection.Insert(0, DataGridView1.SelectedCells.Item(i).RowIndex)
            Else
                IndexCollection.Add(DataGridView1.SelectedCells.Item(i).RowIndex)
            End If
        Next
        For i = 1 To IndexCollection.Count
            DataGridView1.Rows(IndexCollection(i)).Cells("plorder").Value = DataGridView1.Rows(IndexCollection(0)).Cells("plorder").Value
        Next
    End Sub


    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        Try
            If DataGridView1.Columns(e.ColumnIndex).Name = "plorder" Then
                Throw New Exception("Επιτρέπονται μόνο αριθμοί στη στήλη αρίθμησης παλέτας")
            Else
                Throw New Exception("Λανθασμένη μορφή δεδομένων")
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

    Private Sub PackingList_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        TableLayoutPanel1.RowStyles(1).Height = InformationPanel1.GetRequiredHeight
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using f As New PrintPackingList(plid)
            f.Owner = Me
            If DialogResult.OK = f.ShowDialog() And f.printed Then
                Dim r As Integer = -1
                Using s As New SqlCommand("update tbl_packinglists set PRINTDATE='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',PRINTUSERID=" + Form1.activeuserid.ToString + " WHERE ID=" + plid.ToString + " and printdate is null", updconn)
                    updconn.Open()
                    r = s.ExecuteScalar()
                    updconn.Close()
                End Using
                If r > 0 Then
                    Using ut As New PackerUserTransaction
                        ut.WriteEntry(Form1.activeuserid, 35, plid)
                    End Using
                End If
            End If
        End Using
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_KeyUp(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyUp
        If e.KeyCode = Keys.Delete Then
            Dim rows As New List(Of Integer)
            For Each c As DataGridViewCell In DataGridView1.SelectedCells
                If Not rows.Contains(DataGridView1.Rows(c.RowIndex).Cells("palletid").Value) Then
                    rows.Add(DataGridView1.Rows(c.RowIndex).Cells("palletid").Value)
                End If
            Next
            For a As Integer = 0 To DataGridView1.Rows.Count - 1
                If rows.Contains(DataGridView1.Rows(a).Cells("palletid").Value) Then
                    If DataGridView1.Rows(a).Cells("status").Value = -1 Then
                        If DataGridView1.Rows(a).Cells("Progress").Value = "Προς Εισαγωγή" Then
                            DataGridView1.Rows.RemoveAt(a)
                        Else
                            DataGridView1.Rows(a).DefaultCellStyle.BackColor = Color.Red
                            DataGridView1.Rows(a).Cells("Progress").Value = "Προς Διαγραφή"
                        End If
                    Else
                        rows.Remove(a)
                    End If
                End If
            Next
            If rows.Count > 0 And Not printed Then
                Button1.Enabled = True
            End If
        End If
    End Sub

    Private Sub ProcessingButton1_UC_Button1Click() Handles ProcessingButton1.UC_Button1Click
        PLProcessWorker.RunWorkerAsync()
    End Sub
    Private Sub PLProcessWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles PLProcessWorker.DoWork
        Try
            If plstatus = -1 Then
                Using s As New SqlCommand("SELECT ph.plid,count(ph.ID) a,count(ph2.id) b FROM TBL_PALLETHEADERS PH left join TBL_PALLETHEADERS ph2 on ph2.id=ph.ID and ph2.status<0 WHERE ph.PLID=" + plid.ToString + " group by ph.plid", conn)
                    Using dt As New DataTable()
                        conn.Open()
                        Using reader As SqlDataReader = s.ExecuteReader
                            dt.Load(reader)
                        End Using
                        conn.Close()
                        If dt.Rows.Count = 0 Then
                            Throw New Exception("Δεν υπάρχουν παλέτες στο packing list!")
                        ElseIf dt(0).Item("a") > 0 And dt(0).Item("b") = 0 Then
                            Using s2 As New SqlCommand("UPDATE TBL_PACKINGLISTS SET STATUS=2 WHERE ID=" + plid.ToString, conn)
                                conn.Open()
                                e.Result = s2.ExecuteNonQuery()
                                conn.Close()
                            End Using
                            If e.Result > 0 Then
                                e.Result = 0

                            End If
                        ElseIf dt(0).Item("a") = 0 Then
                            Throw New Exception("Δεν υπάρχουν παλέτες στο packing list!")
                        ElseIf dt(0).Item("b") <> 0 Then
                            Throw New Exception("Υπάρχουν παλέτες σε φάση σχεδιασμού ακόμη. Για να μην είναι DRAFT ένα Packing List, πρέπει να είναι υλοποιημένες όλες οι παλέτες του!")
                        End If
                    End Using
                End Using
            ElseIf plstatus = 0 Or plstatus = 2 Then
                Using s As New SqlCommand("SELECT ph.plid,count(ph.ID) a,count(ph2.id) b FROM TBL_PALLETHEADERS PH left join TBL_PALLETHEADERS ph2 on ph2.id=ph.ID and ph2.status<1 WHERE ph.PLID=" + plid.ToString + " group by ph.plid", conn)
                    Using dt As New DataTable()
                        conn.Open()
                        Using reader As SqlDataReader = s.ExecuteReader
                            dt.Load(reader)
                        End Using
                        conn.Close()
                        If dt.Rows.Count = 0 Then
                            Throw New Exception("Δεν υπάρχουν παλέτες στο packing list!")
                        ElseIf dt(0).Item("a") > 0 And dt(0).Item("b") = 0 Then
                            Using s2 As New SqlCommand("UPDATE TBL_PACKINGLISTS SET STATUS=1, closedate=getdate() WHERE ID=" + plid.ToString, conn)
                                conn.Open()
                                e.Result = s2.ExecuteNonQuery()
                                conn.Close()
                            End Using
                            If e.Result > 0 Then
                                e.Result = 1

                            End If
                        ElseIf dt(0).Item("a") = 0 Then
                            Throw New Exception("Δεν υπάρχουν παλέτες στο packing list!")
                        ElseIf dt(0).Item("b") <> 0 Then
                            Throw New Exception("Υπάρχουν ανοιχτές παλέτες. Για να κλείσει ένα Packing List, πρέπει να είναι κλειστές όλες οι παλέτες του!")
                        End If
                    End Using
                End Using
            ElseIf plstatus = 1 Then
                Using s As New SqlCommand("select pl.id,isnull(f.tradecode,'Δεν έχει εισαχθεί!') atlantisstatus from tbl_packinglists pl left join storetrade s on s.sc_plid=pl.id left join fintrade f on f.id=s.ftrid and f.dsrid=9020 WHERE pl.ID=" + plid.ToString, conn)
                    Using dt As New DataTable()
                        conn.Open()
                        Using reader As SqlDataReader = s.ExecuteReader
                            dt.Load(reader)
                        End Using
                        conn.Close()
                        If dt(0).Item("atlantisstatus") = "Δεν έχει εισαχθεί!" Then
                            Using s2 As New SqlCommand("UPDATE TBL_PACKINGLISTS SET STATUS=2 WHERE ID=" + plid.ToString, conn)
                                conn.Open()
                                e.Result = s2.ExecuteNonQuery()
                                conn.Close()
                            End Using
                            If e.Result > 0 Then
                                e.Result = 2

                            End If
                        Else
                            Throw New Exception("Το Packing List έχει εισαχθεί στο Ατλαντίς στο παραστατικό " + dt(0).Item("atlantisstatus") + ". Δεν επιτρέπεται το άνοιγμα του.")
                        End If
                    End Using
                End Using
            End If
        Catch ex As Exception
            e.Result = -1

            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub PLProcessWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles PLProcessWorker.RunWorkerCompleted
        If e.Result = -1 Then
            ProcessingButton1.complete(0)
        ElseIf e.Result = 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(Form1.activeuserid, 19, plid)
            End Using
            ProcessingButton1.complete(1)
        ElseIf e.Result = 1 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(Form1.activeuserid, 20, plid)
            End Using
            ProcessingButton1.complete(1)
        ElseIf e.Result = 2 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(Form1.activeuserid, 19, plid)
            End Using
            ProcessingButton1.complete(1)
        End If
    End Sub

    Dim selection As New List(Of Integer)
    Dim multiple As Boolean = False

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        selection.Clear()
        With DataGridView1
            For Each c As DataGridViewCell In .SelectedCells
                If Not selection.Contains(.Rows(c.RowIndex).Cells("palletid").Value) Then
                    selection.Add(.Rows(c.RowIndex).Cells("palletid").Value)
                End If
            Next
        End With
    End Sub

    Private Sub ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem.Click
        Dim counter As Integer
        Using s As New SqlCommand("select isnull(max(plorder),0)+1 from tbl_palletheaders where plid=" + plid.ToString, conn)
            conn.Open()
            counter = s.ExecuteScalar
            conn.Close()
        End Using
        If selection.Count > 1 Then
            multiple = True
        End If
        Dim found As Boolean = False
        For i As Integer = selection.Count - 1 To 0 Step -1
            For Each r As DataGridViewRow In DataGridView1.Rows
                If r.Cells("palletid").Value = selection(i) Then
                    r.Cells("plorder").Value = counter
                    counter += 1
                    If selection.Count - 1 = selection.Count - DataGridView1.SelectedCells.Count Then
                        multiple = False
                    End If
                    found = True
                    Exit For
                End If
            Next
            If found Then
                found = False
                Continue For
            End If
        Next
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        If selection.Count = 0 Then
            ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem.Enabled = False
        Else
            ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub ΕκκαθάρισηΑρίθμησηςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΕκκαθάρισηΑρίθμησηςToolStripMenuItem.Click
        Try
            Using s As New SqlCommand("update tbl_palletheaders set plorder=null,plid=null where plid=(select id from tbl_packinglists where status<>1 and printuserid is null and id=" + plid.ToString + ")", updconn)
                updconn.Open()
                Dim result = s.ExecuteNonQuery()
                updconn.Close()
                If result > 0 Then
                    printed = True
                    Using ut As New PackerUserTransaction
                        ut.WriteEntry(Form1.activeuserid, 19, plid)
                    End Using
                    lockdown()
                End If
            End Using
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            PalletsWorker.RunWorkerAsync()
        End Try
    End Sub

    Private Sub CustomDGVSearchBox1_ButtonPressed() Handles CustomDGVSearchBox1.ButtonPressed
        CustomDGVSearchBox1.continue_click()
    End Sub

    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting

    End Sub

    Private Sub DataGridView1_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles DataGridView1.RowPrePaint

    End Sub

    Private Sub DataGridView1_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        'Χρωματισμος κελιων γινεται και στη διαδικασια DataGridView1_DataBindingComplete
        If e.ColumnIndex > -1 Then

            With DataGridView1.Columns(e.ColumnIndex)
                If .Name.ToUpper = "PLORDER" Then
                    .DefaultCellStyle.BackColor = Color.Yellow
                    .DefaultCellStyle.Font = f
                End If
                If .Name.ToUpper = "QUANTITY" Then
                    .DefaultCellStyle.BackColor = Color.LightGray
                    .DefaultCellStyle.Font = f
                End If
                If .Name.ToUpper = "CODE" Or .Name.ToUpper = "SUBCODE1" Then
                    .DefaultCellStyle.Font = f
                End If
            End With
        End If
    End Sub

    Private Sub DataGridView1_FilterStringChanged(sender As Object, e As EventArgs) Handles DataGridView1.FilterStringChanged
        Try
            TryCast(DataGridView1.DataSource, DataTable).DefaultView.RowFilter = DataGridView1.FilterString
        Catch
        End Try
    End Sub

    Private Sub DataGridView1_SortStringChanged(sender As Object, e As EventArgs) Handles DataGridView1.SortStringChanged
        Try
            TryCast(DataGridView1.DataSource, DataTable).DefaultView.Sort = DataGridView1.SortString
        Catch
        End Try
    End Sub
End Class