Public Class CapacityManager
    Public NumberOfCustomers As Integer ' Stores the number of customer currently in the store
    Public MaxCapacity As Integer = 30 ' Default max capacity is set to 30

    ' If a customer enters a store, the number of customers is incremented by 1
    ' If the current acapacity is already equal to the maximum capacity, a warning message is displayed
    Sub AddCustomer()
        If CheckStoreIsFull() = True Then
            MessageBox.Show("The building is full", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            NumberOfCustomers = NumberOfCustomers + 1
        End If
    End Sub

    ' If a customer leaves the store, the number of customers is decremented by 1

    Sub RemoveCustomer()
        If CheckCustomersNumberIsPositive = True Then
            NumberOfCustomers = NumberOfCustomers - 1
        End If
    End Sub

    ' This allows for the user to input a new maximum capacity. The new capacity will only be entered if the input is valid, which is checked using the CheckNewCapacityIsValid() subroutine
    Sub ChangeMaxCapacity(ByVal pNewCapacity As Integer)
        MaxCapacity = pNewCapacity
        MessageBox.Show("Capacity changed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' If the user tries to decrement the number of customers below 0, an error message is displayed
    Function CheckCustomersNumberIsPositive() As Boolean
        If NumberOfCustomers <= 0 Then
            MessageBox.Show("You cannot have less than 0 customers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        Else
            Return True
        End If
    End Function

    ' Checks that the number of customers is less than the maximum capacity
    Function CheckStoreIsFull() As Boolean
        If NumberOfCustomers >= MaxCapacity Then
            Return True
        Else
            Return False
        End If
    End Function

    ' Checks whether the value for the new capacity of the store isn't null and is an integer. If it isn't, an exception is thrown and this is handled so an error message is shown to the user and the program doesn't crash
    Function CheckNewCapacityIsValid(pNewCapacity) As Boolean
        Dim Valid As Boolean
        Valid = True
        If pNewCapacity = Nothing Then
            MessageBox.Show("Please enter a value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Valid = False
        ElseIf Integer.TryParse(pNewCapacity, Valid) = False Then
            MessageBox.Show("That isn't an integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Valid = False
        ElseIf pNewCapacity = 0 Then
            MessageBox.Show("Max Capacity cannot be 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Valid = False
        ElseIf pNewCapacity < NumberOfCustomers Then
            MessageBox.Show("New capacities cannot be less than the current customer count", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Valid = False
        End If
        Return Valid
    End Function
End Class
