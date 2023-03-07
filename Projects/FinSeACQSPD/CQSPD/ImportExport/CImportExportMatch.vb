Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

      
    ''' <summary>
    ''' Rappresenta una corrispondenza tra i due sistemi
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CImportExportMatch
        Implements XML.IDMDXMLSerializable

        Public Tipo As String
        Public IDOrigine As Integer
        Public IDDestinazione As Integer


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Tipo = ""
            Me.IDOrigine = 0
            Me.IDDestinazione = 0
        End Sub


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Tipo" : Me.Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOrigine" : Me.IDOrigine = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDestinazione" : Me.IDDestinazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Tipo", Me.Tipo)
            writer.WriteAttribute("IDOrigine", Me.IDOrigine)
            writer.WriteAttribute("IDDestinazione", Me.IDDestinazione)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class





End Class