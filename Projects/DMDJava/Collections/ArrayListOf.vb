Namespace java
    Public Class ArrayList(Of T)
        Implements List(Of T)

        Private m_Base As New System.Collections.ArrayList

        Public Sub New()
            Me.m_Base = New System.Collections.ArrayList
        End Sub

        Public Sub New(ByVal capacity As Integer)
            Me.m_Base = New System.Collections.ArrayList(capacity)
        End Sub

        Public Sub New(ByVal base As Object)
            If (base Is Nothing) Then Throw New ArgumentNullException("base")
            For Each item As Object In base
                Me.m_Base.Add(item)
            Next
        End Sub

        Public Function GetEnumerator() As Global.System.Collections.Generic.IEnumerator(Of T) Implements Global.System.Collections.Generic.IEnumerable(Of T).GetEnumerator
            Return New Enumerator(Of T)(Me.m_Base.GetEnumerator)
        End Function

        Private Function GetEnumerator1() As Global.System.Collections.IEnumerator Implements Global.System.Collections.IEnumerable.GetEnumerator
            Return Me.m_Base.GetEnumerator
        End Function

        Public Function size() As Integer Implements List.size
            Return Me.m_Base.Count
        End Function


        Public Function isEmpty() As Boolean Implements List.isEmpty
            Return Me.m_Base.Count = 0
        End Function

        Public Function contains(o As T) As Boolean Implements ICollection(Of T).contains
            Return Me.m_Base.Contains(o)
        End Function

        Private Function _contains(o As Object) As Boolean Implements List.contains
            Return Me.Contains(o)
        End Function


        Public Function toArray() As Object() Implements List.toArray
            Return Me.m_Base.ToArray()
        End Function

        Private Function _add(o As Object) As Boolean Implements ICollection.add
            Return Me.add(o)
        End Function

        Private Sub _add(index As Integer, o As Object) Implements List.add
            Me.add(index, o)
        End Sub

        Private Function _add(o As T) As Boolean Implements List(Of T).add
            Return Me.add(o)
        End Function

        Public Sub add(index As Integer, o As T) Implements List(Of T).add
            Me.m_Base.Insert(index, o)
        End Sub

        Public Function add(o As T) As Boolean Implements ICollection(Of T).add
            Me.m_Base.Add(o)
            Return True
        End Function

        Private Function _remove(o As Object) As Boolean Implements ICollection.remove
            Return Me.remove(o)
        End Function

        Public Function _remove(o As T) As Boolean Implements List(Of T).remove
            Return Me.remove(o)
        End Function

        Public Function remove(o As T) As Boolean Implements ICollection(Of T).remove
            Dim i As Integer = Me.m_Base.IndexOf(o)
            If (i >= 0) Then
                Me.m_Base.Remove(o)
                Return True
            Else
                Return False
            End If
        End Function

        Private Function _containsAll(col As ICollection) As Boolean Implements ICollection.containsAll
            Return Me.containsAll(col)
        End Function

        Private Function _addAll(col As ICollection) As Boolean Implements ICollection.addAll
            Return Me.addAll(col)
        End Function

        Private Function _addAll(col As ICollection(Of T)) As Boolean Implements List(Of T).addAll
            Return Me.addAll(col)
        End Function

        Public Function addAll(fromIndex As Integer, col As ICollection(Of T)) As Boolean Implements List(Of T).addAll
            For Each o As Object In col
                Me.m_Base.Insert(fromIndex, o)
                fromIndex += 1
            Next
            Return True
        End Function

        Private Function _addAll(fromIndex As Integer, col As ICollection) As Boolean Implements List.addAll
            For Each o As Object In col
                Me.m_Base.Insert(fromIndex, o)
                fromIndex += 1
            Next
            Return True
        End Function

        Public Function addAll(col As ICollection(Of T)) As Boolean Implements ICollection(Of T).addAll
            For Each obj As T In col
                Me.add(obj)
            Next
            Return True
        End Function



        Public Function containsAll(col As ICollection(Of T)) As Boolean Implements ICollection(Of T).containsAll
            For Each item As T In col
                If Me.Contains(item) = False Then Return False
            Next
            Return True
        End Function



        Private Function _indexOf(o As Object) As Integer Implements List.indexOf
            Return Me.indexOf(o)
        End Function

        Public Function indexOf(o As T) As Integer Implements List(Of T).indexOf
            Return Me.m_Base.IndexOf(o)
        End Function


        Private Function _lastIndexOf(o As Object) As Integer Implements List.lastIndexOf
            Return Me.lastIndexOf(o)
        End Function

        Public Function lastIndexOf(o As T) As Integer Implements List(Of T).lastIndexOf
            Return Me.m_Base.LastIndexOf(o)
        End Function





        Private Function _remove(index As Integer) As Object Implements List.remove
            Return Me.remove(index)
        End Function

        Public Function remove(index As Integer) As T Implements List(Of T).remove
            Dim obj As T = Me.m_Base(index)
            Me.m_Base.RemoveAt(index)
            Return obj
        End Function





        Public Function [get](index As Integer) As T Implements List(Of T).get
            Return Me.m_Base(index)
        End Function

        Private Function _get(i As Integer) As Object Implements List.get
            Return Me.[get](i)
        End Function

        Public Function [set](index As Integer, element As T) As T Implements List(Of T).set
            Me.m_Base(index) = element
        End Function

        Private Function _set(index As Integer, element As Object) As Object Implements List.set
            Return Me.[set](index, element)
        End Function

        Private Function _subList(fromIndex As Integer, toIndex As Integer) As List Implements List.subList
            Dim ret As New ArrayList
            For i As Integer = fromIndex To toIndex - 1
                ret.add(Me.m_Base(i))
            Next
            Return ret
        End Function

        Public Function subList(fromIndex As Integer, toIndex As Integer) As List(Of T) Implements List(Of T).subList
            Dim ret As New ArrayList(Of T)
            For i As Integer = fromIndex To toIndex - 1
                ret.add(Me.m_Base(i))
            Next
            Return ret
        End Function

        Public Sub clear() Implements List.clear
            Me.m_Base.Clear()
        End Sub

        Public Overrides Function equals(o As Object) As Boolean Implements List.equals
            Return Me.m_Base.Equals(o)
        End Function

        Public Function hashCode() As Integer Implements List.hashCode
            Return Me.m_Base.GetHashCode
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.m_Base.GetHashCode()
        End Function

        Public Function iterator() As Iterator Implements List.iterator
            Return New Collections.GenericIterator(Of T)(Me)
        End Function



        Public Function toArray(Of T1)() As T1() Implements List.toArray
            Return Me.m_Base.ToArray(GetType(T1))
        End Function

        Public Function removeAll(col As ICollection(Of T)) As Boolean Implements List(Of T).removeAll
            Dim ret As Boolean = True
            For Each item As T In col
                Dim i As Integer = Me.m_Base.IndexOf(item)
                If (i > 0) Then
                    Me.m_Base.RemoveAt(i)
                Else
                    ret = False
                End If
            Next
            Return ret
        End Function

        Private Function _removeAll(col As ICollection) As Boolean Implements ICollection.removeAll
            Dim ret As Boolean = True
            For Each item As Object In col
                Dim i As Integer = Me.m_Base.IndexOf(item)
                If (i > 0) Then
                    Me.m_Base.RemoveAt(i)
                Else
                    ret = False
                End If
            Next
            Return ret
        End Function


        Public Function retainAll(col As ICollection(Of T)) As Boolean Implements List(Of T).retainAll
            Dim ret As Boolean = False
            Dim i = 0, j As Integer = 0
            While (i < Me.size())
                j = -1
                For Each item As T In col
                    If Me.m_Base(i).Equals(item) Then
                        j = i
                        ret = True
                        Exit For
                    End If
                Next
                If (j = -1) Then
                    Me.m_Base.RemoveAt(i)
                Else
                    i += 1
                End If
            End While
            Return ret
        End Function


        Private Function _retainAll(col As ICollection) As Boolean Implements ICollection.retainAll
            Dim ret As Boolean = False
            Dim i = 0, j As Integer = 0
            While (i < Me.size())
                j = -1
                For Each item As Object In col
                    If Me.m_Base(i).Equals(item) Then
                        j = i
                        ret = True
                        Exit For
                    End If
                Next
                If (j = -1) Then
                    Me.m_Base.RemoveAt(i)
                Else
                    i += 1
                End If
            End While
            Return ret
        End Function



    End Class

End Namespace