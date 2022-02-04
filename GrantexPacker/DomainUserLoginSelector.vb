Public Class DomainUserLoginSelector

    Dim dt As DataTable

    Public Property usernames As DataTable
        Get
            Return dt
        End Get
        Set(value As DataTable)
            dt = value
        End Set
    End Property

    Public Sub New(userdatatable As DataTable)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.usernames = userdatatable

    End Sub
    Private Sub DomainUserLoginSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        usernum.Text = dt.Rows.Count
        username.Text = dt.Rows(0).Item("domainuser")
        ComboBox1.DataSource = dt
        ComboBox1.ValueMember = "id"
        ComboBox1.DisplayMember = "name"
    End Sub

    Public ReadOnly Property selected_user As Integer
        Get
            Return selecteduser
        End Get
    End Property


    Dim selecteduser As Integer
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        selecteduser = ComboBox1.SelectedValue
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Environment.Exit(0)
    End Sub
End Class