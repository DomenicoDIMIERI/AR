Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports DMD.Net.Mime
Imports DMD.Net.GenericClient

Namespace Net.Mail


    Public MustInherit Class GenericMailClient
        Implements IDisposable

        Private m_CurrentState As ConnectionState
        Private m_UserName As String
        Private m_Password As String
        Private m_Server As String
        Private m_Port As Integer
        Private m_UseSSL As Boolean
        ''' <summary>
        ''' Traces the various command objects that executed during this objects
        ''' lifetime.
        ''' </summary>
        Public Event Trace(ByVal sender As Object, ByVal message As String)

        ''' <summary>
        ''' Initializes a new instance  
        ''' </summary>
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_CurrentState = ConnectionState.Unknown
        End Sub

        Public Sub New(ByVal userName As String, ByVal password As String, ByVal server As String, Optional ByVal port As Integer = 110, Optional ByVal useSSL As Boolean = False)
            Me.New()
            Me.SetUserName(userName)
            Me.SetPassword(password)
            Me.SetServer(server)
            Me.SetPort(port)
            Me.SetUseSSL(useSSL)
        End Sub

        Public ReadOnly Property UserName As String
            Get
                Return Me.m_UserName
            End Get
        End Property

        Protected Sub SetUserName(ByVal value As String)
            Me.m_UserName = value
        End Sub

        Protected Friend ReadOnly Property Password As String
            Get
                Return Me.m_Password
            End Get
        End Property

        Protected Sub SetPassword(ByVal value As String)
            Me.m_Password = value
        End Sub

        Public ReadOnly Property ServerName As String
            Get
                Return Me.m_Server
            End Get
        End Property

        Protected Sub SetServer(ByVal value As String)
            Me.m_Server = value
        End Sub

        Public ReadOnly Property ServerPort As Integer
            Get
                Return Me.m_Port
            End Get
        End Property

        Protected Sub SetPort(ByVal value As Integer)
            Me.m_Port = value
        End Sub

        Public ReadOnly Property UseSSL As Boolean
            Get
                Return Me.m_UseSSL
            End Get
        End Property

        Protected Sub SetUseSSL(ByVal value As Boolean)
            Me.m_UseSSL = value
        End Sub




        ''' <summary>
        ''' Restituisce lo stato dell'oggetto
        ''' </summary>
        ''' <value>The state of the current.</value>
        Public ReadOnly Property CurrentState As ConnectionState
            Get
                Return Me.m_CurrentState
            End Get
        End Property
        Protected Friend Overridable Sub SetState(ByVal state As ConnectionState)
            Me.m_CurrentState = state
        End Sub



        Protected Overridable Sub OnTrace(ByVal message As String)
            RaiseEvent Trace(Me, message)
        End Sub


        ''' <summary>
        ''' Prepara la connessione al server
        ''' </summary>
        Public MustOverride Sub Connect()


        ''' <summary>
        ''' Effettua l'autenticazione sul server
        ''' </summary>
        ''' <remarks></remarks>
        Public MustOverride Sub Authenticate()


        ''' <summary>
        ''' Elimina il messaggio specificato
        ''' </summary>
        Public MustOverride Sub Dele(ByVal msg As MailMessageEx)

        ''' <summary>
        ''' Effettua un NOOP
        ''' </summary>
        Public MustOverride Sub Noop()

        ''' <summary>
        ''' Ripristina i messaggi eliminati
        ''' </summary>
        Public MustOverride Sub Rset()

        ''' <summary>
        ''' Restituisce una collezione contenente le intestazioni di tutti i messaggi
        ''' </summary>
        ''' <returns>A <c>Pop3ListItem</c> for the requested Pop3Item.</returns>
        Public MustOverride Function GetMessagesList() As CCollection(Of MailMessageEx)

        ''' <summary>
        ''' Restituise le prime
        ''' </summary>
        ''' <param name="index">[in] indice base 1 del messaggio</param>
        ''' <param name="lineCount">[in] numero di linee da restituire</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function Top(ByVal index As Integer, ByVal lineCount As Integer) As MailMessageEx

        ' ''' <summary>
        ' ''' Restituisce il messaggio specificato
        ' ''' </summary>
        ' ''' <param name="index">[in] Indice base 1 del messaggio</param>
        ' ''' <returns></returns>
        'Public MustOverride Function GetMessage(ByVal index As Integer) As MailMessageEx

        ' ''' <summary>
        ' ''' Scarica il messaggio completo
        ' ''' </summary>
        ' ''' <returns></returns>
        'Public MustOverride Function DownloadMessage(ByVal message As MailMessageEx)


        ''' <summary>
        ''' Invia il messaggio di uscita 
        ''' </summary>
        Public MustOverride Sub Quit()

        ''' <summary>
        ''' Disconnette il server
        ''' </summary>
        ''' <remarks></remarks>
        Public MustOverride Sub Disconnect()

        ''' <summary>
        ''' Restituisce il nome del protocollo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride ReadOnly Property Name As String


        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Me.m_UserName = vbNullString
            Me.m_Password = vbNullString
            Me.m_Server = vbNullString
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

    End Class

End Namespace
