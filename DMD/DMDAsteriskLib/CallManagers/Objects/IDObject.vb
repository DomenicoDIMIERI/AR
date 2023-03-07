Namespace CallManagers

    Public Class IDObject
        Inherits AsteriskObject

        Private m_UniqueID As String

        Public Sub New()
            Me.m_UniqueID = ""
        End Sub

        Public Property UniqueID As String
            Get
                Return Me.m_UniqueID
            End Get
            Set(value As String)
                Me.m_UniqueID = Trim(value)
            End Set
        End Property
         

    End Class

End Namespace