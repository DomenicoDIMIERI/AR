Namespace S300

    Public Class S300EventArgs
        Inherits System.EventArgs

        Private m_Device As S300Device

        Public Sub New()
            Me.m_Device = Nothing
        End Sub

        Public Sub New(ByVal device As S300Device)
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
        End Sub

        Public ReadOnly Property Device As S300Device
            Get
                Return Me.m_Device
            End Get
        End Property

    End Class


End Namespace
