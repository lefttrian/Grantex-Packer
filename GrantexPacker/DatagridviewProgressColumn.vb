'Από https://social.msdn.microsoft.com/Forums/windows/en-US/769ca9d6-1e9d-4d76-8c23-db535b2f19c2/sample-code-datagridview-progress-bar-column?forum=winformsdatacontrols

Imports System.Drawing
Imports System.ComponentModel
Public Class DataGridViewProgressColumn
    Inherits DataGridViewImageColumn
    Public Sub New()
        Me.CellTemplate = New DataGridViewProgressCell
    End Sub
End Class
Public Class DataGridViewProgressCell
    Inherits DataGridViewImageCell

    Sub New()
        ValueType = Type.GetType("Integer")
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
        Dim PreFormatVal As String = ""
        Dim text As String = ""
        If Not IsNothing(value) Then
            PreFormatVal = CType(value, String)
        End If
        Dim progressVal As Integer = 0
        If PreFormatVal.Contains(",") Then
            Dim SplitVal As String() = PreFormatVal.Split(",")
            progressVal = CType(SplitVal(0), Integer)
            text = SplitVal(1)
        Else
            Try 'αν δε μπορέσει να το μετατρέψει σε integer, το αφήνει ως κέιμενο με τιμή 0 για τη progressbar
                progressVal = CType(value, Integer)
                text = progressVal.ToString & "%"
            Catch
                progressVal = 0
                text = value
            End Try

        End If
        Dim PreferredColor As Color
        If progressVal < 30 Then
            PreferredColor = Color.Red
        ElseIf progressVal = 100 Then
            PreferredColor = Color.LightGreen
        Else
            PreferredColor = Color.LightYellow
        End If
        Dim percentage As Single = CType((progressVal / 100), Single)
        Dim backBrush As Brush = New SolidBrush(cellStyle.BackColor)
        Dim foreBrush As Brush = New SolidBrush(cellStyle.ForeColor)
        ' Call the base class method to paint the default cell appearance.
        MyBase.Paint(g, clipBounds, cellBounds, rowIndex, cellState,
            value, formattedValue, errorText, cellStyle,
            advancedBorderStyle, paintParts)
        If percentage > 0.0 Then
            ' Draw the progress bar and the text
            g.FillRectangle(New SolidBrush(PreferredColor), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4)
            g.DrawString(text, cellStyle.Font, foreBrush, cellBounds.X + 6, cellBounds.Y + 2)
        Else
            'draw the text
            If Not Me.DataGridView.CurrentCell Is Nothing AndAlso Me.DataGridView.CurrentCell.RowIndex = rowIndex Then
                g.DrawString(text, cellStyle.Font, New SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 2)
            Else
                g.DrawString(text, cellStyle.Font, foreBrush, cellBounds.X + 6, cellBounds.Y + 2)
            End If
        End If
    End Sub
End Class
