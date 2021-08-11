
Imports System.Security.Cryptography
Public Class Encryptor

	' The encryption system uses a caesar cipher to encrypt and decrypt data in a file

	' Shifts each character of the plain text by 3 to the right to encrypt plaintext to ciphertext
	Public Function EncryptData(ByVal plainText As String) As String

		Dim cipherText As String = ""

		For i = 0 To plainText.Length - 1
			Dim plainAsciiNum = Asc(plainText(i))
			Dim shiftedAsciiNum = plainAsciiNum + 3
			Dim shiftedChar = Chr(shiftedAsciiNum)
			cipherText = cipherText + shiftedChar
		Next
		Return cipherText
	End Function

	' Shifts each character of the plain text by 3 to the left to decrypt the ciphertext back to its plaintext format
	Public Function DecryptData(ByVal plainText As String)

		Dim cipherText As String = ""

		For i = 0 To plainText.Length - 1
			Dim plainAsciiNum = Asc(plainText(i))
			Dim shiftedAsciiNum = plainAsciiNum - 3
			Dim shiftedChar = Chr(shiftedAsciiNum)
			cipherText = cipherText + shiftedChar
		Next
		Return cipherText
	End Function
End Class
