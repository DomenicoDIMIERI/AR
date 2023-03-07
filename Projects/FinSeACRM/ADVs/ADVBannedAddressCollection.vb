Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports System.Text.RegularExpressions


Partial Public Class ADV

    Public NotInheritable Class ADVBannedAddressCollection
        Inherits CKeyCollection(Of String)

        Public Sub New()
        End Sub

        Public Shadows Function Add(ByVal bannedAddress As String) As String ' System.Text.RegularExpressions.Regex
            'Dim item As New System.Text.RegularExpressions.Regex(bannedAddress)
            Dim item As String = bannedAddress
            MyBase.Add(bannedAddress, item)
            Return item
        End Function

        ''' <summary>
        ''' Restitusice vero se l'indirizzo o il numero è "bannato" cioè se il test con una delle espressioni contenute in questo elenco è vero
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsBanned(ByVal value As String) As Boolean
            For Each reg As String In Me
                If value Like reg Then Return True
            Next
            Return False
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteTag("keys", Me.Keys.ToArray)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "keys"
                    Me.Clear()
                    Dim keys() As String = Arrays.Convert(Of String)(fieldValue)
                    If (keys IsNot Nothing) Then
                        For Each s As String In keys
                            If (Trim(s) <> "") Then Me.Add(s)
                        Next
                    End If
            End Select
        End Sub

        Friend Sub Read(ByVal reader As DBReader, ByVal fieldName As String)
            Me.Clear()
            Dim str As String = ""
            str = reader.Read(fieldName, str)
            str = Trim(str)
            If (str <> "") Then
                For Each s As String In Split(str, ";")
                    If (Trim(s) <> "") Then Me.Add(s)
                Next
            End If
        End Sub

        Friend Sub Write(ByVal writer As DBWriter, ByVal fieldName As String)
            Dim buffer As New System.Text.StringBuilder
            For Each str As String In Me.Keys
                If (str <> "") Then
                    If buffer.Length > 0 Then buffer.Append(";")
                    buffer.Append(str)
                End If
            Next
            writer.Write(fieldName, buffer.ToString)
        End Sub

    End Class


End Class
