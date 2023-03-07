Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' Effettua il logout
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Logoff
        Inherits Action

        Private m_Username As String
        Private m_Secret As String

        Public Sub New()
            MyBase.New("Logoff")
        End Sub

        

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New LogoffResponse(Me)
        End Function

    End Class

End Namespace