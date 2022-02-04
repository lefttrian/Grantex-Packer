Public Class InformationPanel
    Dim warningstring As New List(Of String)
    Private Sub InformationPanel_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Dim w As Integer = 0
        Dim counter As Integer = 1
        For Each c As InformationPanelItem In FlowLayoutPanel1.Controls
            w += c.Width
            If Me.Width < w Then
                counter += 1
                w = 0
            End If
        Next
        Me.Height = counter * 31 + (counter * 1)
    End Sub

    Public Function GetRequiredHeight()
        Dim w As Integer = 0
        Dim counter As Integer = 1
        If FlowLayoutPanel1.Controls.Count = 0 Then
            Return 0
        Else
            For Each c As InformationPanelItem In FlowLayoutPanel1.Controls
                w += c.Width
                If Me.Width < w Then
                    counter += 1
                    w = 0
                End If
            Next
            Return counter * 31 + (counter * 1)
        End If
    End Function

    Public Sub AddItem(ByVal i As Integer, s As String, Optional link As Boolean = False, Optional data As DataTable = Nothing)
        Dim w As New InformationPanelItem(i, s, link, data)
        ' w.Width = FlowLayoutPanel1.Width - 2
        FlowLayoutPanel1.Controls.Add(w)
        w.Margin = New Padding(1)
    End Sub

    Public Function GetActiveWarnings()
        Return FlowLayoutPanel1.Controls.Count
    End Function

    Public Sub addwarning(ByVal l As Integer, s As String, Optional link As Boolean = False, Optional data As DataTable = Nothing)
        If String.IsNullOrWhiteSpace(s) Then
            Return
        End If
        If warningstring.Contains(s) Then
            Return
        End If
        warningstring.Add(s)
        Me.AddItem(l, s, link, data)
        Me.Dock = DockStyle.Fill
    End Sub

    Public Event ItemLinkClicked(ByVal ItemIndex As Integer)
    Private Sub LinkClicked(ByVal ItemIndex As Integer)
        RaiseEvent ItemLinkClicked(ItemIndex)
    End Sub

    Public Sub Clear()
        warningstring.Clear()
        For i As Integer = FlowLayoutPanel1.Controls.Count - 1 To 0 Step -1
            FlowLayoutPanel1.Controls(i).Dispose()
        Next
    End Sub

End Class
