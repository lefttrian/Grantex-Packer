Public Class DraftPLSearchOrdersOverflow
    Private Sub DraftPLSearchOrdersOverflow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = str
        Me.SetDesktopLocation(po.X, po.Y)
        DataGridView1.ColumnHeadersVisible = False
        'TryCast(Me.Owner, PackingListEstimationManagement).TextBox2.Select()
    End Sub

    Dim str As New DataTable()
    Dim po As Point

    Public Sub New(ByVal Searchdt As DataTable, p As Point)

        ' This call is required by the designer.
        InitializeComponent()
        str = Searchdt
        po = p
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        ' TryCast(Me.Owner, PackingListEstimationManagement).selectedcode = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        ' TryCast(Me.Owner, PackingListEstimationManagement).TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        Me.Close()
        Me.Dispose()
    End Sub
End Class