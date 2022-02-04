<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PrintPackingList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PrintPackingList))
        Me.DataTable1BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataTable2BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.ExtCursor1 = New ExtCursors.ExtCursor()
        Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.Z_PACKER_FULLREPORTBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataTable1BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet1BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Z_PACKER_FULLREPORTTableAdapter = New TESTFINALDataSetTableAdapters.Z_PACKER_FULLREPORTTableAdapter()
        Me.DataTable1TableAdapter1 = New TESTFINALDataSetTableAdapters.DataTable1TableAdapter()
        Me.DataTable2TableAdapter = New TESTFINALDataSetTableAdapters.DataTable2TableAdapter()
        Me.DataTable1TableAdapter = New TESTFINALDataSetTableAdapters.DataTable1TableAdapter()
        Me.BlordeRapothTableAdapter1 = New TESTFINALDataSetTableAdapters.BLORDERapothTableAdapter()
        CType(Me.DataTable1BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataTable2BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Z_PACKER_FULLREPORTBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataTable1BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet1BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataTable1BindingSource1
        '
        Me.DataTable1BindingSource1.DataMember = "DataTable1"
        Me.DataTable1BindingSource1.DataSource = Me.DataTable2BindingSource
        '
        'DataTable2BindingSource
        '
        Me.DataTable2BindingSource.DataSource = Me.TESTFINALDataSet
        Me.DataTable2BindingSource.Position = 0
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ExtCursor1
        '
        Me.ExtCursor1.CursorStream = CType(resources.GetObject("ExtCursor1.CursorStream"), ExtCursors.ExtCursorFileStreamer)
        '
        'ReportViewer1
        '
        Me.ReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewer1.Name = "ReportViewer1"
        Me.ReportViewer1.Size = New System.Drawing.Size(787, 463)
        Me.ReportViewer1.TabIndex = 0
        '
        'Z_PACKER_FULLREPORTBindingSource
        '
        Me.Z_PACKER_FULLREPORTBindingSource.DataMember = "Z_PACKER_FULLREPORT"
        Me.Z_PACKER_FULLREPORTBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'DataTable1BindingSource
        '
        Me.DataTable1BindingSource.DataMember = "DataTable1"
        Me.DataTable1BindingSource.DataSource = Me.TESTFINALDataSet
        '
        'TESTFINALDataSet1BindingSource
        '
        Me.TESTFINALDataSet1BindingSource.DataMember = "DataTable1"
        Me.TESTFINALDataSet1BindingSource.DataSource = Me.TESTFINALDataSet
        '
        'Z_PACKER_FULLREPORTTableAdapter
        '
        Me.Z_PACKER_FULLREPORTTableAdapter.ClearBeforeFill = True
        '
        'DataTable1TableAdapter1
        '
        Me.DataTable1TableAdapter1.ClearBeforeFill = True
        '
        'DataTable2TableAdapter
        '
        Me.DataTable2TableAdapter.ClearBeforeFill = True
        '
        'DataTable1TableAdapter
        '
        Me.DataTable1TableAdapter.ClearBeforeFill = True
        '
        'BlordeRapothTableAdapter1
        '
        Me.BlordeRapothTableAdapter1.ClearBeforeFill = True
        '
        'PrintPackingList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 463)
        Me.Controls.Add(Me.ReportViewer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "PrintPackingList"
        Me.Text = "Γενική αναφορά - Grantex Packer®"
        CType(Me.DataTable1BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataTable2BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Z_PACKER_FULLREPORTBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataTable1BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet1BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Z_PACKER_FULLREPORTBindingSource As BindingSource
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents Z_PACKER_FULLREPORTTableAdapter As TESTFINALDataSetTableAdapters.Z_PACKER_FULLREPORTTableAdapter
    Friend WithEvents TESTFINALDataSet1BindingSource As BindingSource
    Friend WithEvents DataTable1TableAdapter1 As TESTFINALDataSetTableAdapters.DataTable1TableAdapter
    Friend WithEvents ExtCursor1 As ExtCursors.ExtCursor
    Friend WithEvents DataTable2BindingSource As BindingSource
    Friend WithEvents DataTable2TableAdapter As TESTFINALDataSetTableAdapters.DataTable2TableAdapter
    Friend WithEvents DataTable1BindingSource As BindingSource
    Friend WithEvents DataTable1TableAdapter As TESTFINALDataSetTableAdapters.DataTable1TableAdapter
    Friend WithEvents DataTable1BindingSource1 As BindingSource
    Friend WithEvents BlordeRapothTableAdapter1 As TESTFINALDataSetTableAdapters.BLORDERapothTableAdapter
    Friend WithEvents ReportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
End Class
