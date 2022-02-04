<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UsertTransactions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UsertTransactions))
        Me.DoubleBufferedTableLayoutPanel1 = New DoubleBufferedTableLayoutPanel()
        Me.NoPaddingButton2 = New NoPaddingButton()
        Me.AdvancedDataGridView1 = New Zuby.ADGV.AdvancedDataGridView()
        Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.USERIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.USERNAMEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TRANTYPEIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PERTYPEID1DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PERTYPEID2DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TIMEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PERTYPEVALUEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.QUANTITYDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PERTYPEID3DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PHRASEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PKRVIWUSERTRANSACTIONSBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.AtlantisDataSet = New atlantisDataSet()
        Me.AdvancedDataGridViewSearchToolBar1 = New Zuby.ADGV.AdvancedDataGridViewSearchToolBar()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.NoPaddingButton1 = New NoPaddingButton()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PKRVIW_USERTRANSACTIONSTableAdapter = New atlantisDataSetTableAdapters.PKRVIW_USERTRANSACTIONSTableAdapter()
        Me.DoubleBufferedTableLayoutPanel1.SuspendLayout()
        CType(Me.AdvancedDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PKRVIWUSERTRANSACTIONSBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AtlantisDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DoubleBufferedTableLayoutPanel1
        '
        Me.DoubleBufferedTableLayoutPanel1.ColumnCount = 1
        Me.DoubleBufferedTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.NoPaddingButton2, 0, 3)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.AdvancedDataGridView1, 0, 2)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.AdvancedDataGridViewSearchToolBar1, 0, 1)
        Me.DoubleBufferedTableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DoubleBufferedTableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.DoubleBufferedTableLayoutPanel1.Name = "DoubleBufferedTableLayoutPanel1"
        Me.DoubleBufferedTableLayoutPanel1.RowCount = 4
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DoubleBufferedTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.DoubleBufferedTableLayoutPanel1.Size = New System.Drawing.Size(848, 535)
        Me.DoubleBufferedTableLayoutPanel1.TabIndex = 0
        '
        'NoPaddingButton2
        '
        Me.NoPaddingButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.NoPaddingButton2.Dock = System.Windows.Forms.DockStyle.Right
        Me.NoPaddingButton2.Image = Global.My.Resources.Resources.icons8_undo_161
        Me.NoPaddingButton2.Location = New System.Drawing.Point(754, 508)
        Me.NoPaddingButton2.Name = "NoPaddingButton2"
        Me.NoPaddingButton2.OwnerDrawText = Nothing
        Me.NoPaddingButton2.Size = New System.Drawing.Size(91, 24)
        Me.NoPaddingButton2.TabIndex = 5
        Me.NoPaddingButton2.Text = "Κλείσιμο"
        Me.NoPaddingButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.NoPaddingButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.NoPaddingButton2.UseVisualStyleBackColor = True
        '
        'AdvancedDataGridView1
        '
        Me.AdvancedDataGridView1.AllowUserToAddRows = False
        Me.AdvancedDataGridView1.AllowUserToDeleteRows = False
        Me.AdvancedDataGridView1.AutoGenerateColumns = False
        Me.AdvancedDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AdvancedDataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.USERIDDataGridViewTextBoxColumn, Me.USERNAMEDataGridViewTextBoxColumn, Me.TRANTYPEIDDataGridViewTextBoxColumn, Me.PERTYPEID1DataGridViewTextBoxColumn, Me.PERTYPEID2DataGridViewTextBoxColumn, Me.TIMEDataGridViewTextBoxColumn, Me.PERTYPEVALUEDataGridViewTextBoxColumn, Me.QUANTITYDataGridViewTextBoxColumn, Me.PERTYPEID3DataGridViewTextBoxColumn, Me.PHRASEDataGridViewTextBoxColumn})
        Me.AdvancedDataGridView1.DataSource = Me.PKRVIWUSERTRANSACTIONSBindingSource
        Me.AdvancedDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AdvancedDataGridView1.FilterAndSortEnabled = True
        Me.AdvancedDataGridView1.Location = New System.Drawing.Point(3, 63)
        Me.AdvancedDataGridView1.Name = "AdvancedDataGridView1"
        Me.AdvancedDataGridView1.ReadOnly = True
        Me.AdvancedDataGridView1.Size = New System.Drawing.Size(842, 439)
        Me.AdvancedDataGridView1.TabIndex = 0
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
        'USERIDDataGridViewTextBoxColumn
        '
        Me.USERIDDataGridViewTextBoxColumn.DataPropertyName = "USERID"
        Me.USERIDDataGridViewTextBoxColumn.HeaderText = "USERID"
        Me.USERIDDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.USERIDDataGridViewTextBoxColumn.Name = "USERIDDataGridViewTextBoxColumn"
        Me.USERIDDataGridViewTextBoxColumn.ReadOnly = True
        Me.USERIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.USERIDDataGridViewTextBoxColumn.Visible = False
        '
        'USERNAMEDataGridViewTextBoxColumn
        '
        Me.USERNAMEDataGridViewTextBoxColumn.DataPropertyName = "USERNAME"
        Me.USERNAMEDataGridViewTextBoxColumn.HeaderText = "Χρήστης"
        Me.USERNAMEDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.USERNAMEDataGridViewTextBoxColumn.Name = "USERNAMEDataGridViewTextBoxColumn"
        Me.USERNAMEDataGridViewTextBoxColumn.ReadOnly = True
        Me.USERNAMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'TRANTYPEIDDataGridViewTextBoxColumn
        '
        Me.TRANTYPEIDDataGridViewTextBoxColumn.DataPropertyName = "TRANTYPEID"
        Me.TRANTYPEIDDataGridViewTextBoxColumn.HeaderText = "TRANTYPEID"
        Me.TRANTYPEIDDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.TRANTYPEIDDataGridViewTextBoxColumn.Name = "TRANTYPEIDDataGridViewTextBoxColumn"
        Me.TRANTYPEIDDataGridViewTextBoxColumn.ReadOnly = True
        Me.TRANTYPEIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.TRANTYPEIDDataGridViewTextBoxColumn.Visible = False
        '
        'PERTYPEID1DataGridViewTextBoxColumn
        '
        Me.PERTYPEID1DataGridViewTextBoxColumn.DataPropertyName = "PERTYPEID1"
        Me.PERTYPEID1DataGridViewTextBoxColumn.HeaderText = "PERTYPEID1"
        Me.PERTYPEID1DataGridViewTextBoxColumn.MinimumWidth = 22
        Me.PERTYPEID1DataGridViewTextBoxColumn.Name = "PERTYPEID1DataGridViewTextBoxColumn"
        Me.PERTYPEID1DataGridViewTextBoxColumn.ReadOnly = True
        Me.PERTYPEID1DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.PERTYPEID1DataGridViewTextBoxColumn.Visible = False
        '
        'PERTYPEID2DataGridViewTextBoxColumn
        '
        Me.PERTYPEID2DataGridViewTextBoxColumn.DataPropertyName = "PERTYPEID2"
        Me.PERTYPEID2DataGridViewTextBoxColumn.HeaderText = "PERTYPEID2"
        Me.PERTYPEID2DataGridViewTextBoxColumn.MinimumWidth = 22
        Me.PERTYPEID2DataGridViewTextBoxColumn.Name = "PERTYPEID2DataGridViewTextBoxColumn"
        Me.PERTYPEID2DataGridViewTextBoxColumn.ReadOnly = True
        Me.PERTYPEID2DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.PERTYPEID2DataGridViewTextBoxColumn.Visible = False
        '
        'TIMEDataGridViewTextBoxColumn
        '
        Me.TIMEDataGridViewTextBoxColumn.DataPropertyName = "TIME"
        Me.TIMEDataGridViewTextBoxColumn.HeaderText = "Ημ/νια & ώρα"
        Me.TIMEDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.TIMEDataGridViewTextBoxColumn.Name = "TIMEDataGridViewTextBoxColumn"
        Me.TIMEDataGridViewTextBoxColumn.ReadOnly = True
        Me.TIMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'PERTYPEVALUEDataGridViewTextBoxColumn
        '
        Me.PERTYPEVALUEDataGridViewTextBoxColumn.DataPropertyName = "PERTYPEVALUE"
        Me.PERTYPEVALUEDataGridViewTextBoxColumn.HeaderText = "PERTYPEVALUE"
        Me.PERTYPEVALUEDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.PERTYPEVALUEDataGridViewTextBoxColumn.Name = "PERTYPEVALUEDataGridViewTextBoxColumn"
        Me.PERTYPEVALUEDataGridViewTextBoxColumn.ReadOnly = True
        Me.PERTYPEVALUEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.PERTYPEVALUEDataGridViewTextBoxColumn.Visible = False
        '
        'QUANTITYDataGridViewTextBoxColumn
        '
        Me.QUANTITYDataGridViewTextBoxColumn.DataPropertyName = "QUANTITY"
        Me.QUANTITYDataGridViewTextBoxColumn.HeaderText = "QUANTITY"
        Me.QUANTITYDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.QUANTITYDataGridViewTextBoxColumn.Name = "QUANTITYDataGridViewTextBoxColumn"
        Me.QUANTITYDataGridViewTextBoxColumn.ReadOnly = True
        Me.QUANTITYDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.QUANTITYDataGridViewTextBoxColumn.Visible = False
        '
        'PERTYPEID3DataGridViewTextBoxColumn
        '
        Me.PERTYPEID3DataGridViewTextBoxColumn.DataPropertyName = "PERTYPEID3"
        Me.PERTYPEID3DataGridViewTextBoxColumn.HeaderText = "PERTYPEID3"
        Me.PERTYPEID3DataGridViewTextBoxColumn.MinimumWidth = 22
        Me.PERTYPEID3DataGridViewTextBoxColumn.Name = "PERTYPEID3DataGridViewTextBoxColumn"
        Me.PERTYPEID3DataGridViewTextBoxColumn.ReadOnly = True
        Me.PERTYPEID3DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.PERTYPEID3DataGridViewTextBoxColumn.Visible = False
        '
        'PHRASEDataGridViewTextBoxColumn
        '
        Me.PHRASEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.PHRASEDataGridViewTextBoxColumn.DataPropertyName = "PHRASE"
        Me.PHRASEDataGridViewTextBoxColumn.HeaderText = "Περιγραφή κίνησης"
        Me.PHRASEDataGridViewTextBoxColumn.MinimumWidth = 22
        Me.PHRASEDataGridViewTextBoxColumn.Name = "PHRASEDataGridViewTextBoxColumn"
        Me.PHRASEDataGridViewTextBoxColumn.ReadOnly = True
        Me.PHRASEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.PHRASEDataGridViewTextBoxColumn.Width = 118
        '
        'PKRVIWUSERTRANSACTIONSBindingSource
        '
        Me.PKRVIWUSERTRANSACTIONSBindingSource.DataMember = "PKRVIW_USERTRANSACTIONS"
        Me.PKRVIWUSERTRANSACTIONSBindingSource.DataSource = Me.AtlantisDataSet
        '
        'AtlantisDataSet
        '
        Me.AtlantisDataSet.DataSetName = "atlantisDataSet"
        Me.AtlantisDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'AdvancedDataGridViewSearchToolBar1
        '
        Me.AdvancedDataGridViewSearchToolBar1.AllowMerge = False
        Me.AdvancedDataGridViewSearchToolBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.AdvancedDataGridViewSearchToolBar1.Location = New System.Drawing.Point(0, 30)
        Me.AdvancedDataGridViewSearchToolBar1.MaximumSize = New System.Drawing.Size(0, 27)
        Me.AdvancedDataGridViewSearchToolBar1.MinimumSize = New System.Drawing.Size(0, 27)
        Me.AdvancedDataGridViewSearchToolBar1.Name = "AdvancedDataGridViewSearchToolBar1"
        Me.AdvancedDataGridViewSearchToolBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.AdvancedDataGridViewSearchToolBar1.Size = New System.Drawing.Size(848, 27)
        Me.AdvancedDataGridViewSearchToolBar1.TabIndex = 1
        Me.AdvancedDataGridViewSearchToolBar1.Text = "AdvancedDataGridViewSearchToolBar1"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.NoPaddingButton1)
        Me.Panel1.Controls.Add(Me.DateTimePicker1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(842, 24)
        Me.Panel1.TabIndex = 2
        '
        'NoPaddingButton1
        '
        Me.NoPaddingButton1.Image = Global.My.Resources.Resources.icons8_search_16
        Me.NoPaddingButton1.Location = New System.Drawing.Point(585, 0)
        Me.NoPaddingButton1.Name = "NoPaddingButton1"
        Me.NoPaddingButton1.OwnerDrawText = Nothing
        Me.NoPaddingButton1.Size = New System.Drawing.Size(91, 23)
        Me.NoPaddingButton1.TabIndex = 4
        Me.NoPaddingButton1.Text = "Προβολή"
        Me.NoPaddingButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.NoPaddingButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.NoPaddingButton1.UseVisualStyleBackColor = True
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(379, 2)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker1.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(183, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(195, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Ημ/νία έναρξης εβδομάδας προβολής"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(56, 1)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Χρήστης"
        '
        'PKRVIW_USERTRANSACTIONSTableAdapter
        '
        Me.PKRVIW_USERTRANSACTIONSTableAdapter.ClearBeforeFill = True
        '
        'UsertTransactions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.NoPaddingButton2
        Me.ClientSize = New System.Drawing.Size(848, 535)
        Me.Controls.Add(Me.DoubleBufferedTableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "UsertTransactions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ιστορικό"
        Me.DoubleBufferedTableLayoutPanel1.ResumeLayout(False)
        Me.DoubleBufferedTableLayoutPanel1.PerformLayout()
        CType(Me.AdvancedDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PKRVIWUSERTRANSACTIONSBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AtlantisDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DoubleBufferedTableLayoutPanel1 As DoubleBufferedTableLayoutPanel
    Friend WithEvents AdvancedDataGridView1 As AdvancedDataGridView
    Friend WithEvents PKRVIWUSERTRANSACTIONSBindingSource As BindingSource
    Friend WithEvents AtlantisDataSet As atlantisDataSet
    Friend WithEvents AdvancedDataGridViewSearchToolBar1 As AdvancedDataGridViewSearchToolBar
    Friend WithEvents PKRVIW_USERTRANSACTIONSTableAdapter As atlantisDataSetTableAdapters.PKRVIW_USERTRANSACTIONSTableAdapter
    Friend WithEvents Panel1 As Panel
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents NoPaddingButton2 As NoPaddingButton
    Friend WithEvents NoPaddingButton1 As NoPaddingButton
    Friend WithEvents IDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents USERIDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents USERNAMEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents TRANTYPEIDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents PERTYPEID1DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents PERTYPEID2DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents TIMEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents PERTYPEVALUEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents QUANTITYDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents PERTYPEID3DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents PHRASEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
End Class
