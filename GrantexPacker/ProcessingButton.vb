Public Class ProcessingButton

    Public Event UC_Button1Click()

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim i As New PictureBox
        i.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(i)
        'i.Dock = DockStyle.Fill
        i.SizeMode = PictureBoxSizeMode.CenterImage
        i.Size = Button5.Size
        i.Image = My.Resources.rolling__2_
        Button5.Visible = False
        Me.Controls.Add(i)
        i.Location = Button5.Location
        RaiseEvent UC_Button1Click()
    End Sub

    Private Sub revert_state(ByVal s As String)
        Button5.Visible = True
        For Each c As Control In Me.Controls
            Dim i = TryCast(c, PictureBox)
            If Not IsNothing(i) Then
                i.Dispose()
            End If
        Next

    End Sub

    Public Overrides Property Text As String
        Get
            Return Button5.Text
        End Get
        Set(value As String)
            Button5.Text = value
        End Set
    End Property

    Dim MyState As String

    Public Property state As String
        Get
            Return MyState
        End Get
        Set(value As String)
            Change_Type(value)
        End Set
    End Property

    Private Sub Change_Type(ByVal s As String)
        MyState = s
        If MyState = "completed" Then
            Button5.Image = My.Resources.icons8_remove_approval_21
            Me.Text = "Άναίρεση κλεισίματος"
        ElseIf MyState = "normal" Then
            Button5.Image = My.Resources.icons8_approval_pending_30
            Me.Text = "Κλείσιμο Packing List"
        ElseIf MyState = "draft" Then
            Button5.Image = My.Resources.icons8_advance_21
            Me.Text = "Μετατροπή σε κανονικό"
        End If
    End Sub

    Public Sub complete(ByVal status As Integer)
        If status = 1 Then
            For Each c As Control In Me.Controls
                Dim i = TryCast(c, PictureBox)
                If Not IsNothing(i) Then
                    i.Image = My.Resources.icons8_approval_30
                End If
            Next
        Else
            For Each c As Control In Me.Controls
                Dim i = TryCast(c, PictureBox)
                If Not IsNothing(i) Then
                    i.Image = My.Resources.icons8_close_window_30
                End If
            Next
            Timer1.Start()
        End If

    End Sub

    Private Sub ProcessingButton_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MyState = "normal"
    End Sub

    Dim counter As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        counter += 1
        If counter = 5 Then
            revert_state(MyState)
            Timer1.Stop()
            counter = 0
        End If
    End Sub


End Class
