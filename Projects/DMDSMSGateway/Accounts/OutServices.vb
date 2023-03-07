Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

'Namespace DMD


Partial Class SMSGateway



    Public Class OutServicesClass
        Inherits CGeneralClass(Of OutService)

        'Private m_Timer As System.Timers.Timer

        Public Sub New()
            MyBase.New("modSMSServiceAccounts", GetType(OutServiceCursor), -1)

            ' m_Timer = New System.Timers.Timer
            ' m_Timer.Interval = 5000
            ' AddHandler m_Timer.Elapsed, AddressOf handleTimer
        End Sub

        Private Sub handleTimer(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
            Me.CheckNewMessages()
        End Sub

        
        Public Sub CheckNewMessages()
            SyncLock Me
                'm_Timer.Enabled = False

                For Each service As OutService In OutServices.LoadAll
                    Dim device As DMD.Nokia.NokiaDevice = service.GetDevice
                    Dim messages As System.Collections.ReadOnlyCollectionBase = Nothing

                    Try
                        If (device IsNot Nothing) Then
                            'device.SMS.InBox.Messages.Refresh()
                            messages = device.SMS.InBox.Messages
                        End If
                    Catch ex As Exception
                    End Try

                    If messages Is Nothing Then Continue For


                    For Each m As DMD.Nokia.CSMSMessage In messages
                        Dim col As CCollection(Of SMSMessage) = service.FindMessages(m)
                        For Each m1 As SMSMessage In col
                            If GetID(m1) = 0 Then
                                m1.StatoRicezione = SMSReceiveStatus.NotReceived
                                m1.Save()
                                Try
                                    service.NotifyNewMessage(m1)
                                    m1.StatoRicezione = SMSReceiveStatus.Received
                                    m1.DettaglioStatoRicezione = "Notificato a " & service.NotificaA
                                    m1.Save()
                                Catch ex As Exception
                                    m1.DettaglioStatoRicezione = ex.Message
                                End Try
                            End If
                        Next
                    Next


                Next
 
                '  m_Timer.Enabled = True
            End SyncLock


        End Sub

        Public Function GetItemByName(ByVal accountName As String) As OutService
            accountName = Strings.Trim(accountName)
            If (accountName = "") Then Return Nothing
            SyncLock Me
                For Each item As OutService In Me.LoadAll
                    If item.Nome = accountName Then Return item
                Next
                Return Nothing
            End SyncLock
        End Function

    End Class

    Private Shared m_OutServices As OutServicesClass = Nothing

    Public Shared ReadOnly Property OutServices As OutServicesClass
        Get
            If m_OutServices Is Nothing Then m_OutServices = New OutServicesClass
            Return m_OutServices
        End Get
    End Property


End Class

'End Namespace