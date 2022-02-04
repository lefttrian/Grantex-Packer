Imports GrantexPacker.TESTFINALDataSetTableAdapters
Imports Microsoft.Reporting.WinForms
Imports System.Data.SqlClient
Imports System.Configuration

Public Class PrintPackingList
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim myPageSettings As New System.Drawing.Printing.PageSettings()
    Dim plistid As Integer
    Public Sub New(ByVal id As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        plistid = id
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'TODO: This line of code loads data into the 'TESTFINALDataSet.DataTable2' table. You can move, or remove it, as needed.
            'Me.DataTable2TableAdapter.Fill(Me.TESTFINALDataSet.DataTable2)
            'TODO: This line of code loads data into the 'TESTFINALDataSet.Z_PACKER_FULLREPORT' table. You can move, or remove it, as needed.
            'Me.Z_PACKER_FULLREPORTTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_FULLREPORT)
            Cursor.Current = ExtCursor1.Cursor
            Dim CUSTOMER As New ReportParameter("CUSTOMER", Form1.CUSTOMER)
            Dim PLIST As New ReportParameter("PLIST", plistid)
            Dim str As String
            myPageSettings.Landscape = True
            myPageSettings.Margins.Left = 30
            myPageSettings.Margins.Right = 5
            myPageSettings.Margins.Top = 5
            myPageSettings.Margins.Bottom = 5
            'ReportViewer1.LocalReport.SetParameters(PLIST)
            'ReportViewer1.LocalReport.SetParameters(CUSTOMER)

            Z_PACKER_FULLREPORTTableAdapter.Fill(Me.TESTFINALDataSet.Z_PACKER_FULLREPORT, plistid)
            DataTable1TableAdapter1.Fill(Me.TESTFINALDataSet.DataTable1, plistid)

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
WHERE        (ph.PLID =" + plistid.ToString + ")
GROUP BY f.ID, ph.PLID, s.EMAIL, CAST(s.REMARKS AS varchar(MAX))", conn)
            conn.Open()
            str = com.ExecuteScalar
            conn.Close()
            Dim path As String
            conn.Open()
            Using s As New SqlCommand("SELECT isnull(PLTEMPLATE,'-') FROM PKRTBL_CUSTOMER WHERE CUSID=(SELECT PLCUSID FROM TBL_PACKINGLISTS WHERE ID=" + plistid.ToString + ")", conn)
                path = s.ExecuteScalar()
            End Using
            If path = "-" OrElse IsNothing(path) Then
                ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\Packing List Default.rdlc"
            Else
                ReportViewer1.LocalReport.ReportPath = path
            End If
            Dim relorders As New ReportParameter("relorders", str)
            ReportViewer1.LocalReport.SetParameters(relorders)
            Dim rds As ReportDataSource = New ReportDataSource("DataSet1", Z_PACKER_FULLREPORTBindingSource)
            Dim rds2 As ReportDataSource = New ReportDataSource("DataSet2", DataTable1BindingSource)
            Dim rds3 As ReportDataSource = New ReportDataSource("DataSet3", DataTable2BindingSource)
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.DataSources.Add(rds2)
            ReportViewer1.LocalReport.DataSources.Add(rds3)
            'TBL_PALLETHEADERSTableAdapter.Fill(Me.TESTDataSet.TBL_PALLETHEADERS, Form1.FTRID)
            'Me.ReportViewer1.SetPageSettings(myPageSettings)
            Cursor.Current = Cursors.Default
            Me.ReportViewer1.RefreshReport()
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Dim msg As String = ex.Message
            If ex.Message.Contains("An error occurred during local report processing.") Then
                msg = "Κάποιο πρόβλημα προέκυψε κατά τη πρόσβαση στο template. Ελέγξτε ότι έχετε πρόσβαση και δεν έχει διαγραφεί κάποιο αρχείο στον κοινόχρηστο φάκελο."
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, msg, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub BindingSource1_CurrentChanged(sender As Object, e As EventArgs)

    End Sub

    Public printed As Boolean = False
    Private Sub ReportViewer1_Print(sender As Object, e As ReportPrintEventArgs) Handles ReportViewer1.Print
        Dim s As Integer = PrintExportProc()
        If s = -1 Then
            e.Cancel = True
        Else
            printed = True
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub ReportViewer1_ReportExport(sender As Object, e As ReportExportEventArgs) Handles ReportViewer1.ReportExport
        Dim s As Integer = PrintExportProc()
        If s = -1 Then
            e.Cancel = True
        Else
            printed = True
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Function PrintExportProc()
        Dim plstatus As Integer

        If Not IsNothing(TryCast(Me.Owner, PackingList)) Then
            plstatus = TryCast(Me.Owner, PackingList).plstatus
        End If
        Dim r As DialogResult = DialogResult.Yes
        If plstatus <> 1 Then
            r = MessageBox.Show("Εκτυπώνοντας DRAFT ή μη ολοκληρωμένο packing list, κλειδώνετε τη μορφή του, εμποδίζοντας να γίνουν αλλαγές σε παλέτες και τα περιεχόμενα τους. Είστε σίγουροι;", "Προσοχή!", MessageBoxButtons.YesNo)
        End If
        If r = DialogResult.No Then
            Return -1
        Else
            Return 1
        End If
    End Function

End Class