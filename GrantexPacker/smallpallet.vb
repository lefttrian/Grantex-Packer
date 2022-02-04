Imports System.ComponentModel
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Public Class smallpallet
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)

    Dim myid As Integer
    Public Property palletid As String
        Get
            Return myid
        End Get
        Set(value As String)
            myid = value
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

    Private custid As String
    Public Property cusid As String
        Get
            Return custid
        End Get
        Set(value As String)
            custid = value
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
            state_changed()
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
            state_changed()
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
            state_changed()
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

    Private Sub state_changed()
        If IsDraft Then
            With Me
                .Label5.Text = "Προγραμματισμένη"
                .Label5.Visible = True
                If Form1.activeuserdpt <> "SA" And Not (Form1.activeuserdpt.ToUpper = belongingdpt.ToUpper Or Form1.activeuserdpt.ToUpper = lockedbydptvalue.ToUpper Or salesman = Form1.activeuseraid.ToString) Then
                    .locked = True
                    .BackColor = Color.LightSteelBlue
                Else
                    .BackColor = Color.LightSteelBlue
                End If
            End With
        ElseIf packinglist <> String.Empty And closedbydptvalue <> String.Empty Then 'παλέτα σε packing list
            With Me
                .Label4.Text = "Στο packing list " + .packinglist
                .Label4.Visible = True
                .closed = True
                .BackColor = Color.Khaki
                .BorderStyle = BorderStyle.FixedSingle
            End With
        ElseIf closedbydptvalue <> String.Empty Then 'παλέτα ολοκληρωμένη
            With Me
                .Label5.Text = "Κλειστή από: " + .closedbyuservalue
                .Label6.Visible = True
                .Label5.Visible = True
                .closed = True
                .BackColor = Color.LightGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        ElseIf lockedbydptvalue <> String.Empty Then
            With Me
                .Label5.Text = "Κλειδωμένη: " + lockedbyuservalue
                .Label5.Visible = True
                If Form1.activeuserdpt <> "SA" And Not (Form1.activeuserdpt.ToUpper = belongingdpt.ToUpper Or Form1.activeuserdpt.ToUpper = lockedbydptvalue.ToUpper Or salesman = Form1.activeuseraid.ToString) Then
                    .locked = True
                    .BackColor = Color.LightSalmon
                Else
                    .BackColor = Color.Gainsboro
                End If
            End With
        ElseIf lockedbydptvalue = String.Empty Then
            With Me
                .Label5.Visible = False
                .locked = False
                .BackColor = Color.White
            End With
        ElseIf closedbydptvalue = String.Empty Then
            With Me
                .Label5.Text = .closedbyuservalue
                .Label5.Visible = False
                .closed = False
                .BackColor = Color.White
            End With
        ElseIf packinglist = String.Empty Then
            With Me
                .Label5.Visible = False
                .closed = False
                .BackColor = Color.White

            End With
        End If
    End Sub

    Public Property department As String
        Get
            Return belongingdpt
        End Get
        Set(value As String)
            belongingdpt = value

        End Set
    End Property





    Private lockedp As Boolean = False
    Private belongingdpt As String = String.Empty


    Public Property locked As Boolean
        Get
            Return lockedp
        End Get
        Set(value As Boolean)
            lockedp = value

        End Set
    End Property

    Private closedp As Boolean = False

    Public Property closed As Boolean
        Get
            Return closedp
        End Get
        Set(value As Boolean)
            closedp = value

        End Set

    End Property
    Dim locid As Integer = 0
    Public Property locationID As Integer
        Get
            Return locid
        End Get
        Set(value As Integer)
            locid = value

        End Set
    End Property

    Dim loccode As String = ""
    Public Property locationCode As String
        Get
            Return loccode
        End Get
        Set(value As String)
            loccode = value


        End Set
    End Property



    Private Sub smallpallet_MouseEnter(sender As Object, e As EventArgs) Handles MyBase.MouseEnter, MyBase.DragOver

    End Sub

    Dim ticker As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs)
        ticker = ticker + 1

    End Sub
    Private Sub smallpallet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        If locid <> 0 Then
            Label2.Visible = True
        Else
            Label2.Visible = False
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



        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private Sub smallpallet_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick


        Form1.pallet_morph(Me.palletid, 1, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)

    End Sub

    Private Sub Label1_MouseClick(sender As Object, e As MouseEventArgs) Handles Label1.MouseClick
        Form1.pallet_morph(Me.palletid, 1, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)
    End Sub

    Private Sub Label2_MouseClick(sender As Object, e As MouseEventArgs) Handles Label2.MouseClick
        Form1.pallet_morph(Me.palletid, 1, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)
    End Sub

    Private Sub Label6_MouseClick(sender As Object, e As MouseEventArgs) Handles Label6.MouseClick
        Form1.pallet_morph(Me.palletid, 1, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)
    End Sub

    Private Sub Label3_MouseClick(sender As Object, e As MouseEventArgs) Handles Label3.MouseClick
        Form1.pallet_morph(Me.palletid, 1, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)
    End Sub

    Private Sub Label5_MouseClick(sender As Object, e As MouseEventArgs) Handles Label5.MouseClick
        Form1.pallet_morph(Me.palletid, 1, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)
    End Sub

    Private Sub Label4_MouseClick(sender As Object, e As MouseEventArgs) Handles Label4.MouseClick
        Form1.pallet_morph(Me.palletid, 1, Form1.FlowLayoutPanel1.Controls.GetChildIndex(Me), c:=Me)
    End Sub

    Dim cur As Cursor
    Private Sub smallpallet_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If e.Button And MouseButtons.Left = MouseButtons.Left Then
            Dim dx = e.X - MouseDownPos.X
            Dim dy = e.Y - MouseDownPos.Y
            If Math.Abs(dx) >= SystemInformation.DoubleClickSize.Width OrElse
               Math.Abs(dy) >= SystemInformation.DoubleClickSize.Height Then

                If Me.closed And Not Label4.Visible Then
                    Dim txt As String = Me.Label1.Text
                    Dim gr As Graphics = Me.CreateGraphics()

                    Dim sz As SizeF = gr.MeasureString(txt, Form1.boldfont)
                    Dim bmp As New Bitmap(CInt(sz.Width + 60) * 2, CInt(sz.Height + 60))
                    Try

                        gr = Graphics.FromImage(bmp)
                        gr.Clear(Color.White)
                        gr.DrawIcon(My.Resources.importpallet, CInt(sz.Width + 50), 25)
                        gr.DrawString(txt, Form1.boldfont, Brushes.Black, CInt(sz.Width + 60) + 40, 45)
                        bmp.MakeTransparent(Color.White)
                        cur = New Cursor(bmp.GetHicon())
                        Me.DoDragDrop(Me.palletid, DragDropEffects.Copy)
                    Finally
                        gr.Dispose()
                        bmp.Dispose()

                    End Try

                End If

            End If

        End If
    End Sub
    Private MouseDownPos As Point
    Private Sub smallpallet_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        MouseDownPos = e.Location
        If Me.closed = False And Me.locked = False And Not Label4.Visible Then
            Form1.Label19.Text = Me.Label1.Text
        End If
    End Sub

    Private Sub smallpallet_GiveFeedback(sender As Object, e As GiveFeedbackEventArgs) Handles MyBase.GiveFeedback
        e.UseDefaultCursors = False
        Cursor.Current = cur
    End Sub

    Private Sub smallpallet_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop, Label1.DragDrop, Label2.DragDrop, Label6.DragDrop, Label3.DragDrop, Label5.DragDrop, Label4.DragDrop
        Try
            ddd(e)
        Catch
        End Try
    End Sub

    Private Sub ddd(e As DragEventArgs)
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
                message = "Τι ποσότητα του προϊόντος " + row.Cells("ΠΕΡΙΓΡΑΦΗ").Value + " θα τοποθετήσετε στη " + Me.Label1.Text + "; Διαθέσιμη ποσότητα: " + defaultValue.ToString
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


    Private Sub smallpallet_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter, Label1.DragEnter, Label2.DragEnter, Label6.DragEnter, Label3.DragEnter, Label5.DragEnter, Label4.DragEnter
        e.Effect = DragDropEffects.All
    End Sub
End Class
