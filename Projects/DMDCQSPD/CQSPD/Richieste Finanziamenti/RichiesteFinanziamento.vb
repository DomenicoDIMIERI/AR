Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public NotInheritable Class CRichiesteFinanziamentoClass
        Inherits CGeneralClass(Of CRichiestaFinanziamento)

        Public Event NuovaRichiesta(ByVal sender As Object, ByVal e As ItemEventArgs)

        Public Event RichiestaModificata(ByVal sender As Object, ByVal e As ItemEventArgs)

        Public Event RichiestaEliminata(ByVal sender As Object, ByVal e As ItemEventArgs)



        Friend Sub New()
            MyBase.New("modCQSPDRichieste", GetType(CRichiesteFinanziamentoCursor))
        End Sub

        Protected Friend Sub doNuovaRichiesta(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)

            Dim richiesta As CRichiestaFinanziamento = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp

            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If

            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.isClienteInAcquisizione = True
            If richiesta.AssegnatoA Is Nothing Then info.AggiungiAttenzione(richiesta, "Richiesta Finanziamento non ancora presa in carico", "Richiesta Fin " & GetID(richiesta) & " In Carico")
            If richiesta.IDModulo = 0 Then info.AggiungiAttenzione(richiesta, "Modulo Richiesta Finanziamento non caricato", "Richiesta Fin " & GetID(richiesta) & " modulo richiesta")
            If richiesta.IDPrivacy = 0 Then info.AggiungiAttenzione(richiesta, "Modulo Privacy non caricato", "Richiesta Fin " & GetID(richiesta) & " modulo privacy")
            info.AggiornaOperazione(richiesta, "Nuova Richiesta di Finanziamento")

            RaiseEvent NuovaRichiesta(Me, e)
            Me.Module.DispatchEvent(New EventDescription("Create", "Richiesta di Finanziamento Inserita: " & richiesta.NomeCliente, richiesta))
        End Sub

        Protected Friend Sub doRichiestaModificata(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)

            Dim richiesta As CRichiestaFinanziamento = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            'If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
            '    cliente.SetFlag(PFlags.Cliente, True)
            '    cliente.Save()
            'End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.isClienteInAcquisizione = True

            If richiesta.AssegnatoA Is Nothing Then
                info.AggiungiAttenzione(richiesta, "Richiesta Finanziamento non ancora presa in carico", "Richiesta Fin " & GetID(richiesta) & " In Carico")
            Else
                info.RimuoviAttenzione(richiesta, "Richiesta Fin " & GetID(richiesta) & " In Carico")
            End If
            If richiesta.IDModulo = 0 Then
                info.AggiungiAttenzione(richiesta, "Modulo Richiesta Finanziamento non caricato", "Richiesta Fin " & GetID(richiesta) & " modulo richiesta")
            Else
                info.RimuoviAttenzione(richiesta, "Richiesta Fin " & GetID(richiesta) & " modulo richiesta")
            End If
            If richiesta.IDPrivacy = 0 Then
                info.AggiungiAttenzione(richiesta, "Modulo Privacy non caricato", "Richiesta Fin " & GetID(richiesta) & " modulo privacy")
            Else
                info.RimuoviAttenzione(richiesta, "Richiesta Fin " & GetID(richiesta) & " modulo privacy")
            End If
            info.Save()

            RaiseEvent RichiestaModificata(Me, e)
            Me.Module.DispatchEvent(New EventDescription("Edit", "Richiesta di Finanziamento Modificata: " & richiesta.NomeCliente, richiesta))
        End Sub

        Protected Friend Sub doRichiestaEliminata(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)

            Dim richiesta As CRichiestaFinanziamento = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.RimuoviAttenzione(richiesta, "Richiesta Fin " & GetID(richiesta) & " In Carico")
            info.RimuoviAttenzione(richiesta, "Richiesta Fin " & GetID(richiesta) & " modulo richiesta")
            info.RimuoviAttenzione(richiesta, "Richiesta Fin " & GetID(richiesta) & " modulo privacy")
            info.Save()

            RaiseEvent RichiestaEliminata(Me, e)
            Me.Module.DispatchEvent(New EventDescription("Delete", "Richiesta di Finanziamento Eliminata: " & richiesta.NomeCliente, richiesta))
        End Sub

        ''' <summary>
        ''' Restituice un l'elenco delle richieste di finanziamento registrate con qualche anomalia (es. mancanti di documentazione o inserite oltre un certo tempo senza che sia stato effettuato uno studio di fattibilità per la richiesta)
        ''' </summary>
        ''' <param name="idUfficio">[in] ID dell'ufficio a cui limitare la ricerca. Se 0 la ricerca procederà su tutti gli uffici</param>
        ''' <param name="idOperatore">[in] ID dell'operatore a cui limitare la ricerca. Se 0 la ricerca procederà su tutti gli operatori</param>
        ''' <param name="dal">[in] Data da cui far partire la ricerca</param>
        ''' <param name="al">[in] Data a cui terminare la ricerca</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAnomalie(ByVal idUfficio As Integer, ByVal idOperatore As Integer, ByVal dal As Nullable(Of Date), ByVal al As Nullable(Of Date), Optional ByVal ritardoConsentito As Integer = 1) As CCollection(Of OggettoAnomalo)
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim dbSQL As String = ""
            Dim ret As New CCollection(Of OggettoAnomalo)
            Dim oggetto As OggettoAnomalo
            Dim richiesta As CRichiestaFinanziamento

#If Not Debug Then
            try
#End If

            dbSQL &= "SELECT * FROM [tbl_RichiesteFinanziamenti] LEFT JOIN ("
            dbSQL &= "SELECT [ID] As [IDPrat], [IDRichiesta] FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDAzienda]=" & GetID(Anagrafica.Aziende.AziendaPrincipale)
            dbSQL &= ") AS [T1] ON [T1].[IDRichiesta]=[tbl_RichiesteFinanziamenti].[ID] "
            dbSQL &= " WHERE [tbl_RichiesteFinanziamenti].[Stato]=" & ObjectStatus.OBJECT_VALID
            If (dal.HasValue) Then dbSQL &= " AND [tbl_RichiesteFinanziamenti].[Data]>=" & DBUtils.DBDate(dal.Value)
            If (al.HasValue) Then dbSQL &= " AND [tbl_RichiesteFinanziamenti].[Data]<=" & DBUtils.DBDate(al.Value)
            If (idUfficio <> 0) Then dbSQL &= " AND [tbl_RichiesteFinanziamenti].[IDPuntoOperativo]=" & DBUtils.DBNumber(idUfficio)
            If (idOperatore <> 0) Then dbSQL &= " AND [tbl_RichiesteFinanziamenti].[IDPresaInCaricoDa]=" & DBUtils.DBNumber(idOperatore)
            dbSQL &= " AND [T1].[IDPrat] Is Null "

            'dbSQL &= "UNION "
            'dbSQL &= "SELECT *, Null As [IDRichiesta], Null As [IDPrat] FROM [tbl_RichiesteFinanziamenti] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [Data]>=" & DBUtils.DBDate(DateUtils.DateAdd(DateInterval.Year, -1, DateUtils.ToDay)) & " AND ([IDPrivacy]=0 Or [IDPrivacy] Is Null Or [IDModulo] = 0 Or [IDModulo] Is Null)"

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                richiesta = New CRichiestaFinanziamento
                CQSPD.Database.Load(richiesta, dbRis)
                Dim ritardo As Integer = DateUtils.DateDiff(DateInterval.Day, richiesta.Data, Now)
                If ritardo > ritardoConsentito OrElse richiesta.IDModulo = 0 OrElse richiesta.IDPrivacy = 0 Then
                    oggetto = New OggettoAnomalo
                    oggetto.Oggetto = richiesta
                    'oggetto.Gruppo = richiesta.PresaInCaricoDa
                    'If (oggetto.Operatore Is Nothing) Then oggetto.Operatore = richiesta.CreatoDa
                    If (ritardo > ritardoConsentito) Then oggetto.AggiungiAnomalia("La richiesta è stata inserita da " & ritardo & " giorni", 0)
                    If (richiesta.IDModulo = 0) Then oggetto.AggiungiAnomalia("Non è stato caricato il modulo di richiesta di finanziamento", 1)
                    If (richiesta.IDPrivacy = 0) Then oggetto.AggiungiAnomalia("Non è stata caricata la firma della privacy", 1)
                    ret.Add(oggetto)
                End If
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
         
        ''' <summary>
        ''' Restituisce l'ultima richiesta di finanziamento fatta dal cliente specificato
        ''' </summary>
        ''' <param name="pid">[ID] del cliente</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUltimaRichiesta(ByVal pid As Integer) As CRichiestaFinanziamento
            If (pid = 0) Then Return Nothing
            Dim cursor As New CRichiesteFinanziamentoCursor()
            Dim ret As CRichiestaFinanziamento = Nothing
            Try
                cursor.IDCliente.Value = pid
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                cursor.PageSize = 1
                cursor.IgnoreRights = True
                ret = cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
            Return ret
        End Function

        Public Function GetRichiesteByPersona(ByVal idPersona As Integer) As CCollection(Of CRichiestaFinanziamento)
            Dim ret As New CCollection(Of CRichiestaFinanziamento)
            If (idPersona <> 0) Then
                Return Me.GetRichiesteByPersona(Anagrafica.Persone.GetItemById(idPersona))
            End If
            Return ret
        End Function

        Public Function GetRichiesteByPersona(ByVal persona As CPersona) As CCollection(Of CRichiestaFinanziamento)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim ret As New CCollection(Of CRichiestaFinanziamento)
            If (GetID(persona) = 0) Then Return ret
            Dim cursor As New CRichiesteFinanziamentoCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDCliente.Value = GetID(persona)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Dim rich As CRichiestaFinanziamento = cursor.Item
                rich.SetCliente(persona)
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret
        End Function

        Public Function ParseTemplate(ByVal template As String, ByVal richiesta As CRichiestaFinanziamento, ByVal baseURL As String) As String
            Dim ret As String = template
            ret = Replace(ret, "%%USERNAME%%", Users.CurrentUser.UserName)
            ret = Replace(ret, "%%NOMEASSEGNATOA%%", richiesta.NomeAssegnatoA)
            ret = Replace(ret, "%%NOMEPUNTOOPERATIVO%%", richiesta.NomePuntoOperativo)
            ret = Replace(ret, "%%NOTE%%", richiesta.Note)
            ret = Replace(ret, "%%DATA%%", richiesta.Data)
            ret = Me.ParseNib(ret, "%%IMPORTORICHIESTO1%%", richiesta.ImportoRichiesto)
            ret = Me.ParseNib(ret, "%%IMPORTORICHIESTO2%%", richiesta.ImportoRichiesto1)
            ret = Me.ParseNib(ret, "%%RATAMASSIMA%%", richiesta.RataMassima)
            ret = Me.ParseNib(ret, "%%DURATAMASSIMA%%", richiesta.DurataMassima)
            If (richiesta.Privacy Is Nothing) Then
                ret = Replace(ret, "%%URLPRIVACY%%", "")
            Else
                ret = Replace(ret, "%%URLPRIVACY%%", richiesta.Privacy.URL)
            End If
            ret = Replace(ret, "%%NOMECLIENTE%%", richiesta.NomeCliente)
            ret = Replace(ret, "%%IDCLIENTE%%", richiesta.IDCliente)
            ret = Replace(ret, "%%ID%%", GetID(richiesta))
            ret = Replace(ret, "%%BASEURL%%", ApplicationContext.BaseURL)
            Return ret
        End Function

        Private Function ParseNib(ByVal text As String, ByVal nib As String, ByVal value As Nullable(Of Decimal)) As String
            If (value.HasValue) Then
                Return Replace(text, nib, Formats.FormatValuta(value.Value))
            Else
                Return Replace(text, nib, "")
            End If
        End Function

        Private Function ParseNib(ByVal text As String, ByVal nib As String, ByVal value As Nullable(Of Integer)) As String
            If (value.HasValue) Then
                Return Replace(text, nib, Formats.FormatInteger(value.Value))
            Else
                Return Replace(text, nib, "")
            End If
        End Function

        Public Function GetRichiestePendenti(ufficio As CUfficio, operatore As CUser, di As Date?, df As Date?) As CCollection(Of CRichiestaFinanziamento)
            Dim ret As New CCollection(Of CRichiestaFinanziamento)

            Dim dbSQL As String = "SELECT tbl_RichiesteFinanziamenti.*, tbl_CQSPDConsulenze.IDRichiesta As IDRICQ FROM tbl_RichiesteFinanziamenti LEFT JOIN tbl_CQSPDConsulenze ON tbl_RichiesteFinanziamenti.ID=tbl_CQSPDConsulenze.IDRichiesta"
            dbSQL = "SELECT * FROM (" & dbSQL & ") WHERE [IDRICQ] Is Null And [Stato]=" & ObjectStatus.OBJECT_VALID
            If (ufficio IsNot Nothing) Then
                dbSQL &= " AND [IDPuntoOperativo]= " & GetID(ufficio)
            Else
                dbSQL &= " AND [IDPuntoOperativo] In ("
                Dim uffici As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                For i As Integer = 0 To uffici.Count - 1
                    If (i > 0) Then dbSQL &= ","
                    dbSQL &= DBUtils.DBNumber(GetID(uffici(i)))
                Next
                dbSQL &= ")"
            End If
            If (operatore IsNot Nothing) Then dbSQL &= " AND [IDAssegnatoA]= " & GetID(operatore)
            If (di.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(di)
            If (df.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(df)

            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
            While (dbRis.Read)
                Dim item As New CRichiestaFinanziamento
                CQSPD.Database.Load(item, dbRis)
                If (Not item.Soppressa) Then ret.Add(item)
            End While
            dbRis.Dispose()

            Return ret
        End Function


    End Class

    Private Shared m_RichiesteFinanziamento As CRichiesteFinanziamentoClass

    Public Shared ReadOnly Property RichiesteFinanziamento As CRichiesteFinanziamentoClass
        Get
            If (m_RichiesteFinanziamento Is Nothing) Then m_RichiesteFinanziamento = New CRichiesteFinanziamentoClass
            Return m_RichiesteFinanziamento
        End Get
    End Property

End Class
