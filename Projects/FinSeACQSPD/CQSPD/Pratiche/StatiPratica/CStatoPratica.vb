Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Enum StatoPraticaEnum As Integer
        ''' <summary>
        ''' Pratica non ancora caricabile
        ''' </summary>
        ''' <remarks></remarks>
        STATO_PRECARICAMENTO = -10

        ''' <summary>
        ''' La pratica è nello stato iniziale (firma del contratto/SECCI)
        ''' </summary>
        ''' <remarks></remarks>
        STATO_CONTATTO = 0

        ''' <summary>
        ''' La pratica è in stato richiesta delibera
        ''' </summary>
        ''' <remarks></remarks>
        STATO_RICHIESTADELIBERA = 10

        ''' <summary>
        ''' La pratica è stata deliberata
        ''' </summary>
        ''' <remarks></remarks>
        STATO_DELIBERATA = 15

        ''' <summary>
        ''' La pratica è pronta per la liquidazione
        ''' </summary>
        ''' <remarks></remarks>
        STATO_PRONTALIQUIDAZIONE = 17

        ''' <summary>
        ''' La pratica è stata liquidata
        ''' </summary>
        ''' <remarks></remarks>
        STATO_LIQUIDATA = 20

        ''' <summary>
        ''' La pratica è stata archiviata
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ARCHIVIATA = 30

        ''' <summary>
        ''' La pratica è stata estinta anticipatamente
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ESTINTAANTICIPATAMENTE = 40

        ''' <summary>
        ''' La pratica è stata annullata
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ANNULLATA = 1000
    End Enum

    <Flags> _
    Public Enum StatoPraticaFlags As Integer
        None = 0

        ''' <summary>
        ''' Se vero indica che gli utenti normali non possono modificare l'anagrafica di una pratica in questo stato
        ''' </summary>
        ''' <remarks></remarks>
        BLOCCA_ANAGRAFICA = 1

        ''' <summary>
        ''' Se vero indica che gli utenti normali non possono modificare l'offerta di una pratica in questo stato
        ''' </summary>
        ''' <remarks></remarks>
        BLOCCA_OFFERTA = 2


        ''' <summary>
        ''' Se vero indica che una pratica che viene messa in questo stato marca come estinti gli altri prstiti che
        ''' sono flaggati come estinzioni
        ''' </summary>
        ''' <remarks></remarks>
        ACQUISISCI_ESTINZIONI = 4

        ''' <summary>
        ''' Se vero indica che una pratica che viene messa in questo stato marca come non estinti gli altri prestiti
        ''' che sono flaggati come estinzioni
        ''' </summary>
        ''' <remarks></remarks>
        RILASCIA_ESTINZIONI = 8
    End Enum


    ''' <summary>
    ''' Rappresenta uno degli stati possibili per una pratica
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CStatoPratica
        Inherits DBObject

        Private m_Nome As String              '[TEXT] Nome dello stato
        Private m_MacroStato As Nullable(Of StatoPraticaEnum) '[INT]  Valore intero di compatibilità con il vecchio sistema    
        Private m_Descrizione As String             '[TEXT] Descrizione estesa 
        'Private m_CanChangeOfferta As Boolean       '[BOOL] Se vero indica che lo stato consente di modificare i dati del prodotto
        'Private m_CanChangeAnagrafica As Boolean    'Se vero indica che lo stato consente di modificare l'anagrafica del cliente
        'Private m_Vincolante As Boolean             'Se vero indica che lo stato vincola eventuali richieste ed estinzioni alla pratica corrente
        Private m_Attivo As Boolean '[BOOL] Se vero indica che lo stato è attivo
        Private m_StatiSuccessivi As CStatoPratRulesCollection '[CStatoPratRules]  Collezione di oggetti CStatoPratRule che definiscono gli stati successivi possibili
        Private m_IDDefaultTarget As Integer '[INT]  ID dello stato di lavorazione suggerito
        Private m_DefaultTarget As CStatoPratica   'CStatopratica Oggetto che rappresenta lo stato di lavorazione successivo (opzione suggerita)
        Private m_GiorniAvviso As Integer?  'Giorni trascorsi i quali il sistema deve mostrare un avviso
        Private m_GiorniStallo As Integer? 'Giorni trascorsi i quali il sistema considera la pratica in stallo
        Private m_Flags As StatoPraticaFlags        'Flags
        Private m_Attributi As CKeyCollection

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            'Me.m_CanChangeOfferta = True
            'Me.m_CanChangeAnagrafica = True
            Me.m_Attivo = True
            Me.m_StatiSuccessivi = Nothing
            Me.m_MacroStato = StatoPraticaEnum.STATO_CONTATTO
            Me.m_IDDefaultTarget = 0
            Me.m_DefaultTarget = Nothing
            Me.m_GiorniAvviso = Nothing
            Me.m_GiorniStallo = Nothing
            'Me.m_Vincolante = False
            Me.m_Flags = StatoPraticaFlags.None
        End Sub

        Public Overrides Function GetModule() As CModule
            Return StatiPratica.Module
        End Function

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta un valore booleano che indica se lo stato vincola eventuali estinzioni e richieste alla pratica
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property Vincolante As Boolean
        '    Get
        '        Return Me.m_Vincolante
        '    End Get
        '    Set(value As Boolean)
        '        If (Me.m_Vincolante = value) Then Exit Property
        '        Me.m_Vincolante = value
        '        Me.DoChanged("Vincolante", value, Not value)
        '    End Set
        'End Property

        Public Property Flags As StatoPraticaFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As StatoPraticaFlags)
                Dim oldValue As StatoPraticaFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni trascorsi i quali il sistema emette un avviso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiorniAvviso As Integer?
            Get
                Return Me.m_GiorniAvviso
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_GiorniAvviso
                If (oldValue = value) Then Exit Property
                Me.m_GiorniAvviso = value
                Me.DoChanged("GiorniAvviso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni trascorsi i quali il sistema considera in stallo una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiorniStallo As Integer?
            Get
                Return Me.m_GiorniStallo
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_GiorniStallo
                If (oldValue = value) Then Exit Property
                Me.m_GiorniStallo = value
                Me.DoChanged("GiorniStallo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome univoco dello stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una descrizione estesa per lo stato
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
        ''' Restituisec o imposta un valore che indica se lo stato consente di modificare i dati del prodotto offerto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CanChangeOfferta As Boolean
            Get
                Return TestFlag(Me.m_Flags, StatoPraticaFlags.BLOCCA_OFFERTA) = False ' Me.m_CanChangeOfferta
            End Get
            Set(value As Boolean)
                'If (Me.m_CanChangeOfferta = value) Then Exit Property
                'Me.m_CanChangeOfferta = value
                'Me.DoChanged("CanChangeOfferta", value, Not value)
                If (Me.CanChangeOfferta = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, StatoPraticaFlags.BLOCCA_OFFERTA, Not value)
                Me.DoChanged("CanChangeOfferta", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se lo stato consente di modificare i dati dell'anagrafica del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CanChangeAnagrafica As Boolean
            Get
                Return TestFlag(Me.m_Flags, StatoPraticaFlags.BLOCCA_ANAGRAFICA) = False  ' Me.m_CanChangeAnagrafica
            End Get
            Set(value As Boolean)
                'If (Me.m_CanChangeAnagrafica = value) Then Exit Property
                'Me.m_CanChangeAnagrafica = value
                'Me.DoChanged("CanChangeAnagrafica", value, Not value)
                If (Me.CanChangeAnagrafica = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, StatoPraticaFlags.BLOCCA_OFFERTA, Not value)
                Me.DoChanged("CanChangeAnagrafica", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se una pratica nello stato corrente deve acquisire e dichiarare come estinti gli altri prestiti flaggati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AcquisisciEstinzioni As Boolean
            Get
                Return TestFlag(Me.m_Flags, StatoPraticaFlags.ACQUISISCI_ESTINZIONI) ' Me.m_CanChangeAnagrafica
            End Get
            Set(value As Boolean)
                If (Me.AcquisisciEstinzioni = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, StatoPraticaFlags.ACQUISISCI_ESTINZIONI, value)
                Me.DoChanged("AcquisisciEstinzioni", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se una pratica nello stato corrente deve rilasciare e dichiarare come non estinti gli altri prestiti flaggati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RilasciaEstinzioni As Boolean
            Get
                Return TestFlag(Me.m_Flags, StatoPraticaFlags.RILASCIA_ESTINZIONI)
            End Get
            Set(value As Boolean)
                If (Me.RilasciaEstinzioni = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, StatoPraticaFlags.RILASCIA_ESTINZIONI, value)
                Me.DoChanged("RilasciaEstinzioni", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se lo stato è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return Me.m_Attivo
            End Get
            Set(value As Boolean)
                If (Me.m_Attivo = value) Then Exit Property
                Me.m_Attivo = value
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che rappresenta la codifica dello stato nel vecchio sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MacroStato As Nullable(Of StatoPraticaEnum)
            Get
                Return Me.m_MacroStato
            End Get
            Set(value As Nullable(Of StatoPraticaEnum))
                Dim oldValue As Nullable(Of StatoPraticaEnum) = Me.m_MacroStato
                If (oldValue = value) Then Exit Property
                Me.m_MacroStato = value
                Me.DoChanged("OldStatus", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dello stato successivo (suggerito)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDDefaultTarget As Integer
            Get
                Return GetID(Me.m_DefaultTarget, Me.m_IDDefaultTarget)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDefaultTarget
                If oldValue = value Then Exit Property
                Me.m_IDDefaultTarget = value
                Me.m_DefaultTarget = Nothing
                Me.DoChanged("IDDefaultTarget", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato successivo predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultTarget As CStatoPratica
            Get
                If Me.m_DefaultTarget Is Nothing Then Me.m_DefaultTarget = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDDefaultTarget)
                Return Me.m_DefaultTarget
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.m_DefaultTarget
                If (oldValue = value) Then Exit Property
                Me.m_DefaultTarget = value
                Me.m_IDDefaultTarget = GetID(value)
                Me.DoChanged("DefaultTarget", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una collezione di regole che definiscono il passaggio agli stati successivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatiSuccessivi As CStatoPratRulesCollection
            Get
                SyncLock Me
                    If Me.m_StatiSuccessivi Is Nothing Then
                        Me.m_StatiSuccessivi = New CStatoPratRulesCollection
                        Me.m_StatiSuccessivi.Load(Me)
                    End If
                    Return Me.m_StatiSuccessivi
                End SyncLock
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheSTS"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            If (Not ret AndAlso Me.m_StatiSuccessivi IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_StatiSuccessivi)
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If ret And Not (Me.m_StatiSuccessivi Is Nothing) Then Me.m_StatiSuccessivi.Save(force)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_MacroStato = reader.Read("OldStatus", Me.m_MacroStato)
            Me.m_IDDefaultTarget = reader.Read("IDDefaultTarget", Me.m_IDDefaultTarget)
            'Me.m_CanChangeOfferta = reader.Read("CanChangeOfferta", Me.m_CanChangeOfferta)
            'Me.m_CanChangeAnagrafica = reader.Read("CanChangeAnagrafica", Me.m_CanChangeAnagrafica)
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Me.m_GiorniAvviso = reader.Read("GiorniAvviso", Me.m_GiorniAvviso)
            Me.m_GiorniStallo = reader.Read("GiorniStallo", Me.m_GiorniStallo)
            'Me.m_Vincolante = reader.Read("Vincolante", Me.m_Vincolante)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("OldStatus", Me.m_MacroStato)
            writer.Write("IDDefaultTarget", Me.IDDefaultTarget)
            ' writer.Write("CanChangeOfferta", Me.m_CanChangeOfferta)
            ' writer.Write("CanChangeAnagrafica", Me.m_CanChangeOfferta)
            writer.Write("Attivo", Me.m_Attivo)
            writer.Write("GiorniAvviso", Me.m_GiorniAvviso)
            writer.Write("GiorniStallo", Me.m_GiorniStallo)
            ' writer.Write("Vincolante", Me.m_Vincolante)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("MacroStato", Me.m_MacroStato)
            writer.WriteAttribute("IDDefaultTarget", Me.IDDefaultTarget)
            'writer.WriteAttribute("CanChangeOfferta", Me.m_CanChangeOfferta)
            ' writer.WriteAttribute("CanChangeAnagrafica", Me.m_CanChangeAnagrafica)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("GiorniAvviso", Me.m_GiorniAvviso)
            writer.WriteAttribute("GiorniStallo", Me.m_GiorniStallo)
            'writer.WriteAttribute("Vincolante", Me.m_Vincolante)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            writer.WriteTag("StatiSuccessivi", Me.StatiSuccessivi)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MacroStato" : Me.m_MacroStato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDefaultTarget" : Me.m_IDDefaultTarget = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "CanChangeOfferta" : Me.m_CanChangeOfferta = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                    'Case "CanChangeAnagrafica" : Me.m_CanChangeAnagrafica = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "GiorniAvviso" : Me.m_GiorniAvviso = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "GiorniStallo" : Me.m_GiorniStallo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "Vincolante" : Me.m_Vincolante = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatiSuccessivi"
                    Me.m_StatiSuccessivi = CType(fieldValue, CStatoPratRulesCollection)
                    Me.m_StatiSuccessivi.SetOwner(Me)
                Case "Attributi"
                    Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Property Colore As String
            Get
                Return Me.Attributi.GetItemByKey("Colore")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.Colore
                If (oldValue = value) Then Exit Property
                Me.Attributi.SetItemByKey("Colore", value)
                Me.DoChanged("Colore", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.StatiPratica.UpdateCached(Me)
        End Sub

    End Class

End Class
