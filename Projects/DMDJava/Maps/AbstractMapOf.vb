Namespace java
    Public MustInherit Class AbstractMap(Of TKey, TValue)
        Implements Map(Of TKey, TValue)


        Public MustOverride Sub clear() Implements Map.clear

        Private Function _containsKey(key As Object) As Boolean Implements Map.containsKey
            Return Me.containsKey(key)
        End Function

        Private Function _containsValue(value As Object) As Boolean Implements Map.containsValue
            Return Me.containsValue(value)
        End Function

        Private Function _entrySet() As [Set](Of Map.Entry(Of Object, Object)) Implements Map.entrySet
            Return Me.entrySet
        End Function

        Public MustOverride Function entrySet() As [Set](Of Map.Entry(Of TKey, TValue)) Implements Map(Of TKey, TValue).entrySet

        Public MustOverride Overrides Function equals(o As Object) As Boolean Implements Map.equals

        Private Function _get(v As Object) As Object Implements Map.get
            Return Me.[get](v)
        End Function

        Public MustOverride Function [get](v As TKey) As TValue Implements Map(Of TKey, TValue).get

        Public Function hashCode() As Integer Implements Map.hashCode
            Return Me.GetHashCode
        End Function

        Public MustOverride Function isEmpty() As Boolean Implements Map.isEmpty

        Private Function _keySet() As [Set] Implements Map.keySet
            Return Me.keySet
        End Function

        Public MustOverride Function keySet() As [Set](Of TKey) Implements Map(Of TKey, TValue).keySet

        Private Function _put(p1 As Object, value As Object) As Object Implements Map.put
            Return Me.put(p1, value)
        End Function

        Public MustOverride Function put(p1 As TKey, value As TValue) As TValue Implements Map(Of TKey, TValue).put

        Private Sub _putAll(m As Map) Implements Map.putAll
            Me.putAll(m)
        End Sub

        Public MustOverride Sub putAll(m As Map(Of TKey, TValue)) Implements Map(Of TKey, TValue).putAll

        Private Function _remove(key As Object) As Object Implements Map.remove
            Return Me.remove(key)
        End Function

        Public MustOverride Function size() As Integer Implements Map.size

        Private ReadOnly Property _Values As ICollection Implements Map.Values
            Get
                Return Me.Values
            End Get
        End Property

        Public MustOverride Function containsKey(key As TKey) As Boolean Implements Map(Of TKey, TValue).containsKey

        Public MustOverride Function containsValue(value As TValue) As Boolean Implements Map(Of TKey, TValue).containsValue







        Public MustOverride Function remove(key As TKey) As TValue Implements Map(Of TKey, TValue).remove

        Public MustOverride ReadOnly Property Values As ICollection(Of TValue) Implements Map(Of TKey, TValue).Values

    End Class

End Namespace