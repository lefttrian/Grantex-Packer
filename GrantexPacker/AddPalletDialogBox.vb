Public Class AddPalletDialogBox

    Dim yloc As Integer
    Dim xloc As Integer
    Dim id As Integer
    Dim type As Integer
    Dim onlyone As Boolean = False
    Dim quan_limit As Double = Nothing
    Dim remainder As Double = 0
    Dim calcpalletnum As Double = 0
    Public combinePallet As Integer = 0
    Public MainPalletFull As Boolean = True
    Dim pallettype As String = ""

    Public Sub New(ByVal pertypeid As Integer, typeid As Integer, x As Integer, y As Integer, Optional oo As Boolean = False, Optional limit As Double = Nothing, Optional pallet_type As String = "")
        ' This call is required by the designer.
        InitializeComponent()
        id = pertypeid
        type = typeid
        xloc = x
        yloc = y
        onlyone = oo
        quan_limit = limit
        pallettype = pallet_type
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        ' Add any initialization after the InitializeComponent() call.
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

    Private Sub AddPalletDialogBox_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        'Me.Close()
        'Me.Dispose()
    End Sub

    Private Sub AddPalletDialogBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.SetDesktopLocation(xloc, yloc)
        Timer1.Start()
        If onlyone Then
            TextBox1.Text = "1"
            TextBox1.Enabled = False
            TabControl1.TabPages(0).Visible = False
        End If
        Label11.Text = ""
        Label15.Text = ""
        Label21.Text = ""
        Label4.Text = quan_limit.ToString
        Label13.Text = quan_limit.ToString
        TextBox4.Text = quan_limit.ToString
        Label3.Text = pallettype
        Me.Activate()
    End Sub

    Private Sub DoubleBufferedTableLayoutPanel1_Click(sender As Object, e As EventArgs) Handles DoubleBufferedTableLayoutPanel1.Click, Label1.Click, Label2.Click, Label3.Click, Label4.Click, Label5.Click,
            Label6.Click, Label7.Click, Label8.Click, Label9.Click, Panel1.Click, Panel2.Click, Panel3.Click, Panel4.Click, DoubleBufferedTableLayoutPanel2.Click, TabControl1.Click
        timercounter = 0
    End Sub

    Private Sub TextBox1_Enter(sender As Object, e As EventArgs) Handles TextBox1.Enter
        If TextBox1.Text = "πλήθος" Then
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox2_Enter(sender As Object, e As EventArgs) Handles TextBox2.Enter, TextBox3.Enter
        If TryCast(sender, TextBox).Text = "ποσότητα" Then
            TryCast(sender, TextBox).Text = ""
        End If
    End Sub

    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles TextBox1.Leave
        If TextBox1.Text = "" Then
            TextBox1.Text = "πλήθος"
        End If
    End Sub

    Private Sub TextBox2_Leave(sender As Object, e As EventArgs) Handles TextBox2.Leave, TextBox3.Leave
        If TryCast(sender, TextBox).Text = "" Then
            TryCast(sender, TextBox).Text = "ποσότητα"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TabControl1.SelectedIndex = 0 And (Not IsNumeric(TextBox1.Text) OrElse CInt(TextBox1.Text) <= 0) Then
            TextBox1.BackColor = Color.Red
            Return
        ElseIf TabControl1.SelectedIndex = 0 And (Not IsNumeric(TextBox2.Text) OrElse CInt(TextBox2.Text) <= 0) Then
            TextBox2.BackColor = Color.Red
            Return
        ElseIf TabControl1.SelectedIndex = 1 And (Not IsNumeric(TextBox3.Text) OrElse CInt(TextBox3.Text) <= 0) Then
            TextBox3.BackColor = Color.Red
            Return
        ElseIf TabControl1.SelectedIndex = 1 And (Not IsNumeric(TextBox4.Text) OrElse CInt(TextBox4.Text) <= 0) Then
            TextBox4.BackColor = Color.Red
            Return
        ElseIf check_sum() > quan_limit Then
            Return
        Else
            If TabControl1.SelectedIndex = 0 Then
                TryCast(Owner, ProductionDailyPlanQuickPalletPlan).numofpallets = CInt(TextBox1.Text)
                TryCast(Owner, ProductionDailyPlanQuickPalletPlan).quanperpallet = CDbl(TextBox2.Text)
                If NoPaddingCheckbox1.Checked Then
                    TryCast(Owner, ProductionDailyPlanQuickPalletPlan).extrapalletquantity = CDbl(TextBox2.Text)
                Else
                    TryCast(Owner, ProductionDailyPlanQuickPalletPlan).extrapalletquantity = 0
                End If
                TryCast(Owner, ProductionDailyPlanQuickPalletPlan).extrapalletid = 0
            ElseIf TabControl1.SelectedIndex = 1 Then
                TryCast(Owner, ProductionDailyPlanQuickPalletPlan).numofpallets = calcpalletnum
                TryCast(Owner, ProductionDailyPlanQuickPalletPlan).quanperpallet = CDbl(TextBox3.Text)
                If remainder > 0 Then
                    TryCast(Owner, ProductionDailyPlanQuickPalletPlan).extrapalletquantity = remainder
                    TryCast(Owner, ProductionDailyPlanQuickPalletPlan).extrapalletid = combinePallet
                End If

            End If
            TryCast(Owner, ProductionDailyPlanQuickPalletPlan).execute()
        End If
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged, TextBox4.TextChanged
        Dim t As TextBox = TryCast(sender, TextBox)
        If t.BackColor = Color.Red AndAlso IsNumeric(t.Text) AndAlso CInt(t.Text) <> 0 AndAlso CDbl(t.Text) <= quan_limit Then
            t.BackColor = SystemColors.Window
        ElseIf t.Text <> "πλήθος" And t.Text <> "ποσότητα" And t.Text <> "" And (t.BackColor = SystemColors.Window AndAlso (Not IsNumeric(t.Text) OrElse CInt(t.Text) = 0)) Then
            t.BackColor = Color.Red
            Exit Sub
        End If
        If t.Name = "TextBox1" And IsNumeric(t.Text) Then
            If CInt(t.Text) = 1 Then
                NoPaddingCheckbox1.Visible = True
            Else
                NoPaddingCheckbox1.Visible = False
            End If
            check_sum()
        ElseIf IsNumeric(t.Text) Then
            check_sum()
        End If
    End Sub


    Private Function check_sum()
        Dim sum As Double = 0

        If TextBox1.Text <> "πλήθος" And TextBox2.Text <> "ποσότητα" And Not String.IsNullOrWhiteSpace(TextBox1.Text) And Not String.IsNullOrWhiteSpace(TextBox2.Text) Then
            sum = CInt(TextBox1.Text) * CDbl(TextBox2.Text)
            Label11.Visible = True
            Label11.Text = sum.ToString
            If sum > quan_limit Then
                Label11.ForeColor = Color.Red

            Else
                Label11.ForeColor = Color.Black
            End If
        ElseIf TextBox4.Text <> "πλήθος" And TextBox3.Text <> "ποσότητα" And Not String.IsNullOrWhiteSpace(TextBox3.Text) And Not String.IsNullOrWhiteSpace(TextBox4.Text) Then
            Dim q_per_pallet As Double = CDbl(TextBox3.Text)
            Dim req_q As Double = CDbl(TextBox4.Text)
            sum = Math.Floor(req_q / q_per_pallet)
            Label15.Text = sum.ToString
            remainder = req_q Mod q_per_pallet
            calcpalletnum = sum
            Label21.Text = remainder
            If remainder > 0 Then
                LinkLabel1.Visible = True
            Else
                LinkLabel1.Visible = False
            End If
            If sum * q_per_pallet > quan_limit Then
                Label15.ForeColor = Color.Red
            Else
                Label15.ForeColor = Color.Black
            End If
            sum = sum * q_per_pallet
        Else
            Label11.Visible = False
        End If
        Return sum
    End Function

    'Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles 
    '    Dim t As TextBox = TextBox4
    '    If t.BackColor = Color.Red AndAlso IsNumeric(t.Text) AndAlso CInt(t.Text) <> 0 AndAlso CDbl(t.Text) <= quan_limit Then
    '        t.BackColor = SystemColors.Window
    '    ElseIf t.Text <> "πλήθος" And t.Text <> "ποσότητα" And t.Text <> "" And (t.BackColor = SystemColors.Window AndAlso (Not IsNumeric(t.Text) OrElse CInt(t.Text) = 0 OrElse CDbl(t.Text) > quan_limit)) Then
    '        t.BackColor = Color.Red
    '        Exit Sub
    '    End If
    '    If IsNumeric(t.Text) Then
    '        check_sum()
    '    End If
    'End Sub

    Private Sub Label21_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim ls As New List(Of Integer)
        If type = 1 Then
            ls.Add(0)
        ElseIf type = 0 Then
            ls.Add(id)
        End If
        Using f As New ProductionCombineSemifinishedPallets(ls, MousePosition.X, MousePosition.Y, mode:="notNormal")
            If f.ShowDialog = DialogResult.Retry Then
                combinePallet = f.combinePallet
                MainPalletFull = f.MainPalletFull
                LinkLabel1.Text = f.combinePalletCode
            End If
        End Using
    End Sub


End Class