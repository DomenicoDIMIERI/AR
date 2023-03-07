Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class SetCDRUserField
        Inherits AsteriskEvent

       

        Public Sub New()
            MyBase.New("SetCDRUserField")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub
          
    End Class

End Namespace