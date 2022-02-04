Public Class NoPaddingButton
    Inherits Button

    Private odt As String

    Public Property OwnerDrawText As String
        Get
            Return odt
        End Get
        Set(ByVal value As String)
            odt = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)

        If String.IsNullOrEmpty(Text) AndAlso Not String.IsNullOrEmpty(odt) Then
            Dim stringFormat As StringFormat = New StringFormat()
            stringFormat.Alignment = StringAlignment.Center
            stringFormat.LineAlignment = StringAlignment.Center
            e.Graphics.DrawString(OwnerDrawText, Font, New SolidBrush(ForeColor), ClientRectangle, stringFormat)
        End If
    End Sub
End Class
