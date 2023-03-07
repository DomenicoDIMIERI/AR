Namespace java
    Public MustInherit Class AbstractMap
        Implements Map


        Public MustOverride Sub clear() Implements Map.clear

        Public MustOverride Function containsKey(key As Object) As Boolean Implements Map.containsKey

        Public MustOverride Function containsValue(value As Object) As Boolean Implements Map.containsValue

        Public MustOverride Function entrySet() As [Set](Of Map.Entry(Of Object, Object)) Implements Map.entrySet

        Public MustOverride Overrides Function equals(o As Object) As Boolean Implements Map.equals

        Public MustOverride Function [get](v As Object) As Object Implements Map.get

        Public Function hashCode() As Integer Implements Map.hashCode
            Return Me.GetHashCode
        End Function

        Public MustOverride Function isEmpty() As Boolean Implements Map.isEmpty

        Public MustOverride Function keySet() As [Set] Implements Map.keySet

        Public MustOverride Function put(p1 As Object, value As Object) As Object Implements Map.put

        Public MustOverride Sub putAll(m As Map) Implements Map.putAll

        Public MustOverride Function remove(key As Object) As Object Implements Map.remove

        Public MustOverride Function size() As Integer Implements Map.size

        Public MustOverride ReadOnly Property Values As ICollection Implements Map.Values

    End Class

End Namespace