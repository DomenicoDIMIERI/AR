Imports Microsoft.VisualBasic
Imports DMD.Sistema

Namespace Forms

    Public Class CRapportiniCreateEventHandler
        Inherits Sistema.CBaseEventHandler

        Public Overrides Sub NotifyEvent(ByVal e As EventDescription)
            Dim msg As String = Users.CurrentUser.UserName & " (" & Users.CurrentUser.Nominativo & ") - " & e.Descrizione
            Dim m As System.Net.Mail.MailMessage = DMD.Sistema.EMailer.PrepareMessage(DMD.CQSPD.Configuration.FromAddress, DMD.CQSPD.Configuration.NotifyChangesTo, "", "", msg, msg, "", True)
            DMD.Sistema.EMailer.SendMessageAsync(m, True)
        End Sub
    End Class

End Namespace