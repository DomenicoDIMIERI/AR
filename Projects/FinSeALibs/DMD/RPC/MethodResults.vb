Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net

Partial Public Class Sistema

    Public NotInheritable Class MethodResults
        Implements DMD.XML.IDMDXMLSerializable

        Public errorMessage As String
        Public results As Object

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal result As Object)
            Me.New
            Me.errorMessage = vbNullString
            Me.results = result
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "errorMessage" : Me.errorMessage = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "results" : Me.results = fieldValue
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("errorMessage", Me.errorMessage)
            writer.WriteTag("results", Me.results)
        End Sub
    End Class

End Class