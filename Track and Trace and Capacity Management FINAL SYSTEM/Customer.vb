
Imports System.IO 'Inputs the library programs into VB allowing for files to be written/modified
Public Class Customer
    Inherits IOManager ' The customer class 

    'This procedure allows for a customer to be instantiated. This means we can handle customers in files based on certain parameters, which will be the text in text boxes on the customer data form
    ' If the primary key is being written rather than an existing primary key being used to search/delete data, then it is set to -1 
    ' , a rogue value. This tells the system to assign one
    ' The filename attribute is set to Customers.txt, telling the IOManager to handle the customers file
    Sub New(ByVal pPrimaryKey As Integer, ByVal pName As String, ByVal pEmail As String, ByVal pAddress As String, ByVal pPhoneNumber As String)
        If pPrimaryKey = -1 Then
            PrimaryKey = AssignPrimaryKey()
        Else
            Convert.ToChar(pPrimaryKey)
        End If
        PrimaryKey = pPrimaryKey
        Name = pName
        Email = pEmail
        Address = pAddress
        PhoneNumber = pPhoneNumber
        Filename = "Customers.txt"
        RecordType = "Customer"
    End Sub

    ' Goes through the sales file and gets the IDs of all of the staff members in contact with the customer
    ' The system then populates the transaciton array with all of the staff records. The names of the staff members with the IDs found to be in contact with the customer are collected
    ' and these people are told to self isolate
    Sub CaseInCustomer()
        Dim Contacts As New ArrayList
        Dim ContactNames As String
        Dim StaffToFind As Integer
        Dim Encryptor As New Encryptor

        Using FileReader As New StreamReader(File.Open("Sales.txt", FileMode.Open))
            Dim CustomerID As Integer
            Dim StaffID As Integer
            While FileReader.EndOfStream = False
                FileReader.ReadLine()
                CustomerID = FileReader.ReadLine()
                StaffID = FileReader.ReadLine()
                FileReader.ReadLine()
                If CustomerID = PrimaryKey Then
                    Contacts.Add(StaffID)
                End If
            End While
        End Using

        Filename = "Staff.txt"
        PopulateTransactionArray()

        For CustomerCount = 0 To Contacts.Count - 1
            Dim ArrayCount As Integer = 0
            StaffToFind = Contacts(CustomerCount)
            Do
                If TransactionArray(ArrayCount) <> StaffToFind Then
                    ArrayCount = ArrayCount + 5
                End If
            Loop Until TransactionArray(ArrayCount) = StaffToFind Or ArrayCount = TransactionArray.Count - 1
            ContactNames = ContactNames & Encryptor.DecryptData(TransactionArray(ArrayCount + 1)) & " , "
        Next
        If ContactNames <> Nothing Then
            MessageBox.Show(ContactNames & " need to isolate as they came into contact with the positive case")
        Else
            MessageBox.Show("No contacts detected")
        End If
    End Sub
End Class
