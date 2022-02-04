Public Class CalendarSelector

    Public Sub New(ByVal d As Date, x As Integer, y As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        xloc = x
        yloc = y
        ' Add any initialization after the InitializeComponent() call.
        MonthCalendar1.SelectionStart = d

    End Sub
    Dim yloc As Integer
    Dim xloc As Integer
    Private Sub CalendarSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.SetDesktopLocation(xloc, yloc)
    End Sub

    Private Sub CalendarSelector_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub MonthCalendar1_DateChanged(sender As Object, e As DateRangeEventArgs) Handles MonthCalendar1.DateChanged

    End Sub

    Dim last_mouse_down As DateTime = DateTime.Now

    Private Sub MonthCalendar1_DateSelected(sender As Object, e As DateRangeEventArgs) Handles MonthCalendar1.DateSelected
        If ((DateTime.Now - last_mouse_down).TotalMilliseconds <= SystemInformation.DoubleClickTime) Then
            Dim seldate As Date = e.Start
            If e.Start.DayOfWeek = DayOfWeek.Sunday Then seldate = seldate.AddDays(-1)
            Dim monDate As DateTime = seldate.AddDays(DayOfWeek.Monday - seldate.DayOfWeek)
            TryCast(Owner, production).FirstMonday = monDate
            TryCast(Owner, production).TextBox1.Text = monDate.ToShortDateString.ToString + "-" + monDate.AddDays(6).ToShortDateString.ToString
            Me.Close()
            Me.Dispose()
        End If
        last_mouse_down = DateTime.Now
    End Sub
End Class