<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PalletTypesDefaultsItem
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            conn.Dispose()
            updconn.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DoubleBufferedTableLayoutPanel1 = New DoubleBufferedTableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.NoPaddingButton1 = New NoPaddingButton()
        Me.titlelbl = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.quantitylbl = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pallettypelbl = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.variablevaluelbl = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.variablelbl = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.DoubleBufferedTableLayoutPanel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DoubleBufferedTableLayoutPanel1
        '
        Me.DoubleBufferedTableLayoutPanel1.ColumnCount = 1
        Me.DoubleBufferedTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.Panel2, 0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.DoubleBufferedTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DoubleBufferedTableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Name = "DoubleBufferedTableLayoutPanel1"
        Me.DoubleBufferedTableLayoutPanel1.RowCount = 2
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Size = New System.Drawing.Size(584, 78)
        Me.DoubleBufferedTableLayoutPanel1.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.NoPaddingButton1)
        Me.Panel2.Controls.Add(Me.titlelbl)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(578, 24)
        Me.Panel2.TabIndex = 9
        '
        'NoPaddingButton1
        '
        Me.NoPaddingButton1.Dock = System.Windows.Forms.DockStyle.Right
        Me.NoPaddingButton1.Image = Global.My.Resources.Resources.icons8_cancel_16
        Me.NoPaddingButton1.Location = New System.Drawing.Point(549, 0)
        Me.NoPaddingButton1.Name = "NoPaddingButton1"
        Me.NoPaddingButton1.OwnerDrawText = Nothing
        Me.NoPaddingButton1.Size = New System.Drawing.Size(29, 24)
        Me.NoPaddingButton1.TabIndex = 2
        Me.NoPaddingButton1.UseVisualStyleBackColor = True
        '
        'titlelbl
        '
        Me.titlelbl.AutoSize = True
        Me.titlelbl.Dock = System.Windows.Forms.DockStyle.Left
        Me.titlelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.titlelbl.Location = New System.Drawing.Point(0, 0)
        Me.titlelbl.Name = "titlelbl"
        Me.titlelbl.Size = New System.Drawing.Size(55, 16)
        Me.titlelbl.TabIndex = 1
        Me.titlelbl.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.quantitylbl)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.pallettypelbl)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.variablevaluelbl)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.variablelbl)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 33)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(578, 42)
        Me.Panel1.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(177, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Για είδη των οποίων η μεταβλητή "
        '
        'quantitylbl
        '
        Me.quantitylbl.AutoSize = True
        Me.quantitylbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.quantitylbl.Location = New System.Drawing.Point(129, 26)
        Me.quantitylbl.Name = "quantitylbl"
        Me.quantitylbl.Size = New System.Drawing.Size(61, 13)
        Me.quantitylbl.TabIndex = 7
        Me.quantitylbl.Text = "OOOOOO"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(4, 26)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(130, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "θα έχουν χωρητικότητα "
        '
        'pallettypelbl
        '
        Me.pallettypelbl.AutoSize = True
        Me.pallettypelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.pallettypelbl.Location = New System.Drawing.Point(95, 13)
        Me.pallettypelbl.Name = "pallettypelbl"
        Me.pallettypelbl.Size = New System.Drawing.Size(161, 13)
        Me.pallettypelbl.TabIndex = 5
        Me.pallettypelbl.Text = "ΟΜΑΔΑ ΕΙΔΟΥΣ OVERSIZE"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "η παλέτες τύπου "
        '
        'variablevaluelbl
        '
        Me.variablevaluelbl.AutoSize = True
        Me.variablevaluelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.variablevaluelbl.Location = New System.Drawing.Point(386, 0)
        Me.variablevaluelbl.Name = "variablevaluelbl"
        Me.variablevaluelbl.Size = New System.Drawing.Size(135, 13)
        Me.variablevaluelbl.TabIndex = 3
        Me.variablevaluelbl.Text = "ΟΟΟΟΟΟΟΟΟΟΟΟΟΟΟΟ"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(334, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "έχει τιμή "
        '
        'variablelbl
        '
        Me.variablelbl.AutoSize = True
        Me.variablelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.variablelbl.Location = New System.Drawing.Point(177, -1)
        Me.variablelbl.Name = "variablelbl"
        Me.variablelbl.Size = New System.Drawing.Size(161, 13)
        Me.variablelbl.TabIndex = 1
        Me.variablelbl.Text = "ΟΜΑΔΑ ΕΙΔΟΥΣ OVERSIZE"
        '
        'Timer1
        '
        Me.Timer1.Interval = 5000
        '
        'PalletTypesDefaultsItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.DoubleBufferedTableLayoutPanel1)
        Me.MaximumSize = New System.Drawing.Size(1272, 80)
        Me.MinimumSize = New System.Drawing.Size(586, 80)
        Me.Name = "PalletTypesDefaultsItem"
        Me.Size = New System.Drawing.Size(584, 78)
        Me.DoubleBufferedTableLayoutPanel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DoubleBufferedTableLayoutPanel1 As DoubleBufferedTableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents titlelbl As Label
    Friend WithEvents variablelbl As Label
    Friend WithEvents quantitylbl As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents pallettypelbl As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents variablevaluelbl As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents NoPaddingButton1 As NoPaddingButton
    Friend WithEvents Timer1 As Timer
End Class
