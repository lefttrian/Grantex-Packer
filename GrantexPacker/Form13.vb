Imports System.Text.RegularExpressions

Public Class Form13
    Private hiddenfilters As Boolean = False

    Public Property active As Boolean
        Get
            Return Me.hiddenfilters
        End Get

        Set(ByVal value As Boolean)

            Me.hiddenfilters = value
                NotifyPropertyChanged(value)

        End Set

    End Property

    Private Sub NotifyPropertyChanged(ByVal value As Boolean)
        'If value Then
        '    Form1.Button25.Image = My.Resources.active_filters
        '    Form1.ToolTip11.SetToolTip(Form1.Button25, "Υπάρχουν ενεργά φίλτρα που δεν φαίνονται!")
        'Else
        '    Form1.Button25.Image = My.Resources.more_filters21
        '    Form1.ToolTip11.SetToolTip(Form1.Button25, "Περισσότερα φίλτρα")
        'End If
    End Sub

    Private Sub Form13_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        hiddenfilters = False
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = System.Windows.Forms.Cursor.Position
        DateTimePicker1.Value = Form1.DateTimePicker1.Value
        DateTimePicker2.Value = Form1.DateTimePicker2.Value
        TextBox1.Text = Form1.TextBox1.Text
        TextBox2.Text = Form1.TextBox2.Text
        TextBox3.Text = Form1.TextBox3.Text
        cusnamemaskbox.Text = Form1.cusnamemaskbox.Text
        For i As Integer = 0 To Form1.callnamescbox.Items.Count - 1
            Dim obj As Object = Form1.callnamescbox.Items(i)
            callnamescbox.Items.Add(obj)
        Next

        callnamescbox.Text = Form1.callnamescbox.Text
        cuscodemaskbox.Text = Form1.cuscodemaskbox.Text
        ComboBox4.SelectedIndex = Form1.ComboBox4.SelectedIndex
        If Form1.pfilter.Contains("%") Then
            TextBox5.Text = Form1.pfilter.Replace("%", "")
        Else
            TextBox5.Text = Form1.pfilter
        End If
        If Form1.plfilter.Contains("%") Then
            TextBox3.Text = Form1.plfilter.Replace("%", "")
        Else
            TextBox3.Text = Form1.plfilter
        End If

        DateTimePicker3.Value = Today
        DateTimePicker4.Value = Today
        DateTimePicker5.Value = Today
        DateTimePicker6.Value = Today
        DateTimePicker7.Value = Today
        DateTimePicker8.Value = Today
        DateTimePicker9.Value = Today
        DateTimePicker10.Value = Today
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckBox1.Checked = True Then
            Form1.plcdfilterfrom = DateTimePicker3.Value
            Form1.plcdfilterto = DateTimePicker4.Value

        Else
            Form1.plcdfilterfrom = Nothing
            Form1.plcdfilterto = Nothing

        End If
        If CheckBox2.Checked = True Then
            Form1.plodfilterfrom = DateTimePicker5.Value
            Form1.plodfilterto = DateTimePicker6.Value

        Else
            Form1.plodfilterfrom = Nothing
            Form1.plodfilterto = Nothing
        End If
        If CheckBox3.Checked = True Then
            Form1.pcdfilterfrom = DateTimePicker7.Value
            Form1.pcdfilterto = DateTimePicker8.Value

        Else
            Form1.pcdfilterfrom = Nothing
            Form1.pcdfilterto = Nothing
        End If
        If CheckBox4.Checked = True Then
            Form1.podfilterfrom = DateTimePicker9.Value
            Form1.podfilterto = DateTimePicker10.Value

        Else
            Form1.podfilterfrom = Nothing
            Form1.podfilterto = Nothing
        End If
        Dim pfilter As String = TextBox5.Text
        Dim plfilter As String = TextBox3.Text

        If plfilter <> "" Then
            Dim plistmask As String = Regex.Replace(plfilter, "[^0-9\*]", "")
            plistmask = Regex.Replace(plistmask, "[*]", "%")
            If plistmask.IndexOf("%") = -1 Then
                plistmask = "%" + plistmask + "%"
            End If
            Form1.plfilter = plistmask

        Else
            Form1.plfilter = ""
        End If
        If pfilter <> "" Then
            Dim palletmask As String = pfilter
            palletmask = Regex.Replace(palletmask, "[*]", "%")
            If palletmask.IndexOf("%") = -1 Then
                palletmask = "%" + palletmask + "%"
            End If
            Form1.pfilter = palletmask

        Else
            Form1.pfilter = ""
        End If

        If ComboBox4.SelectedIndex <> 0 Or pfilter <> "" Or plfilter <> "" Or CheckBox4.Checked = True Or CheckBox3.Checked = True Or CheckBox2.Checked = True Or CheckBox1.Checked = True Then
            Me.active = True
        Else
            Me.active = False
        End If
        Form1.DateTimePicker1.Value = DateTimePicker1.Value
            Form1.DateTimePicker2.Value = DateTimePicker2.Value
        Form1.TextBox1.Text = TextBox1.Text
        Form1.TextBox2.Text = TextBox2.Text
        Form1.TextBox3.Text = TextBox3.Text
        Form1.cusnamemaskbox.Text = cusnamemaskbox.Text
        Form1.callnamescbox.Text = callnamescbox.Text
        Form1.cuscodemaskbox.Text = cuscodemaskbox.Text
        Form1.ComboBox4.SelectedIndex = ComboBox4.SelectedIndex
        Cursor.Current = ExtCursor1.Cursor
        Form1.Button1.PerformClick()
        Me.Close()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

    End Sub
End Class