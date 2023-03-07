Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers

    <Serializable>
    Public Class AsteriskEventArgs
        Inherits System.EventArgs

        <NonSerialized> Private m_Server As AsteriskCallManager

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Server = Nothing
        End Sub

        Public Sub New(ByVal server As AsteriskCallManager)
            Me.New
            If (server Is Nothing) Then Throw New ArgumentNullException("server")
            Me.m_Server = server
        End Sub

        Public ReadOnly Property Server As AsteriskCallManager
            Get
                Return Me.m_Server
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace