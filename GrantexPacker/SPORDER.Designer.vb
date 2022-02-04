<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SPORDER
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
        Dim ReportDataSource1 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SPORDER))
        Me.SPORDERBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TESTFINALDataSet = New TESTFINALDataSet()
        Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.SPORDERTableAdapter = New TESTFINALDataSetTableAdapters.SPORDERTableAdapter()
        Me.ExtCursor1 = New ExtCursors.ExtCursor()
        CType(Me.SPORDERBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SPORDERBindingSource
        '
        Me.SPORDERBindingSource.DataMember = "SPORDER"
        Me.SPORDERBindingSource.DataSource = Me.TESTFINALDataSet
        '
        'TESTFINALDataSet
        '
        Me.TESTFINALDataSet.DataSetName = "TESTFINALDataSet"
        Me.TESTFINALDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ReportViewer1
        '
        Me.ReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        ReportDataSource1.Name = "SPORDER"
        ReportDataSource1.Value = Me.SPORDERBindingSource
        Me.ReportViewer1.LocalReport.DataSources.Add(ReportDataSource1)
        Me.ReportViewer1.LocalReport.ReportEmbeddedResource = "Report8.rdlc"
        Me.ReportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewer1.Name = "ReportViewer1"
        Me.ReportViewer1.Size = New System.Drawing.Size(596, 464)
        Me.ReportViewer1.TabIndex = 0
        '
        'SPORDERTableAdapter
        '
        Me.SPORDERTableAdapter.ClearBeforeFill = True
        '
        'ExtCursor1
        '
        Me.ExtCursor1.CursorStream = CType(resources.GetObject("ExtCursor1.CursorStream"), ExtCursors.ExtCursorFileStreamer)
        '
        'SPORDER
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(596, 464)
        Me.Controls.Add(Me.ReportViewer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SPORDER"
        Me.Text = "Εντολή παραγγελίας ανταλλακτικών"
        CType(Me.SPORDERBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TESTFINALDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ReportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents SPORDERBindingSource As BindingSource
    Friend WithEvents TESTFINALDataSet As TESTFINALDataSet
    Friend WithEvents SPORDERTableAdapter As TESTFINALDataSetTableAdapters.SPORDERTableAdapter
    Friend WithEvents ExtCursor1 As ExtCursors.ExtCursor
End Class
