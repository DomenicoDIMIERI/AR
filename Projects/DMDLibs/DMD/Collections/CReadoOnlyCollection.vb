Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.Sistema

<Serializable>
Public Class CReadOnlyCollection
    'Inherits System.Collections.CollectionBase
    Implements System.Collections.ICollection, IComparer, XML.IDMDXMLSerializable, IDBObjectCollection

    Protected m_List As Object
    Protected m_Sorted As Boolean
    Private m_Comparer As IComparer
    'Private m_SyncRoot As New Object

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.m_List = New System.Collections.ArrayList
        Me.m_Sorted = False
        Me.m_Comparer = Arrays.DefaultComparer
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)

    End Sub

    Public Overridable ReadOnly Property Count As Integer Implements System.Collections.ICollection.Count
        Get
            Return DirectCast(Me.m_List, System.Collections.ArrayList).Count
        End Get
    End Property

    Public Overridable Sub CopyTo(array As Array, index As Integer) Implements System.Collections.ICollection.CopyTo
        DirectCast(Me.m_List, System.Collections.ArrayList).CopyTo(array, index)
    End Sub

    Public Overridable ReadOnly Property IsSynchronized As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return False
        End Get
    End Property

    Public Overridable ReadOnly Property SyncRoot As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return Me
        End Get
    End Property

    Public Overridable Function GetEnumerator() As IEnumerator Implements IDBObjectCollection.GetEnumerator
        Return DirectCast(Me.m_List, System.Collections.ArrayList).GetEnumerator
    End Function

    Protected Overridable Sub InternalRemoveAt(ByVal index As Integer)
        DirectCast(Me.m_List, System.Collections.ArrayList).RemoveAt(index)
    End Sub

    Protected Overridable Sub InternalAdd(ByVal item As Object)
        DirectCast(Me.m_List, System.Collections.ArrayList).Add(item)
    End Sub

    Protected Overridable Sub InternalInsert(ByVal index As Integer, ByVal item As Object)
        DirectCast(Me.m_List, System.Collections.ArrayList).Insert(index, item)
    End Sub

    Protected Overridable Sub InternalAddRange(ByVal items As System.Collections.ICollection)
        DirectCast(Me.m_List, System.Collections.ArrayList).AddRange(items)
    End Sub

    Protected Overridable Sub OnInsert(index As Integer, value As Object)

    End Sub

    Protected Overridable Sub OnInsertComplete(index As Integer, value As Object)

    End Sub

    Protected Overridable Sub OnSet(index As Integer, oldValue As Object, newValue As Object)

    End Sub

    Protected Overridable Sub OnSetComplete(index As Integer, oldValue As Object, newValue As Object)

    End Sub

    Protected Overridable Function InternalGet(ByVal index As Integer) As Object
        Return DirectCast(Me.m_List, System.Collections.ArrayList).Item(index)
    End Function

    ''' <summary>
    ''' Restituisce o imposta l'oggetto che viene utilizzato per l'ordinamento
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Comparer As Object
        Get
            Return Me.m_Comparer
        End Get
        Set(value As Object)
            If (Me.m_Comparer Is value) Then Exit Property
            Me.m_Comparer = value
            If (Me.m_Sorted) Then Me.Sort()
        End Set
    End Property

    ''' <summary>
    ''' Cerca e restituisce all'interno della collezione l'oggetto con l'ID specificato
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItemById(ByVal id As Integer) As Object
        Dim i As Integer = 0
        If (Me.IsSynchronized) Then
            SyncLock Me.SyncRoot
                While (i < Me.Count)
                    Dim o As Object = Me(i)
                    If GetID(o) = id Then Return o
                    i += 1
                End While
                Return Nothing
            End SyncLock
        Else
            While (i < Me.Count)
                Dim o As Object = Me(i)
                If GetID(o) = id Then Return o
                i += 1
            End While
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Restituisce o imposta un valore booleano che indica se la collezione è ordinata
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Sorted As Boolean
        Get
            Return Me.m_Sorted
        End Get
        Set(value As Boolean)
            If (Me.m_Sorted = value) Then Exit Property
            Me.m_Sorted = value
            If (Me.m_Sorted) Then Me.Sort()
        End Set
    End Property

    Protected Overridable Sub InternalClear()
        DirectCast(Me.m_List, System.Collections.ArrayList).Clear()
    End Sub

    Public Overridable Sub Sort()
        Dim items() As Object
        If (Me.IsSynchronized) Then
            SyncLock Me.SyncRoot
                If Me.Count > 0 Then
                    Dim e As New System.EventArgs
                    Me.OnBeforeSort(e)
                    items = Me.ToArray
                    Arrays.Sort(items, 0, Me.Count, Me)
                    Me.InternalClear()
                    Me.InternalAddRange(items)
                    Me.OnAfterSort(e)
                End If
            End SyncLock
        Else
            If Me.Count > 0 Then
                Dim e As New System.EventArgs
                Me.OnBeforeSort(e)
                items = Me.ToArray
                Arrays.Sort(items, 0, Me.Count, Me)
                Me.InternalClear()
                Me.InternalAddRange(items)
                Me.OnAfterSort(e)
            End If
        End If
    End Sub

    Protected Overridable Sub OnBeforeSort(ByVal e As System.EventArgs)

    End Sub

    Protected Overridable Sub OnAfterSort(ByVal e As System.EventArgs)

    End Sub

    Public Overridable Function ToArray() As Object()
        Dim ret() As Object = Nothing
        If (Me.IsSynchronized) Then
            SyncLock Me.SyncRoot
                If Me.Count > 0 Then
                    ReDim ret(Me.Count - 1)
                    Me.CopyTo(ret, 0)
                End If
                Return ret
            End SyncLock
        Else
            If Me.Count > 0 Then
                ReDim ret(Me.Count - 1)
                Me.CopyTo(ret, 0)
            End If
            Return ret
        End If
    End Function

    Public Overridable Function ToArray(Of T)() As T()
        Dim ret() As T = Nothing
        If (Me.IsSynchronized) Then
            SyncLock Me.SyncRoot
                If Me.Count > 0 Then
                    ReDim ret(Me.Count - 1)
                    Me.CopyTo(ret, 0)
                End If
                Return ret
            End SyncLock
        Else
            If Me.Count > 0 Then
                ReDim ret(Me.Count - 1)
                Me.CopyTo(ret, 0)
            End If
            Return ret
        End If
    End Function

    Default Public ReadOnly Property Item(ByVal index As Integer) As Object
        Get
            Return Me.InternalGet(index)
        End Get
    End Property

    Public Function IndexOf(ByVal item As Object) As Integer
        If (Me.IsSynchronized) Then
            SyncLock Me.SyncRoot
                If Me.m_Sorted Then
                    Return Me.BinarySearch(item, 0, Me.Count - 1)
                Else
                    Return Me.LinearSearch(item, 0, Me.Count - 1)
                End If
            End SyncLock
        Else
            If Me.m_Sorted Then
                Return Me.BinarySearch(item, 0, Me.Count - 1)
            Else
                Return Me.LinearSearch(item, 0, Me.Count - 1)
            End If
        End If
    End Function

    Private Function BinarySearch(ByVal obj As Object, ByVal left As Integer, ByVal right As Integer) As Integer
        Dim ret As Integer = -1
        If (left = right) Then
            ret = IIf(Me.Compare(obj, Me.Item(left)) = 0, left, -1)
        ElseIf (left < right) Then
            Dim m, c As Integer
            m = CInt((left + right) / 2)
            c = Me.Compare(obj, Me.Item(m))
            If (c < 0) Then
                ret = Me.BinarySearch(obj, left, m - 1)
            ElseIf (c > 0) Then
                ret = Me.BinarySearch(obj, m + 1, right)
            Else
                ret = m
            End If
        End If
        Return ret
    End Function

    Private Function LinearSearch(ByVal obj As Object, ByVal left As Integer, ByVal right As Integer) As Integer
        'For i As Integer = left To right
        '    If Me.Compare(obj, Me.Item(i)) = 0 Then Return i
        'Next
        'Return -1
        Return DirectCast(Me.m_List, System.Collections.ArrayList).IndexOf(obj, left, right - left + 1)
    End Function


    Public Function Contains(ByVal item As Object) As Boolean
        Return (Me.IndexOf(item) >= 0)
    End Function


    Protected Overridable Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
        Return Me.m_Comparer.Compare(a, b)
    End Function


    Public Overrides Function ToString() As String
        Dim ret As New System.Text.StringBuilder(2048)
        Dim i As Integer
        If (Me.IsSynchronized) Then
            SyncLock Me.SyncRoot
                For i = 0 To Me.Count - 1
                    If (i > 0) Then ret.Append(vbCrLf)
                    ret.Append(Me(i).ToString)
                Next
                Return ret.ToString
            End SyncLock
        Else
            For i = 0 To Me.Count - 1
                If (i > 0) Then ret.Append(vbCrLf)
                ret.Append(Me(i).ToString)
            Next
            Return ret.ToString
        End If
    End Function

    'Private Sub AddRange(ByVal items() As Object)
    '    Dim i As Integer
    '    For i = 0 To UBound(items)
    '        MyBase.List.Add(items(i))
    '    Next
    'End Sub



    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "items"
                Me.InternalClear()
                'If (TypeOf (fieldValue) Is String AndAlso CStr(fieldValue) = "") Then
                '    'Dim arr() As Object = Arrays.Convert(Of Object)(fieldValue)
                '    'If (arr IsNot Nothing) Then Me.InternalAddRange(arr)
                '    Dim arr() As Object = Nothing
                'Else
                Dim arr() As Object = Arrays.Convert(Of Object)(fieldValue)
                If (arr IsNot Nothing) Then Me.InternalAddRange(arr)
                'End If

            Case Else 'Throw New MissingFieldException(fieldName)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.BeginTag("items")
        If Me.Count > 0 Then writer.Write(Me.ToArray)
        writer.EndTag()
    End Sub

    Protected Overridable Function DropFromDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean Implements IDBMinObject.DropFromDatabase
        Dim i As Integer = 0
        While (i < Me.Count)
            Dim item As DBObjectBase = Me(i)
            item.Delete(force)
            i += 1
        End While
        Return True
    End Function

    Protected Overridable Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean Implements IDBMinObject.SaveToDatabase
        Dim i As Integer = 0
        While (i < Me.Count)
            Dim item As DBObjectBase = Me(i)
            item.Save(force)
            i += 1
        End While
        Return True
    End Function

    Public Overridable Sub Save(Optional ByVal force As Boolean = False)
        Dim i As Integer = 0
        While (i < Me.Count)
            Dim item As DBObjectBase = Me(i)
            item.Save(force)
            i += 1
        End While
    End Sub

    Public Overridable Sub Delete(Optional ByVal force As Boolean = False)
        Dim i As Integer = 0
        While (i < Me.Count)
            Dim item As DBObjectBase = Me(i)
            item.Delete(force)
            i += 1
        End While
    End Sub


End Class
