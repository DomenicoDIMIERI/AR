Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica
Imports DMD.XML

Partial Public Class CustomerCalls

    <Serializable>
    Public Class CSessioneCRM
        Inherits DBObjectPO

        Private m_IDCampagnaCRM As Integer
        Private m_CampagnaCRM As CCampagnaCRM

        Private m_IDUtente As Integer
        Private m_Utente As CUser
        Private m_NomeUtente As String
        Private m_Inizio As Date?
        Private m_Fine As Date?
        Private m_NumeroTelefonateRisposte As Integer
        Private m_NumeroTelefonateNonRisposte As Integer
        Private m_MinutiConversazione As Integer
        Private m_MinutiAttesa As Integer
        Private m_NumeroAppuntamentiFissati As Integer
        Private m_Flags As Integer
        Private m_Attributi As CKeyCollection
        Private m_dmdpage As String
        Private m_IDSupervisore As Integer
        Private m_Supervisore As CUser
        Private m_NomeSupervisore As String
        Private m_LastUpdated As DateTime?

        Public Sub New()
            Me.m_IDCampagnaCRM = 0
            Me.m_CampagnaCRM = Nothing
            Me.m_IDUtente = 0
            Me.m_Utente = Nothing
            Me.m_NomeUtente = ""
            Me.m_Inizio = Nothing
            Me.m_Fine = Nothing
            Me.m_NumeroTelefonateRisposte = 0
            Me.m_NumeroTelefonateNonRisposte = 0
            Me.m_MinutiConversazione = 0
            Me.m_MinutiAttesa = 0
            Me.m_NumeroAppuntamentiFissati = 0
            Me.m_Flags = 0
            Me.m_Attributi = Nothing
            Me.m_dmdpage = ""
            Me.m_IDSupervisore = 0
            Me.m_Supervisore = Nothing
            Me.m_NomeSupervisore = ""
            Me.m_LastUpdated = Nothing
        End Sub

        Public Property LastUpdated As DateTime?
            Get
                Return Me.m_LastUpdated
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_LastUpdated
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_LastUpdated = value
                Me.DoChanged("LastUpdated", value, oldValue)
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
                Me.m_IDSupervisore = GetID(value)
                Me.m_Supervisore = value
                Me.m_NomeSupervisore = "" : If (value IsNot Nothing) Then Me.m_NomeSupervisore = value.Nominativo
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

        ''' <summary>
        ''' Restituisce o imposta l'id della pagina in cui é stata attivata la sessione
        ''' </summary>
        ''' <returns></returns>
        Public Property dmdpage As String
            Get
                Return Me.m_dmdpage
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_dmdpage
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_dmdpage = value
                Me.DoChanged("dmdpage", value, oldValue)
            End Set
        End Property

        Public Property IDCampagnaCRM As Integer
            Get
                Return GetID(Me.m_CampagnaCRM, Me.m_IDCampagnaCRM)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCampagnaCRM
                If (oldValue = value) Then Return
                Me.m_IDCampagnaCRM = value
                Me.m_CampagnaCRM = Nothing
                Me.DoChanged("IDCampagnaCRM", value, oldValue)
            End Set
        End Property

        Public Property CampagnaCRM As CCampagnaCRM
            Get
                If (Me.m_CampagnaCRM Is Nothing) Then Me.m_CampagnaCRM = CustomerCalls.CampagneCRM.GetItemById(Me.m_IDCampagnaCRM)
                Return Me.m_CampagnaCRM
            End Get
            Set(value As CCampagnaCRM)
                Dim oldValue As CCampagnaCRM = Me.CampagnaCRM
                If (oldValue Is value) Then Return
                Me.m_CampagnaCRM = value
                Me.m_IDCampagnaCRM = GetID(value)
                Me.DoChanged("CampagnaCRM", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCampagnaCRM(ByVal value As CCampagnaCRM)
            Me.m_CampagnaCRM = value
            Me.m_IDCampagnaCRM = GetID(value)
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
                If (Calendar.Compare(value, oldValue) = 0) Then Return
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
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_Fine = value
                Me.DoChanged("Fine", value, oldValue)
            End Set
        End Property

        Public Property NumeroTelefonateRisposte As Integer
            Get
                Return Me.m_NumeroTelefonateRisposte
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroTelefonateRisposte
                If (oldValue = value) Then Return
                Me.m_NumeroTelefonateRisposte = value
                Me.DoChanged("NumeroTelefonateRisposte", value, oldValue)
            End Set
        End Property

        Public Property NumeroTelefonateNonRisposte As Integer
            Get
                Return Me.m_NumeroTelefonateNonRisposte
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroTelefonateNonRisposte
                If (oldValue = value) Then Return
                Me.m_NumeroTelefonateRisposte = value
                Me.DoChanged("NumeroTelefonateNonRisposte", value, oldValue)
            End Set
        End Property

        Public Property MinutiConversazione As Integer
            Get
                Return Me.m_MinutiConversazione
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_MinutiConversazione
                If (oldValue = value) Then Return
                Me.m_MinutiConversazione = value
                Me.DoChanged("MinutiConversazione", value, oldValue)
            End Set
        End Property

        Public Property MinutiAttesa As Integer
            Get
                Return Me.m_MinutiAttesa
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_MinutiAttesa
                If (oldValue = value) Then Return
                Me.m_MinutiAttesa = value
                Me.DoChanged("MinutiAttesa", value, oldValue)
            End Set
        End Property

        Public Property NumeroAppuntamentiFissati As Integer
            Get
                Return Me.m_NumeroAppuntamentiFissati
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroAppuntamentiFissati
                If (oldValue = value) Then Return
                Me.m_NumeroAppuntamentiFissati = value
                Me.DoChanged("NumeroAppuntamentiFissati", value, oldValue)
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

        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.SessioniCRM.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CustomerCalls.CRM.StatsDB
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SessioneCRM"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDCampagnaCRM = reader.Read("IDCampagnaCRM", Me.m_IDCampagnaCRM)
            Me.m_IDUtente = reader.Read("IDUtente", Me.m_IDUtente)
            Me.m_NomeUtente = reader.Read("NomeUtente", Me.m_NomeUtente)
            Me.m_Inizio = reader.Read("Inizio", Me.m_Inizio)
            Me.m_Fine = reader.Read("Fine", Me.m_Fine)
            Me.m_NumeroTelefonateRisposte = reader.Read("NumeroTelefonateRisposte", Me.m_NumeroTelefonateRisposte)
            Me.m_NumeroTelefonateNonRisposte = reader.Read("NumeroTelefonateNonRisposte", Me.m_NumeroTelefonateNonRisposte)
            Me.m_MinutiConversazione = reader.Read("MinutiConversazione", Me.m_MinutiConversazione)
            Me.m_MinutiAttesa = reader.Read("MinutiAttesa", Me.m_MinutiAttesa)
            Me.m_NumeroAppuntamentiFissati = reader.Read("NumeroAppuntamentiFissati", Me.m_NumeroAppuntamentiFissati)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_dmdpage = reader.Read("dmdpage", Me.m_dmdpage)
            Me.m_IDSupervisore = reader.Read("IDSupervisore", Me.m_IDSupervisore)
            Me.m_NomeSupervisore = reader.Read("NomeSupervisore", Me.m_NomeSupervisore)
            Me.m_LastUpdated = reader.Read("LastUpdated", Me.m_LastUpdated)
            Dim tmp As String = reader.Read("Attributi", "")
            If (tmp <> "") Then Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDCampagnaCRM", Me.IDCampagnaCRM)
            writer.Write("IDUtente", Me.IDUtente)
            writer.Write("NomeUtente", Me.m_NomeUtente)
            writer.Write("Inizio", Me.m_Inizio)
            writer.Write("Fine", Me.m_Fine)
            writer.Write("NumeroTelefonateRisposte", Me.m_NumeroTelefonateRisposte)
            writer.Write("NumeroTelefonateNonRisposte", Me.m_NumeroTelefonateNonRisposte)
            writer.Write("MinutiConversazione", Me.m_MinutiConversazione)
            writer.Write("MinutiAttesa", Me.m_MinutiAttesa)
            writer.Write("NumeroAppuntamentiFissati", Me.m_NumeroAppuntamentiFissati)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("dmdpage", Me.m_dmdpage)
            writer.Write("IDSupervisore", Me.m_IDSupervisore)
            writer.Write("NomeSupervisore", Me.m_NomeSupervisore)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("LastUpdated", Me.m_LastUpdated)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("IDCampagnaCRM", Me.IDCampagnaCRM)
            writer.WriteAttribute("IDUtente", Me.IDUtente)
            writer.WriteAttribute("NomeUtente", Me.m_NomeUtente)
            writer.WriteAttribute("Inizio", Me.m_Inizio)
            writer.WriteAttribute("Fine", Me.m_Fine)
            writer.WriteAttribute("NumeroTelefonateRisposte", Me.m_NumeroTelefonateRisposte)
            writer.WriteAttribute("NumeroTelefonateNonRisposte", Me.m_NumeroTelefonateNonRisposte)
            writer.WriteAttribute("MinutiConversazione", Me.m_MinutiConversazione)
            writer.WriteAttribute("MinutiAttesa", Me.m_MinutiAttesa)
            writer.WriteAttribute("NumeroAppuntamentiFissati", Me.m_NumeroAppuntamentiFissati)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("dmdpage", Me.m_dmdpage)
            writer.WriteAttribute("IDSupervisore", Me.m_IDSupervisore)
            writer.WriteAttribute("NomeSupervisore", Me.m_NomeSupervisore)
            writer.WriteAttribute("LastUpdated", Me.m_LastUpdated)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDCampagnaCRM" : Me.m_IDCampagnaCRM = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUtente" : Me.m_IDUtente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUtente" : Me.m_NomeUtente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Inizio" : Me.m_Inizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Fine" : Me.m_Fine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "LastUpdated" : Me.m_LastUpdated = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NumeroTelefonateRisposte" : Me.m_NumeroTelefonateRisposte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroTelefonateNonRisposte" : Me.m_NumeroTelefonateNonRisposte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MinutiConversazione" : Me.m_MinutiConversazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MinutiAttesa" : Me.m_MinutiAttesa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroAppuntamentiFissati" : Me.m_NumeroAppuntamentiFissati = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "dmdpage" : Me.m_dmdpage = XML.Utils.Serializer.DeserializeString(fieldValue)
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