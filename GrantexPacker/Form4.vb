Imports System.Configuration
Imports System.Data.SqlClient

Public Class Form4
    Dim counter As Integer = Form1.counter
    Dim orders As ArrayList = New ArrayList()
    Dim txt As String = ""
    Dim indexes As ArrayList = Form1.indexes
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim updconn As New SqlConnection(updString)
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cursor.Current = ExtCursor1.Cursor

        ComboBox1.Items.Clear()
        txt = ""
        orders.Clear()
        Label1.Text = "Το είδος " + Form1.TextBox3.Text
        For j As Integer = 0 To counter - 1
            orders.Add(Form1.DataGridView1.Rows(indexes.Item(j)).Cells("ΠΑΡ").Value)
            txt = txt + " " + Form1.DataGridView1.Rows(indexes.Item(j)).Cells("ΠΑΡ").Value
            ComboBox1.Items.Add(New MyListItem(Form1.DataGridView1.Rows(indexes.Item(j)).Cells("ΠΑΡ").Value, Form1.DataGridView1.Rows(indexes.Item(j)).Cells("stlid").Value))
        Next
        txt = Trim(txt)
        TextBox2.Text = txt
        ComboBox1.SelectedIndex = 0
        Label6.Text = Form1.Label19.Text
        Me.Text = "Εισαγωγή προϊόντος στη παλέτα " + Form1.Label19.Text
        Cursor.Current = Cursors.Default
    End Sub

    'Form overrides dispose to clean up the component list.
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
            orders = Nothing
            indexes = Nothing
            updconn.Dispose()

        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub


    Dim defaultvalue As Integer
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim oItem As MyListItem = CType(ComboBox1.SelectedItem, MyListItem)
        For i As Integer = 0 To Form1.DataGridView1.Rows.Count - 1
            If Form1.DataGridView1.Rows(i).Cells("stlid").Value = oItem.Value Then
                defaultvalue = Form1.DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value
                TextBox1.Text = defaultvalue
                Form1.selectedorder = oItem.Text
                Exit For
            End If
        Next


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim PALLETID As String = ""
        Dim oItem As MyListItem = CType(ComboBox1.SelectedItem, MyListItem)
        Try
            If IsNumeric(TextBox1.Text) Then
                If TextBox1.Text.Length <= 5 And TextBox1.Text > 0 And TextBox1.Text <> "" And TextBox1.Text <= defaultvalue Then
                    Dim rtoadd As New Integer

                    For i As Integer = 0 To Form1.DataGridView1.Rows.Count - 1
                        If Form1.DataGridView1.Rows(i).Cells("stlid").Value = oItem.Value Then

                            rtoadd = i
                        End If
                    Next

                    For Each pi As Control In Form1.FlowLayoutPanel1.Controls
                        Dim normal As pallettemplate = TryCast(pi, pallettemplate)
                        Dim small As smallpallet = TryCast(pi, smallpallet)

                        If normal IsNot Nothing Then
                            If normal.pallettemplatelabel.Text = Label6.Text Then
                                PALLETID = normal.palletid
                                Exit For
                            End If

                        ElseIf small IsNot Nothing Then
                            If small.Label1.Text = Label6.Text Then
                                PALLETID = small.palletid
                                Exit For
                            End If
                        End If
                    Next
                    If Len(PALLETID) = 0 Then
                        Throw New System.Exception("Δεν βρέθηκε η παλέτα.")
                    End If
                    Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.CUSID)
                        pm.AddItem(PALLETID, Form1.DataGridView1.Rows(rtoadd).Cells("iteid").Value, Form1.DataGridView1.Rows(rtoadd).Cells("stlid").Value, Form1.DataGridView1.Rows(rtoadd).Cells("ftrid").Value, TextBox1.Text)
                    End Using
                    'Form1.pallet_exchange(2, PALLETID, TextBox1.Text, Form1.DataGridView1.Rows(rtoadd))
                    Form1.datagridview1_stlquantity(Form1.DataGridView1.Rows(rtoadd).Cells("stlid").Value)
                Else
                    TextBox1.Text = defaultvalue
                    Return
                End If
            Else
                TextBox1.Text = defaultvalue
                Throw New System.Exception("Μόνο αριθμητικές τιμές αποδεκτές!")
            End If
            Me.Close()
            Form1.DataGridView1.Refresh()
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class

Public Class MyListItem
    Private mText As String
    Private mValue As String

    Public Sub New(ByVal pText As String, ByVal pValue As String)
        mText = pText
        mValue = pValue
    End Sub

    Public Sub dispose()
        Me.dispose()
    End Sub

    Public ReadOnly Property Text() As String
        Get
            Return mText
        End Get
    End Property

    Public ReadOnly Property Value() As String
        Get
            Return mValue
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return mText
    End Function
End Class