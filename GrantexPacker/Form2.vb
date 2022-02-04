Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Deployment.Application

Public Class Form2

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim _assemblyInfo As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        If System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed Then
            If System.Diagnostics.Debugger.IsAttached Then
                Version.Text = "Debug Mode"
            Else
                Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
            End If
        Else
            If Not IsNothing(_assemblyInfo) Then
                If System.Diagnostics.Debugger.IsAttached Then
                    Version.Text = "Debug Mode"
                Else
                    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
                End If
            End If
        End If
        'Label4.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString
    End Sub
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim cmd2 As String = "select pu.id,pu.username,pu.connected,pu.DEPARTMENT,isnull(pu.ORDCUSER,0),isnull(pu.lastworkstation,0),isnull(pu.atlantisid,0),isnull(pud.id,0) dptid from tbl_packeruserdata  pu left join PKRTBL_USERDEPARTMENTS pud on pud.code=pu.department"
            Dim foundflag As Integer
            Using comm2 As New SqlCommand(cmd2, conn)
                Using dt2 = New DataTable()
                    conn.Open()
                    Using reader2 As SqlDataReader = comm2.ExecuteReader
                        dt2.Load(reader2)
                        Dim usernames = dt2.AsEnumerable().Select(Function(d) DirectCast(d(1).ToString(), Object)).ToArray()
                        Dim ids = dt2.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray()
                        Dim connected = dt2.AsEnumerable().Select(Function(d) DirectCast(d(2).ToString(), Object)).ToArray()
                        Dim dpt = dt2.AsEnumerable().Select(Function(d) DirectCast(d(3).ToString(), Object)).ToArray()
                        Dim ocu = dt2.AsEnumerable().Select(Function(d) DirectCast(d(4).ToString(), Object)).ToArray()
                        Dim aid = dt2.AsEnumerable().Select(Function(d) DirectCast(d(6).ToString(), Object)).ToArray()
                        Dim dptid = dt2.AsEnumerable().Select(Function(d) DirectCast(d(7).ToString(), Object)).ToArray()
                        conn.Close()
                        For i As Integer = 0 To usernames.Count - 1
                            If StrComp(TextBox1.Text, usernames(i), CompareMethod.Text) = 0 Then
                                If connected(i) = 1 And dt2.Rows(i).Item(5) <> Environment.MachineName Then
                                    Throw New System.Exception("Ο χρήστης είναι ήδη συνδεδεμένος. Δεν γίνεται να συνδεθείτε με το ίδιο username.")
                                Else
                                    foundflag = 1
                                    Form1.activeuserid = ids(i)
                                    Form1.activeuserdpt = dpt(i)
                                    Form1.activeuserocu = ocu(i)
                                    Form1.activeuseraid = aid(i)
                                    Form1.activeuserdptid = dptid(i)
                                End If
                            End If
                        Next
                    End Using
                End Using
            End Using
            If foundflag = 1 Then
                Label3.Visible = True
                TextBox2.Visible = True
                TextBox1.Enabled = False
                Button1.Visible = False
                Button2.Visible = True
                PictureBox2.Visible = False
                PictureBox4.Visible = True
                PictureBox6.Visible = False
            Else
                PictureBox2.Visible = False
                PictureBox4.Visible = False
                PictureBox6.Visible = True
                Throw New System.Exception("Δεν βρέθηκε ο χρήστης. Επικοινωνήστε με τον διαχειριστή.")
            End If
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

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            'For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
            '    Dim ctrl As Control = Me.Controls(i)
            '    ctrl.Dispose()
            'Next
            'My.Resources.packerfulllogo.Dispose()
            'My.Resources.notcompleted.Dispose()
            'My.Resources.completed.Dispose()
            'My.Resources.completed.Dispose()
            'My.Resources.rolling__2_.Dispose()
            conn.Dispose()
            updconn.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim cmd3 As String = "select password from tbl_packeruserdata where id=" + Form1.activeuserid.ToString
            Dim foundflag As Integer
            Using comm3 As New SqlCommand(cmd3, conn)
                Using dt3 = New DataTable()
                    conn.Open()
                    Using reader3 As SqlDataReader = comm3.ExecuteReader


                        dt3.Load(reader3)
                        Dim passwords = dt3.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray()

                        For i As Integer = 0 To passwords.Count - 1
                            If StrComp(TextBox2.Text, passwords(i), CompareMethod.Text) = 0 Then
                                foundflag = 1

                            End If
                        Next

                        If foundflag = 1 Then
                            PictureBox3.Visible = False
                            PictureBox5.Visible = True
                            PictureBox7.Visible = False
                            Me.Refresh()
                            Dim cmd4 As String = "update tbl_packeruserdata set connected=1,lastworkstation='" + Environment.MachineName + "' where id=" + Form1.activeuserid.ToString
                            Using Transaction = TransactionUtils.CreateTransactionScope()
                                Using ut As New PackerUserTransaction
                                    Using comm4 As New SqlCommand(cmd4, updconn)
                                        updconn.Open()
                                        comm4.ExecuteNonQuery()
                                        updconn.Close()
                                    End Using
                                    ut.WriteEntry(Form1.activeuserid, 1, Environment.MachineName)
                                End Using
                                Transaction.Complete()
                            End Using
                        Else
                            conn.Close()
                            PictureBox3.Visible = False
                            PictureBox5.Visible = False
                            PictureBox7.Visible = True
                            Throw New System.Exception("Λάθος κωδικός. Επικοινωνήστε με τον διαχειριστή.")



                        End If
                        conn.Close()
                    End Using
                End Using
            End Using

            Form1.activeuser = TextBox1.Text.ToUpper
            Me.Close()
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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Environment.Exit(0)

    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub TextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button2.PerformClick()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If Len(TextBox1.Text) > 0 Then
            PictureBox4.Visible = False
            PictureBox6.Visible = False
            PictureBox2.Visible = True
        Else
            PictureBox2.Visible = False
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If Len(TextBox2.Text) > 0 Then
            PictureBox5.Visible = False
            PictureBox7.Visible = False
            PictureBox3.Visible = True
        Else
            PictureBox3.Visible = False
        End If
    End Sub

    Private Sub Form2_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        'Dim p As New Pen(Color.Red, 2)
        'e.Graphics.DrawRectangle(p, 0, 0, Me.Width - 1, Me.Height - 1)
        'ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Black, ButtonBorderStyle.Solid)

    End Sub
End Class