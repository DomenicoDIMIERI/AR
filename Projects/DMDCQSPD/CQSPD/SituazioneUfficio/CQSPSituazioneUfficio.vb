Imports DMD
Imports DMD.Databases
Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Partial Class CQSPD

    'Public Class CQSPSituazioneUfficioOp
    '    Public Op As Object
    '    Public Childs As New CCollection(Of CQSPSituazioneUfficioOp)

    'End Class
    Public Class CQSPSituazioneUfficioClienteInfo
        Public Cliente As CPersona
        Public VisiteRicevute As CCollection(Of CVisita)
        Public Richieste As CCollection(Of CRichiestaFinanziamento)
        Public Consulenze As CCollection(Of CQSPDConsulenza)
        Public PraticheInCorso As CCollection(Of CRapportino)
        Public PraticheConcluse As CCollection(Of CRapportino)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Cliente = Nothing
            Me.VisiteRicevute = New CCollection(Of CVisita)
            Me.Richieste = New CCollection(Of CRichiestaFinanziamento)
            Me.Consulenze = New CCollection(Of CQSPDConsulenza)
            Me.PraticheInCorso = New CCollection(Of CRapportino)
            Me.PraticheConcluse = New CCollection(Of CRapportino)()
        End Sub

        Public Sub Load(ByVal info As CQSPSituazioneUfficio, ByVal cliente As CPersona)
            Me.Cliente = cliente
            For Each v As CVisita In info.VisiteRicevute
                If v.IDPersona = GetID(Me.Cliente) Then Me.VisiteRicevute.Add(v)
            Next
            For Each r As CRichiestaFinanziamento In info.RichiestePendenti
                If r.IDCliente = GetID(Me.Cliente) Then Me.Richieste.Add(r)
            Next
            For Each c As CQSPDConsulenza In info.ConsulenzePendenti
                If c.IDCliente = GetID(Me.Cliente) Then Me.Consulenze.Add(c)
            Next
            For Each p As CRapportino In info.PraticheInCorso
                If p.IDCliente = GetID(Me.Cliente) Then Me.PraticheInCorso.Add(p)
            Next
            For Each p As CRapportino In info.PraticheConcluse
                If p.IDCliente = GetID(Me.Cliente) Then Me.PraticheConcluse.Add(p)
            Next
        End Sub

        Private Function GetVisitaPrecedente(ByVal cliente As CPersona, ByVal data As Date, ByVal cache As CCollection(Of CVisita)) As CVisita
            Dim lastV As CVisita = Nothing
            Dim idCliente As Integer = GetID(cliente)
            For Each v As CVisita In cache
                If v.IDPersona = idCliente Then
                    If v.Data > data Then Exit For
                    lastV = v
                End If
            Next
            If (lastV Is Nothing) Then
                Dim cursor As New CVisiteCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Ricevuta.Value = True
                cursor.Data.Value = data
                cursor.Data.Operator = OP.OP_LE
                cursor.IDPersona.Value = idCliente
                lastV = cursor.Item
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (lastV IsNot Nothing) Then
                    cache.Add(lastV)
                    cache.Sort()
                End If
            End If

            Return lastV
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


    Public Class CQSPSituazioneUfficio
        Implements DMD.XML.IDMDXMLSerializable

        Const INCLUDIVISITEFINOAGG = 180 'Include le visite degli ultimi 30 giorni


        Private m_Ufficio As CUfficio = Nothing
        Private m_IDUfficio As Integer = 0

        'Public Ops As CCollection(Of CQSPSituazioneUfficioOp)
        Public VisiteRicevute As CCollection(Of CVisita)
        Public RichiestePendenti As CCollection(Of CRichiestaFinanziamento)
        Public ConsulenzePendenti As CCollection(Of CQSPDConsulenza)
        Public PraticheInCorso As CCollection(Of CRapportino)
        Public PraticheConcluse As CCollection(Of CRapportino)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Ufficio = Nothing
            Me.m_IDUfficio = 0

            'Me.Ops = New CCollection(Of CQSPSituazioneUfficioOp)
            Me.VisiteRicevute = New CCollection(Of CVisita)
            Me.RichiestePendenti = New CCollection(Of CRichiestaFinanziamento)
            Me.ConsulenzePendenti = New CCollection(Of CQSPDConsulenza)
            Me.PraticheInCorso = New CCollection(Of CRapportino)
            Me.PraticheConcluse = New CCollection(Of CRapportino)
        End Sub

        Public Sub New(ByVal ufficio As CUfficio)
            Me.New
            Me.m_Ufficio = ufficio
            Me.m_IDUfficio = GetID(ufficio)
        End Sub

        Public Property IDUfficio As Integer
            Get
                Return GetID(Me.m_Ufficio, Me.m_IDUfficio)
            End Get
            Set(value As Integer)
                If (Me.IDUfficio = value) Then Exit Property
                Me.m_IDUfficio = value
                Me.m_Ufficio = Nothing
            End Set
        End Property

        Public Property Ufficio As CUfficio
            Get
                If (Me.m_Ufficio Is Nothing) Then Me.m_Ufficio = Anagrafica.Uffici.GetItemById(Me.m_IDUfficio)
                Return Me.m_Ufficio
            End Get
            Set(value As CUfficio)
                Me.m_Ufficio = value
                Me.m_IDUfficio = GetID(value)
            End Set
        End Property

        Public Sub Load(ByVal dataInizio As Date?, ByVal dataFine As Date?)
            Dim d1 As Date = DateUtils.DateAdd(DateInterval.Day, -INCLUDIVISITEFINOAGG, DateUtils.ToDay)
            Dim d2 As Date = DateUtils.DateAdd(DateInterval.Second, 24 * 3600 - 1, DateUtils.ToDay)

            Me.VisiteRicevute = CustomerCalls.Visite.GetVisiteRicevute(Me.Ufficio, Nothing, d1, d2)
            Me.RichiestePendenti = CQSPD.RichiesteFinanziamento.GetRichiestePendenti(Me.Ufficio, Nothing, d1, d2)
            Me.ConsulenzePendenti = CQSPD.Consulenze.GetConsulenzeInCorso(Me.Ufficio, Nothing, d2, d2)
            Me.PraticheInCorso = CQSPD.Pratiche.GetPraticheInCorso(Me.Ufficio, Nothing, Nothing, Nothing)
            Me.PraticheConcluse = CQSPD.Pratiche.GetPraticheInCorso(Me.Ufficio, Nothing, dataInizio, dataFine)
            Me.Prepara()
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDUfficio" : Me.m_IDUfficio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "VisiteRicevute" : Me.VisiteRicevute.Clear() : Me.VisiteRicevute.AddRange(fieldValue)
                Case "RichiestePendenti" : Me.RichiestePendenti.Clear() : Me.RichiestePendenti.AddRange(fieldValue)
                Case "ConsulenzePendenti" : Me.ConsulenzePendenti.Clear() : Me.ConsulenzePendenti.AddRange(fieldValue)
                Case "PraticheInCorso" : Me.PraticheInCorso.Clear() : Me.PraticheInCorso.AddRange(fieldValue)
                Case "PraticheConcluse" : Me.PraticheConcluse.Clear() : Me.PraticheConcluse.AddRange(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDUfficio", Me.IDUfficio)
            writer.WriteTag("VisiteRicevute", Me.VisiteRicevute)
            writer.WriteTag("RichiestePendenti", Me.RichiestePendenti)
            writer.WriteTag("ConsulenzePendenti", Me.ConsulenzePendenti)
            writer.WriteTag("PraticheInCorso", Me.PraticheInCorso)
            writer.WriteTag("PraticheConcluse", Me.PraticheConcluse)
        End Sub

        Public Function GetClienti() As CCollection(Of CPersona)
            Dim clienti As New CKeyCollection(Of CPersona)
            For Each v As CVisita In Me.VisiteRicevute
                If v.IDPersona <> 0 AndAlso clienti.GetItemByKey("K" & v.IDPersona) Is Nothing AndAlso v.Persona IsNot Nothing Then
                    clienti.Add("K" & v.IDPersona, v.Persona)
                End If
            Next
            For Each r As CRichiestaFinanziamento In Me.RichiestePendenti
                If r.IDCliente <> 0 AndAlso clienti.GetItemByKey("K" & r.IDCliente) Is Nothing AndAlso r.Cliente IsNot Nothing Then
                    clienti.Add("K" & r.IDCliente, r.Cliente)
                End If
            Next
            For Each c As CQSPDConsulenza In Me.ConsulenzePendenti
                If c.IDCliente <> 0 AndAlso clienti.GetItemByKey("K" & c.IDCliente) Is Nothing AndAlso c.Cliente IsNot Nothing Then
                    clienti.Add("K" & c.IDCliente, c.Cliente)
                End If
            Next
            For Each p As CRapportino In Me.PraticheInCorso
                If p.IDCliente <> 0 AndAlso clienti.GetItemByKey("K" & p.IDCliente) Is Nothing AndAlso p.Cliente IsNot Nothing Then
                    clienti.Add("K" & p.IDCliente, p.Cliente)
                End If
            Next
            For Each p As CRapportino In Me.PraticheConcluse
                If p.IDCliente <> 0 AndAlso clienti.GetItemByKey("K" & p.IDCliente) Is Nothing AndAlso p.Cliente IsNot Nothing Then
                    clienti.Add("K" & p.IDCliente, p.Cliente)
                End If
            Next

            Return New CCollection(Of CPersona)(clienti)
        End Function


        Public Sub Prepara()
            'Me.Ops = New CCollection(Of CQSPSituazioneUfficioOp)

            'Ordiniamo le visite per data
            Me.VisiteRicevute.Sort()

            'Dim clienti As CCollection(Of CPersona) = Me.GetClienti
            'Dim cliente As CPersona
            'Dim v As CVisita
            'Dim r As CRichiestaFinanziamento
            'Dim c As CQSPDConsulenza
            'Dim p As CRapportino


            'Dim visiteXcliente As New CKeyCollection(Of CCollection(Of CVisita))
            'Dim richiesteXvisita As New CKeyCollection(Of CCollection(Of CRichiestaFinanziamento))
            'Dim consulenzeXRichiesta As New CKeyCollection(Of CCollection(Of CQSPDConsulenza))
            'Dim praticheXconsulenza As New CKeyCollection(Of CCollection(Of CRapportino))


            'For Each cliente In clienti
            '    For Each r In Me.RichiestePendenti
            '        If (r.IDCliente = GetID(cliente)) Then
            '            v = Nothing
            '            If (r.TipoContesto = "CVisita") Then
            '                v = Me.VisiteRicevute.GetItemById(r.IDContesto)
            '                If (v Is Nothing) Then
            '                    v = CustomerCalls.Visite.GetItemById(r.IDContesto)
            '                    If (v IsNot Nothing) Then
            '                        Me.VisiteRicevute.Add(v)
            '                        Me.VisiteRicevute.Sort()
            '                    End If
            '                End If
            '            End If
            '            If (v Is Nothing) Then
            '                v = Me.GetVisitaPrecedente(cliente, r.Data, Me.VisiteRicevute)
            '            End If
            '        End If
            '    Next
            'Next

            ''For Each v In Me.VisiteRicevute
            ''    Dim richieste As CCollection(Of CRichiestaFinanziamento) = richiesteXvisita.GetItemByKey("K" & GetID(v))
            ''    If (richieste Is Nothing) Then
            ''        richieste = New CCollection(Of CRichiestaFinanziamento)
            ''        richiesteXvisita.Add("K" & GetID(v), richieste)
            ''    End If
            ''    For Each r In Me.RichiestePendenti
            ''        If r.
            ''    Next Then
            ''Next

        End Sub

        Public Sub Sincronizza()
            'For Each e As CEstinzione In Me.AltriPrestiti
            '    'e.SetPersona(Me.Persona)
            '    e.SetPratica(Me.PraticheInCorso.GetItemById(e.IDPratica))
            'Next
            'For Each ep As EstinzioneXEstintore In Me.Estinzioni
            '    ep.SetEstinzione(Me.AltriPrestiti.GetItemById(ep.IDEstinzione))
            'Next
            ''For Each rich As CRichiestaFinanziamento In Me.RichiesteDiFinanziamento
            ''    rich.m_(Me.Persona)
            ''Next
            'For Each studiof As CQSPDStudioDiFattibilita In Me.StudiDiFattibilita
            '    'studiof.SetCliente(Me.Persona)
            '    studiof.SetRichiesta(Me.RichiesteDiFinanziamento.GetItemById(studiof.IDRichiesta))
            'Next
            'For Each cons As CQSPDConsulenza In Me.Consulenze
            '    cons.SetCliente(Me.Persona)
            '    cons.SetRichiesta(Me.RichiesteDiFinanziamento.GetItemById(cons.IDRichiesta))
            '    cons.SetStudioDiFattibilita(Me.StudiDiFattibilita.GetItemById(cons.IDStudioDiFattibilita))
            '    cons.SetRichiestaApprovazione(Me.RichiesteApprovazione.GetItemById(cons.IDRichiestaApprovazione))
            '    cons.SetOffertaCQS(Me.Offerte.GetItemById(cons.IDOffertaCQS))
            '    cons.SetOffertaPD(Me.Offerte.GetItemById(cons.IDOffertaPD))
            '    Dim col As New CEstinzioniXEstintoreCollection
            '    col.SetEstintore(cons)
            '    For Each ep In Me.Estinzioni
            '        If ep.TipoEstintore = "CQSPDConsulenza" AndAlso ep.IDEstintore = GetID(cons) Then
            '            ep.SetEstintore(cons)
            '            col.Add(ep)
            '        End If
            '    Next
            '    cons.SetEstinzioni(col)
            'Next
            'For Each off As COffertaCQS In Me.Offerte
            '    off.SetCliente(Me.Persona)
            '    off.SetPratica(Me.Pratiche.GetItemById(off.IDPratica))
            'Next
            'For Each stl As CStatoLavorazionePratica In Me.StatiDiLavorazione
            '    stl.SetOfferta(Me.Offerte.GetItemById(stl.IDOfferta))
            'Next
            'For Each prat As CRapportino In Me.Pratiche
            '    prat.SetCliente(Me.Persona)
            '    prat.SetConsulenza(Me.Consulenze.GetItemById(prat.IDConsulenza))
            '    prat.SetRichiestaDiFinanziamento(Me.RichiesteDiFinanziamento.GetItemById(prat.IDRichiestaDiFinanziamento))
            '    prat.SetRichiestaApprovazione(Me.RichiesteApprovazione.GetItemById(prat.IDRichiestaApprovazione))
            '    prat.SetOffertaCorrente(Me.Offerte.GetItemById(prat.IDOffertaCorrente))
            '    prat.SetStatoDiLavorazioneAttuale(Me.StatiDiLavorazione.GetItemById(prat.IDStatoDiLavorazioneAttuale))
            '    Dim col As New CStatiLavorazionePraticaCollection
            '    col.SetPratica(prat)
            '    For Each stl As CStatoLavorazionePratica In Me.StatiDiLavorazione
            '        If stl.IDPratica = GetID(prat) Then
            '            stl.SetPratica(prat)
            '            col.Add(stl)
            '        End If
            '    Next
            '    col.Sort()
            '    prat.SetStatiDiLavorazione(col)

            '    Dim col1 As New CEstinzioniXEstintoreCollection
            '    col1.SetEstintore(prat)
            '    For Each ep In Me.Estinzioni
            '        If ep.TipoEstintore = "CRapportino" AndAlso ep.IDEstintore = GetID(prat) Then
            '            ep.SetEstintore(prat)
            '            col1.Add(ep)
            '        End If
            '    Next
            '    prat.SetEstinzioni(col1)
            'Next
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class