Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' Occurs when a channel is placed on hold/unhold and music is played to the caller.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MusicOnHold
        Inherits AsteriskEvent
         
        
        Public Sub New()
            MyBase.New("MusicOnHold")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property State As String
            Get
                Return Me.GetAttribute("State")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("UniqueID")
            End Get
        End Property
          
    End Class

End Namespace