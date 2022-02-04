Imports System.IO

Public Class document
    Public Property name() As String
        Get
            Return myname
        End Get
        Set(value As String)
            myname = value
            Label1.Text = myname
        End Set
    End Property

    Public Property path() As String
        Get
            Return mypath
        End Get
        Set(value As String)
            mypath = value
        End Set
    End Property

    Public Property id() As Integer
        Get
            Return myid
        End Get
        Set(value As Integer)
            myid = value
        End Set
    End Property

    Dim myname As String = ""
    Dim mypath As String = ""
    Dim myid As Integer = 0

    Public Sub New(ByVal i As Integer, ByVal n As String, ByVal p As String)

        ' This call is required by the designer.
        InitializeComponent()
        Me.name = n
        Me.id = i
        Me.path = p
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Label1_DoubleClick(sender As Object, e As EventArgs) Handles Label1.DoubleClick
        Try
            System.Diagnostics.Process.Start(mypath)
        Catch ex As Exception

            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub
End Class
