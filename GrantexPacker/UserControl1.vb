Imports System.ComponentModel
Imports System.Configuration

Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class pallettemplate
    Implements INotifyPropertyChanged

    Private hasmantis As Boolean = False

    Public Property mantis() As String
        Get
            Return Me.hasmantis
        End Get

        Set(ByVal value As String)
            If Not (value = hasmantis) Then
                Me.hasmantis = value

            End If
        End Set
    End Property

    Private has_dailyplan As Boolean = False

    Public Property hasdailyplan As Boolean
        Get
            Return Me.has_dailyplan
        End Get

        Set(ByVal value As Boolean)
            If Not (value = has_dailyplan) Then
                Me.has_dailyplan = value

            End If
        End Set
    End Property

    Private SetUp_Complete As Boolean

    Public Property SetUpComplete As Boolean
        Get
            Return SetUp_Complete
        End Get
        Set(value As Boolean)
            SetUp_Complete = value
            If value Then
                state_changed()
            End If
        End Set
    End Property


    Private custid As String
    Public Property cusid As String
        Get
            Return custid
        End Get
        Set(value As String)
            custid = value
        End Set
    End Property

    Private mystatus As Integer
    Public Property status As Integer
        Get
            Return mystatus
        End Get
        Set(value As Integer)
            mystatus = value
        End Set
    End Property

    Private CreateDPTID As Integer
    Public Property CreateDPT_ID As Integer
        Get
            Return CreateDPTID
        End Get
        Set(value As Integer)
            CreateDPTID = value
        End Set
    End Property

    Private Sub pallettemplatedatagrid_DragEnter(sender As Object, e As DragEventArgs) Handles pallettemplatedatagrid.DragEnter
        e.Effect = DragDropEffects.All
    End Sub
    Private MouseDownPos As Point


    Public Sub compute_sums()
        Dim sumbp As Double = 0
        Dim sumbl As Double = 0
        Dim sumsp As Double = 0
        Try
            With Me.pallettemplatedatagrid

                For i As Integer = 0 To Me.pallettemplatedatagrid.Rows.Count - 1
                    If Me.pallettemplatedatagrid.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) = "102" Or Me.pallettemplatedatagrid.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) = "202" Then
                        sumbp = sumbp + Me.pallettemplatedatagrid.Rows(i).Cells("QUANT").Value
                    End If
                    If Me.pallettemplatedatagrid.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 1) = "1" And Not Me.pallettemplatedatagrid.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) = "102" Then
                        sumbl = sumbl + Me.pallettemplatedatagrid.Rows(i).Cells("QUANT").Value
                    End If
                    If Me.pallettemplatedatagrid.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 1) = "2" And Not Me.pallettemplatedatagrid.Rows(i).Cells("ΚΩΔΙΚΟΣ").Value.ToString.Substring(0, 3) = "202" Then
                        sumsp = sumsp + Me.pallettemplatedatagrid.Rows(i).Cells("QUANT").Value
                    End If

                Next
            End With
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            Label15.Text = "Άθροισμα sets Σ/Φ/Δ/Α: " + (sumbl + sumbp + sumsp).ToString + "/" + sumbl.ToString + "/" + sumbp.ToString + "/" + sumsp.ToString
        End Try
    End Sub

    Public Property palletid As String
        Get
            Return palletidlbl.Text
        End Get
        Set(value As String)
            palletidlbl.Text = value
        End Set
    End Property

    Private customercode As String
    Public Property customer As String
        Get
            Return customercode
        End Get
        Set(value As String)
            customercode = value
        End Set
    End Property

    Private pinnedloc As Integer = -1
    Public Property pinned As Integer
        Get
            Return pinnedloc
        End Get
        Set(value As Integer)

            pinnedloc = value
            If pinnedloc <> -1 Then
                Form1.FlowLayoutPanel1.Controls.SetChildIndex(Me, value)
                CheckBox2.Checked = True

            Else
                Form1.FlowLayoutPanel1.Controls.SetChildIndex(Me, Form1.palletdeforder.Item(palletid))
                pallettemplate_MouseLeave(Me, EventArgs.Empty)
            End If

        End Set
    End Property

    Public lockedbydptvalue As String = String.Empty
    Public lockedbyuservalue As String = String.Empty
    Public Property lockedbydpt As String 'user,department
        Get
            Return lockedbydptvalue
        End Get
        Set(value As String)
            Dim oldvalue As String = lockedbydptvalue
            If String.IsNullOrWhiteSpace(value) Then
                lockedbydptvalue = String.Empty
                lockedbyuservalue = String.Empty
            Else
                Dim values As String() = value.ToString.Split(",")

                lockedbydptvalue = values(1)
                lockedbyuservalue = values(0)
            End If

            If lockedbydptvalue <> oldvalue Then
                state_changed()
            End If
        End Set
    End Property

    Public closedbydptvalue As String = String.Empty
    Public closedbyuservalue As String = String.Empty
    Public Property closedbydpt As String 'user,department
        Get
            Return closedbydptvalue
        End Get
        Set(value As String)

            Dim oldvalue As String = closedbydptvalue
            If String.IsNullOrWhiteSpace(value) Then
                closedbydptvalue = String.Empty
                closedbyuservalue = String.Empty
            Else

                Dim values As String() = value.ToString.Split(",")
                closedbydptvalue = values(1)
                closedbyuservalue = values(0)
            End If
            If closedbydptvalue <> oldvalue Then
                state_changed()
            End If
        End Set
    End Property

    Private viewstylevalue As String = String.Empty
    Public Property viewstyle As String
        Get
            Return viewstylevalue
        End Get
        Set(value As String)
            Dim oldvalue As String = viewstylevalue
            viewstylevalue = value
            If viewstylevalue <> oldvalue Then
                viewstyle_changed()
            End If
        End Set
    End Property

    Private salesmanvalue As String = String.Empty
    Public Property salesman As String
        Get
            Return salesmanvalue
        End Get
        Set(value As String)
            salesmanvalue = value
        End Set
    End Property

    Public packinglist As String = String.Empty
    Public Property plist As String
        Get
            Return packinglist
        End Get
        Set(value As String)
            Dim oldvalue As String = packinglist
            packinglist = value
            If packinglist <> oldvalue Then
                state_changed()
            End If
        End Set
    End Property

    Public Is_Draft As Boolean = False
    Public Property IsDraft As Boolean
        Get
            Return Is_Draft
        End Get
        Set(value As Boolean)
            If Is_Draft <> value Then
                Is_Draft = value
                state_changed()
            End If
        End Set
    End Property

    Public Belongs_ToPrintedPL As Boolean = False
    Public Property BelongsToPrintedPL As Boolean
        Get
            Return Belongs_ToPrintedPL
        End Get
        Set(value As Boolean)
            If Belongs_ToPrintedPL <> value Then
                Belongs_ToPrintedPL = value
                state_changed()
            End If
        End Set
    End Property

    Private Sub viewstyle_changed()


        If viewstylevalue = "SMALL" Then
            Me.Width = 363
            Me.Height = 342
        ElseIf viewstylevalue = "MEDIUM" Then
            Me.Width = 400
            Me.Height = 450
        ElseIf viewstylevalue = "LARGE" Then
            Me.Width = 400
            Me.Height = 550
        ElseIf viewstylevalue = "MINI" Then
            Me.Width = 365
            Me.Height = 53

        ElseIf viewstylevalue = "ONELINE" Then
            Me.Width = 365
            Me.Height = 205

        End If

    End Sub

    Private Sub state_changed()
        If Not SetUp_Complete Then
            Exit Sub
        End If
        If Me.locid <> 0 Then
            loclbl.Visible = True
        End If
        If Is_Draft Then
            With Me
                .Label12.Visible = True
                .Label12.Text = "Προγραμματισμένη"
                ToolTip5.SetToolTip(Button6, "Φυσικό άνοιγμα παλέτας, βγαίνει από mode σχεδιασμού")
                .Label4.Visible = True
                .Label5.Visible = True
                .Label5.Text = lockedbyuservalue
                .CheckBox1.Checked = True
                .SpecialReadOnly = True
                .BackColor = Color.LightSteelBlue
                Label6.BackColor = Color.LightSteelBlue
                Label5.BackColor = Color.LightSteelBlue
                Label12.BackColor = Color.LightSteelBlue
                Label19.BackColor = Color.LightSteelBlue
                Button6.BackColor = Color.LightSteelBlue
                Button6.Image = My.Resources.icons8_pallet_next_stage_30
                Button7.BackColor = Color.LightSteelBlue
                Button1.BackColor = Color.LightSteelBlue
                Button4.BackColor = Color.LightSteelBlue
                CheckBox1.BackColor = Color.LightSteelBlue
                CheckBox2.BackColor = Color.LightSteelBlue
                .BorderStyle = BorderStyle.FixedSingle
                .ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Enabled = False
                .ΜεταφοράΣτοΑπόθεμαToolStripMenuItem.Enabled = False
            End With
            Dim lst As New List(Of Boolean)
            Dim auaid = Form1.activeuseraid.ToString
            lst.Add(Form1.activeuserdpt <> "SA")
            lst.Add(Form1.activeuserdpt.ToUpper = department.ToUpper)
            lst.Add(Form1.activeuserdpt.ToUpper = lockedbydptvalue.ToUpper)
            lst.Add(salesman = Form1.activeuseraid.ToString)

            If (mystatus < 0 And Not (CreateDPTID = Form1.activeuserdptid Or Form1.activeuserdpt = "PRD" Or Form1.activeuserdpt = "SA")) Then
                Button6.Enabled = False
                ContextMenuStrip2.Enabled = False
                If Not lockedbydptvalue.ToUpper = Form1.activeuserdpt Then
                    Button1.Enabled = False
                    CheckBox1.Enabled = False
                End If
                If hasdailyplan Then
                    Button1.Enabled = False
                    CheckBox1.Enabled = False
                End If
            End If
        ElseIf closedbydptvalue <> String.Empty And packinglist <> String.Empty Then 'παλέτα σε packing list
            With Me
                .Label12.Visible = True
                .Label12.Text = "Στο packing list " + .packinglist
                .Label5.Text = .closedbyuservalue
                .Label6.Visible = True
                .Label5.Visible = True
                .CheckBox1.Checked = True
                '.CheckBox1.Text = ""
                .CheckBox1.ForeColor = Color.Red
                .closed = True
                .BackColor = Color.Khaki
                Label6.BackColor = Color.Khaki
                Label5.BackColor = Color.Khaki
                Label12.BackColor = Color.Khaki
                Label19.BackColor = Color.Khaki
                Button6.BackColor = Color.Khaki
                Button6.Image = My.Resources.icons8_approval_30
                Button7.BackColor = Color.Khaki
                Button1.BackColor = Color.Khaki
                Button4.BackColor = Color.Khaki
                CheckBox1.BackColor = Color.Khaki
                CheckBox2.BackColor = Color.Khaki
                .BorderStyle = BorderStyle.FixedSingle
                .ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Enabled = False
                .ΜεταφοράΣτοΑπόθεμαToolStripMenuItem.Enabled = False
            End With
        ElseIf closedbydptvalue <> String.Empty And packinglist = String.Empty Then 'παλέτα ολοκληρωμένη αλλά όχι σε packing list
            With Me
                .Label5.Text = .closedbyuservalue
                .Label6.Visible = True
                .Label5.Visible = True
                .closed = True
                ToolTip5.SetToolTip(Button6, "Καταργεί το κλείσιμο παλέτας")
                .BackColor = Color.LightGreen
                Label6.BackColor = Color.LightGreen
                Label5.BackColor = Color.LightGreen
                Label12.BackColor = Color.LightGreen
                Label19.BackColor = Color.LightGreen
                Button6.BackColor = Color.LightGreen
                Button6.Image = My.Resources.icons8_remove_approval
                Button7.BackColor = Color.LightGreen
                Button1.BackColor = Color.LightGreen
                Button4.BackColor = Color.LightGreen
                CheckBox1.BackColor = Color.LightGreen
                CheckBox2.BackColor = Color.LightGreen
                .BorderStyle = BorderStyle.FixedSingle
                .ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Enabled = False
            End With
        ElseIf lockedbydptvalue <> String.Empty Then
            With Me
                .Label5.Text = lockedbyuservalue
                .Label4.Visible = True
                .Label5.Visible = True
                .CheckBox1.Checked = True
                '.CheckBox1.Text = ""
                .CheckBox1.ForeColor = Color.Red
                Button6.Image = My.Resources.icons8_approval_30
                If Form1.activeuserdpt <> "SA" And Not (CreateDPTID = Form1.activeuserdptid Or Form1.activeuserdpt.ToUpper = lockedbydptvalue.ToUpper Or salesman = Form1.activeuseraid.ToString) Then
                    .locked = True
                    .BackColor = Color.LightSalmon
                    Label6.BackColor = Color.LightSalmon
                    Label5.BackColor = Color.LightSalmon
                    Label12.BackColor = Color.LightSalmon
                    Label19.BackColor = Color.LightSalmon
                    Button6.BackColor = Color.LightSalmon
                    Button7.BackColor = Color.LightSalmon
                    Button1.BackColor = Color.LightSalmon
                    Button4.BackColor = Color.LightSalmon
                    CheckBox1.BackColor = Color.LightSalmon
                    CheckBox2.BackColor = Color.LightSalmon
                    .ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Enabled = False
                    .ΜεταφοράΣτοΑπόθεμαToolStripMenuItem.Enabled = False
                Else
                    .BackColor = Color.Gainsboro
                    Label6.BackColor = Color.Gainsboro
                    Label5.BackColor = Color.Gainsboro
                    Label12.BackColor = Color.Gainsboro
                    Label19.BackColor = Color.Gainsboro
                    Button6.BackColor = Color.Gainsboro
                    Button7.BackColor = Color.Gainsboro
                    Button1.BackColor = Color.Gainsboro
                    Button4.BackColor = Color.Gainsboro
                    CheckBox1.BackColor = Color.Gainsboro
                    CheckBox2.BackColor = Color.Gainsboro
                End If

            End With
        ElseIf lockedbydptvalue = String.Empty Then
            With Me
                Button6.Image = My.Resources.icons8_approval_30
                .Label4.Visible = False
                .Label5.Visible = False
                .locked = False
                .BackColor = Color.White
                Label6.BackColor = Color.White
                Label5.BackColor = Color.White
                Label12.BackColor = Color.White
                Label19.BackColor = Color.White
                Button6.BackColor = Color.White
                Button7.BackColor = Color.White
                Button1.BackColor = Color.White
                Button4.BackColor = Color.White
                CheckBox1.BackColor = Color.White
                CheckBox2.BackColor = Color.White
            End With
        ElseIf closedbydptvalue = String.Empty Then
            With Me
                Button6.Image = My.Resources.icons8_approval_30
                .Label5.Text = .closedbyuservalue
                .Label6.Visible = False
                .Label5.Visible = False
                .closed = False
                .BackColor = Color.White
                Label6.BackColor = Color.White
                Label5.BackColor = Color.White
                Label12.BackColor = Color.White
                Label19.BackColor = Color.White
                Button6.BackColor = Color.White
                Button7.BackColor = Color.White
                Button1.BackColor = Color.White
                Button4.BackColor = Color.White
                CheckBox1.BackColor = Color.White
                CheckBox2.BackColor = Color.White
            End With
        ElseIf packinglist = String.Empty Then
            With Me
                .Label12.Visible = False
                Button6.Image = My.Resources.icons8_approval_30
                .Label4.Visible = False
                .Label5.Visible = False
                .CheckBox1.Checked = False
                '.CheckBox1.Text = ""
                .CheckBox1.ForeColor = Color.Green
                .closed = False
                .BackColor = Color.White
                Label6.BackColor = Color.White
                Label5.BackColor = Color.White
                Label12.BackColor = Color.White
                Label19.BackColor = Color.White
                Button6.BackColor = Color.White
                Button7.BackColor = Color.White
                Button1.BackColor = Color.White
                Button4.BackColor = Color.White
                CheckBox1.BackColor = Color.White
                CheckBox2.BackColor = Color.White
            End With
        End If
        If Belongs_ToPrintedPL Then
            Me.SpecialReadOnly = True
        End If



    End Sub


    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If


            For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
                Dim ctrl As Control = Me.Controls(i)
                ctrl.Dispose()
            Next
            Timer1.Dispose()
            Me.pallettemplatedatagrid.Dispose()

            'My.Resources.comment_png_22707.Dispose()
            'My.Resources.locate3.Dispose()
            'My.Resources.complete.Dispose()
            'My.Resources.openlock.Dispose()
            'My.Resources.closedlock.Dispose()
            'My.Resources.red_x_mark_4_icon_free_red_x_mark_icons_5br1Kr_clipart.Dispose()
            'My.Resources.importpallet.Dispose()

            Me.Button1.Dispose()
            Me.Button2.Dispose()
            Me.Button3.Dispose()


            Me.Button6.Dispose()
            Me.Button7.Dispose()
            Me.ToolTip1.Dispose()
            Me.ToolTip2.Dispose()
            Me.ToolTip3.Dispose()
            Me.ToolTip4.Dispose()
            Me.ToolTip5.Dispose()
            Me.ToolTip6.Dispose()
            Me.ContextMenuStrip1.Dispose()
            Me.Controls.Clear()
            conn.Dispose()
            updconn.Dispose()
            Me.ToolTip1.Dispose()
            Me.ToolTip2.Dispose()
            Me.ToolTip3.Dispose()
            Me.ToolTip4.Dispose()
            Me.ToolTip5.Dispose()
            Me.ToolTip6.Dispose()
            Me.ΑναλυτικάToolStripMenuItem.Dispose()
            Me.CheckBox1.Dispose()
            Me.CheckBox2.Dispose()
            TabControl1.Dispose()

        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Public Property department As String
        Get
            Return belongingdpt
        End Get
        Set(value As String)
            belongingdpt = value

        End Set
    End Property
    Dim locid As Integer = 0
    Public Property locationID As Integer
        Get
            Return locid
        End Get
        Set(value As Integer)
            locid = value
            ToolTip2.SetToolTip(loclbl, value.ToString)
        End Set
    End Property

    Dim loccode As String = ""
    Public Property locationCode As String
        Get
            Return loccode
        End Get
        Set(value As String)
            loccode = value
            loclbl.Text = loccode
            'Label18.Text = loccode.Substring(loccode.IndexOf(".") + 1, 5)
            'If Label18.Text.Contains("U") Then
            '    Label18.Text = "ΚΑΨΑΝΗΣ " + Label18.Text.Substring(Label18.Text.IndexOf(".") + 1, 2) + "η"
            'ElseIf Label18.Text.Contains("A") Then
            '    Label18.Text = "ΑΠΟΘΗΚΗ " + Label18.Text.Substring(Label18.Text.IndexOf(".") + 1, 2) + "η"
            'ElseIf Label18.Text.Contains("Y") Then
            '    Label18.Text = "ΥΠΟΓΕΙΟ " + Label18.Text.Substring(Label18.Text.IndexOf(".") + 1, 2) + "η"
            'ElseIf Label18.Text.Contains("P") Then
            '    Label18.Text = "ΔΙΣΚΟΦΡΕΝΑ " + Label18.Text.Substring(Label18.Text.IndexOf(".") + 1, 2) + "η"
            'ElseIf Label18.Text.Contains("L") Then
            '    Label18.Text = "ΦΕΡΜΟΥΙΤ " + Label18.Text.Substring(Label18.Text.IndexOf(".") + 1, 2) + "η"
            'End If

        End Set
    End Property

    Private special_readonly As Boolean = False

    Public Property SpecialReadOnly As Boolean 'σε περίπτωση DRAFT παλέτας ή παλέτας σε εκτυπωμένο Packing list η παλέτα κλειδώνει, αλλά επιτρέπεται η εισαγωγή βάρους κα
        Get
            Return special_readonly
        End Get
        Set(value As Boolean)
            special_readonly = value
            If value = True Then
                With Me
                    .Enabled = True
                    .pallettemplatedatagrid.ReadOnly = True
                    .pallettemplatedatagrid.AllowUserToDeleteRows = False
                    If (mystatus = -1) Then
                        .Button1.Enabled = True
                    ElseIf (mystatus = -2 Or mystatus = -3) And (Form1.activeuserdpt = "BL" Or Form1.activeuserdpt = "BP" Or Form1.activeuserdpt = "SA") Then
                        .Button1.Enabled = True
                    Else
                        .Button1.Enabled = False
                    End If
                    .Button3.Enabled = False
                    If Is_Draft Then
                        .Button6.Image = My.Resources.icons8_pallet_next_stage_30
                    Else
                        .Button6.Image = My.Resources.icons8_approval_pending_30__2_
                    End If
                    Button2.Enabled = True
                    TextBox2.Enabled = True
                    TextBox4.Enabled = True
                    .Button8.Enabled = False
                    .CheckBox1.Enabled = True
                    .TextBox3.Enabled = False
                    ComboBox1.Enabled = False
                End With
            ElseIf value = False Then
                With Me
                    .Enabled = True
                    .pallettemplatedatagrid.ReadOnly = False
                    .pallettemplatedatagrid.AllowUserToDeleteRows = True
                    .Button1.Enabled = True
                    .Button2.Enabled = True
                    .Button3.Enabled = True
                    .Button6.Image = My.Resources.icons8_approval_pending_30__2_
                    .Button8.Enabled = True
                    .BorderStyle = BorderStyle.FixedSingle
                    .CheckBox1.Enabled = True
                    .TextBox2.Enabled = True
                    .TextBox3.Enabled = True
                    .TextBox4.Enabled = True
                    ComboBox1.Enabled = True
                End With
            End If

        End Set
    End Property

    Private lockedp As Boolean = False
    Private belongingdpt As String


    Public Property locked As Boolean
        Get
            Return lockedp
        End Get
        Set(value As Boolean)
            lockedp = value
            If value = True Then
                With Me
                    .Enabled = True
                    .pallettemplatedatagrid.ReadOnly = True
                    .pallettemplatedatagrid.AllowUserToDeleteRows = False
                    .Button1.Enabled = False
                    .Button2.Enabled = False
                    .Button3.Enabled = False
                    .Button6.Enabled = False
                    .Button8.Enabled = False
                    .CheckBox1.Enabled = False
                    .TextBox2.Enabled = False
                    .TextBox4.Enabled = False
                    .TextBox3.Enabled = False
                    ComboBox1.Enabled = False
                End With
            ElseIf value = False Then
                With Me
                    .Enabled = True
                    .pallettemplatedatagrid.ReadOnly = False
                    .pallettemplatedatagrid.AllowUserToDeleteRows = True
                    .Button1.Enabled = True
                    .Button2.Enabled = True
                    .Button3.Enabled = True
                    .Button6.Enabled = True
                    .Button8.Enabled = True
                    .BorderStyle = BorderStyle.FixedSingle
                    .CheckBox1.Enabled = True
                    .TextBox2.Enabled = True
                    .TextBox3.Enabled = True
                    .TextBox4.Enabled = True
                    ComboBox1.Enabled = True
                End With
            End If

        End Set
    End Property

    Private closedp As Boolean = False

    Public Property closed As Boolean
        Get
            Return closedp
        End Get
        Set(value As Boolean)
            closedp = value
            If value = True Then
                With Me
                    .Enabled = True
                    .pallettemplatedatagrid.ReadOnly = True
                    .pallettemplatedatagrid.AllowUserToDeleteRows = False
                    .Button1.Enabled = False
                    .Button2.Enabled = False
                    .Button3.Enabled = False
                    .Button8.Enabled = False
                    If Me.packinglist = String.Empty And (Form1.activeuserdpt = belongingdpt Or Form1.activeuserdpt = "SA" Or Me.closedbyuservalue = Form1.activeuser.ToString Or Me.salesman = Form1.activeuseraid.ToString) Then
                        .Button6.Enabled = True
                        .Button6.Image = My.Resources.icons8_remove_approval
                        ToolTip5.SetToolTip(Me.Button6, "Ξεκλειδώνει τη παλέτα.")
                    Else
                        .Button6.Enabled = False
                        .Button6.Image = My.Resources.Resources.icons8_approval_pending_30__2_
                        ToolTip5.SetToolTip(Me.Button6, "Κλείνει τη παλέτα. Πρέπει να έχει συμπληρωθεί το βάρος της παλέτας.")
                    End If
                    .CheckBox1.Enabled = False
                    .TextBox2.Enabled = False
                    .TextBox4.Enabled = False
                    .TextBox3.Enabled = False
                    ComboBox1.Enabled = False
                End With
            ElseIf value = False Then
                With Me
                    .Enabled = True
                    .pallettemplatedatagrid.ReadOnly = False
                    .pallettemplatedatagrid.AllowUserToDeleteRows = True
                    .Button1.Enabled = True
                    .Button2.Enabled = True
                    .Button3.Enabled = True
                    .CheckBox1.Enabled = True
                    .Button8.Enabled = True
                    .TextBox2.Enabled = True
                    .TextBox3.Enabled = True
                    .TextBox4.Enabled = True
                    ComboBox1.Enabled = True
                End With
            End If
        End Set
    End Property

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Private Sub pallettemplatedatagrid_DragDrop(sender As Object, e As DragEventArgs) Handles pallettemplatedatagrid.DragDrop
        Dim row As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)




        If row IsNot Nothing And Not Me.locked And Not Me.closed Then
            Try
                If row.Cells("iteid").Value = 65946 Or row.Cells("iteid").Value = 65947 Or row.Cells("iteid").Value = 65948 Then
                    Throw New System.Exception("Δεν μπορείτε να τοποθετήσετε προσωρινό κωδικό σε παλέτα.")
                End If
                Dim message, title As String
                Dim defaultValue As Double
                Dim myValue As Object

                If row.Cells("backorder").Value <> 0 And row.Cells("ΥΠΟΛ.").Value <> 0 Then
                    defaultValue = row.Cells("ΥΠΟΛ.").Value - row.Cells("backorder").Value

                Else
                    defaultValue = row.Cells("ΥΠΟΛ.").Value

                End If



                ' Set prompt.
                message = "Τι ποσότητα του προϊόντος " + row.Cells("ΠΕΡΙΓΡΑΦΗ").Value + " θα τοποθετήσετε στη " + Me.pallettemplatelabel.Text + "; Διαθέσιμη ποσότητα: " + defaultValue.ToString
                ' Set title.
                title = "Εισάγετε ποσότητα"
                ' Display message, title, and default value.
                Dim Valid As Boolean
                While Valid = False
                    myValue = InputBox(message, title, defaultValue, e.X, e.Y)
                    Try
                        If IsNumeric(myValue) And myValue.ToString.Length <= 5 And myValue <> 0 Then
                            Valid = True
                        ElseIf myValue Is "" Or myValue = 0 Then
                            Return
                        Else
                            Valid = False
                        End If
                    Catch
                        Return
                    End Try
                End While
                If myValue > defaultValue Then
                    Throw New System.Exception("Δεν μπορείτε να εισάγετε ποσότητα μεγαλύτερη της διαθέσιμης!")
                End If
                Dim stlid As Integer = row.Cells("stlid").Value
                Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=Me.cusid)
                    pm.AddItem(palletid, row.Cells("iteid").Value, row.Cells("stlid").Value, row.Cells("ftrid").Value, myValue)
                End Using
                Form1.populate_pallets(Me.palletid.ToString, c:=Me)
                Form1.datagridview1_stlquantity(stlid.ToString)
            Catch ex As Exception
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
                If updconn.State = ConnectionState.Open Then
                    updconn.Close()
                End If
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End Try
        End If

    End Sub

    Private Sub pallettemplatedatagrid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles pallettemplatedatagrid.CellEndEdit
        Dim temp As Integer = 0
        Dim q As Double = 0
        Try
            If Not IsNumeric(pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value) Then

                Throw New System.Exception("Μόνο αριθμητικές τιμές είναι δεκτές")
            End If
            If pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value < 0 Then

                Throw New System.Exception("Μόνο θετικές τιμές είναι δεκτές")
            End If
            If Not pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value = oldcell Then
                For i As Integer = 0 To Form1.DataGridView1.Rows.Count - 1
                    If Form1.DataGridView1.Rows(i).Cells("stlid").Value = pallettemplatedatagrid.Rows(e.RowIndex).Cells("STLID").Value Then
                        If oldcell < pallettemplatedatagrid.Rows(e.RowIndex).Cells(1).Value Then
                            If pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value > oldcell + Form1.DataGridView1.Rows(i).Cells("ΥΠΟΛ.").Value Then
                                pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value = oldcell
                                Throw New System.Exception("Δεν μπορείτε να εισάγετε τη ποσότητα γιατί θα υπερβείτε το όριο για το συγκεκριμένο προϊόν στη συγκεκριμένη παραγγελία!")
                            Else
                                q = pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value - oldcell
                            End If
                        Else
                            q = pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value - oldcell
                        End If
                    End If
                Next
                Using transaction = TransactionUtils.CreateTransactionScope
                    Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=Me.cusid)
                        pm.AddItem(palletid, pallettemplatedatagrid.Rows(e.RowIndex).Cells("iteid").Value, pallettemplatedatagrid.Rows(e.RowIndex).Cells("stlid").Value, pallettemplatedatagrid.Rows(e.RowIndex).Cells("ftrid").Value, q)
                    End Using
                    transaction.Complete()
                End Using
                'success = Form1.pallet_exchange(1, palletid, pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value, pallettemplatedatagrid.Rows(e.RowIndex), True)
                For i As Integer = 0 To Form1.pldt.Rows.Count - 1
                    If Form1.pldt.Rows(i).Item("palletid") = CInt(Me.palletid) Then
                        If Form1.pldt.Rows(i).Item("stlid") = pallettemplatedatagrid.Rows(e.RowIndex).Cells("STLID").Value Then
                            Form1.pldt.Rows(i).Item("quantity") = pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value
                            Exit For
                        End If
                    End If
                Next
                Form1.datagridview1_stlquantity(pallettemplatedatagrid.Rows(e.RowIndex).Cells("stlid").Value)
                Form1.populate_pallets(Me.palletid.ToString, c:=Me)

            End If
        Catch ex As Exception
            'pallettemplatedatagrid.Rows(e.RowIndex).Cells("QUANT").Value = oldcell
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Start()
        Inc = 0
        Dim button4 = New Button()
        button4.Name = "button4"
        button4.Size = New Size(41, 23)
        button4.Location = New Point(215, 33)
        button4.Text = "Ναι"
        button4.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        AddHandler button4.Click, AddressOf Me.Button4_Click
        Dim button5 = New Button()
        button5.Name = "button5"
        button5.Size = New Size(41, 23)
        button5.Location = New Point(262, 33)
        button5.Text = "Όχι"
        button5.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        AddHandler button5.Click, AddressOf Me.Button5_Click
        Dim label1 = New Label
        label1.Name = "label1"
        label1.Text = "ΠΡΟΣΟΧΗ! Είστε σίγουροι;"
        label1.Size = New Size(163, 13)
        label1.Location = New Point(55, 37)
        label1.ForeColor = Color.Red
        Using d As New Drawing.Font("Arial",
                               8.25,
                               FontStyle.Bold)
            label1.Font = d
            Me.Controls.Add(label1)


        End Using
        Me.Controls.Add(button4)
        Me.Controls.Add(button5)
        ' button4.Visible = True
        button5.Visible = True
        button4.Enabled = True
        button5.Enabled = True
        label1.Visible = True
        CheckBox1.Visible = False
        'Label18.Visible = False
        Button1.Visible = False
        Button6.Visible = False
        Button7.Visible = False

        loclbl.Visible = False
    End Sub

    Private Sub pallettemplate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetType(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, Nothing, pallettemplatedatagrid, New Object() {True})
        Me.DoubleBuffered = True
        ToolTip2.SetToolTip(Me.pallettemplatelabel, Me.palletid.ToString)
        Me.viewstyle = Form1.VIEWSTYLE
        If Me.pinned <> -1 Then
            CheckBox2.Checked = True
        End If

        'If Form1.osversion.Major = 6 And Form1.osversion.Minor = 1 Then
        '    Dim font As New Drawing.Font("Segoe UI Symbol Regular",
        '                       20)

        '    Label9.Font = font
        '    Button7.Font = font
        '    Button6.Font = font
        '    CheckBox1.Font = font
        '    Button1.Font = font
        '    Dim p As New Padding(0)
        '    Label9.Font = font
        '    Label9.TextAlign = ContentAlignment.TopCenter

        '    Label9.Margin = p
        '    Button7.Font = font
        '    Button7.TextAlign = ContentAlignment.TopCenter
        '    Button7.Margin = p
        '    Button6.Font = font
        '    Button6.TextAlign = ContentAlignment.TopCenter
        '    Button6.Margin = p
        '    CheckBox1.Font = font
        '    CheckBox1.TextAlign = ContentAlignment.TopCenter
        '    CheckBox1.Margin = p
        '    Button1.Font = font
        '    Button1.TextAlign = ContentAlignment.TopCenter
        '    Button1.Margin = p

        'Else
        '    Dim font As New Drawing.Font("Segoe UI Symbol",
        '                       13)
        '    Dim p As New Padding(0)
        '    Label9.Font = font
        '    Label9.TextAlign = ContentAlignment.TopCenter

        '    Label9.Margin = p
        '    Button7.Font = font
        '    Button7.TextAlign = ContentAlignment.TopCenter
        '    Button7.Margin = p
        '    Button6.Font = font
        '    Button6.TextAlign = ContentAlignment.TopCenter
        '    Button6.Margin = p
        '    CheckBox1.Font = font
        '    CheckBox1.TextAlign = ContentAlignment.TopCenter
        '    CheckBox1.Margin = p
        '    Button1.Font = font
        '    Button1.TextAlign = ContentAlignment.TopCenter
        '    Button1.Margin = p
        'End If
    End Sub



    Private Sub ToolTip1_Popup(sender As Object, e As PopupEventArgs) Handles ToolTip1.Popup

    End Sub
    Dim deleted As Double
    Dim deleted2 As Integer
    Dim deletedROW As DataGridViewRow
    Private Sub pallettemplatedatagrid_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles pallettemplatedatagrid.UserDeletingRow
        Try
            Dim success As Integer = 0
            If e.Row.Cells("frommantis").Value = 1 Then
                e.Cancel = True
                Return
            End If
            Using transaction = TransactionUtils.CreateTransactionScope
                Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=Me.cusid)
                    success = pm.RemoveItem(e.Row.Cells("stlid").Value, palletid, from_mantis:=0)
                End Using
                transaction.Complete()
            End Using
            'success = Form1.pallet_exchange(-1, palletid, 0, e.Row)
            If success <= 0 Then
                e.Cancel = True
                Return
            End If
            deletedROW = e.Row
            deleted = e.Row.Cells("iteid").Value
            deleted2 = e.Row.Cells("stlid").Value
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub pallettemplatedatagrid_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles pallettemplatedatagrid.UserDeletedRow

        Form1.datagridview1_stlquantity(deleted2)
        Form1.populate_pallets(Me.palletid, Me)

    End Sub
    Public oldcell As Double = 0
    Public oldrow As DataGridViewRow = Nothing

    Private Sub pallettemplatedatagrid_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles pallettemplatedatagrid.CellBeginEdit
        oldcell = pallettemplatedatagrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        oldrow = pallettemplatedatagrid.Rows(e.RowIndex)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        If Len(TextBox1.Text) > 0 Then
            Label13.Visible = True
            Label13.Text = Strings.Left(TextBox1.Text, 50).Replace(Environment.NewLine, " ")
        Else
            Label13.Visible = False
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not IsNumeric(Me.TextBox2.Text) Then
            Me.TextBox2.Text = ""

        End If

        Try
            Dim cmd As String = "update tbl_palletheaders set remarks=@textbox1, weight=REPLACE(@textbox2,',','.'), netweight=REPLACE(@textbox4,',','.'), pallettypeid=@comboboxid, LUPDATEUSER=" + Form1.activeuserid.ToString + " where id=" + Me.palletid.ToString
            'Dim cmd As String = "update tbl_palletheaders set remarks='" + Me.TextBox1.Text + "', weight=REPLACE('" + Me.TextBox2.Text.ToString + "',',','.'), netweight=REPLACE('" + Me.TextBox4.Text.ToString + "',',','.'), height=REPLACE('" + Me.TextBox6.Text.ToString + "',',','.'), width=REPLACE('" + Me.TextBox7.Text.ToString + "',',','.'), length=REPLACE('" + Me.TextBox5.Text.ToString + "',',','.'), LUPDATEUSER=" + Form1.activeuserid.ToString + " where id=" + Me.palletid.ToString
            Using sqlcmd As New SqlCommand(cmd, updconn)
                    sqlcmd.Parameters.Add("@textbox1", SqlDbType.Text).Value = TextBox1.Text
                    If Len(TextBox2.Text) > 0 Then
                        sqlcmd.Parameters.Add("@textbox2", SqlDbType.Float).Value = Convert.ToDouble(TextBox2.Text)
                    Else
                        sqlcmd.Parameters.Add("@textbox2", SqlDbType.Float).Value = DBNull.Value
                    End If
                    If Len(TextBox4.Text) > 0 Then
                        sqlcmd.Parameters.Add("@textbox4", SqlDbType.Float).Value = Convert.ToDouble(TextBox4.Text)
                    Else
                        sqlcmd.Parameters.Add("@textbox4", SqlDbType.Float).Value = DBNull.Value
                    End If
                sqlcmd.Parameters.Add("@comboboxid", SqlDbType.Int).Value = ComboBox1.SelectedValue
                updconn.Open()
                If sqlcmd.ExecuteNonQuery() > 0 Then
                    Label17.Text = "Επιτυχής αποθήκευση!"
                    Label17.ForeColor = Color.Green
                    Label17.Visible = True
                    For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                        If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                            If Len(TextBox2.Text) > 0 Then
                                Form1.phdt.Columns("weight").ReadOnly = False
                                Form1.phdt.Rows(i).Item("weight") = Convert.ToDouble(TextBox2.Text)
                                Form1.phdt.Columns("weight").ReadOnly = True
                            End If
                            If Len(TextBox4.Text) > 0 Then
                                Form1.phdt.Columns("netweight").ReadOnly = False
                                Form1.phdt.Rows(i).Item("netweight") = Convert.ToDouble(TextBox4.Text)
                                Form1.phdt.Columns("netweight").ReadOnly = True
                            End If
                            Exit For
                        End If
                    Next
                    Dim txt As String = ""
                    For Each p As SqlParameter In sqlcmd.Parameters
                        If Not txt = "" Then
                            txt = txt + ","
                        End If
                        txt = txt + p.ParameterName + ":" + p.Value.ToString
                    Next
                    Using ut As New PackerUserTransaction
                        ut.WriteEntry(Form1.activeuserid, 39, palletid, value:=txt)
                    End Using
                Else
                    Label17.Text = "Κάτι δεν πήγε καλά!"
                        Label17.ForeColor = Color.Red
                        Label17.Visible = True
                    End If
                    updconn.Close()

            End Using
            Catch ex As Exception
                If updconn.State = ConnectionState.Open Then
                    updconn.Close()
                End If
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
                Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
            End Try

    End Sub
    Dim Inc As Integer = 0
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Me.TextBox2.Text = ""
        Me.TextBox4.Text = ""
        For i As Integer = 0 To Form1.phdt.Rows.Count - 1
            If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                Form1.phdt.Columns("weight").ReadOnly = False
                Form1.phdt.Rows(i).Item("weight") = DBNull.Value
                Form1.phdt.Columns("weight").ReadOnly = True
                Form1.phdt.Columns("netweight").ReadOnly = False
                Form1.phdt.Rows(i).Item("netweight") = DBNull.Value
                Form1.phdt.Columns("netweight").ReadOnly = True
                Exit For
            End If
        Next
        Dim cmd As String = "update tbl_palletheaders set  weight=null, netweight=null, LUPDATEUSER=" + Form1.activeuserid.ToString + " where id=" + Me.palletid
        Dim sqlcmd As New SqlCommand(cmd, updconn)
        updconn.Open()
        Dim res As Integer = -1
        res = sqlcmd.ExecuteNonQuery()
        updconn.Close()
        If res > 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(Form1.activeuserid, 40, palletid)
            End Using
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        'Button4.Visible = False
        For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
            Dim c As Control = Me.Controls(i)
            If c.Name = "button4" Or c.Name = "label1" Or c.Name = "button5" Then
                c.Dispose()
            End If
        Next


        loclbl.Visible = True
        'Label18.Visible = True

        CheckBox1.Visible = True
        Button1.Visible = True
        Button6.Visible = True
        Button7.Visible = True

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Inc += 1
        If Inc > 70 Then
            For i As Integer = (Me.Controls.Count - 1) To 0 Step -1
                Dim c As Control = Me.Controls(i)
                If c.Name = "button4" Or c.Name = "label1" Or c.Name = "button5" Then
                    c.Dispose()
                End If
            Next


            CheckBox1.Visible = True
            Button1.Visible = True
            Button6.Visible = True
            Button7.Visible = True

            'Label18.Visible = True
            loclbl.Visible = True

        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Try
            Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=CInt(custid))
                Dim l As New List(Of Integer)
                l.Add(CInt(Me.palletid))
                pm.Delete(l)
            End Using
            If Form1.FlowLayoutPanel1.Controls.Count = 0 Then
                Dim cmd3 As String = "update TBL_PACKINGLISTS set STATUS=0 where id=" + Form1.plistid.ToString
                Dim sqlcmd3 As New SqlCommand(cmd3, updconn)
                sqlcmd3.ExecuteNonQuery()

            End If
            Form1.palletdeforder.Remove(Me.palletid)
            Form1.build_indexes()
            Dim info As String = Form1.pindex(Me.palletid)
            Dim strs As String() = info.Split(",") 'strings(1) phdt index, strings(2) pldt start, strings(3) pldt end
            Dim phi As Integer = CInt(strs(0))
            Dim pls As Integer = CInt(strs(1))
            Dim ple As Integer = CInt(strs(2))
            If ple >= 0 Then
                For i As Integer = ple To pls Step -1
                    Form1.pldt.Rows.RemoveAt(i)
                Next
            End If
            Form1.phdt.Rows.RemoveAt(phi)
            Form1.pindex.Remove(Me.palletid)
            Form1.FlowLayoutPanel1.Controls.Remove(Me)
            Me.Dispose()
            'Form1.plist_tradecode_report()
            Form1.datagridview1_refresh()
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub



    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

        If Not IsNumeric(Me.TextBox2.Text) Then
            Me.TextBox2.Text = ""
            TextBox4.Text = ""
        Else
            If TextBox2.Text.Contains(".") Then
                TextBox2.Text = TextBox2.Text.Replace(".", ",")
            End If
            Me.TextBox4.Text = TextBox2.Text - 23
        End If

    End Sub

    'Private Sub pallettemplate_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
    '    TabControl1.SelectedIndex = 0
    'End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            CheckBox1.Image = My.Resources.icons8_lock_30__1_
            'ToolTip4.ToolTipTitle = "Κλειδωμένη παλέτα"
            ToolTip4.SetToolTip(CheckBox1, "Πατήστε για να τη ξεκλειδώσετε και να μπορούν και άλλοι χρήστες να επηρεάσουν τα περιεχόμενα της.")
        Else
            CheckBox1.Image = My.Resources.icons8_unlock_30
            ' ToolTip4.ToolTipTitle = "Ξεκλείδωτη παλέτα"
            ToolTip4.SetToolTip(CheckBox1, "Πατήστε για να τη κλειδώσετε. Έτσι εμποδίζετε άλλους χρήστες να πειράξουν τα περιεχόμενα της. Το packing list όμως δεν θα μπορεί να κλείσει.")
        End If

    End Sub

    Private Sub CheckBox1_MouseClick(sender As Object, e As MouseEventArgs) Handles CheckBox1.MouseClick
        Try
            If CheckBox1.Checked = True Then
                Me.Label5.Text = Form1.activeuser
                Dim cmd3 As String = ""
                cmd3 = "update tbl_palletheaders set lockedbyid=" + Form1.activeuserid.ToString + " where id=" + Me.palletid.ToString
                updconn.Open()
                Using sqlcmd3 As New SqlCommand(cmd3, updconn)
                    Dim success As Integer = sqlcmd3.ExecuteNonQuery()
                    updconn.Close()
                    If success <= 0 Then
                        Return
                    Else
                        Using ut As New PackerUserTransaction
                            ut.WriteEntry(Form1.activeuserid, 41, palletid, value:=Me.pallettemplatelabel.Text)
                        End Using
                    End If
                End Using
                CheckBox1.ForeColor = Color.Red
                Me.BackColor = Color.Gainsboro
                Me.Label4.Visible = True
                Me.Label5.Visible = True
                Me.lockedbydpt = Form1.activeuser + "," + Form1.activeuserdpt
                For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                    If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                        Form1.phdt.Columns("lockedbyid").ReadOnly = False
                        Form1.phdt.Rows(i).Item("lockedbyid") = Form1.activeuserid.ToString
                        Form1.phdt.Columns("lockedbyid").ReadOnly = True
                        Exit For
                    End If
                Next
            Else
                updconn.Open()
                Dim cmd3 As String = "update tbl_palletheaders set lockedbyid=null where id=" + Me.palletid.ToString + " and (lockedbyid in (select id from tbl_packeruserdata where department='" + Form1.activeuserdpt + "') or lockedbyid=" + Form1.activeuserid.ToString + " or atlantissalesmanid=(select isnull(atlantisid,0) from tbl_packeruserdata where id=" + Form1.activeuserid.ToString + "))"
                Using sqlcmd3 As New SqlCommand(cmd3, updconn)
                    Dim success As Integer = sqlcmd3.ExecuteNonQuery()
                    updconn.Close()
                    If success <= 0 Then
                        Return
                    Else
                        Using ut As New PackerUserTransaction
                            ut.WriteEntry(Form1.activeuserid, 42, palletid, value:=Me.pallettemplatelabel.Text)
                        End Using
                    End If
                End Using
                Me.BackColor = SystemColors.Control
                CheckBox1.ForeColor = Color.Green
                Me.Label4.Visible = False
                Me.Label5.Visible = False
                Me.lockedbydpt = String.Empty
                For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                    If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                        Form1.phdt.Columns("lockedbyid").ReadOnly = False
                        Form1.phdt.Rows(i).Item("lockedbyid") = DBNull.Value
                        Form1.phdt.Columns("lockedbyid").ReadOnly = True
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            If Me.closed = False Then
                If Not IsDraft Then
                    Dim result As Integer = MessageBox.Show("ΠΡΟΣΟΧΗ: Αν κλείσει η παλέτα δεν θα μπορείτε να την επεξεργαστείτε άλλο και τα προϊόντα θα δεσμευτούν. Είστε σίγουροι;", "ΠΡΟΣΟΧΗ!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                    If result = DialogResult.Yes Then
                        If pallettemplatedatagrid.RowCount = 0 Then
                            Throw New System.Exception("Η παλέτα δεν έχει κανένα προϊόν. Δεν επιτρέπεται να κλείσει.")
                        End If
                        If CheckBox1.Checked = True Then
                            Throw New System.Exception("Ξεκλειδώστε τη παλέτα πρώτα.")
                        End If
                        If Len(TextBox2.Text) = 0 Then
                            Throw New System.Exception("Πρέπει να εισάγετε το βάρος της παλέτας πρώτα.")
                        End If
                        If Len(TextBox4.Text) = 0 Then
                            Throw New System.Exception("Πρέπει να εισάγετε το καθαρό βάρος της παλέτας.")
                        End If
                        If TextBox4.Text = 0 Then
                            Throw New System.Exception("Πρέπει να εισάγετε το καθαρό βάρος της παλέτας.")
                        End If
                        updconn.Open()
                        Dim success As Integer = 0
                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=CInt(custid))
                            success = pm.Close(Me.palletid, TextBox1.Text, TextBox2.Text, TextBox4.Text)
                        End Using
                        If success <= 0 Then
                            Return
                        End If
                        Me.BackColor = Color.LightGreen
                        Me.BorderStyle = BorderStyle.FixedSingle
                        Button6.Image = My.Resources.icons8_approval_30
                        Me.closed = True
                        Label6.Visible = True
                        Label5.Text = Form1.activeuser
                        Label5.Visible = True
                        Me.closedbydpt = Form1.activeuser + "," + Form1.activeuserdpt
                        For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                            If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                                Form1.phdt.Columns("closedbyid").ReadOnly = False
                                Form1.phdt.Rows(i).Item("closedbyid") = Form1.activeuserid.ToString
                                Form1.phdt.Columns("closedbyid").ReadOnly = True
                                Exit For
                            End If
                        Next
                    Else
                        Return
                    End If
                Else
                    Dim result As Integer = -1
                    Dim l As New List(Of Integer)
                    Dim ftr_id As Integer = 0
                    Using s As New SqlCommand("select top 1 ftrid from tbl_palletlines where palletid=" + palletid.ToString + " order by quantity desc", conn)
                        conn.Open()
                        ftr_id = s.ExecuteScalar
                        conn.Close()
                    End Using
                    l.Add(palletid)
                    Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=cusid, ftr_id:=ftr_id)
                        result = pm.Promote(l)
                    End Using
                    If result > 0 Then
                        Me.IsDraft = False
                        state_changed()
                    End If
                End If
            Else
                If Me.BackColor = Color.LightGreen Then
                    Dim success As Integer = -1
                    Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=CInt(custid))
                        success = pm.UnClose(Me.palletid)
                    End Using
                    If success > 0 Then
                        Me.closed = False
                        Label4.Visible = True
                        Label6.Visible = False
                        Button6.Image = My.Resources.icons8_approval_pending_30
                        ToolTip5.SetToolTip(Me.Button6, "Κλείνει τη παλέτα. Πρέπει να έχει συμπληρωθεί το βάρος της παλέτας.")
                        closedbydptvalue = String.Empty
                        Me.plist = String.Empty
                        Me.Label5.Text = Form1.activeuser
                        Me.locked = True
                        Me.lockedbydpt = Form1.activeuser + "," + Form1.activeuserdpt
                        state_changed()
                        For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                            If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                                Form1.phdt.Columns("closedbyid").ReadOnly = False
                                Form1.phdt.Rows(i).Item("closedbyid") = DBNull.Value
                                Form1.phdt.Columns("closedbyid").ReadOnly = True
                                Form1.phdt.Columns("lockedbyid").ReadOnly = False
                                Form1.phdt.Rows(i).Item("lockedbyid") = Form1.activeuserid.ToString
                                Form1.phdt.Columns("lockedbyid").ReadOnly = True
                                Exit For
                            End If
                        Next
                        'Me.BackColor = Color.LightGreen
                        'Me.BorderStyle = BorderStyle.FixedSingle
                        'Me.closed = True
                        'Label6.Visible = True
                        'Label5.Text = Form1.activeuser
                        'Label5.Visible = True
                    End If
                End If

            End If

        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub TextBox2_Leave(sender As Object, e As EventArgs) Handles TextBox2.Leave

    End Sub

    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles TextBox1.Leave

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        Form1.pallettoprint = palletid
        'Form7.ShowDialog()
    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        Form1.TabControl2.SelectedIndex = 1
        'Form1.DataGridView1.CurrentCell = Nothing
        Form1.DataGridView1.ClearSelection()
        'Form1.datagridview1.SelectionMode.
        For i As Integer = 0 To pallettemplatedatagrid.Rows.Count - 1
            For j As Integer = 0 To Form1.DataGridView1.Rows.Count - 1
                If pallettemplatedatagrid.Rows(i).Cells("stlid").Value = Form1.DataGridView1.Rows(j).Cells("stlid").Value Then
                    Form1.DataGridView1.Rows(j).Selected = True
                    'Form1.DataGridView1.CurrentCell = Form1.DataGridView1.Rows(j).Cells(2)
                End If

            Next
        Next

    End Sub

    Private Sub pallettemplate_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown, pallettemplatelabel.MouseDown, loclbl.MouseDown, TabControl1.MouseDown, Label6.MouseDown, Label5.MouseDown, Label12.MouseDown
        MouseDownPos = e.Location
        If Me.closed = False And Me.locked = False And Not Label12.Visible Then
            Form1.Label19.Text = Me.pallettemplatelabel.Text
        End If
        If e.Button = MouseButtons.Right Then

            ContextMenuStrip1.Show(CType(sender, Control), e.Location)
        End If


    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        If Not IsNumeric(Me.TextBox4.Text) Then
            Me.TextBox4.Text = ""
        End If
    End Sub

    Private Sub pallettemplatedatagrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles pallettemplatedatagrid.CellDoubleClick
        Try
            If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then
                Using frm = New ItemDetails(pallettemplatedatagrid.Rows(pallettemplatedatagrid.CurrentRow.Index).Cells(0).Value.ToString)
                    frm.ShowDialog()
                End Using
            ElseIf e.ColumnIndex = 4 Then
                Using frm = New Order(pallettemplatedatagrid.Rows(pallettemplatedatagrid.CurrentRow.Index).Cells(8).Value.ToString)
                    frm.ShowDialog()
                End Using
            End If

        Catch ex As Exception

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
    Dim cur As Cursor
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    'Private Sub pallettemplate_GiveFeedback(sender As Object, e As GiveFeedbackEventArgs) Handles MyBase.GiveFeedback, pallettemplatelabel.GiveFeedback, loclbl.GiveFeedback, TabControl1.GiveFeedback, Label6.GiveFeedback, Label5.GiveFeedback, Label12.GiveFeedback

    '    e.UseDefaultCursors = False
    '    Cursor.Current = cur

    'End Sub

    'Private Sub pallettemplate_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp, pallettemplatelabel.MouseUp, loclbl.MouseUp, TabControl1.MouseUp, Label6.MouseUp, Label5.MouseUp, Label12.MouseUp
    '    Me.Cursor = Cursors.Default
    'End Sub

    'Private Sub pallettemplate_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove, pallettemplatelabel.MouseMove, loclbl.MouseMove, TabControl1.MouseMove, Label6.MouseMove, Label5.MouseMove, Label12.MouseMove
    '    If e.Button And MouseButtons.Left = MouseButtons.Left Then
    '        Dim dx = e.X - MouseDownPos.X
    '        Dim dy = e.Y - MouseDownPos.Y
    '        If Math.Abs(dx) >= SystemInformation.DoubleClickSize.Width OrElse
    '           Math.Abs(dy) >= SystemInformation.DoubleClickSize.Height Then

    '            If Me.closed And Not Label12.Visible Then
    '                Dim txt As String = Me.pallettemplatelabel.Text
    '                Dim gr As Graphics = Me.CreateGraphics()

    '                Dim sz As SizeF = gr.MeasureString(txt, Form1.boldfont)
    '                Dim bmp As New Bitmap(CInt(sz.Width + 60) * 2, CInt(sz.Height + 60))
    '                Try

    '                    gr = Graphics.FromImage(bmp)
    '                    gr.Clear(Color.White)
    '                    gr.DrawIcon(My.Resources.importpallet, CInt(sz.Width + 50), 25)
    '                    gr.DrawString(txt, Form1.boldfont, Brushes.Black, CInt(sz.Width + 60) + 40, 45)
    '                    bmp.MakeTransparent(Color.White)
    '                    cur = New Cursor(bmp.GetHicon())
    '                    Me.DoDragDrop(Me.palletid, DragDropEffects.Copy)
    '                Finally
    '                    gr.Dispose()
    '                    bmp.Dispose()

    '                End Try

    '            End If

    '        End If

    '    End If

    'End Sub

    Private Sub pallettemplate_DoubleClick(sender As Object, e As EventArgs) Handles MyBase.DoubleClick, pallettemplatelabel.DoubleClick, loclbl.DoubleClick, TabControl1.DoubleClick, Label6.DoubleClick, Label5.DoubleClick, Label12.DoubleClick
        Using frm1 As New PalletDetails(Me.palletid)
            frm1.ShowDialog()
        End Using
    End Sub

    Private Sub ΑναλυτικάToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑναλυτικάToolStripMenuItem.Click
        Using frm2 As New PalletDetails(Me.palletid)
            frm2.ShowDialog()
        End Using
    End Sub

    Private Sub pallettemplate_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged

        If Me.Height < 100 And Len(TextBox1.Text) > 0 Then
            Label13.Visible = False
            Label19.Visible = True
        ElseIf Me.Height >= 100 And Len(TextBox1.Text) > 0 Then
            Label13.Visible = True
            Label19.Visible = False
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        'If Me.viewstyle = "MINI" Or Me.viewstyle = "ONELINE" Then
        '    ΕπέκτασηToolStripMenuItem.Text = "Επέκταση"
        'Else

        '    ΕπέκτασηToolStripMenuItem.Text = "Σύμπτυξη"
        'End If
        If hasmantis Then
            ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Text = "Διαγραφή παλετοποίησης MANTIS"
        Else
            ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Text = "Λήψη παλετοποίησης MANTIS"
        End If
        If pallettemplatelabel.Text.Contains("STOCK") Then
            ΜεταφοράΣτοΑπόθεμαToolStripMenuItem.Enabled = False
        Else
            ΜεταφοράΣτοΑπόθεμαToolStripMenuItem.Enabled = True
        End If
    End Sub

    'Private Sub ΕπέκτασηToolStripMenuItem_Click(sender As Object, e As EventArgs)
    '    If ΕπέκτασηToolStripMenuItem.Text = "Σύμπτυξη" Then
    '        Me.viewstyle = "MINI"
    '    Else
    '        Me.viewstyle = "SMALL"
    '    End If


    'End Sub


    Private Sub pallettemplatedatagrid_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles pallettemplatedatagrid.CellValidated

    End Sub

    Private Sub pallettemplatedatagrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles pallettemplatedatagrid.CellValueChanged

    End Sub

    Private Sub pallettemplatedatagrid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles pallettemplatedatagrid.KeyPress
        compute_sums()
    End Sub

    Private Sub pallettemplatedatagrid_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles pallettemplatedatagrid.RowsAdded
        If pallettemplatedatagrid.Rows(e.RowIndex).Cells("frommantis").Value = 1 Then
            pallettemplatedatagrid.Rows(e.RowIndex).ReadOnly = True
            pallettemplatedatagrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGray
            pallettemplatedatagrid.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = Color.Gray

        End If
    End Sub

    Public Sub check_mantis()
        Dim check As Boolean = False
        For Each r As DataGridViewRow In pallettemplatedatagrid.Rows
            If r.Cells("frommantis").Value = 1 Then
                check = True
            End If
        Next
        If check And Not hasmantis Then
            hasmantis = True
        ElseIf Not check And hasmantis Then
            hasmantis = False
        End If
    End Sub


    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            Dim msg = "Εισάγετε νέα παραγγελία που θέλετε να έχει σχέση με ετούτη τη παλέτα"
            Dim result2 As String = InputBox(msg,
                                                      "Εισαγωγή νέας σχετικής παρ παλέτας", " "
                                                      )
            If result2 <> " " And (result2 Like "####" Or result2 Like "#####" Or result2 Like "####[A-Z]" Or result2 Like "#####[A-Z]") Then
                Using com As New SqlCommand("select isnull(1,0) from tbl_packerordercheck t inner join fintrade f on f.id=t.ftrid where t.status<>12 and f.dsrid in (9000,9008) and dbo.get_tradecode(f.ID) like '%" + result2 + "'", updconn)
                    updconn.Open()
                    Dim step1 As Integer = com.ExecuteScalar
                    updconn.Close()
                    Dim boo As New List(Of String)(TextBox9.Text.Split(","c))
                    Dim list1 = boo.Select(Function(i) "'" + i + "'")
                    Dim string2 = String.Join(",", list1.ToArray())
                    If step1 = 1 Then
                        Using com2 As New SqlCommand("select cusid from fintrade f where dsrid in (9000,9008) and dbo.get_tradecode(f.ID) like '%" + result2 + "'", updconn)
                            Using com3 As New SqlCommand("select cusid from TBL_PALLETHEADERS WHERE ID=" + Me.palletid, updconn)
                                updconn.Open()
                                Dim step2a As Integer = com2.ExecuteScalar
                                Dim step2b As Integer = com3.ExecuteScalar
                                updconn.Close()

                                If step2a = step2b Then
                                    Dim s As String = TextBox9.Text + "," + result2.ToString
                                    Using com4 As New SqlCommand("update tbl_palletheaders set orders=orders+','+'" + result2.ToString + "',lupdateuser=" + Form1.activeuserid.ToString + " where id=" + palletid, updconn)
                                        updconn.Open()
                                        Dim success As Integer = com4.ExecuteNonQuery
                                        updconn.Close()
                                        If success > 0 Then
                                            TextBox9.Text = s
                                            For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                                                If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                                                    Form1.phdt.Columns("orders").ReadOnly = False
                                                    Form1.phdt.Rows(i).Item("orders") = s
                                                    Form1.phdt.Columns("orders").ReadOnly = True
                                                    Exit For
                                                End If
                                            Next
                                            Using ut As New PackerUserTransaction
                                                ut.WriteEntry(Form1.activeuserid, 44, palletid, value:=result2.ToString)
                                            End Using
                                        End If
                                    End Using
                                Else
                                    Throw New System.Exception("Η παραγγελία που επιχειρήσατε να εισάγετε ανήκει σε άλλον πελάτη!")
                                End If
                            End Using
                        End Using
                    Else
                        Throw New System.Exception("Δεν υπάρχει διαθέσιμη, μη απεσταλμένη παραγγελία με τον αριθμό που εισάγατε!")
                    End If
                End Using
            End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            Dim msg = "Εισάγετε σχόλια:"
            Dim result2 As String = InputBox(msg,
                                                      "Προσθήκη σχολίων", " "
                                                      )
            If Not String.IsNullOrWhiteSpace(result2) Then
                Dim txt As String = result2 + ", " + Form1.activeuser + ", " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine
                Using cmd As New SqlCommand("update tbl_palletheaders set remarks=@remarks+cast(isnull(remarks,'') as varchar(7000)) where id=" + palletid, updconn)
                    cmd.Parameters.Add("@remarks", SqlDbType.VarChar, 8000).Value = txt
                    updconn.Open()
                    If cmd.ExecuteNonQuery() > 0 Then
                        Label17.Text = "Επιτυχής αποθήκευση!"
                        Label17.ForeColor = Color.Green
                        Label17.Visible = True
                        TextBox1.Text = txt + TextBox1.Text
                        For i As Integer = 0 To Form1.phdt.Rows.Count - 1
                            If Me.palletid = Form1.phdt.Rows(i).Item("ID").ToString Then
                                Form1.phdt.Columns("remarks").ReadOnly = False
                                Form1.phdt.Rows(i).Item("remarks") = TextBox1.Text
                                Form1.phdt.Columns("remarks").ReadOnly = True
                                Exit For
                            End If
                        Next
                        Using ut As New PackerUserTransaction
                            ut.WriteEntry(Form1.activeuserid, 45, palletid, value:=result2)
                        End Using
                    Else
                        Label17.Text = "Κάτι δεν πήγε καλά!"
                        Label17.ForeColor = Color.Red
                        Label17.Visible = True
                    End If
                    updconn.Close()
                End Using
            End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Click
        Try

            If ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Text = "Λήψη παλετοποίησης MANTIS" Then
                If Not Me.closed Then 'ΣΧΕΔΟΝ ΙΔΙΑ ΔΙΑΔΙΚΑΣΙΑ ΜΕ FORM1.Button14_Click
                    Dim cs As String() = Form1.CUSTOMER.Split(" ")
                    Dim cmd As String = "select FTRID,s.a as 'ΠΑΡ',z.iteid,stlid,diff as 'ΠΟΣ', from [dbo].[Z_PACKER_PENDING_ITEMS_PER_ORDER] z 
                            inner join fintrade f on f.id=z.FTRID INNER JOIN CUSTOMER C ON C.ID=F.CUSID
                            left join TBL_PACKERORDCLINES t1 on t1.stlid=z.stlid
                            cross apply (select   dbo.get_tradecode(f.id) a) s
                            where f.DSRID in (9000,9008) and diff<>0 and isnull(t1.sc_recipient,0) in (0,2)
                            And C.CODE='" + cs(0) + "'"
                    Dim check1 As String = ""
                    Dim check2 As String = ""
                    Dim fs As String = ""
                    Dim fs0 As String = ""
                    Using small As New SqlCommand("select distinct ftrid from tbl_packerordercheck z inner join fintrade f on f.id=z.ftrid inner join customer c on c.id=f.cusid where f.dsrid in (9000,9008) and z.status<>12 and c.code='" + cs(0).ToString + "'", conn)
                        Using dt0 As New DataTable
                            conn.Open()
                            Using dt0reader As IDataReader = small.ExecuteReader
                                dt0.Load(dt0reader)
                                conn.Close()
                                fs0 = String.Join(",", dt0.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray())
                            End Using
                        End Using
                    End Using
                    Dim start As String = "select id from fintrade where id in  (" + fs0 + ") 
                                    or sc_relftrid in (" + fs0 + ")"
                    Using startcom As New SqlCommand(start, conn)

                        Using startdt As New DataTable
                            conn.Open()
                            Using startreader As SqlDataReader = startcom.ExecuteReader
                                startdt.Load(startreader)
                                conn.Close()
                                Dim f = startdt.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray()
                                fs = String.Join(",", f)
                            End Using
                        End Using
                    End Using
                    check1 = "select mtr.code 'ΕΙΔΟΣ',mtr.DESCRIPTION 'ΠΕΡΙΓΡΑΦΗ' from
                                        (select stl.iteid i,sum(stl.primaryqty) s from itemtransest stl
										where ftrid in  (" + fs + ")
                                        group by stl.iteid) a
                                        inner join
                                        ( select iteid i,sum(lsumqty) s from sc_qty_mantisax_returns where locationid in
                                        (select locid from TBL_PALLETHEADERS ph where ph.PLID is  null and substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + cs(1).ToString.Replace(".", "") + "') 
                                        group by iteid   ) m      
                                        on a.i=m.i        
                                        inner join material mtr on mtr.ID=a.i      
                                        where m.s>a.s    "
                    check2 = "select m.code 'ΕΙΔΟΣ',m.description 'ΠΕΡΙΓΡΑΦΗ',s.locationcode 'ΘΕΣΗ ΑΠΟΘΗΚΕΥΣΗΣ' from  sc_qty_mantisax_returns s inner join material m on m.id=s.iteid
                                    where locationid in
                                    (select locid from TBL_PALLETHEADERS ph where ph.PLID is null and substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + cs(1).ToString.Replace(".", "") + "') 
                                    and iteid not in
                                    (select iteid from STORETRADELINES where ftrid in (" + fs + "))"

                    Using checkcomm1 As New SqlCommand(check1, conn)
                        Using checkcomm2 As New SqlCommand(check2, conn)
                            Using ch1dt As New DataTable()
                                Using ch2dt As New DataTable()
                                    conn.Open()

                                    Using cr1 As SqlDataReader = checkcomm1.ExecuteReader
                                        ch1dt.Load(cr1)
                                        Using cr2 As SqlDataReader = checkcomm2.ExecuteReader


                                            ch2dt.Load(cr2)
                                            conn.Close()

                                            If ch2dt.Rows.Count > 0 Or ch1dt.Rows.Count > 0 Then
                                                Using frm As New mantiserror(ch1dt, ch2dt)
                                                    frm.ShowDialog()
                                                End Using
                                                Return
                                            End If
                                        End Using
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using
                    Using comm As New SqlCommand(cmd, updconn)
                        Using dt = New DataTable()
                            updconn.Open()
                            Using reader As SqlDataReader = comm.ExecuteReader()
                                dt.Load(reader)
                                updconn.Close()
                                If dt.Rows.Count = 0 Then
                                    Throw New System.Exception("Έχουν κατανεμηθεί όλα τα είδη της παραγγελίας.")
                                End If
                                dt.Columns.Add("newQuant", Type.GetType("System.Double"))

                            End Using



                            Dim cmd1 As String = "select iteid,LSUMQTY,locationID from SC_QTY_MANTISAX_RETURNS where locationID =" + Me.locationID.ToString
                            Using comm1 As New SqlCommand(cmd1, conn)
                                Using dt1 = New DataTable()
                                    conn.Open()
                                    Using reader1 As SqlDataReader = comm1.ExecuteReader()
                                        dt1.Load(reader1)
                                        conn.Close()
                                        If dt1.Rows.Count = 0 Then
                                            Throw New System.Exception("Δεν έχουν καταχωρηθεί είδη στις διαθέσιμες θέσεις αποθήκευσης/παλέτες.")
                                        End If
                                        Using dt3 = New DataTable()
                                            dt3.Columns.Add("ΚΩΔΙΚΟΣ", Type.GetType("System.String"))
                                            dt3.Columns.Add("ΠΕΡΙΓΡΑΦΗ", Type.GetType("System.String"))
                                            dt3.Columns.Add("ΠΑΡ", Type.GetType("System.String"))
                                            dt3.Columns.Add("Παλέτα", Type.GetType("System.String"))
                                            dt3.Columns.Add("Θέση αποθήκευσης", Type.GetType("System.String"))
                                            dt3.Columns.Add("Ποσότητα", Type.GetType("System.Double"))
                                            dt3.Columns.Add("stlid", Type.GetType("System.Int32"))
                                            dt3.Columns.Add("ftrid", Type.GetType("System.Int32"))
                                            dt3.Columns.Add("palletid", Type.GetType("System.Int32"))
                                            dt3.Columns.Add("ΚΩΔΠΕΛ", Type.GetType("System.String"))
                                            dt3.Columns.Add("ΠΕΛ", Type.GetType("System.String"))
                                            dt3.Columns.Add("iteid", Type.GetType("System.Int32"))
                                            For i As Integer = 0 To dt.Rows.Count - 1
                                                dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("ΠΟΣ")

                                                For j As Integer = 0 To dt1.Rows.Count - 1
                                                    If dt1.Rows(j).Item("iteid") = dt.Rows(i).Item("iteid") Then

                                                        Dim quantity As Double = 0
                                                        If dt.Rows(i).Item("newQuant") <= 0 Then
                                                            Continue For
                                                        ElseIf dt1.Rows(j).Item("lsumqty") <= dt.Rows(i).Item("newQuant") Then
                                                            quantity = dt1.Rows(j).Item("lsumqty")
                                                        Else
                                                            quantity = dt.Rows(i).Item("newQuant")
                                                        End If
                                                        dt1.Rows(j).Item("lsumqty") = dt1.Rows(j).Item("lsumqty") - quantity
                                                        dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("newQuant") - quantity
                                                        Dim dr As DataRow = dt3.NewRow
                                                        dr("ΚΩΔΙΚΟΣ") = dt.Rows(i).Item("ΚΩΔΙΚΟΣ")
                                                        dr("ΠΕΡΙΓΡΑΦΗ") = dt.Rows(i).Item("ΠΕΡΙΓΡΑΦΗ")
                                                        dr("ΠΑΡ") = dt.Rows(i).Item("ΠΑΡ")
                                                        dr("Παλέτα") = Me.pallettemplatelabel.Text
                                                        dr("Θέση αποθήκευσης") = Me.locationCode
                                                        dr("Ποσότητα") = quantity
                                                        dr("stlid") = dt.Rows(i).Item("stlid")
                                                        dr("ftrid") = dt.Rows(i).Item("ftrid")
                                                        dr("iteid") = dt.Rows(i).Item("iteid")
                                                        dr("palletid") = Me.palletid
                                                        dr("ΠΕΛ") = cs(1)
                                                        dr("ΚΩΔΠΕΛ") = cs(0)
                                                        If dr("Ποσότητα") = 0 Then
                                                            Continue For
                                                        Else
                                                            dt3.Rows.Add(dr)
                                                        End If
                                                    End If
                                                Next
                                            Next
                                            For i As Integer = 0 To dt.Rows.Count - 1
                                                If dt.Rows(i).Item("newQuant") = dt.Rows(i).Item("ΠΟΣ") Then
                                                    dt.Rows(i).Delete()
                                                End If
                                            Next

                                            Using frm As New datafrommantisfrm(dt, dt3)
                                                frm.ShowDialog()
                                            End Using
                                        End Using
                                    End Using
                                End Using
                            End Using
                        End Using
                    End Using

                End If
                Cursor.Current = Cursors.Default
            ElseIf ΑνανέωσηΠαλετοποίησηςMANTISToolStripMenuItem.Text = "Διαγραφή παλετοποίησης MANTIS" Then 'ΣΧΕΔΟΝ ΙΔΙΑ ΔΙΑΔΙΚΑΣΙΑ ΜΕ FORM1.Button23_Click
                Dim cmd As String = "delete from tbl_palletlines where frommantis=1 and palletid =" + palletid
                Using com As New SqlCommand(cmd, updconn)
                    updconn.Open()
                    Dim success = com.ExecuteNonQuery
                    updconn.Close()

                    If success <= 0 Then
                        Throw New System.Exception("Απέτυχε.")
                    End If
                    For i As Integer = Form1.pldt.Rows.Count - 1 To 0 Step -1
                        If Form1.pldt.Rows(i).Item("frommantis") = 1 Then
                            Form1.pldt.Rows.RemoveAt(i)
                        End If
                    Next
                    Form1.datagridview1_refresh()
                    Form1.orderdgv_refresh()
                    Form1.populate_pallets(Me.palletid)
                    Form1.change_frommantis()
                End Using


            End If
        Catch ex As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub pallettemplatedatagrid_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles pallettemplatedatagrid.RowsRemoved
        check_mantis()
    End Sub

    Public wassmall As Boolean = False


    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            CheckBox2.BackColor = Color.Black
            CheckBox2.ForeColor = Color.White

        Else
            CheckBox2.BackColor = Color.Transparent
            CheckBox2.ForeColor = Color.Black

        End If
    End Sub

    Public Sub checkbox2_clicked(sender As Object, e As EventArgs) Handles CheckBox2.Click
        If CheckBox2.Checked Then


            Me.pinned = Form1.nextpinnedlocation
            Form1.nextpinnedlocation += 1
            'Using sqlcmd As New SqlCommand
            '    sqlcmd.CommandText = "insert into tbl_packerpinnedpallets (palletid,userid) values (" + palletid.ToString + "," + Form1.activeuserid.ToString + ")"
            '    sqlcmd.Connection = updconn
            '    updconn.Open()
            '    sqlcmd.ExecuteNonQuery()
            '    updconn.Close()
            'End Using

        Else

            Form1.nextpinnedlocation -= 1
            Me.pinned = -1
            'Using sqlcmd As New SqlCommand
            '    sqlcmd.CommandText = "delete from tbl_packerpinnedpallets where palletid=" + palletid.ToString + " and userid=" + Form1.activeuserid.ToString
            '    sqlcmd.Connection = updconn
            '    updconn.Open()
            '    sqlcmd.ExecuteNonQuery()
            '    updconn.Close()
            'End Using
        End If
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Cursor.Current = Form1.ExtCursor1.Cursor
        Try
            Dim cmd1 As String = "select iteid,LSUMQTY,locationID from SC_QTY_MANTISAX_RETURNS where locationID =" + Me.locationID.ToString
            Dim cmd2 As String = "select iteid,sum(quantity) as quantity from tbl_palletlines where frommantis=1 and palletid=" + Me.palletid + " and quantity<>0 group by iteid"
            Dim cmd4 As String = "select iteid,stlid,ftrid,quantity from tbl_palletlines where frommantis=1 and palletid=" + Me.palletid
            Dim cmd3 As String = "select z.FTRID,s_ar_pel,z.iteid,z.stlid,diff as 'ΠΟΣ' from [dbo].[Z_PACKER_PENDING_ITEMS_PER_ORDER] z 
                            inner join fintrade f on f.id=z.FTRID 
                            left join TBL_PACKERORDCLINES t1 on t1.stlid=z.stlid and line=1
                            where f.DSRID in (9000,9008) and diff<>0 and isnull(t1.sc_recipient,0) in (0,2)
                            And f.cusid=" + Me.cusid + " order by 2,1 asc"
            Dim cmd5 As String = "select ISNULL(closedbyid,0),ISNULL(plid,0) from tbl_palletheaders where id=" + Me.palletid
            Dim checkdt = New DataTable()
            Using sqlcmd As New SqlCommand(cmd5, conn)
                conn.Open()

                Using reader As SqlDataReader = sqlcmd.ExecuteReader()

                    checkdt.LOAD(reader)


                End Using
                conn.Close()
            End Using
            Dim customer As String = ""

            Using comm1 As New SqlCommand(cmd1, conn)
                Using mantisdt = New DataTable()

                    conn.Open()
                    Using reader1 As SqlDataReader = comm1.ExecuteReader()

                        mantisdt.Load(reader1)
                        conn.Close()


                        If mantisdt.Rows.Count <> 0 And checkdt.Rows(0).Item(0) = 0 And checkdt.Rows(0).Item(1) = 0 Then
                            problem = False
                            mantis_qa_check()
                            If problem Then
                                Return
                            End If
                            conn.Open()
                            Using comm2 As New SqlCommand(cmd2, conn)
                                Using comm3 As New SqlCommand(cmd3, conn)
                                    Using reader2 As SqlDataReader = comm2.ExecuteReader()
                                        Using palletsumdt = New DataTable()
                                            palletsumdt.Load(reader2)
                                            conn.Close()
                                            If customer = "" Then
                                                Using cuscmd As New SqlCommand("select fathername from customer where id=" + Me.cusid, conn)
                                                    conn.Open()
                                                    customer = cuscmd.ExecuteScalar
                                                    conn.Close()
                                                End Using
                                            End If
                                            For i As Integer = Form1.pldt.Rows.Count - 1 To 0 Step -1
                                                If Form1.pldt.Rows(i).Item("PALLETID") = Me.palletid AndAlso Form1.pldt.Rows(i).Item("frommantis") = 1 Then
                                                    Dim found As Boolean = False
                                                    For k As Integer = 0 To mantisdt.Rows.Count - 1
                                                        If mantisdt.Rows(k).Item("iteid") = Form1.pldt.Rows(i).Item("iteid") Then
                                                            found = True
                                                            Exit For
                                                        End If
                                                    Next
                                                    If Not found Then
                                                        Using transaction = TransactionUtils.CreateTransactionScope
                                                            Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=Me.cusid)
                                                                pm.RemoveItem(Form1.pldt.Rows(i).Item("stlid"), palletid, from_mantis:=1)
                                                            End Using
                                                            transaction.Complete()
                                                        End Using
                                                        'Form1.pallet_exchange(-1, Me.palletid, 0, r, frommantis:=True, dontupdate:=True)
                                                    End If
                                                End If
                                            Next
                                            conn.Open()
                                            Dim reader3 As SqlDataReader = comm3.ExecuteReader()
                                            Dim neededdt = New DataTable()
                                            neededdt.Load(reader3)
                                            conn.Close()
                                            Try
                                                Dim lst As New List(Of Integer)
                                                For Each p As DataRow In neededdt.Rows
                                                    lst.Add(p.Item("iteid"))
                                                Next
                                                For j As Integer = mantisdt.Rows().Count - 1 To 0 Step -1
                                                    Dim skip As Boolean = False
                                                    For i As Integer = 0 To palletsumdt.Rows.Count - 1
                                                        If palletsumdt.Rows(i).Item("iteid") = mantisdt.Rows(j).Item("iteid") Then
                                                            Dim inserted As Boolean = False
                                                            Dim needed As Double = 0
                                                            If palletsumdt.Rows(i).Item("quantity") = mantisdt.Rows(j).Item("LSUMQTY") Then
                                                                skip = True
                                                                mantisdt.Rows.RemoveAt(j)
                                                                Exit For
                                                            Else
                                                                needed = mantisdt.Rows(j).Item("LSUMQTY")
                                                                If customer = "" Then
                                                                    Using cuscmd As New SqlCommand("select fathername from customer where id=" + Me.cusid, conn)
                                                                        conn.Open()
                                                                        customer = cuscmd.ExecuteScalar
                                                                        conn.Close()
                                                                    End Using
                                                                End If
                                                                Dim deleted As Boolean = False
                                                                If Not lst.Contains(palletsumdt.Rows(i).Item("iteid")) Then
                                                                    Using ercom As New SqlCommand("delete from tbl_palletlines where palletid=" + Me.palletid + " and frommantis=1 and iteid=" + palletsumdt.Rows(i).Item("iteid").ToString, updconn)
                                                                        updconn.Open()
                                                                        ercom.ExecuteNonQuery()
                                                                        updconn.Close()
                                                                        conn.Open()
                                                                        reader3 = comm3.ExecuteReader
                                                                        neededdt.Clear()
                                                                        neededdt.Load(reader3)
                                                                        conn.Close()
                                                                        deleted = True
                                                                        For ji As Integer = 0 To Form1.pldt.Rows.Count - 1
                                                                            If Form1.pldt.Rows(ji).Item("palletid") = Me.palletid AndAlso Form1.pldt.Rows(ji).Item("iteid") = palletsumdt.Rows(i).Item("iteid") And Form1.pldt.Rows(ji).Item("frommantis") = 1 Then
                                                                                Form1.pldt.Rows.RemoveAt(ji)
                                                                                Exit For
                                                                            End If
                                                                        Next
                                                                    End Using
                                                                End If
                                                                For k As Integer = 0 To neededdt.Rows.Count - 1
                                                                    If neededdt.Rows(k).Item("iteid") = mantisdt.Rows(j).Item("iteid") Then
                                                                        If customer = "" Then
                                                                            Using cuscmd As New SqlCommand("select fathername from customer where id=" + Me.cusid, conn)
                                                                                conn.Open()
                                                                                customer = cuscmd.ExecuteScalar
                                                                                conn.Close()
                                                                            End Using
                                                                        End If
                                                                        Dim q As Double
                                                                        If neededdt.Rows(k).Item("ΠΟΣ") >= needed - palletsumdt.Rows(i).Item("quantity") Then
                                                                            q = needed - palletsumdt.Rows(i).Item("quantity")
                                                                        Else
                                                                            q = neededdt.Rows(k).Item("ΠΟΣ")
                                                                        End If
                                                                        Dim rowindex As Integer = Me.pallettemplatedatagrid.Rows.Add()
                                                                        Using r As DataGridViewRow = Me.pallettemplatedatagrid.Rows(rowindex)
                                                                            r.Cells("iteid").Value = mantisdt.Rows(j).Item("iteid")
                                                                            r.Cells("ftrid").Value = neededdt.Rows(k).Item("ftrid")
                                                                            r.Cells("ΠΑΡ").Value = neededdt.Rows(k).Item("s_ar_pel")
                                                                            r.Cells("stlid").Value = neededdt.Rows(k).Item("stlid")
                                                                            r.Cells("ΠΕΛ").Value = customer
                                                                            r.Cells("ΚΩΔΠΕΛ").Value = Me.customercode
                                                                            Using transaction = TransactionUtils.CreateTransactionScope
                                                                                Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=Me.cusid)
                                                                                    If deleted Then
                                                                                        pm.AddItem(Me.palletid, r.Cells("iteid").Value, r.Cells("stlid").Value, r.Cells("ftrid").Value, needed, from_mantis:=1)
                                                                                        'Form1.pallet_exchange(1, Me.palletid, needed, r, frommantis:=True, dontupdate:=True)
                                                                                    Else
                                                                                        pm.AddItem(Me.palletid, r.Cells("iteid").Value, r.Cells("stlid").Value, r.Cells("ftrid").Value, q, from_mantis:=1)
                                                                                    End If
                                                                                End Using
                                                                                transaction.Complete()
                                                                            End Using
                                                                            inserted = True
                                                                        End Using
                                                                        needed -= q
                                                                        If needed <= 0 Then
                                                                            Exit For
                                                                        End If
                                                                    End If
                                                                Next
                                                            End If
                                                            mantisdt.Rows.RemoveAt(j)
                                                            Exit For
                                                        End If
                                                    Next
                                                    If skip Then
                                                        Continue For
                                                    End If
                                                Next
                                                If mantisdt.Rows.Count > 0 Then
                                                    If customer = "" Then
                                                        Using cuscmd As New SqlCommand("select fathername from customer where id=" + Me.cusid, conn)
                                                            conn.Open()
                                                            customer = cuscmd.ExecuteScalar
                                                            conn.Close()
                                                        End Using
                                                    End If
                                                    For j As Integer = 0 To mantisdt.Rows.Count - 1
                                                        Dim needed = mantisdt.Rows(j).Item("LSUMQTY")
                                                        For k As Integer = 0 To neededdt.Rows.Count - 1
                                                            If neededdt.Rows(k).Item("iteid") = mantisdt.Rows(j).Item("iteid") Then
                                                                Dim q As Double
                                                                If neededdt.Rows(k).Item("ΠΟΣ") >= needed Then
                                                                    q = needed
                                                                Else
                                                                    q = neededdt.Rows(k).Item("ΠΟΣ")
                                                                End If
                                                                Dim rowindex As Integer = Me.pallettemplatedatagrid.Rows.Add()
                                                                Using r As DataGridViewRow = Me.pallettemplatedatagrid.Rows(rowindex)
                                                                    r.Cells("iteid").Value = mantisdt.Rows(j).Item("iteid")
                                                                    r.Cells("ftrid").Value = neededdt.Rows(k).Item("ftrid")
                                                                    r.Cells("ΠΑΡ").Value = neededdt.Rows(k).Item("s_ar_pel")
                                                                    r.Cells("stlid").Value = neededdt.Rows(k).Item("stlid")
                                                                    r.Cells("ΠΕΛ").Value = customer
                                                                    r.Cells("ΚΩΔΠΕΛ").Value = Me.customercode
                                                                    Using transaction = TransactionUtils.CreateTransactionScope
                                                                        Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=Me.cusid)
                                                                            pm.AddItem(Me.palletid, r.Cells("iteid").Value, r.Cells("stlid").Value, r.Cells("ftrid").Value, q, from_mantis:=1)
                                                                            'Form1.pallet_exchange(1, Me.palletid, needed, r, frommantis:=True, dontupdate:=True)
                                                                        End Using
                                                                        transaction.Complete()
                                                                    End Using
                                                                    'Form1.pallet_exchange(1, Me.palletid, q, r, frommantis:=True, dontupdate:=True)
                                                                End Using
                                                                needed -= q
                                                                If needed <= 0 Then
                                                                    Exit For
                                                                End If
                                                            End If
                                                        Next
                                                    Next
                                                End If
                                                If customer = "" Then
                                                    Using cuscmd As New SqlCommand("select fathername from customer where id=" + Me.cusid, conn)
                                                        conn.Open()
                                                        customer = cuscmd.ExecuteScalar
                                                        conn.Close()
                                                    End Using
                                                End If
                                            Finally
                                                reader3.Dispose()
                                                neededdt.Dispose()
                                            End Try
                                        End Using
                                    End Using
                                End Using
                            End Using
                        End If
                    End Using
                End Using
            End Using
            Form1.populate_pallets(palletid.ToString, c:=Me)
            Form1.datagridview1_refresh()
            Cursor.Current = Cursors.Default
        Catch EX As Exception
            If updconn.State = ConnectionState.Open Then
                updconn.Close()
            End If
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            Using errfrm As New errormsgbox(EX.StackTrace.ToString, EX.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using

        End Try
    End Sub

    Dim ticker As Integer = 0
    Private Sub Timer2_Tick(sender As Object, e As EventArgs)
        ticker += 1
        If ticker = 1 Then

        End If
    End Sub

    Private Sub pallettemplate_MouseEnter(sender As Object, e As EventArgs) Handles MyBase.MouseEnter

    End Sub

    Dim problem As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ComboBox1.DataSource = Form1.pallettypes
        ComboBox1.DisplayMember = "name"
        ComboBox1.ValueMember = "id"
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub mantis_qa_check()
        Dim check1 As String = ""
        Dim check2 As String = ""
        Dim fs As String = ""
        Dim fs0 As String = ""
        Using small As New SqlCommand("select distinct ftrid from tbl_packerordercheck z inner join fintrade f on f.id=z.ftrid inner join customer c on c.id=f.cusid where f.dsrid in (9000,9008) and z.status<>12 and c.id=" + cusid, conn)
            Using dt0 As New DataTable
                conn.Open()
                Using dt0reader As IDataReader = small.ExecuteReader
                    dt0.Load(dt0reader)
                    conn.Close()
                    fs0 = String.Join(",", dt0.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray())
                End Using
            End Using
        End Using
        Dim start As String = "select id from fintrade where id in  (" + fs0 + ") 
                                    or sc_relftrid in (" + fs0 + ")"
        Using startcom As New SqlCommand(start, conn)

            Using startdt As New DataTable
                conn.Open()
                Using startreader As SqlDataReader = startcom.ExecuteReader
                    startdt.Load(startreader)
                    conn.Close()
                    Dim f = startdt.AsEnumerable().Select(Function(d) DirectCast(d(0).ToString(), Object)).ToArray()
                    fs = String.Join(",", f)
                End Using
            End Using
        End Using
        check1 = "select mtr.code 'ΕΙΔΟΣ',mtr.DESCRIPTION 'ΠΕΡΙΓΡΑΦΗ' from
                                        (select stl.iteid i,sum(stl.primaryqty) s from itemtransest stl
										where ftrid in  (" + fs + ")
                                        group by stl.iteid) a
                                        inner join
                                        ( select iteid i,sum(lsumqty) s from sc_qty_mantisax_returns where locationid in
                                        (select locid from TBL_PALLETHEADERS ph where ph.PLID is  null and substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + customer.ToString.Replace(".", "") + "') 
                                        group by iteid   ) m      
                                        on a.i=m.i        
                                        inner join material mtr on mtr.ID=a.i      
                                        where m.s>a.s    "
        check2 = "select m.code 'ΕΙΔΟΣ',m.description 'ΠΕΡΙΓΡΑΦΗ',s.locationcode 'ΘΕΣΗ ΑΠΟΘΗΚΕΥΣΗΣ' from  sc_qty_mantisax_returns s inner join material m on m.id=s.iteid
                                    where locationid in
                                    (select locid from TBL_PALLETHEADERS ph where ph.PLID is null and substring(ph.code,1,PATINDEX('%[0123456789]%', ph.code)-1)='" + customer.ToString.Replace(".", "") + "') 
                                    and iteid not in
                                    (select iteid from STORETRADELINES where ftrid in (" + fs + "))"

        Using checkcomm1 As New SqlCommand(check1, conn)
            Using checkcomm2 As New SqlCommand(check2, conn)
                Using ch1dt As New DataTable()
                    Using ch2dt As New DataTable()
                        conn.Open()

                        Using cr1 As SqlDataReader = checkcomm1.ExecuteReader
                            ch1dt.Load(cr1)
                            Using cr2 As SqlDataReader = checkcomm2.ExecuteReader


                                ch2dt.Load(cr2)
                                conn.Close()

                                If ch2dt.Rows.Count > 0 Or ch1dt.Rows.Count > 0 Then
                                    problem = True
                                    Using frm As New mantiserror(ch1dt, ch2dt)
                                        frm.ShowDialog()
                                    End Using
                                    Return
                                End If
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub pallettemplate_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

    End Sub

    Private Sub pallettemplate_MouseLeave(sender As Object, e As EventArgs) Handles MyBase.MouseLeave
        If pinned = -1 And wassmall And Not ClientRectangle.Contains(PointToClient(Control.MousePosition)) Then
            Form1.pallet_morph(Me.palletid, 2, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)

        End If
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click
        Using f As New PalletTypes
            f.ShowDialog()
        End Using
    End Sub

    Private Sub ΜεταφοράΣτοΑπόθεμαToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ΜεταφοράΣτοΑπόθεμαToolStripMenuItem.Click
        Try
            If Not (closedbydptvalue <> String.Empty And packinglist = String.Empty) Then
                Throw New Exception("Μόνο ολοκληρωμένες παλέτες μπορούν να μεταφερθούν στο απόθεμα!")
            End If
            Dim ret = MsgBox("Η παλέτα θα μεταφερθεί στο απόθεμα. Είστε σίγουροι;", MessageBoxButtons.OKCancel, "Είστε σίγουροι;")
            If ret = MsgBoxResult.Ok Then
                Using pm As New PalletManager(Form1.activeuserdpt, Form1.activeuser, Form1.activeuserid, Form1.activeuserdptid, cus_id:=Me.cusid, stock:=True)
                    pm.OrderPalletToStock(Me.palletid)
                End Using
            End If
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 2 Then
            DataGridView1.Visible = False
            Dim pic As New PictureBox
            pic.SizeMode = PictureBoxSizeMode.CenterImage
            pic.Image = My.Resources.rolling
            pic.Dock = DockStyle.Fill
            TabPage3.Controls.Add(pic)
            dpworker.RunWorkerAsync()
        End If
    End Sub

    Private Sub dpworker_DoWork(sender As Object, e As DoWorkEventArgs) Handles dpworker.DoWork
        Using s As New SqlCommand("select pl.iteid,pl.stlid,pl.ftrid,pl.dailyplanid,m.code,dp.date,sum(dp.quantity),'ΣΥΣΚΕΥΑΣΙΑ' from tbl_palletheaders ph inner join tbl_palletlines pl on pl.palletid=ph.id inner join material m on m.id=pl.iteid inner join pkrtbl_dailyplan dp on dp.id=pl.dailyplanid where ph.id=" + palletid.ToString + " group by pl.iteid,pl.stlid,pl.ftrid,pl.dailyplanid,m.code,dp.date", conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                    e.Result = dt
                End Using
            End Using
        End Using
    End Sub

    Private Sub dpworker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles dpworker.RunWorkerCompleted
        Try
            DataGridView1.DataSource = e.Result
            For Each c As DataGridViewColumn In DataGridView1.Columns
                With c
                    If {"iteid", "stlid", "ftrid", "dailyplanid"}.Contains(.Name) Then
                        .Visible = False
                    End If
                End With
            Next
            DataGridView1.ReadOnly = True
            DataGridView1.Visible = True
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        Finally
            For Each c As Control In TabPage3.Controls
                If Not IsNothing(TryCast(c, PictureBox)) Then
                    c.Dispose()
                End If
            Next
        End Try
    End Sub

    Dim rightclickedSTLID As Integer = 0
    Private Sub pallettemplatedatagrid_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles pallettemplatedatagrid.CellMouseClick
        'If e.RowIndex >= 0 AndAlso e.Button = MouseButtons.Right AndAlso ContextMenuStrip2.Enabled Then
        '    ContextMenuStrip2.Show(Cursor.Position.X, Cursor.Position.Y)
        '    rightclickedSTLID = pallettemplatedatagrid.Rows(e.RowIndex).Cells("stlid").Value
        'End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        If rightclickedSTLID > 0 AndAlso ContextMenuStrip2.Enabled Then
            Using s As New SqlCommand("select isnull(sum(s.primaryqty),0) slq, isnull(sum(pl.quantity),0) plq, isnull(sum(dp.quantity),0) dpq from tbl_palletlines pl inner join storetradelines s on s.id=pl.stlid left join pkrtbl_dailyplan dp on dp.id=pl.dailyplanid where pl.stlid=" + rightclickedSTLID.ToString, conn)
                Using dt As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                    End Using
                    conn.Close()
                    If dt(0).Item("plq") + dt(0).Item("dpq") > dt(0).Item("slq") Then
                        Throw New Exception("Άκυρη ποσότητα! Θα προγραμματίσετε μεγαλύτερη ποσότητα από της παραγγελίας!")
                    Else


                    End If
                End Using
            End Using
        End If
    End Sub

    Private Sub loclbl_Click(sender As Object, e As EventArgs) Handles loclbl.Click

    End Sub

    Private Sub loclbl_TextChanged(sender As Object, e As EventArgs) Handles loclbl.TextChanged
        If Not (loclbl.Text = "" Or loclbl.Text = "OOOO.OO.OO" Or loclbl.Text = "0") Then
            loclbl.Visible = True
        End If
    End Sub
End Class
