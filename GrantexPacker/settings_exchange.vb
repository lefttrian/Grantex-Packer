Imports System.Data.SqlClient
Imports System.Configuration


Public Class settings_exchange : Implements IDisposable
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim disposed As Boolean = False

    Public Sub Dispose() _
              Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then Return

        If disposing Then
            conn.Dispose()
            updconn.Dispose()
        End If

        disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Function get_settings()
        Dim i As Integer = 0
        Form1.settings.Clear()
        Dim cmd As String = "select settings from tbl_packeruserdata where id=" + Form1.activeuserid.ToString
        Using comm As New SqlCommand(cmd, conn)

            conn.Open()

            Dim sets As String() = comm.ExecuteScalar().ToString.Split(";")
            For Each s As String In sets
                If s <> "" Then
                    Dim strarr As String() = s.Split("=")
                    Form1.settings.Add(New settingsitem(strarr(0), strarr(1)))

                End If
                i += 1
            Next
            conn.Close()
        End Using
        Return i
    End Function



    Public Function set_setting(ByVal name As String, ByVal value As String)

        Dim cmd As String = "update tbl_packeruserdata set settings='"
        Dim f = Form1.settings.FirstOrDefault(Function(settingsitem) settingsitem.name = name)
        Dim olditem As settingsitem = Nothing
        If f Is Nothing Then
            Form1.settings.Add(New settingsitem(name, value))
            For Each s As settingsitem In Form1.settings
                cmd = cmd + s.name + "=" + s.value + ";"
            Next
            cmd = cmd + "' where id=" + Form1.activeuserid.ToString
        Else
            olditem = f
            Form1.settings.Remove(f)
            f.Dispose()
            Form1.settings.Add(New settingsitem(name, value))
            For Each s As settingsitem In Form1.settings
                If s.name = name Then
                    cmd = cmd + s.name + "=" + value + ";"
                Else
                    cmd = cmd + s.name + "=" + s.value + ";"
                End If

            Next
            cmd = cmd + "' where id=" + Form1.activeuserid.ToString
        End If
        Using comm As New SqlCommand(cmd, updconn)
            updconn.Open()
            Dim success As Integer = comm.ExecuteNonQuery()
            If success <= 0 Then

                Form1.settings.Remove(Form1.settings.FirstOrDefault(Function(settingsitem) settingsitem.name = name))
                Form1.settings.FirstOrDefault(Function(settingsitem) settingsitem.name = name).Dispose()
                Form1.settings.Add(olditem)
                updconn.Close()
                Return 0
            End If
            updconn.Close()

        End Using
        Return 1




    End Function

    Public Function update_settings()
        Dim cmd As String = "update tbl_packeruserdata set settings='"
        For Each s As settingsitem In Form1.settings
            cmd = cmd + s.name + "=" + s.value + ";"
        Next
        cmd = cmd + "' where id=" + Form1.activeuserid.ToString
        Using comm As New SqlCommand(cmd, updconn)
            updconn.Open()
            Dim success As Integer = comm.ExecuteNonQuery()
            If success <= 0 Then
                updconn.Close()
                Return 0
            End If
            updconn.Close()

        End Using
        Return 1
    End Function


End Class
