Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV



    Public Class HandlerTipoCampagnaEMail
        Inherits HandlerTipoCampagna

        Private Shared m_Instance As HandlerTipoCampagnaEMail = Nothing

     
        ''' <summary>
        ''' Restituisce l'istanza predefinita dell'handler
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Instance As HandlerTipoCampagnaEMail
            Get
                If (m_Instance Is Nothing) Then m_Instance = New HandlerTipoCampagnaEMail
                Return m_Instance
            End Get
        End Property


        Private Sub New()
            AddHandler Sistema.EMailer.MessageReceived, AddressOf handleMessageReceived
        End Sub

        Private Sub handleMessageReceived(ByVal sender As Object, ByVal e As MailMessageEventArgs)
            Dim key As String = ""
            key = e.Message.Headers("FSEMLHNDLR")
            Debug.Print("Message received: " & key)
        End Sub

        Public Overrides Function GetHandledType() As TipoCampagnaPubblicitaria
            Return TipoCampagnaPubblicitaria.eMail
        End Function

        Public Overrides Function GetNomeMezzoSpedizione() As String
            Return "e-mail"
        End Function

        Public Overrides Function IsBanned(res As CRisultatoCampagna) As Boolean
            Return ADV.Configuration.BannedEMailAddresses.IsBanned(res.IndirizzoDestinatario) 
        End Function

        Public Overrides Function IsBlocked(res As CRisultatoCampagna) As Boolean
            Return (CustomerCalls.BlackListAdresses.CheckBlocked("e-Mail", res.IndirizzoDestinatario) IsNot Nothing)
        End Function

        Public Overrides Sub Send(item As CRisultatoCampagna)
            If Not EMailer.IsValidAddress(item.IndirizzoDestinatario) Then Exit Sub

            Dim fromAddress As String = IIf(item.Campagna.IndirizzoMittente <> "", item.Campagna.IndirizzoMittente, "")
            If (fromAddress = "") Then Throw New ArgumentNullException("Il mittente non può essere nullo")

            Dim titolo As String = item.ParseTemplate(item.Campagna.Titolo)
            Dim testo As String = item.ParseTemplate(item.Campagna.Testo)
            Dim toAddress As String = IIf(ApplicationContext.IsDebug, "tecnico@DMD.net", item.IndirizzoDestinatario)
            Dim m As MailMessage = EMailer.PrepareMessage(fromAddress, toAddress, "", "", titolo, testo, item.Campagna.NomeMittente, True)
            If (item.Campagna.RichiediConfermaDiLettura) Then
                m.Headers.Add("Disposition-Notification-To", item.Campagna.IndirizzoMittente)
                Dim key As String = ASPSecurity.GetRandomKey(25)
                m.Headers.Add("FSEMLHNDLR", key)
                'm.DeliveryNotificationOptions =
            End If
            Sistema.EMailer.SendMessageAsync(m, True)
            'EMailer.SendMessage(m)
            'EMailer.DisposeMessage(m)

            item.StatoMessaggio = StatoMessaggioCampagna.Inviato
            item.StatoSpedizioneEx = "Inviato"
            item.MessageID = ""
        End Sub



        Public Overrides Function PrepareResults(campagna As CCampagnaPubblicitaria, item As CPersona) As CCollection(Of CRisultatoCampagna)
            Dim ret As New CCollection(Of CRisultatoCampagna)
            For Each contatto As CContatto In item.Recapiti
                If LCase(contatto.Tipo) = "e-mail" AndAlso EMailer.IsValidAddress(contatto.Valore) AndAlso (Not campagna.UsaSoloIndirizziVerificati OrElse (contatto.Validated.HasValue = False) OrElse contatto.Validated.Value) Then
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
        End Sub

       
        Public Overrides Function SupportaConfermaLettura() As Boolean
            Return True
        End Function

        Public Overrides Function SupportaConfermaRecapito() As Boolean
            Return False
        End Function

        Public Overrides Sub UpdateStatus(res As CRisultatoCampagna)

        End Sub

        Protected Overrides Function IsValidAddress(ByRef address As String) As Boolean
            Return EMailer.IsValidAddress(address)
        End Function

        Public Overrides Function GetListaInvio(ByVal campagna As CCampagnaPubblicitaria) As CCollection(Of CRisultatoCampagna)
            Dim ret As New CCollection(Of CRisultatoCampagna)
            Dim cc As CCollection(Of CRisultatoCampagna)
            Dim res As CRisultatoCampagna
            
            If (campagna.UsaListaDinamica AndAlso campagna.ParametriLista = "*") Then
                'Dim dbSQL As String = "SELECT * FROM [tbl_Persone] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND ([eMail1] <> '' Or [eMail2]<>'' Or [WebSite1]<>'' Or [WebSite2]<>'')"
                'Dim dbRis As System.Data.IDataReader = Nothing
                Dim cursor As CPersonaCursor = Nothing
                Try
                    cursor = New CPersonaCursor
                    'dbRis = CRM.Database.ExecuteReader(dbSQL)
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.eMail.Value = ""
                    cursor.eMail.Operator = OP.OP_NE
                    cursor.eMail.IncludeNulls = False
                    While Not cursor.EOF ' dbRis.Read
                        Dim persona As CPersona = cursor.Item ' Anagrafica.Persone.Instantiate(Formats.ToInteger(dbRis("TipoPersona")))
                        'CRM.Database.Load(persona, dbRis)
                        Dim consenso As Nullable(Of Boolean) = persona.GetFlag(PFlags.CF_CONSENSOADV)
                        If (Not consenso.HasValue OrElse consenso.Value) Then
                            cc = Me.PrepareResults(campagna, persona)
                            For Each res In cc
                                res.Stato = ObjectStatus.OBJECT_VALID
                                res.Campagna = campagna
                                res.Destinatario = persona
                                res.TipoCampagna = campagna.TipoCampagna
                                res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
                                If Not Me.IsExcluded(res) Then ret.Add(res)
                            Next
                        End If
                        cursor.MoveNext()
                    End While
                Catch ex As Exception
                    Throw
                Finally
                    'If (dbRis IsNot Nothing) Then dbRis.Dispose()
                    'dbRis = Nothing
                    If (cursor IsNot Nothing) Then cursor.Dispose()
                End Try
            Else
                ret = MyBase.GetListaInvio(campagna)
            End If
            Return ret
        End Function
         

    End Class

End Class