Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses
Imports DMD.CallManagers.Actions
Imports System.ComponentModel
Imports DMD.CallManagers.Events


Namespace CallManagers




    Public Class AsteriskCallManager
        Implements IDisposable


        Private Class Worker
            Inherits BackgroundWorker

            Public o As AsteriskCallManager

            Public Sub New(ByVal o As AsteriskCallManager)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.o = o
            End Sub

            Protected Overrides Sub OnDoWork(e As DoWorkEventArgs)
                Me.Listen()
            End Sub

            Public Sub Listen()
                Dim n As Integer = 0
                Do
                    If Me.CancellationPending Then
                        Debug.Print("Worker Cencelled")
                        Return
                    End If

                    Dim buffer(1024 - 1) As Byte
                    n = Me.o.m_Socket.Receive(buffer)
                    If (n > 0) Then
                        Dim s As String = System.Text.Encoding.ASCII.GetString(buffer, 0, n)
                        SyncLock Me.o.lockObject
                            Me.o.m_Buffer &= s
                        End SyncLock
                    End If
                    SyncLock Me.o.lockObject
                        Dim p As Integer = InStr(Me.o.m_Buffer, vbCrLf & vbCrLf)
                        While (p > 0)
                            Dim data As String = Left(Me.o.m_Buffer, p - 1)
                            Me.o.m_Buffer = Mid(Me.o.m_Buffer, p + 4)
                            Me.o.ParseData(data)
                            p = InStr(Me.o.m_Buffer, vbCrLf & vbCrLf)
                        End While
                    End SyncLock
                Loop
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class

        Public Event Connected(ByVal sender As Object, ByVal e As AsteriskEventArgs)

        Public Event Disconnected(ByVal sender As Object, ByVal e As AsteriskEventArgs)


        ''' <summary>
        ''' Evento generato quando il manager ci notifica un evento remoto
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ManagerEvent(ByVal sender As Object, ByVal e As AsteriskEvent)

        ''' <summary>
        ''' Evento generato quando questo oggetto si autentica correttamente sul manager
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event LoggedIn(ByVal sender As Object, ByVal e As ManagerLoginEventArgs)

        ''' <summary>
        ''' Evento generato quando questo oggetto chiude la comunicazione
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event LoggedOut(ByVal sender As Object, ByVal e As ManagerLogoutEventArgs)


        ''' <summary>
        ''' Evento generato quando il sistema riceve una chiamata in ingresso
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Dial(ByVal sender As Object, ByVal e As DialEvent)


        Protected lockObject As New Object
        Private m_PeerVersion As String
        Private m_Asterisk As String
        Private m_Port As Integer
        Private m_Socket As Socket
        Private m_ServerEndPoint As IPEndPoint
        Private m_UserName As String
        Private m_Password As String
        Private m_Buffer As String
        Private m_Response As String
        Private m_DataLock As New ManualResetEvent(False)
        Private m_ResponseLock As New ManualResetEvent(False)
        Private Delegate Sub ListenDelegate()
        Private m_ListenDelegate As ListenDelegate
        Private m_SupportedEvents As System.Collections.Hashtable = Nothing
        Private m_LastAction As Action
        Private m_Authenticated As Boolean
        Private WithEvents m_Worker1 As Worker
        Private m_Channels As Channels
        Private m_ActionResponseQueues As ActionResponseQueues
        Private m_Peers As Peers

        Public Sub New(ByVal userName As String, ByVal password As String, ByVal asteriskServer As String, Optional ByVal port As Integer = 5038)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_UserName = userName
            Me.m_Password = password
            Me.m_Asterisk = asteriskServer
            Me.m_Port = port
            Me.m_Worker1 = Nothing
            Me.m_Channels = Nothing
        End Sub

        Public ReadOnly Property UserName As String
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property AsteriskServer As String
            Get
                Return Me.m_Asterisk
            End Get
        End Property

        Public ReadOnly Property Port As Integer
            Get
                Return Me.m_Port
            End Get
        End Property

        ''' <summary>
        ''' Restituisce vero se questa istanza è connessa ed autenticata su un server Asterisk
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsConnected() As Boolean
            Return (Me.m_Socket IsNot Nothing)
        End Function

        Public Sub Start() 'Implements DMD.CustomerCalls.ICallManager.Start
            If (Me.IsConnected) Then Throw New InvalidOperationException("Server Asterisk già connesso")

            SyncLock Me.lockObject
                Dim result As IAsyncResult
                Me.m_ServerEndPoint = New IPEndPoint(IPAddress.Parse(Me.m_Asterisk), Me.m_Port)
                Me.m_Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                result = Me.m_Socket.BeginConnect(Me.m_ServerEndPoint, Nothing, Nothing)
                Dim success As Boolean = result.AsyncWaitHandle.WaitOne(5000, True)
                If (Me.m_Socket.Connected) Then
                    Me.m_Socket.EndConnect(result)
                Else
                    ' NOTE, MUST CLOSE THE SOCKET
                    Me.m_Socket.Close()
                    Throw New ApplicationException("Failed to connect server.")
                End If

                Me.m_Buffer = ""
                Dim buffer(1024 - 1) As Byte
                Dim n As Integer = 0
                Do
                    n = Me.m_Socket.Receive(buffer)
                    If (n > 0) Then
                        Me.m_Buffer &= System.Text.Encoding.ASCII.GetString(buffer, 0, n)
                        Dim p As Integer = InStr(Me.m_Buffer, vbCrLf)
                        If (p > 0) Then
                            Me.m_PeerVersion = Left(Me.m_Buffer, p - 1)
                            Me.m_Buffer = Mid(Me.m_Buffer, p + 2)
                            Exit Do
                        End If
                    End If
                Loop While (n > 0)
                'Me.m_ListenDelegate = AddressOf Me.Listen
                'Me.m_ListenDelegate.BeginInvoke(Nothing, Nothing)
                Me.m_Worker1 = New Worker(Me)

                'If (Me.m_Worker.IsBusy = False) Then
                Me.m_Worker1.RunWorkerAsync()
            End SyncLock
            'End If

        End Sub

        Public Function IsAuthenticated() As Boolean
            Return Me.m_Authenticated
        End Function



        Public Sub [Stop]() 'Implements DMD.CustomerCalls.ICallManager.Stop
            If (Not Me.IsConnected) Then Throw New InvalidOperationException("Server Asterisk non connesso")

            SyncLock Me.lockObject
                Dim a As New Logoff
                Dim res As LogoffResponse = Me.Execute(a)
                Debug.Print(res.ToString)
                Me.m_Authenticated = False

                Me.m_Worker1.CancelAsync()
                Me.m_Worker1 = Nothing

                Me.m_Socket.Disconnect(True)
                Me.m_Socket = Nothing

            End SyncLock


            RaiseEvent Disconnected(Me, New AsteriskEventArgs(Me))
        End Sub

        Public Sub Login()
            'Me.Send("Action: login" & vbCrLf & "Username: " & Me.m_UserName & vbCrLf & "Secret: " & Me.m_Password & vbCrLf)
            If Not Me.IsConnected Then Throw New InvalidOperationException("Server Asterisk non connesso")
            Dim a As New Login(Me.m_UserName, Me.m_Password)
            Dim res As LoginResponse = Me.Execute(a)
            Me.m_Authenticated = res.IsSuccess
            If (Not Me.m_Authenticated) Then Throw New Exception("Authentication failed: " & res.Message)
            RaiseEvent LoggedIn(Me, New ManagerLoginEventArgs(Me.m_UserName, res.Message))
        End Sub

        Public Sub Logout()
            If Not Me.IsConnected Then Throw New InvalidOperationException("Server Asterisk non connesso")
            Dim a As New Logoff()
            Dim res As LogoffResponse = Me.Execute(a)
            Me.m_Authenticated = Not res.IsSuccess
            RaiseEvent LoggedOut(Me, New ManagerLogoutEventArgs())
        End Sub

        Public Overridable Function Execute(ByVal action As Action) As ActionResponse
            If (action.RequiresAuthentication AndAlso Me.IsConnected = False) Then Throw New InvalidOperationException("Il server Asterisk non è connesso")
            Me.m_ResponseLock.Reset()
            Me.m_LastAction = action
            If (TypeOf (action) Is AsyncAction) Then
                DirectCast(action, AsyncAction).SetOwner(Me)
                Me.ActionResponseQueues.Add(New ActionResponseQueue(action))
            End If
            Debug.Print(action.ToString)
            action.Send(Me)
            Me.m_ResponseLock.WaitOne()
            Return Me.m_LastAction.Response
        End Function

        ''' <summary>
        ''' Esegue l'azione specificata ed attende la risposta per un massimo di timeOutMs millisecondi
        ''' </summary>
        ''' <param name="action"></param>
        ''' <param name="timeOutMs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Execute(ByVal action As Action, ByVal timeOutMs As Integer) As ActionResponse
            If (action.RequiresAuthentication AndAlso Me.IsConnected = False) Then Throw New InvalidOperationException("Il server Asterisk non è connesso")
            Me.m_ResponseLock.Reset()
            Me.m_LastAction = action
            If (TypeOf (action) Is AsyncAction) Then
                DirectCast(action, AsyncAction).SetOwner(Me)
                Me.ActionResponseQueues.Add(New ActionResponseQueue(action))
            End If
            Debug.Print(action.ToString)
            action.Send(Me)
            Me.m_ResponseLock.WaitOne(timeOutMs)
            Return Me.m_LastAction.Response
        End Function

        ''' <summary>
        ''' Invia un comando e restituisce la risposta in maniera sincrona
        ''' </summary>
        ''' <param name="command"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Sub Send(ByVal command As String)
            Dim buffer() As Byte = System.Text.Encoding.ASCII.GetBytes(command & vbCrLf)
            Me.m_Socket.Send(buffer)
        End Sub

        Private Function GetResponse() As String
            Me.m_Buffer = ""
            Me.m_DataLock.WaitOne()
            Return Me.m_Buffer
        End Function



        Private Sub ParseData(ByVal data As String)
            Dim rows() As String = Split(data, vbCrLf)
            Dim firstRow As New RowEntry(rows(0))
            Select Case LCase(firstRow.Command)
                Case "response"
                    If (Me.m_LastAction IsNot Nothing) Then
                        Me.m_LastAction.ParseResponse(rows)
                        Me.m_ResponseLock.Set()
                    End If
                Case "event" : Me.parseEvent(firstRow, rows)
                Case Else
                    'Debug.Print("Unsupported message: " & firstRow.Command)
                    If (firstRow.Command = "" AndAlso Me.m_LastAction IsNot Nothing) Then
                        Me.m_LastAction.ParseResponse(rows)
                        Me.m_ResponseLock.Set()
                    End If
            End Select
        End Sub



        Private Sub parseEvent(ByVal r As RowEntry, ByVal rows() As String)
            Dim e As AsteriskEvent = Nothing
            Dim t As System.Type = Nothing
            If Me.SupportedEvents.ContainsKey(UCase(r.Params)) Then
                t = Me.SupportedEvents(UCase(r.Params))
            End If
            If (t Is Nothing) Then
                e = New AsteriskEvent()
                e.Parse(rows)
                Debug.Print("EVENTO NON SUPPORTATO!")
                Debug.Print(e.ToString)
            Else
                e = System.Activator.CreateInstance(t)
                e.Parse(rows)
                Debug.Print(e.ToString)
            End If
            Me.dispatchEvent(e)
        End Sub

        Protected Overridable Sub dispatchEvent(ByVal e As AsteriskEvent)
            Select Case UCase(e.EventName)
                Case "NEWCHANNEL" : Me.Channels.SetUpChannel(e)
                Case "HANGUP"
                    e = New Events.HangupEvent(Me, e)
                    Me.Channels.HangUpChannel(e)
                Case "PEERSTATUS" : Me.Peers.UpdatePeerStatus(e)
                Case "LINK", "BRIDGE", "UNLINK" : Me.Links.Update(e)
                Case "DIAL"
                    e = New DialEvent(Me, e)
                    RaiseEvent Dial(Me, e)
            End Select

            If (e.ActionID <> "") Then
                Dim q As ActionResponseQueue = Me.ActionResponseQueues.GetItemByKey(e.ActionID)
                If (q IsNot Nothing) Then
                    q.Items.Add(e)
                    q.Action.NotifyEvent(e)
                End If
            End If

            RaiseEvent ManagerEvent(Me, e)
        End Sub

        Private Function WaitResponse() As String
            Me.m_DataLock.WaitOne()
            Return Me.m_Response
        End Function


        Public ReadOnly Property SupportedEvents As System.Collections.Hashtable
            Get
                SyncLock Me.lockObject
                    If (Me.m_SupportedEvents Is Nothing) Then
                        Me.m_SupportedEvents = New System.Collections.Hashtable
                        Dim a As System.Reflection.Assembly = Me.GetType.Assembly
                        Dim list() As System.Type = a.GetTypes
                        For Each t As System.Type In list
                            If t.IsSubclassOf(GetType(AsteriskEvent)) Then
                                Dim e As AsteriskEvent = a.CreateInstance(t.FullName)
                                Me.m_SupportedEvents.Add(UCase(e.EventName), t)
                            End If
                        Next
                    End If
                    Return Me.m_SupportedEvents
                End SyncLock
            End Get
        End Property



        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Me.Stop()
            Me.m_PeerVersion = vbNullString
            Me.m_Asterisk = vbNullString
            If (Me.m_Socket IsNot Nothing) Then Me.m_Socket.Close() : Me.m_Socket = Nothing
            Me.m_ServerEndPoint = Nothing
            Me.m_UserName = vbNullString
            Me.m_Password = vbNullString
            Me.m_Buffer = vbNullString
            Me.m_Response = vbNullString
            If (Me.m_DataLock IsNot Nothing) Then Me.m_DataLock.Dispose() : Me.m_DataLock = Nothing
            If (Me.m_ResponseLock IsNot Nothing) Then Me.m_ResponseLock.Dispose() : Me.m_ResponseLock = Nothing
            Me.m_ListenDelegate = Nothing
            Me.m_SupportedEvents = Nothing
            Me.m_LastAction = Nothing
            If (Me.m_Worker1 IsNot Nothing) Then Me.m_Worker1.Dispose() : Me.m_Worker1 = Nothing
            Me.m_Channels = Nothing
            Me.m_ActionResponseQueues = Nothing
            Me.m_Peers = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property Channels As Channels
            Get
                If (Me.m_Channels Is Nothing) Then Me.m_Channels = New Channels(Me)
                Return Me.m_Channels
            End Get
        End Property

        Public ReadOnly Property ActionResponseQueues As ActionResponseQueues
            Get
                If (Me.m_ActionResponseQueues Is Nothing) Then Me.m_ActionResponseQueues = New ActionResponseQueues(Me)
                Return Me.m_ActionResponseQueues
            End Get
        End Property

        Public ReadOnly Property Peers As Peers
            Get
                If (Me.m_Peers Is Nothing) Then Me.m_Peers = New Peers(Me)
                Return Me.m_Peers
            End Get
        End Property

        Private m_Links As Links = Nothing

        Public ReadOnly Property Links As Links
            Get
                If (Me.m_Links Is Nothing) Then Me.m_Links = New Links(Me)
                Return Me.m_Links
            End Get
        End Property
    End Class

End Namespace