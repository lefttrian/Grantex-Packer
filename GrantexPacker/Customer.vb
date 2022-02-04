Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Reflection

Public Class Customer

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim loaded As Boolean = False
    Private _customers As New List(Of PTDType)
    Private _pallettypes As New List(Of PTDType)
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub Customer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'TESTFINALDataSet.PKRTBL_CUSTOMER' table. You can move, or remove it, as needed.
        Try
            Me.PKRTBL_CUSTOMERTableAdapter.Fill(Me.TESTFINALDataSet.PKRTBL_CUSTOMER)
            GetType(Zuby.ADGV.AdvancedDataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, DataGridView1, New Object() {True})
            '_customers.Add(New PTDType With {.id = 0, .name = ""})
            '_pallettypes.Add(New PTDType With {.id = 0, .name = ""})
            Using com As New SqlCommand("SELECT ID,FATHERNAME+' '+CODE+' '+NAME NAME FROM CUSTOMER WHERE LEFT(CODE,1)='8' AND FATHERNAME IS NOT NULL order by 2 asc", conn)
                conn.Open()
                Using reader As SqlDataReader = com.ExecuteReader
                    If reader.HasRows = True Then
                        While reader.Read()
                            _customers.Add(New PTDType With {.id = reader("ID"), .name = reader("NAME")})
                        End While
                    End If
                End Using
                conn.Close()
                With TryCast(DataGridView1.Columns(1), DataGridViewComboBoxColumn)
                    .DataSource = _customers
                    .ValueMember = "ID"
                    .DisplayMember = "NAME"
                End With
            End Using
            Using com As New SqlCommand("SELECT ID,description+' '+cast(width as varchar(10))+' '+cast(length as varchar(10))+' '+cast(height as varchar(10)) NAME FROM PKRTBL_PALLETTYPES order by 2 asc", conn)
                conn.Open()
                Using reader As SqlDataReader = com.ExecuteReader
                    If reader.HasRows = True Then
                        While reader.Read()
                            _pallettypes.Add(New PTDType With {.id = reader("ID"), .name = reader("NAME")})
                        End While
                    End If
                End Using
                conn.Close()
                With TryCast(DataGridView1.Columns(2), DataGridViewComboBoxColumn)
                    .DataSource = _pallettypes
                    .ValueMember = "ID"
                    .DisplayMember = "NAME"
                End With
            End Using
            LockUIAccess(Me)
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            loaded = True
            AdvancedDataGridViewSearchToolBar1.SetColumns(DataGridView1.Columns)
        End Try
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            PKRTBL_CUSTOMERTableAdapter.Update(Me.TESTFINALDataSet.PKRTBL_CUSTOMER)
        Catch ex As Exception
            If ex.Message.Contains("Cannot insert the value NULL into column") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Πρέπει να συμπληρώσετε όλες τις πληροφορίες!", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            ElseIf ex.Message.Contains("Violation of UNIQUE KEY constraint") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Ένας πελάτης μπορεί να έχει έναν τύπο παλέτας προεπιλεγμένο", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            Else
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End If
        End Try
    End Sub

    Private Sub Customer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim D As DataTable = Me.TESTFINALDataSet.PKRTBL_CUSTOMER.GetChanges
        If Not IsNothing(D) AndAlso D.Rows.Count > 0 Then
            Dim result1 As DialogResult = MessageBox.Show("Υπάρχουν αλλαγές που δεν έχουν αποθηκευτεί. Είστε σίγουροι;",
                                              "Εκκρεμούν αλλαγές",
                                              MessageBoxButtons.YesNo)
            If result1 = DialogResult.No Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub PKRTBLCUSTOMERBindingSource_ListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles PKRTBLCUSTOMERBindingSource.ListChanged
        If loaded And (e.ListChangedType = System.ComponentModel.ListChangedType.ItemAdded Or e.ListChangedType = System.ComponentModel.ListChangedType.ItemChanged _
            Or e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted) Then
            Button2.Visible = True
        End If
    End Sub

    Private Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar1.Search
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = DataGridView1.CurrentCell.ColumnIndex + 1 >= DataGridView1.ColumnCount
            Dim endrow As Boolean = DataGridView1.CurrentCell.RowIndex + 1 >= DataGridView1.RowCount
            If endcol AndAlso endrow Then
                startColumn = DataGridView1.CurrentCell.ColumnIndex
                startRow = DataGridView1.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, DataGridView1.CurrentCell.ColumnIndex + 1)
                startRow = DataGridView1.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If
        Dim c As DataGridViewCell = DataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = DataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            DataGridView1.CurrentCell = c
        End If
    End Sub

    Private Sub DataGridView1_SortStringChanged(sender As Object, e As EventArgs) Handles DataGridView1.SortStringChanged
        PKRTBLCUSTOMERBindingSource.Sort = DataGridView1.SortString
        'textBox_sort.Text = bindingSource_main.Sort
    End Sub

    Private Sub DataGridView1_FilterStringChanged(sender As Object, e As EventArgs) Handles DataGridView1.FilterStringChanged
        PKRTBLCUSTOMERBindingSource.Filter = DataGridView1.FilterString

        'textBox_filter.Text = bindingSource_main.Filter
    End Sub

    Dim PTColumns As New List(Of String) From {"PLTEMPLATE", "OLTEMPLATE", "CLTEMPLATE", "SLTEMPLATE"}

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 AndAlso PTColumns.Contains(DataGridView1.Columns(e.ColumnIndex).Name) Then
            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = OpenFileDialog1.FileName
            End If
        End If
    End Sub

    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError

    End Sub
End Class