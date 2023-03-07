Imports System.Text

Namespace org.apache.fontbox.cff

    '/**
    ' * This class contains some helper methods handling Type1-Fonts.
    ' *
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class Type1FontUtil

        Private Sub New()
        End Sub

        '/**
        ' * Converts a byte-array into a string with the corresponding hex value. 
        ' * @param bytes the byte array
        ' * @return the string with the hex value
        ' */
        Public Shared Function hexEncode(ByVal bytes() As Byte) As String
            Dim sb As New StringBuilder()
            For i As Integer = 0 To bytes.Length - 1
                Dim [string] As String = NInteger.toHexString(bytes(i) And &HFF)
                If ([string].Length = 1) Then
                    sb.append("0")
                End If
                sb.append([string].ToUpper())
            Next
            Return sb.toString()
        End Function

        '/**
        ' * Converts a string representing a hex value into a byte array.
        ' * @param string the string representing the hex value
        ' * @return the hex value as byte array
        ' */
        Public Shared Function hexDecode(ByVal [string] As String) As Byte()
            If ([string].Length() Mod 2 <> 0) Then
                Throw New ArgumentException
            End If
            Dim bytes() As Byte = Array.CreateInstance(GetType(Byte), [string].Length() \ 2)
            For i As Integer = 0 To [string].Length() - 1 Step 2
                bytes(i \ 2) = NInteger.Parse([string].Substring(i, i + 2), 16)
            Next
            Return bytes
        End Function

        '/**
        ' * Encrypt eexec.
        ' * @param buffer the given data
        ' * @return the encrypted data
        ' */
        Public Shared Function eexecEncrypt(ByVal buffer() As Byte) As Byte()
            Return encrypt(buffer, 55665, 4)
        End Function

        '/**
        ' * Encrypt charstring.
        ' * @param buffer the given data
        ' * @param n blocksize?
        ' * @return the encrypted data
        ' */
        Public Shared Function charstringEncrypt(ByVal buffer() As Byte, ByVal n As Integer) As Byte()
            Return encrypt(buffer, 4330, n)
        End Function

        Private Shared Function encrypt(ByVal plaintextBytes() As Byte, ByVal r As Integer, ByVal n As Integer) As Byte()
            Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), plaintextBytes.Length + n)

            'For i As Integer = 0 To n - 1
            '    buffer(i) = 0
            'Next

            Array.Copy(plaintextBytes, 0, buffer, n, buffer.Length - n)

            Dim c1 As Integer = 52845
            Dim c2 As Integer = 22719

            Dim ciphertextBytes() As Byte = Array.CreateInstance(GetType(Byte), buffer.Length)

            For i As Integer = 0 To buffer.Length - 1
                Dim plain As Integer = buffer(i) And &HFF
                Dim cipher As Integer = plain Xor r >> 8

                ciphertextBytes(i) = cipher

                r = (cipher + r) * c1 + c2 And &HFFFF
            Next

            Return ciphertextBytes
        End Function

        '/**
        ' * Decrypt eexec.
        ' * @param buffer the given encrypted data
        ' * @return the decrypted data
        ' */
        Public Shared Function eexecDecrypt(ByVal buffer() As Byte) As Byte()
            Return decrypt(buffer, 55665, 4)
        End Function

        '/**
        ' * Decrypt charstring.
        ' * @param buffer the given encrypted data
        ' * @param n blocksize?
        ' * @return the decrypted data
        ' */
        Public Shared Function charstringDecrypt(ByVal buffer() As Byte, ByVal n As Integer) As Byte()
            Return decrypt(buffer, 4330, n)
        End Function

        Private Shared Function decrypt(ByVal ciphertextBytes() As Byte, ByVal r As Integer, ByVal n As Integer) As Byte()
            Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), ciphertextBytes.Length)

            Dim c1 As Integer = 52845
            Dim c2 As Integer = 22719

            For i As Integer = 0 To ciphertextBytes.Length - 1
                Dim cipher As Integer = ciphertextBytes(i) And &HFF
                Dim plain As Integer = cipher Xor r >> 8

                buffer(i) = plain

                r = (cipher + r) * c1 + c2 & &HFFFF
            Next

            Dim plaintextBytes() As Byte = Array.CreateInstance(GetType(Byte), ciphertextBytes.Length - n)
            Array.Copy(buffer, n, plaintextBytes, 0, plaintextBytes.Length)

            Return plaintextBytes
        End Function


    End Class

End Namespace