Namespace java

    Public Interface Map


        ''' <summary>
        ''' Removes all of the mappings from this map (optional operation).
        ''' </summary>
        ''' <remarks></remarks>
        Sub clear()

        ''' <summary>
        ''' Returns true if this map contains a mapping for the specified key.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function containsKey(ByVal key As Object) As Boolean

        ''' <summary>
        ''' Returns true if this map maps one or more keys to the specified value.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function containsValue(ByVal value As Object) As Boolean

        ''' <summary>
        ''' Returns a Set view of the mappings contained in this map.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function entrySet() As [Set](Of Map.Entry(Of Object, Object))

        ''' <summary>
        ''' Compares the specified object with this map for equality.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function equals(ByVal o As Object) As Boolean

        ''' <summary>
        ''' Returns the value to which the specified key is mapped, or null if this map contains no mapping for the key.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function [get](ByVal v As Object) As Object

        ''' <summary>
        ''' Returns the hash code value for this map.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function hashCode() As Integer

        ''' <summary>
        ''' Returns true if this map contains no key-value mappings.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function isEmpty() As Boolean

        ''' <summary>
        ''' Returns a Set view of the keys contained in this map.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function keySet() As [Set]

        ''' <summary>
        ''' Associates the specified value with the specified key in this map (optional operation).
        ''' </summary>
        ''' <param name="p1"></param>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Function put(p1 As Object, value As Object) As Object

        ''' <summary>
        ''' Copies all of the mappings from the specified map to this map (optional operation).
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Sub putAll(ByVal m As Map)

        ''' <summary>
        ''' Removes the mapping for a key from this map if it is present (optional operation).
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function remove(ByVal key As Object) As Object

        ''' <summary>
        ''' Returns the number of key-value mappings in this map.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function size() As Integer

        ''' <summary>
        ''' Returns a Collection view of the values contained in this map.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Values() As ICollection

        Class Entry(Of K, V)
            Public Value As V
            Public Key As K

            Public Sub New()
            End Sub

            Public Sub New(ByVal k As K, ByVal v As V)
                Me.Key = k
                Me.Value = v
            End Sub
        End Class

        Class Entry
            Inherits Entry(Of Object, Object)

            Public Sub New()
            End Sub

            Public Sub New(ByVal k As Object, ByVal v As Object)
                MyBase.New(k, v)
            End Sub
        End Class


    End Interface

End Namespace