Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Globalization

Public Class PalletManager
    Implements IDisposable
    Dim activeuserdpt As String
    Dim activeuserdptid As Integer
    Dim activeuser As String
    Dim npc As String
    Dim activeuserid As Integer
    Dim cusid As Integer = 0
    Dim ftrid As String = 0
    Dim customerfn As String
    Dim desiredloctype As String
    Dim newcodes As Object()
    Dim stockpallet As Boolean = False
    Dim names As String() = {"PalletCode", "PalletDptCode", "PalletLocationID", "PalletLocationCode"}
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim mantisconnString As String = ConfigurationManager.ConnectionStrings("mantisconn").ConnectionString.ToString()
    Dim mantisconn As New SqlConnection(mantisconnString)
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim pallettypeid As Integer = 1
    Dim relsalesman As Integer = -1

    Public Sub New(user_dpt As String, user_name As String, activeuser_id As Integer, activeuserdpt_id As Integer, Optional stock As Boolean = False, Optional cus_id As Integer = 0, Optional ftr_id As Integer = 0, Optional stl_id As Integer = 0)
        activeuserdpt = user_dpt
        activeuser = user_name
        activeuserid = activeuser_id
        activeuserdptid = activeuserdpt_id
        If cus_id = 0 And ftr_id = 0 And stl_id = 0 And Not stock Then
            Throw New Exception("Πρέπει να παρασχεθεί τουλάχιστον ένα εκ των FTRID, CUSID, STLID")
        ElseIf stock Then
            stockpallet = stock
        ElseIf stl_id <> 0 Then
            ftrid = ftr_id
            Dim cmd0 As String = "SELECT c.id CUSID,F.ID FTRID,ISNULL(PALLETTYPEID,1) PALLETTYPEID FROM  STORETRADELINES S INNER JOIN FINTRADE F ON S.FTRID=F.ID LEFT JOIN CUSTOMER C ON C.ID=F.CUSID LEFT JOIN PKRTBL_CUSTOMER P ON C.ID=P.CUSID WHERE S.ID=" + stl_id.ToString
            Using sqlcmd As New SqlCommand(cmd0, conn)
                Using dt As New DataTable
                    conn.Open()
                    Using reader As SqlDataReader = sqlcmd.ExecuteReader
                        dt.Load(reader)
                        conn.Close()
                    End Using
                    pallettypeid = dt.Rows(0).Item("PALLETTYPEID")
                    cusid = dt.Rows(0).Item("CUSID")
                    ftrid = dt.Rows(0).Item("FTRID")
                End Using
            End Using
        ElseIf ftr_id <> 0 Then
            ftrid = ftr_id
            Dim cmd0 As String = "SELECT c.id,ISNULL(PALLETTYPEID,1) PALLETTYPEID FROM  CUSTOMER C LEFT JOIN PKRTBL_CUSTOMER P ON C.ID=P.CUSID WHERE C.ID=(SELECT CUSID FROM FINTRADE WHERE ID=" + ftrid.ToString + ")"
            Using sqlcmd As New SqlCommand(cmd0, conn)
                Using dt As New DataTable
                    conn.Open()
                    Using reader As SqlDataReader = sqlcmd.ExecuteReader
                        dt.Load(reader)
                        conn.Close()
                    End Using
                    pallettypeid = dt.Rows(0).Item("PALLETTYPEID")
                    cusid = dt.Rows(0).Item("id")
                End Using
            End Using
        ElseIf cus_id <> 0 Then
            cusid = cus_id
            Dim cmd0 As String = "SELECT ISNULL(PALLETTYPEID,1) FROM  CUSTOMER C LEFT JOIN PKRTBL_CUSTOMER P ON C.ID=P.CUSID WHERE C.ID=" + cusid.ToString
            Using sqlcmd As New SqlCommand(cmd0, conn)
                conn.Open()
                pallettypeid = sqlcmd.ExecuteScalar
                conn.Close()
            End Using
        End If
        Dim cmd1 As String = "select colidsalesman from customer where id=" + cusid.ToString
        Using sqlcmd As New SqlCommand(cmd1, conn)
            conn.Open()
            relsalesman = sqlcmd.ExecuteScalar
            conn.Close()
        End Using

    End Sub
    ''' <summary>
    ''' Checks if MANTIS WMS has the necessary locations available. If not, it creates them, up to a maximum of 99 locations
    ''' </summary>

    Private Sub MantisLocations(ByVal r As Dictionary(Of String, String), numberofpallets As Integer)
        Dim checkcmd As String = "Select  count(loc_id) FROM lvision.dbo.LV_Location WHERE dbo.charindex2('.',loc_Code,2)<>0 and SUBSTRING(loc_Code,1,dbo.charindex2('.',loc_Code,2)) =  '" + r("LocCode") + "' 
And loc_ID Not in (select LocationID from SC_QTY_MANTISAX where dbo.charindex2('.',LocationCode,2)<>0 and SUBSTRING(LocationCode,1,dbo.charindex2('.',LocationCode,2)) = '" + r("LocCode") + "') 
And loc_ID Not in (select locid from TBL_PALLETHEADERS where CODE Like '" + r("PalletCode") + "%' and locid is not null and (plid is null or plid in (select id from tbl_packinglists where status<>1)))"
        Dim availMantisLocs As Integer = 0
        Using s As New SqlCommand(checkcmd, conn)
            conn.Open()
            availMantisLocs = s.ExecuteScalar
            conn.Close()
        End Using
        If availMantisLocs < numberofpallets Then
            Using s As New SqlCommand("PKRPRC_OPENLOCATIONS", mantisconn)
                s.CommandType = CommandType.StoredProcedure
                s.Parameters.Add("@FATHERNAME", SqlDbType.VarChar).Value = r("LocCode").Split(".")(0)
                s.Parameters.Add("@MANTISLEVEL", SqlDbType.Int).Value = CInt(r("LocCode").Split(".")(1).Substring(0, 1))
                s.Parameters.Add("@DPT", SqlDbType.VarChar).Value = r("LocCode").Split(".")(1).Substring(1, 1)
                s.Parameters.Add("@NUMBEROFPALLETS", SqlDbType.Int).Value = numberofpallets
                mantisconn.Open()
                s.ExecuteNonQuery()
                mantisconn.Close()
            End Using
        End If
    End Sub
    ''' <summary>
    ''' Creates one or multiple pallets, returns their IDs
    ''' </summary>
    Public Function Create(numberofpallets As Integer, Optional activeuserdpt_id As Integer = 0, Optional status As Integer = 0, Optional desired_loc As String = "", Optional ftr_id As Integer = 0, Optional pl_id As Integer = 0, Optional isnotfull As Integer = 0)
        If activeuserdpt_id = 0 Then
            activeuserdpt_id = activeuserdptid
        End If
        If pl_id <> 0 Then
            PackingListCheck(New List(Of Integer) From {pl_id}, "packinglist")
        End If
        Dim newpalletids As New DataTable()
        Dim f As Integer
        If ftr_id = 0 Then
            f = ftrid
        Else
            f = ftr_id
        End If
        Dim r As Dictionary(Of String, String)
        r = Calculate_Code_Substrings(desired_loc, f)

        Dim cmd As String = ""
        If stockpallet Then
            cmd = "insert into tbl_palletheaders (CODE, OPENDATE, CREATEUSER, lockedbyid,  PALLETTYPEID, status,ISSTOCK,ISNOTFULL,CREATEDPTID) OUTPUT INSERTED.ID SELECT Q1.n,getdate()," + activeuserid.ToString + "," + activeuserid.ToString + "," + pallettypeid.ToString + "," + status.ToString + ",1,@ISNOTFULL,@DPTID FROM
(select row_number() over (order by n) rn, n from (select distinct top " + numberofpallets.ToString + " n='" + r("PalletCode") + "-'+right('000'+cast(number as varchar(4)),3),number  FROM master..[spt_values] WHERE number BETWEEN 1 AND 1000
and '" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) not in (select code from tbl_palletheaders where code like '" + r("PalletCode") + "-%') order by number asc ) Q ) Q1"
        Else
            If status >= 0 Then
                MantisLocations(r, numberofpallets)
                cmd = "insert into tbl_palletheaders (CODE, OPENDATE, CREATEUSER, lockedbyid, PALLETTYPEID, loccode, locid, orders, atlantissalesmanid, CUSID,status,ISSTOCK,PLID,ISNOTFULL,CREATEDPTID)  OUTPUT INSERTED.ID  SELECT Q1.n,getdate()," + activeuserid.ToString + "," + activeuserid.ToString + "," + pallettypeid.ToString + ",Q2.loc_code,Q2.loc_id,(select dbo.get_tradecode(" + f.ToString + "))," + relsalesman.ToString + "," + cusid.ToString + "," + status.ToString + ",0,@plid,@ISNOTFULL,@DPTID FROM
(select row_number() over (order by n) rn, n from (select distinct top  " + numberofpallets.ToString + "   n='" + r("PalletCode") + "-'+right('000'+cast(number as varchar(4)),3),number  FROM master..[spt_values] WHERE number BETWEEN 1 AND 1000
and '" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) not in (select code from tbl_palletheaders where code like '" + r("PalletCode") + "-%') order by number asc ) Q ) Q1
INNER JOIN (Select  ROW_NUMBER() OVER (ORDER BY LOC_ID) RN,loc_ID,loc_Code FROM lvision.dbo.LV_Location WHERE dbo.charindex2('.',loc_Code,2)<>0 and SUBSTRING(loc_Code,1,dbo.charindex2('.',loc_Code,2)) =  '" + r("LocCode") + "' 
And loc_ID Not in (select LocationID from SC_QTY_MANTISAX where dbo.charindex2('.',LocationCode,2)<>0 and SUBSTRING(LocationCode,1,dbo.charindex2('.',LocationCode,2)) = '" + r("LocCode") + "') 
And loc_ID Not in (select locid from TBL_PALLETHEADERS where CODE Like '" + r("PalletCode") + "%' and locid is not null and (plid is null or plid in (select id from tbl_packinglists where status<>1)))) Q2 ON Q1.RN=Q2.RN"
            ElseIf status = -1 Then
                cmd = "insert into tbl_palletheaders (CODE, OPENDATE, CREATEUSER,  PALLETTYPEID,  orders, atlantissalesmanid, CUSID,status,ISSTOCK,PLID,ISNOTFULL,CREATEDPTID)  OUTPUT INSERTED.ID  SELECT Q1.n,getdate()," + activeuserid.ToString + "," + pallettypeid.ToString + ",(select dbo.get_tradecode(" + f.ToString + "))," + relsalesman.ToString + "," + cusid.ToString + "," + status.ToString + ",0,@plid,@ISNOTFULL,@DPTID  FROM
(select row_number() over (order by n) rn, n from (select distinct top  " + numberofpallets.ToString + "   n='" + r("PalletCode") + "-'+right('000'+cast(number as varchar(4)),3),number  FROM master..[spt_values] WHERE number BETWEEN 1 AND 1000
and '" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) not in (select code from tbl_palletheaders where code like '" + r("PalletCode") + "-%') order by number asc ) Q ) Q1"
            Else
                cmd = "insert into tbl_palletheaders (CODE, OPENDATE, CREATEUSER, lockedbyid, PALLETTYPEID,  orders, atlantissalesmanid, CUSID,status,ISSTOCK,PLID,ISNOTFULL,CREATEDPTID)  OUTPUT INSERTED.ID  SELECT Q1.n,getdate()," + activeuserid.ToString + "," + activeuserid.ToString + "," + pallettypeid.ToString + ",(select dbo.get_tradecode(" + f.ToString + "))," + relsalesman.ToString + "," + cusid.ToString + "," + status.ToString + ",0,@plid,@ISNOTFULL,@DPTID  FROM
(select row_number() over (order by n) rn, n from (select distinct top  " + numberofpallets.ToString + "   n='" + r("PalletCode") + "-'+right('000'+cast(number as varchar(4)),3),number  FROM master..[spt_values] WHERE number BETWEEN 1 AND 1000
and '" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) not in (select code from tbl_palletheaders where code like '" + r("PalletCode") + "-%') order by number asc ) Q ) Q1"
            End If
        End If

        Using sqlcmd As New SqlCommand(cmd, updconn)
            sqlcmd.Parameters.AddWithValue("@ISNOTFULL", isnotfull)
            If activeuserdpt_id = -1 Then sqlcmd.Parameters.AddWithValue("@DPTID", DBNull.Value) Else sqlcmd.Parameters.AddWithValue("@DPTID", activeuserdpt_id)
            If pl_id = 0 Then
                sqlcmd.Parameters.AddWithValue("@plid", DBNull.Value)
            Else
                sqlcmd.Parameters.AddWithValue("@plid", pl_id)
            End If
            updconn.Open()
            Using reader As SqlDataReader = sqlcmd.ExecuteReader
                newpalletids.Load(reader)
                updconn.Close()
            End Using
        End Using
        If newpalletids.Rows.Count > 0 Then
            Dim lst As New List(Of Integer)
            lst = newpalletids.AsEnumerable.Select(Function(ro) CInt(ro(0))).ToList()
            Using ut As New PackerUserTransaction
                ut.WriteEntry(activeuserid, 2, newpalletids.Rows.Count, ut.ReturnItemCodes(lst))
            End Using
            Return newpalletids
        Else
            Return Nothing
        End If
    End Function

    Public Function AssignCreateDPT(ByVal ids As List(Of Integer), dpt_id As Integer)
        Dim res As Integer = -1
        Using s As New SqlCommand("update tbl_palletheaders set createdptid=(@dptid) where id in (" + String.Join(",", ids.ToArray()) + ")", updconn)
            updconn.Open()
            res = s.ExecuteNonQuery
            updconn.Close()
        End Using
        If res > 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(activeuserid, 47, dpt_id, ut.ReturnItemCodes(ids))
            End Using
        End If
        Return res
    End Function

    Public Function AssignCreateDPT(ByVal id As Integer, dpt_id As Integer)
        Dim res As Integer = -1
        Using s As New SqlCommand("update tbl_palletheaders set createdptid=@dptid where id=" + id.ToString, updconn)
            s.Parameters.AddWithValue("@dptid", dpt_id)
            updconn.Open()
            res = s.ExecuteNonQuery
            updconn.Close()
        End Using
        If res > 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(activeuserid, 47, dpt_id, ut.ReturnItemCodes(id))
            End Using
        End If
        Return res
    End Function


    Private Sub PackingListCheck(ByVal ids As List(Of Integer), type As String)

        Dim txt As String = ""
        If type = "palletids" Then
            txt = "Select id from tbl_packinglists where id In (Select plid from tbl_palletheaders where id In (" + String.Join(",", ids.ToArray()) + ")) And (status=1 Or printuserid Is Not null)"
        ElseIf type = "palletlineids" Then
            txt = "Select id from tbl_packinglists where id In (Select plid from tbl_palletheaders where id In (Select palletid from tbl_palletlines where id In (" + String.Join(",", ids.ToArray()) + "))) And (status=1 Or printuserid Is Not null)"
        ElseIf type = "packinglist" Then
            txt = "Select id from tbl_packinglists where id In (" + String.Join(",", ids.ToArray()) + ") And (status=1 Or printuserid Is Not null)"
        End If
        Using s As New SqlCommand(txt, conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                End Using
                conn.Close()
                If dt.Rows.Count <> 0 Then
                    Throw New Exception("Δεν επιτρέπονται αλλαγές σε κλειστό ή εκτυπωμένο Packing List.")
                End If
            End Using
        End Using

    End Sub


    ''' <summary>
    ''' Creates one palletheader ---- deprecated
    ''' </summary>
    Public Function Create_old(ftr_id As Integer, newpalletcode As String, newpalletdptcode As String, Optional PalletLocationID As Integer = 0, Optional PalletLocationCode As String = Nothing, Optional status As Integer = 0, Optional dailyplan_id As Integer = 0)
        Dim newpalletid As Integer
        If ftrid = 0 Then
            ftrid = ftr_id
        End If
        Dim cmd As String = ""
        If stockpallet Then
            cmd = "insert into tbl_palletheaders (CODE, OPENDATE, CREATEUSER,  dptcode, PALLETTYPEID, status,ISSTOCK) values ('" _
                    + newpalletcode + "','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'," + activeuserid.ToString + ",'" + newpalletdptcode + "'," + pallettypeid.ToString + "," + status.ToString + ",1);SELECT @@IDENTITY from tbl_palletheaders"
        Else
            cmd = "insert into tbl_palletheaders (CODE, OPENDATE, CREATEUSER, lockedbyid, dptcode, PALLETTYPEID, loccode, locid, orders, atlantissalesmanid, CUSID,status,ISSTOCK) values ('" _
                    + newpalletcode + "','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'," + activeuserid.ToString + "," + activeuserid.ToString + ",'" + newpalletdptcode + "'," + pallettypeid.ToString + ",@loccode,@locid,(select dbo.get_tradecode(" + ftrid.ToString + "))," + relsalesman.ToString + "," + cusid.ToString + "," + status.ToString + ",0);SELECT @@IDENTITY from tbl_palletheaders"
        End If
        Using sqlcmd As New SqlCommand(cmd, updconn)
            If IsNothing(PalletLocationCode) Then
                sqlcmd.Parameters.Add("@loccode", SqlDbType.VarChar, 50).Value = DBNull.Value
            Else
                sqlcmd.Parameters.Add("@loccode", SqlDbType.VarChar, 50).Value = PalletLocationCode
            End If
            If PalletLocationID = 0 Then
                sqlcmd.Parameters.Add("@locid", SqlDbType.Int).Value = DBNull.Value
            Else
                sqlcmd.Parameters.Add("@locid", SqlDbType.Int).Value = PalletLocationID
            End If
            updconn.Open()
            newpalletid = CInt(sqlcmd.ExecuteScalar())
            updconn.Close()
        End Using
        Return newpalletid
    End Function

    ''' <summary>
    ''' Physically opens a pallet which was in draft mode
    ''' </summary>
    Public Function Promote(ByVal pallet_ids As List(Of Integer))
        Try
            PackingListCheck(pallet_ids, "palletids")
            Dim r As Dictionary(Of String, String)
            r = Calculate_Code_Substrings("", ftrid)
            MantisLocations(r, pallet_ids.Count)
            Dim cmd As String = "update tbl_palletheaders set status=0,loccode=qq.loc_Code,locid=qq.loc_ID from
(select * from
(select row_number() over (order by id) rn1, id from tbl_palletheaders where ID IN (" + String.Join(",", pallet_ids.ToArray()) + ")) Q1 inner join 
(Select  ROW_NUMBER() OVER (ORDER BY LOC_ID) RN,loc_ID,loc_Code FROM lvision.dbo.LV_Location WHERE dbo.charindex2('.',loc_Code,2)<>0 and SUBSTRING(loc_Code,1,dbo.charindex2('.',loc_Code,2)) =  '" + r("LocCode") + "' 
And loc_ID Not in (select LocationID from SC_QTY_MANTISAX where dbo.charindex2('.',LocationCode,2)<>0 and SUBSTRING(LocationCode,1,dbo.charindex2('.',LocationCode,2)) = '" + r("LocCode") + "') 
And loc_ID Not in (select locid from TBL_PALLETHEADERS where CODE Like '" + r("PalletCode") + "%' and locid is not null and (plid is null or plid in (select id from tbl_packinglists where status<>1)))) Q2 on Q2.rn=q1.rn1) QQ
 where tbl_palletheaders.id=QQ.ID"
            'Dim cmd2 As String = "UPDATE TBL_PALLETHEADERS SET STATUS=0 WHERE ID IN (" + String.Join(",", pallet_ids.ToArray()) + ")"
            Dim success = -1
            Using c As New SqlCommand(cmd, conn)
                conn.Open()
                success = c.ExecuteNonQuery()
                conn.Close()
            End Using
            If success > 0 Then
                Using ut As New PackerUserTransaction
                    ut.WriteEntry(activeuserid, 3, pallet_ids.Count, ut.ReturnItemCodes(pallet_ids))
                End Using
            End If
            Return success
        Catch ex As Exception
            If ex.Message.Contains("εκτός ορίων") Then
                Throw New System.Exception("Δεν επιτράπηκε!")
            End If
            Throw ex
        End Try
    End Function

    Public Function Close(ByVal pallet_id As Integer, remarks As String, weight As String, netweight As String)
        Dim success = -1
        Dim cmd3 As String = "update tbl_palletheaders set status=1,closedbyid=" + Form1.activeuserid.ToString + ", remarks=@textbox1, weight=REPLACE(@textbox2,',','.'), netweight=REPLACE(@textbox4,',','.'), closedate='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where id=" + pallet_id.ToString + " and isnull(isnotfull,0)<>1"
        If String.IsNullOrWhiteSpace(weight) Or String.IsNullOrWhiteSpace(netweight) Then
            Throw New Exception("Δεν επιτρέπεται κλείσιμο παλέτας χωρίς βάρη.")
        End If
        If String.IsNullOrWhiteSpace(remarks) Then
            remarks = ""
        End If
        Using sqlcmd3 As New SqlCommand(cmd3, updconn)
            sqlcmd3.Parameters.Add("@textbox1", SqlDbType.Text).Value = remarks
            If Len(weight) > 0 Then
                sqlcmd3.Parameters.Add("@textbox2", SqlDbType.Float).Value = Convert.ToDouble(weight)
            Else
                sqlcmd3.Parameters.Add("@textbox2", SqlDbType.Float).Value = DBNull.Value
            End If
            If Len(netweight) > 0 Then
                sqlcmd3.Parameters.Add("@textbox4", SqlDbType.Float).Value = Convert.ToDouble(netweight)
            Else
                sqlcmd3.Parameters.Add("@textbox4", SqlDbType.Float).Value = DBNull.Value
            End If
            updconn.Open()
            success = sqlcmd3.ExecuteNonQuery()
            updconn.Close()
            If success > 0 Then
                Using ut As New PackerUserTransaction
                    Dim l As New List(Of Integer)
                    l.Add(pallet_id)
                    ut.WriteEntry(activeuserid, 4, pallet_id, value:=ut.ReturnItemCodes(l)(0))
                End Using
            End If
        End Using
        Return success
    End Function

    Public Function UnClose(ByVal pallet_id As Integer)
        Dim success = -1
        Dim t As String = ""
        If Form1.activeuser = "SUPERVISOR" Then
            t = " or 1=1 "
        End If
        Dim cmd3 As String = "Update tbl_palletheaders Set status=0,closedbyid=null, closedate = null, lockedbyid =" + activeuserid.ToString + " where id=" + pallet_id.ToString + " and (lockedbyid in (select id from tbl_packeruserdata where department='" + activeuserdpt + "') or lockedbyid=" + activeuserid.ToString + " or atlantissalesmanid=(select isnull(atlantisid,0) from tbl_packeruserdata where id=" + activeuserid.ToString + ") or closedbyid in (select id from tbl_packeruserdata where department='" + activeuserdpt + "') " + t + ")"
        Using sqlcmd3 As New SqlCommand(cmd3, updconn)
            updconn.Open()
            success = sqlcmd3.ExecuteNonQuery()
            updconn.Close()
            If success > 0 Then
                Using ut As New PackerUserTransaction
                    Dim l As New List(Of Integer)
                    l.Add(pallet_id)
                    ut.WriteEntry(activeuserid, 43, pallet_id, value:=ut.ReturnItemCodes(l)(0))
                End Using
            End If
        End Using
        Return success
    End Function
    ''' <summary>
    ''' Deletes pallet
    ''' </summary>
    Public Function Delete(ByVal pallet_ids As List(Of Integer), Optional skipcheck As Boolean = False)
        Dim success As Integer = -1
        PackingListCheck(pallet_ids, "palletids")
        If Not skipcheck Then
            Using s As New SqlCommand("select dailyplanid from tbl_palletlines where dailyplanid is not null and palletid in (" + String.Join(",", pallet_ids.ToArray) + ")", conn)
                Using dt As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                    End Using
                    conn.Close()
                    If dt.Rows.Count <> 0 Then
                        Throw New Exception("Η παλέτα έχει αντιστοιχισθεί με ημερολόγιο παραγωγής. Επικοινωνήστε με τη παραγωγή ή τον διαχειριστή. PIDs:(" + String.Join(",", pallet_ids.ToArray) + ")")
                    End If
                End Using
            End Using
        End If
        Dim palletcodes As New List(Of String)
        Using ut As New PackerUserTransaction
            palletcodes = ut.ReturnItemCodes(pallet_ids)
            Dim dt As New DataTable()
            Using transaction = TransactionUtils.CreateTransactionScope()
                Dim cmd0 As String = "select id from tbl_palletheaders where id in (" + String.Join(",", pallet_ids.ToArray()) + ") and id not in (select palletid from tbl_palletlines where frommantis=1) and closedbyid is null and (lockedbyid is null or lockedbyid=" + activeuserid.ToString + " or lockedbyid in (select id from tbl_packeruserdata where department='" + activeuserdpt + "') or createdptid=" + activeuserdptid.ToString + ")"
                Using s As New SqlCommand(cmd0, conn)
                    conn.Open()
                    Using reader As SqlDataReader = s.ExecuteReader
                        dt.Load(reader)
                        conn.Close()
                        If dt.Rows.Count <> pallet_ids.Count Then
                            Dim PalletIDExempted As New List(Of Integer)
                            For Each r As DataRow In dt.Rows
                                If Not pallet_ids.Contains(r.Item("id")) Then
                                    PalletIDExempted.Add(r.Item("id"))
                                End If
                            Next
                            If PalletIDExempted.Count > 0 Then
                                Throw New Exception("Ακυρώθηκε λόγω προβλήματος δικαιωμάτων σε παλέτα. Ενημερώστε τον διαχειριστή! PIDs:(" + String.Join(",", PalletIDExempted.ToArray) + ")")
                            Else
                                Throw New Exception("Ακυρώθηκε λόγω προβλήματος δικαιωμάτων σε παλέτα. Επιβεβαιώστε ότι: δεν είναι κλειστή, δεν είναι κλειδωμένη από άλλο τμήμα και δεν περιέχει ήδη από το ΜΑΝΤΙΣ.  PIDs:(" + String.Join(",", pallet_ids.ToArray) + ")")
                            End If
                        End If
                    End Using
                End Using
                Dim pallets As New List(Of Integer)
                For Each r As DataRow In dt.Rows
                    pallets.Add(r.Item("id"))
                Next
                Dim cmd As String = "delete from tbl_palletheaders where id in (" + String.Join(",", pallets.ToArray()) + ") "
                Dim cmd2 As String = "delete from tbl_palletlines where palletid in (" + String.Join(",", pallets.ToArray()) + ")"
                Using sqlcmd As New SqlCommand(cmd, updconn)
                    Using sqlcmd2 As New SqlCommand(cmd2, updconn)
                        updconn.Open()
                        success = sqlcmd2.ExecuteNonQuery()
                        success = sqlcmd.ExecuteNonQuery()
                        updconn.Close()
                    End Using
                End Using
                If success > 0 Then
                    ut.WriteEntry(activeuserid, 5, pallet_ids.Count, palletcodes)
                End If
                transaction.Complete()
            End Using
        End Using
        Return success
    End Function

    ''' <summary>
    ''' locks pallets
    ''' </summary>
    Public Function lock(ByVal pallet_ids As List(Of Integer))
        Dim success As Integer = -1
        Dim cmd As String = "update tbl_palletheaders set lockedbyid=" + activeuserid.ToString + " where id in (" + String.Join(",", pallet_ids.ToArray()) + ") and closedbyid is null and lockedbyid is null"
        Using sqlcmd As New SqlCommand(cmd, updconn)

            updconn.Open()
            success = sqlcmd.ExecuteNonQuery()
            updconn.Close()
        End Using
        If success > 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(activeuserid, 6, pallet_ids.Count, ut.ReturnItemCodes(pallet_ids))
            End Using
        End If
        Return success
    End Function

    ''' <summary>
    ''' unlocks pallets
    ''' </summary>
    Public Function unlock(ByVal pallet_ids As List(Of Integer))
        Dim success As Integer = 0
        Dim cmd As String = ""
        If activeuserdpt = "SA" Then
            cmd = "update tbl_palletheaders set lockedbyid=null where id in (" + String.Join(",", pallet_ids.ToArray()) + ") and closedbyid is null "
        Else
            cmd = "update tbl_palletheaders set lockedbyid=null where id in (" + String.Join(",", pallet_ids.ToArray()) + ") and closedbyid is null and lockedbyid in (select id from tbl_packeruserdata where department='" + activeuserdpt + "')"
        End If
        Using sqlcmd As New SqlCommand(cmd, updconn)
            updconn.Open()
            success = sqlcmd.ExecuteNonQuery()
            updconn.Close()
        End Using
        If success > 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(activeuserid, 7, pallet_ids.Count, ut.ReturnItemCodes(pallet_ids))
            End Using
        End If
        Return success
    End Function

    ''' <summary>
    ''' Returns status of palletline related order
    ''' </summary>
    Private Function ReturnOrderStatus(ByVal ftr_id As Integer)
        Dim v As New Dictionary(Of String, String)
        Using c As New SqlCommand("SELECT STATUS,dbo.get_tradecode(" + ftr_id.ToString + ") tradecode FROM TBL_PACKERORDERCHECK WHERE FTRID=" + ftr_id.ToString, conn)

            conn.Open()
            Using dt As New DataTable()
                Using reader As SqlDataReader = c.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                End Using
                v.Add("STATUS", dt.Rows(0).Item("STATUS"))
                v.Add("tradecode", dt.Rows(0).Item("tradecode"))
            End Using

        End Using

        Return v
    End Function


    ''' <summary>
    ''' Performs various checks before a palletline job
    ''' </summary>

    Public Function PerformPrePLJobChecks(ByVal pallet_id As Integer, ftr_id As Integer, ite_id As Integer, Optional SkipOrders As Boolean = False)
        If ite_id = 65947 Or ite_id = 65946 Or ite_id = 65948 Then
            Return 0
        End If
        Dim orderstatus As Integer = CInt(TryCast(ReturnOrderStatus(ftr_id), Dictionary(Of String, String)).Item("STATUS"))
        If orderstatus < 6 Then
            Throw New System.Exception("Δεν έχουν ολοκληρωθεί ακόμα οι έλεγχοι. Δεν μπορείτε να κατανείμετε είδη μέχρι τότε.")
            Return 0
        End If
        If orderstatus > 12 Then
            Throw New System.Exception("Πώς ακριβώς κατάφερες να βρεθείς εδώ? Δεν μπορείς να μεταφέρεις είδος ολοκληρωμένης παραγγελίας.")
            Return 0
        End If
        Using ccc As New SqlCommand("select isnull(pud.DEPARTMENT,'') dpt,isnull(ph.closedbyid,0) closed,isnull(ph.orders,0) orders,ph.cusid from tbl_palletheaders ph left join tbl_packeruserdata pud on pud.id=ph.LOCKEDBYID where ph.id=" + pallet_id.ToString, conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = ccc.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                End Using
                If dt.Rows(0).Item("dpt") <> "" AndAlso dt.Rows(0).Item("dpt") <> activeuserdpt Then
                    Throw New System.Exception("Η παλέτα είναι κλειδωμένη από άλλο τμήμα. Δεν μπορείτε να μεταφέρετε είδος.")
                    Return 0
                End If
                If dt.Rows(0).Item("closed") > 0 Then
                    Throw New System.Exception("Η παλέτα είναι κλειστή. Δεν επιτρέπονται αλλαγές.")
                    Return 0
                End If
                If dt.Rows(0).Item("orders") = "0" Then
                    Throw New System.Exception("Η παλέτα δεν έχει σχετικά ΠΑΡ. Συμπληρώστε πρώτα σχετική παραγγελία και προσπαθήστε ξανά.")
                    Return 0
                ElseIf Not SkipOrders AndAlso Not dt.Rows(0).Item("orders").ToString.Contains(TryCast(ReturnOrderStatus(ftr_id), Dictionary(Of String, String)).Item("tradecode")) Then
                    Throw New System.Exception("Η παραγγελία του είδους δεν ανήκει στα σχετικά ΠΑΡ της παλέτας. Προσθέστε τη προτού επιχειρήσετε τη κατανομή του είδους στη συγκεκριμένη παλέτα.")
                    Return 0
                End If
                If dt.Rows(0).Item("cusid") <> cusid Then
                    Throw New System.Exception("ΠΡΟΣΟΧΗ! Ο πελάτης της παραγγελίας του είδους δεν συμπίπτει με τον πελάτη της παλέτας!")
                    Return 0
                End If
                Return 1
            End Using
        End Using
    End Function

    Public Function PerformPrePLJobChecks(ByVal pallet_id As Integer)
        Using ccc As New SqlCommand("select isnull(pud.DEPARTMENT,'') dpt,isnull(ph.closedbyid,0) closed,isnull(ph.orders,0) orders,ph.cusid from tbl_palletheaders ph left join tbl_packeruserdata pud on pud.id=ph.LOCKEDBYID where ph.id=" + pallet_id.ToString, conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = ccc.ExecuteReader
                    dt.Load(reader)
                    conn.Close()
                End Using
                If dt.Rows(0).Item("dpt") <> "" AndAlso dt.Rows(0).Item("dpt") <> activeuserdpt Then
                    Throw New System.Exception("Η παλέτα είναι κλειδωμένη από άλλο τμήμα. Δεν μπορείτε να μεταφέρετε είδος.")
                    Return 0
                End If
                If dt.Rows(0).Item("closed") <> 0 Then
                    Throw New System.Exception("Η παλέτα είναι κλειστή. Δεν επιτρέπονται αλλαγές.")
                    Return 0
                End If

                If Not stockpallet AndAlso dt.Rows(0).Item("cusid") <> cusid Then
                    Throw New System.Exception("ΠΡΟΣΟΧΗ! Ο πελάτης της παραγγελίας του είδους δεν συμπίπτει με τον πελάτη της παλέτας!")
                    Return 0
                End If
                Return 1
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Adds palletline
    ''' </summary>
    Public Sub AddItem(ByVal pallet_id As Integer, ite_id As Integer, stl_id As Integer, ftr_id As Integer, quan As Double, Optional batch_num As String = Nothing, Optional from_mantis As Integer = 0, Optional dailyplan_id As Integer = 0, Optional addorder As Boolean = False)
        PackingListCheck(New List(Of Integer) From {pallet_id}, "palletids")
        Dim limit As Double
        If Not stockpallet Then
            If PerformPrePLJobChecks(pallet_id, ftr_id, ite_id, SkipOrders:=addorder) <> 1 Then
                Throw New Exception("Δεν πέρασε τους ελέγχους!")
            End If
        End If
        Using limitcmd As New SqlCommand("SELECT DIFF FROM Z_PACKER_PENDING_ITEMS_PER_ORDER WHERE STLID=" + stl_id.ToString, conn)
            conn.Open()
            limit = limitcmd.ExecuteScalar
            conn.Close()
        End Using
        Dim exists As Double = PalletContainsReturnQuantity(stl_id, pallet_id, dailyplan_id)
        Dim success As Integer = 0
        Dim cmd As String = ""
        If exists = 0 Then 'το είδος από άυτή τη παραγγελία δεν υπάρχει μέσα στη παλέτα, γίνεται insert
            If Not stockpallet Then
                If quan > limit Then
                    Throw New Exception("Η ποσότητα ξεπερνάει το υπόλοιπο για αυτή τη παραγγελία")
                End If
            End If
            cmd = "insert into tbl_palletlines (palletid,iteid,quantity,stlid,ftrid,batchnumber,frommantis,dailyplanid) values (@pallet,@iteid,@quantity,@stlid,@ftrid,@batchnumber,@frommantis,@dailyplanid)"
        Else 'το είδος από άυτή τη παραγγελία  υπάρχει μέσα στη παλέτα, γίνεται update
            If Not stockpallet Then
                If exists + quan > exists + limit Then
                    Throw New Exception("Η ποσότητα ξεπερνάει το υπόλοιπο για αυτή τη παραγγελία")
                End If
            End If
            cmd = "update tbl_palletlines set quantity=@quantity where palletid=@pallet and stlid=@stlid and ftrid=@ftrid and iteid=@iteid and isnull(frommantis,0)=@frommantis"
        End If
        If addorder Then
            Using s As New SqlCommand("update tbl_palletheaders set orders=replace(orders,dbo.get_tradecode(" + ftr_id.ToString + "),'')+' '+dbo.get_tradecode(" + ftr_id.ToString + ") where id=" + pallet_id.ToString, updconn)
                updconn.Open()
                s.ExecuteNonQuery()
                updconn.Close()
            End Using
        End If
        Using sqlcmd As New SqlCommand(cmd, updconn)
            sqlcmd.Parameters.Add("@iteid", SqlDbType.Int).Value = ite_id
            If exists = -1 Then
                sqlcmd.Parameters.Add("@quantity", SqlDbType.Float).Value = quan
            Else
                sqlcmd.Parameters.Add("@quantity", SqlDbType.Float).Value = exists + quan
            End If
            sqlcmd.Parameters.Add("@pallet", SqlDbType.Int).Value = pallet_id
            sqlcmd.Parameters.Add("@stlid", SqlDbType.Int).Value = stl_id
            sqlcmd.Parameters.Add("@ftrid", SqlDbType.Int).Value = ftr_id
            If IsNothing(batch_num) Then
                sqlcmd.Parameters.Add("@batchnumber", SqlDbType.NVarChar, 500).Value = 0
            Else
                sqlcmd.Parameters.Add("@batchnumber", SqlDbType.NVarChar, 500).Value = batch_num
            End If
            sqlcmd.Parameters.Add("@frommantis", SqlDbType.Int).Value = from_mantis
            If dailyplan_id = 0 Then
                sqlcmd.Parameters.Add("@dailyplanid", SqlDbType.Int).Value = DBNull.Value
            Else
                sqlcmd.Parameters.Add("@dailyplanid", SqlDbType.Int).Value = dailyplan_id
            End If
            updconn.Open()
            success = sqlcmd.ExecuteNonQuery()
            updconn.Close()
            If success <= 0 Then
                For Each p As SqlParameter In sqlcmd.Parameters
                    cmd = cmd + " " + p.ParameterName + " " + p.Value.ToString
                Next
                Throw New Exception("Απέτυχε η εισαγωγή:" + cmd)
            Else
                Using sqlcmd2 As New SqlCommand("update tbl_palletheaders Set LUPDATEUSER=" + activeuserid.ToString + " where id=" + pallet_id.ToString, updconn)
                    updconn.Open()
                    sqlcmd2.ExecuteNonQuery()
                    updconn.Close()
                End Using
                If success > 0 Then
                    Using ut As New PackerUserTransaction
                        ut.WriteSTLEntry(activeuserid, 8, stl_id, pallet_id, q:=quan)
                    End Using
                End If
            End If
        End Using
    End Sub

    Public Function RemoveItem(ByVal stl_id As Integer, pallet_id As Integer, Optional from_mantis As Integer = 0)
        PackingListCheck(New List(Of Integer) From {pallet_id}, "palletids")
        Dim success As Integer = 0
        Using c As New SqlCommand("delete from tbl_palletlines where stlid=" + stl_id.ToString + " and palletid=(Select id from tbl_palletheaders where id=" + pallet_id.ToString + " And closedbyid Is null) and isnull(frommantis,0)=@frommantis", updconn)
            c.Parameters.Add("@frommantis", SqlDbType.Int).Value = from_mantis
            updconn.Open()
            success = c.ExecuteNonQuery()
            updconn.Close()
            If success <= 0 Then
                For Each p As SqlParameter In c.Parameters
                    c.CommandText = c.CommandText + " " + p.ParameterName + " " + p.Value
                Next
                Throw New Exception("Απέτυχε η διαγραφή:" + c.CommandText)
            Else
                Using sqlcmd2 As New SqlCommand("update tbl_palletheaders Set LUPDATEUSER=" + activeuserid.ToString + " where id=" + pallet_id.ToString, updconn)
                    updconn.Open()
                    sqlcmd2.ExecuteNonQuery()
                    updconn.Close()
                End Using
                If success > 0 Then
                    Using ut As New PackerUserTransaction
                        ut.WriteSTLEntry(activeuserid, 9, stl_id, pallet_id)
                    End Using
                End If
            End If
        End Using
        Return success
    End Function

    Private Function PalletContainsReturnQuantity(ByVal stl_id As Integer, pallet_id As Integer, dailyplan_id As Integer)
        Dim Quantity As Double = -1
        Dim addtxt As String = ""
        If dailyplan_id > 0 Then
            addtxt = " and dailyplanid=" + dailyplan_id.ToString
        End If
        Using existscmd As New SqlCommand("SELECT ISNULL(quantity,0) quantity FROM TBL_PALLETLINES WHERE PALLETID=" + pallet_id.ToString + " and STLID=" + stl_id.ToString + addtxt, conn)
            conn.Open()
            Quantity = existscmd.ExecuteScalar
            conn.Close()
        End Using
        Return Quantity
    End Function

    ''' <summary>
    ''' Calculates the next pallet main code e.g. VAFR1901-001
    ''' </summary>
    Public Sub GenerateNewPalletCode(Optional ftr_id As String = Nothing, Optional ByVal desired_loc_type As String = Nothing)
        If Not IsNothing(ftr_id) Then
            ftrid = ftr_id
        End If
        desiredloctype = desired_loc_type
        Dim x = pcgenerator()
        newcodes = x
    End Sub

    Public ReadOnly Property AllCodes As Dictionary(Of String, String)
        Get
            If IsNothing(newcodes) Then
                Throw New Exception("Δεν παράχθηκαν κωδικοί. Καλέστε διαδικασία GenerateNewPalletCode πρώτα σε περίπτωση που δεν το κάνατε")
            Else
                Dim r As New Dictionary(Of String, String)
                For i = 0 To newcodes.Count - 1S
                    r.Add(names(i), newcodes(i))
                Next
                Return r
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the pallet's department serial code e.g. BP01 which is here only for legacy reasons, it has been deprecated
    ''' </summary>
    Public ReadOnly Property PalletDptCode As String
        Get
            If IsNothing(newcodes) Then
                Throw New Exception("Δεν παράχθηκαν κωδικοί. Καλέστε διαδικασία GenerateNewPalletCode πρώτα σε περίπτωση που δεν το κάνατε")
            Else
                Return Me.newcodes(1)
            End If

        End Get
    End Property


    Public ReadOnly Property GetFTRID As String
        Get
            If IsNothing(newcodes) Then
                Throw New Exception("Δεν παράχθηκαν κωδικοί. Καλέστε διαδικασία GenerateNewPalletCode πρώτα σε περίπτωση που δεν το κάνατε")
            Else
                Return Me.ftrid
            End If

        End Get
    End Property

    ''' <summary>
    ''' Returns the pallet's WMS location ID
    ''' </summary>
    Public ReadOnly Property PalletLocationID As String
        Get
            If IsNothing(newcodes) Then
                Throw New Exception("Δεν παράχθηκαν κωδικοί. Καλέστε διαδικασία GenerateNewPalletCode πρώτα σε περίπτωση που δεν το κάνατε")
            Else
                Return Me.newcodes(2)
            End If

        End Get
    End Property

    ''' <summary>
    ''' Returns the pallet's WMS location code
    ''' </summary>
    Public ReadOnly Property PalletLocationCode As String
        Get
            If IsNothing(newcodes) Then
                Throw New Exception("Δεν παράχθηκαν κωδικοί. Καλέστε διαδικασία GenerateNewPalletCode πρώτα σε περίπτωση που δεν το κάνατε")
            Else
                Return Me.newcodes(3)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Calculates the substrings of a new pallet, to be used in pallet code creation e.g. VARU1901 and VARU.0P.
    ''' </summary>
    Public Function Calculate_Code_Substrings(desired_loc As String, ByVal f As Integer)
        Try
            If f = 0 And Not stockpallet Then
                Throw New Exception("Δεν έχει παρασχεθεί FTRID!")
            ElseIf f <> 0 And Not stockpallet Then
                Using com As New SqlCommand("select isnull(fathername,'???') from customer where id=" + cusid.ToString, conn)
                    conn.Open()
                    customerfn = com.ExecuteScalar().ToString.Replace(".", "")
                    conn.Close()
                End Using
            ElseIf f = 0 And stockpallet Then
                customerfn = "STOCK"
            End If
            Dim desiredloctype As String = ""
            desiredloctype = desired_loc
            Dim palletfirstcode As String = customerfn + DateTime.Now.ToString("yyMM")
            Dim locname As String = customerfn
            Dim n As String = ""
            If Not stockpallet Then
                Using com2 As New SqlCommand("select mantislevel from tbl_packerordercheck where ftrid=" + f.ToString, conn)
                    conn.Open()
                    n = com2.ExecuteScalar().ToString
                    conn.Close()
                End Using
                Dim userloc As String = ""
                Dim user_special_loc As Integer = 0
                Using s As New SqlCommand("SELECT ISNULL(SPECIALLOC,0) SPECIALLOC,ISNULL(CASE WHEN ULT.DESIREDLOC IS NULL THEN DLT.DESIREDLOC ELSE ULT.DESIREDLOC END,'') DESIREDLOC 
FROM TBL_PACKERUSERDATA PU LEFT JOIN PKRTBL_DPTLOCATIONTYPES DLT ON DLT.DPTCODE=PU.DEPARTMENT LEFT JOIN PKRTBL_USERLOCATIONTYPES ULT ON ULT.USERID=PU.ID where pu.id=" + activeuserid.ToString, conn)
                    Using DT As New DataTable()
                        conn.Open()
                        Using reader As SqlDataReader = s.ExecuteReader
                            DT.Load(reader)
                        End Using
                        conn.Close()
                        userloc = DT.Rows(0).Item("DESIREDLOC")
                        user_special_loc = DT.Rows(0).Item("SPECIALLOC")
                    End Using
                End Using
                userloc = userloc.Replace("#", n)
                If user_special_loc = 1 Then
                    If desiredloctype.Contains("._A.") Then
                        locname = locname + "." + n + "A."
                    ElseIf desiredloctype.Contains("._U.") Then
                        locname = locname + "." + n + "U."
                    ElseIf desiredloctype.Contains("._Y.") Then
                        locname = locname + "." + n + "Y."
                    ElseIf desiredloctype.Contains("._L.") Then
                        locname = locname + "." + n + "L."
                    ElseIf desiredloctype.Contains("._P.") Then
                        locname = locname + "." + n + "P."
                    Else
                        locname = locname + userloc
                    End If
                ElseIf userloc = "" Then
                    Throw New Exception("Αποτυχία αντιστοίχισης χρήστη με θέση αποθήκευσης. Ενημερώστε τον διαχειριστή.")
                Else
                    If desiredloctype.Contains("._A.") Then
                        locname = locname + "." + n + "A."
                    ElseIf desiredloctype.Contains("._U.") Then
                        locname = locname + "." + n + "U."
                    ElseIf desiredloctype.Contains("._Y.") Then
                        locname = locname + "." + n + "Y."
                    ElseIf desiredloctype.Contains("._L.") Then
                        locname = locname + "." + n + "L."
                    ElseIf desiredloctype.Contains("._P.") Then
                        locname = locname + "." + n + "P."
                    Else
                        locname = locname + userloc
                    End If
                End If
            End If
            Dim r As New Dictionary(Of String, String) From {{"PalletCode", palletfirstcode}, {"LocCode", locname}}
            Return r
        Catch ex As Exception
            Dim errtxt As String = ""
            errtxt = "Κάτι δεν πήγε καλά κατά τη σύνδεση με αποθηκευτικό χώρο."
            Throw New System.Exception(errtxt)
        End Try
    End Function


    Public Function AssignDailyPlan(ByVal pallet_id As Integer, SelectedDate As Date)
        Dim checklist As New List(Of Integer) From {pallet_id}
        If SelectedDate < Today Then
            Throw New Exception("Δεν επιτρέπεται η εισαγωγή σε περασμένη ημερομηνία.")
        End If
        PackingListCheck(checklist, "palletids")
        ' DailyPlanQuantityCheck(pallet_id)
        Dim R As Integer = -1
        Dim dfi = DateTimeFormatInfo.CurrentInfo
        Dim calendar = dfi.Calendar
        Dim dpids As New DataTable()
        Using transaction = TransactionUtils.CreateTransactionScope()
            Using s As New SqlCommand("INSERT INTO PKRTBL_DAILYPLAN (TYPE,PERTYPEID1,QUANTITY,DATE,LASTEDITUSERID,WEEKNUMBER) OUTPUT INSERTED.ID SELECT 0,STLID,QUANTITY,@DATE,@USERID,@WEEKNUM FROM TBL_PALLETLINES WHERE PALLETID=" + pallet_id.ToString, updconn)
                s.Parameters.AddWithValue("@USERID", activeuserid)
                s.Parameters.AddWithValue("@DATE", SelectedDate)
                s.Parameters.AddWithValue("@WEEKNUM", calendar.GetWeekOfYear(SelectedDate, dfi.CalendarWeekRule, DayOfWeek.Monday))
                updconn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dpids.Load(reader)
                    updconn.Close()
                End Using
                If dpids.Rows.Count = 0 Then
                    Throw New Exception("Δεν πέτυχε η εισαγωγή. Ενημερώστε τον διαχειριστή. " + s.CommandText)
                End If
            End Using
            Dim dplist As New List(Of Integer)
            dplist = (From ro As DataRow In dpids.Rows.Cast(Of DataRow)() Select CInt(ro("ID"))).ToList
            Using s As New SqlCommand("UPDATE TBL_PALLETLINES SET DAILYPLANID=DP.ID FROM PKRTBL_DAILYPLAN DP WHERE DP.ID IN (" + String.Join(",", dplist.ToArray) + ") AND DP.TYPE=0 AND DP.PERTYPEID1=TBL_PALLETLINES.STLID AND PALLETID=" + pallet_id.ToString, updconn)
                updconn.Open()
                R = s.ExecuteNonQuery()
                updconn.Close()
                If R > 0 Then
                    Using s2 As New SqlCommand("UPDATE TBL_PALLETHEADERS SET LUPDATEUSER=" + activeuserid.ToString + ",status=-2 WHERE ID=" + pallet_id.ToString, updconn)
                        updconn.Open()
                        R = s2.ExecuteNonQuery()
                        updconn.Close()
                    End Using
                    Using ut As New PackerUserTransaction
                        ut.WriteEntry(activeuserid, 10, pallet_id, ut.ReturnDates(dplist)(0).ToString)
                    End Using
                End If
            End Using
            transaction.Complete()
        End Using
        Return R
    End Function

    Private Sub DailyPlanQuantityCheck(ByVal palletid As Integer)
        Using s As New SqlCommand("select isnull(sum(s.primaryqty),0) slq, isnull(sum(pl.quantity),0) plq, isnull(sum(dp.quantity),0) dpq from tbl_palletlines pl inner join storetradelines s on s.id=pl.stlid left join pkrtbl_dailyplan dp on dp.id=pl.dailyplanid where pl.palletid=" + palletid.ToString, conn)
            Using dt As New DataTable()
                conn.Open()
                Using reader As SqlDataReader = s.ExecuteReader
                    dt.Load(reader)
                End Using
                conn.Close()
                If dt(0).Item("plq") + dt(0).Item("dpq") > dt(0).Item("slq") Then
                    Throw New Exception("Άκυρη ποσότητα! Θα ξεπεράσετε τη ποσότητα της παραγγελίας!")
                End If
            End Using
        End Using
    End Sub


    Public Function StockPalletToOrder(ByVal pallet_id As Integer, ftr_id As Integer, Optional desired_loc As String = "")
        PackingListCheck(New List(Of Integer) From {pallet_id}, "palletids")
        Dim ret As Integer = -1
        Dim cmd As String = "select CODE,OPENDATE,CLOSEDATE,REMARKS,CREATEUSER,LUPDATEUSER,WEIGHT,LOCKEDBYID,CLOSEDBYID,PLID,NETWEIGHT,length,height,width,dptcode,loccode,locid,ORDERS,atlantissalesmanid,cusid,plorder,status,PALLETTYPEID,ISSTOCK,ISNOTFULL,PALLETID,ITEID,STLID,ftrid,BATCHNUMBER,quantity,frommantis,pl.ID palletlineid,DAILYPLANID 
from tbl_palletheaders ph left join tbl_palletlines pl on ph.id=pl.PALLETID where ph.id=" + pallet_id.ToString
        Dim cmd1 As String = "select z.iteid,m.subcode1,z.STLID,dbo.get_tradecode(z.FTRID) as tradecode, [ΥΠΟΛ.],pl.quantity from Z_PACKER_ITEMSBROWSER  z left join TBL_PALLETLINES pl on z.iteid=pl.ITEID inner join material m on m.id=z.iteid where z.ftrid=" + ftr_id.ToString + " and pl.PALLETID=" + pallet_id.ToString
        Dim dt As New DataTable()
        Dim dt1 As New DataTable()
        Using s As New SqlCommand(cmd, conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                dt.Load(reader)
                conn.Close()
            End Using
        End Using
        If dt.Rows.Count = 0 Then
            Throw New Exception("Δεν βρέθηκε η παλέτα!")
        End If
        If dt.Rows(0).Item("isstock") = 0 Then
            Throw New Exception("Η παλέτα δεν είναι STOCK.")
        End If
        Using s As New SqlCommand(cmd1, conn)
            conn.Open()
            Using reader As SqlDataReader = s.ExecuteReader
                dt1.Load(reader)
                conn.Close()
            End Using
        End Using
        For Each ro As DataRow In dt1.Rows
            If ro.Item("quantity") > ro.Item("ΥΠΟΛ.") Then
                Throw New Exception("Υπέρβαση υπολοίπου είδους " + ro.Item("subcode1") + " σε ΠΑΡ " + ro.Item("tradecode") + ". Ποσότητα:" + ro.Item("quantity").ToString + " Υπολ:" + ro.Item("ΥΠΟΛ.").ToString)
            End If
        Next
        Dim r As Dictionary(Of String, String)
        r = Calculate_Code_Substrings(desired_loc, ftr_id)
        MantisLocations(r, 1)
        Dim cmd3 As String = "update tbl_palletlines set stlid=s.id, ftrid=" + ftr_id.ToString + " from storetradelines s where palletid=" + pallet_id.ToString + " and s.ftrid=" + ftr_id.ToString + " and s.iteid=tbl_palletlines.iteid"
        Dim cmd2 As String = "update tbl_palletheaders set  REMARKS='Μεταφορά από απόθεμα:'+CODE+' '+isnull(remarks,''),CODE=Q1.n,  lockedbyid=" + activeuserid.ToString + ", loccode=Q2.loc_code, locid=Q2.loc_id, orders=(select dbo.get_tradecode(" + ftr_id.ToString + ")), atlantissalesmanid=" + relsalesman.ToString + ", CUSID=" + cusid.ToString + ",status=1,ISSTOCK=0,ISNOTFULL=0 FROM
(select row_number() over (order by n) rn, n from (select distinct top 1   n='" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) FROM master..[spt_values] WHERE number BETWEEN 1 AND 1000
and '" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) not in (select code from tbl_palletheaders where code like '" + r("PalletCode") + "-%')) Q ) Q1
INNER JOIN (Select  ROW_NUMBER() OVER (ORDER BY LOC_ID) RN,loc_ID,loc_Code FROM lvision.dbo.LV_Location WHERE dbo.charindex2('.',loc_Code,2)<>0 and SUBSTRING(loc_Code,1,dbo.charindex2('.',loc_Code,2)) =  '" + r("LocCode") + "' 
And loc_ID Not in (select LocationID from SC_QTY_MANTISAX where dbo.charindex2('.',LocationCode,2)<>0 and SUBSTRING(LocationCode,1,dbo.charindex2('.',LocationCode,2)) = '" + r("LocCode") + "') 
And loc_ID Not in (select locid from TBL_PALLETHEADERS where CODE Like '" + r("PalletCode") + "%' and locid is not null and (plid is null or plid in (select id from tbl_packinglists where status<>1)))) Q2 ON Q1.RN=Q2.RN where id=" + pallet_id.ToString
        Using transaction = TransactionUtils.CreateTransactionScope()
            Using s As New SqlCommand(cmd3, updconn)
                updconn.Open()
                s.ExecuteNonQuery()
                updconn.Close()
            End Using
            Using s As New SqlCommand(cmd2, updconn)
                updconn.Open()
                ret = s.ExecuteNonQuery()
                updconn.Close()
            End Using
            If ret > 0 Then
                Using ut As New PackerUserTransaction
                    ut.WriteEntry(activeuserid, 11, pallet_id, ftr_id)
                End Using
            End If
            transaction.Complete()
        End Using
        Return ret
    End Function

    Public Function OrderPalletToStock(ByVal pallet_id As Integer)
        PackingListCheck(New List(Of Integer) From {pallet_id}, "palletids")
        PerformPrePLJobChecks(pallet_id)
        Dim ret As Integer = -1
        Dim r As Dictionary(Of String, String)
        r = Calculate_Code_Substrings("", 0)
        Dim cmd1 As String = "update tbl_palletlines set stlid=null,ftrid=null where palletid=" + pallet_id.ToString
        Dim cmd2 As String = "update tbl_palletheaders set REMARKS='Πρώην κανονική παλέτα:'+CODE+' '+isnull(remarks,''),CODE=Q1.n, lockedbyid=" + activeuserid.ToString + ", status=-4,ISSTOCK=1,ISNOTFULL=0,loccode=null,locid=null from
(select row_number() over (order by n) rn, n from (select distinct top 1 n='" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) FROM master..[spt_values] WHERE number BETWEEN 1 AND 1000
and '" + r("PalletCode") + "-'+right('000'+cast(number as varchar(3)),3) not in (select code from tbl_palletheaders where code like '" + r("PalletCode") + "-%')) Q ) Q1 where id=" + pallet_id.ToString
        Using transaction = TransactionUtils.CreateTransactionScope()
            Using s As New SqlCommand(cmd1, updconn)
                updconn.Open()
                s.ExecuteNonQuery()
                updconn.Close()
            End Using
            Using s As New SqlCommand(cmd2, updconn)
                updconn.Open()
                ret = s.ExecuteNonQuery
                updconn.Close()
            End Using
            If ret > 0 Then
                Using ut As New PackerUserTransaction
                    ut.WriteEntry(activeuserid, 12, pallet_id)
                End Using
            End If
            transaction.Complete()
        End Using
        Return ret
    End Function


    ''' <summary>
    ''' Returns next available set of codes 
    ''' </summary>
    Private Function pcgenerator()
        Dim r As Dictionary(Of String, String) = Calculate_Code_Substrings(desiredloctype, ftrid)
        Dim SerialNum As String
        Using comm As New SqlCommand("Select right('000'+cast(count(id)+1 as varchar(3)),3) from TBL_PALLETHEADERS where code like '" + r("PalletCode") + "%'", conn)
            Using dt = New DataTable()
                conn.Open()
                Using reader As SqlDataReader = comm.ExecuteReader()
                    dt.Load(reader)
                    conn.Close()
                    SerialNum = dt.Rows(0).Item(0)
                End Using
            End Using
        End Using
        Dim lid As Integer = 0
        Dim lcode As String = ""
        Dim cmd1 As String = " Select  loc_ID,loc_Code FROM lvision.dbo.LV_Location WHERE dbo.charindex2('.',loc_Code,2)<>0 and SUBSTRING(loc_Code,1,dbo.charindex2('.',loc_Code,2)) =  '" + r("LocCode") + "' 
                                And loc_ID Not in 
                                (select LocationID from SC_QTY_MANTISAX where dbo.charindex2('.',LocationCode,2)<>0 and SUBSTRING(LocationCode,1,dbo.charindex2('.',LocationCode,2)) = '" + r("LocCode") + "') 
                                And loc_ID Not in
                                (select locid from TBL_PALLETHEADERS where CODE Like '" + r("PalletCode") + "%' and locid is not null and (plid is null or plid in (select id from tbl_packinglists where status<>1)))
                                order by loc_Code"  'λίστα θέσεων ΜΑΝΤΙΣ όπου ο κωδικός περιέχει 2 τελείες, ο κωδικός μέχρι τη δεύτερη τελεία ταιριάζει με πελάτη.χρήστη (SFAL.0U), η θέση είναι άδεια και η θέση δεν βρίσκεται συνδεδεμένη με παλέτα που 
        'δεν είναι σε packing list
        Using com1 As New SqlCommand(cmd1, conn)
            Using locdt = New DataTable()
                conn.Open()
                Using locreader As SqlDataReader = com1.ExecuteReader()
                    locdt.Load(locreader)
                    conn.Close()
                    If locdt.Rows.Count = 0 Then
                        Throw New Exception("Δεν υπάρχουν ελεύθερες θέσεις αποθήκευσης.")
                    End If

                End Using
                lid = locdt.Rows(0).Item("loc_ID").ToString
                lcode = locdt.Rows(0).Item("loc_code").ToString
            End Using
        End Using
        Return {r("PalletCode") + "-" + SerialNum, activeuserdpt + "01", lid, lcode}

    End Function

    ''' <summary>
    ''' Moves all items of provided pallets to another pallet, then deletes the provided pallets. All pallets, source and destination alike, must exist. Only for DRAFT, SEMIFINISHED pallets.
    ''' </summary>
    Public Function MoveItems(ByVal fromPallets As List(Of Integer), toPallet As Integer)
        Dim result As Integer = 0
        Using ut As New PackerUserTransaction
            Dim pallets As List(Of String) = ut.ReturnItemCodes(fromPallets)
            Using transaction = TransactionUtils.CreateTransactionScope(IsolationLevel.Serializable) 'rollback feature
                Dim CheckList As New List(Of Integer)
                CheckList.AddRange(fromPallets)
                CheckList.Add(toPallet)
                PackingListCheck(CheckList, "palletids")
                Dim dt As New DataTable()
                dt.Columns.Add("id", GetType(Integer))
                For Each i As Integer In fromPallets
                    Dim row = dt.NewRow()
                    row("id") = i
                    dt.Rows.Add(row)
                Next
                Dim s1 As String = "UPDATE TBL_PALLETLINES SET PALLETID=" + toPallet.ToString + " WHERE PALLETID IN (SELECT ID FROM TBL_PALLETHEADERS WHERE ID IN (" + String.Join(",", fromPallets.ToArray()) + ") AND STATUS IN (-1,-2) AND ISNOTFULL=1)"
                Dim s1check As String = "SELECT STLID,COUNT(STLID) C,SUM(QUANTITY) Q,MIN(ID) I FROM TBL_PALLETLINES where palletid=" + toPallet.ToString + " GROUP BY STLID"
                Dim s2 As String = "UPDATE TBL_PALLETHEADERS SET ORDERS=(SELECT orders FROM get_pallet_orders(@list)) WHERE ID=" + toPallet.ToString
                Dim s3 As String = "DELETE FROM TBL_PALLETHEADERS WHERE ID IN (SELECT ID FROM TBL_PALLETHEADERS WHERE ID IN (" + String.Join(",", fromPallets.ToArray()) + ") AND STATUS IN (-1,-2) AND ISNOTFULL=1)"
                Using s As New SqlCommand(s1, updconn)
                    updconn.Open()
                    result += s.ExecuteNonQuery() 'moves palletlines to toPallet
                    updconn.Close()
                    If result > 0 Then
                        Using ss As New SqlCommand(s2, updconn) 'update ORDERS field in toPallet header record
                            Dim var = ss.Parameters.AddWithValue("@list", dt)
                            var.SqlDbType = SqlDbType.Structured
                            var.TypeName = "dbo.IdList"
                            updconn.Open()
                            result += ss.ExecuteNonQuery()
                            updconn.Close()
                        End Using
                        Using scheck As New SqlCommand(s1check, conn) 'counts occurences of stlid per pallet. 
                            Using data As New DataTable()
                                conn.Open()
                                Using reader As SqlDataReader = scheck.ExecuteReader
                                    data.Load(reader)
                                    conn.Close()
                                End Using
                                If data.Rows.Count > 0 Then
                                    For Each row As DataRow In data.Rows
                                        If row.Item("C") > 1 Then 'If there are multiple records, it will attempt to merge them into one
                                            Dim s1additional1 As String = "DELETE FROM TBL_PALLETLINES WHERE PALLETID=" + toPallet.ToString + " AND ID<>" + row.Item("I").ToString
                                            Dim s1additional2 As String = "UPDATE TBL_PALLETLINES SET QUANTITY=" + row.Item("Q").ToString + " WHERE ID=" + row.Item("I").ToString
                                            Using s1add As New SqlCommand(s1additional1, updconn)
                                                updconn.Open()
                                                Dim r = s1add.ExecuteNonQuery()
                                                updconn.Close()
                                                If r > 0 Then
                                                    Using s1add2 As New SqlCommand(s1additional2, updconn)
                                                        updconn.Open()
                                                        Dim r2 = s1add2.ExecuteNonQuery()
                                                        updconn.Close()
                                                    End Using
                                                End If
                                            End Using
                                        End If
                                    Next
                                Else
                                    Throw New Exception("Δεν επιστράφησαν δεδομένα ενωμένων παλετών!")
                                End If
                            End Using
                        End Using
                        If result > 0 Then
                            Using sss As New SqlCommand(s3, updconn)
                                updconn.Open()
                                result += sss.ExecuteNonQuery()
                                updconn.Close()
                            End Using
                        End If
                        transaction.Complete()
                    End If
                End Using
            End Using
            If result > 0 Then
                ut.WriteEntry(activeuserid, 13, toPallet, pallets)
            End If
        End Using
        Return result
    End Function

    Public Function ToggleSemiFinished(ByVal switch As Boolean, palletid As Integer)
        Dim s As String = ""
        Dim s2 As String = ""
        If switch Then
            s = "UPDATE TBL_PALLETHEADERS SET ISNOTFULL=1 WHERE ID=" + palletid.ToString
            s2 = " ΗΜΙΤΕΛΗ "
        Else
            s = "UPDATE TBL_PALLETHEADERS SET ISNOTFULL=0 WHERE ID=" + palletid.ToString
            s2 = " ΜΗ ΗΜΙΤΕΛΗ "
        End If
        Dim r As Integer
        Using sq As New SqlCommand(s, updconn)
            updconn.Open()
            r = sq.ExecuteNonQuery()
            updconn.Close()
        End Using
        If r > 0 Then
            Using ut As New PackerUserTransaction
                ut.WriteEntry(activeuserid, 14, palletid, s2)
            End Using
        End If
        Return r
    End Function

    Public Function ToggleSemiFinished(ByVal switch As Boolean, palletids As List(Of Integer))
        Dim s As String = ""
        Dim s2 As String = ""
        If switch Then
            s = "UPDATE TBL_PALLETHEADERS SET ISNOTFULL=1 WHERE ID in (" + String.Join(",", palletids.ToArray) + ")"
            s2 = " ΗΜΙΤΕΛΗ "
        Else
            s = "UPDATE TBL_PALLETHEADERS SET ISNOTFULL=0 WHERE ID in (" + String.Join(",", palletids.ToArray) + ")"
            s2 = " ΜΗ ΗΜΙΤΕΛΗ "
        End If
        Dim r As Integer
        Using sq As New SqlCommand(s, updconn)
            updconn.Open()
            r = sq.ExecuteNonQuery()
            updconn.Close()
        End Using
        If r > 0 Then
            Using ut As New PackerUserTransaction
                If switch Then
                    ut.WriteEntry(activeuserid, 48, palletids.Count, ut.ReturnItemCodes(palletids))
                Else
                    ut.WriteEntry(activeuserid, 49, palletids.Count, ut.ReturnItemCodes(palletids))
                End If
            End Using
        End If
        Return r
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                conn.Dispose()

                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
