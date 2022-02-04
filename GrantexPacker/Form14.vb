Public Class Form14
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.logo = True
        Me.Close()
        Dim frm = New PrintOpenLabels
        frm.showdialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Form1.logo = False
        Me.Close()

        Dim frm = New PrintOpenLabels
        frm.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Try
            Form1.logo = True
            Me.Close()
            Dim frm = New PrintCloseLabels
            frm.ShowDialog()
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Form1.logo = False
        Me.Close()
        Dim frm = New PrintCloseLabels
        frm.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Me.Close()
        Dim frm = New Form9_VALEO
        frm.ShowDialog()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        Me.Close()
        Dim frm = New Form9_FERODO
        frm.ShowDialog()
    End Sub
End Class