Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV

    ''' <summary>
    ''' Handler per le spedizioni
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class HandlerTipoCampagna


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Public MustOverride Function SupportaConfermaRecapito() As Boolean

        Public MustOverride Function SupportaConfermaLettura() As Boolean

        Public MustOverride Function GetHandledType() As TipoCampagnaPubblicitaria

        Public MustOverride Function GetNomeMezzoSpedizione() As String

        Public MustOverride Sub Send(ByVal item As CRisultatoCampagna)

        Public MustOverride Function PrepareResults(ByVal campagna As CCampagnaPubblicitaria, ByVal item As CPersona) As CCollection(Of CRisultatoCampagna)

        Protected Overridable Function IsExcluded(ByVal res As CRisultatoCampagna) As Boolean
            Dim items() As String = Split(res.Campagna.ListaNO, vbNewLine)
            Dim nome, indirizzo As String
            For Each value As String In items
                nome = "" : indirizzo = ""
                Me.ParseAddress(value, nome, indirizzo)
                If (res.IndirizzoDestinatario Like indirizzo) Then Return True
            Next
            Return False
        End Function

        Public MustOverride Function IsBanned(ByVal res As CRisultatoCampagna) As Boolean

        Public MustOverride Function IsBlocked(ByVal res As CRisultatoCampagna) As Boolean


        Public Overridable Function GetListaInvio(ByVal campagna As CCampagnaPubblicitaria) As CCollection(Of CRisultatoCampagna)
            'If (Me.UsaListaDinamica) Then
            Dim items As CCollection(Of CPersonaInfo)
            Dim ret As New CCollection(Of CRisultatoCampagna)
            Dim cc As CCollection(Of CRisultatoCampagna)
            Dim res As CRisultatoCampagna
            Dim addresses() As String

            If (campagna.UsaListaDinamica) Then
                If (campagna.ParametriLista = "*") Then
                    Dim cursor As New CPersonaCursor
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IgnoreRights = True
                    Dim t As Double = Timer
                    While Not cursor.EOF
                        Dim consenso As Nullable(Of Boolean) = cursor.Item.GetFlag(PFlags.CF_CONSENSOADV)
                        If (Not consenso.HasValue OrElse consenso.Value) Then
                            cc = Me.PrepareResults(campagna, cursor.Item)
                            For Each res In cc
                                res.Stato = ObjectStatus.OBJECT_VALID
                                res.Campagna = campagna
                                res.Destinatario = cursor.Item
                                res.TipoCampagna = campagna.TipoCampagna
                                res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
                                If Not Me.IsExcluded(res) Then ret.Add(res)
                            Next
                        End If
                        cursor.MoveNext()
                    End While
                    cursor.Dispose()
                    Debug.Print("Time: " & (Timer - t) & " s")
                Else
                    Dim filter As New CRMFinlterI
                    filter.text = campagna.ParametriLista
                    filter.ignoreRights = True
                    filter.nMax = Nothing
                    items = Anagrafica.Persone.Find(filter)
                    For Each item As CPersonaInfo In items
                        Dim consenso As Nullable(Of Boolean) = Nothing
                        If (item.Persona IsNot Nothing) Then consenso = item.Persona.GetFlag(PFlags.CF_CONSENSOADV)
                        If (Not consenso.HasValue OrElse consenso.Value) Then
                            cc = Me.PrepareResults(campagna, item.Persona)
                            For Each res In cc
                                res.Stato = ObjectStatus.OBJECT_VALID
                                res.Campagna = campagna
                                res.Destinatario = item.Persona
                                res.TipoCampagna = campagna.TipoCampagna
                                res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
                                If Not Me.IsExcluded(res) Then ret.Add(res)
                            Next
                        End If
                    Next
                End If
            Else
                addresses = Split(campagna.ParametriLista, ";")
                For Each address In addresses
                    res = New CRisultatoCampagna
                    Me.ParseAddress(address, res.NomeDestinatario, res.IndirizzoDestinatario)
                    If Me.IsValidAddress(res.IndirizzoDestinatario) Then
                        res.Stato = ObjectStatus.OBJECT_VALID
                        res.Campagna = campagna
                        res.TipoCampagna = campagna.TipoCampagna
                        res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
                        If Not Me.IsExcluded(res) Then ret.Add(res)
                    End If
                Next
            End If

            If (campagna.ListaCC <> "") Then
                addresses = Split(campagna.ListaCC, ";")
                For Each address In addresses
                    res = New CRisultatoCampagna
                    Me.ParseAddress(address, res.NomeDestinatario, res.IndirizzoDestinatario)
                    If Me.IsValidAddress(res.IndirizzoDestinatario) Then
                        res.Stato = ObjectStatus.OBJECT_VALID
                        res.Campagna = campagna
                        res.TipoCampagna = campagna.TipoCampagna
                        res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
                        If Not Me.IsExcluded(res) Then ret.Add(res)
                    End If
                Next
            End If

            If (campagna.ListaCCN <> "") Then
                addresses = Split(campagna.ListaCCN, ";")
                For Each address In addresses
                    res = New CRisultatoCampagna
                    Me.ParseAddress(address, res.NomeDestinatario, res.IndirizzoDestinatario)
                    If Me.IsValidAddress(res.IndirizzoDestinatario) Then
                        res.Stato = ObjectStatus.OBJECT_VALID
                        res.Campagna = campagna
                        res.TipoCampagna = campagna.TipoCampagna
                        res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
                        If Not Me.IsExcluded(res) Then ret.Add(res)
                    End If
                Next
            End If

            Return ret
        End Function

        Protected MustOverride Sub ParseAddress(ByVal str As String, ByRef nome As String, ByRef address As String)

        Protected MustOverride Function IsValidAddress(ByRef address As String) As Boolean

        Public Overrides Function ToString() As String
            Return Me.GetNomeMezzoSpedizione
        End Function

        ''' <summary>
        ''' Aggiorna lo stato del messaggio inviato
        ''' </summary>
        ''' <param name="res"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub UpdateStatus(ByVal res As CRisultatoCampagna)

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class