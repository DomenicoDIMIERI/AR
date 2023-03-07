Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.Sistema

Public Class CSyncCollection(Of T)
    Inherits CSyncCollection

    Public Sub New()
    End Sub

    Default Public Shadows Property Item(ByVal index As Integer) As T
        Get
            Return MyBase.Item(index)
        End Get
        Set(value As T)
            MyBase.Item(index) = value
        End Set
    End Property

End Class
