Namespace CallManagers

    Public Class AsteriskObject
        Private m_Owner As AsteriskCallManager

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Owner = Nothing
        End Sub

        Public ReadOnly Property Owner As AsteriskCallManager
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal value As AsteriskCallManager)
            Me.m_Owner = value
        End Sub

    End Class

End Namespace