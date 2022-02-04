Imports System.Data.Objects
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Deployment.Application
Imports System.ComponentModel

Public Class PalletDetails


    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Public palletid As String
    Dim palletcode As String
    Public Sub New(ByVal i As String)
        palletid = i
        InitializeComponent()
    End Sub

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
                Dim ctrl As Control = Me.Controls(i)
                ctrl.Dispose()
            Next
            My.Resources.lock.Dispose()
            My.Resources.completepallet.Dispose()
            My.Resources.sfragida.Dispose()
            My.Resources.pallet_report.Dispose()
            My.Resources.labels_small1.Dispose()
            My.Resources.undo.Dispose()
            Me.PictureBox1.Dispose()
            Me.PictureBox2.Dispose()
            Me.PictureBox3.Dispose()
            conn.Dispose()
            updconn.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Dim lp As Boolean = False
    Dim status As Integer
    Dim salesmanaid As Integer
    Dim createdptid As Integer
    Private Sub Form15_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ComboBox1.DataSource = Form1.pallet_types
            ComboBox1.DisplayMember = "name"
            ComboBox1.ValueMember = "id"
            Cursor.Current = ExtCursor1.Cursor
            Dim cmd As String = ""
            Dim cmd2 As String = ""
            cmd = "SELECT ph.id,ph.[CODE],ph.[OPENDATE],ph.[CLOSEDATE], isnull(isstock,0)  isstock ,ph.[REMARKS],ph.[CREATEUSER],ph.[LUPDATEUSER],ph.[WEIGHT],ph.[NETWEIGHT],ph.[LOCKEDBYID],ph.plid,ph.[CLOSEDBYID],pl.code as pcode,ph.[dptcode],pud.[department],ph.[length],ph.[width],ph.[height],pud.username,pud2.username as un2, pud3.username as un3,ph.remarks,ph.pallettypeid,ph.status,atlantissalesmanid aid,createdptid  FROM [TBL_PALLETHEADERS] ph left join tbl_packeruserdata pud on pud.id=ph.createuser left join tbl_packeruserdata pud2 on pud2.id=ph.closedbyid left join tbl_packeruserdata pud3 on pud3.id=ph.lockedbyid left join tbl_packinglists pl on ph.plid=pl.id where ph.id=" + palletid + " ORDER BY ph.CODE"
            cmd2 = "SELECT [PALLETID]      ,[pallet code]  ,[ITEID]      ,[CODE]      ,[DESCRIPTION]      ,[QUANTITY]      ,[STLID]      ,[tradecode]      ,[CUSCODE]      ,[FATHERNAME],[ftrid],[BATCHNUMBER],FROMMANTIS From [Z_PACKER_FULLPALLETLINES] WHERE palletid=" + palletid
            Using cmdcomm As New SqlCommand(cmd, conn)
                Using cmd2comm As New SqlCommand(cmd2, conn)
                    Using phdt = New DataTable()
                        Using pldt = New DataTable()
                            conn.Open()
                            Using phreader As SqlDataReader = cmdcomm.ExecuteReader()
                                phdt.Load(phreader)
                                Using plreader As SqlDataReader = cmd2comm.ExecuteReader()

                                    pldt.Load(plreader)
                                    conn.Close()
                                    For i As Integer = 0 To 9
                                        DataGridView1.Columns.Add(New DataGridViewTextBoxColumn)
                                    Next
                                    'DataGridView1.AutoGenerateColumns = True
                                    For i As Integer = 0 To pldt.Rows.Count - 1
                                        DataGridView1.Rows.Add()
                                        DataGridView1.Rows(i).Cells(0).Value = pldt.Rows(i).Item("iteid").ToString
                                        DataGridView1.Rows(i).Cells(1).Value = pldt.Rows(i).Item("quantity").ToString
                                        DataGridView1.Rows(i).Cells(2).Value = pldt.Rows(i).Item("code").ToString
                                        DataGridView1.Rows(i).Cells(3).Value = pldt.Rows(i).Item("description").ToString
                                        DataGridView1.Rows(i).Cells(4).Value = pldt.Rows(i).Item("tradecode").ToString
                                        DataGridView1.Rows(i).Cells(5).Value = pldt.Rows(i).Item("CUSCODE").ToString
                                        DataGridView1.Rows(i).Cells(6).Value = pldt.Rows(i).Item("FATHERNAME").ToString
                                        DataGridView1.Rows(i).Cells(7).Value = pldt.Rows(i).Item("stlid").ToString
                                        DataGridView1.Rows(i).Cells(8).Value = pldt.Rows(i).Item("ftrid").ToString
                                        DataGridView1.Rows(i).Cells(9).Value = pldt.Rows(i).Item("BATCHNUMBER").ToString
                                        If Not IsDBNull(pldt.Rows(i).Item("frommantis")) AndAlso pldt.Rows(i).Item("frommantis") = 1 Then
                                            DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
                                        End If
                                    Next
                                    With DataGridView1
                                        .Columns(0).Visible = False
                                        '.Columns(1).Visible = False
                                        .Columns(7).Visible = False
                                        .Columns(8).Visible = False
                                        .Columns(1).HeaderText = "Ποσότητα"
                                        .Columns(2).HeaderText = "Κωδικός"
                                        .Columns(3).HeaderText = "Περιγραφή"
                                        .Columns(4).HeaderText = "ΠΑΡ"
                                        .Columns(9).HeaderText = "BatchNumbers"
                                        .AllowUserToAddRows = False
                                        For i As Integer = 0 To 9
                                            .Columns(i).ReadOnly = True
                                            If i = 9 And (Form1.activeuserdpt = "SA" Or Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "SP") Then
                                                .Columns(9).ReadOnly = False
                                            End If
                                        Next
                                    End With
                                    palletcode = phdt.Rows(0).Item("CODE").ToString
                                    Label1.Text = "Παλέτα " + palletcode + " " + phdt.Rows(0).Item("dptcode").ToString
                                    If Not Len(phdt.Rows(0).Item("pcode").ToString) = 0 Then
                                        Label5.Visible = True
                                        Label5.Text = "Στο Packing list " + phdt.Rows(0).Item("pcode").ToString
                                        Label5.ForeColor = Color.Green
                                    Else
                                        PictureBox1.Image = My.Resources.icons8_error_30

                                        Label5.Text = "Δεν έχει καταχωρηθεί σε Packing list!"
                                        Label5.ForeColor = Color.Red
                                    End If
                                    If phdt.Rows(0).Item("isstock") = 0 Then
                                        Button6.Text = "Μεταφορά στο απόθεμα"
                                    Else
                                        Button6.Text = "Μεταφορά σε παραγγελία"
                                    End If
                                    'If Not Len(phdt.Rows(0).Item("closedate").ToString) = 0 Then
                                    '    Label4.Text = "Έκλεισε από: " + phdt.Rows(0).Item("un2").ToString + " στις " + phdt.Rows(0).Item("closedate").ToString
                                    '    Label3.Text = "Διαστάσεις:L" + phdt.Rows(0).Item("length").ToString + "xW" + phdt.Rows(0).Item("width").ToString + "xH" + phdt.Rows(0).Item("height").ToString + ", Βάρη: " + phdt.Rows(0).Item("weight").ToString + "kg, Net:" + phdt.Rows(0).Item("netweight").ToString + "Kg"
                                    'Else
                                    '    PictureBox3.Image = My.Resources.issues
                                    '    Label3.Text = "Διαστάσεις:L" + phdt.Rows(0).Item("length").ToString + "xW" + phdt.Rows(0).Item("width").ToString + "xH" + phdt.Rows(0).Item("height").ToString
                                    'End If
                                    ComboBox1.SelectedValue = phdt.Rows(0).Item("pallettypeid")
                                    status = phdt.Rows(0).Item("status")
                                    salesmanaid = phdt.Rows(0).Item("aid")
                                    createdptid = phdt.Rows(0).Item("createdptid")
                                    If Not Len(phdt.Rows(0).Item("closedate").ToString) = 0 Then
                                        lp = True
                                        PictureBox3.Image = My.Resources.icons8_approval_30
                                        Label7.Text = "Ολοκληρωμένη!"
                                        Label7.ForeColor = Color.Green
                                        Label4.Text = "Έκλεισε από: " + phdt.Rows(0).Item("un2").ToString + " στις " + phdt.Rows(0).Item("closedate").ToString
                                        'Label3.Text = "Διαστάσεις:L" + phdt.Rows(0).Item("length").ToString + "xW" + phdt.Rows(0).Item("width").ToString + "xH" + phdt.Rows(0).Item("height").ToString + ", Βάρη: " + phdt.Rows(0).Item("weight").ToString + "kg, Net:" + phdt.Rows(0).Item("netweight").ToString + "Kg"
                                    Else
                                        PictureBox3.Image = My.Resources.icons8_error_30
                                        Label4.Visible = False
                                        Label7.Text = "Μη ολοκληρωμένη!"
                                        Label7.ForeColor = Color.Red

                                        'Label3.Text = "Διαστάσεις:L" + phdt.Rows(0).Item("length").ToString + "xW" + phdt.Rows(0).Item("width").ToString + "xH" + phdt.Rows(0).Item("height").ToString
                                    End If
                                    If Not Len(phdt.Rows(0).Item("remarks").ToString) = 0 Then
                                        TextBox1.Text = phdt.Rows(0).Item("remarks")
                                    End If
                                    If Not Len(phdt.Rows(0).Item("un3").ToString) = 0 Then
                                        PictureBox2.Visible = True
                                        Label6.Visible = True
                                        Label6.Text = "Κλειδωμένη από " + phdt.Rows(0).Item("un3").ToString
                                        Label6.ForeColor = Color.Red
                                    Else
                                        PictureBox2.Visible = False
                                        Label6.Visible = False
                                    End If
                                    Label2.Text = "Δημιουργήθηκε από: " + phdt.Rows(0).Item("username").ToString + " στις " + phdt.Rows(0).Item("opendate").ToString
                                    Dim sumbp As Double = 0
                                    Dim sumbl As Double = 0
                                    Dim sumsp As Double = 0
                                    Try

                                        With Me.DataGridView1

                                            For i As Integer = 0 To .Rows.Count - 1
                                                If .Rows(i).Cells(2).Value.ToString.Substring(0, 3) = "102" Or .Rows(i).Cells(2).Value.ToString.Substring(0, 3) = "202" Then
                                                    sumbp = sumbp + .Rows(i).Cells(1).Value
                                                End If
                                                If .Rows(i).Cells(2).Value.ToString.Substring(0, 1) = "1" And Not .Rows(i).Cells(2).Value.ToString.Substring(0, 3) = "102" Then
                                                    sumbl = sumbl + .Rows(i).Cells(1).Value
                                                End If
                                                If .Rows(i).Cells(2).Value.ToString.Substring(0, 1) = "2" And Not .Rows(i).Cells(2).Value.ToString.Substring(0, 3) = "202" Then
                                                    sumsp = sumsp + .Rows(i).Cells(1).Value
                                                End If
                                            Next
                                        End With
                                    Catch ex As Exception
                                        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
                                    Finally
                                        Label8.Text = "Άθροισμα sets Σ/Φ/Δ/Α: " + (sumbl + sumbp + sumsp).ToString + "/" + sumbl.ToString + "/" + sumbp.ToString + "/" + sumsp.ToString
                                    End Try
                                End Using
                            End Using
                        End Using
                    End Using
                End Using
            End Using
            If Form1.activeuser = "SUPERVISOR" Or Form1.activeuserdpt = "SP" Or Form1.activeuserdpt = "EX" Then
                Button4.Visible = True
            End If
            If Label5.ForeColor = Color.Red And Label4.Visible Then
                If (Form1.activeuserdpt = "SA" Or Form1.activeuseraid = salesmanaid Or Form1.activeuserdptid = createdptid) Then
                    Button6.Enabled = True
                End If
            End If
            LockUIAccess(Me)
            Cursor.Current = Cursors.Default
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then

            Using frm As New ItemDetails(DataGridView1.Rows(e.RowIndex).Cells(0).Value)
                frm.ShowDialog()
            End Using
        End If
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            'DataGridView1_CellContentDoubleClick(sender, clickedorder, True)
            Form1.palletsforlabels = palletid
            Using frm As New PrintPalletReport
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub


    Private Sub Button2_MouseClick(sender As Object, e As MouseEventArgs) Handles Button2.MouseClick
        ContextMenuStrip1.Show(CType(sender, Control), e.Location)
    End Sub

    Private Sub ΑνοίγματοςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑνοίγματοςToolStripMenuItem.Click
        Try
            'DataGridView1_CellContentDoubleClick(sender, clickedorder, True)
            Form1.palletsforlabels = palletid
            Using frm As New PrintOpenLabels
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub ΚλεισίματοςToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΚλεισίματοςToolStripMenuItem.Click

        Try
            'If Not lp Then
            '    Throw New System.Exception("Δεν επιτρέπεται εκτύπωση ετικέτας κλεισίματος παλέτας που δεν έχει κλείσει.")
            'End If
            'DataGridView1_CellContentDoubleClick(sender, clickedorder, True)
            Form1.palletsforlabels = palletid
            Using frm As New PrintCloseLabels
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        Try
            If e.ColumnIndex = 9 Then
                Dim value As String

                Using comm As New SqlCommand("update tbl_palletlines set batchnumber=@value where stlid=" + DataGridView1.Rows(e.RowIndex).Cells(7).Value.ToString + " and palletid=" + palletid + " and isnull(frommantis,0)=0", updconn)
                    If DataGridView1.Rows(e.RowIndex).Cells(9).Value Is Nothing Then
                        comm.Parameters.Add("@value", SqlDbType.VarChar, 500).Value = DBNull.Value
                    Else
                        comm.Parameters.Add("@value", SqlDbType.VarChar, 500).Value = DataGridView1.Rows(e.RowIndex).Cells(9).Value.ToString
                    End If

                    updconn.Open()
                    Dim success = comm.ExecuteNonQuery
                    If success <= 0 Then
                        DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Red
                    Else
                        DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Green
                    End If
                    updconn.Close()

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

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim frm As New packingdetails(CInt(palletid), palletcode)
        Using frm
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            Dim msg = "Εισάγετε σχόλια:"
            Dim result2 As String = InputBox(msg,
                                                      "Προσθήκη σχολίων", " "
                                                      )
            If Not String.IsNullOrWhiteSpace(result2) Then
                Dim txt As String = result2 + ", " + Form1.activeuser + ", " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine
                Using cmd As New SqlCommand("update tbl_palletheaders set remarks=@remarks+cast(isnull(remarks,'') as varchar(7000)) where id=" + palletid, updconn)
                    cmd.Parameters.Add("@remarks", SqlDbType.VarChar, 8000).Value = txt
                    updconn.Open()

                    If cmd.ExecuteNonQuery() > 0 Then
                        Label17.Text = "Επιτυχής αποθήκευση!"
                        Label17.ForeColor = Color.Green
                        Label17.Visible = True
                        TextBox1.Text = txt + TextBox1.Text
                        For Each pi As Control In Form1.FlowLayoutPanel1.Controls
                            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                            Dim small As smallpallet = TryCast(pi, smallpallet)
                            If normal IsNot Nothing Then
                                If normal.palletid = palletid Then
                                    normal.TextBox1.Text = txt + TextBox1.Text
                                    For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                                        If Form1.phdt.Rows(i).Item("id").ToString = palletid Then
                                            Form1.phdt.Rows(i).Item("REMARKS") = txt + Form1.phdt.Rows(i).Item("REMARKS")
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If
                            ElseIf small IsNot Nothing Then
                                For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                                    If Form1.phdt.Rows(i).Item("id").ToString = palletid Then
                                        Form1.phdt.Rows(i).Item("REMARKS") = txt + Form1.phdt.Rows(i).Item("REMARKS")
                                        Exit For
                                    End If
                                Next
                            End If
                        Next
                    Else
                        Label17.Text = "Κάτι δεν πήγε καλά!"
                        Label17.ForeColor = Color.Red
                        Label17.Visible = True
                    End If
                    updconn.Close()
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

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            If Button6.Text = "Μεταφορά σε παραγγελία" Then
                Dim dt As New DataTable()
                Using s As New SqlCommand("select distinct f.id as ftrid,c.FATHERNAME,dbo.get_tradecode(f.id) TRADECODE from fintrade f inner join tbl_packerordercheck t on t.ftrid=f.id inner join customer c on c.id=f.cusid inner join storetradelines s on s.ftrid=f.id
                where t.status<12 and s.iteid in (select iteid from tbl_palletlines where palletid=" + Me.palletid.ToString + ")", conn)
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                        conn.Close()
                    End Using
                End Using
                Dim f As New InformationPanelGenericDialog("Διαθέσιμα ΠΑΡ:", dt, MousePosition.X, MousePosition.Y)
                f.Owner = Me
                f.Show()
            Else
                Dim ret = MsgBox("Η παλέτα θα μεταφερθεί στο απόθεμα. Είστε σίγουροι;", MessageBoxButtons.OKCancel, "Είστε σίγουροι;")
                If ret = MsgBoxResult.Ok Then
                    Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, stock:=True)
                        pm.OrderPalletToStock(Me.palletid)
                    End Using
                End If
            End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 1 Then
            DataGridView2.Visible = False
            Dim pic As New PictureBox
            pic.SizeMode = PictureBoxSizeMode.CenterImage
            pic.Image = My.Resources.rolling
            pic.Dock = DockStyle.Fill
            TabPage2.Controls.Add(pic)
            dpworker.RunWorkerAsync()
        End If
    End Sub

    Private Sub dpworker_DoWork(sender As Object, e As DoWorkEventArgs) Handles dpworker.DoWork
        Using s As New SqlCommand("select pl.iteid,pl.stlid,pl.ftrid,pl.dailyplanid,m.code,dp.date,sum(dp.quantity),'ΣΥΣΚΕΥΑΣΙΑ' from tbl_palletheaders ph inner join tbl_palletlines pl on pl.palletid=ph.id inner join material m on m.id=pl.iteid inner join pkrtbl_dailyplan dp on dp.id=pl.dailyplanid where ph.id=" + palletid.ToString + " group by pl.iteid,pl.stlid,pl.ftrid,pl.dailyplanid,m.code,dp.date", conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                    e.Result = dt
                End Using
            End Using
        End Using
    End Sub

    Private Sub dpworker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles dpworker.RunWorkerCompleted
        Try
            DataGridView2.DataSource = e.Result
            For Each c As DataGridViewColumn In DataGridView2.Columns
                With c
                    If {"iteid", "stlid", "ftrid", "dailyplanid"}.Contains(.Name) Then
                        .Visible = False
                    End If
                End With
            Next
            DataGridView2.ReadOnly = True
            DataGridView2.Visible = True
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            For Each c As Control In TabPage2.Controls
                If Not IsNothing(TryCast(c, PictureBox)) Then
                    c.Dispose()
                End If
            Next
        End Try
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Using f As New PalletTypes
            f.ShowDialog()
        End Using
    End Sub

End Class