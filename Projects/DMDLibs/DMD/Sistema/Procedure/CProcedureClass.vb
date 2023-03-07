Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports System.Threading
Imports System.Timers

Namespace Internals


    Public Class CProcedureClass
        Inherits CGeneralClass(Of CProcedura)

        Private Const WORKER_GRANULARITY_MILLI As Integer = 60 * 1000 '1 minuto
        Private stopping As Boolean = False
        Private m_Running As Boolean = False
        Private Shared workerLock As New Object
        Private WithEvents m_SchedulerTimer As System.Timers.Timer
        Private Shared m_RunningThreads As Integer = 0

        Public Sub New()
            MyBase.New("modCalendarProcs", GetType(CProcedureCursor), -1)
            'PRIORITY_HIGHER = -2
            'PRIORITY_HIGH = -1
            'PRIORITY_NORMAL = 0
            'PRIOTITY_LOW = 1
            'PRIORITY_LOWER = 2

            'Me.m_Scheduler = Nothing
            Me.m_SchedulerTimer = New System.Timers.Timer(WORKER_GRANULARITY_MILLI)
        End Sub

        Public Function CountRunningThreads() As Integer
            Return m_RunningThreads
        End Function


        Public Function GetItemByName(ByVal name As String) As CProcedura
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            Dim items As CCollection(Of CProcedura) = Me.LoadAll
            For Each p As CProcedura In items
                If (p.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(p.Nome, name, CompareMethod.Text) = 0) Then Return p
            Next
            Return Nothing
        End Function

        Public Sub StartBackgroundWorker()
            Me.stopping = False
            Me.m_SchedulerTimer.Enabled = True
        End Sub

        Public Sub StopBackgroundWorker()
            Me.stopping = True
            Me.m_SchedulerTimer.Enabled = False
        End Sub

        Private Class ThreadRun
            Public startDate As Date
            Public endDate As Date?
            Public c As CProcedura
            Public thread As System.Threading.Thread

            Public Sub New(ByVal c As CProcedura)
                Me.c = c
                Me.startDate = Now
            End Sub

            Public Sub ThreadStart(ByVal o As Object)
                Me.thread = System.Threading.Thread.CurrentThread
                Select Case Me.c.Priority
                    Case PriorityEnum.PRIORITY_HIGHER
                        Me.thread.Priority = ThreadPriority.Highest
                    Case PriorityEnum.PRIORITY_HIGH
                        Me.thread.Priority = ThreadPriority.AboveNormal
                    Case PriorityEnum.PRIORITY_NORMAL
                        Me.thread.Priority = ThreadPriority.Normal
                    Case PriorityEnum.PRIOTITY_LOW
                        Me.thread.Priority = ThreadPriority.BelowNormal
                    Case PriorityEnum.PRIORITY_LOWER
                        Me.thread.Priority = ThreadPriority.Lowest
                End Select
                'o.thread.Start()

                SyncLock workerLock
                    m_RunningThreads += 1
                End SyncLock

                Try


                    Dim d As Date = Me.startDate
                    Dim d1 As Date = DateUtils.Now
                    Dim erroreP As Exception = Nothing
                    Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: " & c.Nome & " -> Inizio Esecuzione")
                    Try
                        c.Run()
                    Catch ex As Exception
                        erroreP = ex
                    End Try

                    Dim d2 As Date = DateUtils.Now
                    Dim s As CalendarSchedule = c.Programmazione.GetNextSchedule
                    s.UltimaEsecuzione = d
                    s.ConteggioEsecuzioni += 1
                    s.Save()
                    c.Save()

                    If (erroreP Is Nothing) Then
                        Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: " & c.Nome & " -> Fine Esecuzione (" & Formats.FormatDurata((d2 - d1).TotalSeconds) & ")")
                    Else
                        Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: " & c.Nome & " -> Errore (" & Formats.FormatDurata((d2 - d1).TotalSeconds) & ")")
                        Sistema.ApplicationContext.Log(erroreP.Message)
                    End If
                Catch ex As Exception
                    Sistema.ApplicationContext.Log("Procedura [" & TypeName(c) & ":" & GetID(c) & "]: Eccezione: " & ex.Message & vbNewLine & ex.StackTrace)

                End Try
                'SyncLock queueLock
                '    m_Queue.Add(o)
                'End SyncLock

                SyncLock workerLock
                    m_RunningThreads -= 1
                End SyncLock
            End Sub

        End Class



        Private Sub SchedulerThread()
            'SyncLock Me.workerLock
            Try
                If Me.m_Running Then Return
                Me.m_Running = True

                'While (Not Me.stopping)
                Dim d As Date = DateUtils.Now
                Dim tutte As CCollection(Of CProcedura) = Me.LoadAll
                Dim c As CProcedura
                Dim s As CalendarSchedule
                For Each c In tutte
                    If (c.Stato <> ObjectStatus.OBJECT_VALID OrElse TestFlag(c.Flags, ProceduraFlags.Disabilitata)) Then Continue For
                    s = c.Programmazione.GetNextSchedule
                    If (s IsNot Nothing) Then
                        Dim dNext As Nullable(Of Date) = s.CalcolaProssimaEsecuzione
                        If (dNext.HasValue) AndAlso (dNext.Value <= DateUtils.Now()) Then
                            'SyncLock Me.queueLock
                            Dim o As New ThreadRun(c)
                            Dim wc As New WaitCallback(AddressOf o.ThreadStart)
                            If (Not ThreadPool.QueueUserWorkItem(wc, o)) Then
                                Throw New Exception
                            End If

                            'o.thread = New System.Threading.Thread(AddressOf o.ThreadStart)
                            'Me.m_Queue.Add(o)

                            ' End SyncLock
                        End If
                    End If
                    If (Me.stopping) Then Exit For
                Next

                'If (Not Me.stopping) Then
                '    For i As Integer = 0 To 4
                '        SyncLock Me.queueLock
                '            If (Me.m_Queues(i).Count > 0) Then
                '                Me.m_Locks(i).Set()
                '            End If
                '        End SyncLock
                '    Next
                'End If

                'If (Not Me.stopping) Then System.Threading.Thread.Sleep(WORKER_GRANULARITY_MILLI)
                'End While
            Catch ex As Exception
                Try
                    Sistema.ApplicationContext.Log("Procedures: Scheduler Thread Crashed: " & ex.Message)
                    'Me.m_Scheduler = Nothing
                    'Me.m_SchedulerTimer = Nothing
                    'For i As Integer = 0 To 3
                    '    System.Threading.Thread.Sleep(1000)
                    '    Sistema.ApplicationContext.Log("Procedures: Attempt to restart Scheduler Thread (" & i & ")")
                    '    Try
                    '        Me.StartBackgroundWorker()
                    '        Return
                    '    Catch ex1 As Exception

                    '    End Try
                    'Next

                Catch ex1 As Exception

                End Try
            End Try
            ' Me.m_Scheduler = Nothing
            'End SyncLock

            Me.m_Running = False
        End Sub

        Private Sub m_SchedulerTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles m_SchedulerTimer.Elapsed
            Me.SchedulerThread()
        End Sub
    End Class

End Namespace

Partial Class Sistema


    Private Shared m_Procedure As CProcedureClass = Nothing

    Public Shared ReadOnly Property Procedure As CProcedureClass
        Get
            If (m_Procedure Is Nothing) Then m_Procedure = New CProcedureClass
            Return m_Procedure
        End Get
    End Property

End Class

