<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CustomDGVSearchBox
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.DoubleBufferedTableLayoutPanel1 = New DoubleBufferedTableLayoutPanel()
        Me.NoPaddingButton1 = New NoPaddingButton()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.DoubleBufferedTableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DoubleBufferedTableLayoutPanel1
        '
        Me.DoubleBufferedTableLayoutPanel1.ColumnCount = 2
        Me.DoubleBufferedTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.NoPaddingButton1, 1, 0)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.TextBox1, 0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DoubleBufferedTableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Name = "DoubleBufferedTableLayoutPanel1"
        Me.DoubleBufferedTableLayoutPanel1.RowCount = 1
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Size = New System.Drawing.Size(298, 37)
        Me.DoubleBufferedTableLayoutPanel1.TabIndex = 0
        '
        'NoPaddingButton1
        '
        Me.NoPaddingButton1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NoPaddingButton1.Image = Global.My.Resources.Resources.icons8_search_161
        Me.NoPaddingButton1.Location = New System.Drawing.Point(261, 3)
        Me.NoPaddingButton1.Name = "NoPaddingButton1"
        Me.NoPaddingButton1.OwnerDrawText = Nothing
        Me.NoPaddingButton1.Size = New System.Drawing.Size(34, 31)
        Me.NoPaddingButton1.TabIndex = 0
        Me.NoPaddingButton1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(3, 3)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(252, 31)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CustomDGVSearchBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DoubleBufferedTableLayoutPanel1)
        Me.Name = "CustomDGVSearchBox"
        Me.Size = New System.Drawing.Size(298, 37)
        Me.DoubleBufferedTableLayoutPanel1.ResumeLayout(False)
        Me.DoubleBufferedTableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DoubleBufferedTableLayoutPanel1 As DoubleBufferedTableLayoutPanel
    Friend WithEvents NoPaddingButton1 As NoPaddingButton
    Friend WithEvents TextBox1 As TextBox
End Class
