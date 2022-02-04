<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PackingList
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PackingList))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.printuserlbl = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.printdatelbl = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.atlantiscodelbl = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.statuslbl = New System.Windows.Forms.Label()
        Me.createuserlbl = New System.Windows.Forms.Label()
        Me.closedatelbl = New System.Windows.Forms.Label()
        Me.createdatelbl = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cusnamelbl = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.codelbl = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ProcessingButton1 = New ProcessingButton()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.DataGridView1 = New Zuby.ADGV.AdvancedDataGridView()
        Me.InformationPanel1 = New InformationPanel()
        Me.CustomDGVSearchBox1 = New CustomDGVSearchBox()
        Me.PalletsWorker = New System.ComponentModel.BackgroundWorker()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.PLProcessWorker = New System.ComponentModel.BackgroundWorker()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ΕκκαθάρισηΑρίθμησηςToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel3, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.DataGridView1, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.InformationPanel1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.CustomDGVSearchBox1, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1276, 796)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Button2)
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(3, 769)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1270, 24)
        Me.Panel3.TabIndex = 2
        '
        'Button2
        '
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button2.Image = Global.My.Resources.Resources.icons8_undo_16
        Me.Button2.Location = New System.Drawing.Point(1051, 0)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(124, 24)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Επιστροφή/άκυρο"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button1.Enabled = False
        Me.Button1.Image = Global.My.Resources.Resources.icons8_save_16__1_
        Me.Button1.Location = New System.Drawing.Point(1175, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(95, 24)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Αποθήκευση"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.printuserlbl)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.printdatelbl)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.atlantiscodelbl)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.statuslbl)
        Me.Panel1.Controls.Add(Me.createuserlbl)
        Me.Panel1.Controls.Add(Me.closedatelbl)
        Me.Panel1.Controls.Add(Me.createdatelbl)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.cusnamelbl)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.codelbl)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1270, 84)
        Me.Panel1.TabIndex = 0
        '
        'printuserlbl
        '
        Me.printuserlbl.AutoSize = True
        Me.printuserlbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.printuserlbl.Location = New System.Drawing.Point(494, 51)
        Me.printuserlbl.Name = "printuserlbl"
        Me.printuserlbl.Size = New System.Drawing.Size(63, 16)
        Me.printuserlbl.TabIndex = 19
        Me.printuserlbl.Text = "Κωδικός"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label8.Location = New System.Drawing.Point(402, 51)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 16)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Εκτύπωση από"
        '
        'printdatelbl
        '
        Me.printdatelbl.AutoSize = True
        Me.printdatelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.printdatelbl.Location = New System.Drawing.Point(494, 35)
        Me.printdatelbl.Name = "printdatelbl"
        Me.printdatelbl.Size = New System.Drawing.Size(63, 16)
        Me.printdatelbl.TabIndex = 17
        Me.printdatelbl.Text = "Κωδικός"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label7.Location = New System.Drawing.Point(382, 35)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(115, 16)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Ημ/νία εκτύπωσης"
        '
        'atlantiscodelbl
        '
        Me.atlantiscodelbl.AutoSize = True
        Me.atlantiscodelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.atlantiscodelbl.Location = New System.Drawing.Point(494, 19)
        Me.atlantiscodelbl.Name = "atlantiscodelbl"
        Me.atlantiscodelbl.Size = New System.Drawing.Size(63, 16)
        Me.atlantiscodelbl.TabIndex = 15
        Me.atlantiscodelbl.Text = "Κωδικός"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label9.Location = New System.Drawing.Point(364, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(133, 16)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Παραστατικό Atlantis"
        '
        'statuslbl
        '
        Me.statuslbl.AutoSize = True
        Me.statuslbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.statuslbl.Location = New System.Drawing.Point(117, 19)
        Me.statuslbl.Name = "statuslbl"
        Me.statuslbl.Size = New System.Drawing.Size(238, 16)
        Me.statuslbl.TabIndex = 11
        Me.statuslbl.Text = "Αποθηκευμένο κανονικής μορφής"
        '
        'createuserlbl
        '
        Me.createuserlbl.AutoSize = True
        Me.createuserlbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.createuserlbl.Location = New System.Drawing.Point(117, 67)
        Me.createuserlbl.Name = "createuserlbl"
        Me.createuserlbl.Size = New System.Drawing.Size(63, 16)
        Me.createuserlbl.TabIndex = 10
        Me.createuserlbl.Text = "Κωδικός"
        '
        'closedatelbl
        '
        Me.closedatelbl.AutoSize = True
        Me.closedatelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.closedatelbl.Location = New System.Drawing.Point(117, 51)
        Me.closedatelbl.Name = "closedatelbl"
        Me.closedatelbl.Size = New System.Drawing.Size(63, 16)
        Me.closedatelbl.TabIndex = 9
        Me.closedatelbl.Text = "Κωδικός"
        '
        'createdatelbl
        '
        Me.createdatelbl.AutoSize = True
        Me.createdatelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.createdatelbl.Location = New System.Drawing.Point(117, 35)
        Me.createdatelbl.Name = "createdatelbl"
        Me.createdatelbl.Size = New System.Drawing.Size(63, 16)
        Me.createdatelbl.TabIndex = 8
        Me.createdatelbl.Text = "Κωδικός"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label6.Location = New System.Drawing.Point(21, 67)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 16)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Δημιουργία από"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label5.Location = New System.Drawing.Point(1, 51)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(121, 16)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Ημ/νία κλεισίματος"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label4.Location = New System.Drawing.Point(1, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 16)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Ημ/νία δημιουργίας"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label3.Location = New System.Drawing.Point(58, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "STATUS"
        '
        'cusnamelbl
        '
        Me.cusnamelbl.AutoSize = True
        Me.cusnamelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cusnamelbl.Location = New System.Drawing.Point(494, 3)
        Me.cusnamelbl.Name = "cusnamelbl"
        Me.cusnamelbl.Size = New System.Drawing.Size(63, 16)
        Me.cusnamelbl.TabIndex = 3
        Me.cusnamelbl.Text = "Κωδικός"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label2.Location = New System.Drawing.Point(436, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Πελάτης"
        '
        'codelbl
        '
        Me.codelbl.AutoSize = True
        Me.codelbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.codelbl.Location = New System.Drawing.Point(117, 3)
        Me.codelbl.Name = "codelbl"
        Me.codelbl.Size = New System.Drawing.Size(63, 16)
        Me.codelbl.TabIndex = 1
        Me.codelbl.Text = "Κωδικός"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label1.Location = New System.Drawing.Point(65, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Κωδικός"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ProcessingButton1)
        Me.Panel2.Controls.Add(Me.Button4)
        Me.Panel2.Controls.Add(Me.Button3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 93)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1270, 34)
        Me.Panel2.TabIndex = 1
        '
        'ProcessingButton1
        '
        Me.ProcessingButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcessingButton1.Location = New System.Drawing.Point(1101, 3)
        Me.ProcessingButton1.Name = "ProcessingButton1"
        Me.ProcessingButton1.Size = New System.Drawing.Size(166, 28)
        Me.ProcessingButton1.state = "normal"
        Me.ProcessingButton1.TabIndex = 2
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Image = Global.My.Resources.Resources.icons8_print_16
        Me.Button4.Location = New System.Drawing.Point(1004, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(91, 28)
        Me.Button4.TabIndex = 1
        Me.Button4.Text = "Εκτύπωση"
        Me.Button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Image = Global.My.Resources.Resources.icons8_microsoft_excel_19
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(3, 3)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(119, 28)
        Me.Button3.TabIndex = 0
        Me.Button3.Text = "Ανάγνωση Excel"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button3.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.FilterAndSortEnabled = True
        Me.DataGridView1.Location = New System.Drawing.Point(3, 168)
        Me.DataGridView1.Name = "DataGridView1"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridView1.Size = New System.Drawing.Size(1270, 595)
        Me.DataGridView1.TabIndex = 6
        '
        'InformationPanel1
        '
        Me.InformationPanel1.AutoSize = True
        Me.InformationPanel1.Location = New System.Drawing.Point(3, 93)
        Me.InformationPanel1.Name = "InformationPanel1"
        Me.InformationPanel1.Size = New System.Drawing.Size(0, 32)
        Me.InformationPanel1.TabIndex = 7
        '
        'CustomDGVSearchBox1
        '
        Me.CustomDGVSearchBox1.custom_command = Nothing
        Me.CustomDGVSearchBox1.Custom_mode = True
        Me.CustomDGVSearchBox1.custom_parameters = Nothing
        Me.CustomDGVSearchBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CustomDGVSearchBox1.Location = New System.Drawing.Point(3, 133)
        Me.CustomDGVSearchBox1.Name = "CustomDGVSearchBox1"
        Me.CustomDGVSearchBox1.parent_datagridview = Me.DataGridView1
        Me.CustomDGVSearchBox1.Size = New System.Drawing.Size(1270, 29)
        Me.CustomDGVSearchBox1.TabIndex = 8
        '
        'PalletsWorker
        '
        '
        'PLProcessWorker
        '
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem, Me.ΕκκαθάρισηΑρίθμησηςToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(306, 48)
        '
        'ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem
        '
        Me.ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem.Name = "ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem"
        Me.ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem.Size = New System.Drawing.Size(305, 22)
        Me.ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem.Text = "Αυτόματη αρίθμηση βάσει σειράς επιλογής"
        '
        'ΕκκαθάρισηΑρίθμησηςToolStripMenuItem
        '
        Me.ΕκκαθάρισηΑρίθμησηςToolStripMenuItem.Name = "ΕκκαθάρισηΑρίθμησηςToolStripMenuItem"
        Me.ΕκκαθάρισηΑρίθμησηςToolStripMenuItem.Size = New System.Drawing.Size(305, 22)
        Me.ΕκκαθάρισηΑρίθμησηςToolStripMenuItem.Text = "Εκκαθάριση αρίθμησης"
        '
        'PackingList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button2
        Me.ClientSize = New System.Drawing.Size(1276, 796)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "PackingList"
        Me.Text = "Packing List"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents codelbl As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents PalletsWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents Button4 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents cusnamelbl As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents DataGridView1 As AdvancedDataGridView
    Friend WithEvents createuserlbl As Label
    Friend WithEvents closedatelbl As Label
    Friend WithEvents createdatelbl As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents statuslbl As Label
    Friend WithEvents atlantiscodelbl As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ProcessingButton1 As ProcessingButton
    Friend WithEvents PLProcessWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ΑυτόματηΑρίθμησηΒάσειΣειράςΕπιλογήςToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ΕκκαθάρισηΑρίθμησηςToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents printuserlbl As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents printdatelbl As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents InformationPanel1 As InformationPanel
    Friend WithEvents CustomDGVSearchBox1 As CustomDGVSearchBox
End Class
