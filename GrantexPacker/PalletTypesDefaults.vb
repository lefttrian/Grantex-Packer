Imports System.Configuration
Imports System.Data.SqlClient

Public Class PalletTypesDefaults
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn").ConnectionString.ToString()
    Dim updString As String = ConfigurationManager.ConnectionStrings("updconn").ConnectionString.ToString()
    Dim conn As New SqlConnection(connString)
    Dim updconn As New SqlConnection(updString)
    Dim pallettypeid As Integer
    Public Sub New(ByVal p As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pallettypeid = p
    End Sub
    Private Sub PalletTypesDefaults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AddHandler Add0.Click, AddressOf add_button_Click
            Using cmd As New SqlCommand("SELECT ID,DESCRIPTION FROM PKRTBL_ITEMTYPES", conn)
                Using DT As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader
                        DT.Load(reader)
                    End Using
                    conn.Close()
                    Dim c As Integer = 0
                    For Each r As DataRow In DT.Rows
                        If c = 0 Then
                            TabPage1.Name = r.Item("ID").ToString
                            TabPage1.Text = r.Item("DESCRIPTION")
                        Else
                            Dim T As New TabPage
                            T.Name = r.Item("ID").ToString
                            T.Text = r.Item("DESCRIPTION")
                            Dim F As New FlowLayoutPanel
                            F.Dock = DockStyle.Fill
                            F.FlowDirection = FlowDirection.TopDown
                            Dim b As New Button
                            b.Text = "Προσθήκη"
                            b.Name = "Add" + c.ToString
                            b.Image = My.Resources.Resources.icons8_plus_16
                            b.TextImageRelation = TextImageRelation.ImageBeforeText
                            b.Width = 82
                            b.Height = 23
                            F.Controls.Add(b)

                            AddHandler b.Click, AddressOf add_button_Click
                            T.Controls.Add(F)
                            TabControl1.TabPages.Add(T)
                        End If
                        c += 1
                    Next
                End Using
            End Using
            Dim cmdtxt As String = "SELECT PT.[ID]      ,PT.[DESCRIPTION]      ,PT.[WIDTH]      ,PT.[LENGTH]      ,PT.[HEIGHT],	  PTD.[ID]  PTDID    ,PTD.[PALLETTYPEID]      ,PTD.[ITEMTYPE]
      ,PTD.[PTDTID]      ,PTD.[PERTYPEID]      ,PTD.[PERTYPECODE]      ,PTD.[PERTYPEINT]  ,PTD.[PERTYPEFLOAT] ,PTD.[PERTYPEMINFLOAT],PTD.[PERTYPEMAXFLOAT],PTD.[PERTYPEMININT] ,PTD.[PERTYPEMAXINT]   ,PTD.[QUANTITY],	  PTDT.[id]     ,PTDT.[DESCRIPTION]
      ,PTDT.[IMPORTANCE]      ,PTDT.[DESCRIPTION_GR] 
FROM PKRTBL_PALLETTYPES PT LEFT JOIN PKRTBL_PALLETTYPESDEFAULTS PTD  ON PT.ID=PTD.PALLETTYPEID 
LEFT JOIN PKRTBL_PTDTYPES PTDT ON PTDT.ID=PTD.PTDTID WHERE PT.ID=" + pallettypeid.ToString + " ORDER BY ITEMTYPE ASC,PTDT.IMPORTANCE ASC"
            Using cmd As New SqlCommand(cmdtxt, conn)
                Using dt As New DataTable()
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader
                        dt.Load(reader)
                    End Using
                    conn.Close()
                    For Each r As DataRow In dt.Rows
                        For Each t As TabPage In TabControl1.TabPages
                            If t.Name = r.Item("ITEMTYPE").ToString Then
                                Dim c As Integer = TryCast(t.Controls(0), FlowLayoutPanel).Controls.Count
                                Dim value As String
                                If Not IsDBNull(r.Item("PERTYPEID")) Then
                                    value = r.Item("PERTYPEID").ToString
                                ElseIf Not IsDBNull(r.Item("PERTYPECODE")) Then
                                    value = r.Item("PERTYPECODE").ToString
                                ElseIf Not IsDBNull(r.Item("PERTYPEINT")) Then
                                    value = r.Item("PERTYPEINT").ToString
                                ElseIf Not IsDBNull(r.Item("PERTYPEFLOAT")) Then
                                    value = r.Item("PERTYPEFLOAT").ToString
                                ElseIf Not IsDBNull(r.Item("PERTYPEMINFLOAT")) Then
                                    value = r.Item("PERTYPEMINFLOAT").ToString + "-" + r.Item("PERTYPEMAXFLOAT").ToString
                                ElseIf Not IsDBNull(r.Item("PERTYPEMININT")) Then
                                    value = r.Item("PERTYPEMININT").ToString + "-" + r.Item("PERTYPEMAXINT").ToString
                                End If
                                TryCast(t.Controls(0), FlowLayoutPanel).Controls.Add(New PalletTypesDefaultsItem(c.ToString, r.Item("DESCRIPTION_GR"), value, r.Item("DESCRIPTION"), r.Item("QUANTITY"), r.Item("PTDID")))
                            End If
                        Next
                    Next

                End Using

            End Using
        Catch ex As Exception
            Using errfrm As New errormsgbox(ex.StackTrace.ToString, ex.Message, "Κάτι πήγε στραβά!") : errfrm.ShowDialog() : End Using
        End Try
    End Sub

    Private Sub add_button_Click(sender As Object, e As EventArgs)
        Using frm As New PalletTypesDefaultsAdditem(pallettypeid, TryCast(sender.Parent.Parent, TabPage).Name)
            frm.ShowDialog()

        End Using
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub
End Class