Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls


    <Serializable> _
    Public Class StoricoAction
        Implements DMD.XML.IDMDXMLSerializable, IComparable

        Public Data As Nullable(Of Date)
        Public IDOperatore As Integer
        Public NomeOperatore As String
        Public IDCliente As Integer
        Public NomeCliente As String
        Public Note As String
        Public Scopo As String
        Public NumeroOIndirizzo As String
        Public Esito As EsitoChiamata
        Public DettaglioEsito As String
        Public Durata As Double
        Public Attesa As Double
        Private m_Tag As Object
        Public StatoConversazione As StatoConversazione
        Public ActionID As Integer
        Public ActionType As String
        Public ActionSubID As Integer
        Public Ricevuta As Boolean
        Private m_AttachmentID As Integer
        Private m_Attachment As CAttachment

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Property Tag As Object
            Get
                Return Me.m_Tag
            End Get
            Set(value As Object)
                Me.ActionID = GetID(value)
                Me.ActionType = IIf(value Is Nothing, "", TypeName(value))
                Me.m_Tag = value
            End Set
        End Property

        Public Property AttachmentID As Integer
            Get
                Return GetID(Me.m_Attachment, Me.m_AttachmentID)
            End Get
            Set(value As Integer)
                Me.m_AttachmentID = value
                Me.m_Attachment = Nothing
            End Set
        End Property

        Public Property Attachment As CAttachment
            Get
                If (Me.m_Attachment Is Nothing) Then Me.m_Attachment = Sistema.Attachments.GetItemById(Me.m_AttachmentID)
                Return Me.m_Attachment
            End Get
            Set(value As CAttachment)
                Me.m_Attachment = value
                Me.m_AttachmentID = GetID(value)
            End Set
        End Property

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Data" : Me.Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCliente" : Me.IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Scopo" : Me.Scopo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroOIndirizzo" : Me.NumeroOIndirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Esito" : Me.Esito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioEsito" : Me.DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Durata" : Me.Durata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Attesa" : Me.Attesa = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "StatoConversazione" : Me.StatoConversazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ActionID" : Me.ActionID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ActionType" : Me.ActionType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ActionSubID" : Me.ActionSubID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Ricevuta" : Me.Ricevuta = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Tag" : Me.Tag = fieldValue
                Case "AttachmentID" : Me.m_AttachmentID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attachment" : Me.m_Attachment = fieldValue
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Data", Me.Data)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.NomeOperatore)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.NomeCliente)
            writer.WriteAttribute("Scopo", Me.Scopo)
            writer.WriteAttribute("NumeroOIndirizzo", Me.NumeroOIndirizzo)
            writer.WriteAttribute("Esito", Me.Esito)
            writer.WriteAttribute("DettaglioEsito", Me.DettaglioEsito)
            writer.WriteAttribute("Durata", Me.Durata)
            writer.WriteAttribute("Attesa", Me.Attesa)
            writer.WriteAttribute("StatoConversazione", Me.StatoConversazione)
            writer.WriteAttribute("ActionID", Me.ActionID)
            writer.WriteAttribute("ActionType", Me.ActionType)
            writer.WriteAttribute("ActionSubID", Me.ActionSubID)
            writer.WriteAttribute("Ricevuta", Me.Ricevuta)
            writer.WriteAttribute("AttachmentID", Me.AttachmentID)
            writer.WriteTag("Note", Me.Note)
            'writer.WriteTag("Tag", Me.Tag)
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim other As StoricoAction = obj
            Dim ret As Integer = -DateUtils.Compare(Me.Data, other.Data)
            If (ret = 0) Then ret = Strings.Compare(Me.NomeCliente, other.NomeCliente, CompareMethod.Text)
            Return ret
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class