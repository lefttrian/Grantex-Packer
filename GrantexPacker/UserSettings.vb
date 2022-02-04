Imports System.Data.Objects
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Deployment.Application
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Security.Permissions
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Globalization



Public Class UserSettings

    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim username As String = Form1.activeuser
    Dim userid As String = Form1.activeuserid.ToString
    Dim settings As List(Of settingsitem) = Form1.settings
    Dim i As Integer = 1
    Dim sett As New settings_exchange
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Me.Close()
    End Sub

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            sett.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try

    End Sub

    Private Sub Form18_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Cursor.Current = ExtCursor1.Cursor
            Dim cmd2 As String = "SELECT PU.PASSWORD,D.NAME  FROM TBL_PACKERUSERDATA PU LEFT JOIN PKRTBL_USERDEPARTMENTS D ON D.CODE=PU.DEPARTMENT WHERE PU.id=" + userid


            Using comm2 As New SqlCommand(cmd2, conn)
                Using dt As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = comm2.ExecuteReader
                        dt.Load(reader)
                        Label9.Text = username
                        TextBox2.Text = dt.Rows(0).Item("PASSWORD")
                        Label10.Text = dt.Rows(0).Item("NAME")
                    End Using
                End Using


                conn.Close()
            End Using



            For Each s As settingsitem In settings
                If s.name = "fsize" Then
                    ComboBox2.SelectedIndex = ComboBox2.FindStringExact(s.value)
                ElseIf s.name = "palletsize" Then
                    ComboBox1.SelectedIndex = ComboBox1.FindStringExact(s.value)

                ElseIf s.name = "hidedispatchdatedgv1" Then
                    If s.value = "true" Then
                        CheckBox2.Checked = True
                    End If
                End If
            Next
            Cursor.Current = Cursors.Default
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            TextBox2.PasswordChar = ""
        Else
            TextBox2.PasswordChar = "*"
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Cursor.Current = ExtCursor1.Cursor
        For Each s As settingsitem In Form1.settings
            If s.name = "fsize" Then
                s.value = ComboBox2.Items(ComboBox2.SelectedIndex)
            ElseIf s.name = "palletsize" Then
                s.value = ComboBox1.Items(ComboBox1.SelectedIndex)

            ElseIf s.name = "hidedispatchdatedgv1" Then
                If CheckBox2.Checked Then
                    s.value = "true"
                Else
                    s.value = "false"
                End If
            ElseIf s.name = "splitposition" Then
                s.value = Form1.SplitContainer1.SplitterDistance
            End If
        Next
        Dim f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "fsize")
        If f Is Nothing And ComboBox2.SelectedIndex >= 0 Then
            Form1.settings.Add(New settingsitem("fsize", ComboBox2.Items(ComboBox2.SelectedIndex)))
        End If

        f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "palletsize")
        If f Is Nothing And ComboBox1.SelectedIndex >= 0 Then
            Form1.settings.Add(New settingsitem("palletsize", ComboBox1.Items(ComboBox1.SelectedIndex)))
        End If
        f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "splitposition")
        If f Is Nothing Then
            Form1.settings.Add(New settingsitem("splitposition", Form1.SplitContainer1.SplitterDistance))
        End If
        f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "refresh")

        f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "hidedispatchdatedgv1")
        If f Is Nothing And CheckBox2.Checked Then
            Form1.settings.Add(New settingsitem("hidedispatchdatedgv1", "true"))
        ElseIf f Is Nothing And Not CheckBox2.Checked Then
            Form1.settings.Add(New settingsitem("hidedispatchdatedgv1", "false"))
        End If
        Dim odgvorder As New List(Of String)
        Dim cnt As Integer = 0
        While cnt <= Form1.orderdgv.Columns.Count - 1
            For Each c As DataGridViewColumn In Form1.orderdgv.Columns
                If c.DisplayIndex = cnt Then
                    If c.Visible Then
                        odgvorder.Add(c.Name)
                    End If
                    cnt += 1
                Else
                    Continue For
                End If

            Next
        End While

        f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "odgvorder")
        If f Is Nothing And odgvorder.Count > 0 Then
            Form1.settings.Add(New settingsitem("odgvorder", String.Join(",", odgvorder.ToArray())))
        Else
            Form1.settings.Remove(f)
            Form1.settings.Add(New settingsitem("odgvorder", String.Join(",", odgvorder.ToArray())))
        End If
        Dim dgv1order As New List(Of String)
        Dim cnt2 As Integer = 0
        While cnt2 <= Form1.DataGridView1.Columns.Count - 1
            For Each c As DataGridViewColumn In Form1.DataGridView1.Columns
                If c.DisplayIndex = cnt2 Then
                    If c.Visible Then
                        dgv1order.Add(c.Name)
                    End If
                    cnt2 += 1
                Else
                    Continue For
                End If

            Next
        End While

        f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "dgv1order")
        If f Is Nothing And dgv1order.Count > 0 Then
            Form1.settings.Add(New settingsitem("dgv1order", String.Join(",", dgv1order.ToArray())))
        Else
            Form1.settings.Remove(f)
            Form1.settings.Add(New settingsitem("dgv1order", String.Join(",", dgv1order.ToArray())))
        End If

        Dim success As Integer = sett.update_settings
        Dim cmd2 As String = "update tbl_packeruserdata set password=@password where id=" + userid
        Using comm2 As New SqlCommand(cmd2, updconn)
            comm2.Parameters.Add("@password", SqlDbType.VarChar).Value = TextBox2.Text
            updconn.Open()
            success = success + comm2.ExecuteNonQuery()
            updconn.Close()
        End Using
        If success = 2 Then
            Label7.Visible = True
            Label7.ForeColor = Color.Green
            Label7.Text = "Επιτυχία!"
            Button1.Text = "Κλείσιμο"
            Button1.Width = 80
            Using ut As New PackerUserTransaction
                ut.WriteEntry(Form1.activeuserid, 46)
            End Using
        Else
            Label7.Visible = True
            Label7.ForeColor = Color.Red
            Label7.Text = "Κάτι δεν πήγε καλά!"
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "dgv1order")
        If f Is Nothing Then
            Form1.settings.Add(New settingsitem("dgv1order", "Σ,ΕΠΙΛ,ΠΟΣ,ΥΠΟΛ.,ΑΠΟΔΕΚΤΗΣ,ΠΡΟΣΩΠ ΣΧΟΛΙΑ,ΗΜ/ΝΙΑ ΑΠΟΣΤΟΛΗΣ,ΚΩΔΙΚΟΣ,ΠΕΡΙΓΡΑΦΗ,ΠΑΡ,ΠΕΛ,BACKORDER,ΣΧΟΛΙΑ ΕΞΑΓ,ΣΧΟΛΙΑ ΠΑΡΑΓ,ΣΧΟΛΙΑ ΑΠΟΘ,ΣΧΟΛΙΑ ΣΥΣΚ"))
        Else
            Form1.settings.Remove(f)
            Form1.settings.Add(New settingsitem("dgv1order", "Σ,ΕΠΙΛ,ΠΟΣ,ΥΠΟΛ.,ΑΠΟΔΕΚΤΗΣ,ΠΡΟΣΩΠ ΣΧΟΛΙΑ,ΗΜ/ΝΙΑ ΑΠΟΣΤΟΛΗΣ,ΚΩΔΙΚΟΣ,ΠΕΡΙΓΡΑΦΗ,ΠΑΡ,ΠΕΛ,BACKORDER,ΣΧΟΛΙΑ ΕΞΑΓ,ΣΧΟΛΙΑ ΠΑΡΑΓ,ΣΧΟΛΙΑ ΑΠΟΘ,ΣΧΟΛΙΑ ΣΥΣΚ"))
        End If
        f = settings.FirstOrDefault(Function(settingsitem) settingsitem.name = "odgvorder")
        If f Is Nothing Then
            Form1.settings.Add(New settingsitem("odgvorder", "ΠΡΟΣΩΠ ΣΧΟΛΙΑ,ΚΑΤΑΣΤ ΧΡΗΣΤΗ,STATUS,ΠΩΛΗΤΗΣ,ΠΑΡ,ΠΕΛ,ΗΜ/ΝΙΑ ΑΠΟΣΤΟΛΗΣ,ΑΠΟΜΕΝΟΥΝ,ΗΜΕΡΟΜΗΝΙΑ ΚΑΤΑΧΩΡΙΣΗΣ,ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ,ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ,ΕΛ. ΑΠΟΘΗΚΗΣ,ΗΜ/ΝΙΑ ΕΛ.ΑΠΘ.,ΣΧΟΛΙΑ ΑΠΘ,ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ,ΕΛ. ΠΑΡΑΓ,ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ,ΕΛ. ΠΑΡΑΓ2,ΗΜ/ΝΙΑ ΕΛ.ΠΑΡΑΓ.,ΣΧΟΛΙΑ ΠΑΡΑΓ,ΣΧΟΛΙΑ ΣΥΣΚΕΥ"))
        Else
            Form1.settings.Remove(f)
            Form1.settings.Add(New settingsitem("odgvorder", "ΠΡΟΣΩΠ ΣΧΟΛΙΑ,ΚΑΤΑΣΤ ΧΡΗΣΤΗ,STATUS,ΠΩΛΗΤΗΣ,ΠΑΡ,ΠΕΛ,ΗΜ/ΝΙΑ ΑΠΟΣΤΟΛΗΣ,ΑΠΟΜΕΝΟΥΝ,ΗΜΕΡΟΜΗΝΙΑ ΚΑΤΑΧΩΡΙΣΗΣ,ΑΠΘ ΕΛΕΓΧΟΣ ΔΙΣΚ,ΑΠΘ ΕΛΕΓΧΟΣ ΦΕΡ,ΕΛ. ΑΠΟΘΗΚΗΣ,ΗΜ/ΝΙΑ ΕΛ.ΑΠΘ.,ΣΧΟΛΙΑ ΑΠΘ,ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΔΙΣΚ,ΕΛ. ΠΑΡΑΓ,ΠΑΡΑΓ ΕΛΕΓΧΟΣ ΦΕΡ,ΕΛ. ΠΑΡΑΓ2,ΗΜ/ΝΙΑ ΕΛ.ΠΑΡΑΓ.,ΣΧΟΛΙΑ ΠΑΡΑΓ,ΣΧΟΛΙΑ ΣΥΣΚΕΥ"))
        End If
        Dim success As Integer = sett.update_settings
        If success > 0 Then
            Label8.Text = "Επιτυχία!"
            Label8.ForeColor = Color.Green
            Label8.Visible = True
        Else
            Label8.Text = "Πρόβλημα!"
            Label8.ForeColor = Color.Red
            Label8.Visible = True
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using f As New UsertTransactions(userid)
            f.ShowDialog()
        End Using
    End Sub
End Class