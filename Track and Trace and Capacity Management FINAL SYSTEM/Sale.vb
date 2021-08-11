Imports System.IO

Public Class Sale
    Inherits IOManager

    ' A sale contains 4 main fields, the sale ID, the customer ID, the Staff ID, and the date of the sale

    Public ID As Integer
    Public CustomerID As Integer
    Public StaffID As Integer
    Public SaleDate As DateTime

    ' When the sale object is instantiated, the ID is assigned using the AssignPrimaryKey() subroutine and this, along with the ID of the customer, the ID 
    ' of the staff member and the sale date are written to the sale file
    Overrides Sub WriteData()
        ID = AssignPrimaryKey()
        Using FileWriter As New StreamWriter(Filename, FileMode.Append)
            FileWriter.WriteLine(ID)
            FileWriter.WriteLine(CustomerID)
            FileWriter.WriteLine(StaffID)
            FileWriter.WriteLine(SaleDate.ToString.Substring(0, 10))
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " added new sale", System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' This edits the data selected by the user. It places the contents of the sales file into the transaction array. The system then traverses through the 
    ' array using a linear search and searches for the primary key of the record selected by the user. Once this is found, it replaces the fields with the 
    ' fields entered by the user into the text fields on the staff form. The action is then logged

    Overrides Sub EditData()
        Dim Count As Integer = 0
        Dim CurrentItem As String
        PopulateTransactionArray()
        CurrentItem = TransactionArray(Count)
        While CurrentItem <> ID
            CurrentItem = TransactionArray(Count)
            If CurrentItem <> ID Then
                Count = Count + 4
            End If
        End While

        TransactionArray(Count) = ID
        TransactionArray(Count + 1) = CustomerID
        TransactionArray(Count + 2) = StaffID
        TransactionArray(Count + 3) = SaleDate

        System.IO.File.Delete(Filename)
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            For Count = 0 To TransactionArray.Count - 1
                FileWriter.WriteLine(TransactionArray(Count))
            Next
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " edited sale " & ID, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' Assings a primary key to new records by counting the number of records. The reason I have used overriding is that there is a different number of fields in a sale record than a customer
    ' or staff record. Therefore, there needs to be a different implementation that can handle sales records. If I used the original function, I would get inaccurate values, and this would interefere
    ' with searching routines
    Overrides Function AssignPrimaryKey() As Integer
        Dim MaxPrimaryKey As Integer = 0
        Dim CurrentKey As Integer

        If File.Exists(Filename) Then
            Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
                While FileReader.EndOfStream = False
                    CurrentKey = FileReader.ReadLine()
                    If CurrentKey > MaxPrimaryKey Then
                        MaxPrimaryKey = CurrentKey
                    End If
                    FileReader.ReadLine()
                    FileReader.ReadLine()
                    FileReader.ReadLine()
                End While
            End Using
            Return MaxPrimaryKey + 1
        Else
            Return 0
        End If
    End Function

    ' The sale is deleted from the sales file. This action is then logged
    Public Overrides Sub DeleteData()
        Dim Count As Integer = 0
        Dim CurrentItem As String
        PopulateTransactionArray()
        While CurrentItem <> ID
            CurrentItem = TransactionArray(Count)
            If CurrentItem <> ID Then
                Count = Count + 4
            End If
        End While

        TransactionArray.RemoveRange(Count, 4)

        System.IO.File.Delete(Filename)
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            For Count = 0 To TransactionArray.Count - 1
                FileWriter.WriteLine(TransactionArray(Count))
            Next
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " deleted sale " & ID, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' I use Char.IsDigit on the item in the dropdown box as we only want to save the ID, the numerical part of the selection.
    ' the name of the customer can't be converted to integer form
    ' If the pID parameter = -1. indicating an ID value hasn't been selected and that a new record is being added, a new one is assigned
    ' If an error occurs as the user hasn't selected a value on all of the dropdown boxes, then an error message is shown to the user
    Sub New(ByVal pID As Integer, ByVal pCustomerID As String, ByVal pStaffID As String, ByVal pSaleDate As DateTime)

        For Each Character As Char In pCustomerID
            If Char.IsDigit(Character) Then
                CustomerID = CustomerID & Character
            End If
        Next
        For Each Character As Char In pStaffID
            If Char.IsDigit(Character) Then
                StaffID = StaffID & Character
            End If
        Next

        SaleDate = pSaleDate
        Filename = "Sales.txt"
        If pID = -1 Then
            ID = AssignPrimaryKey()
        Else
            ID = pID
        End If
    End Sub

    ' Populates the transaction array with all of the data from the sales file

    Overrides Sub PopulateTransactionArray()
        Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
            While FileReader.EndOfStream = False
                TransactionArray.Add(FileReader.ReadLine)
            End While
        End Using
    End Sub

    ' This is different to the search data functions on the customer and staff form as an ID for a customer in a sale could also be present on another
    ' Therefore, this highlights ALL of the sales that have the requested value
    Public Function SearchData(ByVal SearchValue As String) As List(Of Integer)
        Dim NoOfFields As Integer
        Dim RowIndex As Integer
        Dim CurrentItem As String
        Dim ItemFound As Boolean = False
        Dim Indexes As New List(Of Integer)
        Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
            Do
                CurrentItem = FileReader.ReadLine
                If CurrentItem = SearchValue Then
                    ItemFound = True
                    RowIndex = NoOfFields \ 4 ' This uses integer division to get the value of the record that the field is contained in
                    Indexes.Add(RowIndex)
                End If
                NoOfFields = NoOfFields + 1
            Loop Until FileReader.EndOfStream = True ' Loops through the file until the end of the file is rteached or the data is found
            If ItemFound = True Then
                Return Indexes
            ElseIf ItemFound = False Then
                Indexes.Add(-1)
                Return Indexes
            End If
        End Using
    End Function

End Class
