Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Deployment.Application
Imports System.Reflection
Imports System.Threading
Imports Microsoft.Reporting.WinForms

Public Class Form1

    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim checkdate As Date
    Public activeuser As String, activeuserid As Integer, activeuserdpt As String, activeuserocu As Integer, activeuseraid As Integer, activeuseritemtypeid As Integer, rlimit As Integer, ractive As Integer = 1, pheight As Double = 88, plength As Double = 120, pwidth As Double = 80,
        rowtodistribute As DataGridViewRow, counter As Integer, indexes As ArrayList = New ArrayList(), selectedorder As String, orderraw As Integer, logo As Boolean = False, plfilter As String = "", pfilter As String = "", salesmanfilter As String = "",
        plcdfilterfrom As DateTime? = Nothing, plcdfilterto As DateTime? = Nothing, plodfilterfrom As DateTime? = Nothing, plodfilterto As DateTime? = Nothing, pcdfilterfrom As DateTime? = Nothing, pcdfilterto As DateTime? = Nothing, podfilterfrom As DateTime? = Nothing, podfilterto As DateTime? = Nothing,
         FTRID As Integer, DSRNUMBER As String, CUSTOMER As String, form8type As Integer, page As Integer = 0, lastcode As String = "", pallettoprint As String, rtime As Integer = 0, plistid As Integer, plistcode As String, dgv1rowindex As Integer,
         dgvsender As Object, dgve As DataGridViewCellEventArgs, dragging As Boolean = False, palletsforlabels As String = "0", dgv1columns As New ArrayList(), VIEWSTYLE As String, CUSID As Integer, relsalesman As Integer,
         UserAccess As New DataTable(), DptAccess As New DataTable(), activeuserdptid As Integer, allsalesmans As List(Of String)
    Public settings As New List(Of settingsitem), pallettypes As New DataTable()
    Dim sett As New settings_exchange
    Public osversion As Version, phdt As New DataTable, pldt As New DataTable, usernames As Object, departments As Object, nextpinnedlocation As Integer = 0,
        palletdeforder As New Dictionary(Of Integer, Integer), palletpinorder As New Dictionary(Of Integer, Integer), pindex As New Dictionary(Of Integer, String)
    Dim palnum As Int16 = 1, first_visible_index As Integer, last_visible_index As Integer, selectedstlids As ArrayList = New ArrayList(), updaterstring As String,
        updaterstring2 As String, b1s As Object, b1e As EventArgs, dgv1cmd As New SqlCommand, dgv2cmd As String = "", dgv3cmd As String = "", orderdgvcmd As String = "", relftrids As String = "", customerfn As String, clickedorder As Object,
        ORDERTEMPCODE As Boolean = False, mouseX As Integer, mouseY As Integer, info As DataGridView.HitTestInfo, clicker As Integer, cur As Cursor, SetSortOrderorderdgv As ListSortDirection, SetSortOrderdgv1 As ListSortDirection, SetSortOrderdgv2 As ListSortDirection, nodistribute As Boolean = False,
        iteidcolindex As Integer, stlidcolindex As Integer, ftridcolindex As Integer, startquantcolindex As Integer, codecolindex As Integer, reqquantcolindex As Integer, namecolindex As Integer,
        wcindex As Integer = -1, pcindex As Integer = -1, dgvfontsize As Single = 9.0F, hidedispatch As String = "false", palletlimit As Integer = 150
    Dim oldcursor As Cursor
    Dim splashscreen As New SplashScreen1
    Private MouseDownPos As Point
    Dim comboSource As New Dictionary(Of String, String)()
    Dim itemtypes As New DataTable()
    Dim ReportBoot As Boolean = True
    Dim AdvancedLogin As Boolean = True

    Public boldfont As Font = New Drawing.Font("Arial",
                               11,
                               FontStyle.Bold)
    Public sfont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Public webdf = New System.Drawing.Font("Webdings", 15.0!)
    Public wingf = New System.Drawing.Font("Wingdings", 14.0!, System.Drawing.FontStyle.Bold)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Keyboard.ShiftKeyDown Then
            ReportBoot = False
        End If
        If My.Computer.Keyboard.AltKeyDown Then
            AdvancedLogin = False
        End If
        My.Settings.LoadingComplete = False
        My.Settings.Save()
        If ReportBoot Then
            splashscreen.Show()
        End If
        continue_load()
    End Sub

    Public ReadOnly Property item_types As DataTable
        Get
            Return itemtypes
        End Get
    End Property


    Public ReadOnly Property pallet_types As DataTable
        Get
            Return pallettypes
        End Get
    End Property
    Private Sub continue_load()
        GetType(FlowLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, FlowLayoutPanel1, New Object() {True})
        GetType(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, DataGridView1, New Object() {True})
        GetType(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, orderdgv, New Object() {True})
        GetType(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, DataGridView2, New Object() {True})
        ' Label1.Text = ""
        Dim exePath As String = Application.ExecutablePath()
        My.Settings.downloading = True
        If My.Settings.isFirstRun = True Then
            My.Settings.isFirstRun = False
            My.Settings.Save()
        End If
        osversion = Environment.OSVersion.Version
        Dim _assemblyInfo As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        If System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed Then
            If System.Diagnostics.Debugger.IsAttached Then
                Me.Text = "Grantex Packer® Debug Mode"
                NotifyIcon1.BalloonTipText = "Debug Mode" + Environment.NewLine + conn.DataSource.ToString + ", " + conn.Database.ToString + Environment.NewLine + Environment.MachineName.ToString
                checkdate = Convert.ToDateTime("2020-09-01")
            Else
                Me.Text = "Grantex Packer® " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                NotifyIcon1.BalloonTipText = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString() + Environment.NewLine + conn.DataSource.ToString + ", " + conn.Database.ToString + Environment.NewLine + Environment.MachineName.ToString
                checkdate = Today
            End If
        Else
            If Not IsNothing(_assemblyInfo) Then
                If System.Diagnostics.Debugger.IsAttached Then
                    Me.Text = "Grantex Packer® Debug Mode"
                    NotifyIcon1.BalloonTipText = "Debug Mode" + Environment.NewLine + conn.DataSource.ToString + ", " + conn.Database.ToString + Environment.NewLine + Environment.MachineName.ToString
                    checkdate = Convert.ToDateTime("2020-09-01")
                    'activeuserdpt = "SA"
                    'activeuser = "SUPERVISOR"
                    'activeuserid = 1
                    'activeuserocu = 1
                    'activeuserdpt = "BL"
                    'activeuser = "MANOLOPOULOS"
                    'activeuserid = 9
                    'activeuserocu = 0
                    'activeuserdptid = 5
                    'activeuserdpt = "EX"
                    'activeuser = "NATASSA"
                    'activeuserid = 3
                    'activeuserocu = 0
                    'activeuseraid = 15
                    'activeuserdpt = "SP"
                    'activeuser = "PHILIPPOU"
                    'activeuserid = 10
                    'activeuserocu = 0
                Else
                    Me.Text = "Grantex Packer® " + _assemblyInfo.GetName().Version.ToString()
                    NotifyIcon1.BalloonTipText = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString() + Environment.NewLine + conn.DataSource.ToString + ", " + conn.Database.ToString + Environment.NewLine + Environment.MachineName.ToString
                    checkdate = Today
                End If
            End If
        End If
        ' If Not Me.Text = "Grantex Packer® Debug Mode" Then
        If AdvancedLogin AndAlso Environment.UserDomainName.ToUpper = My.Settings.userdomain.ToUpper Then
            Using s As New SqlCommand("select pu.id,pu.username name,pu.domainuser,pu.connected,pu.DEPARTMENT,isnull(pu.ORDCUSER,0) ORDCUSER,isnull(pu.lastworkstation,0),isnull(pu.atlantisid,0) atlantisid,isnull(pud.id,0) dptid from tbl_packeruserdata  pu left join PKRTBL_USERDEPARTMENTS pud on pud.code=pu.department where pu.connected=0 and pu.domainuser='" + Environment.UserName + "'", conn)
                conn.Open()
                Using dt As New DataTable()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                    End Using
                    conn.Close()
                    If dt.Rows.Count = 0 Then
                        Using Form2
                            Form2.ShowDialog()
                        End Using
                    ElseIf dt.Rows.Count > 1 Then
                        Dim f As New DomainUserLoginSelector(dt)
                        Dim res As DialogResult = f.ShowDialog
                        If res = DialogResult.OK Then
                            Dim selecteduserid As Integer = f.selected_user
                            Dim dv As DataView = New DataView(dt)
                            dv.RowFilter = "id=" + selecteduserid.ToString
                            activeuser = dv.Item(0).Item("name")
                            activeuserid = dv.Item(0).Item("id")
                            activeuserdpt = dv.Item(0).Item("department")
                            activeuserocu = dv.Item(0).Item("ORDCUSER")
                            activeuseraid = dv.Item(0).Item("atlantisid")
                            activeuserdptid = dv.Item(0).Item("dptid")
                        End If
                    Else
                        activeuser = dt.Rows(0).Item("name")
                        activeuserid = dt.Rows(0).Item("id")
                        activeuserdpt = dt.Rows(0).Item("department")
                        activeuserocu = dt.Rows(0).Item("ORDCUSER")
                        activeuseraid = dt.Rows(0).Item("atlantisid")
                        activeuserdptid = dt.Rows(0).Item("dptid")
                    End If
                End Using
            End Using
        Else
            Using Form2
                Form2.ShowDialog()
            End Using
        End If
        Try
            If activeuserid <> 0 Then
                Using ut As New PackerUserTransaction
                    Dim strHostName As String
                    strHostName = System.Net.Dns.GetHostName()
                    ut.WriteEntry(activeuserid, 1, value:=Environment.MachineName + " (" + Net.Dns.GetHostByName(strHostName).AddressList(0).ToString() + ") ")
                End Using
            End If
        Catch
        End Try
        Cursor.Current = ExtCursor1.Cursor
        Using s As New SqlCommand("select id, name from SALESMAN where district is not null", conn)
            conn.Open()
            Dim d As New DataTable()
            Using reader As SqlDataReader = s.ExecuteReader
                    d.Load(reader)
                End Using
                conn.Close()
                allsalesmans = d.Rows.Cast(Of DataRow).Select(Function(dr) dr(0).ToString).ToList
                Dim dro As DataRow
                dro = d.NewRow
                dro.Item("id") = 0
                dro.Item("name") = ""
                d.Rows.InsertAt(dro, 0)
                ComboBox4.DataSource = d
                ComboBox4.DisplayMember = "name"
                ComboBox4.ValueMember = "id"

        End Using
        pallettypes.Clear()
        populate_pallettypes()
        Using s As New SqlCommand("select id,description from pkrtbl_itemtypesgroups", conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                itemtypes.Load(reader)
            End Using
            conn.Close()
        End Using
        ComboBox5.DataSource = itemtypes
        ComboBox5.ValueMember = "ID"
        ComboBox5.DisplayMember = "DESCRIPTION"
        Dim recipientgroups As New DataTable()
        Using s As New SqlCommand("select id,DESCRIPTION from pkrtbl_RECIPIENTGROUPS", conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                recipientgroups.Load(reader)
            End Using
            conn.Close()
        End Using
        load_UI_rights()
        ComboBox6.DataSource = recipientgroups
        ComboBox6.ValueMember = "ID"
        ComboBox6.DisplayMember = "DESCRIPTION"
        Dim cmd2 As String = "select pu.id,pu.username,pu.connected,pu.department from tbl_packeruserdata pu "
        Using comm2 As New SqlCommand(cmd2, conn)
            Using dt2 = New DataTable()
                conn.Open()
                Using reader2 As SqlDataReader = comm2.ExecuteReader
                    dt2.Load(reader2)
                    usernames = dt2.AsEnumerable().Select(Function(d) DirectCast(d(1).ToString(), Object)).ToArray()
                    departments = dt2.AsEnumerable().Select(Function(d) DirectCast(d(3).ToString(), Object)).ToArray()
                    Dim cmd As String = "select distinct isnull(fathername,'')  from customer order by 1"
                    Dim comm As New SqlCommand(cmd, conn)
                    Dim dt = New DataTable()
                    Dim reader As SqlDataReader = comm.ExecuteReader
                    dt.Load(reader)
                    Dim items = dt.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray()
                    callnamescbox.Items.AddRange(items)
                    conn.Close()

                    'PlistLoaded(0)
                    ToolStripMenuItem5.Text = activeuser.ToUpper
                    DateTimePicker2.Value = checkdate
                    DateTimePicker1.Value = checkdate
                    'Button15.Enabled = False
                    'Button13.Enabled = False
                    'Button3.Enabled = False
                    'Panel4.Enabled = False
                    'Panel3.Enabled = False
                    ordflp_update()
                    sett.get_settings()
                    Cursor.Current = Cursors.Default
                    If activeuserdpt = "PRD" Then
                        Button2.Enabled = False
                    End If

                End Using
            End Using
        End Using
        NotifyIcon1.BalloonTipText = NotifyIcon1.BalloonTipText + ", " + activeuser
        NotifyIcon1.ShowBalloonTip(3000)
        For Each s As settingsitem In settings
            If s.name = "fsize" Then
                If s.value = "Μικρά" Then
                    dgvfontsize = 9.0F
                ElseIf s.value = "Κανονικά" Then
                    dgvfontsize = 11.0F
                ElseIf s.value = "Μεγάλα" Then
                    dgvfontsize = 13.0F
                ElseIf s.value = "Πολύ μεγάλα" Then
                    dgvfontsize = 15.0F
                End If

            ElseIf s.name = "palletsize" Then
                If s.value = "Μίνι" Then
                    VIEWSTYLE = "MINI"
                ElseIf s.value = "Μίας γραμμής" Then
                    VIEWSTYLE = "ONELINE"
                ElseIf s.value = "Κανονικές" Then
                    VIEWSTYLE = "SMALL"
                ElseIf s.value = "Μεγάλες" Then
                    VIEWSTYLE = "MEDIUM"
                ElseIf s.value = "Πολύ μεγάλες" Then
                    VIEWSTYLE = "LARGE"
                End If
            ElseIf s.name = "hidedispatchdatedgv1" Then
                hidedispatch = s.value
            ElseIf s.name = "splitposition" Then
                SplitContainer1.SplitterDistance = s.value
            ElseIf s.name = "refresh" Then
                If String.IsNullOrWhiteSpace(s.value) Then
                    s.value = 0
                End If
            End If
        Next
        If activeuserdpt = "SA" Or activeuserdpt = "BP" Or activeuserdpt = "BL" Then
            Label23.Visible = True
            TextBox5.Visible = True
        End If
        If activeuser = "SUPERVISOR" Or activeuser = "THEOFILATOS" Or activeuser = "PHILIPPOU" Then
            ComboBox2.Visible = True
            Label25.Visible = True
            ComboBox2.SelectedIndex = 0
            If activeuser = "SUPERVISOR" Then
                ComboBox2.Items.Add("BL (._L.)")
                ComboBox2.Items.Add("BP (._P.)")
            End If
        End If
        If Not (IsNothing(activeuseraid) OrElse activeuseraid = 0) Then
            ComboBox4.SelectedValue = activeuseraid
        End If
        Dim to_day As Date = checkdate
        If to_day.DayOfWeek = DayOfWeek.Sunday Then to_day = to_day.AddDays(-1)
        monDate = to_day.AddDays(DayOfWeek.Monday - to_day.DayOfWeek)
        'monDate = Convert.ToDateTime("2018/12/03")
        LockUIAccess(Me)
        activeDate = monDate
        Button5.Enabled = False
        Button10.Enabled = False
        DateTimePicker1.Value = checkdate.AddDays(-60)
        DateTimePicker2.Value = checkdate
        CheckBox15.Checked = True
        l.Add(Me.ReportViewer1)
        l.Add(Me.ReportViewer2)
        l.Add(Me.ReportViewer3)
        l.Add(Me.ReportViewer4)
        l.Add(Me.ReportViewer5)
        ReportViewer5.LocalReport.EnableHyperlinks = True
        ReportViewer4.LocalReport.EnableHyperlinks = True
        ReportViewer2.LocalReport.EnableHyperlinks = True
        ReportViewer3.LocalReport.EnableHyperlinks = True
        ReportViewer1.LocalReport.EnableHyperlinks = True
        If My.Settings.LoadingComplete = False Then
            If ReportBoot Then
                TabControl3.SelectedIndex = 1
                Refresh_report()
            Else
                reportworker_complete2()
                TabControl3.SelectedIndex = 0
            End If
            My.Settings.Save()
            End If
    End Sub

    Private Sub load_UI_rights()
        UserAccess.Clear()
        DptAccess.Clear()
        Using s As New SqlCommand("select form,control,locked,NOTVISIBLE,DEFAULTVALUE from pkrtbl_useruiaccess where userid=" + activeuserid.ToString, conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                UserAccess.Load(reader)
            End Using
            conn.Close()
        End Using
        Using s As New SqlCommand("select form,control,locked,NOTVISIBLE,DEFAULTVALUE from pkrtbl_dptuiaccess where DPTCODE='" + activeuserdpt.ToString + "'", conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                DptAccess.Load(reader)
            End Using
            conn.Close()
        End Using
    End Sub

    Dim monDate As Date
    Dim itemtypesgroup As Integer
    Dim recipientsgroup As Integer
    Dim activeDate As Date
    Dim l As New List(Of ReportViewer)

    Private Sub Refresh_report()
        Try
            itemtypesgroup = ComboBox5.SelectedValue
            recipientsgroup = ComboBox6.SelectedValue
            While reportLevel <> 0
                reportLevel -= 1
                ReportViewer1.PerformBack()
            End While
            While reportLevel2 <> 0
                reportLevel2 -= 1
                ReportViewer2.PerformBack()
            End While
            While reportLevel5 <> 0
                reportLevel5 -= 1
                ReportViewer5.PerformBack()
            End While
            For Each rv As ReportViewer In l
                rv.Visible = False
                If rv.Name = "ReportViewer4" Then
                    Dim pic As New PictureBox
                    pic.Image = My.Resources.Facebook_1s_200px
                    pic.SizeMode = PictureBoxSizeMode.CenterImage
                    DoubleBufferedTableLayoutPanel3.Controls.Add(pic, DoubleBufferedTableLayoutPanel3.GetColumn(rv), DoubleBufferedTableLayoutPanel3.GetRow(rv))
                    Me.TableLayoutPanel1.SetColumnSpan(pic, Me.TableLayoutPanel1.GetColumnSpan(rv))
                    Me.TableLayoutPanel1.SetRowSpan(pic, Me.TableLayoutPanel1.GetRowSpan(rv))
                    pic.Dock = DockStyle.Fill
                End If
            Next
            reportworker.RunWorkerAsync()
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Shared Sub ToggleConfigEncryption(ByVal exeConfigName As String)
        ' Takes the executable file name without the
        ' .config extension.
        Try
            ' Open the configuration file and retrieve 
            ' the connectionStrings section.
            Dim config As Configuration = ConfigurationManager.
            OpenExeConfiguration(exeConfigName)

            Dim section As ConnectionStringsSection = DirectCast(
            config.GetSection("connectionStrings"),
            ConnectionStringsSection)

            If section.SectionInformation.IsProtected Then
                ' Remove encryption.
                section.SectionInformation.UnprotectSection()
            Else
                ' Encrypt the section.
                section.SectionInformation.ProtectSection(
              "DataProtectionConfigurationProvider")
            End If

            ' Save the current configuration.
            config.Save()

            Console.WriteLine("Protected={0}",
        section.SectionInformation.IsProtected)

        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
    Dim issorted As Integer = -1
    Dim dgv1vrow As Integer = 0
    Public Sub datagridview1_refresh()
        Try
            dgv1columns.Clear()
            dgv1vrow = DataGridView1.FirstDisplayedScrollingRowIndex
            If dgv1vrow = -1 Then
                dgv1vrow = 0
            End If

            For Each clmn As DataGridViewColumn In DataGridView1.Columns
                clmn.Dispose()
            Next
            If Not DataGridView1.SortedColumn Is Nothing Then
                issorted = DataGridView1.SortedColumn.Index

            End If
            DataGridView1.Columns.Clear()
            DataGridView1.DataSource = Nothing
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.AllowUserToAddRows = False
            ibw.RunWorkerAsync()
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Sub

    Private Sub ibw_DoWork(sender As Object, e As DoWorkEventArgs) Handles ibw.DoWork
        Dim ibwconn = New SqlConnection(connString)
        Dim dt = New DataTable()
        Try
            Using comm = dgv1cmd
                comm.Connection = ibwconn
                comm.CommandType = CommandType.Text
                ibwconn.Open()
                Console.WriteLine("IBW HELLO")


                Using reader As SqlDataReader = comm.ExecuteReader
                    Console.WriteLine("IBW HELLO2")
                    dt.Load(reader)
                    ibwconn.Close()

                    Console.WriteLine("IBW E.RESULT=DT")
                End Using

            End Using
        Catch
        Finally
            e.Result = dt
            dt.Dispose()
            ibwconn.Dispose()
        End Try
    End Sub

    Private Sub ibw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles ibw.RunWorkerCompleted
        'If IsNothing(e.Error) Then
        Dim dt As DataTable = e.Result

        Try


            If Len(relftrids) = 0 Then
                'DataGridView1.Rows.Clear()
                DataGridView1.Columns.Clear()
                DataGridView1.DataSource = Nothing
                DataGridView2.DataSource = Nothing
                'DataGridView2.Rows.Clear()
                DataGridView2.Columns.Clear()


                'orderdgv.Rows.Clear()
                orderdgv.Columns.Clear()
                clearpallets()
                orderdgv.DataSource = Nothing
                Throw New System.Exception("Η αναζήτηση σας δεν επέστρεψε αποτελέσματα!")
            End If
            If Len(relftrids) > 0 Then
                DataGridView1.DataSource = dt
                If DataGridView1.Rows.Count <> 0 Then
                    Dim HiddenColumns As New List(Of String) From {"iteid", "ΚΩΔΠΕΛ", "stlid", "ftrid", "blue", "black", "green", "lightgreen", "gold", "ΚΩΔΙΚΟΣ"}
                    Dim ColumnWidths As New Dictionary(Of String, Integer) From {{"ΑΠΟΔΕΚΤΗΣ", 15}}
                    For Each d As DataGridViewColumn In DataGridView1.Columns
                        If HiddenColumns.Contains(d.Name) Then
                            d.Visible = False
                        End If
                        If ColumnWidths.ContainsKey(d.Name) Then
                            d.Width = ColumnWidths(d.Name)
                        End If
                    Next
                    'DataGridView1.Columns(10).Visible = False
                    DataGridView1.Columns("ΠΟΣ").Width = 50
                    DataGridView1.Columns("ΠΕΛ").Width = 50
                    DataGridView1.Columns("ΥΠΟΛ.").Width = 50
                    'Me.DataGridView1.CurrentCell = Me.DataGridView1.Item(2, 0)
                    DataGridView1.Rows(0).Cells(3).Selected = True
                End If

                'Dim cbclmn As New DataGridViewCheckBoxColumn
                'DataGridView1.Columns.Insert(1, cbclmn)
                'DataGridView1.Columns(1).Name = "ΕΠΙΛ"
                'DataGridView1.Columns("ΕΠΙΛ").Width = 50


                For i As Integer = 0 To DataGridView1.Columns.Count - 1
                    dgv1columns.Add(DataGridView1.Columns(i).Name)
                    DataGridView1.Columns(i).ReadOnly = True
                    '    If DataGridView1.Columns(i).Name = "ΕΠΙΛ" Then
                    '        DataGridView1.Columns(i).ReadOnly = False
                    '    End If

                Next
                Dim seliteid As New List(Of Integer)
                Dim selstlid As New List(Of Integer)
                Try

                    Dim fri As Integer
                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
                        If DataGridView1.Rows(i).Selected Then
                            seliteid.Add(DataGridView1.Rows(i).Cells("iteid").Value)
                            selstlid.Add(DataGridView1.Rows(i).Cells("stlid").Value)
                            fri = DataGridView1.FirstDisplayedScrollingRowIndex
                            If fri = -1 Then
                                fri = 0
                            End If
                        End If
                    Next
                    DataGridView1.ClearSelection()

                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
                        If seliteid.Contains(DataGridView1.Rows(i).Cells("iteid").Value) And seliteid.Contains(DataGridView1.Rows(i).Cells("stlid").Value) Then
                            DataGridView1.FirstDisplayedScrollingRowIndex = fri
                            DataGridView1.Rows(i).Selected = True
                            DataGridView1.CurrentCell = DataGridView1.Rows(i).Cells(1)

                        Else
                            DataGridView1.Rows(i).Selected = False
                        End If

                    Next
                Finally
                    seliteid = Nothing
                    selstlid = Nothing
                End Try


                If issorted > -1 Then

                    DataGridView1.Sort(DataGridView1.Columns(issorted), SetSortOrderdgv1)
                End If
                filtermanager()
            End If


            iteidcolindex = dgv1columns.IndexOf("iteid")
            stlidcolindex = dgv1columns.IndexOf("stlid")
            ftridcolindex = dgv1columns.IndexOf("ftrid")
            startquantcolindex = dgv1columns.IndexOf("ΠΟΣ")
            namecolindex = dgv1columns.IndexOf("ΠΕΡΙΓΡΑΦΗ")
            codecolindex = dgv1columns.IndexOf("ΚΩΔΙΚΟΣ")
            reqquantcolindex = dgv1columns.IndexOf("ΥΠΟΛ.")
            If DataGridView1.Rows.Count > 0 AndAlso DataGridView1.Rows(dgv1vrow).Visible Then
                DataGridView1.FirstDisplayedScrollingRowIndex = dgv1vrow
            End If
            Dim f As New Font("verdana", dgvfontsize, GraphicsUnit.Pixel)
            DataGridView1.DefaultCellStyle.Font = f
            DataGridView1.Columns("ΥΠΟΛ.").DefaultCellStyle.BackColor = Color.LightGray
            DataGridView1.Columns("ΥΠΟΛ.").DefaultCellStyle.Font = boldfont
            CUSTOMER = DataGridView1.Rows(0).Cells("ΚΩΔΠΕΛ").Value.ToString + " " + DataGridView1.Rows(0).Cells("ΠΕΛ").Value.ToString
            customerfn = DataGridView1.Rows(0).Cells("ΠΕΛ").Value.ToString.Replace(".", "")
            DataGridView1.Refresh()
        Catch ex As Exception

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally

            DataGridView1.Visible = True

            dt.Dispose()
            If Not obw.IsBusy And Not plbw.IsBusy And Not palletw.IsBusy Then
                My.Settings.downloading = False
                Button1.Enabled = True
            End If
        End Try
        ' End If
    End Sub
    'Public Function pallet_exchange(ByVal takeorgive As Integer, pallet As String, quantity As Double, row As DataGridViewRow, Optional fromcelledit As Boolean = False, Optional frommantis As Boolean = False, Optional dontupdate As Boolean = False)
    '    'takeorgive=1 τοποθετεί, takeorgive=2 προσθέτει, takeorgive=-1 αφαιρεί 
    '    If updconn.State = ConnectionState.Open Then
    '        updconn.Close()
    '    End If
    '    If conn.State = ConnectionState.Open Then
    '        conn.Close()
    '    End If
    '    Dim limit As Double
    '    Try
    '        If row.Cells("iteid").Value = 65947 Or row.Cells("iteid").Value = 65946 Or row.Cells("iteid").Value = 65948 Then
    '            Return 0
    '        End If

    '        If return_ftrid_status(row.Cells("ftrid").Value) < 6 Then
    '            Throw New System.Exception("Δεν έχουν ολοκληρωθεί ακόμα οι έλεγχοι. Δεν μπορείτε να κατανείμετε είδη μέχρι τότε.")
    '        End If
    '        If return_ftrid_status(row.Cells("ftrid").Value) > 12 Then
    '            Throw New System.Exception("Πώς ακριβώς κατάφερες να βρεθείς εδώ? Δεν μπορείς να μεταφέρεις είδος ολοκληρωμένης παραγγελίας.")
    '        End If

    '        Using ccc As New SqlCommand("select isnull(closedbyid,0),isnull(orders,0) from tbl_palletheaders where id=" + pallet, conn)

    '            conn.Open()
    '            Dim value = ccc.ExecuteScalar
    '            If value <> 0 Then
    '                Throw New System.Exception("Η παλέτα είναι ολοκληρωμένη. Δεν μπορείτε να μεταφέρετε είδος.")
    '            End If
    '            conn.Close()
    '        End Using
    '        Using ccc As New SqlCommand("select isnull(pud.DEPARTMENT,'') from tbl_palletheaders ph left join tbl_packeruserdata pud on pud.id=ph.LOCKEDBYID where ph.id=" + pallet, conn)
    '            conn.Open()
    '            Dim value = ccc.ExecuteScalar
    '            If value <> "" AndAlso value <> activeuserdpt Then
    '                Throw New System.Exception("Η παλέτα είναι κλειδωμένη από άλλο τμήμα. Δεν μπορείτε να μεταφέρετε είδος.")
    '            End If
    '            conn.Close()


    '        End Using
    '        Using ccc As New SqlCommand("select isnull(orders,'') from tbl_palletheaders where id=" + pallet, conn)

    '            conn.Open()
    '            Dim value = ccc.ExecuteScalar
    '            If value = "" Then

    '                Throw New System.Exception("Η παλέτα δεν έχει σχετικά ΠΑΡ. Συμπληρώστε πρώτα σχετική παραγγελία και προσπαθήστε ξανά.")
    '                Return 0
    '            ElseIf Not value.ToString.Contains(row.Cells("ΠΑΡ").Value.ToString.TrimStart("0"c)) Then
    '                Throw New System.Exception("Η παραγγελία του είδους δεν ανήκει στα σχετικά ΠΑΡ της παλέτας. Προσθέστε τη προτού επιχειρήσετε τη κατανομή του είδους στη συγκεκριμένη παλέτα.")
    '                Return 0
    '            End If
    '            conn.Close()
    '        End Using
    '        With FlowLayoutPanel1
    '            For Each pi As Control In FlowLayoutPanel1.Controls
    '                Dim palletid As Integer = 0
    '                Dim customer As String = ""
    '                Dim oldcell As Double

    '                Dim myname As String = ""
    '                Dim normal As pallettemplate = TryCast(pi, pallettemplate)
    '                Dim small As smallpallet = TryCast(pi, smallpallet)

    '                If normal IsNot Nothing Then

    '                    palletid = normal.palletid
    '                    customer = normal.customer
    '                    oldcell = normal.oldcell
    '                    myname = normal.pallettemplatelabel.Text
    '                ElseIf small IsNot Nothing Then

    '                    palletid = small.palletid
    '                    customer = small.customer
    '                    myname = small.Label1.Text
    '                End If
    '                If palletid = pallet Then
    '                    If customer <> row.Cells("ΠΕΛ").Value.ToString.Replace(".", "") Then
    '                        Throw New System.Exception("ΠΡΟΣΟΧΗ!Δεν μπορείτε να εισάγετε είδος του πελάτη " + row.Cells("ΚΩΔΠΕΛ").Value + " σε παλέτα του πελάτη " + customer)
    '                        Return 0
    '                    End If


    '                    Try
    '                        If takeorgive = 1 Or takeorgive = 2 Then
    '                            Using limitcmd As New SqlCommand("SELECT DIFF FROM Z_PACKER_PENDING_ITEMS_PER_ORDER WHERE STLID=" + row.Cells("stlid").Value.ToString, conn)
    '                                conn.Open()
    '                                limit = limitcmd.ExecuteScalar
    '                                conn.Close()
    '                            End Using
    '                            Dim rindex As Integer = -1
    '                            For j As Integer = 0 To pldt.Rows.Count - 1
    '                                If frommantis Then
    '                                    If pldt.Rows(j).Item("palletid") = pallet AndAlso pldt.Rows(j).Item("stlid") = row.Cells("stlid").Value Then
    '                                        rindex = j
    '                                    End If
    '                                Else
    '                                    If pldt.Rows(j).Item("palletid") = pallet AndAlso pldt.Rows(j).Item("stlid") = row.Cells("stlid").Value And Not pldt.Rows(j).Item("frommantis") = 1 Then
    '                                        rindex = j
    '                                    End If
    '                                End If

    '                            Next
    '                            Dim q As Double = 0

    '                            If rindex <> -1 Then
    '                                If takeorgive = 1 Then
    '                                    q = quantity
    '                                ElseIf takeorgive = 2 Then
    '                                    q = pldt.Rows(rindex).Item("QUANTITY") + quantity
    '                                End If


    '                                If fromcelledit And normal IsNot Nothing Then
    '                                    If q > oldcell And limit < q - oldcell Then
    '                                        updconn.Close()
    '                                        Return 0
    '                                    End If
    '                                Else
    '                                    If q > pldt.Rows(rindex).Item("QUANTITY") And
    '                                        limit < q - pldt.Rows(rindex).Item("QUANTITY") Then
    '                                        updconn.Close()
    '                                        Return 0
    '                                    End If
    '                                End If


    '                                Dim cmd As String = ""
    '                                Dim success As Integer = 0

    '                                If frommantis Then
    '                                    cmd = "update tbl_palletlines set quantity=@quantity where palletid=@pallet and stlid=@stlid and ftrid=@ftrid and iteid=@iteid and frommantis=1"
    '                                Else
    '                                    cmd = "update tbl_palletlines set quantity=@quantity where palletid=@pallet and stlid=@stlid and ftrid=@ftrid and iteid=@iteid and frommantis is null"

    '                                End If

    '                                'cmd = "update tbl_palletlines Set quantity=" + q.ToString + " where palletid=" + pallet + " And stlid=" + row.Cells("stlid").Value.ToString + " And ftrid=" + row.Cells("ftrid").Value.ToString
    '                                Dim cmd2 As String = "update tbl_palletheaders Set LUPDATEUSER=@activeuserid where id=@pallet"
    '                                'Dim cmd2 As String = "update tbl_palletheaders Set LUPDATEUSER=" + activeuserid.ToString + " where id=" + pallet
    '                                Using sqlcmd As New SqlCommand(cmd, updconn)
    '                                    Using sqlcmd2 As New SqlCommand(cmd2, updconn)
    '                                        sqlcmd.Parameters.Add("@iteid", SqlDbType.Int).Value = row.Cells("iteid").Value
    '                                        sqlcmd.Parameters.Add("@quantity", SqlDbType.Float).Value = q
    '                                        sqlcmd.Parameters.Add("@pallet", SqlDbType.Int).Value = CInt(pallet)
    '                                        sqlcmd.Parameters.Add("@stlid", SqlDbType.Int).Value = row.Cells("stlid").Value
    '                                        sqlcmd.Parameters.Add("@ftrid", SqlDbType.Int).Value = row.Cells("ftrid").Value
    '                                        sqlcmd2.Parameters.Add("@activeuserid", SqlDbType.Int).Value = activeuserid
    '                                        sqlcmd2.Parameters.Add("@pallet", SqlDbType.Int).Value = CInt(pallet)
    '                                        updconn.Open()
    '                                        success = sqlcmd.ExecuteNonQuery()
    '                                        If success <= 0 Then
    '                                            Return 0

    '                                        End If
    '                                        sqlcmd2.ExecuteNonQuery()
    '                                        updconn.Close()
    '                                    End Using
    '                                End Using
    '                                datagridview1_stlquantity(row.Cells("stlid").Value.ToString)
    '                                'With p.pallettemplatedatagrid

    '                                '    '.Rows(rindex).Cells("iteid").Value = row.Cells("iteid").Value
    '                                '    .Rows(rindex).Cells("QUANT").Value = q
    '                                '    '.Rows(rindex).Cells("ΚΩΔΙΚΟΣ").Value = row.Cells("ΚΩΔΙΚΟΣ").Value
    '                                '    '.Rows(rindex).Cells("ΠΕΡΙΓΡΑΦΗ").Value = row.Cells("ΠΕΡΙΓΡΑΦΗ").Value
    '                                '    '.Rows(rindex).Cells("ΠΑΡ").Value = row.Cells("ΠΑΡ").Value
    '                                '    '.Rows(rindex).Cells("ΚΩΔΠΕΛ").Value = row.Cells("ΚΩΔΠΕΛ").Value
    '                                '    '.Rows(rindex).Cells("ΠΕΛ").Value = row.Cells("ΠΕΛ").Value
    '                                '    '.Rows(rindex).Cells("ftrid").Value = row.Cells("ftrid").Value
    '                                '    '.Rows(rindex).Cells("stlid").Value = row.Cells("stlid").Value
    '                                'End With
    '                                If Not dontupdate Then
    '                                    populate_pallets(palletid.ToString)
    '                                End If




    '                            Else
    '                                If quantity > limit Then
    '                                    Return 0
    '                                End If
    '                                Dim cmd As String = ""
    '                                Dim success As Integer = 0
    '                                cmd = "insert into tbl_palletlines (palletid,iteid,quantity,stlid,ftrid,batchnumber,frommantis) values (@pallet,@iteid,@quantity,@stlid,@ftrid,@batchnumber,@frommantis)"
    '                                'cmd = "insert into tbl_palletlines (palletid,iteid,quantity,stlid,ftrid,batchnumber) values (" + pallet + "," + row.Cells("iteid").Value.ToString + "," + quantity.ToString + "," + row.Cells("stlid").Value.ToString + "," + row.Cells("ftrid").Value.ToString + "," + TextBox5.Text + ")"
    '                                Dim cmd2 As String = "update tbl_palletheaders Set LUPDATEUSER=@activeuserid where id=@pallet"
    '                                'Dim cmd2 As String = "update tbl_palletheaders Set LUPDATEUSER=" + activeuserid.ToString + " where id=" + pallet
    '                                Using sqlcmd As New SqlCommand(cmd, updconn)
    '                                    Using sqlcmd2 As New SqlCommand(cmd2, updconn)
    '                                        sqlcmd.Parameters.Add("@iteid", SqlDbType.Int).Value = row.Cells("iteid").Value
    '                                        sqlcmd.Parameters.Add("@quantity", SqlDbType.Float).Value = quantity
    '                                        sqlcmd.Parameters.Add("@pallet", SqlDbType.Int).Value = CInt(pallet)
    '                                        sqlcmd.Parameters.Add("@stlid", SqlDbType.Int).Value = row.Cells("stlid").Value
    '                                        sqlcmd.Parameters.Add("@ftrid", SqlDbType.Int).Value = row.Cells("ftrid").Value
    '                                        sqlcmd.Parameters.Add("@batchnumber", SqlDbType.NVarChar, 500).Value = TextBox5.Text
    '                                        sqlcmd2.Parameters.Add("@activeuserid", SqlDbType.Int).Value = activeuserid
    '                                        sqlcmd2.Parameters.Add("@pallet", SqlDbType.Int).Value = CInt(pallet)
    '                                        If frommantis Then
    '                                            sqlcmd.Parameters.Add("@frommantis", SqlDbType.Int).Value = 1
    '                                        Else
    '                                            sqlcmd.Parameters.Add("@frommantis", SqlDbType.Int).Value = DBNull.Value
    '                                        End If

    '                                        updconn.Open()
    '                                        success = sqlcmd.ExecuteNonQuery()
    '                                        If success <= 0 Then
    '                                            updconn.Close()
    '                                            Return 0

    '                                        End If
    '                                        sqlcmd2.ExecuteNonQuery()
    '                                        updconn.Close()
    '                                    End Using
    '                                End Using
    '                                If Not dontupdate Then
    '                                    populate_pallets(palletid.ToString)
    '                                End If
    '                            End If
    '                            Return 1
    '                        ElseIf takeorgive = -1 Then
    '                            For i As Integer = pldt.Rows.Count - 1 To 0 Step -1
    '                                If pldt.Rows(i).Item("palletid") = palletid AndAlso pldt.Rows(i).Item("stlid") = row.Cells("stlid").Value Then
    '                                    pldt.Rows.RemoveAt(i)

    '                                    Exit For
    '                                End If
    '                            Next
    '                            GC.Collect()
    '                            Dim success As Integer = 0
    '                            Dim cmd2 As String = ""
    '                            If frommantis Then
    '                                cmd2 = "delete from tbl_palletlines where palletid=(Select id from tbl_palletheaders where id=@pallet And closedbyid Is null) And iteid=@iteid And stlid=@stlid And ftrid=@ftrid and frommantis =1"
    '                            Else
    '                                cmd2 = "delete from tbl_palletlines where palletid=(Select id from tbl_palletheaders where id=@pallet And closedbyid Is null) And iteid=@iteid And stlid=@stlid And ftrid=@ftrid and frommantis is null"
    '                            End If
    '                            Dim cmd As String = "update tbl_palletheaders Set LUPDATEUSER=@activeuserid where id=@pallet"
    '                            Using sqlcmd2 As New SqlCommand(cmd2, updconn)
    '                                Using sqlcmd As New SqlCommand(cmd, updconn)
    '                                    sqlcmd2.Parameters.Add("@iteid", SqlDbType.Int).Value = row.Cells("iteid").Value
    '                                    sqlcmd2.Parameters.Add("@pallet", SqlDbType.Int).Value = CInt(pallet)
    '                                    sqlcmd2.Parameters.Add("@stlid", SqlDbType.Int).Value = row.Cells("stlid").Value
    '                                    sqlcmd2.Parameters.Add("@ftrid", SqlDbType.Int).Value = row.Cells("ftrid").Value
    '                                    sqlcmd.Parameters.Add("@activeuserid", SqlDbType.Int).Value = activeuserid
    '                                    sqlcmd.Parameters.Add("@pallet", SqlDbType.Int).Value = CInt(pallet)
    '                                    updconn.Open()
    '                                    success = sqlcmd2.ExecuteNonQuery()
    '                                    If success <= 0 Then
    '                                        updconn.Close()
    '                                        Return 0

    '                                    End If


    '                                    sqlcmd.ExecuteNonQuery()
    '                                    updconn.Close()
    '                                End Using
    '                            End Using

    '                            Return 1




    '                        End If
    '                    Finally
    '                        If Not dontupdate Then
    '                            If normal IsNot Nothing Then

    '                                normal.compute_sums()
    '                                normal.check_mantis()


    '                            End If

    '                            change_frommantis()
    '                        End If

    '                    End Try

    '                End If


    '            Next


    '        End With

    '    Catch EX As Exception
    '        If updconn.State = ConnectionState.Open Then
    '            updconn.Close()
    '        End If
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '        Return 0
    '    End Try

    'End Function


    Public Function datagridview1_stlquantity(ByVal stlid As Integer)
        Try
            Dim issorted As Integer = -1

            If Not DataGridView1.SortedColumn Is Nothing Then
                issorted = DataGridView1.SortedColumn.Index

            End If
            Dim vrow As Integer = DataGridView1.FirstDisplayedScrollingRowIndex
            If vrow = -1 Then
                vrow = 0
            End If
            Dim cmd As String = "Select [ΥΠΟΛ.],LIGHTGREEN,BLUE,GREEN,GOLD,BLACK,BACKORDER FROM Z_PACKER_ITEMSBROWSER WHERE STLID=@stlid"
            Dim comm As New SqlCommand(cmd, conn)
            comm.Parameters.Add("@stlid", SqlDbType.Int).Value = stlid
            conn.Open()
            Using comm
                Using DT As New DataTable()
                    Using READER As SqlDataReader = comm.ExecuteReader
                        DT.Load(READER)
                        conn.Close()
                        Dim DtGrid = CType(DataGridView1.DataSource, DataTable).Copy()
                        For i As Integer = 0 To DtGrid.Rows.Count - 1
                            If DtGrid.Rows(i).Item("stlid") = stlid Then
                                DtGrid.Columns("ΥΠΟΛ.").ReadOnly = False
                                DtGrid.Rows(i).Item("ΥΠΟΛ.") = DT.Rows(0).Item("ΥΠΟΛ.")
                                DtGrid.Columns("ΥΠΟΛ.").ReadOnly = True
                                DtGrid.Columns("lightgreen").ReadOnly = False
                                DtGrid.Rows(i).Item("lightgreen") = DT.Rows(0).Item("LIGHTGREEN")
                                DtGrid.Columns("lightgreen").ReadOnly = True
                                DtGrid.Columns("blue").ReadOnly = False
                                DtGrid.Rows(i).Item("blue") = DT.Rows(0).Item("BLUE")
                                DtGrid.Columns("blue").ReadOnly = True
                                DtGrid.Columns("GREEN").ReadOnly = False
                                DtGrid.Rows(i).Item("green") = DT.Rows(0).Item("GREEN")
                                DtGrid.Columns("GREEN").ReadOnly = True
                                DtGrid.Columns("gold").ReadOnly = False
                                DtGrid.Rows(i).Item("gold") = DT.Rows(0).Item("GOLD")
                                DtGrid.Columns("gold").ReadOnly = True
                                DtGrid.Columns("black").ReadOnly = False
                                DtGrid.Rows(i).Item("black") = DT.Rows(0).Item("BLACK")
                                DtGrid.Columns("black").ReadOnly = True
                                DtGrid.Columns("BACKORDER").ReadOnly = False
                                DtGrid.Rows(i).Item("BACKORDER") = DT.Rows(0).Item("BACKORDER")
                                DtGrid.Columns("BACKORDER").ReadOnly = True
                                DataGridView1.Columns("Status").ReadOnly = False
                                DataGridView1.Rows(i).Cells("Status").Value = DT.Rows(0).Item("BACKORDER").ToString + "/" + DT.Rows(0).Item("black").ToString + "/" + DT.Rows(0).Item("blue").ToString + "/" + DT.Rows(0).Item("lightgreen").ToString + "/" + DT.Rows(0).Item("green").ToString + "/" + DT.Rows(0).Item("gold").ToString
                                DataGridView1.Columns("Status").ReadOnly = True

                            End If
                        Next
                        DataGridView1.DataSource = DtGrid
                    End Using
                End Using
            End Using
            order_columns(2)
            DataGridView1.Refresh()
            If vrow <= DataGridView1.Rows.Count - 1 Then
                If DataGridView1.Rows(vrow).Visible Then
                    DataGridView1.FirstDisplayedScrollingRowIndex = vrow
                End If
            End If
            filtermanager()
            If issorted > -1 Then
                DataGridView1.Sort(DataGridView1.Columns(issorted), SetSortOrderdgv1)
            End If
        Catch EX As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Function

    Dim dgv2vrow As Integer = 0
    Public Sub datagridview2_refresh()
        Try
            dgv2vrow = DataGridView2.FirstDisplayedScrollingRowIndex
            If dgv2vrow = -1 Then
                dgv2vrow = 0
            End If
            Dim issorted As Integer = -1
            If Not DataGridView2.SortedColumn Is Nothing Then
                issorted = DataGridView2.SortedColumn.Index
            End If
            DataGridView2.DataSource = Nothing
            For Each clmn As DataGridViewColumn In DataGridView2.Columns
                clmn.Dispose()
            Next
            DataGridView2.Columns.Clear()
            DataGridView2.AutoGenerateColumns = True
            DataGridView2.AllowUserToAddRows = False
            DataGridView2.ReadOnly = True
            plbw.RunWorkerAsync()
        Catch EX As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally

        End Try
    End Sub
    Private Sub plbw_DoWork(sender As Object, e As DoWorkEventArgs) Handles plbw.DoWork
        Dim plbwconn = New SqlConnection(connString)
        Dim dt2 As New DataTable()
        'Dim dt3 As New DataTable()
        Try

            Using comm2 As New SqlCommand(dgv2cmd, plbwconn)
                'Using comm3 As New SqlCommand(dgv3cmd, plbwconn)


                plbwconn.Open()

                Using reader2 As SqlDataReader = comm2.ExecuteReader()

                    comm2.CommandType = CommandType.Text
                    'comm3.CommandType = CommandType.Text
                    dt2.Load(reader2)
                    plbwconn.Close()
                    ' Using reader3 As SqlDataReader = comm3.ExecuteReader()


                    'dt3.Load(reader3)

                    ' End Using

                End Using

            End Using
            'End Using
        Catch
        Finally
            e.Result = dt2
            dt2.Dispose()
            plbwconn.Dispose()
        End Try
        Console.WriteLine("PLBW BYE BYE")


    End Sub

    Private Sub plbw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles plbw.RunWorkerCompleted
        '  If IsNothing(e.Error) Then
        Try
            DataGridView2.DataSource = e.Result
            If DataGridView2.RowCount > 0 AndAlso dgv2vrow <= DataGridView2.Rows.Count - 1 AndAlso DataGridView2.Rows(dgv2vrow).Visible Then


                DataGridView2.FirstDisplayedScrollingRowIndex = dgv2vrow

            End If

            'If issorted > -1 Then

            '    DataGridView2.Sort(DataGridView2.Columns(issorted), SetSortOrderdgv2)
            'End If
            Dim f As New Font("verdana", dgvfontsize, GraphicsUnit.Pixel)
            DataGridView2.DefaultCellStyle.Font = f

            DataGridView2.Visible = True
        Catch EX As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            If Not ibw.IsBusy And Not obw.IsBusy And Not palletw.IsBusy Then
                My.Settings.downloading = False
                Button1.Enabled = True
            End If
        End Try

        ' End If
    End Sub

    Dim odgvvrow As Integer = 0

    Public Sub orderdgv_refresh()
        Try
            Dim odgvvrow As Integer = orderdgv.FirstDisplayedScrollingRowIndex
            If odgvvrow = -1 Then
                odgvvrow = 0
            End If
            Dim issorted As Integer = -1
            If Not orderdgv.SortedColumn Is Nothing Then
                issorted = orderdgv.SortedColumn.Index

            End If
            orderdgv.DataSource = Nothing
            My.Resources.issues.Dispose()
            My.Resources.completed.Dispose()
            My.Resources.editorder.Dispose()

            For Each clmn As DataGridViewColumn In orderdgv.Columns
                clmn.Dispose()
            Next
            orderdgv.Columns.Clear()
            orderdgv.Rows.Clear()
            obw.RunWorkerAsync()


        Catch EX As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub


    Private Sub obw_DoWork(sender As Object, e As DoWorkEventArgs) Handles obw.DoWork
        Dim obwconn = New SqlConnection(connString)
        Dim orddt = New DataTable()
        Dim ordcomm As New SqlCommand(orderdgvcmd, obwconn)
        ordcomm.CommandTimeout = 0
        obwconn.Open()
        Dim ordreader As SqlDataReader = ordcomm.ExecuteReader()
        Try
            Console.WriteLine("OBW HELLO")
            ordcomm.CommandType = CommandType.Text

            orddt.Load(ordreader)
            Console.WriteLine("OBW HELLO2")
            obwconn.Close()

        Catch
        Finally
            e.Result = orddt
            orddt.Dispose()
            obwconn.Dispose()
            ordcomm.Dispose()
            ordreader.Dispose()
        End Try
    End Sub

    Private Sub obw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles obw.RunWorkerCompleted
        ' If IsNothing(e.Error) Then
        Try
            With orderdgv

                orderdgv.DataSource = e.Result

                With orderdgv
                    .AutoGenerateColumns = True
                    .AllowUserToAddRows = False
                    .ReadOnly = True
                End With

                If .Rows.Count <> 0 Then

                    Dim HiddenColumns As New List(Of String) From {"ID", "statusnum", "RED", "BLUE", "BLACK", "GREEN", "lightgreen", "GOLD", "ΚΩΔΙΚΟΣ"}
                    Dim ColumnWidths As New Dictionary(Of String, Integer) From {{"ΑΠΟΔΕΚΤΗΣ", 15}}
                    For Each d As DataGridViewColumn In .Columns
                        If HiddenColumns.Contains(d.Name) Then
                            d.Visible = False
                        End If
                        If ColumnWidths.ContainsKey(d.Name) Then
                            d.Width = ColumnWidths(d.Name)
                        End If
                    Next
                    .Columns("ID").Visible = False
                    .Columns("statusnum").Visible = False
                End If
                If .RowCount > 0 AndAlso .Rows(odgvvrow).Visible Then
                    orderdgv.FirstDisplayedScrollingRowIndex = odgvvrow
                End If
                If issorted > -1 Then

                    .Sort(orderdgv.Columns(issorted), SetSortOrderorderdgv)
                End If
            End With
            Dim f As New Font("verdana", dgvfontsize, GraphicsUnit.Pixel)
            orderdgv.DefaultCellStyle.Font = f
            Label3.Visible = False
            orderdgv.Visible = True
        Catch EX As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally

            If Not ibw.IsBusy And Not plbw.IsBusy And Not palletw.IsBusy Then
                My.Settings.downloading = False
                Button1.Enabled = True
            End If
        End Try
        '  End If

    End Sub


    'DEPRECATED PROCEDURE
    'Public Sub orderdgv_refresh()
    '    Try
    '        Dim vrow As Integer = orderdgv.FirstDisplayedScrollingRowIndex
    '        If vrow = -1 Then
    '            vrow = 0
    '        End If
    '        Dim issorted As Integer = -1
    '        If Not orderdgv.SortedColumn Is Nothing Then
    '            issorted = orderdgv.SortedColumn.Index

    '        End If
    '        orderdgv.DataSource = Nothing
    '        My.Resources.issues.Dispose()
    '        My.Resources.completed.Dispose()
    '        My.Resources.editorder.Dispose()

    '        For Each clmn As DataGridViewColumn In orderdgv.Columns
    '            clmn.Dispose()
    '        Next
    '        orderdgv.Columns.Clear()
    '        orderdgv.Rows.Clear()

    '        Using ordcomm As New SqlCommand(orderdgvcmd, conn)
    '            Using orddt = New DataTable()
    '                conn.Open()
    '                Using ordreader As SqlDataReader = ordcomm.ExecuteReader()
    '                    ordcomm.CommandType = CommandType.Text

    '                    Try
    '                        orddt.Load(ordreader)
    '                    Catch ex As Exception
    '                        If conn.State = ConnectionState.Open Then
    '                            conn.Close()
    '                        End If
    '                        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '                    End Try
    '                    With orderdgv
    '                        .AutoGenerateColumns = True
    '                        .AllowUserToAddRows = False
    '                        .ReadOnly = True
    '                    End With
    '                    orderdgv.DataSource = orddt
    '                    conn.Close()
    '                End Using
    '            End Using
    '        End Using
    '        Using wclmn As New DataGridViewImageColumn
    '            Using pclmn As New DataGridViewImageColumn
    '                Using wclmn2 As New DataGridViewImageColumn
    '                    Using pclmn2 As New DataGridViewImageColumn
    '                        wcindex = -1
    '                        pcindex = -1
    '                        For j As Integer = 0 To orderdgv.Columns.Count - 1
    '                            If orderdgv.Columns(j).HeaderText = "ΗΜΕΡΟΜΗΝΙΑ ΚΑΤΑΧΩΡΙΣΗΣ" Then
    '                                wcindex = j + 1
    '                            End If
    '                            If orderdgv.Columns(j).HeaderText = "ΣΧΟΛΙΑ ΑΠΘ" Then
    '                                pcindex = j + 2
    '                            End If
    '                        Next
    '                        orderdgv.Columns.Insert(wcindex, wclmn)
    '                        orderdgv.Columns.Insert(pcindex, pclmn)
    '                        orderdgv.Columns.Insert(orderdgv.Columns("ΕΛ. ΑΠΟΘΗΚΗΣ").Index - 1, wclmn2)
    '                        orderdgv.Columns.Insert(orderdgv.Columns("ΕΛ. ΠΑΡΑΓ2").Index - 1, pclmn2)
    '                        wclmn.HeaderText = "ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ"
    '                        wclmn.Name = "ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ"
    '                        'orderdgv.Columns(wcindex).Name = "ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ"
    '                        'orderdgv.Columns(pcindex).Name = "ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ"
    '                        pclmn.HeaderText = "ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ"
    '                        pclmn.Name = "ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ"
    '                        wclmn2.HeaderText = "ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ"
    '                        wclmn2.Name = "ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ"
    '                        'orderdgv.Columns(wcindex + 1).Name = "ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ"
    '                        'orderdgv.Columns(pcindex + 3).Name = "ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ"
    '                        pclmn2.HeaderText = "ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ"
    '                        pclmn2.Name = "ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ"
    '                        'ΠΡΟΣΟΧΗ INDEX ΑΠΟΘΗΚΗΣ +1 ΓΙΑ ΕΛΕΓΧΟ ΦΕΡΜΟΥΙΤ
    '                        'INDEX ΠΑΡΑΓΩΓΗΣ +2 ΓΙΑ ΕΛΕΓΧΟ ΦΕΡΜΟΥΙΤ ΚΑΘΩΣ ΕΚΕΙ ΕΙΝΑΙ 2 ΟΙ ΧΡΗΣΤΕΣ, ΠΕΝΤΟΥΣΗΣ-ΦΙΤΣΙΟΣ
    '                        With orderdgv
    '                            If .Rows.Count <> 0 Then
    '                                .Columns("ID").Visible = False
    '                                .Columns("WARECHECK").Visible = False
    '                                .Columns("PRODCHECK").Visible = False
    '                                .Columns("WARECHECK2").Visible = False
    '                                .Columns("PRODCHECK2").Visible = False
    '                                .Columns("statusnum").Visible = False
    '                            End If
    '                            For i As Integer = 0 To .Rows.Count - 1
    '                                If .Rows(i).Cells("WARECHECK").Value <> 1 And .Rows(i).Cells("WARECHECK").Value <> 2 And .Rows(i).Cells("WARECHECK").Value <> -1 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.issues
    '                                ElseIf .Rows(i).Cells("WARECHECK").Value = 1 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.completed
    '                                ElseIf .Rows(i).Cells("WARECHECK").Value = 2 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.editorder
    '                                ElseIf .Rows(i).Cells("WARECHECK").Value = -1 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.circle_icon_16061
    '                                End If
    '                                If .Rows(i).Cells("WARECHECK2").Value <> 1 And .Rows(i).Cells("WARECHECK2").Value <> 2 And .Rows(i).Cells("WARECHECK2").Value <> -1 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.issues
    '                                ElseIf .Rows(i).Cells("WARECHECK2").Value = 1 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.completed
    '                                ElseIf .Rows(i).Cells("WARECHECK2").Value = 2 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.editorder
    '                                ElseIf .Rows(i).Cells("WARECHECK2").Value = -1 Then
    '                                    .Rows(i).Cells("ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.circle_icon_16061
    '                                End If
    '                                If .Rows(i).Cells("PRODCHECK").Value <> 1 And .Rows(i).Cells("PRODCHECK").Value <> 2 And .Rows(i).Cells("PRODCHECK").Value <> -1 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.issues
    '                                ElseIf .Rows(i).Cells("PRODCHECK").Value = 1 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.completed
    '                                ElseIf .Rows(i).Cells("PRODCHECK").Value = 2 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.editorder
    '                                ElseIf .Rows(i).Cells("PRODCHECK").Value = -1 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ").Value = My.Resources.circle_icon_16061
    '                                End If
    '                                If .Rows(i).Cells("PRODCHECK2").Value <> 1 And .Rows(i).Cells("PRODCHECK2").Value <> 2 And .Rows(i).Cells("PRODCHECK2").Value <> -1 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.issues
    '                                ElseIf .Rows(i).Cells("PRODCHECK2").Value = 1 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.completed
    '                                ElseIf .Rows(i).Cells("PRODCHECK2").Value = 2 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.editorder
    '                                ElseIf .Rows(i).Cells("PRODCHECK2").Value = -1 Then
    '                                    .Rows(i).Cells("ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ").Value = My.Resources.circle_icon_16061
    '                                End If

    '                            Next

    '                            If .RowCount > 0 AndAlso .Rows(vrow).Visible Then
    '                                orderdgv.FirstDisplayedScrollingRowIndex = vrow
    '                            End If
    '                            If issorted > -1 Then

    '                                .Sort(orderdgv.Columns(issorted), SetSortOrderorderdgv)
    '                            End If

    '                        End With
    '                    End Using
    '                End Using
    '            End Using
    '        End Using
    '        Dim f As New Font("verdana", dgvfontsize, GraphicsUnit.Pixel)
    '        orderdgv.DefaultCellStyle.Font = f

    '    Catch EX As Exception
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try
    'End Sub

    Private Sub orderdgv_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles orderdgv.DataBindingComplete
        Try
            nodistribute = False
            'If DataGridView1.Rows.Count > 0 Then
            With orderdgv
                Dim stc As New DatagridviewStackedProgressColumn
                stc.DefaultCellStyle.NullValue = Nothing
                stc.Name = "Status2"
                stc.HeaderText = "Κατάσταση"
                If Not .Columns.Contains("Status2") Then
                    .Columns.Insert(0, stc)
                    .Columns(0).Name = "Status2"

                    'DataGridView1.Columns(0).Width = 20
                End If

                'For i As Integer = 0 To .Rows.Count - 1
                '    If IsDBNull(.Rows(i).Cells("statusnum").Value) OrElse (.Rows(i).Cells("statusnum").Value = 0) OrElse (.Rows(i).Cells("statusnum").Value = 1) Then
                '        .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                '    ElseIf (.Rows(i).Cells("statusnum").Value >= 2 And .Rows(i).Cells("statusnum").Value < 6) Then
                '        .Rows(i).DefaultCellStyle.BackColor = Color.Khaki
                '    ElseIf (.Rows(i).Cells("statusnum").Value >= 6) Then
                '        .Rows(i).DefaultCellStyle.BackColor = Color.PaleGreen
                '    End If
                'Next
                For i As Integer = 0 To .Columns.Count - 1
                    If .Columns(i).HeaderText = "DP" Then
                        .Columns(i).HeaderText = "Δ ΠΡΓ"
                        .Columns(i).Width = 30

                    End If
                    If .Columns(i).HeaderText = "DA" Then
                        .Columns(i).HeaderText = "Δ ΑΠΘ"
                        .Columns(i).Width = 30
                    End If
                    If .Columns(i).HeaderText = "DAP" Then
                        .Columns(i).HeaderText = "Δ Α-Π"
                        .Columns(i).Width = 30
                    End If
                    If .Columns(i).HeaderText = "FAP" Then
                        .Columns(i).HeaderText = "Φ Α-Π"
                        .Columns(i).Width = 30
                    End If
                    If .Columns(i).HeaderText = "FA" Then
                        .Columns(i).HeaderText = "Φ ΑΠΘ"
                        .Columns(i).Width = 30
                    End If
                    If .Columns(i).HeaderText = "FP" Then
                        .Columns(i).HeaderText = "Φ ΠΡΓ"
                        .Columns(i).Width = 30
                    End If
                    If .Columns(i).HeaderText = "A" Then
                        .Columns(i).HeaderText = "ΑΝΤΛΚ"
                        .Columns(i).Width = 30
                    End If
                    If .Columns(i).HeaderText = "R" Then
                        .Columns(i).HeaderText = "ΠΡΤΣΝ"
                        .Columns(i).Width = 30
                    End If
                    If .Columns(i).HeaderText = "F" Then
                        .Columns(i).HeaderText = "#"
                        .Columns(i).Width = 20
                    End If
                Next
                For i As Integer = 0 To .Rows.Count - 1
                    .Rows(i).DefaultCellStyle.BackColor = Color.PaleGreen
                    If .Rows(i).Cells("DP").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("DP").Style.BackColor = Color.Yellow
                    End If
                    If .Rows(i).Cells("DA").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("DA").Style.BackColor = Color.Yellow
                    End If
                    If .Rows(i).Cells("FA").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("FA").Style.BackColor = Color.Yellow
                    End If
                    If .Rows(i).Cells("FP").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("FP").Style.BackColor = Color.Yellow
                    End If
                    If .Rows(i).Cells("FAP").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("FAP").Style.BackColor = Color.Yellow
                    End If
                    If .Rows(i).Cells("DAP").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("DAP").Style.BackColor = Color.Yellow
                    End If
                    If .Rows(i).Cells("R").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("R").Style.BackColor = Color.Yellow
                    End If
                    If .Rows(i).Cells("A").Value = "ΝΑΙ" Then
                        .Rows(i).Cells("A").Style.BackColor = Color.Yellow
                    End If
                    If Not IsDBNull(.Rows(i).Cells("F").Value) AndAlso .Rows(i).Cells("F").Value = "N" Then
                        .Rows(i).Cells("F").Style.BackColor = Color.Red
                    End If
                    If activeuser = "PHILIPPOU E." Or activeuser = "KASIR" Or activeuser = "KAPSANIS" Then
                        If .Rows(i).Cells("statusnum").Value = 0 Or .Rows(i).Cells("statusnum").Value = 1 Then
                            .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                        ElseIf .Rows(i).Cells("statusnum").Value <> 0 And .Rows(i).Cells("statusnum").Value <> 1 And .Rows(i).Cells("statusnum").Value < 6 And (.Rows(i).Cells("DA").Value = "ΝΑΙ" Or .Rows(i).Cells("FA").Value = "ΝΑΙ") Then
                            .Rows(i).DefaultCellStyle.BackColor = Color.Khaki
                        ElseIf .Rows(i).Cells("statusnum").Value >= 6 And (.Rows(i).Cells("DA").Value = "ΝΑΙ" Or .Rows(i).Cells("FA").Value = "ΝΑΙ") Then
                            Dim check As Boolean = True
                            For j As Integer = 0 To DataGridView1.Rows.Count - 1
                                If DataGridView1.Rows(j).Cells("ftrid").Value = .Rows(i).Cells("id").Value And Not IsDBNull(DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value) AndAlso
                                (DataGridView1.Rows(j).Cells("ΥΠΟΛ.").Value <> 0 And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 1) = "1" _
                                And (DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α")) Then
                                    check = False
                                    Exit For
                                End If
                            Next
                            If Not check Then
                                .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                            End If
                        End If
                    ElseIf activeuserdpt = "BP" Then
                        If .Rows(i).Cells("DP").Value = "ΝΑΙ" Or .Rows(i).Cells("DAP").Value = "ΝΑΙ" Then
                            If .Rows(i).Cells("statusnum").Value < 6 Then
                                .Rows(i).DefaultCellStyle.BackColor = Color.Khaki
                            Else
                                Dim check As Boolean = True
                                For j As Integer = 0 To DataGridView1.Rows.Count - 1
                                    If DataGridView1.Rows(j).Cells("ftrid").Value = .Rows(i).Cells("id").Value And Not IsDBNull(DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value) AndAlso
                                (DataGridView1.Rows(j).Cells("ΥΠΟΛ.").Value <> 0 And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) = "102" _
                                And (DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Π" Or DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α-Π")) Then
                                        check = False
                                        Exit For
                                    End If
                                Next
                                If Not check Then
                                    .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                                End If
                            End If
                        End If
                    ElseIf activeuserdpt = "PRD" Then
                        If activeuser = "PENTOUSIS" Then
                            If .Rows(i).Cells("statusnum").Value = 4 Or .Rows(i).Cells("statusnum").Value = 3 And (.Rows(i).Cells("DP").Value = "ΝΑΙ" Or .Rows(i).Cells("DAP").Value = "ΝΑΙ") Then
                                .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                            ElseIf .Rows(i).Cells("DP").Value = "ΝΑΙ" Or .Rows(i).Cells("DAP").Value = "ΝΑΙ" Then
                                Dim check As Boolean = True
                                For j As Integer = 0 To DataGridView1.Rows.Count - 1
                                    If DataGridView1.Rows(j).Cells("ftrid").Value = .Rows(i).Cells("id").Value And Not IsDBNull(DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value) AndAlso
                                (DataGridView1.Rows(j).Cells("ΥΠΟΛ.").Value <> 0 And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) = "102" _
                                And (DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Π" Or DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α-Π")) Then
                                        check = False
                                        Exit For
                                    End If
                                Next
                                If Not check Then
                                    .Rows(i).DefaultCellStyle.BackColor = Color.Khaki
                                End If
                            End If
                        ElseIf activeuser = "FITSIOS" Then
                            'If .Rows(i).Cells("FP").Value = "ΝΑΙ" Or .Rows(i).Cells("FAP").Value = "ΝΑΙ" Then
                            If .Rows(i).Cells("statusnum").Value = 4 Or .Rows(i).Cells("statusnum").Value = 3 And (.Rows(i).Cells("FP").Value = "ΝΑΙ" Or .Rows(i).Cells("FAP").Value = "ΝΑΙ") Then
                                .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                            ElseIf .Rows(i).Cells("FP").Value = "ΝΑΙ" Or .Rows(i).Cells("FAP").Value = "ΝΑΙ" Then
                                Dim check As Boolean = True
                                For j As Integer = 0 To DataGridView1.Rows.Count - 1
                                    If DataGridView1.Rows(j).Cells("ftrid").Value = .Rows(i).Cells("id").Value And Not IsDBNull(DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value) AndAlso
                                (DataGridView1.Rows(j).Cells("ΥΠΟΛ.").Value <> 0 And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 1) = "1" And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) <> "102" _
                                And (DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Π" Or DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α-Π")) Then
                                        check = False
                                        Exit For
                                    End If
                                Next
                                If Not check Then
                                    .Rows(i).DefaultCellStyle.BackColor = Color.Khaki
                                End If
                            End If
                            ' End If
                        End If
                    ElseIf activeuserdpt = "EX" Then
                        If .Rows(i).Cells("statusnum").Value < 6 Then
                            .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                        End If
                    ElseIf activeuserdpt = "BL" Then
                        If .Rows(i).Cells("FP").Value = "ΝΑΙ" Or .Rows(i).Cells("FAP").Value = "ΝΑΙ" Then
                            If .Rows(i).Cells("statusnum").Value < 6 Then
                                .Rows(i).DefaultCellStyle.BackColor = Color.Khaki
                            Else
                                Dim check As Boolean = True
                                For j As Integer = 0 To DataGridView1.Rows.Count - 1
                                    If DataGridView1.Rows(j).Cells("ftrid").Value = .Rows(i).Cells("id").Value And Not IsDBNull(DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value) AndAlso
                                (DataGridView1.Rows(j).Cells("ΥΠΟΛ.").Value <> 0 And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 1) = "1" And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) <> "102" _
                                And (DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Π" Or DataGridView1.Rows(j).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α-Π")) Then
                                        check = False
                                        Exit For
                                    End If
                                Next
                                If Not check Then
                                    .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                                End If
                            End If

                        End If
                    ElseIf activeuserdpt = "SP" Then
                        If .Rows(i).Cells("A").Value = "ΝΑΙ" Or .Rows(i).Cells("R").Value = "ΝΑΙ" Or .Rows(i).Cells("DA").Value = "ΝΑΙ" Or .Rows(i).Cells("FA").Value = "ΝΑΙ" Then
                            If .Rows(i).Cells("statusnum").Value < 6 Then
                                .Rows(i).DefaultCellStyle.BackColor = Color.Khaki
                            Else
                                Dim check As Boolean = True
                                For j As Integer = 0 To DataGridView1.Rows.Count - 1
                                    If DataGridView1.Rows(j).Cells("ftrid").Value = .Rows(i).Cells("id").Value AndAlso
                                (DataGridView1.Rows(j).Cells("ΥΠΟΛ.").Value <> 0 And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 1) = "2" And DataGridView1.Rows(j).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) <> "202") Then
                                        check = False
                                        Exit For
                                    End If
                                Next
                                If Not check Then
                                    .Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                                End If
                            End If

                        End If
                    End If
                Next
                For i As Integer = 1 To .Rows.Count - 1
                    If orderdgv.Rows(i).Cells("ΠΕΛ").Value <> .Rows(i - 1).Cells("ΠΕΛ").Value And Not nodistribute Then
                        nodistribute = True
                    End If
                Next
                For Each c As DataGridViewColumn In orderdgv.Columns
                    Dim s = "Odgv|" + c.Name
                    Dim f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = s)
                    If f IsNot Nothing Then
                        c.Width = CInt(f.value)
                    End If
                Next
            End With
            'End If
            comboSource.Clear()
            If nodistribute Then
                Button2.Enabled = False
                Button22.Enabled = False
                Button17.Enabled = False
                'button18.enabled = false

                'ComboBox3.Items.Clear()
                ComboBox3.Enabled = False

            Else
                CheckBox16.Checked = False
                comboSource.Add(" ", 0)
                For Each c As DataGridViewRow In orderdgv.Rows
                    If c.Cells("ΠΑΡ").Value.ToString.Contains("BAC") Then
                        comboSource.Add(c.Cells("ΠΑΡ").Value.ToString, c.Cells("id").Value.ToString)
                    Else
                        comboSource.Add(c.Cells("ΠΑΡ").Value.ToString.TrimStart("0"c), c.Cells("id").Value.ToString)

                    End If

                Next
                ComboBox3.DataSource = New BindingSource(comboSource, Nothing)
                ComboBox3.DisplayMember = "Key"
                ComboBox3.ValueMember = "Value"
                If ComboBox3.Items.Count = 2 Then
                    ComboBox3.SelectedIndex = 1
                End If
                ComboBox3.Enabled = True
                Button2.Enabled = True
                Button22.Enabled = True
                Button17.Enabled = True
                'button18.enabled = true
                If orderdgv.RowCount > 0 Then
                    If conn.State = ConnectionState.Closed Then
                        conn.Open()
                    End If
                    Using com As New SqlCommand("select cusid from fintrade where id=" + orderdgv.Rows(0).Cells("id").Value.ToString, conn)
                        ' conn.Open()
                        CUSID = com.ExecuteScalar
                        'conn.Close()

                    End Using
                    Using com As New SqlCommand("select colidsalesman from customer where id=" + CUSID.ToString, conn)
                        'conn.Open()
                        relsalesman = com.ExecuteScalar
                        'conn.Close()

                    End Using
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                    If IsDBNull(relsalesman) Then
                        Throw New System.Exception("Δεν υπάρχει συσχέτιση του πελάτη με πωλητή. Επικοινωνήστε με τον διαχειριστή.")
                    End If
                End If
            End If

            For Each r As DataGridViewRow In orderdgv.Rows
                r.Cells("Status2").Value = r.Cells("red").Value.ToString + "/" + r.Cells("black").Value.ToString + "/" + r.Cells("blue").Value.ToString + "/" + r.Cells("lightgreen").Value.ToString + "/" + r.Cells("green").Value.ToString + "/" + r.Cells("gold").Value.ToString
            Next
            order_columns(1)
        Catch EX As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub order_columns(ByVal type As Integer)

        If type = 1 Then
            Dim foo = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "odgvorder")
            Dim order As New List(Of String)
            If foo IsNot Nothing Then
                order.AddRange(foo.value.ToString.Split(","c))
            End If
            With orderdgv
                For i As Integer = 0 To .Columns.Count - 1
                    If order.Contains(.Columns(i).Name) Then
                        .Columns(i).DisplayIndex = order.IndexOf(.Columns(i).Name)
                        Using f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "Odgv|" + .Columns(i).Name)
                            If f IsNot Nothing Then
                                .Columns(i).Width = CInt(f.value)
                            End If
                        End Using
                    End If
                Next
            End With
        ElseIf type = 2 Then
            Dim foo = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "dgv1order")
            Dim order As New List(Of String)
            If foo IsNot Nothing Then
                order.AddRange(foo.value.ToString.Split(","c))
            End If
            With DataGridView1
                For i As Integer = 0 To .Columns.Count - 1
                    If order.Contains(.Columns(i).Name) Then
                        .Columns(i).DisplayIndex = order.IndexOf(.Columns(i).Name)
                    End If
                Next
                .Columns("Status").Visible = True
                .Columns("Status").DisplayIndex = 0
            End With
        End If
    End Sub

    Private Function return_ftrid_status(ByVal ftrid As Integer)
        Dim rvalue As Integer = -1
        For i As Integer = 0 To orderdgv.Rows.Count - 1
            If orderdgv.Rows(i).Cells("id").Value = ftrid Then
                If IsDBNull(orderdgv.Rows(i).Cells("statusnum").Value) Then
                    rvalue = -1
                Else

                    rvalue = orderdgv.Rows(i).Cells("statusnum").Value

                End If
            End If
        Next
        Return rvalue

    End Function
    Dim TRADECODEMASK As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            My.Settings.downloading = True
            Cursor.Current = ExtCursor1.Cursor
            b1s = sender
            b1e = e
            orderdgv.SuspendLayout()
            DataGridView2.SuspendLayout()
            Button15.Enabled = False
            Button13.Enabled = False
            Button3.Enabled = False
            Panel4.Enabled = False
            Panel3.Enabled = False
            Button1.Enabled = False
            FlowLayoutPanel1.Visible = False
            If Not CheckBox16.Checked Then
                Dim pic As New PictureBox
                pic.Image = My.Resources.rolling
                pic.SizeMode = PictureBoxSizeMode.CenterImage
                TableLayoutPanel4.Controls.Add(pic, TableLayoutPanel4.GetColumn(FlowLayoutPanel1), TableLayoutPanel4.GetRow(FlowLayoutPanel1))
                pic.Dock = DockStyle.Fill
            End If
            orderdgv.Visible = False
            DataGridView1.Visible = False
            DataGridView2.Visible = False
            Application.DoEvents()
            'If Len(cuscodemaskbox.Text) = 0 And Len(plfilter) = 0 And callnamescbox.SelectedIndex <= 0 And Len(cusnamemaskbox.Text) = 0 And DateTimePicker2.Value.Subtract(DateTimePicker1.Value).TotalDays > 7 And Len(TextBox1.Text) = 0 Then
            '    Throw New System.Exception("Εαν το εύρος ημερομηνιών ξεπερνάει τις 7 ημέρες, πρέπει να συμπληρώσετε απαραίτητα ένα εκ των κωδικό πελάτη, συντομογραφία ή επωνυμία πελάτη.")
            'End If
            Dim stopWatch As New Stopwatch()
            stopWatch.Start()
            p.SetValue(Me.DataGridView1, True, Nothing)
            Dim CUSCODEMASK As String = Regex.Replace(cuscodemaskbox.Text, "[^0-9\*\.]", "")
            CUSCODEMASK = Regex.Replace(CUSCODEMASK, "[*]", "%")
            If CUSCODEMASK.IndexOf("%") = -1 Then
                CUSCODEMASK = CUSCODEMASK + "%"
            End If
            Dim itemmask As String = Regex.Replace(TextBox2.Text, "[^0-9\*]", "")
            itemmask = Regex.Replace(itemmask, "[*]", "%")
            If itemmask.IndexOf("%") = -1 Then
                itemmask = itemmask + "%"
            End If
            'Dim motheromask As String = Regex.Replace(TextBox4.Text, "[^0-9\*]", "")
            'motheromask = Regex.Replace(motheromask, "[*]", "%")
            'If motheromask.IndexOf("%") = -1 Then
            '    motheromask = motheromask + "%"
            'End If
            Dim CUSNAMEMASK As String = Regex.Replace(cusnamemaskbox.Text, "[*]", "%")
            If CUSNAMEMASK.IndexOf("%") = -1 Then
                CUSNAMEMASK = "%" + CUSNAMEMASK + "%"
            End If
            TRADECODEMASK = Regex.Replace(TextBox1.Text, "[*]", "%")
            If TRADECODEMASK.IndexOf("%") = -1 Then
                TRADECODEMASK = "%" + TRADECODEMASK + "%"
            End If
            Dim cmd As String = ""
            Using com As New SqlCommand
                If CheckBox14.Checked = True Then
                    cmd = "(Select DISTINCT stl.FTRID
                                 from storetradelines stl  left join material mtr on mtr.id=stl.iteid
                                 LEFT JOIN FINTRADE FTR ON FTR.ID=stl.FTRID
                                 
                                 LEFT join Z_PACKER_FULLITEMQUANTITIES z on z.STLID=stl.id
                                 left join Z_PACKER_BACKORDERED_ITEMS z2 on z2.RELSTLID=stl.ID
                                 left join CUSTOMER cus on ftr.CUSID=cus.ID 
                                    left join SALESMAN SM ON SM.ID=FTR.COLIDSALESMAN
                                 where   FTR.DSRID IN (9000,9008) AND (isnull(z.quantity,0)+isnull(z2.PRIMARYQTY,0)<>stl.PRIMARYQTY)"
                    Dim fnamemask As String = "%"
                    If callnamescbox.SelectedIndex > 0 Then
                        cmd = cmd + " And cus.fathername Like @callname"

                        com.Parameters.Add("@callname", SqlDbType.VarChar, 50).Value = callnamescbox.Items(callnamescbox.SelectedIndex)
                    ElseIf callnamescbox.Text <> "" Then
                        fnamemask = Regex.Replace(callnamescbox.Text, "[^0-9\*]", "")
                        fnamemask = Regex.Replace(fnamemask, "[*]", "%")
                        If fnamemask.IndexOf("%") = -1 Then
                            fnamemask = fnamemask + "%"
                        End If
                        cmd = cmd + " AND cus.fathername LIKE @fnamemask"

                        com.Parameters.Add("@fnamemask", SqlDbType.VarChar, 50).Value = fnamemask
                    End If
                    If ComboBox4.SelectedIndex <> 0 Then
                        cmd = cmd + " and sm.id =" + ComboBox4.SelectedValue.ToString
                    End If
                ElseIf CheckBox15.Checked Then
                    cmd = "(Select DISTINCT stl.FTRID
                                 from storetradelines stl   left join material mtr on mtr.id=stl.iteid 
                                 LEFT JOIN FINTRADE FTR ON FTR.ID=stl.FTRID
                                 
                                 LEFT join Z_PACKER_FULLITEMQUANTITIES z on z.STLID=stl.id
                                 left join Z_PACKER_BACKORDERED_ITEMS z2 on z2.RELSTLID=stl.ID
                                    left join CUSTOMER cus on ftr.CUSID=cus.ID 
                                left join tbl_packerordercheck t on t.ftrid=ftr.id
                            left join SALESMAN SM ON SM.ID=FTR.COLIDSALESMAN
                                 where  FTR.DSRID IN (9000,9008) AND t.status<>12 and stl.iteid in (select id from material where code like @itemmask)"
                    Dim fnamemask As String = "%"

                    If callnamescbox.SelectedIndex > 0 Then
                        cmd = cmd + " And cus.fathername Like @callname"

                        com.Parameters.Add("@callname", SqlDbType.VarChar, 50).Value = callnamescbox.Items(callnamescbox.SelectedIndex)
                    ElseIf callnamescbox.Text <> "" Then
                        fnamemask = Regex.Replace(callnamescbox.Text, "[^0-9\*]", "")
                        fnamemask = Regex.Replace(fnamemask, "[*]", "%")
                        If fnamemask.IndexOf("%") = -1 Then
                            fnamemask = fnamemask + "%"
                        End If
                        cmd = cmd + " AND cus.fathername LIKE @fnamemask"

                        com.Parameters.Add("@fnamemask", SqlDbType.VarChar, 50).Value = fnamemask
                    End If
                    If ComboBox4.SelectedIndex <> 0 Then
                        cmd = cmd + " and sm.id =" + ComboBox4.SelectedValue.ToString
                    End If
                Else

                    cmd = "(select DISTINCT FTR.ID
                                 from storetradelines stl left join material mtr on mtr.id=stl.iteid 
                                 left join fintrade ftr on ftr.id=stl.ftrid 
                                 left join CUSTOMER cus on ftr.CUSID=cus.ID 
                                 
                                 left join fintrade ftr2 on ftr2.id=ftr.sc_relftrid  
                                    left join tbl_palletlines pl on pl.stlid=stl.id 
                                    left join tbl_palletheaders ph on ph.id=pl.palletid
                                    left join tbl_packinglists pcl on pcl.id=ph.plid  
                                    left join salesman sm on sm.id=ftr.colidsalesman
                                    left join tbl_packerordercheck t on t.ftrid=ftr.id
                                 where FTR.APPROVED=1 AND ftr.DSRID IN (9000,9008)  And CUS.CODE like @cuscodemask And CUS.NAME like @cusnamemask and ftr.FTRDATE>=@datetime1 and ftr.FTRDATE<=@datetime2 AND (dbo.get_tradecode(ftr.id) LIKE @tradecodemask or dbo.get_tradecode(ftr2.id) like @tradecodemask) AND mtr.code like @itemmask"

                    com.Parameters.Add("@cuscodemask", SqlDbType.VarChar, 50).Value = CUSCODEMASK
                    com.Parameters.Add("@cusnamemask", SqlDbType.VarChar, 50).Value = CUSNAMEMASK
                    com.Parameters.Add("@datetime1", SqlDbType.DateTime).Value = DateTimePicker1.Value
                    com.Parameters.Add("@datetime2", SqlDbType.DateTime).Value = DateTimePicker2.Value
                    com.Parameters.Add("@tradecodemask", SqlDbType.VarChar, 50).Value = TRADECODEMASK

                    'If Not motheromask = "%" Then
                    '    cmd = cmd + " And (ftr2.tradecode+FTR.M_RELFTRIDINC Like @motheromask or ftr.tradecode like @motheromask)"

                    '    com.Parameters.Add("@motheromask", SqlDbType.VarChar, 50).Value = motheromask
                    'End If
                    Dim fnamemask As String = "%"
                    If callnamescbox.SelectedIndex > 0 Then
                        cmd = cmd + " And cus.fathername Like @callname"

                        com.Parameters.Add("@callname", SqlDbType.VarChar, 50).Value = callnamescbox.Items(callnamescbox.SelectedIndex)
                    ElseIf callnamescbox.Text <> "" Then
                        fnamemask = Regex.Replace(callnamescbox.Text, "[^0-9\*]", "")
                        fnamemask = Regex.Replace(fnamemask, "[*]", "%")
                        If fnamemask.IndexOf("%") = -1 Then
                            fnamemask = fnamemask + "%"
                        End If
                        cmd = cmd + " AND cus.fathername LIKE @fnamemask"

                        com.Parameters.Add("@fnamemask", SqlDbType.VarChar, 50).Value = fnamemask
                    End If
                    If plfilter <> "" Then
                        cmd = cmd + " and pcl.code like @plfilter"

                        com.Parameters.Add("@plfilter", SqlDbType.VarChar, 50).Value = plfilter
                    End If
                    If pfilter <> "" Then
                        cmd = cmd + " and ph.code like @pfilter"

                        com.Parameters.Add("@pfilter", SqlDbType.VarChar, 50).Value = pfilter
                    End If
                    If ComboBox4.SelectedIndex <> 0 Then
                        cmd = cmd + " and sm.id =" + ComboBox4.SelectedValue.ToString
                    End If
                    If Not IsNothing(plcdfilterfrom) Then
                        cmd = cmd + " and pcl.closedate >= @plcdfilterfrom"

                        com.Parameters.Add("@plcdfilterfrom", SqlDbType.DateTime).Value = plcdfilterfrom
                    End If
                    If Not IsNothing(plcdfilterto) Then
                        cmd = cmd + " and pcl.closedate <=@plcdfilterto"

                        com.Parameters.Add("@plcdfilterto", SqlDbType.DateTime).Value = plcdfilterto

                    End If
                    If Not IsNothing(plodfilterfrom) Then
                        cmd = cmd + " and pcl.opendate >=@plodfilterfrom"

                        com.Parameters.Add("@plodfilterfrom", SqlDbType.DateTime).Value = plodfilterfrom
                    End If
                    If Not IsNothing(plodfilterto) Then
                        cmd = cmd + " and pcl.opendate <=@plodfilterto "

                        com.Parameters.Add("@plodfilterto", SqlDbType.DateTime).Value = plodfilterto
                    End If
                    If Not IsNothing(pcdfilterfrom) Then
                        cmd = cmd + " and ph.closedate >=@pcdfilterfrom"

                        com.Parameters.Add("@pcdfilterfrom", SqlDbType.DateTime).Value = pcdfilterfrom
                    End If
                    If Not IsNothing(pcdfilterto) Then
                        cmd = cmd + " and ph.closedate <=@pcdfilterto "

                        com.Parameters.Add("@pcdfilterto", SqlDbType.DateTime).Value = pcdfilterto
                    End If
                    If Not IsNothing(podfilterfrom) Then
                        cmd = cmd + " and ph.opendate >=@podfilterfrom"

                        com.Parameters.Add("@podfilterfrom", SqlDbType.DateTime).Value = podfilterfrom
                    End If
                    If Not IsNothing(podfilterto) Then
                        cmd = cmd + " and ph.opendate <=@podfilterto"

                        com.Parameters.Add("@podfilterto", SqlDbType.DateTime).Value = podfilterto
                    End If
                    If CheckBox17.Checked Then
                        cmd = cmd + " and t.status=12"
                    End If
                End If
                cmd = cmd + ")"
                com.Parameters.Add("@itemmask", SqlDbType.VarChar, 50).Value = itemmask
                com.Connection = conn
                com.CommandText = cmd
                conn.Open()
                Dim list As New List(Of Integer)
                If CheckBox14.Checked Or CheckBox15.Checked Then
                    Using rd As SqlDataReader = com.ExecuteReader
                        If rd.HasRows Then
                            Do While rd.Read()
                                list.Add(rd.Item("FTRID"))
                            Loop
                        Else
                            DataGridView1.Visible = True
                            DataGridView2.Visible = True
                            orderdgv.Visible = True

                            For Each co As Control In TableLayoutPanel4.Controls
                                If Not IsNothing(TryCast(co, PictureBox)) Then
                                    co.Dispose()
                                End If
                            Next
                            FlowLayoutPanel1.Visible = True
                            Throw New System.Exception("Η αναζήτηση σας δεν επέστρεψε αποτελέσματα!")
                        End If
                        relftrids = String.Join(",", list.ToArray)
                    End Using
                Else
                    Using rd As SqlDataReader = com.ExecuteReader
                        If rd.HasRows Then
                            Do While rd.Read()
                                list.Add(rd.Item("ID"))
                            Loop
                        Else
                            DataGridView1.Visible = True
                            DataGridView2.Visible = True
                            orderdgv.Visible = True
                            For Each co As Control In TableLayoutPanel4.Controls
                                If Not IsNothing(TryCast(co, PictureBox)) Then
                                    co.Dispose()
                                End If
                            Next
                            FlowLayoutPanel1.Visible = True
                            Throw New System.Exception("Η αναζήτηση σας δεν επέστρεψε αποτελέσματα!")
                        End If
                        relftrids = String.Join(",", list.ToArray)
                    End Using
                End If

                conn.Close()

                If list.Count > 50 Then
                    Label3.Text = "Φορτώνει " + list.Count.ToString + " παραγγελίες. Θα πάρει λίγη ώρα, υπομονή!"
                    Label3.Visible = True
                    Label3.Refresh()
                End If
                Application.DoEvents()
                Dim cmd_N As String = ""
                If hidedispatch = "true" Then
                    cmd_N = "select [iteid]      ,[ΠΟΣ]  ,[ΥΠΟΛ.],black,blue,lightgreen,green,gold  ,[ΑΠΟΔΕΚΤΗΣ],comments as 'ΠΡΟΣΩΠ ΣΧΟΛΙΑ' ,[ΚΩΔΙΚΟΣ]   ,SUBCODE1 AS 'ΕΝΑΛ ΚΩΔ',SUBCODE2 AS 'ΠΑΛ ΚΩΔ'   ,[ΠΕΡΙΓΡΑΦΗ]      ,[ΠΑΡ]      ,[ΚΩΔΠΕΛ]      ,[ΠΕΛ]      ,[Z_PACKER_ITEMSBROWSER].stlid      ,[ftrid]           ,[BACKORDER]      ,[SECJUSTIFICATION] AS 'ΣΧΟΛΙΑ ΕΞΑΓ'
      ,[PRODCOMMENTS] AS 'ΣΧΟΛΙΑ ΠΑΡΑΓ'      ,[WARECOMMENTS] AS 'ΣΧΟΛΙΑ ΑΠΟΘ'      ,[PACKCOMMENTS] AS 'ΣΧΟΛΙΑ ΣΥΣΚ'  FROM [Z_PACKER_ITEMSBROWSER] left join tbl_packeruserorderlinecomments t on t.stlid=[Z_PACKER_ITEMSBROWSER].stlid and t.userid=" + activeuserid.ToString + "
                                 where  ftrid IN (" + relftrids + ")  "
                Else
                    cmd_N = "select [iteid]      ,[ΠΟΣ]  ,[ΥΠΟΛ.],black,blue,lightgreen,green,gold  ,[ΑΠΟΔΕΚΤΗΣ],comments as 'ΠΡΟΣΩΠ ΣΧΟΛΙΑ'  ,M_DISPATCHDATE as 'ΗΜ/ΝΙΑ ΑΠΟΣΤΟΛΗΣ' ,[ΚΩΔΙΚΟΣ]   ,SUBCODE1 AS 'ΕΝΑΛ ΚΩΔ' ,SUBCODE2 AS 'ΠΑΛ ΚΩΔ'  ,[ΠΕΡΙΓΡΑΦΗ]      ,[ΠΑΡ]      ,[ΚΩΔΠΕΛ]      ,[ΠΕΛ]      ,[Z_PACKER_ITEMSBROWSER].stlid      ,[ftrid]           ,[BACKORDER]      ,[SECJUSTIFICATION] AS 'ΣΧΟΛΙΑ ΕΞΑΓ'
      ,[PRODCOMMENTS] AS 'ΣΧΟΛΙΑ ΠΑΡΑΓ'      ,[WARECOMMENTS] AS 'ΣΧΟΛΙΑ ΑΠΟΘ'      ,[PACKCOMMENTS] AS 'ΣΧΟΛΙΑ ΣΥΣΚ'  FROM [Z_PACKER_ITEMSBROWSER] left join tbl_packeruserorderlinecomments t on t.stlid=[Z_PACKER_ITEMSBROWSER].stlid and t.userid=" + activeuserid.ToString + "
                                 where  ftrid IN (" + relftrids + ")  "
                End If

                com.CommandText = cmd_N
                com.Connection = conn
                dgv1cmd = com
                datagridview1_refresh()
            End Using


            Dim cmd2 As String = ""
            If CheckBox1.Checked = True Then
                cmd2 = "SELECT     pl.id, pl.CODE AS ΚΩΔΙΚΟΣ, pl.OPENDATE AS [ΗΜ/ΝΙΑ ΔΗΜ], pud.USERNAME AS [ΔΗΜ ΑΠΟ], ISNULL(pl.STATUS, 0) AS ST, (SELECT TOP 1 RELORDERS FROM Z_PACKER_TRADECODES_PER_PLIST2 WHERE PLID=PL.ID) AS 'ΣΧΕΤΙΚΕΣ ΠΑΡ'
	                           ,
(select COUNT(id) from TBL_PALLETHEADERS where PLID=pl.id) as 'ΠΛΗΘΟΣ ΠΑΛΕΤΩΝ',(select isnull(fathername,'') from customer where id=(select top 1 cusid from tbl_palletheaders where plid=pl.id)) as 'ΠΕΛΑΤΗΣ' FROM         dbo.TBL_PACKINGLISTS AS pl LEFT OUTER JOIN
                                                  dbo.tbl_packeruserdata AS pud ON pl.CREATEUSER = pud.ID "
            Else

                cmd2 = "SELECT     pl.id, pl.CODE AS ΚΩΔΙΚΟΣ, pl.OPENDATE AS [ΗΜ/ΝΙΑ ΔΗΜ], pud.USERNAME AS [ΔΗΜ ΑΠΟ], ISNULL(pl.STATUS, 0) AS ST, 
(SELECT TOP 1 RELORDERS FROM Z_PACKER_TRADECODES_PER_PLIST2 WHERE PLID=pl.ID) AS 'ΣΧΕΤΙΚΕΣ ΠΑΡ'  ,
(select COUNT(id) from TBL_PALLETHEADERS where PLID=pl.id) as 'ΠΛΗΘΟΣ ΠΑΛΕΤΩΝ', (select isnull(fathername,'') from customer where id=(select top 1 cusid from tbl_palletheaders where plid=pl.id)) as 'ΠΕΛΑΤΗΣ'
FROM         dbo.TBL_PACKINGLISTS AS pl LEFT OUTER JOIN   dbo.tbl_packeruserdata AS pud ON pl.CREATEUSER = pud.ID 
 WHERE pl.id in (SELECT DISTINCT id from tbl_packinglists where id not in (select distinct isnull(plid,0) from tbl_palletheaders) ) 
 OR  pl.ID IN (SELECT DISTINCT PH.plid FROM TBL_PALLETHEADERS PH LEFT JOIN TBL_PALLETLINES PLS ON PH.ID=PLS.PALLETID WHERE PLS.FTRID IN (" + relftrids + ")) or (pl.plcusid in (select cusid from fintrade where id in (" + relftrids + ")) and pl.status<>1)" 'ΠΡΟΣΟΧΗ ΣΤΟ ISNULL


            End If

            dgv2cmd = cmd2
            Dim cmd3 As String = ""

            cmd3 = "SELECT [ITEID]      ,[ftrid]  ,[TRADECODE]      ,[quantity]  FROM [dbo].[Z_PACKER_FULLITEMQUANTITIES] where ftrid in (" + relftrids + ")"


            dgv3cmd = cmd3
            Dim ordcmd As String = ""
            ''
            ' 
            'ΠΑΛΙΟ QUERY
            '      ordcmd = "SELECT [ID]   ,[statusnum],t.comments as 'ΠΡΟΣΩΠ ΣΧΟΛΙΑ',u.status as 'ΚΑΤΑΣΤ ΧΡΗΣΤΗ' ,z.[STATUS]    ,[ΠΩΛΗΤΗΣ]  ,[ΠΑΡ]      ,[ΠΕΛ], [M_DISPATCHDATE]  AS 'ΗΜ/ΝΙΑ ΑΠΟΣΤΟΛΗΣ'     ,(CAST([ΑΠΟΜΕΝΟΥΝ] AS VARCHAR(10))+'/'+CAST([ΣΥΝ_ΠΟΣΟΤ] AS VARCHAR(10))) AS 'ΑΠΟΜΕΝΟΥΝ',[ΗΜ/ΝΙΑ]  AS 'ΗΜΕΡΟΜΗΝΙΑ ΚΑΤΑΧΩΡΙΣΗΣ'   ,[WARECHECK] ,[WARECHECK2]   ,[ΕΛ. ΑΠΟΘΗΚΗΣ]                   ,[ΗΜ/ΝΙΑ ΕΛ.ΑΠΘ.]
            ',[ΣΧΟΛΙΑ ΑΠΘ]   ,[PRODCHECK] ,[ΕΛ. ΠΑΡΑΓ] ,[PRODCHECK2]        ,[ΕΛ. ΠΑΡΑΓ2]      ,[ΗΜ/ΝΙΑ ΕΛ.ΠΑΡΑΓ.]      ,[ΣΧΟΛΙΑ ΠΑΡΑΓ]      ,[ΣΧΟΛΙΑ ΣΥΣΚΕΥ]  FROM [dbo].[Z_PACKER_ORDERBROWSER] z left join tbl_packeruserordercomments t on t.ftrid=z.id and t.userid=" + activeuserid.ToString + " cross apply [dbo].[ftr_peruserinvolvement](z.id," + activeuserid.ToString + ") u    where ID in (" + relftrids + ")"

            ordcmd = "SELECT [ID]   ,OC.lightgreen,OC.BLUE,OC.GREEN,OC.GOLD,OC.BLACK,OC.RED,[statusnum],F,t.comments as 'ΠΡΟΣΩΠ ΣΧΟΛΙΑ' ,z.[STATUS]   
,[ΠΩΛΗΤΗΣ]  ,[ΠΑΡ]      ,[ΠΕΛ], [M_DISPATCHDATE]  AS 'ΗΜ/ΝΙΑ ΑΠΟΣΤΟΛΗΣ'    ,JUSTIFICATION AS 'ΣΧΟΛΙΑ ΕΞΑΓ' ,(CAST([ΑΠΟΜΕΝΟΥΝ] AS VARCHAR(10))+'/'+CAST([ΣΥΝ_ΠΟΣΟΤ] AS VARCHAR(10))) AS 'ΑΠΟΜΕΝΟΥΝ',[ΗΜ/ΝΙΑ]  AS 'ΗΜΕΡΟΜΗΝΙΑ ΚΑΤΑΧΩΡΙΣΗΣ'   
   ,[ΣΧΟΛΙΑ ΑΠΘ]     ,[ΣΧΟΛΙΑ ΠΑΡΑΓ]      ,[ΣΧΟΛΙΑ ΣΥΣΚΕΥ] ,isnull(Z.FA,'') FA,isnull(Z.FP,'') FP,isnull(Z.FAP,'') FAP,isnull(Z.DA,'') DA,isnull(Z.DP,'') DP,isnull(Z.DAP,'') DAP,isnull(Z.A,'') A,isnull(Z.R,'') R

FROM [dbo].[Z_PACKER_ORDERBROWSER] z left join tbl_packeruserordercomments t on t.ftrid=z.id and t.userid=" + activeuserid.ToString + "  left join PKRVIW_ORDERCOLORS OC ON OC.FTRID=Z.ID  where ID in (" + relftrids + ")"
            ordcmd = ordcmd + " order by [ΠΑΡ] asc"
            orderdgvcmd = ordcmd
            clearpallets()
            datagridview2_refresh()
            orderdgv_refresh()
            If Not CheckBox16.Checked Then
                SplitContainer1.Panel2Collapsed = False
                CheckBox12.Image = My.Resources.icons8_forward_16
                populate_pallets("ftrid")
            Else
                SplitContainer1.Panel2Collapsed = True
                CheckBox12.Image = My.Resources.icons8_back_16
            End If
            Button13.Enabled = True
            Button3.Enabled = True
            'DataGridView1.VirtualMode = True
            'Button12.Enabled = True
            Button13.Enabled = True
            Button3.Enabled = True
            Panel4.Enabled = True
            Panel3.Enabled = True
            stopWatch.Stop()
            Dim ts As TimeSpan = stopWatch.Elapsed
            Dim elapsedTime As String = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10)
            'Label11.Text = "Φορτώθηκαν " + orderdgv.RowCount.ToString + " παραγγελίες, " + DataGridView1.RowCount.ToString + " προϊόντα και " + DataGridView2.RowCount.ToString + " packing lists σε " + elapsedTime + " δευτ/πτα."
            'Dim clmn As New DataGridViewCheckBoxColumn
            'clmn.ReadOnly = False
            'DataGridView1.Columns.Add(clmn)
            'DataGridView2.VirtualMode=True
            orderdgv.ResumeLayout()
            DataGridView2.ResumeLayout()
            Cursor.Current = Cursors.Default
            If rlimit > 0 Then
                rtime = 0
            End If
        Catch EX As Exception
            My.Settings.downloading = False
            Button1.Enabled = True
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            'Dim errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog()
        Finally

        End Try
    End Sub




    Dim newpalletid As Integer = -1
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If ComboBox3.SelectedIndex = 0 Then
                Throw New System.Exception("Επιλέξτε σχετική παραγγελία πρώτα, πάνω δεξιά.")
            End If
            Using pm As New PalletManager(activeuserdpt, activeuser, activeuserid, activeuserdptid, cus_id:=CUSID)
                'pm.GenerateNewPalletCode(DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Value, ComboBox2.Items(ComboBox2.SelectedIndex).ToString)
                'Dim newcodes As New Dictionary(Of String, String)
                'newcodes = pm.AllCodes
                Dim newpalletcode As String = ""
                'Dim newpalletdptcode As String = newcodes("PalletDptCode")
                'Dim key As String = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Key
                'Dim ftrid As String = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Value
                If Not nodistribute Then
                    Dim pallet As New pallettemplate
                    'Dim re As Dictionary(Of String, String) = pm.Calculate_Code_Substrings(ComboBox2.Items(ComboBox2.SelectedIndex), DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Value)
                    Dim d As String = ""
                    If ComboBox2.SelectedIndex > -1 Then
                        d = ComboBox2.Items(ComboBox2.SelectedIndex)
                    End If
                    Using newid As DataTable = pm.Create(1, desired_loc:=d, ftr_id:=DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Value)
                        If newid.Rows(0).Item("ID") <= 0 Then
                            newpalletid = -1
                            pallet.Dispose()
                            Return
                        End If
                        Using S As New SqlCommand("SELECT CODE,LOCID,LOCCODE FROM TBL_PALLETHEADERS WHERE ID=" + newid.Rows(0).Item("ID").ToString, conn)
                            Using DT As New DataTable()
                                conn.Open()
                                Using READER As SqlDataReader = S.ExecuteReader
                                    DT.Load(READER)
                                    pallet.locationID = DT.Rows(0).Item("LOCID")
                                    pallet.locationCode = DT.Rows(0).Item("LOCCODE")
                                    pallet.pallettemplatelabel.Text = DT.Rows(0).Item("CODE")
                                    newpalletcode = DT.Rows(0).Item("CODE")
                                End Using
                                conn.Close()
                            End Using
                        End Using
                        pallet.palletid = newid.Rows(0).Item("ID")
                        FlowLayoutPanel1.Controls.Add(pallet)
                        pallet.pinned = nextpinnedlocation
                        nextpinnedlocation += 1
                        Dim r As DataRow = phdt.NewRow
                        r.Item("id") = newid.Rows(0).Item("ID")
                        r.Item("code") = newpalletcode
                        r.Item("createuser") = activeuserid
                        r.Item("locid") = pallet.locationID
                        r.Item("loccode") = pallet.locationCode
                        r.Item("atlantissalesmanid") = relsalesman.ToString
                        r.Item("cusid") = CUSID.ToString
                        r.Item("printuserid") = 0
                        phdt.Rows.Add(r)
                        pindex.Add(newid.Rows(0).Item("ID"), (phdt.Rows.Count - 1).ToString + ", -1, -1")
                        Application.DoEvents()
                        populate_pallets(newid.Rows(0).Item("ID"), c:=pallet)
                    End Using
                End If
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

    Private Sub DataGridView2_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView2.SelectionChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If Not DataGridView1.SelectedRows.Count > 0 Then
                Throw New System.Exception("Επιλέξτε κάποιο είδος πρώτα!")
            End If
            Dim l As New Dictionary(Of String, Integer)
            For Each r As DataGridViewRow In DataGridView1.SelectedRows
                l.Add("stlid", r.Cells("stlid").Value)
            Next
            Dim f As New ItemDistribution(Cursor.Position.X, Cursor.Position.Y, integer_parameters_equal_:=l)
            f.Owner = Me
            f.Show()
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Sub

    Private Sub FlowLayoutPanel1_MouseEnter(sender As Object, e As EventArgs) Handles FlowLayoutPanel1.MouseEnter
        FlowLayoutPanel1.Focus()
    End Sub

    Private Sub FlowLayoutPanel1_MouseHover(sender As Object, e As EventArgs) Handles FlowLayoutPanel1.MouseHover
        FlowLayoutPanel1.Focus()
    End Sub

    Private Sub FlowLayoutPanel1_MouseClick(sender As Object, e As MouseEventArgs) Handles FlowLayoutPanel1.MouseClick
        FlowLayoutPanel1.Focus()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        ' ToggleConfigEncryption(Application.ExecutablePath())
        Dim r As Integer = -1
        Dim cmd4 As String = "update tbl_packeruserdata Set connected=0 where id=" + activeuserid.ToString
        Using comm4 As New SqlCommand(cmd4, updconn)
            updconn.Open()
            r = comm4.ExecuteNonQuery()
            updconn.Close()
        End Using
        If r > 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(activeuserid, 15)
            End Using
        End If
        System.Windows.Forms.Application.Exit()

    End Sub


    Private Sub Button5_Click_1(sender As Object, e As EventArgs)
        Try
            palletsforlabels = "0"
            For Each pi As Control In FlowLayoutPanel1.Controls
                Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                Dim small As smallpallet = TryCast(pi, smallpallet)
                If normal IsNot Nothing Then
                    If normal.Visible Then
                        palletsforlabels = palletsforlabels + ", " + normal.palletid
                    End If
                ElseIf small IsNot Nothing Then
                    If small.Visible Then
                        palletsforlabels = palletsforlabels + ", " + small.palletid
                    End If
                End If
            Next
            rtime = 0
            Using f As New PrintPalletReport
                f.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)
        Using UserSettings
            UserSettings.ShowDialog()
        End Using
    End Sub

    Dim ordtime As Integer
    Dim oldorder As String
    Dim oldorddate As Date


    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim message, title As String
        Dim defaultValue As String
        Dim myValue As String = ""
        Dim myCusid As Integer = 0
        Dim myIsDraft As Boolean = False
        Using frm As New PackingListCreate(Cursor.Position.X, Cursor.Position.Y)
            Dim d As DialogResult
            d = frm.ShowDialog()
            If d = DialogResult.OK Then
                myValue = frm.plcode
                myCusid = frm.cusid
                myIsDraft = frm.IsDraft
            Else
                Return
            End If
        End Using
        Try
            Dim newpalid As Integer = -1
            'ΔΗΜΙΟΥΡΓΙΑ PACKING LIST. ΠΡΟΣΟΧΗ ΣΤΗ ΣΕΙΡΑ  ΤΩΝ ΕΝΤΟΛΩΝ ΠΑΙΖΕΙ ΡΟΛΟ ΓΙΑ ΝΑ ΠΑΡΕΙ ΤΟ ΣΩΣΤΟ PL ID Η ΕΝΤΟΛΗ plistcodecomm
            Dim cmd2 As String = "insert into tbl_packinglists (code, OPENDATE, CREATEUSER, STATUS, PLCUSID) values (@code,@opendate,@activeuserid,@status,@plcusid);Select @@IDENTITY from tbl_packinglists"
            Using sqlcmd2 As New SqlCommand(cmd2, updconn)
                sqlcmd2.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = myValue
                sqlcmd2.Parameters.Add("@opendate", SqlDbType.DateTime).Value = DateTime.Now
                sqlcmd2.Parameters.Add("@activeuserid", SqlDbType.Int).Value = activeuserid
                sqlcmd2.Parameters.Add("@plcusid", SqlDbType.Int).Value = myCusid
                If myIsDraft Then
                    sqlcmd2.Parameters.Add("@status", SqlDbType.Int).Value = -1
                Else
                    sqlcmd2.Parameters.Add("@status", SqlDbType.Int).Value = 0
                End If
                updconn.Open()
                newpalid = sqlcmd2.ExecuteScalar()
                updconn.Close()
                If newpalid > 0 Then
                    Using ut As New PackerUserTransaction
                        ut.WriteEntry(activeuserid, 17)
                    End Using
                End If
            End Using
            datagridview2_refresh()
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

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        erclose = True
        Try
            Dim r As Integer = -1
            Dim cmd4 As String = "update tbl_packeruserdata Set connected=0 where id=" + activeuserid.ToString
            updconn.Open()
            Using comm4 As New SqlCommand(cmd4, updconn)
                r = comm4.ExecuteNonQuery()
                updconn.Close()
            End Using
            If r > 0 Then
                Using ut As New PackerUserTransaction
                    ut.WriteEntry(activeuserid, 15)
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

    Public erclose As Boolean = False
    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try

            Using frm = New ItemDetails(Me.DataGridView1.Rows(Me.DataGridView1.CurrentRow.Index).Cells("iteid").Value.ToString)
                frm.ShowDialog()
            End Using

        Catch ex As Exception

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try



    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ' ToggleConfigEncryption(Application.ExecutablePath())
        System.Windows.Forms.Application.Exit()
    End Sub

    Private Sub DataGridView1_MouseDown(sender As Object, e As MouseEventArgs) Handles DataGridView1.MouseDown
        Try
            MouseDownPos = e.Location
            info = Me.DataGridView1.HitTest(e.X, e.Y)
            If info.RowIndex > -1 And info.ColumnIndex > 0 Then
                Dim Row As DataGridViewRow = Me.DataGridView1.Rows(info.RowIndex).Clone
                Row = Me.DataGridView1.Rows(info.RowIndex)
                rowtodistribute = Row
                Row.Dispose()
            End If
            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = DataGridView1.Rows(info.RowIndex).Cells(0)
            DataGridView1.Rows(info.RowIndex).Selected = True
            mouseX = e.X
            mouseY = e.Y
            If e.Button = MouseButtons.Right AndAlso info.RowIndex > -1 And Not nodistribute Then

                ContextMenuStrip2.Items(0).Text = "Κατανομή μέρους ποσότητας στη πρώτη διαθέσιμη παλέτα του τμήματος " + activeuserdpt
                ContextMenuStrip2.Items(1).Text = "Κατανομή μέρους της ποσότητας σε όλες παλέτες του τμήματος " + activeuserdpt
                ContextMenuStrip2.Items(2).Text = "Κατανομή μέρους της ποσότητας σε συγκεκριμένες παλέτες του τμήματος " + activeuserdpt
                ContextMenuStrip2.Items(3).Text = "Δημιουργία παλετών και κατανομή ποσότητας"
                ContextMenuStrip2.Items(4).Text = "Επισκόπηση σύνθεσης & θέσεων αποθήκευσης"
                ContextMenuStrip2.Items(5).Text = "Αλλαγή αποδέκτη είδους"
                ContextMenuStrip2.Show(CType(sender, Control), e.Location)
            End If
        Catch
        End Try

    End Sub

    Private Sub DataGridView2_CellDoubleClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick
        If e.RowIndex >= 0 Then

            Using frm As New PackingList(DataGridView2.Rows(e.RowIndex).Cells("id").Value)
                frm.ShowDialog()
            End Using

        End If
    End Sub

    Public Sub build_indexes()
        pindex.Clear()
        For i As Integer = 0 To phdt.Rows.Count - 1
            pindex.Add(phdt.Rows(i).Item("ID"), i.ToString + ",-1,-1")

        Next

        Dim start_ As Integer = 0
        Dim end_ As Integer = 0
        If pldt.Rows.Count = 1 Then
            Dim value_ As String = pindex(pldt.Rows(0).Item("palletid"))
            value_ = Replace(value_, ",-1,-1", "") + "," + start_.ToString + "," + end_.ToString
            pindex(pldt.Rows(0).Item("palletid")) = value_
            Return
        End If
        For i As Integer = 1 To pldt.Rows.Count - 1

            If pldt.Rows(i).Item("palletid") = pldt.Rows(i - 1).Item("palletid") Then
                end_ = i
            Else
                Dim value_ As String = pindex(pldt.Rows(i - 1).Item("palletid"))
                value_ = Replace(value_, ",-1,-1", "") + "," + start_.ToString + "," + end_.ToString
                pindex(pldt.Rows(i - 1).Item("palletid")) = value_ 'INDEX παλετών KEY: γραμμή σε phdt,πρωτη γραμμή σε pldt,τελευταία γραμμή σε pldt
                start_ = i
                end_ = i
            End If
            If i = pldt.Rows.Count - 1 Then
                Dim value_ As String = pindex(pldt.Rows(i).Item("palletid"))
                value_ = Replace(value_, ",-1,-1", "") + "," + start_.ToString + "," + i.ToString
                pindex(pldt.Rows(i).Item("palletid")) = value_
            End If
        Next
    End Sub
    Private Sub clearpallets()
        For i As Integer = FlowLayoutPanel1.Controls.Count - 1 To 0 Step -1
            FlowLayoutPanel1.Controls(i).Dispose()

        Next
        palletdeforder.Clear()
        FlowLayoutPanel1.Controls.Clear()
    End Sub

    Public Sub populate_pallettypes()
        Try
            Using com As New SqlCommand("Select id, description+ '('+cast(width as varchar(4))+'x'+cast(length as varchar(4))+'x'+cast(height as varchar(4))+')' as name,width,length,height FROM pkrtbl_pallettypes order by width asc", conn)
                conn.Open()
                pallettypes.Load(com.ExecuteReader)
                conn.Close()
            End Using
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Dim cmd As String = ""
    Dim cmd2 As String = ""
    Public Sub populate_pallets(ByVal type As String, Optional ByVal c As Control = Nothing)
        '"SELECT ph.id,ph.[CODE],ph.[OPENDATE],ph.[CLOSEDATE],ph.[REMARKS],ph.[CREATEUSER],ph.[LUPDATEUSER],ph.[WEIGHT],ph.[NETWEIGHT],ph.[LOCKEDBYID],ph.plid,ph.[CLOSEDBYID],pl.code as pcode,ph.[dptcode],pud.[department],ph.[length],ph.[width],ph.[height],isnull(locid,0) as 'locid',isnull(loccode,0) as 'loccode',PH.orders
        '                FROM [TBL_PALLETHEADERS] ph left join tbl_packeruserdata pud on pud.id=ph.createuser left join tbl_packinglists pl on ph.plid=pl.id where ph.id in (select distinct palletid from tbl_palletlines where ftrid in (" + var + ")) 
        '                OR (SUBSTRING(PH.CODE,1,PATINDEX('%[0-9]%',ph.code)-1) IN (SELECT REPLACE(FATHERNAME,'.','') FROM CUSTOMER C INNER JOIN FINTRADE F ON F.CUSID=C.ID WHERE F.ID IN (" + var + ")  )
        '                AND PH.ID NOT IN (SELECT DISTINCT PALLETID FROM TBL_PALLETLINES)) ORDER BY ph.CODE"

        Dim var As String = relftrids
        Try
            If type = "plid" Or type = "ftrid" Then
                nextpinnedlocation = 0
                Application.DoEvents()
                clearpallets()
                Dim s As String =
                        "Or (SUBSTRING(PH.CODE, 1, PATINDEX('%[0-9]%',ph.code)-1) IN (SELECT REPLACE(FATHERNAME,'.','') FROM CUSTOMER C INNER JOIN FINTRADE F ON F.CUSID=C.ID WHERE F.ID IN (" + var + ")  )
                        And PH.ID In (Select DISTINCT PALLETID FROM TBL_PALLETLINES WHERE FTRID In (Select ID FROM FINTRADE WHERE TRADECODE Like '" + TRADECODEMASK + "')) And PH.ORDERS Is Not NULL)"
                If type = "plid" Then
                    cmd = "SELECT ph.id,ph.[CODE],ph.[OPENDATE],ph.[CLOSEDATE],ph.[REMARKS],ph.[CREATEUSER],ph.createdptid,ph.[LUPDATEUSER],ph.[WEIGHT],ph.[NETWEIGHT],ph.[LOCKEDBYID],ph.plid,ph.[CLOSEDBYID],pl.code as pcode,ph.[dptcode],pud.[department],ph.PALLETTYPEID,C.FATHERNAME  FROM [TBL_PALLETHEADERS] ph left join tbl_packeruserdata pud on pud.id=ph.createuser left join tbl_packinglists pl on ph.plid=pl.id  LEFT JOIN CUSTOMER C ON C.ID=PH.CUSID where PLID=" + var + " ORDER BY ph.CODE"
                    cmd2 = "SELECT [PALLETID]      ,[pallet code]      ,[ITEID]      ,[CODE]      ,[DESCRIPTION]      ,[QUANTITY]      ,[STLID]      ,[tradecode]      ,[CUSCODE]      ,[FATHERNAME],[ftrid],isnull([dailyplanid],0) dailyplanid From [Z_PACKER_FULLPALLETLINES] WHERE PLID=" + var
                ElseIf type = "ftrid" Then
                    If Not String.IsNullOrWhiteSpace(TextBox1.Text) Then
                        cmd = "SELECT ph.id,ph.status,ph.[CODE],ph.[OPENDATE],ph.[CLOSEDATE],ph.[REMARKS],ph.[CREATEUSER],ph.createdptid,ph.[LUPDATEUSER],ph.[WEIGHT],ph.[NETWEIGHT],ph.[LOCKEDBYID],ph.plid,ph.[CLOSEDBYID],pl.code as pcode,isnull(pl.printuserid,0) printuserid,ph.[dptcode],pud.[department],ph.PALLETTYPEID ,isnull(locid,0) as 'locid',isnull(loccode,0) as 'loccode',PH.orders,isnull(ph.atlantissalesmanid,0) atlantissalesmanid,cusid, ROW_NUMBER() over(ORDER BY ph.CODE ) as rown,C.FATHERNAME
                        FROM [TBL_PALLETHEADERS] ph left join tbl_packeruserdata pud on pud.id=ph.createuser left join tbl_packinglists pl on ph.plid=pl.id   LEFT JOIN CUSTOMER C ON C.ID=PH.CUSID
                        where isnull(ph.status,0)>=-3 and ph.id in (select distinct palletid from tbl_palletlines where ftrid in (" + var + ")) 
                        OR (SUBSTRING(PH.CODE,1,PATINDEX('%[0-9]%',ph.code)-1) IN (SELECT REPLACE(FATHERNAME,'.','') FROM CUSTOMER C INNER JOIN FINTRADE F ON F.CUSID=C.ID WHERE F.ID IN (" + var + ")  )
                        AND PH.ID NOT IN (SELECT DISTINCT PALLETID FROM TBL_PALLETLINES) AND PH.ORDERS IS not NULL)  OR (SUBSTRING(PH.CODE,1,PATINDEX('%[0-9]%',ph.code)-1) IN (SELECT REPLACE(FATHERNAME,'.','') FROM CUSTOMER C INNER JOIN FINTRADE F ON F.CUSID=C.ID WHERE F.ID IN (" + var + ")  )
                        AND  PH.ORDERS IS NULL) " + s + " ORDER BY ph.CODE"
                    Else
                        cmd = "SELECT ph.id,ph.status,ph.[CODE],ph.[OPENDATE],ph.[CLOSEDATE],ph.[REMARKS],ph.[CREATEUSER],ph.createdptid,ph.[LUPDATEUSER],ph.[WEIGHT],ph.[NETWEIGHT],ph.[LOCKEDBYID],ph.plid,ph.[CLOSEDBYID],pl.code as pcode,isnull(pl.printuserid,0) printuserid,ph.[dptcode],pud.[department],ph.PALLETTYPEID ,isnull(locid,0) as 'locid',isnull(loccode,0) as 'loccode',PH.orders,isnull(ph.atlantissalesmanid,0) atlantissalesmanid,cusid, ROW_NUMBER() over(ORDER BY ph.CODE ) as rown,C.FATHERNAME
                        FROM [TBL_PALLETHEADERS] ph left join tbl_packeruserdata pud on pud.id=ph.createuser left join tbl_packinglists pl on ph.plid=pl.id LEFT JOIN CUSTOMER C ON C.ID=PH.CUSID
                        where isnull(ph.status,0)>=-3 and  ph.id in (select distinct palletid from tbl_palletlines where ftrid in (" + var + ")) 
                        OR (SUBSTRING(PH.CODE,1,PATINDEX('%[0-9]%',ph.code)-1) IN (SELECT REPLACE(FATHERNAME,'.','') FROM CUSTOMER C INNER JOIN FINTRADE F ON F.CUSID=C.ID WHERE F.ID IN (" + var + ")  )
                        AND PH.ID NOT IN (SELECT DISTINCT PALLETID FROM TBL_PALLETLINES) AND PH.ORDERS IS not NULL) OR (SUBSTRING(PH.CODE,1,PATINDEX('%[0-9]%',ph.code)-1) IN (SELECT REPLACE(FATHERNAME,'.','') FROM CUSTOMER C INNER JOIN FINTRADE F ON F.CUSID=C.ID WHERE F.ID IN (" + var + ")  )
                        AND  PH.ORDERS IS NULL)  ORDER BY ph.CODE"
                    End If
                    cmd2 = "SELECT [PALLETID]      ,[pallet code]      ,[ITEID]      ,[CODE]      ,[DESCRIPTION]      ,[QUANTITY]      ,[STLID]      ,[tradecode]      ,[CUSCODE]      ,[FATHERNAME],[ftrid],isnull(frommantis,0) 'frommantis',isnull([dailyplanid],0) dailyplanid From [Z_PACKER_FULLPALLETLINES] WHERE palletid in (select distinct palletid from tbl_palletlines where isnull(status,0)>=-3 and  ftrid in (" + var + ")) order by palletid,quantity desc"

                End If
                phdt.Clear()
                pldt.Clear()
                palletdeforder.Clear()
                palletpinorder.Clear()
                pindex.Clear()
                palletw.RunWorkerAsync()

            Else
                'FlowLayoutPanel1.Visible = False
                FlowLayoutPanel1.SuspendLayout()
                Dim cmd As String = ""
                Dim cmd2 As String = ""
                Dim tempphdt As New DataTable, temppldt As New DataTable
                Try
                    Dim list As New List(Of String)(type.Split(","))

                    Dim wastype As String = ""
                    Dim wasindex As Integer
                    'Dim pinned As Integer = -1

                    Dim normal As pallettemplate = TryCast(c, pallettemplate)
                    Dim small As smallpallet = TryCast(c, smallpallet)
                    If normal IsNot Nothing Then
                        If list.Contains(normal.palletid.ToString) Then

                            wastype = "normal"
                            wasindex = FlowLayoutPanel1.Controls.IndexOf(c)
                            'pinned = normal.pinned
                        End If
                    ElseIf small IsNot Nothing Then
                        If list.Contains(small.palletid.ToString) Then
                            wastype = "small"
                            wasindex = FlowLayoutPanel1.Controls.IndexOf(c)
                        End If

                    End If



                    cmd = "SELECT ph.id,ph.status,ph.[CODE],ph.[OPENDATE],ph.[CLOSEDATE],ph.[REMARKS],ph.[CREATEUSER],ph.createdptid,ph.[LUPDATEUSER],ph.[WEIGHT],ph.[NETWEIGHT],ph.[LOCKEDBYID],ph.plid,ph.[CLOSEDBYID],pl.code as pcode,isnull(pl.printuserid,0) printuserid,ph.[dptcode],pud.[department],ph.PALLETTYPEID ,isnull(locid,0) as 'locid',isnull(loccode,0) as 'loccode',PH.orders,isnull(ph.atlantissalesmanid,0) atlantissalesmanid,cusid, ROW_NUMBER() over(ORDER BY ph.CODE ) as rown,C.FATHERNAME
                        FROM [TBL_PALLETHEADERS] ph left join tbl_packeruserdata pud on pud.id=ph.createuser left join tbl_packinglists pl on ph.plid=pl.id LEFT JOIN CUSTOMER C ON C.ID=PH.CUSID
                        where isnull(ph.status,0)>=-3 and  ph.id in (" + type + ") ORDER BY ph.CODE"

                    cmd2 = "SELECT [PALLETID]      ,[pallet code]      ,[ITEID]      ,[CODE]      ,[DESCRIPTION]      ,[QUANTITY]      ,[STLID]      ,[tradecode]      ,[CUSCODE]      ,[FATHERNAME],[ftrid],isnull(frommantis,0) 'frommantis',isnull([dailyplanid],0) dailyplanid From [Z_PACKER_FULLPALLETLINES] WHERE palletid in (select id from tbl_palletheaders where isnull(status,0)>=-3 and id=" + type + ") order by palletid,quantity desc"



                    Using cmdcomm As New SqlCommand(cmd, conn)
                        Using cmd2comm As New SqlCommand(cmd2, conn)

                            conn.Open()
                            Using plreader As SqlDataReader = cmd2comm.ExecuteReader()
                                temppldt.Load(plreader)

                                Using phreader As SqlDataReader = cmdcomm.ExecuteReader()
                                    tempphdt.Load(phreader)
                                    conn.Close()

                                    For Each i As String In list
                                        Dim info As String = pindex(CInt(i))
                                        Dim strs As String() = info.Split(",") 'strings(1) phdt index, strings(2) pldt start, strings(3) pldt end
                                        Dim phi As Integer = CInt(strs(0))
                                        Dim pls As Integer = CInt(strs(1))
                                        Dim ple As Integer = CInt(strs(2))
                                        If ple >= 0 Then

                                            For k As Integer = ple To pls Step -1
                                                Try
                                                    pldt.Rows.RemoveAt(k)
                                                Catch
                                                    Continue For
                                                End Try
                                            Next

                                        End If
                                        For j As Integer = 1 To phdt.Columns.Count - 1
                                            phdt.Columns(j).ReadOnly = False
                                            phdt.Rows(phi).Item(j) = tempphdt.Rows(0).Item(j)
                                            phdt.Columns(j).ReadOnly = True

                                        Next
                                    Next
                                    For i As Integer = 0 To temppldt.Rows.Count - 1

                                        pldt.ImportRow(temppldt.Rows(i))

                                    Next
                                    build_indexes()

                                    For Each i As String In list
                                        Dim info As String = pindex(CInt(i))
                                        Dim strs As String() = info.Split(",") 'strings(1) phdt index, strings(2) pldt start, strings(3) pldt end
                                        Dim phi As Integer = CInt(strs(0))
                                        Dim pls As Integer = CInt(strs(1))
                                        Dim ple As Integer = CInt(strs(2))
                                        If wastype = "normal" Then
                                            If normal.pinned <> -1 Then
                                                pallet_morph(CInt(i), 1, wasindex, pinned:=True, c:=c)
                                            Else
                                                pallet_morph(CInt(i), 1, wasindex, c:=c)
                                            End If
                                        ElseIf wastype = "small" Then
                                            pallet_morph(CInt(i), 2, wasindex, c:=c)
                                        End If
                                    Next

                                End Using
                            End Using
                        End Using
                    End Using

                Finally
                    tempphdt.Dispose()
                    temppldt.Dispose()
                    'FlowLayoutPanel1.Visible = True
                    FlowLayoutPanel1.ResumeLayout()
                End Try
            End If

            'change_frommantis()

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

    Private Sub palletw_DoWork(sender As Object, e As DoWorkEventArgs) Handles palletw.DoWork
        Dim palletwconn = New SqlConnection(connString)
        Try
            Using cmdcomm As New SqlCommand(cmd, palletwconn)
                Using cmd2comm As New SqlCommand(cmd2, palletwconn)

                    palletwconn.Open()
                    Console.WriteLine("palletw HELLO")
                    Using phreader As SqlDataReader = cmdcomm.ExecuteReader()
                        phdt.Load(phreader)
                        palletwconn.Close()
                        palletwconn.Open()
                        Using plreader As SqlDataReader = cmd2comm.ExecuteReader()
                            pldt.Load(plreader)
                            palletwconn.Close()
                            GC.Collect()



                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception

        Finally
            palletwconn.Dispose()
        End Try
    End Sub

    Private Sub palletw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles palletw.RunWorkerCompleted
        Try

            build_indexes()
            filter_pallets()
            For Each co As Control In TableLayoutPanel4.Controls
                If Not IsNothing(TryCast(co, PictureBox)) Then
                    co.Dispose()
                End If
            Next
            FlowLayoutPanel1.Visible = True
        Catch EX As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally

            If Not ibw.IsBusy And Not plbw.IsBusy And Not obw.IsBusy Then
                Button1.Enabled = True
                My.Settings.downloading = False
            End If
        End Try

    End Sub


    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Using frm As New PackingList(DataGridView2.SelectedRows(0).Cells("ID").Value)
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub DataGridView2_Sorted(sender As Object, e As EventArgs) Handles DataGridView2.Sorted
        Dim GridSortOrder As System.Windows.Forms.SortOrder
        GridSortOrder = DataGridView2.SortOrder

        If GridSortOrder = System.Windows.Forms.SortOrder.Ascending Then
            SetSortOrderdgv2 = ListSortDirection.Ascending
        ElseIf GridSortOrder = System.Windows.Forms.SortOrder.Descending Then
            SetSortOrderdgv2 = ListSortDirection.Descending
        ElseIf GridSortOrder = System.Windows.Forms.SortOrder.None Then
            SetSortOrderdgv2 = ListSortDirection.Ascending

        End If
        For i As Integer = 0 To DataGridView2.RowCount - 1
            If DataGridView2.Rows(i).Cells("ST").Value = 0 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.notcompleted
            ElseIf DataGridView2.Rows(i).Cells("ST").Value = 2 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.saved
            ElseIf DataGridView2.Rows(i).Cells("ST").Value = 3 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.issues
            Else
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.completed
            End If
        Next


        DataGridView2.Refresh()
    End Sub

    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        ORDERTEMPCODE = False


        If DataGridView1.Rows.Count = 0 Then
            Button13.Enabled = False
            Button17.Enabled = False
            'button18.enabled = false
            Button3.Enabled = False
            Panel4.Enabled = False
            Panel3.Enabled = False
        Else
            Button13.Enabled = True
            Button17.Enabled = True
            'button18.enabled = true
            Button3.Enabled = True
            Panel4.Enabled = True
            Panel3.Enabled = True
        End If
        Dim comclmn As New DataGridViewImageColumn
        comclmn.DefaultCellStyle.NullValue = Nothing
        comclmn.Name = "Σ"
        If Not DataGridView1.Columns.Contains("Σ") Then
            DataGridView1.Columns.Insert(0, comclmn)
            DataGridView1.Columns(0).Name = "Σ"
            DataGridView1.Columns(0).Width = 20
        End If
        Dim stc As New DatagridviewStackedProgressColumn
        stc.DefaultCellStyle.NullValue = Nothing
        stc.Name = "Status"
        stc.HeaderText = "Κατάσταση"
        If Not DataGridView1.Columns.Contains("Status") Then
            DataGridView1.Columns.Insert(0, stc)
            DataGridView1.Columns(0).Name = "Status"
            'DataGridView1.Columns(0).Width = 20
        End If

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("ΠΟΣ").Value = DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value Then
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.White
            ElseIf DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value = 0 Then
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
            Else
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.Khaki
            End If
            With DataGridView1
                .Rows(i).Cells("Status").Value = .Rows(i).Cells("BACKORDER").Value.ToString + "/" + .Rows(i).Cells("black").Value.ToString + "/" + .Rows(i).Cells("blue").Value.ToString + "/" + .Rows(i).Cells("lightgreen").Value.ToString + "/" + .Rows(i).Cells("green").Value.ToString + "/" + .Rows(i).Cells("gold").Value.ToString
            End With
            If DataGridView1.Rows(i).Cells("iteid").Value = 65947 Or DataGridView1.Rows(i).Cells("iteid").Value = 65946 Or DataGridView1.Rows(i).Cells("iteid").Value = 65948 Then
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.Red
                ORDERTEMPCODE = True
                For Each cell As DataGridViewCell In DataGridView1.Rows(i).Cells
                    cell.ToolTipText = "Προσωρινός κωδικός"

                Next

            End If
            If DataGridView1.Rows(i).Cells("BACKORDER").Value <> 0 Then
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.DarkOrange
                'Dim formToolTip = New System.Windows.Forms.ToolTip
                For Each cell As DataGridViewCell In DataGridView1.Rows(i).Cells
                    cell.ToolTipText = "Backordered ποσότητα: " + DataGridView1.Rows(i).Cells("BACKORDER").Value.ToString
                Next
                DataGridView1.Rows(i).Cells("Σ").Value = My.Resources._6BTgqqGi8
            End If

            If DataGridView1.Rows(i).Cells("BACKORDER").Value <> 0 Then
                DataGridView1.Rows(i).Cells("Σ").Value = My.Resources._6BTgqqGi8
            ElseIf Not (IsDBNull(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΑΠΟΘ").Value) OrElse String.IsNullOrWhiteSpace(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΑΠΟΘ").Value)) _
                                            Or Not (IsDBNull(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΣΥΣΚ").Value) OrElse String.IsNullOrWhiteSpace(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΣΥΣΚ").Value)) _
                                            Or Not (IsDBNull(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΕΞΑΓ").Value) OrElse String.IsNullOrWhiteSpace(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΕΞΑΓ").Value)) Or Not (IsDBNull(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΠΑΡΑΓ").Value) OrElse String.IsNullOrWhiteSpace(DataGridView1.Rows(i).Cells("ΣΧΟΛΙΑ ΠΑΡΑΓ").Value)) Then
                DataGridView1.Rows(i).Cells("Σ").Value = My.Resources.comment_png_22707

            End If

        Next
        With DataGridView1
            For Each c As DataGridViewColumn In DataGridView1.Columns
                Dim f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "1dgv|" + c.Name)
                If f IsNot Nothing Then
                    c.Width = CInt(f.value)
                End If
            Next
        End With
        'orderdgv_DataBindingComplete(Me, Nothing)
        If ORDERTEMPCODE = True Then
            PictureBox6.Visible = True
        Else
            PictureBox6.Visible = False
        End If
        order_columns(2)
        selection_indicators()
        DataGridView1.Sort(DataGridView1.Columns("ΚΩΔΙΚΟΣ"), ListSortDirection.Ascending)
    End Sub

    'Private Sub DataGridView2_DragEnter(sender As Object, e As DragEventArgs) Handles DataGridView2.DragEnter
    '    e.Effect = DragDropEffects.All
    '    oldcursor = Cursor
    'End Sub

    'Private Sub DataGridView2_DragLeave(sender As Object, e As EventArgs) Handles DataGridView2.DragLeave


    'End Sub

    'Private Sub DataGridView2_DragDrop(sender As Object, e As DragEventArgs) Handles DataGridView2.DragDrop
    '    Dim palletid As String = e.Data.GetData(GetType(String))
    '    Dim p As Point = DataGridView2.PointToClient(New Point(e.X, e.Y))
    '    Dim hit As DataGridView.HitTestInfo = DataGridView2.HitTest(p.X, p.Y)
    '    If hit.RowIndex >= 0 Then
    '        Dim plid As Integer = DataGridView2.Rows(hit.RowIndex).Cells("id").Value
    '        Dim plname As String = DataGridView2.Rows(hit.RowIndex).Cells("ΚΩΔΙΚΟΣ").Value

    '        Dim cmd As String = "update tbl_palletheaders set plid=" + plid.ToString + ", plorder=(select count(id) from tbl_palletheaders where plid=" + plid.ToString + ")+1 where id=" + palletid.ToString
    '        Dim cmd2 As String = "update tbl_packinglists set status=2 where id=" + plid.ToString
    '        Using comm As New SqlCommand(cmd, updconn)
    '            Using comm2 As New SqlCommand(cmd2, updconn)
    '                Try
    '                    Dim r1 As Integer = -1
    '                    Dim r2 As Integer = -1
    '                    If DataGridView2.Rows(hit.RowIndex).Cells("ST").Value <> 1 Then
    '                        updconn.Open()
    '                        r1 = comm.ExecuteNonQuery()
    '                        r2 = comm2.ExecuteNonQuery()
    '                        updconn.Close()
    '                        If r1 > 0 And r2 > 0 Then
    '                            Using ut As New PackerUserTransaction
    '                                ut.WriteEntry(activeuserid, 16, plid, palletid)
    '                            End Using
    '                        End If
    '                        datagridview2_refresh()
    '                        For Each pi As Control In FlowLayoutPanel1.Controls
    '                            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
    '                            Dim small As smallpallet = TryCast(pi, smallpallet)
    '                            If normal IsNot Nothing Then
    '                                If normal.palletid = palletid Then
    '                                    populate_pallets(palletid.ToString, c:=normal)
    '                                End If
    '                            ElseIf small IsNot Nothing Then
    '                                If small.palletid = palletid Then
    '                                    populate_pallets(palletid.ToString, c:=small)
    '                                End If
    '                            End If
    '                        Next
    '                    End If
    '                Catch ex As Exception
    '                    If updconn.State = ConnectionState.Open Then
    '                        updconn.Close()
    '                    End If
    '                    Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '                End Try
    '            End Using
    '        End Using

    '    End If

    'End Sub

    Private Sub DataGridView2_MouseLeave(sender As Object, e As EventArgs) Handles DataGridView2.MouseLeave


    End Sub

    Private Sub DataGridView1_MouseMove(sender As Object, e As MouseEventArgs) Handles DataGridView1.MouseMove
        If e.Button And MouseButtons.Left = MouseButtons.Left Then
            Dim dx = e.X - MouseDownPos.X
            Dim dy = e.Y - MouseDownPos.Y
            If (Math.Abs(dx) >= SystemInformation.DoubleClickSize.Width OrElse
               Math.Abs(dy) >= SystemInformation.DoubleClickSize.Height) And Not nodistribute Then
                Dim cerr As Boolean = True
                Try
                    dgv1rowindex = info.RowIndex

                    Dim Row As DataGridViewRow = Me.DataGridView1.Rows(info.RowIndex).Clone
                    Row = Me.DataGridView1.Rows(info.RowIndex)
                    If Row.Cells("iteid").Value = 65946 Or Row.Cells("iteid").Value = 65947 Or Row.Cells("iteid").Value = 65948 Then
                        cerr = False

                    End If

                    Dim txt As String = DataGridView1.Rows(dgv1rowindex).Cells("ΚΩΔΙΚΟΣ").Value + " " + DataGridView1.Rows(dgv1rowindex).Cells("ΠΕΡΙΓΡΑΦΗ").Value + " " + DataGridView1.Rows(dgv1rowindex).Cells("ΠΑΡ").Value

                    Dim gr As Graphics = Me.CreateGraphics()


                    Dim sz As SizeF = gr.MeasureString(txt, boldfont)
                    Dim bmp As New Bitmap(CInt(sz.Width + 60) * 2, CInt(sz.Height + 60))
                    Try
                        gr = Graphics.FromImage(bmp)
                        gr.Clear(Color.White)
                        gr.DrawIcon(My.Resources.grantexbox1, CInt(sz.Width + 50), 25)
                        gr.DrawString(txt, boldfont, Brushes.Black, CInt(sz.Width + 60) + 40, 45)

                        gr.Dispose()
                        bmp.MakeTransparent(Color.White)
                        cur = New Cursor(bmp.GetHicon())

                        Me.DataGridView1.DoDragDrop(Row, DragDropEffects.Copy)

                    Finally
                        gr.Dispose()

                        bmp.Dispose()
                        Row.Dispose()
                    End Try


                Catch ex As Exception
                    If cerr = False Then
                        If updconn.State = ConnectionState.Open Then
                            updconn.Close()
                        End If
                        If conn.State = ConnectionState.Open Then
                            conn.Close()
                        End If
                        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
                    End If
                End Try
            Else



            End If
        End If
    End Sub

    Private Sub CheckBox13_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox13.CheckedChanged
        filter_pallets()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint


        Panel1.BorderStyle = BorderStyle.None
        e.Graphics.DrawRectangle(New Pen(Color.Maroon, 2), Panel1.ClientRectangle)


    End Sub

    Private Sub FlowLayoutPanel3_Paint(sender As Object, e As PaintEventArgs) Handles FlowLayoutPanel3.Paint


        FlowLayoutPanel3.BorderStyle = BorderStyle.None
        e.Graphics.DrawRectangle(New Pen(Color.Maroon, 2), FlowLayoutPanel3.ClientRectangle)


    End Sub

    Private Sub Panel7_Paint(sender As Object, e As PaintEventArgs) Handles Panel7.Paint
        Panel7.BorderStyle = BorderStyle.None
        e.Graphics.DrawRectangle(New Pen(Color.Maroon, 2), Panel7.ClientRectangle)

    End Sub



    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles Panel4.Paint
        Panel4.BorderStyle = BorderStyle.None
        e.Graphics.DrawRectangle(New Pen(Color.Maroon, 2), Panel4.ClientRectangle)
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub cusnamemaskbox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cusnamemaskbox.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub callnamescbox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles callnamescbox.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub orderdgv_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles orderdgv.ColumnWidthChanged
        If Not My.Settings.downloading Then
            Dim name As String = "Odgv|" + e.Column.Name
            Using f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = name)
                If f Is Nothing Then
                    settings.Add(New settingsitem(name, e.Column.Width))
                Else
                    settings.Remove(f)
                    f.Dispose()
                    settings.Add(New settingsitem(name, e.Column.Width))
                End If

            End Using
        End If
    End Sub

    Private Sub DataGridView1_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridView1.ColumnWidthChanged
        If Not My.Settings.downloading Then
            Dim name As String = "1dgv|" + e.Column.Name
            Using f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = name)
                If f Is Nothing Then
                    settings.Add(New settingsitem(name, e.Column.Width))
                Else
                    settings.Remove(f)
                    f.Dispose()
                    settings.Add(New settingsitem(name, e.Column.Width))
                End If
            End Using
        End If
    End Sub

    Private Sub DataGridView2_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridView2.ColumnWidthChanged
        If Not My.Settings.downloading Then
            Dim name As String = "2dgv|" + e.Column.Name
            Using f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = name)
                If f Is Nothing Then
                    settings.Add(New settingsitem(name, e.Column.Width))
                Else
                    settings.Remove(f)
                    f.Dispose()
                    settings.Add(New settingsitem(name, e.Column.Width))
                End If
            End Using
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        filtermanager()

    End Sub

    Private Sub cuscodemaskbox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cuscodemaskbox.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub Button2_EnabledChanged(sender As Object, e As EventArgs) Handles Button2.EnabledChanged
        If (activeuserdpt <> "SA" And activeuserdpt <> "SP" And activeuserdpt <> "BL" And activeuserdpt <> "BP") Or nodistribute Then
            Button2.Enabled = False
        End If
    End Sub

    'Private Sub Button23_Click_1(sender As Object, e As EventArgs)
    '    Try
    '        If nodistribute Then
    '            Throw New System.Exception("Δεν επιτρέπεται παρά μόνο εάν έχετε μία παραγγελία φορτωμένη!")
    '        End If
    '        'Dim l As New List(Of String)

    '        pallets_for_mantis()
    '        If palletids.Count = 0 Then
    '            Throw New System.Exception("Καμία διαθέσιμη παλέτα!")
    '        End If

    '        Dim cmd As String = "delete from tbl_palletlines where frommantis=1 and palletid in (" + String.Join(",", palletids.ToArray()) + ") "
    '        Using com As New SqlCommand(cmd, updconn)
    '            updconn.Open()
    '            Dim success = com.ExecuteNonQuery
    '            updconn.Close()

    '            If success <= 0 Then
    '                Throw New System.Exception("Απέτυχε.")
    '            End If
    '            populate_pallets("ftrid")
    '            datagridview1_refresh()
    '            orderdgv_refresh()
    '        End Using
    '    Catch ex As Exception
    '        If updconn.State = ConnectionState.Open Then
    '            updconn.Close()
    '        End If
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try
    'End Sub


    Private Sub FlowLayoutPanel1_ControlRemoved(sender As Object, e As ControlEventArgs) Handles FlowLayoutPanel1.ControlRemoved
        Dim cnt As Integer = 0
        If FlowLayoutPanel1.Visible Then
            If FlowLayoutPanel1.Controls.Count > 0 Then
                For Each c As Control In FlowLayoutPanel1.Controls
                    If c.Visible Then
                        cnt += 1
                    End If
                Next
                Label24.Text = cnt.ToString + " παλέτες"
                Label24.Visible = True
            Else
                Label24.Visible = False
            End If
        End If

    End Sub

    Private Sub CheckBox15_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox15.CheckedChanged
        If CheckBox15.Checked Then
            CheckBox14.Checked = False

        End If
        If CheckBox15.Checked = True Then
            DateTimePicker1.Enabled = False
            DateTimePicker2.Enabled = False
            TextBox1.Enabled = False

            cusnamemaskbox.Enabled = False

            cuscodemaskbox.Enabled = False
            'ComboBox4.Enabled = False
            ' Button25.Enabled = False
        Else
            DateTimePicker1.Enabled = True
            DateTimePicker2.Enabled = True
            TextBox1.Enabled = True

            cusnamemaskbox.Enabled = True

            cuscodemaskbox.Enabled = True
            'ComboBox4.Enabled = True
            'Button25.Enabled = True
        End If
    End Sub

    Private Sub NotifyIcon1_Click(sender As Object, e As EventArgs) Handles NotifyIcon1.Click

    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        NotifyIcon1.ShowBalloonTip(3000)
    End Sub

    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button14_Click_2(sender As Object, e As EventArgs)


    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub ΠαραγωγήToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΠαραγωγήToolStripMenuItem1.Click
        Dim f As New production
        f.Show()
    End Sub

    Private Sub ΠροσωπικέςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΠροσωπικέςToolStripMenuItem1.Click
        Using frm As New UserSettings
            frm.ShowDialog()
        End Using
    End Sub


    Private Sub ΠαλετώνToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΠαλετώνToolStripMenuItem1.Click

        Using frm As New PalletTypes
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub ΑρχικήToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑρχικήToolStripMenuItem1.Click
        TabControl3.SelectedIndex = 1
    End Sub

    Private Sub ΠαραγγελίεςΣυσκευασίαToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΠαραγγελίεςσυσκευασίαToolStripMenuItem1.Click
        TabControl3.SelectedIndex = 0
    End Sub

    Private Sub ΈξοδοςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΈξοδοςToolStripMenuItem1.Click
        ' ToggleConfigEncryption(Application.ExecutablePath())
        Dim r As Integer = -1
        Dim cmd4 As String = "update tbl_packeruserdata Set connected=0 where id=" + activeuserid.ToString
        Using comm4 As New SqlCommand(cmd4, updconn)
            updconn.Open()
            r = comm4.ExecuteNonQuery()
            updconn.Close()
            If r > 0 Then
                Using ut As New PackerUserTransaction
                    ut.WriteEntry(activeuserid, 15)
                End Using
            End If
        End Using
        System.Windows.Forms.Application.Exit()
    End Sub

    Private Sub CheckBox18_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox18.CheckedChanged
        If CheckBox18.Checked Then
            CheckBox18.Image = My.Resources.icons8_expand_arrow_16
            TableLayoutPanel1.RowStyles.Item(0).Height = 0
            PictureBox14.Visible = True
        Else
            CheckBox18.Image = My.Resources.icons8_collapse_arrow_16
            TableLayoutPanel1.RowStyles.Item(0).Height = 130
            PictureBox14.Visible = False
        End If
    End Sub

    Private Sub ΠελατώνToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ΠελατώνToolStripMenuItem1.Click
        Using f As New Customer
            f.ShowDialog()
        End Using
    End Sub

    Private Sub CheckBox19_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox19.CheckedChanged
        If CheckBox19.Checked Then
            CheckBox19.Text = "Οριζόντια"
            SplitContainer1.Orientation = Orientation.Horizontal
        Else
            CheckBox19.Text = "Κάθετη"
            SplitContainer1.Orientation = Orientation.Vertical
        End If
    End Sub

    Dim reportLevel As Integer = 0
    Private Sub ReportViewer1_Drillthrough(sender As Object, e As DrillthroughEventArgs) Handles ReportViewer1.Drillthrough
        reportLevel += 1
        'Dim MONDAY As String = ""
        'Dim SUNDAY As String = ""
        'Dim ItemTypes As String = ""
        'Dim SelectedItem As String = ""
        Dim report As LocalReport = e.Report
        Dim lst As IList(Of ReportParameter) = report.OriginalParametersToDrillthrough
        'For Each param As ReportParameter In lst
        '    If param.Name = "MONDAY" Then
        '        MONDAY = param.Values(0)
        '    End If
        '    If param.Name = "SUNDAY" Then
        '        SUNDAY = param.Values(0)
        '    End If
        '    If param.Name = "ItemTypes" Then
        '        ItemTypes = param.Values(0)
        '    End If
        '    If param.Name = "SelectedItem" Then
        '        SelectedItem = param.Values(0)
        '    End If
        'Next
        report.DataSources.Add(New ReportDataSource("DataSet1", MainA0BindingSource))
    End Sub

    Private Sub ReportViewer1_Back(sender As Object, e As BackEventArgs) Handles ReportViewer1.Back
        reportLevel -= 1
        'Dim report As LocalReport = e.ParentReport
        'report.DataSources.Add(New ReportDataSource("DataSet1", MainA0BindingSource))
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        If My.Settings.LoadingComplete Then
            Refresh_report()
        End If
    End Sub

    Dim reportLevel2 As Integer = 0
    Private Sub ReportViewer2_Drillthrough(sender As Object, e As DrillthroughEventArgs) Handles ReportViewer2.Drillthrough
        reportLevel2 += 1
        'Dim MONDAY As String = ""
        'Dim SUNDAY As String = ""
        'Dim ItemTypes As String = ""
        'Dim SelectedItem As String = ""
        Dim report As LocalReport = e.Report
        Dim lst As IList(Of ReportParameter) = report.OriginalParametersToDrillthrough
        'For Each param As ReportParameter In lst
        '    If param.Name = "MONDAY" Then
        '        MONDAY = param.Values(0)
        '    End If
        '    If param.Name = "SUNDAY" Then
        '        SUNDAY = param.Values(0)
        '    End If
        '    If param.Name = "ItemTypes" Then
        '        ItemTypes = param.Values(0)
        '    End If
        '    If param.Name = "SelectedItem" Then
        '        SelectedItem = param.Values(0)
        '    End If
        'Next
        report.DataSources.Add(New ReportDataSource("DataSet1", MainA0BindingSource))
    End Sub

    Private Sub ReportViewer2_Back(sender As Object, e As BackEventArgs) Handles ReportViewer2.Back
        reportLevel2 -= 1
        'Dim report As LocalReport = e.ParentReport
        'report.DataSources.Add(New ReportDataSource("DataSet1", MainA0BindingSource))
    End Sub
    Dim reportLevel5 As Integer = 0
    Private Sub ReportViewer5_Drillthrough(sender As Object, e As DrillthroughEventArgs) Handles ReportViewer5.Drillthrough
        reportLevel5 += 1
        Dim report As LocalReport = e.Report
        Dim lst As IList(Of ReportParameter) = report.OriginalParametersToDrillthrough
        report.DataSources.Add(New ReportDataSource("DataSet1", MainE0BindingSource))
    End Sub

    Private Sub Button22_MouseClick(sender As Object, e As MouseEventArgs) Handles Button22.MouseClick
        ContextMenuStrip5.Show(CType(sender, Control), e.Location)
    End Sub

    Private Sub ΑνοίγματοςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑνοίγματοςToolStripMenuItem.Click
        Try
            'DataGridView1_CellContentDoubleClick(sender, clickedorder, True)
            palletsforlabels = "0"
            For Each pi As Control In FlowLayoutPanel1.Controls
                Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                Dim small As smallpallet = TryCast(pi, smallpallet)
                If normal IsNot Nothing Then
                    If normal.Visible Then
                        palletsforlabels = palletsforlabels + "," + normal.palletid
                    End If
                ElseIf small IsNot Nothing Then
                    If small.Visible Then
                        palletsforlabels = palletsforlabels + "," + small.palletid
                    End If
                End If
            Next
            Using frm As New PrintOpenLabels
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub ΚλεισίματοςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΚλεισίματοςToolStripMenuItem.Click

        Try
            palletsforlabels = "0"
            For Each pi As Control In FlowLayoutPanel1.Controls
                Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                Dim small As smallpallet = TryCast(pi, smallpallet)
                If normal IsNot Nothing Then
                    If normal.Visible Then
                        palletsforlabels = palletsforlabels + "," + normal.palletid
                    End If
                ElseIf small IsNot Nothing Then
                    If small.Visible Then
                        palletsforlabels = palletsforlabels + "," + small.palletid
                    End If
                End If
            Next
            Using frm As New PrintCloseLabels
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        Try
            If activeuser = "SUPERVISOR" Or Me.Text.Contains("Debug") Then
                Dim usr As String
                usr = InputBox("Δώσε username", "Δώσε username", " ")
                Using s As New SqlCommand("select pud.id dptid,* from tbl_packeruserdata pu inner join pkrtbl_userdepartments pud on pud.code=pu.department where username=@USR", conn)
                    conn.Open()
                    s.Parameters.Add("@USR", sqlDbType:=SqlDbType.VarChar).Value = usr
                    Using dt As New DataTable()
                        Using reader As SqlDataReader = s.ExecuteReader
                            dt.Load(reader)
                        End Using
                        conn.Close()
                        If dt.Rows.Count = 0 Then
                            Throw New Exception
                        End If
                        activeuserid = dt.Rows(0).Item("id")
                        If IsDBNull(dt.Rows(0).Item("atlantisid")) Then activeuseraid = Nothing Else activeuseraid = dt.Rows(0).Item("atlantisid")
                        activeuser = dt.Rows(0).Item("username")
                        activeuserdpt = dt.Rows(0).Item("department")
                        activeuserdptid = dt.Rows(0).Item("dptid")
                        activeuserocu = dt.Rows(0).Item("ORDCUSER")
                        ToolStripMenuItem5.Text = activeuser
                        load_UI_rights()
                    End Using

                End Using
            Else
                Using frm As New UserSettings
                    frm.ShowDialog()
                End Using
            End If
        Catch
        End Try
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs, Optional test As Integer = 1) Handles ToolStripMenuItem6.Click
        Dim link As Uri = New Uri(ContextMenuStrip6.Text)
        If link.Authority = "someaction" Then
            Dim sep As Char() = New Char() {"="c}
            Dim param = link.Query.Split(sep)
            Dim rowId As String = param(1)
            Using f As New ItemDetails(rowId)
                f.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem7.Click
        Dim link As Uri = New Uri(ContextMenuStrip6.Text)
        If link.Authority = "someaction" Then
            Button8.PerformClick()
            Dim sep As Char() = New Char() {"="c}
            Dim param = link.Query.Split(sep)
            Dim rowId As String = param(1)
            Dim trdcd As String = param(2)
            Dim itecd As String = param(3)
            Me.TextBox1.Text = trdcd
            DateTimePicker1.Value = checkdate.AddMonths(-12)
            TabControl3.SelectedIndex = 0
            CheckBox15.Checked = False
            Me.Button1.PerformClick()
        End If
    End Sub


    Private Sub ReportViewer5_Back(sender As Object, e As BackEventArgs) Handles ReportViewer5.Back
        reportLevel5 -= 1
    End Sub

    Private Sub Button5_Click_2(sender As Object, e As EventArgs) Handles Button5.Click
        If activeDate > monDate Then
            activeDate = activeDate.AddDays(-7)
            Refresh_report()
        End If
        If activeDate = monDate Then
            Button5.Enabled = False
            Button10.Enabled = False
        End If
    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        If activeDate < monDate.AddMonths(1) And activeDate >= monDate Then
            activeDate = activeDate.AddDays(7)
            If activeDate >= monDate.AddMonths(1) Then
                Button6.Enabled = False
            End If
            Refresh_report()
            Button5.Enabled = True
            Button10.Enabled = True
        End If
    End Sub

    Private Sub Button10_Click_1(sender As Object, e As EventArgs) Handles Button10.Click
        activeDate = monDate
        Button5.Enabled = False
        Button10.Enabled = False
        Button6.Enabled = True
        Refresh_report()
    End Sub

    Private Sub ReportViewer3_Hyperlink(sender As Object, e As HyperlinkEventArgs) Handles ReportViewer3.Hyperlink, ReportViewer4.Hyperlink, ReportViewer2.Hyperlink, ReportViewer1.Hyperlink, ReportViewer5.Hyperlink
        e.Cancel = True
        ContextMenuStrip6.Text = e.Hyperlink
        ContextMenuStrip6.Show(Cursor.Position)

    End Sub


    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        DateTimePicker2.Value = checkdate
        TextBox1.Text = ""
        TextBox2.Text = ""
        cusnamemaskbox.Text = ""
        callnamescbox.SelectedIndex = 0
        callnamescbox.Text = ""
        cuscodemaskbox.Text = ""
        ComboBox4.SelectedIndex = 0
        ComboBox4.Text = ""
        CheckBox14.Checked = False
        CheckBox15.Checked = False
        CheckBox16.Checked = False
        CheckBox17.Checked = False
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

    End Sub

    Private Sub reportworker_DoWork(sender As Object, e As DoWorkEventArgs) Handles reportworker.DoWork
        Try
            'TODO: This line of code loads data into the 'TESTFINALDataSet.DataTable2' table. You can move, or remove it, as needed.
            'Me.DataTable2TableAdapter.Fill(Me.TESTFINALDataSet.DataTable2)
            'TODO: This line of code loads data into the 'TESTFINALDataSet.Z_PACKER_FULLREPORT' table. You can move, or remove it, as needed.
            'Me.Z_PACKER_FULLREPORTTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_FULLREPORT)

            'Dim MONDAY As New ReportParameter("MONDAY", monDate.ToString("yyyy/MM/dd"))
            'Dim SUNDAY As New ReportParameter("SUNDAY", monDate.AddDays(6).ToString("yyyy/MM/dd"))
            'Dim ItemTypes As New ReportParameter("ItemTypes", it)

            ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\MainA0.rdlc"
            ReportViewer4.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\MainD0.rdlc"
            ReportViewer3.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\MainB0.rdlc"
            ReportViewer2.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\MainC0.rdlc"
            ReportViewer5.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\MainE0.rdlc"
            'ElseIf reportLevel = 1 Then
            '    ReportViewer1.PerformBack()
            '    'ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\MainA1.rdlc"
            'Dim SelectedCategory As New ReportParameter("SelectedCategory", "")
            'Dim SelectedQuality As New ReportParameter("SelectedQuality", "")
            'ReportViewer1.LocalReport.SetParameters(SelectedCategory)
            'ReportViewer1.LocalReport.SetParameters(SelectedQuality)

            'ReportViewer1.LocalReport.SetParameters(MONDAY)
            'ReportViewer1.LocalReport.SetParameters(SUNDAY)
            'ReportViewer1.LocalReport.SetParameters(ItemTypes)
            ' monDate.ToString("yyyy/MM/dd"), monDate.AddDays(6).ToString("yyyy/MM/dd")Dim salesmanid As String = ""
            Dim salesmanid As String = ""
            If IsNothing(activeuseraid) OrElse activeuseraid = 0 Then
                salesmanid = String.Join(",", allsalesmans.ToArray)
            Else
                salesmanid = activeuseraid.ToString
            End If
            MainA0TableAdapter.Fill(Me.MainReportDataSource.MainA0, activeDate.ToString("yyyy/MM/dd"), activeDate.AddDays(6).ToString("yyyy/MM/dd"), recipientsgroup, itemtypesgroup, salesmanid)
            MainB0TableAdapter.Fill(Me.MainReportDataSource.MainB0, recipientsgroup, itemtypesgroup, salesmanid)
            MainE0TableAdapter.Fill(Me.MainReportDataSource.MainE0, activeDate.ToString("yyyy/MM/dd"), activeDate.AddDays(6).ToString("yyyy/MM/dd"), itemtypesgroup, salesmanid)
            'MainD0TableAdapter.Fill(Me.MainReportDataSource.MainD0, "2018/01/01", "2019/12/31", recipientsgroup, itemtypesgroup)
            Dim res As New List(Of ReportDataSource)
            Dim rds As ReportDataSource = New ReportDataSource("DataSet1", MainA0BindingSource)
            Dim rds2 As ReportDataSource = New ReportDataSource("DataSet1", MainA0BindingSource)
            Dim rds3 As ReportDataSource = New ReportDataSource("DataSet1", MainB0BindingSource)
            Dim rds4 As ReportDataSource = New ReportDataSource("DataSet1", MainA0BindingSource)
            Dim rds5 As ReportDataSource = New ReportDataSource("DataSet1", MainE0BindingSource)
            res.Add(rds)
            res.Add(rds2)
            res.Add(rds3)
            res.Add(rds4)
            res.Add(rds5)
            e.Result = res
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.Message.Contains("An error occurred during local report processing.") Then
                msg = "Κάποιο πρόβλημα προέκυψε κατά τη πρόσβαση στο template. Ελέγξτε ότι έχετε πρόσβαση και δεν έχει διαγραφεί κάποιο αρχείο στον κοινόχρηστο φάκελο."
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, msg, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub




    Private Sub reportworker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles reportworker.RunWorkerCompleted
        Dim res As List(Of ReportDataSource) = e.Result
        ReportViewer1.LocalReport.DataSources.Add(res(0))
        ReportViewer4.LocalReport.DataSources.Add(res(1))
        ReportViewer3.LocalReport.DataSources.Add(res(2))
        ReportViewer2.LocalReport.DataSources.Add(res(3))
        ReportViewer5.LocalReport.DataSources.Add(res(4))
        For i As Integer = DoubleBufferedTableLayoutPanel3.Controls.Count - 1 To 0 Step -1
            If Not IsNothing(TryCast(DoubleBufferedTableLayoutPanel3.Controls(i), PictureBox)) Then
                DoubleBufferedTableLayoutPanel3.Controls(i).Dispose()
            End If
        Next
        For Each rv As ReportViewer In l
            rv.Visible = True
            rv.RefreshReport()
        Next
        If Not My.Settings.LoadingComplete Then
            splashscreen.Dispose()
        End If
        reportworker_complete2()
    End Sub

    Private Sub reportworker_complete2()
        If Not My.Settings.LoadingComplete Then
            My.Settings.LoadingComplete = True
            My.Settings.Save()
            Me.Opacity = 100
        End If
    End Sub

    'Private Sub Button14_Click_1(sender As Object, e As EventArgs)
    '    Using frm As New PackingListEstimationManagement
    '        frm.ShowDialog()
    '    End Using
    'End Sub
    'Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
    '    Try
    '        If Not nodistribute And FlowLayoutPanel1.Controls.Count > 0 Then
    '            Dim cs As String() = CUSTOMER.Split(" ")
    '            Dim cmd As String = "select pl.FTRID,s.a as 'ΠΑΡ',pl.iteid,pl.stlid,z.diff as 'ΠΟΣ',m.CODE 'ΚΩΔΙΚΟΣ',ph.locid,m.DESCRIPTION 'ΠΕΡΙΓΡΑΦΗ' from tbl_palletlines pl left join
    '                        tbl_palletheaders ph on ph.id=pl.palletid left join
    '                        [dbo].[Z_PACKER_PENDING_ITEMS_PER_ORDER] z on pl.stlid=z.stlid
    '                        inner join fintrade f on f.id=pl.FTRID INNER JOIN CUSTOMER C ON C.ID=F.CUSID

    '                        left join material m on m.id=pl.iteid
    '                        cross apply (select   dbo.get_tradecode(f.id) a) s
    '                        where f.DSRID in (9000,9008) 
    '                        And C.CODE='" + cs(0) + "'"
    '            Using comm As New SqlCommand(cmd, updconn)
    '                Using dt = New DataTable()
    '                    updconn.Open()
    '                    Using reader As SqlDataReader = comm.ExecuteReader()
    '                        dt.Load(reader)
    '                        updconn.Close()
    '                        If dt.Rows.Count = 0 Then
    '                            Throw New System.Exception("Έχουν κατανεμηθεί όλα τα είδη της παραγγελίας.")
    '                        End If
    '                        dt.Columns.Add("newQuant", Type.GetType("System.Double"))

    '                    End Using

    '                    pallets_for_mantis()

    '                    If locids.Count = 0 Then
    '                        Throw New System.Exception("Δεν υπάρχουν διαθέσιμες παλέτες/θέσεις αποθήκευσης που είναι κλειδωμένες από εσάς.")
    '                    End If
    '                    Dim cmd1 As String = "select iteid,lsumqty,locationID from SC_QTY_MANTISAX_RETURNS where locationID in (" + String.Join(",", locids.ToArray()) + ")"
    '                    Using comm1 As New SqlCommand(cmd1, conn)
    '                        Using dt1 = New DataTable()
    '                            conn.Open()
    '                            Using reader1 As SqlDataReader = comm1.ExecuteReader()
    '                                dt1.Load(reader1)
    '                                conn.Close()
    '                                If dt1.Rows.Count = 0 Then
    '                                    Throw New System.Exception("Δεν έχουν καταχωρηθεί είδη στις διαθέσιμες θέσεις αποθήκευσης/παλέτες.")
    '                                End If
    '                                Using dt3 = New DataTable()
    '                                    dt3.Columns.Add("ΚΩΔΙΚΟΣ", Type.GetType("System.String"))
    '                                    dt3.Columns.Add("ΠΕΡΙΓΡΑΦΗ", Type.GetType("System.String"))
    '                                    dt3.Columns.Add("ΠΑΡ", Type.GetType("System.String"))
    '                                    dt3.Columns.Add("Παλέτα", Type.GetType("System.String"))
    '                                    dt3.Columns.Add("Θέση αποθήκευσης", Type.GetType("System.String"))
    '                                    dt3.Columns.Add("Ποσότητα", Type.GetType("System.Double"))
    '                                    dt3.Columns.Add("stlid", Type.GetType("System.Int32"))
    '                                    dt3.Columns.Add("ftrid", Type.GetType("System.Int32"))
    '                                    dt3.Columns.Add("palletid", Type.GetType("System.Int32"))
    '                                    dt3.Columns.Add("locationid", Type.GetType("System.Int32"))
    '                                    dt3.Columns.Add("lsumqty", Type.GetType("System.Double"))
    '                                    dt3.Columns.Add("ΚΩΔΠΕΛ", Type.GetType("System.String"))
    '                                    dt3.Columns.Add("ΠΕΛ", Type.GetType("System.String"))
    '                                    dt3.Columns.Add("iteid", Type.GetType("System.Int32"))
    '                                    For i As Integer = 0 To dt.Rows.Count - 1
    '                                        dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("ΠΟΣ")

    '                                        For j As Integer = 0 To dt1.Rows.Count - 1
    '                                            If dt1.Rows(j).Item("iteid") = dt.Rows(i).Item("iteid") Then
    '                                                Dim IND As Integer = locids.IndexOf(dt1.Rows(j).Item("locationID").ToString)
    '                                                Dim quantity As Double = 0
    '                                                If dt.Rows(i).Item("newQuant") <= 0 Then
    '                                                    Continue For
    '                                                ElseIf dt1.Rows(j).Item("lsumqty") <= dt.Rows(i).Item("newQuant") Then
    '                                                    quantity = dt1.Rows(j).Item("lsumqty")
    '                                                Else
    '                                                    quantity = dt.Rows(i).Item("newQuant")
    '                                                End If
    '                                                dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("newQuant") - quantity
    '                                                Dim dr As DataRow = dt3.NewRow
    '                                                dr("ΚΩΔΙΚΟΣ") = dt.Rows(i).Item("ΚΩΔΙΚΟΣ")
    '                                                dr("ΠΕΡΙΓΡΑΦΗ") = dt.Rows(i).Item("ΠΕΡΙΓΡΑΦΗ")
    '                                                dr("ΠΑΡ") = dt.Rows(i).Item("ΠΑΡ")
    '                                                dr("Παλέτα") = palletcodes(IND)
    '                                                dr("Θέση αποθήκευσης") = loccodes(IND)
    '                                                dr("Ποσότητα") = quantity
    '                                                dr("stlid") = dt.Rows(i).Item("stlid")
    '                                                dr("ftrid") = dt.Rows(i).Item("ftrid")
    '                                                dr("iteid") = dt.Rows(i).Item("iteid")
    '                                                dr("palletid") = palletids(IND)
    '                                                dr("locationid") = locids(IND)
    '                                                dr("lsumqty") = dt1.Rows(j).Item("lsumqty")
    '                                                dr("ΠΕΛ") = cs(1)
    '                                                dr("ΚΩΔΠΕΛ") = cs(0)
    '                                                dt3.Rows.Add(dr)
    '                                            End If
    '                                        Next
    '                                    Next
    '                                    For i As Integer = 0 To dt.Rows.Count - 1
    '                                        If dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("ΠΟΣ") Then
    '                                            dt.Rows(i).Delete()
    '                                        End If
    '                                    Next

    '                                    Using frm As New datafrommantisfrm(dt, dt3, True) '*************ΠΡΟΣΟΧΗ ΕΔΩ Η ΔΙΑΦΟΡΑ ΜΕ BUTTON14*********
    '                                        frm.ShowDialog()
    '                                    End Using
    '                                End Using
    '                            End Using
    '                        End Using
    '                    End Using
    '                End Using
    '            End Using

    '        End If
    '    Catch ex As Exception
    '        If updconn.State = ConnectionState.Open Then
    '            updconn.Close()
    '        End If
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try

    'End Sub

    Private Sub FlowLayoutPanel1_ControlAdded(sender As Object, e As ControlEventArgs) Handles FlowLayoutPanel1.ControlAdded
        Dim cnt As Integer = 0
        If FlowLayoutPanel1.Visible Then
            If FlowLayoutPanel1.Controls.Count > 0 Then
                For Each c As Control In FlowLayoutPanel1.Controls
                    If c.Visible Then
                        cnt += 1
                    End If
                Next
                Label24.Text = cnt.ToString + " παλέτες"
                Label24.Visible = True
            Else
                Label24.Visible = False
            End If
        End If


    End Sub


    Private Sub PictureBox1_SizeChanged(sender As Object, e As EventArgs) Handles PictureBox1.SizeChanged
        If PictureBox1.Width < 130 Or PictureBox1.Height < 59 Then
            PictureBox1.Image = My.Resources.G_red
        Else
            PictureBox1.Image = My.Resources.packer2_logo
        End If
    End Sub

    Private Sub DataGridView2_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView2.DataBindingComplete
        If DataGridView2.Rows.Count > 0 Then
            Button15.Enabled = True
        Else
            Button15.Enabled = False
        End If
        If DataGridView2.Rows.Count <> 0 Then
            DataGridView2.Columns("id").Visible = False
            Me.DataGridView2.CurrentCell = Me.DataGridView2.Item(2, 0)
            DataGridView2.Columns("ST").Visible = False
            DataGridView2.Columns("ΣΧΕΤΙΚΕΣ ΠΑΡ").Width = 300
        End If
        If Not DataGridView2.Columns.Contains("STATUS") Then
            Dim plsclmn As New DataGridViewImageColumn
            DataGridView2.Columns.Insert(0, plsclmn)
            DataGridView2.Columns(0).Name = "STATUS"
            DataGridView2.Columns("STATUS").Width = 50
        End If
        For i As Integer = 0 To DataGridView2.RowCount - 1
            If DataGridView2.Rows(i).Cells("ST").Value = 0 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.notcompleted
            ElseIf DataGridView2.Rows(i).Cells("ST").Value = 2 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.saved
            ElseIf DataGridView2.Rows(i).Cells("ST").Value = 3 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.issues
            ElseIf DataGridView2.Rows(i).Cells("ST").Value = -1 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.orangeclock
                DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            ElseIf DataGridView2.Rows(i).Cells("ST").Value = 1 Then
                DataGridView2.Rows(i).Cells("STATUS").Value = My.Resources.completed
            End If
        Next
        With DataGridView2
            For i As Integer = 0 To .Columns.Count - 1
                Dim f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "2dgv|" + .Columns(i).Name)
                If f IsNot Nothing Then
                    .Columns(i).Width = CInt(f.value)
                End If
            Next
        End With
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Dim selrow As Integer = DataGridView2.CurrentRow.Index
            If selrow >= 0 Then

                Dim palletnum As Integer = 0
                Using cmd As New SqlCommand("select count(id) from tbl_palletheaders where plid=" + DataGridView2.Rows(selrow).Cells("id").Value.ToString, conn)
                    conn.Open()
                    palletnum = cmd.ExecuteScalar
                    conn.Close()
                End Using
                If palletnum = 0 Then
                    Using cmd As New SqlCommand("delete from tbl_packinglists where id=" + DataGridView2.Rows(selrow).Cells("id").Value.ToString, updconn)
                        updconn.Open()
                        cmd.ExecuteNonQuery()
                        updconn.Close()
                    End Using
                    datagridview2_refresh()
                Else
                    Throw New System.Exception("Το Packing list προς διαγραφή πρέπει να είναι άδειο.")
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
        End Try

    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Selected Then
                DataGridView1.Rows(i).Selected = False
            End If
        Next

    End Sub

    Private Sub Panel1_DragDrop(sender As Object, e As DragEventArgs) Handles Panel1.DragDrop
        Dim row As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
        If row IsNot Nothing Then
            Try
                Using row
                    TextBox1.Text = row.Cells("ΠΑΡ").Value
                    DateTimePicker1.Value = row.Cells("ΗΜΕΡΟΜΗΝΙΑ ΚΑΤΑΧΩΡΙΣΗΣ").Value
                    CheckBox14.Checked = False
                    CheckBox15.Checked = False
                    CheckBox16.Checked = False
                    Button1.PerformClick()
                End Using
            Catch

            End Try
        End If
    End Sub

    Private Sub ΑλλαγήΑποδέκτηΕίδουςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑλλαγήΑποδέκτηΕίδουςToolStripMenuItem.Click
        Dim frm As New recipientchange(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("iteid").Value.ToString, DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("ftrid").Value.ToString, DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("stlid").Value.ToString, Cursor.Position)
        Using frm
            frm.ShowDialog()
        End Using
    End Sub
    Dim b27click As Integer = 0

    Private Sub ΔιαγραφήστήληςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΔιαγραφήστήληςToolStripMenuItem.Click
        If dgvtype = 1 Then
            orderdgv.Columns(clickedheader).Visible = False
        Else
            DataGridView1.Columns(clickedheader).Visible = False
        End If


    End Sub
    Dim dgvtype As Integer
    Dim clickedheader As Integer
    Private Sub orderdgv_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles orderdgv.ColumnHeaderMouseClick
        If e.Button = MouseButtons.Right Then
            clickedheader = e.ColumnIndex
            dgvtype = 1
            ContextMenuStrip3.Show(MousePosition.X, MousePosition.Y)

        End If
    End Sub
    Private Sub DataGridView1_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.ColumnHeaderMouseClick
        If e.Button = MouseButtons.Right Then
            clickedheader = e.ColumnIndex
            dgvtype = 2
            ContextMenuStrip3.Show(MousePosition.X, MousePosition.Y)

        End If
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        b27click += 1
        If b27click = 1 Then
            Button27.Text = "Κωδικό παλέτας"
            Button27.BackColor = Color.Gainsboro
            order_pallets(1)
        ElseIf b27click = 2 Then
            Button27.Text = "Σχετικά ΠΑΡ"
            Button27.BackColor = Color.LightGray
            order_pallets(2)
        ElseIf b27click = 3 Then
            Button27.Text = "Θέση αποθήκευσης"
            Button27.BackColor = Color.Silver
            order_pallets(3)
        ElseIf b27click = 4 Then
            Button27.Text = "Σειρά ανοίγματος"
            Button27.BackColor = Color.White
            order_pallets(4)
            b27click = 0

        End If
    End Sub

    Dim clicker2 As Integer
    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        clicker2 = clicker2 + 1
        filter_pallets()
    End Sub

    Private Sub order_pallets(ByVal type As Integer)
        FlowLayoutPanel1.SuspendLayout()

        If type = 1 Then
            Dim list As New List(Of String)
            For Each pi As Control In FlowLayoutPanel1.Controls
                Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                Dim small As smallpallet = TryCast(pi, smallpallet)
                If normal IsNot Nothing Then
                    list.Add(normal.pallettemplatelabel.Text)
                ElseIf small IsNot Nothing Then
                    list.Add(small.Label1.Text)
                End If



            Next
            list.Sort()
            list.Reverse()
            For i As Integer = 0 To list.Count - 1
                For Each pi As Control In FlowLayoutPanel1.Controls
                    Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                    Dim small As smallpallet = TryCast(pi, smallpallet)
                    If normal IsNot Nothing Then
                        If normal.pallettemplatelabel.Text = list(i) Then
                            FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)

                        End If
                    ElseIf small IsNot Nothing Then
                        If small.Label1.Text = list(i) Then
                            FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)

                        End If
                    End If


                Next
            Next
        ElseIf type = 2 Then
            Dim list As New List(Of String)
            For i As Integer = 0 To phdt.Rows.Count - 1
                list.Add(phdt.Rows(i).Item("orders"))

            Next
            list.Sort()
            list.Reverse()
            For i As Integer = 0 To list.Count - 1
                For j As Integer = 0 To phdt.Rows.Count - 1
                    If phdt.Rows(j).Item("orders") = list(i) Then
                        For Each pi As Control In FlowLayoutPanel1.Controls
                            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                            Dim small As smallpallet = TryCast(pi, smallpallet)
                            If normal IsNot Nothing Then
                                If normal.palletid = phdt.Rows(j).Item("id") Then
                                    FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)
                                End If

                            ElseIf small IsNot Nothing Then
                                If small.palletid = phdt.Rows(j).Item("id") Then
                                    FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)
                                End If
                            End If


                        Next
                    End If
                Next

            Next
        ElseIf type = 3 Then
            Dim list As New List(Of String)
            For i As Integer = 0 To phdt.Rows.Count - 1
                list.Add(phdt.Rows(i).Item("loccode"))

            Next
            list.Sort()
            list.Reverse()
            For i As Integer = 0 To list.Count - 1
                For j As Integer = 0 To phdt.Rows.Count - 1
                    If phdt.Rows(j).Item("loccode") = list(i) Then
                        For Each pi As Control In FlowLayoutPanel1.Controls
                            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                            Dim small As smallpallet = TryCast(pi, smallpallet)
                            If normal IsNot Nothing Then
                                If normal.palletid = phdt.Rows(j).Item("id") Then
                                    FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)
                                    Exit For
                                End If

                            ElseIf small IsNot Nothing Then
                                If small.palletid = phdt.Rows(j).Item("id") Then
                                    FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)
                                    Exit For
                                End If
                            End If


                        Next
                    End If
                Next

            Next
        ElseIf type = 4 Then
            Dim list As New List(Of String)
            For i As Integer = 0 To phdt.Rows.Count - 1
                list.Add(phdt.Rows(i).Item("id"))

            Next
            list.Sort()
            list.Reverse()
            For i As Integer = 0 To list.Count - 1
                For j As Integer = 0 To phdt.Rows.Count - 1
                    If phdt.Rows(j).Item("id") = list(i) Then
                        For Each pi As Control In FlowLayoutPanel1.Controls
                            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                            Dim small As smallpallet = TryCast(pi, smallpallet)
                            If normal IsNot Nothing Then
                                If normal.palletid = phdt.Rows(j).Item("id") Then
                                    FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)
                                    Exit For
                                End If

                            ElseIf small IsNot Nothing Then
                                If small.palletid = phdt.Rows(j).Item("id") Then
                                    FlowLayoutPanel1.Controls.SetChildIndex(pi, 0)
                                    Exit For
                                End If
                            End If


                        Next
                    End If
                Next

            Next
        End If
        FlowLayoutPanel1.ResumeLayout()

    End Sub

    'Private Sub ΔημιουργίαΠαλετώνΚαιΚατανομήΠοσότηταςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΔημιουργίαΠαλετώνΚαιΚατανομήΠοσότηταςToolStripMenuItem.Click
    '    Try
    '        Dim message, title As String
    '        Dim defaultValue As String
    '        Dim myValue As Object
    '        defaultValue = ""
    '        ' Set prompt.
    '        message = "Εισάγετε ποσότητα που θέλετε να εισαχθεί σε κάθε παλέτα:"
    '        ' Set title.
    '        title = "Ποσότητα είδους ανά παλέτα"
    '        ' Display message, title, and default value.
    '        Dim Valid As Boolean
    '        While Valid = False
    '            myValue = InputBox(message, title, defaultValue, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y)
    '            Try
    '                If myValue.ToString Like "#" Or myValue.ToString Like "###" Or myValue.ToString Like "##" Then

    '                    Valid = True
    '                ElseIf myValue Is "" Or myValue = 0 Then
    '                    Valid = False
    '                Else
    '                    Valid = False
    '                End If
    '            Catch
    '                Return
    '            End Try
    '        End While
    '        Dim posothta As Integer = CInt(myValue)
    '        Dim Valid2 As Boolean
    '        Dim myValue2 As Object
    '        Dim defaultValue2 As Double = DataGridView1.Rows(info.RowIndex).Cells("ΥΠΟΛ.").Value
    '        Dim message2, title2 As String
    '        ' Set prompt.
    '        message2 = "Τι ποσότητα εκ του συνόλου θα κατανεμηθεί;"
    '        ' Set title.
    '        title2 = "Συνολική ποσότητα προς κατανομή"
    '        ' Display message, title, and default value.
    '        While Valid2 = False
    '            myValue2 = InputBox(message2, title2, defaultValue2, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y)
    '            Try
    '                If (myValue2.ToString Like "#" Or myValue2.ToString Like "###" Or myValue2.ToString Like "##") And myValue2 <= defaultValue2 And myValue2 > 0 And Convert.ToDouble(myValue2) >= Convert.ToDouble(myValue) Then

    '                    Valid2 = True
    '                ElseIf myValue2 Is "" Or myValue2 = 0 Then
    '                    Valid2 = False
    '                Else
    '                    Valid2 = False
    '                End If
    '            Catch
    '                Return
    '            End Try
    '        End While


    '        Dim limit As Double = CInt(myValue2)
    '        Dim pallets As Integer = Math.Ceiling(limit / posothta)
    '        Dim result As Integer = MessageBox.Show("Θα δημιουργηθούν " + pallets.ToString + " παλέτες, με " + posothta.ToString + " σετ\τεμάχια από το είδος " + DataGridView1.Rows(info.RowIndex).Cells("ΠΕΡΙΓΡΑΦΗ").Value.ToString + " στη κάθε μία. Συμφωνείτε;" _
    '        , "ΠΡΟΣΟΧΗ!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
    '        If result = DialogResult.Yes Then
    '            For i As Integer = 1 To pallets
    '                Dim success As Integer = 0
    '                Button2.PerformClick()

    '                If newpalletid > 0 And posothta * i <= limit Then

    '                    success = pallet_exchange(2, newpalletid, posothta.ToString, DataGridView1.Rows(info.RowIndex))
    '                ElseIf newpalletid > 0 And posothta * i > limit Then
    '                    posothta = limit Mod posothta
    '                    success = pallet_exchange(2, newpalletid, posothta.ToString, DataGridView1.Rows(info.RowIndex))
    '                Else
    '                    Throw New System.Exception("Κάτι δε πήγε καλά κατά τη δημιουργία της παλέτας " + i.ToString + "...")
    '                End If
    '                If success <= 0 Then
    '                    Throw New System.Exception("Κάτι δε πήγε καλά κατά τη δημιουργία της παλέτας " + i.ToString + "...")
    '                End If
    '            Next
    '        Else
    '            Return
    '        End If
    '        datagridview1_refresh()

    '    Catch ex As Exception
    '        If updconn.State = ConnectionState.Open Then
    '            updconn.Close()
    '        End If
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try
    'End Sub

    Private Sub Button8_Click_1(sender As Object, e As EventArgs)
        Dim frm As New report_issue
        frm.ShowDialog()
    End Sub

    Private Sub orderdgv_MouseDown(sender As Object, e As MouseEventArgs) Handles orderdgv.MouseDown
        MouseDownPos = e.Location
        info = Me.orderdgv.HitTest(e.X, e.Y)
        If info.RowIndex > -1 Then
            Me.orderdgv.ClearSelection()
            Me.orderdgv.Rows(info.RowIndex).Selected = True
            Me.orderdgv.CurrentCell = Me.orderdgv.Rows(info.RowIndex).Cells("ΠΑΡ")

            Dim Row As DataGridViewRow = Me.orderdgv.Rows(info.RowIndex)
            'If Row.Cells(0).Value = 65946 Or Row.Cells(0).Value = 65947 Or Row.Cells(0).Value = 65948 Then
            '    cerr = False

            'End If
            Using Row
                rowtodistribute = Row.Clone()
                For i As Integer = 0 To Row.Cells.Count - 1
                    rowtodistribute.Cells(i).Value = Row.Cells(i).Value

                Next
            End Using
        End If

    End Sub

    Private Sub orderdgv_MouseMove(sender As Object, e As MouseEventArgs) Handles orderdgv.MouseMove
        If e.Button And MouseButtons.Left = MouseButtons.Left Then
            Dim dx = e.X - MouseDownPos.X
            Dim dy = e.Y - MouseDownPos.Y
            If Math.Abs(dx) >= SystemInformation.DoubleClickSize.Width OrElse
               Math.Abs(dy) >= SystemInformation.DoubleClickSize.Height Then
                Dim cerr As Boolean = True
                Try
                    Using Row As DataGridViewRow = Me.orderdgv.Rows(info.RowIndex)


                        Me.orderdgv.DoDragDrop(Row, DragDropEffects.Copy)
                    End Using

                Catch ex As Exception
                    If cerr = False Then
                        If updconn.State = ConnectionState.Open Then
                            updconn.Close()
                        End If
                        If conn.State = ConnectionState.Open Then
                            conn.Close()
                        End If
                        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
                    End If
                End Try
            Else



            End If
        End If
    End Sub

    Private Sub ordflp_update()
        Using comm As New SqlCommand("select top 5 case when fathername Is null then dbo.get_tradecode(f.id) +' ???'  else dbo.get_tradecode(f.id) +' '+fathername end as name, 
CONVERT(VARCHAR(19),f.FTRTIME,20) AS FTRTIME 
from fintrade f left join customer c on c.id=f.cusid 
where f.dsrid in (9000,9008) and f.approved=1
order by f.ftrtime desc", updconn)
            Using dt = New DataTable()
                updconn.Open()
                Using reader As SqlDataReader = comm.ExecuteReader()


                    dt.Load(reader)
                    updconn.Close()
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim o As New notification
                        o.myname = dt.Rows(i).Item("name").ToString
                        o.createtime = dt.Rows(i).Item("ftrtime") 'DateTime.ParseExact(dt.Rows(i).Item("ftrtime").ToString, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)
                        'ordflp.Controls.Add(o)
                    Next
                    If oldorder <> Nothing Then
                        If oldorder <> dt.Rows(0).Item("name").ToString And oldorddate < dt.Rows(0).Item("ftrtime") Then
                            Using frm = New Form16(dt.Rows(0).Item("name").ToString, dt.Rows(0).Item("ftrtime").ToString)
                                frm.Show()
                            End Using
                        End If
                        If oldorder <> dt.Rows(0).Item("name").ToString And oldorder <> dt.Rows(1).Item("name").ToString And oldorddate < dt.Rows(1).Item("ftrtime") Then
                            Using frm = New Form16(dt.Rows(1).Item("name").ToString, dt.Rows(1).Item("ftrtime").ToString)
                                frm.Show()
                            End Using
                        End If
                        If oldorder <> dt.Rows(0).Item("name").ToString And oldorder <> dt.Rows(1).Item("name").ToString And oldorder <> dt.Rows(2).Item("name").ToString And oldorddate < dt.Rows(2).Item("ftrtime") Then
                            Using frm = New Form16(dt.Rows(2).Item("name").ToString, dt.Rows(2).Item("ftrtime").ToString)
                                frm.Show()
                            End Using
                        End If

                    End If
                    If dt.Rows.Count > 0 AndAlso IsDBNull(dt.Rows(0).Item("name")) Then
                        oldorder = dt.Rows(0).Item("name")
                        oldorddate = dt.Rows(0).Item("ftrtime")
                    End If
                End Using
            End Using
        End Using

    End Sub

    Private Sub Label13_Doubleclick(sender As Object, e As EventArgs)
        Try
            If activeuser = "SUPERVISOR" Or Me.Text.Contains("Debug") Then
                Dim usr As String
                usr = InputBox("Δώσε username", "Δώσε username", " ")
                Using s As New SqlCommand("select pud.id dptid,* from tbl_packeruserdata pu inner join pkrtbl_userdepartments pud on pud.code=pu.department where username=@USR", conn)
                    conn.Open()
                    s.Parameters.Add("@USR", sqlDbType:=SqlDbType.VarChar).Value = usr
                    Using dt As New DataTable()
                        Using reader As SqlDataReader = s.ExecuteReader
                            dt.Load(reader)
                        End Using
                        conn.Close()
                        If dt.Rows.Count = 0 Then
                            Throw New Exception
                        End If
                        activeuserid = dt.Rows(0).Item("id")
                        If IsDBNull(dt.Rows(0).Item("atlantisid")) Then activeuseraid = Nothing Else activeuseraid = dt.Rows(0).Item("atlantisid")
                        activeuser = dt.Rows(0).Item("username")
                        activeuserdpt = dt.Rows(0).Item("department")
                        activeuserdptid = dt.Rows(0).Item("dptid")
                        activeuserocu = dt.Rows(0).Item("ORDCUSER")
                        ToolStripMenuItem5.Text = activeuser.ToUpper
                        load_UI_rights()
                    End Using
                End Using
            End If
        Catch
        End Try
    End Sub

    Private Sub ΜίαςΓραμμήςToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'VIEWSTYLE = "ONELINE"
        'FlowLayoutPanel1.SuspendLayout()

        'For Each pallet As pallettemplate In FlowLayoutPanel1.Controls

        '    pallet.viewstyle = "ONELINE"
        'Next
        'FlowLayoutPanel1.ResumeLayout()
    End Sub


    Private Sub CheckBox14_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox14.CheckedChanged
        If CheckBox14.Checked Then
            CheckBox15.Checked = False

        End If
        If CheckBox14.Checked = True Then
            DateTimePicker1.Enabled = False
            DateTimePicker2.Enabled = False
            TextBox1.Enabled = False

            cusnamemaskbox.Enabled = False
            TextBox2.Enabled = False
            cuscodemaskbox.Enabled = False
            'ComboBox4.Enabled = False
            'Button25.Enabled = False
        Else
            DateTimePicker1.Enabled = True
            DateTimePicker2.Enabled = True
            TextBox1.Enabled = True
            TextBox2.Enabled = True
            cusnamemaskbox.Enabled = True

            cuscodemaskbox.Enabled = True
            ' ComboBox4.Enabled = True
            'Button25.Enabled = True
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.N AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) Then
            Using frm As New Form16("00TEST00234343", Date.Parse("2016/12/03 17:00:15"))
                frm.Show()
            End Using
        End If
        If (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.AltKeyDown AndAlso e.KeyCode = Keys.C) Then
            MsgBox(conn.ConnectionString + " " + updconn.ConnectionString)
        End If
    End Sub

    Private Sub orderdgv_Sorted(sender As Object, e As EventArgs) Handles orderdgv.Sorted


        Dim GridSortOrder As System.Windows.Forms.SortOrder
        GridSortOrder = orderdgv.SortOrder

        If GridSortOrder = System.Windows.Forms.SortOrder.Ascending Then
            SetSortOrderorderdgv = ListSortDirection.Ascending
        ElseIf GridSortOrder = System.Windows.Forms.SortOrder.Descending Then
            SetSortOrderorderdgv = ListSortDirection.Descending
        ElseIf GridSortOrder = System.Windows.Forms.SortOrder.None Then
            SetSortOrderorderdgv = ListSortDirection.Ascending

        End If

        order_columns(1)
    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint
        Panel3.BorderStyle = BorderStyle.None
        e.Graphics.DrawRectangle(New Pen(Color.Maroon, 2), Panel3.ClientRectangle)
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then

            Try
                If DataGridView1.Columns(e.ColumnIndex).Name = "Σ" Then
                    Using frm = New itemcomments(Me.DataGridView1.Rows(e.RowIndex).Cells("stlid").Value, Me.DataGridView1.Rows(e.RowIndex).Cells("ftrid").Value)
                        frm.ShowDialog()
                    End Using
                ElseIf DataGridView1.Columns(e.ColumnIndex).Name = "Status" Then
                    Dim dt As New DataTable()
                    dt.Columns.Add("stlid", GetType(Integer))
                    dt.Columns.Add("s")
                    dt.Columns.Add("t", GetType(Double))
                    Dim piecolumns As New Dictionary(Of String, String) From {{"BACKORDER", "Backorder"}, {"blue", "Σε σχεδιασμένες παλέτες"}, {"black", "Εκκρεμούν"}, {"lightgreen", "Σε παλέτα"}, {"gold", "Απεσταλμένα"}, {"green", "Σε κλειστές παλέτες"}}
                    Dim stlid = DataGridView1.Rows(e.RowIndex).Cells("stlid").Value
                    For Each c As KeyValuePair(Of String, String) In piecolumns
                        Dim dtrow As DataRow = dt.NewRow
                        dtrow("stlid") = stlid
                        dtrow("s") = c.Value
                        dtrow("t") = DataGridView1.Rows(e.RowIndex).Cells(c.Key).Value
                        dt.Rows.Add(dtrow)
                    Next
                    Dim f As New DatagridviewStackedProgressColumnReportForm(DataGridView1.Rows(e.RowIndex).Cells("stlid").Value, DataGridView1.Rows(e.RowIndex).Cells("ΕΝΑΛ ΚΩΔ").Value, DataGridView1.Rows(e.RowIndex).Cells("ΠΑΡ").Value, Cursor.Position.X, Cursor.Position.Y, dt, stl_or_ftrid_id:=DataGridView1.Rows(e.RowIndex).Cells("stlid").Value, rec_values:="1,2,3,4,5,6", type:="stlid")
                    f.Owner = Me
                    f.Show()
                Else
                    Using frm = New ItemDetails(Me.DataGridView1.Rows(e.RowIndex).Cells("iteid").Value.ToString)
                        frm.ShowDialog()
                    End Using
                End If



            Catch ex As Exception

                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End Try
        End If
    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        clicker = clicker + 1
        filter_pallets()
    End Sub

    Private Sub DataGridView2_DragOver(sender As Object, e As DragEventArgs) Handles DataGridView2.DragOver
        Try


            Dim p As Point = DataGridView2.PointToClient(New Point(e.X, e.Y))
            Dim hit As DataGridView.HitTestInfo = DataGridView2.HitTest(p.X, p.Y)

            'If hit.Type = DataGridViewHitTestType.Cell Then
            If hit.Type = DataGridViewHitTestType.Cell Then

                If DataGridView2.Rows(hit.RowIndex).Cells("ST").Value <> 1 Then
                    'Cursor.Current = oldcursor
                    DataGridView2.Rows(hit.RowIndex).Selected = True
                End If


            End If

        Catch
        End Try
    End Sub

    Private Sub Button12_MouseDown(sender As Object, e As MouseEventArgs) Handles Button12.MouseDown
        Dim info As DataGridView.HitTestInfo = Me.DataGridView1.HitTest(e.X, e.Y)
        mouseX = e.X
        mouseY = e.Y
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            PictureBox2.Visible = True
            CheckBox3.Checked = False
            CheckBox4.Checked = False
            CheckBox8.Checked = False
        ElseIf CheckBox2.Checked = False Then
            PictureBox2.Visible = False
            'updater(3)
        End If
        filtermanager()
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            PictureBox3.Visible = True
            CheckBox2.Checked = False
            CheckBox4.Checked = False

        ElseIf CheckBox3.Checked = False Then
            PictureBox3.Visible = False

        End If
        filtermanager()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            PictureBox4.Visible = True
            CheckBox3.Checked = False
            CheckBox8.Checked = False
        ElseIf CheckBox4.Checked = False Then
            PictureBox4.Visible = False

        End If
        filtermanager()
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked = True Then
            CheckBox6.Checked = False
            CheckBox5.Checked = False



        ElseIf CheckBox7.Checked = False Then

        End If
        filtermanager()
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            CheckBox7.Checked = False
            CheckBox5.Checked = False

        ElseIf CheckBox6.Checked = False Then

        End If
        filtermanager()
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            CheckBox7.Checked = False
            CheckBox6.Checked = False

        ElseIf CheckBox5.Checked = False Then

        End If
        filtermanager()
    End Sub

    Private Sub filtermanager()
        Me.DataGridView1.CurrentCell = Nothing


        For i As Integer = 0 To DataGridView1.Rows.Count - 1

            DataGridView1.Rows(i).Visible = True


        Next

        If CheckBox2.Checked = True Then

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.LightGreen Then
                    DataGridView1.Rows(i).Visible = False

                End If
            Next
            'updater(3)
        ElseIf CheckBox2.Checked = False Then

            'updater(3)
        End If
        If CheckBox3.Checked = True Then
            If CheckBox8.Checked = True Then
                For i As Integer = 0 To DataGridView1.Rows.Count - 1

                    If DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.White And DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.Khaki Then
                        DataGridView1.Rows(i).Visible = False

                    End If
                Next
            Else
                For i As Integer = 0 To DataGridView1.Rows.Count - 1
                    If DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.Khaki Then
                        DataGridView1.Rows(i).Visible = False

                    End If

                Next
            End If

            'updater(3)
        ElseIf CheckBox3.Checked = False Then

            'updater(3)
        End If
        If CheckBox4.Checked = True Then

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.Red Then
                    DataGridView1.Rows(i).Visible = False

                End If
            Next
            'updater(3)
        ElseIf CheckBox4.Checked = False Then

            'updater(3)
        End If
        If CheckBox8.Checked = True Then
            If CheckBox3.Checked = True Then
                For i As Integer = 0 To DataGridView1.Rows.Count - 1

                    If DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.White And DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.Khaki Then
                        DataGridView1.Rows(i).Visible = False

                    End If
                Next
            Else
                For i As Integer = 0 To DataGridView1.Rows.Count - 1

                    If DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.White Then
                        DataGridView1.Rows(i).Visible = False

                    End If
                Next
            End If
            'updater(3)
        ElseIf CheckBox8.Checked = False Then

            'updater(3)
        End If
        If CheckBox7.Checked = True Then

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If Not (DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("1") And Not DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("102")) Then
                    DataGridView1.Rows(i).Visible = False

                End If

            Next
        ElseIf CheckBox7.Checked = False Then

        End If
        If CheckBox6.Checked = True Then

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If Not (DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("102") Or DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("202") Or DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("280") Or DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("281") Or DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("285") Or DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("282")) Then
                    DataGridView1.Rows(i).Visible = False

                End If

            Next
        ElseIf CheckBox6.Checked = False Then

        End If
        If CheckBox5.Checked = True Then

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If Not (DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("2") And Not DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.StartsWith("202")) Then
                    DataGridView1.Rows(i).Visible = False

                End If

            Next
        ElseIf CheckBox5.Checked = False Then

        End If
        If ComboBox1.SelectedIndex = 2 Then
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If Not IsDBNull(DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value) Then

                    If DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Π" Or DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "A-Π" Then
                        DataGridView1.Rows(i).Visible = False

                    End If
                End If
            Next
        ElseIf ComboBox1.SelectedIndex = 1 Then
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If Not IsDBNull(DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value) Then
                    If DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α" Or DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "A-Π" Then
                        DataGridView1.Rows(i).Visible = False

                    End If
                End If
            Next
        End If
        Dim sum As Double = 0
        Dim remain As Double = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1

            If DataGridView1.Rows(i).Visible Then
                sum = sum + DataGridView1.Rows(i).Cells("ΠΟΣ").Value
                remain = remain + DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value
            End If
        Next
        Label21.Text = "Υπόλοιπο\Κατανεμημένα\Σύνολο ορατών:  " + remain.ToString + "\" + (sum - remain).ToString + "\" + sum.ToString
    End Sub



    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = True Then
            PictureBox5.Visible = True
            CheckBox2.Checked = False
            CheckBox4.Checked = False

        ElseIf CheckBox8.Checked = False Then
            PictureBox5.Visible = False

        End If
        filtermanager()
    End Sub

    Private Sub DataGridView1_Sorted(sender As Object, e As EventArgs) Handles DataGridView1.Sorted
        Dim GridSortOrder As System.Windows.Forms.SortOrder
        GridSortOrder = DataGridView1.SortOrder

        If GridSortOrder = System.Windows.Forms.SortOrder.Ascending Then
            SetSortOrderdgv1 = ListSortDirection.Ascending
        ElseIf GridSortOrder = System.Windows.Forms.SortOrder.Descending Then
            SetSortOrderdgv1 = ListSortDirection.Descending
        ElseIf GridSortOrder = System.Windows.Forms.SortOrder.None Then
            SetSortOrderdgv1 = ListSortDirection.Ascending

        End If

        order_columns(2)
        filtermanager()
    End Sub

    Private Sub TextBox1_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox1.DragDrop
        Using row As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
            If row IsNot Nothing Then
                Try

                    TextBox1.Text = row.Cells("ΠΑΡ").Value

                Catch

                End Try
            End If
        End Using
    End Sub

    Private Sub cuscodemaskbox_DragDrop(sender As Object, e As DragEventArgs) Handles cuscodemaskbox.DragDrop
        Using row As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
            If row IsNot Nothing And Not (row.Cells(3).Value.ToString Like "00ΠΑΞ*" Or row.Cells(3).Value.ToString Like "00BAC*") Then
                Try
                    cuscodemaskbox.Text = row.Cells("ΚΩΔΠΕΛ").Value

                Catch

                End Try
            End If
        End Using

    End Sub

    Private Sub callnamescbox_DragDrop(sender As Object, e As DragEventArgs) Handles callnamescbox.DragDrop
        Using row As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
            If row IsNot Nothing And Not (row.Cells(3).Value.ToString Like "00ΠΑΞ*" Or row.Cells(3).Value.ToString Like "00BAC*") Then
                Try
                    callnamescbox.Text = row.Cells("ΠΕΛ").Value

                Catch

                End Try
            End If
        End Using
    End Sub

    Private Sub TextBox1_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox1.DragEnter
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub TextBox2_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox2.DragEnter
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub cuscodemaskbox_DragEnter(sender As Object, e As DragEventArgs) Handles cuscodemaskbox.DragEnter
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub callnamescbox_DragEnter(sender As Object, e As DragEventArgs) Handles callnamescbox.DragEnter
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub TextBox2_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox2.DragDrop
        Using row As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
            If row IsNot Nothing And Not (row.Cells(3).Value.ToString Like "00ΠΑΞ*" Or row.Cells(3).Value.ToString Like "00BAC*") Then
                Try
                    TextBox2.Text = row.Cells(2).Value

                Catch

                End Try
            End If
        End Using
    End Sub

    Private Sub Panel1_DragEnter(sender As Object, e As DragEventArgs) Handles Panel1.DragEnter
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged

        selection_indicators()
        Dim sum As Double = 0
        Dim remain As Double = 0
        If DataGridView1.SelectedRows.Count > 0 Then
            For i As Integer = 0 To DataGridView1.Rows.Count - 1

                If DataGridView1.Rows(i).Selected Then
                    sum = sum + DataGridView1.Rows(i).Cells("ΠΟΣ").Value
                    remain = remain + DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value
                End If
            Next
            Label21.Text = "Υπόλοιπο\Κατανεμημένα\Σύνολο επιλεγμένων:  " + remain.ToString + "\" + (sum - remain).ToString + "\" + sum.ToString
        Else
            For i As Integer = 0 To DataGridView1.Rows.Count - 1

                If DataGridView1.Rows(i).Visible Then
                    sum = sum + DataGridView1.Rows(i).Cells("ΠΟΣ").Value
                    remain = remain + DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value
                End If
            Next
            Label21.Text = "Υπόλοιπο\Κατανεμημένα\Σύνολο ορατών:  " + remain.ToString + "\" + (sum - remain).ToString + "\" + sum.ToString

        End If

        'End If
    End Sub
    Private Sub selection_indicators()
        If DataGridView1.FirstDisplayedCell IsNot Nothing Then
            first_visible_index = DataGridView1.FirstDisplayedCell.RowIndex
            last_visible_index = DataGridView1.Rows.Count
            For i As Integer = DataGridView1.Rows.Count - 1 To DataGridView1.FirstDisplayedCell.RowIndex Step -1
                If DataGridView1.Rows(i).Displayed Then

                    last_visible_index = DataGridView1.Rows(i).Index
                    Exit For

                End If

            Next
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1.Rows(i).Index < first_visible_index And DataGridView1.Rows(i).Selected = True Then
                    PictureBox7.Visible = True
                    Exit For
                Else
                    PictureBox7.Visible = False

                End If
            Next
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1.Rows(i).Index > last_visible_index And DataGridView1.Rows(i).Selected = True Then
                    PictureBox8.Visible = True
                    Exit For
                Else
                    PictureBox8.Visible = False
                End If
            Next
        End If
    End Sub

    Private Sub DataGridView1_Scroll(sender As Object, e As ScrollEventArgs) Handles DataGridView1.Scroll

        selection_indicators()


    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If DataGridView1.RowCount > 0 Then

            Dim selectedRowCount As Integer = DataGridView1.SelectedRows.Count
            If selectedRowCount > 0 Then

                selection_indicators()
            End If
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Button2.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        'Using Me.Cursor = New Cursor(Cursor.Current.Handle)
        Dim message, title As String
        Dim defaultValue As Double
        Dim myValue As Object
        defaultValue = 1
        ' Set prompt.
        message = "Πόσες παλέτες επιθυμείτε να προστεθούν;"
        ' Set title.
        title = "Εισάγετε ποσότητα"
        ' Display message, title, and default value.
        Dim Valid As Boolean
        While Valid = False
            myValue = InputBox(message, title, defaultValue, Cursor.Position.X, Cursor.Position.Y)
            Try
                If IsNumeric(myValue) And myValue.ToString.Length <= 2 And myValue <> 0 Then
                    Valid = True
                ElseIf myValue Is "" Or myValue = 0 Then
                    Return
                Else
                    Valid = False
                End If
                If myValue > 50 Then
                    Throw New System.Exception("Δεν μπορείτε να εισάγετε πάνω από 50 παλέτες απευθείας.")
                End If
            Catch
                Return
            End Try
        End While
        For i As Integer = 1 To myValue
            Button2.PerformClick()
        Next
    End Sub

    Private Sub CheckBox9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked = True Then
            CheckBox10.Checked = False
            CheckBox11.Checked = False

        End If
        filter_pallets()
    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked = True Then
            CheckBox9.Checked = False
            CheckBox11.Checked = False


        End If
        filter_pallets()
    End Sub

    Private Sub CheckBox11_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox11.CheckedChanged
        If CheckBox11.Checked = True Then
            CheckBox10.Checked = False
            CheckBox9.Checked = False

        End If
        filter_pallets()
    End Sub

    Private Sub filter_pallets()
        FlowLayoutPanel1.Visible = False
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        TableLayoutPanel4.Controls.Add(pic, TableLayoutPanel4.GetColumn(FlowLayoutPanel1), TableLayoutPanel4.GetRow(FlowLayoutPanel1))
        pic.Dock = DockStyle.Fill
        For Each c As Control In FlowLayoutPanel1.Controls
            c.Visible = True
        Next
        Dim q As Integer = 0
        If FlowLayoutPanel1.Controls.Count = 0 Then
            If phdt.Rows.Count < 20 Then
                For Each i As KeyValuePair(Of Integer, String) In pindex
                    pallet_morph(i.Key, 1, q, pinned:=True)
                    q += 1
                Next
            Else
                For Each i As KeyValuePair(Of Integer, String) In pindex
                    pallet_morph(i.Key, 2, q)
                    q += 1
                Next
            End If
        End If
        Application.DoEvents()
        Dim visiblepallets As New List(Of Integer)
        Dim counter As Integer = 0
        For k As Integer = 0 To phdt.Rows.Count - 1
            Dim palletid As Integer = phdt.Rows(k).Item("ID")
            visiblepallets.Add(palletid)
            If clicker = 1 Then
                Button7.Text = "Προγραμματισμένες"
                Button7.BackColor = Color.LightSteelBlue
                Button7.Width = 140
                If IsDBNull(phdt.Rows(k).Item("status")) OrElse phdt.Rows(k).Item("status") >= 0 Then
                    visiblepallets.Remove(phdt.Rows(k).Item("ID"))
                End If
            ElseIf clicker = 2 Then
                Button7.Text = "Μόνο μη ολοκληρωμένες"
                Button7.BackColor = Color.Gainsboro
                Button7.Width = 159
                If Not IsDBNull(phdt.Rows(k).Item("CLOSEDBYID")) Or (Not IsDBNull(phdt.Rows(k).Item("status")) AndAlso phdt.Rows(k).Item("status") < 0) Then
                    visiblepallets.Remove(phdt.Rows(k).Item("ID"))
                End If
            ElseIf clicker = 3 Then
                Button7.Text = "Oλοκληρωμένες προς εισαγωγή"
                Button7.BackColor = Color.LightGreen
                Button7.Width = 196
                If IsDBNull(phdt.Rows(k).Item("CLOSEDBYID")) Or Not IsDBNull(phdt.Rows(k).Item("PLID")) Then
                    visiblepallets.Remove(phdt.Rows(k).Item("ID"))
                End If
            ElseIf clicker = 4 Then
                Button7.Text = "Κατανεμημένες σε Packing list"
                Button7.BackColor = Color.Khaki
                Button7.Width = 189
                If IsDBNull(phdt.Rows(k).Item("PLID")) Then
                    visiblepallets.Remove(phdt.Rows(k).Item("ID"))
                End If
            ElseIf clicker = 5 Then
                Button7.Text = "Όλες"
                Button7.BackColor = Color.White
                Button7.Width = 62
                clicker = 0
            End If
            If clicker2 = 1 Then
                Button26.Text = "Περιέχουν είδη Π"
                Button26.BackColor = Color.Gainsboro
                Button26.Width = 196
                Dim prodlist As New List(Of Integer)
                Dim flag As Boolean = False
                For i As Integer = 0 To DataGridView1.Rows.Count - 1
                    If Not IsDBNull(DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value) AndAlso DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Π" Then
                        prodlist.Add(DataGridView1.Rows(i).Cells("stlid").Value)
                    End If
                Next
                For i As Integer = 0 To pldt.Rows.Count - 1
                    If pldt.Rows(i).Item("PALLETID") = palletid Then
                        If prodlist.Contains(pldt.Rows(i).Item("STLID")) Then
                            flag = False
                            Exit For
                        End If
                        flag = True
                    End If
                Next
                If flag Then
                    visiblepallets.Remove(palletid)
                End If
            ElseIf clicker2 = 2 Then
                Button26.Text = "Περιέχουν είδη Α-Π"
                Button26.BackColor = Color.LightGray
                Button26.Width = 196
                Dim prodlist As New List(Of Integer)
                Dim flag As Boolean = False
                For i As Integer = 0 To DataGridView1.Rows.Count - 1
                    If Not IsDBNull(DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value) AndAlso DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α-Π" Then
                        prodlist.Add(DataGridView1.Rows(i).Cells("stlid").Value)
                    End If
                Next
                For i As Integer = 0 To pldt.Rows.Count - 1
                    If pldt.Rows(i).Item("PALLETID") = palletid Then
                        If prodlist.Contains(pldt.Rows(i).Item("STLID")) Then
                            flag = False
                            Exit For
                        End If
                        flag = True
                    End If
                Next
                If flag Then
                    visiblepallets.Remove(palletid)
                End If
            ElseIf clicker2 = 3 Then
                Button26.Text = "Περιέχουν είδη Α"
                Button26.BackColor = Color.Silver
                Button26.Width = 196
                Dim prodlist As New List(Of Integer)
                Dim flag As Boolean = False
                For i As Integer = 0 To DataGridView1.Rows.Count - 1
                    If IsDBNull(DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value) OrElse DataGridView1.Rows(i).Cells("ΑΠΟΔΕΚΤΗΣ").Value = "Α" Then
                        prodlist.Add(DataGridView1.Rows(i).Cells("stlid").Value)
                    End If
                Next
                For i As Integer = 0 To pldt.Rows.Count - 1
                    If pldt.Rows(i).Item("PALLETID") = palletid Then
                        If prodlist.Contains(pldt.Rows(i).Item("STLID")) Then
                            flag = False
                            Exit For
                        End If
                        flag = True
                    End If
                Next
                If flag Then
                    visiblepallets.Remove(palletid)
                End If
            ElseIf clicker2 = 4 Then
                Button26.Text = "Όλες"
                Button26.BackColor = Color.White
                Button26.Width = 62
                clicker2 = 0
            End If
            If CheckBox13.Checked Then
                If (Not IsDBNull(phdt.Rows(k).Item("CLOSEDBYID"))) _
                    Or (Not IsDBNull(phdt.Rows(k).Item("LOCKEDBYID")) AndAlso (Not (departments(phdt.Rows(k).Item("LOCKEDBYID") - 1) = activeuserdpt Or activeuserdpt = "SA" Or phdt.Rows(k).Item("atlantissalesmanid") = activeuseraid))) Then
                    visiblepallets.Remove(palletid)
                End If
            End If
            If CheckBox9.Checked = True Then
                If Not phdt.Rows(k).Item("loccode").ToString.Contains("L.") Then
                    visiblepallets.Remove(palletid)
                End If
            ElseIf CheckBox10.Checked = True Then
                If Not phdt.Rows(k).Item("loccode").ToString.Contains("P.") Then
                    visiblepallets.Remove(palletid)
                End If
            ElseIf CheckBox11.Checked = True Then
                If Not (phdt.Rows(k).Item("loccode").ToString.Contains("U.") Or phdt.Rows(k).Item("loccode").ToString.Contains("A.")) Then
                    visiblepallets.Remove(palletid)
                End If
            End If
        Next
        'clearpallets()
        counter = visiblepallets.Distinct.Count
        Label24.Visible = True
        If counter = 0 Then
            Label24.Text = "0 παλέτες ορατές"
            Label24.ForeColor = Color.Red
        ElseIf counter = phdt.Rows.Count Then
            Label24.Text = counter.ToString + " παλέτες"
            Label24.ForeColor = Color.Black
        Else
            Label24.Text = counter.ToString + " παλέτες"
            Label24.ForeColor = Color.DarkRed
        End If
        For Each pi As Control In FlowLayoutPanel1.Controls
            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
            Dim small As smallpallet = TryCast(pi, smallpallet)
            If normal IsNot Nothing Then
                If Not visiblepallets.Contains(normal.palletid) Then
                    normal.Visible = False
                End If
            ElseIf small IsNot Nothing Then
                If Not visiblepallets.Contains(small.palletid) Then
                    small.Visible = False
                End If
            End If
        Next
        For Each co As Control In TableLayoutPanel4.Controls
            If Not IsNothing(TryCast(co, PictureBox)) Then
                co.Dispose()
            End If
        Next
        FlowLayoutPanel1.Visible = True
    End Sub



    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        Try
            Dim message, title As String
            Dim defaultValue As Double
            Dim myValue As Object
            Dim availpal As String = ""
            Dim prevpalid As Integer = 0
            For Each pallet As Control In FlowLayoutPanel1.Controls
                Dim normal As pallettemplate = TryCast(pallet, pallettemplate)
                Dim small As smallpallet = TryCast(pallet, smallpallet)
                If normal IsNot Nothing Then
                    If normal.department = activeuserdpt And normal.locked = False And normal.closed = False Then
                        If prevpalid = 0 Then
                            prevpalid = normal.palletid
                        End If
                        If normal.palletid <= prevpalid Then
                            prevpalid = normal.palletid
                            availpal = normal.pallettemplatelabel.Text
                        End If
                    End If
                ElseIf small IsNot Nothing Then
                    If small.department = activeuserdpt And small.locked = False And small.closed = False Then
                        If prevpalid = 0 Then
                            prevpalid = small.palletid
                        End If
                        If small.palletid <= prevpalid Then
                            prevpalid = small.palletid
                            availpal = small.Label1.Text
                        End If
                    End If
                End If
            Next
            If rowtodistribute.Cells(iteidcolindex).Value = 65946 Or rowtodistribute.Cells(iteidcolindex).Value = 65947 Or rowtodistribute.Cells(iteidcolindex).Value = 65948 Then
                Throw New System.Exception("Δεν μπορείτε να τοποθετήσετε προσωρινό κωδικό σε παλέτα.")
            End If
            defaultValue = rowtodistribute.Cells(reqquantcolindex).Value
            If availpal = "" Then
                Throw New System.Exception("Δεν υπάρχει διαθέσιμη παλέτα.")
            End If
            ' Set prompt.
            message = "Τι μέρος της ποσότητας παραγγελίας θέλετε να τοποθετηθεί στη παλέτα " + availpal + ". Διαθέσιμη ποσότητα: " + defaultValue.ToString
            ' Set title.
            title = "Εισάγετε ποσότητα"
            ' Display message, title, and default value.
            Dim Valid As Boolean
            myValue = InputBox(message, title, defaultValue, Cursor.Position.X, Cursor.Position.Y)
            While Valid = False
                Try
                    If IsNumeric(myValue) And myValue.ToString.Length <= 5 And myValue > 0 Then
                        rowtodistribute.Cells(startquantcolindex).Value = myValue
                        Valid = True
                    ElseIf myValue Is "" Or myValue = 0 Then
                        Return
                    Else
                        Valid = False
                    End If
                Catch
                    Return
                End Try
            End While
            If myValue > defaultValue Then
                Throw New System.Exception("Δεν μπορείτε να υπερβείτε τη διαθέσιμη ποσότητα.")
            End If
            Dim exists As Boolean = False
            Dim quant As Double = 0
            Double.TryParse(myValue, quant)
            Using pm As New PalletManager(activeuserdpt, activeuser, activeuserid, activeuserdptid, cus_id:=CUSID)
                pm.AddItem(prevpalid, rowtodistribute.Cells("iteid").Value, rowtodistribute.Cells("stlid").Value, rowtodistribute.Cells("ftrid").Value, quant)
                'pallet_exchange(2, prevpalid, quant, rowtodistribute)
            End Using
            datagridview1_stlquantity(rowtodistribute.Cells(stlidcolindex).Value)
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




    'Private Sub ToolStripMenuItem4_Click_1(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
    '    Try

    '        Dim message, title As String
    '        Dim defaultValue As Double
    '        Dim myValue As Object


    '        Dim avpalcount As Integer = 0
    '        Dim prevpalid = New ArrayList()
    '        For Each pallet As Control In FlowLayoutPanel1.Controls
    '            Dim normal As pallettemplate = TryCast(pallet, pallettemplate)
    '            Dim small As smallpallet = TryCast(pallet, smallpallet)
    '            If normal IsNot Nothing Then


    '                If normal.department = activeuserdpt And normal.closed = False And normal.locked = False Then
    '                    avpalcount = avpalcount + 1
    '                    prevpalid.Add(normal.palletid)
    '                End If


    '            ElseIf small IsNot Nothing Then

    '                If normal.department = activeuserdpt And normal.closed = False And normal.locked = False Then
    '                    avpalcount = avpalcount + 1
    '                    prevpalid.Add(normal.palletid)
    '                End If

    '            End If

    '        Next
    '        If rowtodistribute.Cells(iteidcolindex).Value = 65946 Or rowtodistribute.Cells(iteidcolindex).Value = 65947 Or rowtodistribute.Cells(iteidcolindex).Value = 65948 Then
    '            Throw New System.Exception("Δεν μπορείτε να τοποθετήσετε προσωρινό κωδικό σε παλέτα.")
    '        End If
    '        defaultValue = rowtodistribute.Cells(reqquantcolindex).Value
    '        If avpalcount = 0 Then
    '            Throw New System.Exception("Δεν υπάρχει διαθέσιμη παλέτα.")
    '        End If
    '        ' Set prompt.
    '        message = "Τι μέρος της ποσότητας παραγγελίας θέλετε να τοποθετηθεί σε κάθε παλέτα του τμήματος " + activeuserdpt + ". Διαθέσιμη ποσότητα: " + defaultValue.ToString
    '        ' Set title.
    '        title = "Εισάγετε ποσότητα"
    '        ' Display message, title, and default value.
    '        Dim Valid As Boolean
    '        myValue = InputBox(message, title, defaultValue, Cursor.Position.X, Cursor.Position.Y)
    '        While Valid = False

    '            Try
    '                If IsNumeric(myValue) And myValue.ToString.Length <= 5 And myValue <> 0 Then

    '                    Valid = True
    '                ElseIf myValue Is "" Or myValue = 0 Then
    '                    Return
    '                Else
    '                    Valid = False
    '                End If


    '            Catch ex As Exception
    '                Return

    '            End Try

    '        End While
    '        Dim cmd As String = ""
    '        Dim cmd2 As String = ""
    '        If myValue > defaultValue Then
    '            Throw New System.Exception("Δεν μπορείτε να υπερβείτε τη διαθέσιμη ποσότητα.")
    '        End If
    '        If myValue * avpalcount > defaultValue Then
    '            Throw New System.Exception("Αν τοποθετηθεί η ποσότητα που εισάγατε σε κάθε παλέτα του τμήματος, θα υπερβαίνατε τη ποσότητα της παραγγελίας.")
    '        End If
    '        For j As Integer = 1 To avpalcount
    '            Dim exists As Boolean = False
    '            Dim quant As Double = 0
    '            Double.TryParse(myValue, quant)




    '            pallet_exchange(2, prevpalid(j - 1), quant, rowtodistribute)




    '        Next
    '        datagridview1_stlquantity(rowtodistribute.Cells(stlidcolindex).Value)


    '    Catch ex As Exception
    '        If updconn.State = ConnectionState.Open Then
    '            updconn.Close()
    '        End If
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try
    'End Sub

    'Private Sub ΚατανομήΜέρουςΠοσότηταςΣεΣυγκεκριμένεςΠαλέτεςΤουΤμήματοςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΚατανομήΜέρουςΠοσότηταςΣεΣυγκεκριμένεςΠαλέτεςΤουΤμήματοςToolStripMenuItem.Click
    '    Try
    '        Dim stlidv = rowtodistribute.Cells(stlidcolindex).Value
    '        Using frm As New distribute(rowtodistribute.Cells(stlidcolindex).Value, rowtodistribute.Cells(codecolindex).Value, rowtodistribute.Cells(namecolindex).Value, rowtodistribute.Cells(reqquantcolindex).Value, rowtodistribute)
    '            frm.ShowDialog()
    '        End Using
    '        datagridview1_stlquantity(stlidv)
    '    Catch ex As Exception
    '        If updconn.State = ConnectionState.Open Then
    '            updconn.Close()
    '        End If
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try
    'End Sub



    Private Sub TextBox3_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox3.KeyUp
        If e.KeyCode = Keys.Enter Then
            Try
                If TextBox3.Text.Length = 15 Then
                    counter = 0
                    selectedorder = ""
                    indexes.Clear()
                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
                        If DataGridView1.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value = TextBox3.Text Then
                            counter = counter + 1
                            indexes.Add(i)
                            selectedorder = DataGridView1.Rows(i).Cells("ΠΑΡ").Value
                        End If
                    Next
                    If counter = 0 Then
                        Throw New System.Exception("Ο κωδικός που σκανάρατε δεν βρέθηκε στα είδη που έχουν φορτωθεί")
                    End If
                    If counter > 0 Then
                        If Label19.Text.Length = 0 Then
                            Throw New System.Exception("Δεν έχετε επιλέξει παλέτα!")
                        End If
                        Using frm As New Form4
                            frm.ShowDialog()
                        End Using
                    End If
                    'For j As Integer = 0 To DataGridView1.Rows.Count - 1
                    '    If DataGridView1.Rows(j).Cells(2).Value = TextBox3.Text Then
                    '        DataGridView1.Rows(j).Selected = True
                    '    End If
                    'Next
                ElseIf (TextBox3.Text.Length = 8 Or TextBox3.Text.Length = 9) And TextBox3.Text.Substring(7, 1) = "-" Then
                    Dim pfound As Boolean = False
                    For Each pallet As Control In FlowLayoutPanel1.Controls
                        Dim normal As pallettemplate = TryCast(pallet, pallettemplate)
                        Dim small As smallpallet = TryCast(pallet, smallpallet)
                        If normal IsNot Nothing Then


                            If TextBox3.Text = normal.pallettemplatelabel.Text Then
                                Label19.Text = normal.pallettemplatelabel.Text
                                pfound = True
                                Exit For
                            End If


                        ElseIf small IsNot Nothing Then

                            If TextBox3.Text = small.Label1.Text Then
                                Label19.Text = small.Label1.Text
                                pfound = True
                                Exit For
                            End If

                        End If

                    Next
                    If pfound = False Then
                        TextBox3.Clear()
                        Throw New System.Exception("Δεν βρέθηκε η παλέτα στο επιλεγμένο packing list")
                    End If
                Else
                    Throw New System.Exception("Άγνωστη μορφή κωδικού")
                End If
                Me.TextBox3.Clear()
            Catch ex As Exception
                TextBox3.Clear()
                If updconn.State = ConnectionState.Open Then
                    updconn.Close()
                End If
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End Try
        End If

    End Sub




    Private Sub DataGridView4_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs)
        Dim info As DataGridView.HitTestInfo = Me.DataGridView1.HitTest(e.X, e.Y)
        'mouseX = e.X
        'mouseY = e.Y
        If e.Button = MouseButtons.Right AndAlso info.RowIndex > -1 Then
            ContextMenuStrip2.Items(0).Text = "Μεταφορά επιλεγμένων προϊόντων σε παλέτα του τμήματος " + activeuserdpt
            ContextMenuStrip2.Show(CType(sender, Control), e.Location)
        End If
    End Sub


    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Try
            If FlowLayoutPanel1.Controls.Count = 0 Or Label19.Text = "" Then
                Throw New System.Exception("Δημιουργείστε παλέτα πρώτα.")
            End If
            form8type = 3
            Using frm As New Form8
                frm.ShowDialog()
            End Using

        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        'Try
        '    If Not DataGridView1.SelectedRows.Count > 0 Then
        '        Throw New System.Exception("Επιλέξτε κάποιο είδος πρώτα!")
        '    End If
        '    If DataGridView1.SelectedRows.Count > 1 Then
        '        Throw New System.Exception("Επιλέξτε ένα μόνο είδος")
        '    End If
        '    Dim result As Integer = MessageBox.Show("ΠΡΟΣΟΧΗ! Αυτό θα διαγράψει το είδος από οποιαδήποτε ξεκλείδωτη παλέτα του τμήματος " + activeuserdpt + " βρεθεί. Είστε σίγουροι;" _
        '                                        , "ΠΡΟΣΟΧΗ!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        '    Dim selection As Integer
        '    Dim counter As Integer = 0
        '    If result = DialogResult.Yes Then
        '        For k As Integer = 0 To DataGridView1.Rows.Count - 1
        '            If DataGridView1.Rows(k).Selected Then
        '                selection = k
        '                Exit For
        '            End If
        '        Next
        '        Dim success As Integer = 0
        '        For Each pallet As Control In FlowLayoutPanel1.Controls
        '            using normal As pallettemplate = TryCast(pallet, pallettemplate)
        '            using small As smallpallet = TryCast(pallet, smallpallet)
        '            If normal IsNot Nothing Then


        '                If (normal.Label7.Text.Substring(0, 2) = activeuserdpt Or activeuserdpt = "SA") And Not normal.locked And Not normal.closed And Not pallet.BackColor = Color.Khaki Then

        '                    For i As Integer = 0 To normal.pallettemplatedatagrid.Rows.Count - 1

        '                        If normal.pallettemplatedatagrid.Rows(i).Cells("iteid").Value = DataGridView1.Rows(selection).Cells("iteid").Value And normal.pallettemplatedatagrid.Rows(i).Cells("stlid").Value = DataGridView1.Rows(selection).Cells("stlid").Value Then
        '                            success = pallet_exchange(-1, normal.palletid, 0, DataGridView1.Rows(selection))
        '                            If success <= 0 Then
        '                                Return
        '                            End If
        '                            counter = counter + 1
        '                            normal.pallettemplatedatagrid.Rows.RemoveAt(i)
        '                        End If

        '                    Next
        '                End If


        '            ElseIf small IsNot Nothing Then

        '                If TextBox3.Text = small.Label1.Text Then
        '                    Label19.Text = small.Label1.Text
        '                    pfound = True
        '                    Exit For
        '                End If

        '            End If

        '        Next
        '    End If
        '    datagridview1_refresh()
        '    orderdgv_refresh()
        '    Me.Label11.Text = "Διεγράφη ο κωδικός " + DataGridView1.Rows(selection).Cells("ΚΩΔΙΚΟΣ").Value + " από " + counter.ToString + " παλέτες."
        'Catch ex As Exception
        '    If updconn.State = ConnectionState.Open Then
        '        updconn.Close()
        '    End If
        '    If conn.State = ConnectionState.Open Then
        '        conn.Close()
        '    End If
        '    Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        'End Try

    End Sub

    Private Sub ΑποεπιλογήΌλωνToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑποεπιλογήΌλωνToolStripMenuItem.Click
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Visible Then


                DataGridView1.Rows(i).Cells("ΕΠΙΛ").Value = False



            End If
        Next
        selectedstlids.Clear()


    End Sub

    Private Sub ΕπιλογήΌλωνToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΕπιλογήΌλωνToolStripMenuItem.Click
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Visible Then
                Dim checked As Boolean = CType(DataGridView1.Rows(i).Cells("ΕΠΙΛ").Value, Boolean)
                If Not checked Then
                    DataGridView1.Rows(i).Cells("ΕΠΙΛ").Value = True
                    selectedstlids.Add(DataGridView1.Rows(i).Cells("stlid").Value)
                End If
            End If
        Next
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs)
        'Try
        '    For Each pallet As pallettemplate In FlowLayoutPanel1.Controls
        '        pallet.CheckBox1.Checked = False
        '        Dim cmd2 As String = "update tbl_palletheaders set lockedbyid=null where id=" + pallet.palletid
        '        Using sqlcmd2 As New SqlCommand(cmd2, updconn)
        '            updconn.Open()
        '            sqlcmd2.ExecuteNonQuery()
        '            updconn.Close()
        '        End Using
        '    Next
        '    'updater(1)
        'Catch ex As Exception
        '    If updconn.State = ConnectionState.Open Then
        '        updconn.Close()
        '    End If
        '    If conn.State = ConnectionState.Open Then
        '        conn.Close()
        '    End If
        '    Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        'End Try
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs)
        'Try
        '    For Each pallet As pallettemplate In FlowLayoutPanel1.Controls
        '        pallet.CheckBox1.Checked = True
        '        Dim cmd2 As String = "update tbl_palletheaders set lockedbyid=" + activeuserid + " where id=" + pallet.palletid
        '        Using sqlcmd2 As New SqlCommand(cmd2, updconn)
        '            updconn.Open()
        '            sqlcmd2.ExecuteNonQuery()
        '            updconn.Close()
        '        End Using
        '    Next
        '    'updater(1)
        'Catch ex As Exception
        '    If updconn.State = ConnectionState.Open Then
        '        updconn.Close()
        '    End If
        '    If conn.State = ConnectionState.Open Then
        '        conn.Close()
        '    End If
        '    Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        'End Try
    End Sub

    'Private Sub Button21_Click(sender As Object, e As EventArgs)
    '    Try
    '        For Each pallet As pallettemplate In FlowLayoutPanel1.Controls

    '            pallet.CheckBox1.Checked = True
    '            Dim cmd2 As String = "update tbl_palletheaders set lockedbyid=(select top 1 id from tbl_packeruserdata where department='" + pallet.Label7.Text.Substring(0, 2) + "') where id=" + pallet.palletid
    '            Using sqlcmd2 As New SqlCommand(cmd2, updconn)
    '                updconn.Open()
    '                sqlcmd2.ExecuteNonQuery()
    '                updconn.Close()
    '            End Using
    '        Next
    '        'updater(1)
    '    Catch ex As Exception
    '        If updconn.State = ConnectionState.Open Then
    '            updconn.Close()
    '        End If
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try
    'End Sub



    Private Sub DataGridView1_GiveFeedback(sender As Object, e As GiveFeedbackEventArgs) Handles DataGridView1.GiveFeedback

        e.UseDefaultCursors = False

        Cursor.Current = cur

    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs)
        Try
            'DataGridView1_CellContentDoubleClick(sender, clickedorder, True)
            rtime = 0
            Using PrintOpenLabels
                PrintOpenLabels.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub



    Private Sub orderdgv_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles orderdgv.CellDoubleClick
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then
            orderraw = e.RowIndex
            If orderdgv.Columns(e.ColumnIndex).Name = "F" AndAlso Not IsDBNull(orderdgv.Rows(orderraw).Cells("F").Value) Then
                Dim docs As New List(Of Integer)
                Using SQLCOM As New SqlCommand("SELECT DOCID FROM DOCREL WHERE MASTERID=" + orderdgv.Rows(orderraw).Cells("ID").Value.ToString, conn)
                    Using DT = New DataTable
                        conn.Open()

                        Using READER As SqlDataReader = SQLCOM.ExecuteReader
                            DT.Load(READER)
                            If DT.Rows.Count > 0 Then
                                'Button6.Visible = True
                                For i As Integer = 0 To DT.Rows.Count - 1
                                    docs.Add(DT.Rows(i).Item(0))
                                Next
                            End If

                        End Using
                        conn.Close()
                    End Using
                End Using
                Using frm As New reldocs(docs)
                    frm.ShowDialog()

                End Using
            ElseIf orderdgv.Columns(e.ColumnIndex).Name = "Status2" Then
                Dim dt As New DataTable()
                dt.Columns.Add("stlid", GetType(Integer))
                dt.Columns.Add("s")
                dt.Columns.Add("t", GetType(Double))
                Dim piecolumns As New Dictionary(Of String, String) From {{"RED", "Backorder"}, {"blue", "Σε σχεδιασμένες παλέτες"}, {"black", "Εκκρεμούν"}, {"lightgreen", "Σε παλέτα"}, {"gold", "Απεσταλμένα"}, {"green", "Σε κλειστές παλέτες"}}
                Dim ftrid = orderdgv.Rows(e.RowIndex).Cells("id").Value
                For Each c As KeyValuePair(Of String, String) In piecolumns
                    Dim dtrow As DataRow = dt.NewRow
                    dtrow("stlid") = ftrid
                    dtrow("s") = c.Value
                    dtrow("t") = orderdgv.Rows(e.RowIndex).Cells(c.Key).Value
                    dt.Rows.Add(dtrow)
                Next
                Dim f As New DatagridviewStackedProgressColumnReportForm(orderdgv.Rows(e.RowIndex).Cells("id").Value, "ΟΛΑ", orderdgv.Rows(e.RowIndex).Cells("ΠΑΡ").Value, Cursor.Position.X, Cursor.Position.Y, dt, stl_or_ftrid_id:=orderdgv.Rows(e.RowIndex).Cells("id").Value, rec_values:="1,2,3,4,5,6", type:="ftrid")
                f.Owner = Me
                f.Show()
            Else

                Dim frm As New Order(orderdgv.Rows(orderraw).Cells("ID").Value)
                frm.Show()
            End If


        End If
    End Sub


    Private Sub TabControl2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl2.SelectedIndexChanged
        If TabControl2.SelectedIndex = 0 Or TabControl2.SelectedIndex = 1 Then
            CheckBox12.Visible = True

        Else
            CheckBox12.Visible = False
            CheckBox12.Checked = False
        End If
        If TabControl2.SelectedIndex = 1 Then
            Label21.Visible = True
            Button16.Visible = True
        Else
            Label21.Visible = False
            Button16.Visible = False
        End If
    End Sub

    Private Sub CheckBox12_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox12.CheckedChanged
        If CheckBox12.Checked = True Then
            SplitContainer1.Panel2Collapsed = True
            CheckBox12.Image = My.Resources.icons8_back_16
            CheckBox2.BackColor = Color.Transparent
        Else
            SplitContainer1.Panel2Collapsed = False
            CheckBox12.Image = My.Resources.icons8_forward_16
        End If


    End Sub

    Private Sub Button25_Click_1(sender As Object, e As EventArgs)
        Form13.ShowDialog()
    End Sub

    'Private Sub ΜικρέςΠαλέτεςToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ΜικρέςΠαλέτεςToolStripMenuItem1.Click
    '    VIEWSTYLE = "SMALL"
    '    FlowLayoutPanel1.SuspendLayout()
    '    For Each pallet As pallettemplate In FlowLayoutPanel1.Controls
    '        pallet.viewstyle = "SMALL"
    '    Next
    '    FlowLayoutPanel1.ResumeLayout()
    'End Sub



    'Private Sub ΜεγάλεςΠαλέτεςToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ΜεγάλεςΠαλέτεςToolStripMenuItem1.Click
    '    VIEWSTYLE = "LARGE"
    '    FlowLayoutPanel1.SuspendLayout()
    '    For Each pallet As pallettemplate In FlowLayoutPanel1.Controls

    '        pallet.viewstyle = "LARGE"
    '    Next
    '    FlowLayoutPanel1.ResumeLayout()

    'End Sub

    'Private Sub ΜεσαίεςΠαλέτεςToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ΜεσαίεςΠαλέτεςToolStripMenuItem1.Click
    '    VIEWSTYLE = "MEDIUM"
    '    FlowLayoutPanel1.SuspendLayout()
    '    For Each pallet As pallettemplate In FlowLayoutPanel1.Controls

    '        pallet.viewstyle = "MEDIUM"
    '    Next
    '    FlowLayoutPanel1.ResumeLayout()
    'End Sub







    Private Sub Button22_Click(sender As Object, e As EventArgs)
        palletsforlabels = "0"
        For Each pi As Control In FlowLayoutPanel1.Controls
            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
            Dim small As smallpallet = TryCast(pi, smallpallet)
            If normal IsNot Nothing Then
                If normal.Visible Then
                    palletsforlabels = palletsforlabels + "," + normal.palletid
                End If
            ElseIf small IsNot Nothing Then
                If small.Visible Then
                    palletsforlabels = palletsforlabels + "," + small.palletid
                End If
            End If
        Next
        Using Form14
            Form14.ShowDialog()
        End Using
    End Sub

    Private Sub TextBox3_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox3.DragDrop
        Dim row As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
        If row IsNot Nothing Then
            Try
                TextBox3.Text = row.Cells(2).Value
                TextBox3.Focus()
                SendKeys.Send("{ENTER}")
            Catch

            End Try
        End If
    End Sub

    Private Sub TextBox3_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox3.DragEnter
        e.Effect = DragDropEffects.All
    End Sub
    Private Sub ΕπισκόπησηΣύνθεσηςΘέσεωνΑποθήκευσηςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΕπισκόπησηΣύνθεσηςΘέσεωνΑποθήκευσηςToolStripMenuItem.Click
        Try
            Using frm = New ItemDetails(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("iteid").Value.ToString)
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Dim locids As New List(Of String)
    Dim loccodes As New List(Of String)
    Dim palletids As New List(Of String)
    Dim palletcodes As New List(Of String)

    Private Sub pallets_for_mantis()
        Cursor.Current = ExtCursor1.Cursor
        locids.Clear()
        loccodes.Clear()
        palletids.Clear()
        palletcodes.Clear()
        populate_pallets("ftrid")
        For Each pi As Control In FlowLayoutPanel1.Controls
            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
            Dim small As smallpallet = TryCast(pi, smallpallet)
            If normal IsNot Nothing Then
                If Not normal.locationID = 0 Then
                    If normal.BackColor = Color.Gainsboro And (normal.Label5.Text = activeuser Or (normal.department = "BP" And activeuserdpt = "BP")) Then
                        locids.Add(normal.locationID.ToString)
                        loccodes.Add(normal.locationCode)
                        palletids.Add(normal.palletid.ToString)
                        palletcodes.Add(normal.pallettemplatelabel.Text)
                    End If
                End If
            ElseIf small IsNot Nothing Then
                If Not small.locationID = 0 Then
                    If small.BackColor = Color.Gainsboro And (small.Label5.Text.ToString.Contains(activeuser) Or (small.department = "BP" And activeuserdpt = "BP")) Then
                        locids.Add(small.locationID.ToString)
                        loccodes.Add(small.locationCode)
                        palletids.Add(small.palletid.ToString)
                        palletcodes.Add(small.Label1.Text)
                    End If
                End If
            End If

            '    If p.locationCode Like "*.?U.*" Then
            '        locids.Add(p.locationID.ToString)
            '        loccodes.Add(p.locationCode)
            '        palletids.Add(p.palletid.ToString)
            '        palletcodes.Add(p.pallettemplatelabel.Text)
            '    End If
            'ElseIf activeuser = "PHILIPPOU" Then
            '    If ComboBox2.SelectedItem = "Αποθήκης (._A.)" Then

            '        If p.locationCode Like "*.?A.*" Then
            '            locids.Add(p.locationID.ToString)
            '            loccodes.Add(p.locationCode)
            '            palletids.Add(p.palletid.ToString)
            '            palletcodes.Add(p.pallettemplatelabel.Text)
            '        End If
            '    ElseIf ComboBox2.SelectedItem = "Υπογείου (._Y.)" Then
            '        If p.locationCode Like "*.?Y.*" Then
            '            locids.Add(p.locationID.ToString)
            '            loccodes.Add(p.locationCode)
            '            palletids.Add(p.palletid.ToString)
            '            palletcodes.Add(p.pallettemplatelabel.Text)
            '        End If
            '    End If
            'ElseIf activeuser = "THEOFILATOS" Or activeuser = "SUPERVISOR" Then
            '    If ComboBox2.SelectedItem = "Αποθήκης (._A.)" Then

            '        If p.locationCode Like "*.?A.*" Then
            '            locids.Add(p.locationID.ToString)
            '            loccodes.Add(p.locationCode)
            '            palletids.Add(p.palletid.ToString)
            '            palletcodes.Add(p.pallettemplatelabel.Text)
            '        End If
            '    ElseIf ComboBox2.SelectedItem = "Υπογείου (._Y.)" Then
            '        If p.locationCode Like "*.?Y.*" Then
            '            locids.Add(p.locationID.ToString)
            '            loccodes.Add(p.locationCode)
            '            palletids.Add(p.palletid.ToString)
            '            palletcodes.Add(p.pallettemplatelabel.Text)
            '        End If
            '    ElseIf ComboBox2.SelectedItem = "Καψάνη (_U.)" Then
            '        If p.locationCode Like "*.?U.*" Then
            '            locids.Add(p.locationID.ToString)
            '            loccodes.Add(p.locationCode)
            '            palletids.Add(p.palletid.ToString)
            '            palletcodes.Add(p.pallettemplatelabel.Text)
            '        End If
            '    End If
            'End If

        Next
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs)
        Try
            Cursor.Current = ExtCursor1.Cursor
            If Not nodistribute And FlowLayoutPanel1.Controls.Count > 0 Then
                Dim cs As String() = CUSTOMER.Split(" ")
                Dim cmd As String = "select FTRID,s.a as 'ΠΑΡ',z.iteid,stlid,diff as 'ΠΟΣ',m.CODE 'ΚΩΔΙΚΟΣ',m.DESCRIPTION 'ΠΕΡΙΓΡΑΦΗ' from [dbo].[Z_PACKER_PENDING_ITEMS_PER_ORDER] z 
                            inner join fintrade f on f.id=z.FTRID INNER JOIN CUSTOMER C ON C.ID=F.CUSID
                            left join material m on m.id=z.iteid
                            cross apply (select   dbo.get_tradecode(f.id) a) s
                            where f.DSRID in (9000,9008) and diff<>0
                            And C.CODE='" + cs(0) + "'"  'ολα τα ειδη που εκκρεμουν για εναν πελατη σε ολες τις παραγγελιες του
                Dim check1 As String = ""
                Dim check2 As String = ""
                Dim fs As String = ""
                Dim fs0 As String = ""
                Using small As New SqlCommand("select distinct ftrid from Z_PACKER_PENDING_ITEMS_PER_ORDER z inner join fintrade f on f.id=z.ftrid inner join customer c on c.id=f.cusid where f.dsrid in (9000,9008) and diff<>0 and c.code='" + cs(0).ToString + "'", conn)
                    Using dt0 As New DataTable
                        conn.Open()
                        Using dt0reader As IDataReader = small.ExecuteReader
                            dt0.Load(dt0reader)
                            conn.Close()
                            fs0 = String.Join(",", dt0.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray())
                        End Using
                    End Using
                End Using
                Dim start As String = "select id from fintrade where id in  (" + fs0 + ") 
                                    or sc_relftrid in (" + fs0 + ")"
                Using startcom As New SqlCommand(start, conn)

                    Using startdt As New DataTable
                        conn.Open()
                        Using startreader As SqlDataReader = startcom.ExecuteReader
                            startdt.Load(startreader)
                            conn.Close()
                            Dim f = startdt.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray()
                            fs = String.Join(",", f)
                        End Using
                    End Using
                End Using
                check1 = "select mtr.code 'ΕΙΔΟΣ',mtr.DESCRIPTION 'ΠΕΡΙΓΡΑΦΗ' from
                                        (select stl.iteid i,sum(stl.primaryqty) s from storetradelines stl
										where ftrid in  (" + fs + ")
                                        group by stl.iteid) a
                                        inner join
                                        ( select iteid i,sum(lsumqty) s from sc_qty_mantisax_returns where locationid in
                                        (select locid from TBL_PALLETHEADERS ph where ph.PLID is  null and substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + cs(1).ToString.Replace(".", "") + "') 
                                        group by iteid   ) m      
                                        on a.i=m.i        
                                        inner join material mtr on mtr.ID=a.i      
                                        where m.s>a.s    "
                check2 = "select m.code 'ΕΙΔΟΣ',m.description 'ΠΕΡΙΓΡΑΦΗ',s.locationcode 'ΘΕΣΗ ΑΠΟΘΗΚΕΥΣΗΣ' from  sc_qty_mantisax_returns s inner join material m on m.id=s.iteid
                                    where locationid in
                                    (select locid from TBL_PALLETHEADERS ph where ph.PLID is null and substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + cs(1).ToString.Replace(".", "") + "') 
                                    and iteid not in
                                    (select iteid from STORETRADELINES where ftrid in (" + fs + "))"
                check1 = "select mtr.code 'ΕΙΔΟΣ',mtr.DESCRIPTION 'ΠΕΡΙΓΡΑΦΗ' from
                                        (select stl.iteid i,sum(stl.primaryqty) s from storetradelines stl
										inner Join fintrade f on f.id=stl.ftrid
                                        where f.id In  (Select distinct ftrid from Z_PACKER_PENDING_ITEMS_PER_ORDER z inner join fintrade f On f.id=z.ftrid inner join customer c On c.id=f.cusid where f.dsrid In (9000, 9008) And diff<>0 And c.code='" + cs(0).ToString + "') or f.sc_relftrid in (select distinct ftrid from Z_PACKER_PENDING_ITEMS_PER_ORDER z inner join fintrade f on f.id=z.ftrid inner join customer c on c.id=f.cusid where f.dsrid in (9000,9008) and diff<>0 and c.code='" + cs(0).ToString + "')
                                        Group by stl.iteid) a
                                        inner Join
                                        ( select iteid i,sum(lsumqty) s from sc_qty_mantisax_returns where locationid in
                                        (select locid from TBL_PALLETHEADERS ph where ph.PLID Is  null And substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + cs(1).ToString.Replace(".", "") + "') 
                                        Group by iteid   ) m      
                                        On a.i=m.i        
                                        inner Join material mtr on mtr.ID=a.i      
                                        where m.s > a.s  "
                check2 = "Select m.code 'ΕΙΔΟΣ',m.description 'ΠΕΡΙΓΡΑΦΗ',s.locationcode 'ΘΕΣΗ ΑΠΟΘΗΚΕΥΣΗΣ' from  sc_qty_mantisax_returns s inner join material m on m.id=s.iteid
                                    where locationid in
                                    (select locid from TBL_PALLETHEADERS ph where ph.PLID is null and substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + cs(1).ToString.Replace(".", "") + "') 
                                    and iteid not in
                                    (select iteid from STORETRADELINES where ftrid in (select distinct ftrid from Z_PACKER_PENDING_ITEMS_PER_ORDER z inner join fintrade f on f.id=z.ftrid inner join customer c on c.id=f.cusid where f.dsrid in (9000,9008) and diff<>0 and c.code='" + cs(0).ToString + "'))"

                Using checkcomm1 As New SqlCommand(check1, conn)
                    Using checkcomm2 As New SqlCommand(check2, conn)
                        Using ch1dt As New DataTable()
                            Using ch2dt As New DataTable()
                                conn.Open()

                                Using cr1 As SqlDataReader = checkcomm1.ExecuteReader
                                    ch1dt.Load(cr1)
                                    Using cr2 As SqlDataReader = checkcomm2.ExecuteReader


                                        ch2dt.Load(cr2)
                                        conn.Close()
                                        If ch2dt.Rows.Count > 0 Or ch1dt.Rows.Count > 0 Then
                                            Using frm As New mantiserror(ch1dt, ch2dt)
                                                frm.ShowDialog()
                                            End Using
                                            Return
                                        End If
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using
                End Using
                Using comm As New SqlCommand(cmd, updconn)
                    Using dt = New DataTable()
                        updconn.Open()
                        Using reader As SqlDataReader = comm.ExecuteReader()
                            dt.Load(reader)
                            updconn.Close()
                            If dt.Rows.Count = 0 Then
                                Throw New System.Exception("Έχουν κατανεμηθεί όλα τα είδη της παραγγελίας.")
                            End If
                            dt.Columns.Add("newQuant", Type.GetType("System.Double"))

                        End Using

                        pallets_for_mantis()

                        If locids.Count = 0 Then
                            Throw New System.Exception("Δεν υπάρχουν διαθέσιμες παλέτες/θέσεις αποθήκευσης που είναι κλειδωμένες από εσάς.")
                        End If
                        Dim cmd1 As String = "select iteid,LSUMQTY,locationID from SC_QTY_MANTISAX_RETURNS where locationID in (" + String.Join(",", locids.ToArray()) + ")"

                        Using comm1 As New SqlCommand(cmd1, conn)
                            Using dt1 = New DataTable()
                                conn.Open()
                                Using reader1 As SqlDataReader = comm1.ExecuteReader()
                                    dt1.Load(reader1)
                                    conn.Close()
                                    If dt1.Rows.Count = 0 Then
                                        Throw New System.Exception("Δεν έχουν καταχωρηθεί είδη στις διαθέσιμες θέσεις αποθήκευσης/παλέτες.")
                                    End If
                                    Using dt3 = New DataTable()
                                        dt3.Columns.Add("ΚΩΔΙΚΟΣ", Type.GetType("System.String"))
                                        dt3.Columns.Add("ΠΕΡΙΓΡΑΦΗ", Type.GetType("System.String"))
                                        dt3.Columns.Add("ΠΑΡ", Type.GetType("System.String"))
                                        dt3.Columns.Add("Παλέτα", Type.GetType("System.String"))
                                        dt3.Columns.Add("Θέση αποθήκευσης", Type.GetType("System.String"))
                                        dt3.Columns.Add("Ποσότητα", Type.GetType("System.Double"))
                                        dt3.Columns.Add("stlid", Type.GetType("System.Int32"))
                                        dt3.Columns.Add("ftrid", Type.GetType("System.Int32"))
                                        dt3.Columns.Add("palletid", Type.GetType("System.Int32"))
                                        dt3.Columns.Add("ΚΩΔΠΕΛ", Type.GetType("System.String"))
                                        dt3.Columns.Add("ΠΕΛ", Type.GetType("System.String"))
                                        dt3.Columns.Add("iteid", Type.GetType("System.Int32"))

                                        For i As Integer = 0 To dt.Rows.Count - 1
                                            dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("ΠΟΣ")

                                            For j As Integer = 0 To dt1.Rows.Count - 1
                                                If dt1.Rows(j).Item("iteid") = dt.Rows(i).Item("iteid") Then
                                                    Dim IND As Integer = locids.IndexOf(dt1.Rows(j).Item("locationID").ToString)
                                                    Dim quantity As Double = 0


                                                    If dt.Rows(i).Item("newQuant") <= 0 Then

                                                        Continue For

                                                    ElseIf dt1.Rows(j).Item("lsumqty") <= dt.Rows(i).Item("newQuant") Then
                                                        quantity = dt1.Rows(j).Item("lsumqty")
                                                    Else
                                                        quantity = dt.Rows(i).Item("newQuant")

                                                    End If
                                                    dt1.Rows(j).Item("lsumqty") = dt1.Rows(j).Item("lsumqty") - quantity
                                                    dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("newQuant") - quantity
                                                    Dim dr As DataRow = dt3.NewRow
                                                    dr("ΚΩΔΙΚΟΣ") = dt.Rows(i).Item("ΚΩΔΙΚΟΣ")
                                                    dr("ΠΕΡΙΓΡΑΦΗ") = dt.Rows(i).Item("ΠΕΡΙΓΡΑΦΗ")
                                                    dr("ΠΑΡ") = dt.Rows(i).Item("ΠΑΡ")
                                                    dr("Παλέτα") = palletcodes(IND)
                                                    dr("Θέση αποθήκευσης") = loccodes(IND)
                                                    dr("Ποσότητα") = quantity
                                                    dr("stlid") = dt.Rows(i).Item("stlid")
                                                    dr("ftrid") = dt.Rows(i).Item("ftrid")
                                                    dr("iteid") = dt.Rows(i).Item("iteid")
                                                    dr("palletid") = palletids(IND)
                                                    dr("ΠΕΛ") = cs(1)
                                                    dr("ΚΩΔΠΕΛ") = cs(0)
                                                    If dr("Ποσότητα") = 0 Then
                                                        Continue For
                                                    Else

                                                        dt3.Rows.Add(dr)
                                                    End If
                                                End If
                                            Next
                                        Next
                                        For i As Integer = 0 To dt.Rows.Count - 1
                                            If dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("ΠΟΣ") Then
                                                dt.Rows(i).Delete()
                                            End If
                                        Next
                                        Using frm As New datafrommantisfrm(dt, dt3)
                                            frm.ShowDialog()
                                        End Using
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using
                End Using

            End If
            Cursor.Current = Cursors.Default
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

    Public Sub change_frommantis()

        'If activeuserdpt = "SP" Or activeuserdpt = "SA" Or activeuserdpt = "BP" Or activeuserdpt = "BL" Then
        '    Button14.Enabled = True
        '    Button23.Enabled = False
        '    For i As Integer = 0 To pldt.Rows.Count - 1
        '        If pldt.Rows(i).Item("frommantis") = 1 Then
        '            Button14.Enabled = False
        '            Button23.Enabled = True
        '            Exit For
        '        End If
        '    Next

        'Else
        '    Button14.Enabled = False
        '    Button23.Enabled = False

        '    Return
        'End If

    End Sub

    Public Sub pallet_morph(ByVal id As Integer, ByVal type As Integer, ByVal flpindex As Integer, Optional pinned As Boolean = False, Optional c As Control = Nothing)
        FlowLayoutPanel1.SuspendLayout()
        Dim info As String = pindex(id)
        Dim strs As String() = info.Split(",") 'strings(1) phdt index, strings(2) pldt start, strings(3) pldt end
        Dim phi As Integer = CInt(strs(0))

        Dim pls As Integer = CInt(strs(1))
        Dim ple As Integer = CInt(strs(2))

        palletdeforder(id) = flpindex
        If type = 1 Then 'smallpallet->pallettemplate
            Dim p As New pallettemplate

            With p

                .palletid = Me.phdt.Rows(phi).Item("ID").ToString
                If Not Me.phdt.Rows(phi).Item("atlantissalesmanid") = 0 Then
                    p.salesman = Me.phdt.Rows(phi).Item("atlantissalesmanid").ToString
                End If
                If Not Len(Me.phdt.Rows(phi).Item("weight").ToString) = 0 Then
                    p.TextBox2.Text = Me.phdt.Rows(phi).Item("weight").ToString
                End If
                If Not Len(Me.phdt.Rows(phi).Item("netweight").ToString) = 0 Then
                    p.TextBox4.Text = Me.phdt.Rows(phi).Item("netweight").ToString
                End If
                p.ComboBox1.SelectedValue = Me.phdt.Rows(phi).Item("pallettypeid")
                If Not Len(Me.phdt.Rows(phi).Item("OPENDATE").ToString) = 0 Then
                    .TextBox10.Text = Me.phdt.Rows(phi).Item("OPENDATE").ToString
                End If
                p.TextBox3.Text = Me.usernames(Me.phdt.Rows(phi).Item("createuser") - 1)
                If Not Len(Me.phdt.Rows(phi).Item("lupdateuser").ToString) = 0 Then
                    p.TextBox8.Text = Me.usernames(Me.phdt.Rows(phi).Item("lupdateuser") - 1)
                End If
                If Not Len(Me.phdt.Rows(phi).Item("remarks").ToString) = 0 Then
                    p.TextBox1.Text = Me.phdt.Rows(phi).Item("remarks").ToString
                End If
                p.pallettemplatelabel.Text = Me.phdt.Rows(phi).Item("code")
                p.cusid = Me.phdt.Rows(phi).Item("cusid")
                p.customer = Me.phdt.Rows(phi).Item("FATHERNAME").ToString.Replace(".", "")
                p.CreateDPT_ID = Me.phdt.Rows(phi).Item("createdptid")
                p.department = Me.phdt.Rows(phi).Item("department")
                If Not Me.phdt.Rows(phi).Item("locid") = 0 Then
                    p.locationID = Me.phdt.Rows(phi).Item("locid")
                End If
                If Not Len(Me.phdt.Rows(phi).Item("loccode")) = 0 Then
                    p.locationCode = Me.phdt.Rows(phi).Item("loccode")
                End If
                If Not IsDBNull(phdt.Rows(phi).Item("Status")) AndAlso phdt.Rows(phi).Item("Status") < 0 Then
                    p.status = phdt.Rows(phi).Item("Status")
                End If
                If phdt.Rows(phi).Item("printuserid") <> 0 Then
                    p.SpecialReadOnly = True
                End If
                If Not IsDBNull(phdt.Rows(phi).Item("Status")) AndAlso phdt.Rows(phi).Item("Status") < 0 Then
                    p.IsDraft = True
                End If
                If Not Len(Me.phdt.Rows(phi).Item("LOCKEDBYID").ToString) = 0 Then
                    p.lockedbydpt = Me.usernames(Me.phdt.Rows(phi).Item("LOCKEDBYID") - 1) + "," + Me.departments(Me.phdt.Rows(phi).Item("LOCKEDBYID") - 1)
                End If
                If Not Len(Me.phdt.Rows(phi).Item("CLOSEDBYID").ToString) = 0 Then
                    p.closedbydpt = Me.usernames(Me.phdt.Rows(phi).Item("CLOSEDBYID") - 1) + "," + Me.departments(Me.phdt.Rows(phi).Item("CLOSEDBYID") - 1)
                End If
                If Not Len(Me.phdt.Rows(phi).Item("PLID").ToString) = 0 Then
                    p.plist = Me.phdt.Rows(phi).Item("pcode").ToString
                End If
                If Not Len(Me.phdt.Rows(phi).Item("orders").ToString) = 0 Then
                    p.TextBox9.Text = Me.phdt.Rows(phi).Item("orders")
                End If
                If pls <> -1 Then

                    For i As Integer = pls To ple
                        p.pallettemplatedatagrid.Rows.Add()
                        Dim rowindex As Integer = p.pallettemplatedatagrid.Rows.Count - 1
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("iteid").Value = Me.pldt.Rows(i).Item("iteid").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("quant").Value = Me.pldt.Rows(i).Item("quantity").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("ΚΩΔΙΚΟΣ").Value = Me.pldt.Rows(i).Item("code").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("ΠΕΡΙΓΡΑΦΗ").Value = Me.pldt.Rows(i).Item("description").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("ΠΑΡ").Value = Me.pldt.Rows(i).Item("tradecode").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("ΚΩΔΠΕΛ").Value = Me.pldt.Rows(i).Item("CUSCODE").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("ΠΕΛ").Value = Me.pldt.Rows(i).Item("FATHERNAME").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("stlid").Value = Me.pldt.Rows(i).Item("stlid").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("ftrid").Value = Me.pldt.Rows(i).Item("ftrid").ToString
                        p.pallettemplatedatagrid.Rows(rowindex).Cells("dailyplanid").Value = Me.pldt.Rows(i).Item("dailyplanid")
                        If Me.pldt.Rows(i).Item("dailyplanid") <> 0 And Not p.hasdailyplan Then
                            p.hasdailyplan = True
                        End If
                        If Me.pldt.Rows(i).Item("frommantis") <> 0 Then
                            p.pallettemplatedatagrid.Rows(rowindex).Cells("frommantis").Value = Me.pldt.Rows(i).Item("frommantis").ToString
                            p.mantis = Me.pldt.Rows(i).Item("frommantis").ToString
                            p.pallettemplatedatagrid.Rows(rowindex).ReadOnly = True
                            p.pallettemplatedatagrid.Rows(rowindex).DefaultCellStyle.BackColor = Color.LightGray
                            p.pallettemplatedatagrid.Rows(rowindex).DefaultCellStyle.SelectionBackColor = Color.Gray
                        End If
                    Next
                End If
                p.pallettemplatedatagrid.Sort(p.pallettemplatedatagrid.Columns("ΚΩΔΙΚΟΣ"), ListSortDirection.Ascending)
                p.compute_sums()
            End With
            p.viewstyle = VIEWSTYLE
            Me.FlowLayoutPanel1.Controls.Add(p)
            Me.FlowLayoutPanel1.Controls.SetChildIndex(p, flpindex)
            p.wassmall = True
            If pinned Then
                p.pinned = flpindex
                nextpinnedlocation += 1
                'Else
                '    p.wassmall = True
            End If
            p.pallettemplatedatagrid.Sort(p.pallettemplatedatagrid.Columns("ΚΩΔΙΚΟΣ"), ListSortDirection.Ascending)
            p.SetUpComplete = True
        ElseIf type = 2 Then 'pallettemplate->smallpallet
            Dim pallet As New smallpallet
            pallet.Label1.Text = phdt.Rows(phi).Item("code")
            pallet.palletid = phdt.Rows(phi).Item("ID")
            pallet.customer = Regex.Replace(pallet.Label1.Text, "[^a-zA-Z ]", "")
            pallet.cusid = phdt.Rows(phi).Item("cusid").ToString
            If Not Me.phdt.Rows(phi).Item("atlantissalesmanid") = 0 Then
                pallet.salesman = Me.phdt.Rows(phi).Item("atlantissalesmanid").ToString
            End If
            If Not phdt.Rows(phi).Item("loccode") = "0" Then
                pallet.Label2.Text = phdt.Rows(phi).Item("loccode")
                pallet.locationCode = phdt.Rows(phi).Item("loccode")
            End If
            If Not phdt.Rows(phi).Item("locid") = 0 Then
                pallet.locationID = phdt.Rows(phi).Item("locid")
            End If
            If Not Len(phdt.Rows(phi).Item("LOCKEDBYID").ToString) = 0 Then
                pallet.lockedbydpt = usernames(phdt.Rows(phi).Item("LOCKEDBYID") - 1) + "," + departments(phdt.Rows(phi).Item("LOCKEDBYID") - 1)
            End If
            If Not Len(phdt.Rows(phi).Item("CLOSEDBYID").ToString) = 0 Then
                pallet.closedbydpt = usernames(phdt.Rows(phi).Item("CLOSEDBYID") - 1) + "," + departments(phdt.Rows(phi).Item("CLOSEDBYID") - 1)
            End If

            If Not Len(phdt.Rows(phi).Item("PLID").ToString) = 0 Then
                pallet.plist = phdt.Rows(phi).Item("pcode").ToString
            End If
            If Not Len(phdt.Rows(phi).Item("remarks").ToString) = 0 Then
                pallet.Label3.Visible = True
                pallet.Label3.Text = Strings.Left(phdt.Rows(phi).Item("remarks").ToString, 50).Replace(Environment.NewLine, " ")
            Else
                pallet.Label3.Visible = False
            End If
            If Not IsDBNull(phdt.Rows(phi).Item("Status")) AndAlso phdt.Rows(phi).Item("Status") < 0 Then
                pallet.IsDraft = True
                pallet.status = phdt.Rows(phi).Item("Status")
            End If
            If pls <> -1 Then
                For j As Integer = pls To ple
                    pallet.Label6.Text = pldt.Rows(j).Item("QUANTITY").ToString + "x " + pldt.Rows(j).Item("CODE").ToString + " " + pldt.Rows(j).Item("DESCRIPTION").ToString
                    Exit For
                Next
            End If
            If pallet.Label6.Text = "Label6" Then
                pallet.Label6.Text = ""
            End If
            If phdt.Rows(phi).Item("loccode").ToString.Contains(".P") Then
                pallet.department = "BP"
            ElseIf phdt.Rows(phi).Item("loccode").ToString.Contains(".L") Then
                pallet.department = "BL"
            Else
                pallet.department = "SP"
            End If

            FlowLayoutPanel1.Controls.Add(pallet)

            Me.FlowLayoutPanel1.Controls.SetChildIndex(pallet, flpindex)

        End If
        If c IsNot Nothing Then
            c.Dispose()
        End If
        FlowLayoutPanel1.ResumeLayout()
    End Sub

End Class


