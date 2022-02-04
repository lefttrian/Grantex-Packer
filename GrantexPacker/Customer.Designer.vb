<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Customer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DoubleBufferedTableLayoutPanel1 = New DoubleBufferedTableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New Zuby.ADGV.AdvancedDataGridView()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CUSIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.PALLETTYPEIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.PLTEMPLATE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SLTEMPLATE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PKRTBLCUSTOMERBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.AdvancedDataGridViewSearchToolBar1 = New Zuby.ADGV.AdvancedDataGridViewSearchToolBar()
        Me.PKRTBL_CUSTOMERTableAdapter = New TESTFINALDataSetTableAdapters.PKRTBL_CUSTOMERTableAdapter()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.DoubleBufferedTableLayoutPanel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PKRTBLCUSTOMERBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.DoubleBufferedTableLayoutPanel1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1144, 558)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(4, 530)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1136, 24)
        Me.Panel1.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button2.Image = Global.My.Resources.Resources.icons8_ok_16
        Me.Button2.Location = New System.Drawing.Point(937, 0)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(97, 24)
        Me.Button2.TabIndex = 3
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
        Me.Button1.Location = New System.Drawing.Point(1034, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(102, 24)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Κλείσιμο"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DoubleBufferedTableLayoutPanel1
        '
        Me.DoubleBufferedTableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.DoubleBufferedTableLayoutPanel1.ColumnCount = 1
        Me.DoubleBufferedTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.Panel2, 0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.DataGridView1, 0, 2)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.AdvancedDataGridViewSearchToolBar1, 0, 1)
        Me.DoubleBufferedTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DoubleBufferedTableLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.DoubleBufferedTableLayoutPanel1.Name = "DoubleBufferedTableLayoutPanel1"
        Me.DoubleBufferedTableLayoutPanel1.RowCount = 3
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Size = New System.Drawing.Size(1136, 519)
        Me.DoubleBufferedTableLayoutPanel1.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(5, 5)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1126, 34)
        Me.Panel2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(482, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ρυθμίσεις πελάτη - αντιστοίχιση τύπου παλέτας-φορμών εκτυπώσεων"
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.CUSIDDataGridViewTextBoxColumn, Me.PALLETTYPEIDDataGridViewTextBoxColumn, Me.PLTEMPLATE, Me.Column1, Me.Column2, Me.SLTEMPLATE})
        Me.DataGridView1.DataSource = Me.PKRTBLCUSTOMERBindingSource
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.FilterAndSortEnabled = True
        Me.DataGridView1.Location = New System.Drawing.Point(5, 79)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1126, 435)
        Me.DataGridView1.TabIndex = 4
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.MinimumWidth = 22
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ID.Visible = False
        '
        'CUSIDDataGridViewTextBoxColumn
        '
        Me.CUSIDDataGridViewTextBoxColumn.DataPropertyName = "CUSID"
        Me.CUSIDDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.CUSIDDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = True
        Me.CUSIDDataGridViewTextBoxColumn.HeaderText = "Πελάτης"
        Me.CUSIDDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.CUSIDDataGridViewTextBoxColumn.Name = "CUSIDDataGridViewTextBoxColumn"
        Me.CUSIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CUSIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.CUSIDDataGridViewTextBoxColumn.Width = 300
        '
        'PALLETTYPEIDDataGridViewTextBoxColumn
        '
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.DataPropertyName = "PALLETTYPEID"
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = True
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.HeaderText = "Τύπος παλέτας"
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.MaxDropDownItems = 20
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.Name = "PALLETTYPEIDDataGridViewTextBoxColumn"
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.PALLETTYPEIDDataGridViewTextBoxColumn.Width = 150
        '
        'PLTEMPLATE
        '
        Me.PLTEMPLATE.DataPropertyName = "PLTEMPLATE"
        Me.PLTEMPLATE.HeaderText = "Packing List"
        Me.PLTEMPLATE.MinimumWidth = 22
        Me.PLTEMPLATE.Name = "PLTEMPLATE"
        Me.PLTEMPLATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.PLTEMPLATE.Width = 150
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "OLTEMPLATE"
        Me.Column1.HeaderText = "Ετικέτες Ανοίγματος"
        Me.Column1.MinimumWidth = 22
        Me.Column1.Name = "Column1"
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.Column1.Width = 150
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "CLTEMPLATE"
        Me.Column2.HeaderText = "Ετικέτες Κλεισίματος"
        Me.Column2.MinimumWidth = 22
        Me.Column2.Name = "Column2"
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.Column2.Width = 150
        '
        'SLTEMPLATE
        '
        Me.SLTEMPLATE.DataPropertyName = "SLTEMPLATE"
        Me.SLTEMPLATE.HeaderText = "Ετικέτες Αποστολής"
        Me.SLTEMPLATE.MinimumWidth = 22
        Me.SLTEMPLATE.Name = "SLTEMPLATE"
        Me.SLTEMPLATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'PKRTBLCUSTOMERBindingSource
        '
        Me.PKRTBLCUSTOMERBindingSource.DataMember = "PKRTBL_CUSTOMER"
        Me.PKRTBLCUSTOMERBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'AdvancedDataGridViewSearchToolBar1
        '
        Me.AdvancedDataGridViewSearchToolBar1.AllowMerge = False
        Me.AdvancedDataGridViewSearchToolBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.AdvancedDataGridViewSearchToolBar1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.AdvancedDataGridViewSearchToolBar1.Location = New System.Drawing.Point(2, 44)
        Me.AdvancedDataGridViewSearchToolBar1.MaximumSize = New System.Drawing.Size(0, 27)
        Me.AdvancedDataGridViewSearchToolBar1.MinimumSize = New System.Drawing.Size(0, 27)
        Me.AdvancedDataGridViewSearchToolBar1.Name = "AdvancedDataGridViewSearchToolBar1"
        Me.AdvancedDataGridViewSearchToolBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.AdvancedDataGridViewSearchToolBar1.Size = New System.Drawing.Size(1132, 27)
        Me.AdvancedDataGridViewSearchToolBar1.TabIndex = 5
        Me.AdvancedDataGridViewSearchToolBar1.Text = "AdvancedDataGridViewSearchToolBar1"
        '
        'PKRTBL_CUSTOMERTableAdapter
        '
        Me.PKRTBL_CUSTOMERTableAdapter.ClearBeforeFill = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Filter = ".rdlc files|*.rdlc"
        Me.OpenFileDialog1.InitialDirectory = "\\192.1.1.7\common\ΤΡΙΑΝΤΑΦΥΛΛΟΥ ΛΕΥΤΕΡΗΣ\Grantex Packer\Reports"
        Me.OpenFileDialog1.Title = "Επιλέξτε αρχείο .rdlc"
        '
        'Customer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button1
        Me.ClientSize = New System.Drawing.Size(1144, 558)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "Customer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ρυθμίσεις πελάτη"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.DoubleBufferedTableLayoutPanel1.ResumeLayout(False)
        Me.DoubleBufferedTableLayoutPanel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PKRTBLCUSTOMERBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents DoubleBufferedTableLayoutPanel1 As DoubleBufferedTableLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents PKRTBLCUSTOMERBindingSource As BindingSource
    Friend WithEvents PKRTBL_CUSTOMERTableAdapter As TESTFINALDataSetTableAdapters.PKRTBL_CUSTOMERTableAdapter
    Friend WithEvents DataGridView1 As AdvancedDataGridView
    Friend WithEvents AdvancedDataGridViewSearchToolBar1 As AdvancedDataGridViewSearchToolBar
    Friend WithEvents ID As DataGridViewTextBoxColumn
    Friend WithEvents CUSIDDataGridViewTextBoxColumn As DataGridViewComboBoxColumn
    Friend WithEvents PALLETTYPEIDDataGridViewTextBoxColumn As DataGridViewComboBoxColumn
    Friend WithEvents PLTEMPLATE As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents SLTEMPLATE As DataGridViewTextBoxColumn
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
End Class
