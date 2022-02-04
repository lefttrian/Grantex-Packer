Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Deployment.Application

Public Class itemcomments
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim stlid As Integer
    Dim ftrid As Integer
    Public Sub New(ByVal s As Integer, f As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        stlid = s
        ftrid = f
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Dim t1, t2, t3, t4, t5, t6, t7, t8 As String

    Private Sub Label18_DoubleClick(sender As Object, e As EventArgs) Handles Label18.DoubleClick
        Using frm As New ItemDetails(iteid)
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub Label17_DoubleClick(sender As Object, e As EventArgs) Handles Label17.DoubleClick
        Using frm As New ItemDetails(iteid)
            frm.ShowDialog()
        End Using
    End Sub

    Dim iteid As Integer

    Private Sub orderlabel_DoubleClick(sender As Object, e As EventArgs) Handles orderlabel.DoubleClick
        Using frm As New Order(ftrid)
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub itemcomments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim c As String = "select dbo.get_tradecode(f.id),sm.NAME,'ΑΠΟΣΤΟΛΗ: '+isnull(cast (s0.m_dispatchdate as varchar(30)),'-'),cast(isnull(f.justification,'') as varchar(8000))+isnull(s0.secjustification,'') as exordc
                            ,isnull(t1.warecomments,'') wordc,isnull(t1.prodcomments,'')prodordc,isnull(t1.packcomments,'') pordc,isnull(t3.comments,'') persordc,isnull(s.SECJUSTIFICATION,'') exlc
                            ,isnull(t2.warecomments,'') wlc,isnull(t2.prodcomments,'') prodlc,isnull(t2.packcomments,'') plc, ISNULL(t4.comments,'') perslc,m.code,m.description,m.id as matid
                            from STORETRADELINES s
                            left join FINTRADE f on f.ID=s.ftrid
                            left join storetrade s0  on s.ftrid=s0.ftrid
                            left join tbl_packerordercheck t1 on t1.ftrid=s.ftrid
                            left join tbl_packerordclines t2 on t2.STLID=s.id
                            left join TBL_PACKERUSERORDERCOMMENTS t3 on t3.ftrid=s.ftrid and t3.userid=" + Form1.activeuserid.ToString + "
                            left join TBL_PACKERUSERORDERLINECOMMENTS t4 on t4.stlid=s.id and t4.userid=" + Form1.activeuserid.ToString + "
                            left join SALESMAN sm on sm.ID=f.COLIDSALESMAN
                            left join material m on m.id=s.iteid
                            where s.id=" + stlid.ToString + " and (line=1  OR LINE IS NULL)  "
        Using com As New SqlCommand(c, conn)
            Using dt As New DataTable
                conn.Open()
                Dim reader As SqlDataReader = com.ExecuteReader
                dt.Load(reader)
                conn.Close()
                orderlabel.Text = dt.Rows(0).Item(0)
                salesmanlabel.Text = dt.Rows(0).Item(1)
                datelabel.Text = dt.Rows(0).Item(2)
                Label17.Text = dt.Rows(0).Item("code")
                Label18.Text = dt.Rows(0).Item("description")
                iteid = dt.Rows(0).Item("matid")

                For Each con1 As Control In Me.TableLayoutPanel1.Controls
                    If TypeOf (con1) Is System.Windows.Forms.Panel Then
                        For Each con As Control In con1.Controls
                            If TypeOf (con) Is System.Windows.Forms.TextBox Then
                                For Each col As DataColumn In dt.Columns
                                    If col.ColumnName = con.Name Then
                                        con.Text = dt.Rows(0).Item(col)
                                    End If
                                Next
                            End If

                        Next
                    End If
                Next
            End Using

        End Using
        If Form1.activeuser = "SUPERVISOR" Then
            For Each con1 As Control In Me.TableLayoutPanel1.Controls
                If TypeOf (con1) Is System.Windows.Forms.Panel Then
                    For Each con As Control In con1.Controls
                        If TypeOf (con) Is System.Windows.Forms.TextBox Then
                            DirectCast(con, System.Windows.Forms.TextBox).ReadOnly = False
                        End If

                    Next
                End If
            Next
        End If
        Dim dpt As String = Form1.activeuserdpt
        If dpt = "SP" Then
            wordc.ReadOnly = False
            wlc.ReadOnly = False
        ElseIf dpt = "BL" Or dpt = "BP" Then
            plc.ReadOnly = False
            pordc.ReadOnly = False
        ElseIf dpt = "PRD" Then
            prodlc.ReadOnly = False
            prodordc.ReadOnly = False
        End If
        t1 = prodordc.Text
        t2 = wordc.Text
        t3 = pordc.Text
        t4 = persordc.Text
        t5 = prodlc.Text
        t6 = wlc.Text
        t7 = plc.Text
        t8 = perslc.Text
        Label11.Text = ""
        Label14.Text = ""
        Label15.Text = ""
        Label16.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Cursor.Current = ExtCursor1.Cursor
            Dim dpt As String = Form1.activeuserdpt
            Dim cmd As String = "update tbl_packerordercheck set "
            If dpt = "SP" Then
                cmd = cmd + " warecomments=@oc"

            ElseIf dpt = "BL" Or dpt = "BP" Then
                cmd = cmd + " packcomments=@oc"
            ElseIf dpt = "PRD" Then
                cmd = cmd + " prodcomments=@oc"
            End If
            cmd = cmd + " where ftrid=(select ftrid from storetradelines where id=" + stlid.ToString + ")"
            updconn.Open()
            Using sqlcmd As New SqlCommand(cmd, updconn)
                If dpt = "SP" And wordc.Text <> t2 Then
                    sqlcmd.Parameters.Add("@oc", SqlDbType.VarChar, 8000).Value = wordc.Text
                ElseIf (dpt = "BL" Or dpt = "BP") And pordc.Text <> t3 Then
                    sqlcmd.Parameters.Add("@oc", SqlDbType.VarChar, 8000).Value = pordc.Text
                ElseIf dpt = "PRD" And prodordc.Text <> t1 Then
                    sqlcmd.Parameters.Add("@oc", SqlDbType.VarChar, 8000).Value = prodordc.Text
                End If
                If sqlcmd.Parameters.Contains("@oc") Then
                    Dim success As Integer = sqlcmd.ExecuteNonQuery()
                    Label16.Visible = True
                    If success > 0 Then
                        Label16.Text = " Σχόλια παραγγελίας " + dpt + "ΟΚ"
                        Label16.ForeColor = Color.Green

                    Else
                        Label16.Text = " Σχόλια παραγγελίας " + dpt + " ΟΧΙ"
                        Label16.ForeColor = Color.Red
                    End If

                End If
            End Using


            Using sqlcmd2 As New SqlCommand("[addcomments]")
                sqlcmd2.CommandType = CommandType.StoredProcedure
                sqlcmd2.Connection = updconn
                sqlcmd2.Parameters.Add("@FTRID", SqlDbType.Int).Value = ftrid
                sqlcmd2.Parameters.Add("@userid", SqlDbType.Int).Value = Form1.activeuserid
                If persordc.Text <> t4 Then
                    sqlcmd2.Parameters.Add("@comments", SqlDbType.VarChar, 8000).Value = persordc.Text
                End If
                If sqlcmd2.Parameters.Contains("@comments") Then
                    Dim success As Integer = sqlcmd2.ExecuteNonQuery()
                    Label15.Visible = True
                    If success > 0 Then
                        Label15.Text = " Σχόλια παραγγελίας χρήστη ΟΚ"
                        Label15.ForeColor = Color.Green
                    Else
                        Label15.Text = " Σχόλια παραγγελίας χρήστη ΟΧΙ"
                        Label15.ForeColor = Color.Red
                    End If

                End If
            End Using

            Using sqlcmd3 As New SqlCommand("[ordclines_comments]", updconn)
                sqlcmd3.CommandType = CommandType.StoredProcedure
                Dim DptTransactions As New Dictionary(Of String, Integer) From {{"SP", 31}, {"BP", 27}, {"PRD", 32}, {"BL", 27}}
                If dpt = "SP" And wlc.Text <> t6 Then
                    sqlcmd3.Parameters.Add("@comments", SqlDbType.VarChar, 8000).Value = wlc.Text
                    sqlcmd3.Parameters.Add("@type", SqlDbType.Int).Value = 3
                ElseIf (dpt = "BL" Or dpt = "BP") And plc.Text <> t7 Then
                    sqlcmd3.Parameters.Add("@comments", SqlDbType.VarChar, 8000).Value = plc.Text
                    sqlcmd3.Parameters.Add("@type", SqlDbType.Int).Value = 2
                ElseIf dpt = "PRD" And prodlc.Text <> t5 Then
                    sqlcmd3.Parameters.Add("@comments", SqlDbType.VarChar, 8000).Value = prodlc.Text
                    sqlcmd3.Parameters.Add("@type", SqlDbType.Int).Value = 1
                End If
                sqlcmd3.Parameters.Add("@stlid", SqlDbType.Int).Value = stlid
                If sqlcmd3.Parameters.Contains("@comments") Then
                    Label14.Visible = True
                    Dim success As Integer = sqlcmd3.ExecuteNonQuery()
                    If success > 0 Then
                        Label14.Text = " Σχόλια είδους " + dpt + " ΟΚ"
                        Label14.ForeColor = Color.Green
                    Else
                        Label14.Text = " Σχόλια είδους " + dpt + " ΟΧΙ"
                        Label14.ForeColor = Color.Red
                    End If

                End If
            End Using


            Using sqlcmd4 As New SqlCommand("[addstlidcomments]")
                sqlcmd4.CommandType = CommandType.StoredProcedure
                sqlcmd4.Connection = updconn
                sqlcmd4.Parameters.Add("@stlid", SqlDbType.Int).Value = stlid
                sqlcmd4.Parameters.Add("@userid", SqlDbType.Int).Value = Form1.activeuserid
                If perslc.Text <> t8 Then
                    sqlcmd4.Parameters.Add("@comments", SqlDbType.VarChar, 8000).Value = perslc.Text
                End If

                If sqlcmd4.Parameters.Contains("@comments") Then
                    Dim success As Integer = sqlcmd4.ExecuteNonQuery()
                    Label11.Visible = True
                    If success > 0 Then
                        Label11.Text = " Σχόλια είδους χρήστη ΟΚ"
                        Label11.ForeColor = Color.Green
                    Else
                        Label11.Text = " Σχόλια είδους χρήστη ΟΧΙ"
                        Label11.ForeColor = Color.Red
                    End If

                End If

            End Using
            updconn.Close()
            Form1.orderdgv_refresh()
            Form1.datagridview1_refresh()
            Cursor.Current = Cursors.Default
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