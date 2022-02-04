Imports System.Configuration
Imports System.Data.SqlClient

Public Class reldocs
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim docs As New List(Of Integer)
    Public Sub New(ByVal d As List(Of Integer))


        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        docs = d
    End Sub

    Private Sub reldocs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Using s As New SqlCommand("select id,descr,filename from documents where id in (" + String.Join(",", docs.ToArray) + ")", conn)
                Using dt = New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                        conn.Close()


                    End Using
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim d As New document(dt.Rows(i).Item("id"), dt.Rows(i).Item("descr"), dt.Rows(i).Item("filename"))
                        FlowLayoutPanel1.Controls.Add(d)

                    Next
                End Using
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
End Class