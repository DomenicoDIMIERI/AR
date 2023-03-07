Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public Class SMSDeliverEventArgs
        Inherits System.EventArgs

        Private m_MessageID As String
        Private m_TargetNumber As String
        Private m_DeliveryDate As Nullable(Of Date)
        Private m_Status As MessageStatusEnum

        Public Sub New(ByVal messageID As String, ByVal targetNumber As String, ByVal status As MessageStatusEnum, ByVal deliveryDate As Nullable(Of Date))
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_MessageID = messageID
            Me.m_TargetNumber = targetNumber
            Me.m_DeliveryDate = deliveryDate
            Me.m_Status = status
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property MessageID As String
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Public ReadOnly Property TargetNumber As String
            Get
                Return Me.m_TargetNumber
            End Get
        End Property

        Public ReadOnly Property DeliveryDate As Nullable(Of Date)
            Get
                Return Me.m_DeliveryDate
            End Get
        End Property

        Public ReadOnly Property Status As MessageStatusEnum
            Get
                Return Me.m_Status
            End Get
        End Property


    End Class

     

End Class