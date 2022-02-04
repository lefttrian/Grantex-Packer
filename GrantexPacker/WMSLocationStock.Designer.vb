<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WMSLocationStock
    Inherits System.Windows.Forms.Form


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WMSLocationStock))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ITEIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LOCATIONCODEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LSUMQTYDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LQTYFREEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SCQTYMANTISAXBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.ITEIDDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LocationCodeDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LSUMQTYDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LQTYFREEDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SCQTYMANTISAXRETURNSBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.SC_QTY_MANTISAXTableAdapter = New TESTFINALDataSetTableAdapters.SC_QTY_MANTISAXTableAdapter()
        Me.SC_QTY_MANTISAX_RETURNSTableAdapter = New TESTFINALDataSetTableAdapters.SC_QTY_MANTISAX_RETURNSTableAdapter()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ExtCursor1 = New ExtCursors.ExtCursor()
        Me.Button3 = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SCQTYMANTISAXBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SCQTYMANTISAXRETURNSBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Label1"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ITEIDDataGridViewTextBoxColumn, Me.LOCATIONCODEDataGridViewTextBoxColumn, Me.LSUMQTYDataGridViewTextBoxColumn, Me.LQTYFREEDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.SCQTYMANTISAXBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(8, 49)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.Size = New System.Drawing.Size(287, 234)
        Me.DataGridView1.TabIndex = 1
        '
        'ITEIDDataGridViewTextBoxColumn
        '
        Me.ITEIDDataGridViewTextBoxColumn.DataPropertyName = "ITEID"
        Me.ITEIDDataGridViewTextBoxColumn.HeaderText = "ITEID"
        Me.ITEIDDataGridViewTextBoxColumn.Name = "ITEIDDataGridViewTextBoxColumn"
        Me.ITEIDDataGridViewTextBoxColumn.ReadOnly = True
        Me.ITEIDDataGridViewTextBoxColumn.Visible = False
        '
        'LOCATIONCODEDataGridViewTextBoxColumn
        '
        Me.LOCATIONCODEDataGridViewTextBoxColumn.DataPropertyName = "LOCATIONCODE"
        Me.LOCATIONCODEDataGridViewTextBoxColumn.HeaderText = "Θέση"
        Me.LOCATIONCODEDataGridViewTextBoxColumn.Name = "LOCATIONCODEDataGridViewTextBoxColumn"
        Me.LOCATIONCODEDataGridViewTextBoxColumn.ReadOnly = True
        Me.LOCATIONCODEDataGridViewTextBoxColumn.Width = 150
        '
        'LSUMQTYDataGridViewTextBoxColumn
        '
        Me.LSUMQTYDataGridViewTextBoxColumn.DataPropertyName = "LSUMQTY"
        Me.LSUMQTYDataGridViewTextBoxColumn.HeaderText = "Ποσότητα"
        Me.LSUMQTYDataGridViewTextBoxColumn.Name = "LSUMQTYDataGridViewTextBoxColumn"
        Me.LSUMQTYDataGridViewTextBoxColumn.ReadOnly = True
        Me.LSUMQTYDataGridViewTextBoxColumn.Width = 150
        '
        'LQTYFREEDataGridViewTextBoxColumn
        '
        Me.LQTYFREEDataGridViewTextBoxColumn.DataPropertyName = "LQTYFREE"
        Me.LQTYFREEDataGridViewTextBoxColumn.HeaderText = "LQTYFREE"
        Me.LQTYFREEDataGridViewTextBoxColumn.Name = "LQTYFREEDataGridViewTextBoxColumn"
        Me.LQTYFREEDataGridViewTextBoxColumn.ReadOnly = True
        Me.LQTYFREEDataGridViewTextBoxColumn.Visible = False
        '
        'SCQTYMANTISAXBindingSource
        '
        Me.SCQTYMANTISAXBindingSource.DataMember = "SC_QTY_MANTISAX"
        Me.SCQTYMANTISAXBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView2.AutoGenerateColumns = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ITEIDDataGridViewTextBoxColumn1, Me.LocationCodeDataGridViewTextBoxColumn1, Me.LSUMQTYDataGridViewTextBoxColumn1, Me.LQTYFREEDataGridViewTextBoxColumn1})
        Me.DataGridView2.DataSource = Me.SCQTYMANTISAXRETURNSBindingSource
        Me.DataGridView2.Location = New System.Drawing.Point(301, 49)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(287, 234)
        Me.DataGridView2.TabIndex = 3
        '
        'ITEIDDataGridViewTextBoxColumn1
        '
        Me.ITEIDDataGridViewTextBoxColumn1.DataPropertyName = "ITEID"
        Me.ITEIDDataGridViewTextBoxColumn1.HeaderText = "ITEID"
        Me.ITEIDDataGridViewTextBoxColumn1.Name = "ITEIDDataGridViewTextBoxColumn1"
        Me.ITEIDDataGridViewTextBoxColumn1.ReadOnly = True
        Me.ITEIDDataGridViewTextBoxColumn1.Visible = False
        '
        'LocationCodeDataGridViewTextBoxColumn1
        '
        Me.LocationCodeDataGridViewTextBoxColumn1.DataPropertyName = "LocationCode"
        Me.LocationCodeDataGridViewTextBoxColumn1.HeaderText = "Θέση"
        Me.LocationCodeDataGridViewTextBoxColumn1.Name = "LocationCodeDataGridViewTextBoxColumn1"
        Me.LocationCodeDataGridViewTextBoxColumn1.ReadOnly = True
        Me.LocationCodeDataGridViewTextBoxColumn1.Width = 150
        '
        'LSUMQTYDataGridViewTextBoxColumn1
        '
        Me.LSUMQTYDataGridViewTextBoxColumn1.DataPropertyName = "LSUMQTY"
        Me.LSUMQTYDataGridViewTextBoxColumn1.HeaderText = "Ποσότητα"
        Me.LSUMQTYDataGridViewTextBoxColumn1.Name = "LSUMQTYDataGridViewTextBoxColumn1"
        Me.LSUMQTYDataGridViewTextBoxColumn1.ReadOnly = True
        Me.LSUMQTYDataGridViewTextBoxColumn1.Width = 150
        '
        'LQTYFREEDataGridViewTextBoxColumn1
        '
        Me.LQTYFREEDataGridViewTextBoxColumn1.DataPropertyName = "LQTYFREE"
        Me.LQTYFREEDataGridViewTextBoxColumn1.HeaderText = "LQTYFREE"
        Me.LQTYFREEDataGridViewTextBoxColumn1.Name = "LQTYFREEDataGridViewTextBoxColumn1"
        Me.LQTYFREEDataGridViewTextBoxColumn1.ReadOnly = True
        Me.LQTYFREEDataGridViewTextBoxColumn1.Visible = False
        '
        'SCQTYMANTISAXRETURNSBindingSource
        '
        Me.SCQTYMANTISAXRETURNSBindingSource.DataMember = "SC_QTY_MANTISAX_RETURNS"
        Me.SCQTYMANTISAXRETURNSBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'SC_QTY_MANTISAXTableAdapter
        '
        Me.SC_QTY_MANTISAXTableAdapter.ClearBeforeFill = True
        '
        'SC_QTY_MANTISAX_RETURNSTableAdapter
        '
        Me.SC_QTY_MANTISAX_RETURNSTableAdapter.ClearBeforeFill = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Διαθέσιμα"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(298, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(135, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Δεσμευμένα/επιστροφές"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.Image = Global.My.Resources.Resources.icons8_undo_16
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(514, 300)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Κλείσιμο"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button2.Image = Global.My.Resources.Resources.icons8_module_16
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(8, 300)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(184, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Επισκόπηση σύνθεσης είδους"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button3.AutoSize = True
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Button3.Location = New System.Drawing.Point(198, 300)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(84, 23)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "1"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Form12
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button1
        Me.ClientSize = New System.Drawing.Size(603, 326)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form12"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Θέσεις αποθήκευσης είδους - Grantex Packer®"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SCQTYMANTISAXBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SCQTYMANTISAXRETURNSBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents SCQTYMANTISAXBindingSource As BindingSource
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents SCQTYMANTISAXRETURNSBindingSource As BindingSource
    Friend WithEvents SC_QTY_MANTISAXTableAdapter As TESTFINALDataSetTableAdapters.SC_QTY_MANTISAXTableAdapter
    Friend WithEvents SC_QTY_MANTISAX_RETURNSTableAdapter As TESTFINALDataSetTableAdapters.SC_QTY_MANTISAX_RETURNSTableAdapter
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ITEIDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LOCATIONCODEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LSUMQTYDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LQTYFREEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ITEIDDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents LocationCodeDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents LSUMQTYDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents LQTYFREEDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents Button2 As Button
    Friend WithEvents ExtCursor1 As ExtCursors.ExtCursor
    Friend WithEvents Button3 As Button
End Class
