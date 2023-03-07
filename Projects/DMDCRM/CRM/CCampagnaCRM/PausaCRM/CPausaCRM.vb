Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica
Imports DMD.XML

Partial Public Class CustomerCalls

    Public Enum StatoPausaCRM As Integer
        NonChiesta = 0
        Chiesta = 1
        InValutazione = 2
        Approvata = 3
        Negata = 4
        Iniziata = 5
        Terminata = 6
        AnnullataPrimaValutazione = 7
        AnnullataDopoValutazione = 8
    End Enum

    <Serializable>
    Public Class CPausaCRM
        Inherits DBObjectPO

        Private m_IDSessioneCRM As Integer
        Private m_SessioneCRM As CSessioneCRM

        Private m_IDUtente As Integer
        Private m_Utente As CUser
        Private m_NomeUtente As String
        Private m_OraRichiesta As Date?
        Private m_OraInizioValutazione As Date?
        Private m_OraFineValutazione As Date?
        Private m_OraPrevista As Date?
        Private m_Inizio As Date?
        Private m_Fine As Date?
        Private m_Motivo As String
        Private m_DurataPrevista As Integer?
        Private m_DettaglioMotivo As String
        Private m_IDSupervisore As Integer
        Private m_Supervisore As CUser
        Private m_NomeSupervisore As String
        Private m_EsitoSupervisione As String
        Private m_NoteAmministrative As String
        Private m_StatoRichiesta As StatoPausaCRM
        Private m_Flags As Integer
        Private m_Attributi As CKeyCollection

        Public Sub New()
            Me.m_IDSessioneCRM = 0
            Me.m_SessioneCRM = Nothing
            Me.m_IDUtente = 0
            Me.m_Utente = Nothing
            Me.m_NomeUtente = ""
            Me.m_Inizio = Nothing
            Me.m_Fine = Nothing
            Me.m_Motivo = ""
            Me.m_DurataPrevista = Nothing
            Me.m_DettaglioMotivo = ""
            Me.m_IDSupervisore = 0
            Me.m_Supervisore = Nothing
            Me.m_NomeSupervisore = ""
            Me.m_EsitoSupervisione = ""
            Me.m_NoteAmministrative = ""
            Me.m_StatoRichiesta = StatoPausaCRM.NonChiesta
            Me.m_Flags = 0
            Me.m_Attributi = Nothing
            Me.m_OraRichiesta = Nothing
            Me.m_OraInizioValutazione = Nothing
            Me.m_OraFineValutazione = Nothing
            Me.m_OraPrevista = Nothing
        End Sub

        Public Property IDSessioneCRM As Integer
            Get
                Return GetID(Me.m_SessioneCRM, Me.m_IDSessioneCRM)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSessioneCRM
                If (oldValue = value) Then Return
                Me.m_IDSessioneCRM = value
                Me.m_SessioneCRM = Nothing
                Me.DoChanged("IDSessioneCRM", value, oldValue)
            End Set
        End Property

        Public Property SessioneCRM As CSessioneCRM
            Get
                If (Me.m_SessioneCRM Is Nothing) Then Me.m_SessioneCRM = CustomerCalls.SessioniCRM.GetItemById(Me.m_IDSessioneCRM)
                Return Me.m_SessioneCRM
            End Get
            Set(value As CSessioneCRM)
                Dim oldValue As CSessioneCRM = Me.SessioneCRM
                If (oldValue Is value) Then Return
                Me.m_SessioneCRM = value
                Me.m_IDSessioneCRM = GetID(value)
                Me.DoChanged("SessioneCRM", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetSessioneCRM(ByVal value As CSessioneCRM)
            Me.m_SessioneCRM = value
            Me.m_IDSessioneCRM = GetID(value)
        End Sub

        Public Property IDUtente As Integer
            Get
                Return GetID(Me.m_Utente, Me.m_IDUtente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUtente
                If (oldValue = value) Then Return
                Me.m_IDUtente = value
                Me.m_Utente = Nothing
                Me.DoChanged("IDUtente", value, oldValue)
            End Set
        End Property

        Public Property Utente As CUser
            Get
                If (Me.m_Utente Is Nothing) Then Me.m_Utente = Sistema.Users.GetItemById(Me.m_IDUtente)
                Return Me.m_Utente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Utente
                If (oldValue Is value) Then Return
                Me.m_Utente = value
                Me.m_IDUtente = GetID(value)
                Me.m_NomeUtente = "" : If (value IsNot Nothing) Then Me.m_NomeUtente = value.Nominativo
                Me.DoChanged("Utente", value, oldValue)
            End Set
        End Property

        Public Property NomeUtente As String
            Get
                Return Me.m_NomeUtente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeUtente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeUtente = value
                Me.DoChanged("NomeUtente", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetUtente(ByVal value As CUser)
            Me.m_Utente = value
            Me.m_IDUtente = GetID(value)
        End Sub

        Public Property Inizio As Date?
            Get
                Return Me.m_Inizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_Inizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Inizio = value
                Me.DoChanged("Inizio", value, oldValue)
            End Set
        End Property

        Public Property Fine As Date?
            Get
                Return Me.m_Fine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_Fine
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Fine = value
                Me.DoChanged("Fine", value, oldValue)
            End Set
        End Property

        Public Property Motivo As String
            Get
                Return Me.m_Motivo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Motivo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Motivo = value
                Me.DoChanged("Motivo", value, oldValue)
            End Set
        End Property

        Public Property DurataPrevista As Integer?
            Get
                Return Me.m_DurataPrevista
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_DurataPrevista
                If (oldValue = value) Then Return
                Me.m_DurataPrevista = value
                Me.DoChanged("DurataPrevista", value, oldValue)
            End Set
        End Property

        Public Property DettaglioMotivo As String
            Get
                Return Me.m_DettaglioMotivo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioMotivo
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_DettaglioMotivo = value
                Me.DoChanged("DettaglioMotivo", value, oldValue)
            End Set
        End Property

        Public Property IDSupervisore As Integer
            Get
                Return GetID(Me.m_Supervisore, Me.m_IDSupervisore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSupervisore
                If (oldValue = value) Then Return
                Me.m_IDSupervisore = value
                Me.m_Supervisore = Nothing
                Me.DoChanged("IDSupervisore", value, oldValue)
            End Set
        End Property

        Public Property Supervisore As CUser
            Get
                If (Me.m_Supervisore Is Nothing) Then Me.m_Supervisore = Sistema.Users.GetItemById(Me.m_IDSupervisore)
                Return Me.m_Supervisore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Supervisore
                If (oldValue Is value) Then Return
                Me.m_Supervisore = value
                Me.m_IDSupervisore = GetID(value)
                Me.m_NomeSupervisore = ""
                If (value IsNot Nothing) Then Me.m_NomeSupervisore = value.Nominativo
                Me.DoChanged("Supervisore", value, oldValue)
            End Set
        End Property

        Public Property NomeSupervisore As String
            Get
                Return Me.m_NomeSupervisore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeSupervisore
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeSupervisore = value
                Me.DoChanged("NomeSupervisore", value, oldValue)
            End Set
        End Property

        Public Property EsitoSupervisione As String
            Get
                Return Me.m_EsitoSupervisione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_EsitoSupervisione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_EsitoSupervisione = value
                Me.DoChanged("EsitoSupervisione", value, oldValue)
            End Set
        End Property


        Public Property NoteAmministrative As String
            Get
                Return Me.m_NoteAmministrative
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NoteAmministrative
                If (oldValue = value) Then Return
                Me.m_NoteAmministrative = value
                Me.DoChanged("NoteAmministrative", value, oldValue)
            End Set
        End Property

        Public Property StatoRichiesta As StatoPausaCRM
            Get
                Return Me.m_StatoRichiesta
            End Get
            Set(value As StatoPausaCRM)
                Dim oldValue As StatoPausaCRM = Me.m_StatoRichiesta
                If (oldValue = value) Then Return
                Me.m_StatoRichiesta = value
                Me.DoChanged("StatoRichiesta", value, oldValue)
            End Set
        End Property

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        Public Property OraRichiesta As Date?
            Get
                Return Me.m_OraRichiesta
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_OraRichiesta
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_OraRichiesta = value
                Me.DoChanged("OraRichiesta", value, oldValue)
            End Set
        End Property

        Public Property OraInizioValutazione As Date?
            Get
                Return Me.m_OraInizioValutazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_OraInizioValutazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_OraInizioValutazione = value
                Me.DoChanged("OraInizioValutazione", value, oldValue)
            End Set
        End Property

        Public Property OraFineValutazione As Date?
            Get
                Return Me.m_OraFineValutazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_OraFineValutazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_OraFineValutazione = value
                Me.DoChanged("OraFineValutazione", value, oldValue)
            End Set
        End Property

        Public Property OraPrevista As Date?
            Get
                Return Me.m_OraPrevista
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_OraPrevista
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_OraPrevista = value
                Me.DoChanged("OraPrevista", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.PauseCRM.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CustomerCalls.CRM.StatsDB
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PausaCRM"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDSessioneCRM = reader.Read("IDSessioneCRM", Me.m_IDSessioneCRM)
            Me.m_IDUtente = reader.Read("IDUtente", Me.m_IDUtente)
            Me.m_NomeUtente = reader.Read("NomeUtente", Me.m_NomeUtente)
            Me.m_Inizio = reader.Read("Inizio", Me.m_Inizio)
            Me.m_Fine = reader.Read("Fine", Me.m_Fine)
            Me.m_Motivo = reader.Read("Motivo", Me.m_Motivo)
            Me.m_DurataPrevista = reader.Read("DurataPrevista", Me.m_DurataPrevista)
            Me.m_DettaglioMotivo = reader.Read("DettaglioMotivo", Me.m_DettaglioMotivo)
            Me.m_IDSupervisore = reader.Read("IDSupervisore", Me.m_IDSupervisore)
            Me.m_NomeSupervisore = reader.Read("NomeSupervisore", Me.m_NomeSupervisore)
            Me.m_EsitoSupervisione = reader.Read("EsitoSupervisione", Me.m_EsitoSupervisione)
            Me.m_NoteAmministrative = reader.Read("NoteAmministrative", Me.m_NoteAmministrative)
            Me.m_StatoRichiesta = reader.Read("StatoRichiesta", Me.m_StatoRichiesta)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_OraRichiesta = reader.Read("OraRichiesta", Me.m_OraRichiesta)
            Me.m_OraInizioValutazione = reader.Read("OraInizioValutazione", Me.m_OraInizioValutazione)
            Me.m_OraFineValutazione = reader.Read("OraFineValutazione", Me.m_OraFineValutazione)
            Me.m_OraPrevista = reader.Read("OraPrevista", Me.m_OraPrevista)
            Dim tmp As String = reader.Read("Attributi", "")
            If (tmp <> "") Then Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDSessioneCRM", Me.IDSessioneCRM)
            writer.Write("IDUtente", Me.IDUtente)
            writer.Write("NomeUtente", Me.m_NomeUtente)
            writer.Write("Inizio", Me.m_Inizio)
            writer.Write("Fine", Me.m_Fine)
            writer.Write("Motivo", Me.m_Motivo)
            writer.Write("DurataPrevista", Me.m_DurataPrevista)
            writer.Write("DettaglioMotivo", Me.m_DettaglioMotivo)
            writer.Write("IDSupervisore", Me.IDSupervisore)
            writer.Write("NomeSupervisore", Me.m_NomeSupervisore)
            writer.Write("EsitoSupervisione", Me.m_EsitoSupervisione)
            writer.Write("NoteAmministrative", Me.m_NoteAmministrative)
            writer.Write("StatoRichiesta", Me.m_StatoRichiesta)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("OraRichiesta", Me.m_OraRichiesta)
            writer.Write("OraInizioValutazione", Me.m_OraInizioValutazione)
            writer.Write("OraFineValutazione", Me.m_OraFineValutazione)
            writer.Write("OraPrevista", Me.m_OraPrevista)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("IDSessioneCRM", Me.IDSessioneCRM)
            writer.WriteAttribute("IDUtente", Me.IDUtente)
            writer.WriteAttribute("NomeUtente", Me.m_NomeUtente)
            writer.WriteAttribute("Inizio", Me.m_Inizio)
            writer.WriteAttribute("Fine", Me.m_Fine)
            writer.WriteAttribute("Motivo", Me.m_Motivo)
            writer.WriteAttribute("DurataPrevista", Me.m_DurataPrevista)
            writer.WriteAttribute("DettaglioMotivo", Me.m_DettaglioMotivo)
            writer.WriteAttribute("IDSupervisore", Me.IDSupervisore)
            writer.WriteAttribute("NomeSupervisore", Me.m_NomeSupervisore)
            writer.WriteAttribute("EsitoSupervisione", Me.m_EsitoSupervisione)
            writer.WriteAttribute("NoteAmministrative", Me.m_NoteAmministrative)
            writer.WriteAttribute("StatoRichiesta", Me.m_StatoRichiesta)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("OraRichiesta", Me.m_OraRichiesta)
            writer.WriteAttribute("OraInizioValutazione", Me.m_OraInizioValutazione)
            writer.WriteAttribute("OraFineValutazione", Me.m_OraFineValutazione)
            writer.WriteAttribute("OraPrevista", Me.m_OraPrevista)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDSessioneCRM" : Me.m_IDSessioneCRM = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUtente" : Me.m_IDUtente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUtente" : Me.m_NomeUtente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Inizio" : Me.m_Inizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Fine" : Me.m_Fine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Motivo" : Me.m_Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DurataPrevista" : Me.m_DurataPrevista = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioMotivo" : Me.m_DettaglioMotivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDSupervisore" : Me.IDSupervisore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeSupervisore" : Me.m_NomeSupervisore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EsitoSupervisione" : Me.m_EsitoSupervisione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NoteAmministrative" : Me.m_NoteAmministrative = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoRichiesta" : Me.m_StatoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OraRichiesta" : Me.m_OraRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraInizioValutazione" : Me.m_OraInizioValutazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraFineValutazione" : Me.m_OraFineValutazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraPrevista" : Me.m_OraPrevista = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append(Me.m_NomeUtente)
            ret.Append(" [")
            ret.Append(Formats.FormatUserDateTime(Me.m_Inizio))
            ret.Append(" - ")
            ret.Append(Formats.FormatUserDateTime(Me.m_Fine))
            ret.Append("]")
            Return ret.ToString
        End Function

    End Class



End Class