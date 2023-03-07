Namespace java
    Public Class ArrayList
        Implements List

        Private m_Base As System.Collections.ArrayList
        Private _p1 As Integer

        Public Sub New()
            Me.m_Base = New System.Collections.ArrayList
        End Sub

        Public Sub New(ByVal base As System.Collections.ArrayList)
            If (base Is Nothing) Then Throw New ArgumentNullException("base")
            Me.m_Base = base
        End Sub

        Sub New(p1 As Integer)
            ' TODO: Complete member initialization 
            _p1 = p1
        End Sub

        Public Function GetEnumerator() As Global.System.Collections.IEnumerator Implements Global.System.Collections.IEnumerable.GetEnumerator
            Return Me.m_Base.GetEnumerator
        End Function

        Public Function [get](i As Integer) As Object Implements List.get
            Return Me.m_Base(i)
        End Function

        Public Function size() As Integer Implements List.size
            Return Me.m_Base.Count
        End Function

        Public Function contains(o As Object) As Boolean Implements List.contains
            Return Me.m_Base.Contains(o)
        End Function

        Public Function isEmpty() As Boolean Implements List.isEmpty
            Return Me.m_Base.Count = 0
        End Function

        Public Function toArray() As Object() Implements List.toArray
            Return Me.m_Base.ToArray()
        End Function

        Public Function add(o As Object) As Boolean Implements ICollection.add
            Me.m_Base.Add(o)
            Return True
        End Function

        Public Function remove(o As Object) As Boolean Implements ICollection.remove
            Dim i As Integer = Me.m_Base.IndexOf(o)
            If (i >= 0) Then
                Me.m_Base.Remove(o)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function containsAll(col As ICollection) As Boolean Implements List.containsAll
            For Each item As Object In col
                If Me.contains(item) = False Then Return False
            Next
            Return True
        End Function

        Public Function addAll(col As ICollection) As Boolean Implements ICollection.addAll
            For Each item As Object In col
                Me.m_Base.Add(item)
            Next
            Return True
        End Function

        Public Function addAll(fromIndex As Integer, col As ICollection) As Boolean Implements List.addAll
            For Each o As Object In col
                Me.m_Base.Insert(fromIndex, o)
                fromIndex += 1
            Next
            Return True
        End Function

        Public Function indexOf(o As Object) As Integer Implements List.indexOf
            Return Me.m_Base.IndexOf(o)
        End Function

        Public Function lastIndexOf(o As Object) As Integer Implements List.lastIndexOf
            Return Me.m_Base.LastIndexOf(o)
        End Function

        Public Function remove(index As Integer) As Object Implements List.remove
            Dim obj As Object = Me.m_Base(index)
            Me.m_Base.RemoveAt(index)
            Return obj
        End Function

        Public Sub add(index As Integer, o As Object) Implements List.add
            Me.m_Base.Insert(index, o)
        End Sub

        Public Function [set](index As Integer, element As Object) As Object Implements List.set
            Me.m_Base(index) = element
            Return element
        End Function

        Public Function subList(fromIndex As Integer, toIndex As Integer) As List Implements List.subList
            Dim ret As New ArrayList
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
            Return Me.GetHashCode
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.m_Base.GetHashCode
        End Function

        Public Function iterator() As Iterator Implements List.iterator
            Return New Collections.GenericIterator(Of Object)(Me)
        End Function

        Public Function removeAll(col As ICollection) As Boolean Implements List.removeAll
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

        Public Function retainAll(col As ICollection) As Boolean Implements List.retainAll
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

        Public Function toArray(Of T)() As T() Implements List.toArray
            Return Me.m_Base.ToArray(GetType(T))
        End Function

    End Class

End Namespace