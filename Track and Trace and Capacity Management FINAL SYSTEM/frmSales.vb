Imports System.IO


Public Class frmSales

    Dim FileName As String = "Sales.txt"

    ' When the button is pressed to add a new sale, a new sale object is created with the customer, staff member and date selected by the user on the form
    ' This only executes if all of the fields have been filled out to prevent errors from occuring

    Private Sub btnAddSale_Click(sender As Object, e As EventArgs) Handles btnAddSale.Click
        If cmbCustomer.SelectedItem = Nothing Or cmbStaffMember.SelectedItem = Nothing Or dtpDate.Value = Nothing Then
            MessageBox.Show("You haven't filled all of the fields out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Dim Sale As New Sale(-1, cmbCustomer.SelectedItem, cmbStaffMember.SelectedItem, dtpDate.Value)
            Sale.WriteData()
            MessageBox.Show("Data has been written to the disk", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            PopulateTable()
        End If

    End Sub

    ' This goes through the customers file and populates the selection box for customers with the ID of the customer and the name of the customer in brackets.
    Sub GetCustomers()
        Dim Encryptor As New Encryptor
        Using FileReader As New StreamReader("Customers.txt", FileMode.Open)
            While FileReader.EndOfStream = False
                Dim SelectionString As String
                Dim ID As String
                Dim Name As String

                ID = FileReader.ReadLine()
                Name = FileReader.ReadLine()
                Name = Encryptor.DecryptData(Name)
                SelectionString = ID & " (" & Name & ")"

                ' Skips the rest of the record as this won't be placed in the dropdown box

                FileReader.ReadLine()
                FileReader.ReadLine()
                FileReader.ReadLine()

                cmbCustomer.Items.Add(SelectionString)
            End While
        End Using
    End Sub

    ' This populates the transaction array with the names and IDs of all of the staff members from the staff file. It then sorts them into name order
    Sub GetStaff()
        Using FileReader As New StreamReader("Staff.txt", FileMode.Open)
            Dim Encryptor As New Encryptor
            While FileReader.EndOfStream = False
                Dim SelectionString As String
                Dim ID As String
                Dim Name As String

                ID = FileReader.ReadLine()
                Name = FileReader.ReadLine()
                Name = Encryptor.DecryptData(Name)
                SelectionString = ID & " (" & Name & ")"

                ' Skips the rest of the record as this won't be placed in the dropdown box

                FileReader.ReadLine()
                FileReader.ReadLine()
                FileReader.ReadLine()

                cmbStaffMember.Items.Add(SelectionString)
            End While
        End Using
    End Sub

    ' Populates the data grid view with all of the sales in the system
    Sub PopulateTable()
        Dim Table As New DataTable("Table")
        Dim PK As String
        Dim CustomerID As Integer
        Dim StaffID As Integer
        Dim DateOfSale As String

        Table.Columns.Add("Sale ID")
        Table.Columns.Add("Customer ID")
        Table.Columns.Add("Staff ID")
        Table.Columns.Add("Date Of Sale")
        If File.Exists(FileName) Then
            Using FileReader As New StreamReader(File.Open(FileName, FileMode.Open))
                While FileReader.EndOfStream = False
                    PK = FileReader.ReadLine()
                    CustomerID = FileReader.ReadLine()
                    StaffID = FileReader.ReadLine()
                    DateOfSale = FileReader.ReadLine()
                    Table.Rows.Add(PK, CustomerID, StaffID, DateOfSale)
                End While
            End Using
        Else
            MessageBox.Show("Sale file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        dgrSalesData.DataSource = Table
        dgrSalesData.Columns(0).Width = 90
        dgrSalesData.Columns(1).Width = 90
        dgrSalesData.Columns(2).Width = 90
        dgrSalesData.Columns(3).Width = 180
    End Sub

    ' When the form is loaded, using the GetCustomers() and GetStaff() methods, the dropdown boxes are populated with the IDs of each staff member and customer
    ' stored by the system
    ' The data grid table is also populated
    Private Sub frmSales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetCustomers()
        GetStaff()
        PopulateTable()
    End Sub

    ' Exits the form
    Private Sub btnExit_Click_1(sender As Object, e As EventArgs) Handles btnExit.Click
        frmMenu.Show()
        Me.Close()
    End Sub

    ' Triggers the EditData() subroutine on the Sales object. It then updates the table to show the edited data
    ' If the user doesn't select a record, an error message is shown
    ' If there are no records stored, the user is told that the file doesn't exist and records need to be added.
    Private Sub btnEditCustomer_Click(sender As Object, e As EventArgs) Handles btnEditCustomer.Click
        Try
            If File.Exists(FileName) And cmbCustomer.SelectedItem <> Nothing And cmbStaffMember.SelectedItem <> Nothing And dtpDate.Value <> Nothing Then

                Dim Sale As New Sale(dgrSalesData.SelectedRows(0).Cells(0).Value, cmbCustomer.SelectedItem, cmbStaffMember.SelectedItem, dtpDate.Value)
                Sale.EditData()
                PopulateTable()
                MessageBox.Show("Salesuccessfully edited", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " edited sale", System.DateTime.Now)
            ElseIf cmbCustomer.SelectedItem = Nothing Or cmbStaffMember.SelectedItem = Nothing Or dtpDate.Value = Nothing Then
                MessageBox.Show("You haven't filled all of the fields out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("There are no records stored, please add records to use this feature", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch
            MessageBox.Show("You haven't selected a record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Triggers the DeleteData() subroutine on the Sales object. It then updates the table to remove the deleted data
    ' If the user doesn't select a record, an error message is shown
    ' If there are no records stored, the user is told that the file doesn't exist and records need to be added
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If File.Exists(FileName) Then
                Dim Sale As New Sale(dgrSalesData.SelectedRows(0).Cells(0).Value, dgrSalesData.SelectedRows(0).Cells(1).Value, dgrSalesData.SelectedRows(0).Cells(2).Value, dtpDate.Value)
                Sale.DeleteData()
                PopulateTable()
                Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " deleted sale", System.DateTime.Now)
                MessageBox.Show("Sale successfully deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("There are no records stored, please add records to use this feature", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch
            MessageBox.Show("You haven't selected a record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Searches for the sale searched for. If the item is found, the record it's contained in is highlighted and a messagebox is shown showing the IDs of these fields. Otherwise, the user is told it isn't in the database
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim RowIndex As Integer
        Dim Sale As New Sale(1, 1, 1, System.DateTime.Now)
        If txtSearch.Text <> Nothing Then
            For Each Row As DataGridViewRow In dgrSalesData.Rows
                Row.DefaultCellStyle.BackColor = Color.White
            Next
            Dim Indexes As New List(Of Integer)
            Indexes = Sale.SearchData(txtSearch.Text)

            If Indexes(0) <> -1 Then
                For Count = 0 To Indexes.Count - 1
                    RowIndex = Indexes(Count)
                    dgrSalesData.Rows(Indexes(Count)).DefaultCellStyle.BackColor = Color.LightBlue
                Next
                MessageBox.Show("Items found", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("The data you searched for isn't present in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("You haven't entered a search value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        ClearSearchBar()
    End Sub

    ' Clears the search bar
    Sub ClearSearchBar()
        txtSearch.Text = Nothing
    End Sub


End Class