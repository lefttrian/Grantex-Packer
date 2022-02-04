Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Reflection

Public Class UsertTransactions
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim userid As Integer = 0
    Public Sub New(user_id)

        ' This call is required by the designer.
        InitializeComponent()
        userid = user_id
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub NoPaddingButton2_Click(sender As Object, e As EventArgs) Handles NoPaddingButton2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub UsertTransactions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            GetType(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, AdvancedDataGridView1, New Object() {True})
            Using s As New SqlCommand("SELECT ID,USERNAME FROM TBL_PACKERUSERDATA WHERE USERNAME IS NOT NULL", conn)
                Dim dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                End Using
                conn.Close()
                ComboBox1.DataSource = dt
                ComboBox1.DisplayMember = "USERNAME"
                ComboBox1.ValueMember = "ID"
            End Using
            ComboBox1.SelectedValue = Form1.activeuserid
            If Form1.activeuserdpt <> "SA" Then
                ComboBox1.Enabled = False
            End If
            DateTimePicker1.Value = DateAdd(DateInterval.Day, -7, Today)
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Dim visibleColumns As New List(Of String) From {{"USERNAME"}, {"TIME"}, {"PHRASE"}}
    Private Sub NoPaddingButton1_Click(sender As Object, e As EventArgs) Handles NoPaddingButton1.Click
        Try
            PKRVIWUSERTRANSACTIONSBindingSource.DataSource = Nothing
            PKRVIWUSERTRANSACTIONSBindingSource.Clear()
            AtlantisDataSet.PKRVIW_USERTRANSACTIONS.Clear()
            PKRVIW_USERTRANSACTIONSTableAdapter.Fill(AtlantisDataSet.PKRVIW_USERTRANSACTIONS, ComboBox1.SelectedValue, DateTimePicker1.Value)
            PKRVIWUSERTRANSACTIONSBindingSource.DataSource = AtlantisDataSet
            PKRVIWUSERTRANSACTIONSBindingSource.DataMember = "PKRVIW_USERTRANSACTIONS"
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            For Each c As DataGridViewColumn In AdvancedDataGridView1.Columns
                If Not visibleColumns.Contains(c.DataPropertyName) Then
                    c.Visible = False
                End If
            Next
            AdvancedDataGridViewSearchToolBar1.SetColumns(AdvancedDataGridView1.Columns)
        End Try
    End Sub


    Private Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar1.Search
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = AdvancedDataGridView1.CurrentCell.ColumnIndex + 1 >= AdvancedDataGridView1.ColumnCount
            Dim endrow As Boolean = AdvancedDataGridView1.CurrentCell.RowIndex + 1 >= AdvancedDataGridView1.RowCount

            If endcol AndAlso endrow Then
                startColumn = AdvancedDataGridView1.CurrentCell.ColumnIndex
                startRow = AdvancedDataGridView1.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, AdvancedDataGridView1.CurrentCell.ColumnIndex + 1)
                startRow = AdvancedDataGridView1.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If

        Dim c As DataGridViewCell = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            AdvancedDataGridView1.CurrentCell = c
        End If
    End Sub


End Class