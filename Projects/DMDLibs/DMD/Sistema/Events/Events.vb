Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.Sistema

Namespace Internals

    Public NotInheritable Class CEventsClass
        Inherits CGeneralClass(Of CEventLog)

        Public Event UnhandledException(ByVal e As Exception)

        Private m_StopEvents As Boolean
        Private m_LogEvents As Boolean
        'Private m_EventHandlers As RegisteredEventHandlers

        Friend Sub New()
            MyBase.New("SysEventsModule", GetType(CEventsCursor))
            Me.m_LogEvents = False
            Me.m_StopEvents = False
        End Sub

        ''' <summary>
        ''' Restituisce o impopsta un valore booleano che indica se gli eventi devono essere bloccati a livello glibale.
        ''' Questo impedisce sia la memorizzazione nel log che l'esecuzione degli handlers associati ad un evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StopEvents As Boolean
            Get
                Return m_StopEvents
            End Get
            Set(value As Boolean)
                m_StopEvents = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se gli eventi devono essere memorizzati nel file di log
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogEvents As Boolean
            Get
                Return Me.m_LogEvents
            End Get
            Set(value As Boolean)
                Me.m_LogEvents = value
            End Set
        End Property

        ''' <summary>
        ''' Memorizza i dettagli dell'evento e lo notifica a tutti gli handlers registrati
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Friend Sub DispatchEvent(ByVal e As EventDescription)
            If (Me.StopEvents = True) Then Exit Sub

            If (e.Parametri.ContainsKey("CurrentUser") = False) Then e.Parametri.Add("CurrentUser", Sistema.Users.CurrentUser)

            If (Me.m_LogEvents) Then
                Dim objLog As New CEventLog(e)
                objLog.Save()
            End If

            Dim items As CCollection(Of IEventHandler) = RegisteredEventHandlers.GetHandlers(e.Module, e.EventName)
            For Each handler As IEventHandler In items
                '#If Not Debug Then
#If Not DEBUG Then
                Try
#End If
                '#End If
                handler.NotifyEvent(e)
                '#If Not Debug Then
#If Not DEBUG Then
                Catch ex As Exception
                    NotifyUnhandledException(New EventHandlerException(e, handler, ex))
                End Try
#End If
                '#End If
            Next

        End Sub

        'Private Delegate Sub DispatchEventCaller(ByVal e As EventDescription)
        'Private m_Dispatcher As DispatchEventCaller = AddressOf DispatchEvent

        ' ''' <summary>
        ' ''' Genera l'evento specificato
        ' ''' </summary>
        ' ''' <param name="m"></param>
        ' ''' <param name="eventName"></param>
        ' ''' <param name="parameters"></param>
        ' ''' <remarks></remarks>
        'Public Sub DispatchEvent(ByVal m As CModule, ByVal eventName As String, ByVal description As String, ByVal parameters As Object)
        '    If m_StopEvents Then Return
        '    Dim e As New EventDescription(Now, eventName, m, Users.CurrentUser, description, parameters)
        '    e.Parametri.Add("application", Sistema.ApplicationContext.Description)
        '    'm_Dispatcher.BeginInvoke(e, Nothing, Nothing)
        '    Me.DispatchEvent(e)
        'End Sub

        '''' <summary>
        '''' Genera l'evento specificato
        '''' </summary>
        '''' <param name="m"></param>
        '''' <param name="eventName"></param>
        '''' <param name="parameters"></param>
        '''' <remarks></remarks>
        'Friend Function DispatchEventAsync1(ByVal m As CModule, ByVal eventName As String, ByVal description As String, ByVal parameters As Object) As IAsyncResult
        '    If m_StopEvents Then Return Nothing
        '    Dim e As New EventDescription(Now, eventName, m, Users.CurrentUser, description, parameters)
        '    e.Parametri.Add("application", Sistema.ApplicationContext.Description)
        '    Return m_Dispatcher.BeginInvoke(e, Nothing, Nothing)
        'End Function

        ''' <summary>
        ''' Questo memodo genera l'evento UnhandledException 
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub NotifyUnhandledException(ByVal e As Exception)
            Try
                RaiseEvent UnhandledException(e)
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try
        End Sub


    End Class
End Namespace

Partial Public Class Sistema




#Region "Events"

    <Serializable> _
    Public Class EventHandlerException
        Inherits Exception

        Private m_E As EventDescription
        Private m_Handler As IEventHandler
        Private m_Exception As Exception

        Public Sub New(ByVal e As EventDescription, ByVal handler As IEventHandler, ByVal ex As Exception)
            MyBase.New("Eccezione generata per l'azione (" & e.Module.ModuleName & ", " & e.EventName & ") dall'handler " & TypeName(handler), ex)
            Me.m_E = e
            Me.m_Handler = handler
            Me.m_Exception = ex
        End Sub

        Public ReadOnly Property [Module] As CModule
            Get
                Return Me.m_E.Module
            End Get
        End Property

        Public ReadOnly Property EventName As String
            Get
                Return Me.m_E.EventName
            End Get
        End Property

        Public ReadOnly Property Description As String
            Get
                Return Me.m_E.Descrizione
            End Get
        End Property

        Public ReadOnly Property Descrittore As Object
            Get
                Return Me.m_E.Descrittore
            End Get
        End Property

        Public ReadOnly Property Handler As Object
            Get
                Return Me.m_Handler
            End Get
        End Property

        Public ReadOnly Property Exeption As Exception
            Get
                Return Me.m_Exception
            End Get
        End Property


    End Class
#End Region


    Private Shared m_Events As CEventsClass = Nothing


    ''' <summary>
    ''' Gestione degli eventi di sistema
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Events As CEventsClass
        Get
            If m_Events Is Nothing Then m_Events = New CEventsClass
            Return m_Events
        End Get
    End Property
End Class