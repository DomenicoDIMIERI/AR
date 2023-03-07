Imports Microsoft.VisualBasic
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Sistema

Namespace Forms

    Public Class CRMEditActionHandler
        Inherits DMD.Sistema.CBaseEventHandler


        Public Overrides Sub NotifyEvent(ByVal e As EventDescription)
            Dim toAddress As String = CustomerCalls.CRM.Module.Settings.GetValueString("NotifyChangesTo", "")
            If (toAddress <> "") Then
                Dim item As CContattoUtente = e.Descrittore
                If (item.Persona.TipoPersona = TipoPersona.PERSONA_FISICA) Then
                    Select Case item.Scopo
                        Case "Richiesta Conteggio Estintivo"
                            Dim text As String = "Il cliente " & item.NomePersona & " (CF: " & Formats.FormatCodiceFiscale(item.Persona.CodiceFiscale) & ") ha richiesto un conteggio estintivo all'operatore " & item.NomeOperatore & " dell'ufficio di " & item.NomePuntoOperativo
                            Dim m As System.Net.Mail.MailMessage = DMD.Sistema.EMailer.PrepareMessage("robot@DMD.net", toAddress, "", "", text, e.Descrizione, "", False)
                            EMailer.SendMessageAsync(m, True)
                    End Select
                End If
            End If
        End Sub

    End Class

End Namespace