Public Class CKeyCollection(Of T)
    Inherits CKeyCollection
    Implements IEnumerable(Of T)

    Public Sub New()
    End Sub

    Public Sub New(ByVal col As CKeyCollection)
        MyBase.New(col)
    End Sub

    Public Sub New(ByVal keys() As String, ByVal items() As T)
        Me.New()
        For i As Integer = 0 To UBound(keys)
            Me.Add(keys(i), items(i))
        Next
    End Sub

    Public Shadows Function GetItemByKey(ByVal key As String) As T
        Return MyBase.GetItemByKey(key)
    End Function

    Public Shadows Function GetItemById(ByVal id As Integer) As T
        Return MyBase.GetItemById(id)
    End Function

    Public Shadows Sub Add(ByVal key As String, ByVal item As T)
        MyBase.Add(key, item)
    End Sub

    Public Shadows Sub Remove(ByVal item As T)
        MyBase.Remove(item)
    End Sub

    Public Shadows Function IndexOf(ByVal item As T) As Integer
        Return MyBase.IndexOf(item)
    End Function

    Public Shadows Function Contains(ByVal item As T) As Boolean
        Return MyBase.Contains(item)
    End Function


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

    Public Shadows Function ToArray() As T()
        Return MyBase.ToArray(Of T)()
    End Function

    Public Overridable Shadows Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return New Enumerator(Of T)(MyBase.GetEnumerator)
    End Function

End Class
