<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PrintBrakeLiningsOrderWarehouse
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
        Dim ReportDataSource1 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PrintBrakeLiningsOrderWarehouse))
        Me.BLORDERapothBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.ExtCursor1 = New ExtCursors.ExtCursor()
        Me.BLORDERapothTableAdapter = New TESTFINALDataSetTableAdapters.BLORDERapothTableAdapter()
        CType(Me.BLORDERapothBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BLORDERapothBindingSource
        '
        Me.BLORDERapothBindingSource.DataMember = "BLORDERapoth"
        Me.BLORDERapothBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.EnforceConstraints = False
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ReportViewer1
        '
        Me.ReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        ReportDataSource1.Name = "blorder_apothiki"
        ReportDataSource1.Value = Me.BLORDERapothBindingSource
        Me.ReportViewer1.LocalReport.DataSources.Add(ReportDataSource1)
        Me.ReportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewer1.Name = "ReportViewer1"
        Me.ReportViewer1.Size = New System.Drawing.Size(618, 446)
        Me.ReportViewer1.TabIndex = 0
        '
        'ExtCursor1
        '
        Me.ExtCursor1.CursorStream = CType(resources.GetObject("ExtCursor1.CursorStream"), ExtCursors.ExtCursorFileStreamer)
        '
        'BLORDERapothTableAdapter
        '
        Me.BLORDERapothTableAdapter.ClearBeforeFill = True
        '
        'blorder_apothiki
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(618, 446)
        Me.Controls.Add(Me.ReportViewer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "blorder_apothiki"
        Me.Text = "Εντολή παραγγελίας φερμουΐτ Μορφοποίηση Αποθήκης"
        CType(Me.BLORDERapothBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ReportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents ExtCursor1 As ExtCursors.ExtCursor
    Friend WithEvents BLORDERapothBindingSource As BindingSource
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents BLORDERapothTableAdapter As TESTFINALDataSetTableAdapters.BLORDERapothTableAdapter
End Class
