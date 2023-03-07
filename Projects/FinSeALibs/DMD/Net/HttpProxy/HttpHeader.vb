Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net

Namespace Net.HTTPProxy

    Public Class HttpHeader

        Public Key As String
        Public Value As String
        
        Public Sub New()
        End Sub

        Public Sub New(ByVal key As String, ByVal value As String)
            Me.Key = Trim(key)
            Me.Value = Trim(value)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Key & ": " & Me.Value
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.ToString.GetHashCode
        End Function

    End Class

End Namespace
