Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net

Partial Public Class Sistema


    Public Enum AsyncStateStatusEnum As Integer
        READYTOSEND = 0
        SENDING = 1
        COMPLETED = 3
        ABORTED = 4
    End Enum

    Public NotInheritable Class AsyncState

        Public startTime As Date
        Public endTime As Date? = Nothing
        Friend m_methodName As String
        Friend m_parameters As Object()
        Friend m_handler As IRPCCallHandler
        Friend waiting As Integer

        Private counter As Integer
        Private retry As Integer
        Private timeout As Integer
        Public Stato As AsyncStateStatusEnum
        Public ID As Integer

        Friend _result As AsyncResult
        Friend thread As System.Threading.Thread

        Private Shared m_Timer As New System.Timers.Timer(5000)
        Private Shared m_List As New System.Collections.ArrayList

        Shared Sub New()
            AddHandler m_Timer.Elapsed, AddressOf timerClick
            m_Timer.Enabled = True
        End Sub



        Private Shared Sub timerClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim i As Integer = 0
            Do
                Dim o As AsyncState = Nothing
                SyncLock m_List
                    If (i < m_List.Count) Then
                        o = m_List(i)
                    End If
                End SyncLock

                If (o Is Nothing) Then Exit Do

                Select Case o.Stato
                    Case AsyncStateStatusEnum.ABORTED, AsyncStateStatusEnum.COMPLETED
                        SyncLock m_List
                            m_List.Remove(o)
                        End SyncLock
                    Case Else
                        If (o.ExeTimeMilliseconds > o.timeout) Then
                            SyncLock m_List
                                m_List.Remove(o)
                            End SyncLock
                            Select Case o.Stato
                                Case AsyncStateStatusEnum.ABORTED, AsyncStateStatusEnum.COMPLETED
                                Case Else
                                    Try
                                        o.doTimeout()
                                    Catch ex As Exception
                                        Sistema.Events.NotifyUnhandledException(ex)
                                    End Try
                            End Select
                        Else
                            i += 1
                        End If
                End Select


            Loop
        End Sub

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            ' Me.req = Nothing
            Me.startTime = Now
            Me.m_methodName = ""
            Me._result = Nothing
            Me.waiting = 0
            Me.counter = 0
            Me.retry = 2
            Me.timeout = 30000
            Me.m_handler = Nothing
            Me.m_parameters = Nothing
            Me.Stato = AsyncStateStatusEnum.READYTOSEND
            Me.ID = 0
        End Sub

        Public Sub New(ByVal methodName As String, ByVal params() As Object, ByVal handler As Object)
            Me.New
            Me.m_methodName = methodName
            Me.m_parameters = params
            Me.m_handler = handler
        End Sub

        Public ReadOnly Property MethodName As String
            Get
                Return Me.m_methodName
            End Get
        End Property


        Friend Sub ThreadStart(ByVal obj As Object)
            Dim req As AsyncState = obj
            req.thread = System.Threading.Thread.CurrentThread

            SyncLock m_List
                m_List.Add(Me)
            End SyncLock
            Me.startTime = Now

            Try
                Me._result = New AsyncResult(RPC.InvokeMethod1(Me.MethodName, Me.m_parameters))
            Catch ex As Exception
                Try
                    Me.doError(255, ex.Message)
                    Exit Sub
                Catch ex1 As Exception
                    Sistema.Events.NotifyUnhandledException(ex1)
                End Try
            End Try

            Try
                Me.doComplete()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
        End Sub

        Public ReadOnly Property ExeTimeMilliseconds As Long
            Get
                If (Me.endTime.HasValue) Then
                    Return (Me.endTime.Value - Me.startTime).TotalMilliseconds
                Else
                    Return (Now - Me.startTime).TotalMilliseconds
                End If
            End Get
        End Property

        Public Sub Cancel()
            If (Me.thread Is Nothing) Then Return
            Try
                Me.thread.Abort()
                Me.thread.Join()
                SyncLock m_List
                    m_List.Remove(Me)
                End SyncLock
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
            Me.Stato = AsyncStateStatusEnum.ABORTED
            Me.thread = Nothing
            Me.doError(-1, "Operazione annullata")
        End Sub

        Public Function getResult() As Object
            Return Me._result
        End Function

        Friend Sub doComplete()
            SyncLock m_List
                m_List.Remove(Me)
            End SyncLock
            Me.Stato = AsyncStateStatusEnum.COMPLETED
            If (Me.m_handler IsNot Nothing) Then Me.m_handler.OnAsyncComplete(Me._result)
            Me.thread = Nothing
        End Sub

        Friend Sub doError(ByVal errorCode As Integer, ByVal errorMessage As String)
            SyncLock m_List
                m_List.Remove(Me)
            End SyncLock
            Me._result = New AsyncResult(errorCode, errorMessage)
            If (Me.m_handler IsNot Nothing) Then Me.m_handler.OnAsyncError(Me._result)
            Me.thread = Nothing
        End Sub

        Friend Sub doTimeout()
            If (Me.thread Is Nothing) Then Return
            Try
                Me.thread.Abort()
                'Me.thread.Join()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
            Me.doError(-2, "Timeout")
            Me.thread = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class