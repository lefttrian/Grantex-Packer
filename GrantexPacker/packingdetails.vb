Imports System.Configuration
Imports System.Data.SqlClient

Public Class packingdetails
    Dim PALLET As Integer = 0
    Dim pname As String = ""
    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Public Sub New(ByVal i As Integer, name As String)
        PALLET = i
        pname = name
        InitializeComponent()
    End Sub

    Private Sub packingdetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cursor.Current = ExtCursor1.Cursor
        Dim CMD As String = "select pl.id,pl.quantity 'Αριθμός σετ',mu.edicode 'Σετ των',m.code 'Είδος',m.description 'Περιγραφή',p.packs '# πακέτων',p.quan_perpack 'Ποσότητα ανά πακέτο',p.pack 'Α/A πακέτου' from tbl_palletlines pl left join material m on m.id=pl.iteid left join mesunit mu on mu.codeid=m.mu1 left join tbl_packerplinespacking p on p.plineid=pl.id where pl.palletid=" + PALLET.ToString + " order by m.code,pl.id"
        Using comm As New SqlCommand(CMD, conn)
            conn.Open()
            Using dt As New DataTable
                Using reader As SqlDataReader = comm.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                    DataGridView1.DataSource = dt
                    Dim View As DataView = New DataView(dt)
                    Dim distinctValues As DataTable = View.ToTable(True, "Είδος")


                    distinctValues.Columns.Add("Σύνολο σε κουτιά")
                    DataGridView2.DataSource = distinctValues

                    dgv2_refresh()
                    DataGridView2.ReadOnly = True
                    For Each c As DataGridViewColumn In DataGridView1.Columns
                        If c.Name = "# πακέτων" Or c.Name = "Ποσότητα ανά πακέτο" Or c.Name = "Α/A πακέτου" Then
                            c.ReadOnly = False
                        Else
                            c.ReadOnly = True
                        End If
                        If c.Name = "id" Then
                            c.Visible = False
                        End If
                    Next
                End Using
            End Using
        End Using
        Label1.Text = "Πακετοποίηση παλέτας " + pname
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Cursor.Current = ExtCursor1.Cursor
        Dim plineids As New List(Of Integer)
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            plineids.Add(DataGridView1.Rows(i).Cells(0).Value)
            plineids = plineids.Distinct.ToList
        Next
        Dim txt As String = String.Join(",", plineids.ToArray())
        Dim success As Integer = 0
        Using com As New SqlCommand("DELETE FROM TBL_PACKERPLINESPACKING WHERE plineid in (" + txt + ")", updconn)
            updconn.Open()
            success = com.ExecuteNonQuery()
            updconn.Close()
            If success < 0 Then
                Return
            End If
        End Using
        success = 0
        Dim issue As Boolean = False
        For Each l As DataGridViewRow In DataGridView1.Rows
            If IsDBNull(l.Cells("# πακέτων").Value) And IsDBNull(l.Cells("Ποσότητα ανά πακέτο").Value) And IsDBNull(l.Cells("Α/A πακέτου").Value) Then
                Continue For
            End If
            Dim cmd As String = "INSERT INTO TBL_PACKERPLINESPACKING (PLINEID,PACKS,QUAN_PERPACK,PACK) VALUES (@PLINEID,@PACKS,@QUAN_PERPACK,@PACK)"
            Using com As New SqlCommand(cmd, updconn)
                com.Parameters.Add("@PLINEID", SqlDbType.Int).Value = l.Cells(0).Value
                com.Parameters.Add("@PACKS", SqlDbType.Float).Value = l.Cells("# πακέτων").Value
                com.Parameters.Add("@QUAN_PERPACK", SqlDbType.Float).Value = l.Cells("Ποσότητα ανά πακέτο").Value
                com.Parameters.Add("@PACK", SqlDbType.Float).Value = l.Cells("Α/A πακέτου").Value
                updconn.Open()
                success = com.ExecuteNonQuery()
                updconn.Close()
                If success > 0 Then
                    l.DefaultCellStyle.BackColor = Color.Green
                Else
                    l.DefaultCellStyle.BackColor = Color.Red
                    issue = True
                End If
            End Using
        Next
        If issue Then
            Label2.ForeColor = Color.Red
            Label2.Text = "Υπήρξε κάποιο πρόβλημα!"
            Label2.Visible = True
            Button2.Text = "Κλείσιμο"
        Else
            Label2.ForeColor = Color.Green
            Label2.Text = "Επιτυχής εισαγωγή."
            Label2.Visible = True
            Button2.Text = "Κλείσιμο"
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub dgv2_refresh()
        Dim boundSet As DataTable = DataGridView2.DataSource
        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            Dim sum As Double = 0
            For j As Integer = 0 To DataGridView1.Rows.Count - 1
                With DataGridView1
                    If .Rows(j).Cells("Είδος").Value = DataGridView2.Rows(i).Cells("Είδος").Value Then
                        Dim v1 As Double
                        If IsDBNull(.Rows(j).Cells("# πακέτων").Value) Then
                            v1 = 0
                        Else
                            v1 = .Rows(j).Cells("# πακέτων").Value
                        End If
                        Dim v2 As Double
                        If IsDBNull(.Rows(j).Cells("Ποσότητα ανά πακέτο").Value) Then
                            v2 = 0
                        Else
                            v2 = .Rows(j).Cells("Ποσότητα ανά πακέτο").Value
                        End If

                        sum = sum + (v1 * v2)
                    End If
                End With
            Next
            boundSet.Rows(i).Item(1) = sum

        Next
        boundSet.AcceptChanges()

        With DataGridView2
            For j As Integer = 0 To DataGridView2.Rows.Count - 1
                Dim val As Double = 0
                For i As Integer = 0 To DataGridView1.Rows.Count - 1
                    If .Rows(j).Cells(0).Value = DataGridView1.Rows(i).Cells("Είδος").Value Then
                        val = DataGridView1.Rows(i).Cells("Αριθμός σετ").Value * DataGridView1.Rows(i).Cells("Σετ των").Value
                        Exit For
                    End If
                Next
                If .Rows(j).Cells(1).Value = val And .Rows(j).Cells(1).Value > 0 Then
                    .Rows(j).DefaultCellStyle.BackColor = Color.Green
                ElseIf .Rows(j).Cells(1).Value > val Then
                    .Rows(j).DefaultCellStyle.BackColor = Color.Red

                End If

            Next
            .ClearSelection()
        End With
    End Sub


    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As DataGridViewCellEventArgs) _
                                           Handles DataGridView1.CellContentClick
        Dim senderGrid = DirectCast(sender, DataGridView)

        If TypeOf senderGrid.Columns(e.ColumnIndex) Is DataGridViewLinkColumn AndAlso
           e.RowIndex >= 0 Then

            If senderGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = "Προσθήκη" Then
                'senderGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.Value = "Αφαίρεση"
                Dim boundSet As DataTable = DataGridView1.DataSource
                Dim newRow As DataRow = boundSet.NewRow
                With newRow
                    .Item(0) = boundSet.Rows(e.RowIndex).Item(0)
                    .Item(1) = boundSet.Rows(e.RowIndex).Item(1)
                    .Item(2) = boundSet.Rows(e.RowIndex).Item(2)
                    .Item(3) = boundSet.Rows(e.RowIndex).Item(3)
                    .Item(boundSet.Columns.Count - 1) = "Αφαίρεση"
                End With
                boundSet.Rows.Add(newRow)
                boundSet.AcceptChanges()


            Else
                DataGridView1.Rows.RemoveAt(e.RowIndex)
                dgv2_refresh()
            End If
        End If

    End Sub

    Private Sub DataGridView1_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded

    End Sub

    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        If Not DataGridView1.Columns.Contains("Προσθήκη πακέτου") Then


            Dim btn As New DataGridViewLinkColumn
            btn.HeaderText = "Προσθήκη πακέτου"
            btn.Text = "Προσθήκη πακέτου"
            btn.Name = "btn"
            Dim boundSet As DataTable = DataGridView1.DataSource
            boundSet.Columns.Add("Προσθήκη πακέτου")

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                DataGridView1.Rows(i).Cells("Προσθήκη πακέτου").Value = "Προσθήκη"
                boundSet.Rows(i).Item("Προσθήκη πακέτου") = "Προσθήκη"
                If i + 1 <= DataGridView1.Rows.Count - 1 Then

                    If DataGridView1.Rows(i).Cells("id").Value = DataGridView1.Rows(i + 1).Cells("id").Value Then
                        DataGridView1.Rows(i).Cells("Προσθήκη πακέτου").Value = "Αφαίρεση"
                    Else
                        DataGridView1.Rows(i).Cells("Προσθήκη πακέτου").Value = "Προσθήκη"
                    End If
                End If
                If i = DataGridView1.Rows.Count - 1 Then
                    If i <> 0 AndAlso DataGridView1.Rows(i).Cells("id").Value = DataGridView1.Rows(i - 1).Cells("id").Value Then
                        DataGridView1.Rows(i).Cells("Προσθήκη πακέτου").Value = "Αφαίρεση"

                    Else
                        DataGridView1.Rows(i).Cells("Προσθήκη πακέτου").Value = "Προσθήκη"
                    End If
                End If
            Next
            boundSet.AcceptChanges()
            DataGridView1.Columns.Remove("Προσθήκη πακέτου")
            btn.DataPropertyName = "Προσθήκη πακέτου"
            DataGridView1.Columns.Insert(DataGridView1.Columns.Count, btn)
            DataGridView1.Columns(DataGridView1.Columns.Count - 1).Name = "Προσθήκη πακέτου"
            DataGridView1.Refresh()
        End If
    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit

        With DataGridView1
            If .Columns(e.ColumnIndex).Name = "# πακέτων" Or .Columns(e.ColumnIndex).Name = "Ποσότητα ανά πακέτο" Then
                If .Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString Like "#" Or .Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString Like "##" _
Or .Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString Like "###" Or .Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString Like "####" Or .Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString Like "#####" Then

                    Dim sum As Double = 0
                    For i As Integer = 0 To .Rows.Count - 1

                        If .Rows(i).Cells("id").Value = .Rows(e.RowIndex).Cells("id").Value Then
                            Dim v1 As Double
                            If IsDBNull(.Rows(i).Cells("# πακέτων").Value) Then
                                v1 = 0
                            Else
                                v1 = .Rows(i).Cells("# πακέτων").Value
                            End If
                            Dim v2 As Double
                            If IsDBNull(.Rows(i).Cells("Ποσότητα ανά πακέτο").Value) Then
                                v2 = 0
                            Else
                                v2 = .Rows(i).Cells("Ποσότητα ανά πακέτο").Value
                            End If

                            sum = sum + (v1 * v2)
                        End If
                    Next
                    If sum > .Rows(e.RowIndex).Cells("Αριθμός σετ").Value * .Rows(e.RowIndex).Cells("Σετ των").Value Then
                        .Rows(e.RowIndex).Cells(e.ColumnIndex).Value = oldvalue
                    End If
                    DataGridView2.ReadOnly = False
                    dgv2_refresh()

                    DataGridView2.ReadOnly = True
                    DataGridView2.Refresh()
                End If
            End If
        End With
    End Sub
    Dim oldvalue As Double = 0
    Private Sub DataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit
        With DataGridView1
            If .Columns(e.ColumnIndex).Name = "# πακέτων" Or .Columns(e.ColumnIndex).Name = "Ποσότητα ανά πακέτο" Then
                If Not IsDBNull(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                    oldvalue = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                Else
                    oldvalue = 0
                End If
            End If
        End With
    End Sub

    Private Sub DataGridView2_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView2.RowPostPaint

    End Sub

    Private Sub DataGridView2_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellValueChanged

    End Sub
End Class