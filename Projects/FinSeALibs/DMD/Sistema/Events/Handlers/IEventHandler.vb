Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class EventDescription
        Inherits System.EventArgs

        Private m_Data As Date
        Private m_EventName As String
        Private m_Module As CModule
        Private m_Utente As CUser
        Private m_Descrizione As String
        Private m_Descrittore As Object
        Private m_Parametri As CKeyCollection

        Public Sub New()
        End Sub

        Public Sub New(ByVal eventName As String, ByVal descrizione As String, ByVal oggetto As Object)
            Me.m_Data = Now
            Me.m_EventName = eventName
            Me.m_Utente = Users.CurrentUser
            Me.m_Descrizione = descrizione
            Me.m_Descrittore = oggetto
        End Sub

        Public Sub New(ByVal data As Date, ByVal eventName As String, ByVal m As CModule, ByVal u As CUser, ByVal descrizione As String, ByVal descrittore As Object)
            Me.m_Data = data
            Me.m_EventName = eventName
            Me.m_Module = m
            Me.m_Utente = u
            Me.m_Descrizione = descrizione
            Me.m_Descrittore = descrittore
        End Sub

        Public ReadOnly Property Parametri As CKeyCollection
            Get
                If (Me.m_Parametri Is Nothing) Then Me.m_Parametri = New CKeyCollection
                Return Me.m_Parametri
            End Get
        End Property

        Public ReadOnly Property Data As Date
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property EventName As String
            Get
                Return Me.m_EventName
            End Get
        End Property

        Public ReadOnly Property Utente As CUser
            Get
                Return Me.m_Utente
            End Get
        End Property

        Public ReadOnly Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Descrittore As Object
            Get
                Return Me.m_Descrittore
            End Get
        End Property

        Public ReadOnly Property [Module] As CModule
            Get
                Return Me.m_Module
            End Get
        End Property

        Friend Sub SetModule(ByVal value As CModule)
            Me.m_Module = value
        End Sub

    End Class

    ''' <summary>
    ''' Interfaccia implementata dagli handler degli eventi di sistema
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IEventHandler

        ''' <summary>
        ''' Metodo richiamato dal sistema per notificare l'evento all'handler
        ''' </summary>
        ''' <param name="e">[in] Descrittore dell'evento</param>
        Sub NotifyEvent(ByVal e As EventDescription)

    End Interface

End Class