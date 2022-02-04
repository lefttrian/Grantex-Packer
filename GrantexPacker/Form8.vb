Imports System.Configuration
Imports System.Data.SqlClient

Public Class Form8
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Dim mydatagrid As New DataGridView()
    Dim selplist
    Dim selpallet As String = ""

    'Form overrides dispose to clean up the component list.
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
            updconn.Dispose()
            conn.Dispose()
            mydatagrid.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Dim optionalstlid As Integer = 0
    'Dim optionalform8type As Integer = 0
    'Public Sub New(Optional ByVal i As Integer = 0, Optional ByVal x As Integer = 0)
    '    ' This call is required by the designer.
    '    InitializeComponent()
    '    optionalstlid = i
    '    optionalform8type = x
    '    ' Add any initialization after the InitializeComponent() call.
    'End Sub


    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cursor.Current = ExtCursor1.Cursor
        mydatagrid.ShowCellToolTips = True
        mydatagrid.AllowUserToAddRows = False
        ComboBox1.Visible = False
        Dim hiddencols As New List(Of String) From {"gold", "green", "lightgreen", "black", "blue", "backorder"}
        Button2.Visible = False
        Dim extracmd As String = ""
        Try
            'If Form1.form8type = 2 Or optionalform8type = 2 Then
            '    TabControl1.Controls.Clear()
            '    Dim selstlid As Integer = 0
            '    If optionalstlid <> 0 Then
            '        selstlid = optionalstlid
            '    Else
            '        selstlid = Form1.DataGridView1.Rows(Form1.DataGridView1.CurrentRow.Index).Cells("stlid").Value
            '    End If
            '    Dim tempdt As New DataTable()
            '    Using tempcmd As New SqlCommand("select m.subcode1,dbo.get_tradecode(ftrid) from storetradelines s inner join material m on m.id=s.iteid where s.id=" + selstlid.ToString, conn)
            '        conn.Open()
            '        Using reader As SqlDataReader = tempcmd.ExecuteReader
            '            tempdt.Load(reader)
            '        End Using
            '        conn.Close()
            '    End Using
            '    Dim cmd As String = "SELECT code  as 'ΚΩΔ ΕΙΔΟΥΣ'    ,[TRADECODE]  AS 'ΠΑΡΑΓΓΕΛΙΑ'    ,[pallet code]   AS 'ΚΩΔ ΠΑΛΕΤΑΣ'   ,[quantity] AS 'ΠΟΣΟΤΗΤΑ',iteid,palletid,DBO.PKRFN_GETPALLETSTATUS(Z1.PALLETID) 'ΚΑΤΑΣΤΑΣΗ ΠΑΛΕΤΑΣ'   FROM [Z_PACKER_FULLITEMQUANTITIES2] where stlid=" + selstlid.ToString
            '    Using itemusageinfo As New SqlCommand(cmd, conn)
            '        itemusageinfo.CommandType = CommandType.Text
            '        Using iudt = New DataTable()
            '            Using paltotnumcmd As New SqlCommand("select distinct [pallet code] from Z_PACKER_FULLITEMQUANTITIES2 where stlid=" + selstlid.ToString, conn)
            '                Using paltotdt = New DataTable()
            '                    conn.Open()
            '                    Using usagereader As SqlDataReader = itemusageinfo.ExecuteReader
            '                        iudt.Load(usagereader)
            '                        Using paltotreader As SqlDataReader = paltotnumcmd.ExecuteReader
            '                            paltotdt.Load(paltotreader)
            '                            conn.Close()
            '                            Label1.Text = "Κατανομή προϊόντος " + tempdt(0)(0)
            '                            Label2.Text = " από " + tempdt(0)(1) + " σε παλέτες."
            '                            Label3.Text = "Σύνολο παλετών: " + paltotdt.Rows.Count.ToString
            '                            Dim myTabPage As New TabPage()
            '                            myTabPage.Text = "Χρήση προϊόντος σε παλέτες"
            '                            TabControl1.TabPages.Add(myTabPage)
            '                            mydatagrid.DataSource = iudt
            '                            mydatagrid.ReadOnly = True
            '                            AddHandler Me.mydatagrid.CellDoubleClick, AddressOf dgvcdc2
            '                            TabControl1.TabPages(0).Controls.Add(mydatagrid)
            '                            With mydatagrid
            '                                .Columns(4).Visible = False
            '                                .Columns(5).Visible = False
            '                            End With
            '                            mydatagrid.Dock = DockStyle.Fill
            '                            Me.Text = "Κατανομή είδους σε παλέτες - Grantex Packer®"
            '                            'End Using
            '                        End Using
            '                    End Using
            '                End Using
            '            End Using
            '        End Using
            '    End Using
            '    Using bocmd As New SqlCommand("select dbo.get_tradecode(f.id) as tradecode,s1.m_boqty as qty from storetradelines s1 left join fintrade f on s1.m_boftrid=f.id where s1.id=" + selstlid.ToString, conn)
            '        conn.Open()
            '        Using bodt = New DataTable()
            '            Using boreader As SqlDataReader = bocmd.ExecuteReader
            '                bodt.Load(boreader)
            '                conn.Close()
            '                If Not IsDBNull(bodt.Rows(0).Item("tradecode")) Then
            '                    Label5.Text = bodt.Rows(0).Item("qty").ToString + " ΤΕΜΑΧΙΑ ΣΤΟ BACKORDER " + bodt.Rows(0).Item("tradecode")
            '                    Label5.Visible = True
            '                End If
            '            End Using
            '        End Using
            '    End Using
            'Else
            If Form1.form8type = 3 Then
                mydatagrid.Columns.Clear()
                Dim selected As ArrayList = New ArrayList()
                For i As Integer = 0 To Form1.DataGridView1.Rows.Count - 1
                    If Form1.DataGridView1.Rows(i).Cells("ΕΠΙΛ").Value = True And Form1.DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value > 0 And Not Form1.DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.Red Then
                        selected.Add(i)
                    End If
                Next
                If selected.Count = 0 Then
                    Throw New System.Exception("Επιλέξτε είδος ή είδος χωρίς μηδενικό υπόλοιπο.")
                End If
                For i As Integer = 0 To selected.Count - 1
                    Dim row As New DataGridViewRow
                    Using row
                        row = Form1.DataGridView1.Rows(selected(i)).Clone()
                        For j As Integer = 0 To Form1.DataGridView1.Columns.Count - 1
                            Dim clmn As New DataGridViewTextBoxColumn
                            Using clmn
                                mydatagrid.Columns.Add(clmn)
                                row.Cells(j).Value = Form1.DataGridView1.Rows(selected(i)).Cells(j).Value
                            End Using
                        Next
                    End Using
                    mydatagrid.Rows.Add(row)
                Next


                For j As Integer = 0 To Form1.DataGridView1.Columns.Count - 1
                    mydatagrid.Columns(j).HeaderText = Form1.DataGridView1.Columns(j).HeaderText
                    mydatagrid.Columns(j).Name = Form1.DataGridView1.Columns(j).Name
                    If Not Form1.DataGridView1.Columns(j).Visible Then
                        mydatagrid.Columns(j).Visible = False
                    End If
                    If Form1.DataGridView1.Columns(j).HeaderText = "ΠΟΣ" Or Form1.DataGridView1.Columns(j).HeaderText = "ΕΠΙΛ" Then
                        mydatagrid.Columns(j).Visible = False
                    End If
                    If Form1.DataGridView1.Columns(j).HeaderText = "ΥΠΟΛ." Then
                        Dim clmn As New DataGridViewTextBoxColumn
                        Using clmn
                            clmn.HeaderText = "Ποσότητα"
                            clmn.Name = "Ποσότητα"
                            clmn.DefaultCellStyle.BackColor = Color.LightGray
                            mydatagrid.Columns.Add(clmn)
                        End Using
                    End If
                    mydatagrid.Columns(j).DefaultCellStyle.BackColor = Color.White
                    mydatagrid.Columns(j).ReadOnly = True
                Next
                mydatagrid.Columns("Ποσότητα").DisplayIndex = 0
                mydatagrid.Columns("Ποσότητα").ReadOnly = False
                For i As Integer = 0 To mydatagrid.Rows.Count - 1
                    mydatagrid.Rows(i).Cells("Ποσότητα").Value = mydatagrid.Rows(i).Cells("ΥΠΟΛ.").Value
                Next
                mydatagrid.Columns("ΥΠΟΛ.").DisplayIndex = 1
                ' mydatagrid.DataSource = mydatagrid
                'mydatagrid.ReadOnly = True
                TabControl1.TabPages(0).Controls.Add(mydatagrid)
                For Each pi As Control In Form1.FlowLayoutPanel1.Controls
                    Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                    Dim small As smallpallet = TryCast(pi, smallpallet)
                    If normal IsNot Nothing Then
                        If normal.closed = False And normal.locked = False Then
                            Dim txt As String = normal.loclbl.Text + " / " + normal.pallettemplatelabel.Text
                            ComboBox1.Items.Add(txt)
                        End If
                    ElseIf small IsNot Nothing Then
                        If small.closed = False And small.locked = False Then
                            Dim txt As String = small.Label2.Text + " / " + small.Label1.Text
                            ComboBox1.Items.Add(txt)
                        End If
                    End If
                Next
                ComboBox1.SelectedIndex = 0
                For i As Integer = 0 To ComboBox1.Items.Count - 1
                    Dim words As String() = ComboBox1.Items(i).ToString.Split(" / ")

                    If words(2) = Form1.Label19.Text Then
                        ComboBox1.SelectedIndex = i
                    End If
                Next
                Label1.Text = "Μαζική εισαγωγή κωδικών σε παλέτα"
                Label3.Text = "Αλλάξτε τις ποσότητες κατά προτίμηση. " + Environment.NewLine() + "Αν μετά την εισαγωγή η γραμμή του είδους γίνει κόκκινη, σημαίνει ότι το είδος δεν μεταφέρθηκε."
                Label2.Text = "Στην ενεργή παλέτα : "
                ComboBox1.Visible = True

                mydatagrid.Dock = DockStyle.Fill
                mydatagrid.AllowUserToAddRows = False
                AddHandler mydatagrid.CellBeginEdit, AddressOf mydatagrid_cellbeginedit
                AddHandler mydatagrid.CellValueChanged, AddressOf mydatagrid_cellchange
                TabControl1.TabPages(0).Text = "Είδη"
                Button2.Visible = True
                Me.Text = "Μαζική εισαγωγή ειδών σε παλέτα - Grantex Packer®"
            ElseIf Form1.form8type = 4 Then
                RadioButton2.Visible = True
                RadioButton1.Visible = True
                Button3.Visible = True
                Button4.Visible = True
                TabControl1.Controls.Clear()
                selplist = Form1.DataGridView2.Rows(Form1.DataGridView2.CurrentRow.Index).Cells("id").Value
                Form1.plistid = selplist
                Dim cmd As String = "select distinct [pallet code],palletid from Z_PACKER_FULLPALLETLINES where plid=" + selplist.ToString
                Button7.Visible = True
                Button5.Visible = True
                Using comm As New SqlCommand(cmd, conn)
                    comm.CommandType = CommandType.Text
                    Using com As New SqlCommand("SELECT TOP 1 RELORDERS FROM Z_PACKER_TRADECODES_PER_PLIST2 WHERE plID=" + selplist.ToString, conn)
                        Using com2 As New SqlCommand("select status from tbl_packinglists where id=" + selplist.ToString, conn)
                            conn.Open()
                            Using dt = New DataTable()
                                Dim relorders As String = com.ExecuteScalar()
                                Using reader As SqlDataReader = comm.ExecuteReader

                                    dt.Load(reader)
                                    Dim status As Integer = com2.ExecuteScalar()
                                    Dim paltotnum As Integer = dt.Rows.Count
                                    Label1.Text = "Packing list " + Form1.DataGridView2.Rows(Form1.DataGridView2.CurrentRow.Index).Cells("ΚΩΔΙΚΟΣ").Value
                                    Label2.Visible = True
                                    Label2.Text = "Σχετικές παραγγελίες: "
                                    Label3.Text = "Σύνολο παλετών: " + paltotnum.ToString
                                    Label2.Text = Label2.Text + relorders
                                    For i As Integer = 0 To dt.Rows.Count - 1
                                        Dim newdgv As DataGridViewTabPage = New DataGridViewTabPage()


                                        newdgv.Text = dt.Rows(i).Item(0)
                                        TabControl1.Controls.Add(newdgv)
                                        Dim cmd2 As String = "select palletid as 'palletid' ,iteid as 'iteid', [tradecode] as 'ΠΑΡΑΓΓ.',code as 'ΚΩΔ ΕΙΔΟΥΣ',description as 'ΠΕΡΙΓΡΑΦΗ',quantity as 'ΠΟΣΟΤΗΤΑ',cuscode as 'ΚΩΔ ΠΕΛ',fathername as 'ΠΕΛ',ftrid from Z_PACKER_FULLPALLETLINES where plid=" + selplist.ToString + " and palletid=" + dt.Rows(i).Item(1).ToString
                                        Using comm2 As New SqlCommand(cmd2, conn)
                                            'comm2.CommandType = CommandType.Text
                                            Using dt2 = New DataTable
                                                Using reader2 As SqlDataReader = comm2.ExecuteReader
                                                    dt2.Load(reader2)


                                                    newdgv.datasource = dt2.Copy()
                                                    dt2.Clear()
                                                End Using
                                            End Using
                                        End Using

                                        newdgv.hide = True
                                        newdgv.hide = True
                                        newdgv.Dock = DockStyle.Fill


                                    Next
                                    conn.Close()
                                    If status = 1 Then
                                        Button4.Image = My.Resources.cancel_completed_small
                                        Button4.Text = "Αναίρεση ολοκλήρωσης"
                                        Button4.Width = 160
                                        Me.BackColor = Color.LightGreen
                                        Button3.Enabled = False
                                    ElseIf status <> 1 Then
                                        Button4.Image = My.Resources.complete
                                        Button4.Text = "Ολοκλήρωση"
                                        Button4.Width = 94
                                        Me.BackColor = SystemColors.Control
                                    End If
                                End Using
                            End Using
                        End Using
                    End Using
                End Using







                Me.Text = "Λεπτομέρειες Packing list - Grantex Packer®"
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


    Private Sub mydatagrid_cellchange(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        Try

            If sender.rows(e.RowIndex).cells("Ποσότητα").value > oldvalue Then
                Dim maxvalue As String = ""
                For i As Integer = 0 To Form1.DataGridView1.Rows.Count - 1
                    If Form1.DataGridView1.Rows(i).Cells("stlid").Value = sender.rows(e.RowIndex).cells("stlid").value Then
                        maxvalue = Form1.DataGridView1.Rows(i).Cells(9).Value
                    End If
                Next
                If sender.rows(e.RowIndex).cells("Ποσότητα").value - maxvalue > 0 Then
                    Throw New System.Exception("Δεν μπορείτε να χρησιμοποιήσετε ποσότητα μεγαλύτερη της διαθέσιμης!")
                End If

            End If
            If sender.rows(e.RowIndex).cells("Ποσότητα").value <= 0 Then
                Throw New System.Exception("Δεν μπορείτε να εισάγετε μηδενική ή αρνητική ποσότητα!")
            End If
            If Not IsNumeric(sender.rows(e.RowIndex).cells("Ποσότητα").value) Then
                Throw New System.Exception("Μόνο αριθμητικές τιμές γίνονται δεκτές!")
            End If

        Catch ex As Exception
            sender.rows(e.RowIndex).cells("Ποσότητα").value = oldvalue
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
    Dim oldvalue As Double

    Private Sub mydatagrid_cellbeginedit(ByVal sender As Object, ByVal e As DataGridViewCellCancelEventArgs)
        oldvalue = sender.rows(e.RowIndex).cells("Ποσότητα").value
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Try
        '    Dim palletid As String = "xxx"
        '    For i As Integer = 0 To mydatagrid.Rows.Count - 1
        '        For Each pi As Control In Form1.FlowLayoutPanel1.Controls
        '            Dim normal As pallettemplate = TryCast(pi, pallettemplate)
        '            Dim small As smallpallet = TryCast(pi, smallpallet)
        '            If normal IsNot Nothing Then
        '                palletid = normal.palletid
        '            ElseIf small IsNot Nothing Then
        '                palletid = small.palletid
        '            End If
        '        Next
        '    Next
        '    For i As Integer = 0 To mydatagrid.Rows.Count - 1
        '        If Not mydatagrid.Rows(i).DefaultCellStyle.BackColor = Color.Red Then
        '            Dim success As Integer = 0
        '            Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid)
        '                success = Form1.pallet_exchange(2, palletid, mydatagrid.Rows(i).Cells("Ποσότητα").Value.ToString, mydatagrid.Rows(i))
        '            If success > 0 Then
        '                mydatagrid.Rows(i).DefaultCellStyle.BackColor = Color.Green
        '                mydatagrid.Rows(i).DefaultCellStyle.BackColor = Color.Green
        '                mydatagrid.Rows(i).Cells(4).ToolTipText = "Επιτυχής καταχώριση"
        '            Else
        '                mydatagrid.Rows(i).DefaultCellStyle.BackColor = Color.Red
        '                mydatagrid.Rows(i).DefaultCellStyle.BackColor = Color.Red
        '                mydatagrid.Rows(i).Cells(4).ToolTipText = "Ανεπιτυχής καταχώριση"
        '            End If


        '        End If
        '    Next
        '    Button2.Visible = False
        '    Form1.DataGridView1.Refresh()
        '    mydatagrid.Refresh()
        '    mydatagrid.Columns("Ποσότητα").ReadOnly = True
        '    Form1.datagridview1_refresh()
        '    Form1.populate_pallets("ftrid")
        'Catch ex As Exception
        '    If updconn.State = ConnectionState.Open Then
        '        updconn.Close()
        '    End If
        '    If conn.State = ConnectionState.Open Then
        '        conn.Close()
        '    End If
        '   Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        'End Try
        'Cursor = Cursors.Default
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim words As String() = ComboBox1.Items(ComboBox1.SelectedIndex).ToString.Split(" / ")

        selpallet = words(2)
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try

            If Not Me.BackColor = Color.LightGreen Then
                Dim selpallet As String = TabControl1.SelectedTab.Text
                Dim result As Integer = MessageBox.Show("Η παλέτα " + selpallet + " θα αφαιρεθεί από το " + Label1.Text + ". Είστε σίγουροι;" _
                                        , "ΠΡΟΣΟΧΗ!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                If result = DialogResult.Yes Then
                    Dim success As Integer = 0
                    Dim cmd As String = "update tbl_palletheaders set plid=null where code like '" + selpallet + "'"
                    Using comm = New SqlCommand(cmd, updconn)
                        updconn.Open()
                        success = comm.ExecuteNonQuery()
                        updconn.Close()
                    End Using
                    If success <= 0 Then
                        Return
                    Else
                        Using ut As New PackerUserTransaction
                            ut.WriteEntry(Form1.activeuserid, 18, selplist, selpallet)
                        End Using
                    End If
                    TabControl1.TabPages.RemoveAt(TabControl1.SelectedIndex)
                    MessageBox.Show("Η παλέτα " + selpallet + " αφαιρέθηκε από το " + Label1.Text + "." _
                                            , "Αφαιρέθηκε παλέτα.", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    If TabControl1.Controls.Count = 0 Then
                        Dim r As Integer = -1
                        Dim cmd2 As String = "update tbl_packinglists set status=0 where code like '" + Label1.Text.Replace("Packing list ", "") + "'"
                        Using comm2 = New SqlCommand(cmd2, updconn)
                            updconn.Open()
                            r = comm2.ExecuteNonQuery()
                            updconn.Close()
                        End Using
                        If r > 0 Then
                            Using ut As New PackerUserTransaction
                                ut.WriteEntry(Form1.activeuserid, 19, selplist)
                            End Using
                        End If
                        Me.Close()
                    End If

                    Form1.datagridview2_refresh()
                    Form1.populate_pallets("ftrid")
                End If
            Else
                Throw New System.Exception("Δεν μπορείτε να επηρεάσετε τις παλέτες ολοκληρωμένου packing list. Ανοίξτε το packing list πρώτα!")
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

    Private Sub RadioButton1_Click(sender As Object, e As EventArgs) Handles RadioButton1.Click
        Try

            If RadioButton1.Checked Then
                RadioButton1.Checked = True
                RadioButton2.Checked = False
                Button3.Visible = True
                Button4.Visible = True
                TabControl1.Controls.Clear()
                selplist = Form1.DataGridView2.Rows(Form1.DataGridView2.CurrentRow.Index).Cells("id").Value
                Dim cmd As String = "select distinct [pallet code],palletid from Z_PACKER_FULLPALLETLINES where plid=" + selplist.ToString
                Using comm As New SqlCommand(cmd, conn)
                    comm.CommandType = CommandType.Text
                    Using com As New SqlCommand("SELECT TOP 1 RELORDERS FROM Z_PACKER_TRADECODES_PER_PLIST2 WHERE PlID=" + selplist.ToString, conn)
                        conn.Open()
                        Using dt = New DataTable()
                            Dim relorders As String = com.ExecuteScalar()
                            Using reader As SqlDataReader = comm.ExecuteReader
                                dt.Load(reader)
                                Dim paltotnum As Integer = dt.Rows.Count

                                Label1.Text = "Packing list " + Form1.DataGridView2.Rows(Form1.DataGridView2.CurrentRow.Index).Cells("ΚΩΔΙΚΟΣ").Value
                                Label2.Visible = True
                                Label2.Text = "Σχετικές παραγγελίες: "
                                Label3.Text = "Σύνολο παλετών: " + paltotnum.ToString
                                Label2.Text = Label2.Text + relorders
                                For i As Integer = 0 To dt.Rows.Count - 1
                                    Dim newdgv As DataGridViewTabPage = New DataGridViewTabPage()


                                    newdgv.Text = dt.Rows(i).Item(0)
                                    TabControl1.Controls.Add(newdgv)
                                    Dim cmd2 As String = "select palletid as 'palletid' ,iteid as 'iteid', [TRADECODE] as 'ΠΑΡΑΓΓ.',code as 'ΚΩΔ ΕΙΔΟΥΣ',description as 'ΠΕΡΙΓΡΑΦΗ',quantity as 'ΠΟΣΟΤΗΤΑ',cuscode as 'ΚΩΔ ΠΕΛ',fathername as 'ΠΕΛ',ftrid from Z_PACKER_FULLPALLETLINES where plid=" + selplist.ToString + " and palletid=" + dt.Rows(i).Item(1).ToString
                                    Using comm2 As New SqlCommand(cmd2, conn)
                                        'comm2.CommandType = CommandType.Text
                                        Using dt2 = New DataTable
                                            Using reader2 As SqlDataReader = comm2.ExecuteReader
                                                dt2.Load(reader2)


                                                newdgv.datasource = dt2.Copy()
                                                dt2.Clear()

                                                newdgv.hide = True
                                                newdgv.hide = True
                                                newdgv.Dock = DockStyle.Fill
                                            End Using
                                        End Using
                                    End Using



                                Next

                                conn.Close()
                            End Using
                        End Using
                    End Using
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

    Private Sub RadioButton2_Click(sender As Object, e As EventArgs) Handles RadioButton2.Click
        Try

            If RadioButton2.Checked Then
                RadioButton2.Checked = True
                RadioButton1.Checked = False
                Button3.Visible = False
                Button4.Visible = True
                TabControl1.Controls.Clear()
                selplist = Form1.DataGridView2.Rows(Form1.DataGridView2.CurrentRow.Index).Cells("id").Value
                Dim cmd As String = "select distinct replace(tradecode,'00ΠΑΞ',''),ftrid from Z_PACKER_FULLPALLETLINES where plid=" + selplist.ToString
                Using comm As New SqlCommand(cmd, conn)
                    comm.CommandType = CommandType.Text
                    conn.Open()
                    Using dt = New DataTable()
                        Using reader As SqlDataReader = comm.ExecuteReader
                            dt.Load(reader)
                            Using paltotnum As New SqlCommand("select distinct palletid from Z_PACKER_FULLPALLETLINES where plid=" + selplist.ToString, conn)
                                Using paltotdt = New DataTable()
                                    Using paltotreader As SqlDataReader = paltotnum.ExecuteReader
                                        paltotdt.Load(paltotreader)
                                        Label1.Text = "Packing list " + Form1.DataGridView2.Rows(Form1.DataGridView2.CurrentRow.Index).Cells("ΚΩΔΙΚΟΣ").Value
                                        Label2.Visible = True
                                        Label2.Text = "Σχετικές παραγγελίες: "
                                        Label3.Text = "Σύνολο παλετών: " + paltotdt.Rows.Count.ToString

                                        For i As Integer = 0 To dt.Rows.Count - 1
                                            Label2.Text = Label2.Text + dt.Rows(i).Item(0) + " "
                                            Dim newdgv As DataGridViewTabPage = New DataGridViewTabPage()


                                            newdgv.Text = dt.Rows(i).Item(0)
                                            TabControl1.Controls.Add(newdgv)
                                            Dim cmd2 As String = "select palletid as 'palletid' ,iteid as 'iteid', [pallet code] as 'ΚΩΔ.ΠΑΛ.',code as 'ΚΩΔ ΕΙΔΟΥΣ',description as 'ΠΕΡΙΓΡΑΦΗ',quantity as 'ΠΟΣΟΤΗΤΑ',cuscode as 'ΚΩΔ ΠΕΛ',fathername as 'ΠΕΛ',ftrid from Z_PACKER_FULLPALLETLINES where plid=" + selplist.ToString + " and ftrid=" + dt.Rows(i).Item(1).ToString
                                            Using comm2 As New SqlCommand(cmd2, conn)
                                                'comm2.CommandType = CommandType.Text
                                                Using dt2 = New DataTable
                                                    Using reader2 As SqlDataReader = comm2.ExecuteReader
                                                        dt2.Load(reader2)


                                                        newdgv.datasource = dt2.Copy()
                                                        dt2.Clear()

                                                        newdgv.hide = True
                                                        newdgv.hide = True
                                                        newdgv.Dock = DockStyle.Fill
                                                    End Using
                                                End Using

                                            End Using
                                        Next
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using
                End Using

                conn.Close()
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
        Try
            Dim success As Integer = 0
            If Button4.Text = "Ολοκλήρωση" Then

                Dim result = MessageBox.Show("Αυτό θα σημάνει το packing list ως ολοκληρωμένο, διαθέσιμο για εισαγωγή στο Ατλαντίς και δε θα μπορεί κάποιος να επηρεάσει τα περιεχόμενα του. Συνέχεια;", "Ολοκλήρωση Packing List", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                If result = DialogResult.OK Then
                    Cursor.Current = ExtCursor1.Cursor
                    Dim rfcmd As String = "SELECT [ftrid],tradecode  From [Z_PACKER_TRADECODES_PER_PLIST] where plid=" + selplist.ToString
                    Using rfcomm As New SqlCommand(rfcmd, conn)
                        Using rfdt = New DataTable()
                            conn.Open()
                            Using rfreader As SqlDataReader = rfcomm.ExecuteReader
                                rfdt.Load(rfreader)
                                conn.Close()
                                Dim ftrids As String = ""
                                Dim tradecodes As String = ""
                                For i As Integer = 0 To rfdt.Rows.Count - 1
                                    If i = 0 Then
                                        ftrids = ftrids + rfdt.Rows(i).Item("ftrid").ToString
                                        tradecodes = tradecodes + rfdt.Rows(i).Item("tradecode").ToString
                                    Else
                                        ftrids = ftrids + "," + rfdt.Rows(i).Item("ftrid").ToString
                                        tradecodes = tradecodes + "," + rfdt.Rows(i).Item("tradecode").ToString
                                    End If
                                Next
                                Dim cmd As String = "Select mtr.code  ,[diff] from Z_PACKER_PENDING_ITEMS_PER_ORDER z left join material mtr on mtr.id=z.iteid where ftrid In (" + ftrids + ") and diff<>0"
                                Using comm As New SqlCommand(cmd, conn)
                                    Using dt = New DataTable()
                                        conn.Open()
                                        Using reader As SqlDataReader = comm.ExecuteReader
                                            dt.Load(reader)
                                            conn.Close()
                                            Dim issues As String = ""
                                            For i As Integer = 0 To dt.Rows.Count - 1
                                                If i = dt.Rows.Count - 1 Then
                                                    issues = issues + dt.Rows(i).Item("code")
                                                Else
                                                    issues = issues + dt.Rows(i).Item("code") + Environment.NewLine
                                                End If

                                            Next
                                            If Len(issues) <> 0 Then
                                                Dim result1 = MessageBox.Show("Οι παραγγελίες " + tradecodes + " έχουν τα παρακάτω προϊόντα σε εκκρεμότητα:" + Environment.NewLine + issues + Environment.NewLine + "Σίγουρα θέλετε να κλείσετε το packing list;", "Είδη σε εκκρεμότητα", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                                                If result1 = DialogResult.Cancel Then
                                                    Return
                                                End If
                                            End If

                                            Dim cmd2 As String = "update tbl_packinglists set status=1, closedate='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where id=" + selplist.ToString
                                            Using com As New SqlCommand(cmd2, updconn)
                                                updconn.Open()
                                                success = com.ExecuteNonQuery()
                                                updconn.Close()
                                                If success <= 0 Then
                                                    Label4.ForeColor = Color.Red
                                                    Label4.Text = "Κάτι δεν πήγε καλά..."
                                                    Label4.Visible = True
                                                    Return
                                                Else
                                                    Using ut As New PackerUserTransaction
                                                        ut.WriteEntry(Form1.activeuserid, 20, selplist)
                                                    End Using
                                                End If
                                                Using sqlcom As New SqlCommand("order_status_update")
                                                    sqlcom.CommandType = CommandType.StoredProcedure
                                                    sqlcom.Parameters.Add("@ID", SqlDbType.Int).Value = CInt(selplist)
                                                    sqlcom.Parameters.Add("@phase", SqlDbType.Int).Value = 1
                                                    sqlcom.Connection = updconn
                                                    updconn.Open()
                                                    sqlcom.ExecuteNonQuery()
                                                    updconn.Close()

                                                End Using
                                                Me.BackColor = Color.LightGreen
                                                Button4.Image = My.Resources.cancel_completed_small
                                                Button4.Text = "Αναίρεση ολοκλήρωσης"
                                                Button4.Width = 160
                                                Button3.Enabled = False
                                                Label4.ForeColor = Color.Green
                                                Label4.Text = "Επιτυχία!"
                                                Label4.Visible = True
                                                Form1.datagridview2_refresh()
                                                Cursor.Current = Cursors.Default
                                            End Using
                                        End Using
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using

                End If
            Else
                Cursor.Current = ExtCursor1.Cursor
                Dim check As String = "SELECT ISNULL(ftr.tradecode,0) FROM STORETRADE  s left join fintrade ftr on s.ftrid=ftr.id WHERE s.sc_plid=" + selplist.ToString
                Using checkcom As New SqlCommand(check, conn)
                    conn.Open()
                    Dim checkresult As String = checkcom.ExecuteScalar()
                    conn.Close()
                    Cursor.Current = Cursors.Default
                    If checkresult = Nothing Then
                        Dim result = MessageBox.Show("Αυτό θα επαναφέρει το packing list ως μη ολοκληρωμένο. Δεν θα είναι διαθέσιμο για εισαγωγή στο Ατλαντίς και θα μπορούν να επηρεάσουν τα περιεχόμενα του. Συνέχεια;", "Άνοιγμα Packing List", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                        If result = DialogResult.OK Then
                            Cursor.Current = ExtCursor1.Cursor
                            Dim cmd As String = "update tbl_packinglists set status=2, closedate=null where id=" + selplist.ToString
                            Using com As New SqlCommand(cmd, updconn)
                                updconn.Open()
                                success = com.ExecuteNonQuery()
                                updconn.Close()
                                If success <= 0 Then
                                    Label4.ForeColor = Color.Red
                                    Label4.Text = "Κάτι δεν πήγε καλά..."
                                    Label4.Visible = True
                                    Return
                                Else
                                    Using ut As New PackerUserTransaction
                                        ut.WriteEntry(Form1.activeuserid, 19, selplist)
                                    End Using
                                End If
                                Using sqlcom As New SqlCommand("order_status_update")
                                    sqlcom.CommandType = CommandType.StoredProcedure
                                    sqlcom.Parameters.Add("@ID", SqlDbType.Int).Value = CInt(selplist)
                                    sqlcom.Parameters.Add("@phase", SqlDbType.Int).Value = 3
                                    sqlcom.Connection = updconn
                                    updconn.Open()
                                    sqlcom.ExecuteNonQuery()
                                    updconn.Close()
                                End Using
                                Me.BackColor = SystemColors.Control
                                Button4.Image = My.Resources.complete
                                Button4.Text = "Ολοκλήρωση"
                                Button4.Width = 94
                                Button3.Enabled = True
                                Label4.ForeColor = Color.Green
                                Label4.Text = "Επιτυχία!"
                                Label4.Visible = True
                                Form1.datagridview2_refresh()
                                Cursor.Current = Cursors.Default
                            End Using
                        End If
                    Else
                        Throw New System.Exception("Το " + Label1.Text + " έχει εισαχθεί στο Atlantis στο παραστατικό " + checkresult + ". Δεν μπορείτε να το ανοίξετε. Επικοινωνήστε με τον διαχειριστή.")
                    End If
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

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            Dim nm As String = ""
            Using c As New SqlCommand("select top 1 name from customer c inner join tbl_palletheaders p on p.cusid=c.id where p.plid=" + Form1.plistid.ToString, conn)
                conn.Open()
                nm = c.ExecuteScalar()
                conn.Close()
            End Using

        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
    Public Sub dgvcdc2(ByVal sender As Object, e As DataGridViewCellEventArgs)

        If e.ColumnIndex = 2 Then
            Using frm As New PalletDetails(sender.rows(e.RowIndex).cells(5).value)
                frm.ShowDialog()
            End Using
        ElseIf e.ColumnIndex = 0 Then
            Using frm As New ItemDetails(sender.rows(e.RowIndex).cells(4).value)
                frm.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            Using F As New PrintPackingListWarehouse
                F.ShowDialog()
            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Form8_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub
End Class

