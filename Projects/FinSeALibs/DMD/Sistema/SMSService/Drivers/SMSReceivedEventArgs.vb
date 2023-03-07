Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public Class SMSReceivedEventArgs
        Inherits System.EventArgs

        Private m_Modem As SMSDriverModem
        Private m_MessageID As String
        Private m_SenderNumber As String
        Private m_ReceiveDate As Date
        Private m_Message As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal modem As SMSDriverModem, ByVal messageID As String, ByVal senderNumber As String, ByVal receivedDate As Date, ByVal message As String)
            Me.New
            Me.m_Modem = modem
            Me.m_MessageID = messageID
            Me.m_SenderNumber = senderNumber
            Me.m_ReceiveDate = receivedDate
            Me.m_Message = message
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property Modem As SMSDriverModem
            Get
                Return Me.m_Modem
            End Get
        End Property

        Public ReadOnly Property MessageID As String
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Public ReadOnly Property SenderNumber As String
            Get
                Return Me.m_SenderNumber
            End Get
        End Property

        Public ReadOnly Property ReceiveDate As Date
            Get
                Return Me.m_ReceiveDate
            End Get
        End Property

        Public ReadOnly Property Message As String
            Get
                Return Me.m_Message
            End Get
        End Property


    End Class

 
End Class