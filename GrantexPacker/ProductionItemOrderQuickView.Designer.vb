<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProductionItemOrderQuickView
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProductionItemOrderQuickView))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.DataGridView1 = New Zuby.ADGV.AdvancedDataGridView()
        Me.itemorderdetails = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.work = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MainWorker = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ftrid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tradecode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ftrdate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.iteid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.stlid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.item = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dispatchdate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.statusnum = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.statusname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.quantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.DataGridView1)
        Me.Panel1.Controls.Add(Me.CheckBox2)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(821, 352)
        Me.Panel1.TabIndex = 0
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(3, 16)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(271, 17)
        Me.CheckBox2.TabIndex = 10
        Me.CheckBox2.Text = "Σύνοπτική εμφάνιση (όχι επιμέρους part numbers)"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(320, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Label2"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.Label4.Location = New System.Drawing.Point(774, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(19, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "99"
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.BackColor = System.Drawing.Color.Red
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(796, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(20, 20)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Χ"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.ColumnHeadersHeight = 35
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ftrid, Me.tradecode, Me.ftrdate, Me.iteid, Me.stlid, Me.item, Me.itemorderdetails, Me.work, Me.dispatchdate, Me.statusnum, Me.statusname, Me.quantity})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridView1.FilterAndSortEnabled = True
        Me.DataGridView1.Location = New System.Drawing.Point(3, 55)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.Size = New System.Drawing.Size(813, 292)
        Me.DataGridView1.TabIndex = 2
        '
        'itemorderdetails
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Webdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.itemorderdetails.DefaultCellStyle = DataGridViewCellStyle3
        Me.itemorderdetails.HeaderText = "Είδος->ΠΑΡ"
        Me.itemorderdetails.MinimumWidth = 22
        Me.itemorderdetails.Name = "itemorderdetails"
        Me.itemorderdetails.ReadOnly = True
        Me.itemorderdetails.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.itemorderdetails.ToolTipText = "Εμφανίζει την ανάλυση του είδους για αυτή τη ΠΑΡ"
        Me.itemorderdetails.Width = 70
        '
        'work
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Webdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.work.DefaultCellStyle = DataGridViewCellStyle4
        Me.work.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.work.HeaderText = "Load in Packer"
        Me.work.MinimumWidth = 22
        Me.work.Name = "work"
        Me.work.ReadOnly = True
        Me.work.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.work.Text = ""
        Me.work.ToolTipText = "Ανοίγει τη παραγγελία στο κεντρικό παράθυρο του Packer"
        Me.work.Width = 70
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(3, 32)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(311, 17)
        Me.CheckBox1.TabIndex = 1
        Me.CheckBox1.Text = "Συνολικές ποσότητες (όλοι οι αποδέκτες Π,Α,Α-Π,Α-Σ κλπ)"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Label1"
        '
        'MainWorker
        '
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "ftrid"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn1.Visible = False
        Me.DataGridViewTextBoxColumn1.Width = 30
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Blue
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTextBoxColumn2.HeaderText = "ΠΑΡ"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn2.Width = 80
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "ftrdate"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn3.Visible = False
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "iteid"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn4.Visible = False
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "stlid"
        Me.DataGridViewTextBoxColumn5.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn5.Visible = False
        '
        'DataGridViewTextBoxColumn6
        '
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Blue
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn6.HeaderText = "Είδος"
        Me.DataGridViewTextBoxColumn6.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Ημ/νια Αποστ."
        Me.DataGridViewTextBoxColumn7.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn7.Width = 96
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "statusnum"
        Me.DataGridViewTextBoxColumn8.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn8.Visible = False
        Me.DataGridViewTextBoxColumn8.Width = 80
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "Κατάσταση Παρ."
        Me.DataGridViewTextBoxColumn9.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn9.Width = 107
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "ΥΠΟΛ."
        Me.DataGridViewTextBoxColumn10.MinimumWidth = 22
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn10.Width = 69
        '
        'ftrid
        '
        Me.ftrid.HeaderText = "ftrid"
        Me.ftrid.MinimumWidth = 22
        Me.ftrid.Name = "ftrid"
        Me.ftrid.ReadOnly = True
        Me.ftrid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ftrid.Visible = False
        Me.ftrid.Width = 30
        '
        'tradecode
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Blue
        Me.tradecode.DefaultCellStyle = DataGridViewCellStyle1
        Me.tradecode.HeaderText = "ΠΑΡ"
        Me.tradecode.MinimumWidth = 22
        Me.tradecode.Name = "tradecode"
        Me.tradecode.ReadOnly = True
        Me.tradecode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.tradecode.Width = 80
        '
        'ftrdate
        '
        Me.ftrdate.HeaderText = "ftrdate"
        Me.ftrdate.MinimumWidth = 22
        Me.ftrdate.Name = "ftrdate"
        Me.ftrdate.ReadOnly = True
        Me.ftrdate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ftrdate.Visible = False
        '
        'iteid
        '
        Me.iteid.HeaderText = "iteid"
        Me.iteid.MinimumWidth = 22
        Me.iteid.Name = "iteid"
        Me.iteid.ReadOnly = True
        Me.iteid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.iteid.Visible = False
        '
        'stlid
        '
        Me.stlid.HeaderText = "stlid"
        Me.stlid.MinimumWidth = 22
        Me.stlid.Name = "stlid"
        Me.stlid.ReadOnly = True
        Me.stlid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.stlid.Visible = False
        '
        'item
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Blue
        Me.item.DefaultCellStyle = DataGridViewCellStyle2
        Me.item.HeaderText = "Είδος"
        Me.item.MinimumWidth = 22
        Me.item.Name = "item"
        Me.item.ReadOnly = True
        Me.item.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'dispatchdate
        '
        Me.dispatchdate.HeaderText = "Ημ/νια Αποστ."
        Me.dispatchdate.MinimumWidth = 22
        Me.dispatchdate.Name = "dispatchdate"
        Me.dispatchdate.ReadOnly = True
        Me.dispatchdate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.dispatchdate.Width = 96
        '
        'statusnum
        '
        Me.statusnum.HeaderText = "statusnum"
        Me.statusnum.MinimumWidth = 22
        Me.statusnum.Name = "statusnum"
        Me.statusnum.ReadOnly = True
        Me.statusnum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.statusnum.Visible = False
        Me.statusnum.Width = 80
        '
        'statusname
        '
        Me.statusname.HeaderText = "Κατάσταση Παρ."
        Me.statusname.MinimumWidth = 22
        Me.statusname.Name = "statusname"
        Me.statusname.ReadOnly = True
        Me.statusname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.statusname.Width = 107
        '
        'quantity
        '
        Me.quantity.HeaderText = "ΥΠΟΛ."
        Me.quantity.MinimumWidth = 22
        Me.quantity.Name = "quantity"
        Me.quantity.ReadOnly = True
        Me.quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.quantity.Width = 69
        '
        'ProductionItemOrderQuickView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button2
        Me.ClientSize = New System.Drawing.Size(821, 352)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(821, 352)
        Me.MinimumSize = New System.Drawing.Size(690, 352)
        Me.Name = "ProductionItemOrderQuickView"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "ProductionItemOrderQuickView"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents DataGridView1 As Zuby.ADGV.AdvancedDataGridView
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents MainWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label2 As Label
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents ftrid As DataGridViewTextBoxColumn
    Friend WithEvents tradecode As DataGridViewTextBoxColumn
    Friend WithEvents ftrdate As DataGridViewTextBoxColumn
    Friend WithEvents iteid As DataGridViewTextBoxColumn
    Friend WithEvents stlid As DataGridViewTextBoxColumn
    Friend WithEvents item As DataGridViewTextBoxColumn
    Friend WithEvents itemorderdetails As DataGridViewButtonColumn
    Friend WithEvents work As DataGridViewButtonColumn
    Friend WithEvents dispatchdate As DataGridViewTextBoxColumn
    Friend WithEvents statusnum As DataGridViewTextBoxColumn
    Friend WithEvents statusname As DataGridViewTextBoxColumn
    Friend WithEvents quantity As DataGridViewTextBoxColumn
End Class
