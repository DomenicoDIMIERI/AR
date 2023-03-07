Namespace java
    Public Class HashMap
        Inherits AbstractMap

        Private m_Base As New System.Collections.Hashtable

        Public Sub New()
            Me.m_Base = New System.Collections.Hashtable
        End Sub

        Public Sub New(ByVal initialCapacity As Integer)
            Me.m_Base = New System.Collections.Hashtable(initialCapacity)
        End Sub

        Public Sub New(ByVal initialCapacity As Integer, ByVal loadFactor As Single)
            Me.m_Base = New System.Collections.Hashtable(initialCapacity, loadFactor)
        End Sub

        Public Sub New(ByVal m As Map)
            Me.m_Base = New System.Collections.Hashtable()
            Me.putAll(m)
        End Sub

        Public Overrides Sub clear()
            Me.m_Base.Clear()
        End Sub

        Public Overrides Function containsKey(key As Object) As Boolean
            Return Me.m_Base.ContainsKey(key)
        End Function

        Public Overrides Function containsValue(value As Object) As Boolean
            Return Me.m_Base.ContainsValue(value)
        End Function

        Public Overrides Function entrySet() As [Set](Of Map.Entry(Of Object, Object))
            Dim ret As New TreeSet(Of Map.Entry(Of Object, Object))
            For Each k As Object In Me.m_Base.Keys
                ret.add(New Map.Entry(Of Object, Object)(k, Me.m_Base(k)))
            Next
            Return ret
        End Function

        Public Overloads Overrides Function equals(o As Object) As Boolean
            Return TypeOf (o) Is HashMap AndAlso DirectCast(o, HashMap).m_Base.Equals(Me.m_Base)
        End Function

        Public Overrides Function [get](v As Object) As Object
            Return Me.m_Base(v)
        End Function

        Public Overrides Function isEmpty() As Boolean
            Return Me.m_Base.Count = 0
        End Function

        Public Overrides Function keySet() As [Set]
            Dim ret As New TreeSet
            For Each k As Object In Me.m_Base.Keys
                ret.add(k)
            Next
            Return ret
        End Function

        Public Overrides Function put(p1 As Object, value As Object) As Object
            If (Me.m_Base.ContainsKey(p1)) Then
                Me.m_Base(p1) = value
            Else
                Me.m_Base.Add(p1, value)
            End If
            Return value
        End Function

        Public Overrides Sub putAll(m As Map)
            For Each o As Map.Entry(Of Object, Object) In m.entrySet
                Me.m_Base.Add(o.Key, o.Value)
            Next
        End Sub

        Public Overrides Function remove(key As Object) As Object
            Dim o As Object = Me.m_Base(key)
            Me.m_Base.Remove(key)
            Return o
        End Function

        Public Overrides Function size() As Integer
            Return Me.m_Base.Count
        End Function

        Public Overrides ReadOnly Property Values As ICollection
            Get
                Dim ret As New TreeSet
                For Each o As Object In Me.m_Base.Values
                    ret.add(o)
                Next
                Return ret
            End Get
        End Property
    End Class

End Namespace