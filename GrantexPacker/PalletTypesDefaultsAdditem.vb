Imports System.Configuration
Imports System.Data.SqlClient


Public Class PalletTypesDefaultsAdditem
    Dim PTName As String
    Dim PTId As Integer
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Private _PTDType As New List(Of PTDType)
    Private _MaterialItem As New List(Of PTDType)
    Dim itemtype As String
    Dim loaded As Boolean = False

    Public Sub New(ByVal i As Integer, it As String)

        ' This call is required by the designer.
        InitializeComponent()
        PTId = i
        itemtype = it
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub PalletTypesDefaultsAdditem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            _PTDType.Add(New PTDType With {.id = 0, .name = ""})
            Using com As New SqlCommand("SELECT DISTINCT DESCRIPTION FROM PKRTBL_PALLETTYPES PT WHERE PT.ID=" + PTId.ToString, conn)
                conn.Open()
                PTLBL.Text = com.ExecuteScalar()
                conn.Close()
            End Using

            Using com As New SqlCommand("SELECT ID,DESCRIPTION_GR FROM PKRTBL_PTDTYPES", conn)
                conn.Open()
                Using reader As SqlDataReader = com.ExecuteReader
                    If reader.HasRows = True Then
                        While reader.Read()
                            _PTDType.Add(New PTDType With {.id = reader("ID"), .name = reader("DESCRIPTION_GR")})
                        End While
                    End If

                End Using
                conn.Close()

                ComboBox1.DataSource = _PTDType
                ComboBox1.DisplayMember = "name"
                ComboBox1.ValueMember = "id"

            End Using

        Catch EX As Exception
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            loaded = True
        End Try

    End Sub
    Dim selection As Integer = -1
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If loaded Then
            _MaterialItem.Clear()
            _MaterialItem.Add(New PTDType With {.id = 0, .name = ""})
            Try
                selection = _PTDType(ComboBox1.SelectedIndex).id
                ComboBox2.DataSource = Nothing
                If ComboBox2.Enabled = False Then
                    ComboBox2.Enabled = True
                End If
                If selection <> 3 Then
                    Label5.Visible = False
                End If
                Dim addcomm1 As String = ""
                Dim addcomm2 As String = ""

                If selection = 4 Then
                    Using com As New SqlCommand("SELECT ID,SUBCODE1+' '+DESCRIPTION d FROM MATERIAL M WHERE M.ID IN (SELECT ITEID FROM SPECIFICATIONLINES
 WHERE SPCID IN (SELECT s.ID FROM SPECIFICATION S INNER JOIN MATERIAL M ON M.ID=S.ITEID INNER JOIN FLDCUSTBL1 F ON F.CODEID=M.FLTID1 WHERE F.PKRTBL_ITEMTYPESID=" + itemtype + ")) AND ISACTIVE=1  ORDER BY 2 ASC", conn)
                        conn.Open()
                        Using reader As SqlDataReader = com.ExecuteReader
                            If reader.HasRows = True Then
                                While reader.Read()
                                    _MaterialItem.Add(New PTDType With {.id = reader("ID"), .name = reader("d")})
                                End While
                            End If

                        End Using
                        conn.Close()
                        ComboBox2.DataSource = _MaterialItem
                        ComboBox2.DisplayMember = "name"
                        ComboBox2.ValueMember = "id"
                    End Using
                ElseIf selection = 1 Then
                    Using com As New SqlCommand("SELECT ID,SUBCODE1+' '+DESCRIPTION d FROM MATERIAL M INNER JOIN FLDCUSTBL1 F ON M.FLTID1 =F.CODEID WHERE F.PKRTBL_ITEMTYPESID=" + itemtype + " AND ISACTIVE=1 ORDER BY 2 ASC", conn)
                        conn.Open()
                        Using dt As New DataTable()
                            Using reader As SqlDataReader = com.ExecuteReader
                                'If reader.HasRows = True Then
                                '    While reader.Read()
                                '        _MaterialItem.Add(New PTDType With {.id = reader("ID"), .name = reader("d")})
                                '    End While
                                'End If
                                dt.Load(reader)
                                If dt.Rows.Count > 0 Then
                                    For i As Integer = 0 To dt.Rows.Count - 1
                                        _MaterialItem.Add(New PTDType With {.id = dt.Rows(i).Item("ID"), .name = dt.Rows(i).Item("d")})
                                    Next
                                End If
                            End Using
                        End Using
                        conn.Close()
                        ComboBox2.DataSource = _MaterialItem
                        ComboBox2.DisplayMember = "name"
                        ComboBox2.ValueMember = "id"
                    End Using
                ElseIf selection = 2 Then

                    Using com As New SqlCommand("SELECT codeid,CAST(CODEID AS VARCHAR(10))+' '+descr DESCR FROM FLDCUSTBL1 WHERE PKRTBL_ITEMTYPESID=" + itemtype + "  ORDER BY 2 ASC", conn)
                        conn.Open()
                        Using reader As SqlDataReader = com.ExecuteReader
                            If reader.HasRows = True Then
                                While reader.Read()
                                    _MaterialItem.Add(New PTDType With {.id = reader("codeid"), .name = reader("descr")})
                                End While
                            End If

                        End Using
                        conn.Close()
                        ComboBox2.DataSource = _MaterialItem
                        ComboBox2.DisplayMember = "name"
                        ComboBox2.ValueMember = "id"
                    End Using
                ElseIf selection = 3 Then
                    Label5.Visible = True

                ElseIf selection = 5 Then
                    Using com As New SqlCommand("SELECT I.codeid,CAST(I.CODEID AS VARCHAR(10))+' '+I.descr DESCR FROM ITEMGROUP I INNER JOIN FLDCUSTBL1 F ON F.CODEID=LEFT(I.CODEID,3) WHERE F.PKRTBL_ITEMTYPESID=" + itemtype + "  ORDER BY 2 ASC", conn)
                        conn.Open()
                        Using reader As SqlDataReader = com.ExecuteReader
                            If reader.HasRows = True Then
                                While reader.Read()
                                    _MaterialItem.Add(New PTDType With {.id = reader("codeid"), .name = reader("descr")})
                                End While
                            End If

                        End Using
                        conn.Close()
                        ComboBox2.DataSource = _MaterialItem
                        ComboBox2.DisplayMember = "name"
                        ComboBox2.ValueMember = "id"
                    End Using


                End If
            Catch ex As Exception
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End Try
        End If

    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim r As Integer = -1
        Try
            If ComboBox1.SelectedIndex = 0 Then
                ComboBox1.BackColor = Color.Red
                Throw New Exception("Επιλέξτε κατηγορία μεταβλητής!")

            End If
            If ComboBox2.SelectedIndex = 0 Then
                ComboBox2.BackColor = Color.Red
                Throw New Exception("Επιλέξτε τιμή μεταβλητής!")

            End If
            If String.IsNullOrWhiteSpace(TextBox2.Text) Then
                TextBox2.BackColor = Color.Red
                Throw New Exception("Απαιτείται να συμπληρωθεί ποσότητα.")

            End If
            If Not IsNumeric(TextBox2.Text) Then
                TextBox2.BackColor = Color.Red
                Throw New Exception("Μόνο αριθμητικές τιμές αποδεκτές στο πεδίο ποσότητας!")
            End If
            selection = _PTDType(ComboBox1.SelectedIndex).id
            If (selection = 1 Or selection = 2 Or selection = 4 Or selection = 5) And Not String.IsNullOrWhiteSpace(ComboBox2.Text) Then
                Using com As New SqlCommand("INSERT INTO PKRTBL_PALLETTYPESDEFAULTS (PALLETTYPEID,ITEMTYPE,PTDTID,PERTYPEID,QUANTITY) VALUES (@1,@2,@3,@4,@5)", updconn)
                    com.Parameters.AddWithValue("@1", PTId)
                    com.Parameters.AddWithValue("@2", itemtype)
                    com.Parameters.AddWithValue("@3", _PTDType(ComboBox1.SelectedIndex).id.ToString)
                    com.Parameters.AddWithValue("@4", _MaterialItem(ComboBox2.SelectedIndex).id.ToString)
                    com.Parameters.AddWithValue("@5", TextBox2.Text)
                    updconn.Open()
                    r = com.ExecuteNonQuery()
                    updconn.Close()
                End Using
            ElseIf selection = 3 Then
                If Not ComboBox2.Text.Contains("-") Then
                    ComboBox2.BackColor = Color.Red
                    Throw New Exception("Το βάρος πρέπει να έχει μορφή εύρους από-έως. Π.χ. 9,5-11")
                Else
                    Dim txt As String() = ComboBox2.Text.Split("-")
                    If Not IsNumeric(txt(0)) Or Not IsNumeric(txt(1)) Then
                        ComboBox2.BackColor = Color.Red
                        Throw New Exception("Επιτρέπονται μόνο ακέραιοι ή δεκαδικοί αριθμοί στα βάρη")
                    End If
                    If CDbl(txt(0)) > CDbl(txt(1)) Then
                        ComboBox2.BackColor = Color.Red
                        Throw New Exception("Ο πρώτος αριθμός του εύρους πρέπει να είναι μικρότερος ή ίσος του δεύτερου αριθμού.")
                    End If
                    Using com As New SqlCommand("INSERT INTO PKRTBL_PALLETTYPESDEFAULTS (PALLETTYPEID,ITEMTYPE,PTDTID,QUANTITY,PERTYPEMINFLOAT,PERTYPEMAXFLOAT) VALUES (@1,@2,@3,@4,@5,@6)", updconn)
                        com.Parameters.AddWithValue("@1", PTId)
                        com.Parameters.AddWithValue("@2", itemtype)
                        com.Parameters.AddWithValue("@3", _PTDType(ComboBox1.SelectedIndex).id.ToString)
                        com.Parameters.AddWithValue("@4", TextBox2.Text)
                        com.Parameters.AddWithValue("@5", CDbl(txt(0)))
                        com.Parameters.AddWithValue("@6", CDbl(txt(1)))
                        updconn.Open()
                        r = com.ExecuteNonQuery()
                        updconn.Close()
                    End Using
                End If
            End If

        Catch ex As Exception
            If ex.Message.Contains("Cannot insert the value NULL into column") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Πρέπει να συμπληρώσετε όλες τις πληροφορίες!", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            ElseIf ex.Message.Contains("Violation of UNIQUE KEY constraint") Then
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, "Υπάρχει ήδη κανόνας με τα συγκεκριμένα κριτήρια!", "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            Else
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End If

            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
        Finally
            If r > 0 Then
                Me.Dispose()
            End If
        End Try
    End Sub

    Private Sub ComboBox2_TextChanged(sender As Object, e As EventArgs) Handles ComboBox2.TextChanged
        If loaded Then
            Try

                Dim txt As String() = ComboBox2.Text.Split("-")
                If selection = 3 And Not String.IsNullOrWhiteSpace(ComboBox2.Text) Then
                    If Not ComboBox2.Text.Contains("-") Then
                        ComboBox2.BackColor = Color.Red
                    ElseIf Not IsNumeric(txt(0)) Or Not IsNumeric(txt(1)) Then
                        ComboBox2.BackColor = Color.Red
                    ElseIf Not CDbl(txt(0)) <= CDbl(txt(1)) Then
                        ComboBox2.BackColor = Color.Red
                    Else
                        ComboBox2.BackColor = SystemColors.Window
                    End If
                End If
            Catch ex As Exception
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End Try
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Dispose()
    End Sub

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged
        If ComboBox1.BackColor = Color.Red And ComboBox1.SelectedIndex > 0 Then
            ComboBox1.BackColor = SystemColors.Window
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.BackColor = Color.Red And Not String.IsNullOrWhiteSpace(TextBox2.Text) Then
            TextBox2.BackColor = SystemColors.Window
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.BackColor = Color.Red And ComboBox2.SelectedIndex > 0 Then
            ComboBox2.BackColor = SystemColors.Window
        End If
    End Sub
End Class

Class PTDType
    Property name As String
    Property id As Integer
End Class

Class MaterialItem
    Property name As String
    Property id As Integer
End Class
