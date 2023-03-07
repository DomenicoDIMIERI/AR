Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    ''' <summary>
    ''' Stati di una commissione
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StatoEstrattoContributivo As Integer
        DaRichiedere = 0

        Richiesto = 1

        Assegnato = 2

        Evaso = 3

        Sospeso = -1

        Errore = 255
    End Enum

    ''' <summary>
    ''' Rappresenta una richiesta di estratto contributivo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class EstrattoContributivo
        Inherits DBObjectPO

        Private m_Richiedente As CUser                  'Operatore che ha richiesto l'estratto
        Private m_IDRichiedente As Integer              'ID dell'operatore che ha richiesto l'estratto
        Private m_NomeRichiedente As String             'Nome dell'operatore che ha richiesto l'estratto
        Private m_DataRichiesta As Nullable(Of Date)    'Data ed ora della richiesta
        Private m_DataAssegnazione As Nullable(Of Date) 'Data ed ora di presa in carico della richiesta
        Private m_AssegnatoA As CUser                   'Utente che ha preso in carico la richiesta
        Private m_IDAssegnatoA As Integer               'ID dell'utente che ha preso in carico la richiesta
        Private m_NomeAssegnatoA As String              'Nome dell'utente che ha preso in carico la richiesta
        Private m_StatoRichiesta As StatoEstrattoContributivo 'Stato di lavorazione
        Private m_DataCompletamento As Nullable(Of Date) 'Data ed ora di completamento della richiesta (o di annullamento)
        Private m_IDCliente As Integer                   'ID del cliente per cui è stata fatta la richiesta
        Private m_Cliente As CPersonaFisica             'Cliente per cui è stata fatta la richiesta
        Private m_NomeCliente As String                 'Nome del cliente per cui è stata fatta la richiesta
        Private m_IDAmministrazione As Integer          'ID dell'amministrazione a cui è stata fatta la richiesta
        Private m_Amministrazione As CAzienda           'Amministrazione a cui è stata fatta la richiesta
        Private m_NomeAmministrazione As String         'Nome dell'amministrazione a cui è stata fatta la richiesta

        Private m_IDDelega As Integer                   'ID dell'allegato relativo alla delega del cliente
        Private m_Delega As CAttachment
        Private m_IDDocumentoRiconoscimento As Integer  'ID dell'allegato contenente il documento di riconoscimento
        Private m_DocumentoRiconoscimento As CAttachment
        Private m_IDCodiceFiscale As Integer            'ID dell'allegato relativo al codice fiscale del cliente
        Private m_CodiceFiscale As CAttachment

        'Private m_IDAllegato As Integer                 'ID dell'allegato prodotto come risposta
        'Private m_Allegato As CAttachment               'Allegato prodotto come risposta
        Private m_Messages As CAnnotazioni
        Private m_Allegati As CAttachments
        Private m_Note As String

        Private m_Source As Object          'Oggetto che ha generato la richiesta (verrà informato in caso delle modifiche)
        Private m_SourceType As String      'Tipo dell'oggetto che ha generato la richiesta
        Private m_SourceID As Integer       'ID dell'oggetto che ha generato la richiesta

        Public Sub New()
            Me.m_Richiedente = Nothing
            Me.m_IDRichiedente = 0
            Me.m_NomeRichiedente = vbNullString
            Me.m_DataRichiesta = Nothing
            Me.m_AssegnatoA = Nothing
            Me.m_IDAssegnatoA = 0
            Me.m_NomeAssegnatoA = vbNullString
            Me.m_DataAssegnazione = Nothing
            Me.m_StatoRichiesta = StatoEstrattoContributivo.DaRichiedere
            Me.m_DataCompletamento = Nothing
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = vbNullString
            Me.m_IDAmministrazione = 0
            Me.m_Amministrazione = Nothing
            Me.m_NomeAmministrazione = vbNullString
            Me.m_IDDelega = 0
            Me.m_Delega = Nothing
            Me.m_IDDocumentoRiconoscimento = 0
            Me.m_DocumentoRiconoscimento = Nothing
            Me.m_IDCodiceFiscale = 0
            Me.m_CodiceFiscale = Nothing
            'Me.m_IDAllegato = 0
            'Me.m_Allegato = Nothing
            Me.m_Messages = Nothing
            Me.m_Allegati = Nothing
            Me.m_Note = vbNullString
            Me.m_Source = Nothing
            Me.m_SourceID = 0
            Me.m_SourceType = ""
        End Sub

        Public Property Source As Object
            Get
                If (Me.m_Source Is Nothing AndAlso Me.m_SourceType <> "" AndAlso Me.m_SourceID <> 0) Then Me.m_Source = Sistema.Types.GetItemByTypeAndId(Me.m_SourceType, Me.m_SourceID)
                Return Me.m_Source
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                Me.m_SourceType = TypeName(value)
                Me.m_SourceID = GetID(value)
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property

        Public Property SourceType As String
            Get
                If (Me.m_Source IsNot Nothing) Then Return TypeName(Me.m_Source)
                Return Me.m_SourceType
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.SourceType
                If (oldValue = value) Then Exit Property
                Me.m_SourceType = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceType", value, oldValue)
            End Set
        End Property

        Public Property SourceID As Integer
            Get
                Return GetID(Me.m_Source, Me.m_SourceID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property


        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Property Delega As CAttachment
            Get
                If (Me.m_Delega Is Nothing) Then Me.m_Delega = Attachments.GetItemById(Me.m_IDDelega)
                Return Me.m_Delega
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Delega
                If (oldValue Is value) Then Exit Property
                Me.m_Delega = value
                Me.m_IDDelega = GetID(value)
                Me.DoChanged("Delega", value, oldValue)
            End Set
        End Property

        Public Property IDDelega As Integer
            Get
                Return GetID(Me.m_Delega, Me.m_IDDelega)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDelega
                If (oldValue = value) Then Exit Property
                Me.m_IDDelega = value
                Me.m_Delega = Nothing
                Me.DoChanged("IDDelega", value, oldValue)
            End Set
        End Property

        Public Property DocumentoRiconoscimento As CAttachment
            Get
                If (Me.m_DocumentoRiconoscimento Is Nothing) Then Me.m_DocumentoRiconoscimento = Attachments.GetItemById(Me.m_IDDocumentoRiconoscimento)
                Return Me.m_DocumentoRiconoscimento
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_DocumentoRiconoscimento
                If (oldValue Is value) Then Exit Property
                Me.m_DocumentoRiconoscimento = value
                Me.m_IDDocumentoRiconoscimento = GetID(value)
                Me.DoChanged("DocumentoRiconoscimento", value, oldValue)
            End Set
        End Property

        Public Property IDDocumentoRiconoscimento As Integer
            Get
                Return GetID(Me.m_DocumentoRiconoscimento, Me.m_IDDocumentoRiconoscimento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDocumentoRiconoscimento
                If (oldValue = value) Then Exit Property
                Me.m_IDDocumentoRiconoscimento = value
                Me.m_DocumentoRiconoscimento = Nothing
                Me.DoChanged("IDDocumentoRiconoscimento", value, oldValue)
            End Set
        End Property

        Public Property IDCodiceFiscale As Integer
            Get
                Return GetID(Me.m_CodiceFiscale, Me.m_IDCodiceFiscale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCodiceFiscale
                If (oldValue = value) Then Exit Property
                Me.m_IDCodiceFiscale = value
                Me.m_CodiceFiscale = Nothing
                Me.DoChanged("IDCodiceFiscale", value, oldValue)
            End Set
        End Property

        Public Property CodiceFiscale As CAttachment
            Get
                If (Me.m_CodiceFiscale Is Nothing) Then Me.m_CodiceFiscale = Attachments.GetItemById(Me.m_IDCodiceFiscale)
                Return Me.m_CodiceFiscale
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_CodiceFiscale
                If (oldValue Is value) Then Exit Property
                Me.m_CodiceFiscale = value
                Me.m_IDCodiceFiscale = GetID(value)
                Me.DoChanged("CodiceFiscale", value, oldValue)
            End Set
        End Property

        'Public Property Allegato As CAttachment
        '    Get
        '        If (Me.m_Allegato Is Nothing) Then Me.m_Allegato = Attachments.GetItemById(Me.m_IDAllegato)
        '        Return Me.m_Allegato
        '    End Get
        '    Set(value As CAttachment)
        '        Dim oldValue As CAttachment = Me.m_Allegato
        '        If (oldValue Is value) Then Exit Property
        '        Me.m_Allegato = value
        '        Me.m_IDAllegato = GetID(value)
        '        Me.DoChanged("Allegato", value, oldValue)
        '    End Set
        'End Property

        'Public Property IDAllegato As Integer
        '    Get
        '        Return GetID(Me.m_Allegato, Me.m_IDAllegato)
        '    End Get
        '    Set(value As Integer)
        '        Dim oldValue As Integer = Me.IDAllegato
        '        If (oldValue = value) Then Exit Property
        '        Me.m_IDAllegato = value
        '        Me.m_Allegato = Nothing
        '        Me.DoChanged("IDAllegato", value, oldValue)
        '    End Set
        'End Property

        Public ReadOnly Property Allegati As CAttachments
            Get
                If (Me.m_Allegati Is Nothing) Then Me.m_Allegati = New CAttachments(Me)
                Return Me.m_Allegati
            End Get
        End Property

        Public ReadOnly Property Messaggi As CAnnotazioni
            Get
                If (Me.m_Messages Is Nothing) Then Me.m_Messages = New CAnnotazioni(Me)
                Return Me.m_Messages
            End Get
        End Property

        Public Property Richiedente As CUser
            Get
                If (Me.m_Richiedente Is Nothing) Then Me.m_Richiedente = Sistema.Users.GetItemById(Me.m_IDRichiedente)
                Return Me.m_Richiedente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_Richiedente
                If (oldValue Is value) Then Exit Property
                Me.m_Richiedente = value
                Me.m_IDRichiedente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeRichiedente = value.Nominativo
                Me.DoChanged("Richiedente", value, oldValue)
            End Set
        End Property

        Public Property IDRichiedente As Integer
            Get
                Return GetID(Me.m_Richiedente, Me.m_IDRichiedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiedente = value
                Me.m_Richiedente = Nothing
                Me.DoChanged("IDRichiedente", value, oldValue)
            End Set
        End Property

        Public Property NomeRichiedente As String
            Get
                Return Me.m_NomeRichiedente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_NomeRichiedente = value
                Me.DoChanged("NomeRichiedente", value, oldValue)
            End Set
        End Property

        Public Property DataRichiesta As Nullable(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        Public Property AssegnatoA As CUser
            Get
                If (Me.m_AssegnatoA Is Nothing) Then Me.m_AssegnatoA = Sistema.Users.GetItemById(Me.m_IDAssegnatoA)
                Return Me.m_AssegnatoA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_AssegnatoA
                If (oldValue Is value) Then Exit Property
                Me.m_AssegnatoA = value
                If (value IsNot Nothing) Then Me.m_NomeAssegnatoA = value.Nominativo
                Me.DoChanged("AssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property IDAssegnatoA As Integer
            Get
                Return GetID(Me.m_AssegnatoA, Me.m_IDAssegnatoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_IDAssegnatoA = value
                Me.m_AssegnatoA = Nothing
                Me.DoChanged("IDAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property NomeAssegnatoA As String
            Get
                Return Me.m_NomeAssegnatoA
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssegnatoA = value
                Me.DoChanged("NomeAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property DataAssegnazione As Nullable(Of Date)
            Get
                Return Me.m_DataAssegnazione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAssegnazione
                If (oldValue = value) Then Exit Property
                Me.m_DataAssegnazione = value
                Me.DoChanged("DataAssegnazione", value, oldValue)
            End Set
        End Property

        Public Property StatoRichiesta As StatoEstrattoContributivo
            Get
                Return Me.m_StatoRichiesta
            End Get
            Set(value As StatoEstrattoContributivo)
                Dim oldValue As StatoEstrattoContributivo = Me.m_StatoRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_StatoRichiesta = value
                Me.DoChanged("StatoRichiesta", value, oldValue)
            End Set
        End Property

        Public Property DataCompletamento As Nullable(Of Date)
            Get
                Return Me.m_DataCompletamento
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataCompletamento
                If (oldValue = value) Then Exit Property
                Me.m_DataCompletamento = value
                Me.DoChanged("DataCompletamento", value, oldValue)
            End Set
        End Property

        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        Public Property Cliente As CPersonaFisica
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        Public Property IDAmministrazione As Integer
            Get
                Return GetID(Me.m_Amministrazione, Me.m_IDAmministrazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAmministrazione
                If (oldValue = value) Then Exit Property
                Me.m_IDAmministrazione = value
                Me.m_Amministrazione = Nothing
                Me.DoChanged("IDAmministrazione", value, oldValue)
            End Set
        End Property

        Public Property Amministrazione As CAzienda
            Get
                If (Me.m_Amministrazione Is Nothing) Then Me.m_Amministrazione = Anagrafica.Aziende.GetItemById(Me.m_IDAmministrazione)
                Return Me.m_Amministrazione
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Amministrazione
                If (oldValue Is value) Then Exit Property
                Me.m_Amministrazione = value
                If (value IsNot Nothing) Then Me.m_NomeAmministrazione = value.Nominativo
                Me.m_IDAmministrazione = GetID(value)
                Me.DoChanged("Amministrazione", value, oldValue)
            End Set
        End Property

        Public Property NomeAmministrazione As String
            Get
                Return Me.m_NomeAmministrazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAmministrazione
                If (oldValue = value) Then Exit Property
                Me.m_NomeAmministrazione = value
                Me.DoChanged("NomeAmministrazione", value, oldValue)
            End Set
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return EstrattiContributivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeEstrattiC"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDRichiedente = reader.Read("IDRichiedente", Me.m_IDRichiedente)
            Me.m_NomeRichiedente = reader.Read("NomeRichiedente", Me.m_NomeRichiedente)
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)
            Me.m_IDAssegnatoA = reader.Read("IDAssegnatoA", Me.m_IDAssegnatoA)
            Me.m_NomeAssegnatoA = reader.Read("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            Me.m_DataAssegnazione = reader.Read("DataAssegnazione", Me.m_DataAssegnazione)
            Me.m_StatoRichiesta = reader.Read("StatoRichiesta", Me.m_StatoRichiesta)
            Me.m_DataCompletamento = reader.Read("DataCompletamento", Me.m_DataCompletamento)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_IDAmministrazione = reader.Read("IDAmministrazione", Me.m_IDAmministrazione)
            Me.m_NomeAmministrazione = reader.Read("NomeAmministrazione", Me.m_NomeAmministrazione)
            Me.m_IDDelega = reader.Read("IDDelega", Me.m_IDDelega)
            Me.m_IDDocumentoRiconoscimento = reader.Read("IDDocRic", Me.m_IDDocumentoRiconoscimento)
            Me.m_IDCodiceFiscale = reader.Read("IDCF", Me.m_IDCodiceFiscale)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_SourceType = reader.Read("SourceType", Me.m_SourceType)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            'Me.m_IDAllegato = reader.Read("IDAllegato", Me.m_IDAllegato)
            Try
                Me.m_Messages = XML.Utils.Serializer.Deserialize(reader.Read("Messaggi", ""))
            Catch ex As Exception
                Me.m_Messages = Nothing
            End Try

            Try
                Me.m_Allegati = XML.Utils.Serializer.Deserialize(reader.Read("Allegati", ""))
            Catch ex As Exception
                Me.m_Allegati = Nothing
            End Try

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDRichiedente", Me.IDRichiedente)
            writer.Write("NomeRichiedente", Me.m_NomeRichiedente)
            writer.Write("DataRichiesta", Me.m_DataRichiesta)
            writer.Write("IDAssegnatoA", Me.IDAssegnatoA)
            writer.Write("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.Write("DataAssegnazione", Me.m_DataAssegnazione)
            writer.Write("StatoRichiesta", Me.m_StatoRichiesta)
            writer.Write("DataCompletamento", Me.m_DataCompletamento)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("IDAmministrazione", Me.IDAmministrazione)
            writer.Write("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.Write("IDDelega", Me.IDDelega)
            writer.Write("IDDocRic", Me.IDDocumentoRiconoscimento)
            writer.Write("IDCF", Me.IDCodiceFiscale)
            'writer.Write("IDAllegato", Me.IDAllegato)
            writer.Write("Note", Me.m_Note)
            writer.Write("SourceType", Me.SourceType)
            writer.Write("SourceID", Me.SourceID)
            writer.Write("Messaggi", XML.Utils.Serializer.Serialize(Me.Messaggi))
            writer.Write("Allegati", XML.Utils.Serializer.Serialize(Me.Allegati))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDRichiedente", Me.IDRichiedente)
            writer.WriteAttribute("NomeRichiedente", Me.m_NomeRichiedente)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("IDAssegnatoA", Me.IDAssegnatoA)
            writer.WriteAttribute("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.WriteAttribute("DataAssegnazione", Me.m_DataAssegnazione)
            writer.WriteAttribute("StatoRichiesta", Me.m_StatoRichiesta)
            writer.WriteAttribute("DataCompletamento", Me.m_DataCompletamento)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IDAmministrazione", Me.IDAmministrazione)
            writer.WriteAttribute("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.WriteAttribute("IDDelega", Me.IDDelega)
            writer.WriteAttribute("IDDocRic", Me.IDDocumentoRiconoscimento)
            writer.WriteAttribute("IDCF", Me.IDCodiceFiscale)
            writer.WriteAttribute("SourceType", Me.SourceType)
            writer.WriteAttribute("SourceID", Me.SourceID)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Allegati", Me.Allegati)
            writer.WriteTag("Messaggi", Me.Messaggi)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDRichiedente" : Me.m_IDRichiedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRichiedente" : Me.m_NomeRichiedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDAssegnatoA" : Me.m_IDAssegnatoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAssegnatoA" : Me.m_NomeAssegnatoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAssegnazione" : Me.m_DataAssegnazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoRichiesta" : Me.m_StatoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataCompletamento" : Me.m_DataCompletamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAmministrazione" : Me.m_IDAmministrazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAmministrazione" : Me.m_NomeAmministrazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDelega" : Me.m_IDDelega = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDocRic" : Me.m_IDDocumentoRiconoscimento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCF" : Me.m_IDCodiceFiscale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Allegati" : Me.m_Allegati = fieldValue : Me.m_Allegati.SetOwner(Me)
                Case "Messaggi" : Me.m_Messages = fieldValue : Me.m_Messages.SetOwner(Me)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceType" : Me.m_SourceType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        ''' <summary>
        ''' Salva e richiede
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Richiedi()
            If (Me.StatoRichiesta <> StatoEstrattoContributivo.DaRichiedere) Then Throw New InvalidOperationException("Impossibile effettuare la richiesta in questo stato")
            Me.Richiedente = Users.CurrentUser
            Me.DataRichiesta = Now
            Me.StatoRichiesta = StatoEstrattoContributivo.Richiesto
            Me.Save()
            Me.OnRichiesta(New System.EventArgs)
        End Sub

        Protected Overridable Sub OnRichiesta(ByVal e As System.EventArgs)
            'RaiseEvent PresaInCarico(Me, e)
            Me.GetModule.DispatchEvent(New EventDescription("richiesta", "Richiesta dell'estratto contributivo ID= " & Me.ID, Me))
        End Sub

        ''' <summary>
        ''' Salva e prende in carico la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub PrendiInCarico()
            If (Me.StatoRichiesta <> StatoEstrattoContributivo.Richiesto) Then Throw New InvalidOperationException("Impossibile prendere in carico la richiesta in questo stato")
            Me.AssegnatoA = Users.CurrentUser
            Me.DataAssegnazione = Now
            Me.StatoRichiesta = StatoEstrattoContributivo.Assegnato
            Me.Save()
            Me.OnPresaInCarico(New System.EventArgs)
        End Sub

        Protected Overridable Sub OnPresaInCarico(ByVal e As System.EventArgs)
            'RaiseEvent PresaInCarico(Me, e)
            Me.GetModule.DispatchEvent(New EventDescription("presaincarico", "Presa in carico dell'estratto contributivo ID= " & Me.ID, Me))
        End Sub

        ''' <summary>
        ''' Salva ed evade
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Evadi()
            If (Me.StatoRichiesta <> StatoEstrattoContributivo.Assegnato AndAlso Me.StatoRichiesta <> StatoEstrattoContributivo.Sospeso) Then Throw New InvalidOperationException("Impossibile evadere la richiesta in questo stato")
            'If (Me.Allegati.Count = 0) Then Throw New InvalidOperationException("Impossibile evadere la richiesta senza allegare la documentazione")
            Me.DataCompletamento = Now
            Me.StatoRichiesta = StatoEstrattoContributivo.Evaso
            Me.Save()
            Me.OnEvaso(New System.EventArgs)
        End Sub

        Protected Overridable Sub OnEvaso(ByVal e As System.EventArgs)
            'RaiseEvent PresaInCarico(Me, e)
            Me.GetModule.DispatchEvent(New EventDescription("evaso", "Evaso l'estratto contributivo ID= " & Me.ID, Me))
        End Sub

        ''' <summary>
        ''' Salva ed errore
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Errore()
            If (Me.StatoRichiesta <> StatoEstrattoContributivo.Assegnato AndAlso Me.StatoRichiesta <> StatoEstrattoContributivo.Sospeso) Then Throw New InvalidOperationException("Impossibile evadere con errore la richiesta in questo stato")
            Me.DataCompletamento = Now
            Me.StatoRichiesta = StatoEstrattoContributivo.Errore
            Me.Save()
            Me.OnErrore(New System.EventArgs)
        End Sub

        Protected Overridable Sub OnErrore(ByVal e As System.EventArgs)
            'RaiseEvent PresaInCarico(Me, e)
            Me.GetModule.DispatchEvent(New EventDescription("errore", "Evaso con errore l'estratto contributivo ID= " & Me.ID, Me))
        End Sub

        Public Function AddMessage(ByVal message As String) As CAnnotazione
            Dim note As CAnnotazione = Me.Messaggi.Add(message)
            note.ForceUser(Sistema.Users.CurrentUser, DateUtils.Now)
            Me.Save(True)
            Me.GetModule.DispatchEvent(New EventDescription("message", "Comunicazione relativa alla richiesta di estratto ID= " & Me.ID, Me))
            Return note
        End Function

        Public Function AddAllegato(ByVal att As CAttachment) As CAttachment
            Me.Allegati.Add(att)
            att.ForceUser(Sistema.Users.CurrentUser, DateUtils.Now)
            Me.Save(True)
            Return att
        End Function

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Office.EstrattiContributivi.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Office.EstrattiContributivi.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            Office.EstrattiContributivi.doItemModified(New ItemEventArgs(Me))
        End Sub

        ''' <summary>
        ''' Sospende la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Sospendi()
            If (Me.StatoRichiesta <> StatoEstrattoContributivo.Assegnato) Then Throw New InvalidOperationException("Impossibile sospendere la richiesta in questo stato")
            Me.DataCompletamento = Now
            Me.StatoRichiesta = StatoEstrattoContributivo.Sospeso
            Me.Save()
            Me.OnSospeso(New System.EventArgs)
        End Sub

        Protected Overridable Sub OnSospeso(ByVal e As System.EventArgs)
            'RaiseEvent PresaInCarico(Me, e)
            Me.GetModule.DispatchEvent(New EventDescription("sospeso", "Sospeso l'estratto contributivo ID= " & Me.ID, Me))
        End Sub
    End Class



End Class