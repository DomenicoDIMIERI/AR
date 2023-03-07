Namespace java
    Public Class Hashtable(Of K, V)
        Inherits Hashtable
        Implements Map(Of K, V)

        Public Shadows Function containsKey(key As K) As Boolean Implements Map(Of K, V).containsKey
            Return MyBase.containsKey(key)
        End Function

        Public Shadows Function containsValue(value As V) As Boolean Implements Map(Of K, V).containsValue
            Return MyBase.containsValue(value)
        End Function

        Public Shadows Function entrySet() As [Set](Of Map.Entry(Of K, V)) Implements Map(Of K, V).entrySet
            Dim ret As New HashSet(Of Map.Entry(Of K, V))
            For Each item As Map.Entry(Of Object, Object) In MyBase.entrySet
                ret.add(New Map.Entry(Of K, V)(item.Key, item.Value))
            Next
            Return ret
        End Function

        Public Shadows Function [get](v As K) As V Implements Map(Of K, V).get
            Return MyBase.get(v)
        End Function

        Public Shadows Function keySet() As [Set](Of K) Implements Map(Of K, V).keySet
            Dim ret As New HashSet(Of K)
            For Each key As Object In MyBase.keySet
                ret.add(key)
            Next
            Return ret
        End Function

        Public Shadows Function put(p1 As K, value As V) As V Implements Map(Of K, V).put
            Return MyBase.put(p1, value)
        End Function

        Public Shadows Sub putAll(m As Map(Of K, V)) Implements Map(Of K, V).putAll
            MyBase.putAll(m)
        End Sub

        Public Shadows Function remove(key As K) As V Implements Map(Of K, V).remove
            Return MyBase.remove(key)
        End Function

        Public Shadows ReadOnly Property Values As ICollection(Of V) Implements Map(Of K, V).Values
            Get
                Dim ret As New HashSet(Of V)
                For Each item As Object In MyBase.Values
                    ret.add(item)
                Next
                Return ret
            End Get
        End Property
    End Class

End Namespace