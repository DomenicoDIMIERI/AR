Imports DMD
Imports DMD.Databases
Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class CQSPD

   
    Public Class CQSPSituazionePersona
        Implements DMD.XML.IDMDXMLSerializable

      

        Private m_Persona As CPersona = Nothing
        Private m_IDPersona As Integer = 0
        Public AltriPrestiti As CCollection(Of CEstinzione)
        Public RichiesteConteggi As CCollection(Of CRichiestaConteggio)
        Public RichiesteDiFinanziamento As CCollection(Of CRichiestaFinanziamento)
        Public StudiDiFattibilita As CCollection(Of CQSPDStudioDiFattibilita)
        Public Consulenze As CCollection(Of CQSPDConsulenza)
        Public Offerte As CCollection(Of COffertaCQS)
        Public Pratiche As CCollection(Of CRapportino)
        ' Public StatiDiLavorazione As CCollection(Of CStatoLavorazionePratica)
        Public RichiesteApprovazione As CCollection(Of CRichiestaApprovazione)
        Public Estinzioni As CCollection(Of EstinzioneXEstintore)
        Public FinestreLavorazione As CCollection(Of FinestraLavorazione)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Persona = Nothing
            Me.m_IDPersona = 0
            Me.RichiesteConteggi = New CCollection(Of CRichiestaConteggio)
            Me.AltriPrestiti = New CCollection(Of CEstinzione)
            Me.RichiesteDiFinanziamento = New CCollection(Of CRichiestaFinanziamento)
            Me.StudiDiFattibilita = New CCollection(Of CQSPDStudioDiFattibilita)
            Me.Consulenze = New CCollection(Of CQSPDConsulenza)
            Me.Offerte = New CCollection(Of COffertaCQS)
            Me.Pratiche = New CCollection(Of CRapportino)
            'Me.StatiDiLavorazione = New CCollection(Of CStatoLavorazionePratica)
            Me.RichiesteApprovazione = New CCollection(Of CRichiestaApprovazione)
            Me.Estinzioni = New CCollection(Of EstinzioneXEstintore)
            Me.FinestreLavorazione = New CCollection(Of FinestraLavorazione)
        End Sub

        Public Sub New(ByVal persona As CPersona)
            Me.New()
            Me.m_Persona = persona
            Me.m_IDPersona = GetID(persona)
        End Sub

        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                If (Me.IDPersona = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
            End Set
        End Property

        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
            End Set
        End Property

        Public Sub Load()
            Me.AltriPrestiti = CQSPD.Estinzioni.GetEstinzioniByPersona(Me.Persona)
            Me.RichiesteDiFinanziamento = CQSPD.RichiesteFinanziamento.GetRichiesteByPersona(Me.Persona)
            Me.RichiesteConteggi = CQSPD.RichiesteConteggi.GetRichiesteByPersona(Me.Persona)
            Me.Consulenze = CQSPD.Consulenze.GetConsulenzeByPersona(Me.Persona)
            'Me.Offerte = CQSPD.Offerte.GetOfferteByPersona(Me.Persona)
            Me.Pratiche = CQSPD.Pratiche.GetPraticheByPersona(Me.Persona)
            Me.StudiDiFattibilita = CQSPD.StudiDiFattibilita.GetStudiDiFattibilitaByPersona(Me.Persona)
            Me.FinestreLavorazione = CQSPD.FinestreDiLavorazione.GetFinestreByPersona(Me.Persona)
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AltriPrestiti" : Me.AltriPrestiti.Clear() : Me.AltriPrestiti.AddRange(fieldValue)
                Case "RichiesteDiFinanziamento" : Me.RichiesteDiFinanziamento.Clear() : Me.RichiesteDiFinanziamento.AddRange(fieldValue)
                Case "StudiDiFattibilita" : Me.StudiDiFattibilita.Clear() : Me.StudiDiFattibilita.AddRange(fieldValue)
                Case "Consulenze" : Me.Consulenze.Clear() : Me.Consulenze.AddRange(fieldValue)
                Case "Offerte" : Me.Offerte.Clear() : Me.Offerte.AddRange(fieldValue)
                Case "Pratiche" : Me.Pratiche.Clear() : Me.Pratiche.AddRange(fieldValue)
                Case "RichiesteConteggi" : Me.RichiesteConteggi.Clear() : Me.RichiesteConteggi.AddRange(fieldValue)
                    'Case "StatiDiLavorazione" : Me.StatiDiLavorazione.Clear() : Me.StatiDiLavorazione.AddRange(fieldValue)
                Case "RichiesteApprovazione" : Me.RichiesteApprovazione.Clear() : Me.RichiesteApprovazione.AddRange(fieldValue)
                Case "Estinzioni" : Me.Estinzioni.Clear() : Me.Estinzioni.AddRange(fieldValue)
                Case "FinestreLavorazione" : Me.FinestreLavorazione.Clear() : Me.FinestreLavorazione.AddRange(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            Me.Prepara()
            Me.Controlla()
            'writer.Settings.SetValueBool("CRapportino_SerializeStatiLavorazione", False)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteTag("AltriPrestiti", Me.AltriPrestiti)
            writer.WriteTag("Estinzioni", Me.Estinzioni)
            writer.WriteTag("RichiesteDiFinanziamento", Me.RichiesteDiFinanziamento)
            writer.WriteTag("StudiDiFattibilita", Me.StudiDiFattibilita)
            writer.WriteTag("Consulenze", Me.Consulenze)
            writer.WriteTag("Offerte", Me.Offerte)
            writer.WriteTag("Pratiche", Me.Pratiche)
            'writer.WriteTag("StatiDiLavorazione", Me.StatiDiLavorazione)
            writer.WriteTag("RichiesteApprovazione", Me.RichiesteApprovazione)
            writer.WriteTag("FinestreLavorazione", Me.FinestreLavorazione)
            writer.WriteTag("RichiesteConteggi", Me.RichiesteConteggi)
        End Sub

        
        Public Sub Prepara()
            For Each ricc As CRichiestaConteggio In Me.RichiesteConteggi
                If (Me.FinestreLavorazione.GetItemById(ricc.IDFinestraLavorazione) Is Nothing AndAlso ricc.FinestraLavorazione IsNot Nothing) Then Me.FinestreLavorazione.Add(ricc.FinestraLavorazione)
            Next
            For Each rich As CRichiestaFinanziamento In Me.RichiesteDiFinanziamento
                If Me.FinestreLavorazione.GetItemById(rich.IDFinestraLavorazione) Is Nothing AndAlso rich.FinestraLavorazione IsNot Nothing Then Me.FinestreLavorazione.Add(rich.FinestraLavorazione)
            Next
            For Each cons As CQSPDConsulenza In Me.Consulenze
                For Each e As EstinzioneXEstintore In cons.Estinzioni
                    If Me.AltriPrestiti.GetItemById(e.IDEstinzione) Is Nothing AndAlso e.Estinzione IsNot Nothing Then Me.AltriPrestiti.Add(e.Estinzione)
                    Me.Estinzioni.Add(e)
                Next
                If Me.StudiDiFattibilita.GetItemById(cons.IDStudioDiFattibilita) Is Nothing AndAlso cons.StudioDiFattibilita IsNot Nothing Then Me.StudiDiFattibilita.Add(cons.StudioDiFattibilita)
                If Me.Offerte.GetItemById(cons.IDOffertaCQS) Is Nothing AndAlso cons.OffertaCQS IsNot Nothing Then Me.Offerte.Add(cons.OffertaCQS)
                If Me.Offerte.GetItemById(cons.IDOffertaPD) Is Nothing AndAlso cons.OffertaPD IsNot Nothing Then Me.Offerte.Add(cons.OffertaPD)
                If Me.RichiesteDiFinanziamento.GetItemById(cons.IDRichiesta) Is Nothing AndAlso cons.Richiesta IsNot Nothing Then Me.RichiesteDiFinanziamento.Add(cons.Richiesta)
                If Me.RichiesteApprovazione.GetItemById(cons.IDRichiestaApprovazione) Is Nothing AndAlso cons.RichiestaApprovazione IsNot Nothing Then Me.RichiesteApprovazione.Add(cons.RichiestaApprovazione)
                If Me.FinestreLavorazione.GetItemById(cons.IDFinestraLavorazione) Is Nothing AndAlso cons.FinestraLavorazione IsNot Nothing Then Me.FinestreLavorazione.Add(cons.FinestraLavorazione)
            Next
            For Each prat As CRapportino In Me.Pratiche
                For Each e As EstinzioneXEstintore In prat.Estinzioni
                    If Me.AltriPrestiti.GetItemById(e.IDEstinzione) Is Nothing AndAlso e.Estinzione IsNot Nothing Then Me.AltriPrestiti.Add(e.Estinzione)
                    Me.Estinzioni.Add(e)
                Next
                If Me.Offerte.GetItemById(prat.IDOffertaCorrente) Is Nothing AndAlso prat.OffertaCorrente IsNot Nothing Then Me.Offerte.Add(prat.OffertaCorrente)
                If Me.RichiesteDiFinanziamento.GetItemById(prat.IDRichiestaApprovazione) Is Nothing AndAlso prat.RichiestaApprovazione IsNot Nothing Then Me.RichiesteApprovazione.Add(prat.RichiestaApprovazione)
                'If Me.StatiDiLavorazione.GetItemById(prat.IDStatoDiLavorazioneAttuale) Is Nothing AndAlso prat.StatoDiLavorazioneAttuale IsNot Nothing Then Me.StatiDiLavorazione.Add(prat.StatoDiLavorazioneAttuale)
                For Each stl As CStatoLavorazionePratica In prat.StatiDiLavorazione
                    ' Me.StatiDiLavorazione.Add(stl)
                    If Me.Offerte.GetItemById(stl.IDOfferta) Is Nothing AndAlso stl.Offerta IsNot Nothing Then Me.Offerte.Add(stl.Offerta)
                Next
                If Me.FinestreLavorazione.GetItemById(prat.IDFinestraLavorazione) Is Nothing AndAlso prat.FinestraLavorazione IsNot Nothing Then Me.FinestreLavorazione.Add(prat.FinestraLavorazione)
            Next
        End Sub

        Private Sub Controlla()
            Me.Sincronizza()
            For Each p As CRapportino In Me.Pratiche
                If (p.RichiestaDiFinanziamento Is Nothing) Then p.RichiestaDiFinanziamento = Me.FindRichiesta(p)
                If (p.Consulenza Is Nothing) Then p.Consulenza = Me.FindConsulenza(p)
                If (p.IsChanged) Then p.Save()
            Next
            For Each c As CQSPDConsulenza In Me.Consulenze
                If (c.StudioDiFattibilita Is Nothing) Then c.StudioDiFattibilita = Me.FindSF(c)
                If (c.Richiesta Is Nothing) Then c.Richiesta = Me.FindRichiesta(c)
                If (c.IsChanged) Then c.Save()
            Next
            For Each s As CQSPDStudioDiFattibilita In Me.StudiDiFattibilita
                If (s.Richiesta Is Nothing) Then s.Richiesta = Me.FindRichiesta(s)
                If (s.IsChanged) Then s.Save()
            Next
        End Sub

        Private Function FindConsulenza(ByVal p As CRapportino) As CQSPDConsulenza
            If (p.FinestraLavorazione IsNot Nothing AndAlso p.FinestraLavorazione.StudioDiFattibilita IsNot Nothing) Then Return p.FinestraLavorazione.StudioDiFattibilita

            Dim dp As Date? = p.DataDecorrenza
            Dim c As CQSPDConsulenza = Nothing
            If (dp.HasValue = False) Then dp = p.CreatoIl
            For Each c In Me.Consulenze
                Dim dc As Date? = c.DataConsulenza
                If (dc.HasValue = False) Then dc = c.CreatoIl
                Dim diff As Integer = Calendar.DateDiff(DateInterval.Day, dc.Value, dp.Value)
                If (diff >= 0 AndAlso diff <= 30) Then Return c
            Next
            c = New CQSPDConsulenza
            c.Cliente = Me.Persona
            'If p.Prodotto IsNot Nothing AndAlso p.Prodotto.IdTipoContratto = "D" Then
            '    c.OffertaPD = p.OffertaCorrente
            'Else
            '    c.OffertaCQS = p.OffertaCorrente
            'End If
            c.Stato = ObjectStatus.OBJECT_VALID
            c.DataConsulenza = dp.Value
            c.DataConferma = dp.Value
            'c.DataDecorrenza = p.DataDecorrenza
            c.DataProposta = dp.Value
            c.Descrizione = "Consulenza Creata automaticamente dalla pratica " & p.NumeroPratica
            c.FinestraLavorazione = p.FinestraLavorazione
            c.Eta = Me.Persona.Eta(dp.Value)
            c.Consulente = p.Consulente
            c.Azienda = p.Impiego.Azienda
            c.DataAssunzione = p.Impiego.DataAssunzione
            c.Richiesta = p.RichiestaDiFinanziamento
            c.PuntoOperativo = p.PuntoOperativo
            c.StatoConsulenza = StatiConsulenza.ACCETTATA
            c.Save()
            Me.Consulenze.Add(c)
            Return c
        End Function

        Private Function FindSF(ByVal c As CQSPDConsulenza) As CQSPDStudioDiFattibilita
            Dim dp As Date? = c.DataConsulenza
            If (dp.HasValue = False) Then dp = c.CreatoIl

            Dim s As CQSPDStudioDiFattibilita = Nothing
            For Each s In Me.StudiDiFattibilita
                Dim dc As Date = s.Data
                Dim diff As Integer = Calendar.DateDiff(DateInterval.Day, dc, dp.Value)
                If (diff >= 0 AndAlso diff <= 30) Then Return s
            Next
            s = New CQSPDStudioDiFattibilita
            s.Cliente = Me.Persona
            s.Stato = ObjectStatus.OBJECT_VALID
            s.Data = dp.Value
            s.PuntoOperativo = c.PuntoOperativo
            s.Richiesta = c.Richiesta
            s.Save()
            Me.StudiDiFattibilita.Add(s)
            Return s
        End Function

        Private Function FindRichiesta(ByVal p As CRapportino) As CRichiestaFinanziamento
            Dim dp As Date? = p.DataDecorrenza
            Dim r As CRichiestaFinanziamento = Nothing
            If (dp.HasValue = False) Then dp = p.CreatoIl
            For Each r In Me.RichiesteDiFinanziamento
                Dim dc As Date = r.Data
                Dim diff As Integer = Calendar.DateDiff(DateInterval.Day, dc, dp.Value)
                If (diff >= 0 AndAlso diff <= 30) Then Return r
            Next
            r = New CRichiestaFinanziamento
            r.Cliente = Me.Persona
            r.Stato = ObjectStatus.OBJECT_VALID
            r.Data = dp.Value
            r.PuntoOperativo = p.PuntoOperativo
            r.Note = "Richiesta Creata automaticamente dalla pratica " & p.NumeroPratica
            r.FinestraLavorazione = p.FinestraLavorazione
            r.StatoRichiesta = StatoRichiestaFinanziamento.EVASA
            'r.AssegnatoA = p.Stato
            r.Save()
            Me.RichiesteDiFinanziamento.Add(r)
            Return r
        End Function

        Private Function FindRichiesta(ByVal c As CQSPDConsulenza) As CRichiestaFinanziamento
            If (c.FinestraLavorazione IsNot Nothing AndAlso c.FinestraLavorazione.RichiestaFinanziamento IsNot Nothing) Then Return c.FinestraLavorazione.RichiestaFinanziamento

            Dim dp As Date? = c.DataConsulenza
            If (dp.HasValue = False) Then dp = c.CreatoIl
            Dim r As CRichiestaFinanziamento = Nothing
            For Each r In Me.RichiesteDiFinanziamento
                Dim dc As Date = r.Data
                Dim diff As Integer = Calendar.DateDiff(DateInterval.Day, dc, dp.Value)
                If (diff >= 0 AndAlso diff <= 30) Then Return r
            Next
            r = New CRichiestaFinanziamento
            r.Cliente = Me.Persona
            r.Stato = ObjectStatus.OBJECT_VALID
            r.Data = dp.Value
            r.PuntoOperativo = c.PuntoOperativo
            r.Note = "Richiesta Creata automaticamente dalla consulenza del " & Formats.FormatUserDateTime(dp)
            r.StatoRichiesta = StatoRichiestaFinanziamento.EVASA
            r.Save()

            Me.RichiesteDiFinanziamento.Add(r)

            Return r
        End Function

        Private Function FindRichiesta(ByVal s As CQSPDStudioDiFattibilita) As CRichiestaFinanziamento
            Dim dp As Date? = s.Data
            If (dp.HasValue = False) Then dp = s.CreatoIl
            Dim r As CRichiestaFinanziamento = Nothing
            For Each r In Me.RichiesteDiFinanziamento
                Dim dc As Date = r.Data
                Dim diff As Integer = Calendar.DateDiff(DateInterval.Day, dc, dp.Value)
                If (diff >= 0 AndAlso diff <= 30) Then Return r
            Next
            r = New CRichiestaFinanziamento
            r.Cliente = Me.Persona
            r.Stato = ObjectStatus.OBJECT_VALID
            r.Data = dp.Value
            r.PuntoOperativo = s.PuntoOperativo
            r.Note = "Richiesta Creata automaticamente lo studio di fattibilità del " & Formats.FormatUserDateTime(dp)
            r.StatoRichiesta = StatoRichiestaFinanziamento.EVASA
            r.Save()

            Me.RichiesteDiFinanziamento.Add(r)

            Return r
        End Function

        Public Sub Sincronizza()
            For Each ricc As CRichiestaConteggio In Me.RichiesteConteggi
                ricc.SetCliente(Me.Persona)
                ricc.SetFinestraLavorazione(Me.FinestreLavorazione.GetItemById(ricc.IDFinestraLavorazione))
            Next
            For Each e As CEstinzione In Me.AltriPrestiti
                e.SetPersona(Me.Persona)
                e.SetPratica(Me.Pratiche.GetItemById(e.IDPratica))
            Next
            For Each ep As EstinzioneXEstintore In Me.Estinzioni
                ep.SetEstinzione(Me.AltriPrestiti.GetItemById(ep.IDEstinzione))
            Next
            For Each rich As CRichiestaFinanziamento In Me.RichiesteDiFinanziamento
                rich.SetCliente(Me.Persona)
                rich.SetFinestraLavorazione(Me.FinestreLavorazione.GetItemById(rich.IDFinestraLavorazione))
            Next
            For Each studiof As CQSPDStudioDiFattibilita In Me.StudiDiFattibilita
                studiof.SetCliente(Me.Persona)
                studiof.SetRichiesta(Me.RichiesteDiFinanziamento.GetItemById(studiof.IDRichiesta))
            Next
            For Each cons As CQSPDConsulenza In Me.Consulenze
                cons.SetCliente(Me.Persona)
                cons.SetRichiesta(Me.RichiesteDiFinanziamento.GetItemById(cons.IDRichiesta))
                cons.SetStudioDiFattibilita(Me.StudiDiFattibilita.GetItemById(cons.IDStudioDiFattibilita))
                cons.SetRichiestaApprovazione(Me.RichiesteApprovazione.GetItemById(cons.IDRichiestaApprovazione))
                cons.SetOffertaCQS(Me.Offerte.GetItemById(cons.IDOffertaCQS))
                cons.SetOffertaPD(Me.Offerte.GetItemById(cons.IDOffertaPD))
                cons.SetFinestraLavorazione(Me.FinestreLavorazione.GetItemById(cons.IDFinestraLavorazione))
                Dim col As New CEstinzioniXEstintoreCollection
                col.SetEstintore(cons)
                For Each ep In Me.Estinzioni
                    If ep.TipoEstintore = "CQSPDConsulenza" AndAlso ep.IDEstintore = GetID(cons) Then
                        ep.SetEstintore(cons)
                        col.Add(ep)
                    End If
                Next
                cons.SetEstinzioni(col)
            Next
            For Each off As COffertaCQS In Me.Offerte
                off.SetCliente(Me.Persona)
                off.SetPratica(Me.Pratiche.GetItemById(off.IDPratica))
            Next
            'For Each stl As CStatoLavorazionePratica In Me.StatiDiLavorazione
            '    stl.SetOfferta(Me.Offerte.GetItemById(stl.IDOfferta))
            'Next
            For Each prat As CRapportino In Me.Pratiche
                prat.SetCliente(Me.Persona)
                prat.SetConsulenza(Me.Consulenze.GetItemById(prat.IDConsulenza))
                prat.SetRichiestaDiFinanziamento(Me.RichiesteDiFinanziamento.GetItemById(prat.IDRichiestaDiFinanziamento))
                prat.SetRichiestaApprovazione(Me.RichiesteApprovazione.GetItemById(prat.IDRichiestaApprovazione))
                prat.SetOffertaCorrente(Me.Offerte.GetItemById(prat.IDOffertaCorrente))
                'prat.SetStatoDiLavorazioneAttuale(Me.StatiDiLavorazione.GetItemById(prat.IDStatoDiLavorazioneAttuale))
                prat.SetFinestraLavorazione(Me.FinestreLavorazione.GetItemById(prat.IDFinestraLavorazione))
                'Dim col As New CStatiLavorazionePraticaCollection
                ' col.SetPratica(prat)
                For Each stl As CStatoLavorazionePratica In prat.StatiDiLavorazione
                    'If stl.IDPratica = GetID(prat) Then
                    stl.SetPratica(prat)
                    stl.SetOfferta(Me.Offerte.GetItemById(stl.IDOfferta))
                    'col.Add(stl)
                    'End If
                Next
                'col.Sort()
                'prat.SetStatiDiLavorazione(col)

                Dim col1 As New CEstinzioniXEstintoreCollection
                col1.SetEstintore(prat)
                For Each ep In Me.Estinzioni
                    If ep.TipoEstintore = "CRapportino" AndAlso ep.IDEstintore = GetID(prat) Then
                        ep.SetEstintore(prat)
                        col1.Add(ep)
                    End If
                Next
                prat.SetEstinzioni(col1)
            Next

            For Each fl As FinestraLavorazione In Me.FinestreLavorazione
                Dim altriPrestiti As New CCollection(Of CEstinzione)
                For Each ap As CEstinzione In Me.AltriPrestiti
                    Dim di As Date? = fl.DataInizioLavorazione : If (di.HasValue = False) Then di = fl.DataInizioLavorabilita
                    If (ap.IsInCorsoOFutura(di)) Then
                        altriPrestiti.Add(ap)
                    End If
                Next
                fl.SetAltriPrestiti(altriPrestiti)
                fl.SetCQS(Me.Pratiche.GetItemById(fl.IDCQS))
                fl.SetPD(Me.Pratiche.GetItemById(fl.IDPD))
                fl.SetCQSI(Me.Pratiche.GetItemById(fl.IDCQSI))
                fl.SetPDI(Me.Pratiche.GetItemById(fl.IDPDI))
                fl.SetRichiestaFinanziamento(Me.RichiesteDiFinanziamento.GetItemById(fl.IDRichiestaFinanziamento))
                fl.SetStudioDiFattibilita(Me.Consulenze.GetItemById(fl.IDStudioDiFattibilita))
            Next
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function CanDeleteAltroPrestito(ByVal e As CEstinzione) As String()
            Dim ret As New System.Collections.ArrayList
            If (e.Stato = ObjectStatus.OBJECT_VALID) Then
                For Each esti As EstinzioneXEstintore In Me.Estinzioni
                    If esti.Stato = ObjectStatus.OBJECT_VALID AndAlso esti.TipoEstintore <> "" AndAlso esti.IDEstintore <> 0 AndAlso esti.IDEstinzione = GetID(e) Then
                        ret.Add(esti.TipoEstintore & "[" & esti.IDEstintore & "]")
                    End If
                Next
            End If
            Return DirectCast(ret.ToArray(GetType(String)), String())
        End Function
    End Class


End Class