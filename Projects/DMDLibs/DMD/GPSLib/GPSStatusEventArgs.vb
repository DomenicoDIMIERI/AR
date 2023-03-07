Namespace GPSLib

    Public Class GPSStatusEventArgs
        Inherits System.EventArgs

        Private m_Enabled As Boolean

        Public Sub New()
            Me.m_Enabled = False
        End Sub

        Public Sub New(ByVal enabled As Boolean)
            Me.m_Enabled = enabled
        End Sub

        Public ReadOnly Property Enabled As Boolean
            Get
                Return Me.m_Enabled
            End Get
        End Property



    End Class


End Namespace