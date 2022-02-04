<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PrintPalletReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PrintPalletReport))
        Me.ExtCursor1 = New ExtCursors.ExtCursor()
        Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.Z_PACKER_PALLETREPORTBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Z_PACKER_PALLETREPORTTableAdapter = New TESTFINALDataSetTableAdapters.Z_PACKER_PALLETREPORTTableAdapter()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Z_PACKER_PALLETREPORTBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ReportViewer1
        '
        Me.ReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReportViewer1.LocalReport.ReportEmbeddedResource = "Report1.rdlc"
        Me.ReportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewer1.Name = "ReportViewer1"
        Me.ReportViewer1.Size = New System.Drawing.Size(985, 608)
        Me.ReportViewer1.TabIndex = 0
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Z_PACKER_PALLETREPORTBindingSource
        '
        Me.Z_PACKER_PALLETREPORTBindingSource.DataMember = "Z_PACKER_PALLETREPORT"
        Me.Z_PACKER_PALLETREPORTBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'Z_PACKER_PALLETREPORTTableAdapter
        '
        Me.Z_PACKER_PALLETREPORTTableAdapter.ClearBeforeFill = True
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(985, 608)
        Me.Controls.Add(Me.ReportViewer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form3"
        Me.Text = "Αναφορά παλέτας - Grantex Packer®"
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Z_PACKER_PALLETREPORTBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExtCursor1 As ExtCursors.ExtCursor
    Friend WithEvents ReportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents Z_PACKER_PALLETREPORTBindingSource As BindingSource
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents Z_PACKER_PALLETREPORTTableAdapter As TESTFINALDataSetTableAdapters.Z_PACKER_PALLETREPORTTableAdapter
End Class
