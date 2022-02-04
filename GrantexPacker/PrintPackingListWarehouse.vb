Imports GrantexPacker.TESTFINALDataSetTableAdapters
Imports Microsoft.Reporting.WinForms
Imports System.Data.SqlClient
Imports System.Configuration

Public Class PrintPackingListWarehouse
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim myPageSettings As New System.Drawing.Printing.PageSettings()
    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'TESTFINALDataSet.DataTable2' table. You can move, or remove it, as needed.
        'Me.DataTable2TableAdapter.Fill(Me.TESTFINALDataSet.DataTable2)
        'TODO: This line of code loads data into the 'TESTFINALDataSet.Z_PACKER_FULLREPORT' table. You can move, or remove it, as needed.
        'Me.Z_PACKER_FULLREPORTTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_FULLREPORT)
        ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\Packing List Warehouse.rdlc"
        Cursor.Current = ExtCursor1.Cursor
        Dim CUSTOMER As New ReportParameter("CUSTOMER", Form1.CUSTOMER)
        Dim PLIST As New ReportParameter("PLIST", Form1.plistid)
        Dim str As String
        myPageSettings.Landscape = True
        myPageSettings.Margins.Left = 30
        myPageSettings.Margins.Right = 5
        myPageSettings.Margins.Top = 5
        myPageSettings.Margins.Bottom = 5
        'ReportViewer1.LocalReport.SetParameters(PLIST)
        'ReportViewer1.LocalReport.SetParameters(CUSTOMER)

        Z_PACKER_FULLREPORTTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_FULLREPORT, Form1.plistid)
        DataTable1TableAdapter.Fill(Me.TESTFINALDataSet.DataTable1, Form1.plistid)
        Dim com As New SqlCommand("SELECT   top 1     CAST(STUFF
                             ((SELECT DISTINCT ', ' + dbo.get_tradecode(ftr.ID)
                                 FROM            TBL_PALLETLINES AS PL2 LEFT OUTER JOIN
                                                          TBL_PALLETHEADERS AS ph2 ON ph2.ID = PL2.PALLETID LEFT OUTER JOIN
                                                          FINTRADE AS ftr ON ftr.ID = PL2.ftrid
                                 WHERE        (ph2.PLID = ph.PLID) FOR XML PATH('')), 1, 1, '') AS TEXT)
as relorders
FROM            FINTRADE AS f INNER JOIN
                         SALESMAN AS s ON f.COLIDSALESMAN = s.ID INNER JOIN
                         TBL_PALLETLINES AS pl ON pl.ftrid = f.ID LEFT OUTER JOIN
                         TBL_PALLETHEADERS AS ph ON ph.ID = pl.PALLETID
WHERE        (ph.PLID =" + Form1.plistid.ToString + ")
GROUP BY f.ID, ph.PLID, s.EMAIL, CAST(s.REMARKS AS varchar(MAX))", updconn)
        updconn.Open()
        str = com.ExecuteScalar
        updconn.Close()

        Dim relorders As New ReportParameter("relorders", str)
        ReportViewer1.LocalReport.SetParameters(relorders)
        'TBL_PALLETHEADERSTableAdapter.Fill(Me.TESTDataSet.TBL_PALLETHEADERS, Form1.FTRID)
        'Me.ReportViewer1.SetPageSettings(myPageSettings)


        Me.ReportViewer1.RefreshReport()
        Me.ReportViewer1.RefreshReport()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub BindingSource1_CurrentChanged(sender As Object, e As EventArgs)

    End Sub
End Class