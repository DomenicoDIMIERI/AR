Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV



    Public Class HandlerTipoCampagnaFax
        Inherits HandlerTipoCampagna

        Private Shared m_Instance As HandlerTipoCampagnaFax = Nothing

        Private Sub New()
            AddHandler Sistema.FaxService.FaxReceived, AddressOf handleFaxReceived
        End Sub

        Private Sub handleFaxReceived(ByVal sender As Object, ByVal e As FaxReceivedEventArgs)

        End Sub

        ''' <summary>
        ''' Restituisce l'istanza predefinita dell'handler
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Instance As HandlerTipoCampagnaFax
            Get
                If (m_Instance Is Nothing) Then m_Instance = New HandlerTipoCampagnaFax
                Return m_Instance
            End Get
        End Property


        Public Overrides Function GetHandledType() As TipoCampagnaPubblicitaria
            Return TipoCampagnaPubblicitaria.Fax
        End Function

        Public Overrides Function GetNomeMezzoSpedizione() As String
            Return "Fax"
        End Function

        Public Overrides Function IsBanned(res As CRisultatoCampagna) As Boolean
            Return ADV.Configuration.BannedFaxAddresses.IsBanned(res.IndirizzoDestinatario) 
        End Function

        Public Overrides Function IsBlocked(res As CRisultatoCampagna) As Boolean
            Return (CustomerCalls.BlackListAdresses.CheckBlocked("Fax", res.IndirizzoDestinatario) IsNot Nothing) OrElse _
                   (CustomerCalls.BlackListAdresses.CheckBlocked("Telefono", res.IndirizzoDestinatario) IsNot Nothing)
        End Function

        Public Overrides Sub Send(item As CRisultatoCampagna)
            Dim testo As String

            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (item.Campagna Is Nothing) Then Throw New ArgumentNullException("item.Campagna")

            If (FaxService.DefaultDriver Is Nothing) Then Throw New InvalidOperationException("Nessun driver installato per inviare SMS")

            If Not FaxService.IsValidNumber(item.IndirizzoDestinatario) Then Exit Sub

            Dim options As New FaxDriverOptions
            options.SenderName = item.Campagna.NomeMittente
            'options.RichiediConfermaDiLettur = item.Campagna.RichiediConfermaDiLettura

            Try
                testo = item.ParseTemplate(item.Campagna.Testo)
            Catch ex As Exception
                Throw New InvalidOperationException("Errore durante il parsing del testo della campagna", ex)
            End Try

            Try
                'Dim msgID As String = FaxService.Send(item.IndirizzoDestinatario, testo, options)
                'item.MessageID = msgID
                item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore
                item.StatoSpedizioneEx = "Non implementato"
            Catch ex As Exception
                Throw New InvalidOperationException("Errore durante l'invio del messaggio", ex)
            End Try
        End Sub

        Public Overrides Function PrepareResults(campagna As CCampagnaPubblicitaria, item As CPersona) As CCollection(Of CRisultatoCampagna)
            Dim ret As New CCollection(Of CRisultatoCampagna)
            For Each contatto As CContatto In item.Recapiti
                If LCase(contatto.Tipo) = "fax" AndAlso (contatto.Valore <> "") AndAlso _
                   (Not campagna.UsaSoloIndirizziVerificati OrElse contatto.Validated) Then
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
        '    'If (Me.UsaListaDinamica) Then
        '    Dim items As CCollection(Of CPersonaInfo)
        '    Dim ret As New CCollection(Of CRisultatoCampagna)
        '    Dim cc As CCollection(Of CRisultatoCampagna)
        '    Dim res As CRisultatoCampagna


        '    If (campagna.UsaListaDinamica) Then
        '        items = Anagrafica.Persone.Find(campagna.ParametriLista, True)
        '        For Each item As CPersonaInfo In items
        '            cc = Me.PrepareResults(campagna, item.Persona)
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
            Return False
        End Function

        Public Overrides Sub UpdateStatus(res As CRisultatoCampagna)
            Throw New NotImplementedException
        End Sub

        Protected Overrides Function IsValidAddress(ByRef address As String) As Boolean
            Return FaxService.IsValidNumber(address)
        End Function

    End Class

End Class