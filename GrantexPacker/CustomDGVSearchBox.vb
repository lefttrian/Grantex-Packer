
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Globalization


Public Class CustomDGVSearchBox

    Public Event ButtonPressed()
    Dim custommode As Boolean = True
    Public Property Custom_mode As Boolean
        Set(value As Boolean)
            custommode = value
        End Set
        Get
            Return custommode
        End Get
    End Property

    Public Property parent_datagridview As AdvancedDataGridView
        Set(value As AdvancedDataGridView)
            dgv = value
        End Set
        Get
            Return dgv
        End Get
    End Property

    Dim custom_comm As String
    Public Property custom_command As String
        Set(value As String)
            custom_comm = value
        End Set
        Get
            Return custom_comm
        End Get
    End Property

    Dim parameters As Dictionary(Of String, String)

    Public Property custom_parameters As Dictionary(Of String, String)
        Set(value As Dictionary(Of String, String))
            parameters = value
        End Set
        Get
            Return parameters
        End Get
    End Property



    Dim dgv As AdvancedDataGridView

    Private Sub NoPaddingButton1_Click(sender As Object, e As EventArgs) Handles NoPaddingButton1.Click
        If custommode Then
            RaiseEvent ButtonPressed()
        Else
            continue_click()
        End If
    End Sub

    Dim tempdt As New DataTable()

    Public Sub continue_click()
        dgv.ClearSelection()
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            Return
        End If
        Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString
        Dim conn As New SqlConnection(connString)
        If IsNothing(dgv) Then
            Throw New Exception("Δεν έχει οριστεί parent_datagridview")
        End If
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        Dim endcol As Boolean = dgv.CurrentCell.ColumnIndex + 1 >= dgv.ColumnCount
        Dim endrow As Boolean = dgv.CurrentCell.RowIndex + 1 >= dgv.RowCount
        If endcol AndAlso endrow Then
            startColumn = dgv.CurrentCell.ColumnIndex
            startRow = dgv.CurrentCell.RowIndex
        Else
            startColumn = If(endcol, 0, dgv.CurrentCell.ColumnIndex + 1)
            startRow = dgv.CurrentCell.RowIndex + (If(endcol, 1, 0))
        End If
        Dim c As DataGridViewCell = dgv.FindCell(TextBox1.Text, Nothing, startRow, startColumn, False, True)
        If c Is Nothing AndAlso restartsearch Then
            c = dgv.FindCell(TextBox1.Text, Nothing, 0, 0, True, True)
        End If
        If c IsNot Nothing Then
            dgv.CurrentCell = c
        ElseIf custom_comm IsNot Nothing Then
            If Not text_searched Then
                Dim scom As String = custom_comm
                scom = scom.Replace("textbox", "%" + TextBox1.Text + "%")
                scom = scom.Replace("*", "%")
                If Not IsNothing(parameters) Then
                    For Each k As KeyValuePair(Of String, String) In parameters
                        scom = scom.Replace(k.Key, k.Value)
                        's.Parameters.AddWithValue(k.Key, k.Value)
                    Next
                End If
                tempdt.Clear()
                Using s As New SqlCommand(scom, conn)
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        tempdt.Load(reader)
                        text_searched = True
                    End Using
                    conn.Close()
                    'ΤΑ ΕΠΙΣΤΡΕΦΟΜΕΝΑ ΠΕΔΙΑ ΠΡΕΠΕΙ ΝΑ ΕΧΟΥΝ ΙΔΙΑ ΟΝΟΜΑΤΑ ΜΕ ΚΑΠΟΙΕΣ ΣΤΗΛΕΣ ΤΟΥ PARENT DATAGRIDVIEW ΚΑΙ ΤΟ INDEX ΤΗΣ ΣΤΗΛΗΣ ΝΑ ΣΧΕΤΙΖΕΤΑΙ ΜΕ ΤΗ ΣΗΜΑΣΙΑ Ν=1 ΣΗΜΑΝΤΙΚΟΤΕΡΟ
                End Using
            End If
            Dim lst As New List(Of Integer)
            If tempdt.Rows.Count > 0 Then
                For Each dr As DataRow In tempdt.Rows
                    For Each dc As DataColumn In tempdt.Columns
                        For i = 0 To dgv.Rows.Count - 1
                            If dgv.Rows(i).Cells(dc.ColumnName).Value = dr.Item(dc.ColumnName) Then
                                lst.Add(i)
                            End If
                        Next
                    Next
                Next
            End If
            Dim groups = lst.GroupBy(Function(x) x)
            If groups.Count() > 0 Then
                Dim Largestcount = (From count In groups
                                    Order By groups.Count Descending
                                    Select count).First()
                For Each grp In groups
                    If grp.Count = Largestcount.Count Then
                        dgv.Rows(grp(0)).Cells(0).Selected = True
                    End If
                Next
            End If
        End If
    End Sub

    Dim text_searched As Boolean = False

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        text_searched = False
        tempdt.Clear()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
            e.SuppressKeyPress = True
            NoPaddingButton1.PerformClick()
        End If
    End Sub

    'Public Class CustomDGVSearchBoxButtonPressedEventArgs
    '    Inherits EventArgs

    '    Public Property parameters As New Dictionary(Of String, String)

    '    Public Sub New(params As Dictionary(Of String, String))
    '        parameters = params
    '    End Sub
    'End Class
    'Private Function NoPaddingButton1_Click(sender As Object, e As EventArgs) Handles NoPaddingButton1.Click
    '    Try
    '        For Each r As DataGridViewRow In dgv.Rows
    '            For Each c As DataGridViewCell In r.Cells
    '                If c.Value.ToString.Contains(TextBox1.Text) Then
    '                    lst.Add(r.Index, c.ColumnIndex)
    '                End If
    '            Next
    '        Next
    '        Return lst
    '    Catch ex As Exception
    '        Return lst
    '        Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
    '    End Try
    'End Function
End Class
