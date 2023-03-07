Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    Public NotInheritable Class CConsulenzeClass
        Inherits CGeneralClass(Of CQSPDConsulenza)

        ''' <summary>
        ''' Evento generato quando viene inserita una nuova consulenza
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaInserita(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene proposta al cliente
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaProposta(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene accettata dal cliente
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaAccettata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene rifiutata dal cliente
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaRifiutata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene bocciata da un operatore o da un supervisore
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaBocciata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la consulenza viene eliminata
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaEliminata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando una consulenza viene modificata
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaModificata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando viene richiesta una valutazione della consulenza ad un supervisore
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaRichiestaApprovazione(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando un supervisore approva la consulenza
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaApprovata(ByVal e As ItemEventArgs)


        ''' <summary>
        ''' Evento generato quando un supervisore approva la consulenza
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaNegata(ByVal e As ItemEventArgs)
        ''' <summary>
        ''' Evento generato quando un supervisore prende in carico la richiesta di valutazione 
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConsulenzaPresaInCarico(ByVal e As ItemEventArgs)


        Friend Sub New()
            MyBase.New("modConsulenzeCQS", GetType(CQSPDConsulenzaCursor))
        End Sub


        Public Function GetUltimaConsulenzaProposta(ByVal persona As CPersonaFisica) As CQSPDConsulenza
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Return GetUltimaConsulenzaProposta(GetID(persona))
        End Function

        Public Function GetUltimaConsulenzaProposta(ByVal idPersona As Integer) As CQSPDConsulenza
            If (idPersona = 0) Then Return Nothing
            Dim cursor As New CQSPDConsulenzaCursor
            Dim ret As CQSPDConsulenza
            cursor.PageSize = 1
            cursor.IDCliente.Value = idPersona
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoConsulenza.Value = StatiConsulenza.PROPOSTA
            cursor.StatoConsulenza.Operator = OP.OP_GE
            cursor.DataProposta.SortOrder = SortEnum.SORT_DESC
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function GetConsulenzeByPersona(ByVal persona As CPersona) As CCollection(Of CQSPDConsulenza)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim ret As New CCollection(Of CQSPDConsulenza)
            If (GetID(persona) = 0) Then Return ret
            Dim cursor As New CQSPDConsulenzaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDCliente.Value = GetID(persona)
            cursor.DataConsulenza.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Dim q As CQSPDConsulenza = cursor.Item
                q.SetCliente(persona)
                ret.Add(q)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function GetConsulenzeByPersona(ByVal idPersona As Integer) As CCollection(Of CQSPDConsulenza)
            Return Me.GetConsulenzeByPersona(Anagrafica.Persone.GetItemById(idPersona))
        End Function

        Friend Sub doConsulenzaProposta(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " proposto al cliente")
            RaiseEvent ConsulenzaProposta(e)
        End Sub

        Friend Sub doConsulenzaAccettata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " accettato dal cliente")
            RaiseEvent ConsulenzaAccettata(e)
        End Sub

        Friend Sub doConsulenzaRifiutata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " rifiutato dal cliente")
            RaiseEvent ConsulenzaRifiutata(e)
        End Sub

        Friend Sub doConsulenzaInserita(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            MyBase.doItemCreated(e)
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " registrato")
            RaiseEvent ConsulenzaInserita(e)
        End Sub

        Friend Sub doConsulenzaBocciata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " registrato e non fattibilie")
            RaiseEvent ConsulenzaBocciata(e)
        End Sub

        Friend Sub doConsulenzaEliminata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            MyBase.doItemDeleted(e)
            RaiseEvent ConsulenzaEliminata(e)
        End Sub

        Friend Sub doConsulenzaModificata(ByVal e As ItemEventArgs(Of CQSPDConsulenza))
            MyBase.doItemModified(e)
            RaiseEvent ConsulenzaModificata(e)
        End Sub

        Private Function GetNumeroOfferta(ByVal c As CQSPDConsulenza) As Integer
            If (c Is Nothing OrElse c.StudioDiFattibilita Is Nothing) Then Return 1
            Return 1 + c.StudioDiFattibilita.Proposte.IndexOf(c)
        End Function

        Private Function FormatOfferta(ByVal o As COffertaCQS) As String
            If (o Is Nothing) Then Return Nothing
            Return o.NomeProdotto & " - " & o.NomeCessionario & " (" & Formats.FormatValuta(o.Rata) & "x" & Formats.FormatInteger(o.Durata) & " = " & Formats.FormatValuta(o.MontanteLordo) & ")"
        End Function

        Function FormatStatoEx(ByVal stato As StatiConsulenza) As String
            Dim items() As StatiConsulenza = {StatiConsulenza.INSERITA, StatiConsulenza.PROPOSTA, StatiConsulenza.ACCETTATA, StatiConsulenza.RIFIUTATA, StatiConsulenza.BOCCIATA}
            Dim names() As String = {"Solo Inserita", "Proposta al cliente", "Accettata dal cliente", "Rifiutata dal cliente", "Bocciata o Non fattibile"}
            Return names(Arrays.IndexOf(items, stato))
        End Function

        Public Function ParseTemplate(ByVal template As String, ByVal context As CKeyCollection) As String
            Dim ret As String = template
            Dim consulenza As CQSPDConsulenza = context("Consulenza")
            Dim currentUser As CUser = context("CurrentUser")
            
            ret = Replace(ret, "%%NOMECONSULENTE%%", consulenza.NomeConsulente)
            ret = Replace(ret, "%%IDCLIENTE%%", consulenza.IDCliente)
            ret = Replace(ret, "%%NOMECLIENTE%%", consulenza.NomeCliente)
            ret = Replace(ret, "%%NUMEROOFFERTA%%", Me.GetNumeroOfferta(consulenza))
            ret = Replace(ret, "%%ID%%", GetID(consulenza))
            ret = Replace(ret, "%%BASEURL%%", ApplicationContext.BaseURL)
            ret = Replace(ret, "%%NOTE%%", consulenza.Descrizione)
            ret = Replace(ret, "%%USERNAME%%", currentUser.Nominativo)
            ret = Replace(ret, "%%DESCRIZIONECQS%%", Me.FormatOfferta(consulenza.OffertaCQS))
            ret = Replace(ret, "%%DESCRIZIONEPD%%", Me.FormatOfferta(consulenza.OffertaPD))
            ret = Replace(ret, "%%NETTOALLAMANO%%", Formats.FormatValuta(consulenza.NettoRicavo - consulenza.SommaEstinzioni))
            ret = Replace(ret, "%%NETTORICAVO%%", Formats.FormatValuta(consulenza.NettoRicavo))
            ret = Replace(ret, "%%SOMMAESTINZIONI%%", Formats.FormatValuta(consulenza.SommaEstinzioni))
            ret = Replace(ret, "%%SOMMAPIGNORAMENTI%%", Formats.FormatValuta(consulenza.SommaPignoramenti))
            ret = Replace(ret, "%%SOMMATRATTENUTE%%", Formats.FormatValuta(consulenza.SommaTrattenuteVolontarie))
            ret = Replace(ret, "%%STATOCONSULENZA%%", CQSPD.Consulenze.FormatStato(consulenza.Stato))
            ret = Replace(ret, "%%STATOCONSULENZAEX%%", CQSPD.Consulenze.FormatStatoEx(consulenza.Stato))
            ret = Replace(ret, "%%DATAPROPOSTA%%", Formats.FormatUserDateTime(consulenza.DataProposta))
            ret = Replace(ret, "%%DATACONFERMA%%", Formats.FormatUserDateTime(consulenza.DataConferma))
            ret = Replace(ret, "%%DATAINSERIMENTO%%", Formats.FormatUserDateTime(consulenza.CreatoIl))
            ret = Replace(ret, "%%MOTIVOANNULLAMENTO%%", consulenza.MotivoAnnullamento)
            ret = Replace(ret, "%%DETTAGLIOANNULLAMENTO%%", consulenza.DettaglioAnnullamento)
            ret = Me.ParseNib(ret, "%%DATACONSULENZA%%", Formats.FormatUserDateTime(consulenza.DataConsulenza))
            Return ret
        End Function

        Private Function ParseNib(ByVal text As String, ByVal nib As String, ByVal value As Nullable(Of Date)) As String
            If (value.HasValue) Then
                Return Strings.Replace(text, nib, Formats.FormatUserDateTime(value))
            Else
                Return Strings.Replace(text, nib, "")
            End If
        End Function

        Public Function GetUltimaConsulenza(ByVal idPersona As Integer) As CQSPDConsulenza
            Dim cursor As New CQSPDConsulenzaCursor
            Try
                cursor.IDCliente.Value = idPersona
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.DataConsulenza.SortOrder = SortEnum.SORT_DESC
                'cursor.StatoConsulenza().setValue(StatiConsulenza.ACCETTATA);
                cursor.PageSize = 1
                Return cursor.Item
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        Function FormatStato(ByVal stato As StatiConsulenza) As String
            Dim items() As StatiConsulenza = {StatiConsulenza.INSERITA, StatiConsulenza.PROPOSTA, StatiConsulenza.ACCETTATA, StatiConsulenza.RIFIUTATA, StatiConsulenza.BOCCIATA}
            Dim names() As String = {"Inserita", "Proposta", "Accettata", "Rifiutata", "Bocciata"}
            Return names(Arrays.IndexOf(items, stato))
        End Function

        Protected Friend Sub DoOnRequireApprovation(e As ItemEventArgs)
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " in richiesta approvazione")
            RaiseEvent ConsulenzaRichiestaApprovazione(e)
        End Sub

        Protected Friend Sub DoOnApprovata(e As ItemEventArgs)
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " approvato da " & Users.CurrentUser.Nominativo)
            RaiseEvent ConsulenzaApprovata(e)
        End Sub

        Protected Friend Sub DoOnNegata(e As ItemEventArgs)
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " negato da " & Users.CurrentUser.Nominativo)
            RaiseEvent ConsulenzaNegata(e)
        End Sub

        Protected Friend Sub DoOnRifiutata(e As ItemEventArgs)
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " rifiutato dal cliente")
            RaiseEvent ConsulenzaBocciata(e)
        End Sub

        Protected Friend Sub DoOnInCarico(e As ItemEventArgs)
            Dim consulenza As CQSPDConsulenza = e.Item
            Dim cliente As CPersona = consulenza.Cliente
            'Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.getp
            If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                cliente.SetFlag(PFlags.Cliente, True)
                cliente.Save()
            End If
            Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
            info.AggiornaOperazione(consulenza, "Studio di fattibilità del " & Formats.FormatUserDateTime(consulenza.DataConsulenza) & " in supervisione")
            RaiseEvent ConsulenzaPresaInCarico(e)
        End Sub

        Function GetConsulenzeInCorso(ufficio As CUfficio, operatore As CUser, di As Date?, df As Date?) As CCollection(Of CQSPDConsulenza)
            Dim cursor As New CQSPDConsulenzaCursor
            Dim ret As New CCollection(Of CQSPDConsulenza)

            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoConsulenza.Value = StatiConsulenza.ACCETTATA
            If (ufficio IsNot Nothing) Then cursor.IDPuntoOperativo.Value = GetID(ufficio)
            If (operatore IsNot Nothing) Then cursor.IDConsulente.Value = GetID(operatore)

            If (di.HasValue) Then
                If (df.HasValue) Then
                    cursor.DataConsulenza.Between(di.Value, df.Value)
                Else
                    cursor.DataConsulenza.Value = di.Value
                    cursor.DataConsulenza.Operator = OP.OP_GE
                End If
            ElseIf (df.HasValue) Then
                cursor.DataConsulenza.Value = df.Value
                cursor.DataConsulenza.Operator = OP.OP_LE
            End If

            Dim dbSQL As String
            dbSQL = "SELECT * FROM (SELECT [T1].*, [T2].[ID] AS [IDGrp] FROM (" & cursor.GetSQL & ") AS [T1] INNER JOIN (SELECT [ID] FROM [tbl_CQSPDGrpConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [CntPratiche]=0 GROUP BY [ID]) AS [T2] ON [T1].[IDGruppo]=[T2].[ID]) WHERE Not ([IDGrp] Is Null)"
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                Dim item As New CQSPDConsulenza
                CQSPD.Database.Load(item, dbRis)
                ret.Add(item)
            End While
            dbRis.Dispose()

            Return ret
        End Function



    End Class

    Private Shared m_Consulenze As CConsulenzeClass = Nothing

    Public Shared ReadOnly Property Consulenze As CConsulenzeClass
        Get
            If (m_Consulenze Is Nothing) Then m_Consulenze = New CConsulenzeClass
            Return m_Consulenze
        End Get
    End Property


End Class
