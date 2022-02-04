Public Class errormsgbox

    Dim stack As String
    Dim message As String
    Dim title As String

    Private Sub errormsgbox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = message
        Me.Text = title
        TextBox1.Visible = False
        Me.Height = 134
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
            My.Resources.icons8_close_window_80.Dispose()
            PictureBox1.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal stck As String, ByVal msg As String, ByVal ttl As String)
        stack = msg + Environment.NewLine + stck
        message = msg
        title = ttl
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If Not Me.Height = 425 Then
            Me.Height = 425
            TextBox1.Text = stack
            TextBox1.Visible = True
            LinkLabel1.Text = "Λιγότερα"
        Else
            Me.Height = 134
            TextBox1.Visible = False
            LinkLabel1.Text = "Περισσότερα"
        End If
    End Sub
    Dim lblcorrected As Boolean = False
    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles Label1.TextChanged
        If Len(Label1.Text) > 130 And Not lblcorrected Then
            Label1.Text = Label1.Text.Substring(0, 130) + " (...) [πατήστε περισσότερα...]"
            lblcorrected = True
        End If
    End Sub
End Class
