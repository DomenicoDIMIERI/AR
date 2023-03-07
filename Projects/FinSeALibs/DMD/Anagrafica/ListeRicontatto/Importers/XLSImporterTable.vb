Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports System
Imports DMD.Net.Mail
Imports DMD.Anagrafica
Imports DMD.XML

Partial Public Class Anagrafica

    <Serializable>
    Public Class XLSImporterTable
        Implements DMD.XML.IDMDXMLSerializable

        Public Name As String
        Public Columns As CCollection(Of XLSImporterColumn)

        Public Sub New()
            Me.Name = ""
            Me.Columns = New CCollection(Of XLSImporterColumn)
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Name", Me.Name)
            writer.WriteTag("Columns", Me.Columns)
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Name" : Me.Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Columns" : Me.Columns.AddRange(fieldValue)
            End Select
        End Sub
    End Class


End Class
