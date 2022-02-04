Imports System.Data.SqlClient

Public Class InformationPanelGenericDialog
    Dim dt As DataTable
    Dim s As String
    Dim xloc As Integer
    Dim yloc As Integer
    Dim type As Integer

    Public Sub New(ByVal Message As String, data As DataTable, x As Integer, y As Integer)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        dt = data
        s = Message
        xloc = x
        yloc = y
    End Sub

    Private Sub InformationPanelGenericDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If s = "Υπάρχουν παλέτες σε φάση σχεδιασμού που δεν έχουν αντιστοιχισθεί με daily plan!" Then
            type = 1
            Label1.Text = "Παλέτες που δεν έχουν εισαχθεί σε daily plan:"
        ElseIf s = "Υπάρχουν Daily Plans χωρίς αντιστοιχισμένες παλέτες!" Then
            type = 2
            Label1.Text = "Daily plans χωρίς παλέτες:"
            DoubleBufferedTableLayoutPanel1.RowStyles(3).Height = 0
            Me.Width = 320
        ElseIf s = "Αποτελέσματα αναζήτησης:" Then
            type = 2
            Label1.Text = s
            DoubleBufferedTableLayoutPanel1.RowStyles(3).Height = 0
        ElseIf s = "Διαθέσιμα ΠΑΡ:" Then
            type = 3
            Label1.Text = s
            DoubleBufferedTableLayoutPanel1.RowStyles(3).Height = 0
        End If
        DataGridView1.DataSource = dt
        Me.SetLocation(xloc, yloc)
        AdvancedDataGridViewSearchToolBar1.SetColumns(DataGridView1.Columns)


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
    Private Sub InformationPanelGenericDialog_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        If Not DataGridView1.Columns.Contains("button") And type = 2 Then
            Dim clmn As New DataGridViewButtonColumn
            clmn.Name = "button"
            clmn.HeaderText = "Μετάβαση στο Daily Plan"
            clmn.Text = "Μετάβαση"
            DataGridView1.Columns.Add(clmn)
        End If
        If Not DataGridView1.Columns.Contains("button") And type = 3 Then
            Dim clmn As New DataGridViewButtonColumn
            clmn.Name = "button"
            clmn.HeaderText = "Επιλογή ΠΑΡ"
            clmn.Text = "Επιλογή"
            DataGridView1.Columns.Add(clmn)
        End If
        If Not DataGridView1.Columns.Contains("selectday") And type = 1 Then
            Dim clmn2 As New DataGridViewComboBoxColumn
            clmn2.Name = "selectday"
            Dim list As New Dictionary(Of Integer, String) From {{1, "Δευτέρα"}, {2, "Τρίτη"}, {3, "Τετάρτη"}, {4, "Πέμπτη"}, {5, "Παρασκευή"}, {6, "Σάββατο"}, {7, "Κυριακή"}}
            clmn2.HeaderText = "Ημέρα εβδομάδας"
            clmn2.DataSource = list.ToArray
            clmn2.ValueMember = "Key"
            clmn2.DisplayMember = "Value"
            clmn2.ReadOnly = False
            'clmn2.DefaultCellStyle.NullValue = 1
            DataGridView1.Columns.Add(clmn2)
        End If
        If Not DataGridView1.Columns.Contains("checkbox") And type = 1 Then
            Dim clmn3 As New DataGridViewCheckBoxColumn
            clmn3.Name = "checkbox"
            clmn3.HeaderText = "ΕΠΙΛ"
            clmn3.ReadOnly = False
            'clmn2.DefaultCellStyle.NullValue = 1
            DataGridView1.Columns.Insert(0, clmn3)
        End If
        Dim visiblecolumns As New List(Of String) From {"code", "username", "itecode", "opendate", "button", "selectday", "checkbox", "weeknumber", "year", "weeknumber", "fathername", "tradecode"}
        Dim editablecolumns As New List(Of String) From {"selectday", "checkbox"}
        Dim colwidths As New Dictionary(Of String, Integer) From {{"checkbox", 30}, {"itecode", 100}}
        For Each c As DataGridViewColumn In DataGridView1.Columns
            If Not visiblecolumns.Contains(c.Name.ToString.ToLower) Then
                c.Visible = False
            End If
            If Not editablecolumns.Contains(c.Name.ToString.ToLower) Then
                c.ReadOnly = True
            End If
            If colwidths.ContainsKey(c.Name.ToString.ToLower) Then
                c.Width = colwidths(c.Name.ToString.ToLower)
            End If
        Next
        Dim todayWeekNum As Integer = If(DateTime.Today.DayOfWeek = DayOfWeek.Sunday, 7, CInt(DateTime.Today.DayOfWeek))
        For Each r As DataGridViewRow In DataGridView1.Rows
            If type = 1 Then
                If DataGridView1.Columns.Contains("selectday") Then
                    r.Cells("selectday").Value = todayWeekNum
                End If
            ElseIf type = 2 Then
                r.Cells("button").Value = "Μετάβαση"
            End If
        Next
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
    End Sub


    'improvement needed
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            If e.RowIndex > -1 Then
                If DataGridView1.Columns(e.ColumnIndex).Name = "button" Then
                    If type = 2 Then
                        TryCast(Me.Owner, production).change_date(DataGridView1.Rows(e.RowIndex).Cells("weeknumber").Value, DataGridView1.Rows(e.RowIndex).Cells("year").Value)
                        TryCast(Me.Owner, production).highlight(DataGridView1.Rows(e.RowIndex).Cells("ID").Value)
                    End If
                    If type = 3 Then
                        Dim selpallet As Integer = 0
                        If IsNothing(TryCast(Me.Owner, production)) Then
                            selpallet = TryCast(Me.Owner, PalletDetails).palletid
                        Else
                            selpallet = TryCast(Me.Owner, production).selectedPalletid
                        End If
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, ftr_id:=DataGridView1.Rows(e.RowIndex).Cells("ftrid").Value)
                            pm.StockPalletToOrder(selpallet.ToString, DataGridView1.Rows(e.RowIndex).Cells("ftrid").Value)
                        End Using
                    End If
                ElseIf DataGridView1.Columns(e.ColumnIndex).Name = "checkbox" Then
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim result As Integer = 0
            For Each r As DataGridViewRow In DataGridView1.Rows
                If r.Cells("checkbox").Value = True Then
                    Using transaction = TransactionUtils.CreateTransactionScope()
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=r.Cells("cusid").Value)
                            result += pm.AssignDailyPlan(r.Cells("ID").Value, TryCast(Me.Owner, production).FirstMonday.AddDays(r.Cells("selectday").Value - 1))
                        End Using
                        transaction.Complete()
                    End Using
                End If
            Next
            If result > 0 Then
                TryCast(Me.Owner, production).dp2work()
            End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub


    Private Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar1.Search
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


End Class