
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Globalization


'line statuses
'-1 - row will be deleted
' 0 - database row without changes 
' 1 - inserted, new row
' 2 - database row with changes from user


Public Class production
    Implements IMessageFilter
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim dpconn1 As New SqlConnection(connString)
    Dim dpconn2 As New SqlConnection(connString)
    Dim stconn As New SqlConnection(connString)
    Dim nwconn As New SqlConnection(connString)
    Dim nw2conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim p = GetType(Zuby.ADGV.AdvancedDataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim ypos As Integer = 0
    Dim ValidCodes = New DataTable()
    Dim dpdgv2FirstVisibleColumnIndex As Integer = 0
    Public FirstMonday As Date
    Dim NotFirstLoad As Boolean = False
    Public weekOfYear As Integer

    Private Sub production_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'TESTFINALDataSet.MATERIAL' table. You can move, or remove it, as needed.
        For Each c As DataGridViewColumn In dpdgv2.Columns
            If c.Visible = True Then
                dpdgv2FirstVisibleColumnIndex = c.Index
                Exit For
            End If
        Next
        p.SetValue(Me.DataGridView1, True, Nothing)
        p.SetValue(Me.dpdgv1, True, Nothing)
        p.SetValue(Me.dpdgv2, True, Nothing)
        p.setvalue(Me.stockdgv, True, Nothing)
        Label1.Text = "Αυτόματη ανανέωση σε " + timerlimit.ToString + " δευτερόλεπτα"
        'Using command As SqlCommand = New SqlCommand("select code,subcode1 from material where isactive=1", conn)
        '    conn.Open()
        '    Using reader As SqlDataReader = command.ExecuteReader
        '        ValidCodes.load(reader)
        '    End Using
        '    conn.Close()
        'End Using
        If Today.DayOfWeek = DayOfWeek.Sunday Then Today = Today.AddDays(-1)
        Dim monDate As DateTime = today.AddDays(DayOfWeek.Monday - today.DayOfWeek)
        FirstMonday = monDate
        Textbox1.Text = monDate.ToShortDateString.ToString + "-" + monDate.AddDays(6).ToShortDateString.ToString
        Label4.Text = Label4.Text + " " + Date.Today.ToShortDateString
        Dim cmd As String = "select cast(codeid as varchar(5))+'-'+descr from FLDCUSTBL1"
        Dim comm As New SqlCommand(cmd, conn)
        Dim dt = New DataTable()
        conn.Open()
        Dim reader As SqlDataReader = comm.ExecuteReader
        dt.Load(reader)
        Dim items = dt.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray()
        ComboBox1.Items.AddRange(items)
        conn.Close()
        ComboBox1.SelectedIndex = 0
        LockUIAccess(Me)
        If Not GroupBox1.Enabled Then
            If Form1.activeuserdpt = "SP" Then
                CheckBox3.Checked = True
                CheckedListBox2.SetItemChecked(0, 1)
                CheckedListBox2.SetItemChecked(1, 0)
                CheckedListBox2.SetItemChecked(2, 0)
            ElseIf Form1.activeuserdpt = "BL" Then
                CheckBox2.Checked = True
                CheckedListBox2.SetItemChecked(0, 0)
                CheckedListBox2.SetItemChecked(1, 0)
                CheckedListBox2.SetItemChecked(2, 1)
            ElseIf Form1.activeuserdpt = "BP" Then
                CheckBox1.Checked = True
                CheckedListBox2.SetItemChecked(0, 0)
                CheckedListBox2.SetItemChecked(1, 1)
                CheckedListBox2.SetItemChecked(2, 0)
            End If
        End If
        If Not GroupBox2.Enabled Then
            If Form1.activeuserdpt = "SP" Then
                CheckedListBox1.SetItemChecked(0, 0)
                CheckedListBox1.SetItemChecked(1, 1)
                CheckedListBox1.SetItemChecked(2, 1)
                CheckedListBox1.SetItemChecked(3, 1)
                CheckedListBox1.SetItemChecked(4, 1)
            ElseIf Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Then
                CheckedListBox1.SetItemChecked(0, 1)
                CheckedListBox1.SetItemChecked(1, 0)
                CheckedListBox1.SetItemChecked(2, 1)
                CheckedListBox1.SetItemChecked(3, 0)
                CheckedListBox1.SetItemChecked(4, 1)
            End If
        End If
        CustomDGVSearchBox2.custom_command = "select distinct left(m.subcode1,Param3) code,datepart(wk,m_dispatchdate) weeknum,year(m_dispatchdate) yr from storetrade st inner join storetradelines s on s.ftrid=st.ftrid inner join material m on s.iteid=m.id inner join tbl_packerordercheck poc on poc.ftrid=s.ftrid left join TBL_PACKERORDCLINES pocl on pocl.stlid=s.id and pocl.line=1 where pocl.sc_recipient in (select Value from dbo.f_split('Param1',',')) and fltid1 in (Param2) and poc.status>=6 and poc.status<12 and  (m.descr2 like 'textbox'  or dbo.get_tradecode(st.ftrid) like 'textbox')"
        CustomDGVSearchBox3.custom_command = "select distinct dailyplanid as dpid from tbl_palletlines pl inner join tbl_palletheaders ph on ph.id=pl.palletid inner join pkrtbl_dailyplan dp on dp.id=pl.dailyplanid where ph.code like 'textbox' and dp.weeknumber=cast(Param1 as int)"
        ComboBox1.Enabled = False
        NotFirstLoad = True
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Application.AddMessageFilter(Me)
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
        '' Retrigger timer on keyboard and mouse messages
        If (m.Msg >= &H100 And m.Msg <= &H109) Or (m.Msg >= &H200 And m.Msg <= &H20E) Then
            If Me.ContainsFocus And Me.OwnedForms.Count = 0 Then
                timercounter = 0
            ElseIf Me.OwnedForms.Count <> 0 Then
                Timer1.Stop()
            ElseIf Not Timer1.Enabled Then
                Timer1.Start()
            End If
        End If
    End Function


    Public Shared Function FindControlRecursive(ByVal list As List(Of Control), ByVal parent As Control, ByVal ctrlType As System.Type) As List(Of Control)
        If parent Is Nothing Then Return list
        If parent.GetType Is ctrlType Then
            list.Add(parent)
        End If
        For Each child As Control In parent.Controls
            FindControlRecursive(list, child, ctrlType)
        Next
        Return list
    End Function

    Private Function IsMaterialCode(ByVal code As String) As Boolean
        Dim foundRow() As DataRow
        foundRow = ValidCodes.Select("code='" + code + "'")
        If foundRow.Count = 0 Then
            foundRow = ValidCodes.Select("subcode1='" + code + "'")
        End If
        If foundRow.Count = 0 Then
            Return False
        Else Return True
        End If
    End Function

    Private Function ReturnMaterialCode(ByVal code As String) As String
        Dim foundRow() As DataRow
        foundRow = ValidCodes.Select("code='" + code + "'")
        Return foundRow(0)(1).ToString
    End Function

    Private list As New List(Of Integer)

    Dim dt = New DataTable()
    Dim dt2 = New DataTable()
    Dim dpdt = New DataTable()
    Dim dpdt2 = New DataTable()
    Dim savedretvalue As String = ""
    Dim savedindices As CheckedListBox.CheckedIndexCollection
    Dim savedtype As Integer = 0
    Dim timercounter As Integer = 0
    Dim timerlimit As Integer = 300
    Dim totalsets As Double = 0
    Dim category As String = ""

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            list.Clear()
            If CheckedListBox1.CheckedItems.Count = 0 Then
                Throw New Exception("Πρέπει να επιλέξετε τουλάχιστον έναν Αποδέκτη")
            End If
            If CheckedListBox2.CheckedItems.Count = 0 Then
                Throw New Exception("Πρέπει να επιλέξετε τουλάχιστον ένα επίπεδο ομαδοποίησης")
            End If
            If Not CheckBox1.Checked And Not CheckBox2.Checked And Not CheckBox3.Checked And Not CheckBox4.Checked Then
                Throw New Exception("Επιλέξτε κατηγορία είδους")
            End If
            For Each l In CheckedListBox1.CheckedItems
                list.Add(CheckedListBox1.Items.IndexOf(l) + 1)
            Next
            savedretvalue = String.Join(",", list.ToArray)
            If CheckBox1.Checked Then
                savedtype = 0
            ElseIf CheckBox2.Checked Then
                savedtype = 1
            ElseIf CheckBox3.Checked Then
                savedtype = 3
            ElseIf CheckBox4.Checked Then
                savedtype = 4
            End If
            savedindices = CheckedListBox2.CheckedIndices
            category = ComboBox1.Items(ComboBox1.SelectedIndex).ToString.Substring(0, 3)
            work()
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

    Private Sub work()
        Try
            For Each c As Form In Me.OwnedForms
                c.Dispose()
            Next
            mainwork()
            dp1work()
            dp2work()
            stockwork()
            'notificationswork()
            'Dim allTxt As New List(Of Control)
            'For Each c As Zuby.ADGV.AdvancedDataGridView In FindControlRecursive(allTxt, Me, GetType(Zuby.ADGV.AdvancedDataGridView))
            '    Dim pic As New PictureBox
            '    pic.SizeMode = PictureBoxSizeMode.CenterImage
            '    pic.Image = My.Resources.rolling
            '    c.Parent.Controls.Add(pic)
            '    'tb.Controls.Add(pic, 0, 2)
            '    pic.Dock = DockStyle.Fill
            '    c.Visible = False
            'Next
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

    Private Sub mainwork()
        Try
            With DataGridView1
                .DataSource = Nothing
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                .Refresh()
                Dim pic As New PictureBox
                pic.SizeMode = PictureBoxSizeMode.CenterImage
                pic.Image = My.Resources.rolling
                .Parent.Controls.Add(pic)
                'tb.Controls.Add(pic, 0, 2)
                pic.Dock = DockStyle.Fill
                .Visible = False
            End With
            dt.Clear()
            dt.Columns.Clear()
            mainworker.RunWorkerAsync()
        Catch

        End Try
    End Sub

    Private Sub dp1work()
        Try
            With dpdgv1
                .DataSource = Nothing
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                .Refresh()
                Dim pic As New PictureBox
                pic.SizeMode = PictureBoxSizeMode.CenterImage
                pic.Image = My.Resources.rolling
                .Parent.Controls.Add(pic)
                'tb.Controls.Add(pic, 0, 2)
                pic.Dock = DockStyle.Fill
                .Visible = False
            End With
            dpdgv1.Columns.Clear()
            dpdgv1.DataSource = Nothing

            dpdt.Clear()
            dpdt.Columns.Clear()
            dpworker.RunWorkerAsync()
        Catch

        End Try
    End Sub

    Public Sub dp2work()
        Try
            dpdgv2.CancelEdit()
            With dpdgv2
                RemoveHandler .CellValueChanged, AddressOf dpdgv2_CellValueChanged
                .DataSource = Nothing
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                .Refresh()
                Dim pic As New PictureBox
                pic.SizeMode = PictureBoxSizeMode.CenterImage
                pic.Image = My.Resources.rolling
                .Parent.Controls.Add(pic)
                'tb.Controls.Add(pic, 0, 2)
                pic.Dock = DockStyle.Fill
                .Visible = False
            End With
            'dpdgv2.Columns.Clear()
            dpdgv2.Rows.Clear()
            dpdgv2.DataSource = Nothing

            dpdt2.Clear()
            dpdt2.Columns.Clear()
            dpworker2.RunWorkerAsync()
            notificationswork()
        Catch

        End Try
    End Sub

    Public Sub stockwork()
        Try
            stockdgv.CancelEdit()
            With stockdgv
                .DataSource = Nothing
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                .Refresh()
                Dim pic As New PictureBox
                pic.SizeMode = PictureBoxSizeMode.CenterImage
                pic.Image = My.Resources.rolling
                .Parent.Controls.Add(pic)
                'tb.Controls.Add(pic, 0, 2)
                pic.Dock = DockStyle.Fill
                .Visible = False
            End With
            'dpdgv2.Columns.Clear()
            stockdgv.Rows.Clear()
            stockdgv.DataSource = Nothing
            stockworker.RunWorkerAsync()
        Catch

        End Try
    End Sub


    Public Sub notificationswork()
        InformationPanel1.Clear()
        notificationsworker.RunWorkerAsync()
        notificationsworker2.RunWorkerAsync()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CheckedListBox1.SetItemChecked(0, 1)
        CheckedListBox1.SetItemChecked(1, 0)
        CheckedListBox1.SetItemChecked(2, 1)
        CheckedListBox1.SetItemChecked(3, 0)
        CheckedListBox1.SetItemChecked(4, 1)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        CheckedListBox1.SetItemChecked(0, 0)
        CheckedListBox1.SetItemChecked(1, 1)
        CheckedListBox1.SetItemChecked(2, 1)
        CheckedListBox1.SetItemChecked(3, 1)
        CheckedListBox1.SetItemChecked(4, 1)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        CheckedListBox1.SetItemChecked(0, 1)
        CheckedListBox1.SetItemChecked(1, 1)
        CheckedListBox1.SetItemChecked(2, 1)
        CheckedListBox1.SetItemChecked(3, 1)
        CheckedListBox1.SetItemChecked(4, 1)
    End Sub

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork

        Try
            Using tempdt1 = New DataTable()
                Dim str As String = ""
                If savedtype = 0 Then
                    str = "exec pkrprc_pending_BP @r,@l"
                ElseIf savedtype = 1 Then
                    str = "exec pkrprc_pending_BL @r,@l"
                ElseIf savedtype = 3 Then
                    str = "exec pkrprc_pending_SP @r,@l"
                ElseIf savedtype = 4 Then
                    str = "exec pkrprc_pending_cat @r,@l,'remaining',@c"
                End If
                Using command As SqlCommand = New SqlCommand(str, conn)
                    command.Parameters.AddWithValue("@r", savedretvalue)
                    command.Parameters.AddWithValue("@l", savedindices(0))
                    If savedtype = 4 Then
                        command.Parameters.AddWithValue("@c", category)
                    End If
                    conn.Open()
                    Using reader As SqlDataReader = command.ExecuteReader
                        tempdt1.Load(reader)
                    End Using
                    conn.Close()
                End Using
                Dim l1 As New List(Of String)
                For Each c As DataColumn In TryCast(tempdt1, DataTable).Columns
                    If System.Text.RegularExpressions.Regex.IsMatch(c.ColumnName, "^[a-zA-Z]+$") Then
                        Continue For
                    End If
                    l1.Add(c.ColumnName)
                    'c.DataType = System.Type.GetType("System.String")
                Next

                If RadioButton2.Checked Then
                    Using tempdt2 = New DataTable()
                        Dim str2 As String = ""
                        If savedtype = 0 Then
                            str2 = "exec pkrprc_pending_BP @r,@l,'original'"
                        ElseIf savedtype = 1 Then
                            str2 = "exec pkrprc_pending_BL @r,@l,'original'"
                        ElseIf savedtype = 3 Then
                            str2 = "exec pkrprc_pending_SP @r,@l,'original'"
                        ElseIf savedtype = 4 Then
                            str2 = "exec pkrprc_pending_SP @r,@l,'original',@c"
                        End If
                        Using command As SqlCommand = New SqlCommand(str2, conn)
                            command.Parameters.AddWithValue("@r", savedretvalue)
                            command.Parameters.AddWithValue("@l", savedindices(0))
                            If savedtype = 4 Then
                                command.Parameters.AddWithValue("@c", category)
                            End If
                            conn.Open()
                            Using reader As SqlDataReader = command.ExecuteReader
                                tempdt2.Load(reader)
                            End Using
                            conn.Close()
                        End Using

                        For col = 0 To TryCast(tempdt1, DataTable).Columns.Count - 1
                            TryCast(dt, DataTable).Columns.Add(New DataColumn(tempdt1.Columns(col).ColumnName))
                            'TryCast(dt, DataTable).Columns(col).ReadOnly = False
                            For row = 0 To TryCast(tempdt1, DataTable).Rows.Count - 1
                                If col = 0 Then
                                    Dim r As DataRow = dt.newrow
                                    dt.rows.add(r)
                                End If
                                Dim v As String = ""
                                If Not IsDBNull(tempdt1.Rows(row)(col)) Then
                                    v = tempdt1.Rows(row)(col).ToString
                                End If
                                Dim vv As String = ""
                                If Not IsDBNull(tempdt2.Rows(row)(col)) Then
                                    vv = tempdt2.Rows(row)(col).ToString
                                End If
                                If vv <> "" And v <> "" And TryCast(dt, DataTable).Columns(col).ColumnName <> "code" Then
                                    dt.rows(row)(col) = v.ToString + "/" + vv.ToString
                                ElseIf vv <> "" And v <> "" And TryCast(dt, DataTable).Columns(col).ColumnName = "code" Then
                                    dt.rows(row)(col) = v.ToString
                                Else
                                    dt.rows(row)(col) = Nothing
                                End If

                            Next
                            TryCast(dt, DataTable).Columns(col).ReadOnly = True
                        Next
                    End Using

                End If

                Dim ts As Double = 0
                If RadioButton1.Checked Then
                    dt = tempdt1
                    dt.Columns.Add("sum", GetType(Double))
                    For Each r As DataRow In dt.Rows
                        Dim s As Double = 0
                        For Each c As DataColumn In dt.Columns
                            If c.ColumnName <> "code" And c.ColumnName <> "sum" Then
                                If Not IsDBNull(r(c.ColumnName)) Then
                                    s += r(c.ColumnName)
                                    ts += r(c.ColumnName)
                                End If

                            End If
                        Next
                        r("sum") = s
                    Next
                    totalsets = ts
                    If ts = 0 Then
                        Throw New Exception("Δεν επέστρεψαν αποτελέσματα!")
                    End If
                    TryCast(dt, DataTable).Columns("sum").SetOrdinal(1)
                ElseIf RadioButton2.Checked Then
                    dt.Columns.Add("sum", GetType(Double))
                    For Each r As DataRow In dt.Rows
                        Dim s1 As Double = 0
                        Dim s2 As Double = 0
                        For Each c As DataColumn In dt.Columns
                            If c.ColumnName <> "code" And c.ColumnName <> "sum" Then
                                If Not IsDBNull(r(c.ColumnName)) Then
                                    Dim w As String() = r(c.ColumnName).Split(New Char() {"/"c})
                                    s1 += CDbl(Val(w(0)))
                                    's2 += CDbl(Val(w(1)))
                                    ts += s1
                                End If
                            End If
                        Next
                        r("sum") = s1 '+ "/" + s2.ToString
                    Next
                    TryCast(dt, DataTable).Columns("sum").SetOrdinal(1)
                End If
                Dim ls As String = String.Join(",", l1.ToArray)
                Dim coms As String = "SELECT F.ID,DBO.GET_TRADECODE(F.ID) TRADECODE,C.FATHERNAME,S.M_DISPATCHDATE FROM FINTRADE F INNER JOIN CUSTOMER C ON F.CUSID=C.ID INNER JOIN STORETRADE S ON S.FTRID=F.ID WHERE F.ID IN (" + ls + ")"
                Using command As SqlCommand = New SqlCommand(coms, conn)
                    conn.Open()
                    Using reader As SqlDataReader = command.ExecuteReader
                        dt2.load(reader)
                    End Using
                    conn.Close()
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
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Dim remarksdt As New DataTable()

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted
        Try
            'For i = 1 To 2
            '    Dim r = TryCast(dt, DataTable).NewRow
            '    TryCast(dt, DataTable).Rows.InsertAt(r, 0)
            'Next

            'For Each c As DataRow In TryCast(dt2, DataTable).Rows

            '    DataGridView1.Rows(0).Cells(c(0).ToString).Value = c(2)
            '    DataGridView1.Rows(1).Cells(c(0).ToString).Value = c(3)
            'Next            dpdgv1.ColumnHeadersVisible = False

            With DataGridView1
                .ColumnHeadersVisible = False
                .DataSource = dt
                .ColumnHeadersVisible = True
                '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                '.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                '.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            End With

            DataGridView1.Columns(0).Frozen = True
            Dim MinusColumns As Integer = 0
            Dim HasData As Boolean = False
            Dim ftridList As New List(Of Integer)
            For Each c As DataGridViewColumn In DataGridView1.Columns

                If c.Name = "code" Or c.Name = "sum" Then
                    If c.Name = "sum" Then
                        c.DefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold)
                    End If
                    Continue For
                Else
                    ftridList.Add(c.Name)
                End If
                If RadioButton1.Checked Then
                    Dim s As Double = 0
                    For i = 0 To DataGridView1.Rows.Count - 1
                        If Not IsDBNull(DataGridView1.Rows(i).Cells(c.Index).Value) Then
                            s += DataGridView1.Rows(i).Cells(c.Index).Value
                            HasData = True
                        End If
                    Next
                    Dim dt3 As DataRow() = dt2.Select("ID = " + c.Name)
                    c.HeaderText = dt3(0)(2) + Environment.NewLine + dt3(0)(1) + Environment.NewLine + dt3(0)(3) + Environment.NewLine + s.ToString + " sets"
                    'If s = 0 Then
                    '    c.Visible = False
                    '    MinusColumns += 1
                    'End If
                ElseIf RadioButton2.Checked Then
                    Dim s1 As Double = 0
                    Dim s2 As Double = 0
                    For i = 0 To DataGridView1.Rows.Count - 1
                        If Not IsDBNull(DataGridView1.Rows(i).Cells(c.Index).Value) Then
                            HasData = True
                            Dim w As String()
                            Dim v1 As Double = 0
                            Dim v2 As Double = 0
                            w = DataGridView1.Rows(i).Cells(c.Index).Value.ToString.Split(New Char() {"/"c})
                            v1 += CDbl(Val(w(0)))
                            v2 += CDbl(Val(w(1)))
                            s1 += v1
                            s2 += v2
                        End If
                    Next
                    Dim dt3 As DataRow() = dt2.Select("ID = " + c.Name)
                    c.HeaderText = dt3(0)(2) + Environment.NewLine + dt3(0)(1) + Environment.NewLine + dt3(0)(3) + Environment.NewLine + s1.ToString + " sets" '+ "/" + s2.ToString + " sets"
                    'If s = 0 Then
                    '    c.Visible = False
                    '    MinusColumns += 1
                    'End If
                End If
                If Not HasData Then
                    c.Visible = False
                    MinusColumns += 1
                End If
            Next

            Using s As New SqlCommand("select code,ftrid from pkrtbl_production where ftrid in (" + String.Join(",", ftridList.ToArray) + ")", conn)
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    remarksdt.Load(reader)
                End Using
                conn.Close()
            End Using
            color_comments()


            ToolStripStatusLabel1.Text = "Εμφανίζονται " + (DataGridView1.Columns.Count - 2 - MinusColumns).ToString + " παραγγελίες."
            ToolStripStatusLabel2.Text = "Προς συσκευασία: " + totalsets.ToString + " sets."
            DataGridView1.Refresh()
            DataGridView1.Visible = True
            timercounter = 0
            Timer1.Start()
            Label1.Visible = True
            Button6.Visible = True
            Button7.Visible = True
            Button1.Image = My.Resources.icons8_available_updates_30
            Button1.Text = "Ανανέωση"
        Catch EX As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            If EX.Message.Contains("'DBNull'") Then
                Using errfrm As New errormsgbox(EX.StackTrace.ToString, "Δεν επέστρεψε αποτελέσματα!", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            Else
                Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End If


        Finally

            For Each c As Control In TableLayoutPanel2.Controls
                If Not IsNothing(TryCast(c, PictureBox)) Then
                    c.Dispose()
                End If

            Next
        End Try
    End Sub

    Private Sub color_comments()
        For Each dr As DataRow In remarksdt.Rows
            For Each dgvr As DataGridViewRow In DataGridView1.Rows
                If dr.Item("code") = dgvr.Cells("code").Value Then
                    If DataGridView1.Columns.Contains(dr.Item("ftrid")) Then
                        DataGridView1.Rows(dgvr.Index).Cells(dr.Item("ftrid").ToString).Style.BackColor = Color.Orange
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs)
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = DataGridView1.CurrentCell.ColumnIndex + 1 >= DataGridView1.ColumnCount
            Dim endrow As Boolean = DataGridView1.CurrentCell.RowIndex + 1 >= DataGridView1.RowCount

            If endcol AndAlso endrow Then
                startColumn = DataGridView1.CurrentCell.ColumnIndex
                startRow = DataGridView1.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, DataGridView1.CurrentCell.ColumnIndex + 1)
                startRow = DataGridView1.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = DataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = DataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            DataGridView1.CurrentCell = c
        End If
    End Sub


    Private Sub AdvancedDataGridViewSearchToolBar2_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs)
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = dpdgv1.CurrentCell.ColumnIndex + 1 >= dpdgv1.ColumnCount
            Dim endrow As Boolean = dpdgv1.CurrentCell.RowIndex + 1 >= dpdgv1.RowCount

            If endcol AndAlso endrow Then
                startColumn = dpdgv1.CurrentCell.ColumnIndex
                startRow = dpdgv1.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, dpdgv1.CurrentCell.ColumnIndex + 1)
                startRow = dpdgv1.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = dpdgv1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = dpdgv1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            dpdgv1.CurrentCell = c
        End If
    End Sub

    Private Sub AdvancedDataGridViewSearchToolBar3_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs)
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = dpdgv2.CurrentCell.ColumnIndex + 1 >= dpdgv2.ColumnCount
            Dim endrow As Boolean = dpdgv2.CurrentCell.RowIndex + 1 >= dpdgv2.RowCount

            If endcol AndAlso endrow Then
                startColumn = dpdgv2.CurrentCell.ColumnIndex
                startRow = dpdgv2.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, dpdgv2.CurrentCell.ColumnIndex + 1)
                startRow = dpdgv2.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = dpdgv2.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = dpdgv2.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            dpdgv2.CurrentCell = c
        End If
    End Sub

    Private Sub AdvancedDataGridViewSearchToolBar4_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs)
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = stockdgv.CurrentCell.ColumnIndex + 1 >= stockdgv.ColumnCount
            Dim endrow As Boolean = stockdgv.CurrentCell.RowIndex + 1 >= stockdgv.RowCount

            If endcol AndAlso endrow Then
                startColumn = stockdgv.CurrentCell.ColumnIndex
                startRow = stockdgv.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, stockdgv.CurrentCell.ColumnIndex + 1)
                startRow = stockdgv.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = stockdgv.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = stockdgv.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            stockdgv.CurrentCell = c
        End If
    End Sub





    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            CheckBox2.Checked = False
            CheckBox3.Checked = False
            CheckBox4.Checked = False
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            CheckBox1.Checked = False
            CheckBox3.Checked = False
            CheckBox4.Checked = False
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            CheckBox1.Checked = False
            CheckBox2.Checked = False
            CheckBox4.Checked = False
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            ComboBox1.Enabled = True
            CheckBox1.Checked = False
            CheckBox2.Checked = False
            CheckBox3.Checked = False
        Else
            ComboBox1.Enabled = False
        End If
    End Sub

    Private Sub CheckedListBox2_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox2.ItemCheck
        If e.CurrentValue = CheckState.Unchecked Then
            For Each indexChecked In CheckedListBox2.CheckedIndices
                If indexChecked <> e.Index Then
                    CheckedListBox2.SetItemChecked(indexChecked, 0)
                End If
            Next
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex < 0 And e.ColumnIndex >= 0 Then
            If DataGridView1.Columns(e.ColumnIndex).Name <> "sum" And DataGridView1.Columns(e.ColumnIndex).Name <> "code" Then
                Using f As New Order(DataGridView1.Columns(e.ColumnIndex).Name)
                    f.ShowDialog()
                End Using
            End If
        ElseIf e.RowIndex >= 0 And e.ColumnIndex >= 0 AndAlso Not IsDBNull(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
            If DataGridView1.Columns(e.ColumnIndex).HeaderText <> "sum" And DataGridView1.Columns(e.ColumnIndex).HeaderText <> "code" Then
                Dim ftrid As Integer = dt.columns(e.ColumnIndex).columnname
                Dim itecode As String = DataGridView1.Rows(e.RowIndex).Cells(0).Value
                Dim frm As New ProductionItemQuickReport(ftrid, itecode, Cursor.Position.X, Cursor.Position.Y, savedretvalue)
                frm.Owner = Me
                frm.Show()
            Else
                Dim itecode As String = DataGridView1.Rows(e.RowIndex).Cells(0).Value
                Dim frm As New ProductionItemOrderQuickView(savedretvalue, itecode, Cursor.Position.X, Cursor.Position.Y)
                frm.Owner = Me
                frm.Show()
            End If
        End If
    End Sub

    Private Sub DataGridView1_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.ColumnHeaderMouseClick

    End Sub

    Private Sub DataGridView1_ColumnHeaderMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.ColumnHeaderMouseDoubleClick
        If dt.columns(e.ColumnIndex).columnname <> "sum" And dt.columns(e.ColumnIndex).columnname <> "code" Then
            Using f As New Order(dt.columns(e.ColumnIndex).columnname)
                f.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not dpdgv2.IsCurrentRowDirty Then
            If Me.OwnedForms.Count = 0 Then
                timercounter += 1
            End If
            If (timerlimit - timercounter) < 60 Then
                Label1.Text = "Αυτόματη ανανέωση σε " + Math.Round((timerlimit - timercounter), 2).ToString + " δευτ/πτα."
            Else
                Label1.Text = "Αυτόματη ανανέωση σε " + Math.Round(((timerlimit - timercounter) / 60), 2).ToString + " λεπτά."
            End If
            If timercounter = timerlimit Then
                dpdgv2.EndEdit()
                work()
            End If
        End If
    End Sub

    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles Label1.TextChanged
        If timercounter > timerlimit - 50 Then
            Label1.ForeColor = Color.Firebrick
        Else
            Label1.ForeColor = SystemColors.ButtonShadow
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Dispose()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If Timer1.Enabled Then
            Timer1.Stop()
            Button6.Image = My.Resources.PLAY
        Else
            Timer1.Start()
            Button6.Image = My.Resources.PAUSE
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        timercounter = 0
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button8_Click_1(sender As Object, e As EventArgs)
        Try
            dpdgv1.DataSource = Nothing
            dpdt = Transpose(dpdt)
            dpdgv1.ColumnHeadersVisible = False
            dpdgv1.DataSource = dpdt
            dpdgv1.ColumnHeadersVisible = True
            dpdgv1.Refresh()
            For Each c As Control In TableLayoutPanel3.Controls
                If Not IsNothing(TryCast(c, PictureBox)) Then
                    c.Dispose()
                End If
            Next
            SplitContainer1.Visible = True
            'orientationworker.RunWorkerAsync()
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If dpconn1.State = ConnectionState.Open Then
                dpconn1.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Dim lvl As Integer
    Private Sub dpworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles dpworker.DoWork
        Try
            Dim str2 As String = ""
            Dim str As String = "select left(m.subcode1,@l) code," +'dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0) date,
                                "cast(day(dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)) as varchar(2))+'/'+
                                cast(month(dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)) as varchar(2))+'-'+
                                CONVERT (varchar(10), dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)+4, 103) AS week,DATEPART(WK,st.m_dispatchdate) WEEKNUM,YEAR(ST.m_dispatchdate) YR, sum(stl.PRIMARYQTY) AS start ,sum(ISNULL(stl.M_BOQTY, 0)) AS BACKORDER,
                                sum(ISNULL(z.quantity, 0) - ISNULL(z.closed, 0)-ISNULL(z.sent, 0)) AS lightgreen, sum(ISNULL(z.quantitywithscheduled - z.quantity, 0)) AS blue, sum(STL.PRIMARYQTY - ISNULL(z.quantitywithscheduled, 0) - ISNULL(STL.M_BOQTY, 0) ) AS black, sum(ISNULL(z.closed, 0)) AS green, 
                        sum( ISNULL(z.sent, 0) ) AS gold, 
           DateAdd(wk, datediff(wk, 0, st.m_dispatchdate), 0) startdate, dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)+4 enddate
                                from 
	                                iTEMTRANSEST s 
	                                inner join storetradelines stl on stl.id=s.stlid
	                                inner join material m on m.id=s.iteid
	                                inner join fintrade f on f.id=s.ftrid
	                                inner join storetrade st on st.ftrid=f.id
	                                inner join tbl_packerordercheck t on t.ftrid=f.ID left outer join
	                                dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = stl.ID left join
	                                tbl_packerordclines pocl on pocl.stlid=s.stlid and isnull(pocl.line,1)=1 left join
                                pkrtbl_dailyplan dp on dp.type=0 and dp.pertypeid1=stl.id and dp.id=z.dailyplanid
                            where
                            f.DSRID=9000 and f.SOURCE=5 and t.status<12 and t.status>=6 and pocl.sc_recipient in (" + savedretvalue + ")
                            and "
            Dim str3 As String = " group by left(m.subcode1,@l),YEAR(ST.m_dispatchdate),DATEPART(WK,st.m_dispatchdate),cast(day(dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)) as varchar(2))+'/'+
                                cast(month(dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)) as varchar(2))+'-'+
                                CONVERT (varchar(10), dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)+4, 103), dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0) , dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)+4 " +'dateadd(wk, datediff(wk, 0, st.m_dispatchdate), 0)
                            "ORDER BY 4 ASC,3 ASC,1 ASC "
            If savedtype = 0 Then
                str2 = "left(m.subcode1,3) in ('102','202')"
            ElseIf savedtype = 1 Then
                str2 = "left(m.subcode1,1)='1' and left(m.subcode1,3)<>'102'"
            ElseIf savedtype = 3 Then
                str2 = "left(m.subcode1,1)='2' and left(m.subcode1,3)<>'202'"
            ElseIf savedtype = 4 Then
                str2 = "left(m.subcode1,1)='" + category + "'"
            End If
            If savedindices(0) = 0 Then
                lvl = 9
            ElseIf savedindices(0) = 1 Then
                lvl = 11
            Else
                lvl = 12
            End If
            Using command As SqlCommand = New SqlCommand(str + str2 + str3, dpconn1)
                command.Parameters.AddWithValue("@l", lvl)
                dpconn1.Open()
                Using reader As SqlDataReader = command.ExecuteReader
                    dpdt.Load(reader)
                End Using
                dpconn1.Close()
            End Using
            e.Result = dpdt
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If dpconn1.State = ConnectionState.Open Then
                dpconn1.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            If dpconn1.State = ConnectionState.Open Then
                dpconn1.Close()
            End If
        End Try
    End Sub

    Private Sub dpworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles dpworker.RunWorkerCompleted
        dpdgv1.DataSource = e.Result

    End Sub

    Private Function Transpose(ByVal table As DataTable) As DataTable
        Dim flippedTable As New DataTable
        'creates as many columns as rows in source table
        flippedTable.Columns.AddRange(
            table.Select.Select(
                Function(dr) New DataColumn("col" & table.Rows.IndexOf(dr), GetType(Object))
                ).ToArray)
        'iterates columns in source table
        For Each dc As DataColumn In table.Columns
            'get array of values of column in each row and add as new row in target table
            flippedTable.Rows.Add(table.Select.Select(Function(dr) dr(dc)).ToArray)
        Next
        Return flippedTable
    End Function


    Private Sub SplitContainer1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved

    End Sub

    Private Sub dpdgv1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dpdgv1.DataBindingComplete
        Try
            Dim col As New DataGridViewButtonColumn
            col.DefaultCellStyle.Font = New Font("Wingdings", 10.0F)
            col.DefaultCellStyle.SelectionBackColor = Color.White
            col.DefaultCellStyle.SelectionForeColor = Color.Black
            col.Text = ""
            col.Name = "buttonclmn"
            col.UseColumnTextForButtonValue = True
            If Not dpdgv1.Columns.Contains("buttonclmn") Then
                dpdgv1.Columns.Add(col)
            End If
            Dim col2 As New DatagridviewStackedProgressColumn
            col2.Name = "Status"
            If Not dpdgv1.Columns.Contains("Status") Then
                dpdgv1.Columns.Insert(2, col2)
            End If
            Dim ColVisibles As String() = {"code", "week", "buttonclmn", "Status", "start"} 'ΟΡΑΤΕΣ
            Dim ColHeaderNames As New Dictionary(Of String, String) From {{"code", "ΚΩΔΙΚΟΣ"}, {"week", "ΕΒΔΟΜΑΔΑ"}, {"buttonclmn", ""}, {"Status", "Κατάσταση"}, {"start", "ΠΟΣ"}} 'ΤΙΤΛΟΙ ΚΕΦΑΛΙΔΩΝ ΟΡΑΤΩΝ ΣΤΗΛΩΝ
            Dim ColWidths As New Dictionary(Of String, Integer) From {{"code", 70}, {"week", 100}, {"start", 50}, {"Status", 100}, {"buttonclmn", 42}} 'ΠΛΑΤΗ ΟΡΑΤΩΝ ΣΤΗΛΩΝ
            For Each c As DataGridViewColumn In dpdgv1.Columns
                If ColVisibles.Contains(c.Name) Then
                    c.Visible = True
                    c.HeaderText = ColHeaderNames(c.Name)
                    c.Width = ColWidths(c.Name)
                Else
                    c.Visible = False
                End If
            Next

            For Each r As DataGridViewRow In dpdgv1.Rows
                If r.Cells("BLACK").Value = 0 Then
                    r.Cells("buttonclmn").Style.ForeColor = Color.LightGray
                    r.Cells("buttonclmn").Style.SelectionForeColor = Color.LightGray
                End If
                Dim scv As String = r.Cells("BACKORDER").Value.ToString + "/" + r.Cells("black").Value.ToString + "/" + r.Cells("blue").Value.ToString + "/" + r.Cells("lightgreen").Value.ToString + "/" + r.Cells("green").Value.ToString + "/" + r.Cells("gold").Value.ToString
                If Not IsNothing(scv) Then
                    r.Cells("Status").Value = scv
                End If
            Next
            With dpdgv1
                .ColumnHeadersVisible = False
                .ColumnHeadersVisible = True
                '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                '.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                '.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            End With

            'For Each r As DataGridViewRow In dpdgv1.Rows
            '    r.Cells("buttonclmn").Value = "A"
            'Next
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If dpconn1.State = ConnectionState.Open Then
                dpconn1.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

        Finally
            dpdgv1.Visible = True
            dpdgv1.Refresh()
            For Each c As Control In TableLayoutPanel5.Controls
                If Not IsNothing(TryCast(c, PictureBox)) Then
                    c.Dispose()
                End If

            Next
            SplitContainer1.SplitterDistance = 399
        End Try


    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            RadioButton1.Checked = False
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            RadioButton2.Checked = False
        End If
    End Sub

    Private Sub dpdgv2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv2.CellContentClick

    End Sub

    Public Function ReturnDailyPlanCandidateData(ByVal itecode As String, startdate As Date, enddate As Date)

        Dim cmd As String = "SELECT f.id ftrid,isnull(c.fathername,'???')+' '+DBO.GET_TRADECODE(F.ID) tradecode,f.ftrdate,m.id iteid,s.id stlid,m.subcode1,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,sum(CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END) QTY, sum(ISNULL(z.quantity, 0))  DISTRIBED,m.m_partno,mnf.descr brand
FROM FINTRADE F INNER JOIN TBL_PACKERORDERCHECK T1 ON T1.FTRID=F.ID INNER JOIN STORETRADELINES S ON S.FTRID=F.ID 
INNER JOIN STORETRADE ST ON ST.FTRID=F.ID
INNER JOIN MATERIAL M ON M.ID=S.ITEID LEFT JOIN TBL_PACKERORDCLINES T ON T.STLID=S.ID AND T.LINE=1
left join customer c on c.id=f.cusid
LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join tbl_recipients t2 on t2.id=t.sc_recipient
 LEFT OUTER JOIN
                         dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID
left join TBL_PACKERORDERSTATUS t3 on t3.id=t1.status
left join manufacturer mnf on mnf.codeid=m.mnfid
WHERE  M.SUBCODE1 LIKE '" + itecode + "%' AND F.DSRID=9000 AND T1.STATUS<12 AND T1.STATUS>=6 and t.sc_recipient in (1,3,5)  AND ST.M_DISPATCHDATE >=CONVERT(datetime, '" + startdate + "', 103) AND ST.M_DISPATCHDATE<=CONVERT(datetime, '" + enddate + "', 103) 
GROUP BY  f.id,DBO.GET_TRADECODE(F.ID),m.id,s.id,m.subcode1,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,f.ftrdate,c.fathername,m.m_partno,mnf.descr
having sum(ISNULL(z.quantity, 0))<sum(S.PRIMARYQTY)
ORDER BY 7 ASC"
        Dim resultdt As New DataTable()
        Using sc As New SqlCommand(cmd, conn)
            conn.Open()

            Using reader As SqlDataReader = sc.ExecuteReader
                resultdt.Load(reader)
            End Using
            conn.Close()
        End Using
        Return resultdt
    End Function

    Private Sub dpdgv1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv1.CellContentClick
        Try
            If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then
                If dpdgv1.Columns(e.ColumnIndex).Name = "buttonclmn" And dpdgv1.Rows(e.RowIndex).Cells("black").Value <> 0 Then
                    If Not IsNothing(dpdgv2.CurrentRow) AndAlso Not dpdgv2.CurrentRow.IsNewRow AndAlso Not dpdgv2.CurrentRow.Cells("RowValidated").Value Then
                        Throw New Exception("Ολοκληρώστε πρώτα την εισαγωγή της προηγούμενης γραμμής")
                    End If
                    If Not dpdgv2.AllowUserToAddRows Then
                        Return
                    End If
                    'MessageBox.Show(dpdgv1.Rows(e.RowIndex).Cells("startdate").Value)
                    Dim itecode As String = dpdgv1.Rows(e.RowIndex).Cells("code").Value
                    Dim resultdt As DataTable = ReturnDailyPlanCandidateData(itecode, dpdgv1.Rows(e.RowIndex).Cells("startdate").Value, dpdgv1.Rows(e.RowIndex).Cells("enddate").Value)
                    If resultdt.Rows.Count > 1 Then
                        Dim frm As New ProductionItemOrderQuickView(savedretvalue, itecode, Cursor.Position.X, Cursor.Position.Y, dpdgv1.Rows(e.RowIndex).Cells("startdate").Value, dpdgv1.Rows(e.RowIndex).Cells("enddate").Value, True)
                        Dim fres = frm.ShowDialog()
                        If fres = DialogResult.OK Then
                            Dim r = frm.ResultRow
                            Dim res As New Dictionary(Of String, String)
                            Using c As New SqlCommand("Select m_partno, descr from material m left join manufacturer mnf On m.mnfid=mnf.codeid where m.id=" + r("iteid").ToString, conn)
                                conn.Open()
                                Using READER As SqlDataReader = c.ExecuteReader
                                    While READER.Read()
                                        For i As Integer = 0 To READER.FieldCount - 1
                                            res.Add(READER.GetName(i), READER(i))
                                        Next
                                    End While

                                End Using
                                conn.Close()
                            End Using
                            Dim NewRowIndex = dpdgv2.Rows.Add()
                            Dim NewRow As DataGridViewRow = dpdgv2.Rows(NewRowIndex)
                            NewRow.Cells("linestatus").Value = "0"
                            NewRow.Cells("dpid").Value = Nothing
                            NewRow.Cells("type").Value = 0
                            NewRow.Cells("pertypeid1").Value = r("stlid")
                            NewRow.Cells("pertypeid2").Value = Nothing
                            NewRow.Cells("ftrid").Value = r("ftrid")
                            NewRow.Cells("iteid").Value = (r("iteid"))
                            NewRow.Cells("code").Value = (r("subcode1"))
                            NewRow.Cells("partno").Value = res("m_partno")
                            NewRow.Cells("brand").Value = res("descr")
                            NewRow.Cells("tradecode").Value = (r("tradecode"))
                            NewRow.Cells("quantity").Value = (r("qty"))
                            NewRow.Cells("quantitylimit").Value = (r("qty"))
                            NewRow.Cells("plandate").Value = Nothing
                            NewRow.Cells("day").Value = Nothing
                            NewRow.Cells("remarks").Value = Nothing
                            NewRow.Cells("RowValidated").Value = False
                            dpdgv2.CurrentCell = dpdgv2.Rows(NewRowIndex).Cells("plandate")
                            dpdgv2.BeginEdit(True)
                            dpdgv2.NotifyCurrentCellDirty(True)
                            'dpdgv2.Rows.Add(New ("", "1", r("id"), r("type"), r("pertypeid1"), r("pertypeid2"), r("ftrid"), r("iteid"), r("tradecode"), r("quantity"), r("day"), r("Date"), r("remarks")))
                            'dpdgv2.Rows(dpdgv2.Rows.Count - 1).Cells("WorkButton").Value = 

                        End If
                    Else
                        Dim r = resultdt.Rows(0)

                        Dim NewRowIndex = dpdgv2.Rows.Add()
                        Dim NewRow As DataGridViewRow = dpdgv2.Rows(NewRowIndex)
                        NewRow.Cells("linestatus").Value = "0"
                        NewRow.Cells("dpid").Value = Nothing
                        NewRow.Cells("type").Value = 0
                        NewRow.Cells("pertypeid1").Value = r("stlid")
                        NewRow.Cells("pertypeid2").Value = Nothing
                        NewRow.Cells("ftrid").Value = r("ftrid")
                        NewRow.Cells("iteid").Value = (r("iteid"))
                        NewRow.Cells("code").Value = (r("subcode1"))
                        NewRow.Cells("partno").Value = (r("m_partno"))
                        NewRow.Cells("brand").Value = (r("brand"))
                        NewRow.Cells("tradecode").Value = (r("tradecode"))
                        NewRow.Cells("quantity").Value = (r("qty"))
                        NewRow.Cells("quantitylimit").Value = (r("qty"))
                        NewRow.Cells("plandate").Value = Nothing
                        NewRow.Cells("day").Value = Nothing
                        NewRow.Cells("remarks").Value = Nothing
                        dpdgv2.CurrentCell = dpdgv2.Rows(NewRowIndex).Cells("plandate")
                        dpdgv2.BeginEdit(True)
                        dpdgv2.NotifyCurrentCellDirty(True)
                    End If

                End If
            End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally


        End Try
    End Sub

    Private Sub dpdgv2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv2.CellDoubleClick

    End Sub
    Private Sub dpdgv2_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dpdgv2.CellMouseEnter
        If e.ColumnIndex >= 0 AndAlso (dpdgv2.Columns(e.ColumnIndex).Name = "code" Or dpdgv2.Columns(e.ColumnIndex).Name = "tradecode") Then
            Me.dpdgv2.Cursor = Cursors.Hand
        Else
            Me.dpdgv2.Cursor = Cursors.Default
        End If

    End Sub

    Private Sub dpdgv2_CellMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dpdgv2.CellMouseLeave
        Me.dpdgv2.Cursor = Cursors.Default
    End Sub

    Private Sub dpdgv1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv1.CellDoubleClick
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then

            If dpdgv1.Columns(e.ColumnIndex).Name = "Status" Then
                Dim dt As New DataTable()
                dt.Columns.Add("stlid", GetType(Integer))
                dt.Columns.Add("s")
                dt.Columns.Add("t", GetType(Double))
                Dim piecolumns As New Dictionary(Of String, String) From {{"BACKORDER", "Backorder"}, {"blue", "Σε σχεδιασμένες παλέτες"}, {"black", "Εκκρεμούν"}, {"lightgreen", "Σε παλέτα"}, {"gold", "Απεσταλμένα"}, {"green", "Σε κλειστές παλέτες"}}
                'Dim stlid = dpdgv1.Rows(e.RowIndex).Cells("stlid").Value
                For Each c As KeyValuePair(Of String, String) In piecolumns
                    Dim dtrow As DataRow = dt.NewRow
                    dtrow("stlid") = 0
                    dtrow("s") = c.Value
                    dtrow("t") = dpdgv1.Rows(e.RowIndex).Cells(c.Key).Value
                    dt.Rows.Add(dtrow)
                Next
                Using f As New DatagridviewStackedProgressColumnReportForm(0, dpdgv1.Rows(e.RowIndex).Cells("code").Value, dpdgv1.Rows(e.RowIndex).Cells("week").Value, Cursor.Position.X, Cursor.Position.Y, dt, savedretvalue, dpdgv1.Rows(e.RowIndex).Cells("weeknum").Value, "week", dpdgv1.Rows(e.RowIndex).Cells("yr").Value)
                    f.Owner = Me
                    f.ShowDialog()
                End Using
            ElseIf dpdgv1.Columns(e.ColumnIndex).Name <> "buttonclmn" Then
                Dim itecode As String = dpdgv1.Rows(e.RowIndex).Cells("code").Value
                Dim frm As New ProductionItemOrderQuickView(savedretvalue, itecode, Cursor.Position.X, Cursor.Position.Y, dpdgv1.Rows(e.RowIndex).Cells("startdate").Value, dpdgv1.Rows(e.RowIndex).Cells("enddate").Value)
                frm.Owner = Me
                frm.Show()
            End If
        End If
    End Sub

    Private Sub dpworker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles dpworker2.DoWork
        Try
            Dim dfi = DateTimeFormatInfo.CurrentInfo
            Dim calendar = dfi.Calendar
            weekOfYear = calendar.GetWeekOfYear(FirstMonday, dfi.CalendarWeekRule, DayOfWeek.Monday)
            Dim EXTRA As String = ""
            If savedtype = 0 Then
                EXTRA = " And left(code, 3) In ('102','202') "
            ElseIf savedtype = 1 Then
                EXTRA = " AND left(code,1)='1' and left(code,3)<>'102' "
            ElseIf savedtype = 3 Then
                EXTRA = " AND left(code,1)='2' and left(code,3)<>'202' "
            ElseIf savedtype = 4 Then
                EXTRA = " AND left(code,1)='" + category + "' "
            End If
            Dim str As String = "SELECT [ID]
                                  ,[TYPE]
                                  ,[PERTYPEID1]
                                  ,[ftrid]
                                  ,[iteid]
                                  ,[code],[partno],[brand]
                                  ,[tradecode]
                                    ,[TMX]
                                  ,[QUANTITY]
                                  ,[day]
                                  ,[DATE]
                                  ,[REMARKS]
                                  ,[PERTYPEID2],[sum]
                              FROM [dbo].[PKRVIW_DAILYPLAN] where WEEKNUMBER=" + weekOfYear.ToString + " AND YEAR(DATE)=" + FirstMonday.Year.ToString + EXTRA + " ORDER BY 12 ASC"

            'date>='" + FirstMonday.ToString("yyyy/MM/dd") + "' and date<='" + FirstMonday.AddDays(6).ToString("yyyy/MM/dd") + "' order by 12 asc"
            Dim today As Date = Date.Today
            Using command As SqlCommand = New SqlCommand(str, dpconn2)
                dpconn2.Open()
                Using reader As SqlDataReader = command.ExecuteReader
                    dpdt2.Load(reader)
                End Using
                dpconn2.Close()
            End Using
            e.Result = dpdt2
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If dpconn2.State = ConnectionState.Open Then
                dpconn2.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            If dpconn2.State = ConnectionState.Open Then
                dpconn2.Close()
            End If
        End Try
    End Sub

    Private Sub dpworker2_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles dpworker2.RunWorkerCompleted
        Try
            Dim today As Date = Date.Today
            If today.DayOfWeek = DayOfWeek.Sunday Then today = today.AddDays(-1)
            Dim monDate As DateTime = today.AddDays(DayOfWeek.Monday - today.DayOfWeek)
            With dpdgv2
                .ColumnHeadersVisible = False
                .ColumnHeadersVisible = True
                If FirstMonday = monDate Or monDate.AddDays(7) = FirstMonday Or monDate.AddDays(14) = FirstMonday Or monDate.AddDays(21) = FirstMonday Or monDate.AddDays(28) = FirstMonday Then
                    dpdgv2.AllowUserToAddRows = True
                    dpdgv2.ReadOnly = False
                Else
                    dpdgv2.AllowUserToAddRows = False
                    dpdgv2.ReadOnly = True
                End If
                '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                '.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                '.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            End With
            For Each r As DataRow In e.Result.Rows
                Dim NewRowIndex = dpdgv2.Rows.Add()
                Dim NewRow As DataGridViewRow = dpdgv2.Rows(NewRowIndex)
                NewRow.Cells("linestatus").Value = "1"
                NewRow.Cells("dpid").Value = r("id")
                NewRow.Cells("type").Value = r("type")
                NewRow.Cells("pertypeid1").Value = r("pertypeid1")
                NewRow.Cells("pertypeid2").Value = r("pertypeid2")
                NewRow.Cells("ftrid").Value = r("ftrid")
                NewRow.Cells("iteid").Value = (r("iteid"))
                NewRow.Cells("code").Value = (r("code"))
                NewRow.Cells("partno").Value = (r("partno"))
                NewRow.Cells("brand").Value = (r("brand"))
                NewRow.Cells("TMX").Value = (r("TMX"))
                NewRow.Cells("tradecode").Value = (r("tradecode"))
                NewRow.Cells("quantity").Value = (r("quantity"))
                NewRow.Cells("plandate").Value = (r("date"))
                NewRow.Cells("day").Value = (r("day"))
                NewRow.Cells("remarks").Value = (r("remarks"))
                NewRow.Cells("RowValidated").Value = True
                NewRow.Cells("pallets").Value = r("sum")
                If r("sum") = 0 Then
                    NewRow.Cells("pallets").Style.BackColor = Color.LightSalmon
                ElseIf r("sum") = r("quantity") Then
                    NewRow.Cells("pallets").Style.BackColor = Color.LightGreen
                ElseIf r("sum") < r("quantity") Then
                    NewRow.Cells("pallets").Style.BackColor = Color.LightYellow
                End If
                'dpdgv2.Rows.Add(New ("", "1", r("id"), r("type"), r("pertypeid1"), r("pertypeid2"), r("ftrid"), r("iteid"), r("tradecode"), r("quantity"), r("day"), r("date"), r("remarks")))
                'dpdgv2.Rows(dpdgv2.Rows.Count - 1).Cells("WorkButton").Value = 
                If CType(NewRow.Cells("plandate").Value, DateTime).Date < DateTime.Today Then
                    NewRow.ReadOnly = True
                    NewRow.DefaultCellStyle.BackColor = Color.LightGray
                End If
            Next

        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If dpconn2.State = ConnectionState.Open Then
                dpconn2.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            dpdgv2.Visible = True
            AddHandler dpdgv2.CellValueChanged, AddressOf dpdgv2_CellValueChanged
            dpdgv2.Refresh()
            For Each c As Control In TableLayoutPanel3.Controls
                If Not IsNothing(TryCast(c, PictureBox)) Then
                    c.Dispose()
                End If

            Next
            If requested_highlight <> 0 Then
                dpdgv2.ClearSelection()
                dpdgv2.CurrentCell = Nothing
                dpdgv2.Rows(0).Selected = False
                For Each r As DataGridViewRow In dpdgv2.Rows
                    If r.Cells("dpid").Value = requested_highlight Then
                        r.Selected = True
                    End If
                Next
                requested_highlight = 0
            End If
        End Try
    End Sub

    Private Sub DataGridView1_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridView1.ColumnWidthChanged
    End Sub

    Private Sub dpdgv1_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dpdgv1.ColumnWidthChanged

    End Sub

    Private Sub dpdgv2_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dpdgv2.ColumnWidthChanged

    End Sub


    Private Sub dpdgv2_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dpdgv2.CellValidating
        'If dpdgv2.Columns(e.ColumnIndex).Name = "code" AndAlso Not e.FormattedValue = "" Then
        '    Dim SearchTerm As String = ""
        '    Dim CellValue As String = e.FormattedValue
        '    With CellValue
        '        If .Contains("-") And .Length <> 16 Then
        '            SearchTerm = CellValue + "%"
        '        ElseIf .Contains("-") And .Length = 16 Then

        '            SearchTerm = CellValue


        '        ElseIf Not .Contains("-") And .Length = 15 Then


        '            SearchTerm = CellValue
        '        ElseIf .Equals("") Then
        '            e.Cancel = True
        '            Return
        '        Else
        '            SearchTerm = CellValue
        '        End If
        '    End With
        '    Using frm As New ProductionSearchForm(SearchTerm)
        '        Dim dr As DialogResult = frm.ShowDialog()
        '        If dr = DialogResult.Cancel Then
        '            e.Cancel = True
        '            'SendKeys.Send("+{HOME}")
        '            'SendKeys.Send("{DEL}")
        '            dpdgv2.CancelEdit()
        '        ElseIf dr = DialogResult.OK Then
        '            Selectedid = frm.Selectedid
        '            SelectedCode = frm.SelectedCode
        '            Selectedftrid = frm.Selectedftrid
        '            e.Cancel = False
        '        End If
        '    End Using
        'End If
    End Sub

    Private SearchResults As New Dictionary(Of String, Object)

    Private Sub dpdgv2_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv2.CellEndEdit
        Try
            If dpdgv2.Columns(e.ColumnIndex).Name = "code" Then
                Dim cellvalue As String = dpdgv2.Rows(e.RowIndex).Cells("code").Value
                If Not cellvalue = "" Then
                    Dim SearchTerm As String = ""
                    With cellvalue
                        If .Contains("-") And .Length <> 16 Then
                            SearchTerm = cellvalue + "%"
                        ElseIf .Contains("-") And .Length = 16 Then
                            SearchTerm = cellvalue
                        ElseIf Not .Contains("-") And .Length = 15 Then
                            SearchTerm = cellvalue
                        ElseIf .Equals("") Then
                            'e.Cancel = True
                            Return
                        Else
                            SearchTerm = cellvalue
                        End If
                    End With
                    Using frm As New ProductionSearchForm(SearchTerm, savedretvalue)
                        frm.Owner = Me
                        Dim dr As DialogResult = frm.ShowDialog()
                        If dr = DialogResult.Cancel Then
                            'e.Cancel = True
                            'SendKeys.Send("+{HOME}")
                            'SendKeys.Send("{DEL}")
                            dpdgv2.CancelEdit()
                            dpdgv2.Rows.RemoveAt(e.RowIndex)
                        ElseIf dr = DialogResult.OK Then
                            SearchResults = frm.ResultList
                            dpdgv2.Rows(e.RowIndex).Cells("linestatus").Value = 0
                            dpdgv2.Rows(e.RowIndex).Cells("code").Value = SearchResults.Item("SUBCODE1")
                            dpdgv2.Rows(e.RowIndex).Cells("iteid").Value = SearchResults.Item("ITEID")
                            dpdgv2.Rows(e.RowIndex).Cells("ftrid").Value = SearchResults.Item("FTRID")
                            dpdgv2.Rows(e.RowIndex).Cells("tradecode").Value = SearchResults.Item("TRADECODE")
                            dpdgv2.Rows(e.RowIndex).Cells("partno").Value = SearchResults.Item("PARTNO")
                            dpdgv2.Rows(e.RowIndex).Cells("brand").Value = SearchResults.Item("BRAND")
                            dpdgv2.Rows(e.RowIndex).Cells("quantitylimit").Value = 0
                            dpdgv2.Rows(e.RowIndex).Cells("RowValidated").Value = False
                            If IsNothing(SearchResults.Item("FTRID")) Then
                                dpdgv2.Rows(e.RowIndex).Cells("type").Value = 1
                                dpdgv2.Rows(e.RowIndex).Cells("pertypeid1").Value = SearchResults.Item("ITEID")
                            Else
                                dpdgv2.Rows(e.RowIndex).Cells("type").Value = 0
                                dpdgv2.Rows(e.RowIndex).Cells("pertypeid1").Value = SearchResults.Item("STLID")
                            End If
                            If IsNothing(SearchResults.Item("QUANTITY")) Then
                                dpdgv2.Rows(e.RowIndex).Cells("quantity").Style.BackColor = Color.Yellow
                                dpdgv2.Rows(e.RowIndex).Cells("quantity").Selected = True
                            Else
                                dpdgv2.Rows(e.RowIndex).Cells("quantity").Value = SearchResults.Item("QUANTITY")
                                dpdgv2.Rows(e.RowIndex).Cells("quantitylimit").Value = SearchResults.Item("QUANTITY")
                            End If
                            'e.Cancel = False
                        End If
                    End Using
                Else
                    dpdgv2.CancelEdit()
                    Return
                End If
            ElseIf dpdgv2.Columns(e.ColumnIndex).Name = "quantity" Then
                If Not IsNothing(dpdgv2.Rows(e.RowIndex).Cells("ftrid").Value) AndAlso (IsDBNull(dpdgv2.Rows(e.RowIndex).Cells("quantitylimit").Value) Or IsNothing(dpdgv2.Rows(e.RowIndex).Cells("quantitylimit").Value)) Then
                    Using comm As New SqlCommand("SELECT sum(CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END) FROM  
                                    STORETRADELINES S LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID WHERE S.ID=" + dpdgv2.Rows(e.RowIndex).Cells("pertypeid1").Value.ToString, conn)
                        conn.Open()
                        dpdgv2.Rows(e.RowIndex).Cells("quantitylimit").Value = comm.ExecuteScalar()
                        conn.Close()
                    End Using

                End If
                If Not IsNumeric(dpdgv2.Rows(e.RowIndex).Cells("quantity").Value) Then
                    dpdgv2.Rows(e.RowIndex).Cells("quantity").Value = DBNull.Value
                End If
                If Not (IsDBNull(dpdgv2.Rows(e.RowIndex).Cells("quantity").Value) Or IsNothing(dpdgv2.Rows(e.RowIndex).Cells("quantity").Value)) AndAlso dpdgv2.Rows(e.RowIndex).Cells("quantity").Value > dpdgv2.Rows(e.RowIndex).Cells("quantitylimit").Value And Not IsNothing(dpdgv2.Rows(e.RowIndex).Cells("ftrid").Value) Then
                    dpdgv2.Rows(e.RowIndex).Cells("quantity").Value = dpdgv2.Rows(e.RowIndex).Cells("quantitylimit").Value
                End If
            ElseIf dpdgv2.Columns(e.ColumnIndex).Name = "plandate" Then

                If Not IsDBNull(dpdgv2.Rows(e.RowIndex).Cells("plandate").Value) AndAlso dpdgv2.Rows(e.RowIndex).Cells("plandate").Value < Date.Today Then
                    dpdgv2.Rows(e.RowIndex).Cells("plandate").Value = DBNull.Value
                End If
            End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If dpconn2.State = ConnectionState.Open Then
                dpconn2.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            'dpdgv2.EndEdit()
            'Dim iCol = dpdgv2.CurrentCell.ColumnIndex
            'Dim iRow = dpdgv2.CurrentCell.RowIndex
            'If iCol = dpdgv2.Columns.Count - 1 Then
            '    If iRow < dpdgv2.Rows.Count - 1 Then
            '        dpdgv2.CurrentCell = dpdgv2(8, iRow + 1)
            '    End If
            'Else
            '    If iRow < dpdgv2.Rows.Count - 1 Then
            '        SendKeys.Send("{up}")
            '    End If
            '    dpdgv2.CurrentCell = dpdgv2(iCol + 1, iRow)
            'End If
        End Try
    End Sub


    Private Sub dpdgv2_KeyDown(sender As Object, e As KeyEventArgs) Handles dpdgv2.KeyDown

        'If e.KeyCode = Keys.Enter Then
        '    e.SuppressKeyPress = True
        '    Dim iCol = dpdgv2.CurrentCell.ColumnIndex
        '    Dim iRow = dpdgv2.CurrentCell.RowIndex
        '    If Not dpdgv2.Rows(iRow).

        '    If iCol = dpdgv2.Columns.Count - 1 Then
        '        If Not IsNothing(dpdgv2.Rows(iRow).Cells("quantity").Value) Then

        '            If iRow < dpdgv2.Rows.Count - 1 Then
        '                dpdgv2.CurrentCell = dpdgv2(dpdgv2FirstVisibleColumnIndex, iRow + 1)
        '            End If
        '        Else
        '            dpdgv2.CurrentCell = dpdgv2.Rows(iRow).Cells("quantity")
        '        End If
        '    Else
        '        dpdgv2.CurrentCell = dpdgv2(iCol + 1, iRow)
        '    End If
        'End If
    End Sub

    Private Sub dpdgv2_RowValidating(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dpdgv2.RowValidating
        If Not IsNothing(dpdgv2.Rows(e.RowIndex).Cells("code").Value) AndAlso (IsNothing(dpdgv2.Rows(e.RowIndex).Cells("quantity").Value) Or IsDBNull(dpdgv2.Rows(e.RowIndex).Cells("quantity").Value)) Then
            dpdgv2.CurrentCell = dpdgv2.Rows(e.RowIndex).Cells("quantity")
            e.Cancel = True
        End If
        If Not IsNothing(dpdgv2.Rows(e.RowIndex).Cells("code").Value) AndAlso (IsNothing(dpdgv2.Rows(e.RowIndex).Cells("plandate").Value) Or IsDBNull(dpdgv2.Rows(e.RowIndex).Cells("plandate").Value)) Then
            dpdgv2.CurrentCell = dpdgv2.Rows(e.RowIndex).Cells("plandate")
            e.Cancel = True
        End If
    End Sub

    Private Sub TextBox1_MouseClick(sender As Object, e As MouseEventArgs) Handles Textbox1.MouseClick
        Dim frm As New CalendarSelector(FirstMonday, Cursor.Position.X - 355, Cursor.Position.Y - 20)
        frm.Owner = Me
        frm.Show()
    End Sub

    Public Sub change_date(ByVal d As Integer, year As Integer)
        Dim startDate As New DateTime(year, 1, 1)
        Dim weekDate As DateTime = DateAdd(DateInterval.WeekOfYear, d - 1, startDate)
        Dim monday As Date = DateAdd(DateInterval.Day, (-weekDate.DayOfWeek) + 1, weekDate)
        FirstMonday = monday
        TabControl1.SelectedIndex = 1
        Textbox1.Text = monday.ToShortDateString.ToString + "-" + monday.AddDays(6).ToShortDateString.ToString
        If Not dpworker2.IsBusy Then
            dp2work()
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        FirstMonday = FirstMonday.AddDays(-7)
        Textbox1.Text = FirstMonday.ToShortDateString.ToString + "-" + FirstMonday.AddDays(6).ToShortDateString.ToString
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        FirstMonday = FirstMonday.AddDays(7)
        Textbox1.Text = FirstMonday.ToShortDateString.ToString + "-" + FirstMonday.AddDays(6).ToShortDateString.ToString
    End Sub

    Private Sub Textbox1_TextChanged(sender As Object, e As EventArgs) Handles Textbox1.TextChanged
        If NotFirstLoad Then
            dp2work()
        End If
    End Sub

    Private Sub dpdgv2_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv2.CellValueChanged
        If dpdgv2.Columns(e.ColumnIndex).Name = "plandate" Then
            If Not IsDBNull(dpdgv2.Rows(e.RowIndex).Cells("plandate").Value) Then
                Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(dpdgv2.Rows(e.RowIndex).Cells("plandate").Value)
                Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
                dpdgv2.Rows(e.RowIndex).Cells("day").Value = dayName
            Else
                dpdgv2.Rows(e.RowIndex).Cells("day").Value = DBNull.Value
            End If
        End If
        If e.RowIndex >= 0 Then
            dpdgv2.Rows(e.RowIndex).Cells("RowValidated").Value = False
        End If
    End Sub

    Private Sub dpdgv2_RowValidated(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv2.RowValidated
        If e.RowIndex <> dpdgv2.NewRowIndex AndAlso Not dpdgv2.Rows(e.RowIndex).Cells("RowValidated").Value AndAlso (dpdgv2.Rows(e.RowIndex).Cells("linestatus").Value = 0 Or dpdgv2.Rows(e.RowIndex).Cells("linestatus").Value = 1) And Not IsNothing(dpdgv2.Rows(e.RowIndex).Cells("code").Value) Then
            dpdgv2.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightYellow
            dpdgv2.Rows(e.RowIndex).Cells("quantity").Style.BackColor = Color.LightYellow
            dpdgv2.Rows(e.RowIndex).Cells("RowValidated").Value = True
            If dpdgv2.Rows(e.RowIndex).Cells("linestatus").Value = 1 Then
                dpdgv2.Rows(e.RowIndex).Cells("linestatus").Value = 2
            End If
        End If
        validatedrows.Add(dpdgv2.Rows(e.RowIndex))
    End Sub

    Dim validatedrows As New List(Of DataGridViewRow)

    Private Sub production_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            If dpdgv2.IsCurrentRowDirty And Not validatedrows.Contains(dpdgv2.CurrentRow) Then
                dpdgv2.Rows.Remove(dpdgv2.CurrentRow)
            End If
        End If
        e.Handled = False
    End Sub

    Private Sub Button8_Click_2(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            For Each r As DataGridViewRow In dpdgv2.Rows
                If Not r.Index = dpdgv2.NewRowIndex And Not r.DefaultCellStyle.BackColor = Color.LightGray Then
                    If r.Cells("linestatus").Value = 0 Then
                        If r.Cells("type").Value = 0 Then

                            Using c As New SqlCommand("select pertypeid1, sum(isnull(dp.quantity,0)) dpq, sum(isnull(s.primaryqty,0)) sq from pkrtbl_dailyplan dp inner join storetradelines s on dp.type=0 and dp.pertypeid1=s.id where type=0 and pertypeid1=" + r.Cells("pertypeid1").Value.ToString + " group by pertypeid1", conn)
                                conn.Open()
                                Using d As New DataTable()
                                    Using READER As SqlDataReader = c.ExecuteReader
                                        d.Load(READER)
                                    End Using
                                    If d.Rows.Count > 0 AndAlso d.Rows(0).Item("dpq") + r.Cells("quantity").Value > d.Rows(0).Item("sq") Then
                                        dpdgv2.Rows(r.Index).DefaultCellStyle.BackColor = Color.Red
                                        Throw New Exception("Η επιλεγμένη κίνηση θα οδηγούσε σε υπέρβαση ποσότητας της παραγγελίας! Γραμμή " + (r.Index + 1).ToString)
                                    End If
                                End Using
                                conn.Close()
                            End Using


                        End If
                        Dim cmd As String = "INSERT INTO PKRTBL_DAILYPLAN (TYPE,PERTYPEID1,QUANTITY,DATE,REMARKS,PERTYPEID2,LASTEDITUSERID,WEEKNUMBER) VALUES (@type,@pertypeid1,@quantity,@date,@remarks,@pertypeid2,@userid," + weekOfYear.ToString + ")"
                        Using sc As New SqlCommand(cmd, updconn)
                            sc.Parameters.Add("@type", SqlDbType.Int).Value = r.Cells("type").Value
                            sc.Parameters.Add("@pertypeid1", SqlDbType.Int).Value = r.Cells("pertypeid1").Value
                            sc.Parameters.Add("@quantity", SqlDbType.Float).Value = r.Cells("quantity").Value
                            sc.Parameters.Add("@date", SqlDbType.Date).Value = r.Cells("plandate").Value
                            sc.Parameters.Add("@userid", SqlDbType.Int).Value = Form1.activeuserid
                            If (IsNothing(r.Cells("remarks").Value)) Then
                                sc.Parameters.Add("@remarks", SqlDbType.Text).Value = DBNull.Value
                            Else
                                sc.Parameters.Add("@remarks", SqlDbType.Text).Value = r.Cells("remarks").Value
                            End If
                            If (IsNothing(r.Cells("pertypeid2").Value)) Then
                                sc.Parameters.Add("@pertypeid2", SqlDbType.Int).Value = DBNull.Value
                            Else
                                sc.Parameters.Add("@pertypeid2", SqlDbType.Int).Value = r.Cells("pertypeid2").Value
                            End If
                            updconn.Open()
                            Dim res As Integer = -1
                            res = sc.ExecuteNonQuery()
                            updconn.Close()
                            If res > 0 Then
                                Dim TransTypePerDPType As New Dictionary(Of Integer, Integer) From {{0, 36}, {1, 37}}  ' 0 παραγωγή 1 απόθεμα
                                Using ut As New PackerUserTransaction
                                    ut.WriteEntry(Form1.activeuserid, TransTypePerDPType(sc.Parameters("@type").Value), sc.Parameters("@pertypeid1").Value, value:="ΗΜΝΙΑ/ΣΧΟΛΙΑ:" + r.Cells("plandate").Value.ToString + "/" + r.Cells("remarks").Value, q:=sc.Parameters("@quantity").Value)
                                End Using
                            End If
                        End Using
                    ElseIf r.Cells("linestatus").Value = 2 Then

                        Using c As New SqlCommand("select pertypeid1, sum(isnull(dp.quantity,0)) dpq, sum(isnull(s.primaryqty,0)) sq from pkrtbl_dailyplan dp inner join storetradelines s on dp.type=0 and dp.pertypeid1=s.id where type=0 and pertypeid1=" + r.Cells("pertypeid1").Value + " and id<>" + r.Cells("dpid").Value + " group by pertypeid1", conn)
                            conn.Open()
                            Using d As New DataTable()
                                Using READER As SqlDataReader = c.ExecuteReader
                                    d.Load(READER)
                                End Using
                                If d.Rows.Count > 0 AndAlso d.Rows(0).Item("dpq") + r.Cells("quantity").Value > d.Rows(0).Item("sq") Then
                                    dpdgv2.Rows(r.Index).DefaultCellStyle.BackColor = Color.Red
                                    Throw New Exception("Η επιλεγμένη κίνηση θα οδηγούσε σε υπέρβαση ποσότητας της παραγγελίας! Γραμμή " + (r.Index + 1).ToString)
                                End If
                            End Using
                            conn.Close()
                        End Using

                        Dim cmd As String = "UPDATE PKRTBL_DAILYPLAN SET TYPE=@type,PERTYPEID1=@pertypeid1,QUANTITY=@quantity,DATE=@date,REMARKS=@remarks,PERTYPEID2=@pertypeid2,LASTEDITUSERID=@userid,WEEKNUMBER=" + weekOfYear.ToString + " WHERE ID=@ID"
                        Using sc As New SqlCommand(cmd, updconn)
                            sc.Parameters.Add("@ID", SqlDbType.Int).Value = r.Cells("dpid").Value
                            sc.Parameters.Add("@type", SqlDbType.Int).Value = r.Cells("type").Value
                            sc.Parameters.Add("@pertypeid1", SqlDbType.Int).Value = r.Cells("pertypeid1").Value
                            sc.Parameters.Add("@quantity", SqlDbType.Float).Value = r.Cells("quantity").Value
                            sc.Parameters.Add("@date", SqlDbType.Date).Value = r.Cells("plandate").Value
                            sc.Parameters.Add("@userid", SqlDbType.Int).Value = Form1.activeuserid
                            If (IsNothing(r.Cells("remarks").Value)) Then
                                sc.Parameters.Add("@remarks", SqlDbType.Text).Value = DBNull.Value
                            Else
                                sc.Parameters.Add("@remarks", SqlDbType.Text).Value = r.Cells("remarks").Value
                            End If
                            If (IsNothing(r.Cells("pertypeid2").Value)) Then
                                sc.Parameters.Add("@pertypeid2", SqlDbType.Int).Value = DBNull.Value
                            Else
                                sc.Parameters.Add("@pertypeid2", SqlDbType.Int).Value = r.Cells("pertypeid2").Value
                            End If
                            updconn.Open()
                            Dim res As Integer = -1
                            res = sc.ExecuteNonQuery()
                            updconn.Close()
                            If res > 0 Then
                                Dim TransTypePerDPType As New Dictionary(Of Integer, Integer) From {{0, 36}, {1, 37}}  ' 0 παραγωγή 1 απόθεμα
                                Using ut As New PackerUserTransaction
                                    ut.WriteEntry(Form1.activeuserid, TransTypePerDPType(sc.Parameters("@type").Value), sc.Parameters("@pertypeid1").Value, value:="ΗΜΝΙΑ/ΣΧΟΛΙΑ:" + r.Cells("plandate").Value.ToString + "/" + r.Cells("remarks").Value, q:=sc.Parameters("@quantity").Value)
                                End Using
                            End If
                        End Using
                    ElseIf r.Cells("linestatus").Value = -1 Then

                        Using transaction = TransactionUtils.CreateTransactionScope()
                            Dim res As Integer = 0
                            Using s As New SqlCommand("update tbl_palletlines set dailyplanid=null where dailyplanid=@id", updconn)
                                s.Parameters.Add("@id", SqlDbType.Int).Value = r.Cells("dpid").Value
                                updconn.Open()
                                res = s.ExecuteNonQuery()
                                updconn.Close()
                            End Using
                            res = -1
                            Using sc As New SqlCommand("delete from PKRTBL_DAILYPLAN WHERE ID=@ID", updconn)
                                sc.Parameters.Add("@ID", SqlDbType.Int).Value = r.Cells("dpid").Value
                                updconn.Open()
                                res = sc.ExecuteNonQuery()
                                updconn.Close()
                            End Using
                            If res > 0 Then
                                Using ut As New PackerUserTransaction
                                    ut.WriteEntry(Form1.activeuserid, 38, r.Cells("dpid").Value, r.Cells("code").Value + "/" + r.Cells("plandate").Value + "/" + r.Cells("quantity").Value.ToString)
                                End Using
                            End If
                            transaction.Complete()
                        End Using
                    End If
                End If
            Next
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            If dpconn2.State = ConnectionState.Open Then
                dpconn2.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            dp2work()
        End Try
    End Sub

    Dim DeletedRows As New Dictionary(Of Integer, Integer)

    Private Sub dpdgv2_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles dpdgv2.UserDeletingRow
        If e.Row.ReadOnly Then
            e.Cancel = True
            Return
        End If
        If (e.Row.Cells("linestatus").Value = 1 Or e.Row.Cells("linestatus").Value = 2) And Not DeletedRows.ContainsKey(e.Row.Cells("dpid").Value) Then
            DeletedRows.Add(e.Row.Cells("dpid").Value, e.Row.Cells("linestatus").Value)
            e.Row.Cells("linestatus").Value = -1
            e.Row.DefaultCellStyle.BackColor = Color.LightCoral
        ElseIf e.Row.Cells("linestatus").Value = -1 Then
            e.Row.Cells("linestatus").Value = DeletedRows(e.Row.Cells("dpid").Value)
            If DeletedRows(e.Row.Cells("dpid").Value) = 2 Then
                e.Row.DefaultCellStyle.BackColor = Color.LightYellow
            ElseIf DeletedRows(e.Row.Cells("dpid").Value) = 1 Then
                e.Row.DefaultCellStyle.BackColor = Color.White
            End If
            DeletedRows.Remove(e.Row.Cells("dpid").Value)
        End If
        e.Cancel = True
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim today As Date = Date.Today
        If today.DayOfWeek = DayOfWeek.Sunday Then today = today.AddDays(-1)
        Dim monDate As DateTime = today.AddDays(DayOfWeek.Monday - today.DayOfWeek)
        FirstMonday = monDate
        Textbox1.Text = monDate.ToShortDateString.ToString + "-" + monDate.AddDays(6).ToShortDateString.ToString
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        dp2work()
    End Sub

    Private Sub dpdgv2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dpdgv2.CellClick
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then
            If dpdgv2.Columns(e.ColumnIndex).Name = "tradecode" And e.RowIndex >= 0 And e.RowIndex <> dpdgv2.NewRowIndex Then
                Using f As New Order(dpdgv2.Rows(e.RowIndex).Cells("ftrid").Value)
                    f.ShowDialog()
                End Using
            ElseIf dpdgv2.Columns(e.ColumnIndex).Name = "code" And e.RowIndex >= 0 And e.RowIndex <> dpdgv2.NewRowIndex Then

                Using f As New ItemDetails(dpdgv2.Rows(e.RowIndex).Cells("iteid").Value)
                    f.ShowDialog()
                End Using
            ElseIf dpdgv2.Columns(e.ColumnIndex).Name = "pallets" And e.RowIndex >= 0 And e.RowIndex <> dpdgv2.NewRowIndex Then
                Dim f As New ProductionDailyPlanQuickPalletPlan(dpdgv2.Rows(e.RowIndex).Cells("dpid").Value, dpdgv2.Rows(e.RowIndex).Cells("pertypeid1").Value, dpdgv2.Rows(e.RowIndex).Cells("type").Value, Cursor.Position.X, Cursor.Position.Y, e.RowIndex, ite_id:=dpdgv2.Rows(e.RowIndex).Cells("iteid").Value, ftr_id:=dpdgv2.Rows(e.RowIndex).Cells("ftrid").Value)
                f.Owner = Me
                f.Show()

            End If
        End If
    End Sub

    Private Sub Textbox1_Click(sender As Object, e As EventArgs) Handles Textbox1.Click

    End Sub

    Private Sub dpdgv2_SelectionChanged(sender As Object, e As EventArgs) Handles dpdgv2.SelectionChanged

    End Sub

    Private Sub notificationsworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles notificationsworker.DoWork
        Using s As New SqlCommand("select palletid as id,ftrid,stlid,cusid,ph.code,cast(pl.q as varchar(10))+'x'+m.subcode1 itecode,pu.username,opendate,dailyplanid from (select row_number() over (partition by palletid order by palletid asc) rn ,palletid,stlid,ftrid,max(quantity) q,iteid,dailyplanid from tbl_palletlines group by palletid,stlid,ftrid,iteid,dailyplanid) pl left join TBL_PALLETHEADERS ph on ph.id=pl.PALLETID and pl.rn=1 left join tbl_packeruserdata pu on pu.id=ph.createuser left join material m on m.id=pl.iteid where status in (-1,-2) and DAILYPLANID is null", nwconn)
            Using dt As New DataTable()
                nwconn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                End Using
                nwconn.Close()
                e.Result = dt
            End Using
        End Using

    End Sub

    Private Sub notificationsworker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles notificationsworker2.DoWork
        Using s As New SqlCommand("select distinct dp.id,weeknumber,year(date) year from PKRTBL_DAILYPLAN DP left join TBL_PALLETLINES pl on pl.DAILYPLANID=dp.ID
group by dp.id,weeknumber,date,dp.quantity
having sum(isnull(pl.quantity,0))<>dp.quantity
", nw2conn)
            Using dt As New DataTable()
                nw2conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                End Using
                nw2conn.Close()
                e.Result = dt
            End Using
        End Using

    End Sub

    Private Sub notificationsworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles notificationsworker.RunWorkerCompleted, notificationsworker2.RunWorkerCompleted
        If Not IsNothing(e.Result) Then
            Dim dt As DataTable = TryCast(e.Result, DataTable)
            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    Dim ctrl As System.ComponentModel.BackgroundWorker = TryCast(sender, System.ComponentModel.BackgroundWorker)
                    If Not IsNothing(ctrl) AndAlso ctrl Is notificationsworker Then
                        InformationPanel1.addwarning(1, "Υπάρχουν παλέτες σε φάση σχεδιασμού που δεν έχουν αντιστοιχισθεί με daily plan!", link:=True, data:=dt)
                        TableLayoutPanel1.RowStyles(1).Height = InformationPanel1.GetRequiredHeight
                    ElseIf Not IsNothing(ctrl) AndAlso ctrl Is notificationsworker2 Then
                        InformationPanel1.addwarning(1, "Υπάρχουν Daily Plans χωρίς αντιστοιχισμένες παλέτες!", link:=True, data:=dt)
                        TableLayoutPanel1.RowStyles(1).Height = InformationPanel1.GetRequiredHeight
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub PackingList_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        TableLayoutPanel1.RowStyles(1).Height = InformationPanel1.GetRequiredHeight
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        '(FIELD_TITLE,(RELATIONSHIP1FOREIGNTABLE,R1FT_FKEY,R1FT_PKEY,RELATIONSHIP2FOREIGNTABLE,R2FT_FKEY,R2PT_PKEY,WHEREFIELD,EXTRAWHERE,SELECTION))
        Dim list As New Dictionary(Of String, List(Of String))
        list.Add("Είδος σε ΠΑΡ", New List(Of String) From {"STORETRADELINES", "ID", "PERTYPEID1", "MATERIAL", "ID", "STORETRADELINES.ITEID", "DESCR2", "AND TYPE=0 ORDER BY DATE DESC", "PKRTBL_DAILYPLAN.ID,MATERIAL.SUBCODE1,MATERIAL.M_PARTNO,WEEKNUMBER,YEAR(DATE) YEAR"})
        list.Add("Παραγγελία", New List(Of String) From {"STORETRADELINES", "ID", "PERTYPEID1", "", "", "", "DBO.GET_TRADECODE(STORETRADELINES.FTRID)", "", "PKRTBL_DAILYPLAN.ID,DBO.GET_TRADECODE(STORETRADELINES.FTRID),WEEKNUMBER,YEAR(DATE) YEAR"})
        list.Add("Είδος σε απόθεμα", New List(Of String) From {"MATERIAL", "ID", "PERTYPEID1", "", "", "", "DESCR2", "AND TYPE=1 ORDER BY DATE DESC", "PKRTBL_DAILYPLAN.ID,MATERIAL.SUBCODE1,MATERIAL.M_PARTNO,WEEKNUMBER,YEAR(DATE) YEAR"})
        list.Add("Παλέτα", New List(Of String) From {"TBL_PALLETLINES", "DAILYPLANID", "ID", "TBL_PALLETHEADERS", "ID", "TBL_PALLETLINES.PALLETID", "CODE", " ORDER BY DATE DESC", "PKRTBL_DAILYPLAN.ID,TBL_PALLETHEADERS.CODE,WEEKNUMBER,YEAR(DATE) YEAR"})
        Dim f As New SmallSearchBox(MousePosition.X, MousePosition.Y, "PKRTBL_DAILYPLAN", list)
        f.Owner = Me
        f.Show()
    End Sub

    Dim requested_highlight As Integer = 0
    Public Sub highlight(ByVal dpid As Integer)
        requested_highlight = dpid
    End Sub

    Private Sub stockworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles stockworker.DoWork
        Dim cmd As String = "SELECT PH.ID palletid,ph.code,ph.orders,ph.cusid,cast(pl.q as varchar(10))+'x'+m.subcode1+'-ORGNL/WVA:'+m.M_INDEX+'-BRAND:'+MNF.DESCR item  FROM TBL_PALLETHEADERS PH LEFT JOIN (select row_number() over (partition by palletid order by palletid asc) rn ,palletid,max(quantity) q,iteid from tbl_palletlines group by palletid,iteid ) pl
on ph.id=pl.PALLETID and rn=1  LEFT JOIN MATERIAL M ON M.ID=PL.ITEID LEFT JOIN TBL_PACKERUSERDATA PU ON PU.ID=PH.CREATEUSER left join manufacturer mnf on mnf.codeid=m.mnfid where isnull(ph.isstock,0)=1"
        Try
            Dim dt As New DataTable()
            Using S As New SqlCommand(cmd, stconn)
                stconn.Open()
                Using reader As SqlDataReader = S.ExecuteReader
                    dt.Load(reader)
                End Using
                stconn.Close()
            End Using
            e.Result = dt
        Catch ex As Exception
            If stconn.State = ConnectionState.Open Then
                stconn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub stockworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles stockworker.RunWorkerCompleted
        stockdgv.DataSource = e.Result
        stockdgv.Visible = True
        stockdgv.Refresh()
        For Each c As Control In DoubleBufferedTableLayoutPanel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        stockwork()
    End Sub

    Private Sub stockdgv_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles stockdgv.CellDoubleClick
        If e.RowIndex >= 0 Then
            Using f As New PalletDetails(stockdgv.Rows(e.RowIndex).Cells("palletid").Value)
                f.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub stockdgv_SelectionChanged(sender As Object, e As EventArgs) Handles stockdgv.SelectionChanged
        If stockdgv.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In stockdgv.SelectedRows
                If row.Index >= 0 Then
                    Button16.Visible = True
                Else
                    Button16.Visible = False
                End If
            Next
        Else
            Button16.Visible = False
        End If
    End Sub

    Public selectedPalletid As Integer = 0
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Try
            If stockdgv.SelectedRows.Count > 0 AndAlso stockdgv.SelectedRows(0).Index > 0 Then
                Dim dt As New DataTable()
                Using s As New SqlCommand("select distinct f.id as ftrid,c.FATHERNAME,dbo.get_tradecode(f.id) TRADECODE from fintrade f inner join tbl_packerordercheck t on t.ftrid=f.id inner join customer c on c.id=f.cusid inner join storetradelines s on s.ftrid=f.id
                where t.status<12 and s.iteid in (select iteid from tbl_palletlines where palletid=" + stockdgv.SelectedRows(0).Cells("palletid").Value.ToString + ")", conn)
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                        conn.Close()
                    End Using
                End Using
                selectedPalletid = stockdgv.SelectedRows(0).Cells("palletid").Value
                Dim f As New InformationPanelGenericDialog("Διαθέσιμα ΠΑΡ:", dt, MousePosition.X, MousePosition.Y)
                f.Owner = Me
                f.Show()
            End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub dpdgv2_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dpdgv2.DataBindingComplete
        dpdgv2.ClearSelection()
        dpdgv2.CurrentCell = Nothing
        dpdgv2.Rows(0).Selected = False
    End Sub

    Private Sub CustomDGVSearchBox2_ButtonPressed() Handles CustomDGVSearchBox2.ButtonPressed
        Dim dic As New Dictionary(Of String, String)
        dic.Add("Param1", savedretvalue)
        dic.Add("Param3", lvl)
        If savedtype = 0 Then
            dic.Add("Param2", "select codeid from FLDCUSTBL1 where PKRTBL_ITEMTYPESID=1")
        ElseIf savedtype = 1 Then
            dic.Add("Param2", "select codeid from FLDCUSTBL1 where PKRTBL_ITEMTYPESID=2")
        ElseIf savedtype = 3 Then
            dic.Add("Param2", "select codeid from FLDCUSTBL1 where PKRTBL_ITEMTYPESID=3")
        ElseIf savedtype = 4 Then
            dic.Add("Param2", "'" + category + "'")
        End If
        CustomDGVSearchBox2.custom_parameters = dic

        CustomDGVSearchBox2.continue_click()
    End Sub

    Private Sub CustomDGVSearchBox2_Load(sender As Object, e As EventArgs) Handles CustomDGVSearchBox2.Load

    End Sub

    Private Sub CustomDGVSearchBox3_ButtonPressed() Handles CustomDGVSearchBox3.ButtonPressed
        Dim dic As New Dictionary(Of String, String)
        dic.Add("Param1", weekOfYear)
        CustomDGVSearchBox3.custom_parameters = dic
        CustomDGVSearchBox3.continue_click()
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Dim EXTRA As String = ""
        If savedtype = 0 Then
            EXTRA = " AND left(code,3) in ('102','202') "
        ElseIf savedtype = 1 Then
            EXTRA = " AND left(code,1)='1' and left(code,3)<>'102' "
        ElseIf savedtype = 3 Then
            EXTRA = " AND left(code,1)='2' and left(code,3)<>'202' "
        ElseIf savedtype = 4 Then
            EXTRA = " AND left(code,1)='" + category + "' "
        End If
        Dim f As New productionWeekSum(Cursor.Position.X, Cursor.Position.Y, weekOfYear, FirstMonday.Year, EXTRA)
        f.Owner = Me
        f.Show()
    End Sub

    'Private Sub DataGridView1_SortStringChanged(sender As Object, e As EventArgs) Handles dpdgv1.SortStringChanged
    '    dpdgv1.DataSource.Sort = dpdgv1.SortString
    '    'textBox_sort.Text = bindingSource_main.Sort
    'End Sub

    'Private Sub DataGridView1_FilterStringChanged(sender As Object, e As EventArgs) Handles dpdgv1.FilterStringChanged
    '    dpdgv1.DataSource.Filter = dpdgv1.FilterString

    '    'textBox_filter.Text = bindingSource_main.Filter
    'End Sub

    'Private Sub DataGridView2_SortStringChanged(sender As Object, e As EventArgs) Handles dpdgv2.SortStringChanged
    '    dpdgv2.DataSource.Sort = dpdgv2.SortString
    '    'textBox_sort.Text = bindingSource_main.Sort
    'End Sub

    'Private Sub DataGridView2_FilterStringChanged(sender As Object, e As EventArgs) Handles dpdgv2.FilterStringChanged
    '    dpdgv2.DataSource.Filter = dpdgv2.FilterString

    '    'textBox_filter.Text = bindingSource_main.Filter
    'End Sub

    Private Sub DataGridView2_FilterStringChanged(sender As Object, e As EventArgs) Handles dpdgv2.FilterStringChanged
        Try
            TryCast(dpdgv1.DataSource, DataTable).DefaultView.RowFilter = dpdgv2.FilterString
        Catch
        End Try
    End Sub

    Private Sub DataGridView2_SortStringChanged(sender As Object, e As EventArgs) Handles dpdgv2.SortStringChanged
        Try
            TryCast(dpdgv1.DataSource, DataTable).DefaultView.Sort = dpdgv2.SortString
        Catch
        End Try
    End Sub
    Private Sub DataGridView1_FilterStringChanged(sender As Object, e As EventArgs) Handles dpdgv1.FilterStringChanged
        Try
            TryCast(dpdgv1.DataSource, DataTable).DefaultView.RowFilter = dpdgv1.FilterString
        Catch
        End Try
    End Sub

    Private Sub DataGridView1_SortStringChanged(sender As Object, e As EventArgs) Handles dpdgv1.SortStringChanged
        Try
            TryCast(dpdgv1.DataSource, DataTable).DefaultView.Sort = dpdgv1.SortString
        Catch
        End Try
    End Sub

    Private Sub DataGridView1_FilterStringChanged_1(sender As Object, e As EventArgs) Handles DataGridView1.FilterStringChanged
        Try
            TryCast(DataGridView1.DataSource, DataTable).DefaultView.RowFilter = DataGridView1.FilterString
        Catch
        End Try
    End Sub

    Private Sub stockdgv_FilterStringChanged(sender As Object, e As EventArgs) Handles stockdgv.FilterStringChanged
        Try
            TryCast(stockdgv.DataSource, DataTable).DefaultView.RowFilter = stockdgv.FilterString
        Catch
        End Try
    End Sub

    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        Try
            For Each c As DataGridViewColumn In DataGridView1.Columns
                c.Visible = True
                Dim count As Integer = 0
                For Each r As DataGridViewRow In DataGridView1.Rows
                    If IsDBNull(r.Cells(c.Index).Value) Then
                        count += 1
                    End If
                Next
                If count = DataGridView1.Rows.Count Then
                    c.Visible = False
                End If
            Next
            color_comments()
        Catch
        End Try
    End Sub

    Private Sub DataGridView1_SortStringChanged_1(sender As Object, e As EventArgs) Handles DataGridView1.SortStringChanged
        Try
            TryCast(DataGridView1.DataSource, DataTable).DefaultView.Sort = DataGridView1.SortString
        Catch
        End Try
    End Sub

    Private Sub stockdgv_SortStringChanged(sender As Object, e As EventArgs) Handles stockdgv.SortStringChanged
        Try
            TryCast(stockdgv.DataSource, DataTable).DefaultView.Sort = stockdgv.SortString
        Catch
        End Try
    End Sub

    Private Sub dpdgv2_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dpdgv2.RowsAdded

    End Sub

    Private Sub CustomDGVSearchBox4_ButtonPressed() Handles CustomDGVSearchBox4.ButtonPressed
        CustomDGVSearchBox4.continue_click()
    End Sub

    Private Sub CustomDGVSearchBox5_ButtonPressed() Handles CustomDGVSearchBox5.ButtonPressed
        CustomDGVSearchBox5.continue_click()
    End Sub
End Class