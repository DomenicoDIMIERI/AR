Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CMdbDBConnection
        Inherits COleDBConnection

        Public Sub New()
        End Sub

        Public Sub New(ByVal fileName As String)
            Me.Path = fileName
        End Sub

    End Class

End Class


