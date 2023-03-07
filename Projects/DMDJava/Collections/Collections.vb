Namespace java
    Public NotInheritable Class Collections

        Public Shared Function unmodifiableMap(Of K, V)(ByVal m As Map(Of K, V)) As Map(Of K, V)
            Return m
        End Function

        Public Shared Function unmodifiableMap(ByVal m As Map) As Map
            Return m
        End Function


        Public Shared Sub reverse(Of T)(ByVal lst As ArrayList(Of T))
            For i As Integer = 0 To CInt(lst.size / 2) '- 1
                Dim tmp As T = lst.get(i)
                lst.set(i, lst.get(lst.size - 1 - i))
                lst.set(lst.size - 1 - i, tmp)
            Next
        End Sub

        Public Shared Function synchronizedMap(ByVal m As Map) As Map
            Return m
        End Function

        Public Shared Sub sort(Of T)(ByVal col As ArrayList(Of T))
            Throw New NotImplementedException
        End Sub

        Shared Sub sort(Of T)(textList As List(Of T), comparator As Global.System.Collections.Generic.IComparer(Of T))
            Throw New NotImplementedException
        End Sub

        Public Shared Function asList(Of T)(ByVal ParamArray argumens() As T) As List(Of T)
            Return New ArrayList(Of T)(argumens)
        End Function


        Public Class GenericIterator(Of T)
            Implements Iterator

            Private m_Col As ICollection(Of T)
            Private m_Current As Integer
            Private m_Items() As T

            Public Sub New(ByVal col As ICollection(Of T))
                Me.m_Col = col
                Me.m_Current = -1
                Me.m_Items = col.toArray(Of T)()
            End Sub

            Public Overridable Function hasNext() As Boolean Implements Iterator.hasNext
                Return Me.m_Current < Me.m_Col.size
            End Function

            Public Overridable Function [next]() As T
                Me.m_Current += 1
                Return Me.m_Items(Me.m_Current)
            End Function

            Private Function _next() As Object Implements Iterator.next
                Return Me.next
            End Function
        End Class

        Shared Function unmodifiableSet([set] As [Set]) As [Set]
            Return [set]
        End Function

        Shared Function singletonList(Of T)(obj As T) As List(Of T)
            Throw New NotImplementedException
        End Function

    End Class

End Namespace