
Imports System.IO
Public Class IOManager

    Protected PrimaryKey As String ' This cannot be an integer as the text file stores data as strings of characters
    Protected Name As String
    Protected Email As String
    Protected Address As String
    Protected PhoneNumber As String
    Protected TransactionArray As New ArrayList
    Dim Encryptor As New Encryptor
    Public RecordType As String ' Used to tell the system which type of record is being added so it can be logged
    Public Filename As String
    Public FieldTypeToSearch As String

    'This procedure writes a customer to the customers text file. The BinaryWriter from the System.IO library we imported allows this. Filename is the name of the customers file, "Customers.bin". Filemode.append
    ' means that we can edit the data in the file. The assigned primary key, name, email, address and phone number are written. This action is written to the data log
    'The primary key is assigned using the function AssignPrimaryKey
    Overridable Sub WriteData()
        Dim Encryptor As New Encryptor
        PrimaryKey = AssignPrimaryKey()
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            FileWriter.WriteLine(PrimaryKey.ToString)
            Name = Encryptor.EncryptData(Name)
            FileWriter.WriteLine(Name)
            Email = Encryptor.EncryptData(Email)
            FileWriter.WriteLine(Email)
            Address = Encryptor.EncryptData(Address)
            FileWriter.WriteLine(Address)
            PhoneNumber = Encryptor.EncryptData(PhoneNumber)
            FileWriter.WriteLine(PhoneNumber)
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " added new " & RecordType, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub


    ' This method first populates the transaction array. The user selects the record they want to edit and enters the new fields into the text box. The system searches for the primary key of the record to be edited, and replaces
    ' it with the data from the text box fields and it keeps the primary key the same
    ' The file is then rewritten without the data which the user entered
    Overridable Sub EditData()
        Dim Count As Integer = 0
        Dim CurrentItem As String
        PopulateTransactionArray()
        CurrentItem = TransactionArray(Count)




        While CurrentItem <> PrimaryKey
            CurrentItem = TransactionArray(Count)
            If CurrentItem <> PrimaryKey Then
                Count = Count + 5
            End If
        End While

        TransactionArray(Count + 1) = Encryptor.EncryptData(Name)
        TransactionArray(Count + 2) = Encryptor.EncryptData(Email)
        TransactionArray(Count + 3) = Encryptor.EncryptData(Address)
        TransactionArray(Count + 4) = Encryptor.EncryptData(PhoneNumber)

        System.IO.File.Delete(Filename)
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            Count = 0
            While Count <= TransactionArray.Count - 1
                FileWriter.WriteLine(TransactionArray(Count))
                Count = Count + 1
            End While
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " edited " & RecordType & PrimaryKey, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' This method first populates the transaction array. The user selects the record they want to delete, and the system searches for the record in the transaction array before deleting it. 
    ' The file is then rewritten without the data which the user requested to delete and this is written to the data log
    Overridable Sub DeleteData()
        Dim Count As Integer = 0
        Dim CurrentItem As String
        PopulateTransactionArray()

        While CurrentItem <> PrimaryKey
            CurrentItem = TransactionArray(Count)
            If CurrentItem <> PrimaryKey Then
                Count = Count + 5
            End If
        End While

        TransactionArray.RemoveRange(Count, 5)

        System.IO.File.Delete(Filename)

        Count = 0
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            If TransactionArray.Count <> 0 Then
                Do
                    FileWriter.WriteLine(TransactionArray(Count))
                    Count = Count + 1
                Loop Until Count = TransactionArray.Count
            End If
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " deleted " & RecordType & PrimaryKey, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' This uses a linear search to search for the record based on the data entered. If it finds the data, it will return the index of the record which the 
    ' data lies in. If the data isn't found, it returns -1 (Null value)
    Public Function SearchData(ByVal SearchValue As String) As List(Of Integer)
        Dim NoOfFields As Integer
        Dim RowIndex As Integer
        Dim CurrentItem As String
        Dim ItemFound As Boolean = False
        Dim Indexes As New List(Of Integer)
        Dim Encryptor As New Encryptor
        Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
            Do
                CurrentItem = FileReader.ReadLine
                CurrentItem = Encryptor.DecryptData(CurrentItem)
                If CurrentItem = SearchValue Then
                    ItemFound = True
                    RowIndex = NoOfFields \ 5 ' This uses integer division to get the value of the record that the field is contained in
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

    ' This populates an arraylist which temporarily holds the data while it is being edited/deleted before the customer file is overwritten
    Overridable Sub PopulateTransactionArray()
        Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))

            Dim Encryptor As New Encryptor
            While FileReader.EndOfStream = False
                Dim CurrentID As String
                Dim CurrentName As String
                Dim CurrentEmail As String
                Dim CurrentAddress As String
                Dim CurrentPhone As String
                CurrentID = FileReader.ReadLine()
                CurrentName = FileReader.ReadLine()
                CurrentEmail = FileReader.ReadLine()
                CurrentAddress = FileReader.ReadLine()
                CurrentPhone = FileReader.ReadLine()
                TransactionArray.Add(CurrentID)
                TransactionArray.Add(CurrentName)
                TransactionArray.Add(CurrentEmail)
                TransactionArray.Add(CurrentAddress)
                TransactionArray.Add(CurrentPhone)
            End While
        End Using
    End Sub

    'Assigns a primary key to the record to be added, meaning it can be uniquely identified. This is done by adding 1 to the maximum primary key in the data
    Public Overridable Function AssignPrimaryKey() As Integer
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
                    FileReader.ReadLine()
                End While
            End Using
            Return MaxPrimaryKey + 1
        Else
            Return 0
        End If
    End Function
End Class
