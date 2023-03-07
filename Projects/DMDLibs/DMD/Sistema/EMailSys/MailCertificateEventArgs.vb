Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

''Public Const cdoSendUsingPickup = 1 'Send message using the local SMTP service pickup directory. 
''Public Const cdoSendUsingPort = 2 'Send the message using the network (SMTP over the network). 
''Public Const cdoSendUsingMethod = "http://schemas.microsoft.com/cdo/configuration/sendusing"

''Public Const cdoSMTPServer = "http://schemas.microsoft.com/cdo/configuration/smtpserver"
''Public Const cdoSMTPServerPort = "http://schemas.microsoft.com/cdo/configuration/smtpserverport"
''Public Const cdoSMTPConnectionTimeout = "http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout"

''Public Const cdoAnonymous = 0 'Do not authenticate
''Public Const cdoBasic = 1 'basic (clear-text) authentication
''Public Const cdoNTLM = 2 'NTLM

Partial Public Class Sistema

    Public Class MailCertificateEventArgs
        Inherits System.EventArgs

        Private m_Certificate As X509Certificate
        Private m_Chain As X509Chain
        Private m_sslPolicyErrors As SslPolicyErrors
        Private m_Allow As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Certificate = Nothing
            Me.m_Chain = Nothing
            Me.m_sslPolicyErrors = SslPolicyErrors.None
            Me.m_Allow = False
        End Sub

        Public Sub New(ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors)
            Me.New
            Me.m_Certificate = certificate
            Me.m_Chain = chain
            Me.m_sslPolicyErrors = sslPolicyErrors
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property Certificate As X509Certificate
            Get
                Return Me.m_Certificate
            End Get
        End Property

        Public ReadOnly Property Chain As X509Chain
            Get
                Return Me.m_Chain
            End Get
        End Property

        Public ReadOnly Property sslPolicyErrors As SslPolicyErrors
            Get
                Return Me.m_sslPolicyErrors
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se accettare o meno il certificato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Allow As Boolean
            Get
                Return Me.m_Allow
            End Get
            Set(value As Boolean)
                Me.m_Allow = value
            End Set
        End Property

    End Class


End Class