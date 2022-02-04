Imports System.Drawing
Imports System.ComponentModel
Public Class DatagridviewStackedProgressColumn
    Inherits DataGridViewImageColumn
    'Από https://social.msdn.microsoft.com/Forums/windows/en-US/769ca9d6-1e9d-4d76-8c23-db535b2f19c2/sample-code-datagridview-progress-bar-column?forum=winformsdatacontrols

    Public Sub New()
        Me.CellTemplate = New DataGridViewStackedProgressCell
    End Sub
End Class
Public Class DataGridViewStackedProgressCell
    Inherits DataGridViewImageCell

    Sub New()
        ValueType = Type.GetType("String")
    End Sub
    ' Method required to make the Progress Cell consistent with the default Image Cell. 
    ' The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an Integer.
    Protected Overrides Function GetFormattedValue(
        ByVal value As Object,
        ByVal rowIndex As Integer,
        ByRef cellStyle As DataGridViewCellStyle,
        ByVal valueTypeConverter As TypeConverter,
        ByVal formattedValueTypeConverter As TypeConverter,
        ByVal context As DataGridViewDataErrorContexts
        ) As Object
        Static emptyImage As Bitmap = New Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        GetFormattedValue = emptyImage
    End Function

    Protected Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal clipBounds As System.Drawing.Rectangle, ByVal cellBounds As System.Drawing.Rectangle, ByVal rowIndex As Integer, ByVal cellState As System.Windows.Forms.DataGridViewElementStates, ByVal value As Object, ByVal formattedValue As Object, ByVal errorText As String, ByVal cellStyle As System.Windows.Forms.DataGridViewCellStyle, ByVal advancedBorderStyle As System.Windows.Forms.DataGridViewAdvancedBorderStyle, ByVal paintParts As System.Windows.Forms.DataGridViewPaintParts)
        Try
            Dim PreFormatVal As String = ""
            If IsNothing(value) Then
                Return
            Else
                PreFormatVal = CType(value, String)
            End If
            Dim progressVal As Integer = 0
            MyBase.Paint(g, clipBounds, cellBounds, rowIndex, cellState,
            value, formattedValue, errorText, cellStyle,
            advancedBorderStyle, paintParts)
            g.FillRectangle(New SolidBrush(Color.Black), cellBounds.X + 1, cellBounds.Y + 1, cellBounds.Width - 2, cellBounds.Height - 2)
            'Dim backBrush As Brush = New SolidBrush(cellStyle.BackColor)
            'Dim foreBrush As Brush = New SolidBrush(cellStyle.ForeColor)
            If PreFormatVal.Contains("/") Then
                Dim SplitVal As String() = PreFormatVal.Split("/")
                Dim total As Double = 0
                Dim values As New List(Of Double)
                Dim percents As New List(Of Double)
                Dim colors As New List(Of Color) From {Color.Red, Color.DarkGray, Color.LightBlue, Color.LightGreen, Color.Green, Color.Gold}
                Dim forecolors As New List(Of Color) From {Color.White, Color.Black, Color.Black, Color.Black, Color.White, Color.Black}
                For Each v As String In SplitVal
                    If v = "" Then
                        Continue For
                    Else
                        values.Add(CDbl(v))
                    End If

                Next
                total = values.Sum()
                For Each v As Double In values
                    percents.Add(v / total)
                Next
                Dim xpos As Integer = cellBounds.X + 2
                For i As Integer = 0 To percents.Count - 1
                    Dim col As Color = colors(i)
                    If percents(i) > 0 Then
                        Dim xlim As Integer = Convert.ToInt32((percents(i) * (cellBounds.Width - 4)))
                        g.FillRectangle(New SolidBrush(col), xpos, cellBounds.Y + 2, xlim, cellBounds.Height - 4)
                        Dim textsize As Size = TextRenderer.MeasureText(values(i).ToString, cellStyle.Font)
                        If textsize.Width < xlim Then
                            Using foreBrush As Brush = New SolidBrush(forecolors(i))
                                g.DrawString(values(i).ToString, cellStyle.Font, foreBrush, xpos + 2, cellBounds.Y + 2)
                            End Using
                        End If
                        xpos += xlim
                            'g.DrawString((percents(counter) * 100).ToString, cellStyle.Font, foreBrush, xpos, cellBounds.Y + 2)
                        End If
                Next
            Else
                Throw New Exception("Άκυρη μορφή δεδομένων στη στήλη StackedProgress")
            End If


            ' Call the base class method to paint the default cell appearance.

            'If percentage > 0.0 Then
            '    ' Draw the progress bar and the text
            '    g.FillRectangle(New SolidBrush(PreferredColor), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4)
            '    g.DrawString(text, cellStyle.Font, foreBrush, cellBounds.X + 6, cellBounds.Y + 2)
            'Else
            '    'draw the text
            '    If Not Me.DataGridView.CurrentCell Is Nothing AndAlso Me.DataGridView.CurrentCell.RowIndex = rowIndex Then
            '        g.DrawString(text, cellStyle.Font, New SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 2)
            '    Else
            '        g.DrawString(text, cellStyle.Font, foreBrush, cellBounds.X + 6, cellBounds.Y + 2)
            '    End If
            'End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
End Class


