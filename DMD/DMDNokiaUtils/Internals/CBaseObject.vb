Imports DMD.Nokia

Namespace Internals

    ''' <summary>
    ''' Oggetto base
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CBaseObject
        Friend m_Device As NokiaDevice

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Device = Nothing
        End Sub

        Public Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
        End Sub

        ''' <summary>
        ''' Restituisce il device
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property Device As Nokia.NokiaDevice
            Get
                Return Me.m_Device
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Protected Friend Overridable Sub SetDevice(ByVal value As Nokia.NokiaDevice)
            Me.m_Device = value
        End Sub

    End Class

End Namespace