Public Class frmChangeCapacity

    ' When the enter button is pressed, it triggers the ChangeMaxCapacity() subroutine on the CapacityManager object we declared in the CapacityManager form
    ' If the data entered was valid, the form will close and it will update the labels on the capacity manager form to show the new capacity
    Private Sub btnEnter_Click(sender As Object, e As EventArgs) Handles btnEnter.Click
        If frmCapacityManager.CapacityManager.CheckNewCapacityIsValid(txtNewCapacity.Text) = True Then
            frmCapacityManager.CapacityManager.ChangeMaxCapacity(txtNewCapacity.Text)
            frmCapacityManager.UpdateLabels()
            Me.Close()
        End If

    End Sub
End Class