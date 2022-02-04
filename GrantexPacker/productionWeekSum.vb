Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Transactions
Public Class productionWeekSum

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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Dim weeknum As Integer = 0
    Dim yr As Integer = 0
    Dim xloc As Integer
    Dim yloc As Integer
    Dim extra As String
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Public Sub New(ByVal x As Integer, y As Integer, week_num As Integer, yea As Integer, extra_ As String)

        ' This call is required by the designer.
        InitializeComponent()
        weeknum = week_num
        yr = yea
        xloc = x
        yloc = y
        extra = extra_
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

    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point
    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseDown, Panel1.MouseDown, Panel2.MouseDown, Label2.MouseDown ' Add more handles here (Example: PictureBox1.MouseDown)
        If e.Button = MouseButtons.Left Then
            MoveForm = True
            Me.Cursor = Cursors.NoMove2D
            MoveForm_MousePosition = e.Location
        End If
    End Sub

    Public Sub MoveForm_MouseMove(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseMove, Panel1.MouseMove, Panel2.MouseMove, Label2.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If
    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles _
    DoubleBufferedTableLayoutPanel1.MouseUp, Panel1.MouseUp, Panel2.MouseUp, Label2.MouseUp ' Add more handles here (Example: PictureBox1.MouseUp)
        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub TableLayoutPanel1_Click(sender As Object, e As EventArgs) Handles DoubleBufferedTableLayoutPanel1.Click, Panel1.Click, Panel2.Click, Label2.Click, AdvancedDataGridView1.Click
        timercounter = 0
    End Sub
    Private Sub productionWeekSum_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AdvancedDataGridView1.Visible = False
        Dim pic As New PictureBox
        pic.Image = My.Resources.rolling
        pic.SizeMode = PictureBoxSizeMode.CenterImage
        DoubleBufferedTableLayoutPanel1.Controls.Add(pic, DoubleBufferedTableLayoutPanel1.GetColumn(AdvancedDataGridView1), DoubleBufferedTableLayoutPanel1.GetRow(AdvancedDataGridView1))
        pic.Dock = DockStyle.Fill
        mainworker.RunWorkerAsync()
        SetLocation(xloc, yloc)
        Timer1.Start()
    End Sub

    Private Sub mainworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles mainworker.DoWork
        Dim selectaddcmd As String = ""
        Dim groupaddcmd As String = ""
        If extra.Contains("AND left(code,1)='1' and left(code,3)<>'102' ") Then
            selectaddcmd = " os 'Oversize',"
            groupaddcmd = ",os"
        End If
        Dim cmd As String = "SELECT left([code],9) 'Είδος' ,quality 'Ποιότητα', " + selectaddcmd + "
     sum([QUANTITY]) 'ΠΟΣ',
    sum([TMX]) 'ΤΜΧ'
  FROM [dbo].[PKRVIW_DAILYPLAN] dp   where weeknumber=" + weeknum.ToString + " and year(date)=" + yr.ToString + " " + extra + " group by left([code],9),quality " + groupaddcmd
        Using s As New SqlCommand(cmd, conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                End Using
                e.Result = dt
            End Using
        End Using
    End Sub

    Private Sub mainworker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles mainworker.RunWorkerCompleted
        AdvancedDataGridView1.DataSource = e.Result
        AdvancedDataGridViewSearchToolBar1.SetColumns(AdvancedDataGridView1.Columns)
        For Each c As Control In DoubleBufferedTableLayoutPanel1.Controls
            If Not IsNothing(TryCast(c, PictureBox)) Then
                c.Dispose()
            End If
        Next
        AdvancedDataGridView1.Visible = True
    End Sub

    Private Sub AdvancedDataGridView1_FilterStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView1.FilterStringChanged
        Try
            TryCast(AdvancedDataGridView1.DataSource, DataTable).DefaultView.RowFilter = AdvancedDataGridView1.FilterString
        Catch
        End Try
    End Sub

    Private Sub AdvancedDataGridView1_SortStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView1.SortStringChanged
        Try
            TryCast(AdvancedDataGridView1.DataSource, DataTable).DefaultView.Sort = AdvancedDataGridView1.SortString
        Catch
        End Try
    End Sub
    Private Sub AdvancedDataGridViewSearchToolBar1_Search(sender As Object, e As Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs) Handles AdvancedDataGridViewSearchToolBar1.Search
        Dim restartsearch As Boolean = True
        Dim startColumn As Integer = 0
        Dim startRow As Integer = 0
        If Not e.FromBegin Then
            Dim endcol As Boolean = AdvancedDataGridView1.CurrentCell.ColumnIndex + 1 >= AdvancedDataGridView1.ColumnCount
            Dim endrow As Boolean = AdvancedDataGridView1.CurrentCell.RowIndex + 1 >= AdvancedDataGridView1.RowCount

            If endcol AndAlso endrow Then
                startColumn = AdvancedDataGridView1.CurrentCell.ColumnIndex
                startRow = AdvancedDataGridView1.CurrentCell.RowIndex
            Else
                startColumn = If(endcol, 0, AdvancedDataGridView1.CurrentCell.ColumnIndex + 1)
                startRow = AdvancedDataGridView1.CurrentCell.RowIndex + (If(endcol, 1, 0))
            End If
        End If

        Dim c As DataGridViewCell = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), startRow, startColumn, e.WholeWord, e.CaseSensitive)
        If c Is Nothing AndAlso restartsearch Then
            c = AdvancedDataGridView1.FindCell(e.ValueToSearch, If(e.ColumnToSearch IsNot Nothing, e.ColumnToSearch.Name, Nothing), 0, 0, e.WholeWord, e.CaseSensitive)
        End If
        If c IsNot Nothing Then
            AdvancedDataGridView1.CurrentCell = c
        End If
    End Sub
End Class