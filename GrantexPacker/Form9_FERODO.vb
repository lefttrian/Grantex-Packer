Public Class Form9_FERODO
    Private Sub Form9_FERODO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'TESTFINALDataSet.Z_PACKER_VALEOPALLET' table. You can move, or remove it, as needed.
        'Me.Z_PACKER_VALEOPALLETTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_VALEOPALLET)
        Dim myPageSettings As New System.Drawing.Printing.PageSettings()
        'TODO: This line of code loads data into the 'TESTFINALDataSet.DataTable2' table. You can move, or remove it, as needed.
        Cursor.Current = ExtCursor1.Cursor
        'Dim logo As New ReportParameter("logo", Form1.logo)
        'Dim PLIST As New ReportParameter("PLIST", Form1.plistid)
        'myPageSettings.PaperSize.Kind = Printing.PaperKind.Custom
        Dim yy As New System.Drawing.Printing.PaperSize("A4", 827, 1169)

        'myPageSettings.Document = New Printing.PrintDocument

        myPageSettings.PaperSize = yy
        myPageSettings.Landscape = False
        myPageSettings.Margins.Left = 0
        myPageSettings.Margins.Right = 0
        myPageSettings.Margins.Top = 0
        myPageSettings.Margins.Bottom = 0
        'myPageSettings.PaperSize =

        'ReportViewer1.LocalReport.SetParameters(PLIST)
        'ReportViewer1.LocalReport.SetParameters(logo)
        Try
            Me.Z_PACKER_VALEOPALLETTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_VALEOPALLET, Form1.palletsforlabels)
        Catch
        End Try
        Me.ReportViewer1.SetPageSettings(myPageSettings)
        Me.ReportViewer1.RefreshReport()
        'Me.ReportViewer1.RefreshReport()
        'Me.ReportViewer1.SetPageSettings(myPageSettings)
        Me.ReportViewer1.RefreshReport()
        Me.ReportViewer1.RefreshReport()
        Cursor.Current = Cursors.Default
    End Sub
End Class