Imports System.IO
Public Class Staff

    Inherits IOManager
    Sub New(ByVal pPrimaryKey As Integer, ByVal pName As String, ByVal pEmail As String, ByVal pAddress As String, ByVal pPhoneNumber As String)
        If pPrimaryKey = -1 Then
            PrimaryKey = AssignPrimaryKey()
        Else
            PrimaryKey = Convert.ToString(pPrimaryKey)
        End If
        Name = pName
        Email = pEmail
        Address = pAddress
        PhoneNumber = pPhoneNumber
        Filename = "Staff.txt"
    End Sub

    ' Goes through the sales file and gets the IDs of all of the customers in contact with the staff member
    ' The system then populates the transaciton array with all of the customer records. The names of the customers with the IDs found to be in contact with the staff member are collected
    ' and these people are told to self isolate
    Sub CaseInStaff()
        Dim Contacts As New ArrayList
        Dim ContactNames As String
        Dim CustomerToFind As Integer
        Dim Encryptor As New Encryptor

        Using FileReader As New StreamReader(File.Open("Sales.txt", FileMode.Open))
            Dim CustomerID As Integer
            Dim StaffID As Integer
            While FileReader.EndOfStream = False
                FileReader.ReadLine()
                CustomerID = FileReader.ReadLine()
                StaffID = FileReader.ReadLine()
                FileReader.ReadLine()
                If StaffID = PrimaryKey Then
                    Contacts.Add(CustomerID)
                End If
            End While
        End Using

        Filename = "Customers.txt"
        PopulateTransactionArray()

        For CustomerCount = 0 To Contacts.Count - 1
            Dim ArrayCount As Integer = 0
            CustomerToFind = Contacts(CustomerCount)
            Do
                If TransactionArray(ArrayCount) <> CustomerToFind Then
                    ArrayCount = ArrayCount + 5
                End If
            Loop Until TransactionArray(ArrayCount) = CustomerToFind Or ArrayCount = TransactionArray.Count - 1
            ContactNames = ContactNames & Encryptor.DecryptData(TransactionArray(ArrayCount + 1)) & " , "
        Next
        If ContactNames <> Nothing Then
            MessageBox.Show(ContactNames & " need to isolate as they came into contact with the positive case")
        Else
            MessageBox.Show("No contacts detected")
        End If
    End Sub

End Class
