Imports System.Configuration
Imports System.Data.SqlClient

Public Class PackerUserTransaction
    Implements IDisposable

    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Public Function WriteEntry(ByVal user_id As Integer, transactiontype_id As Integer, pertype_id1 As Integer, pertype_id2 As Integer, value As String) 'πρόταση με 2 αντικείμενα πχ Χρήστης Α έβαλε ποσότητα Β είδους Γ σε παλέτα Δ
        Dim ret As Integer = -1
        If user_id = 0 Or transactiontype_id = 0 Or pertype_id1 = 0 Or pertype_id2 = 0 Or value = "" Then
            Throw New Exception("Δεν είναι ορθά τα ορίσματα")
        End If
        Using s As New SqlCommand()
            s.Connection = updconn
            s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,PERTYPEID1,PERTYPEID2,PERTYPEVALUE,TIME) VALUES (@USERID,@TRANTYPEID,@PERTYPEID1,@PERTYPEID2,@PERTYPEVALUE,GETDATE())"
            s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
            s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
            s.Parameters.Add("@PERTYPEID1", SqlDbType.Int).Value = pertype_id1
            s.Parameters.Add("@PERTYPEID2", SqlDbType.Int).Value = pertype_id2
            s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = value.ToString
            updconn.Open()
            ret = s.ExecuteNonQuery()
            updconn.Close()
        End Using
        Return ret
    End Function


    'Public Function WriteSTLEntry(ByVal user_id As Integer, transactiontype_id As Integer, pertype_id1 As Integer, pertype_id3 As Integer, Optional value As String = "", Optional q As Double = 0) 'Επειδή ο πίνακας storetradelines καταλαμβάνει var1 & var2 στα transactions, αν χρειάζεται var3 καλείται αυτή η function
    '    Dim ret As Integer = -1
    '    If user_id = 0 Or transactiontype_id = 0 Or pertype_id1 = 0 Or pertype_id3 = 0 Or value = "" Then
    '        Throw New Exception("Δεν είναι ορθά τα ορίσματα")
    '    End If
    '    Using s As New SqlCommand()
    '        s.Connection = updconn
    '        s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,PERTYPEID1,PERTYPEID3,PERTYPEVALUE,QUANTITY,TIME) VALUES (@USERID,@TRANTYPEID,@PERTYPEID1,@PERTYPEID3,@PERTYPEVALUE,@QUANTITY,GETDATE())"
    '        s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
    '        s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
    '        s.Parameters.Add("@PERTYPEID1", SqlDbType.Int).Value = pertype_id1
    '        s.Parameters.Add("@PERTYPEID3", SqlDbType.Int).Value = pertype_id3
    '        If value = "" Then s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = value Else s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = DBNull.Value
    '        If q <> 0 Then s.Parameters.Add("@QUANTITY", SqlDbType.VarChar).Value = q Else s.Parameters.Add("@QUANTITY", SqlDbType.VarChar).Value = DBNull.Value
    '        updconn.Open()
    '        ret = s.ExecuteNonQuery()
    '        updconn.Close()
    '    End Using
    '    Return ret
    'End Function

    Public Function WriteSTLEntry(ByVal user_id As Integer, transactiontype_id As Integer, pertype_id1 As Integer, pertype_id3 As Integer, Optional q As Double = 0) 'Επειδή ο πίνακας storetradelines καταλαμβάνει var1 & var2 στα transactions. Το id3 μετατρεπεται σε value
        Dim ret As Integer = -1
        If user_id = 0 Or transactiontype_id = 0 Or pertype_id1 = 0 Or pertype_id3 = 0 Then
            Throw New Exception("Δεν είναι ορθά τα ορίσματα")
        End If
        Using s As New SqlCommand()
            s.Connection = updconn
            s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,PERTYPEID1,PERTYPEVALUE,QUANTITY,TIME) VALUES (@USERID,@TRANTYPEID,@PERTYPEID1,@PERTYPEVALUE,@QUANTITY,GETDATE())"
            s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
            s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
            s.Parameters.Add("@PERTYPEID1", SqlDbType.Int).Value = pertype_id1
            s.Parameters.Add("@PERTYPEID3", SqlDbType.Int).Value = pertype_id3
            Dim lst As New List(Of Integer)
            lst.Add(pertype_id3)
            s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = ReturnItemCodes(lst)(0)
            If q <> 0 Then s.Parameters.Add("@QUANTITY", SqlDbType.Float).Value = q Else s.Parameters.Add("@QUANTITY", SqlDbType.Float).Value = DBNull.Value
            updconn.Open()
            ret = s.ExecuteNonQuery()
            updconn.Close()
        End Using
        Return ret
    End Function

    ''' <summary>
    '''  πρόταση με αντικείμενο πχ χρήστης Α έκανε κάτι στο αντικείμενο Β
    ''' </summary>
    Public Function WriteEntry(ByVal user_id As Integer, transactiontype_id As Integer, pertype_id1 As Integer) 'πρόταση με αντικείμενο πχ χρήστης Α έκανε κάτι στο αντικείμενο Β
        Dim ret As Integer = -1
        If user_id = 0 Or transactiontype_id = 0 Or pertype_id1 = 0 Then
            Throw New Exception("Δεν είναι ορθά τα ορίσματα")
        End If
        Using s As New SqlCommand()
            s.Connection = updconn
            s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,PERTYPEID1,TIME) VALUES (@USERID,@TRANTYPEID,@PERTYPEID1,GETDATE())"
            s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
            s.Parameters.Add("@PERTYPEID1", SqlDbType.Int).Value = pertype_id1
            s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
            updconn.Open()
            ret = s.ExecuteNonQuery()
            updconn.Close()
        End Using
        Return ret
    End Function
    ''' <summary>
    '''  'πρόταση χωρίς αντικείμενο πχ χρήστης Α έκανε κάτι
    ''' </summary>
    Public Function WriteEntry(ByVal user_id As Integer, transactiontype_id As Integer) 'πρόταση χωρίς αντικείμενο πχ χρήστης Α έκανε κάτι
        Dim ret As Integer = -1
        If user_id = 0 Or transactiontype_id = 0 Then
            Throw New Exception("Δεν είναι ορθά τα ορίσματα")
        End If
        Using s As New SqlCommand()
            s.Connection = updconn
            s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,TIME) VALUES (@USERID,@TRANTYPEID,GETDATE())"
            s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
            s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
            updconn.Open()
            ret = s.ExecuteNonQuery()
            updconn.Close()
        End Using
        Return ret
    End Function

    ''' <summary>
    ''' πρόταση με 1 αντικείμενο και ποσότητα πχ Χρήστης Α δημιούργησε ποσότητα Β είδους Γ με τιμή Δ
    ''' </summary>
    Public Function WriteEntry(ByVal user_id As Integer, transactiontype_id As Integer, pertype_id1 As Integer, Optional value As String = "", Optional q As Double = 0) 'πρόταση με 1 αντικείμενο και ποσότητα πχ Χρήστης Α δημιούργησε ποσότητα Β είδους Γ
        Dim ret As Integer = -1
        If user_id = 0 Or transactiontype_id = 0 Or pertype_id1 = 0 Or value = "" Then
            Throw New Exception("Δεν είναι ορθά τα ορίσματα")
        End If
        Using s As New SqlCommand()
            s.Connection = updconn
            s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,PERTYPEID1,PERTYPEVALUE,QUANTITY,TIME) VALUES (@USERID,@TRANTYPEID,@PERTYPEID1,@PERTYPEVALUE,@QUANTITY,GETDATE())"
            s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
            s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
            s.Parameters.Add("@PERTYPEID1", SqlDbType.Int).Value = pertype_id1
            If value = "" Then s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = value Else s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = DBNull.Value
            If q <> 0 Then s.Parameters.Add("@QUANTITY", SqlDbType.VarChar).Value = q Else s.Parameters.Add("@QUANTITY", SqlDbType.VarChar).Value = DBNull.Value
            updconn.Open()
            ret = s.ExecuteNonQuery()
            updconn.Close()
        End Using
        Return ret
    End Function


    ''' <summary>
    ''' πρόταση χρήστης Α δημιούργησε λίστα Χ πραγμάτων
    ''' </summary>
    Public Function WriteEntry(ByVal user_id As Integer, transactiontype_id As Integer, q As Double, value As List(Of String)) 'πρόταση χρήστης Α δημιούργησε Χ πράγματα:(λίστα)
        Dim ret As Integer = -1
        If user_id = 0 Or transactiontype_id = 0 Or q = 0 Or value.Count = 0 Then
            Throw New Exception("Δεν είναι ορθά τα ορίσματα")
        End If
        Using s As New SqlCommand()
            s.Connection = updconn
            s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,QUANTITY,PERTYPEVALUE,TIME) VALUES (@USERID,@TRANTYPEID,@QUANTITY,@PERTYPEVALUE,GETDATE())"
            s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
            s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
            s.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = q
            s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = String.Join(",", value.ToArray)
            updconn.Open()
            ret = s.ExecuteNonQuery()
            updconn.Close()
        End Using
        Return ret
    End Function

    ''' <summary>
    ''' πρόταση χρήστης Α συνδέθηκε στο μηχάνημα Χ
    ''' </summary>
    Public Function WriteEntry(ByVal user_id As Integer, transactiontype_id As Integer, value As String) 'πρόταση χρήστης Α δημιούργησε Χ πράγματα:(λίστα)
        Dim ret As Integer = -1
        If user_id = 0 Or transactiontype_id = 0 Or value = "" Then
            Throw New Exception("Δεν είναι ορθά τα ορίσματα")
        End If
        Using s As New SqlCommand()
            s.Connection = updconn
            s.CommandText = "INSERT INTO PKRTBL_USERTRANSACTIONS (USERID,TRANTYPEID,PERTYPEVALUE,TIME) VALUES (@USERID,@TRANTYPEID,@PERTYPEVALUE,GETDATE())"
            s.Parameters.Add("@USERID", SqlDbType.Int).Value = user_id
            s.Parameters.Add("@TRANTYPEID", SqlDbType.Int).Value = transactiontype_id
            s.Parameters.Add("@PERTYPEVALUE", SqlDbType.VarChar).Value = value
            updconn.Open()
            ret = s.ExecuteNonQuery()
            updconn.Close()
        End Using
        Return ret
    End Function


    ''' <summary>
    ''' δέχεται λίστα ids και επιστρέφει λίστα κωδικών από έναν πίνακα
    ''' </summary>

    Public Function ReturnItemCodes(ByVal lst As List(Of Integer), Optional table As String = "tbl_palletheaders")
        Dim dt As New DataTable()
        Dim cmd As String
        cmd = "select code from " + table + " where id in (" + String.Join(",", lst.ToArray) + ")"
        Using s As New SqlCommand(cmd, conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                dt.Load(reader)
            End Using
            conn.Close()
        End Using
        Return dt.AsEnumerable.Select(Function(ro) ro(0).ToString).ToList()
    End Function


    ''' <summary>
    ''' δέχεται id και επιστρέφει ΚΩΔΙΚΌ από έναν πίνακα
    ''' </summary>

    Public Function ReturnItemCodes(ByVal lst As Integer, Optional table As String = "tbl_palletheaders")
        Dim dt As String
        Dim cmd As String
        cmd = "select code from " + table + " where id =" + lst.ToString
        Using s As New SqlCommand(cmd, conn)
            conn.Open()
            dt = s.ExecuteScalar
            conn.Close()
        End Using
        Return dt
    End Function

    ''' <summary>
    ''' δέχεται λίστα ids και επιστρέφει λίστα ημερομηνιών από έναν πίνακα
    ''' </summary>

    Public Function ReturnDates(ByVal lst As List(Of Integer), Optional table As String = "pkrtbl_dailyplan")
        Dim dt As New DataTable()
        Dim cmd As String
        cmd = "select date from " + table + " where id in (" + String.Join(",", lst.ToArray) + ")"
        Using s As New SqlCommand(cmd, conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                dt.Load(reader)
            End Using
            conn.Close()
        End Using
        Return dt.AsEnumerable.Select(Function(ro) ro(0).ToString).ToList()
    End Function

    ''' <summary>
    ''' δέχεται ftrid και επιστρέφει status
    ''' </summary>

    Public Function ReturnOrderStatus(ByVal ftrid As Integer)
        Dim res As Integer = 0
        Dim cmd As String
        cmd = "select status from tbl_packerordercheck where ftrid=" + ftrid.ToString
        Using s As New SqlCommand(cmd, conn)
            conn.Open()
            res = s.ExecuteScalar()
            conn.Close()
        End Using
        Return res
    End Function


    ''' <summary>
    ''' δέχεται code τμήματος και επιστρέφει id
    ''' </summary>

    Public Function ReturnUserDptID(ByVal code As String)
        Dim res As Integer = 0
        Dim cmd As String
        cmd = "select id from PKRTBL_USERDEPARTMENTS where code=" + code
        Using s As New SqlCommand(cmd, conn)
            conn.Open()
            res = s.ExecuteScalar()
            conn.Close()
        End Using
        Return res
    End Function


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            updconn.Dispose()
            conn.Dispose()
            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
