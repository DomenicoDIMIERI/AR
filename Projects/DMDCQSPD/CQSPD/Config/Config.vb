Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Evento generato quando viene modificata la configurazione 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event ConfigurationChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    <Flags> _
    Public Enum CQSPDConfigFlags As Integer
        None = 0

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le richiesta
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITRICHIESTA = 1

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITPROPOSTA = 2

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le pratiche
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITPRATICA = 4

        ''' <summary>
        ''' Consenti agli utenti che non dispongono del diritto change_status di modificare gli altri prestiti
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSRE_ALLOWEDITALTRIPRESTITI = 8

        ''' <summary>
        ''' Consente la correzione della decorrenza delle proposte
        ''' </summary>
        ''' <remarks></remarks>
        NORMALUSER_ALLOWEDITDECORRENZAPROP = 16

        ''' <summary>
        ''' Indica al sistema che deve essere effettuata l'archiviazione automatica delle pratiche liquidate dopo N giorni
        ''' </summary>
        ''' <remarks></remarks>
        ARCHIVIA_AUTOMATICAMENTE = 32

        ''' <summary>
        ''' Indica al sistema di utilizzare l'interfaccia multicessionario
        ''' </summary>
        ''' <remarks></remarks>
        SISTEMA_MULTICESSIONARIO = 64

        ''' <summary>
        ''' Consente di proporre al cliente degli studi di fattibilità bypassando il controllo del supervisore.
        ''' La proposta sarà comunque inviata al supervisore se lo studio di fattibilità viene convertito in pratica.
        ''' </summary>
        ''' <remarks></remarks>
        CONSENTI_PROPOSTE_NONAPPROVATE = 256

    End Enum

    ''' <summary>
    ''' Configurazione del modulo CreditoV
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCQSPDConfig
        Inherits DBObject
        Implements ISettingsOwner

        Private m_Flags As CQSPDConfigFlags
        Private m_ValoreMinimoUpfront As Decimal
        Private m_SogliaNotificaUpFront As Decimal
        Private m_GiorniAnticipoRifin As Integer
        Private m_PercCompletamentoRifn As Double
        Private m_IDStatoPredefinito As Integer
        Private m_StatoPredefinito As CStatoPratica
        Private m_FromAddress As String
        Private m_FromDiplayName As String
        Private m_NotifyDeleteTo As String
        Private m_NotifyChangesTo As String
        Private m_WatchSubject As String
        Private m_WatchTemplate As String
        Private m_DeleteSubject As String
        Private m_DeleteTemplate As String
        Private m_NotifyWarningsTo As String
        Private m_NotifyRichiesteApprovazioneTo As String
        Private m_TemplateRichiesteApprovazione As String
        Private m_TemplateConfermaApprovazione As String
        Private m_TemplateNegaApprovazione As String
        Private m_TemplateChangeStatus As String
        Private m_ChangeStatusSubject As String
        Private m_ReqApprSubject As String
        Private m_ApprSubject As String
        Private m_NegaSubject As String
        Private m_NotificaSogliaSubject As String
        Private m_NotificaSogliaTemplate As String
        Private m_NotificaSogliaTo As String
        Private m_PraticaCreataSubject As String
        Private m_PraticaCreataTemplate As String
        Private m_BCC As String
        Private m_PraticaAnnullataSubject As String
        Private m_PraticaAnnullataTemplate As String

        Private m_ConsulenzaInseritaSubject As String
        Private m_ConsulenzaInseritaTemplate As String
        Private m_ConsulenzaAccettataSubject As String
        Private m_ConsulenzaAccettataTemplate As String
        Private m_ConsulenzaBocciataSubject As String
        Private m_ConsulenzaBocciataTemplate As String
        Private m_ConsulenzaPropostaSubject As String
        Private m_ConsulenzaPropostaTemplate As String
        Private m_ConsulenzaRifiutataSubject As String
        Private m_ConsulenzaRifiutataTemplate As String
        Private m_ConsulenzaRichiestaApprovazioneSubject As String
        Private m_ConsulenzaRichiestaApprovazioneTemplate As String
        Private m_ConsulenzaConfermaApprovazioneSubject As String
        Private m_ConsulenzaConfermaApprovazioneTemplate As String
        Private m_ConsulenzaNegaApprovazioneSubject As String
        Private m_ConsulenzaNegaApprovazioneTemplate As String

        Private m_RichiestaFinanziamentoSubject As String
        Private m_RichiestaFinanziamentoTemplate As String

        Private m_GiorniArchiviazione As Integer
        Private m_UltimaArchiviazione As Nullable(Of Date)

        Private m_Overflow As CSettings

        Public Sub New()
            Me.m_Flags = CQSPDConfigFlags.None
            Me.m_ValoreMinimoUpfront = 100
            Me.m_SogliaNotificaUpFront = 150
            Me.m_GiorniAnticipoRifin = 150
            Me.m_PercCompletamentoRifn = 40
            Me.m_IDStatoPredefinito = 0
            Me.m_StatoPredefinito = Nothing
            Me.m_FromAddress = ""
            Me.m_FromDiplayName = ""
            Me.m_NotifyDeleteTo = ""
            Me.m_NotifyChangesTo = ""
            Me.m_WatchSubject = ""
            Me.m_WatchTemplate = ""
            Me.m_DeleteSubject = ""
            Me.m_DeleteTemplate = ""
            Me.m_NotifyWarningsTo = ""
            Me.m_NotifyRichiesteApprovazioneTo = ""
            Me.m_TemplateRichiesteApprovazione = ""
            Me.m_TemplateConfermaApprovazione = ""
            Me.m_TemplateNegaApprovazione = ""
            Me.m_TemplateChangeStatus = ""
            Me.m_ChangeStatusSubject = ""
            Me.m_ReqApprSubject = ""
            Me.m_ApprSubject = ""
            Me.m_NegaSubject = ""
            Me.m_NotificaSogliaSubject = ""
            Me.m_NotificaSogliaTemplate = ""
            Me.m_NotificaSogliaTo = ""
            Me.m_PraticaCreataSubject = ""
            Me.m_PraticaCreataTemplate = ""
            Me.m_BCC = ""
            Me.m_PraticaAnnullataSubject = ""
            Me.m_PraticaAnnullataTemplate = ""
            Me.m_ConsulenzaInseritaSubject = ""
            Me.m_ConsulenzaInseritaTemplate = ""
            Me.m_ConsulenzaAccettataSubject = ""
            Me.m_ConsulenzaAccettataTemplate = ""
            Me.m_ConsulenzaBocciataSubject = ""
            Me.m_ConsulenzaBocciataTemplate = ""
            Me.m_ConsulenzaPropostaSubject = ""
            Me.m_ConsulenzaPropostaTemplate = ""
            Me.m_ConsulenzaRifiutataSubject = ""
            Me.m_ConsulenzaRifiutataTemplate = ""
            Me.m_ConsulenzaRichiestaApprovazioneSubject = ""
            Me.m_ConsulenzaRichiestaApprovazioneTemplate = ""
            Me.m_ConsulenzaConfermaApprovazioneSubject = ""
            Me.m_ConsulenzaConfermaApprovazioneTemplate = ""
            Me.m_ConsulenzaNegaApprovazioneSubject = ""
            Me.m_ConsulenzaNegaApprovazioneTemplate = ""
            Me.m_RichiestaFinanziamentoSubject = ""
            Me.m_RichiestaFinanziamentoTemplate = ""

            Me.m_GiorniArchiviazione = 5
            Me.m_UltimaArchiviazione = Nothing
            Me.m_Overflow = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property ConsentiProposteSenzaSupervisore As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.CONSENTI_PROPOSTE_NONAPPROVATE)
            End Get
            Set(value As Boolean)
                If (Me.ConsentiProposteSenzaSupervisore = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.Flags, CQSPDConfigFlags.CONSENTI_PROPOSTE_NONAPPROVATE, value)
                Me.DoChanged("ConentiProposteSenzaSupervisore", value, Not value)
            End Set
        End Property

        Public ReadOnly Property Overflow As CSettings Implements ISettingsOwner.Settings
            Get
                SyncLock Me
                    If (Me.m_Overflow Is Nothing) Then Me.m_Overflow = New CSettings(Me)
                    Return Me.m_Overflow
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica al sistema se deve essere utilizzata l'interfaccia per i sistema multicessionari
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SistemaMulticessionario As Boolean
            Get
                Return TestFlag(Me.m_Flags, CQSPDConfigFlags.SISTEMA_MULTICESSIONARIO)
            End Get
            Set(value As Boolean)
                If (Me.SistemaMulticessionario = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, CQSPDConfigFlags.SISTEMA_MULTICESSIONARIO, value)
                Me.DoChanged("SistemaMulticessionario", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultima archiviazione automatica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UltimaArchiviazione As Nullable(Of Date)
            Get
                Return Me.m_UltimaArchiviazione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_UltimaArchiviazione
                If (oldValue = value) Then Exit Property
                Me.m_UltimaArchiviazione = value
                Me.DoChanged("UltimaArchiviazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve archiviare automaticamente le pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ArchiviaAutomaticamente As Boolean
            Get
                Return TestFlag(Me.m_Flags, CQSPDConfigFlags.ARCHIVIA_AUTOMATICAMENTE)
            End Get
            Set(value As Boolean)
                If (Me.ArchiviaAutomaticamente = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, CQSPDConfigFlags.ARCHIVIA_AUTOMATICAMENTE, value)
                Me.DoChanged("ArchiviaAutomaticamente", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni dopo cui archiviare automaticamente la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiorniArchiviazione As Integer
            Get
                Return Me.m_GiorniArchiviazione
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_GiorniArchiviazione
                If (oldValue = value) Then Exit Property
                Me.m_GiorniArchiviazione = value
                Me.DoChanged("GiorniArchiviazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As CQSPDConfigFlags
            Get
                Return Me.m_Flags  'DMD.CQSPD.Pratiche.Module.Settings.GetValueInt("Flags", 0)
            End Get
            Set(value As CQSPDConfigFlags)
                Dim oldValue As CQSPDConfigFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                'DMD.CQSPD.Pratiche.Module.Settings.SetValueInt("Flags", CInt(value))
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le richieste di finanziamento già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditRichieste As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITRICHIESTA)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditRichieste = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITRICHIESTA, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditProposte As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPROPOSTA)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditProposte = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPROPOSTA, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditPratiche As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPRATICA)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditPratiche = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPRATICA, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToEditAltriPrestiti As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSRE_ALLOWEDITALTRIPRESTITI)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToEditAltriPrestiti = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSRE_ALLOWEDITALTRIPRESTITI, value)
            End Set
        End Property

        ''' <summary>
        ''' Consente agli utenti normali di modificare la decorrenza delle proposte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNormalUsersToCorrectDecorrenzaProposte As Boolean
            Get
                Return TestFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITDECORRENZAPROP)
            End Get
            Set(value As Boolean)
                If (Me.AllowNormalUsersToCorrectDecorrenzaProposte = value) Then Exit Property
                Me.Flags = SetFlag(Me.Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITDECORRENZAPROP, value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore minimo caricabile per l'UpFront da parte di un utente normale.
        ''' Sotto questo valore solo gli operatori con il diritto change_status possono caricare le offerte 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreMinimoUpFront As Decimal
            Get
                Return Me.m_ValoreMinimoUpfront ' DMD.CQSPD.Pratiche.Module.Settings.GetValueDouble("ValoreMinimoUpfront", 100)
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_ValoreMinimoUpfront
                If (oldValue = value) Then Exit Property
                Me.m_ValoreMinimoUpfront = value ' DMD.CQSPD.Pratiche.Module.Settings.SetValueDouble("ValoreMinimoUpfront", value)
                Me.DoChanged("ValoreMinimoUpFront", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la soglia sotto la quale viene generata una codinzione di attenzione per la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SogliaNotificaUpFront As Decimal
            Get
                Return Me.m_SogliaNotificaUpFront
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SogliaNotificaUpFront
                If (oldValue = value) Then Exit Property
                Me.m_SogliaNotificaUpFront = value
                Me.DoChanged("SogliaNotificaUpFront", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni da sottrarre alla data in cui un prestito diventa rifinanziabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiorniAnticipoRifin As Integer
            Get
                Return Me.m_GiorniAnticipoRifin ' DMD.CQSPD.Pratiche.Module.Settings.GetValueInt("GiorniAnticipoRifin", 90)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_GiorniAnticipoRifin '
                If (oldValue = value) Then Exit Property
                Me.m_GiorniAnticipoRifin = value '  DMD.CQSPD.Pratiche.Module.Settings.SetValueInt("GiorniAnticipoRifin", value)
                Me.DoChanged("GiorniAnticipoRifin", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imosta il tempo (in percentuale rispetto alla durata) in cui un prestito diventa rifinanziabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PercCompletamentoRifn As Double
            Get
                Return Me.m_PercCompletamentoRifn ' DMD.CQSPD.Pratiche.Module.Settings.GetValueDouble("PercCompletamentoRifin", 40)
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_PercCompletamentoRifn
                If (oldValue = value) Then Exit Property
                Me.m_PercCompletamentoRifn = value '                DMD.CQSPD.Pratiche.Module.Settings.SetValueDouble("PercCompletamentoRifin", value)
                Me.DoChanged("PercCompletamentoRifn", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dello stato predefinito (quando viene caricata una nuova pratica)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStatoPredefinito As Integer
            Get
                Return GetID(Me.m_StatoPredefinito, Me.m_IDStatoPredefinito) ' DMD.CQSPD.Pratiche.Module.Settings.GetValueInt("IDStatoPredefinito", 0)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoPredefinito
                If (oldValue = value) Then Exit Property '                If (GetID(Me.m_StatoPredefinito) = value) Then Exit Property
                Me.m_IDStatoPredefinito = value '  DMD.CQSPD.Pratiche.Module.Settings.SetValueInt("IDStatoPredefinito", value)
                Me.m_StatoPredefinito = Nothing
                Me.DoChanged("IDStatoPredefinito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituiscec o imposta lo stato predefinito per le nuove pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoPredefinito As CStatoPratica
            Get
                If (Me.m_StatoPredefinito Is Nothing) Then Me.m_StatoPredefinito = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDStatoPredefinito)
                Return Me.m_StatoPredefinito
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.StatoPredefinito
                If (oldValue Is value) Then Exit Property
                Me.m_StatoPredefinito = value
                Me.m_IDStatoPredefinito = GetID(value)
                Me.DoChanged("StatoPredefinito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo da cui vengono inviate le email provenienti da questo modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BCC As String
            Get
                Return Me.m_BCC
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_BCC
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_BCC = value
                Me.DoChanged("BCC", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo da cui vengono inviate le email provenienti da questo modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromAddress As String
            Get
                Return Me.m_FromAddress ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("FromAddress")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FromAddress
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_FromAddress = value '                DMD.CQSPD.Pratiche.Module.Settings.SetValueString("FromAddress", Trim(value))
                Me.DoChanged("FromAddress", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome visualizzato come mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromDisplayName As String
            Get
                Return Me.m_FromDiplayName  ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("FromAddress")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FromDiplayName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_FromDiplayName = value '                DMD.CQSPD.Pratiche.Module.Settings.SetValueString("FromAddress", Trim(value))
                Me.DoChanged("FromDiplayName", value, oldValue)
            End Set
        End Property

        ' ''' <summary>
        ' ''' Elenco di indirizzi, separati da , di indirizzi a cui notificare l'eliminazione delle pratiche
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property NotifyDeleteTo As String
        '    Get
        '        Return Me.m_NotifyDeleteTo ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyDeleteTo")
        '    End Get
        '    Set(value As String)
        '        Dim oldValue As String = Me.m_NotifyDeleteTo
        '        value = Strings.Trim(value)
        '        If (oldValue = value) Then Exit Property
        '        Me.m_NotifyDeleteTo = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyDeleteTo", Trim(value))
        '        Me.DoChanged("NotifyDeleteTo", value, oldValue)
        '    End Set
        'End Property

        ''' <summary>
        ''' Elenco di indirizzi separati da "," a cui inviare gli avvisi generati per le pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyChangesTo As String
            Get
                Return Me.m_NotifyChangesTo ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyChangesTo")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NotifyChangesTo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NotifyChangesTo = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyChangesTo", Trim(value))
                Me.DoChanged("NotifyChangesTo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail che viene inviata per notificare le condizioni di avviso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property WatchSubject As String
            Get
                Return Me.m_WatchSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("WatchSubject")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_WatchSubject
                If (oldValue = value) Then Exit Property
                Me.m_WatchSubject = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("WatchSubject", Trim(value))
                Me.DoChanged("WatchSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modello utilizzato per le email inviate per notificare delle condizioni di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property WatchTemplate As String
            Get
                Return Me.m_WatchTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_WatchTemplate
                If (oldValue = value) Then Exit Property
                Me.m_WatchTemplate = value
                Me.DoChanged("WatchTemplat", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'inserimento di una nuova richiesta di finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaFinanziamentoSubject As String
            Get
                Return Me.m_RichiestaFinanziamentoSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_RichiestaFinanziamentoSubject
                If (oldValue = value) Then Exit Property
                Me.m_RichiestaFinanziamentoSubject = value
                Me.DoChanged("RichiestaFinanziamentoSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail utilizzata per notificare l'inserimento di una nuova richiesta di finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaFinanziamentoTemplate As String
            Get
                Return Me.m_RichiestaFinanziamentoTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_RichiestaFinanziamentoTemplate
                If (oldValue = value) Then Exit Property
                Me.m_RichiestaFinanziamentoTemplate = value
                Me.DoChanged("RichiestaFinanziamentoTemplate", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail che viene inviata per notificare l'eliminazione di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteSubject As String
            Get
                Return Me.m_DeleteSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("DeleteSubject")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DeleteSubject
                If (oldValue = value) Then Exit Property
                Me.m_DeleteSubject = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("DeleteSubject", Trim(value))
                Me.DoChanged("DeleteSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del modello di mail utilizzato per notificare l'eliminazione di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteTemplate As String
            Get
                Return Me.m_DeleteTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DeleteTemplate
                If (oldValue = value) Then Exit Property
                Me.m_DeleteTemplate = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("DeleteSubject", Trim(value))
                Me.DoChanged("DeleteTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli indirizzi email a cui notificare le modifiche fatte alle pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyWarningsTo As String
            Get
                Return Me.m_NotifyChangesTo ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyWarningsTo")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NotifyChangesTo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NotifyChangesTo = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyWarningsTo", Trim(value))
                Me.DoChanged("NotifyWarningsTo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli indirizzi email a cui inviare le richieste di approvazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyRichiesteApprovazioneTo As String
            Get
                Return Me.m_NotifyRichiesteApprovazioneTo ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyConfirmsTo")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NotifyRichiesteApprovazioneTo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NotifyRichiesteApprovazioneTo = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotifyConfirmsTo", Trim(value))
                Me.DoChanged("NotifyRichiesteApprovazioneTo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template della mail inviata per notificare le richieste di approvazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateRichiesteApprovazione As String
            Get
                Return Me.m_TemplateRichiesteApprovazione ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateRichAppr")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateRichAppr", Trim(value))
                Dim oldValue As String = Me.m_TemplateRichiesteApprovazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TemplateRichiesteApprovazione = value
                Me.DoChanged("TemplateRichiesteApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template della mail che viene inviata per notificare l'approvazione di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateConfermaApprovazione As String
            Get
                Return Me.m_TemplateConfermaApprovazione ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateConfAppr")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TemplateConfermaApprovazione
                If (oldValue = value) Then Exit Property
                Me.m_TemplateConfermaApprovazione = value 'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateConfAppr", Trim(value))
                Me.DoChanged("TemplateConfermaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template usato per le email inviate per notificare la mancata approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateNegaApprovazione As String
            Get
                Return Me.m_TemplateNegaApprovazione ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateNegaAppr")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateNegaAppr", Trim(value))
                Dim oldValue As String = Me.m_TemplateNegaApprovazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TemplateNegaApprovazione = value
                Me.DoChanged("TemplateNegaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del template utilizzato per le email inviate per notificare la modifica di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplateChangeStatus As String
            Get
                Return Me.m_TemplateChangeStatus ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateChangeStatus")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("TemplateChangeStatus", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TemplateChangeStatus
                If (oldValue = value) Then Exit Property
                Me.m_TemplateChangeStatus = value
                Me.DoChanged("TemplateChangeStatus", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare le modifiche fatte alle pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChangeStatusSubject As String
            Get
                Return Me.m_ChangeStatusSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("ChangeStatusSubject")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("ChangeStatusSubject", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ChangeStatusSubject
                If (oldValue = value) Then Exit Property
                Me.m_ChangeStatusSubject = value
                Me.DoChanged("ChangeStatusSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per le richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReqApprSubject As String
            Get
                Return Me.m_ReqApprSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("ReqApprSubject")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("ReqApprSubject", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ReqApprSubject
                If (oldValue = value) Then Exit Property
                Me.m_ReqApprSubject = value
                Me.DoChanged("ReqApprSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o impsota il soggetto delle mail inviate per notificare l'approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ApprSubject As String
            Get
                Return Me.m_ApprSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("ApprSubject")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("ApprSubject", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ApprSubject
                If (oldValue = value) Then Exit Property
                Me.m_ApprSubject = value
                Me.DoChanged("ApprSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per negare una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NegaSubject As String
            Get
                Return Me.m_NegaSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NegaSubject")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NegaSubject", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NegaSubject
                If (oldValue = value) Then Exit Property
                Me.m_NegaSubject = value
                Me.DoChanged("NegaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare le proposte caricate sotto la soglia minima dell'UpFront
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotificaSogliaSubject As String
            Get
                Return Me.m_NotificaSogliaSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotificaSogliaSubject")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotificaSogliaSubject", Trim(value))
                Dim oldValue As String = Me.m_NotificaSogliaSubject
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NotificaSogliaSubject = value
                Me.DoChanged("NotificaSogliaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del templete utilizzato per le email inviate per notificare una proposta caricata sotto la soglia minima dell'UpFront
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotificaSogliaTemplate As String
            Get
                Return Me.m_NotificaSogliaTemplate ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotificaSogliaTemplate")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotificaSogliaTemplate", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NotificaSogliaTemplate
                If (oldValue = value) Then Exit Property
                Me.m_NotificaSogliaTemplate = value
                Me.DoChanged("NotificaSogliaTemplate", value, oldValue)
            End Set
        End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta l'elenco degli indirizzi email (separati da ,) a cui notificare il caricamento di una proposta sotto la soglia minima dell'UpFront
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property NotificaSogliaTo As String
        '    Get
        '        Return Me.m_NotificaSogliaTo ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotificaSogliaTo")
        '    End Get
        '    Set(value As String)
        '        'DMD.CQSPD.Pratiche.Module.Settings.GetValueString("NotificaSogliaTo", Trim(value))
        '        value = Strings.Trim(value)
        '        Dim oldValue As String = Me.m_NotificaSogliaTo
        '        If (oldValue = value) Then Exit Property
        '        Me.m_NotificaSogliaTo = value
        '        Me.DoChanged("NotificaSogliaTo", value, oldValue)
        '    End Set
        'End Property



        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare la creazione di una nuova pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaCreataSubject() As String
            Get
                Return Me.m_PraticaCreataSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("PraticaCreataSubject", "")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.SetValueString("PraticaCreataSubject", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_PraticaCreataSubject
                If (oldValue = value) Then Exit Property
                Me.m_PraticaCreataSubject = value
                Me.DoChanged("PraticaCreataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare la creazione di una nuova pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaCreataTemplate() As String
            Get
                Return Me.m_PraticaCreataTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_PraticaCreataTemplate
                If (oldValue = value) Then Exit Property
                Me.m_PraticaCreataSubject = value
                Me.DoChanged("PraticaCreataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'annullamento di una pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaAnnullataSubject() As String
            Get
                Return Me.m_PraticaAnnullataSubject ' DMD.CQSPD.Pratiche.Module.Settings.GetValueString("PraticaCreataSubject", "")
            End Get
            Set(value As String)
                'DMD.CQSPD.Pratiche.Module.Settings.SetValueString("PraticaCreataSubject", Trim(value))
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_PraticaAnnullataSubject
                If (oldValue = value) Then Exit Property
                Me.m_PraticaAnnullataSubject = value
                Me.DoChanged("PraticaAnnullataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'annullamento di una nuova pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PraticaAnnullataTemplate() As String
            Get
                Return Me.m_PraticaAnnullataTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_PraticaAnnullataTemplate
                If (oldValue = value) Then Exit Property
                Me.m_PraticaAnnullataSubject = value
                Me.DoChanged("PraticaAnnullataTemplate", value, oldValue)
            End Set
        End Property

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)

            Me.Overflow.SetValueString("ConsulenzaRichiestaApprovazioneSubject", Me.m_ConsulenzaRichiestaApprovazioneSubject)
            Me.Overflow.SetValueString("ConsulenzaRichiestaApprovazioneTemplate", Me.m_ConsulenzaRichiestaApprovazioneTemplate)
            Me.Overflow.SetValueString("ConsulenzaConfermaApprovazioneSubject", Me.m_ConsulenzaConfermaApprovazioneSubject)
            Me.Overflow.SetValueString("ConsulenzaConfermaApprovazioneTemplate", Me.m_ConsulenzaConfermaApprovazioneTemplate)
            Me.Overflow.SetValueString("ConsulenzaNegaApprovazioneSubject", Me.m_ConsulenzaNegaApprovazioneSubject)
            Me.Overflow.SetValueString("ConsulenzaNegaApprovazioneTemplate", Me.m_ConsulenzaNegaApprovazioneTemplate)

            If (ret) Then
                'CQSPD.Pratiche.Module.Settings.Save(True)
                SetConfiguration(Me)
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare l'inserimento di un nuovo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaInseritaSubject As String
            Get
                Return Me.m_ConsulenzaInseritaSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaInseritaSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaInseritaSubject = value
                Me.DoChanged("ConsulenzaInseritaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso del modello della mail utilizzata per notificare l'inserimento di un nuovo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaInseritaTemplate As String
            Get
                Return Me.m_ConsulenzaInseritaTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaInseritaTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaInseritaTemplate = value
                Me.DoChanged("ConsulenzaInseritaTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata quando un cliente accetta uno studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaAccettataSubject As String
            Get
                Return Me.m_ConsulenzaAccettataSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaAccettataSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaAccettataSubject = value
                Me.DoChanged("ConsulenzaAccettataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata quando un cliente accetta uno studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaAccettataTemplate As String
            Get
                Return Me.m_ConsulenzaAccettataTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaAccettataTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaAccettataTemplate = value
                Me.DoChanged("ConsulenzaAccettataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata quando un operatore boccia lo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaBocciataSubject As String
            Get
                Return Me.m_ConsulenzaBocciataSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaBocciataSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaBocciataSubject = value
                Me.DoChanged("ConsulenzaBocciataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata quando un operatore boccia uno studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaBocciataTemplate As String
            Get
                Return Me.m_ConsulenzaBocciataTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaBocciataTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaBocciataTemplate = value
                Me.DoChanged("ConsulenzaBocciataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per notificare che un operatore ha proposto uno studio di fattibilità ad un cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaPropostaSubject As String
            Get
                Return Me.m_ConsulenzaPropostaSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaPropostaSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaPropostaSubject = value
                Me.DoChanged("ConsulenzaPropostaSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare che un operatore ha proposto uno studio di fattibilità ad un cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaPropostaTemplate As String
            Get
                Return Me.m_ConsulenzaPropostaTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaPropostaTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaPropostaTemplate = value
                Me.DoChanged("ConsulenzaPropostaTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto della mail inviata per noficiare che un cliente ha rifiutato uno studio di fattibilità proposto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRifiutataSubject As String
            Get
                Return Me.m_ConsulenzaRifiutataSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaRifiutataSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaRifiutataSubject = value
                Me.DoChanged("ConsulenzaRifiutataSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare che un cliente ha rifiutato uno studio di fattibilità proposto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRifiutataTemplate As String
            Get
                Return Me.m_ConsulenzaRifiutataTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaRifiutataTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaRifiutataTemplate = value
                Me.DoChanged("ConsulenzaRifiutataTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto delle mail inviate per notificare le richieste di approvazione per una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRichiestaApprovazioneSubject As String
            Get
                Return Me.m_ConsulenzaRichiestaApprovazioneSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaRichiestaApprovazioneSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaRichiestaApprovazioneSubject = value
                Me.DoChanged("ConsulenzaRichiestaApprovazioneSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare le richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaRichiestaApprovazioneTemplate As String
            Get
                Return Me.m_ConsulenzaRichiestaApprovazioneTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaRichiestaApprovazioneTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaRichiestaApprovazioneTemplate = value
                Me.DoChanged("ConsulenzaRichiestaApprovazioneTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto delle mail inviate per notificare le autorizzazioni delle richieste di approvazione per una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaConfermaApprovazioneSubject As String
            Get
                Return Me.m_ConsulenzaConfermaApprovazioneSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaConfermaApprovazioneSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaConfermaApprovazioneSubject = value
                Me.DoChanged("ConsulenzaConfermaApprovazioneSubject ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare le autorizzazioni delle  richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaConfermaApprovazioneTemplate As String
            Get
                Return Me.m_ConsulenzaConfermaApprovazioneTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaConfermaApprovazioneTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaConfermaApprovazioneTemplate = value
                Me.DoChanged("ConsulenzaConfermaApprovazioneTemplate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il soggetto delle mail inviate per notificare le negazioni delle richieste di approvazione per una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaNegaApprovazioneSubject As String
            Get
                Return Me.m_ConsulenzaNegaApprovazioneSubject
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaNegaApprovazioneSubject
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaNegaApprovazioneSubject = value
                Me.DoChanged("ConsulenzaNegaApprovazioneSubject ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il template della mail inviata per notificare le negazioni delle  richieste di approvazione di una proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsulenzaNegaApprovazioneTemplate As String
            Get
                Return Me.m_ConsulenzaNegaApprovazioneTemplate
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ConsulenzaNegaApprovazioneTemplate
                If (oldValue = value) Then Exit Property
                Me.m_ConsulenzaNegaApprovazioneTemplate = value
                Me.DoChanged("ConsulenzaNegaApprovazioneTemplate", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConfig"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_ValoreMinimoUpfront = reader.Read("ValoreMinimoUpfront", Me.m_ValoreMinimoUpfront)
            Me.m_SogliaNotificaUpFront = reader.Read("SogliaNotificaUpFront", Me.m_SogliaNotificaUpFront)
            Me.m_GiorniAnticipoRifin = reader.Read("GiorniAnticipoRifin", Me.m_GiorniAnticipoRifin)
            Me.m_PercCompletamentoRifn = reader.Read("PercCompletamentoRifn", Me.m_PercCompletamentoRifn)
            Me.m_IDStatoPredefinito = reader.Read("IDStatoPredefinito", Me.m_IDStatoPredefinito)
            Me.m_FromAddress = reader.Read("FromAddress", Me.m_FromAddress)
            Me.m_FromDiplayName = reader.Read("FromDisplayName", Me.m_FromDiplayName)
            Me.m_NotifyDeleteTo = reader.Read("NotifyDeleteTo", Me.m_NotifyDeleteTo)
            Me.m_NotifyChangesTo = reader.Read("NotifyChangesTo", Me.m_NotifyChangesTo)
            Me.m_NotifyChangesTo = reader.Read("NotifyChangesTo", Me.m_NotifyChangesTo)
            Me.m_WatchSubject = reader.Read("WatchSubject", Me.m_WatchSubject)
            Me.m_WatchTemplate = reader.Read("WatchTemplate", Me.m_WatchTemplate)
            Me.m_DeleteSubject = reader.Read("DeleteSubject", Me.m_DeleteSubject)
            Me.m_DeleteTemplate = reader.Read("DeleteTemplate", Me.m_DeleteTemplate)
            Me.m_NotifyWarningsTo = reader.Read("NotifyWarningsTo", Me.m_NotifyWarningsTo)
            Me.m_NotifyRichiesteApprovazioneTo = reader.Read("NotifyRichiesteApprovazioneTo", Me.m_NotifyRichiesteApprovazioneTo)
            Me.m_TemplateRichiesteApprovazione = reader.Read("TemplateRichiesteApprovazione", Me.m_TemplateRichiesteApprovazione)
            Me.m_TemplateConfermaApprovazione = reader.Read("TemplateConfermaApprovazione", Me.m_TemplateConfermaApprovazione)
            Me.m_TemplateNegaApprovazione = reader.Read("TemplateNegaApprovazione", Me.m_TemplateNegaApprovazione)
            Me.m_TemplateChangeStatus = reader.Read("TemplateChangeStatus", Me.m_TemplateChangeStatus)
            Me.m_ChangeStatusSubject = reader.Read("ChangeStatusSubject", Me.m_ChangeStatusSubject)
            Me.m_ReqApprSubject = reader.Read("ReqApprSubject", Me.m_ReqApprSubject)
            Me.m_ApprSubject = reader.Read("ApprSubject", Me.m_ApprSubject)
            Me.m_NegaSubject = reader.Read("NegaSubject", Me.m_NegaSubject)
            Me.m_NotificaSogliaSubject = reader.Read("NotificaSogliaSubject", Me.m_NotificaSogliaSubject)
            Me.m_NotificaSogliaTemplate = reader.Read("NotificaSogliaTemplate", Me.m_NotificaSogliaTemplate)
            Me.m_NotificaSogliaTo = reader.Read("NotificaSogliaTo", Me.m_NotificaSogliaTo)
            Me.m_PraticaCreataSubject = reader.Read("PraticaCreataSubject", Me.m_PraticaCreataSubject)
            Me.m_PraticaCreataTemplate = reader.Read("PraticaCreataTemplate", Me.m_PraticaCreataTemplate)
            Me.m_PraticaAnnullataSubject = reader.Read("PraticaAnnullataSubject", Me.m_PraticaAnnullataSubject)
            Me.m_PraticaAnnullataTemplate = reader.Read("PraticaAnnullataTemplate", Me.m_PraticaAnnullataTemplate)

            Me.m_ConsulenzaInseritaSubject = reader.Read("ConsulenzaInseritaSubject", Me.m_ConsulenzaInseritaSubject)
            Me.m_ConsulenzaInseritaTemplate = reader.Read("ConsulenzaInseritaTemplate", Me.m_ConsulenzaInseritaTemplate)
            Me.m_ConsulenzaAccettataSubject = reader.Read("ConsulenzaAccettataSubject", Me.m_ConsulenzaAccettataSubject)
            Me.m_ConsulenzaAccettataTemplate = reader.Read("ConsulenzaAccettataTemplate", Me.m_ConsulenzaAccettataTemplate)
            Me.m_ConsulenzaBocciataSubject = reader.Read("ConsulenzaBocciataSubject", Me.m_ConsulenzaBocciataSubject)
            Me.m_ConsulenzaBocciataTemplate = reader.Read("ConsulenzaBocciataTemplate", Me.m_ConsulenzaBocciataTemplate)
            Me.m_ConsulenzaPropostaSubject = reader.Read("ConsulenzaPropostaSubject", Me.m_ConsulenzaPropostaSubject)
            Me.m_ConsulenzaPropostaTemplate = reader.Read("ConsulenzaPropostaTemplate", Me.m_ConsulenzaPropostaTemplate)
            Me.m_ConsulenzaRifiutataSubject = reader.Read("ConsulenzaRifiutataSubject", Me.m_ConsulenzaRifiutataSubject)
            Me.m_ConsulenzaRifiutataTemplate = reader.Read("ConsulenzaRifiutataTemplate", Me.m_ConsulenzaRifiutataTemplate)

            Me.m_RichiestaFinanziamentoSubject = reader.Read("RichiestaFinanziamentoSubject", Me.m_RichiestaFinanziamentoSubject)
            Me.m_RichiestaFinanziamentoTemplate = reader.Read("RichiestaFinanziamentoTemplate", Me.m_RichiestaFinanziamentoTemplate)

            Me.m_BCC = reader.Read("BCC", Me.m_BCC)

            Me.m_GiorniArchiviazione = reader.Read("GiorniArchiviazione", Me.m_GiorniArchiviazione)
            Me.m_UltimaArchiviazione = reader.Read("UltimaArchiviazione", Me.m_UltimaArchiviazione)

            Dim ret As Boolean = MyBase.LoadFromRecordset(reader)

            Me.m_ConsulenzaRichiestaApprovazioneSubject = Me.Overflow.GetValueString("ConsulenzaRichiestaApprovazioneSubject", Me.m_ConsulenzaRichiestaApprovazioneSubject)
            Me.m_ConsulenzaRichiestaApprovazioneTemplate = Me.Overflow.GetValueString("ConsulenzaRichiestaApprovazioneTemplate", Me.m_ConsulenzaRichiestaApprovazioneTemplate)
            Me.m_ConsulenzaConfermaApprovazioneSubject = Me.Overflow.GetValueString("ConsulenzaConfermaApprovazioneSubject", Me.m_ConsulenzaConfermaApprovazioneSubject)
            Me.m_ConsulenzaConfermaApprovazioneTemplate = Me.Overflow.GetValueString("ConsulenzaConfermaApprovazioneTemplate", Me.m_ConsulenzaConfermaApprovazioneTemplate)
            Me.m_ConsulenzaNegaApprovazioneSubject = Me.Overflow.GetValueString("ConsulenzaNegaApprovazioneSubject", Me.m_ConsulenzaNegaApprovazioneSubject)
            Me.m_ConsulenzaNegaApprovazioneTemplate = Me.Overflow.GetValueString("ConsulenzaNegaApprovazioneTemplate", Me.m_ConsulenzaNegaApprovazioneTemplate)

            Return ret
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Flags", Me.m_Flags)
            writer.Write("ValoreMinimoUpfront", Me.m_ValoreMinimoUpfront)
            writer.Write("SogliaNotificaUpFront", Me.m_SogliaNotificaUpFront)
            writer.Write("GiorniAnticipoRifin", Me.m_GiorniAnticipoRifin)
            writer.Write("PercCompletamentoRifn", Me.m_PercCompletamentoRifn)
            writer.Write("IDStatoPredefinito", Me.IDStatoPredefinito)
            writer.Write("FromAddress", Me.m_FromAddress)
            writer.Write("FromDisplayName", Me.m_FromDiplayName)
            writer.Write("NotifyDeleteTo", Me.m_NotifyDeleteTo)
            writer.Write("NotifyChangesTo", Me.m_NotifyChangesTo)
            writer.Write("WatchSubject", Me.m_WatchSubject)
            writer.Write("WatchTemplate", Me.m_WatchTemplate)
            writer.Write("DeleteSubject", Me.m_DeleteSubject)
            writer.Write("DeleteTemplate", Me.m_DeleteTemplate)
            writer.Write("NotifyWarningsTo", Me.m_NotifyWarningsTo)
            writer.Write("NotifyRichiesteApprovazioneTo", Me.m_NotifyRichiesteApprovazioneTo)
            writer.Write("TemplateRichiesteApprovazione", Me.m_TemplateRichiesteApprovazione)
            writer.Write("TemplateConfermaApprovazione", Me.m_TemplateConfermaApprovazione)
            writer.Write("TemplateNegaApprovazione", Me.m_TemplateNegaApprovazione)
            writer.Write("TemplateChangeStatus", Me.m_TemplateChangeStatus)
            writer.Write("ChangeStatusSubject", Me.m_ChangeStatusSubject)
            writer.Write("ReqApprSubject", Me.m_ReqApprSubject)
            writer.Write("ApprSubject", Me.m_ApprSubject)
            writer.Write("NegaSubject", Me.m_NegaSubject)
            writer.Write("NotificaSogliaSubject", Me.m_NotificaSogliaSubject)
            writer.Write("NotificaSogliaTemplate", Me.m_NotificaSogliaTemplate)
            writer.Write("NotificaSogliaTo", Me.m_NotificaSogliaTo)
            writer.Write("PraticaCreataSubject", Me.m_PraticaCreataSubject)
            writer.Write("PraticaCreataTemplate", Me.m_PraticaCreataTemplate)
            writer.Write("PraticaAnnullataSubject", Me.m_PraticaAnnullataSubject)
            writer.Write("PraticaAnnullataTemplate", Me.m_PraticaAnnullataTemplate)

            writer.Write("ConsulenzaInseritaSubject", Me.m_ConsulenzaInseritaSubject)
            writer.Write("ConsulenzaInseritaTemplate", Me.m_ConsulenzaInseritaTemplate)
            writer.Write("ConsulenzaAccettataSubject", Me.m_ConsulenzaAccettataSubject)
            writer.Write("ConsulenzaAccettataTemplate", Me.m_ConsulenzaAccettataTemplate)
            writer.Write("ConsulenzaBocciataSubject", Me.m_ConsulenzaBocciataSubject)
            writer.Write("ConsulenzaBocciataTemplate", Me.m_ConsulenzaBocciataTemplate)
            writer.Write("ConsulenzaPropostaSubject", Me.m_ConsulenzaPropostaSubject)
            writer.Write("ConsulenzaPropostaTemplate", Me.m_ConsulenzaPropostaTemplate)
            writer.Write("ConsulenzaRifiutataSubject", Me.m_ConsulenzaRifiutataSubject)
            writer.Write("ConsulenzaRifiutataTemplate", Me.m_ConsulenzaRifiutataTemplate)

            writer.Write("RichiestaFinanziamentoSubject", Me.m_RichiestaFinanziamentoSubject)
            writer.Write("RichiestaFinanziamentoTemplate", Me.m_RichiestaFinanziamentoTemplate)


            writer.Write("BCC", Me.m_BCC)

            writer.Write("GiorniArchiviazione", Me.m_GiorniArchiviazione)
            writer.Write("UltimaArchiviazione", Me.m_UltimaArchiviazione)



            Return MyBase.SaveToRecordset(writer)
        End Function



        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("ValoreMinimoUpfront", Me.m_ValoreMinimoUpfront)
            writer.WriteAttribute("SogliaNotificaUpFront", Me.m_SogliaNotificaUpFront)
            writer.WriteAttribute("GiorniAnticipoRifin", Me.m_GiorniAnticipoRifin)
            writer.WriteAttribute("PercCompletamentoRifn", Me.m_PercCompletamentoRifn)
            writer.WriteAttribute("IDStatoPredefinito", Me.IDStatoPredefinito)
            writer.WriteAttribute("FromAddress", Me.m_FromAddress)
            writer.WriteAttribute("FromDiplayName", Me.m_FromDiplayName)
            writer.WriteAttribute("NotifyDeleteTo", Me.m_NotifyDeleteTo)
            writer.WriteAttribute("NotifyChangesTo", Me.m_NotifyChangesTo)
            writer.WriteAttribute("WatchSubject", Me.m_WatchSubject)
            writer.WriteAttribute("WatchTemplate", Me.m_WatchTemplate)
            writer.WriteAttribute("DeleteSubject", Me.m_DeleteSubject)
            writer.WriteAttribute("DeleteTemplate", Me.m_DeleteTemplate)
            writer.WriteAttribute("NotifyWarningsTo", Me.m_NotifyWarningsTo)
            writer.WriteAttribute("NotifyRichiesteApprovazioneTo", Me.m_NotifyRichiesteApprovazioneTo)
            writer.WriteAttribute("TemplateRichiesteApprovazione", Me.m_TemplateRichiesteApprovazione)
            writer.WriteAttribute("TemplateConfermaApprovazione", Me.m_TemplateConfermaApprovazione)
            writer.WriteAttribute("TemplateNegaApprovazione", Me.m_TemplateNegaApprovazione)
            writer.WriteAttribute("TemplateChangeStatus", Me.m_TemplateChangeStatus)
            writer.WriteAttribute("ChangeStatusSubject", Me.m_ChangeStatusSubject)
            writer.WriteAttribute("ReqApprSubject", Me.m_ReqApprSubject)
            writer.WriteAttribute("ApprSubject", Me.m_ApprSubject)
            writer.WriteAttribute("NegaSubject", Me.m_NegaSubject)
            writer.WriteAttribute("NotificaSogliaSubject", Me.m_NotificaSogliaSubject)
            writer.WriteAttribute("NotificaSogliaTemplate", Me.m_NotificaSogliaTemplate)
            writer.WriteAttribute("NotificaSogliaTo", Me.m_NotificaSogliaTo)
            writer.WriteAttribute("PraticaCreataSubject", Me.m_PraticaCreataSubject)
            writer.WriteAttribute("PraticaCreataTemplate", Me.m_PraticaCreataTemplate)
            writer.WriteAttribute("PraticaAnnullataSubject", Me.m_PraticaAnnullataSubject)
            writer.WriteAttribute("PraticaAnnullataTemplate", Me.m_PraticaAnnullataTemplate)


            writer.WriteAttribute("ConsulenzaInseritaSubject", Me.m_ConsulenzaInseritaSubject)
            writer.WriteAttribute("ConsulenzaInseritaTemplate", Me.m_ConsulenzaInseritaTemplate)
            writer.WriteAttribute("ConsulenzaAccettataSubject", Me.m_ConsulenzaAccettataSubject)
            writer.WriteAttribute("ConsulenzaAccettataTemplate", Me.m_ConsulenzaAccettataTemplate)
            writer.WriteAttribute("ConsulenzaBocciataSubject", Me.m_ConsulenzaBocciataSubject)
            writer.WriteAttribute("ConsulenzaBocciataTemplate", Me.m_ConsulenzaBocciataTemplate)
            writer.WriteAttribute("ConsulenzaPropostaSubject", Me.m_ConsulenzaPropostaSubject)
            writer.WriteAttribute("ConsulenzaPropostaTemplate", Me.m_ConsulenzaPropostaTemplate)
            writer.WriteAttribute("ConsulenzaRifiutataSubject", Me.m_ConsulenzaRifiutataSubject)
            writer.WriteAttribute("ConsulenzaRifiutataTemplate", Me.m_ConsulenzaRifiutataTemplate)

            writer.WriteAttribute("RichiestaFinanziamentoSubject", Me.m_RichiestaFinanziamentoSubject)
            writer.WriteAttribute("RichiestaFinanziamentoTemplate", Me.m_RichiestaFinanziamentoTemplate)
            writer.WriteAttribute("GiorniArchiviazione", Me.m_GiorniArchiviazione)
            writer.WriteAttribute("UltimaArchiviazione", Me.m_UltimaArchiviazione)

            writer.WriteAttribute("ConsRichAppSub", Me.m_ConsulenzaRichiestaApprovazioneSubject)
            writer.WriteAttribute("ConsRichAppTem", Me.m_ConsulenzaRichiestaApprovazioneTemplate)

            writer.WriteAttribute("ConsConfAppSub", Me.m_ConsulenzaConfermaApprovazioneSubject)
            writer.WriteAttribute("ConsConfAppTem", Me.m_ConsulenzaConfermaApprovazioneTemplate)

            writer.WriteAttribute("ConsNegaAppSub", Me.m_ConsulenzaNegaApprovazioneSubject)
            writer.WriteAttribute("ConsNegaAppTem", Me.m_ConsulenzaNegaApprovazioneTemplate)

            writer.WriteAttribute("BCC", Me.m_BCC)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValoreMinimoUpfront" : Me.m_ValoreMinimoUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SogliaNotificaUpFront" : Me.m_SogliaNotificaUpFront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "GiorniAnticipoRifin" : Me.m_GiorniAnticipoRifin = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PercCompletamentoRifn" : Me.m_PercCompletamentoRifn = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDStatoPredefinito" : Me.m_IDStatoPredefinito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FromAddress" : Me.m_FromAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FromDiplayName" : Me.m_FromDiplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotifyDeleteTo" : Me.m_NotifyDeleteTo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotifyChangesTo" : Me.m_NotifyChangesTo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "WatchSubject" : Me.m_WatchSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "WatchTemplate" : Me.m_WatchTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DeleteSubject" : Me.m_DeleteSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DeleteTemplate" : Me.m_DeleteTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotifyWarningsTo" : Me.m_NotifyWarningsTo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotifyRichiesteApprovazioneTo" : Me.m_NotifyRichiesteApprovazioneTo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TemplateRichiesteApprovazione" : Me.m_TemplateRichiesteApprovazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TemplateConfermaApprovazione" : Me.m_TemplateConfermaApprovazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TemplateNegaApprovazione" : Me.m_TemplateNegaApprovazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TemplateChangeStatus" : Me.m_TemplateChangeStatus = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ChangeStatusSubject" : Me.m_ChangeStatusSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ReqApprSubject" : Me.m_ReqApprSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ApprSubject" : Me.m_ApprSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NegaSubject" : Me.m_NegaSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotificaSogliaSubject" : Me.m_NotificaSogliaSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotificaSogliaTemplate" : Me.m_NotificaSogliaTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotificaSogliaTo" : Me.m_NotificaSogliaTo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PraticaCreataSubject" : Me.m_PraticaCreataSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PraticaCreataTemplate" : Me.m_PraticaCreataTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PraticaAnnullataSubject" : Me.m_PraticaAnnullataSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PraticaAnnullataTemplate" : Me.m_PraticaAnnullataTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "ConsulenzaInseritaSubject" : Me.m_ConsulenzaInseritaSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaInseritaTemplate" : Me.m_ConsulenzaInseritaTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaAccettataSubject" : Me.m_ConsulenzaAccettataSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaAccettataTemplate" : Me.m_ConsulenzaAccettataTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaBocciataSubject" : Me.m_ConsulenzaBocciataSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaBocciataTemplate" : Me.m_ConsulenzaBocciataTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaPropostaSubject" : Me.m_ConsulenzaPropostaSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaPropostaTemplate" : Me.m_ConsulenzaPropostaTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaRifiutataSubject" : Me.m_ConsulenzaRifiutataSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsulenzaRifiutataTemplate" : Me.m_ConsulenzaRifiutataTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RichiestaFinanziamentoSubject" : Me.m_RichiestaFinanziamentoSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RichiestaFinanziamentoTemplate" : Me.m_RichiestaFinanziamentoTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GiorniArchiviazione" : Me.m_GiorniArchiviazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UltimaArchiviazione" : Me.m_UltimaArchiviazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ConsRichAppSub" : Me.m_ConsulenzaRichiestaApprovazioneSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsRichAppTem" : Me.m_ConsulenzaRichiestaApprovazioneTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsConfAppSub" : Me.m_ConsulenzaConfermaApprovazioneSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsConfAppTem" : Me.m_ConsulenzaConfermaApprovazioneTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsNegaAppSub" : Me.m_ConsulenzaNegaApprovazioneSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsNegaAppTem" : Me.m_ConsulenzaNegaApprovazioneTemplate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "BCC" : Me.m_BCC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Return "Configurazione Modulo Credito V"
        End Function

        Public Sub Load()
            Dim dbRis As System.Data.IDataReader = Me.GetConnection.ExecuteReader("SELECT * FROM [" & Me.GetTableName & "] ORDER BY [ID] ASC")
            If dbRis.Read Then
                CQSPD.Database.Load(Me, dbRis)
            End If
            dbRis.Dispose()
        End Sub

         
    End Class


    Private Shared m_Config As CCQSPDConfig

    ''' <summary>
    ''' Oggetto che contiene alcuni parametri relativi alla configurazione del modulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Configuration As CCQSPDConfig
        Get
            If m_Config Is Nothing Then
                m_Config = New CCQSPDConfig
                m_Config.Load()
            End If
            Return m_Config
        End Get
    End Property

    Friend Shared Sub SetConfiguration(ByVal c As CCQSPDConfig)
        m_Config = c
        Dim e As New System.EventArgs
        RaiseEvent ConfigurationChanged(Nothing, e)
    End Sub

End Class
