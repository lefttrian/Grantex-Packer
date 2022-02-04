Imports System.Data.SqlClient
Imports System.Configuration
Public Class settingsitem : Implements IDisposable
    Dim settingname As String
    Dim settingvalue As String
    Public Property name As String
        Get
            Return settingname
        End Get
        Set(value As String)
            settingname = value
        End Set
    End Property
    Public Property value As String
        Get
            Return settingvalue
        End Get
        Set(value As String)
            settingvalue = value
        End Set

    End Property

    Public Sub New(ByVal n As String,
                       ByVal v As String)
        settingname = n
        settingvalue = v

    End Sub

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            'conn.Dispose()
            'updconn.Dispose()

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub


    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub
#End Region


End Class