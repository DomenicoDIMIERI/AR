Imports DMD.Sistema

Public Class CSyncKeyCollection(Of T)
    Inherits CSyncKeyCollection

    Public Sub New()
    End Sub

    Public Sub New(ByVal col As CKeyCollection)
        MyBase.New(col)
    End Sub

    Default Public Shadows Property Item(ByVal index As Integer) As T
        Get
            Return MyBase.Item(index)
        End Get
        Set(value As T)
            MyBase.Item(index) = value
        End Set
    End Property

    Default Public Shadows Property Item(ByVal key As String) As T
        Get
            Return MyBase.Item(key)
        End Get
        Set(value As T)
            MyBase.Item(key) = value
        End Set
    End Property

End Class
