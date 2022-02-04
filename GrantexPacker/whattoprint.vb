Public Class whattoprint
    Private Sub whattoprint_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CheckedListBox1.SetItemChecked(0, 1)
        CheckedListBox1.SetItemChecked(1, 0)
        CheckedListBox1.SetItemChecked(2, 0)
        CheckedListBox1.SetItemChecked(3, 0)
        CheckedListBox1.SetItemChecked(4, 0)
        Button6.PerformClick()
    End Sub
    Private list As New List(Of Integer)
    Public retvalue As String = ""
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        For Each l In CheckedListBox1.CheckedItems
            list.Add(CheckedListBox1.Items.IndexOf(l) + 1)
        Next
        retvalue = String.Join(",", list.ToArray)

        DialogResult = DialogResult.OK
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        CheckedListBox1.SetItemChecked(0, 0)
        CheckedListBox1.SetItemChecked(1, 1)
        CheckedListBox1.SetItemChecked(2, 0)
        CheckedListBox1.SetItemChecked(3, 0)
        CheckedListBox1.SetItemChecked(4, 0)
        Button6.PerformClick()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        CheckedListBox1.SetItemChecked(0, 1)
        CheckedListBox1.SetItemChecked(1, 0)
        CheckedListBox1.SetItemChecked(2, 1)
        CheckedListBox1.SetItemChecked(3, 0)
        CheckedListBox1.SetItemChecked(4, 1)
        Button6.PerformClick()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        CheckedListBox1.SetItemChecked(0, 0)
        CheckedListBox1.SetItemChecked(1, 1)
        CheckedListBox1.SetItemChecked(2, 1)
        CheckedListBox1.SetItemChecked(3, 1)
        CheckedListBox1.SetItemChecked(4, 1)
        Button6.PerformClick()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CheckedListBox1.SetItemChecked(0, 1)
        CheckedListBox1.SetItemChecked(1, 1)
        CheckedListBox1.SetItemChecked(2, 1)
        CheckedListBox1.SetItemChecked(3, 1)
        CheckedListBox1.SetItemChecked(4, 1)
        list.Add(0)
        Button6.PerformClick()
    End Sub

    Private Sub whattoprint_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class