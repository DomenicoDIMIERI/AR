Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net

Namespace Net.HTTPProxy

    Public Class ProxyCacheMissEventArgs
        Inherits ProxyRequestEventArgs

        
        Public Sub New()
        End Sub

        Public Sub New(ByVal req As ProxyRequest)
            MyBase.New(req)
        End Sub

    End Class

End Namespace
