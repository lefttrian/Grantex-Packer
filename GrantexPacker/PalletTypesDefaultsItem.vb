Imports System.Configuration
Imports System.Data.SqlClient

Public Class PalletTypesDefaultsItem
    Implements IDisposable
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)

    Dim counter As String
    Dim variable As String
    Dim variablevalue As String
    Dim pallettype As String
    Dim quantity As String
    Dim id As Integer

    Public Sub New(ByVal c As String, v As String, vv As String, p As String, q As String, i As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        counter = c
        variable = v
        variablevalue = vv
        pallettype = p
        quantity = q
        id = i
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub PalletTypesDefaultsItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        titlelbl.Text = "Κανόνας " + counter
        variablelbl.Text = variable
        variablevaluelbl.Text = variablevalue
        pallettypelbl.Text = pallettype
        quantitylbl.Text = quantity
    End Sub

    Private Sub NoPaddingButton1_Click(sender As Object, e As EventArgs) Handles NoPaddingButton1.Click
        Try
            NoPaddingButton1.Visible = False
            Dim l As New Label
            l.Text = "Είστε σίγουροι;"
            l.Name = "deletelabel"
            Dim a As New Button
            a.Text = "Ναι"
            a.Name = "acceptbutton"
            Dim b As New Button
            b.Text = "Όχι"
            b.Name = "rejectbutton"
            AddHandler a.Click, AddressOf acceptdelete
            AddHandler b.Click, AddressOf rejectdelete
            Panel2.Controls.Add(l)
            Panel2.Controls.Add(a)
            Panel2.Controls.Add(b)
            l.Top = NoPaddingButton1.Top
            l.Left = NoPaddingButton1.Left - (l.Width + a.Width + b.Width + 4)
            a.Top = l.Top
            a.Left = l.Left + l.Width + 2
            b.Top = a.Top
            b.Left = a.Left + a.Width + 2
            Timer1.Start()
            'l.Location = New Point(NoPaddingButton1.Location.X, NoPaddingButton1.Location.Y - 50)
            'a.Location = New Point(l.Location.X, l.Location.Y + l.Width + 2)
            'b.Location = New Point(a.Location.X, a.Location.Y + a.Width + 2)
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub acceptdelete()
        Using com As New SqlCommand("Delete from PKRTBL_PALLETTYPESDEFAULTS WHERE ID=" + id.ToString, updconn)
            updconn.Open()
            Dim r As Integer = com.ExecuteNonQuery
            updconn.Close()
            If r > 0 Then
                Me.Dispose()
            End If
        End Using

    End Sub

    Private Sub rejectdelete()
        clearbuttons()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        clearbuttons()
        Timer1.Stop()
    End Sub

    Private Sub clearbuttons()
        For i As Integer = Panel2.Controls.Count - 1 To 0 Step -1
            Dim c As Control = Panel2.Controls(i)
            If c.Name = "acceptbutton" Or c.Name = "rejectbutton" Or c.Name = "deletelabel" Then
                c.Dispose()
            End If
        Next
        NoPaddingButton1.Visible = True

    End Sub
End Class
