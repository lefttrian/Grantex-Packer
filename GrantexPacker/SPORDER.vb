Imports System.Configuration
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WinForms

Public Class SPORDER

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)


    Private Sub SPORDER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'TESTFINALDataSet.SPORDER' table. You can move, or remove it, as needed.
        Cursor.Current = ExtCursor1.Cursor
        ReportViewer1.LocalReport.ReportPath = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports\SparePartsOrder.rdlc"
        Dim cmd As String = "SELECT FTRTIME ,C.FATHERNAME,
                    b.descr,
                    dbo.get_tradecode(" + ftrid.ToString + "),ST.M_DISPATCHDATE,M_ORDERDETAILS,M_RECIPIENTS,cast(isnull(f.justification,'') as varchar(8000))+isnull(SECJUSTIFICATION,''),b.q,s.name
                    FROM FINTRADE F LEFT JOIN CUSTOMER C ON C.ID=F.CUSID 
                    LEFT JOIN
                    STORETRADE ST ON ST.FTRID=F.ID left join
                    salesman s on s.id=f.colidsalesman left join
                    (select top 1 ftrid,sum(primaryqty) q, STUFF(( SELECT distinct ', ' +  mnf2.descr          FROM MANUFACTURER mnf2   
inner join material m2 on m2.MNFID=mnf2.codeid 
inner join  storetradelines s2 on s2.iteid=m2.id   where s2.ftrid=s.ftrid    FOR XML PATH ('') )  , 1, 1, '')  AS descr
from storetradelines s inner join material m on m.id=s.iteid
where ftrid=" + ftrid.ToString + " and substring(m.code,1,1)='2' and substring(m.code,1,3) not in ('202') group by ftrid,m.mnfid) b on b.ftrid=f.id
                    WHERE F.ID=" + ftrid.ToString
        Dim cmd2 As String = "select * from [dbo].[ftr_checkedby_report](" + ftrid.ToString + ",3)"
        Using COM As New SqlCommand(cmd, updconn)
            Using DT As New DataTable
                updconn.Open()
                Using READER As SqlDataReader = COM.ExecuteReader
                    DT.Load(READER)

                End Using
                updconn.Close()
                Dim rcpnts As String
                Using com2 As New SqlCommand(" select dbo.[ftr_recipients_report](" + ftrid.ToString + "," + orderphase.ToString + ",3)", updconn)
                    updconn.Open()
                    rcpnts = com2.ExecuteScalar
                    updconn.Close()
                End Using
                Dim pc As String
                Using com3 As New SqlCommand(" select comments from TBL_PACKERUSERORDERCOMMENTS where ftrid=" + ftrid.ToString + " and userid=" + Form1.activeuserid.ToString, conn)
                    conn.Open()
                    pc = com3.ExecuteScalar
                    conn.Close()
                End Using
                Dim pos As String
                Using com4 As New SqlCommand(" select 
SUBSTRING(comments,charindex('{',comments,0)+1,charindex('}',comments,0)-charindex('{',comments,0)-case when charindex('}',comments,0)=0 then 0 else 1 end )   
 from TBL_PACKERUSERORDERCOMMENTS where ftrid=" + ftrid.ToString + " and userid=" + Form1.activeuserid.ToString, conn)
                    conn.Open()
                    pos = com4.ExecuteScalar
                    conn.Close()
                End Using
                Dim CREATEDATE As New ReportParameter("CREATEDATE", DT.Rows(0).Item(0).ToString)
                Dim FATHERNAME As New ReportParameter("FATHERNAME", DT.Rows(0).Item(1).ToString)
                Dim BRAND As New ReportParameter("BRAND", DT.Rows(0).Item(2).ToString)
                Dim ORDERNUM As New ReportParameter("ORDERNUM", DT.Rows(0).Item(3).ToString)
                Dim DISPATCHDATE As New ReportParameter("DISPATCHDATE", DT.Rows(0).Item(4).ToString)
                Dim ORDERDETAILS As New ReportParameter("ORDERDETAILS", DT.Rows(0).Item(5).ToString)
                Dim RECIPIENTS As New ReportParameter("RECIPIENTS", rcpnts)
                Dim comments As New ReportParameter("COMMENTS", DT.Rows(0).Item(7).ToString)
                Dim setquant As New ReportParameter("setquant", DT.Rows(0).Item(8).ToString)
                Dim salesman As New ReportParameter("salesman", DT.Rows(0).Item(9).ToString)
                Dim perscomments As New ReportParameter("PERSONALCOMMENTS", pc)
                Dim posit As New ReportParameter("POSITION", pos)
                Dim op As New ReportParameter("ORDERPHASE", orderphase)
                ReportViewer1.LocalReport.SetParameters(op)
                Me.SPORDERTableAdapter.Fill(Me.TESTFINALDataSet.SPORDER, ftrid)
                ReportViewer1.LocalReport.SetParameters(CREATEDATE)
                ReportViewer1.LocalReport.SetParameters(FATHERNAME)
                ReportViewer1.LocalReport.SetParameters(BRAND)
                ReportViewer1.LocalReport.SetParameters(ORDERNUM)
                ReportViewer1.LocalReport.SetParameters(DISPATCHDATE)
                ReportViewer1.LocalReport.SetParameters(ORDERDETAILS)
                ReportViewer1.LocalReport.SetParameters(RECIPIENTS)
                ReportViewer1.LocalReport.SetParameters(comments)
                ReportViewer1.LocalReport.SetParameters(setquant)
                ReportViewer1.LocalReport.SetParameters(salesman)
                ReportViewer1.LocalReport.SetParameters(posit)
                ReportViewer1.LocalReport.SetParameters(perscomments)
            End Using
        End Using
        Using com2 As New SqlCommand(cmd2, conn)
            Using DT2 As New DataTable
                conn.Open()
                Using READER2 As SqlDataReader = com2.ExecuteReader
                    DT2.Load(READER2)

                End Using
                conn.Close()
                For i As Integer = 0 To DT2.Rows.Count - 1
                    If i = 0 Then
                        Dim c1 As New ReportParameter("C1", DT2.Rows(i).Item("title").ToString)
                        Dim CD1 As New ReportParameter("CD1", DT2.Rows(i).Item("checkdate").ToString)
                        ReportViewer1.LocalReport.SetParameters(c1)
                        ReportViewer1.LocalReport.SetParameters(CD1)
                    End If
                    If i = 1 Then
                        Dim c2 As New ReportParameter("C2", DT2.Rows(i).Item("title").ToString)
                        Dim CD2 As New ReportParameter("CD2", DT2.Rows(i).Item("checkdate").ToString)
                        ReportViewer1.LocalReport.SetParameters(c2)
                        ReportViewer1.LocalReport.SetParameters(CD2)
                    End If
                    If i = 2 Then
                        Dim c3 As New ReportParameter("C3", DT2.Rows(i).Item("title").ToString)
                        Dim CD3 As New ReportParameter("CD3", DT2.Rows(i).Item("checkdate").ToString)
                        ReportViewer1.LocalReport.SetParameters(c3)
                        ReportViewer1.LocalReport.SetParameters(CD3)
                    End If
                    If i = 3 Then
                        Dim c4 As New ReportParameter("C4", DT2.Rows(i).Item("title").ToString)
                        Dim CD4 As New ReportParameter("CD4", DT2.Rows(i).Item("checkdate").ToString)
                        ReportViewer1.LocalReport.SetParameters(c4)
                        ReportViewer1.LocalReport.SetParameters(CD4)
                    End If
                    If i = 4 Then
                        Dim c5 As New ReportParameter("C5", DT2.Rows(i).Item("title").ToString)
                        Dim CD5 As New ReportParameter("CD5", DT2.Rows(i).Item("checkdate").ToString)
                        ReportViewer1.LocalReport.SetParameters(c5)
                        ReportViewer1.LocalReport.SetParameters(CD5)
                    End If
                Next
            End Using
        End Using
        Me.ReportViewer1.RefreshReport()
        Cursor.Current = Cursors.Default

    End Sub
    Dim orderphase As Integer
    Dim ftrid As Integer
    Public Sub New(ByVal value As Integer, ByVal value2 As Integer)
        ftrid = value

        orderphase = value2
        InitializeComponent()
    End Sub


    Private Sub ReportViewer1_PrintingBegin(sender As Object, e As ReportPrintEventArgs) Handles ReportViewer1.PrintingBegin
        Dim txt As String = "Εντολή Ανταλλακτικών, " + Form1.activeuser + ", " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine
        Using cmd As New SqlCommand("update tbl_packerordercheck set printed=@printed+cast(isnull(printed,'') as varchar(7000)) where ftrid=" + ftrid.ToString, updconn)
            cmd.Parameters.Add("@printed", SqlDbType.VarChar, 8000).Value = txt
            updconn.Open()

            cmd.ExecuteNonQuery()
            updconn.Close()
        End Using
    End Sub
End Class