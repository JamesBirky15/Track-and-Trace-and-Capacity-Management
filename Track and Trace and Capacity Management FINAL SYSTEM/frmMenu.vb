Imports System.IO

Public Class frmMenu


    'When the exit button is pressed, if the user presses yes on the subsequent warning message, the application closes using the Application.Exit command
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        If MessageBox.Show("Are you sure you want to quit?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = 6 Then ' The box returns 6 if the yes button is pressed
            Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " logged out", System.DateTime.Now)
            DataLogger.WriteTransaction()
            Application.Exit()
            My.Computer.Audio.Play(My.Resources.Shutdown, AudioPlayMode.WaitToComplete)
        End If
    End Sub

    'This code executes when the "Customer Contacts Log" button is clicked. It will open the form showing the contact details of the customers who visited the store
    Private Sub btnCustomerContacts_Click(sender As Object, e As EventArgs) Handles btnCustomerContacts.Click
        frmCustomers.Show()
        Me.Close() 'Hides the main menu
    End Sub

    'This code executes when the "Manage Store Capacity" button is clicked. It will open the form allowing for the user to control the number of customers in the building
    Private Sub btnManageCapacity_Click(sender As Object, e As EventArgs) Handles btnManageCapacity.Click
        frmCapacityManager.Show()
        Me.Close()
    End Sub

    ' Only system admins can add,edit and delete staff members. Therefore, I go back to the authenticator object instantiated on the login form. This tells the system whether or not the user that logged in is an admin.
    ' If the user is an admin, the staff form will show. If the user isn't an admin, an error message is displayed and access is denied
    Private Sub btnViewStaff_Click(sender As Object, e As EventArgs) Handles btnViewStaff.Click

        If frmLogin.Admin = True Then
            frmStaff.Show()
            Me.Close()
        Else
            MessageBox.Show("Only administrators can access this feature", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    ' When the main menu is opened, the authenticator object checks whether the user is an admin. This is then used to display whether or not the user is an admin on the menu screen
    ' As normal users can't access the transaction and case logs, the buttons to view the logs are hidden
    Private Sub frmMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If frmLogin.Admin = True Then
            lblVersion.Text = "Version: Administrator"
            btnViewLog.Show()
            lblViewLog.Show()
            btnCasesLog.Show()
            lblViewCasesLog.Show()
        Else
            lblVersion.Text = "Version: User"
            btnViewLog.Hide()
            lblViewLog.Hide()
            btnCasesLog.Hide()
            lblViewCasesLog.Hide()
        End If
    End Sub

    ' Opens the sales form
    Private Sub btnSales_Click(sender As Object, e As EventArgs) Handles btnSales.Click
        frmSales.Show()
        Me.Close()
    End Sub

    ' Opens the data log form
    Private Sub btnViewLog_Click(sender As Object, e As EventArgs) Handles btnViewLog.Click
        frmDataLog.Show()
        Me.Close()
    End Sub

    ' Opens the case log form
    ' If there are no cases stored, an error message is shownn
    Private Sub btnCasesLog_Click(sender As Object, e As EventArgs) Handles btnCasesLog.Click
        If File.Exists("Cases.txt") Then
            frmCaseLog.Show()
            Me.Close()
        Else
            MessageBox.Show("There are no cases stored", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class