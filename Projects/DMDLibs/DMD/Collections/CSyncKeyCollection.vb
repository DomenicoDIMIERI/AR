Imports DMD.Sistema

Public Class CSyncKeyCollection
    Inherits CKeyCollection

    Protected lockObject As New Object

    Public Sub New()
    End Sub

    Public Overrides ReadOnly Property IsSynchronized As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property SyncRoot As Object
        Get
            Return Me.lockObject
        End Get
    End Property

    Public Sub New(ByVal col As CKeyCollection)
        MyBase.New(col)
    End Sub
     
End Class
