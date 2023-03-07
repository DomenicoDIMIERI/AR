Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public Enum MessageStatusEnum As Integer
        Unknown = 0
        Scheduled = 1
        Sent = 2
        Delivered = 3
        [Error] = 4
        Timeout = 5
        BadNumber = 6
        Waiting = 7
    End Enum

    Public Class MessageStatus
        Public MessageID As String
        Public SenderNumber As String
        Public SenderID As String
        Public TargetNumber As String
        Public TargetName As String
        Public SentTime As Nullable(Of Date)
        Public DeliveryTime As Nullable(Of Date)
        Public ReadTime As Nullable(Of Date)
        Public MessageStatus As MessageStatusEnum
        Public MessageStatusEx As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class