Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class PackingListCreate
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim customers As New DataTable()
    Dim x, y As Integer

    Public Sub New(ByVal xloc As Integer, yloc As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        x = xloc - Me.Size.Width
        y = yloc
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub PackingListCreate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.SetDesktopLocation(x, y)
        Dim cmd As String = "select  id, isnull(fathername,'')+' '+code+'-'+name  from customer where fathername is not null order by 2"
        Dim comm As New SqlCommand(cmd, conn)
        conn.Open()
        Dim reader As SqlDataReader = comm.ExecuteReader
        customers.Load(reader)
        conn.Close()
        Dim items = customers.AsEnumerable().Select(Function(d) DirectCast(d(1).ToString(), Object)).ToArray()
        ComboBox1.Items.AddRange(items)
        conn.Close()
    End Sub

    Public plcode As String = ""
    Public cusid As Integer
    Public IsDraft As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Regex.IsMatch(TextBox1.Text, "^[0-9]{4}$") Or Regex.IsMatch(TextBox1.Text, "^[S0-9]{5}$") Or Regex.IsMatch(TextBox1.Text, "^[0-9]{5}$") Then
            If ComboBox1.SelectedIndex >= 0 Then
                plcode = TextBox1.Text
                cusid = customers.Rows(ComboBox1.SelectedIndex).Item("id")
                If CheckBox1.Checked Then
                    IsDraft = True
                End If
                Me.DialogResult = DialogResult.OK
            Else
                ComboBox1.BackColor = Color.Red
            End If

        Else
            TextBox1.BackColor = Color.Red

        End If

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.BackColor = Color.Red Then
            TextBox1.BackColor = SystemColors.Window
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.BackColor = Color.Red Then
            ComboBox1.BackColor = SystemColors.Window
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub
End Class