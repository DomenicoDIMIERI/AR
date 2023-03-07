Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    Public NotInheritable Class CStudiDiFattibilitaClass
        Inherits CGeneralClass(Of CQSPDStudioDiFattibilita)


        Friend Sub New()
            MyBase.New("modGruppiConsulenzeCQS", GetType(CQSPDStudiDiFattibilitaCursor))
        End Sub


        Public Function GetUltimoStudioDiFattibilita(ByVal persona As CPersonaFisica) As CQSPDStudioDiFattibilita
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Return GetUltimoStudioDiFattibilita(GetID(persona))
        End Function

        Public Function GetAnomalie(ByVal idUfficio As Integer, ByVal idOperatore As Integer, ByVal dal As Nullable(Of Date), ByVal al As Nullable(Of Date), Optional ByVal ritardoConsentito As Integer = 1) As CCollection(Of OggettoAnomalo)
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim consulenza As CQSPDConsulenza
            Dim dbSQL As String
            Dim ret As New CCollection(Of OggettoAnomalo)

#If Not Debug Then
        Try
#End If
            dbSQL = ""
            dbSQL &= "SELECT * FROM [tbl_CQSPDConsulenze] LEFT JOIN ("
            dbSQL &= "SELECT [ID] As [IDPrat], [IDConsulenza] FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDAzienda]=" & GetID(Anagrafica.Aziende.AziendaPrincipale )
            dbSQL &= ") As [T1] ON [tbl_CQSPDConsulenze].[ID]=[T1].[IDConsulenza]"
            dbSQL &= " WHERE [T1].[IDPrat] Is Null And [tbl_CQSPDConsulenze].[Stato] = " & ObjectStatus.OBJECT_VALID
            If (idUfficio <> 0) Then dbSQL &= " AND [tbl_CQSPDConsulenze].[IDPuntoOperativo] = " & DBUtils.DBNumber(idUfficio)
            If (idOperatore <> 0) Then dbSQL &= " AND [tbl_CQSPDConsulenze].[IDConfermataDa] = " & DBUtils.DBNumber(idOperatore)
            If (dal.HasValue) Then dbSQL &= " AND [tbl_CQSPDConsulenze].[DataConsulenza] >= " & DBUtils.DBDate(dal.Value)
            If (al.HasValue) Then dbSQL &= " AND [tbl_CQSPDConsulenze].[DataConsulenza] <= " & DBUtils.DBDate(al.Value)
            dbSQL &= " And [tbl_CQSPDConsulenze].[IDProduttore] = " & GetID(Anagrafica.Aziende.AziendaPrincipale ) & " And [tbl_CQSPDConsulenze].[StatoConsulenza] = " & StatiConsulenza.ACCETTATA

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                consulenza = New CQSPDConsulenza
                CQSPD.Database.Load(consulenza, dbRis)

                Dim oggetto As New OggettoAnomalo
                oggetto.Oggetto = consulenza
                'oggetto.Operatore = consulenza.ConfermataDa

                Dim d1 As Date = consulenza.CreatoIl
                If (consulenza.DataConferma.HasValue) Then d1 = consulenza.DataConferma.Value
                Dim ritardo As Integer = DateUtils.DateDiff(DateInterval.Day, d1, Now)
                oggetto.AggiungiAnomalia("La proposta è stata accettata da " & ritardo & " e non è stata ancora generato il secci", 0)
                ret.Add(oggetto)
            End While

#If Not Debug Then
            catch ex as Exception
            throw
            finally
#End If
            If (dbRis IsNot Nothing) Then dbRis.Dispose()
            dbRis = Nothing
#If Not Debug Then
            end try
#End If
            

            Return ret
        End Function

        Public Function GetUltimoStudioDiFattibilita(ByVal idPersona As Integer) As CQSPDStudioDiFattibilita
            If (idPersona = 0) Then Return Nothing
            Dim cursor As New CQSPDStudiDiFattibilitaCursor
            Dim ret As CQSPDStudioDiFattibilita = Nothing
            Try
                cursor.PageSize = 1
                cursor.IDCliente.Value = idPersona
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                ret = cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
            Return ret
        End Function

        Public Function GetStudiDiFattibilitaByPersona(ByVal persona As CPersona) As CCollection(Of CQSPDStudioDiFattibilita)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim ret As New CCollection(Of CQSPDStudioDiFattibilita)
            If (GetID(persona) = 0) Then Return ret
            Dim cursor As New CQSPDStudiDiFattibilitaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDCliente.Value = GetID(persona)
            cursor.Data.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Dim sf As CQSPDStudioDiFattibilita = cursor.Item
                sf.SetCliente(persona)
                ret.Add(sf)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function GetStudiDiFattibilitaByPersona(ByVal idPersona As Integer) As CCollection(Of CQSPDStudioDiFattibilita)
            Return Me.GetStudiDiFattibilitaByPersona(Anagrafica.Persone.GetItemById(idPersona))
        End Function

        Public Function GetStudiDiFattibilitaByRichiesta(ByVal richiesta As CRichiestaFinanziamento) As CCollection(Of CQSPDStudioDiFattibilita)
            If (richiesta Is Nothing) Then Throw New ArgumentNullException("richiesta")
            Return GetStudiDiFattibilitaByPersona(GetID(richiesta))
        End Function

        Public Function GetStudiDiFattibilitaByRichiesta(ByVal idRichiesta As Integer) As CCollection(Of CQSPDStudioDiFattibilita)
            Dim ret As New CCollection(Of CQSPDStudioDiFattibilita)
            If (idRichiesta <> 0) Then
                Dim cursor As New CQSPDStudiDiFattibilitaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDRichiesta.Value = idRichiesta
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If
            Return ret
        End Function

        Public Function ParseTemplate(ByVal template As String, ByVal consulenza As CQSPDStudioDiFattibilita, ByVal baseURL As String) As String
            Dim ret As String = template
            ret = Replace(ret, "%%NOMECONSULENTE%%", consulenza.NomeConsulente)
            ret = Replace(ret, "%%NOMECLIENTE%%", consulenza.NomeCliente)
            ret = Replace(ret, "%%ID%%", GetID(consulenza))
            ret = Replace(ret, "%%BASEURL%%", ApplicationContext.BaseURL)
            Return ret
        End Function

        Public Sub AggiornaPratiche(ByVal idConsulenza As Integer)
            If (idConsulenza = 0) Then Exit Sub
            Me.AggiornaPratiche(CQSPD.Consulenze.GetItemById(idConsulenza))
        End Sub

        Public Sub AggiornaPratiche(ByVal consulenza As CQSPDConsulenza)
            If consulenza Is Nothing Then Throw New ArgumentNullException("consulenza")
            If consulenza.StudioDiFattibilita IsNot Nothing Then Me.AggiornaPratiche(consulenza.StudioDiFattibilita)
        End Sub

        ''' <summary>
        ''' Aggiorna la tabella delle statistiche relative alla pratiche generate da questo studio di fattibilità
        ''' </summary>
        ''' <param name="studiof"></param>
        ''' <remarks></remarks>
        Public Sub AggiornaPratiche(ByVal studiof As CQSPDStudioDiFattibilita)
            SyncLock Me
                If studiof Is Nothing Then Throw New ArgumentNullException("studiof")
                Dim cnt As Integer = 0
                Dim cntOk As Integer = 0
                Dim cntNo As Integer = 0
                Dim stLiquidata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
                Dim stAnnullata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

                For Each consulenza As CQSPDConsulenza In studiof.Proposte
                    cnt += Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [IDConsulenza]=" & GetID(consulenza) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID))
                    cntOk += Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [IDConsulenza]=" & GetID(consulenza) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDStatoAttuale]=" & GetID(stLiquidata)))
                    cntNo += Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [IDConsulenza]=" & GetID(consulenza) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDStatoAttuale]=" & GetID(stAnnullata)))
                Next

                CQSPD.Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpConsulenze] Set [CntPratiche]=" & cnt & ", [CntPraticheOk]=" & cntOk & ", [CntPraticheNo]=" & cntNo & " WHERE [ID]=" & GetID(studiof))
            End SyncLock
        End Sub


    End Class

    Private Shared m_StudiDiFattibilita As CStudiDiFattibilitaClass = Nothing

    Public Shared ReadOnly Property StudiDiFattibilita As CStudiDiFattibilitaClass
        Get
            If (m_StudiDiFattibilita Is Nothing) Then m_StudiDiFattibilita = New CStudiDiFattibilitaClass
            Return m_StudiDiFattibilita
        End Get
    End Property


End Class
