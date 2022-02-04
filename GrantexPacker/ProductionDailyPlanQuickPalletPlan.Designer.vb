<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ProductionDailyPlanQuickPalletPlan
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.NoPaddingCheckbox1 = New NoPaddingCheckbox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.RemoveSemiFinished = New NoPaddingButton()
        Me.SetSemiFinished = New NoPaddingButton()
        Me.combinebutton = New NoPaddingButton()
        Me.unlockbutton = New NoPaddingButton()
        Me.lockbutton = New NoPaddingButton()
        Me.openbutton = New NoPaddingButton()
        Me.closebutton = New NoPaddingButton()
        Me.deletebutton = New NoPaddingButton()
        Me.NoPaddingButton3 = New NoPaddingButton()
        Me.printbutton = New NoPaddingButton()
        Me.NoPaddingButton1 = New NoPaddingButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.mainworker = New System.ComponentModel.BackgroundWorker()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 5, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label8, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label9, 3, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(654, 93)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Panel1, 6)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(648, 24)
        Me.Panel1.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(109, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Παλετοποίηση για:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.Label4.Location = New System.Drawing.Point(600, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(19, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "99"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Red
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(625, 1)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(20, 20)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Χ"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TableLayoutPanel1.SetColumnSpan(Me.FlowLayoutPanel1, 6)
        Me.FlowLayoutPanel1.Controls.Add(Me.NoPaddingCheckbox1)
        Me.FlowLayoutPanel1.Controls.Add(Me.Panel3)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 53)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(648, 37)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'NoPaddingCheckbox1
        '
        Me.NoPaddingCheckbox1.Appearance = System.Windows.Forms.Appearance.Button
        Me.NoPaddingCheckbox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NoPaddingCheckbox1.Font = New System.Drawing.Font("Wingdings", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.NoPaddingCheckbox1.Location = New System.Drawing.Point(3, 3)
        Me.NoPaddingCheckbox1.Name = "NoPaddingCheckbox1"
        Me.NoPaddingCheckbox1.OwnerDrawText = Nothing
        Me.NoPaddingCheckbox1.Size = New System.Drawing.Size(18, 29)
        Me.NoPaddingCheckbox1.TabIndex = 0
        Me.NoPaddingCheckbox1.Text = ""
        Me.NoPaddingCheckbox1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.NoPaddingCheckbox1, "Επέκταση παλετών")
        Me.NoPaddingCheckbox1.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.RemoveSemiFinished)
        Me.Panel3.Controls.Add(Me.SetSemiFinished)
        Me.Panel3.Controls.Add(Me.combinebutton)
        Me.Panel3.Controls.Add(Me.unlockbutton)
        Me.Panel3.Controls.Add(Me.lockbutton)
        Me.Panel3.Controls.Add(Me.openbutton)
        Me.Panel3.Controls.Add(Me.closebutton)
        Me.Panel3.Controls.Add(Me.deletebutton)
        Me.Panel3.Controls.Add(Me.NoPaddingButton3)
        Me.Panel3.Controls.Add(Me.printbutton)
        Me.Panel3.Controls.Add(Me.NoPaddingButton1)
        Me.Panel3.Location = New System.Drawing.Point(27, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(612, 30)
        Me.Panel3.TabIndex = 1
        '
        'RemoveSemiFinished
        '
        Me.RemoveSemiFinished.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RemoveSemiFinished.Image = Global.My.Resources.Resources.icons8_empty_flag_16
        Me.RemoveSemiFinished.Location = New System.Drawing.Point(227, 0)
        Me.RemoveSemiFinished.Name = "RemoveSemiFinished"
        Me.RemoveSemiFinished.OwnerDrawText = Nothing
        Me.RemoveSemiFinished.Size = New System.Drawing.Size(30, 29)
        Me.RemoveSemiFinished.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.RemoveSemiFinished, "Αφαιρεί τη σήμανση ημιτελούς από τη παλέτα")
        Me.RemoveSemiFinished.UseVisualStyleBackColor = True
        Me.RemoveSemiFinished.Visible = False
        '
        'SetSemiFinished
        '
        Me.SetSemiFinished.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SetSemiFinished.Image = Global.My.Resources.Resources.icons8_flag_filled_16__1_
        Me.SetSemiFinished.Location = New System.Drawing.Point(260, 0)
        Me.SetSemiFinished.Name = "SetSemiFinished"
        Me.SetSemiFinished.OwnerDrawText = Nothing
        Me.SetSemiFinished.Size = New System.Drawing.Size(30, 29)
        Me.SetSemiFinished.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.SetSemiFinished, "Σημαίνει τη παλέτα ως ημιτελή")
        Me.SetSemiFinished.UseVisualStyleBackColor = True
        Me.SetSemiFinished.Visible = False
        '
        'combinebutton
        '
        Me.combinebutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.combinebutton.Image = Global.My.Resources.Resources.icons8_combine_semifinished
        Me.combinebutton.Location = New System.Drawing.Point(69, 0)
        Me.combinebutton.Name = "combinebutton"
        Me.combinebutton.OwnerDrawText = Nothing
        Me.combinebutton.Size = New System.Drawing.Size(30, 29)
        Me.combinebutton.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.combinebutton, "Ένωση με άλλη ημιτελή")
        Me.combinebutton.UseVisualStyleBackColor = True
        Me.combinebutton.Visible = False
        '
        'unlockbutton
        '
        Me.unlockbutton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.unlockbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.unlockbutton.Image = Global.My.Resources.Resources.icons8_unlock_pallet_20
        Me.unlockbutton.Location = New System.Drawing.Point(470, 0)
        Me.unlockbutton.Name = "unlockbutton"
        Me.unlockbutton.OwnerDrawText = Nothing
        Me.unlockbutton.Size = New System.Drawing.Size(30, 29)
        Me.unlockbutton.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.unlockbutton, "Ξεκλειδώνει τις επιλεγμένες παλέτες")
        Me.unlockbutton.UseVisualStyleBackColor = True
        Me.unlockbutton.Visible = False
        '
        'lockbutton
        '
        Me.lockbutton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lockbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lockbutton.Image = Global.My.Resources.Resources.icons8_lock_pallet_20
        Me.lockbutton.Location = New System.Drawing.Point(437, 0)
        Me.lockbutton.Name = "lockbutton"
        Me.lockbutton.OwnerDrawText = Nothing
        Me.lockbutton.Size = New System.Drawing.Size(30, 29)
        Me.lockbutton.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.lockbutton, "Κλειδώνει τις επιλεγμένες παλέτες")
        Me.lockbutton.UseVisualStyleBackColor = True
        Me.lockbutton.Visible = False
        '
        'openbutton
        '
        Me.openbutton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.openbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.openbutton.Image = Global.My.Resources.Resources.icons8_pallet_next_stage_20
        Me.openbutton.Location = New System.Drawing.Point(515, 0)
        Me.openbutton.Name = "openbutton"
        Me.openbutton.OwnerDrawText = Nothing
        Me.openbutton.Size = New System.Drawing.Size(30, 29)
        Me.openbutton.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.openbutton, "Φυσικό άνοιγμα επιλεγμένων παλετών που βρίσκονται σε στάδιο σχεδιασμού")
        Me.openbutton.UseVisualStyleBackColor = True
        Me.openbutton.Visible = False
        '
        'closebutton
        '
        Me.closebutton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.closebutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.closebutton.Image = Global.My.Resources.Resources.icons8_pallet_complete_20
        Me.closebutton.Location = New System.Drawing.Point(548, 0)
        Me.closebutton.Name = "closebutton"
        Me.closebutton.OwnerDrawText = Nothing
        Me.closebutton.Size = New System.Drawing.Size(30, 29)
        Me.closebutton.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.closebutton, "Ολοκλήρωση επιλεγμένων παλετών που έχουν προηγουμένως ανοιχθεί φυσικά")
        Me.closebutton.UseVisualStyleBackColor = True
        Me.closebutton.Visible = False
        '
        'deletebutton
        '
        Me.deletebutton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.deletebutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.deletebutton.Image = Global.My.Resources.Resources.icons8_pallet_delete_20
        Me.deletebutton.Location = New System.Drawing.Point(581, 0)
        Me.deletebutton.Name = "deletebutton"
        Me.deletebutton.OwnerDrawText = Nothing
        Me.deletebutton.Size = New System.Drawing.Size(30, 29)
        Me.deletebutton.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.deletebutton, "Διαγραφή επιλεγμένων παλετών (μόνο σε φάση σχεδιασμού)")
        Me.deletebutton.UseVisualStyleBackColor = True
        Me.deletebutton.Visible = False
        '
        'NoPaddingButton3
        '
        Me.NoPaddingButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NoPaddingButton3.Image = Global.My.Resources.Resources.icons8_add_many_pallets_20
        Me.NoPaddingButton3.Location = New System.Drawing.Point(36, 0)
        Me.NoPaddingButton3.Name = "NoPaddingButton3"
        Me.NoPaddingButton3.OwnerDrawText = Nothing
        Me.NoPaddingButton3.Size = New System.Drawing.Size(30, 29)
        Me.NoPaddingButton3.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.NoPaddingButton3, "Προσθήκη πολλών παλετών (βάσει των ρυθμίσεων πελάτη)")
        Me.NoPaddingButton3.UseVisualStyleBackColor = True
        '
        'printbutton
        '
        Me.printbutton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.printbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.printbutton.Image = Global.My.Resources.Resources.icons8_print_161
        Me.printbutton.Location = New System.Drawing.Point(191, 0)
        Me.printbutton.Name = "printbutton"
        Me.printbutton.OwnerDrawText = Nothing
        Me.printbutton.Size = New System.Drawing.Size(30, 29)
        Me.printbutton.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.printbutton, "Εκτύπωση ετικετών για τις επιλεγμένες παλέτες")
        Me.printbutton.UseVisualStyleBackColor = True
        Me.printbutton.Visible = False
        '
        'NoPaddingButton1
        '
        Me.NoPaddingButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NoPaddingButton1.Image = Global.My.Resources.Resources.icons8_add_one_pallet_201
        Me.NoPaddingButton1.Location = New System.Drawing.Point(3, 0)
        Me.NoPaddingButton1.Name = "NoPaddingButton1"
        Me.NoPaddingButton1.OwnerDrawText = Nothing
        Me.NoPaddingButton1.Size = New System.Drawing.Size(30, 29)
        Me.NoPaddingButton1.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.NoPaddingButton1, "Προσθήκη μίας παλέτας (βάσει των ρυθμίσεων πελάτη)")
        Me.NoPaddingButton1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Location = New System.Drawing.Point(3, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(102, 20)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Προγραμ."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Location = New System.Drawing.Point(111, 30)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(102, 20)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Label5"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Location = New System.Drawing.Point(543, 30)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(108, 20)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Label6"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label7.Location = New System.Drawing.Point(435, 30)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(102, 20)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Συσκευ."
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label8.Location = New System.Drawing.Point(219, 30)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(102, 20)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Ανοιγμ."
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label9.Location = New System.Drawing.Point(327, 30)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(102, 20)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Label9"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(656, 95)
        Me.Panel2.TabIndex = 1
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'mainworker
        '
        '
        'ProductionDailyPlanQuickPalletPlan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button2
        Me.ClientSize = New System.Drawing.Size(656, 95)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ProductionDailyPlanQuickPalletPlan"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "ProductionDailyPlanQuickPalletPlan"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents NoPaddingCheckbox1 As NoPaddingCheckbox
    Friend WithEvents Button2 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents printbutton As NoPaddingButton
    Friend WithEvents NoPaddingButton1 As NoPaddingButton
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents NoPaddingButton3 As NoPaddingButton
    Friend WithEvents Label3 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents deletebutton As NoPaddingButton
    Friend WithEvents closebutton As NoPaddingButton
    Friend WithEvents openbutton As NoPaddingButton
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents unlockbutton As NoPaddingButton
    Friend WithEvents lockbutton As NoPaddingButton
    Friend WithEvents combinebutton As NoPaddingButton
    Friend WithEvents mainworker As System.ComponentModel.BackgroundWorker
    Friend WithEvents RemoveSemiFinished As NoPaddingButton
    Friend WithEvents SetSemiFinished As NoPaddingButton
End Class
