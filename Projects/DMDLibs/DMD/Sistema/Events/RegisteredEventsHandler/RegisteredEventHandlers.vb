Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Sistema


Namespace Internals

    ''' <summary>
    ''' Accede al modulo degli eventi registrati
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CRegisteredEventHandlersClass
        Inherits CGeneralClass(Of RegisteredEventHandler)

        Friend Sub New()
            MyBase.New("modRegisteredEventsHandlers", GetType(RegisteredEventHandlerCursor), -1)
        End Sub

        ''' <summary>
        ''' Registra un gestore di eventi
        ''' </summary>
        ''' <param name="handler"></param>
        ''' <param name="m"></param>
        ''' <param name="eventName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Register(ByVal handler As Object, ByVal m As CModule, ByVal eventName As String) As RegisteredEventHandler
            If (handler Is Nothing) Then Throw New ArgumentNullException("handler")
            eventName = Trim(eventName)
            Dim r As RegisteredEventHandler = GetItem(handler, m, eventName)
            If (r Is Nothing) Then
                r = New RegisteredEventHandler
                r.Module = m
                r.EventName = eventName
                r.ClassName = handler.GetType.FullName
                r.Save()
            End If
            Me.UpdateCached(r)
            Return r
        End Function

        Private Function GetItem(ByVal handler As Object, ByVal m As CModule, ByVal eventName As String) As RegisteredEventHandler
            eventName = Trim(eventName)
            Dim items As CCollection(Of RegisteredEventHandler) = Me.LoadAll
            For Each r As RegisteredEventHandler In items
                If (r.Module Is m) AndAlso (r.EventName = eventName) AndAlso (r.CreateHandler Is handler.GetType) Then Return r
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Cancella un gestore di eventi
        ''' </summary>
        ''' <param name="handler"></param>
        ''' <param name="m"></param>
        ''' <param name="eventName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Unregister(ByVal handler As Object, ByVal m As CModule, ByVal eventName As String) As RegisteredEventHandler
            If (handler Is Nothing) Then Throw New ArgumentNullException("handler")
            eventName = Trim(eventName)
            Dim r As RegisteredEventHandler = GetItem(handler, m, eventName)
            If (r IsNot Nothing) Then
                r.Delete()
                Me.UpdateCached(r)
            End If

            Return r
        End Function


        ''' <summary>
        ''' Restituisce la collezione di handlers registrati per l'evento specifico del modulo
        ''' </summary>
        ''' <param name="m"></param>
        ''' <param name="eventName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetHandlers(ByVal m As CModule, ByVal eventName As String) As CCollection(Of IEventHandler)
            Dim items As CCollection(Of RegisteredEventHandler) = Me.LoadAll

            Dim ret As New CCollection(Of IEventHandler)
            Dim mID As Integer = GetID(m)
            Dim handler As Object

            eventName = Trim(eventName)


            For Each r As RegisteredEventHandler In items
                If (r.Active AndAlso (r.ModuleID = mID OrElse r.ModuleID = 0) AndAlso (r.EventName = eventName OrElse r.EventName = "")) Then
                    Try
                        handler = r.CreateHandler()
                        If (handler IsNot Nothing) Then ret.Add(r.CreateHandler)
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                    Finally
                        handler = Nothing
                    End Try
                End If
            Next
            Return ret
        End Function


    End Class

End Namespace

Partial Public Class Sistema


    Private Shared m_RegisteredEventHandler As CRegisteredEventHandlersClass = Nothing

    Public Shared ReadOnly Property RegisteredEventHandlers As CRegisteredEventHandlersClass
        Get
            If (m_RegisteredEventHandler Is Nothing) Then m_RegisteredEventHandler = New CRegisteredEventHandlersClass
            Return m_RegisteredEventHandler
        End Get
    End Property

End Class