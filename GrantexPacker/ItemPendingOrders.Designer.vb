<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ItemPendingOrders
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.mainworker = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button()
        Me.DoubleBufferedTableLayoutPanel1 = New DoubleBufferedTableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CustomDGVSearchBox1 = New CustomDGVSearchBox()
        Me.AdvancedDataGridView1 = New Zuby.ADGV.AdvancedDataGridView()
        Me.Panel1.SuspendLayout()
        Me.DoubleBufferedTableLayoutPanel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.AdvancedDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.DoubleBufferedTableLayoutPanel1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(785, 405)
        Me.Panel1.TabIndex = 0
        '
        'mainworker
        '
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Button2
        '
        Me.Button2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Button2.BackColor = System.Drawing.Color.Red
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(754, 1)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(20, 20)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Χ"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'DoubleBufferedTableLayoutPanel1
        '
        Me.DoubleBufferedTableLayoutPanel1.ColumnCount = 1
        Me.DoubleBufferedTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.Panel2, 0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.CustomDGVSearchBox1, 0, 1)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.AdvancedDataGridView1, 0, 2)
        Me.DoubleBufferedTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DoubleBufferedTableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Name = "DoubleBufferedTableLayoutPanel1"
        Me.DoubleBufferedTableLayoutPanel1.RowCount = 3
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Size = New System.Drawing.Size(783, 403)
        Me.DoubleBufferedTableLayoutPanel1.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(777, 24)
        Me.Panel2.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.Label4.Location = New System.Drawing.Point(729, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(19, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "99"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CustomDGVSearchBox1
        '
        Me.CustomDGVSearchBox1.custom_command = Nothing
        Me.CustomDGVSearchBox1.Custom_mode = True
        Me.CustomDGVSearchBox1.custom_parameters = Nothing
        Me.CustomDGVSearchBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CustomDGVSearchBox1.Location = New System.Drawing.Point(3, 33)
        Me.CustomDGVSearchBox1.Name = "CustomDGVSearchBox1"
        Me.CustomDGVSearchBox1.parent_datagridview = Me.AdvancedDataGridView1
        Me.CustomDGVSearchBox1.Size = New System.Drawing.Size(777, 29)
        Me.CustomDGVSearchBox1.TabIndex = 2
        '
        'AdvancedDataGridView1
        '
        Me.AdvancedDataGridView1.AllowUserToAddRows = False
        Me.AdvancedDataGridView1.AllowUserToDeleteRows = False
        Me.AdvancedDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.AdvancedDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AdvancedDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AdvancedDataGridView1.FilterAndSortEnabled = True
        Me.AdvancedDataGridView1.Location = New System.Drawing.Point(3, 68)
        Me.AdvancedDataGridView1.Name = "AdvancedDataGridView1"
        Me.AdvancedDataGridView1.ReadOnly = True
        Me.AdvancedDataGridView1.RowHeadersVisible = False
        Me.AdvancedDataGridView1.Size = New System.Drawing.Size(777, 332)
        Me.AdvancedDataGridView1.TabIndex = 3
        '
        'ItemPendingOrders
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button2
        Me.ClientSize = New System.Drawing.Size(785, 405)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ItemPendingOrders"
        Me.Text = "ItemDistribution"
        Me.Panel1.ResumeLayout(False)
        Me.DoubleBufferedTableLayoutPanel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.AdvancedDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents DoubleBufferedTableLayoutPanel1 As DoubleBufferedTableLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents CustomDGVSearchBox1 As CustomDGVSearchBox
    Friend WithEvents AdvancedDataGridView1 As AdvancedDataGridView
    Friend WithEvents mainworker As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As Timer
End Class
