Imports System.IO 'Inputs the required libraries to perform file IO operations

Public Class AuthenticationManager

    Inherits IOManager

    Private ID As String
    Public Admin As Boolean
    Public Username As String
    Private Password As String

    Public Filename As String = "details.txt" 'Stores the name of the file which will be used to store the username/password data so it can be accessed by the methods

    'Checks that the details entered by the user are valid by going through the details binary file using a do loop and seeing if the details entered are stored. If they are,
    ' the user will be granted access as the function returns true. Otherwise, the function returns false as seen under Else.
    ' If the user has admin permissions, shown by "Admin" in the text file, the user will be granted admin permissions allowing them to add new users
    Function CheckDetails()
        Dim Encryptor As New Encryptor
        Dim CurrentID As String
        Dim CurrentUsername As String
        Dim CurrentPassword As String
        Dim CurrentAdminDetails As String
        Dim DetailsCorrect As Boolean = False
        Using DetailsReader As New StreamReader(File.Open(Filename, FileMode.Open))
            Do
                CurrentID = DetailsReader.ReadLine()
                CurrentAdminDetails = DetailsReader.ReadLine()
                CurrentAdminDetails = Encryptor.DecryptData(CurrentAdminDetails)
                CurrentID = Encryptor.DecryptData(CurrentAdminDetails)
                CurrentUsername = DetailsReader.ReadLine()
                CurrentUsername = Encryptor.DecryptData(CurrentUsername)
                CurrentPassword = DetailsReader.ReadLine()
                CurrentPassword = Encryptor.DecryptData(CurrentPassword)
                If CurrentUsername = Username And CurrentPassword = Password Then
                    DetailsCorrect = True
                    If CurrentAdminDetails = "Admin" Then
                        Admin = True
                    Else
                        Admin = False
                    End If
                End If
            Loop Until DetailsReader.EndOfStream = True Or DetailsCorrect = True
        End Using
        If DetailsCorrect = True Then
            Return True
        Else
            Return False
        End If
    End Function


    ' Uses the VB StreamWriter library to allow admins to write new authentication details to the user details file. It will write the username, password, and wether or not they are an admin
    Overrides Sub WriteData()
        Dim Encryptor As New Encryptor
        ID = AssignPrimaryKey()
        Using DetailsWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            DetailsWriter.WriteLine(ID)
            If Admin = True Then
                DetailsWriter.WriteLine(Encryptor.EncryptData("Admin"))
            Else
                DetailsWriter.WriteLine(Encryptor.EncryptData("Not Admin"))
            End If
            Username = Encryptor.EncryptData(Username)
            DetailsWriter.WriteLine(Username)
            Password = Encryptor.EncryptData(Password)
            DetailsWriter.WriteLine(Password)
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " added new user " & ID, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' This method first populates the transaction array. The user selects the record they want to delete, and the system searches for the record in the transaction array before deleting it. 
    ' The file is then rewritten without the data which the user requested to delete
    Overrides Sub DeleteData()
        Dim Count As Integer = 0
        Dim CurrentItem As String
        PopulateTransactionArray()
        Do
            CurrentItem = TransactionArray(Count)
            If CurrentItem <> ID Then
                Count = Count + 4
            End If
        Loop Until CurrentItem = ID
        TransactionArray.RemoveRange(Count, 4)
        System.IO.File.Delete(Filename)
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            For Count = 0 To TransactionArray.Count - 1
                FileWriter.WriteLine(TransactionArray(Count))
            Next
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " deleted user " & ID, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' This method first populates the transaction array. The user selects the record they want to edit, and the system searches for the record in the transaction array before replacing it with the new data entered by the user. 
    ' The file is then rewritten with the edited data
    Public Overrides Sub EditData()
        Dim Encryptor As New Encryptor
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
        If Admin = True Then
            TransactionArray(Count + 1) = Encryptor.EncryptData("Admin")
        ElseIf Admin = False Then
            TransactionArray(Count + 1) = Encryptor.EncryptData("Not Admin")
        End If
        TransactionArray(Count + 2) = Encryptor.EncryptData(Username)
        TransactionArray(Count + 3) = Encryptor.EncryptData(Password)
        System.IO.File.Delete(Filename)
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            For Count = 0 To TransactionArray.Count - 1
                FileWriter.WriteLine(TransactionArray(Count))
            Next
        End Using
        Dim DataLogger As New DataLogger(frmLogin.txtUsername.Text, " edited user " & ID, System.DateTime.Now)
        DataLogger.WriteTransaction()
    End Sub

    ' Populates the transaction array with the data from the details master file.
    Sub PopulateTransactionArray()
        Using FileReader As New StreamReader(File.Open(Filename, FileMode.Open))
            Dim Encryptor As New Encryptor
            While FileReader.EndOfStream = False
                Dim CurrentItem As String
                CurrentItem = FileReader.ReadLine()
                Encryptor.DecryptData(CurrentItem)
                TransactionArray.Add(CurrentItem)
            End While
        End Using
    End Sub

    ' Assigns a primary key to new records
    Public Overrides Function AssignPrimaryKey() As Integer
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

    ' This subroutine is used to instantiate a new authentication manager object
    ' If a null value is entered as a parameter, a new primary key is assigned. Otherwise, the primary key parameter is entered as the ID attribute of the object
    ' This is also used to tell the system whether the user being instantiated is a user
    Sub New(ByVal pID As String, ByVal pAdmin As String, ByVal pUsername As String, ByVal pPassword As String)
        If pID = -1 Then
            AssignPrimaryKey()
        Else
            ID = pID
        End If
        If pAdmin = "Admin" Then
            Admin = True
        Else
            Admin = False
        End If
        Username = pUsername
        Password = pPassword
    End Sub
End Class
