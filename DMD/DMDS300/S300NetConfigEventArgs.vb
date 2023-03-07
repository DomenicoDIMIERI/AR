Namespace S300

    Public Class S300NetConfigEventArgs
        Inherits S300EventArgs

        Private m_Config As CKT_DLL.NETINFO

        Public Sub New()
            Me.m_Config = Nothing
        End Sub

        Public Sub New(ByVal device As S300Device, ByVal config As CKT_DLL.NETINFO)
            MyBase.New(device)
            'If (config Is Nothing) Then Throw New ArgumentNullException("config")
            Me.m_Config = config
        End Sub

        Public ReadOnly Property Config As CKT_DLL.NETINFO
            Get
                Return Me.m_Config
            End Get
        End Property

    End Class


End Namespace
