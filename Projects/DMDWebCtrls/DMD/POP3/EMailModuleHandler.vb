Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Office
Imports DMD.WebSite
Imports DMD.Anagrafica

Imports DMD.Databases
Imports DMD.Net

Namespace Forms

 

    Public Class EMailModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New DMD.Office.MailMessageCursor
        End Function


        Public Function DownloadMessages(ByVal renderer As Object) As String
            Dim appID As Integer = RPC.n2int(Me.GetParameter(renderer, "appid", ""))
            ' Dim app As MailApplication = MailApplications.GetItemById(appID)
            Dim nitems As Integer? = RPC.n2int(GetParameter(renderer, "nitems", ""))
            Dim downloadedItems As New CCollection(Of MailMessage)
            'If (nitems.HasValue) Then
            '    downloadedItems = app.DownloadEmails(nitems.Value)
            'Else
            '    downloadedItems = app.DownloadEmails()
            'End If
            'app.CheckEMails()
            Return XML.Utils.Serializer.Serialize(downloadedItems)
        End Function


        'Public Function SendReceive() As String
        '    Dim appID As Integer = RPC.n2int(Me.GetParameter(renderer, "appid", ""))
        '    Dim app As MailApplication = MailApplications.GetItemById(appID)
        '    Dim nitems As Integer? = RPC.n2int(GetParameter(renderer, "nitems", ""))
        '    Dim downloadedItems As CCollection(Of MailMessage)
        '    If (nitems.HasValue) Then
        '        downloadedItems = app.SendReceive(nitems.Value)
        '    Else
        '        downloadedItems = app.SendReceive()
        '    End If
        '    Return XML.Utils.Serializer.Serialize(downloadedItems)
        'End Function

        Public Function NotifyChanges(ByVal renderer As Object) As String
            Dim appID As Integer = RPC.n2int(Me.GetParameter(renderer, "appid", ""))
            'Dim app As MailApplication = MailApplications.GetItemById(appID)
            ' app.NotifyChanges()
            Return ""
        End Function



    End Class



End Namespace