Namespace java
    Public Class TreeMap
        Inherits AbstractMap
        Implements SortedMap

        Private m_base As System.Collections.Generic.SortedList(Of Object, Object)


        ''' <summary>
        ''' Constructs a new, empty tree map, using the natural ordering of its keys.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.m_base = New System.Collections.Generic.SortedList(Of Object, Object)
        End Sub

        ''' <summary>
        ''' Constructs a new, empty tree map, ordered according to the given comparator.
        ''' </summary>
        ''' <param name="comparator"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal comparator As Global.System.Collections.Generic.IComparer(Of Object))
            Me.m_base = New System.Collections.Generic.SortedList(Of Object, Object)(comparator)
        End Sub

        ''' <summary>
        ''' Constructs a new tree map containing the same mappings as the given map, ordered according to the natural ordering of its keys.
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal m As Map)
            Me.m_base = New System.Collections.Generic.SortedList(Of Object, Object)
            Me.putAll(m)
        End Sub

        ''' <summary>
        ''' Constructs a new tree map containing the same mappings and using the same ordering as the specified sorted map.
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal m As SortedMap)
            Me.m_base = New System.Collections.Generic.SortedList(Of Object, Object)(New CWrapper(m.comparator))
            Me.putAll(m)
        End Sub

        Private Class CWrapper
            Implements Global.System.Collections.Generic.IComparer(Of Object)

            Private m_Comparer As Global.System.Collections.IComparer

            Public Sub New(ByVal c As Global.System.Collections.IComparer)
                Me.m_Comparer = c
            End Sub

            Public Function Compare(x As Object, y As Object) As Integer Implements Global.System.Collections.Generic.IComparer(Of Object).Compare
                Return Me.m_Comparer.Compare(x, y)
            End Function

        End Class



        Public Overrides Sub clear()
            Me.m_base.Clear()
        End Sub

        Public Overrides Function containsKey(key As Object) As Boolean
            Return Me.m_base.ContainsKey(key)
        End Function

        Public Overrides Function containsValue(value As Object) As Boolean
            Return Me.m_base.ContainsValue(value)
        End Function

        Public Overrides Function entrySet() As [Set](Of Map.Entry(Of Object, Object))
            Dim ret As New HashSet
            For Each k As Object In Me.m_base.Keys
                ret.add(New Map.Entry(Of Object, Object)(k, Me.m_base(k)))
            Next
            Return ret
        End Function

        Public Overloads Overrides Function equals(o As Object) As Boolean
            Return TypeOf (o) Is TreeMap AndAlso (DirectCast(o, TreeMap).m_base.Equals(Me.m_base))
        End Function

        Public Overrides Function [get](v As Object) As Object
            Return Me.m_base(v)
        End Function

        Public Overrides Function isEmpty() As Boolean
            Return Me.m_base.Count = 0
        End Function

        Public Overrides Function keySet() As [Set]
            Dim ret As New HashSet
            For Each k As Object In Me.m_base.Keys
                ret.add(k)
            Next
            Return ret
        End Function

        Public Overrides Function put(ByVal key As Object, value As Object) As Object
            Me.m_base.Add(key, value)
            Return value
        End Function

        Public Overrides Sub putAll(m As Map)
            For Each i As Map.Entry(Of Object, Object) In m.entrySet
                Me.m_base.Add(i.Key, i.Value)
            Next
        End Sub

        Public Overrides Function remove(key As Object) As Object
            Dim i As Integer = Me.m_base.IndexOfKey(key)
            Dim k As Object = Me.m_base(i)
            Me.m_base.RemoveAt(i)
            Return k
        End Function

        Public Overrides Function size() As Integer
            Return Me.m_base.Count
        End Function

        Public Overrides ReadOnly Property Values As ICollection
            Get
                Dim ret As New HashSet
                For Each v As Object In Me.m_base.Values
                    ret.add(v)
                Next
                Return ret
            End Get
        End Property

        Public Function comparator() As Global.System.Collections.IComparer Implements SortedMap.comparator
            Return Me.m_base.Comparer
        End Function

        Public Function firstKey() As Object Implements SortedMap.firstKey
            Return Me.m_base.Keys(0)
        End Function

        Public Function headMap(toKey As Object) As SortedMap Implements SortedMap.headMap
            Dim ret As SortedMap = Me.NewMap
            For Each k As Object In Me.m_base.Keys
                Dim v As Object = Me.m_base(k)
                If Me.comparator.Compare(v, toKey) < 0 Then
                    ret.put(k, v)
                End If
            Next
            Return ret
        End Function

        'Public Function headMap(toKey As Object, ByVal inclusive As Boolean) As NavigableMap

        'End Function

        Public Function lastKey() As Object Implements SortedMap.lastKey
            Return Me.m_base.Keys(Me.m_base.Count - 1)
        End Function

        Public Function subMap(fromKey As Object, toKey As Object) As SortedMap Implements SortedMap.subMap
            Dim ret As SortedMap = Me.NewMap
            For Each k As Object In Me.m_base.Keys
                Dim v As Object = Me.m_base(k)
                If Me.comparator.Compare(v, fromKey) >= 0 AndAlso Me.comparator.Compare(v, toKey) < 0 Then
                    ret.put(k, v)
                End If
            Next
            Return ret
        End Function

        Public Function tailMap(fromKey As Object) As SortedMap Implements SortedMap.tailMap
            Dim ret As SortedMap = Me.NewMap
            For Each k As Object In Me.m_base.Keys
                Dim v As Object = Me.m_base(k)
                If Me.comparator.Compare(v, fromKey) >= 0 Then
                    ret.put(k, v)
                End If
            Next
            Return ret
        End Function

        Protected Overridable Function NewMap() As SortedMap
            Return Activator.CreateInstance(Me.GetType, {Me.comparator})
        End Function
    End Class

End Namespace