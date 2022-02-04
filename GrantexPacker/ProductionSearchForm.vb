Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class ProductionSearchForm

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim SearchStr As String = ""
    Dim retvalues As String = ""
    Public ResultList As New Dictionary(Of String, Object)
    Dim NotFirstRun As Boolean = False

    Public Sub New(ByVal str As String, R As String)

        ' This call is required by the designer.
        InitializeComponent()
        SearchStr = str.Replace("*", "%")
        SearchStr = SearchStr.Replace("+", "%")
        SearchStr = SearchStr.Replace("=", "%")
        SearchStr = SearchStr.Replace("&", "%")
        retvalues = R
        If Not SearchStr.Contains("%") Then
            SearchStr = SearchStr + "%"
        End If
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub ProductionSearchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        DataGridView1.Select()
        DataGridView2.ClearSelection()
        DataGridView2.CurrentCell = Nothing
        p.SetValue(Me.DataGridView1, True, Nothing)
        p.SetValue(Me.DataGridView2, True, Nothing)
        MATERIALTableAdapter.Fill(TESTFINALDataSet.MATERIAL, SearchStr)
        If DataGridView1.Rows.Count > 0 Then
            StartMainWorker(0)

        End If
        NotFirstRun = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex < 0 Then
            Return
        End If
        ResultList.Add("ITEID", DataGridView1.Rows(e.RowIndex).Cells("ID").Value)
        ResultList.Add("SUBCODE1", DataGridView1.Rows(e.RowIndex).Cells("SUBCODE1").Value)
        ResultList.Add("PARTNO", DataGridView1.Rows(e.RowIndex).Cells("MPARTNO").Value)
        ResultList.Add("BRAND", DataGridView1.Rows(e.RowIndex).Cells("brand").Value)
        ResultList.Add("FTRID", Nothing)
        ResultList.Add("TRADECODE", Nothing)
        ResultList.Add("QUANTITY", Nothing)
        Me.DialogResult = DialogResult.OK
    End Sub

    Dim SelectedRow As DataGridViewRow
    Private Sub DataGridView1_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles DataGridView1.RowStateChanged

    End Sub

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork
        Try
            Dim cmd As String = "SELECT f.id ftrid,isnull(c.fathername, '???') + ' ' + dbo.get_tradecode(f.id) 
                         + ' ' + sm.name  tradecode,s.id stlid,ST.M_DISPATCHDATE,t3.NAME, sum(CASE WHEN ISNULL(ITE.PRIMARYQTY, 0) = 0 THEN 0 ELSE s.PRIMARYQTY - ISNULL(z.quantity, 0) END)  rmn
FROM FINTRADE F INNER JOIN TBL_PACKERORDERCHECK T1 ON T1.FTRID=F.ID INNER JOIN STORETRADELINES S ON S.FTRID=F.ID 
INNER JOIN STORETRADE ST ON ST.FTRID=F.ID
INNER JOIN MATERIAL M ON M.ID=S.ITEID LEFT JOIN TBL_PACKERORDCLINES T ON T.STLID=S.ID AND T.LINE=1
left join customer c on c.id=f.cusid
left join salesman sm on sm.id=f.colidsalesman
LEFT JOIN  dbo.Z_PACKER_FULLITEMQUANTITIES AS z ON z.STLID = s.ID left join tbl_recipients t2 on t2.id=t.sc_recipient
left join TBL_PACKERORDERSTATUS t3 on t3.id=t1.status  LEFT OUTER JOIN
                         dbo.ITEMTRANSEST AS ITE ON ITE.STLID = s.ID
WHERE  M.id=" + SelectedRow.Cells("ID").Value.ToString + " AND F.DSRID=9000 AND T1.STATUS<12 AND T1.STATUS>=6 and t.sc_recipient in (" + retvalues + ") 
GROUP BY  f.id,DBO.GET_TRADECODE(F.ID),m.id,s.id,m.subcode1,ST.M_DISPATCHDATE,T1.STATUS,t3.NAME,f.ftrdate,c.fathername,sm.name 
having sum(ISNULL(z.quantity, 0))<sum(S.PRIMARYQTY)
ORDER BY 4 ASC"
            Dim dt As New DataTable()
            Using sc As New SqlCommand(cmd, conn)
                conn.Open()
                Using reader As SqlDataReader = sc.ExecuteReader

                    dt.Load(reader)
                    conn.Close()
                End Using
            End Using
            e.Result = dt
        Catch ex As Exception

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted
        Try
            For Each r As DataRow In TryCast(e.Result, DataTable).Rows
                DataGridView2.Rows.Add(New String() {r("ftrid"), r("tradecode"), r("stlid"), r("m_dispatchdate"), r("name"), r("rmn")})
            Next
            DataGridView2.Visible = True
            DataGridView2.ClearSelection()
            DataGridView2.CurrentCell = Nothing
            For Each c As Control In SplitContainer1.Panel2.Controls
                If Not IsNothing(TryCast(c, PictureBox)) Then
                    c.Dispose()
                End If

            Next
        Catch ex As Exception

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        StartMainWorker(e.RowIndex)

    End Sub

    Private Sub StartMainWorker(RowIndex As Integer)
        If RowIndex < 0 Then
            Return
        End If
        SelectedRow = DataGridView1.Rows(RowIndex)
        If mainworker.IsBusy Then
            mainworker.CancelAsync()
            While mainworker.IsBusy
                Application.DoEvents()
            End While
        End If
        DataGridView2.Visible = False
        DataGridView2.DataSource = Nothing
        DataGridView2.Rows.Clear()
        Dim pic As New PictureBox
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        pic.Image = My.Resources.rolling
        SplitContainer1.Panel2.Controls.Add(pic)
        'tb.Controls.Add(pic, 0, 2)
        pic.Dock = DockStyle.Fill
        mainworker.RunWorkerAsync()
    End Sub

    Private Sub DataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick
        If e.RowIndex < 0 Then
            Return
        End If
        ResultList.Add("ITEID", SelectedRow.Cells("ID").Value)
        ResultList.Add("SUBCODE1", SelectedRow.Cells("SUBCODE1").Value)
        ResultList.Add("PARTNO", SelectedRow.Cells("MPARTNO").Value)
        ResultList.Add("BRAND", SelectedRow.Cells("brand").Value)
        ResultList.Add("FTRID", DataGridView2.Rows(e.RowIndex).Cells("ftrid").Value)
        ResultList.Add("STLID", DataGridView2.Rows(e.RowIndex).Cells("stlid").Value)
        ResultList.Add("TRADECODE", DataGridView2.Rows(e.RowIndex).Cells("tradecode").Value)
        ResultList.Add("QUANTITY", DataGridView2.Rows(e.RowIndex).Cells("remaining").Value)
        Me.DialogResult = DialogResult.OK
    End Sub



    Private Sub ProductionSearchForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not TryCast(Me.Owner, production).Timer1.Enabled Then
            TryCast(Me.Owner, production).Timer1.Start()
        End If
        If mainworker.IsBusy Then
            mainworker.CancelAsync()
            While mainworker.IsBusy
                Application.DoEvents()
            End While
        End If
    End Sub

    Dim MovedRight As Integer = 0

    Private Sub ProductionSearchForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Try
            If e.KeyCode = Keys.Right Then
                e.SuppressKeyPress = True
                If DataGridView2.Rows.Count > 0 Then
                    If MovedRight = 0 Then MovedRight += 1
                    DataGridView2.Select()
                    DataGridView2.CurrentCell = DataGridView2.Rows(0).Cells("tradecode")
                End If

            ElseIf e.KeyCode = Keys.Left Then
                e.SuppressKeyPress = True
                If MovedRight = 1 Then MovedRight -= 1
                DataGridView1.Select()
                DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells("subcode1")

            ElseIf e.KeyCode = Keys.Enter And MovedRight = 0 Then
                If IsNothing(SelectedRow) Then
                    Me.DialogResult = DialogResult.Cancel
                    Me.Close()
                    Return
                End If
                ResultList.Add("ITEID", SelectedRow.Cells("ID").Value)
                ResultList.Add("SUBCODE1", SelectedRow.Cells("SUBCODE1").Value)
                ResultList.Add("PARTNO", SelectedRow.Cells("MPARTNO").Value)
                ResultList.Add("BRAND", SelectedRow.Cells("brand").Value)
                ResultList.Add("FTRID", Nothing)
                ResultList.Add("TRADECODE", Nothing)
                ResultList.Add("QUANTITY", Nothing)
                Me.DialogResult = DialogResult.OK
            ElseIf e.KeyCode = Keys.Enter And MovedRight = 1 Then
                If IsNothing(SelectedRow) Then
                    Me.DialogResult = DialogResult.Cancel
                    Me.Close()
                    Return
                End If
                ResultList.Add("ITEID", SelectedRow.Cells("ID").Value)
                ResultList.Add("SUBCODE1", SelectedRow.Cells("SUBCODE1").Value)
                ResultList.Add("PARTNO", SelectedRow.Cells("MPARTNO").Value)
                ResultList.Add("BRAND", SelectedRow.Cells("brand").Value)
                ResultList.Add("FTRID", DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Cells("ftrid").Value)
                ResultList.Add("STLID", DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Cells("stlid").Value)
                ResultList.Add("TRADECODE", DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Cells("tradecode").Value)
                ResultList.Add("QUANTITY", DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Cells("remaining").Value)
                Me.DialogResult = DialogResult.OK
            End If
            e.Handled = False
        Catch ex As Exception

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If NotFirstRun And DataGridView1.CurrentCell.RowIndex > -1 Then
            StartMainWorker(DataGridView1.CurrentCell.RowIndex)
        End If
    End Sub
End Class