Public Class OrderApprovalSeal

    Dim user As String = ""
    Dim t As Integer = 0
    Dim _date As String = ""

    Public Sub New(ByVal username As String, type As Integer, dt As String)

        ' This call is required by the designer.
        InitializeComponent()
        user = username
        t = type
        _date = dt
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub OrderApprovalSeal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = user
        Label2.Text = _date
        If t = 2 Then
            Me.BackColor = Color.Beige
            PictureBox1.Image = My.Resources.icons8_sand_timer_30
        ElseIf t = 1 Then
            Me.BackColor = Color.LightGreen
            PictureBox1.Image = My.Resources.icons8_approval_30
        Else
            Me.BackColor = Color.LightCoral
            PictureBox1.Image = My.Resources.icons8_exercise_30
            ToolTip1.SetToolTip(PictureBox1, "Η παραγγελία παρακάμπτει τη συνήθη διαδικασία ελέγχων!")
            ToolTip1.SetToolTip(Label1, "Η παραγγελία παρακάμπτει τη συνήθη διαδικασία ελέγχων!")
            ToolTip1.SetToolTip(Label2, "Η παραγγελία παρακάμπτει τη συνήθη διαδικασία ελέγχων!")
        End If
    End Sub

    Private Sub OrderApprovalSeal_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        If t = 2 Then
            ControlPaint.DrawBorder(e.Graphics, Me.DisplayRectangle, Color.Gold, ButtonBorderStyle.Solid)
        ElseIf t = 1 Then
            ControlPaint.DrawBorder(e.Graphics, Me.DisplayRectangle, Color.DarkGreen, ButtonBorderStyle.Solid)
        Else
            ControlPaint.DrawBorder(e.Graphics, Me.DisplayRectangle, Color.LightCoral, ButtonBorderStyle.Solid)
        End If

    End Sub
End Class
