Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports DMD.CustomerCalls

Partial Public Class CQSPD

    <Serializable> _
    Public Class COpStatRecord
        Implements DMD.XML.IDMDXMLSerializable

        Private m_IDPuntoOperativo As Integer
        Private m_IDOperatore As Integer
        Private m_FromDate As Date?
        Private m_ToDate As Date?

        Private m_OnlyValid As Boolean
        Private m_Changed As Boolean
        Private m_IgnoreRights As Boolean

        Private m_OutCalls As New CStatisticheOperazione
        Private m_InCalls As New CStatisticheOperazione
        Private m_OutDates As New CStatisticheOperazione
        Private m_InDates As New CStatisticheOperazione
        Private m_VisiteDistinteRicevute As New CStatisticheOperazione

        Private m_PraticheCDN As Integer
        Private m_PraticheCDM As Decimal
        Private m_PersoneContattate As Integer
        Private m_Consulenze As New CStatisticheOperazione
        Private m_Richieste As New CStatisticheOperazione
        Private m_DurataConsulenze As Double
        Private m_ConteggioConsulenzeVisiteEsterne As Integer
        Private m_ConteggioConsulenzeVisiteUfficio As Integer
        Private m_ConteggioConsulenzeTelefonateRicevute As Integer
        Private m_ConteggioConsulenzeTelefonateEffettuate As Integer
        Private m_PraticheConsulente As New CStatisticheOperazione
        Private m_PraticheConcluseConsulente As New CStatisticheOperazione
        Private m_PraticheAnnullateConsulente As New CStatisticheOperazione
        Private m_PraticheConcluseConsulenza As New CStatisticheOperazione
        Private m_PraticheAnnullateConsulenza As New CStatisticheOperazione
        Private m_Infos As CKeyCollection(Of CStatisticheOperazione)
        Private m_ArrOperatori() As Integer
        Private m_ArrConsulenti() As Integer






        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_OnlyValid = True
            Me.m_Changed = False
            Me.m_IgnoreRights = False
            Me.m_IDOperatore = 0
            Me.m_IDPuntoOperativo = 0
            Me.m_FromDate = Calendar.ToDay
            Me.m_ToDate = DateAdd("d", 1, Me.m_FromDate)
            Me.Reset()
        End Sub

        Public Sub Invalidate()
            Me.m_Changed = True
            Me.Reset()
        End Sub

        Public Sub Validate()
            If Me.m_Changed = True Then Me.Calculate()
        End Sub

        Public Function Stato(ByVal s As CStatoPratica) As CStatisticheOperazione
            Me.Validate()
            Return Me.m_Infos.GetItemByKey("K" & GetID(s))
        End Function

        Public ReadOnly Property PraticheConcluseConsulenza As CStatisticheOperazione
            Get
                Return Me.m_PraticheConcluseConsulenza
            End Get
        End Property

        Public ReadOnly Property PraticheAnnullateConsulenza As CStatisticheOperazione
            Get
                Return Me.m_PraticheAnnullateConsulenza
            End Get
        End Property

        Public ReadOnly Property PraticheConsulente As CStatisticheOperazione
            Get
                Return Me.m_PraticheConsulente
            End Get
        End Property

        Public ReadOnly Property PraticheConcluseConsulente As CStatisticheOperazione
            Get
                Return Me.m_PraticheConcluseConsulente
            End Get
        End Property

        Public ReadOnly Property PraticheAnnullateConsulente As CStatisticheOperazione
            Get
                Return Me.m_PraticheAnnullateConsulente
            End Get
        End Property


        Public ReadOnly Property Consulenze As CStatisticheOperazione
            Get
                Me.Validate()
                Return Me.m_Consulenze
            End Get
        End Property


        Public ReadOnly Property RichiesteDiFinanziamento As CStatisticheOperazione
            Get
                Me.Validate()
                Return Me.m_Richieste
            End Get
        End Property



        ''' <summary>
        ''' Restituisce il numero totale di consulenze esterne
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConteggioConsulenzeVisiteEsterne As Integer
            Get
                Me.Validate()
                Return Me.m_ConteggioConsulenzeVisiteEsterne
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il numero totale di consulenze in ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConteggioConsulenzeVisiteUfficio As Integer
            Get
                Me.Validate()
                Return Me.m_ConteggioConsulenzeVisiteUfficio
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il numero totale di consulenze effettuate durante una telefonata ricevuta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConteggioConsulenzeTelefonateRicevute As Integer
            Get
                Me.Validate()
                Return Me.m_ConteggioConsulenzeTelefonateRicevute
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il numero totale di consulenze effettuate durante una telefonata effettuata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConteggioConsulenzeTelefonateEffettuate As Integer
            Get
                Me.Validate()
                Return Me.m_ConteggioConsulenzeTelefonateEffettuate
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'oggetto deve eseguire i calcoli come superuser
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IgnoreRights As Boolean
            Get
                Return Me.m_IgnoreRights
            End Get
            Set(value As Boolean)
                If (Me.m_IgnoreRights = value) Then Exit Property
                Me.m_IgnoreRights = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore di cui restituire le statistiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return Me.m_IDOperatore
            End Get
            Set(value As Integer)
                If (Me.m_IDOperatore = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del punto operativo a cui restringere le statistiche.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Se IDOperatore è 0 l'oggetto effettua la somma di tutti gli operatori appartenenti al punto operativo. Se, invece, IDOperatore è non 0 e IDPuntoOperativo è non zero l'oggetto restringe le statistiche dell'operatore al solo punto operativo specificato</remarks>
        Public Property IDPuntoOperativo As Integer
            Get
                Return Me.m_IDPuntoOperativo
            End Get
            Set(value As Integer)
                If (Me.m_IDPuntoOperativo = value) Then Exit Property
                Me.m_IDPuntoOperativo = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data iniziale dell'intervallo considerato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromDate As Date?
            Get
                Return Me.m_FromDate
            End Get
            Set(value As Date?)
                If (Calendar.Compare(value, Me.m_FromDate) = 0) Then Exit Property
                Me.m_FromDate = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data finale dell'intervallo considerato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ToDate As Date?
            Get
                Return Me.m_ToDate
            End Get
            Set(value As Date?)
                If (Calendar.Compare(Me.m_ToDate, value) = 0) Then Exit Property
                Me.m_ToDate = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il periodo considerato
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Sub SetPeriodo(ByVal value As String)
            Dim intervallo As CIntervalloData = Calendar.PeriodoToDates(value, Me.m_FromDate, Me.m_ToDate)
            Me.m_FromDate = intervallo.Inizio
            Me.m_ToDate = intervallo.Fine
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se considerare i soli utenti validi (quando IDPuntoOperativo è non nullo e IDOperatore è nullo)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                If (Me.m_OnlyValid = value) Then Exit Property
                Me.m_OnlyValid = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il numero di persone fisiche (diverse) contattate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PersoneContattate As Integer
            Get
                Me.Validate()
                Return Me.m_PersoneContattate
            End Get
        End Property


        Public ReadOnly Property InCalls As CStatisticheOperazione
            Get
                Me.Validate()
                Return Me.m_InCalls
            End Get
        End Property


        Public ReadOnly Property OutCalls As CStatisticheOperazione
            Get
                Me.Validate()
                Return Me.m_OutCalls
            End Get
        End Property



        Public ReadOnly Property OutDates As CStatisticheOperazione
            Get
                Me.Validate()
                Return Me.m_OutDates
            End Get
        End Property


        Public ReadOnly Property InDates As CStatisticheOperazione
            Get
                Me.Validate()
                Return Me.m_InDates
            End Get
        End Property

        Public ReadOnly Property VisiteDistinteRicevute As CStatisticheOperazione
            Get
                Me.Validate()
                Return Me.m_VisiteDistinteRicevute
            End Get
        End Property



        ''' <summary>
        ''' Restituisce il numero di pratiche in cui la fonte è impostata come 'Contatto Diretto' e l'operatore appartiene al gruppo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PraticheContattoDirettoN As Integer
            Get
                Me.Validate()
                Return Me.m_PraticheCDN
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il montante lordo delle pratiche in cui la fonte è impostata come 'Contatto diretto' e l'operatore appartiene al gruppo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PraticheContattoDirettoM As Decimal
            Get
                Me.Validate()
                Return Me.m_PraticheCDM
            End Get
        End Property


        ''' <summary>
        ''' Calcola
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Calculate()
            Me.m_Changed = False
            Me.Reset()
            If Me.IDOperatore = 0 Then
                Me.AddData(Me.GetUtenti)
            Else
                Me.AddData(New Integer() {Me.IDOperatore})
            End If
        End Sub

        Private Sub Reset()
            Me.m_OutCalls = New CStatisticheOperazione
            Me.m_InCalls = New CStatisticheOperazione
            Me.m_OutDates = New CStatisticheOperazione
            Me.m_InDates = New CStatisticheOperazione
            Me.m_VisiteDistinteRicevute = New CStatisticheOperazione
            Me.m_PraticheConsulente = New CStatisticheOperazione
            Me.m_PraticheConcluseConsulente = New CStatisticheOperazione
            Me.m_PraticheAnnullateConsulente = New CStatisticheOperazione
            Me.m_PraticheConcluseConsulenza = New CStatisticheOperazione
            Me.m_PraticheAnnullateConsulenza = New CStatisticheOperazione
            Me.m_Richieste = New CStatisticheOperazione
            Me.m_PraticheCDM = 0
            Me.m_PraticheCDN = 0
            Me.m_PersoneContattate = 0
            Me.m_Consulenze = New CStatisticheOperazione
            Me.m_DurataConsulenze = 0
            Me.m_ConteggioConsulenzeTelefonateEffettuate = 0
            Me.m_ConteggioConsulenzeTelefonateRicevute = 0
            Me.m_ConteggioConsulenzeVisiteEsterne = 0
            Me.m_ConteggioConsulenzeVisiteUfficio = 0
            Me.m_PraticheCDM = 0
            Me.m_PraticheCDN = 0
            Me.m_PersoneContattate = 0
            Me.m_DurataConsulenze = 0
            Me.m_ConteggioConsulenzeVisiteEsterne = 0
            Me.m_ConteggioConsulenzeVisiteUfficio = 0
            Me.m_ConteggioConsulenzeTelefonateRicevute = 0
            Me.m_ConteggioConsulenzeTelefonateEffettuate = 0
            Me.m_Infos = New CKeyCollection(Of CStatisticheOperazione)
        End Sub

        Private Function GetUtenti() As Integer()
            Dim ret As New System.Collections.ArrayList
            Dim user As CUser
            Dim i As Integer
            If (Me.m_IDPuntoOperativo = 0) Then
                For Each user In Sistema.Users.LoadAll
                    If user.Visible AndAlso (OnlyValid = False OrElse user.UserStato = UserStatus.USER_ENABLED) Then
                        ret.Add(GetID(user))
                    End If
                Next
            Else
                Dim ufficio As CUfficio = Anagrafica.Uffici.GetItemById(Me.m_IDPuntoOperativo)
                For i = 0 To ufficio.Utenti.Count - 1
                    user = ufficio.Utenti(i)
                    If (user IsNot Nothing) AndAlso (user.Visible) AndAlso (OnlyValid = False OrElse user.UserStato = UserStatus.USER_ENABLED) Then
                        ret.Add(GetID(user))
                    End If
                Next
            End If

            Return ret.ToArray(GetType(Integer))
        End Function

        Private Sub AddData(ByVal operatori As Integer())
            Me.m_ArrOperatori = operatori
            Me.m_ArrConsulenti = Nothing

            Dim idConsulenti As New System.Collections.ArrayList
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            Dim opArray As String = DBUtils.MakeArrayStr(operatori)
            Dim telStats As CStatisticheOperazione
            Dim t As Double = Timer
            telStats = CustomerCalls.Telefonate.GetOutCallsStats(Me.m_IDPuntoOperativo, operatori, Me.m_FromDate, Me.m_ToDate, Me.m_IgnoreRights)
            Debug.Print("ContaTelefonateEffettuatePerData: " & (Timer - t))

            Me.m_OutCalls.Numero += telStats.Numero
            Me.m_OutCalls.TotalLen += telStats.TotalLen
            Me.m_OutCalls.TotalWait += telStats.TotalWait
            Me.m_OutCalls.MinWait = IIf(Me.m_OutCalls.Numero > 0, Math.Min(Me.m_OutCalls.MinWait, telStats.MinWait), telStats.MinWait)
            Me.m_OutCalls.MinLen = Math.Min(Me.m_OutCalls.MinLen, telStats.MinLen)
            Me.m_OutCalls.MaxLen = Math.Max(Me.m_OutCalls.MaxLen, telStats.MaxLen)

            t = Timer
            telStats = CustomerCalls.Telefonate.GetInCallStats(Me.m_IDPuntoOperativo, operatori, Me.m_FromDate, Me.m_ToDate, Me.m_IgnoreRights)
            Debug.Print("ContaTelefonateRicevutePerData: " & (Timer - t))
            Me.m_InCalls.Numero += telStats.Numero
            Me.m_InCalls.TotalLen += telStats.TotalLen
            Me.m_InCalls.TotalWait += telStats.TotalWait
            Me.m_InCalls.MinLen = Math.Min(Me.m_InCalls.MinLen, telStats.MinLen)
            Me.m_InCalls.MaxLen = Math.Max(Me.m_InCalls.MaxLen, telStats.MaxLen)

            t = Timer
            ' Me.m_PersoneContattate += CustomerCalls.Telefonate.ContaPersoneContattate(Me.m_IDPuntoOperativo, operatori, Me.m_FromDate, Me.m_ToDate, Me.m_IgnoreRights)


            telStats = Visite.GetOutVisitsStats(Me.m_IDPuntoOperativo, operatori, Me.m_FromDate, Me.m_ToDate, Me.m_IgnoreRights)
            Me.m_OutDates.Numero += telStats.Numero
            Me.m_OutDates.TotalLen += telStats.TotalLen
            Me.m_OutDates.TotalWait += telStats.TotalWait
            Me.m_OutDates.MinLen = Math.Min(Me.m_OutDates.MinLen, telStats.MinLen)
            Me.m_OutDates.MaxLen = Math.Max(Me.m_OutDates.MaxLen, telStats.MaxLen)

            telStats = Visite.GetInVisitsStats(Me.m_IDPuntoOperativo, operatori, Me.m_FromDate, Me.m_ToDate, Me.m_IgnoreRights)
            Me.m_InDates.Numero += telStats.Numero
            Me.m_InDates.TotalLen += telStats.TotalLen
            Me.m_InDates.TotalWait += telStats.TotalWait
            Me.m_InDates.MinLen = Math.Min(Me.m_InDates.MinLen, telStats.MinLen)
            Me.m_InDates.MaxLen = Math.Max(Me.m_InDates.MaxLen, telStats.MaxLen)


            ''Pratiche come consulente
            If (operatori.Length = 0) Then idConsulenti.Add(0)

            For i As Integer = 0 To UBound(operatori)
                Dim consulente As CConsulentePratica = DMD.CQSPD.Consulenti.GetItemByUser(operatori(i))
                If GetID(consulente) <> 0 Then idConsulenti.Add(GetID(consulente))
            Next

            If (idConsulenti.Count > 0) Then
                Me.m_ArrConsulenti = idConsulenti.ToArray(GetType(Integer))
                Dim opConsulente As String = DBUtils.MakeArrayStr(Me.m_ArrConsulenti)

                'Contiamo le consulenze
                t = Timer
                dbSQL = "SELECT Count(*) As [Num], Sum([tbl_CQSPDConsulenze].[Durata]) As [DurataTotale] FROM [tbl_CQSPDConsulenze] WHERE [IDConsulente] In (" & opConsulente & ") And [Stato]=" & ObjectStatus.OBJECT_VALID
                If (Me.m_FromDate.HasValue) Then dbSQL &= " AND [DataConsulenza]>=" & DBUtils.DBDate(Me.m_FromDate)
                If (Me.m_ToDate.HasValue) Then dbSQL &= " And [DataConsulenza]<" & DBUtils.DBDate(Me.m_ToDate)
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                If dbRis.Read Then
                    Me.m_Consulenze.Numero += Formats.ToInteger(dbRis("Num"))
                    Me.m_Consulenze.TotalLen += Formats.ToDouble(dbRis("DurataTotale"))
                End If
                dbRis.Dispose()
                Debug.Print("Pratiche come consulente: " & (Timer - t))


                'Contiamo le pratiche concluse 
                Dim cursor As New CRapportiniCursor
                dbRis = Nothing
                cursor.IgnoreRights = True
                cursor.Fields.Clear()
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDStatoAttuale.ValueIn(New Object() {GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)), GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA))})
                cursor.IDConsulente.ValueIn(m_ArrConsulenti)
                cursor.StatoContatto.Clear()
                cursor.StatoContatto.IDToStato = GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO))
                cursor.StatoContatto.MacroStato = Nothing
                cursor.StatoContatto.Inizio = Me.m_FromDate
                cursor.StatoContatto.Fine = Me.m_ToDate
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                cursor.Flags.IncludeNulls = True

                Try
                    dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ")"
                    dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                    If (dbRis.Read) Then
                        Me.m_PraticheConcluseConsulenza.Numero = Formats.ToInteger(dbRis("Num"))
                        Me.m_PraticheConcluseConsulenza.Valore = Formats.ToValuta(dbRis("ML"))
                    End If
                Catch ex As Exception
                    Throw
                Finally
                    If dbRis IsNot Nothing Then dbRis.Dispose()
                    dbRis = Nothing
                    cursor.Reset1()
                End Try

                cursor.Fields.Clear()
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDStatoAttuale.ValueIn(New Object() {GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA))})
                cursor.StatoContatto.Clear()
                cursor.StatoContatto.IDToStato = GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO))
                cursor.StatoContatto.Inizio = Me.m_FromDate
                cursor.StatoContatto.Fine = Me.m_ToDate
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                cursor.Flags.IncludeNulls = True

                Try
                    dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ")"
                    dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                    If (dbRis.Read) Then
                        Me.m_PraticheAnnullateConsulenza.Numero = Formats.ToInteger(dbRis("Num"))
                        Me.m_PraticheAnnullateConsulenza.Valore = Formats.ToValuta(dbRis("ML"))
                    End If
                Catch ex As Exception
                    Throw
                Finally
                    dbRis.Dispose()
                    cursor.Reset1()
                End Try

                '------------------------
                cursor.Fields.Clear()
                cursor.WhereClauses.Clear()
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDStatoAttuale.Clear()
                ' cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                cursor.IDConsulente.ValueIn(Me.m_ArrConsulenti)
                cursor.StatoContatto.Clear()
                cursor.StatoContatto.IDToStato = GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO))
                cursor.StatoContatto.Inizio = Me.m_FromDate
                cursor.StatoContatto.Fine = Me.m_ToDate
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                cursor.Flags.IncludeNulls = True

                dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ")"
                Try
                    dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                    If (dbRis.Read) Then
                        Me.m_PraticheConsulente.Numero = Formats.ToInteger(dbRis("Num"))
                        Me.m_PraticheConsulente.Valore = Formats.ToValuta(dbRis("ML"))
                    End If
                Catch ex As Exception
                    Throw
                Finally
                    dbRis.Dispose()
                    cursor.Reset1()
                End Try

                cursor.Fields.Clear()
                cursor.WhereClauses.Clear()
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDStatoAttuale.ValueIn(New Object() {GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)), GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA))})
                ' cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                cursor.IDConsulente.ValueIn(Me.m_ArrConsulenti)
                cursor.StatoContatto.Clear()
                cursor.StatoContatto.IDToStato = GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO))
                cursor.StatoContatto.Inizio = Me.m_FromDate
                cursor.StatoContatto.Fine = Me.m_ToDate
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                cursor.Flags.IncludeNulls = True

                dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ")"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    Me.m_PraticheConcluseConsulente.Numero = Formats.ToInteger(dbRis("Num"))
                    Me.m_PraticheConcluseConsulente.Valore = Formats.ToValuta(dbRis("ML"))
                End If
                dbRis.Dispose()
                cursor.Reset1()

                cursor.Fields.Clear()
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDStatoAttuale.ValueIn(New Object() {GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA))})
                'cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                cursor.IDConsulente.ValueIn(Me.m_ArrConsulenti)
                cursor.StatoContatto.Clear()
                cursor.StatoContatto.IDToStato = GetID(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO))
                cursor.StatoContatto.Inizio = Me.m_FromDate
                cursor.StatoContatto.Fine = Me.m_ToDate
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                cursor.Flags.IncludeNulls = True

                dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ")"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    Me.m_PraticheAnnullateConsulente.Numero = Formats.ToInteger(dbRis("Num"))
                    Me.m_PraticheAnnullateConsulente.Valore = Formats.ToValuta(dbRis("ML"))
                End If
                dbRis.Dispose()
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


            End If

            'Richieste registrate
            dbSQL = "SELECT Count(*) As [Num], Sum([Durata]) As [Durata] FROM [tbl_RichiesteFinanziamenti] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND  [IDPresaInCaricoDa] In (" & opArray & ") "
            If (Me.m_FromDate.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(Me.m_FromDate)
            If (Me.m_ToDate.HasValue) Then dbSQL &= " And [Data]<" & DBUtils.DBDate(Me.m_ToDate)

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            If (dbRis.Read) Then
                Me.m_Richieste.Numero = Formats.ToInteger(dbRis("Num"))
                Me.m_Richieste.TotalLen = Formats.ToInteger(dbRis("Durata"))
            End If

            'Pratiche inserite 
            For Each s As CStatoPratica In CQSPD.StatiPratica.GetSequenzaStandard
                Dim info As CStatisticheOperazione
                'dbSQL = "SELECT Count(*) As [Num], Sum([tbl_Rapportini].[MontanteLordo]) As [ML] FROM [tbl_Rapportini] INNER JOIN [tbl_PraticheSTL] ON [tbl_Rapportini].[ID]=[tbl_PraticheSTL].[IDPratica] WHERE [tbl_Rapportini].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND (([tbl_Rapportini].[Flags] AND " & PraticaFlags.HIDDEN & ")<>" & PraticaFlags.HIDDEN & ")  AND ([tbl_PraticheSTL].[IDOperatore] In (" & opArray & ")) "
                'If (Me.m_FromDate.HasValue) Then dbSQL &= " AND [tbl_PraticheSTL].[Data]>=" & DBUtils.DBDate(Me.m_FromDate)
                'If (Me.m_ToDate.HasValue) Then dbSQL &= " And [tbl_PraticheSTL].[Data]<" & DBUtils.DBDate(Me.m_ToDate)
                'dbSQL &= " AND [tbl_PraticheSTL].[IDToStato] = " & GetID(s)
                'For Each opid In operatori
                Dim cursor As New CRapportiniCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                If (Me.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = Me.IDPuntoOperativo

                cursor.StatoContatto.MacroStato = Nothing
                cursor.StatoContatto.IDToStato = GetID(s)
                cursor.StatoContatto.Inizio = Me.m_FromDate
                cursor.StatoContatto.Fine = Me.m_ToDate
                If (Me.IDOperatore <> 0) Then cursor.StatoContatto.IDOperatore = Me.IDOperatore
                dbSQL = "SELECT Count(*) As [Num], Sum([tbl_Rapportini].[MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ")"
                cursor.Dispose()

                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                If dbRis.Read Then
                    info = Me.m_Infos.GetItemByKey("K" & GetID(s))
                    If (info Is Nothing) Then
                        info = New CStatisticheOperazione
                        info.Numero = 0
                        info.Valore = 0
                        Me.m_Infos.Add("K" & GetID(s), info)
                    End If
                    'info.Stato = s
                    info.Numero += Formats.ToInteger(dbRis("Num"))
                    info.Valore += Formats.ToValuta(dbRis("ML"))
                End If
                dbRis.Dispose()
                'Next

            Next
            Debug.Print("Pratiche inserite: " & (Timer - t))
        End Sub

        Function GetArrayOperatori() As Integer()
            Me.Validate()
            Return Me.m_ArrOperatori
        End Function

        Function GetArrayConsulenti() As Integer()
            Me.Validate()
            Return Me.m_ArrConsulenti
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "PO" : Me.m_IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OP" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DI" : Me.m_FromDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TD" : Me.m_ToDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OV" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "CH" : Me.m_Changed = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IR" : Me.m_IgnoreRights = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "PraticheCDN" : Me.m_PraticheCDN = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PraticheCDM" : Me.m_PraticheCDM = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "PersoneContattate" : Me.m_PersoneContattate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DurataConsulenze" : Me.m_DurataConsulenze = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ConteggioConsulenzeVisiteEsterne" : Me.m_ConteggioConsulenzeVisiteEsterne = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConteggioConsulenzeVisiteUfficio" : Me.m_ConteggioConsulenzeVisiteUfficio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConteggioConsulenzeTelefonateRicevute" : Me.m_ConteggioConsulenzeTelefonateRicevute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConteggioConsulenzeTelefonateEffettuate" : Me.m_ConteggioConsulenzeTelefonateEffettuate = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "OutCalls" : Me.m_OutCalls = XML.Utils.Serializer.ToObject(fieldValue)
                Case "InCalls" : Me.m_InCalls = XML.Utils.Serializer.ToObject(fieldValue)
                Case "OutDates" : Me.m_OutDates = XML.Utils.Serializer.ToObject(fieldValue)
                Case "InDates" : Me.m_InDates = XML.Utils.Serializer.ToObject(fieldValue)
                Case "VisiteDistinteRicevute" : Me.m_VisiteDistinteRicevute = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Consulenze" : Me.m_Consulenze = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Richieste" : Me.m_Richieste = XML.Utils.Serializer.ToObject(fieldValue)
                Case "PraticheConsulente" : Me.m_PraticheConsulente = XML.Utils.Serializer.ToObject(fieldValue)
                Case "PraticheConcluseConsulente" : Me.m_PraticheConcluseConsulente = XML.Utils.Serializer.ToObject(fieldValue)
                Case "PraticheAnnullateConsulente" : Me.m_PraticheAnnullateConsulente = XML.Utils.Serializer.ToObject(fieldValue)
                Case "PraticheConcluseConsulenza" : Me.m_PraticheConcluseConsulenza = XML.Utils.Serializer.ToObject(fieldValue)
                Case "PraticheAnnullateConsulenza" : Me.m_PraticheAnnullateConsulenza = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Infos" : Me.m_Infos = XML.Utils.Serializer.ToObject(fieldValue)
                Case "ArrOperatori" : Me.m_ArrOperatori = XML.Utils.Serializer.ToObject(fieldValue)
                Case "ArrConsulenti" : Me.m_ArrConsulenti = XML.Utils.Serializer.ToObject(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("PO", Me.IDPuntoOperativo)
            writer.WriteAttribute("OP", Me.IDOperatore)
            writer.WriteAttribute("DI", Me.m_FromDate)
            writer.WriteAttribute("TD", Me.m_ToDate)
            writer.WriteAttribute("OV", Me.m_OnlyValid)
            writer.WriteAttribute("CH", Me.m_Changed)
            writer.WriteAttribute("IR", Me.m_IgnoreRights)
            writer.WriteAttribute("PraticheCDN", Me.m_PraticheCDN)
            writer.WriteAttribute("PraticheCDM", Me.m_PraticheCDM)
            writer.WriteAttribute("PersoneContattate", Me.m_PersoneContattate)
            writer.WriteAttribute("DurataConsulenze", Me.m_DurataConsulenze)
            writer.WriteAttribute("ConteggioConsulenzeVisiteEsterne", Me.m_ConteggioConsulenzeVisiteEsterne)
            writer.WriteAttribute("ConteggioConsulenzeVisiteUfficio", Me.m_ConteggioConsulenzeVisiteUfficio)
            writer.WriteAttribute("ConteggioConsulenzeTelefonateRicevute", Me.m_ConteggioConsulenzeTelefonateRicevute)
            writer.WriteAttribute("ConteggioConsulenzeTelefonateEffettuate", Me.m_ConteggioConsulenzeTelefonateEffettuate)

            writer.WriteTag("OutCalls", Me.m_OutCalls)
            writer.WriteTag("InCalls", Me.m_InCalls)
            writer.WriteTag("OutDates", Me.m_OutDates)
            writer.WriteTag("InDates", Me.m_InDates)
            writer.WriteTag("VisiteDistinteRicevute", Me.m_VisiteDistinteRicevute)
            writer.WriteTag("Consulenze", Me.m_Consulenze)
            writer.WriteTag("Richieste", Me.m_Richieste)
            writer.WriteTag("PraticheConsulente", Me.m_PraticheConsulente)
            writer.WriteTag("PraticheConcluseConsulente", Me.m_PraticheConcluseConsulente)
            writer.WriteTag("PraticheAnnullateConsulente", Me.m_PraticheAnnullateConsulente)
            writer.WriteTag("PraticheConcluseConsulenza", Me.m_PraticheConcluseConsulenza)
            writer.WriteTag("PraticheAnnullateConsulenza", Me.m_PraticheAnnullateConsulenza)
            writer.WriteTag("Infos", Me.m_Infos)
            writer.WriteTag("ArrOperatori", Me.m_ArrOperatori)
            writer.WriteTag("ArrConsulenti", Me.m_ArrConsulenti)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class