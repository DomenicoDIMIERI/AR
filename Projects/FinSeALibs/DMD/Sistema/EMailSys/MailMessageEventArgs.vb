Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Security.Cryptography.X509Certificates
Imports DMD.Net.Mail


Partial Public Class Sistema

    Public Class MailMessageEventArgs
        Inherits System.EventArgs

        Private m_Error As System.Exception
        Private m_Cancelled As Boolean
        Private m_Message As MailMessageEx
        Private m_Account As CEmailAccount

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Message = Nothing
            Me.m_Cancelled = False
            Me.m_Error = Nothing
        End Sub

        Public Sub New(ByVal message As MailMessageEx, Optional ByVal cancelled As Boolean = False, Optional ByVal exception As System.Exception = Nothing)
            Me.New
            If (message Is Nothing) Then Throw New ArgumentNullException("message")
            Me.m_Message = message
            Me.m_Cancelled = cancelled
            Me.m_Error = exception
        End Sub

        Public Sub New(ByVal account As CEmailAccount, ByVal message As MailMessageEx, Optional ByVal cancelled As Boolean = False, Optional ByVal exception As System.Exception = Nothing)
            Me.New
            If (account Is Nothing) Then Throw New ArgumentNullException("account")
            If (message Is Nothing) Then Throw New ArgumentNullException("message")
            Me.m_Account = account
            Me.m_Message = message
            Me.m_Cancelled = cancelled
            Me.m_Error = exception
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce l'account utilizzato per inviare la posta (o per riceverla)
        ''' Se NULL indica che è stato usato l'account predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Account As CEmailAccount
            Get
                Return Me.m_Account
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Message As MailMessageEx
            Get
                Return Me.m_Message
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un valore booleano che indica se l'invio/ricezione del messaggio è stato annullato o ha generato errori
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Cancelled As Boolean
            Get
                Return Me.m_Cancelled
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'errore generato durante l'invio/ricezione del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property [Error] As System.Exception
            Get
                Return Me.m_Error
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            If (Me.m_Account IsNot Nothing) Then
                ret.Append("Account: ")
                ret.Append(Me.m_Account.AccountName)
                ret.Append(vbNewLine)
            End If
            If (Me.m_Message IsNot Nothing) Then
                ret.Append("Messaggio: ")
                ret.Append(Me.m_Message.Subject)
                ret.Append(vbNewLine)
                ret.Append(Me.m_Message.Body)
            End If
            Return ret.ToString
        End Function


    End Class


End Class