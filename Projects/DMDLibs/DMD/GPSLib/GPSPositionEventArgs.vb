Namespace GPSLib

    Public Class GPSPositionEventArgs
        Inherits System.EventArgs

        Private m_Pos As GPSPosition

        Public Sub New()
            Me.m_Pos = Nothing
        End Sub

        Public Sub New(ByVal pos As GPSPosition)
            Me.m_Pos = pos
        End Sub

        Public ReadOnly Property Position As GPSPosition
            Get
                Return Me.m_Pos
            End Get
        End Property
    End Class


End Namespace