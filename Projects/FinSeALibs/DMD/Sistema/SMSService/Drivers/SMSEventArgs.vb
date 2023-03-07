Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public Class SMSEventArgs
        Inherits System.EventArgs

        Private m_Modem As SMSDriverModem
        Private m_EventType As String
        Private m_EventParameters As Object

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal modem As SMSDriverModem, ByVal eventType As String, ByVal eventParameters As Object)
            Me.New
            Me.m_Modem = modem
            Me.m_EventType = eventType
            Me.m_EventParameters = eventParameters
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

        Public ReadOnly Property EventType As String
            Get
                Return Me.m_EventType
            End Get
        End Property

        Public ReadOnly Property EventParameters As Object
            Get
                Return Me.m_EventParameters
            End Get
        End Property


    End Class

 
End Class