Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class Anagrafica

    Public Class CRMFinlterI
        Implements DMD.XML.IDMDXMLSerializable, ICloneable

        Public Tipo As String
        Public text As String
        Public nMax As Nullable(Of Integer)
        Public IntelliSearch As Boolean
        Public flags As Nullable(Of PFlags)
        Public ignoreRights As Boolean
        Public tipoPersona As Nullable(Of TipoPersona)
        Public IDPuntoOperativo As Integer
        Public DettaglioEsito As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Tipo = ""
            Me.text = ""
            Me.nMax = Nothing
            Me.IntelliSearch = True
            Me.flags = Nothing
            Me.ignoreRights = False
            Me.tipoPersona = Nothing
            Me.IDPuntoOperativo = 0
            Me.DettaglioEsito = ""
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Tipo" : Me.Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "text" : Me.text = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "nMax" : Me.nMax = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IntelliSearch" : Me.IntelliSearch = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "flags" : Me.flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ignoreRights" : Me.ignoreRights = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "tipoPersona" : Me.tipoPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPuntoOperativo" : Me.IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioEsito" : Me.DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Tipo", Me.Tipo)
            writer.WriteAttribute("text", Me.text)
            writer.WriteAttribute("nMax", Me.nMax)
            writer.WriteAttribute("IntelliSearch", Me.IntelliSearch)
            writer.WriteAttribute("flags", Me.flags)
            writer.WriteAttribute("ignoreRights", Me.ignoreRights)
            writer.WriteAttribute("tipoPersona", Me.tipoPersona)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("DettaglioEsito", Me.DettaglioEsito)
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class