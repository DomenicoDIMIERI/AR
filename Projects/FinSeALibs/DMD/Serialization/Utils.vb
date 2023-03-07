Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.Sistema

Namespace Serializable
    Public NotInheritable Class Utils
        Private Sub New()
        End Sub

        Public Shared ReadOnly Serializer As New BinarySerializer


        Public Shared Function XMLTypeName(ByVal obj As Object) As String
            Dim elemType As String = Sistema.Types.vbTypeName(obj)
            Dim i As Integer = InStr(elemType, "(Of ")
            If (i > 0) Then elemType = Left(elemType, i - 1)
            Return elemType
        End Function

    End Class
End Namespace
