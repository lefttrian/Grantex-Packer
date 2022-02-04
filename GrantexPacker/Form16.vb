Public Class Form16
    Dim t1, t2 As String
    Public Sub New(ByVal value As String, ByVal value2 As Date)
        t1 = value
        t2 = value2.ToString
        InitializeComponent()
    End Sub

    Private Sub Form16_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Text = t1
        Label3.Text = t2
        Dim r As Rectangle = Screen.PrimaryScreen.WorkingArea

        Me.Location = New Point(r.Right - Me.Width, r.Bottom - Me.Height * (Application.OpenForms().OfType(Of Form16).Count))
        fade_in()

    End Sub

    Public Sub fade_in()
        For FadeIn = 0.0 To 1.1 Step 0.1
            Me.Opacity = FadeIn
            Me.Refresh()
            Threading.Thread.Sleep(100)
        Next
        CType(Form1, System.Windows.Forms.Form).Focus()
    End Sub


    Dim time As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        time += 1
        If time = 1 Then
            time = 0
            Me.Close()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class