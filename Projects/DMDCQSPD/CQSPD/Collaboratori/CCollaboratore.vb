Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD


    Public Enum StatoProduttore As Integer
        STATO_ATTIVO = UserStatus.USER_ENABLED
        STATO_DISABILITATO = UserStatus.USER_DISABLED
        STATO_ELIMINATO = UserStatus.USER_DELETED
        STATO_INATTIVAZIONE = UserStatus.USER_NEW
        STATO_SOSPESO = UserStatus.USER_SUSPENDED
        'STATO_INVALID = UserStatus.USER_TEMP
    End Enum

    <Serializable> _
    Public Class CCollaboratore
        Inherits DBObjectPO
        Implements IFonte

        Private m_User As CUser
        Private m_UserID As Integer
        Private m_NomeUtente As String

        Private m_PersonaID As Integer
        Private m_Persona As CPersona
        Private m_NomePersona As String

        Private m_Rapporto As String
        Private m_Attivo As Boolean
        Private m_DataInizioRapporto As Nullable(Of Date)
        Private m_DataFineRapporto As Nullable(Of Date)
        Private m_Tipo As String
        Private m_ReferenteID As Integer
        Private m_Referente As CTeamManager
        Private m_AttivatoDaID As Integer
        Private m_AttivatoDa As CUser
        Private m_DataAttivazione As Nullable(Of Date)


        Private m_Trattative As CTrattativeCollaboratore
        Private m_ListinoPredefinitoID As Integer
        Private m_ListinoPredefinito As CProfilo
        'Private  As CPratichePerCollabCollection

        Private m_NumeroIscrizioneUIC As String
        Private m_DataIscrizioneUIC As Nullable(Of Date)
        Private m_TipoIscrizioneUIC As String
        Private m_DataRinnovoUIC As Nullable(Of Date)

        Private m_DataVisuraProtesti As Nullable(Of Date)
        Private m_EsitoVisuraProtesti As Integer

        Private m_NumeroIscrizioneRUI As String
        Private m_DataIscrizioneRUI As Nullable(Of Date)
        Private m_DataRinnovoRUI As Nullable(Of Date)

        Private m_NumeroIscrizioneISVAP As String
        Private m_DataIscrizioneISVAP As Nullable(Of Date)
        Private m_DataRinnovoISVAP As Nullable(Of Date)

        'Private m_ProdottiDistribuiti
        'Private m_Acconti

        'Private m_Commercializza_cqs_delega
        'Private m_Commercializza_cqs_delega_con_chi
        'Private m_Commercializza_cqs_delega_con_acconti
        'Private m_Netto_erogato_per_pratica
        'Private m_Teg_per_pratica_60_mesi
        'Private m_Teg_per_pratica_120_mesi
        'Private m_Tempi_erogazione_gg
        'Private m_notifica_pratica_a_cura_finanziaria
        'Private m_notifica_pratica_a_cura_broker
        'Private m_procura
        'Private m_collegamento_per_preventivi_IP_dinamico
        'Private m_collegamento_per_preventivi_IP_statico
        'Private m_Tempi_liquidazione_per_tipo_prodotto_gg
        'Private m_Distribuzione_produzione_Statale_Perc
        'Private m_Distribuzione_produzione_Pubblico_Perc
        'Private m_Distribuzione_produzione_Privato_Perc
        'Private m_Distribuzione_produzione_Pensionato_Perc
        'Private m_Fatturato_montante_lordo_mensile
        'Private m_Fatturato_montante_lordo_annuo
        'Private m_Max_provvigionale_caricato
        'Private m_Note_Provvigionale_Max
        'Private m_Proposte
        'Private m_MAIL_referente
        'Private m_Tipologia_Med_A
        'Private m_Tipologia_Med_B
        'Private m_Tipologia_Med_C
        'Private m_SCHEDA
        'Private m_Esito
        'Private m_Raccolta_Preventivi_competitor
        'Private m_Allegato_preventivo_competitor
        'Private m_Stampa_Preventivi_finsab_confronto
        'Private m_Allegato_preventivi_finsab
        'Private m_Riepilogo_esigenze_mediatore
        'Private m_Spread_finsab_da_assegnare_conv_inpdap
        'Private m_Provv_Mediatore_da_assegnare_conv_inpdap
        'Private m_Spread_finsab_da_assegnare_ordinario
        'Private m_Provv_Mediatore_da_assegnare_ordinario
        'Private m_ProfiloFuturo

        Public Sub New()
            Me.m_PersonaID = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = vbNullString

            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_NomeUtente = vbNullString
 
            Me.m_ReferenteID = 0
            Me.m_Referente = Nothing
            Me.m_Trattative = Nothing
            Me.m_ListinoPredefinitoID = 0
            Me.m_ListinoPredefinito = Nothing
            Me.m_Attivo = True
            'Me.m_BUIs = Nothing
            'Me.m_Acconti = False
            'Me.m_Commercializza_cqs_delega = False
            'Me.m_Commercializza_cqs_delega_con_acconti = False
            'Me.m_notifica_pratica_a_cura_finanziaria = False
            'Me.m_notifica_pratica_a_cura_broker = False
            'Me.m_procura = False
            'Me.m_collegamento_per_preventivi_IP_dinamico = False
            'Me.m_collegamento_per_preventivi_IP_statico = False
            'Me.m_Tipologia_Med_A = False
            'Me.m_Tipologia_Med_B = False
            'Me.m_Tipologia_Med_C = False
            'Me.m_Raccolta_Preventivi_competitor = False
            'Me.m_Stampa_Preventivi_finsab_confronto = False
            'Me.m_Pratiche = Nothing
            Me.m_AttivatoDa = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.Collaboratori.Module
        End Function

        Public Function IsValid(ByVal atDate As Date) As Boolean
            Return DateUtils.CheckBetween(atDate, Me.m_DataInizioRapporto, Me.m_DataFineRapporto)
        End Function

        Public Function IsValid() As Boolean
            Return Me.IsValid(Now)
        End Function



        'Public ReadOnly Property Pratiche As CPratichePerCollabCollection
        '    Get
        '        If (Me.m_Pratiche Is Nothing) Then
        '            Me.m_Pratiche = New CPratichePerCollabCollection
        '            Me.m_Pratiche.Initialize(Me)
        '        End If
        '        Return Me.m_Pratiche
        '    End Get
        'End Property

        Public Property ListinoPredefinito As CProfilo
            Get
                If (Me.m_ListinoPredefinito Is Nothing) Then Me.m_ListinoPredefinito = CQSPD.Profili.GetItemById(Me.m_ListinoPredefinitoID)
                Return Me.m_ListinoPredefinito
            End Get
            Set(value As CProfilo)
                Dim oldValue As CProfilo = Me.ListinoPredefinito
                If (oldValue = value) Then Exit Property
                Me.m_ListinoPredefinito = value
                Me.m_ListinoPredefinitoID = GetID(value)
                Me.DoChanged("ListinoPredefinito", value, oldValue)
            End Set
        End Property

        Public Property ListinoPredefinitoID As Integer
            Get
                Return GetID(Me.m_ListinoPredefinito, Me.m_ListinoPredefinitoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ListinoPredefinitoID
                If oldValue = value Then Exit Property
                Me.m_ListinoPredefinitoID = value
                Me.m_ListinoPredefinito = Nothing
                Me.DoChanged("ListinoPredefinitoID", value, oldValue)
            End Set
        End Property

        Public Property DataVisuraProtesti As Nullable(Of Date)
            Get
                Return Me.m_DataVisuraProtesti
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataVisuraProtesti
                If (oldValue = value) Then Exit Property
                Me.m_DataVisuraProtesti = value
                Me.DoChanged("DataVisuraProtesti", value, oldValue)
            End Set
        End Property

        Protected Friend Sub InvalidateTrattative()
            Me.m_Trattative = Nothing
        End Sub

        Public Property EsitoVisuraProtesti As Integer
            Get
                Return Me.m_EsitoVisuraProtesti
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_EsitoVisuraProtesti
                If (oldValue = value) Then Exit Property
                Me.m_EsitoVisuraProtesti = value
                Me.DoChanged("EsitoVisuraProtesti", value, oldValue)
            End Set
        End Property

        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_PersonaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If oldValue = value Then Exit Property
                Me.m_PersonaID = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_PersonaID)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Persona
                If (oldValue = value) Then Exit Property
                Me.m_Persona = value
                Me.m_PersonaID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomePersona
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property


        Public Property Referente As CTeamManager
            Get
                If (Me.m_Referente Is Nothing) Then Me.m_Referente = CQSPD.TeamManagers.GetItemById(Me.m_ReferenteID)
                Return Me.m_Referente
            End Get
            Set(value As CTeamManager)
                Dim oldValue As CTeamManager = Me.Referente
                If (oldValue = value) Then Exit Property
                Me.m_Referente = value
                Me.m_ReferenteID = GetID(value)
                Me.DoChanged("Referente", value, oldValue)
            End Set
        End Property

        Public Property ReferenteID As Integer
            Get
                Return GetID(Me.m_Referente, Me.m_ReferenteID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ReferenteID
                If oldValue = value Then Exit Property
                Me.m_ReferenteID = value
                Me.m_Referente = Nothing
                Me.DoChanged("ReferenteID", value, oldValue)
            End Set
        End Property

        Public Property AttivatoDa As CUser
            Get
                If (Me.m_AttivatoDa Is Nothing) Then Me.m_AttivatoDa = Sistema.Users.GetItemById(Me.m_AttivatoDaID)
                Return Me.m_AttivatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.AttivatoDa
                If (oldValue = value) Then Exit Property
                Me.m_AttivatoDa = value
                Me.m_AttivatoDaID = GetID(value)
                Me.DoChanged("AttivatoDa", value, oldValue)
            End Set
        End Property

        Public Property AttivatoDaID As Integer
            Get
                Return GetID(Me.m_AttivatoDa, Me.m_AttivatoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.AttivatoDaID
                If oldValue = value Then Exit Property
                Me.m_AttivatoDaID = value
                Me.m_AttivatoDa = Nothing
                Me.DoChanged("AttivatoDaID", value, oldValue)
            End Set
        End Property

        Public Property DataAttivazione As Nullable(Of Date)
            Get
                Return Me.m_DataAttivazione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAttivazione
                If (oldValue = value) Then Exit Property
                Me.m_DataAttivazione = value
                Me.DoChanged("DataAttivazione", value, oldValue)
            End Set
        End Property

        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If oldValue = value Then Exit Property
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        Public Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue = value) Then Exit Property
                Me.m_User = value
                Me.m_UserID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeUtente = value.Nominativo
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Public Property NomeUtente As String
            Get
                Return Me.m_NomeUtente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeUtente
                If (oldValue = value) Then Exit Property
                Me.m_NomeUtente = value
                Me.DoChanged("NomeUtente", value, oldValue)
            End Set
        End Property


        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Trattative As CTrattativeCollaboratore
            Get
                If (Me.m_Trattative Is Nothing) Then Me.m_Trattative = New CTrattativeCollaboratore(Me)
                Return Me.m_Trattative
            End Get
        End Property

        Public Property Rapporto As String
            Get
                Return Me.m_Rapporto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Rapporto
                If (oldValue = value) Then Exit Property
                Me.m_Rapporto = value
                Me.DoChanged("Rapporto", value, oldValue)
            End Set
        End Property

        Public Property DataInizioRapporto As Nullable(Of Date)
            Get
                Return Me.m_DataInizioRapporto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizioRapporto
                If (oldValue = value) Then Exit Property
                Me.m_DataInizioRapporto = value
                Me.DoChanged("DataInizioRapporto", value, oldValue)
            End Set
        End Property

        Public Property DataFineRapporto As Nullable(Of Date)
            Get
                Return Me.m_DataFineRapporto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFineRapporto
                If (oldValue = value) Then Exit Property
                Me.m_DataFineRapporto = value
                Me.DoChanged("DataFineRapporto", value, oldValue)
            End Set
        End Property

        Public Property DataIscrizioneUIC As Nullable(Of Date)
            Get
                Return Me.m_DataIscrizioneUIC
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataIscrizioneUIC
                If (oldValue = value) Then Exit Property
                Me.m_DataIscrizioneUIC = value
                Me.DoChanged("DataIscrizioneUIC", value, oldValue)
            End Set
        End Property

        Public Property NumeroIscrizioneUIC As String
            Get
                Return Me.m_NumeroIscrizioneUIC
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NumeroIscrizioneUIC
                If (oldValue = value) Then Exit Property
                Me.m_NumeroIscrizioneUIC = value
                Me.DoChanged("NumeroIscrizioneUIC", value, oldValue)
            End Set
        End Property

        Public Property DataIscrizioneRUI As Nullable(Of Date)
            Get
                Return Me.m_DataIscrizioneRUI
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataIscrizioneRUI
                If (oldValue = value) Then Exit Property
                Me.m_DataIscrizioneRUI = value
                Me.DoChanged("DataIscrizioneRUI", value, oldValue)
            End Set
        End Property

        Public Property NumeroIscrizioneRUI As String
            Get
                Return Me.m_NumeroIscrizioneRUI
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NumeroIscrizioneRUI
                If (oldValue = value) Then Exit Property
                Me.m_NumeroIscrizioneRUI = value
                Me.DoChanged("NumeroIscrizioneUIC", value, oldValue)
            End Set
        End Property

        Public Property DataRinnovoRUI As Nullable(Of Date)
            Get
                Return Me.m_DataRinnovoRUI
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRinnovoRUI
                If (oldValue = value) Then Exit Property
                Me.m_DataRinnovoRUI = value
                Me.DoChanged("DataRinnovoRUI", value, oldValue)
            End Set
        End Property

        Public Property NumeroIscrizioneISVAP As String
            Get
                Return Me.m_NumeroIscrizioneISVAP
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NumeroIscrizioneISVAP
                If (oldValue = value) Then Exit Property
                Me.m_NumeroIscrizioneISVAP = value
                Me.DoChanged("NumeroIscrizioneISVAP", value, oldValue)
            End Set
        End Property

        Public Property DataIscrizioneISVAP As Nullable(Of Date)
            Get
                Return Me.m_DataIscrizioneISVAP
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataIscrizioneISVAP
                If (oldValue = value) Then Exit Property
                Me.m_DataIscrizioneISVAP = value
                Me.DoChanged("DataIscrizioneISVAP", value, oldValue)
            End Set
        End Property

        Public Property DataRinnovoISVAP As Nullable(Of Date)
            Get
                Return Me.m_DataRinnovoISVAP
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRinnovoISVAP
                If (oldValue = value) Then Exit Property
                Me.m_DataRinnovoISVAP = value
                Me.DoChanged("DataRinnovoISVAP", value, oldValue)
            End Set
        End Property

        Public Property TipoIscrizioneUIC As String
            Get
                Return Me.m_TipoIscrizioneUIC
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoIscrizioneUIC
                If (oldValue = value) Then Exit Property
                Me.m_TipoIscrizioneUIC = value
                Me.DoChanged("TipoIscrizioneUIC", value, oldValue)
            End Set
        End Property

        Public Property DataRinnovoUIC As Nullable(Of Date)
            Get
                Return Me.m_DataRinnovoUIC
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRinnovoUIC
                If (oldValue = value) Then Exit Property
                Me.m_DataRinnovoUIC = value
                Me.DoChanged("DataRinnovoUIC", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Collaboratori"
        End Function

        Private Function EnsureGroup(ByVal grpName As String) As CGroup
            Dim grp As CGroup = Sistema.Groups.GetItemByName(grpName)
            If (grp Is Nothing) Then
                grp = New CGroup(grpName)
            End If
            grp.Stato = ObjectStatus.OBJECT_VALID
            grp.Save()
            Return grp
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim grp As CGroup
            Dim ret As Boolean

            If Not (Me.m_User Is Nothing) Then Me.m_User.Save(force)
            If Not (Me.m_ListinoPredefinito Is Nothing) Then Me.m_ListinoPredefinito.Save(force)
            If Not (Me.m_Persona Is Nothing) Then
                Me.m_Persona.Stato = ObjectStatus.OBJECT_VALID
                Me.m_Persona.Save()
            End If

            ret = MyBase.SaveToDatabase(dbConn, force)

            If Not (Me.m_Trattative Is Nothing) Then Me.m_Trattative.Save(force)
            'If Not (Me.m_Pratiche Is Nothing) Then Me.m_Pratiche.Save(force)

            'Assicuriamoci che l'oggetto appartenga al gruppo corretto
            If Me.UserID <> 0 Then
                Select Case LCase(Me.m_Tipo)
                    Case "mediatore"
                        grp = Me.EnsureGroup("Collaboratori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Agenti")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Segnalatori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Mediatori")
                        If Not grp.Members.Contains(Me.UserID) Then grp.Members.Add(Me.UserID)


                    Case "agente"
                        grp = Me.EnsureGroup("Collaboratori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Mediatori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Segnalatori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Agenti")
                        If Not grp.Members.Contains(Me.UserID) Then grp.Members.Add(Me.UserID)



                    Case "segnalatore"
                        grp = Me.EnsureGroup("Collaboratori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Mediatori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Agenti")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Segnalatori")
                        If Not grp.Members.Contains(Me.UserID) Then grp.Members.Add(Me.UserID)


                    Case "collaboratore"
                        grp = Me.EnsureGroup("Mediatori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Agenti")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Segnalatori")
                        If grp.Members.Contains(Me.UserID) Then grp.Members.Remove(Me.UserID)
                        grp = Me.EnsureGroup("Collaboratori")
                        If Not grp.Members.Contains(Me.UserID) Then grp.Members.Add(Me.UserID)



                    Case Else
                        Throw New ArgumentException("Tipo collaboratore non supportato: " & Me.m_Tipo)
                End Select

                grp = Sistema.Groups.GetItemByName("Users")
                If Not grp.Members.Contains(Me.UserID) Then
                    grp.Members.Add(Me.UserID)
                End If
            End If

            Dim fonte As CFonte
            'Dim canale As CCanale

            Select Case LCase(Me.m_Tipo)
                Case "mediatore" : fonte = Anagrafica.Fonti.GetItemByName("Mediatori", "Mediatori", Me.NomePersona)
                Case "agente" : fonte = Anagrafica.Fonti.GetItemByName("Agenti", "Agenti", Me.NomePersona)
                Case "segnalatore" : fonte = Anagrafica.Fonti.GetItemByName("Segnalatori", "Segnalatori", Me.NomePersona)
                Case "collaboratore" : fonte = Anagrafica.Fonti.GetItemByName("Collaboratori", "Collaboratori", Me.NomePersona)
                Case Else : Throw New ArgumentException("Tipo collaboratore non supportato: " & Me.m_Tipo)
            End Select

            If (fonte Is Nothing) Then fonte = New CFonte
            fonte.Nome = Me.NomePersona
            fonte.Tipo = Me.Tipo
            fonte.Stato = ObjectStatus.OBJECT_VALID
            fonte.IconURL = Me._IconURL
            fonte.Save()


            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_UserID = reader.Read("Utente", Me.m_UserID)
            Me.m_NomeUtente = reader.Read("NomeUtente", Me.m_NomeUtente)

            Me.m_PersonaID = reader.Read("Persona", Me.m_PersonaID)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)

            Me.m_DataIscrizioneUIC = reader.Read("DataIscrizioneUIC", Me.m_DataIscrizioneUIC)
            Me.m_NumeroIscrizioneUIC = reader.Read("NumeroIscrizioneUIC", Me.m_NumeroIscrizioneUIC)
            Me.m_TipoIscrizioneUIC = reader.Read("TipoIscrizioneUIC", Me.m_TipoIscrizioneUIC)
            Me.m_DataRinnovoUIC = reader.Read("DataRinnovoUIC", Me.m_DataRinnovoUIC)

            Me.m_NumeroIscrizioneRUI = reader.Read("NumeroIscrizioneRUI", Me.m_NumeroIscrizioneRUI)
            Me.m_DataIscrizioneRUI = reader.Read("DataIscrizioneRUI", Me.m_DataIscrizioneRUI)
            Me.m_DataRinnovoRUI = reader.Read("DataRinnovoRUI", Me.m_DataRinnovoRUI)

            Me.m_NumeroIscrizioneISVAP = reader.Read("NumeroIscrizioneISVAP", Me.m_NumeroIscrizioneISVAP)
            Me.m_DataIscrizioneISVAP = reader.Read("DataIscrizioneISVAP", Me.m_DataIscrizioneISVAP)
            Me.m_DataRinnovoISVAP = reader.Read("DataRinnovoISVAP", Me.m_DataRinnovoISVAP)

            Me.m_DataVisuraProtesti = reader.Read("DataVisuraProtesti", Me.m_DataVisuraProtesti)
            Me.m_EsitoVisuraProtesti = reader.Read("EsitoVisuraProtesti", Me.m_EsitoVisuraProtesti)

            Me.m_AttivatoDaID = reader.Read("AttivatoDa", Me.m_AttivatoDaID)
            Me.m_DataAttivazione = reader.Read("DataAttivazione", Me.m_DataAttivazione)

            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_ReferenteID = reader.Read("Referente", Me.m_ReferenteID)
            Me.m_ListinoPredefinitoID = reader.Read("ListinoPredefinito", Me.m_ListinoPredefinitoID)

            Me.m_Rapporto = reader.Read("Rapporto", Me.m_Rapporto)

            Me.m_DataInizioRapporto = reader.Read("DataInizioRapporto", Me.m_DataInizioRapporto)
            Me.m_DataFineRapporto = reader.Read("DataFineRapporto", Me.m_DataFineRapporto)

            'm_Commercializza_cqs_delega = dbRis ("Commercializza_cqs_delega")
            'm_Commercializza_cqs_delega_con_chi = dbRis ("Commercializza_cqs_delega_con_chi")
            'm_Commercializza_cqs_delega_con_acconti = dbRis ("Commercializza_cqs_delega_con_acconti")
            'm_Netto_erogato_per_pratica = dbRis ("Netto_erogato_per_pratica")
            'm_Teg_per_pratica_60_mesi = dbRis ("Teg_per_pratica_60_mesi")
            'm_Teg_per_pratica_120_mesi = dbRis ("Teg_per_pratica_120_mesi")
            'm_Tempi_erogazione_gg = dbRis ("Tempi_erogazione_gg")
            'm_notifica_pratica_a_cura_finanziaria = dbRis ("notifica_pratica_a_cura_finanziaria")
            'm_notifica_pratica_a_cura_broker = dbRis ("notifica_pratica_a_cura_broker")
            'm_procura = dbRis ("procura")
            'm_collegamento_per_preventivi_IP_dinamico = dbRis ("collegamento_per_preventivi_IP_dinamico")
            'm_collegamento_per_preventivi_IP_statico = dbRis ("collegamento_per_preventivi_IP_statico")
            'm_Tempi_liquidazione_per_tipo_prodotto_gg = dbRis ("Tempi_liquidazione_per_tipo_prodotto_gg")
            'm_Distribuzione_produzione_Statale_Perc = dbRis ("Distribuzione_produzione_Statale_Perc")
            'm_Distribuzione_produzione_Pubblico_Perc = dbRis ("Distribuzione_produzione_Pubblico_Perc")
            'm_Distribuzione_produzione_Privato_Perc = dbRis ("Distribuzione_produzione_Privato_Perc")
            'm_Distribuzione_produzione_Pensionato_Perc = dbRis ("Distribuzione_produzione_Pensionato_Perc")
            'm_Fatturato_montante_lordo_mensile = dbRis ("Fatturato_montante_lordo_mensile")
            'm_Fatturato_montante_lordo_annuo = dbRis ("Fatturato_montante_lordo_annuo")
            'm_Max_provvigionale_caricato = dbRis ("Max_provvigionale_caricato")
            'm_Note_Provvigionale_Max = dbRis ("Note_Provvigionale_Max")
            'm_Proposte = dbRis ("Proposte")
            'm_MAIL_referente = dbRis ("MAIL_referente")
            'm_Tipologia_Med_A = dbRis ("Tipologia_Med_A")
            'm_Tipologia_Med_B = dbRis ("Tipologia_Med_B")
            'm_Tipologia_Med_C = dbRis ("Tipologia_Med_C")
            'm_SCHEDA = dbRis ("SCHEDA")
            'm_Esito = dbRis ("Esito")
            'm_Raccolta_Preventivi_competitor = dbRis ("Raccolta_Preventivi_competitor")
            'm_Allegato_preventivo_competitor = dbRis ("Allegato_preventivo_competitor")
            'm_Stampa_Preventivi_finsab_confronto = dbRis ("Stampa_Preventivi_finsab_confronto")
            'm_Allegato_preventivi_finsab = dbRis ("Allegato_preventivi_finsab")
            'm_Riepilogo_esigenze_mediatore = dbRis ("Riepilogo_esigenze_mediatore")
            'm_Spread_finsab_da_assegnare_conv_inpdap = dbRis ("Spread_finsab_da_assegnare_conv_inpdap")
            'm_Provv_Mediatore_da_assegnare_conv_inpdap = dbRi s("Provv_Mediatore_da_assegnare_conv_inpdap")
            'm_Spread_finsab_da_assegnare_ordinario = dbRis ("Spread_finsab_da_assegnare_ordinario")
            'm_Provv_Mediatore_da_assegnare_ordinario = dbRis ("Provv_Mediatore_da_assegnare_ordinario")
            'm_ProfiloFuturo = dbRis ("ProfiloFuturo")

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Utente", GetID(Me.m_User, Me.m_UserID))
            writer.Write("NomeUtente", Me.m_NomeUtente)

            writer.Write("Persona", GetID(Me.m_Persona, Me.m_PersonaID))
            writer.Write("NomePersona", Me.m_NomePersona)

            
            writer.Write("NumeroIscrizioneUIC", Me.m_NumeroIscrizioneUIC)
            writer.Write("TipoIscrizioneUIC", Me.m_TipoIscrizioneUIC)
            writer.Write("DataIscrizioneUIC", Me.m_DataIscrizioneUIC)
            writer.Write("DataRinnovoUIC", Me.m_DataRinnovoUIC)

            writer.Write("NumeroIscrizioneRUI", Me.m_NumeroIscrizioneRUI)
            writer.Write("DataIscrizioneRUI", Me.m_DataIscrizioneRUI)
            writer.Write("DataRinnovoRUI", Me.m_DataRinnovoRUI)

            writer.Write("NumeroIscrizioneISVAP", Me.m_NumeroIscrizioneISVAP)
            writer.Write("DataIscrizioneISVAP", Me.m_DataIscrizioneISVAP)
            writer.Write("DataRinnovoISVAP", Me.m_DataRinnovoISVAP)

            writer.Write("DataVisuraProtesti", Me.m_DataVisuraProtesti)
            writer.Write("EsitoVisuraProtesti", Me.m_EsitoVisuraProtesti)

            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Referente", GetID(Me.m_Referente, Me.m_ReferenteID))
            writer.Write("AttivatoDa", GetID(Me.m_AttivatoDa, Me.m_AttivatoDaID))
            writer.Write("DataAttivazione", Me.m_DataAttivazione)
            writer.Write("ListinoPredefinito", GetID(Me.m_ListinoPredefinito, Me.m_ListinoPredefinitoID))
            writer.Write("Rapporto", Me.m_Rapporto)
            writer.Write("DataInizioRapporto", Me.m_DataInizioRapporto)
            writer.Write("DataFineRapporto", Me.m_DataFineRapporto)
            'dbRis("ProdottiDistribuiti") = m_ProdottiDistribuiti
            'dbRis("Acconti") = m_Acconti


            'dbRis("Commercializza_cqs_delega") = m_Commercializza_cqs_delega
            'dbRis("Commercializza_cqs_delega_con_chi") = m_Commercializza_cqs_delega_con_chi
            'dbRis("Commercializza_cqs_delega_con_acconti") = m_Commercializza_cqs_delega_con_acconti
            'dbRis("Netto_erogato_per_pratica") = m_Netto_erogato_per_pratica
            'dbRis("Teg_per_pratica_60_mesi") = m_Teg_per_pratica_60_mesi
            'dbRis("Teg_per_pratica_120_mesi") = m_Teg_per_pratica_120_mesi
            'dbRis("Tempi_erogazione_gg") = m_Tempi_erogazione_gg
            'dbRis("notifica_pratica_a_cura_finanziaria") = m_notifica_pratica_a_cura_finanziaria
            'dbRis("notifica_pratica_a_cura_broker") = m_notifica_pratica_a_cura_broker
            'dbRis("procura") = m_procura
            'dbRis("collegamento_per_preventivi_IP_dinamico") = m_collegamento_per_preventivi_IP_dinamico
            'dbRis("collegamento_per_preventivi_IP_statico") = m_collegamento_per_preventivi_IP_statico
            'dbRis("Tempi_liquidazione_per_tipo_prodotto_gg") = m_Tempi_liquidazione_per_tipo_prodotto_gg
            'dbRis("Distribuzione_produzione_Statale_Perc") = m_Distribuzione_produzione_Statale_Perc
            'dbRis("Distribuzione_produzione_Pubblico_Perc") = m_Distribuzione_produzione_Pubblico_Perc
            'dbRis("Distribuzione_produzione_Privato_Perc") = m_Distribuzione_produzione_Privato_Perc
            'dbRis("Distribuzione_produzione_Pensionato_Perc") = m_Distribuzione_produzione_Pensionato_Perc
            'dbRis("Fatturato_montante_lordo_mensile") = m_Fatturato_montante_lordo_mensile
            'dbRis("Fatturato_montante_lordo_annuo") = m_Fatturato_montante_lordo_annuo
            'dbRis("Max_provvigionale_caricato") = m_Max_provvigionale_caricato
            'dbRis("Note_Provvigionale_Max") = m_Note_Provvigionale_Max
            'dbRis("Proposte") = m_Proposte
            'dbRis("MAIL_referente") = m_MAIL_referente
            'dbRis("Tipologia_Med_A") = m_Tipologia_Med_A
            'dbRis("Tipologia_Med_B") = m_Tipologia_Med_B
            'dbRis("Tipologia_Med_C") = m_Tipologia_Med_C
            'dbRis("SCHEDA") = m_SCHEDA
            'dbRis("Esito") = m_Esito
            'dbRis("Raccolta_Preventivi_competitor") = m_Raccolta_Preventivi_competitor
            'dbRis("Allegato_preventivo_competitor") = m_Allegato_preventivo_competitor
            'dbRis("Stampa_Preventivi_finsab_confronto") = m_Stampa_Preventivi_finsab_confronto
            'dbRis("Allegato_preventivi_finsab") = m_Allegato_preventivi_finsab
            'dbRis("Riepilogo_esigenze_mediatore") = m_Riepilogo_esigenze_mediatore
            'dbRis("Spread_finsab_da_assegnare_conv_inpdap") = m_Spread_finsab_da_assegnare_conv_inpdap
            'dbRis("Provv_Mediatore_da_assegnare_conv_inpdap") = m_Provv_Mediatore_da_assegnare_conv_inpdap
            'dbRis("Spread_finsab_da_assegnare_ordinario") = m_Spread_finsab_da_assegnare_ordinario
            'dbRis("Provv_Mediatore_da_assegnare_ordinario") = m_Provv_Mediatore_da_assegnare_ordinario
            'dbRis("ProfiloFuturo") = m_ProfiloFuturo

            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.NomePersona
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("User", Me.UserID)
            writer.WriteAttribute("NomeUtente", Me.m_NomeUtente)
            writer.WriteAttribute("Persona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("Rapporto", Me.m_Rapporto)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("DataInizioRapporto", Me.m_DataInizioRapporto)
            writer.WriteAttribute("DataFineRapporto", Me.m_DataFineRapporto)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Referente", Me.ReferenteID)
            writer.WriteAttribute("AttivatoDa", Me.AttivatoDaID)
            writer.WriteAttribute("DataAttivazione", Me.m_DataAttivazione)
            writer.WriteAttribute("ListinoPredefinito", Me.ListinoPredefinitoID)
            writer.WriteAttribute("NumeroUIC", Me.m_NumeroIscrizioneUIC)
            writer.WriteAttribute("DataUIC", Me.m_DataIscrizioneUIC)
            writer.WriteAttribute("TipoUIC", Me.m_TipoIscrizioneUIC)
            writer.WriteAttribute("RinnovoUIC", Me.m_DataRinnovoUIC)
            writer.WriteAttribute("DataVisura", Me.m_DataVisuraProtesti)
            writer.WriteAttribute("EsitoVisura", Me.m_EsitoVisuraProtesti)
            writer.WriteAttribute("NumeroRUI", Me.m_NumeroIscrizioneRUI)
            writer.WriteAttribute("DataRUI", Me.m_DataIscrizioneRUI)
            writer.WriteAttribute("RinnovoRUI", Me.m_DataRinnovoRUI)
            writer.WriteAttribute("NumeroISVAP", Me.m_NumeroIscrizioneISVAP)
            writer.WriteAttribute("DataISVAP", Me.m_DataIscrizioneISVAP)
            writer.WriteAttribute("RinnovoISVAP", Me.m_DataRinnovoISVAP)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "User" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUtente" : Me.m_NomeUtente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Persona" : Me.m_PersonaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Rapporto" : Me.m_Rapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataInizioRapporto" : Me.m_DataInizioRapporto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFineRapporto" : Me.m_DataFineRapporto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Referente" : Me.m_ReferenteID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AttivatoDa" : Me.m_AttivatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataAttivazione" : Me.m_DataAttivazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ListinoPredefinito" : Me.m_ListinoPredefinitoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroUIC" : Me.m_NumeroIscrizioneUIC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataUIC" : Me.m_DataIscrizioneUIC = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoUIC" : Me.m_TipoIscrizioneUIC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RinnovoUIC" : Me.m_DataRinnovoUIC = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataVisura" : Me.m_DataVisuraProtesti = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "EsitoVisura" : Me.m_EsitoVisuraProtesti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroRUI" : Me.m_NumeroIscrizioneRUI = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRUI" : Me.m_DataIscrizioneRUI = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RinnovoRUI" : Me.m_DataRinnovoRUI = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NumeroISVAP" : Me.m_NumeroIscrizioneISVAP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataISVAP" : Me.m_DataIscrizioneISVAP = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RinnovoISVAP" : Me.m_DataRinnovoISVAP = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Private ReadOnly Property NomeFonte As String Implements IFonte.Nome
            Get
                Return Me.NomePersona
            End Get
        End Property

        Private ReadOnly Property _IconURL As String Implements IFonte.IconURL
            Get
                Return "/widgets/images/default.gif"
            End Get
        End Property

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.Collaboratori.UpdateCached(Me)
        End Sub


    End Class

End Class