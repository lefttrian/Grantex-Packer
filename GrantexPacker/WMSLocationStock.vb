Imports System.Data.Objects
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Deployment.Application

Public Class WMSLocationStock
    Dim iteid As String
    Dim desc As String
    Dim code As String
    Dim cc As String
    Dim vb As Boolean
    Private Sub Form12_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Cursor.Current = ExtCursor1.Cursor
        Me.Label1.Text = code + " " + desc
        Me.SC_QTY_MANTISAXTableAdapter.Fill(Me.TESTFINALDataSet.SC_QTY_MANTISAX, iteid)
        Me.SC_QTY_MANTISAX_RETURNSTableAdapter.Fill(Me.TESTFINALDataSet.SC_QTY_MANTISAX_RETURNS, iteid)
        Cursor.Current = Cursors.Default
        If vb = False Then
            Button2.Visible = False
        End If
        Button3.Text = "Θέσεις αποθήκευσης για " + code.Substring(0, 8) + "..."
    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub



    Private Sub button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal i As String, d As String, c As String, Optional visiblebutton As Boolean = True, Optional cc As String = "")
        iteid = i
        desc = d
        If cc = "" Then
            code = c
        Else
            code = cc
        End If

        vb = visiblebutton
        InitializeComponent()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using frm = New ItemDetails(iteid)
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using frm = New Form17(iteid, code)
            frm.ShowDialog()
        End Using
    End Sub
End Class