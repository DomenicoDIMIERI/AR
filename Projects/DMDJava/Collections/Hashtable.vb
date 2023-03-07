
Namespace java

    Public Class Hashtable
        Inherits Dictionary
        Implements ICloneable, Map

        Private m_hashtable As System.Collections.Hashtable

        Sub New()
        End Sub

        Sub New(ByVal initialCapacity As Integer)
            Me.m_hashtable = New System.Collections.Hashtable(initialCapacity)
        End Sub

        Sub New(ByVal initialCapacity As Integer, ByVal loadFactor As Single)
            Me.m_hashtable = New System.Collections.Hashtable(initialCapacity, loadFactor)
        End Sub

        Sub New(ByVal m As Map)
            Me.m_hashtable = New System.Collections.Hashtable
            For Each o As Map.Entry(Of Object, Object) In m.entrySet
                Me.m_hashtable.Add(o.Key, o.Value)
            Next
        End Sub


        Public Sub clear() Implements Map.clear
            Me.m_hashtable.Clear()
        End Sub

        ''' <summary>
        ''' Tests if some key maps into the specified value in this hashtable.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function contains(ByVal value As Object) As Boolean
            Return Me.m_hashtable.Contains(value)
        End Function


        Public Function containsKey(key As Object) As Boolean Implements Map.containsKey
            Return Me.m_hashtable.ContainsKey(key)
        End Function

        Public Function containsValue(value As Object) As Boolean Implements Map.containsValue
            Return Me.m_hashtable.ContainsValue(value)
        End Function

        Public Function entrySet() As [Set](Of Map.Entry(Of Object, Object)) Implements Map.entrySet
            Dim ret As New HashSet
            For Each k As Object In Me.m_hashtable.Keys
                ret.add(New Map.Entry(Of Object, Object)(k, Me.m_hashtable.Item(k)))
            Next
            Return ret
        End Function

        Public Overrides Function equals(o As Object) As Boolean Implements Map.equals
            Return Dir(TypeOf (o) Is Hashtable) AndAlso (DirectCast(o, Hashtable).m_hashtable.Equals(Me.m_hashtable))
        End Function

        Public Overrides Function [get](v As Object) As Object Implements Map.get
            Return Me.m_hashtable.Item(v)
        End Function

        Public Function hashCode() As Integer Implements Map.hashCode
            Return Me.GetHashCode
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.m_hashtable.GetHashCode
        End Function

        Public Overrides Function isEmpty() As Boolean Implements Map.isEmpty
            Return Me.m_hashtable.Count = 0
        End Function

        Public Function keySet() As [Set] Implements Map.keySet
            Dim ret As New HashSet
            For Each k As Object In Me.m_hashtable.Keys
                ret.add(k)
            Next
            Return ret
        End Function

        Public Overrides Function put(Key As Object, value As Object) As Object Implements Map.put
            Me.m_hashtable.Add(Key, value)
            Return value
        End Function

        Public Sub putAll(m As Map) Implements Map.putAll
            For Each v As Map.Entry(Of Object, Object) In m.entrySet
                Me.m_hashtable.Add(v.Key, v.Value)
            Next
        End Sub

        Public Overrides Function remove(key As Object) As Object Implements Map.remove
            Dim v As Object = Me.m_hashtable.Item(key)
            Me.m_hashtable.Remove(key)
            Return v
        End Function

        Public Overrides Function size() As Integer Implements Map.size
            Return Me.m_hashtable.Count
        End Function

        Public ReadOnly Property Values As ICollection Implements Map.Values
            Get
                Dim ret As New HashSet
                For Each o As Object In Me.m_hashtable.Values
                    ret.add(o)
                Next
                Return ret
            End Get
        End Property

        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Return New Hashtable(Me)
        End Function

        Public Overrides Function elements() As Global.System.Collections.IEnumerable
            Return Me.m_hashtable.Values
        End Function

        Public Overrides Function keys() As Global.System.Collections.IEnumerable
            Return Me.m_hashtable.Keys
        End Function
    End Class

End Namespace