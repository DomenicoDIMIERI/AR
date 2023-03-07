Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    Public Enum StatoChiamataRegistrata As Integer
        ''' <summary>
        ''' Stato sconosciuto
        ''' </summary>
        Sconosciuto = 0

        ''' <summary>
        ''' La chiamata è in fase di composizione
        ''' </summary>
        Composizione = 1

        ''' <summary>
        ''' La chiamata è in corso (la controparte ha risposto)
        ''' </summary>
        InCorso = 2

        ''' <summary>
        ''' La chiamata è terminata senza che la controparte abbia risposto
        ''' </summary>
        ''' <remarks></remarks>
        NonRisposto = 3

        ''' <summary>
        ''' Il chiamante ha agganciato
        ''' </summary>
        ''' <remarks></remarks>
        AgganciatoChiamante = 4

        ''' <summary>
        ''' Il chiamato ha agganciato
        ''' </summary>
        AgganciatoChiamato = 5

        ''' <summary>
        ''' Il chiamato ha rifiutato la chiamata
        ''' </summary>
        Rifiutata = 6


        ''' <summary>
        ''' La chiamata non è stata effettuata a causa di un errore
        ''' </summary>
        ''' <remarks></remarks>
        Errore = 255

    End Enum

    Public Enum EsitoChiamataRegistrata As Integer
        NonRisposto = 0

        Risposto = 1
    End Enum

    Public Enum QualitaChiamata As Integer
        Ottima = -100
        Buona = -50
        Normale = 0
        Scarsa = 50
        Pessima = 100
    End Enum

    ''' <summary>
    ''' Rappresenta una chiamata registrata
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class ChiamataRegistrata
        Inherits DBObjectPO

        Private m_IDChiamata As String          'ID della chiamata (identifica la chiamata nel contesto del centralino)
        Private m_StatoChiamata As StatoChiamataRegistrata 'Stato della chiamata
        Private m_EsitoChiamataEx As String
        Private m_EsitoChiamata As EsitoChiamataRegistrata 'Esito della chiamata
        Private m_StatoChiamataEx As String

        Private m_DataInizio As Date?           'Data e ora di inizio della chiamata (composizione)
        Private m_DataRisposta As Date?         'In caso di risposta data e ora della risposta della controparte (o di un centralino/voce automatica)
        Private m_DataFine As Date?             'Data e ora di fine della chiamata

        Private m_IDPBX As Integer              'ID del centralino che ha gestito la chiamata
        Private m_PBX As PBX                 'Centralino che ha gestito la chiamata
        Private m_NomePBX As String             'Nome del centralino che ha gestito la chiamata

        Private m_IDChiamante As Integer
        Private m_Chiamante As CPersona
        Private m_NomeChiamante As String

        Private m_IDChiamato As Integer
        Private m_Chiamato As CPersona
        Private m_NomeChiamato As String

        Private m_DaNumero As String
        Private m_ANumero As String

        Private m_NomeCanale As String
        Private m_Qualita As QualitaChiamata


        Public Sub New()
            Me.m_IDChiamata = ""
            Me.m_StatoChiamata = StatoChiamataRegistrata.Sconosciuto
            Me.m_EsitoChiamataEx = ""
            Me.m_EsitoChiamata = EsitoChiamataRegistrata.NonRisposto
            Me.m_StatoChiamataEx = ""

            Me.m_DataInizio = Nothing
            Me.m_DataRisposta = Nothing
            Me.m_DataFine = Nothing

            Me.m_IDPBX = 0
            Me.m_PBX = Nothing
            Me.m_NomePBX = ""

            Me.m_IDChiamante = 0
            Me.m_Chiamante = Nothing
            Me.m_NomeChiamante = ""

            Me.m_IDChiamato = 0
            Me.m_Chiamato = Nothing
            Me.m_NomeChiamato = ""

            Me.m_DaNumero = ""
            Me.m_ANumero = ""

            Me.m_NomeCanale = ""
            Me.m_Qualita = QualitaChiamata.Normale
        End Sub

        Public Property IDChiamata As String
            Get
                Return Me.m_IDChiamata
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IDChiamata
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_IDChiamata = value
                Me.DoChanged("IDChiamata", value, oldValue)
            End Set
        End Property

        Public Property StatoChiamata As StatoChiamataRegistrata
            Get
                Return Me.m_StatoChiamata
            End Get
            Set(value As StatoChiamataRegistrata)
                Dim oldValue As StatoChiamataRegistrata = Me.m_StatoChiamata
                If (oldValue = value) Then Return
                Me.m_StatoChiamata = value
                Me.DoChanged("StatoChiamata", value, oldValue)
            End Set
        End Property

        Public Property StatoChiamataEx As String
            Get
                Return Me.m_StatoChiamataEx
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_StatoChiamataEx
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_StatoChiamataEx = value
                Me.DoChanged("StatoChiamataEx", value, oldValue)
            End Set
        End Property

        Public Property EsitoChiamata As EsitoChiamataRegistrata 'Esito della chiamata
            Get
                Return Me.m_EsitoChiamata
            End Get
            Set(value As EsitoChiamataRegistrata)
                Dim oldValue As EsitoChiamataRegistrata = Me.m_EsitoChiamata
                If (oldValue = value) Then Return
                Me.m_EsitoChiamata = value
                Me.DoChanged("EsitoChiamata", value, oldValue)
            End Set
        End Property


        Public Property EsitoChiamataEx As String
            Get
                Return Me.m_EsitoChiamataEx
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_EsitoChiamataEx
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_EsitoChiamataEx = value
                Me.DoChanged("EsitoChiamataEx", value, oldValue)
            End Set
        End Property


        Public Property DataInizio As Date?
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        Public Property DataRisposta As Date?
            Get
                Return Me.m_DataRisposta
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRisposta
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRisposta = value
                Me.DoChanged("DataRisposta", value, oldValue)
            End Set
        End Property

        Public Property DataFine As Date?
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFine
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        Public Property IDPBX As Integer
            Get
                Return GetID(Me.m_PBX, Me.m_IDPBX)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPBX
                If (oldValue = value) Then Return
                Me.m_IDPBX = value
                Me.m_PBX = Nothing
                Me.DoChanged("IDPBX", value, oldValue)
            End Set
        End Property

        Public Property PBX As PBX                 'Centralino che ha gestito la chiamata
            Get
                If (Me.m_PBX Is Nothing) Then Me.m_PBX = Office.PBXs.GetItemById(Me.m_IDPBX)
                Return Me.m_PBX
            End Get
            Set(value As PBX)
                Dim oldValue As PBX = Me.m_PBX
                If (oldValue Is value) Then Return
                Me.m_PBX = value
                Me.m_IDPBX = GetID(value)
                Me.DoChanged("PBX", value, oldValue)
            End Set
        End Property

        Public Property NomePBX As String
            Get
                Return Me.m_NomePBX
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePBX
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomePBX = value
                Me.DoChanged("NomePBX", value, oldValue)
            End Set
        End Property

        Public Property IDChiamante As Integer
            Get
                Return GetID(Me.m_Chiamante, Me.m_IDChiamante)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDChiamante
                If (oldValue = value) Then Return
                Me.m_IDChiamante = value
                Me.m_Chiamante = Nothing
                Me.DoChanged("IDChiamante", value, oldValue)
            End Set
        End Property

        Public Property Chiamante As CPersona
            Get
                If (Me.m_Chiamante Is Nothing) Then Me.m_Chiamante = Anagrafica.Persone.GetItemById(Me.m_IDChiamante)
                Return Me.m_Chiamante
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Chiamante
                If (oldValue Is value) Then Return
                Me.m_Chiamante = value
                Me.m_IDChiamante = GetID(value)
                Me.m_NomeChiamante = "" : If (value IsNot Nothing) Then Me.m_NomeChiamante = value.Nominativo
                Me.DoChanged("Chiamante", value, oldValue)
            End Set
        End Property

        Public Property NomeChiamante As String
            Get
                Return Me.m_NomeChiamante
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeChiamante
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeChiamante = value
                Me.DoChanged("NomeChiamante", value, oldValue)
            End Set
        End Property

        Public Property IDChiamato As Integer
            Get
                Return GetID(Me.m_Chiamato, Me.m_IDChiamato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDChiamato
                If (oldValue = value) Then Return
                Me.m_IDChiamato = value
                Me.m_Chiamato = Nothing
                Me.DoChanged("IDChiamanto", value, oldValue)
            End Set
        End Property

        Public Property Chiamato As CPersona
            Get
                If (Me.m_Chiamato Is Nothing) Then Me.m_Chiamato = Anagrafica.Persone.GetItemById(Me.m_IDChiamato)
                Return Me.m_Chiamato
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Chiamato
                If (oldValue Is value) Then Return
                Me.m_Chiamato = value
                Me.m_IDChiamato = GetID(value)
                Me.m_NomeChiamato = "" : If (value IsNot Nothing) Then Me.m_NomeChiamato = value.Nominativo
                Me.DoChanged("Chiamato", value, oldValue)
            End Set
        End Property

        Public Property NomeChiamato As String
            Get
                Return Me.m_NomeChiamato
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeChiamato
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeChiamato = value
                Me.DoChanged("NomeChiamato", value, oldValue)
            End Set
        End Property

        Public Property DaNumero As String
            Get
                Return Me.m_DaNumero
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DaNumero
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Return
                Me.m_DaNumero = value
                Me.DoChanged("DaNumero", value, oldValue)
            End Set
        End Property

        Public Property ANumero As String
            Get
                Return Me.m_ANumero
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ANumero
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Return
                Me.m_ANumero = value
                Me.DoChanged("ANumero", value, oldValue)
            End Set
        End Property

        Public Property NomeCanale As String
            Get
                Return Me.m_NomeCanale
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCanale
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeCanale = value
                Me.DoChanged("NomeCanale", value, oldValue)
            End Set
        End Property

        Public Property Qualita As QualitaChiamata
            Get
                Return Me.m_Qualita
            End Get
            Set(value As QualitaChiamata)
                Dim oldValue As QualitaChiamata = Me.m_Qualita
                If (oldValue = value) Then Return
                Me.m_Qualita = value
                Me.DoChanged("Qualita", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_NomePBX & ", " & Formats.FormatUserDateTime(Me.m_DataInizio) & ", " & Me.m_DaNumero & " -> " & Me.m_ANumero & ", " & [Enum].GetName(GetType(StatoChiamataRegistrata), Me.m_StatoChiamata)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.PBXs.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.ChiamateRegistrate.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRegCall"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDChiamata = reader.Read("IDChiamata", Me.m_IDChiamata)
            Me.m_StatoChiamata = reader.Read("StatoChiamata", Me.m_StatoChiamata)
            Me.m_StatoChiamataEx = reader.Read("StatoChiamataEx", Me.m_StatoChiamataEx)
            Me.m_EsitoChiamata = reader.Read("EsitoChiamata", Me.m_EsitoChiamata)
            Me.m_EsitoChiamataEx = reader.Read("EsitoChiamataEx", Me.m_EsitoChiamataEx)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataRisposta = reader.Read("DataRisposta", Me.m_DataRisposta)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_IDPBX = reader.Read("IDPBX", Me.m_IDPBX)
            Me.m_NomePBX = reader.Read("NomePBX", Me.m_NomePBX)
            Me.m_IDChiamante = reader.Read("IDChiamante", Me.m_IDChiamante)
            Me.m_NomeChiamante = reader.Read("NomeChiamante", Me.m_NomeChiamante)
            Me.m_IDChiamato = reader.Read("IDChiamato", Me.m_IDChiamato)
            Me.m_NomeChiamato = reader.Read("NomeChiamato", Me.m_NomeChiamato)
            Me.m_DaNumero = reader.Read("DaNumero", Me.m_DaNumero)
            Me.m_ANumero = reader.Read("ANumero", Me.m_ANumero)
            Me.m_NomeCanale = reader.Read("NomeCanale", Me.m_NomeCanale)
            Me.m_Qualita = reader.Read("Qualita", Me.m_Qualita)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDChiamata", Me.m_IDChiamata)
            writer.Write("StatoChiamata", Me.m_StatoChiamata)
            writer.Write("StatoChiamataEx", Me.m_StatoChiamataEx)
            writer.Write("EsitoChiamata", Me.m_EsitoChiamata)
            writer.Write("EsitoChiamataEx", Me.m_EsitoChiamataEx)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataRisposta", Me.m_DataRisposta)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("IDPBX", Me.IDPBX)
            writer.Write("NomePBX", Me.m_NomePBX)
            writer.Write("IDChiamante", Me.IDChiamante)
            writer.Write("NomeChiamante", Me.m_NomeChiamante)
            writer.Write("IDChiamato", Me.IDChiamato)
            writer.Write("NomeChiamato", Me.m_NomeChiamato)
            writer.Write("DaNumero", Me.m_DaNumero)
            writer.Write("ANumero", Me.m_ANumero)
            writer.Write("NomeCanale", Me.m_NomeCanale)
            writer.Write("Qualita", Me.m_Qualita)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDChiamata", Me.m_IDChiamata)
            writer.WriteAttribute("StatoChiamata", Me.m_StatoChiamata)
            writer.WriteAttribute("StatoChiamataEx", Me.m_StatoChiamataEx)
            writer.WriteAttribute("EsitoChiamata", Me.m_EsitoChiamata)
            writer.WriteAttribute("EsitoChiamataEx", Me.m_EsitoChiamataEx)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataRisposta", Me.m_DataRisposta)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("IDPBX", Me.IDPBX)
            writer.WriteAttribute("NomePBX", Me.m_NomePBX)
            writer.WriteAttribute("IDChiamante", Me.IDChiamante)
            writer.WriteAttribute("NomeChiamante", Me.m_NomeChiamante)
            writer.WriteAttribute("IDChiamato", Me.IDChiamato)
            writer.WriteAttribute("NomeChiamato", Me.m_NomeChiamato)
            writer.WriteAttribute("DaNumero", Me.m_DaNumero)
            writer.WriteAttribute("ANumero", Me.m_ANumero)
            writer.WriteAttribute("NomeCanale", Me.m_NomeCanale)
            writer.WriteAttribute("Qualita", Me.m_Qualita)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDChiamata" : Me.m_IDChiamata = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoChiamata" : Me.m_StatoChiamata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoChiamataEx" : Me.m_StatoChiamataEx = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EsitoChiamata" : Me.m_EsitoChiamata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "EsitoChiamataEx" : Me.m_EsitoChiamataEx = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataRisposta" : Me.m_DataRisposta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPBX" : Me.m_IDPBX = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePBX" : Me.m_NomePBX = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDChiamante" : Me.m_IDChiamante = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeChiamante" : Me.m_NomeChiamante = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDChiamato" : Me.m_IDChiamato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeChiamato" : Me.m_NomeChiamato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DaNumero" : Me.m_DaNumero = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ANumero" : Me.m_ANumero = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeCanale" : Me.m_NomeCanale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Qualita" : Me.m_Qualita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


    End Class



End Class