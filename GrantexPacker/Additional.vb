Module Additional

    Public Sub LockUIAccess(ByVal c As Form)
        If Form1.activeuserdpt <> "SA" Then
            Dim dv As DataView = New DataView(Form1.DptAccess)
            dv.RowFilter = "FORM='" + c.Name + "'"
            For Each r As DataRowView In dv
                Dim t As Control() = c.Controls.Find(r.Item("CONTROL"), True)
                Dim tbxs As Object
                If IsNothing(t) Or t.Length = 0 Then
                    For Each con As Control In GetAll(c, GetType(MenuStrip))
                        Dim ms As MenuStrip = TryCast(con, MenuStrip)
                        If Not IsNothing(ms) Then
                            tbxs = ms.Items.Find(r.Item("CONTROL"), True)
                        End If
                    Next
                Else
                    tbxs = t
                End If
                If (Not IsNothing(tbxs) And tbxs.Length > 0) Then
                    If r.Item("LOCKED") = 1 Then
                        tbxs(0).Enabled = False
                    Else
                        tbxs(0).Enabled = True
                    End If
                    If r.Item("NOTVISIBLE") = 1 Then
                        tbxs(0).Visible = False
                    Else
                        tbxs(0).Visible = True
                    End If
                    If tbxs(0).GetType = GetType(ComboBox) AndAlso Not IsDBNull(r.Item("DEFAULTVALUE")) Then
                        TryCast(tbxs(0), ComboBox).SelectedValue = r.Item("DEFAULTVALUE")
                    End If
                End If
            Next
            Dim dv2 As DataView = New DataView(Form1.UserAccess)
            dv2.RowFilter = "FORM='" + c.Name + "'"
            For Each r As DataRowView In dv2
                Dim t As Control() = c.Controls.Find(r.Item("CONTROL"), True)
                Dim tbxs As Object
                If IsNothing(t) Or t.Length = 0 Then
                    For Each con As Control In GetAll(c, GetType(MenuStrip))
                        Dim ms As MenuStrip = TryCast(con, MenuStrip)
                        If Not IsNothing(ms) Then
                            tbxs = ms.Items.Find(r.Item("CONTROL"), True)
                        End If
                    Next
                Else
                    tbxs = t
                End If
                If (Not IsNothing(tbxs) And tbxs.Length > 0) Then
                    If r.Item("LOCKED") = 1 Then
                        tbxs(0).Enabled = False
                    Else
                        tbxs(0).Enabled = True
                    End If
                    If r.Item("NOTVISIBLE") = 1 Then
                        tbxs(0).Visible = False
                    Else
                        tbxs(0).Visible = True
                    End If
                    If tbxs(0).GetType = GetType(ComboBox) AndAlso Not IsDBNull(r.Item("DEFAULTVALUE")) Then
                        TryCast(tbxs(0), ComboBox).SelectedValue = r.Item("DEFAULTVALUE")
                    End If
                End If
            Next
        End If
    End Sub

    Public Function GetAll(ByVal control As Control, ByVal type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAll(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type)
    End Function
End Module
