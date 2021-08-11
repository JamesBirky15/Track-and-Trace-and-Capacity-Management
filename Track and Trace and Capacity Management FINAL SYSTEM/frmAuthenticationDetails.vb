
Imports System.IO
Public Class frmAuthenticationDetails

    Structure UserRecord
        Dim ID As String
        Dim Admin As String
        Dim Username As String
        Dim Password As String
    End Structure


    ' Validation checks are performed on the password input to check that it's at least 10 characters long. If this isn't complete, an error message is shown
    ' When the user clicks the button to add a new user, the Write User procedure is triggered so the data entered into the text boxes is written to the details file and the table is repopulated to show the new data
    ' If the user attempts to edit a duplicate username or password, an error message is shown
    Private Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Try
            Dim Authenticator As New AuthenticationManager(-1, cmbAdmin.SelectedItem, txtUsername.Text, txtPassword.Text)
            If LengthCheck() = True And txtUsername.Text <> Nothing And txtPassword.Text <> Nothing And cmbAdmin.SelectedItem <> Nothing Then
                Authenticator.WriteData()
                PopulateTable()
                MessageBox.Show("New user has been written", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearInputs()
            ElseIf txtUsername.Text = Nothing Or txtPassword.Text = Nothing Or cmbAdmin.SelectedItem = Nothing Then
                MessageBox.Show("You haven't filled all of the fields out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf LengthCheck() = False Then
                MessageBox.Show("Password isn't long enough", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If
        Catch
            MessageBox.Show("An error has occured")
        End Try
    End Sub

    ' Populates the data grid with the data from the details file
    Private Sub PopulateTable()

        Dim Encryptor As New Encryptor
        Dim Filename As String = "details.txt"
        Dim Table As New DataTable("Table")
        Table.Columns.Add("ID")
        Table.Columns.Add("Admin?")
        Table.Columns.Add("Username")
        Table.Columns.Add("Password")
        Try
            Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
                While FileReader.EndOfStream = False
                    Dim User As New UserRecord
                    User.ID = FileReader.ReadLine
                    User.Admin = FileReader.ReadLine
                    User.Admin = Encryptor.DecryptData(User.Admin)
                    User.Username = FileReader.ReadLine
                    User.Username = Encryptor.DecryptData(User.Username)
                    User.Password = FileReader.ReadLine
                    User.Password = Encryptor.DecryptData(User.Password)
                    Table.Rows.Add(User.ID, User.Admin, User.Username, User.Password)
                End While
            End Using
        Catch
        End Try
        dgrDetails.DataSource = Table
        dgrDetails.Columns(1).Width = 180
        dgrDetails.Columns(2).Width = 180
    End Sub


    ' When the form opens, the table is populated with the data from the user file. If the file isn't available, an error message shows
    Private Sub frmAuthenticationDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            PopulateTable()
        Catch
            MessageBox.Show("User Details file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Asks the user if thet want to delete the user. If they press yes, the DeleteData() subroutine is triggered on the AuthentcationManager, which deletes the selected user
    Private Sub btnDeleteUser_Click(sender As Object, e As EventArgs) Handles btnDeleteUser.Click

        ' A message box returns 6 if yes is pressed

        Try
            Dim Authenticator As New AuthenticationManager(dgrDetails.SelectedRows(0).Cells(0).Value, dgrDetails.SelectedRows(0).Cells(1).Value, dgrDetails.SelectedRows(0).Cells(2).Value, dgrDetails.SelectedRows(0).Cells(3).Value)
            If MessageBox.Show("Are you sure you want to delete this user", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = 6 Then
                Authenticator.Filename = "details.txt"
                Authenticator.DeleteData()
                PopulateTable()
                MessageBox.Show("User has been deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch
                MessageBox.Show("You haven't selected a user to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        frmStaff.Show()
        Me.Close()
    End Sub

    ' Replaces the user selected with the details entered into the text fields
    ' If the admin doesn't fill all of the fields out, the details are invalid, or a user hasn't been selected, an error message is shown
    ' If the user attempts to edit a duplicate username or password, an error message is shown
    Private Sub btnEditUser_Click(sender As Object, e As EventArgs) Handles btnEditUser.Click
        Try
            If cmbAdmin.SelectedItem = Nothing Or txtUsername.Text = Nothing Or txtPassword.Text = Nothing Then
                MessageBox.Show("You haven't filled all of the fields out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf LengthCheck() = False Then
                MessageBox.Show("Password isn't long enough", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                Dim Authenticator As New AuthenticationManager(dgrDetails.SelectedRows(0).Cells(0).Value, cmbAdmin.SelectedItem, txtUsername.Text, txtPassword.Text)
                Authenticator.Filename = "details.txt"
                Authenticator.EditData()
                PopulateTable()
                MessageBox.Show("User successfully edited", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch
            MessageBox.Show("You haven't selected a user to edit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Checks the password is at least 10 characters long
    Private Function LengthCheck()
        Dim RequiredLength As Integer = 10
        If txtPassword.TextLength < 10 Then
            Return False
        Else
            Return True
        End If
    End Function

    ' Clears the input fields
    Private Sub ClearInputs()
        txtUsername.Text = Nothing
        txtPassword.Text = Nothing
        cmbAdmin.SelectedItem = Nothing
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub lblPasswordLength_Click(sender As Object, e As EventArgs) Handles lblPasswordLength.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub cmbAdmin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAdmin.SelectedIndexChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub dgrDetails_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgrDetails.CellContentClick

    End Sub
End Class