Public Class DataGridViewTabPage
    Inherits TabPage
    Dim disposed As Boolean = False
    Private _grid As New DataGridView

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
        End If

        For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
            Dim ctrl As Control = Me.Controls(i)
            ctrl.Dispose()
        Next
        disposed = True

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Sub New(ByVal text As String)
        MyBase.New(text)

        'Set properties of _grid here, e.g.
        Me._grid.Dock = DockStyle.Fill

        'Add the grid to the page.
        Me.Controls.Add(Me._grid)
        With Me._grid
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AutoGenerateColumns = True
            .ShowCellToolTips = True
        End With
        AddHandler Me._grid.CellDoubleClick, AddressOf dgvcdc
    End Sub

    Public Sub dgvcdc(ByVal sender As Object, e As DataGridViewCellEventArgs)

        If e.ColumnIndex = 2 Then
            If sender.rows(e.RowIndex).cells(2).value.ToString.Contains("ΠΑΞ") Then
                Using frm As New Order(sender.rows(e.RowIndex).cells(8).value)
                    frm.ShowDialog()
                End Using
            Else
                Using frm As New PalletDetails(sender.rows(e.RowIndex).cells(0).value)
                    frm.ShowDialog()
                End Using
            End If


        ElseIf e.ColumnIndex = 3 Or e.ColumnIndex = 4 Then
            Using frm As New ItemDetails(sender.rows(e.RowIndex).cells(1).value)
                frm.ShowDialog()
            End Using

        End If
    End Sub


    Public Property datasource As DataTable
        Get
            Return Me._grid.DataSource
        End Get
        Set(value As DataTable)
            Me._grid.DataSource = value
            'Me._grid.Columns.Add(New DataGridViewImageColumn)
            'For i As Integer = 0 To _grid.Rows.Count - 1
            '    _grid.Rows(i).Cells(_grid.Columns.Count - 1).Value = My.Resources.REMOVEPALLET
            'Next
        End Set
    End Property
    Dim h As Boolean = False
    Public Property hide As Boolean
        Get
            Return h
        End Get
        Set(value As Boolean)
            h = value
            If h Then
                Me._grid.Columns(0).Visible = False
                Me._grid.Columns(1).Visible = False
                Me._grid.Columns(8).Visible = False
            Else
                Me._grid.Columns(0).Visible = True
                Me._grid.Columns(1).Visible = True
                Me._grid.Columns(8).Visible = True
            End If

        End Set

    End Property



End Class