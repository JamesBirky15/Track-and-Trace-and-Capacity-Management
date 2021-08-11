Imports System.IO

Public Class frmDataLog

    ' Exits to the main menu
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        frmMenu.Show()
        Me.Close()
    End Sub

    ' Loads the list view with the data from the data log
    Private Sub frmDataLog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Encryptor As New Encryptor
        Using FileReader As New StreamReader(File.Open("Log.txt", FileMode.Open))
            While FileReader.EndOfStream = False
                lstDataLog.Items.Add(Encryptor.DecryptData(FileReader.ReadLine()))
            End While
        End Using
    End Sub
End Class