Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.Objects
Imports System.Data.SqlClient
Imports System.Configuration

Imports System.Deployment.Application
Public Class report_issue

    Dim p = GetType(DataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub report_issue_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = Form1.activeuser
        TextBox2.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        Label6.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox3.Text <> "" Or TextBox4.Text <> "" Then
            Try
                Cursor.Current = ExtCursor1.Cursor
                Dim fileloc As String = "none"
                Dim dtime As String = TextBox2.Text
                dtime = dtime.Replace(":", "_")
                dtime = dtime.Replace("/", "_")
                dtime = dtime.Replace(" ", "_")
                If Label6.Text <> "" Then
                    fileloc = "\\DC-SERVER\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\issues\" + TextBox1.Text + "_" + dtime + Path.GetExtension(Label6.Text)
                    File.Copy(Label6.Text, fileloc)
                End If
                Dim cmd As String = "insert into tbl_packerissues (username,issue,cause,datetime,imageloc) values ('" + TextBox1.Text + "','" + TextBox3.Text + "','" + TextBox4.Text + "','" + TextBox2.Text + "','" + fileloc + "')"
                Dim comm As SqlCommand = New SqlCommand(cmd, updconn)
                updconn.Open()
                comm.ExecuteNonQuery()
                updconn.Close()
                Cursor.Current = Cursors.Default
                Me.Close()
            Catch ex As Exception
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
               Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

            End Try
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class