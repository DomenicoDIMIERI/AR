Imports DMD.Sistema

Partial Class WebSite

    <Serializable>
    Public Class SessionEventArgs
        Inherits System.EventArgs

        <NonSerialized> Private m_Session As CSiteSession

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal session As CSiteSession)
            Me.New
            If (session Is Nothing) Then Throw New ArgumentNullException("session")
            Me.m_Session = session
        End Sub

        Public ReadOnly Property Session As CSiteSession
            Get
                Return Me.m_Session
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class
