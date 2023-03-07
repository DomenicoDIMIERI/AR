Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' Fired when two voice channels are linked together and voice data exchange commences.
    ''' </summary>
    ''' <remarks>Several Link events may be seen for a single call. This can occur when Asterisk fails to setup a native bridge for the call. As far as I can tell, this is when Asterisk must sit between two telephones and perform CODEC conversion on their behalf.</remarks>
    Public Class Bridge
        Inherits Link

       

        Public Sub New()
            MyBase.New()
            Me.SetAttribute("Event", "Bridge")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

         
    End Class

End Namespace