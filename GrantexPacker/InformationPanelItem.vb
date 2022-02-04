Public Class InformationPanelItem
    Implements IDisposable

    Dim formtype As Integer = -1
    Dim message As String = ""
    Dim visible_link As Boolean = False
    Dim dt As DataTable = Nothing
    Public Sub New(ByVal type As Integer, msg As String, Optional visiblelink As Boolean = False, Optional data As DataTable = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        formtype = type
        message = msg
        dt = data
        If visiblelink Then
            visible_link = visiblelink
        End If
        'Me.Width = Me.Parent.Width - 2
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub InformationPanelItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If formtype = 0 Then
                PictureBox1.Image = My.Resources.icons8_cancel_30
                Panel1.BackColor = Color.MistyRose
                Label1.ForeColor = Color.Red
                Me.BackColor = Color.Red
            ElseIf formtype = 1 Then
                PictureBox1.Image = My.Resources.icons8_error_30
                Panel1.BackColor = Color.LightYellow
                Label1.ForeColor = Color.DarkGoldenrod
                Me.BackColor = Color.DarkGoldenrod
            ElseIf formtype = 2 Then
                PictureBox1.Image = My.Resources.icons8_info_30
                Panel1.BackColor = Color.Azure
                Label1.ForeColor = Color.DarkBlue
                Me.BackColor = Color.DarkBlue
            ElseIf formtype = 3 Then
                PictureBox1.Image = My.Resources.icons8_approval_30
                Panel1.BackColor = Color.LightGreen
                Label1.ForeColor = Color.Green
                Me.BackColor = Color.Green
            Else
                Throw New Exception("Επιλέξτε τύπο ειδοποίησης 0 για σφάλμα, 1 για προειδοποίηση, 2 για πληροφορία, 3 για έγκριση")
            End If
            If visible_link Then
                LinkLabel1.Visible = True
            End If
            Label1.Text = message
            If visible_link Then
                Me.Width = Label1.Width + PictureBox1.Width + 5 + LinkLabel1.Width
            Else
                Me.Width = Label1.Width + PictureBox1.Width + 5
            End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim f As New InformationPanelGenericDialog(message, dt, MousePosition.X, MousePosition.Y)
        f.Owner = GetProdParent(Me)
        f.Show()
    End Sub

    Public Function GetProdParent(currentControl As Control) As Control
        Dim parent As Control = Nothing

        Do While (currentControl IsNot Nothing) AndAlso (currentControl IsNot production)
            parent = If(currentControl.Parent, parent)
            currentControl = currentControl.Parent
        Loop

        Return parent
    End Function
    ' IDisposable

End Class
