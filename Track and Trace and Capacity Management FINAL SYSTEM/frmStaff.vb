
Imports System.IO
Public Class frmStaff

    Structure StaffRecord
        Dim Name As String
        Dim Email As String
        Dim Phone As String
        Dim Address As String
    End Structure

    Dim FileName As String = "Staff.txt"

    ' Exits to the main menu and closes the form
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        frmMenu.Show()
        Me.Close()
    End Sub

    ' Opens the users form
    Private Sub btnManageAuthentication_Click(sender As Object, e As EventArgs) Handles btnManageAuthentication.Click
        frmAuthenticationDetails.Show()
        Me.Close()
    End Sub

    ' If the data is valid, the system adds a new staff member to the staff file using the WriteData() subroutine on the staff class
    ' The new record is then added to the data grid view and the action is logged
    Private Sub btnAddStaff_Click_1(sender As Object, e As EventArgs) Handles btnAddStaff.Click
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
            Dim Staff As New Staff(-1, txtName.Text, txtEmail.Text, txtAddress.Text, txtPhone.Text)
            Staff.Filename = "Staff.txt"
            Staff.WriteData()
            MessageBox.Show("Staff member has been written to the disk", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            PopulateTable()
            ClearInputs()
        End If
    End Sub

    ' When the form opens, the data grid is populated
    Private Sub frmStaff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateTable()
    End Sub

    ' Populates the data grid with all of the records present in the staff file
    Private Sub PopulateTable()
        Dim Table As New DataTable("Table")
        Dim Name As String
        Dim Email As String
        Dim PhoneNumber As String
        Dim Address As String
        Dim PK As String
        Dim Encryptor As New Encryptor

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
                    Address = FileReader.ReadLine()
                    Address = Encryptor.DecryptData(Address)
                    PhoneNumber = FileReader.ReadLine()
                    PhoneNumber = Encryptor.DecryptData(PhoneNumber)
                    Table.Rows.Add(PK, Name, Email, Address, PhoneNumber)
                End While
            End Using
        Else
            MessageBox.Show("Staff file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        dgrStaffData.DataSource = Table
        dgrStaffData.Columns(0).Width = 90
        dgrStaffData.Columns(1).Width = 180
        dgrStaffData.Columns(2).Width = 90
        dgrStaffData.Columns(3).Width = 90
        dgrStaffData.Columns(4).Width = 120
    End Sub

    ' Triggers the DeleteData() subroutine on the Staff object. This deletes the selected staff member from the staff file
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            Dim Staff As New Staff(dgrStaffData.SelectedRows(0).Cells(0).Value, dgrStaffData.SelectedRows(0).Cells(1).Value, dgrStaffData.SelectedRows(0).Cells(2).Value, dgrStaffData.SelectedRows(0).Cells(3).Value, dgrStaffData.SelectedRows(0).Cells(4).Value)
            Staff.DeleteData()
            PopulateTable()
            MessageBox.Show("Details successfully edited", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch
            MessageBox.Show("You haven't selected a record to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' If the data is valid, triggers the EditData() subroutine on the Staff object. This replaces the selected staff member with the staff member entered into the text fields
    Private Sub btnEditStaff_Click(sender As Object, e As EventArgs) Handles btnEditStaff.Click
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
                Dim Staff As New Staff(dgrStaffData.SelectedRows(0).Cells(0).Value, txtName.Text, txtEmail.Text, txtAddress.Text, txtPhone.Text)
                Staff.EditData()
                PopulateTable()
                ClearInputs()
                MessageBox.Show("Details successfully edited", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch
                MessageBox.Show("You haven't selected a staff record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' Allows the user to search for a staff member. If the staff member being searched for is found, it is highlighted. Otherwise, a message box displays
    ' showing that the data isn't present in the database
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim RowIndex As Integer
        Dim Staff As New Staff(-1, "", "", "", "")
        If txtSearch.Text <> Nothing Then
            For Each Row As DataGridViewRow In dgrStaffData.Rows
                Row.DefaultCellStyle.BackColor = Color.White
            Next
            Dim Indexes As New List(Of Integer)
            Indexes = Staff.SearchData(txtSearch.Text)
            If Indexes(0) <> -1 Then
                For Count = 0 To Indexes.Count - 1
                    RowIndex = Indexes(Count)
                    dgrStaffData.Rows(Indexes(Count)).DefaultCellStyle.BackColor = Color.LightBlue
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

    'Checks the name to ensure that all of the characters entered are letter

    Private Function NameCheck() As Boolean
        Dim NameCorrect As Boolean = True
        For Count = 0 To txtName.Text.Length - 1
            If Not Char.IsLetter(txtName.Text.Chars(Count)) Then
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
        Dim Filename As String = "Staffs.txt"
        Dim Valid As Boolean = True
        Dim Staff As New StaffRecord
        Try
            Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
                While FileReader.EndOfStream = False
                    FileReader.ReadLine()
                    Staff.Email = FileReader.ReadLine()
                    Staff.Phone = FileReader.ReadLine
                    If Staff.Email = EmailToWrite Or Staff.Phone = PhoneToWrite Then
                        Valid = False
                    End If
                End While
            End Using
        Catch
            Valid = True
        End Try
        Return Valid
    End Function

    ' Checks the user has selected a staff member and prompts them with a warning message and a choice on whether or not to proceed
    ' If the user selects yes, the CaseInStaff subroutine is executed on the staff object

    Private Sub btnCaseInStaff_Click(sender As Object, e As EventArgs) Handles btnCaseInStaff.Click
        Try
            Dim Staff As New Staff(dgrStaffData.SelectedRows(0).Cells(0).Value, "", "", "", "")
            If MessageBox.Show("Are you sure you want to do this? If you select the wrong staff member, customers could have to self isolate unnecessarily", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = 6 Then
                Staff.CaseInStaff()
                Dim CaseLogger As New DataLogger("", "", System.DateTime.Now)
                CaseLogger.WritePositiveCase(dgrStaffData.SelectedRows(0).Cells(0).Value, dgrStaffData.SelectedRows(0).Cells(1).Value, "Staff Member")
            End If
        Catch ex As Exception
            MessageBox.Show("You haven't selected a staff member", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Clears the text inputs
    Private Sub ClearInputs()
        txtName.Text = Nothing
        txtEmail.Text = Nothing
        txtAddress.Text = Nothing
        txtPhone.Text = Nothing
    End Sub
End Class