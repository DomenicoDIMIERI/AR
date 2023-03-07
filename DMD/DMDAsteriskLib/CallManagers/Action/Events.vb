Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions


    ''' <summary>
    ''' Contol Event Flow: Enable/Disable sending of events to this manager client.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Events
        Inherits AsyncAction

        Private m_EventMask As DebugFlags = DebugFlags.On
        
        Public Sub New()
            MyBase.New("Events")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <param name="enabled">[in] Se vero abilita tutti gli eventi</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal enabled As Boolean)
            Me.New()
            Me.m_EventMask = IIf(enabled, DebugFlags.On, DebugFlags.Off)
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <param name="flags">[in] flags</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal flags As DebugFlags)
            Me.New()
            Me.m_EventMask = flags
        End Sub

        Public ReadOnly Property EventMask As DebugFlags
            Get
                Return Me.m_EventMask
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "EVENTMASK: " & Me.GetMask(Me.EventMask) & vbCrLf
        End Function

         
        Protected Function GetMask(ByVal flags As DebugFlags) As String
            Select Case flags
                Case DebugFlags.On : Return "ON"
                Case DebugFlags.Off : Return "OFF"
                Case Else
                    Dim f As DebugFlags
                    Dim ret As String = ""
                    For Each f In [Enum].GetValues(GetType(DebugFlags))
                        If (flags And f) = f Then
                            If (ret <> "") Then ret &= ","
                            ret &= [Enum].GetName(GetType(DebugFlags), f)
                        End If
                    Next
                    Return ret
            End Select
        End Function

        Public Overrides Function IsAsync() As Boolean
            If (Me.EventMask = DebugFlags.On) Then Return True
            Return MyBase.IsAsync()
        End Function
             

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New EventsResponse(Me)
        End Function

    End Class

End Namespace