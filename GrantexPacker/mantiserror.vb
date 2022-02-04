Public Class mantiserror

    Dim overflow, wrong As DataTable

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Public Sub New(o As DataTable, w As DataTable)

        ' This call is required by the designer.
        InitializeComponent()
        overflow = o
        wrong = w
        DataGridView1.DataSource = wrong
        DataGridView2.DataSource = overflow
        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class