Public Class notification

    Public Property myname As String
        Get
            Return Label1.Text
        End Get
        Set(value As String)
            Label1.Text = value
        End Set
    End Property
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
            My.Resources.exclamation_logo_icon_44036.Dispose()
            Timer1.Dispose()
            Me.Controls.Clear()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub


    Private createdon As DateTime
    Public Property createtime As DateTime
        Get
            Return createdon.Date.ToString()
        End Get
        Set(value As DateTime)
            createdon = value
        End Set
    End Property

    Public Property color As Color
        Get
            Return Me.BackColor
        End Get
        Set(value As Color)
            Me.BackColor = value
        End Set
    End Property

    Public Property bold As Boolean
        Get
            Return Me.Label1.Font.Bold
        End Get
        Set(value As Boolean)
            Dim boldfont = New Font(Label1.Font, FontStyle.Bold)
            Dim normalfont = New Font(Label1.Font, FontStyle.Regular)
            If value = True Then
                        Me.Label1.Font = boldfont
                    Else
                        Me.Label1.Font = normalfont
                    End If

        End Set
    End Property

    Private Sub notification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim elapsedTime As TimeSpan = DateTime.Now().Subtract(Me.createdon)
        If elapsedTime.TotalMinutes < 10 Then
            Me.color = Color.DarkOrange
            Me.bold = True
        ElseIf elapsedTime.TotalMinutes >= 10 And elapsedTime.TotalMinutes < 100 Then
            Me.color = Color.Orange
            Me.bold = True
        ElseIf elapsedTime.TotalMinutes >= 100 And elapsedTime.TotalDays <= 1 Then
            Me.color = Color.NavajoWhite
        Else
            Me.color = Color.Transparent
        End If
        Timer1.Interval = 100000
        Timer1.Start()
    End Sub

    Dim timer As Integer

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        timer += 1
        If timer = 6 Then
            timer = 0
            If DateDiff(DateInterval.Minute, DateTime.Now(), Me.createdon) < 1 Then
                Me.color = Color.DarkOrange
                Me.bold = True
            ElseIf DateDiff(DateInterval.Minute, DateTime.Now(), Me.createdon) >= 1 And DateDiff(DateInterval.Minute, DateTime.Now(), Me.createdon) < 10 Then
                Me.color = Color.Orange
                Me.bold = True
            ElseIf DateDiff(DateInterval.Minute, DateTime.Now(), Me.createdon) >= 10 And DateDiff(DateInterval.Day, DateTime.Now(), Me.createdon) <= 1 Then
                Me.color = Color.NavajoWhite
            Else
                Me.color = Color.Transparent
            End If

        End If
    End Sub

    Private Sub notification_DoubleClick(sender As Object, e As EventArgs) Handles MyBase.DoubleClick
        Form1.TextBox1.Text = Me.myname.Substring(0, Me.myname.IndexOf(" "))
    End Sub

    Private Sub Label1_DoubleClick(sender As Object, e As EventArgs) Handles Label1.DoubleClick
        Form1.TextBox1.Text = Me.myname.Substring(0, Me.myname.IndexOf(" "))
    End Sub
End Class
