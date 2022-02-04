Public Class Form17
    Dim iteid As String

    Dim code As String

    Private Sub button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal i As String, c As String)
        iteid = i

        code = c

        InitializeComponent()
    End Sub

    Private Sub Form17_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cursor.Current = ExtCursor1.Cursor
        Me.Label1.Text = "Αποθέματα για είδη " + code.Substring(0, 8).ToString
        Me.ExtramantisinfoTableAdapter.Fill(Me.TESTFINALDataSet.extramantisinfo, CInt(code.Substring(0, 8)))
        Me.Extramantisinfo_returnsTableAdapter.Fill(Me.TESTFINALDataSet.extramantisinfo_returns, CInt(code.Substring(0, 8)))
        Cursor.Current = Cursors.Default


    End Sub
End Class