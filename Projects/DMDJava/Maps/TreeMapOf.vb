
Namespace java

    Public Class TreeMap(Of TKey, TValue)
        Inherits TreeMap
        Implements SortedMap(Of TKey, TValue)


        ''' <summary>
        ''' Constructs a new, empty tree map, using the natural ordering of its keys.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructs a new, empty tree map, ordered according to the given comparator.
        ''' </summary>
        ''' <param name="comparator"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal comparator As Global.System.Collections.Generic.IComparer(Of TKey))
            MyBase.New(New CWrapper(comparator))
        End Sub

        Private Class CWrapper
            Implements Global.System.Collections.Generic.IComparer(Of Object)

            Private m_Comparer As Global.System.Collections.Generic.IComparer(Of TKey)

            Public Sub New(ByVal c As Global.System.Collections.Generic.IComparer(Of TKey))
                Me.m_Comparer = c
            End Sub

            Public Function Compare(x As Object, y As Object) As Integer Implements Global.System.Collections.Generic.IComparer(Of Object).Compare
                Return Me.m_Comparer.Compare(x, y)
            End Function

        End Class

        ''' <summary>
        ''' Constructs a new tree map containing the same mappings as the given map, ordered according to the natural ordering of its keys.
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal m As Map(Of TKey, TValue))
            MyBase.New(m)
        End Sub

        ''' <summary>
        ''' Constructs a new tree map containing the same mappings and using the same ordering as the specified sorted map.
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal m As SortedMap(Of TKey, TValue))
            MyBase.New(m)
        End Sub

        Public Shadows Function containsKey(key As TKey) As Boolean Implements Map(Of TKey, TValue).containsKey
            Return MyBase.containsKey(key)
        End Function

        Public Shadows Function containsValue(value As TValue) As Boolean Implements Map(Of TKey, TValue).containsValue
            Return MyBase.containsValue(value)
        End Function

        Public Shadows Function entrySet() As [Set](Of Map.Entry(Of TKey, TValue)) Implements Map(Of TKey, TValue).entrySet
            Dim ret As New HashSet(Of Map.Entry(Of TKey, TValue))
            For Each item As Map.Entry(Of Object, Object) In MyBase.entrySet
                ret.add(New Map.Entry(Of TKey, TValue)(item.Key, item.Value))
            Next
            Return ret
        End Function

        Public Shadows Function [get](v As TKey) As TValue Implements Map(Of TKey, TValue).get
            Return MyBase.get(v)
        End Function

        Public Shadows Function keySet() As [Set](Of TKey) Implements Map(Of TKey, TValue).keySet
            Dim ret As New HashSet(Of TKey)
            For Each k As Object In MyBase.keySet
                ret.add(k)
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
                Dim ret As New HashSet(Of TValue)
                For Each item As Object In MyBase.Values
                    ret.add(item)
                Next
                Return ret
            End Get
        End Property

        Public Shadows Function comparator() As Global.System.Collections.Generic.IComparer(Of TKey) Implements SortedMap(Of TKey, TValue).comparator
            Return MyBase.comparator
        End Function

        Public Shadows Function firstKey() As TKey Implements SortedMap(Of TKey, TValue).firstKey
            Return MyBase.firstKey
        End Function

        Public Shadows Function headMap(toKey As TKey) As SortedMap(Of TKey, TValue) Implements SortedMap(Of TKey, TValue).headMap
            Return MyBase.headMap(toKey)
        End Function

        Public Shadows Function lastKey() As TKey Implements SortedMap(Of TKey, TValue).lastKey
            Return MyBase.lastKey
        End Function

        Public Shadows Function subMap(fromKey As TKey, toKey As TKey) As SortedMap(Of TKey, TValue) Implements SortedMap(Of TKey, TValue).subMap
            Return MyBase.subMap(fromKey, toKey)
        End Function

        Public Shadows Function tailMap(fromKey As TKey) As SortedMap(Of TKey, TValue) Implements SortedMap(Of TKey, TValue).tailMap
            Return MyBase.tailMap(fromKey)
        End Function



    End Class

End Namespace