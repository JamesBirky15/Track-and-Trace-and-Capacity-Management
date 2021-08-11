Public Class frmCapacityManager

    Public CapacityManager As New CapacityManager

    ' When the form is loaded, the number of customers is set to 0 and the maximum capacity is set to its default value
    Private Sub frmCapacityManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateLabels()
    End Sub

    ' When the "Customer has entered" button is pressed, a customer is added
    Private Sub BtnAddCustomer_Click(sender As Object, e As EventArgs) Handles BtnAddCustomer.Click
        CapacityManager.AddCustomer()
        UpdateLabels()
    End Sub

    ' When the "Customer has left" button is pressed, a customer is removed
    Private Sub btnRemoveCustomer_Click(sender As Object, e As EventArgs) Handles btnRemoveCustomer.Click
        CapacityManager.RemoveCustomer()
        UpdateLabels()
    End Sub

    ' Updates the labels to show any changes in capacity, maximum capacity, and whether or not the building is full
    Public Sub UpdateLabels()
        lblNoOfCustomers.Text = "Number of Customers: " & CapacityManager.NumberOfCustomers
        lblMaxCapacity.Text = "Max Capacity: " & CapacityManager.MaxCapacity
        If CapacityManager.CheckStoreIsFull = True Then
            lblBuildingFull.Show()
        Else
            lblBuildingFull.Hide()
        End If
    End Sub

    ' If the button is pressed to change the store capacity, the Change Capacity form opens
    Private Sub btnChangeCapacity_Click(sender As Object, e As EventArgs) Handles btnChangeCapacity.Click
        frmChangeCapacity.Show()
    End Sub

    ' Closes down the form and opens the menu when the exit button is pressed
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        frmMenu.Show()
        Me.Close()
    End Sub
End Class