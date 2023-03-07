Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica




Partial Public Class CQSPD

    ''' <summary>
    ''' Rappresenta dei dati aggiuntivi per la pratica (relazione 1 a 1)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CInfoPratica
        Inherits DBObjectBase

        Private m_IDPratica As Integer
        Private m_Pratica As CRapportino

        Private m_IDCommerciale As Integer
        Private m_Commerciale As CTeamManager
        'Private m_NomeCommerciale As String


        Private m_IDDistributore As Integer
        Private m_Distributore As CDistributore

        'Private m_IDPraticaOrigine      '[INT] ID della pratica originaria
        Private m_TrasferitoDaURL As String '[TEXT] 
        Private m_DataTrasferimento As Nullable(Of Date) '[DATE] Data e ora del trasferimento della pratica
        Private m_TrasferitoA As String        '[TEXT] Stringa che identifica univocamente l'azienda esterna
        Private m_IDTrasferitoDa As Integer '[INT]  ID dell'utente che ha effettuato il trasferimento
        Private m_TrasferitoDa As CUser '[CUser]Oggetto utente che ha effettuato il trasferimento
        Private m_IDPraticaTrasferita As Integer  '[INT]  ID della pratica memorizzata sul sistema dell'azienda esterna
        Private m_DataAggiornamentoPT As Nullable(Of Date) '[DATE] Data e ora dell'ultimo aggiornamento sullo stato della pratica richiesto al sistema esterno
        Private m_EsitoAggiornamentoPT As Integer '[INT]  Codice che identifica il risultato dell'ultimo aggiornamento (0 = ok, ... da definire gli errori) 

        Private m_Costo As Nullable(Of Decimal)

        Private m_IDPraticaDiRiferimento As Integer
        Private m_PraticaDiRiferimento As CRapportino

        Private m_IDCorrettaDa As Integer
        Private m_CorrettaDa As CUser
        Private m_DataCorrezione As Nullable(Of Date)
        Private m_IDCorrezione As Integer
        Private m_Correzione As COffertaCQS

        Private m_MotivoSconto As String
        Private m_MotivoScontoDettaglio As String
        Private m_IDScontoAutorizzatoDa As Integer
        Private m_ScontoAutorizzatoDa As CUser
        Private m_ScontoAutorizzatoIl As Nullable(Of Date)
        Private m_ScontoAutorizzatoNote As String

        Private m_NoteAmministrative As String

        Public Sub New()
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing

            Me.m_IDPraticaTrasferita = 0
            Me.m_DataAggiornamentoPT = Nothing
            Me.m_EsitoAggiornamentoPT = 0
            Me.m_IDTrasferitoDa = 0
            Me.m_TrasferitoDa = Nothing
            Me.m_Costo = Nothing
            Me.m_IDPraticaDiRiferimento = 0
            Me.m_PraticaDiRiferimento = Nothing
            Me.m_IDCommerciale = 0
            Me.m_Commerciale = Nothing

            Me.m_IDCorrettaDa = 0
            Me.m_CorrettaDa = Nothing
            Me.m_DataCorrezione = Nothing
            Me.m_IDCorrezione = 0
            Me.m_Correzione = Nothing

            Me.m_MotivoSconto = vbNullString
            Me.m_MotivoScontoDettaglio = vbNullString
            Me.m_IDScontoAutorizzatoDa = 0
            Me.m_ScontoAutorizzatoDa = Nothing
            Me.m_ScontoAutorizzatoIl = Nothing
            Me.m_ScontoAutorizzatoNote = vbNullString

            Me.m_NoteAmministrative = ""
        End Sub

        Public Property NoteAmministrative As String
            Get
                Return Me.m_NoteAmministrative
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NoteAmministrative
                If (oldValue = value) Then Exit Property
                Me.m_NoteAmministrative = value
                Me.DoChanged("NoteAmministrative", value, oldValue)
            End Set
        End Property

        Public Property IDCorrettaDa As Integer
            Get
                Return GetID(Me.m_CorrettaDa, Me.m_IDCorrettaDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCorrettaDa
                If (oldValue = value) Then Exit Property
                Me.m_IDCorrettaDa = value
                Me.m_CorrettaDa = Nothing
                Me.DoChanged("IDCorrettaDa", value, oldValue)
            End Set
        End Property

        Public Property CorrettaDa As CUser
            Get
                If (Me.m_CorrettaDa Is Nothing) Then Me.m_CorrettaDa = Sistema.Users.GetItemById(Me.m_IDCorrettaDa)
                Return Me.m_CorrettaDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_CorrettaDa
                If (oldValue Is value) Then Exit Property
                Me.m_CorrettaDa = value
                Me.m_IDCorrettaDa = GetID(value)
                Me.DoChanged("CorrettaDa", value, oldValue)
            End Set
        End Property

        Public Property DataCorrezione As Nullable(Of Date)
            Get
                Return Me.m_DataCorrezione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataCorrezione
                If (oldValue = value) Then Exit Property
                Me.m_DataCorrezione = value
                Me.DoChanged("DataCorrezione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica a cui fa riferimento questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPratica As Integer
            Get
                Return GetID(Me.m_Pratica, Me.m_IDPratica)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPratica
                If (oldValue = value) Then Exit Property
                Me.m_IDPratica = value
                Me.m_Pratica = Nothing
                Me.DoChanged("IDPratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica a cui fa riferimento questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pratica As CRapportino
            Get
                If (Me.m_Pratica Is Nothing) Then Me.m_Pratica = DMD.CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.m_Pratica
                If (oldValue Is value) Then Exit Property
                Me.m_Pratica = value
                Me.m_IDPratica = GetID(value)
                Me.DoChanged("Pratica", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPratica(ByVal p As CRapportino)
            Me.m_Pratica = p
            Me.m_IDPratica = GetID(p)
        End Sub


        ''' <summary>
        ''' Restituisce o imposta l'ID dell'offerta che corrisponde ad un'eventuale correzione amministrativa (contabile)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCorrezione As Integer
            Get
                Return GetID(Me.m_Correzione, Me.m_IDCorrezione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCorrezione
                If (oldValue = value) Then Exit Property
                Me.m_IDCorrezione = value
                Me.m_Correzione = Nothing
                Me.DoChanged("IDCorrezione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'offerta che corrisponde ad una eventuale correzione amministrativa della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Correzione As COffertaCQS
            Get
                If (Me.m_Correzione Is Nothing) Then Me.m_Correzione = DMD.CQSPD.Offerte.GetItemById(Me.m_IDCorrezione)
                Return Me.m_Correzione
            End Get
            Set(value As COffertaCQS)
                Dim oldValue As COffertaCQS = Me.m_Correzione
                If (oldValue Is value) Then Exit Property
                Me.m_Correzione = value
                Me.m_IDCorrezione = GetID(value)
                Me.DoChanged("Correzione", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function



        ''' <summary>
        ''' Restituisce o imposta il motivo dello sconto (eventuale) rispetto alla provvigione massima applicabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoSconto As String
            Get
                Return Me.m_MotivoSconto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_MotivoSconto
                If (oldValue = value) Then Exit Property
                Me.m_MotivoSconto = value
                Me.DoChanged("MotivoSconto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle informazioni aggiuntive sul motivo dello sconto alla provvigione massima applicabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoScontoDettaglio As String
            Get
                Return Me.m_MotivoScontoDettaglio
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_MotivoScontoDettaglio
                If (oldValue = value) Then Exit Property
                Me.m_MotivoScontoDettaglio = value
                Me.DoChanged("MotivoScontoDettaglio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha autorizzato lo sconto rispetto alla provvigione massima
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDScontoAutorizzatoDa As Integer
            Get
                Return GetID(Me.m_ScontoAutorizzatoDa, Me.m_IDScontoAutorizzatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDScontoAutorizzatoDa
                If oldValue = value Then Exit Property
                Me.m_IDScontoAutorizzatoDa = value
                Me.m_ScontoAutorizzatoDa = Nothing
                Me.DoChanged("IDScontoAutorizzatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha autorizzato lo sconto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScontoAutorizzatoDa As CUser
            Get
                If Me.m_ScontoAutorizzatoDa Is Nothing Then Me.m_ScontoAutorizzatoDa = Users.GetItemById(Me.m_IDScontoAutorizzatoDa)
                Return Me.m_ScontoAutorizzatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_ScontoAutorizzatoDa
                If (oldValue = value) Then Exit Property
                Me.m_ScontoAutorizzatoDa = value
                Me.m_IDScontoAutorizzatoDa = GetID(value)
                Me.DoChanged("ScontoAutorizzatoDa", value, oldValue)
            End Set
        End Property

        Public Property ScontoAutorizzatoIl As Nullable(Of Date)
            Get
                Return Me.m_ScontoAutorizzatoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ScontoAutorizzatoIl
                If (oldValue = value) Then Exit Property
                Me.m_ScontoAutorizzatoIl = value
                Me.DoChanged("ScontoAutorizzatoIl", value, oldValue)
            End Set
        End Property

        Public Property ScontoAutorizzatoNote As String
            Get
                Return Me.m_ScontoAutorizzatoNote
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ScontoAutorizzatoNote
                If (oldValue = value) Then Exit Property
                Me.m_ScontoAutorizzatoNote = value
                Me.DoChanged("ScontoAutorizzatoNote", value, oldValue)
            End Set
        End Property


        Public Property PraticaDiRiferimento As CRapportino
            Get
                If (Me.m_PraticaDiRiferimento Is Nothing) Then Me.m_PraticaDiRiferimento = DMD.CQSPD.Pratiche.GetItemById(Me.m_IDPraticaDiRiferimento)
                Return Me.m_PraticaDiRiferimento
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.m_PraticaDiRiferimento
                If (oldValue = value) Then Exit Property
                Me.m_PraticaDiRiferimento = value
                Me.m_IDPraticaDiRiferimento = GetID(value)
                Me.DoChanged("PraticaDiRiferimento", value, oldValue)
            End Set
        End Property

        Public Property IDPraticaDiRiferimento As Integer
            Get
                Return GetID(Me.m_PraticaDiRiferimento, Me.m_IDPraticaDiRiferimento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPraticaDiRiferimento
                If oldValue = value Then Exit Property
                Me.m_PraticaDiRiferimento = Nothing
                Me.m_IDPraticaDiRiferimento = value
                Me.DoChanged("IDPraticaDiRiferimento", value, oldValue)
            End Set
        End Property

        Public Property Commerciale As CTeamManager
            Get
                If (Me.m_Commerciale Is Nothing) Then Me.m_Commerciale = DMD.CQSPD.TeamManagers.GetItemById(Me.m_IDCommerciale)
                Return Me.m_Commerciale
            End Get
            Set(value As CTeamManager)
                Dim oldValue As CTeamManager = Me.m_Commerciale
                If (oldValue = value) Then Exit Property
                Me.m_Commerciale = value
                Me.m_IDCommerciale = GetID(value)
                'If (value IsNot Nothing) Then Me.m_NomeCommerciale = value.Nominativo
                Me.DoChanged("Commerciale", value, oldValue)
            End Set
        End Property

        Public Property IDCommerciale As Integer
            Get
                Return GetID(Me.m_Commerciale, Me.m_IDCommerciale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCommerciale
                If oldValue = value Then Exit Property
                Me.m_IDCommerciale = value
                Me.m_Commerciale = Nothing
                Me.DoChanged("IDCommerciale", value, oldValue)
            End Set
        End Property

        Public Property IDDistributore As Integer
            Get
                Return GetID(Me.m_Distributore, Me.m_IDDistributore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDistributore
                If (oldValue = value) Then Exit Property
                Me.m_IDDistributore = value
                Me.m_Distributore = Nothing
                Me.DoChanged("IDDistributore", value, oldValue)
            End Set
        End Property

        Public Property Distributore As CDistributore
            Get
                If (Me.m_Distributore Is Nothing) Then Me.m_Distributore = Anagrafica.Distributori.GetItemById(Me.m_IDDistributore)
                Return Me.m_Distributore
            End Get
            Set(value As CDistributore)
                Dim oldValue As CDistributore = Me.m_Distributore
                If (oldValue Is value) Then Exit Property
                Me.m_Distributore = value
                Me.m_IDDistributore = GetID(value)
                Me.DoChanged("Distributore", value, oldValue)
            End Set
        End Property

        'Public Property NomeCommerciale As String
        '    Get
        '        Return Me.m_NomeCommerciale
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_NomeCommerciale
        '        If (oldValue = value) Then Exit Property
        '        Me.m_NomeCommerciale = value
        '        Me.DoChanged("NomeCommerciale", value, oldValue)
        '    End Set
        'End Property




        'Public Property NomeConsulente As String
        '    Get
        '        Return Me.m_NomeConsulente
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_NomeConsulente
        '        If (oldValue = value) Then Exit Property
        '        Me.m_NomeConsulente = value
        '        Me.DoChanged("NomeConsulente", value, oldValue)
        '    End Set
        'End Property

        Public Property Costo As Nullable(Of Decimal)
            Get
                Return Me.m_Costo
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Costo
                If (oldValue = value) Then Exit Property
                Me.m_Costo = value
                Me.DoChanged("Costo", value, oldValue)
            End Set
        End Property



        Public Property DataTrasferimento As Nullable(Of Date)
            Get
                Return Me.m_DataTrasferimento
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataTrasferimento
                If (oldValue = value) Then Exit Property
                Me.m_DataTrasferimento = value
                Me.DoChanged("DataTrasferimento", value, oldValue)
            End Set
        End Property

        Public Property TrasferitoA As String
            Get
                Return Me.m_TrasferitoA
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TrasferitoA
                If (oldValue = value) Then Exit Property
                Me.m_TrasferitoA = value
                Me.DoChanged("TrasferitoA", value, oldValue)
            End Set
        End Property

        Public Property IDTrasferitoDa As Integer
            Get
                Return GetID(Me.m_TrasferitoDa, Me.m_IDTrasferitoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTrasferitoDa
                If oldValue = value Then Exit Property
                Me.m_TrasferitoDa = Nothing
                Me.m_IDTrasferitoDa = value
                Me.DoChanged("TrasferitoDaID", value, oldValue)
            End Set
        End Property

        Public Property TrasferitoDa As CUser
            Get
                If (Me.m_TrasferitoDa Is Nothing) Then Me.m_TrasferitoDa = Sistema.Users.GetItemById(Me.m_IDTrasferitoDa)
                Return Me.m_TrasferitoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_TrasferitoDa
                If (oldValue = value) Then Exit Property
                Me.m_TrasferitoDa = value
                Me.m_IDTrasferitoDa = GetID(value)
                Me.DoChanged("TrasferitoDa", value, oldValue)
            End Set
        End Property

        Public Property TrasferitoDaURL As String
            Get
                Return Me.m_TrasferitoDaURL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TrasferitoDaURL
                If (oldValue = value) Then Exit Property
                Me.m_TrasferitoDaURL = value
                Me.DoChanged("TrasferitoDaURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica memorizzata sul sistema esterno presso cui è stata inviata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPraticaTrasferita As Integer
            Get
                Return Me.m_IDPraticaTrasferita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDPraticaTrasferita
                If (oldValue = value) Then Exit Property
                Me.m_IDPraticaTrasferita = value
                Me.DoChanged("IDPraticaTrasferita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo controllo fatto sul sistema esterno per verificare lo stato della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAggiornamentoPT As Nullable(Of Date)
            Get
                Return Me.m_DataAggiornamentoPT
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAggiornamentoPT
                If (oldValue = value) Then Exit Property
                Me.m_DataAggiornamentoPT = value
                Me.DoChanged("DataAggiornamentoPT", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice di errore relativo all'ultimo controllo fatto sul sistema esterno per verificare lo stato della pratica.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EsitoAggiornamentoPT As Integer
            Get
                Return Me.m_EsitoAggiornamentoPT
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_EsitoAggiornamentoPT
                If (oldValue = value) Then Exit Property
                Me.m_EsitoAggiornamentoPT = value
                Me.DoChanged("EsitoAggiornamentoPT", value, oldValue)
            End Set
        End Property



        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheInfo"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDPratica = reader.Read("IDPratica", Me.m_IDPratica)
            Me.m_IDCommerciale = reader.Read("IDCommerciale", Me.m_IDCommerciale)
            Me.m_IDDistributore = reader.Read("IDDistributore", Me.m_IDDistributore)
            Me.m_TrasferitoDaURL = reader.Read("TrasferitaDaURL", Me.m_TrasferitoDaURL)
            Me.m_DataTrasferimento = reader.Read("DataTrasferimento", Me.m_DataTrasferimento)
            Me.m_TrasferitoA = reader.Read("TrasferitoA", Me.m_TrasferitoA)
            Me.m_IDTrasferitoDa = reader.Read("IDTrasferitoDa", Me.m_IDTrasferitoDa)
            Me.m_IDPraticaTrasferita = reader.Read("IDPraticaTrasferita", Me.m_IDPraticaTrasferita)
            Me.m_DataAggiornamentoPT = reader.Read("DataAggiornamentoPT", Me.m_DataAggiornamentoPT)
            Me.m_EsitoAggiornamentoPT = reader.Read("EsitoAggiornamentoPT", Me.m_EsitoAggiornamentoPT) 'Me.m_EsitoAggiornamentoPT = reader.GetValue(Of Integer)("EsitoAggiornamentoPT", 0)
            Me.m_Costo = reader.Read("Costo", Me.m_Costo)
            Me.m_IDPraticaDiRiferimento = reader.Read("IDPraticaDiRiferimento", Me.m_IDPraticaDiRiferimento)
            Me.m_IDCorrezione = reader.Read("IDCorrezione", Me.m_IDCorrezione)
            Me.m_MotivoSconto = reader.Read("MotivoSconto", Me.m_MotivoSconto)
            Me.m_MotivoScontoDettaglio = reader.Read("MotivoScontoDettaglio", Me.m_MotivoScontoDettaglio)
            Me.m_IDScontoAutorizzatoDa = reader.Read("IDScontoAutorizzatoDa", Me.m_IDScontoAutorizzatoDa)
            Me.m_ScontoAutorizzatoIl = reader.Read("ScontoAutorizzatoIl", Me.m_ScontoAutorizzatoIl)
            Me.m_ScontoAutorizzatoNote = reader.Read("ScontoAutorizzatoNote", Me.m_ScontoAutorizzatoNote)
            Me.m_IDCorrettaDa = reader.Read("IDCorrettaDa", Me.m_IDCorrettaDa)
            Me.m_DataCorrezione = reader.Read("DataCorrezione", Me.m_DataCorrezione)
            Me.m_NoteAmministrative = reader.Read("NoteAmministrative", Me.m_NoteAmministrative)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDPratica", Me.IDPratica)
            writer.Write("MotivoSconto", Me.m_MotivoSconto)
            writer.Write("MotivoScontoDettaglio", Me.m_MotivoScontoDettaglio)
            writer.Write("IDScontoAutorizzatoDa", Me.IDScontoAutorizzatoDa)
            writer.Write("ScontoAutorizzatoIl", Me.m_ScontoAutorizzatoIl)
            writer.Write("ScontoAutorizzatoNote", Me.m_ScontoAutorizzatoNote)
            writer.Write("IDCorrezione", Me.IDCorrezione)
            writer.Write("IDCommerciale", Me.IDCommerciale)
            writer.Write("IDPraticaDiRiferimento", Me.IDPraticaDiRiferimento)
            writer.Write("TrasferitaDaURL", Me.m_TrasferitoDaURL)
            writer.Write("DataTrasferimento", Me.m_DataTrasferimento)
            writer.Write("TrasferitoA", Me.m_TrasferitoA)
            writer.Write("IDTrasferitoDa", Me.IDTrasferitoDa)
            writer.Write("DataAggiornamentoPT", Me.m_DataAggiornamentoPT)
            writer.Write("EsitoAggiornamentoPT", Me.m_EsitoAggiornamentoPT) 'Me.m_EsitoAggiornamentoPT = reader.GetValue(Of Integer)("EsitoAggiornamentoPT", 0)
            writer.Write("Costo", Me.m_Costo)
            writer.Write("IDPraticaTrasferita", Me.m_IDPraticaTrasferita)
            writer.Write("IDDistributore", Me.IDDistributore)
            writer.Write("IDCorrettaDa", Me.IDCorrettaDa)
            writer.Write("DataCorrezione", Me.m_DataCorrezione)
            writer.Write("NoteAmministrative", Me.m_NoteAmministrative)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("IDCommerciale", Me.IDCommerciale)
            writer.WriteAttribute("TrasferitoDaURL", Me.m_TrasferitoDaURL)
            writer.WriteAttribute("DataTrasferimento", Me.m_DataTrasferimento)
            writer.WriteAttribute("TrasferitoA", Me.m_TrasferitoA)
            writer.WriteAttribute("TrasferitaDaURL", Me.m_TrasferitoDaURL)
            writer.WriteAttribute("TrasferitoDaID", Me.IDTrasferitoDa)
            writer.WriteAttribute("IDPraticaTrasferita", Me.IDPraticaTrasferita)
            writer.WriteAttribute("DataAggiornamentoPT", Me.m_DataAggiornamentoPT)
            writer.WriteAttribute("EsitoAggiornamentoPT", Me.m_EsitoAggiornamentoPT)
            writer.WriteAttribute("Costo", Me.m_Costo)
            writer.WriteAttribute("PraticaDiRiferimentoID", Me.IDPraticaDiRiferimento)
            writer.WriteAttribute("MotivoSconto", Me.m_MotivoSconto)
            writer.WriteAttribute("IDScontoAutorizzatoDa", Me.IDScontoAutorizzatoDa)
            writer.WriteAttribute("ScontoAutorizzatoIl", Me.m_ScontoAutorizzatoIl)
            writer.WriteAttribute("ScontoAutorizzatoNote", Me.m_ScontoAutorizzatoNote)
            writer.WriteAttribute("IDCorrezione", Me.IDCorrezione)
            writer.WriteAttribute("IDDistributore", Me.IDDistributore)
            writer.WriteAttribute("IDCorrettaDa", Me.IDCorrettaDa)
            writer.WriteAttribute("DataCorrezione", Me.m_DataCorrezione)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("MotivoScontoDettaglio", Me.m_MotivoScontoDettaglio)
            writer.WriteTag("NoteAmministrative", Me.m_NoteAmministrative)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCommerciale" : Me.m_IDCommerciale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TrasferitoDaURL" : Me.m_TrasferitoDaURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataTrasferimento" : Me.m_DataTrasferimento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TrasferitoA" : Me.m_TrasferitoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TrasferitaDaURL" : Me.m_TrasferitoDaURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TrasferitoDaID" : Me.m_IDTrasferitoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPraticaTrasferita" : Me.m_IDPraticaTrasferita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataAggiornamentoPT" : Me.m_DataAggiornamentoPT = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "EsitoAggiornamentoPT" : Me.m_EsitoAggiornamentoPT = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Costo" : Me.m_Costo = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "PraticaDiRiferimentoID" : Me.m_IDPraticaDiRiferimento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MotivoSconto" : Me.m_MotivoSconto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MotivoScontoDettaglio" : Me.m_MotivoScontoDettaglio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDScontoAutorizzatoDa" : Me.m_IDScontoAutorizzatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    ' Case "NomeScontoAutorizzatoDa" : Me.m_NomeScontoAutorizzatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ScontoAutorizzatoIl" : Me.m_ScontoAutorizzatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ScontoAutorizzatoNote" : Me.m_ScontoAutorizzatoNote = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCorrezione" : Me.m_IDCorrezione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDistributore" : Me.m_IDDistributore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCorrettaDa" : Me.m_IDCorrettaDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataCorrezione" : Me.m_DataCorrezione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NoteAmministrative" : Me.m_NoteAmministrative = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class
