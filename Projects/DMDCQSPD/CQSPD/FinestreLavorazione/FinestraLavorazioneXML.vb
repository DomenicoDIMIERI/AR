Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Office

Partial Public Class CQSPD
 
    <Serializable> _
    Public Class FinestraLavorazioneXML
        Implements DMD.XML.IDMDXMLSerializable

        Public W As FinestraLavorazione
        Private StatiLavorazione As CCollection(Of CStatoLavorazionePratica)
        Private Offerte As CCollection(Of COffertaCQS)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.W = Nothing
            Me.StatiLavorazione = New CCollection(Of CStatoLavorazionePratica)
            Me.Offerte = New CCollection(Of COffertaCQS)
        End Sub

        Public Sub New(ByVal w As FinestraLavorazione)
            Me.New()
            If (w Is Nothing) Then Throw New ArgumentNullException("w")
            Me.W = w
        End Sub

        Public Sub Prepara()
            Me.StatiLavorazione.Clear()
            Me.Offerte.Clear()


            Me.AggiungiOfferteSF(Me.W.StudioDiFattibilita)
            Me.AggiungiOfferteP(Me.W.CQS)
            Me.AggiungiOfferteP(Me.W.PD)
            Me.AggiungiOfferteP(Me.W.CQSI)
            Me.AggiungiOfferteP(Me.W.PDI)
        End Sub

        Public Sub Sincronizza()
            Me.SincronizzaSF(Me.W.StudioDiFattibilita)
            Me.SincronizzaP(Me.W.CQS)
            Me.SincronizzaP(Me.W.PD)
            Me.SincronizzaP(Me.W.CQSI)
            Me.SincronizzaP(Me.W.PDI)
        End Sub

        Private Sub SincronizzaSF(ByVal sf As CQSPDConsulenza)
            If (sf Is Nothing) Then Exit Sub
            sf.SetOffertaCQS(Me.Offerte.GetItemById(sf.IDOffertaCQS))
            sf.SetOffertaPD(Me.Offerte.GetItemById(sf.IDOffertaPD))
        End Sub

        Private Sub SincronizzaP(ByVal p As CRapportino)
            If (p Is Nothing) Then Exit Sub
            Dim col As New CStatiLavorazionePraticaCollection
            For Each stl In Me.StatiLavorazione
                If stl.IDPratica = GetID(p) Then
                    stl.SetPratica(p)
                    stl.SetOfferta(Me.Offerte.GetItemById(stl.IDOfferta))
                    col.Add(stl)
                End If
            Next
            col.SetPratica(p)
            col.Sort()
            p.SetStatiDiLavorazione(col)
        End Sub

        Private Sub AggiungiOfferteSF(ByVal sf As CQSPDConsulenza)
            If (sf Is Nothing) Then Exit Sub
            If (Me.Offerte.GetItemById(sf.IDOffertaCQS) Is Nothing AndAlso sf.OffertaCQS IsNot Nothing) Then Me.Offerte.Add(sf.OffertaCQS)
            If (Me.Offerte.GetItemById(sf.IDOffertaPD) Is Nothing AndAlso sf.OffertaPD IsNot Nothing) Then Me.Offerte.Add(sf.OffertaPD)
        End Sub

        Private Sub AggiungiOfferteP(ByVal p As CRapportino)
            If (p Is Nothing) Then Exit Sub
            For Each sl As CStatoLavorazionePratica In p.StatiDiLavorazione
                If Me.StatiLavorazione.GetItemById(GetID(sl)) Is Nothing Then Me.StatiLavorazione.Add(sl)
                If Me.Offerte.GetItemById(sl.IDOfferta) Is Nothing AndAlso sl.Offerta IsNot Nothing Then Me.Offerte.Add(sl.Offerta)
            Next
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "W" : Me.W = fieldValue
                Case "RF" : Me.W.SetRichiestaFinanziamento(fieldValue)
                Case "SF" : Me.W.SetStudioDiFattibilita(fieldValue)
                Case "CQS" : Me.W.SetCQS(fieldValue)
                Case "PD" : Me.W.SetPD(fieldValue)
                Case "CQSI" : Me.W.SetCQSI(fieldValue)
                Case "PDI" : Me.W.SetPDI(fieldValue)
                Case "Offerte" : Me.Offerte.Clear() : Me.Offerte.AddRange(fieldValue)
                Case "STLP" : Me.StatiLavorazione.Clear() : Me.StatiLavorazione.AddRange(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("W", Me.W)
            writer.WriteTag("RF", Me.W.RichiestaFinanziamento)
            writer.WriteTag("SF", Me.W.StudioDiFattibilita)
            writer.WriteTag("CQS", Me.W.CQS)
            writer.WriteTag("PD", Me.W.PD)
            writer.WriteTag("CQSI", Me.W.CQSI)
            writer.WriteTag("PDI", Me.W.PDI)
            writer.WriteTag("STLP", Me.StatiLavorazione)
            writer.WriteTag("Offerte", Me.Offerte)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class
