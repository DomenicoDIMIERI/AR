Imports DMD
Imports DMD.Sistema
Imports DMD.CustomerCalls
Imports DMD.Anagrafica
Imports DMD.Databases

Partial Class CustomerCalls

    <Serializable>
    Public Class CRMFindFilter
        Implements DMD.XML.IDMDXMLSerializable

        Public ID As Integer
        Public Nominativo As String
        Public TipoOggetto As String
        Public IDPuntoOperativo As Integer
        Public IDOperatore As Integer
        Public IDPersona As Integer
        Public Flags As Integer
        Public Numero As String
        Public Contenuto As String
        Public Etichetta As String
        Public Dal As Nullable(Of Date)
        Public Al As Nullable(Of Date)
        Public Scopo As String
        Public Esito As Nullable(Of Integer)
        Public DettaglioEsito As String
        Public StatoConversazione As Nullable(Of Integer)
        Public IDContesto As Nullable(Of Integer)
        Public TipoContesto As String
        Public nMax As Integer?
        Public IgnoreRights As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.ID = 0
            Me.Nominativo = ""
            Me.TipoOggetto = ""
            Me.IDPuntoOperativo = 0
            Me.IDOperatore = 0
            Me.IDPersona = 0
            Me.Flags = 0
            Me.Numero = ""
            Me.Contenuto = ""
            Me.Etichetta = ""
            Me.Dal = Nothing
            Me.Al = Nothing
            Me.Scopo = ""
            Me.Esito = Nothing
            Me.DettaglioEsito = ""
            Me.StatoConversazione = Nothing
            Me.IDContesto = Nothing
            Me.TipoContesto = ""
            Me.nMax = Nothing
            Me.IgnoreRights = False
        End Sub


        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Nominativo" : Me.Nominativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoOggetto" : Me.TipoOggetto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPuntoOperativo" : Me.IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPersona" : Me.IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Esito" : Me.Esito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoConversazione" : Me.StatoConversazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Numero" : Me.Numero = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Contenuto" : Me.Contenuto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioEsito" : Me.DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Etichetta" : Me.Etichetta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Dal" : Me.Dal = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Al" : Me.Al = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoContesto" : Me.TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContesto" : Me.IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "nMax" : Me.nMax = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IgnoreRights" : Me.IgnoreRights = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ID" : Me.ID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Scopo" : Me.Scopo = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Nominativo", Me.Nominativo)
            writer.WriteAttribute("TipoOggetto", Me.TipoOggetto)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("Flags", Me.Flags)
            writer.WriteAttribute("Numero", Me.Numero)
            writer.WriteAttribute("Contenuto", Me.Contenuto)
            writer.WriteAttribute("Etichetta", Me.Etichetta)
            writer.WriteAttribute("Dal", Me.Dal)
            writer.WriteAttribute("Al", Me.Al)
            writer.WriteAttribute("Esito", Me.Esito)
            writer.WriteAttribute("DettaglioEsito", Me.DettaglioEsito)
            writer.WriteAttribute("StatoConversazione", Me.StatoConversazione)
            writer.WriteAttribute("IDContesto", Me.IDContesto)
            writer.WriteAttribute("TipoContesto", Me.TipoContesto)
            writer.WriteAttribute("nMax", Me.nMax)
            writer.WriteAttribute("IgnoreRights", Me.nMax)
            writer.WriteAttribute("ID", Me.ID)
            writer.WriteAttribute("Scopo", Me.Scopo)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class