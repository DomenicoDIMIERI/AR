Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.Sistema

<Serializable>
Public Class CCollection
    Inherits DMD.CReadOnlyCollection

    Public Event CollectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Sub New()
    End Sub

    Public Sub New(ByVal items As IEnumerable)
        Me.New()
        Me.AddRange(items)
    End Sub

    Public Sub Clear()
        Dim e As New System.EventArgs
        Me.OnClear()
        Me.InternalClear()
        Me.OnClearComplete()
        Me.OnCollectionChanged(e)
    End Sub

    Public Overrides ReadOnly Property IsSynchronized As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Overridable Sub OnClear()

    End Sub

    Protected Overridable Sub OnClearComplete()

    End Sub

    Protected Overridable Sub OnCollectionChanged(ByVal e As System.EventArgs)
        RaiseEvent CollectionChanged(Me, e)
    End Sub

    Public Overridable Property Capacity As Integer
        Get
            Return DirectCast(Me.m_List, System.Collections.ArrayList).Capacity
        End Get
        Set(value As Integer)
            DirectCast(Me.m_List, System.Collections.ArrayList).Capacity = value
        End Set
    End Property

    Private Sub InternalMerge(ByVal col As CReadOnlyCollection)
        Dim tmpa() As Object
        Dim tmpb() As Object
        Dim tmpc() As Object

        If (col.Count > 0) Then
            tmpb = col.ToArray
            Arrays.Sort(tmpb, 0, col.Count, Me.Comparer)
            If Me.Count = 0 Then
                tmpc = tmpb
            Else
                tmpa = Me.ToArray
                Arrays.Sort(tmpa, 0, Me.Count, Me.Comparer)
                tmpc = Arrays.Merge(tmpa, 0, Me.Count, tmpb, 0, col.Count, Me.Comparer)
            End If
            Me.InternalClear()
            Me.AddRange(tmpc)
        End If
    End Sub

    Public Overridable Sub Merge(ByVal col As CReadOnlyCollection)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Me.InternalMerge(col)
            End SyncLock
        Else
            Me.InternalMerge(col)
        End If
    End Sub


    Public Sub Add(ByVal item As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim index As Integer = Me.Count
                Me.OnInsert(index, item)
                Me.InternalAdd(item)
                If Me.Sorted Then Me.Sort()
                Me.OnInsertComplete(index, item)
            End SyncLock
        Else
            Dim index As Integer = Me.Count
            Me.OnInsert(index, item)
            Me.InternalAdd(item)
            If Me.Sorted Then Me.Sort()
            Me.OnInsertComplete(index, item)
        End If
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal item As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Me.OnInsert(index, item)
                Me.InternalInsert(index, item)
                If Me.Sorted Then Me.Sort()
                Me.OnInsertComplete(index, item)
            End SyncLock
        Else
            Me.OnInsert(index, item)
            Me.InternalInsert(index, item)
            If Me.Sorted Then Me.Sort()
            Me.OnInsertComplete(index, item)
        End If
    End Sub

    Default Public Shadows Property Item(ByVal index As Integer) As Object
        Get
            If Me.IsSynchronized Then
                SyncLock Me.SyncRoot
                    If (index < 0 OrElse index >= Me.Count) Then Throw New IndexOutOfRangeException()
                    Return MyBase.InternalGet(index)
                End SyncLock
            Else
                If (index < 0 OrElse index >= Me.Count) Then Throw New IndexOutOfRangeException()
                Return MyBase.InternalGet(index)
            End If
        End Get
        Set(value As Object)
            If Me.IsSynchronized Then
                SyncLock Me.SyncRoot
                    If (index < 0 OrElse index >= Me.Count) Then Throw New IndexOutOfRangeException()
                    Dim oldValue As Object = Me.InternalGet(index)
                    Me.OnSet(index, oldValue, value)
                    Me.InternalSet(index, value)
                    Me.OnSetComplete(index, oldValue, value)
                End SyncLock
            Else
                If (index < 0 OrElse index >= Me.Count) Then Throw New IndexOutOfRangeException()
                Dim oldValue As Object = Me.InternalGet(index)
                Me.OnSet(index, oldValue, value)
                Me.InternalSet(index, value)
                Me.OnSetComplete(index, oldValue, value)
            End If
        End Set
    End Property

    Protected Overridable Sub InternalSet(ByVal index As Integer, ByVal value As Object)
        DirectCast(Me.m_List, System.Collections.ArrayList).Item(index) = value
    End Sub

    Public Overridable Sub RemoveAt(ByVal index As Integer)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim value As Object = Me.InternalGet(index)
                Me.OnRemove(index, value)
                Me.InternalRemoveAt(index)
                Me.OnRemoveComplete(index, value)
                Me.OnCollectionChanged(New System.EventArgs)
            End SyncLock
        Else
            Dim value As Object = Me.InternalGet(index)
            Me.OnRemove(index, value)
            Me.InternalRemoveAt(index)
            Me.OnRemoveComplete(index, value)
            Me.OnCollectionChanged(New System.EventArgs)
        End If
    End Sub

    Protected Overridable Sub OnRemove(index As Integer, value As Object)

    End Sub

    Protected Overridable Sub OnRemoveComplete(index As Integer, value As Object)

    End Sub


    Protected Overrides Sub InternalRemoveAt(ByVal index As Integer)
        DirectCast(Me.m_List, System.Collections.ArrayList).RemoveAt(index)
    End Sub

    Public Sub Remove(ByVal item As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Me.RemoveAt(Me.IndexOf(item))
            End SyncLock
        Else
            Me.RemoveAt(Me.IndexOf(item))
        End If
    End Sub

    Public Sub AddRange(ByVal items As IEnumerable)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                For Each item As Object In items
                    Dim index As Integer = Me.Count
                    Me.OnInsert(index, item)
                    Me.InternalAdd(item)
                    Me.OnInsertComplete(index, item)
                Next
                If Me.Sorted Then Me.Sort()
            End SyncLock
        Else
            For Each item As Object In items
                Dim index As Integer = Me.Count
                Me.OnInsert(index, Item)
                Me.InternalAdd(Item)
                Me.OnInsertComplete(index, item)
            Next
            If Me.Sorted Then Me.Sort()
        End If
    End Sub

    Protected Overrides Sub OnInsertComplete(index As Integer, value As Object)
        MyBase.OnInsertComplete(index, value)
        Me.OnCollectionChanged(New System.EventArgs)
    End Sub

   
    Protected Overrides Sub OnSetComplete(index As Integer, oldValue As Object, newValue As Object)
        MyBase.OnSetComplete(index, oldValue, newValue)
        Me.OnCollectionChanged(New System.EventArgs)
    End Sub

    Public Overridable Function IsChanged() As Boolean
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                For Each o As Object In Me
                    If (DBUtils.IsChanged(o)) Then Return True
                Next
                Return False
            End SyncLock
        Else
            For Each o As Object In Me
                If (DBUtils.IsChanged(o)) Then Return True
            Next
            Return False
        End If
    End Function

    Public Overridable Sub SetChanged(ByVal value As Boolean)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                For Each o As Object In Me
                    DBUtils.SetChanged(o, value)
                Next
            End SyncLock
        Else
            For Each o As Object In Me
                DBUtils.SetChanged(o, value)
            Next
        End If
    End Sub

    Public Overridable Function IntersectWith(ByVal col As CCollection) As CCollection
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim ret As CCollection = Types.CreateInstance(Me.GetType)
                For Each a As Object In Me
                    For Each b As Object In col
                        If (Arrays.Compare(a, b, Me.Comparer) = 0) Then
                            ret.Add(a)
                        End If
                    Next
                Next
                Return ret
            End SyncLock
        Else
            Dim ret As CCollection = Types.CreateInstance(Me.GetType)
            For Each a As Object In Me
                For Each b As Object In col
                    If (Arrays.Compare(a, b, Me.Comparer) = 0) Then
                        ret.Add(a)
                    End If
                Next
            Next
            Return ret
        End If
    End Function

End Class
