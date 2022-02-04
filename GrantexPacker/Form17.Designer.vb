<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form17
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            ExtramantisinfoTableAdapter.Dispose()
            ExtramantisinfoBindingSource.Dispose()
            ExtramantisinforeturnsBindingSource.Dispose()
            Extramantisinfo_returnsTableAdapter.Dispose()
            'TESTFINALDataSet.Dispose()
            ExtCursor1.Dispose()
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form17))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.CODEDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SUBCODE2DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LOCATIONCODEDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LSUMQTYDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LQTYFREEDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ExtramantisinforeturnsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.CODEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SUBCODE2DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LOCATIONCODEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.descr = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LSUMQTYDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LQTYFREEDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ExtramantisinfoBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ExtCursor1 = New ExtCursors.ExtCursor()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ExtramantisinfoTableAdapter = New TESTFINALDataSetTableAdapters.extramantisinfoTableAdapter()
        Me.Extramantisinfo_returnsTableAdapter = New TESTFINALDataSetTableAdapters.extramantisinfo_returnsTableAdapter()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtramantisinforeturnsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtramantisinfoBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(514, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(135, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Δεσμευμένα/επιστροφές"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Διαθέσιμα"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.Image = Global.My.Resources.Resources.undo2
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(945, 577)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 32)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Κλείσιμο"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView2.AutoGenerateColumns = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CODEDataGridViewTextBoxColumn1, Me.SUBCODE2DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn1, Me.LOCATIONCODEDataGridViewTextBoxColumn1, Me.LSUMQTYDataGridViewTextBoxColumn1, Me.LQTYFREEDataGridViewTextBoxColumn1})
        Me.DataGridView2.DataSource = Me.ExtramantisinforeturnsBindingSource
        Me.DataGridView2.Location = New System.Drawing.Point(517, 49)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(503, 522)
        Me.DataGridView2.TabIndex = 11
        '
        'CODEDataGridViewTextBoxColumn1
        '
        Me.CODEDataGridViewTextBoxColumn1.DataPropertyName = "CODE"
        Me.CODEDataGridViewTextBoxColumn1.HeaderText = "ΚΩΔΙΚΟΣ"
        Me.CODEDataGridViewTextBoxColumn1.Name = "CODEDataGridViewTextBoxColumn1"
        Me.CODEDataGridViewTextBoxColumn1.ReadOnly = True
        '
        'SUBCODE2DataGridViewTextBoxColumn1
        '
        Me.SUBCODE2DataGridViewTextBoxColumn1.DataPropertyName = "SUBCODE2"
        Me.SUBCODE2DataGridViewTextBoxColumn1.HeaderText = "ΠΑΛ ΚΩΔΙΚΟΣ"
        Me.SUBCODE2DataGridViewTextBoxColumn1.Name = "SUBCODE2DataGridViewTextBoxColumn1"
        Me.SUBCODE2DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "descr"
        Me.DataGridViewTextBoxColumn1.HeaderText = "ΜΑΡΚΑ"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'LOCATIONCODEDataGridViewTextBoxColumn1
        '
        Me.LOCATIONCODEDataGridViewTextBoxColumn1.DataPropertyName = "LOCATIONCODE"
        Me.LOCATIONCODEDataGridViewTextBoxColumn1.HeaderText = "ΘΕΣΗ"
        Me.LOCATIONCODEDataGridViewTextBoxColumn1.Name = "LOCATIONCODEDataGridViewTextBoxColumn1"
        Me.LOCATIONCODEDataGridViewTextBoxColumn1.ReadOnly = True
        '
        'LSUMQTYDataGridViewTextBoxColumn1
        '
        Me.LSUMQTYDataGridViewTextBoxColumn1.DataPropertyName = "LSUMQTY"
        Me.LSUMQTYDataGridViewTextBoxColumn1.HeaderText = "ΣΥΝ ΠΟΣΟΤΗΤΑ"
        Me.LSUMQTYDataGridViewTextBoxColumn1.Name = "LSUMQTYDataGridViewTextBoxColumn1"
        Me.LSUMQTYDataGridViewTextBoxColumn1.ReadOnly = True
        '
        'LQTYFREEDataGridViewTextBoxColumn1
        '
        Me.LQTYFREEDataGridViewTextBoxColumn1.DataPropertyName = "LQTYFREE"
        Me.LQTYFREEDataGridViewTextBoxColumn1.HeaderText = "ΕΛΕΥΘ ΠΟΣΟΤΗΤΑ"
        Me.LQTYFREEDataGridViewTextBoxColumn1.Name = "LQTYFREEDataGridViewTextBoxColumn1"
        Me.LQTYFREEDataGridViewTextBoxColumn1.ReadOnly = True
        '
        'ExtramantisinforeturnsBindingSource
        '
        Me.ExtramantisinforeturnsBindingSource.DataMember = "extramantisinfo_returns"
        Me.ExtramantisinforeturnsBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CODEDataGridViewTextBoxColumn, Me.SUBCODE2DataGridViewTextBoxColumn, Me.LOCATIONCODEDataGridViewTextBoxColumn, Me.descr, Me.LSUMQTYDataGridViewTextBoxColumn, Me.LQTYFREEDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.ExtramantisinfoBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(8, 49)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.Size = New System.Drawing.Size(505, 522)
        Me.DataGridView1.TabIndex = 9
        '
        'CODEDataGridViewTextBoxColumn
        '
        Me.CODEDataGridViewTextBoxColumn.DataPropertyName = "CODE"
        Me.CODEDataGridViewTextBoxColumn.HeaderText = "ΚΩΔΙΚΟΣ"
        Me.CODEDataGridViewTextBoxColumn.Name = "CODEDataGridViewTextBoxColumn"
        Me.CODEDataGridViewTextBoxColumn.ReadOnly = True
        '
        'SUBCODE2DataGridViewTextBoxColumn
        '
        Me.SUBCODE2DataGridViewTextBoxColumn.DataPropertyName = "SUBCODE2"
        Me.SUBCODE2DataGridViewTextBoxColumn.HeaderText = "ΠΑΛ.ΚΩΔΙΚΟΣ"
        Me.SUBCODE2DataGridViewTextBoxColumn.Name = "SUBCODE2DataGridViewTextBoxColumn"
        Me.SUBCODE2DataGridViewTextBoxColumn.ReadOnly = True
        '
        'LOCATIONCODEDataGridViewTextBoxColumn
        '
        Me.LOCATIONCODEDataGridViewTextBoxColumn.DataPropertyName = "LOCATIONCODE"
        Me.LOCATIONCODEDataGridViewTextBoxColumn.HeaderText = "ΘΕΣΗ"
        Me.LOCATIONCODEDataGridViewTextBoxColumn.Name = "LOCATIONCODEDataGridViewTextBoxColumn"
        Me.LOCATIONCODEDataGridViewTextBoxColumn.ReadOnly = True
        '
        'descr
        '
        Me.descr.DataPropertyName = "descr"
        Me.descr.HeaderText = "ΜΑΡΚΑ"
        Me.descr.Name = "descr"
        Me.descr.ReadOnly = True
        '
        'LSUMQTYDataGridViewTextBoxColumn
        '
        Me.LSUMQTYDataGridViewTextBoxColumn.DataPropertyName = "LSUMQTY"
        Me.LSUMQTYDataGridViewTextBoxColumn.HeaderText = "ΣΥΝ ΠΟΣΟΤΗΤΑ"
        Me.LSUMQTYDataGridViewTextBoxColumn.Name = "LSUMQTYDataGridViewTextBoxColumn"
        Me.LSUMQTYDataGridViewTextBoxColumn.ReadOnly = True
        '
        'LQTYFREEDataGridViewTextBoxColumn
        '
        Me.LQTYFREEDataGridViewTextBoxColumn.DataPropertyName = "LQTYFREE"
        Me.LQTYFREEDataGridViewTextBoxColumn.HeaderText = "ΕΛΕΥΘ ΠΟΣΟΤΗΤΑ"
        Me.LQTYFREEDataGridViewTextBoxColumn.Name = "LQTYFREEDataGridViewTextBoxColumn"
        Me.LQTYFREEDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ExtramantisinfoBindingSource
        '
        Me.ExtramantisinfoBindingSource.DataMember = "extramantisinfo"
        Me.ExtramantisinfoBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'ExtCursor1
        '
        Me.ExtCursor1.CursorStream = CType(resources.GetObject("ExtCursor1.CursorStream"), ExtCursors.ExtCursorFileStreamer)
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
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Label1"
        '
        'ExtramantisinfoTableAdapter
        '
        Me.ExtramantisinfoTableAdapter.ClearBeforeFill = True
        '
        'Extramantisinfo_returnsTableAdapter
        '
        Me.Extramantisinfo_returnsTableAdapter.ClearBeforeFill = True
        '
        'Form17
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button1
        Me.ClientSize = New System.Drawing.Size(1032, 621)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form17"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form17"
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtramantisinforeturnsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtramantisinfoBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents ExtCursor1 As ExtCursors.ExtCursor
    Friend WithEvents Label1 As Label
    Friend WithEvents ExtramantisinforeturnsBindingSource As BindingSource
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents ExtramantisinfoBindingSource As BindingSource
    Friend WithEvents ExtramantisinfoTableAdapter As TESTFINALDataSetTableAdapters.extramantisinfoTableAdapter
    Friend WithEvents Extramantisinfo_returnsTableAdapter As TESTFINALDataSetTableAdapters.extramantisinfo_returnsTableAdapter
    Friend WithEvents CODEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents SUBCODE2DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LOCATIONCODEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents descr As DataGridViewTextBoxColumn
    Friend WithEvents LSUMQTYDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LQTYFREEDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents CODEDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents SUBCODE2DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents LOCATIONCODEDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents LSUMQTYDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents LQTYFREEDataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
End Class
