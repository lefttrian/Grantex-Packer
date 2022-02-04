
Imports System.Configuration
Imports System.Data.SqlClient
Public Class ItemDistribution
    Dim p = GetType(Zuby.ADGV.AdvancedDataGridView).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim string_parameters As New Dictionary(Of String, String)
    Dim integer_parameters As New Dictionary(Of String, Integer)
    Dim integer_parameters_not_equal As New Dictionary(Of String, Integer)
    Dim list_parameters As New Dictionary(Of String, String)
    Dim x, y As Integer
    Public Sub New(ByVal x_ As Integer, y_ As Integer, Optional string_parameters_ As Dictionary(Of String, String) = Nothing, Optional integer_parameters_equal_ As Dictionary(Of String, Integer) = Nothing, Optional integer_parameters_not_equal_ As Dictionary(Of String, Integer) = Nothing, Optional list_parameters_ As Dictionary(Of String, String) = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        string_parameters = string_parameters_
        integer_parameters = integer_parameters_equal_
        list_parameters = list_parameters_
        integer_parameters_not_equal = integer_parameters_not_equal_
        x = x_
        y = y_
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub SetLocation(ByVal x As Integer, y As Integer)
        Dim formTopLeft As New Point(x, y)
        Dim formTopRight As New Point(x + Me.Width, y)
        Dim formBottomleft As New Point(x, y + Me.Height)
        Dim formBottomRight As New Point(x + Me.Width, y + Me.Height)
        Dim l As New List(Of Point) From {formTopLeft, formTopRight, formBottomleft, formBottomRight}
        Dim ActiveScreen As Screen = Screen.FromControl(Me)
        Dim xloc As Integer = x
        Dim yloc As Integer = y
        For Each p As Point In l
            If Not ActiveScreen.WorkingArea.Contains(p) Then
                If ActiveScreen.WorkingArea.Right < p.X Then
                    xloc = ActiveScreen.WorkingArea.Right - Me.Width
                ElseIf ActiveScreen.WorkingArea.Left > p.X Then
                    xloc = ActiveScreen.WorkingArea.Left
                ElseIf ActiveScreen.WorkingArea.Bottom < p.Y Then
                    yloc = ActiveScreen.WorkingArea.Bottom - Me.Height
                ElseIf ActiveScreen.WorkingArea.Top > p.Y Then
                    yloc = ActiveScreen.WorkingArea.Top
                End If
            End If
        Next
        Me.SetDesktopLocation(xloc, yloc)
    End Sub

    Private Sub ItemDistribution_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        doWork()
        Timer1.Start()
    End Sub
    Private Sub doublebufferedTableLayoutPanel1_Click(sender As Object, e As EventArgs) Handles DoubleBufferedTableLayoutPanel1.Click, Panel1.Click, Panel2.Click, AdvancedDataGridView1.Click, CustomDGVSearchBox1.Click
        timercounter = 0
    End Sub
    Private Sub doWork()
        AdvancedDataGridView1.Visible = False
        AdvancedDataGridView1.DataSource = Nothing
        SetLocation(x, y)
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        DoubleBufferedTableLayoutPanel1.Controls.Add(pic, 0, 2)
        pic.Dock = DockStyle.Fill
        mainworker.RunWorkerAsync()
    End Sub

    Private Sub CustomDGVSearchBox1_ButtonPressed() Handles CustomDGVSearchBox1.ButtonPressed
        CustomDGVSearchBox1.continue_click()
    End Sub

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted
        AdvancedDataGridView1.DataSource = e.Result
        For Each c As Control In DoubleBufferedTableLayoutPanel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
        AdvancedDataGridView1.Visible = True
    End Sub

    Dim timercounter As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Me.OwnedForms.Count = 0 Then
            timercounter += 1
        End If
        Label4.Text = (30 - timercounter).ToString
        If timercounter = 30 Then
            Me.Close()
            Me.Dispose()
        End If

    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        doWork()
    End Sub

    Dim hiddenCols As New List(Of String) From {"PALLETID", "ITEID", "STLID", "FTRID"}
    Dim ColNames As New Dictionary(Of String, String) From {{"PALLETCODE", "ΠΑΛΕΤΑ"}, {"CODE", "ΕΙΔΟΣ"}, {"PALLETSTATUS", "ΚΑΤΑΣΤΑΣΗ"}, {"q", "ΠΟΣ"}, {"WEEKNUM", "ΕΒΔΟΜΑΔΑ"}, {"YR", "ΕΤΟΣ"}, {"TRADECODE", "ΠΑΡ"}}

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork
        Try
            Dim dt As New DataTable()
            Dim selecttxt As String = ""
            Dim grouptxt As String = ""
            If CheckBox1.Checked Then
                selecttxt = " PALLETID,PALLETCODE,ISNULL(DBO.PKRFN_GETPALLETSTATUS(PALLETID),'') PALLETSTATUS, sum(quantity) q "
                grouptxt = " group by  PALLETID,PALLETCODE,ISNULL(DBO.PKRFN_GETPALLETSTATUS(PALLETID),'')"
            Else
                selecttxt = " PALLETID,PALLETCODE,ITEID,SUBCODE1 AS CODE,STLID,FTRID,TRADECODE,WEEKNUM,YR,ISNULL(DBO.PKRFN_GETPALLETSTATUS(PALLETID),'') PALLETSTATUS,sum(quantity) q"
                grouptxt = " group by   PALLETID,PALLETCODE,ITEID,SUBCODE1,STLID,FTRID,TRADECODE,WEEKNUM,YR,ISNULL(DBO.PKRFN_GETPALLETSTATUS(PALLETID),'') "
            End If
            Using s As New SqlCommand("Select " + selecttxt + " FROM PKRVIW_PALLETITEMSTATUS where palletid is not null", conn)
                If Not IsNothing(string_parameters) Or Not IsNothing(integer_parameters) Or Not IsNothing(integer_parameters_not_equal) Or Not IsNothing(list_parameters) Then
                    's.CommandText += " WHERE "
                    Dim total_params As Integer = 1
                    If Not IsNothing(string_parameters) Then

                        For Each c As KeyValuePair(Of String, String) In string_parameters
                            If total_params > 0 Then
                                s.CommandText += " AND "
                            End If
                            s.CommandText += c.Key.Replace("@", "")
                            s.CommandText += " Like '%'+@" + c.Key + "+'%'"
                            s.Parameters.AddWithValue("@" + c.Key, c.Value)
                            total_params += 1
                        Next
                    End If
                    If Not IsNothing(integer_parameters) Then
                        For Each c As KeyValuePair(Of String, Integer) In integer_parameters
                            If total_params > 0 Then
                                s.CommandText += " AND "
                            End If
                            s.CommandText += c.Key.Replace("@", "")
                            s.CommandText += "=@" + c.Key
                            s.Parameters.AddWithValue("@" + c.Key, c.Value)
                            total_params += 1
                        Next
                    End If
                    If Not IsNothing(integer_parameters_not_equal) Then
                        For Each c As KeyValuePair(Of String, Integer) In integer_parameters_not_equal
                            If total_params > 0 Then
                                s.CommandText += " AND "
                            End If
                            s.CommandText += c.Key.Replace("@", "")
                            s.CommandText += "<>@" + c.Key
                            s.Parameters.AddWithValue("@" + c.Key, c.Value)
                            total_params += 1
                        Next
                    End If
                    If Not IsNothing(list_parameters) Then
                        For Each c As KeyValuePair(Of String, String) In list_parameters
                            If total_params > 0 Then
                                s.CommandText += " AND "
                            End If
                            s.CommandText += c.Key.Replace("@", "")
                            s.CommandText += " in (select value from dbo.f_split(@" + c.Key + ",',')) "
                            s.Parameters.AddWithValue("@" + c.Key, c.Value)
                            total_params += 1
                        Next
                    End If
                    s.CommandText += grouptxt
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                        'dt.Constraints.Clear()
                    End Using
                    conn.Close()
                End If
            End Using
            e.Result = dt
        Catch ex As Exception

        End Try
    End Sub

    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point

    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseDown, Panel1.MouseDown, DoubleBufferedTableLayoutPanel1.MouseDown, Panel1.MouseDown, Panel2.MouseDown ' Add more handles here (Example: PictureBox1.MouseDown)
        If e.Button = MouseButtons.Left Then
            MoveForm = True
            Me.Cursor = Cursors.NoMove2D
            MoveForm_MousePosition = e.Location
        End If
    End Sub

    Private Sub DatagridviewStackedProgressColumnReportForm_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        If Me.OwnedForms.Count = 0 Then
            Me.Close()
            Me.Dispose()
        End If
    End Sub

    Public Sub MoveForm_MouseMove(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseMove, Panel1.MouseMove, Panel2.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If
    End Sub

    Private Sub DoubleBufferedTableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles DoubleBufferedTableLayoutPanel1.Paint

    End Sub

    Private Sub AdvancedDataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles AdvancedDataGridView1.DataBindingComplete

        For Each c As DataGridViewColumn In AdvancedDataGridView1.Columns
            If hiddenCols.Contains(c.Name) Then
                c.Visible = False
            End If
            If ColNames.ContainsKey(c.Name) Then
                c.HeaderText = ColNames(c.Name)
            End If
        Next
    End Sub

    Private Sub AdvancedDataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView1.CellDoubleClick
        If e.RowIndex > -1 And e.ColumnIndex > -1 Then
            With AdvancedDataGridView1
                If .Columns(e.ColumnIndex).Name = "CODE" Then
                    Using f As New ItemDetails(.Rows(e.RowIndex).Cells("iteid").Value)
                        f.Owner = Me
                        f.ShowDialog()
                    End Using
                ElseIf .Columns(e.ColumnIndex).Name = "PALLETCODE" Then
                    Using f As New PalletDetails(.Rows(e.RowIndex).Cells("palletid").Value)
                        f.Owner = Me
                        f.ShowDialog()
                    End Using
                ElseIf .Columns(e.ColumnIndex).Name = "TRADECODE" Then
                    Using f As New Order(.Rows(e.RowIndex).Cells("ftrid").Value)
                        f.Owner = Me
                        f.ShowDialog()
                    End Using
                End If
            End With
        End If
    End Sub

    Private Sub TableLayoutPanel1_Click(sender As Object, e As EventArgs)

    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseUp, Panel1.MouseUp, Panel2.MouseUp ' Add more handles here (Example: PictureBox1.MouseUp)
        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If
    End Sub
End Class