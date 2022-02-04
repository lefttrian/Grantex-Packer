Public Class datafrommantisfrm

    Dim data As New DataTable()
    Dim data1 As New DataTable()
    Dim type As Boolean

    Public Sub New(ByVal dt As DataTable, dt1 As DataTable, Optional t As Boolean = False)
        data = dt
        data1 = dt1
        type = t
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub datafrommantisfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.DataSource = data
        With DataGridView1
            .Columns("FTRID").Visible = False
            .Columns("stlid").Visible = False
            .Columns("iteid").Visible = False

            .Columns("newQuant").HeaderText = "Υπόλοιπο μετά τη μεταφορά"
        End With
        DataGridView2.DataSource = data1
        With DataGridView2
            .Columns("FTRID").Visible = False
            .Columns("stlid").Visible = False
            .Columns("iteid").Visible = False
            .Columns("palletid").Visible = False


        End With
        'If type Then 'ΑΝΑΝΕΩΣΗ ΔΕΔΟΜΕΝΩΝ
        '    For Each r As DataGridViewRow In DataGridView2.Rows
        '        For Each p As pallettemplate In Form1.FlowLayoutPanel1.Controls
        '            If p.locationID = r.Cells("locationID").Value Then
        '                For Each d As DataGridViewRow In p.pallettemplatedatagrid.Rows
        '                    If d.Cells("iteid").Value = r.Cells("iteid").Value And d.Cells("QUANT").Value = r.Cells("lsumqty").Value And d.Cells("frommantis").Value = 1 And p.locationID = r.Cells("locid").Value Then
        '                        r.DefaultCellStyle.BackColor = Color.Green
        '                    ElseIf d.Cells("iteid").Value = r.Cells("iteid").Value And d.Cells("QUANT").Value <> r.Cells("lsumqty").Value And d.Cells("frommantis").Value = 1 And p.locationID = r.Cells("locid").Value Then
        '                        r.DefaultCellStyle.BackColor = Color.Khaki
        '                    Else
        '                        r.DefaultCellStyle.BackColor = Color.Yellow
        '                    End If
        '                Next
        '            End If
        '        Next
        '    Next
        'End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Dispose()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Cursor.Current = ExtCursor1.Cursor
        Try
            ProgressBar1.Maximum = DataGridView2.Rows.Count
            ProgressBar1.Value = 0
            Dim err As Boolean = False
            Dim list As New List(Of Integer)
            For i As Integer = 0 To DataGridView2.Rows.Count - 1
                If type AndAlso (DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.Green) Then
                    Continue For
                End If
                list.Add(DataGridView2.Rows(i).Cells("palletid").Value)
                reportlbl.Text = "Εισαγωγή είδους " + DataGridView2.Rows(i).Cells("ΠΕΡΙΓΡΑΦΗ").Value + " * " + DataGridView2.Rows(i).Cells("Ποσότητα").Value.ToString + " στη παλέτα " + DataGridView2.Rows(i).Cells("Παλέτα").Value
                ' Dim success = Form1.pallet_exchange(1, DataGridView2.Rows(i).Cells("palletid").Value, DataGridView2.Rows(i).Cells("Ποσότητα").Value, DataGridView2.Rows(i), frommantis:=True, dontupdate:=True)

                'If success > 0 Then
                '    DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.Green
                'Else
                '    DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.Red
                '    err = True
                'End If
                ProgressBar1.Value = ProgressBar1.Value + 1

            Next
            If err Then
                reportlbl.Text = "Ολοκληρώθηκε με σφάλματα."
                reportlbl.ForeColor = Color.Red
            Else
                reportlbl.Text = "Ολοκληρώθηκε επιτυχώς."
                reportlbl.ForeColor = Color.Green
                Button3.Enabled = False
            End If
            MsgBox(String.Join(",", list.Distinct.ToList))
            Form1.populate_pallets(String.Join(",", list.Distinct.ToList))
            Form1.datagridview1_refresh()
            Form1.orderdgv_refresh()
            Form1.change_frommantis()
        Catch ex As Exception

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub
End Class