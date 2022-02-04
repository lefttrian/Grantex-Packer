Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Reflection

Public Class PalletTypes

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim loaded As Boolean = False
    Private Sub EditDefaults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'TESTFINALDataSet.PKRTBL_PALLETTYPES' table. You can move, or remove it, as needed.
        Try
            GetType(Zuby.ADGV.AdvancedDataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, DataGridView1, New Object() {True})
            Me.PKRTBL_PALLETTYPESTableAdapter.Fill(Me.TESTFINALDataSet.PKRTBL_PALLETTYPES)
            LockUIAccess(Me)
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            conn.Close()
            loaded = True
            AdvancedDataGridViewSearchToolBar1.SetColumns(DataGridView1.Columns)
        End Try
    End Sub

    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            PKRTBL_PALLETTYPESTableAdapter.Update(Me.TESTFINALDataSet.PKRTBL_PALLETTYPES)
        Catch ex As Exception
            If ex.Message.Contains("Cannot insert the value NULL into column") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Πρέπει να συμπληρώσετε όλες τις πληροφορίες!", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            ElseIf ex.Message.Contains("Violation of UNIQUE KEY constraint") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Η ονομασία της παλέτας πρέπει να είναι μοναδική!", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            ElseIf ex.Message.Contains("FK_PKRTBL_PALLETTYPESDEFAULTS_PKRTBL_PALLETTYPES") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Ο τύπος παλέτας έχει συνδεδεμένες προεπιλεγμένες ποσότητες οι οποίες πρέπει να διαγραφούν πρώτα.", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            ElseIf ex.Message.Contains("IX_PKRTBL_PALLETTYPES_1") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Οι διαστάσεις έχουν ήδη εισαχθεί σε άλλη παλέτα!", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            ElseIf ex.Message.Contains("The DELETE statement conflicted") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Ο τύπος παλέτας έχει αντιστοιχιστεί με παλέτες. Δεν επιτρέπεται η διαγραφή του.", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            Else
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End If
        Finally
            Form1.populate_pallettypes()
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try


            If e.RowIndex >= 0 And DataGridView1.Columns(e.ColumnIndex).Name = "buttonclmn" And Not DataGridView1.Rows(e.RowIndex).IsNewRow Then

                If e.RowIndex > Me.TESTFINALDataSet.PKRTBL_PALLETTYPES.Rows.Count - 1 OrElse Me.TESTFINALDataSet.PKRTBL_PALLETTYPES.Rows(e.RowIndex).Item("ID") < 0 Then
                    Throw New Exception("Πρέπει να αποθηκεύσετε τον νέο τύπο προτού αλλάξετε τις προεπιλεγμένες ποσότητες του!")
                End If
                Using FRM As New PalletTypesDefaults(Me.TESTFINALDataSet.PKRTBL_PALLETTYPES.Rows(e.RowIndex).Item("ID"))
                    FRM.ShowDialog()
                End Using
            End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub PalletTypes_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim D As DataTable = Me.TESTFINALDataSet.PKRTBL_PALLETTYPES.GetChanges
        If Not IsNothing(D) AndAlso D.Rows.Count > 0 Then
            Dim result1 As DialogResult = MessageBox.Show("Υπάρχουν αλλαγές που δεν έχουν αποθηκευτεί. Είστε σίγουροι;",
                                              "Εκκρεμούν αλλαγές",
                                              MessageBoxButtons.YesNo)
            If result1 = DialogResult.No Then
                e.Cancel = True
            End If


        End If
    End Sub

    Private Sub PKRTBLPALLETTYPESBindingSource_DataError(sender As Object, e As BindingManagerDataErrorEventArgs) Handles PKRTBLPALLETTYPESBindingSource.DataError

        'Using errfrm As New errormsgbox(e.Exception.StackTrace.ToString, e.Exception.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

    End Sub


    Private Sub PKRTBLPALLETTYPESBindingSource_CurrentChanged(sender As Object, e As EventArgs) Handles PKRTBLPALLETTYPESBindingSource.CurrentChanged

    End Sub


    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        Try
            If (DataGridView1.Columns(e.ColumnIndex).Name = "HEIGHT" Or DataGridView1.Columns(e.ColumnIndex).Name = "WIDTH" Or
            DataGridView1.Columns(e.ColumnIndex).Name = "LENGTH") And Not IsNumeric(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                Throw New Exception("Επιτρέπονται μόνο αριθμητικές τιμές")
            End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub PKRTBLPALLETTYPESBindingSource_ListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles PKRTBLPALLETTYPESBindingSource.ListChanged
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
        PKRTBLPALLETTYPESBindingSource.Sort = DataGridView1.SortString
        'textBox_sort.Text = bindingSource_main.Sort
    End Sub

    Private Sub DataGridView1_FilterStringChanged(sender As Object, e As EventArgs) Handles DataGridView1.FilterStringChanged
        PKRTBLPALLETTYPESBindingSource.Filter = DataGridView1.FilterString

        'textBox_filter.Text = bindingSource_main.Filter
    End Sub

    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        Try

            Throw New Exception("Λάθος μορφή δεδομένων!")

        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub


End Class