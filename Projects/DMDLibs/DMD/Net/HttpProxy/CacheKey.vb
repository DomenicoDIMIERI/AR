Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Net.HTTPProxy

    Public Class CacheKey
        Private Const null As Object = Nothing

        Public AbsoluteUri As String
        Public UserAgent As String

        Public Sub New(ByVal requestUri As String, ByVal userAgent As String)
            AbsoluteUri = requestUri
            userAgent = userAgent
        End Sub

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Dim key As CacheKey = obj
            If (key IsNot null) Then
                Return (key.AbsoluteUri = AbsoluteUri AndAlso key.UserAgent = UserAgent)
            End If
            Return False
        End Function

        Public Overrides Function GetHashCode() As Integer
            Dim s As String = AbsoluteUri & vbCr & UserAgent
            Return s.GetHashCode()
        End Function
    End Class

End Namespace
