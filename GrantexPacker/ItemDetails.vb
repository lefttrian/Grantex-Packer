Imports System.Data.SqlClient
Imports System.Data.Objects
Imports System.Text.RegularExpressions
Imports System.Configuration

Public Class ItemDetails
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim iteid As String
    Public Sub New(ByVal i As String)
        iteid = i

        InitializeComponent()
    End Sub

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
                Dim ctrl As Control = Me.Controls(i)
                ctrl.Dispose()
            Next
            conn.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cursor.Current = ExtCursor1.Cursor
        Try
            Dim cmd As String = "SELECT MATERIAL.CODE,MATERIAL.subcode1 AS 'ΝΕΟΣ ΚΩΔΙΚΟΣ',isnull(MATERIAL.w_search2,'') as 'info', isnull(MATERIAL.subcode2,'') AS 'ΠΑΛΙΟΣ ΚΩΔΙΚΟΣ', 
MATERIAL.DESCRIPTION AS 'ΠΕΡΙΓΡΑΦΗ',material.m_barcode as 'Barcode',MATERIAL.FLDSTRING1 AS 'ΕΙΔ.ΧΑΡ.1',MATERIAL.FLDSTRING2 AS 'ΕΙΔ.ΧΑΡ.2',MATERIAL.M_INDEX AS '1ο WVA/ORGNAL', STD=case when MATERIAL.m_standard=1 then 'Ναι' else 'Όχι' end,ΧΡΗΣΗ=case when MATERIAL.exclusiv=1 then 'Ναι' else 'Όχι' end, MATERIAL.M_PARTNO AS 'PART NUMBER',material.factorycode as 'ΚΩΔ ΕΡΓΟΣΤ', MNF.descr AS 'ΜΑΡΚΑ', igs.DESCR AS 'ΠΟΙΟΤΗΤΑ', ict.DESCR AS 'ΠΡΟΜΗΘΕΥΤΗΣ', MATERIAL.FLDFLOAT1 AS 'OS', material.w_info as 'ΕΠΙΛΕΟΝ ΠΛΗΡΟΦΟΡΙΕΣ',MU.DESCR AS 'ΜΟΝΑΔΑ ΜΕΤΡΗΣΗΣ',
mtr.id,(CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY1) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR.DESCRIPTION+SPACE(1) ) AS 'ΗΜΙΕΤΟΙΜΟ 1', mtr2.id,(CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY2) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR2.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR2.DESCRIPTION+SPACE(1) ) AS 'HMIETOIMO 2', mtr3.id,(CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY3) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR3.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR3.DESCRIPTION+SPACE(1) ) AS 'HMIETOIMO 3', 
mtr4.id,(CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY4) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR4.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR4.DESCRIPTION+SPACE(1) )AS 'ΕΛΑΤΗΡΙΟ 1',mtr5.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY5) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR5.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR5.DESCRIPTION+SPACE(1) ) AS 'ΕΛΑΤΗΡΙΟ 2', mtr6.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY6) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR6.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR6.DESCRIPTION+SPACE(1) ) AS 'ΚΙΤ 1',
mtr7.id,(CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY7) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR7.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR7.DESCRIPTION+SPACE(1) ) AS 'KIT 2',mtr8.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY8) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR8.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR8.DESCRIPTION+SPACE(1) ) AS 'KIT 3',mtr9.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY9) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR9.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR9.DESCRIPTION+SPACE(1) ) AS 'ΑΙΣΘΗΤΗΡΑΣ 1', 
mtr10.id,(CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY10) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR10.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR10.DESCRIPTION+SPACE(1) ) AS 'ΑΙΣΘΗΤΗΡΑΣ 2',mtr11.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY11) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR11.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR11.DESCRIPTION ) AS 'ΠΡΙΤΣΙΝΙ',mtr12.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY12) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR12.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR12.DESCRIPTION+SPACE(1) ) AS 'KOYTI 1',
mtr13.id,(CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY13) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR13.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR13.DESCRIPTION+SPACE(1) ) AS 'KOYTI 2',mtr14.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY14) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR14.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR14.DESCRIPTION+SPACE(1) ) AS 'ETIKETA 1',mtr15.id, (CONVERT(VARCHAR(3),MATERIAL.M_SPECQTY15) +'x'+SPACE(1)+CONVERT(VARCHAR(20),MTR15.SUBCODE1)+SPACE(1)+'-'+SPACE(1)+MTR15.DESCRIPTION+SPACE(1) )AS 'ETIKETA 2' 
FROM MATERIAL left join MANUFACTURER mnf on MNFID=mnf.CODEID left join ITEMGROUP2 igs on IGSID=igs.CODEID left join ITEMCATEGORY ict on ICTID=ict.CODEID LEFT JOIN MATERIAL MTR ON MATERIAL.M_SPECID1=MTR.ID LEFT JOIN MATERIAL MTR2 ON MATERIAL.M_SPECID2=MTR2.ID LEFT JOIN MATERIAL MTR3 ON MATERIAL.M_SPECID3=MTR3.ID LEFT JOIN MATERIAL MTR4 ON MATERIAL.M_SPECID4=MTR4.ID LEFT JOIN MATERIAL MTR5 ON MATERIAL.M_SPECID5=MTR5.ID LEFT JOIN MATERIAL MTR6 ON MATERIAL.M_SPECID6=MTR6.ID LEFT JOIN MATERIAL MTR7 ON MATERIAL.M_SPECID7=MTR7.ID LEFT JOIN MATERIAL MTR8 ON MATERIAL.M_SPECID8=MTR8.ID LEFT JOIN MATERIAL MTR9 ON MATERIAL.M_SPECID9=MTR9.ID LEFT JOIN MATERIAL MTR10 ON MATERIAL.M_SPECID10=MTR10.ID LEFT JOIN MATERIAL MTR11 ON MATERIAL.M_SPECID11=MTR11.ID LEFT JOIN MATERIAL MTR12 ON MATERIAL.M_SPECID12=MTR12.ID LEFT JOIN MATERIAL MTR13 ON MATERIAL.M_SPECID13=MTR13.ID LEFT JOIN MATERIAL MTR14 ON MATERIAL.M_SPECID14=MTR14.ID LEFT JOIN MATERIAL MTR15 ON MATERIAL.M_SPECID15=MTR15.ID LEFT JOIN MESUNIT MU ON MU.CODEID=MATERIAL.MU1
                    where material.id=" + iteid
            Using comm As New SqlCommand(cmd, conn)
                comm.CommandType = CommandType.Text
                conn.Open()
                Using dt = New DataTable()
                    Using reader As SqlDataReader = comm.ExecuteReader
                        dt.Load(reader)

                        conn.Close()

                        Using table As New DataTable()
                            Using table2 As New DataTable()
                                For i As Integer = 0 To dt.Rows.Count
                                    table2.Columns.Add(i.ToString)
                                    table.Columns.Add(i.ToString)
                                Next
                                table.Columns.Add()
                                Dim r As DataRow
                                Dim r2 As DataRow
                                Label1.Text = dt.Rows(0).Item("ΝΕΟΣ ΚΩΔΙΚΟΣ")
                                Label5.Text = dt.Rows(0).Item("CODE")
                                Label2.Text = dt.Rows(0).Item("ΠΕΡΙΓΡΑΦΗ")
                                Label3.Text = "Παλαιός κωδικός: " & dt.Rows(0).Item("ΠΑΛΙΟΣ ΚΩΔΙΚΟΣ")
                                Label4.Text = dt.Rows(0).Item("info")
                                r = table.NewRow()
                                For k As Integer = 19 To dt.Columns.Count - 1

                                    r(0) = dt.Columns(k).ColumnName

                                    If dt.Columns(k).ColumnName.Contains("id") Then
                                        r(1) = dt.Rows(0).Item(k)
                                        Continue For
                                    Else
                                        r(2) = dt.Rows(0).ItemArray(k)
                                    End If
                                    table.Rows.Add(r)
                                    r = table.NewRow()
                                Next
                                For k As Integer = 4 To 18
                                    r2 = table2.NewRow()
                                    r2(0) = dt.Columns(k).ColumnName
                                    r2(1) = dt.Rows(0).ItemArray(k)
                                    table2.Rows.Add(r2)
                                Next
                                DataGridView1.DataSource = table
                                DataGridView2.DataSource = table2
                                DataGridView1.Columns(1).Visible = False
                            End Using
                        End Using
                        Using column2 As DataGridViewColumn = DataGridView1.Columns(2)
                            column2.Width = 1000
                            DataGridView2.Columns(1).Width = 500
                            Cursor.Current = Cursors.Default
                        End Using
                    End Using
                End Using
            End Using
            LockUIAccess(Me)
        Catch ex As Exception

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs)

    End Sub

    Private Sub ΑντιγραφήToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑντιγραφήToolStripMenuItem.Click
        If ContextMenuStrip1.SourceControl.Name = "Label1" Then
            My.Computer.Clipboard.SetText(Label1.Text)
        Else
            My.Computer.Clipboard.SetText(Label5.Text)
        End If

    End Sub

    Private Sub ΑντιγραφήToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ΑντιγραφήToolStripMenuItem1.Click
        My.Computer.Clipboard.SetText(Label2.Text)
    End Sub

    Private Sub ΑντιγραφήToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ΑντιγραφήToolStripMenuItem2.Click
        My.Computer.Clipboard.SetText(Label3.Text)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Using frm = New WMSLocationStock(iteid, Label2.Text, Label1.Text, False, Label5.Text)
            frm.ShowDialog()
        End Using


    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Try
            If Not IsDBNull(DataGridView1.Rows(e.RowIndex).Cells(1).Value) Then
                Dim frm As New ItemDetails(DataGridView1.Rows(e.RowIndex).Cells(1).Value)
                frm.ShowDialog()
            End If
        Catch ex As Exception

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim l As New Dictionary(Of String, Integer)
        l.Add("stl.ITEID", iteid)
        l.Add("spcl.iteid", iteid)
        Dim f As New ItemPendingOrders(Cursor.Position.X, Cursor.Position.Y, integer_parameters_equal_:=l)
        f.Owner = Me
        f.ShowDialog()

    End Sub
End Class