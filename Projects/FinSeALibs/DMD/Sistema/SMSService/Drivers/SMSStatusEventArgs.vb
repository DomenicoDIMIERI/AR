Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public Class SMSStatusEventArgs
        Inherits System.EventArgs

        Private m_Modem As SMSDriverModem
        Private m_MessageID As String
        Private m_Status As MessageStatusEnum
        Private m_StatusEx As String
        Private m_Time As Date?

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal modem As SMSDriverModem, ByVal messageID As String, ByVal status As MessageStatusEnum, ByVal statusEx As String, ByVal time As Date?)
            Me.New
            Me.m_Modem = modem
            Me.m_MessageID = messageID
            Me.m_Status = status
            Me.m_StatusEx = statusEx
            Me.m_Time = time
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

        Public ReadOnly Property Status As MessageStatusEnum
            Get
                Return Me.m_Status
            End Get
        End Property

        Public ReadOnly Property StatusEx As String
            Get
                Return Me.m_StatusEx
            End Get
        End Property

        Public ReadOnly Property Time As Date?
            Get
                Return Me.m_Time
            End Get
        End Property


    End Class


End Class