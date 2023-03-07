Namespace java
    Public Class HashMap(Of TKey, TValue)
        Inherits HashMap
        Implements Map(Of TKey, TValue)

        Public Sub New()
        End Sub

        Sub New(ByVal initialCapacity As Integer)
            MyBase.New(initialCapacity)
        End Sub

        Public Sub New(ByVal initialCapacity As Integer, ByVal loadFactor As Single)
            MyBase.New(initialCapacity, loadFactor)
        End Sub

        Public Sub New(ByVal m As Map(Of TKey, TValue))
            MyBase.New(m)
        End Sub

        Public Shadows Function containsKey(key As TKey) As Boolean Implements Map(Of TKey, TValue).containsKey
            Return MyBase.containsKey(key)
        End Function

        Public Shadows Function containsValue(value As TValue) As Boolean Implements Map(Of TKey, TValue).containsValue
            Return MyBase.containsValue(value)
        End Function

        Public Shadows Function entrySet() As [Set](Of Map.Entry(Of TKey, TValue)) Implements Map(Of TKey, TValue).entrySet
            Dim ret As New TreeSet(Of Map.Entry(Of TKey, TValue))
            For Each item As Map.Entry(Of Object, Object) In MyBase.entrySet
                ret.add(New Map.Entry(Of TKey, TValue)(item.Key, item.Value))
            Next
            Return ret
        End Function

        Public Shadows Function [get](v As TKey) As TValue Implements Map(Of TKey, TValue).get
            Return MyBase.[get](v)
        End Function

        Public Shadows Function keySet() As [Set](Of TKey) Implements Map(Of TKey, TValue).keySet
            Dim ret As New TreeSet(Of TKey)
            For Each o As Object In MyBase.keySet
                ret.add(o)
            Next
            Return ret
        End Function

        Public Shadows Function put(p1 As TKey, value As TValue) As TValue Implements Map(Of TKey, TValue).put
            Return MyBase.put(p1, value)
        End Function

        Public Shadows Sub putAll(m As Map(Of TKey, TValue)) Implements Map(Of TKey, TValue).putAll
            MyBase.putAll(m)
        End Sub

        Public Shadows Function remove(key As TKey) As TValue Implements Map(Of TKey, TValue).remove
            Return MyBase.remove(key)
        End Function

        Public Shadows ReadOnly Property Values As ICollection(Of TValue) Implements Map(Of TKey, TValue).Values
            Get
                Dim ret As New TreeSet(Of TValue)
                For Each item As Object In MyBase.Values
                    ret.add(item)
                Next
                Return ret
            End Get
        End Property
    End Class

End Namespace