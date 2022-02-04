Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Reflection
Imports Microsoft.Reporting.WinForms

Public Class PrintOpenLabels


    Private Sub Form9_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
        Dim conn As New SqlConnection(connString)
        Try
            Cursor.Current = ExtCursor1.Cursor
            Dim path As String
            conn.Open()
            Using s As New SqlCommand("SELECT isnull(OLTEMPLATE,'-') FROM PKRTBL_CUSTOMER WHERE CUSID=(SELECT distinct CUSID FROM TBL_PALLETHEADERS WHERE ID IN (" + Form1.palletsforlabels + "))", conn)
                path = s.ExecuteScalar()
            End Using
            conn.Close()
            If path = "-" OrElse IsNothing(path) Then
                ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\OpenLabels.rdlc"
            Else
                ReportViewer1.LocalReport.ReportPath = path
            End If
            Dim myPageSettings As New System.Drawing.Printing.PageSettings()
            'TODO: This line of code loads data into the 'TESTFINALDataSet.DataTable2' table. You can move, or remove it, as needed.

            'Dim logo As New ReportParameter("logo", Form1.logo)
            'Dim PLIST As New ReportParameter("PLIST", Form1.plistid)
            'myPageSettings.PaperSize.Kind = Printing.PaperKind.Custom
            Dim yy As New System.Drawing.Printing.PaperSize("9x9", 354, 354)

            'myPageSettings.Document = New Printing.PrintDocument

            myPageSettings.PaperSize = yy
            myPageSettings.Landscape = True
            myPageSettings.Margins.Left = 1
            myPageSettings.Margins.Right = 1
            myPageSettings.Margins.Top = 1
            myPageSettings.Margins.Bottom = 1
            'myPageSettings.PaperSize =

            'ReportViewer1.LocalReport.SetParameters(PLIST)
            'ReportViewer1.LocalReport.SetParameters(logo)

            Me.DataTable2TableAdapter.Fill(Me.TESTFINALDataSet.DataTable2, Form1.palletsforlabels)
            Me.ReportViewer1.SetPageSettings(myPageSettings)
            Me.ReportViewer1.RefreshReport()
            Cursor.Current = Cursors.Default
            'Me.ReportViewer1.RefreshReport()
            'Me.ReportViewer1.SetPageSettings(myPageSettings)
        Catch EX As Exception
            Dim msg As String = ""
            If EX.Message.Contains("Subquery returned more than 1 value.") Then
                msg = "Επιτρέπονται μαζικές εκτυπώσεις παλετών που αναφέρονται σε έναν πελάτη."
            Else
                msg = EX.Message
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, msg, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Form9_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.ReportViewer1.LocalReport.ReleaseSandboxAppDomain()
        Me.ReportViewer1.Dispose()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            ExtCursor1.Dispose()
            DataTable2BindingSource.Dispose()
            DataTable2TableAdapter.Dispose()
            TESTFINALDataSet.Dispose()
            ReportViewer1.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class