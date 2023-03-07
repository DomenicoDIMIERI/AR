Imports DMD
Imports DMD.Databases
Imports System.Net

Public Class ItemEventArgs(Of T)
    Inherits ItemEventArgs

    Public Sub New()
    End Sub

    Public Sub New(ByVal item As T)
        MyBase.New(item)
    End Sub

    Public Shadows ReadOnly Property Item As T
        Get
            Return MyBase.Item
        End Get
    End Property
End Class
