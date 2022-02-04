Public Class palletlistitem
    Private n As String
    Private i As String
    Private a As String

    Public Property name() As String
        Get
            Return n
        End Get
        Set(ByVal value As String)
            n = value
        End Set
    End Property

    Public Property value() As String
        Get
            Return i
        End Get
        Set(ByVal value As String)
            i = value
        End Set
    End Property
    Public Property abbr() As String
        Get
            Return a
        End Get
        Set(ByVal value As String)
            a = value
        End Set
    End Property


    Public Sub New(ByRef pStr1 As String, pstr2 As String, Optional pstr3 As String = "")
        n = pStr1
        i = pstr2
        a = pstr3
    End Sub
End Class
