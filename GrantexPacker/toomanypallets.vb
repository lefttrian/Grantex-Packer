Public Class toomanypallets
    Private Sub toomanypallets_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Form1.rtimer.Stop()
        Form1.rtime = Form1.rlimit
        Label1.Text = "Η αναζήτηση σας επέστρεψε " + num.ToString + " παλέτες από πολλαπλούς πελάτες. Αυτό θα έχει ως συνέπεια σημαντική καθυστέρηση της εφαρμογής. Επιλέξτε πώς θέλετε να συνεχίσετε:"
    End Sub

    Dim num As Integer
    Public Sub New(ByVal n As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        num = n
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub toomanypallets_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Form1.rlimit <> 0 Then
            Form1.rtimer.Start()
        End If

    End Sub
End Class