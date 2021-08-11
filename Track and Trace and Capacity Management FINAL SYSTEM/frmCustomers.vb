Imports System.IO
Public Class frmCustomers

    Dim FileName As String = "Customers.txt"
    Structure CustomerRecord
        Dim Name As String
        Dim Email As String
        Dim Phone As String
        Dim Address As String
    End Structure

    'This code excecutes when the exit button is hit. The customers form closes and the system goes back to the main menu
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        frmMenu.Show()
        Me.Close()
    End Sub

    'This code executes when the Add Customer button is clicked. It executes the WriteCustomerToFile command on the customer object we instantiated with the parameters being the details we entered into the 
    ' text boxes, meaning a customer can be added to the text file which has the details entered into the text box
    'If there is an error with the data, a message box shows alerting the user that an error has occured. Otherwise, it tells the user that the customer has succesfully been written
    Private Sub btnAddCustomer_Click(sender As Object, e As EventArgs) Handles btnAddCustomer.Click
        If CheckDataIsValid() = False Then
            If NameCheck() = False Then
                MessageBox.Show("A name cannot contain characters other than letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf EmailCheck() = False Then
                MessageBox.Show("An email must contain @", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf PhoneCheck() = False Then
                MessageBox.Show("A phone number can only contain integers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf DuplicateCheck(txtName.Text, txtEmail.Text, txtAddress.Text, txtPhone.Text) = False Then
                MessageBox.Show("You have entered a duplicate record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            End If
        Else
            Dim Customer As New Customer(-1, txtName.Text, txtEmail.Text, txtAddress.Text, txtPhone.Text)
            Customer.Filename = "Customers.txt"
            Try
                Customer.WriteData()
                MessageBox.Show("Customer has been written to the disk", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                PopulateTable()
                ClearInputs()
            Catch
                MessageBox.Show("An error has occured")
            End Try



        End If

    End Sub

    'Runs a presence check and a type check to determine whether all of the fields have been filled out and the data being entered for certain fields are of the appropriate data type
    Private Function CheckDataIsValid()
        If PresenceCheck() = False Then
            MessageBox.Show("You haven't filled all of the fields out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        Else
            If NameCheck() = True And EmailCheck() = True And PhoneCheck() = True And DuplicateCheck(txtName.Text, txtEmail.Text, txtAddress.Text, txtPhone.Text) = True Then
                Return True
            Else

                Return False
            End If
        End If

    End Function

    'Checks that all of the fields have been filled out. If data is missing, it returns false, otherwise, it returns true
    Private Function PresenceCheck() As Boolean
        If txtName.Text = Nothing Or txtEmail.Text = Nothing Or txtAddress.Text = Nothing Or txtPhone.Text = Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    'Checks the name to ensure that all of the characters entered are letters or spaces

    Private Function NameCheck() As Boolean
        Dim NameCorrect As Boolean = True
        For Count = 0 To txtName.Text.Length - 1
            If Not Char.IsLetter(txtName.Text.Chars(Count)) Or Char.IsWhiteSpace(txtName.Text.Chars(Count)) Then
                NameCorrect = False ' If it is found that a character in a name isn't a letter, the NameCorrect variable is set to false
            End If
        Next
        Return NameCorrect
    End Function

    ' Checks whether @ is present in the email field
    Private Function EmailCheck() As Boolean
        Dim AtPosition As Integer

        AtPosition = txtEmail.Text.IndexOf("@")
        If AtPosition = -1 Then
            Return False
        Else
            Return True
        End If

    End Function

    ' Checks whether all of the characters in the phone number are integers
    Private Function PhoneCheck() As Boolean
        Dim Valid As Boolean = True
        For Count = 0 To txtPhone.Text.Length - 1
            Try
                Convert.ToInt32(txtPhone.Text.Substring(Count, 1))
            Catch ex As Exception
                Valid = False
            End Try
        Next
        Return Valid
    End Function



    ' Goes through all of the data using a loop and checks for duplicate records, which stops storage space being wasted. 
    ' The try catch statement ensures that the duplicate check doesn't execute if the record being added is the first, otherwise, due to the absence of the file,
    ' the system crashes
    Function DuplicateCheck(ByVal NameToWrite As String, ByVal EmailToWrite As String, ByVal AddressToWrite As String, ByVal PhoneToWrite As String) As Boolean
        Dim Filename As String = "Customers.txt"
        Dim Valid As Boolean = True
        Dim Customer As CustomerRecord
        Try
            Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
                While FileReader.EndOfStream = False
                    FileReader.ReadLine()
                    Customer.Email = FileReader.ReadLine()
                    Customer.Phone = FileReader.ReadLine
                    If Customer.Email = EmailToWrite Or Customer.Phone = PhoneToWrite Then
                        Valid = False
                    End If
                End While
            End Using
        Catch
            Valid = True
        End Try
        Return Valid

    End Function


    ' First, the system runs the Decryptor routine on the encryptor object to decrpyt the data from the file
    ' Populates the data grid view with the decrypted data from the customer data file. If the file isn't found, an error message is shown to the user and no data
    ' is added
    Private Sub PopulateTable()
        Dim Encryptor As New Encryptor
        Dim Table As New DataTable("Table")
        Dim Name As String
        Dim Email As String
        Dim PhoneNumber As String
        Dim Address As String
        Dim PK As String

        Table.Columns.Add("ID")
        Table.Columns.Add("Name")
        Table.Columns.Add("Email Address")
        Table.Columns.Add("Address")
        Table.Columns.Add("Phone number")
        If File.Exists(FileName) Then
            Using FileReader As New StreamReader(File.Open(FileName, FileMode.Open))
                While FileReader.EndOfStream = False
                    PK = FileReader.ReadLine()
                    Name = FileReader.ReadLine()
                    Name = Encryptor.DecryptData(Name)
                    Email = FileReader.ReadLine()
                    Email = Encryptor.DecryptData(Email)
                    Address = (FileReader.ReadLine())
                    Address = Encryptor.DecryptData(Address)
                    PhoneNumber = FileReader.ReadLine()
                    PhoneNumber = Encryptor.DecryptData(PhoneNumber)
                    Table.Rows.Add(PK, Name, Email, Address, PhoneNumber)
                End While
            End Using
        Else
            MessageBox.Show("Customer file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        dgrCustomerData.DataSource = Table
        dgrCustomerData.Columns(0).Width = 90
        dgrCustomerData.Columns(1).Width = 180
        dgrCustomerData.Columns(2).Width = 180
        dgrCustomerData.Columns(3).Width = 180
        dgrCustomerData.Columns(4).Width = 100
    End Sub

    ' This allows for the data grid view to be populated with the customer data when the form is loaded
    Private Sub frmCustomers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateTable()
    End Sub

    ' When the delete button is presssed, the customer record highlighted is deleted. If a user hasn't been selected, an error message is shown to the user
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            Dim Customer As New Customer(dgrCustomerData.SelectedRows(0).Cells(0).Value, dgrCustomerData.SelectedRows(0).Cells(1).Value, dgrCustomerData.SelectedRows(0).Cells(2).Value, dgrCustomerData.SelectedRows(0).Cells(3).Value, dgrCustomerData.SelectedRows(0).Cells(4).Value)
            Customer.DeleteData()
            dgrCustomerData.Rows.Remove(dgrCustomerData.SelectedRows(0))
            MessageBox.Show("Details successfully deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch
            MessageBox.Show("Please select a customer record to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


    ' If the data is present, this subroutine highlights the row containg the data that was searched for. If the data isn't present, an error message is shown
    ' If the user hasn't searched for a value, an error message is shown
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim RowIndex As Integer
        Dim Customer As New Customer(-1, "", "", "", "")
        If txtSearch.Text <> Nothing Then
            For Each Row As DataGridViewRow In dgrCustomerData.Rows
                Row.DefaultCellStyle.BackColor = Color.White
            Next
            Dim Indexes As New List(Of Integer)
            Indexes = Customer.SearchData(txtSearch.Text)
            If Indexes(0) <> -1 Then
                For Count = 0 To Indexes.Count - 1
                    RowIndex = Indexes(Count)
                    dgrCustomerData.Rows(Indexes(Count)).DefaultCellStyle.BackColor = Color.LightBlue
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

    ' Clears the search bar once it is used
    Sub ClearSearchBar()
        txtSearch.Clear()
    End Sub

    ' Clears the data input text boxes when they're used
    Sub ClearInputs()
        txtName.Clear()
        txtEmail.Clear()
        txtAddress.Clear()
        txtPhone.Clear()
    End Sub

    ' Triggers the EditCustomer() subroutine on the Customer object. If a customer record hasn't been selected, an error message is displayed to a user.
    ' The fields entered unedrgo validation to ensure that the inputs entered are valid like the data that has been written to the system
    Private Sub btnEditCustomer_Click(sender As Object, e As EventArgs) Handles btnEditCustomer.Click
        If CheckDataIsValid() = False Then
            If NameCheck() = False Then
                MessageBox.Show("A name cannot contain characters other than letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf EmailCheck() = False Then
                MessageBox.Show("An email must contain @", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf PhoneCheck() = False Then
                MessageBox.Show("A phone number can only contain integers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf DuplicateCheck(txtName.Text, txtEmail.Text, txtAddress.Text, txtPhone.Text) = False Then
                MessageBox.Show("You have entered a duplicate record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            End If
        Else
            Try

                Dim Customer As New Customer(dgrCustomerData.SelectedRows(0).Cells(0).Value, txtName.Text, txtEmail.Text, txtAddress.Text, txtPhone.Text)
                Customer.EditData()
                PopulateTable()
                ClearInputs()
                MessageBox.Show("Details successfully edited", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch
                MessageBox.Show("You haven't selected a record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

    End Sub

    ' A meesage box is displayed to the user. If they click yes, the contacts of the case are shown on a message box and the case is logged to the cases file
    Private Sub btnCaseInCustomer_Click(sender As Object, e As EventArgs) Handles btnCaseInCustomer.Click
        Try
            Dim Customer As New Customer(dgrCustomerData.SelectedRows(0).Cells(0).Value, "", "", "", "")
            If MessageBox.Show("Are you sure you want to do this? If you select the wrong staff member, customers could have to self isolate unnecessarily", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = 6 Then
                Customer.CaseInCustomer()
                Dim CaseLogger As New DataLogger("", "", System.DateTime.Now)
                CaseLogger.WritePositiveCase(dgrCustomerData.SelectedRows(0).Cells(0).Value, dgrCustomerData.SelectedRows(0).Cells(1).Value, "Customer")
            End If
        Catch
            MessageBox.Show("You haven't selected a customer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class