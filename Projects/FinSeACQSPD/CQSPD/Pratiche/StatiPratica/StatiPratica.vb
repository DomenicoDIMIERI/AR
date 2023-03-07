Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    '.---------------------------------------------

    ''' <summary>
    ''' Classe che consente di accedere agli stati pratica
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CStatiPraticaClass
        Inherits CGeneralClass(Of CStatoPratica)

        Friend Sub New()
            MyBase.New("CQSPDStatPrat", GetType(CStatoPraticaCursor), -1)
        End Sub

        Public Function FormatMacroStato(ByVal stato As Nullable(Of StatoPraticaEnum)) As String
            Return DMD.CQSPD.Pratiche.FormatStatoPratica(stato)
        End Function
         

        ''' <summary>
        ''' Restituisce lo stato pratica iniziale (per una nuova pratica)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDefault() As CStatoPratica
            Return DMD.CQSPD.Configuration.StatoPredefinito
        End Function

        Public Function GetStatiSuccessivi(ByVal statoAttuale As CStatoPratica) As CStatoPratRulesCollection
            Return statoAttuale.StatiSuccessivi
        End Function

        Public Function GetStatiSuccessivi(ByVal pratica As CRapportino) As CCollection(Of CStatoPratRule)
            Dim ret As New CCollection(Of CStatoPratRule)
            Dim ra As CRichiestaApprovazione = pratica.RichiestaApprovazione
            If (ra IsNot Nothing AndAlso ra.StatoRichiesta <> StatoRichiestaApprovazione.APPROVATA) Then
                Dim stAnnullata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
                Dim rule As CStatoPratRule
                For Each rule In pratica.StatoAttuale.StatiSuccessivi
                    If (rule.IDTarget = GetID(stAnnullata)) Then ret.Add(rule)
                Next
            ElseIf (pratica.StatoAttuale IsNot Nothing) Then
                If (pratica.StatoAttuale.StatiSuccessivi.Count > 0) Then ret.AddRange(pratica.StatoAttuale.StatiSuccessivi.ToArray)
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CCollection contenente tutti gli stati attivi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStatiAttivi() As CCollection(Of CStatoPratica)
            Dim ret As New CCollection(Of CStatoPratica)
            For Each item As CStatoPratica In Me.LoadAll
                If (item.Attivo) Then ret.Add(item)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il valore corrispondente al vecchio sistema secondo il campo OldStatus di compatibilità
        ''' </summary>
        ''' <param name="ms"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByCompatibleID(ByVal ms As StatoPraticaEnum) As CStatoPratica
            For Each item As CStatoPratica In Me.LoadAll
                If (item.MacroStato.HasValue AndAlso item.MacroStato.Value = ms) Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetSequenzaStandard() As CCollection(Of CStatoPratica)
            Dim stato As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
            Dim items As New CCollection(Of CStatoPratica)
            While stato IsNot Nothing
                items.Add(stato)
                stato = stato.DefaultTarget
            End While
            stato = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
            If (stato IsNot Nothing) Then items.Add(stato)
            Return items
        End Function

        Function StatoContatto() As CStatoPratica
            Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
            If (ret Is Nothing) Then
                ret = New CStatoPratica
                ret.MacroStato = StatoPraticaEnum.STATO_CONTATTO
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Nome = "SECCI"
                ret.Save()
            End If
            Return ret
        End Function

        Function StatoRichiestaDelibera() As CStatoPratica
            Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_RICHIESTADELIBERA)
            If (ret Is Nothing) Then
                ret = New CStatoPratica
                ret.MacroStato = StatoPraticaEnum.STATO_RICHIESTADELIBERA
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Nome = "Richiesta Delibera"
                ret.Save()
            End If
            Return ret
        End Function

        Function StatoLiquidato() As CStatoPratica
            Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
            If (ret Is Nothing) Then
                ret = New CStatoPratica
                ret.MacroStato = StatoPraticaEnum.STATO_LIQUIDATA
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Nome = "Liquidata"
                ret.Save()
            End If
            Return ret
        End Function

        Function StatoAnnullato() As CStatoPratica
            Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
            If (ret Is Nothing) Then
                ret = New CStatoPratica
                ret.MacroStato = StatoPraticaEnum.STATO_ANNULLATA
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Nome = "Annullata"
                ret.Save()
            End If
            Return ret
        End Function

        Function StatoArchiviato() As CStatoPratica
            Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
            If (ret Is Nothing) Then
                ret = New CStatoPratica
                ret.MacroStato = StatoPraticaEnum.STATO_ARCHIVIATA
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Nome = "Archiviata"
                ret.Save()
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce lo stato di pre-caricamento
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function StatoPreCaricamento() As CStatoPratica
            Dim ret As CStatoPratica = Me.GetItemByCompatibleID(StatoPraticaEnum.STATO_PRECARICAMENTO)
            If (ret Is Nothing) Then
                ret = New CStatoPratica
                ret.MacroStato = StatoPraticaEnum.STATO_PRECARICAMENTO
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Nome = "Pre-Caricamento"
                ret.Save()
            End If
            Return ret
        End Function


    End Class


    Private Shared m_StatiPratica As CStatiPraticaClass = Nothing

    Public Shared ReadOnly Property StatiPratica As CStatiPraticaClass
        Get
            If (m_StatiPratica Is Nothing) Then m_StatiPratica = New CStatiPraticaClass
            Return m_StatiPratica
        End Get
    End Property

End Class
