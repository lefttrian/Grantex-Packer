Imports System.Configuration
Imports System.Data.SqlClient

Public Class ProductionCombineSemifinishedPallets
    Dim cusid As Integer
    Dim pertypeid As Integer
    Dim palletids As New List(Of Integer)
    Dim yloc As Integer
    Dim xloc As Integer
    Dim form_mode As String
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()

    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Public combinePallet As Integer = 0
    Public combinePalletCode As String = ""
    Public MainPalletFull As Boolean = True

    Public Sub New(pallet_ids As List(Of Integer), x As Integer, y As Integer, Optional mode As String = "normal")
        ' This call is required by the designer.
        InitializeComponent()
        If mode <> "normal" Then
            pertypeid = pallet_ids(0)
        Else
            palletids = pallet_ids
        End If
        xloc = x
        yloc = y
        form_mode = mode
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub ProductionCombineSemifinishedPallets_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            '+ String.Join(",", palletids.ToArray()) + ")"
            If form_mode <> "normal" Then
                Button2.Text = "Εισαγωγή σε επιλεγμένη"
                Button2.Width = TextRenderer.MeasureText(Button2.Text, Button2.Font).Width
                If pertypeid = 0 Then

                Else

                End If
            End If
            SetLocation(xloc, yloc)
            work()
            Timer1.Start()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.Message.Contains("Subquery returned more than 1 value.") Then
                msg = "Η λειτουργία είναι άκυρη λόγω αναφοράς σε παλέτες πολλών πελατών. Επικοινωνήστε με τον διαχειριστή"
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, msg, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
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

    Private Sub work()
        DataGridView1.Hide()
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        TableLayoutPanel1.Controls.Add(pic, 0, 2)
        pic.Dock = DockStyle.Fill
        TableLayoutPanel1.SetColumnSpan(pic, 2)
        mainworker.RunWorkerAsync()
    End Sub

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork
        Dim add_text As String = ""
        If form_mode <> "normal" And pertypeid <> 0 Then
            add_text = " ph.cusid=(select cusid from fintrade where id=(select ftrid from storetradelines where id=" + pertypeid.ToString + "))"
        ElseIf form_mode <> "normal" And pertypeid = 0 Then
            add_text = " ph.code like 'STOCK-%'"
        Else
            add_text = " ph.cusid= (select distinct cusid from tbl_palletheaders where id in (" + String.Join(",", palletids.ToArray()) + "))"
        End If
        Using s As New SqlCommand("select PH.ID palletid,ph.code,ph.orders,ph.cusid,c.fathername as cusname,cast(pl.q as varchar(10))+'x'+m.subcode1+'-ORGNL/WVA:'+m.M_INDEX+'-BRAND:'+MNF.DESCR item from tbl_palletheaders ph left join
(select row_number() over (partition by palletid order by palletid asc) rn ,palletid,max(quantity) q,iteid from tbl_palletlines group by palletid,iteid ) pl
on ph.id=pl.PALLETID and rn=1 left join material m on m.id=pl.ITEID left join customer c on c.id=ph.cusid left join manufacturer mnf on mnf.codeid=m.mnfid where ph.isnotfull=1 and " + add_text, conn)
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

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted
        DataGridView1.DataSource = e.Result
        DataGridView1.Show()
        For Each c As Control In TableLayoutPanel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
        Dim dt = TryCast(e.Result, DataTable)
        Dim HiddenColumns As New List(Of String) From {"palletid", "cusid", "cusname"}
        If form_mode <> "normal" Then
            HiddenColumns.Add("include")
        End If
        Dim ColumnNames As New Dictionary(Of String, String) From {{"code", "ΚΩΔ ΠΑΛ"}, {"item", "1ο ΕΙΔΟΣ"}, {"orders", "ΣΧΕΤ ΠΑΡ"}}
        If Not IsNothing(dt) And dt.Rows.Count > 0 Then
            cusid = dt.Rows(0).Item("cusid")
            Label1.Text = "Ημιτελείς παλέτες του πελάτη " + dt.Rows(0).Item("cusname")
            Dim clmn As New DataGridViewCheckBoxColumn
            clmn.Name = "include"
            clmn.HeaderText = "ΠΕΡΙΕΧ"
            clmn.Width = 35
            DataGridView1.Columns.Insert(0, clmn)

            Dim clmn2 As New DataGridViewCheckBoxColumn
            clmn2.Name = "main"
            clmn2.HeaderText = "ΚΥΡΙΑ"
            clmn2.Width = 30
            DataGridView1.Columns.Insert(0, clmn2)
            For Each c As DataGridViewColumn In DataGridView1.Columns
                If HiddenColumns.Contains(c.Name) Then
                    c.Visible = False
                End If

                If ColumnNames.ContainsKey(c.Name) Then
                    c.HeaderText = ColumnNames(c.Name)
                End If
            Next
            For Each r As DataGridViewRow In DataGridView1.Rows
                If palletids.Contains(r.Cells("palletid").Value) Then
                    r.Cells("include").Value = True
                End If
            Next
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Dim timercounter As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        timercounter += 1
        Label9.Text = (30 - timercounter).ToString
        If timercounter = 30 Then
            Me.Close()
            Me.Dispose()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point
    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseDown, Panel1.MouseDown, Panel4.MouseDown, TableLayoutPanel1.MouseDown ' Add more handles here (Example: PictureBox1.MouseDown)
        If e.Button = MouseButtons.Left Then
            MoveForm = True
            Me.Cursor = Cursors.NoMove2D
            MoveForm_MousePosition = e.Location
        End If
    End Sub

    Public Sub MoveForm_MouseMove(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseMove, Panel1.MouseMove, Panel4.MouseMove, TableLayoutPanel1.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If
    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles _
    TableLayoutPanel1.MouseUp, Panel1.MouseUp, Panel4.MouseUp, TableLayoutPanel1.MouseUp ' Add more handles here (Example: PictureBox1.MouseUp)
        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged

        If e.RowIndex >= 0 AndAlso DataGridView1.Columns(e.ColumnIndex).Name = "main" Then
            If DataGridView1.Rows(e.RowIndex).Cells("main").Value = True Then
                DataGridView1.Rows(e.RowIndex).Cells("include").Value = True
                DataGridView1.Rows(e.RowIndex).Cells("include").ReadOnly = True
            Else
                DataGridView1.Rows(e.RowIndex).Cells("include").Value = False
                DataGridView1.Rows(e.RowIndex).Cells("include").ReadOnly = False
            End If
            For Each r As DataGridViewRow In DataGridView1.Rows
                If r.Index = e.RowIndex Then
                    Continue For
                Else
                    r.Cells("main").Value = False
                End If
            Next
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub DataGridView1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged
        If DataGridView1.IsCurrentCellDirty Then
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim MainPallet As Integer = 0
            Dim pallets As New List(Of Integer)
            For Each r As DataGridViewRow In DataGridView1.Rows
                If r.Cells("main").Value = True Then
                    MainPallet = r.Cells("palletid").Value
                    combinePalletCode = r.Cells("code").Value
                ElseIf r.Cells("main").Value = False AndAlso r.Cells("include").Value = True Then
                    pallets.Add(r.Cells("palletid").Value)
                End If
            Next
            If MainPallet = 0 Then
                Throw New Exception("Δεν έχει επιλεγεί κύρια παλέτα!")
            End If
            Dim result As Integer = 0
            If form_mode <> "normal" Then
                result = 1
                combinePallet = MainPallet
                Me.DialogResult = DialogResult.Retry
                Me.Close()
            Else
                Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=cusid)
                    result = pm.MoveItems(pallets, MainPallet)
                End Using
                If result > 0 Then
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
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

    Private Sub NoPaddingCheckbox1_CheckedChanged(sender As Object, e As EventArgs) Handles NoPaddingCheckbox1.CheckedChanged
        With NoPaddingCheckbox1
            If .Checked Then
                MainPalletFull = False
            Else
                MainPalletFull = True
            End If
        End With

    End Sub
End Class