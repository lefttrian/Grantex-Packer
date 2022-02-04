<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PalletTypes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PalletTypes))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New Zuby.ADGV.AdvancedDataGridView()
        Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DESCRIPTIONDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.WIDTHDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LENGTHDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HEIGHTDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.buttonclmn = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.PKRTBLPALLETTYPESBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.AdvancedDataGridViewSearchToolBar1 = New Zuby.ADGV.AdvancedDataGridViewSearchToolBar()
        Me.PKRTBL_PALLETTYPESTableAdapter = New TESTFINALDataSetTableAdapters.PKRTBL_PALLETTYPESTableAdapter()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PKRTBLPALLETTYPESBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.DataGridView1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.AdvancedDataGridViewSearchToolBar1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(524, 481)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Τύποι παλετών"
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.DESCRIPTIONDataGridViewTextBoxColumn, Me.WIDTHDataGridViewTextBoxColumn, Me.LENGTHDataGridViewTextBoxColumn, Me.HEIGHTDataGridViewTextBoxColumn, Me.buttonclmn})
        Me.DataGridView1.DataSource = Me.PKRTBLPALLETTYPESBindingSource
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.FilterAndSortEnabled = True
        Me.DataGridView1.Location = New System.Drawing.Point(5, 59)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(514, 385)
        Me.DataGridView1.TabIndex = 3
        '
        'IDDataGridViewTextBoxColumn
        '
        Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
        Me.IDDataGridViewTextBoxColumn.HeaderText = "ID"
        Me.IDDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
        Me.IDDataGridViewTextBoxColumn.ReadOnly = True
        Me.IDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.IDDataGridViewTextBoxColumn.Visible = False
        '
        'DESCRIPTIONDataGridViewTextBoxColumn
        '
        Me.DESCRIPTIONDataGridViewTextBoxColumn.DataPropertyName = "DESCRIPTION"
        Me.DESCRIPTIONDataGridViewTextBoxColumn.HeaderText = "DESCRIPTION"
        Me.DESCRIPTIONDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.DESCRIPTIONDataGridViewTextBoxColumn.Name = "DESCRIPTIONDataGridViewTextBoxColumn"
        Me.DESCRIPTIONDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'WIDTHDataGridViewTextBoxColumn
        '
        Me.WIDTHDataGridViewTextBoxColumn.DataPropertyName = "WIDTH"
        Me.WIDTHDataGridViewTextBoxColumn.HeaderText = "WIDTH"
        Me.WIDTHDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.WIDTHDataGridViewTextBoxColumn.Name = "WIDTHDataGridViewTextBoxColumn"
        Me.WIDTHDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'LENGTHDataGridViewTextBoxColumn
        '
        Me.LENGTHDataGridViewTextBoxColumn.DataPropertyName = "LENGTH"
        Me.LENGTHDataGridViewTextBoxColumn.HeaderText = "LENGTH"
        Me.LENGTHDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.LENGTHDataGridViewTextBoxColumn.Name = "LENGTHDataGridViewTextBoxColumn"
        Me.LENGTHDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'HEIGHTDataGridViewTextBoxColumn
        '
        Me.HEIGHTDataGridViewTextBoxColumn.DataPropertyName = "HEIGHT"
        Me.HEIGHTDataGridViewTextBoxColumn.HeaderText = "HEIGHT"
        Me.HEIGHTDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.HEIGHTDataGridViewTextBoxColumn.Name = "HEIGHTDataGridViewTextBoxColumn"
        Me.HEIGHTDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'buttonclmn
        '
        Me.buttonclmn.HeaderText = "Ποσότητες"
        Me.buttonclmn.MinimumWidth = 22
        Me.buttonclmn.Name = "buttonclmn"
        Me.buttonclmn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.buttonclmn.Text = ""
        Me.buttonclmn.Visible = False
        Me.buttonclmn.Width = 70
        '
        'PKRTBLPALLETTYPESBindingSource
        '
        Me.PKRTBLPALLETTYPESBindingSource.DataMember = "PKRTBL_PALLETTYPES"
        Me.PKRTBLPALLETTYPESBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(5, 452)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(514, 24)
        Me.Panel1.TabIndex = 4
        '
        'Button2
        '
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button2.Image = Global.My.Resources.Resources.icons8_ok_16
        Me.Button2.Location = New System.Drawing.Point(315, 0)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(97, 24)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Αποθήκευση"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button1.Image = Global.My.Resources.Resources.icons8_undo_16
        Me.Button1.Location = New System.Drawing.Point(412, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(102, 24)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Κλείσιμο"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'AdvancedDataGridViewSearchToolBar1
        '
        Me.AdvancedDataGridViewSearchToolBar1.AllowMerge = False
        Me.AdvancedDataGridViewSearchToolBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.AdvancedDataGridViewSearchToolBar1.Location = New System.Drawing.Point(2, 24)
        Me.AdvancedDataGridViewSearchToolBar1.MaximumSize = New System.Drawing.Size(0, 27)
        Me.AdvancedDataGridViewSearchToolBar1.MinimumSize = New System.Drawing.Size(0, 27)
        Me.AdvancedDataGridViewSearchToolBar1.Name = "AdvancedDataGridViewSearchToolBar1"
        Me.AdvancedDataGridViewSearchToolBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.AdvancedDataGridViewSearchToolBar1.Size = New System.Drawing.Size(520, 27)
        Me.AdvancedDataGridViewSearchToolBar1.TabIndex = 5
        Me.AdvancedDataGridViewSearchToolBar1.Text = "AdvancedDataGridViewSearchToolBar1"
        '
        'PKRTBL_PALLETTYPESTableAdapter
        '
        Me.PKRTBL_PALLETTYPESTableAdapter.ClearBeforeFill = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "ID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "ID"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "DESCRIPTION"
        Me.DataGridViewTextBoxColumn2.HeaderText = "DESCRIPTION"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "WIDTH"
        Me.DataGridViewTextBoxColumn3.HeaderText = "WIDTH"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "LENGTH"
        Me.DataGridViewTextBoxColumn4.HeaderText = "LENGTH"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "HEIGHT"
        Me.DataGridViewTextBoxColumn5.HeaderText = "HEIGHT"
        Me.DataGridViewTextBoxColumn5.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'PalletTypes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button1
        Me.ClientSize = New System.Drawing.Size(524, 481)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(940, 520)
        Me.MinimumSize = New System.Drawing.Size(540, 520)
        Me.Name = "PalletTypes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Τύποι παλετών"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PKRTBLPALLETTYPESBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label1 As Label
    Friend WithEvents DataGridView1 As Zuby.ADGV.AdvancedDataGridView
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents PKRTBLPALLETTYPESBindingSource As BindingSource
    Friend WithEvents PKRTBL_PALLETTYPESTableAdapter As TESTFINALDataSetTableAdapters.PKRTBL_PALLETTYPESTableAdapter
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents AdvancedDataGridViewSearchToolBar1 As AdvancedDataGridViewSearchToolBar
    Friend WithEvents IDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DESCRIPTIONDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents WIDTHDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LENGTHDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents HEIGHTDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents buttonclmn As DataGridViewButtonColumn
End Class
