Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV


    ''' <summary>
    ''' Gestore generico delle campagne SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class HandlerTipoCampagnaSMS
        Inherits HandlerTipoCampagna

        Private Shared m_Instance As HandlerTipoCampagnaSMS = Nothing

        Private Sub New()
            AddHandler Sistema.SMSService.SMSReceived, AddressOf handleSMSReceived
        End Sub

        Private Sub handleSMSReceived(ByVal sender As Object, ByVal e As SMSReceivedEventArgs)
            Debug.Print(e.MessageID)
        End Sub


        ''' <summary>
        ''' Restituisce l'istanza predefinita dell'handler
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Instance As HandlerTipoCampagnaSMS
            Get
                If (m_Instance Is Nothing) Then m_Instance = New HandlerTipoCampagnaSMS
                Return m_Instance
            End Get
        End Property

        Public Overrides Function GetHandledType() As TipoCampagnaPubblicitaria
            Return TipoCampagnaPubblicitaria.SMS
        End Function

        Public Overrides Function GetNomeMezzoSpedizione() As String
            Return "SMS"
        End Function

        Public Overrides Function IsBanned(res As CRisultatoCampagna) As Boolean
            Return ADV.Configuration.BannedSMSAddresses.IsBanned(res.IndirizzoDestinatario)
        End Function

        Public Overrides Function IsBlocked(res As CRisultatoCampagna) As Boolean
            Return (CustomerCalls.BlackListAdresses.CheckBlocked("Cellulare", res.IndirizzoDestinatario) IsNot Nothing) OrElse _
                   (CustomerCalls.BlackListAdresses.CheckBlocked("Telefono", res.IndirizzoDestinatario) IsNot Nothing)
        End Function

        Public Overrides Sub Send(ByVal item As CRisultatoCampagna)
            Dim testo As String

            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (item.Campagna Is Nothing) Then Throw New ArgumentNullException("item.Campagna")

            If (SMSService.DefaultDriver Is Nothing) Then Throw New InvalidOperationException("Nessun driver installato per inviare SMS")

            If Not SMSService.IsValidNumber(item.IndirizzoDestinatario) Then Exit Sub

            Dim options As New SMSDriverOptions
            options.Mittente = item.Campagna.NomeMittente
            options.RichiediConfermaDiLettura = item.Campagna.RichiediConfermaDiLettura

            Try
                testo = item.ParseTemplate(item.Campagna.Testo)
            Catch ex As Exception
                Throw New InvalidOperationException("Errore durante il parsing del testo della campagna", ex)
            End Try

            Try
                Dim msgID As String = SMSService.Send(item.IndirizzoDestinatario, testo, options)
                item.MessageID = msgID
                Dim stato As MessageStatus = SMSService.GetStatus(msgID)
                Select Case stato.MessageStatus
                    Case MessageStatusEnum.BadNumber : item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : item.StatoSpedizioneEx = "Numero errato"
                    Case MessageStatusEnum.Delivered : item.StatoMessaggio = StatoMessaggioCampagna.Inviato : item.DataConsegna = stato.DeliveryTime
                    Case MessageStatusEnum.Error : item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : item.StatoSpedizioneEx = stato.MessageStatusEx
                    Case MessageStatusEnum.Scheduled : item.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione : item.StatoSpedizioneEx = "Invio ritardato"
                    Case MessageStatusEnum.Sent : item.StatoMessaggio = StatoMessaggioCampagna.Inviato
                    Case MessageStatusEnum.Timeout : item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : item.StatoSpedizioneEx = "Timeout"
                    Case MessageStatusEnum.Waiting : item.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione : item.StatoSpedizioneEx = "Ritardato"
                    Case Else : item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : item.StatoSpedizioneEx = stato.MessageStatusEx
                End Select
            Catch ex As Exception
                Throw New InvalidOperationException("Errore durante l'invio del messaggio", ex)
            End Try
        End Sub

        Public Overrides Function PrepareResults(campagna As CCampagnaPubblicitaria, item As CPersona) As CCollection(Of CRisultatoCampagna)
            If (campagna Is Nothing) Then Throw New ArgumentNullException("campagna")
            If (item Is Nothing) Then Throw New ArgumentNullException("item")

            Dim ret As New CCollection(Of CRisultatoCampagna)

            For Each contatto As CContatto In item.Recapiti
                If ((LCase(Left(contatto.Valore, 1)) = "3") AndAlso SMSService.IsValidNumber(contatto.Valore) AndAlso (Not campagna.UsaSoloIndirizziVerificati OrElse contatto.Validated)) Then
                    Dim t As Boolean = False
                    For Each tmp As CRisultatoCampagna In ret
                        If tmp.IndirizzoDestinatario = contatto.Valore Then
                            t = True
                            Exit For
                        End If
                    Next
                    If (Not t) Then
                        Dim r As New CRisultatoCampagna
                        r.IndirizzoDestinatario = contatto.Valore
                        ret.Add(r)
                    End If
                    If (campagna.UsaUnSoloContattoPerPersona) Then Exit For
                End If
            Next

            Return ret
        End Function

        'Public Overrides Function GetListaInvio(ByVal campagna As CCampagnaPubblicitaria) As CCollection(Of CRisultatoCampagna)
        '    If (campagna Is Nothing) Then Throw New ArgumentNullException("campagna")

        '    Dim ret As New CCollection(Of CRisultatoCampagna)
        '    Dim res As CRisultatoCampagna


        '    If (campagna.UsaListaDinamica) Then
        '        Dim items As CCollection(Of CPersonaInfo) = Anagrafica.Persone.Find(campagna.ParametriLista, True)
        '        If (items Is Nothing) Then Throw New InvalidOperationException("La funzione Find ha restituito NULL")
        '        For Each item As CPersonaInfo In items
        '            Dim cc As CCollection(Of CRisultatoCampagna) = Me.PrepareResults(campagna, item.Persona)
        '            For Each res In cc
        '                res.Stato = ObjectStatus.OBJECT_VALID
        '                res.Campagna = campagna
        '                res.Destinatario = item.Persona
        '                res.TipoCampagna = campagna.TipoCampagna
        '                res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
        '                ret.Add(res)
        '            Next
        '        Next
        '    Else
        '        Dim addresses() As String = Split(campagna.ParametriLista, ";")
        '        For Each address In addresses
        '            res = New CRisultatoCampagna
        '            Me.ParseAddress(address, res.NomeDestinatario, res.IndirizzoDestinatario)
        '            If (res.IndirizzoDestinatario <> "") Then
        '                res.Stato = ObjectStatus.OBJECT_VALID
        '                res.Campagna = campagna
        '                res.TipoCampagna = campagna.TipoCampagna
        '                res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
        '                ret.Add(res)
        '            End If
        '        Next
        '    End If


        '    Return ret
        'End Function

        Protected Overrides Sub ParseAddress(ByVal str As String, ByRef nome As String, ByRef address As String)
            str = Replace(Trim(str), "  ", " ")
            str = Replace(str, vbCr, "")
            str = Replace(str, vbLf, "")

            nome = ""
            address = ""

            If (str = "") Then Exit Sub

            Dim i As Integer = InStr(str, "<")
            nome = ""
            address = ""
            If (i > 0) Then
                If (i > 1) Then
                    nome = Left(str, i - 1)
                    address = Trim(Mid(str, i + 1))
                    i = InStr(address, ">")
                    If (i > 0) Then
                        address = Trim(Left(address, i - 1))
                    End If
                Else
                    address = str
                End If
            Else
                address = str
            End If
            address = Formats.ParsePhoneNumber(address)
        End Sub

       
        Public Overrides Function SupportaConfermaLettura() As Boolean
            Return False
        End Function

        Public Overrides Function SupportaConfermaRecapito() As Boolean
            If (SMSService.DefaultDriver Is Nothing) Then Throw New InvalidOperationException("Nessun driver installato per inviare SMS")
            Return SMSService.DefaultDriver.SupportaConfermaDiRecapito
        End Function


        Public Overrides Sub UpdateStatus(ByVal res As CRisultatoCampagna)
            If (SMSService.DefaultDriver Is Nothing) Then Throw New InvalidOperationException("Nessun driver installato per inviare SMS")
            Try
                Dim status As MessageStatus = SMSService.GetStatus(res.MessageID)

                Select Case status.MessageStatus
                    Case MessageStatusEnum.BadNumber : res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : res.StatoSpedizioneEx = "Numero errato"
                    Case MessageStatusEnum.Delivered : res.StatoMessaggio = StatoMessaggioCampagna.Inviato : res.DataConsegna = status.DeliveryTime : res.StatoSpedizioneEx = "Inviato"
                    Case MessageStatusEnum.Error : res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : res.StatoSpedizioneEx = status.MessageStatusEx
                    Case MessageStatusEnum.Scheduled : res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione : res.StatoSpedizioneEx = "Invio ritardato"
                    Case MessageStatusEnum.Sent : res.StatoMessaggio = StatoMessaggioCampagna.Inviato : res.StatoSpedizioneEx = "Inviato"
                    Case MessageStatusEnum.Timeout : res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : res.StatoSpedizioneEx = "Timeout"
                    Case MessageStatusEnum.Waiting : res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione : res.StatoSpedizioneEx = "Ritardato"
                    Case Else : res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore : res.StatoSpedizioneEx = status.MessageStatusEx
                End Select
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try
        End Sub

        Protected Overrides Function IsValidAddress(ByRef address As String) As Boolean
            Return SMSService.IsValidNumber(address)
        End Function

    End Class

End Class