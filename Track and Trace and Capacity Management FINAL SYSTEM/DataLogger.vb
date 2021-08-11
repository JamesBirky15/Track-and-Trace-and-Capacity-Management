Imports System.IO

Public Class DataLogger

    Private User As String
    Private Action As String
    Private Time As Date
    Private Filename As String

    ' Writes to the transaction log log in the form "User, Action, at Time"
    ' eg "JamesBirky15 added new user at 10:45 on 7/01/2021"
    Sub WriteTransaction()
        Filename = "Log.txt"
        Dim Encryptor As New Encryptor
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            FileWriter.WriteLine(Encryptor.EncryptData(User & " " & Action & " at " & Time.ToString))
        End Using
    End Sub

    ' Writes a positive case to the cases log in form "Positive case was recorded in StaffMember/Customer ID Name at Time)
    Sub WritePositiveCase(ByVal CaseID As String, ByVal CaseName As String, ByVal Type As String)
        Dim Encryptor As New Encryptor
        Filename = "Cases.txt"
        Using FileWriter As New StreamWriter(File.Open(Filename, FileMode.Append))
            FileWriter.WriteLine(Encryptor.EncryptData("Positive case was recorded in " & (Type & " " & CaseID.ToString & "  (" & CaseName & ") at " & Time.ToString)))
        End Using
    End Sub

    ' Used to instantiate a new object by assigning values to the user, action, and time variables
    Sub New(ByVal pUser As String, ByVal pAction As String, ByVal pTime As Date)
        User = pUser
        Action = pAction
        Time = pTime
    End Sub
End Class
