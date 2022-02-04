Public Class PopUpNotification

    Dim txt As String = ""
    Dim x, y As Integer
    Public Sub New(ntfction_string As String, x_ As Integer, y_ As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        txt = ntfction_string
        ' Add any initialization after the InitializeComponent() call.
        x = x_
        y = y_
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

    Private Sub NoPaddingButton1_Click(sender As Object, e As EventArgs) Handles NoPaddingButton1.Click
        Me.Dispose()
    End Sub

    Private Sub Label3_TextChanged(sender As Object, e As EventArgs) Handles Label3.TextChanged
        Dim textSize As Size = TextRenderer.MeasureText(Label3.Text, Label3.Font)
        'Dim widths As Int32() = TableLayoutPanel1.GetColumnWidths()
        Dim Width As Integer = TableLayoutPanel1.ColumnStyles(0).Width
        Dim height As Integer = TableLayoutPanel1.RowStyles(0).Height
        If textSize.Width > Width Then
            If textSize.Width - Width + Me.Width > Me.MaximumSize.Width Then
                If textSize.Height > Me.MaximumSize.Height Then
                    Me.Height = Me.MaximumSize.Height
                Else
                    Me.Height = textSize.Height
                End If

                Me.Width += textSize.Width - Width
            End If
        End If
    End Sub

    Private Sub PopUpNotification_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        Me.Dispose()
    End Sub

    Private Sub PopUpNotification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetLocation(x, y)
        Label3.Text = txt
    End Sub
End Class