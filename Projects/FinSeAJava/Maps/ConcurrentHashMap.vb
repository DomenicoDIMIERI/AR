Namespace java
    Public Class ConcurrentHashMap(Of TKey, TValue)
        Inherits AbstractMap(Of TKey, TValue)

        Private m_Lock As New Object
        Private map As HashMap(Of TKey, TValue)

        Public Sub New()
            Me.map = New HashMap(Of TKey, TValue)
        End Sub

        Public Sub New(ByVal capacity As Integer)
            Me.map = New HashMap(Of TKey, TValue)(capacity)
        End Sub


        Public Overrides Sub clear()
            SyncLock Me.m_Lock
                map.clear()
            End SyncLock
        End Sub

        Public Overrides Function containsKey(key As TKey) As Boolean
            SyncLock Me.m_Lock
                Return Me.map.containsKey(key)
            End SyncLock
        End Function

        Public Overrides Function containsValue(value As TValue) As Boolean
            SyncLock Me.m_Lock
                Return Me.map.containsValue(value)
            End SyncLock
        End Function

        Public Overrides Function entrySet() As [Set](Of Map.Entry(Of TKey, TValue))
            SyncLock Me.m_Lock
                Return Me.map.entrySet
            End SyncLock
        End Function

        Public Overloads Overrides Function equals(o As Object) As Boolean
            SyncLock Me.m_Lock
                Return (TypeOf (o) Is ConcurrentHashMap(Of TKey, TValue)) AndAlso (DirectCast(o, ConcurrentHashMap(Of TKey, TValue)).map.equals(Me.map))
            End SyncLock
        End Function

        Public Overrides Function [get](v As TKey) As TValue
            SyncLock Me.m_Lock
                Return Me.map.get(v)
            End SyncLock
        End Function

        Public Overrides Function isEmpty() As Boolean
            SyncLock Me.m_Lock
                Return Me.map.isEmpty
            End SyncLock
        End Function

        Public Overrides Function keySet() As [Set](Of TKey)
            SyncLock Me.m_Lock
                Return Me.map.keySet
            End SyncLock
        End Function

        Public Overrides Function put(p1 As TKey, value As TValue) As TValue
            SyncLock Me.m_Lock
                Return Me.map.put(p1, value)
            End SyncLock
        End Function

        Public Overrides Sub putAll(m As Map(Of TKey, TValue))
            SyncLock Me.m_Lock
                Me.map.putAll(m)
            End SyncLock
        End Sub

        Public Overrides Function remove(key As TKey) As TValue
            SyncLock Me.m_Lock
                Return Me.map.remove(key)
            End SyncLock
        End Function

        Public Overrides Function size() As Integer
            SyncLock Me.m_Lock
                Return Me.map.size
            End SyncLock
        End Function

        Public Overrides ReadOnly Property Values As ICollection(Of TValue)
            Get
                SyncLock Me.m_Lock
                    Return Me.map.Values
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' If the specified key is not already associated with a value, associate it with the given value.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function putIfAbsent(key As TKey, value As TValue) As TValue
            SyncLock Me.m_Lock
                If Not Me.map.containsKey(key) Then
                    Return Me.map.put(key, value)
                Else
                    Return Me.map.get(key)
                End If
            End SyncLock
        End Function

    End Class

End Namespace