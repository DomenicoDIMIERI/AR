Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.Sistema

Namespace XML

    Public Enum XMLSerializeMethod As Integer
        Document = 0
        None = 1
    End Enum

    Public NotInheritable Class Utils
        Private Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Shared ReadOnly Serializer As New XMLSerializer


        Public Shared Function XMLTypeName(ByVal obj As Object) As String
            Dim elemType As String = Sistema.Types.vbTypeName(obj)
            Dim i As Integer = InStr(elemType, "(Of ")
            If (i > 0) Then elemType = Left(elemType, i - 1)
            Return elemType
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class
End Namespace
