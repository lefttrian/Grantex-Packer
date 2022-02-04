Imports Microsoft.Reporting.WinForms

Public Class PrintPalletReport

    Dim myPageSettings As New System.Drawing.Printing.PageSettings()
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If Form1.FlowLayoutPanel1.Controls.Count > 0 Then
                'TODO: This line of code loads data into the 'TESTFINALDataSet.Z_PACKER_PALLETREPORT' table. You can move, or remove it, as needed.
                'Me.Z_PACKER_PALLETREPORTTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_PALLETREPORT)
                ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\PalletReport.rdlc"
                Cursor.Current = ExtCursor1.Cursor
                Dim CUSTOMER As New ReportParameter("CUSTOMER", Form1.CUSTOMER)
                Dim data As New ReportParameter("PLIST", Form1.palletsforlabels)
                myPageSettings.Landscape = True
                myPageSettings.Margins.Left = 30
                myPageSettings.Margins.Right = 5
                myPageSettings.Margins.Top = 5
                myPageSettings.Margins.Bottom = 5
                ReportViewer1.LocalReport.SetParameters(data)
                ReportViewer1.LocalReport.SetParameters(CUSTOMER)
                ReportViewer1.LocalReport.DataSources.Clear()
                ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", Me.Z_PACKER_PALLETREPORTBindingSource))
                Z_PACKER_PALLETREPORTTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_PALLETREPORT, Form1.palletsforlabels)
                'TBL_PALLETHEADERSTableAdapter.Fill(Me.TESTDataSet.TBL_PALLETHEADERS, Form1.FTRID)
                'Me.ReportViewer1.SetPageSettings(myPageSettings)
                Me.ReportViewer1.RefreshReport()
                Cursor.Current = Cursors.Default
            End If
            Me.ReportViewer1.RefreshReport()
            Me.ReportViewer1.RefreshReport()
        Catch
        End Try
    End Sub
End Class