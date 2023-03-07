Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.CustomerCalls
Imports DMD.XML

Namespace Forms


    Public Class CCreditoVHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Function GetSFInCorso(ByVal renderer As Object) As String
            Dim persone As New CKeyCollection(Of CPersonaFisica)
            Dim finestre As New CCollection(Of FinestraLavorazioneXML)

            Dim cursor As New FinestraLavorazioneCursor()
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
            cursor.IDStudioDiFattibilita.Value = 0
            cursor.IDStudioDiFattibilita.Operator = OP.OP_NE
            cursor.StatoStudioDiFattibilita.ValueIn({StatoOfferteFL.Sconosciuto, StatoOfferteFL.InLavorazione})
            cursor.StatoCQS.Value = StatoOfferteFL.Sconosciuto
            cursor.StatoPD.Value = StatoOfferteFL.Sconosciuto
            cursor.StatoCQSI.Value = StatoOfferteFL.Sconosciuto
            cursor.StatoPDI.Value = StatoOfferteFL.Sconosciuto
            While (Not cursor.EOF())
                Dim w As New FinestraLavorazioneXML(cursor.Item)
                w.Prepara()
                If (w.W.IDCliente <> 0 AndAlso persone.GetItemByKey("K" & w.W.IDCliente) Is Nothing AndAlso w.W.Cliente IsNot Nothing) Then
                    persone.Add("K" & w.W.IDCliente, w.W.Cliente)
                    finestre.Add(w)
                End If
                cursor.MoveNext()
            End While
            cursor.Dispose()


            Dim ret As New CKeyCollection
            ret.Add("persone", New CCollection(persone))
            ret.Add("finestre", finestre)

            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetPraticheCaricare(ByVal renderer As Object) As String
            Dim persone As New CKeyCollection(Of CPersonaFisica)
            Dim finestre As New CCollection(Of FinestraLavorazioneXML)

            Dim cursor As New FinestraLavorazioneCursor()
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
            cursor.IDStudioDiFattibilita.Value = 0
            cursor.IDStudioDiFattibilita.Operator = OP.OP_NE
            cursor.StatoStudioDiFattibilita.Value = StatoOfferteFL.Liquidata
            cursor.StatoCQS.Value = StatoOfferteFL.Sconosciuto
            cursor.StatoPD.Value = StatoOfferteFL.Sconosciuto
            cursor.StatoCQSI.Value = StatoOfferteFL.Sconosciuto
            cursor.StatoPDI.Value = StatoOfferteFL.Sconosciuto
            While (Not cursor.EOF())
                Dim w As New FinestraLavorazioneXML(cursor.Item)
                w.Prepara()
                If (w.W.IDCliente <> 0 AndAlso persone.GetItemByKey("K" & w.W.IDCliente) Is Nothing AndAlso w.W.Cliente IsNot Nothing) Then
                    persone.Add("K" & w.W.IDCliente, w.W.Cliente)
                    finestre.Add(w)
                End If
                cursor.MoveNext()
            End While
            cursor.Dispose()


            Dim ret As New CKeyCollection
            ret.Add("persone", New CCollection(persone))
            ret.Add("finestre", finestre)

            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetStudiDaFare(ByVal renderer As Object) As String

            'Dim di As Date = RPC.n2date(GetParameter(renderer, "fd"))

            Dim persone As New CKeyCollection(Of CPersonaFisica)
            Dim finestre As New CCollection(Of FinestraLavorazioneXML)

            Dim cursor As New FinestraLavorazioneCursor()
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
            cursor.IDRichiestaCertificato.Value = 0
            cursor.IDRichiestaCertificato.Operator = OP.OP_NE
            'cursor.StatoRichiestaCertificato.Value = StatoOfferteFL.Liquidata
            cursor.IDStudioDiFattibilita.Value = 0
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
            While (Not cursor.EOF())
                Dim w As New FinestraLavorazioneXML(cursor.Item)
                w.Prepara()
                If (w.W.IDCliente <> 0 AndAlso persone.GetItemByKey("K" & w.W.IDCliente) Is Nothing AndAlso w.W.Cliente IsNot Nothing) Then
                    persone.Add("K" & w.W.IDCliente, w.W.Cliente)
                    finestre.Add(w)
                End If
                cursor.MoveNext()
            End While
            cursor.Dispose()


            Dim ret As New CKeyCollection
            ret.Add("persone", New CCollection(persone))
            ret.Add("finestre", finestre)

            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.Pratiche.GetItemById(id)
        End Function

        Public Function GetCSDaRichiedere(ByVal renderer As Object) As String

            'Dim di As Date = RPC.n2date(GetParameter(renderer, "fd"))

            Dim persone As New CKeyCollection(Of CPersonaFisica)
            Dim finestre As New CCollection(Of FinestraLavorazioneXML)

            Dim cursor As New FinestraLavorazioneCursor()
            Dim arrpo As CCollection = XML.Utils.Serializer.Deserialize(Trim(RPC.n2str(GetParameter(renderer, "arrpo", ""))))
            If (arrpo.Count > 0) Then
                cursor.IDPuntoOperativo.ValueIn(arrpo.ToArray)
            End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDBustaPaga.Value = 0
            cursor.IDBustaPaga.Operator = OP.OP_NE
            cursor.IDRichiestaCertificato.Value = 0
            cursor.IDStudioDiFattibilita.Value = 0
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
            While (Not cursor.EOF())
                Dim w As New FinestraLavorazioneXML(cursor.Item)
                w.Prepara()
                If (w.W.IDCliente <> 0 AndAlso persone.GetItemByKey("K" & w.W.IDCliente) Is Nothing AndAlso w.W.Cliente IsNot Nothing) Then
                    persone.Add("K" & w.W.IDCliente, w.W.Cliente)
                    finestre.Add(w)
                End If
                cursor.MoveNext()
            End While
            cursor.Dispose()


            Dim ret As New CKeyCollection
            ret.Add("persone", New CCollection(persone))
            ret.Add("finestre", finestre)

            Return XML.Utils.Serializer.Serialize(ret)
        End Function


        Public Function GetFinestreRinnovabili(ByVal renderer As Object) As String

            'Dim di As Date = RPC.n2date(GetParameter(renderer, "fd"))

            Dim di As Date = Calendar.DateAdd(DateInterval.Day, -CQSPD.Configuration.GiorniAnticipoRifin, Calendar.ToDay())
            Dim df As Date = Calendar.DateAdd(DateInterval.Day, CQSPD.Configuration.GiorniAnticipoRifin, Calendar.ToDay())

            Dim cursor As New FinestraLavorazioneCursor
            Dim arrpo As CCollection = XML.Utils.Serializer.Deserialize(Trim(RPC.n2str(GetParameter(renderer, "arrpo", ""))))
            If (arrpo.Count > 0) Then
                cursor.IDPuntoOperativo.ValueIn(arrpo.ToArray)
            End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
            cursor.Flags.Value = FinestraLavorazioneFlags.Rinnovo
            cursor.Flags.Operator = OP.OP_ALLBITAND
            cursor.DataInizioLavorabilita.Between(di, df) ';//= Calendar.DateAdd(DateInterval.Day, 30, Calendar.ToDay)  'Between(di.Value, df.Value)
            cursor.DataInizioLavorabilita.SortOrder = SortEnum.SORT_ASC

            Dim finestre As New CCollection(Of FinestraLavorazioneXML)

            While (Not cursor.EOF())
                Dim w As FinestraLavorazione = cursor.Item
                Dim wx As New FinestraLavorazioneXML(w)
                wx.Prepara()
                finestre.Add(wx)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Dim persone As New CCollection(Of CPersonaFisica)
            Dim tmp As New System.Collections.ArrayList
            For Each w As FinestraLavorazioneXML In finestre
                If (w.W.IDCliente <> 0) Then tmp.Add(w.W.IDCliente)
            Next

            If (tmp.Count > 0) Then
                Dim cursor1 As New CPersonaFisicaCursor
                Dim arr As Integer() = tmp.ToArray(GetType(Integer))

                cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor1.ID.ValueIn(arr)
                While Not cursor1.EOF
                    persone.Add(cursor1.Item)
                    cursor1.MoveNext()
                End While

                cursor1.Dispose() : cursor1 = Nothing
            End If

            Dim ret As New CKeyCollection
            ret.Add("persone", New CCollection(persone))
            ret.Add("finestre", finestre)

            Return XML.Utils.Serializer.Serialize(ret)
        End Function


        Public Function GetVisitePerConsulenza(ByVal renderer As Object) As String
            Dim cursor As CVisiteCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim cursor1 As CPersonaFisicaCursor = Nothing

            Try
                Dim di As Date = RPC.n2date(GetParameter(renderer, "fd", ""))
                Dim df As Date? = RPC.n2date(GetParameter(renderer, "td", ""))
                Dim arrpo As CCollection = XML.Utils.Serializer.Deserialize(Trim(RPC.n2str(GetParameter(renderer, "arrpo", ""))))

                cursor = New CVisiteCursor
                If (arrpo.Count > 0) Then cursor.IDPuntoOperativo.ValueIn(arrpo.ToArray)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Scopo.ValueIn({"Prima Consulenza", "Consulenza Successiva", "Consulenza"})
                cursor.Data.Between(di, df)
                'cursor.Data.Operator = OP.OP_GE
                'cursor.PageSize = 1000

                Dim personeIDs As New CKeyCollection(Of Integer)
                Dim pid As Integer
                Dim dbSQL As String = "SELECT [IDPersona] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona]"
                dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                While dbRis.Read ' (Not cursor.EOF())
                    'var con = cursor.getItem();
                    'pid = cursor.Item.IDPersona
                    pid = Formats.ToInteger(dbRis("IDPersona"))
                    If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    pid = cursor.Item.IDPerContoDi
                    If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    ' cursor.MoveNext()
                End While
                dbRis.Dispose() : dbRis = Nothing
                cursor.Dispose() : cursor = Nothing

                Dim finestre As New CCollection(Of FinestraLavorazioneXML)
                Dim persone As New CCollection(Of CPersonaFisica)

                If (personeIDs.Count > 0) Then
                    Dim arr() As Integer = personeIDs.ToArray()

                    cursor1 = New CPersonaFisicaCursor
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor1.ID.ValueIn(arr)
                    cursor1.PageSize = 1000
                    While Not cursor1.EOF
                        Dim p As CPersonaFisica = cursor1.Item
                        persone.Add(p)

                        Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(p)
                        If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetUltimaFinestraLavorata(p)

                        If (w IsNot Nothing) Then
                            Dim wx As New FinestraLavorazioneXML(w)
                            wx.Prepara()
                            finestre.Add(wx)
                        End If

                        cursor1.MoveNext()
                    End While
                    cursor1.Dispose() : cursor1 = Nothing
                End If

                Dim ret As New CKeyCollection
                ret.Add("persone", persone)
                ret.Add("finestre", finestre)

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function GetClientiStay(ByVal renderer As Object) As String
            Dim cursor As CRichiestaConteggioCursor = Nothing
            Dim cursor1 As CPersonaFisicaCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim di As Date = RPC.n2date(GetParameter(renderer, "fd", ""))
                Dim df As Date? = RPC.n2date(GetParameter(renderer, "td", ""))
                Dim arrpo As CCollection = XML.Utils.Serializer.Deserialize(Trim(RPC.n2str(GetParameter(renderer, "arrpo", ""))))

                cursor = New CRichiestaConteggioCursor
                If (arrpo.Count > 0) Then cursor.IDPuntoOperativo.ValueIn(arrpo.ToArray)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.DataRichiesta.Between(di, df)
                'cursor.Data.Operator = OP.OP_GE
                cursor.PageSize = 1000

                Dim personeIDs As New CKeyCollection(Of Integer)
                Dim pid As Integer
                Dim dbSQL As String = "SELECT [IDCliente] FROM (" & cursor.GetSQL & ") GROUP BY [IDCliente]"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read ' (Not cursor.EOF())
                    'var con = cursor.getItem();
                    'pid = cursor.Item.IDCliente
                    pid = Formats.ToInteger(dbRis("IDCliente"))
                    If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    'cursor.MoveNext()
                End While
                dbRis.Dispose() : dbRis = Nothing
                cursor.Dispose() : cursor = Nothing

                Dim finestre As New CCollection(Of FinestraLavorazioneXML)
                Dim persone As New CCollection(Of CPersonaFisica)

                If (personeIDs.Count > 0) Then
                    Dim arr() As Integer = personeIDs.ToArray()

                    cursor1 = New CPersonaFisicaCursor
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor1.ID.ValueIn(arr)
                    cursor1.PageSize = 1000
                    While Not cursor1.EOF
                        Dim p As CPersonaFisica = cursor1.Item
                        persone.Add(p)

                        Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(p)
                        If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetUltimaFinestraLavorata(p)

                        If (w IsNot Nothing) Then
                            Dim wx As New FinestraLavorazioneXML(w)
                            wx.Prepara()
                            finestre.Add(wx)
                        End If

                        cursor1.MoveNext()
                    End While
                    cursor1.Dispose()
                End If

                Dim ret As New CKeyCollection
                ret.Add("persone", persone)
                ret.Add("finestre", finestre)

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing

            End Try
        End Function

        Public Function GetFinestreContatti(ByVal renderer As Object) As String
            Dim cursor As CCustomerCallsCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim cursor1 As CPersonaFisicaCursor = Nothing

            Try
                Dim di As Date = RPC.n2date(GetParameter(renderer, "di", ""))
                Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))
                Dim arrpo As CCollection = XML.Utils.Serializer.Deserialize(Trim(RPC.n2str(GetParameter(renderer, "arrpo", ""))))

                cursor = New CCustomerCallsCursor
                If (arrpo.Count > 0) Then cursor.IDPuntoOperativo.ValueIn(arrpo.ToArray)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                'cursor.Scopo.ValueIn({"Prima Consulenza", "Consulenza Successiva", "Consulenza"})
                cursor.Data.Between(di, df)
                'cursor.Data.Operator = OP.OP_GE
                'cursor.PageSize = 1000

                Dim personeIDs As New CKeyCollection(Of Integer)
                Dim pid As Integer
                Dim dbSQL As String = "SELECT [IDPersona] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona]"
                cursor.Dispose() : cursor = Nothing

                dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                While dbRis.Read ' (Not cursor.EOF())
                    'var con = cursor.getItem();
                    'pid = cursor.Item.IDPersona
                    pid = Formats.ToInteger(dbRis("IDPersona"))
                    If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    'pid = cursor.Item.IDPerContoDi
                    'If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    ' cursor.MoveNext()
                End While
                dbRis.Dispose() : dbRis = Nothing

                Dim finestre As New CCollection(Of FinestraLavorazioneXML)
                Dim persone As New CCollection(Of CPersonaFisica)

                If (personeIDs.Count > 0) Then
                    Dim arr() As Integer = personeIDs.ToArray()

                    cursor1 = New CPersonaFisicaCursor
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor1.ID.ValueIn(arr)
                    cursor1.PageSize = 1000
                    While Not cursor1.EOF
                        Dim p As CPersonaFisica = cursor1.Item
                        persone.Add(p)

                        Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(p)
                        If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetUltimaFinestraLavorata(p)

                        If (w IsNot Nothing) Then
                            Dim wx As New FinestraLavorazioneXML(w)
                            wx.Prepara()
                            finestre.Add(wx)
                        End If

                        cursor1.MoveNext()
                    End While
                    cursor1.Dispose() : cursor1 = Nothing
                End If

                Dim ret As New CKeyCollection
                ret.Add("persone", persone)
                ret.Add("finestre", finestre)

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing


            End Try
        End Function

        Public Function GetFinestrePassati(ByVal renderer As Object) As String
            Dim cursor As CVisiteCursor = Nothing
            Dim cursor1 As CPersonaFisicaCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim di As Date = RPC.n2date(GetParameter(renderer, "fd", ""))
                Dim df As Date? = RPC.n2date(GetParameter(renderer, "td", ""))

                Dim arrpo As CCollection = XML.Utils.Serializer.Deserialize(Trim(RPC.n2str(GetParameter(renderer, "arrpo", ""))))

                cursor = New CVisiteCursor
                cursor.Ricevuta.Value = True
                If (arrpo.Count > 0) Then cursor.IDPuntoOperativo.ValueIn(arrpo.ToArray)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                'cursor.Scopo.ValueIn({"Prima Consulenza", "Consulenza Successiva", "Consulenza"})
                cursor.Data.Between(di, df)
                'cursor.Data.Operator = OP.OP_GE
                'cursor.PageSize = 1000

                Dim personeIDs As New CKeyCollection(Of Integer)
                Dim pid As Integer
                Dim dbSQL As String = "SELECT [IDPersona] FROM (" & cursor.GetSQL & ") GROUP BY [IDPersona]"
                cursor.Dispose() : cursor = Nothing

                dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                While dbRis.Read ' (Not cursor.EOF())
                    'var con = cursor.getItem();
                    'pid = cursor.Item.IDPersona
                    pid = Formats.ToInteger(dbRis("IDPersona"))
                    If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    'pid = cursor.Item.IDPerContoDi
                    'If pid <> 0 AndAlso Not (personeIDs.ContainsKey("K" & pid)) Then personeIDs.Add("K" & pid, pid)
                    ' cursor.MoveNext()
                End While
                dbRis.Dispose() : dbRis = Nothing


                Dim finestre As New CCollection(Of FinestraLavorazioneXML)
                Dim persone As New CCollection(Of CPersonaFisica)

                If (personeIDs.Count > 0) Then
                    Dim arr() As Integer = personeIDs.ToArray()

                    cursor1 = New CPersonaFisicaCursor
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor1.ID.ValueIn(arr)
                    cursor1.PageSize = arr.Length
                    While Not cursor1.EOF
                        Dim p As CPersonaFisica = cursor1.Item
                        persone.Add(p)

                        Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(p)
                        If (w Is Nothing) Then w = CQSPD.FinestreDiLavorazione.GetUltimaFinestraLavorata(p)

                        If (w IsNot Nothing) Then
                            Dim wx As New FinestraLavorazioneXML(w)
                            wx.Prepara()
                            finestre.Add(wx)
                        End If

                        cursor1.MoveNext()
                    End While
                    cursor1.Dispose() : cursor1 = Nothing

                End If

                Dim ret As New CKeyCollection
                ret.Add("persone", persone)
                ret.Add("finestre", finestre)

                Return XML.Utils.Serializer.Serialize(ret)

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing
            End Try
        End Function

        Public Function GetConfiguration(ByVal renderer As Object) As String
            'If Not Me.CanList(Nothing) Then Throw New PermissionDeniedException(Me.Module, "list")
            Return XML.Utils.Serializer.Serialize(CQSPD.Configuration)
        End Function

        Public Function SaveConfiguration(ByVal renderer As Object) As String
            If Not Me.CanConfigure() Then Throw New PermissionDeniedException(Me.Module, "configure")
            Dim testo As String = RPC.n2str(GetParameter(renderer, "testo", ""))
            Dim c As CCQSPDConfig = XML.Utils.Serializer.Deserialize(testo)
            c.Save(True)
            Return ""
        End Function

        Public Function GetSituazionePersona(ByVal renderer As Object) As String
            Dim ret As New CQSPSituazionePersona '
            ret.IDPersona = RPC.n2int(GetParameter(renderer, "pid", "0"))
            ret.Load()
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetStatoLavorazione(ByVal renderer As Object) As String
            Dim cursor As FinestraLavorazioneCursor = Nothing

            Try
                Dim poID As Integer = RPC.n2int(GetParameter(renderer, "po", ""))
                Dim stato As String = Strings.Trim(RPC.n2str(GetParameter(renderer, "stato", "")))
                'Dim periodo As String = RPC.n2str(GetParameter(renderer, "periodo"))
                Dim di As Date? '= RPC.n2date(GetParameter(renderer, "di"))
                Dim df As Date? '= RPC.n2date(GetParameter(renderer, "df"))

                di = Calendar.DateAdd(DateInterval.Year, -1, Calendar.ToDay)
                df = Calendar.DateAdd(DateInterval.Day, CQSPD.Configuration.GiorniAnticipoRifin, Calendar.ToDay)
                ' If (df.HasValue = False) Then df = Calendar.GetLastMonthDay(Calendar.ToDay)
                ' If (di.HasValue = False) Then di = Calendar.GetPrevMonthFirstDay(Calendar.ToDay)

                'Inseriamo tutte le finestre di lavorazione precedenti
                cursor = New FinestraLavorazioneCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
                cursor.Flags.Value = FinestraLavorazioneFlags.Rinnovo
                cursor.Flags.Operator = OP.OP_ALLBITAND
                'If (di.HasValue) Then
                '    cursor.DataInizioLavorabilita.Value = di.Value
                '    cursor.DataInizioLavorabilita.Operator = OP.OP_GE
                '    If (df.HasValue) Then
                '        cursor.DataInizioLavorabilita.Between(di.Value, df.Value)
                '    End If
                'ElseIf df.HasValue Then
                '    cursor.DataInizioLavorabilita.Value = df.Value
                '    cursor.DataInizioLavorabilita.Operator = OP.OP_LE
                'End If
                cursor.DataInizioLavorabilita.Between(di, df) ' = Calendar.DateAdd(DateInterval.Day, 30, Calendar.ToDay)  'Between(di.Value, df.Value)
                'cursor.DataInizioLavorabilita.Operator = OP.OP_LE
                If (poID <> 0) Then cursor.IDPuntoOperativo.Value = poID

                Dim ret As New CCollection(Of FinestraLavorazioneXML)
                Dim w As FinestraLavorazioneXML
                While Not cursor.EOF
                    w = New FinestraLavorazioneXML(cursor.Item)
                    w.Prepara()
                    ret.Add(w)
                    cursor.MoveNext()
                End While
                cursor.Dispose() : cursor = Nothing

                cursor = New FinestraLavorazioneCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
                While Not cursor.EOF
                    w = New FinestraLavorazioneXML(cursor.Item)
                    w.Prepara()
                    ret.Add(w)
                    cursor.MoveNext()
                End While
                cursor.Dispose()

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function GetFinanziamentiRinnovabili(ByVal renderer As Object) As String
            Dim cursor As CRicontattiCursor = Nothing

            Try
                Dim poID As Integer = RPC.n2int(GetParameter(renderer, "po", ""))
                Dim opID As Integer = RPC.n2int(GetParameter(renderer, "op", ""))
                Dim periodo As String = RPC.n2str(GetParameter(renderer, "periodo", ""))
                Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
                Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))
                Dim ret As New System.Collections.ArrayList

                cursor = New CRicontattiCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
                cursor.DataPrevista.Between(Calendar.DateAdd(DateInterval.Day, 30, Now), Calendar.DateAdd(DateInterval.Day, -30, Now))
                cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC

                cursor.WhereClauses.Add("([NomeLista]='Altri Prestiti Rinnovabili' Or [NomeLista]='Rinnovi CQS/DEL')")
                If (poID <> 0) Then cursor.IDPuntoOperativo.Value = poID
                If (opID <> 0) Then cursor.IDAssegnatoA.Value = opID

                'Dim interval As CIntervalloData = Calendar.PeriodoToDates(periodo, di, df)
                'di = interval.Inizio
                'df = interval.Fine
                'If (di.HasValue AndAlso df.HasValue AndAlso di.Value = df.Value) Then df = Calendar.DateAdd(DateInterval.Second, 24 * 3600 - 1, di.Value)

                'If (di.HasValue) Then
                '    cursor.DataPrevista.Value = di.Value
                '    cursor.DataPrevista.Operator = OP.OP_GE
                '    If (df.HasValue) Then
                '        cursor.DataPrevista.Value1 = df.Value
                '        cursor.DataPrevista.Operator = OP.OP_BETWEEN
                '    End If
                'ElseIf (df.HasValue) Then
                '    cursor.DataPrevista.Value = df.Value
                '    cursor.DataPrevista.Operator = OP.OP_LE
                'End If

                While Not cursor.EOF
                    Dim ric As CRicontatto = cursor.Item
                    Dim persona As CPersona = ric.Persona
                    If (persona IsNot Nothing AndAlso persona.Stato = ObjectStatus.OBJECT_VALID AndAlso persona.TipoPersona = TipoPersona.PERSONA_FISICA AndAlso persona.Deceduto = False) Then
                        Dim info As New CActivePerson(ric)
                        'With DirectCast(r.Person, CPersonaFisica)
                        '    r.MoreInfo.Add("Nato A:", .NatoA.NomeComune & " il " & Formats.FormatUserDate(.DataNascita))
                        '    If .ImpiegoPrincipale IsNot Nothing Then
                        '        r.MoreInfo.Add("Impiego", FormatImpiego(.ImpiegoPrincipale))
                        '    End If
                        'End With
                        ret.Add(info)
                    End If
                    cursor.MoveNext()
                End While
                cursor.Dispose() : cursor = Nothing

                If (ret.Count > 0) Then
                    Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
                Else
                    Return ""
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function GetRichiestaPendenti(ByVal renderer As Object) As String
            'Dim ret As New System.Collections.ArrayList
            Dim poID As Integer = RPC.n2int(GetParameter(renderer, "po", ""))
            Dim opID As Integer = RPC.n2int(GetParameter(renderer, "op", ""))
            Dim periodo As String = RPC.n2str(GetParameter(renderer, "periodo", ""))
            Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))

            Dim interval As CIntervalloData = Calendar.PeriodoToDates(periodo, di, df)
            di = interval.Inizio
            df = interval.Fine
            If (df.HasValue) Then df = Calendar.DateAdd(DateInterval.Second, 24 * 3600 - 1, Calendar.GetDatePart(df.Value))

            Dim ufficio As CUfficio = Anagrafica.Uffici.GetItemById(poID)
            Dim operatore As CUser = Sistema.Users.GetItemById(opID)
            Dim ret As CCollection(Of CRichiestaFinanziamento) = CQSPD.RichiesteFinanziamento.GetRichiestePendenti(ufficio, operatore, di, df)



            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetConsulenzeInCorso(ByVal renderer As Object) As String
            Dim t1 As Double = Timer

            Dim poID As Integer = RPC.n2int(GetParameter(renderer, "po", ""))
            Dim opID As Integer = RPC.n2int(GetParameter(renderer, "op", ""))
            Dim periodo As String = RPC.n2str(GetParameter(renderer, "periodo", ""))
            Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))

            Dim interval As CIntervalloData = Calendar.PeriodoToDates(periodo, di, df)
            di = interval.Inizio
            df = interval.Fine
            If (df.HasValue) Then df = Calendar.DateAdd(DateInterval.Second, 24 * 3600 - 1, Calendar.GetDatePart(df.Value))


            Dim ret As CCollection(Of CQSPDConsulenza)
            Dim ufficio As CUfficio = Anagrafica.Uffici.GetItemById(poID)
            Dim operatore As CUser = Sistema.Users.GetItemById(opID)

            ret = CQSPD.Consulenze.GetConsulenzeInCorso(ufficio, operatore, di, df)


            Debug.Print("GetConsulenzeInCorso: " & (Timer - t1) & " s")
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetSecci(ByVal renderer As Object) As String

            Dim poID As Integer = RPC.n2int(GetParameter(renderer, "po", ""))
            Dim opID As Integer = RPC.n2int(GetParameter(renderer, "op", ""))
            Dim periodo As String = RPC.n2str(GetParameter(renderer, "periodo", ""))
            Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))

            Dim interval As CIntervalloData = Calendar.PeriodoToDates(periodo, di, df)
            di = interval.Inizio
            df = interval.Fine
            If (di.HasValue AndAlso df.HasValue AndAlso di.Value = df.Value) Then df = Calendar.DateAdd(DateInterval.Second, 24 * 3600 - 1, di.Value)

            Dim ufficio As CUfficio = Anagrafica.Uffici.GetItemById(poID)
            Dim operatore As CUser = Sistema.Users.GetItemById(opID)

            Dim ret As CCollection(Of CRapportino) = CQSPD.Pratiche.GetSecci(ufficio, operatore, di, df)



            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Function CreateElencoOperatori(ByVal renderer As Object) As String
            Dim ov As Boolean = RPC.n2bool(GetParameter(renderer, "ov", ""))
            Dim selID As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "id", ""))
            Return CQSPDUtils.CreateElencoOperatori(Users.GetItemById(selID), ov)
        End Function

        Public Function GetFastStats(ByVal renderer As Object) As String
            Dim strFilter As String = RPC.n2str(GetParameter(renderer, "filter", ""))
            Dim filter As CQSFilter = XML.Utils.Serializer.Deserialize(strFilter)
            Dim stats As CQSFastStats = CQSPD.Pratiche.GetFastStats(filter)
            Return XML.Utils.Serializer.Serialize(stats)
        End Function

        Public Function GetStatisticheLiquidato(ByVal renderer As Object) As String
            If Not CQSPD.Module.UserCanDoAction("statistiche_liquidato") Then Throw New PermissionDeniedException(CQSPD.Module, "statistiche_liquidato")
            Dim filter As CQSPD.LiquidatoFilter = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
            Dim stats As New CQSPD.LiquidatoStats
            stats.Apply(filter)

            'Grafico
            Dim imgWidth As Integer = Math.Ceiling((filter.ChartWidth - 30) / 2)
            Dim imgHeight As Integer = Math.Ceiling((filter.ChartHeight - 10) / 2)

            Dim chart As New CChart
            chart.Type = ChartTypes.Lines
            chart.Title.Text = "Liquidato Mensile (Montante Lordo)"

            For i As Integer = 1 To 12
                chart.Labels.Add(Calendar.GetShortMonthName(i))
            Next

            'Dim obiettivo As CObiettivoPratica
            'Dim item As LiquidatoStatsItem
            'If (stats.items.Count > 0) Then
            '    For i As Integer = 0 To filter.Anni.Count - 1
            '        Dim serie As CChartSerie = chart.Series.Add(filter.Anni(i))
            '        For j As Integer = 1 To 12
            '            item = stats.items.GetItemByKey("K" & filter.Anni(i) & "_" & j)
            '            If (item Is Nothing) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(item.LiquidatoSum)
            '            End If
            '        Next

            '        serie = chart.Series.Add("Obiettivo " & filter.Anni(i))
            '        For j As Integer = 1 To 12
            '            obiettivo = Me.GetObiettivo(arrPO, filter.Anni(i), j)
            '            If (obiettivo.MontanteLordoLiq.HasValue = False) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(obiettivo.MontanteLordoLiq.Value)
            '            End If
            '        Next
            '    Next
            'End If



            'chart.Grid.ShowYGrid = True
            'chart.Grid.ShowXGrid = True
            'chart.XAxe.ShowValues = True
            'chart.YAxe.ShowValues = True

            'Dim renderer As New LineChartRenderer(chart)
            'Dim fileName As String = WebSite.Instance.Configuration.TempFolder & ASPSecurity.GetRandomKey(8) & ".jpg"
            'Dim img As New System.Drawing.Bitmap(w, h)
            'Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(img)
            'g.PageUnit = Drawing.GraphicsUnit.Pixel
            'renderer.PaintTo(g, New System.Drawing.Rectangle(5, 5, img.Width - 10, img.Height - 20))
            'g.Dispose()
            'img.Save(Sistema.ApplicationContext.MapPath(fileName), System.Drawing.Imaging.ImageFormat.Jpeg)

            'html &= "<a href=""#"" onclick=""_" & Me.Name & ".ShowChart('ML'); return false;""><img id=""" & Me.Name & "imgML"" src=""" & Strings.URLEncode(fileName) & """ border=""0"" width=""" & imgWidth & "px"" height=""" & imgHeight & "px"" /></a>"
            'img.Dispose()
            'html &= "</td>"

            ''------------------

            'html &= "<td>"

            'chart = New CChart
            'chart.Type = ChartTypes.Lines
            'chart.Title.Text = "Liquidato Mensile (Numero Pratiche)"

            'For i As Integer = 1 To 12
            '    chart.Labels.Add(Calendar.GetShortMonthName(i))
            'Next

            'If (items.Count > 0) Then
            '    For i As Integer = 0 To UBound(arrAnni)
            '        Dim serie As CChartSerie = chart.Series.Add(arrAnni(i))
            '        For j As Integer = 1 To 12
            '            item = items.GetItemByKey("K" & arrAnni(i) & "_" & j)
            '            If (item Is Nothing) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(item.LiquidatoCnt)
            '            End If
            '        Next

            '        serie = chart.Series.Add("Obiettivo " & arrAnni(i))
            '        For j As Integer = 1 To 12
            '            obiettivo = Me.GetObiettivo(arrPO, arrAnni(i), j)
            '            If (obiettivo.NumeroPraticheLiq.HasValue = False) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(obiettivo.NumeroPraticheLiq.Value)
            '            End If
            '        Next
            '    Next
            'End If
            'chart.Grid.ShowYGrid = True
            'chart.Grid.ShowXGrid = True
            'chart.XAxe.ShowValues = True
            'chart.YAxe.ShowValues = True

            'renderer = New LineChartRenderer(chart)
            'fileName = WebSite.Instance.Configuration.TempFolder & ASPSecurity.GetRandomKey(8) & ".jpg"
            'img = New System.Drawing.Bitmap(w, h)
            'g = System.Drawing.Graphics.FromImage(img)
            'g.PageUnit = Drawing.GraphicsUnit.Pixel
            'renderer.PaintTo(g, New System.Drawing.Rectangle(5, 5, img.Width - 10, img.Height - 20))
            'g.Dispose()
            'img.Save(Sistema.ApplicationContext.MapPath(fileName), System.Drawing.Imaging.ImageFormat.Jpeg)
            'html &= "<a href=""#"" onclick=""_" & Me.Name & ".ShowChart('NP'); return false;""><img id=""" & Me.Name & "imgNP""  src=""" & Strings.URLEncode(fileName) & """ border=""0"" width=""" & imgWidth & "px"" height=""" & imgHeight & "px"" /></a>"
            'img.Dispose()
            'html &= "</td>"
            'html &= "</tr>"

            ''------------------
            'html &= "<tr>"
            'html &= "<td>"

            'chart = New CChart
            'chart.Type = ChartTypes.Lines
            'chart.Title.Text = "UpFront (%)"

            'For i As Integer = 1 To 12
            '    chart.Labels.Add(Calendar.GetShortMonthName(i))
            'Next

            'If (items.Count > 0) Then
            '    For i As Integer = 0 To UBound(arrAnni)
            '        Dim serie As CChartSerie = chart.Series.Add(arrAnni(i))
            '        For j As Integer = 1 To 12
            '            item = items.GetItemByKey("K" & arrAnni(i) & "_" & j)
            '            If (item Is Nothing) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(Me.GetPercentage(item.LiquidatoUpfront, item.LiquidatoSum))
            '            End If
            '        Next

            '        serie = chart.Series.Add("Obiettivo " & arrAnni(i))
            '        For j As Integer = 1 To 12
            '            obiettivo = Me.GetObiettivo(arrPO, arrAnni(i), j)
            '            If (obiettivo.UpFrontLiq.HasValue = False) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(obiettivo.UpFrontLiq.Value)
            '            End If
            '        Next
            '    Next
            'End If
            'chart.Grid.ShowYGrid = True
            'chart.Grid.ShowXGrid = True
            'chart.XAxe.ShowValues = True
            'chart.YAxe.ShowValues = True

            'renderer = New LineChartRenderer(chart)
            'fileName = WebSite.Instance.Configuration.TempFolder & ASPSecurity.GetRandomKey(8) & ".jpg"
            'img = New System.Drawing.Bitmap(w, h)
            'g = System.Drawing.Graphics.FromImage(img)
            'g.PageUnit = Drawing.GraphicsUnit.Pixel
            'renderer.PaintTo(g, New System.Drawing.Rectangle(5, 5, img.Width - 10, img.Height - 20))
            'g.Dispose()
            'img.Save(Sistema.ApplicationContext.MapPath(fileName), System.Drawing.Imaging.ImageFormat.Jpeg)
            'html &= "<a href=""#"" onclick=""_" & Me.Name & ".ShowChart('UF'); return false;""><img id=""" & Me.Name & "imgUF"" src=""" & Strings.URLEncode(fileName) & """ border=""0"" width=""" & imgWidth & "px"" height=""" & imgHeight & "px"" /></a>"
            'img.Dispose()
            'html &= "</td>"
            ''--------------
            'html &= "<td>"

            'chart = New CChart
            'chart.Type = ChartTypes.Lines
            'chart.Title.Text = "UpFront (€)"

            'For i As Integer = 1 To 12
            '    chart.Labels.Add(Calendar.GetShortMonthName(i))
            'Next

            'If (items.Count > 0) Then
            '    For i As Integer = 0 To UBound(arrAnni)
            '        Dim serie As CChartSerie = chart.Series.Add(arrAnni(i))
            '        For j As Integer = 1 To 12
            '            item = items.GetItemByKey("K" & arrAnni(i) & "_" & j)
            '            If (item Is Nothing) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(item.LiquidatoUpfront)
            '            End If
            '        Next

            '        serie = chart.Series.Add("Obiettivo " & arrAnni(i))
            '        For j As Integer = 1 To 12
            '            obiettivo = Me.GetObiettivo(arrPO, arrAnni(i), j)
            '            If (obiettivo.ValoreSpreadLiq.HasValue = False) Then
            '                serie.Values.Add(0)
            '            Else
            '                serie.Values.Add(obiettivo.ValoreSpreadLiq.Value)
            '            End If
            '        Next
            '    Next
            'End If
            'chart.Grid.ShowYGrid = True
            'chart.Grid.ShowXGrid = True
            'chart.XAxe.ShowValues = True
            'chart.YAxe.ShowValues = True

            'renderer = New LineChartRenderer(chart)
            'fileName = WebSite.Instance.Configuration.TempFolder & ASPSecurity.GetRandomKey(8) & ".jpg"
            'img = New System.Drawing.Bitmap(w, h)
            'g = System.Drawing.Graphics.FromImage(img)
            'g.PageUnit = Drawing.GraphicsUnit.Pixel
            'renderer.PaintTo(g, New System.Drawing.Rectangle(5, 5, img.Width - 10, img.Height - 20))
            'g.Dispose()
            'img.Save(Sistema.ApplicationContext.MapPath(fileName), System.Drawing.Imaging.ImageFormat.Jpeg)
            'html &= "<a href=""#"" onclick=""_" & Me.Name & ".ShowChart('VUF'); return false;""><img id=""" & Me.Name & "imgVUF"" src=""" & Strings.URLEncode(fileName) & """ border=""0"" width=""" & imgWidth & "px"" height=""" & imgHeight & "px"" /></a>"
            'img.Dispose()

            'html &= "</td>"
            'html &= "</tr>"
            'html &= "</table>"


            'html &= "</div>"

            Return XML.Utils.Serializer.Serialize(stats)
        End Function


    End Class

End Namespace