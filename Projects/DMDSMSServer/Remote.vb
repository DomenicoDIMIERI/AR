Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports System.Net.Mail
Imports FinSeA.Sistema
Imports FinSeA
Imports FinSeA.Anagrafica
Imports FinSeA.Databases
Imports System.Net
Imports FinSeA.SMSGateway

Public Class Remote
    Public lResolve As Integer = 60 * 1000
    Public lConnect As Integer = 60 * 1000
    Public lSend As Integer = 120 * 1000
    Public lReceive As Integer = 120 * 1000

    Public Shared Event UserLoggedIn(ByVal sender As Object, ByVal e As UserLoginEventArgs)
    Public Shared Event UserLoggedOut(ByVal sender As Object, ByVal e As UserLogoutEventArgs)
    Public Shared Event UploadProgress(ByVal sender As Object, ByVal e As UploadProgressChangedEventArgs)
    Public Shared Event UploadCompleted(ByVal sender As Object, ByVal e As UploadFileCompletedEventArgs)

    Public Shared WithEvents client As New WebClient
#If DEBUG Then
    Const serverName As String = "http://localhost:13423"
#Else
    const serverName as string = "http://www.prestitidonato.it"
#End If

    Public Shared Function GetMessagesToSend() As CCollection(Of SMSMessage)
        Dim url As String = "/widgets/websvcf/gsmgtw.aspx?_a=GetMessagesToSend"
        Dim text As String = XML.Utils.Serializer.DeserializeString(RPC.InvokeMethod(serverName & url))
        Return XML.Utils.Serializer.Deserialize(Text)
    End Function


    Private Shared Sub client_UploadFileCompleted(sender As Object, e As UploadFileCompletedEventArgs) Handles client.UploadFileCompleted
        RaiseEvent UploadCompleted(sender, e)
    End Sub

    Private Shared Sub client_UploadProgressChanged(sender As Object, e As UploadProgressChangedEventArgs) Handles client.UploadProgressChanged
        RaiseEvent UploadProgress(sender, e)
    End Sub


End Class

