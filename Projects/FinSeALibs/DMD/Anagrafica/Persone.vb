Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals.CIndexingService
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public NotInheritable Class CPersoneClass
        Inherits CGeneralClass(Of CPersona)

        ''' <summary>
        ''' Numero massimo di parole oltre il quale non viene utilizzato l'algoritmo "intelligente" per la ricerca dei nomi 
        ''' </summary>
        ''' <remarks></remarks>
        Const MAXWORDSINTELLISEARCH As Integer = 10

        Private ReadOnly m_FindHandlersLock As New Object
        Private ReadOnly m_FindHandlers As CKeyCollection(Of FindPersonaHandler)

        Friend Sub New()
            MyBase.New("Persone", GetType(CPersonaCursor))
            Me.m_FindHandlers = New CKeyCollection(Of FindPersonaHandler)
        End Sub

        Public Function GetInstalledFindHandlers() As CCollection(Of FindPersonaHandler)
            SyncLock Me.m_FindHandlersLock
                Return New CCollection(Of FindPersonaHandler)(Me.m_FindHandlers)
            End SyncLock
        End Function

        Public Function GetFindHandler(ByVal command As String) As FindPersonaHandler
            SyncLock Me.m_FindHandlersLock
                command = LCase(Strings.Trim(command))
                Return Me.m_FindHandlers.GetItemByKey(command)
            End SyncLock
        End Function

        Public Sub RemoveFindHandler(ByVal h As FindPersonaHandler)
            SyncLock Me.m_FindHandlersLock
                Me.m_FindHandlers.Remove(h)
            End SyncLock
        End Sub

        Public Sub InstallFindHandler(ByVal command As String, ByVal h As FindPersonaHandler)
            SyncLock Me.m_FindHandlersLock
                command = LCase(Strings.Trim(command))
                Me.m_FindHandlers.Add(command, h)
                Me.m_FindHandlers.Sort()
            End SyncLock
        End Sub


        Public Overrides Function GetItemById(id As Integer) As CPersona
            Dim conn As CDBConnection = Me.GetConnection
            If (conn.IsRemote) Then
                Return MyBase.GetItemById(id)
            Else
                Dim dbRis As System.Data.IDataReader = Nothing
                Try
                    If (id = 0) Then Return Nothing
                    Dim dbSQL As String = "SELECT * FROM [tbl_Persone] WHERE [ID]=" & id
                    Dim ret As CPersona = Nothing
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    If (dbRis.Read) Then
                        ret = Instantiate(Formats.ToInteger(dbRis("TipoPersona")))
                        APPConn.Load(ret, dbRis)
                    End If
                    Return ret
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            End If
        End Function

        Public Function InvertCodiceFiscale(ByVal cf As String) As CPersonaFisica
            Dim ret As New CPersonaFisica
            Dim mesi As String = "ABCDEHLMPRST"
            cf = Formats.ParseCodiceFiscale(cf)
            If (cf = vbNullString) Then Return Nothing
            ret.NatoA.NomeComune = Comuni.GetNomeComuneByCatasto(Strings.Mid(cf, 12, 4))
            ret.CodiceFiscale = cf
            Dim gg As Integer = Formats.ToInteger(Strings.Mid(cf, 10, 2))
            Dim mm As Integer = 1 + mesi.IndexOf(Strings.Mid(cf, 9, 1))
            Dim aa As Integer = 1900 + Formats.ToInteger(Strings.Mid(cf, 7, 2))
            If (gg > 40) Then
                ret.Sesso = "F"
                gg -= 40
            Else
                ret.Sesso = "M"
            End If
            Try
                ret.DataNascita = Calendar.MakeDate(aa, mm, gg)
            Catch ex As Exception
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' Data una stringa contenente il cognome ed il nome di una persona, tenta di separare i due componenti
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="cognome"></param>
        ''' <param name="nome"></param>
        ''' <remarks></remarks>
        Public Sub SplitCognomeNome(ByVal value As String, ByRef cognome As String, ByRef nome As String)
            Dim p As Integer
            value = Replace(Trim(value), "  ", " ")
            p = InStrRev(value, " ")
            If (p > 1) Then
                cognome = Left(value, p - 1)
                nome = Mid(value, p + 1)
            Else
                cognome = value
                nome = vbNullString
            End If
        End Sub

#Region "Find"

        Private Sub AddPersonInfo(ByVal col As CCollection(Of CPersonaInfo), ByVal dbRis As System.Data.IDataReader)
            Dim info As New CPersonaInfo ' Persone.Instantiate( dbRis("TipoPersona")  )
            info.IDPersona = dbRis("ID")
            info.NomePersona = Strings.JoinW(Trim(Strings.ToNameCase(Formats.ToString(dbRis("Nome")))), " ", Strings.UCase(Formats.ToString(dbRis("Cognome"))))
            info.Notes = ""
            If Not info.Persona Is Nothing Then
                With info.Persona
                    info.Deceduto = .Deceduto
                    info.IconURL = .IconURL
                    If (.NatoA.NomeComune <> "") Then info.Notes = Strings.Combine(info.Notes, Strings.JoinW("Nato a: ", .NatoA.NomeComune), ", ")
                    If Not Types.IsNull(.DataNascita) Then info.Notes = Strings.Combine(info.Notes, Strings.JoinW("Nato il: ", Formats.FormatUserDate(.DataNascita)), ", ")
                    If (.CodiceFiscale <> "") Then info.Notes = Strings.Combine(info.Notes, Strings.JoinW("C.F: ", Formats.FormatCodiceFiscale(.CodiceFiscale)), ", ")
                    If (.DomiciliatoA.ToString <> "") Then info.Notes = Strings.Combine(info.Notes, Strings.JoinW("Indirizzo: ", .DomiciliatoA.ToString), ", ")
                End With
            End If
            col.Add(info)
        End Sub

        Private Function AddPersonInfo(ByVal col As CCollection(Of CPersonaInfo), ByVal p As CPersona) As CPersonaInfo
            Dim info As New CPersonaInfo(p) ' Persone.Instantiate( dbRis("TipoPersona")  )
            col.Add(info)
            Return info
        End Function

        Private Sub Find_ByAzienda(
                                  ByVal nomeAzienda As String,
                                  ByVal col As CCollection(Of CPersonaInfo),
                                  ByVal filter As CRMFinlterI,
                                  ByVal findIn As Integer()
                                  )
            Dim cursor As CPersonaFisicaCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                nomeAzienda = Trim(nomeAzienda)
                If nomeAzienda = "" Then Exit Sub

                Dim items As CCollection(Of CResult) = Sistema.IndexingService.Find(nomeAzienda, Nothing) ' filter.nMax)
                If (items.Count = 0) Then Exit Sub

                cursor = New CPersonaFisicaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
                If (filter.DettaglioEsito <> vbNullString) Then cursor.DettaglioEsito.Value = filter.DettaglioEsito
                Dim tmp As New System.Collections.ArrayList
                For Each res As CResult In items
                    tmp.Add(res.OwnerID)
                Next
                Dim arr() As Integer = tmp.ToArray(GetType(Integer))
                cursor.Impiego_IDAzienda.ValueIn(arr)
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    Me.AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Sub

        Private Sub Find_ByEnte(
                               ByVal nomeAzienda As String,
                               ByVal col As CCollection(Of CPersonaInfo),
                               ByVal filter As CRMFinlterI,
                               ByVal findIn() As Integer
                               )
            Dim cursor As CPersonaFisicaCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                nomeAzienda = Trim(nomeAzienda)
                If Len(nomeAzienda) < 3 Then Exit Sub

                cursor = New CPersonaFisicaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Impiego_NomeEntePagante.Value = Strings.JoinW(nomeAzienda, "%")
                cursor.Impiego_NomeEntePagante.Operator = OP.OP_LIKE
                cursor.IgnoreRights = filter.ignoreRights
                cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.DettaglioEsito <> vbNullString) Then cursor.DettaglioEsito.Value = filter.DettaglioEsito
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo

                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Sub

        Private Sub Find_ByCategoria(
                                    ByVal categoriaAzienda As String,
                                    ByVal col As CCollection(Of CPersonaInfo),
                                    ByVal filter As CRMFinlterI,
                                    ByVal findIn As Integer()
                                    )
            Dim cursor As CPersonaFisicaCursor = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                categoriaAzienda = Trim(categoriaAzienda)
                If (categoriaAzienda = "") Then Exit Sub

                cursor = New CPersonaFisicaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Impiego_CategoriaAzienda.Value = categoriaAzienda
                cursor.Impiego_CategoriaAzienda.Operator = OP.OP_EQ
                cursor.IgnoreRights = filter.ignoreRights
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)

                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByTipologia(
                                    ByVal tipologiaAzienda As String,
                                    ByVal col As CCollection(Of CPersonaInfo),
                                    ByVal filter As CRMFinlterI,
                                    ByVal findIn() As Integer
                                    )
            Dim cursor As CPersonaFisicaCursor = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                tipologiaAzienda = Trim(tipologiaAzienda)
                If (tipologiaAzienda = "") Then Exit Sub

                cursor = New CPersonaFisicaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Impiego_TipologiaAzienda.Value = tipologiaAzienda
                cursor.Impiego_TipologiaAzienda.Operator = OP.OP_EQ
                cursor.IgnoreRights = filter.ignoreRights
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch Ex As Exception
                Sistema.Events.NotifyUnhandledException(Ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByIndirizzo(
                                    ByVal indirizzo As String,
                                    ByVal col As CCollection(Of CPersonaInfo),
                                    ByVal filter As CRMFinlterI,
                                    ByVal findIn() As Integer
                                    )
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                indirizzo = Trim(indirizzo)
                If (indirizzo = "") Then Return

                Dim address As New CIndirizzo(indirizzo)
                If (address.Via = "") AndAlso (address.Citta = "") Then Return

                Dim dbSQL As New System.Text.StringBuilder
                dbSQL.Append("SELECT [tbl_Persone].* FROM [tbl_Persone] ")
                dbSQL.Append("INNER JOIN (")
                dbSQL.Append("SELECT [Persona] FROM [tbl_Indirizzi] WHERE ")
                If (Len(address.Via) > 3) Then
                    dbSQL.Append("[Via] Like ")
                    dbSQL.Append(DBUtils.DBString(Strings.JoinW(address.ToponimoEVia, "%")))
                Else
                    dbSQL.Append("[Via] = ")
                    dbSQL.Append(DBUtils.DBString(address.ToponimoEVia))
                End If
                If (Len(address.Citta) > 3) Then
                    dbSQL.Append("And [Citta] ALike ")
                    dbSQL.Append(DBUtils.DBString(Strings.JoinW(address.Citta, "%")))
                ElseIf (Len(address.Citta) > 0) Then
                    dbSQL.Append("And [Citta] = ")
                    dbSQL.Append(DBUtils.DBString(address.Citta))
                End If
                If (Len(address.Provincia) > 3) Then
                    dbSQL.Append("And [Provincia] ALike ")
                    dbSQL.Append(DBUtils.DBString(Strings.JoinW(address.Via, "%")))
                ElseIf (Len(address.Provincia) > 0) Then
                    dbSQL.Append("And [Provincia] = ")
                    dbSQL.Append(DBUtils.DBString(address.Provincia))
                End If
                If (Len(address.Civico) > 0) Then
                    dbSQL.Append("And [Civico] = ")
                    dbSQL.Append(DBUtils.DBString(address.Civico))
                End If
                If (Len(address.CAP) > 0) Then
                    dbSQL.Append("And [CAP] = ")
                    dbSQL.Append(DBUtils.DBString(address.CAP))
                End If

                dbSQL.Append(") AS [T1] ON [tbl_Persone].[ID]=[T1].[Persona] WHERE [tbl_Persone].[Stato] = ")
                dbSQL.Append(ObjectStatus.OBJECT_VALID)

                If (findIn IsNot Nothing) Then
                    dbSQL.Append(" AND [tbl_Persone].[ID] In (")
                    dbSQL.Append(Me.JoinID(findIn, ","))
                    dbSQL.Append(") ")
                End If

                If (filter.tipoPersona.HasValue) Then
                    dbSQL.Append(" AND [TipoPersona]=")
                    dbSQL.Append(DBUtils.DBNumber(filter.tipoPersona.Value))
                End If

                If (filter.IDPuntoOperativo <> 0) Then
                    dbSQL.Append(" AND [IDPuntoOperativo]=")
                    dbSQL.Append(filter.IDPuntoOperativo)
                End If

                If (filter.flags.HasValue) Then
                    dbSQL.Append(" AND (([PFlags] AND ")
                    dbSQL.Append(filter.flags.Value)
                    dbSQL.Append(") <>0)")
                End If

                If (Not filter.ignoreRights AndAlso Not Anagrafica.Persone.Module.UserCanDoAction("list")) Then
                    dbSQL.Append(" AND [IDPuntoOperativo] In (0")

                    Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                    For i As Integer = 0 To items.Count - 1
                        dbSQL.Append(", ")
                        dbSQL.Append(DBUtils.DBNumber(GetID(items(i))))
                    Next
                    dbSQL.Append(")")
                End If


                dbRis = APPConn.ExecuteReader(dbSQL.ToString)
                Dim cnt As Integer = 0
                While dbRis.Read And (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    Dim tipoPersona As TipoPersona = Formats.ToInteger(dbRis("TipoPersona"))
                    Dim p As CPersona = Anagrafica.Persone.Instantiate(tipoPersona)
                    APPConn.Load(p, dbRis)
                    AddPersonInfo(col, p)
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

        End Sub

        Private Sub Find_ByID(
                              ByVal param As String,
                              ByVal col As CCollection(Of CPersonaInfo),
                              ByVal filter As CRMFinlterI
                              )
            Dim id As Integer = Formats.ToInteger(param)
            Dim item As CPersona = Anagrafica.Persone.GetItemById(id)
            If (item IsNot Nothing) Then
                AddPersonInfo(col, item)
            End If
        End Sub



        Private Sub Find_ByComune(
                                 ByVal indirizzo As String,
                                 ByVal col As CCollection(Of CPersonaInfo),
                                 ByVal filter As CRMFinlterI,
                                 ByVal findIn() As Integer
                                 )
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                Dim nomeComune As String = Luoghi.GetComune(indirizzo)
                Dim nomeProvincia As String = Luoghi.GetProvincia(indirizzo)

                If (nomeComune = "") Then Return

                Dim dbSQL As New System.Text.StringBuilder
                dbSQL.Append("SELECT [tbl_Persone].* FROM [tbl_Persone] ")
                dbSQL.Append("INNER JOIN (")
                dbSQL.Append("SELECT [Persona] FROM [tbl_Indirizzi] WHERE ")
                If (nomeProvincia = "") Then
                    If (Len(nomeComune) > 3) Then
                        dbSQL.Append(" [Citta] ALike ")
                        dbSQL.Append(DBUtils.DBString(Strings.JoinW(nomeComune, "%")))
                    Else
                        dbSQL.Append(" [Citta] = ")
                        dbSQL.Append(DBUtils.DBString(nomeComune))
                    End If
                Else
                    dbSQL.Append(" [Citta] = ")
                    dbSQL.Append(DBUtils.DBString(nomeComune))
                    If (Len(nomeProvincia) > 2) Then
                        dbSQL.Append(" And [Provincia] ALike ")
                        dbSQL.Append(DBUtils.DBString(Strings.JoinW(nomeProvincia, "%")))
                    Else
                        dbSQL.Append(" And [Provincia] = ")
                        dbSQL.Append(DBUtils.DBString(nomeProvincia))
                    End If
                End If
                dbSQL.Append(") AS [T1] ON [tbl_Persone].[ID] = [T1].[Persona] WHERE [tbl_Persone].[Stato]=")
                dbSQL.Append(ObjectStatus.OBJECT_VALID)

                If (Arrays.Len(findIn) > 0) Then
                    dbSQL.Append(" AND [tbl_Persone].[ID] In (")
                    dbSQL.Append(Me.JoinID(findIn, ","))
                    dbSQL.Append(") ")
                End If

                If (filter.tipoPersona.HasValue) Then
                    dbSQL.Append(" AND ([TipoPersona]=")
                    dbSQL.Append(DBUtils.DBNumber(filter.tipoPersona.Value))
                    dbSQL.Append(")")
                End If

                If (filter.IDPuntoOperativo <> 0) Then
                    dbSQL.Append(" AND [IDPuntoOperativo]=")
                    dbSQL.Append(filter.IDPuntoOperativo)
                End If

                If (filter.flags.HasValue) Then
                    dbSQL.Append(" AND (([PFlags] AND ")
                    dbSQL.Append(filter.flags.Value)
                    dbSQL.Append(") <>0)")
                End If
                If (Not filter.ignoreRights AndAlso Not Anagrafica.Persone.Module.UserCanDoAction("list")) Then
                    dbSQL.Append(" AND [IDPuntoOperativo] In (0")

                    Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                    For i As Integer = 0 To items.Count - 1
                        dbSQL.Append(", ")
                        dbSQL.Append(DBUtils.DBNumber(GetID(items(i))))
                    Next
                    dbSQL.Append(")")
                End If

                dbRis = APPConn.ExecuteReader(dbSQL.ToString)
                Dim cnt As Integer = 0
                While dbRis.Read And (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    Dim tipoPersona As TipoPersona = Formats.ToInteger(dbRis("TipoPersona"))
                    Dim p As CPersona = Anagrafica.Persone.Instantiate(tipoPersona)
                    APPConn.Load(p, dbRis)
                    AddPersonInfo(col, p)
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Private Sub Find_ByProvincia(
                                    ByVal nomeProvincia As String,
                                    ByVal col As CCollection(Of CPersonaInfo),
                                    ByVal filter As CRMFinlterI,
                                    ByVal findIn() As Integer
                                    )
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")
                If (Arrays.Len(findIn) > 10000) Then Throw New OutOfMemoryException("Query troppo complessa")

                nomeProvincia = Trim(nomeProvincia)
                If (nomeProvincia = "") Then Return

                Dim dbSQL As New System.Text.StringBuilder
                dbSQL.Append("SELECT [tbl_Persone].* FROM [tbl_Persone] ")
                dbSQL.Append("INNER JOIN (")
                dbSQL.Append("SELECT [Persona] FROM [tbl_Indirizzi] WHERE ")
                If (Len(nomeProvincia) > 3) Then
                    dbSQL.Append("[Provincia] ALike ")
                    dbSQL.Append(DBUtils.DBString(Strings.JoinW(nomeProvincia, "%")))
                Else
                    dbSQL.Append("[Provincia] = ")
                    dbSQL.Append(DBUtils.DBString(nomeProvincia))
                End If
                dbSQL.Append(") AS [T1] ON [tbl_Persone].[ID]=[T1].[Persona] WHERE [tbl_Persone].[Stato] = ")
                dbSQL.Append(ObjectStatus.OBJECT_VALID)

                If (filter.tipoPersona.HasValue) Then
                    dbSQL.Append(" AND [TipoPersona]=")
                    dbSQL.Append(DBUtils.DBNumber(filter.tipoPersona.Value))
                End If

                If (filter.IDPuntoOperativo <> 0) Then
                    dbSQL.Append(" AND [IDPuntoOperativo]=")
                    dbSQL.Append(filter.IDPuntoOperativo)
                End If

                If (filter.DettaglioEsito <> "") Then
                    dbSQL.Append(" AND [DettaglioEsito]=")
                    dbSQL.Append(DBUtils.DBString(filter.DettaglioEsito))
                End If

                If (filter.flags.HasValue) Then
                    dbSQL.Append(" AND (([PFlags] AND ")
                    dbSQL.Append(filter.flags.Value)
                    dbSQL.Append(") <>0)")
                End If

                If (findIn IsNot Nothing) Then
                    dbSQL.Append(" AND [tbl_Persone].[ID] In (")
                    dbSQL.Append(Me.JoinID(findIn, ","))
                    dbSQL.Append(")")
                End If

                If (Not filter.ignoreRights AndAlso Not Anagrafica.Persone.Module.UserCanDoAction("list")) Then
                    dbSQL.Append(" AND [IDPuntoOperativo] In (0")

                    Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                    For i As Integer = 0 To items.Count - 1
                        dbSQL.Append(", ")
                        dbSQL.Append(DBUtils.DBNumber(GetID(items(i))))
                    Next

                    dbSQL.Append(")")
                End If


                dbRis = APPConn.ExecuteReader(dbSQL.ToString)
                Dim cnt As Integer = 0
                While dbRis.Read And (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    Dim tipoPersona As TipoPersona = Formats.ToInteger(dbRis("TipoPersona"))
                    Dim p As CPersona = Anagrafica.Persone.Instantiate(tipoPersona)
                    APPConn.Load(p, dbRis)
                    AddPersonInfo(col, p)
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Private Sub Find_ByCFOrPIVA(
                                   ByVal value As String,
                                   ByVal col As CCollection(Of CPersonaInfo),
                                   ByVal filter As CRMFinlterI,
                                   ByVal findIn As Integer()
                                   )
            Dim cursor As CPersonaCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")


                Dim cf As String = Formats.ParseCodiceFiscale(value)
                Dim piva As String = Formats.ParsePartitaIVA(value)

                cursor = New CPersonaCursor
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.DettaglioEsito <> vbNullString) Then cursor.DettaglioEsito.Value = filter.DettaglioEsito

                Dim cnt As Integer = 0
                If Len(cf) >= 6 Then
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.CodiceFiscale.Value = Strings.JoinW(cf, "%")
                    cursor.CodiceFiscale.Operator = OP.OP_LIKE

                    While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                        AddPersonInfo(col, cursor.Item)
                        cursor.MoveNext()
                        cnt += 1
                    End While
                    cursor.Reset1()
                End If

                If (cnt = 0) AndAlso (Len(piva) >= 11) Then
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.CodiceFiscale.Clear()
                    cursor.WhereClauses.Add(Strings.JoinW("([PartitaIVA] Like '", Strings.Replace(piva, "'", "''"), "%') Or ([CodiceFiscale] Like '", Strings.Replace(piva, "'", "''"), "%')"))
                    While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                        AddPersonInfo(col, cursor.Item)
                        cursor.MoveNext()
                        cnt += 1
                    End While
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Sub

        Private Sub Find_ByPIVA(
                                   ByVal value As String,
                                   ByVal col As CCollection(Of CPersonaInfo),
                                   ByVal filter As CRMFinlterI,
                                   ByVal findIn() As Integer
                                   )
            Dim cursor As CPersonaCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                Dim piva As String = Formats.ParsePartitaIVA(value)
                If (Len(piva) <= 6) Then Return

                cursor = New CPersonaCursor
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito

                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.CodiceFiscale.Clear()
                cursor.PartitaIVA.Value = Strings.JoinW(piva, "%")
                cursor.PartitaIVA.Operator = OP.OP_LIKE

                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Sub

        Public Function FindByTelefono(ByVal value As String, ByVal filter As CRMFinlterI) As CCollection(Of CPersonaInfo)
            Dim ret As New CCollection(Of CPersonaInfo)
            Find_ByTelefono(value, ret, filter, Nothing)
            Return ret
        End Function

        Public Function FindByEMail(ByVal value As String, ByVal filter As CRMFinlterI) As CCollection(Of CPersonaInfo)
            Dim ret As New CCollection(Of CPersonaInfo)
            Find_ByEMail(value, ret, filter, Nothing)
            Return ret
        End Function


        Private Sub Find_ByTelefono(
                                   ByVal value As String,
                                   ByVal col As CCollection(Of CPersonaInfo),
                                   ByVal filter As CRMFinlterI,
                                   ByVal findIn As Integer()
                                   )
            Dim cursor As CPersonaCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                Dim num As String = Formats.ParsePhoneNumber(value)
                If Len(num) < 3 Then Exit Sub

                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito

                cursor.Telefono.Value = Strings.JoinW(num, "%")
                cursor.Telefono.Operator = OP.OP_LIKE
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub


        Private Sub Find_ByEMail(
                                ByVal value As String,
                                ByVal col As CCollection(Of CPersonaInfo),
                                ByVal filter As CRMFinlterI,
                                ByVal findIn As Integer()
                                )
            Dim cursor As CPersonaCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                Dim param As String = Replace(Formats.ParseEMailAddress(value), "'", "''")
                If Len(param) < 3 Then Exit Sub

                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito
                cursor.eMail.Value = Strings.JoinW(param, "%")
                cursor.eMail.Operator = OP.OP_LIKE
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If

                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByWebSite(
                                  ByVal value As String,
                                  ByVal col As CCollection(Of CPersonaInfo),
                                  ByVal filter As CRMFinlterI,
                                  ByVal findIn() As Integer
                                  )
            Dim cursor As CPersonaCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                Dim param As String = Formats.ParseWebAddress(value)
                If Len(param) < 3 Then Exit Sub

                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito

                cursor.WebSite.Value = Strings.JoinW(param, "%")
                cursor.WebSite.Operator = OP.OP_LIKE
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByDataRicontatto(
                                         ByVal value As String,
                                         ByVal col As CCollection(Of CPersonaInfo),
                                         ByVal filter As CRMFinlterI,
                                         ByVal findIn() As Integer
                                         )
            Dim cursor As CPersonaCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")


                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito

                Dim wherePart As String = cursor.GetWherePart
                Dim wherePartLimit As String = cursor.GetWherePartLimit
                wherePart = Strings.Combine(wherePart, wherePartLimit, " AND ")

                Dim dbSQL As New System.Text.StringBuilder

                'TO DO , findIn

                value = Replace(Trim(value), "  ", " ")
                Select Case value
                    Case "nessuna", "non impostata", "nessuo", "null"
                        'Cerco tutte le anagrafiche senza data di ricontatto
                        dbSQL.Append("SELECT * FROM (SELECT [T1].* FROM [tbl_Persone] AS [T1] LEFT JOIN (SELECT * FROM [tbl_Ricontatti] WHERE [tbl_Ricontatti].[Stato]=")
                        dbSQL.Append(ObjectStatus.OBJECT_VALID)
                        dbSQL.Append(" AND [StatoRicontatto]=")
                        dbSQL.Append(StatoRicontatto.PROGRAMMATO)
                        dbSQL.Append(" AND ([NomeLista]='' OR [NomeLista] Is Null)) AS [T2] ON [T1].[ID] = [T2].[IDPersona] WHERE [T1].[TipoPersona]=")
                        dbSQL.Append(TipoPersona.PERSONA_FISICA)
                        dbSQL.Append(" AND [T2].[ID] Is Null) WHERE ")
                        dbSQL.Append(wherePart)
                        dbSQL.Append(" ORDER BY [Nome] & ' ' & [Cognome] ASC")
                    Case ""
                        '?
                    Case Else
                        Dim nibbles() As String
                        Dim da As Nullable(Of Date) = Nothing
                        Dim a As Nullable(Of Date) = Nothing

                        If (LCase(Left(value, 3)) = "tra") Then
                            nibbles = Split(Mid(value, 4), " e ")
                            If (nibbles.Length = 2) Then
                                da = Formats.ToDate(nibbles(0))
                                a = Formats.ToDate(nibbles(1))
                            Else
                                Throw New FormatException("Il formato deve essere del tipo 'tra 31/12/2000 e 01/01/2001'")
                            End If
                        ElseIf (LCase(Left(value, "7")) = "dopo il") Then
                            da = Formats.ToDate(Mid(value, 8))
                        ElseIf (LCase(Left(value, "9")) = "prima del") Then
                            a = Formats.ToDate(Mid(value, 10))
                        Else
                            Dim interval As CIntervalloData = Calendar.PeriodoToDates(value, da, a)
                            da = interval.Inizio
                            a = interval.Fine
                            If (da.HasValue AndAlso a.HasValue AndAlso da.Value = a.Value) Then a = Calendar.DateAdd(DateInterval.Second, 24 * 3600 - 1, da.Value)
                        End If

                        If (da.HasValue) Then
                            If (a.HasValue) Then
                                dbSQL.Append("SELECT * FROM (SELECT [T1].* FROM [tbl_Persone] AS [T1] INNER JOIN (SELECT * FROM [tbl_Ricontatti] WHERE [tbl_Ricontatti].[Stato]=")
                                dbSQL.Append(ObjectStatus.OBJECT_VALID)
                                dbSQL.Append(" AND [StatoRicontatto]=")
                                dbSQL.Append(StatoRicontatto.PROGRAMMATO)
                                dbSQL.Append(" AND ([NomeLista]='' OR [NomeLista] Is Null) AND [DataPrevista] BETWEEN ")
                                dbSQL.Append(DBUtils.DBDate(da))
                                dbSQL.Append(" AND ")
                                dbSQL.Append(DBUtils.DBDate(a))
                                dbSQL.Append(") AS [T2] ON [T1].[ID] = [T2].[IDPersona] WHERE [T1].[TipoPersona]=")
                                dbSQL.Append(TipoPersona.PERSONA_FISICA)
                                dbSQL.Append(") WHERE ")
                                dbSQL.Append(wherePart)
                                dbSQL.Append(" ORDER BY [Nome] & ' ' & [Cognome] ASC")
                            Else
                                dbSQL.Append("SELECT * FROM (SELECT [T1].* FROM [tbl_Persone] AS [T1] INNER JOIN (SELECT * FROM [tbl_Ricontatti] WHERE [tbl_Ricontatti].[Stato]=")
                                dbSQL.Append(ObjectStatus.OBJECT_VALID)
                                dbSQL.Append(" AND [StatoRicontatto]=")
                                dbSQL.Append(StatoRicontatto.PROGRAMMATO)
                                dbSQL.Append(" AND ([NomeLista]='' OR [NomeLista] Is Null) AND [DataPrevista] >= ")
                                dbSQL.Append(DBUtils.DBDate(da))
                                dbSQL.Append(") AS [T2] ON [T1].[ID] = [T2].[IDPersona] WHERE [T1].[TipoPersona]=")
                                dbSQL.Append(TipoPersona.PERSONA_FISICA)
                                dbSQL.Append(") WHERE ")
                                dbSQL.Append(wherePart)
                                dbSQL.Append(" ORDER BY [Nome] & ' ' & [Cognome] ASC")
                            End If
                        Else
                            If (a.HasValue) Then
                                dbSQL.Append("SELECT * FROM (SELECT [T1].* FROM [tbl_Persone] AS [T1] INNER JOIN (SELECT * FROM [tbl_Ricontatti] WHERE [tbl_Ricontatti].[Stato]=")
                                dbSQL.Append(ObjectStatus.OBJECT_VALID)
                                dbSQL.Append(" AND [StatoRicontatto]=")
                                dbSQL.Append(StatoRicontatto.PROGRAMMATO)
                                dbSQL.Append(" AND ([NomeLista]='' OR [NomeLista] Is Null) AND [DataPrevista] <= ")
                                dbSQL.Append(DBUtils.DBDate(a))
                                dbSQL.Append(") AS [T2] ON [T1].[ID] = [T2].[IDPersona] WHERE [T1].[TipoPersona]=")
                                dbSQL.Append(TipoPersona.PERSONA_FISICA)
                                dbSQL.Append(") WHERE ")
                                dbSQL.Append(wherePart)
                                dbSQL.Append(" ORDER BY [Nome] & ' ' & [Cognome] ASC")
                            Else
                                dbSQL.Clear()
                            End If
                        End If
                End Select

                If (dbSQL.Length > 0) Then
                    Dim cnt As Integer = 0
                    dbRis = APPConn.ExecuteReader(dbSQL.ToString)
                    While (dbRis.Read) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                        Dim item As New CPersonaFisica
                        APPConn.Load(item, dbRis)
                        If (filter.flags.HasValue = False OrElse TestFlag(item.Flags, filter.flags.Value)) Then AddPersonInfo(col, item)
                        cnt += 1
                    End While
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Public Function SincronizzaPersone(ByVal ricontatti As CCollection(Of CRicontatto)) As CCollection(Of CPersona)
            Dim arr As Integer() = {}
            For Each ric As CRicontatto In ricontatti
                If (ric.IDPersona <> 0 AndAlso Arrays.BinarySearch(Of Integer)(arr, ric.IDPersona) < 0) Then arr = Arrays.InsertSorted(Of Integer)(arr, ric.IDPersona)
            Next
            Dim tmp As New CKeyCollection(Of CPersona)
            If (arr.Length > 0) Then
                Dim cursor As New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ID.ValueIn(arr)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    tmp.Add("K" & GetID(cursor.Item), cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose() : cursor = Nothing

                For Each ric As CRicontatto In ricontatti
                    ric.SetPersona(tmp.GetItemByKey("K" & ric.IDPersona))
                Next
            End If
            Return New CCollection(Of CPersona)(tmp)
        End Function

        Private Sub Find_ByLista(
                                ByVal nomeLista As String,
                                ByVal col As CCollection(Of CPersonaInfo),
                                ByVal filter As CRMFinlterI
                                )
            Dim cursor As ListaRicontattoItemCursor = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                nomeLista = Trim(nomeLista)
                If (nomeLista = "") Then Exit Sub

                cursor = New ListaRicontattoItemCursor
                If (Right(nomeLista, 1) <> "%") Then nomeLista = Strings.JoinW(nomeLista, "%")
                cursor.NomeLista.Value = nomeLista
                cursor.NomeLista.Operator = OP.OP_LIKE
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                cursor.PageSize = 1000
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo

                Dim cnt As Integer = 0
                Dim ricontatti As New CCollection(Of CRicontatto)
                Dim ric As CRicontatto
                While (Not cursor.EOF) 'AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    ric = cursor.Item
                    ricontatti.Add(ric)
                    cursor.MoveNext()
                End While
                cursor.Dispose() : cursor = Nothing

                Me.SincronizzaPersone(ricontatti)
                For Each ric In ricontatti
                    If (ric.Persona IsNot Nothing) Then
                        If (
                           (filter.tipoPersona.HasValue = False OrElse filter.tipoPersona.Value = ric.Persona.TipoPersona) AndAlso
                           (filter.flags.HasValue = False OrElse TestFlag(ric.Persona.Flags, filter.flags.Value)) AndAlso
                           (filter.DettaglioEsito = "" OrElse filter.DettaglioEsito = ric.Persona.DettaglioEsito)
                           ) Then
                            Dim info As CPersonaInfo = AddPersonInfo(col, ric.Persona)
                            cnt += 1
                            If (filter.nMax.HasValue AndAlso cnt >= filter.nMax.Value) Then Exit For
                        End If
                    End If
                Next

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByCompleanno(
                                     ByVal value As String,
                                     ByVal col As CCollection(Of CPersonaInfo),
                                     ByVal filter As CRMFinlterI,
                                     ByVal findIn() As Integer
                                     )
            Dim cursor As CPersonaCursor = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                value = Replace(Trim(value), "  ", "")
                Dim d1, d2 As Date?
                Dim intervallo As CIntervalloData = Calendar.PeriodoToDates(value, d1, d2)
                d1 = intervallo.Inizio
                d2 = intervallo.Fine

                Dim dbSQL As New System.Text.StringBuilder
                Select Case LCase(value)
                    Case "domani"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(Calendar.ToMorrow))
                        dbSQL.Append(" And Day([DataNascita]) = ")
                        dbSQL.Append(Day(Calendar.ToMorrow))
                        dbSQL.Append(")")
                    Case "oggi"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(Calendar.ToDay))
                        dbSQL.Append(" And Day([DataNascita]) = ")
                        dbSQL.Append(Day(Calendar.ToDay))
                        dbSQL.Append(")")
                    Case "ieri"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(Calendar.YesterDay))
                        dbSQL.Append(" And Day([DataNascita]) = ")
                        dbSQL.Append(Day(Calendar.YesterDay))
                        dbSQL.Append(")")
                    Case "questa settimana"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(d1))
                        dbSQL.Append(" And Day([DataNascita]) >= ")
                        dbSQL.Append(Day(d1))
                        dbSQL.Append(" And Day([DataNascita]) <= ")
                        dbSQL.Append(Day(d2))
                        dbSQL.Append(")")
                    Case "la settimana scorsa", "la scorsa scorsa"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(d1))
                        dbSQL.Append(" And Day([DataNascita]) >= ")
                        dbSQL.Append(Day(d1))
                        dbSQL.Append(") And Day([DataNascita] <= ")
                        dbSQL.Append(Day(d2))
                        dbSQL.Append(")")
                    Case "la settimana prossima", "la prossima settimana"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(d1))
                        dbSQL.Append(" And Day([DataNascita]) >= ")
                        dbSQL.Append(Day(d1))
                        dbSQL.Append(") And Day([DataNascita] <= ")
                        dbSQL.Append(Day(d2))
                        dbSQL.Append(")")
                    Case "questo mese"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(d1))
                        dbSQL.Append(")")
                    Case "il mese scorso", "lo scorso mese"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(d1))
                        dbSQL.Append(")")
                    Case "il mese prossimo", "il prossimo mese"
                        dbSQL.Append("(Month([DataNascita]) = ")
                        dbSQL.Append(Month(d1))
                        dbSQL.Append(")")
                    Case Else
                        Exit Sub
                End Select

                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (dbSQL.Length > 0) Then cursor.WhereClauses.Add(dbSQL.ToString)
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByRelazione(
                                    ByVal tipoRelazione As String,
                                    ByVal value As String,
                                    ByVal col As CCollection(Of CPersonaInfo),
                                    ByVal filter As CRMFinlterI
                                    )
            Dim cursor As CRelazioneParentaleCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                value = Trim(value)
                If (value = "") Then Exit Sub

                Dim tmp As New CKeyCollection(Of CPersona)
                cursor = New CRelazioneParentaleCursor
                cursor.IgnoreRights = filter.ignoreRights
                cursor.NomeRelazione.Value = tipoRelazione
                cursor.NomePersona2.Value = Strings.JoinW("%", value, "%")
                cursor.NomePersona2.Operator = OP.OP_LIKE
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Dim n As Integer = 0
                While (Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse n < filter.nMax.Value))
                    Dim rel As CRelazioneParentale = cursor.Item
                    Dim p As CPersona = rel.Persona1
                    If (p IsNot Nothing AndAlso p.Stato = ObjectStatus.OBJECT_VALID) Then
                        tmp.Add("K" & GetID(p), p)
                    End If
                    cursor.MoveNext()
                    n += 1
                End While
                cursor.Dispose()
                cursor = Nothing

                Dim inverse() As String = Anagrafica.RelazioniParentali.GetInvertedRelations(tipoRelazione)
                For Each str As String In inverse
                    cursor = New CRelazioneParentaleCursor
                    cursor.IgnoreRights = filter.ignoreRights
                    cursor.NomeRelazione.Value = str
                    cursor.NomePersona1.Value = Strings.JoinW("%", value, "%")
                    cursor.NomePersona1.Operator = OP.OP_LIKE
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    While (Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse n < filter.nMax.Value))
                        Dim rel As CRelazioneParentale = cursor.Item
                        Dim p As CPersona = rel.Persona2
                        If (p IsNot Nothing AndAlso p.Stato = ObjectStatus.OBJECT_VALID) Then
                            If (tmp.GetItemByKey("K" & GetID(p)) Is Nothing) Then
                                tmp.Add("K" & GetID(p), p)
                            End If
                        End If
                        cursor.MoveNext()
                        n += 1
                    End While

                Next

                For Each p As CPersona In tmp
                    If (filter.IDPuntoOperativo = 0 OrElse filter.IDPuntoOperativo = p.IDPuntoOperativo) AndAlso
                       (filter.flags.HasValue = False OrElse TestFlag(p.Flags, filter.flags.Value)) AndAlso
                       (filter.DettaglioEsito = "" OrElse filter.DettaglioEsito = p.DettaglioEsito) Then
                        Me.AddPersonInfo(col, p)
                    End If
                Next
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByRelazioneInversa(
                                           ByVal tipoRelazione As String,
                                           ByVal value As String,
                                           ByVal col As CCollection(Of CPersonaInfo),
                                           ByVal filter As CRMFinlterI
                                           )
            Dim cursor As CRelazioneParentaleCursor = Nothing
            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                value = Trim(value)
                If (value = "") Then Exit Sub

                Dim tmp As New CKeyCollection

                cursor = New CRelazioneParentaleCursor
                cursor.IgnoreRights = filter.ignoreRights
                cursor.NomeRelazione.Value = tipoRelazione
                cursor.NomePersona1.Value = Strings.JoinW("%", value, "%")
                cursor.NomePersona1.Operator = OP.OP_LIKE
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Dim n As Integer = 0
                While (Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse n < filter.nMax.Value))
                    Dim rel As CRelazioneParentale = cursor.Item
                    Dim p As CPersona = rel.Persona2
                    If (p IsNot Nothing AndAlso p.Stato = ObjectStatus.OBJECT_VALID) Then
                        tmp.Add("K" & GetID(p), p)
                    End If
                    cursor.MoveNext()
                End While
                cursor.Dispose()
                cursor = Nothing

                Dim inverse() As String = Anagrafica.RelazioniParentali.GetInvertedRelations(tipoRelazione)
                For Each str As String In inverse
                    cursor = New CRelazioneParentaleCursor
                    cursor.IgnoreRights = filter.ignoreRights
                    cursor.NomeRelazione.Value = str
                    cursor.NomePersona2.Value = Strings.JoinW("%", value, "%")
                    cursor.NomePersona2.Operator = OP.OP_LIKE
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    While (Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse n < filter.nMax.Value))
                        Dim rel As CRelazioneParentale = cursor.Item
                        Dim p As CPersona = rel.Persona1
                        If (p IsNot Nothing AndAlso p.Stato = ObjectStatus.OBJECT_VALID) Then
                            If (tmp.GetItemByKey("K" & GetID(p)) Is Nothing) Then
                                tmp.Add("K" & GetID(p), p)
                            End If
                        End If
                        cursor.MoveNext()
                        n += 1
                    End While
                    cursor.Dispose()
                    cursor = Nothing
                Next

                For Each p As CPersona In tmp
                    If (filter.IDPuntoOperativo = 0 OrElse filter.IDPuntoOperativo = p.IDPuntoOperativo) AndAlso
                       (filter.flags.HasValue = False OrElse TestFlag(p.Flags, filter.flags.Value)) AndAlso
                       (filter.DettaglioEsito = "" OrElse filter.DettaglioEsito = p.DettaglioEsito) Then
                        Me.AddPersonInfo(col, p)
                    End If
                Next
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Private Sub Find_ByNomeIntelli(
                               ByVal value As String,
                               ByVal col As CCollection(Of CPersonaInfo),
                               ByVal filter As CRMFinlterI,
                               ByVal findIn As Integer()
                               )
            Const DIMSCAGLIONE As Integer = 5000    'La ricerca nella tabella persone viene scaglionata
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")


                Dim items As CCollection = Sistema.IndexingService.Find(value, findIn, filter.nMax)
                Dim i As Integer = 0

                Dim dbSQL As New System.Text.StringBuilder
                dbSQL.Append("SELECT * FROM [tbl_Persone] WHERE [Stato]=")
                dbSQL.Append(ObjectStatus.OBJECT_VALID)
                If (filter.tipoPersona.HasValue) Then
                    If (filter.tipoPersona.Value = TipoPersona.PERSONA_FISICA) Then
                        dbSQL.Append(" AND [TipoPersona]=")
                        dbSQL.Append(TipoPersona.PERSONA_FISICA)
                    Else
                        dbSQL.Append(" AND [TipoPersona]<>")
                        dbSQL.Append(TipoPersona.PERSONA_FISICA)
                    End If
                End If
                If (filter.DettaglioEsito <> "") Then
                    dbSQL.Append(" AND [DettaglioEsito]=")
                    dbSQL.Append(DBUtils.DBString(filter.DettaglioEsito))
                End If

                If (filter.flags.HasValue) Then
                    dbSQL.Append(" AND (([PFlags] AND ")
                    dbSQL.Append(filter.flags.Value)
                    dbSQL.Append(") <> 0)")
                End If

                If (filter.IDPuntoOperativo <> 0) Then
                    dbSQL.Append(" AND [IDPuntoOperativo]=")
                    dbSQL.Append(filter.IDPuntoOperativo)
                End If

                Dim tmpSQL As New System.Text.StringBuilder
                If Not Anagrafica.Persone.Module.UserCanDoAction("list") Then
                    If Anagrafica.Persone.Module.UserCanDoAction("list_office") Then
                        tmpSQL.Append("[IDPuntoOperativo] = 0")
                        Dim uffici As CCollection(Of CUfficio) = Users.CurrentUser.Uffici
                        For i = 0 To uffici.Count - 1
                            tmpSQL.Append(" OR ")
                            tmpSQL.Append("[IDPuntoOperativo] = " & DBUtils.DBNumber(GetID(uffici(i))))
                        Next
                    End If

                    If Anagrafica.Persone.Module.UserCanDoAction("list_own") Then
                        tmpSQL.Append(" OR ([CreatoDa]=")
                        tmpSQL.Append(GetID(Users.CurrentUser))
                        tmpSQL.Append(")")
                    End If

                    If tmpSQL.Length = 0 Then tmpSQL.Append("0<>0")
                End If

                If (tmpSQL.Length > 0) Then
                    dbSQL.Append(" AND (")
                    dbSQL.Append(tmpSQL)
                    dbSQL.Append(")")
                End If

                col.Capacity = items.Count
                i = 0
                While (i < items.Count)
                    Dim buffer As New System.Text.StringBuilder
                    Dim j As Integer = 0
                    While (i < items.Count AndAlso j < DIMSCAGLIONE)
                        Dim res As CResult = items(i)
                        If (j > 0) Then buffer.Append(",")
                        buffer.Append(DBUtils.DBNumber(res.OwnerID))
                        j += 1
                        i += 1
                    End While



                    tmpSQL.Clear()
                    tmpSQL.Append(dbSQL)
                    tmpSQL.Append(" AND [ID] In (")
                    tmpSQL.Append(buffer.ToString)
                    tmpSQL.Append(")")

                    dbRis = APPConn.ExecuteReader(tmpSQL.ToString)

                    Dim words() As WordInfo = Sistema.IndexingService.SplitWords(value)
                    While (dbRis.Read AndAlso (filter.nMax.HasValue = False OrElse col.Count < filter.nMax.Value))
                        Dim o As CPersona = Anagrafica.Persone.Instantiate(Formats.ToInteger(dbRis("TipoPersona")))
                        APPConn.Load(o, dbRis)
                        If Me.VerificaNomePersona(words, o) Then AddPersonInfo(col, o)
                    End While

                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Private Function GetWords(ByVal o As CPersona) As CIndexingService.WordInfo()
            Return Sistema.IndexingService.GetIndexableWords(o)
        End Function

        Private Function CloneWords(ByVal words() As CIndexingService.WordInfo) As CIndexingService.WordInfo()
            Dim ret() As CIndexingService.WordInfo
            ReDim ret(UBound(words))
            For i As Integer = 0 To UBound(words)
                ret(i) = words(i).Clone
            Next
            Return ret
        End Function

        Private Function VerificaNomePersona(ByVal wordsToFind() As CIndexingService.WordInfo, ByVal o As Object) As Boolean
            Return True

            'Dim words1() As Sistema.CIndexingService.WordInfo = Me.GetWords(o)
            'Dim words() As Sistema.CIndexingService.WordInfo = Me.CloneWords(wordsToFind)

            'For Each w As Sistema.CIndexingService.WordInfo In words
            '    If w.IsLike Then
            '        For Each w1 As Sistema.CIndexingService.WordInfo In words1
            '            If Strings.Left(w1.Word, Len(w.Word)) = w.Word AndAlso w.Frequenza > 0 AndAlso w1.Frequenza > 0 Then
            '                w.Frequenza -= 1
            '                w1.Frequenza -= 1
            '                Exit For
            '            End If
            '        Next
            '    Else
            '        For Each w1 As Sistema.CIndexingService.WordInfo In words1
            '            If w.Word = w1.Word AndAlso w.Frequenza > 0 AndAlso w1.Frequenza > 0 Then
            '                w.Frequenza -= 1
            '                w1.Frequenza -= 1
            '            End If
            '        Next
            '    End If
            'Next

            'For Each w As Sistema.CIndexingService.WordInfo In words
            '    If w.Frequenza > 0 Then Return False
            'Next
            'Return True

            ' ''Dim w, w1 As Sistema.CIndexingService.WordInfo
            ''If (Arrays.Len(words1) < 0) Then Return False

            ' ''Dim i As Integer = 0
            ' ''Dim j As Integer
            ' ''While (i <= UBound(words))
            ' ''    w = words(i)
            ' ''    If (w.IsLike) Then
            ' ''    Else
            ' ''        j = 0
            ' ''        While (j <= UBound(words1))
            ' ''            w1 = words1(j)
            ' ''            If w1.Word = w.Word Then
            ' ''                If (w1.Frequenza < w.Frequenza) Then
            ' ''                    Return False
            ' ''                Else
            ' ''                    words = Arrays.RemoveAt(words, i)
            ' ''                    words1 = Arrays.RemoveAt(words1, j)
            ' ''                End If
            ' ''            Else
            ' ''                j += 1
            ' ''            End If
            ' ''        End While
            ' ''    End If
            ' ''End While

            ' ''w1 = words1(UBound(words1))
            ' ''Return Strings.Left(w1.Word, Len(w.Word)) = w.Word AndAlso w1.Frequenza >= w.Frequenza

            ''Return True
        End Function

        Private Sub Find_ByNomeStandard(
                              ByVal value As String,
                              ByVal col As CCollection(Of CPersonaInfo),
                              ByVal filter As CRMFinlterI,
                              ByVal findIn() As Integer
                              )
            Dim cursor As CPersonaCursor = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                Dim name As String = Replace(Trim(value), "  ", " ")
                name = Replace(value, "'", "''")
                Dim name1 As String = Strings.OnlyChars(name)

                cursor = New CPersonaCursor
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights

                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If

                If (name <> "") Then
                    Dim dbSQL As New System.Text.StringBuilder
                    dbSQL.Append("(")
                    If (Len(name) < 3) Then
                        dbSQL.Append("([Alias1] = '")
                        dbSQL.Append(name)
                        dbSQL.Append("') Or ")
                        dbSQL.Append("([Alias2] = '")
                        dbSQL.Append(name)
                        dbSQL.Append("') ")
                    Else
                        dbSQL.Append("([Alias1] Like '")
                        dbSQL.Append(name)
                        dbSQL.Append("%') Or ")
                        dbSQL.Append("([Alias2] Like '")
                        dbSQL.Append(name)
                        dbSQL.Append("%') ")
                    End If
                    'If (Len(name1) >= 3) Then
                    '    dbSQL.Append(" Or ([RNome1] Like '")
                    '    dbSQL.Append(name1)
                    '    dbSQL.Append("%') Or ")
                    '    dbSQL.Append("([RNome2] Like '")
                    '    dbSQL.Append(name1)
                    '    dbSQL.Append("%')")
                    'Else
                    '    dbSQL.Append("Or ([RNome1] = '")
                    '    dbSQL.Append(name1)
                    '    dbSQL.Append("') Or ")
                    '    dbSQL.Append("([RNome2] = '")
                    '    dbSQL.Append(name1)
                    '    dbSQL.Append("')")
                    'End If
                    dbSQL.Append(")")
                    cursor.WhereClauses.Add(dbSQL.ToString)
                End If

                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub



        Private Sub Find_ByNome(
                               ByVal value As String,
                               ByVal col As CCollection(Of CPersonaInfo),
                               ByVal filter As CRMFinlterI,
                               ByVal findIn() As Integer
                               )
            value = Strings.Trim(value)
            Dim i As Integer = Strings.ContaRipetizioni(value, " ")
            If (value <> "" AndAlso Sistema.IndexingService.Database IsNot Nothing AndAlso (filter.IntelliSearch OrElse i <= MAXWORDSINTELLISEARCH)) Then
                Me.Find_ByNomeIntelli(value, col, filter, findIn)
            ElseIf (value <> "") Then
                Me.Find_ByNomeStandard(value, col, filter, findIn)
            End If
        End Sub

        Private Sub Find_ByTipoRapporto(
                                       ByVal tipoRapporto As String,
                                       ByVal col As CCollection(Of CPersonaInfo),
                                       ByVal filter As CRMFinlterI,
                                       ByVal findIn As Integer()
                                       )
            Dim cursor As CPersonaFisicaCursor = Nothing

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
                If (col Is Nothing) Then Throw New ArgumentNullException("col")

                Dim tr As CTipoRapporto = Anagrafica.TipiRapporto.GetItemByIdTipoRapporto(tipoRapporto)
                If (tr IsNot Nothing) Then tipoRapporto = tr.IdTipoRapporto
                If (tipoRapporto = "") Then Exit Sub

                cursor = New CPersonaFisicaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Impiego_TipoRapporto.Value = tipoRapporto
                cursor.Impiego_TipoRapporto.Operator = OP.OP_EQ
                cursor.IgnoreRights = filter.ignoreRights
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                If (filter.DettaglioEsito <> "") Then cursor.DettaglioEsito.Value = filter.DettaglioEsito
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Public Function Find(ByVal filters() As CRMFinlterI) As CCollection(Of CPersonaInfo)
            If (filters Is Nothing) Then Throw New ArgumentNullException("filters")

            Dim ret As New CCollection(Of CPersonaInfo)
            Dim findIn As Integer() = Nothing
            Dim nMax As Integer? = Nothing
            For i As Integer = filters.Length - 1 To 0 Step -1
                nMax = Math.Min(nMax, filters(i).nMax)
            Next

            If (nMax.HasValue AndAlso nMax.Value < 1) Then Return ret

            For i As Integer = filters.Length - 1 To 0 Step -1
                Dim filter As CRMFinlterI = filters(i)
                If (filters.Length > 1) Then filter.nMax = Nothing
                ret = Me.Find(filter, findIn)
                If (ret.Count = 0) Then Exit For

                If (filters.Length > 1) Then
                    findIn = Arrays.CreateInstance(Of Integer)(ret.Count)
                    For j As Integer = 0 To ret.Count - 1
                        findIn(j) = ret(j).IDPersona
                    Next
                End If
                'ret.Comparer = New ByIdComparer
            Next

            If (nMax.HasValue AndAlso ret.Count > nMax.Value) Then
                While (ret.Count > nMax.Value)
                    ret.RemoveAt(ret.Count - 1)
                End While
            End If

            Return ret
        End Function


        Public Function FindPF(ByVal filters() As CRMFinlterI) As CCollection(Of CPersonaInfo)
            If (filters Is Nothing) Then Throw New ArgumentNullException("filters")

            Dim ret As New CCollection(Of CPersonaInfo)
            Dim findIn As Integer() = Nothing
            Dim nMax As Integer? = Nothing
            For i As Integer = filters.Length - 1 To 0 Step -1
                nMax = Math.Min(nMax, filters(i).nMax)
            Next

            If (nMax.HasValue AndAlso nMax.Value < 1) Then Return ret

            For i As Integer = filters.Length - 1 To 0 Step -1
                Dim filter As CRMFinlterI = filters(i)
                filter.nMax = Nothing
                ret = Me.FindPF(filter, findIn)
                If (ret.Count = 0) Then Exit For
                If (filters.Length > 1) Then
                    findIn = Arrays.CreateInstance(Of Integer)(ret.Count)
                    For j As Integer = 0 To ret.Count - 1
                        findIn(j) = ret(j).IDPersona
                    Next
                End If
                'ret.Comparer = New ByIdComparer
            Next

            If (nMax.HasValue AndAlso ret.Count > nMax.Value) Then
                While (ret.Count > nMax.Value)
                    ret.RemoveAt(ret.Count - 1)
                End While
            End If

            Return ret
        End Function


        Public Function FindAZ(ByVal filters() As CRMFinlterI) As CCollection(Of CPersonaInfo)
            If (filters Is Nothing) Then Throw New ArgumentNullException("filters")

            Dim ret As New CCollection(Of CPersonaInfo)
            Dim findIn As Integer() = Nothing
            Dim nMax As Integer? = Nothing
            For i As Integer = filters.Length - 1 To 0 Step -1
                nMax = Math.Min(nMax, filters(i).nMax)
            Next

            If (nMax.HasValue AndAlso nMax.Value < 1) Then Return ret

            For i As Integer = filters.Length - 1 To 0 Step -1
                Dim filter As CRMFinlterI = filters(i)
                filter.nMax = Nothing
                ret = Me.FindAZ(filter, findIn)
                If (ret.Count = 0) Then Exit For
                If (filters.Length > 1) Then
                    findIn = Arrays.CreateInstance(Of Integer)(ret.Count)
                    For j As Integer = 0 To ret.Count - 1
                        findIn(j) = ret(j).IDPersona
                    Next
                End If
                'ret.Comparer = New ByIdComparer
            Next

            If (nMax.HasValue AndAlso ret.Count > nMax.Value) Then
                While (ret.Count > nMax.Value)
                    ret.RemoveAt(ret.Count)
                End While
            End If

            Return ret
        End Function

        Private Class ByIdComparer
            Implements IComparer

            Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
                Return DirectCast(x, CPersonaInfo).IDPersona.CompareTo(DirectCast(y, CPersonaInfo).IDPersona)
            End Function
        End Class

        Public Function Find(ByVal filter As CRMFinlterI) As CCollection(Of CPersonaInfo)
            Return Me.Find(filter, Nothing)
        End Function

        Private Function Find(ByVal filter As CRMFinlterI, ByVal findIn() As Integer) As CCollection(Of CPersonaInfo)
            Const CMDID As String = "id:"
            Const CMDAZIENDA As String = "azienda:"
            Const CMDENTE As String = "ente:"
            Const CMDCATEGORIA As String = "categoria:"
            Const CMDTIPOLOGIA As String = "tipologia:"
            Const CMDTELEFONO As String = "telefono:"
            Const CMDINDIRIZZO As String = "indirizzo:"
            Const CMDNOME As String = "nome:"
            Const CMDPROVINCIA As String = "provincia:"
            Const CMDCOMUNE As String = "comune:"
            Const CMDEMAIL As String = "e-mail:"
            Const CDCF As String = "codice fiscale:"
            Const PIVA As String = "partita iva:"
            Const CDTR As String = "tipo rapporto:"
            Const CDCOMPLEANNO As String = "compleanno:"
            Const CMDDATARICONTATTO As String = "data ricontatto:"
            Const CMDLISTA As String = "lista:"
            Const CMDNUMERODOCUMENTO As String = "numero documento:"
            Const CMDPO As String = "punto operativo:"

            Dim retCol As CCollection(Of CPersonaInfo) = Nothing
            Dim name, cf, num As String 'name1

            Dim parametriLista As String = filter.text
            parametriLista = Replace(Trim(parametriLista), "  ", " ")
            parametriLista = Replace(parametriLista, vbCr, vbLf)
            Dim params() As String = Split(parametriLista, vbLf)

            For i As Integer = 0 To UBound(params)
                Dim param As String = params(i)
                Dim col As New CCollection(Of CPersonaInfo)
                param = Trim(Replace(param, "  ", " "))

                name = param
                cf = Formats.ParseCodiceFiscale(name)
                num = Formats.ParsePhoneNumber(name)

                'name = LCase(Replace(name, "'", "''"))
                'name1 = Strings.OnlyChars(name)
                'cf = Replace(cf, "'", "''")
                'num = Replace(num, "'", "''")

                col.Sorted = False
                Select Case LCase(Trim(filter.Tipo))
                    Case "id" : Me.Find_ByID(filter.text, col, filter)
                    Case "azienda" : Me.Find_ByAzienda(filter.text, col, filter, findIn)
                    Case "ente" : Me.Find_ByEnte(filter.text, col, filter, findIn)
                    Case "categoria" : Me.Find_ByCategoria(filter.text, col, filter, findIn)
                    Case "tipologia" : Me.Find_ByTipologia(filter.text, col, filter, findIn)
                    Case "telefono" : Me.Find_ByTelefono(filter.text, col, filter, findIn)
                    Case "indirizzo" : Me.Find_ByIndirizzo(filter.text, col, filter, findIn)
                    Case "nome" : Me.Find_ByNome(filter.text, col, filter, findIn)
                    Case "provincia" : Me.Find_ByProvincia(filter.text, col, filter, findIn)
                    Case "comune" : Me.Find_ByComune(filter.text, col, filter, findIn)
                    Case "e-mail" : Me.Find_ByEMail(filter.text, col, filter, findIn)
                    Case "codice fiscale" : Me.Find_ByCFOrPIVA(filter.text, col, filter, findIn)
                    Case "partita iva" : Me.Find_ByPIVA(filter.text, col, filter, findIn)
                    Case "tipo rapporto" : Me.Find_ByTipoRapporto(filter.text, col, filter, findIn)
                    Case "compleanno" : Me.Find_ByCompleanno(filter.text, col, filter, findIn)
                    Case "data ricontatto" : Me.Find_ByDataRicontatto(filter.text, col, filter, findIn)
                    Case "lista" : Me.Find_ByLista(filter.text, col, filter)
                    Case "numero documento" : Me.Find_ByNumeroDocumento(filter.text, filter, col, findIn)
                    Case "punto operativo" : Me.Find_ByPO(filter.text, filter, col, findIn)
                    Case Else
                        Dim t As Boolean = False
                        Dim gestori As CCollection(Of FindPersonaHandler) = GetInstalledFindHandlers()
                        For Each g As FindPersonaHandler In gestori
                            t = LCase(Trim(g.GetHandledCommand())) = LCase(Trim(filter.Tipo))
                            If (t) Then
                                g.Find(name, filter, col)
                                Exit For
                            End If
                        Next


                        If Not t Then
                            If Left(LCase(param), Len(CMDAZIENDA)) = CMDAZIENDA Then
                                Find_ByAzienda(Mid(param, Len(CMDAZIENDA) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDENTE)) = CMDENTE Then
                                Find_ByEnte(Mid(param, Len(CMDENTE) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDTIPOLOGIA)) = CMDTIPOLOGIA Then
                                Find_ByTipologia(Mid(param, Len(CMDTIPOLOGIA) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDCATEGORIA)) = CMDCATEGORIA Then
                                Find_ByCategoria(Mid(param, Len(CMDCATEGORIA) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDID)) = CMDID Then
                                Find_ByID(Mid(param, Len(CMDID) + 1), col, filter)
                            ElseIf Left(LCase(param), Len(CMDINDIRIZZO)) = CMDINDIRIZZO Then
                                Find_ByIndirizzo(Mid(param, Len(CMDINDIRIZZO) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDTELEFONO)) = CMDTELEFONO Then
                                Find_ByTelefono(Mid(param, Len(CMDTELEFONO) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDNOME)) = CMDNOME Then
                                Find_ByNome(Mid(param, Len(CMDNOME) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDPROVINCIA)) = CMDPROVINCIA Then
                                Find_ByProvincia(Mid(param, Len(CMDPROVINCIA) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDCOMUNE)) = CMDCOMUNE Then
                                Find_ByComune(Mid(param, Len(CMDCOMUNE) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDEMAIL)) = CMDEMAIL Then
                                Find_ByEMail(Mid(param, Len(CMDEMAIL) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CDCF)) = CDCF Then
                                Find_ByCFOrPIVA(Mid(param, Len(CDCF) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(PIVA)) = PIVA Then
                                Find_ByPIVA(Mid(param, Len(PIVA) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CDTR)) = CDTR Then
                                Find_ByTipoRapporto(Mid(param, Len(CDTR) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CDCOMPLEANNO)) = CDCOMPLEANNO Then
                                Find_ByCompleanno(Mid(param, Len(CDCOMPLEANNO) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDDATARICONTATTO)) = CMDDATARICONTATTO Then
                                Find_ByDataRicontatto(Mid(param, Len(CMDDATARICONTATTO) + 1), col, filter, findIn)
                            ElseIf Left(LCase(param), Len(CMDLISTA)) = CMDLISTA Then
                                Find_ByLista(Mid(param, Len(CMDLISTA) + 1), col, filter)
                            ElseIf (Left(LCase(param), Len(CMDNUMERODOCUMENTO)) = CMDNUMERODOCUMENTO) Then
                                Me.Find_ByNumeroDocumento(Mid(param, Len(CMDNUMERODOCUMENTO) + 1), filter, col, findIn)
                            ElseIf (Left(LCase(param), Len(cmdpo)) = CMDPO) Then
                                Me.Find_ByPO(Mid(param, Len(CMDPO) + 1), filter, col, findIn)
                            Else
                                Dim relazioni() As String = Anagrafica.RelazioniParentali.GetSupportedNames
                                For Each rel As String In relazioni
                                    If LCase(Strings.Left(param, Len(rel) + 1)) = Strings.JoinW(LCase(rel), ":") Then
                                        Me.Find_ByRelazione(rel, Mid(param, Len(rel) + 2), col, filter)
                                        t = True
                                    ElseIf LCase(Strings.Left(param, Len(rel) + 4)) = Strings.JoinW(LCase(rel), " di:") Then
                                        Me.Find_ByRelazioneInversa(rel, Mid(param, Len(rel) + 5), col, filter)
                                        t = True
                                    End If
                                Next

                                If (Not t) Then
                                    If (IsEmail(name)) Then
                                        Find_ByEMail(name, col, filter, findIn)
                                    ElseIf (IsWebSite(name)) Then
                                        Find_ByWebSite(name, col, filter, findIn)
                                    ElseIf (IsCFOPIVA(name)) Then
                                        Find_ByCFOrPIVA(cf, col, filter, findIn)
                                        If (Len(num) > 3) Then Find_ByTelefono(num, col, filter, findIn)
                                    ElseIf (IsNumeroDocumento(name)) Then
                                        Find_ByNumeroDocumento(name, filter, col, findIn)
                                    ElseIf (IsPhoneNumber(name)) Then
                                        Find_ByTelefono(num, col, filter, findIn)
                                    Else
                                        t = False
                                        'Dim gestori As CCollection(Of FindPersonaHandler) = GetInstalledFindHandlers()
                                        For Each g As FindPersonaHandler In gestori
                                            Try
                                                t = g.CanHandle(param, filter)
                                                If (t) Then
                                                    g.Find(name, filter, col)
                                                    Exit For
                                                End If
                                            Catch ex As Exception
                                                Sistema.Events.NotifyUnhandledException(ex)
                                            End Try
                                        Next
                                        If (t = False) Then
                                            Find_ByNome(name, col, filter, findIn)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                End Select

                If (retCol Is Nothing) Then
                    retCol = col
                Else
                    retCol = retCol.IntersectWith(col)
                End If
            Next

            'retCol.Sort()
            Return retCol
        End Function

        Private Function IsEmail(ByVal value As String) As Boolean
            value = LCase(Replace(value, " ", ""))
            Return Sistema.EMailer.IsValidAddress(value)
        End Function

        Private Function IsPhoneNumber(ByVal value As String) As Boolean
            value = Trim(value)
            Dim ch As String = Left(value, 1)
            If (InStr("0123456789+", ch) <= 0) Then Return False
            Return Len(Formats.ParsePhoneNumber(value)) > 3
        End Function

        Private Function IsWebSite(ByVal value As String) As Boolean
            value = LCase(Replace(value, " ", ""))
            If (Left(value, 4) = "www." OrElse Left(value, 5) = "http:" OrElse Left(value, 6) = "https:") Then
                Return True
            ElseIf (Left(value, 4) = "ftp." OrElse Left(value, 4) = "ftp:" OrElse Left(value, 7) = "ftps:") Then
                Return True
            Else
                Return False
            End If
        End Function


        Public Function IsNumeroDocumento(ByVal value As String) As Boolean
            value = LCase(Replace(value, " ", ""))
            If (Len(value) <> 9) Then Return False
            If Char.IsLetter(Mid(value, 1, 1)) AndAlso Char.IsLetter(Mid(value, 2, 1)) Then
                For i As Integer = 3 To 9
                    If Not Char.IsNumber(Mid(value, i, 1)) Then Return False
                Next
                Return True
            ElseIf Char.IsLetter(Mid(value, 8, 1)) AndAlso Char.IsLetter(Mid(value, 9, 1)) Then
                For i As Integer = 1 To 7
                    If Not Char.IsNumber(Mid(value, i, 1)) Then Return False
                Next
                Return True
            End If
            Return False
        End Function

        Private Function IsCFOPIVA(ByVal value As String) As Boolean
            Dim cf As String = Formats.ParseCodiceFiscale(value)
            If (Len(cf) >= 16) Then Return True
            Dim piva As String = Formats.ParsePartitaIVA(value)
            If (Len(piva) >= 11) Then Return True
            Return False
        End Function

        ''' <summary>
        ''' Restituisce una collezione di oggetto CPersonaInfo contenete le sole persone fisiche corrispondenti ai parametri di ricerca specificati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindPF(ByVal filter As CRMFinlterI) As CCollection(Of CPersonaInfo)
            Return Me.FindPF(filter, Nothing)
        End Function

        Private Function FindPF(ByVal filter As CRMFinlterI, ByVal findIn As Integer()) As CCollection(Of CPersonaInfo)
            Const CMDAZIENDA As String = "azienda:"
            Const CMDENTE As String = "ente:"
            Const CMDTELEFONO As String = "telefono:"
            Const CMDINDIRIZZO As String = "indirizzo:"
            Const CMDNOME As String = "nome:"
            Const CMDPROVINCIA As String = "provincia:"
            Const CMDCOMUNE As String = "comune:"
            Const CMDEMAIL As String = "e-mail:"
            Const CDCF As String = "codice fiscale:"
            Const PIVA As String = "partita iva:"
            Const CDTR As String = "tipo rapporto:"
            Const CDCOMPLEANNO As String = "compleanno:"
            Const CMDNUMERODOCUMENTO As String = "numero documento:"
            Const CMDPO As String = "punto operativo:"

            Dim col As New CCollection(Of CPersonaInfo)
            Dim name, cf, num As String 'name1, 

            Dim param As String = Trim(Replace(filter.text, "  ", " "))
            name = param
            cf = Formats.ParseCodiceFiscale(name)
            num = Formats.ParsePhoneNumber(name)

            'name = LCase(Replace(name, "'", "''"))
            'name1 = Strings.OnlyChars(name)
            'cf = Replace(cf, "'", "''")
            'num = Replace(num, "'", "''")
            filter = filter.Clone
            filter.tipoPersona = TipoPersona.PERSONA_FISICA

            col.Sorted = False
            Select Case LCase(Trim(filter.Tipo))
                Case "id" : Me.Find_ByID(filter.text, col, filter)
                Case "azienda" : Me.Find_ByAzienda(filter.text, col, filter, findIn)
                Case "ente" : Me.Find_ByEnte(filter.text, col, filter, findIn)
                Case "categoria" : Me.Find_ByCategoria(filter.text, col, filter, findIn)
                Case "tipologia" : Me.Find_ByTipologia(filter.text, col, filter, findIn)
                Case "telefono" : Me.Find_ByTelefono(filter.text, col, filter, findIn)
                Case "indirizzo" : Me.Find_ByIndirizzo(filter.text, col, filter, findIn)
                Case "nome" : Me.Find_ByNome(filter.text, col, filter, findIn)
                Case "provincia" : Me.Find_ByProvincia(filter.text, col, filter, findIn)
                Case "comune" : Me.Find_ByComune(filter.text, col, filter, findIn)
                Case "e-mail" : Me.Find_ByEMail(filter.text, col, filter, findIn)
                Case "codice fiscale" : Me.Find_ByCFOrPIVA(filter.text, col, filter, findIn)
                Case "partita iva" : Me.Find_ByPIVA(filter.text, col, filter, findIn)
                Case "tipo rapporto" : Me.Find_ByTipoRapporto(filter.text, col, filter, findIn)
                Case "compleanno" : Me.Find_ByCompleanno(filter.text, col, filter, findIn)
                Case "data ricontatto" : Me.Find_ByDataRicontatto(filter.text, col, filter, findIn)
                Case "lista" : Me.Find_ByLista(filter.text, col, filter)
                Case "numero documento" : Me.Find_ByNumeroDocumento(filter.text, filter, col, findIn)
                Case "punto operativo" : Me.Find_ByPO(filter.text, filter, col, findIn)
                Case Else
                    If Left(LCase(param), Len(CMDAZIENDA)) = CMDAZIENDA Then
                        Find_ByAzienda(Mid(param, Len(CMDAZIENDA) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDENTE)) = CMDENTE Then
                        Find_ByEnte(Mid(param, Len(CMDAZIENDA) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDINDIRIZZO)) = CMDINDIRIZZO Then
                        Find_ByIndirizzo(Mid(param, Len(CMDINDIRIZZO) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDTELEFONO)) = CMDTELEFONO Then
                        Find_ByTelefono(Mid(param, Len(CMDTELEFONO) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDNOME)) = CMDNOME Then
                        Find_ByNome(Mid(param, Len(CMDNOME) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDPROVINCIA)) = CMDPROVINCIA Then
                        Find_ByProvincia(Mid(param, Len(CMDPROVINCIA) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDCOMUNE)) = CMDCOMUNE Then
                        Find_ByComune(Mid(param, Len(CMDCOMUNE) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDEMAIL)) = CMDEMAIL Then
                        Find_ByEMail(Mid(param, Len(CMDEMAIL) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CDCF)) = CDCF Then
                        Find_ByCFOrPIVA(Mid(param, Len(CDCF) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(PIVA)) = PIVA Then
                        Find_ByPIVA(Mid(param, Len(PIVA) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CDTR)) = CDTR Then
                        Find_ByTipoRapporto(Mid(param, Len(CDTR) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CDCOMPLEANNO)) = CDCOMPLEANNO Then
                        Find_ByCompleanno(Mid(param, Len(CDCOMPLEANNO) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDNUMERODOCUMENTO)) = CMDNUMERODOCUMENTO Then
                        Me.Find_ByNumeroDocumento(Mid(param, Len(CMDNUMERODOCUMENTO) + 1), filter, col, findIn)
                    ElseIf (Left(LCase(param), Len(cmdpo)) = CMDPO) Then
                        Me.Find_ByPO(Mid(param, Len(CMDPO) + 1), filter, col, findIn)
                    Else
                        'If (col.Count = 0) And (cf <> "") Then
                        '    Find_ByCFOrPIVA(cf, col, TipoPersona.PERSONA_FISICA, nMax)
                        'End If
                        'If (col.Count = 0) And ((Left(name, 1) >= "a" And Left(name, 1) <= "z")) Then
                        '    Find_ByNome(name, col, ignoreRights, TipoPersona.PERSONA_FISICA, nMax, intelliSearch)
                        'End If
                        'If (col.Count = 0) And (num <> "") Then
                        '    Find_ByTelefono(num, col, ignoreRights, TipoPersona.PERSONA_FISICA, nMax)
                        'End If
                        'If (col.Count = 0) And (param <> "") Then
                        '    Find_ByEMail(param, col, ignoreRights, TipoPersona.PERSONA_FISICA, nMax)
                        'End If
                        If (IsEmail(name)) Then
                            Find_ByEMail(name, col, filter, findIn)
                        ElseIf (IsWebSite(name)) Then
                            Find_ByWebSite(name, col, filter, findIn)
                        ElseIf (IsCFOPIVA(name)) Then
                            Find_ByCFOrPIVA(cf, col, filter, findIn)
                            If (Len(num) > 3) Then Find_ByTelefono(num, col, filter, findIn)
                        ElseIf (IsNumeroDocumento(name)) Then
                            Find_ByNumeroDocumento(name, filter, col, findIn)
                        ElseIf (IsPhoneNumber(name)) Then
                            Find_ByTelefono(num, col, filter, findIn)
                        Else
                            Find_ByNome(name, col, filter, findIn)
                        End If
                    End If
            End Select
            'col.Sort()
            Return col
        End Function

        ''' <summary>
        ''' Restituisce una collezione di oggetto CPersonaInfo contenete le sole aziende corrispondenti ai parametri di ricerca specificati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindAZ(ByVal filter As CRMFinlterI) As CCollection(Of CPersonaInfo)
            Return Me.FindAZ(filter, Nothing)
        End Function

        Private Function FindAZ(ByVal filter As CRMFinlterI, ByVal findIn As Integer()) As CCollection(Of CPersonaInfo)
            Const CMDAZIENDA As String = "azienda:"
            Const CMDENTE As String = "ente:"
            Const CMDTELEFONO As String = "telefono:"
            Const CMDINDIRIZZO As String = "indirizzo:"
            Const CMDNOME As String = "nome:"
            Const CMDPROVINCIA As String = "provincia:"
            Const CMDCOMUNE As String = "comune:"
            Const CMDEMAIL As String = "e-mail:"
            Const CDCF As String = "codice fiscale:"
            Const PIVA As String = "partita iva:"
            Const CDTR As String = "tipo rapporto:"
            Const CDCOMPLEANNO As String = "compleanno:"
            Const CMDNUMERODOCUMENTO As String = "numero documento:"
            Const CMDPO As String = "punto operativo:"

            Dim col As New CCollection(Of CPersonaInfo)
            Dim name, cf, num As String

            Dim param As String = Trim(Replace(filter.text, "  ", " "))
            name = param
            cf = Formats.ParseCodiceFiscale(name)
            num = Formats.ParsePhoneNumber(name)

            filter = filter.Clone
            filter.tipoPersona = TipoPersona.PERSONA_GIURIDICA
            'name = LCase(Replace(name, "'", "''"))
            'name1 = Strings.OnlyChars(name)
            'cf = Replace(cf, "'", "''")
            'num = Replace(num, "'", "''")

            col.Sorted = False
            Select Case LCase(Trim(filter.Tipo))
                Case "id" : Me.Find_ByID(filter.text, col, filter)
                Case "azienda" : Me.Find_ByAzienda(filter.text, col, filter, findIn)
                Case "ente" : Me.Find_ByEnte(filter.text, col, filter, findIn)
                Case "categoria" : Me.Find_ByCategoria(filter.text, col, filter, findIn)
                Case "tipologia" : Me.Find_ByTipologia(filter.text, col, filter, findIn)
                Case "telefono" : Me.Find_ByTelefono(filter.text, col, filter, findIn)
                Case "indirizzo" : Me.Find_ByIndirizzo(filter.text, col, filter, findIn)
                Case "nome" : Me.Find_ByNome(filter.text, col, filter, findIn)
                Case "provincia" : Me.Find_ByProvincia(filter.text, col, filter, findIn)
                Case "comune" : Me.Find_ByComune(filter.text, col, filter, findIn)
                Case "e-mail" : Me.Find_ByEMail(filter.text, col, filter, findIn)
                Case "codice fiscale" : Me.Find_ByCFOrPIVA(filter.text, col, filter, findIn)
                Case "partita iva" : Me.Find_ByPIVA(filter.text, col, filter, findIn)
                Case "tipo rapporto" : Me.Find_ByTipoRapporto(filter.text, col, filter, findIn)
                Case "compleanno" : Me.Find_ByCompleanno(filter.text, col, filter, findIn)
                Case "data ricontatto" : Me.Find_ByDataRicontatto(filter.text, col, filter, findIn)
                Case "lista" : Me.Find_ByLista(filter.text, col, filter)
                Case "numero documento" : Me.Find_ByNumeroDocumento(filter.text, filter, col, findIn)
                Case "punto operativo" : Me.Find_ByPO(filter.text, filter, col, findIn)
                Case Else
                    If Left(LCase(param), Len(CMDAZIENDA)) = CMDAZIENDA Then
                        Find_ByNome(Mid(param, Len(CMDAZIENDA) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDENTE)) = CMDENTE Then
                        Find_ByNome(Mid(param, Len(CMDENTE) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDINDIRIZZO)) = CMDINDIRIZZO Then
                        Find_ByIndirizzo(Mid(param, Len(CMDINDIRIZZO) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDTELEFONO)) = CMDTELEFONO Then
                        Find_ByTelefono(Mid(param, Len(CMDTELEFONO) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDNOME)) = CMDNOME Then
                        Find_ByNome(Mid(param, Len(CMDNOME) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDPROVINCIA)) = CMDPROVINCIA Then
                        Find_ByProvincia(Mid(param, Len(CMDPROVINCIA) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDCOMUNE)) = CMDCOMUNE Then
                        Find_ByComune(Mid(param, Len(CMDCOMUNE) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDEMAIL)) = CMDEMAIL Then
                        Find_ByEMail(Mid(param, Len(CMDEMAIL) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CDCF)) = CDCF Then
                        Find_ByCFOrPIVA(Mid(param, Len(CDCF) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(PIVA)) = PIVA Then
                        Find_ByPIVA(Mid(param, Len(PIVA) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CDTR)) = CDTR Then
                        Find_ByTipoRapporto(Mid(param, Len(CDTR) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CDCOMPLEANNO)) = CDCOMPLEANNO Then
                        Find_ByCompleanno(Mid(param, Len(CDCOMPLEANNO) + 1), col, filter, findIn)
                    ElseIf Left(LCase(param), Len(CMDNUMERODOCUMENTO)) = CMDNUMERODOCUMENTO Then
                        Me.Find_ByNumeroDocumento(Mid(param, Len(CMDNUMERODOCUMENTO) + 1), filter, col, findIn)
                    ElseIf (Left(LCase(param), Len(cmdpo)) = CMDPO) Then
                        Me.Find_ByPO(Mid(param, Len(CMDPO) + 1), filter, col, findIn)
                    Else
                        'If (col.Count = 0) And (cf <> "") Then
                        '    Find_ByCFOrPIVA(cf, col, TipoPersona.PERSONA_GIURIDICA, nMax)
                        'End If
                        'If (col.Count = 0) And ((Left(name, 1) >= "a" And Left(name, 1) <= "z")) Then
                        '    Find_ByNome(name, col, ignoreRights, TipoPersona.PERSONA_GIURIDICA, nMax, intelliSearch)
                        'End If
                        'If (col.Count = 0) And (num <> "") Then
                        '    Find_ByTelefono(num, col, ignoreRights, TipoPersona.PERSONA_GIURIDICA, nMax)
                        'End If
                        'If (col.Count = 0) And (param <> "") Then
                        '    Find_ByEMail(param, col, ignoreRights, TipoPersona.PERSONA_GIURIDICA, nMax)
                        'End If
                        If (IsEmail(name)) Then
                            Find_ByEMail(name, col, filter, findIn)
                        ElseIf (IsWebSite(name)) Then
                            Find_ByWebSite(name, col, filter, findIn)
                        ElseIf (IsCFOPIVA(name)) Then
                            Find_ByCFOrPIVA(cf, col, filter, findIn)
                            If (Len(num) > 3) Then Find_ByTelefono(num, col, filter, findIn)
                        ElseIf (IsNumeroDocumento(name)) Then
                            Find_ByNumeroDocumento(name, filter, col, findIn)
                        ElseIf (IsPhoneNumber(name)) Then
                            Find_ByTelefono(num, col, filter, findIn)
                        Else
                            Find_ByNome(name, col, filter, findIn)
                        End If
                    End If
            End Select
            'col.Sort()
            Return col
        End Function



#End Region

        Public Function GetPersonaByParams(ByVal nominativo As String, ByVal dataNascita As Nullable(Of Date)) As CPersonaFisica
            Dim cursor As CPersonaFisicaCursor = Nothing
            Try
                nominativo = Trim(nominativo)
                If (nominativo = "" AndAlso dataNascita.HasValue = False) Then Return Nothing

                cursor = New CPersonaFisicaCursor
                If nominativo <> vbNullString Then cursor.Nominativo.Value = nominativo
                If dataNascita.HasValue Then cursor.DataNascita.Value = dataNascita.Value

                If cursor.Count = 1 Then
                    Return cursor.Item
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function Instantiate(ByVal tipoPersona As TipoPersona) As CPersona
            Select Case tipoPersona
                Case Anagrafica.TipoPersona.PERSONA_FISICA : Return New CPersonaFisica
                Case Else : Return New CAzienda
            End Select
        End Function

        Public Function CreatePersonaFisica(ByVal nome As String, ByVal cognome As String) As CPersona
            Dim ret As New CPersonaFisica
            ret.Nome = nome
            ret.Cognome = cognome
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Save()
            Return ret
        End Function

        'Public Function CreateAzienda(ByVal ragioneSociale As String) As CAzienda
        '    Dim ret As New CAzienda
        '    ret.RagioneSociale = ragioneSociale
        '    ret.Stato = ObjectStatus.OBJECT_VALID
        '    APPConn.Save(ret)
        '    Return ret
        'End Function

        ''' <summary>
        ''' Restituisce l'elenco delle persone che compiono gli anni 
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCompleanniDi(ByVal data As Date) As CCollection(Of CPersonaFisica)
            Dim cursor As CPersonaCursor = Nothing
            Try
                Dim ret As New CCollection(Of CPersonaFisica)

                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.DataMorte.Value = Nothing
                cursor.DataNascita.Value = Nothing
                cursor.DataNascita.Operator = OP.OP_NE
                cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
                cursor.WhereClauses.Add(Strings.JoinW("Month([DataNascita])=", Month(Today), " And Day([DataNascita])=", Day(Today)))
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function



        Public Function GetPersonaByCF(ByVal cf As String) As CPersona
            Dim cursor As CPersonaCursor = Nothing
            Try
                cf = Formats.ParseCodiceFiscale(cf)
                If (cf = vbNullString) Then Return Nothing


                cursor = New CPersonaCursor
                cursor.CodiceFiscale.Value = cf
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                Return cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function FindPersoneByCF(ByVal cf As String) As CCollection(Of CPersona)
            Dim cursor As CPersonaCursor = Nothing
            Dim ret As New CCollection(Of CPersona)

            Try
                cf = Formats.ParseCodiceFiscale(cf)
                If (cf = vbNullString) Then Return ret

                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.CodiceFiscale.Value = Formats.ParseCodiceFiscale(cf)
                cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function FindPersonaByPIVA(ByVal cf As String) As CPersona
            Dim cursor As CPersonaCursor = Nothing
            Try
                cf = Formats.ParsePartitaIVA(cf)
                If (cf = vbNullString) Then Return Nothing

                cursor = New CPersonaCursor
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.PartitaIVA.Value = cf
                Return cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function AssegnaResidentiPerProvincia(ByVal nomeProvincia As String, ByVal po As CUfficio, Optional ByVal force As Boolean = False) As Integer
            Dim cursor As CPersonaCursor = Nothing
            Dim ret As Integer

            Try
                cursor = New CPersonaCursor
                cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ResidenteA_Provincia.Value = Trim(nomeProvincia)
                If Not force Then
                    cursor.IDPuntoOperativo.Value = 0
                    cursor.IDPuntoOperativo.IncludeNulls = True
                End If
                ret = cursor.Count

                While Not cursor.EOF
                    cursor.Item.PuntoOperativo = po
                    cursor.Item.Save()
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function AssegnaResidentiPerComune(ByVal nomeComune As String, ByVal nomeProvincia As String, ByVal po As CUfficio, Optional ByVal force As Boolean = False) As Integer
            Dim cursor As CPersonaCursor = Nothing
            Try
                Dim ret As Integer

                cursor = New CPersonaCursor
                cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ResidenteA_Citta.Value = Trim(nomeComune)
                cursor.ResidenteA_Provincia.Value = Trim(nomeProvincia)
                If Not force Then
                    cursor.IDPuntoOperativo.Value = 0
                    cursor.IDPuntoOperativo.IncludeNulls = True
                End If
                ret = cursor.Count
                While Not cursor.EOF
                    cursor.Item.PuntoOperativo = po
                    cursor.Item.Save()
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function GetPersonaById(ByVal id As Integer) As CPersonaFisica
            If (id = 0) Then Return Nothing
            Dim ret As CPersonaFisica = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                dbRis = APPConn.ExecuteReader(Strings.JoinW("SELECT * FROM [tbl_Persone] WHERE [ID]=", CStr(id), " AND [TipoPersona]=", CStr(TipoPersona.PERSONA_FISICA)))
                If (dbRis.Read) Then
                    ret = New CPersonaFisica
                    APPConn.Load(ret, dbRis)
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Private Sub Find_ByNumeroDocumento(ByVal numero As String, filter As CRMFinlterI, col As CCollection(Of CPersonaInfo), ByVal findIn As Integer())
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim cursor As CPersonaCursor = Nothing

            Try
                If (col Is Nothing) Then Throw New ArgumentNullException("col")
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")

                numero = Strings.Replace(numero, " ", "")
                If (numero = "") Then Exit Sub

                Dim dbSQL As New System.Text.StringBuilder
                dbSQL.Append("SELECT [OwnerID] FROM [tbl_Attachments] WHERE [Stato]=")
                dbSQL.Append(ObjectStatus.OBJECT_VALID)
                dbSQL.Append(" And ([OwnerType] ='CPersonaFisica' Or [OwnerType]='CAzienda') AND [Parametro]=")
                dbSQL.Append(DBUtils.DBString(numero))
                dbSQL.Append(" GROUP BY [OwnerID]")

                dbRis = Sistema.Attachments.Database.ExecuteReader(dbSQL.ToString)

                Dim list As New System.Collections.ArrayList
                While dbRis.Read
                    Dim id As Integer = Formats.ToInteger(dbRis("OwnerID"))
                    If (id <> 0) Then
                        list.Add(id)
                    End If
                End While
                dbRis.Dispose()
                If (list.Count = 0) Then Exit Sub

                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Dim arr() As Integer = list.ToArray(GetType(Integer))
                If (findIn IsNot Nothing) Then
                    Arrays.Sort(arr)
                    Arrays.Sort(findIn)
                    arr = Arrays.Join(arr, findIn)
                End If
                cursor.ID.ValueIn(arr)

                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    Me.AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
                cursor.Dispose()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose()
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Sub

        Private Sub Find_ByPO(ByVal nome As String, filter As CRMFinlterI, col As CCollection(Of CPersonaInfo), ByVal findIn As Integer())
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim cursor As CPersonaCursor = Nothing

            Try
                If (col Is Nothing) Then Throw New ArgumentNullException("col")
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")

                nome = Strings.LCase(Strings.Trim(nome))
                nome = Strings.Replace(nome, "  ", " ")
                If (nome = "") Then Exit Sub


                cursor = New CPersonaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                If (findIn IsNot Nothing) Then cursor.ID.ValueIn(findIn)
                cursor.NomePuntoOperativo.Value = nome
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    Me.AddPersonInfo(col, cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
                cursor.Dispose()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose()
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Sub

        Private Function JoinID(ByVal findIn As Integer(), ByVal sep As String) As String
            Dim ret As New System.Text.StringBuilder
            Dim l As Integer = Arrays.Len(findIn)
            For i As Integer = 0 To l - 1
                If (i > 0) Then ret.Append(",")
                ret.Append(DBUtils.DBNumber(findIn(i)))
            Next
            Return ret.ToString
        End Function


        Public Function FindPersoneByNumero(ByVal numero As String) As CCollection(Of CPersona)
            Dim cursor As CContattoCursor = Nothing
            Dim cursor1 As CPersonaCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim persone As New CCollection(Of CPersona)

            Try
                numero = Formats.ParsePhoneNumber(numero)
                If (Len(numero) <= 3) Then Return persone

                Dim numeri() As String
                If (Left(numero, 1) = "+") Then
                    numero = "00" & Mid(numero, 2)
                End If
                If (Left(numero, 4) = "0039") AndAlso Len(numero) > 5 Then
                    numeri = {numero, Mid(numero, 5)}
                Else
                    numeri = {numero}
                End If

                cursor = New CContattoCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Tipo.ValueIn({"Cellulare", "Telefono", "Fax"})
                cursor.Valore.ValueIn(numeri)
                cursor.IgnoreRights = True

                Dim arr() As Integer = {}
                dbRis = APPConn.ExecuteReader("SELECT DISTINCT [Persona] FROM (" & cursor.GetSQL & ")")
                cursor.Dispose() : cursor = Nothing

                While dbRis.Read
                    arr = Arrays.Append(arr, Formats.ToInteger(dbRis("Persona")))
                End While
                dbRis.Dispose() : dbRis = Nothing


                If (Arrays.Len(arr) > 0) Then
                    cursor1 = New CPersonaCursor
                    cursor1.ID.ValueIn(arr)
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor1.IgnoreRights = True
                    While Not cursor1.EOF
                        persone.Add(cursor1.Item)
                        cursor1.MoveNext()
                    End While
                    cursor1.Dispose() : cursor1 = Nothing
                End If

                Return Me.CheckIsSingleNumber(numero, persone)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing
            End Try


        End Function


        Private Function CheckIsSingleNumber(ByVal number As String, ByVal persone As CCollection(Of CPersona)) As CCollection(Of CPersona)
            Dim validate As New CCollection(Of CPersona)
            Dim i As Integer = 0
            While (i < persone.Count)
                Dim p As CPersona = persone(i)
                Dim f As Boolean = False
                For Each r As CContatto In p.Recapiti
                    If r.Stato = ObjectStatus.OBJECT_VALID AndAlso (r.Valore = number OrElse "0039" & r.Valore = number) Then
                        If r.Validated.HasValue Then
                            If r.Validated.Value = True Then
                                validate.Add(p)
                                Exit For
                            Else
                                persone.RemoveAt(i)
                                f = True
                            End If
                        End If
                    End If
                Next
                If (Not f) Then i += 1
            End While

            If (validate.Count > 0) Then
                Return validate
            Else
                Return persone
            End If
        End Function
    End Class
End Namespace


Partial Public Class Anagrafica



    Private Shared m_Persone As CPersoneClass = Nothing

    Public Shared ReadOnly Property Persone As CPersoneClass
        Get
            If (m_Persone Is Nothing) Then m_Persone = New CPersoneClass
            Return m_Persone
        End Get
    End Property


End Class