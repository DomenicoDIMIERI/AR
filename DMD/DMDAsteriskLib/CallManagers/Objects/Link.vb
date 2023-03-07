Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers


    Public Class Link
        Inherits AsteriskObject
        Private m_Channel1 As Channel
        Private m_Channel2 As Channel

        Public Sub New()
        End Sub

        Public Property Channel1 As Channel
            Get
                Return Me.m_Channel1
            End Get
            Set(value As Channel)
                Me.m_Channel1 = value
            End Set
        End Property

        Public Property Channel2 As Channel
            Get
                Return Me.m_Channel2
            End Get
            Set(value As Channel)
                Me.m_Channel2 = value
            End Set
        End Property




    End Class

End Namespace