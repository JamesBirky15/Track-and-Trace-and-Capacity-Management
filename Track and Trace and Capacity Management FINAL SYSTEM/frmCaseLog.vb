Imports System.IO

Public Class frmCaseLog

    ' Exits back to the main menu
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        frmMenu.Show()
        Me.Close()
    End Sub

    ' Loads the list view with the data from the cases log

    Private Sub frmCaseLog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Encryptor As New Encryptor
        Try
            Using FileReader As New StreamReader(File.Open("Cases.txt", FileMode.Open))
                While FileReader.EndOfStream = False
                    lstCaseLog.Items.Add(Encryptor.DecryptData(FileReader.ReadLine()))
                End While
            End Using
        Catch
            MessageBox.Show("There are no cases stored", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class