Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office

    Public Enum StatoDocumentoContabile As Integer
        Registrato = 0
        Emesso = 1
        Evaso = 2
    End Enum

    ''' <summary>
    ''' Rappresenta un documento contabile
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class DocumentoContabile
        Inherits DBObjectPO

        Private m_TipoDocumento As String
        Private m_NumeroDocumento As String
        Private m_DataRegistrazione As Date?
        Private m_DataEmissione As Date?
        Private m_DataEvasione As Date?
        Private m_StatoDocumento As StatoDocumentoContabile

        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_NomeCliente As String
        Private m_IndirizzoCliente As CIndirizzo
        Private m_CodiceFiscaleCliente As String
        Private m_PartitaIVACliente As String

        Private m_IDFornitore As Integer
        Private m_Fornitore As CPersona
        Private m_NomeFornitore As String
        Private m_IndirizzoFornitore As CIndirizzo
        Private m_CodiceFiscaleFornitore As String
        Private m_PartitaIVAFornitore As String

        Private m_IndirizzoSpedizione As CIndirizzo

        'Private m_RiferimentoADocumenti As RiferimentoADocumentiCollection
        Private m_Descrizione As String

        Private m_Flags As Integer

        Private m_Attribiti As CKeyCollection

        Private m_TotaleImponibile As Decimal?
        Private m_TotaleIvato As Decimal?

        Private m_VociPagamento As VociDiPagamentoPerDocumento

        Private m_Source As Object
        Private m_SourceType As String
        Private m_SourceID As Integer
        Private m_SourceParams As String


        Public Sub New()
            Me.m_TipoDocumento = ""
            Me.m_NumeroDocumento = ""
            Me.m_DataRegistrazione = Nothing
            Me.m_DataEmissione = Nothing
            Me.m_DataEvasione = Nothing
            Me.m_StatoDocumento = StatoDocumentoContabile.Registrato
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_IndirizzoCliente = New CIndirizzo
            Me.m_CodiceFiscaleCliente = ""
            Me.m_PartitaIVACliente = ""
            Me.m_IDFornitore = 0
            Me.m_Fornitore = Nothing
            Me.m_NomeFornitore = ""
            Me.m_IndirizzoFornitore = New CIndirizzo
            Me.m_CodiceFiscaleFornitore = ""
            Me.m_PartitaIVAFornitore = ""
            Me.m_Descrizione = ""

            Me.m_IndirizzoSpedizione = New CIndirizzo

            Me.m_Flags = 0

            Me.m_Attribiti = Nothing

            Me.m_TotaleImponibile = Nothing
            Me.m_TotaleIvato = Nothing
            Me.m_VociPagamento = Nothing


            Me.m_Source = Nothing
            Me.m_SourceType = ""
            Me.m_SourceID = 0
            Me.m_SourceParams = ""
        End Sub

      

        ''' <summary>
        ''' Restituisce o imposta il documento o l'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Source As Object
            Get
                If (Me.m_Source Is Nothing) Then Me.m_Source = Sistema.Types.GetItemByTypeAndId(Me.m_SourceType, Me.m_SourceID)
                Return Me.m_Source
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                Me.m_SourceType = ""
                If (value IsNot Nothing) Then Me.m_SourceType = TypeName(value)
                Me.m_SourceID = GetID(value)
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetSource(ByVal value As Object)
            Me.m_Source = value
            Me.m_SourceID = GetID(value)
            Me.m_SourceType = "" : If (value IsNot Nothing) Then Me.m_SourceType = TypeName(value)
        End Sub

        ''' <summary>
        ''' Restituisec o imposta il tipo del documento o l'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceType As String
            Get
                Return Me.m_SourceType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SourceType
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_SourceType = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del documento o dell'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi per il documento o l'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceParams As String
            Get
                Return Me.m_SourceParams
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SourceParams
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_SourceParams = value
                Me.DoChanged("SourceParams", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetSourceParams(ByVal value As String)
            Me.m_SourceParams = value
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il tipo del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoDocumento As String
            Get
                Return Me.m_TipoDocumento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoDocumento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoDocumento = value
                Me.DoChanged("TipoDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroDocumento As String
            Get
                Return Me.m_NumeroDocumento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NumeroDocumento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NumeroDocumento = value
                Me.DoChanged("NumeroDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di registrazione del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRegistrazione As Date?
            Get
                Return Me.m_DataRegistrazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRegistrazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRegistrazione = value
                Me.DoChanged("DataRegistrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di emissione del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEmissione As Date?
            Get
                Return Me.m_DataEmissione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEmissione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataEmissione = value
                Me.DoChanged("DataEmissione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di evasione del documento (es. per gli ordini)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEvasione As Date?
            Get
                Return Me.m_DataEvasione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEvasione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataEvasione = value
                Me.DoChanged("DataEvasione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoDocumento As StatoDocumentoContabile
            Get
                Return Me.m_StatoDocumento
            End Get
            Set(value As StatoDocumentoContabile)
                Dim oldValue As StatoDocumentoContabile = Me.m_StatoDocumento
                If (oldValue = value) Then Exit Property
                Me.m_StatoDocumento = value
                Me.DoChanged("StatoDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il cliente 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then
                    Me.m_NomeCliente = value.Nominativo
                Else
                    Me.m_NomeCliente = ""
                End If
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'indirizzo del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirizzoCliente As CIndirizzo
            Get
                Return Me.m_IndirizzoCliente
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice fiscale del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceFiscaleCliente As String
            Get
                Return Me.m_CodiceFiscaleCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceFiscaleCliente
                value = Formats.ParseCodiceFiscale(value)
                If (oldValue = value) Then Exit Property
                Me.m_CodiceFiscaleCliente = value
                Me.DoChanged("CodiceFiscaleCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la partita IVA del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartitaIVACliente As String
            Get
                Return Me.m_PartitaIVACliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_PartitaIVACliente
                value = Formats.ParsePartitaIVA(value)
                If (oldValue = value) Then Exit Property
                Me.m_PartitaIVACliente = value
                Me.DoChanged("PartitaIVACliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFornitore As Integer
            Get
                Return GetID(Me.m_Fornitore, Me.m_IDFornitore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFornitore
                If (oldValue = value) Then Exit Property
                Me.m_IDFornitore = value
                Me.m_Fornitore = Nothing
                Me.DoChanged("IDFornitore", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il Fornitore 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Fornitore As CPersona
            Get
                If (Me.m_Fornitore Is Nothing) Then Me.m_Fornitore = Anagrafica.Persone.GetItemById(Me.m_IDFornitore)
                Return Me.m_Fornitore
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Fornitore
                If (oldValue Is value) Then Exit Property
                Me.m_Fornitore = value
                Me.m_IDFornitore = GetID(value)
                If (value IsNot Nothing) Then
                    Me.m_NomeFornitore = value.Nominativo
                Else
                    Me.m_NomeFornitore = ""
                End If
                Me.DoChanged("Fornitore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del Fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeFornitore As String
            Get
                Return Me.m_NomeFornitore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeFornitore
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeFornitore = value
                Me.DoChanged("NomeFornitore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'indirizzo del Fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirizzoFornitore As CIndirizzo
            Get
                Return Me.m_IndirizzoFornitore
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice fiscale del Fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceFiscaleFornitore As String
            Get
                Return Me.m_CodiceFiscaleFornitore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceFiscaleFornitore
                value = Formats.ParseCodiceFiscale(value)
                If (oldValue = value) Then Exit Property
                Me.m_CodiceFiscaleFornitore = value
                Me.DoChanged("CodiceFiscaleFornitore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la partita IVA del Fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartitaIVAFornitore As String
            Get
                Return Me.m_PartitaIVAFornitore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_PartitaIVAFornitore
                value = Formats.ParsePartitaIVA(value)
                If (oldValue = value) Then Exit Property
                Me.m_PartitaIVAFornitore = value
                Me.DoChanged("PartitaIVAFornitore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'indirizzo di spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirizzoSpedizione As CIndirizzo
            Get
                Return Me.m_IndirizzoSpedizione
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il campo descrizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice un campo 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attribiti As CKeyCollection
            Get
                If (Me.m_Attribiti Is Nothing) Then Me.m_Attribiti = New CKeyCollection
                Return Me.m_Attribiti
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il totale imponibile del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TotaleImponibile As Decimal?
            Get
                Return Me.m_TotaleImponibile
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_TotaleImponibile
                If (oldValue = value) Then Exit Property
                Me.m_TotaleImponibile = value
                Me.DoChanged("TotaleImponibile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il totale ivato del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TotaleIvato As Decimal?
            Get
                Return Me.m_TotaleIvato
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_TotaleIvato
                If (oldValue = value) Then Exit Property
                Me.m_TotaleIvato = value
                Me.DoChanged("TotaleIvato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco delle movimentazioni di denaro causate da questo documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VociDiPagamento As VociDiPagamentoPerDocumento
            Get
                If (Me.m_VociPagamento Is Nothing) Then Me.m_VociPagamento = New VociDiPagamentoPerDocumento(Me)
                Return Me.m_VociPagamento
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.DocumentiContabili.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDocumentiContabili"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            Me.m_IndirizzoCliente.SetChanged(False)
            Me.m_IndirizzoFornitore.SetChanged(False)
            Me.m_IndirizzoSpedizione.SetChanged(False)
            If (Me.m_VociPagamento IsNot Nothing) Then Me.m_VociPagamento.SetChanged(False)
            MyBase.OnAfterSave(e)
        End Sub

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_IndirizzoCliente.IsChanged OrElse Me.m_IndirizzoFornitore.IsChanged OrElse Me.m_IndirizzoSpedizione.IsChanged OrElse (Me.m_VociPagamento IsNot Nothing AndAlso Me.m_VociPagamento.IsChanged)
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret AndAlso Me.m_VociPagamento IsNot Nothing) Then Me.m_VociPagamento.Save()
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_TipoDocumento = reader.Read("TipoDocumento", Me.m_TipoDocumento)
            Me.m_NumeroDocumento = reader.Read("NumeroDocumento", Me.m_NumeroDocumento)
            Me.m_DataRegistrazione = reader.Read("DataRegistrazione", Me.m_DataRegistrazione)
            Me.m_DataEmissione = reader.Read("DataEmissione", Me.m_DataEmissione)
            Me.m_DataEvasione = reader.Read("DataEvasione", Me.m_DataEvasione)
            Me.m_StatoDocumento = reader.Read("StatoDocumento", Me.m_StatoDocumento)
            Me.m_TotaleImponibile = reader.Read("TotaleImponibile", Me.m_TotaleImponibile)
            Me.m_TotaleIvato = reader.Read("TotaleIvato", Me.m_TotaleIvato)

            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            With Me.m_IndirizzoCliente
                .Nome = reader.Read("INDCLT_LABEL", .Nome)
                .ToponimoViaECivico = reader.Read("INDCLT_VIA", .ToponimoViaECivico)
                .CAP = reader.Read("INDCLT_CAP", .CAP)
                .Citta = reader.Read("INDCLT_CITTA", .Citta)
                .Provincia = reader.Read("INDCLT_PROV", .Provincia)
                .SetChanged(False)
            End With
            Me.m_CodiceFiscaleCliente = reader.Read("CodiceFiscaleCliente", Me.m_CodiceFiscaleCliente)
            Me.m_PartitaIVACliente = reader.Read("PartitaIVACliente", Me.m_PartitaIVACliente)

            Me.m_IDFornitore = reader.Read("IDFornitore", Me.m_IDFornitore)
            Me.m_NomeFornitore = reader.Read("NomeFornitore", Me.m_NomeFornitore)
            With Me.m_IndirizzoFornitore
                .Nome = reader.Read("INDFNT_LABEL", .Nome)
                .ToponimoViaECivico = reader.Read("INDFNT_VIA", .ToponimoViaECivico)
                .CAP = reader.Read("INDFNT_CAP", .CAP)
                .Citta = reader.Read("INDFNT_CITTA", .Citta)
                .Provincia = reader.Read("INDFNT_PROV", .Provincia)
                .SetChanged(False)
            End With
            Me.m_CodiceFiscaleFornitore = reader.Read("CodiceFiscaleFornitore", Me.m_CodiceFiscaleFornitore)
            Me.m_PartitaIVAFornitore = reader.Read("PartitaIVAFornitore", Me.m_PartitaIVAFornitore)

            With Me.m_IndirizzoSpedizione
                .Nome = reader.Read("INDSPD_LABEL", .Nome)
                .ToponimoViaECivico = reader.Read("INDSPD_VIA", .ToponimoViaECivico)
                .CAP = reader.Read("INDSPD_CAP", .CAP)
                .Citta = reader.Read("INDSPD_CITTA", .Citta)
                .Provincia = reader.Read("INDSPD_PROV", .Provincia)
                .SetChanged(False)
            End With

            'Private m_RiferimentoADocumenti As RiferimentoADocumentiCollection
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)

            Me.m_Flags = reader.Read("Flags", Me.m_Flags)

            Try
                Me.m_Attribiti = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attribiti = Nothing
            End Try

            Me.m_SourceType = reader.Read("SourceType", Me.m_SourceType)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            Me.m_SourceParams = reader.Read("SourceParams", Me.m_SourceParams)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("TipoDocumento", Me.m_TipoDocumento)
            writer.Write("NumeroDocumento", Me.m_NumeroDocumento)
            writer.Write("DataRegistrazione", Me.m_DataRegistrazione)
            writer.Write("DataEmissione", Me.m_DataEmissione)
            writer.Write("DataEvasione", Me.m_DataEvasione)
            writer.Write("StatoDocumento", Me.m_StatoDocumento)
            writer.Write("TotaleImponibile", Me.m_TotaleImponibile)
            writer.Write("TotaleIvato", Me.m_TotaleIvato)

            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            With Me.m_IndirizzoCliente
                writer.Write("INDCLT_LABEL", .Nome)
                writer.Write("INDCLT_VIA", .ToponimoViaECivico)
                writer.Write("INDCLT_CAP", .CAP)
                writer.Write("INDCLT_CITTA", .Citta)
                writer.Write("INDCLT_PROV", .Provincia)
            End With
            writer.Write("CodiceFiscaleCliente", Me.m_CodiceFiscaleCliente)
            writer.Write("PartitaIVACliente", Me.m_PartitaIVACliente)

            writer.Write("IDFornitore", Me.IDFornitore)
            writer.Write("NomeFornitore", Me.m_NomeFornitore)
            With Me.m_IndirizzoFornitore
                writer.Write("INDFNT_LABEL", .Nome)
                writer.Write("INDFNT_VIA", .ToponimoViaECivico)
                writer.Write("INDFNT_CAP", .CAP)
                writer.Write("INDFNT_CITTA", .Citta)
                writer.Write("INDFNT_PROV", .Provincia)
            End With
            writer.Write("CodiceFiscaleFornitore", Me.m_CodiceFiscaleFornitore)
            writer.Write("PartitaIVAFornitore", Me.m_PartitaIVAFornitore)

            With Me.m_IndirizzoSpedizione
                writer.Write("INDSPD_LABEL", .Nome)
                writer.Write("INDSPD_VIA", .ToponimoViaECivico)
                writer.Write("INDSPD_CAP", .CAP)
                writer.Write("INDSPD_CITTA", .Citta)
                writer.Write("INDSPD_PROV", .Provincia)
            End With

            'Private m_RiferimentoADocumenti As RiferimentoADocumentiCollection
            writer.Write("Descrizione", Me.m_Descrizione)

            writer.Write("Flags", Me.m_Flags)

            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attribiti))

            writer.Write("SourceType", Me.m_SourceType)
            writer.Write("SourceID", Me.SourceID)
            writer.Write("SourceParams", Me.m_SourceParams)


            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("TipoDocumento", Me.m_TipoDocumento)
            writer.WriteAttribute("NumeroDocumento", Me.m_NumeroDocumento)
            writer.WriteAttribute("DataRegistrazione", Me.m_DataRegistrazione)
            writer.WriteAttribute("DataEmissione", Me.m_DataEmissione)
            writer.WriteAttribute("DataEvasione", Me.m_DataEvasione)
            writer.WriteAttribute("StatoDocumento", Me.m_StatoDocumento)
            writer.WriteAttribute("TotaleImponibile", Me.m_TotaleImponibile)
            writer.WriteAttribute("TotaleIvato", Me.m_TotaleIvato)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("CodiceFiscaleCliente", Me.m_CodiceFiscaleCliente)
            writer.WriteAttribute("PartitaIVACliente", Me.m_PartitaIVACliente)
            writer.WriteAttribute("IDFornitore", Me.IDFornitore)
            writer.WriteAttribute("NomeFornitore", Me.m_NomeFornitore)
            writer.WriteAttribute("CodiceFiscaleFornitore", Me.m_CodiceFiscaleFornitore)
            writer.WriteAttribute("PartitaIVAFornitore", Me.m_PartitaIVAFornitore)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("SourceType", Me.m_SourceType)
            writer.WriteAttribute("SourceID", Me.SourceID)
            writer.WriteAttribute("SourceParams", Me.m_SourceParams)

            MyBase.XMLSerialize(writer)
            writer.WriteTag("INDCLT", Me.IndirizzoCliente)
            writer.WriteTag("INDFNT", Me.IndirizzoFornitore)
            writer.WriteTag("INDSPD", Me.IndirizzoSpedizione)
            writer.WriteTag("Attributi", Me.Attribiti)
            writer.WriteTag("VociDiPagamento", Me.VociDiPagamento)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "TipoDocumento" : Me.m_TipoDocumento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroDocumento" : Me.m_NumeroDocumento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRegistrazione" : Me.m_DataRegistrazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataEmissione" : Me.m_DataEmissione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataEvasione" : Me.m_DataEvasione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoDocumento" : Me.m_StatoDocumento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TotaleImponibile" : Me.m_TotaleImponibile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TotaleIvato" : Me.m_TotaleIvato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceFiscaleCliente" : Me.m_CodiceFiscaleCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PartitaIVACliente" : Me.m_PartitaIVACliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFornitore" : Me.m_IDFornitore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeFornitore" : Me.m_NomeFornitore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceFiscaleFornitore" : Me.m_CodiceFiscaleFornitore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PartitaIVAFornitore" : Me.m_PartitaIVAFornitore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "INDCLT" : Me.m_IndirizzoCliente = CType(fieldValue, CIndirizzo)
                Case "INDFNT" : Me.m_IndirizzoFornitore = CType(fieldValue, CIndirizzo)
                Case "INDSPD" : Me.m_IndirizzoSpedizione = CType(fieldValue, CIndirizzo)
                Case "Attributi" : Me.m_Attribiti = CType(fieldValue, CKeyCollection)
                Case "VociDiPagamento" : Me.m_VociPagamento = CType(fieldValue, VociDiPagamentoPerDocumento) : Me.m_VociPagamento.SetDocumento(Me)
                Case "SourceType" : Me.m_SourceType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SourceParams" : Me.m_SourceParams = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select



        End Sub

    End Class



End Class