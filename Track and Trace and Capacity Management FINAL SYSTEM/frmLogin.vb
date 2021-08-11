Public Class frmLogin

    Public Admin As String


    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim Authenticator As New AuthenticationManager(-1, "Not Admin", txtUsername.Text, txtPassword.Text)
        If txtUsername.Text = Nothing Or txtPassword.Text = Nothing Then 'If the username/password fields haven't been filled out, an error message is displayed
            MessageBox.Show("You haven't filled all of the fields out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            If Authenticator.CheckDetails() = True Then 'Runs the CheckDetails function in the Authentication Manager class to check whether the details are valid
                ' If the function returns true, the user will be granted access and the main menu displayed
                Admin = Authenticator.Admin
                frmMenu.Show()
                Me.Hide()
                Dim DataLogger As New DataLogger(txtUsername.Text, "logged in", System.DateTime.Now)
                DataLogger.WriteTransaction()
            Else 'If the function returns false, an error message is displayed and the failed login attempt is written to the data log
                MessageBox.Show("Incorrect username/password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Dim DataLogger As New DataLogger("", "Failed login attempt ", System.DateTime.Now)
                DataLogger.WriteTransaction()
            End If
        End If

    End Sub

    ' If the user presses the exit button, and selects yes on the subsequent message box, the system exits
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        If MessageBox.Show("Are you sure you want to quit?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = 6 Then ' The box returns 6 if the yes button is pressed
            Application.Exit()
        End If
    End Sub
End Class
