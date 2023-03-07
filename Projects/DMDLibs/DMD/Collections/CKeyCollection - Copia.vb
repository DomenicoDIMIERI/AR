Imports DMD.Sistema

Public Class CKeyCollection
    Inherits CCollection



    Private m_Keys() As String
    Private m_Indicies() As Integer
    Private Shared m_KeyComparer As New CStringComparer(CompareMethod.Binary)
    
    Public Sub New()
        Me.m_Keys = {}
        Me.m_Indicies = {}
    End Sub

    Public Overrides ReadOnly Property IsSynchronized As Boolean
        Get
            Return False
        End Get
    End Property

    Public Sub New(ByVal col As CKeyCollection)
        Me.New()
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim keys() As String = col.Keys
                For i As Integer = 0 To UBound(keys)
                    Me.Add(keys(i), col(keys(i)))
                Next
            End SyncLock
        Else
            Dim keys() As String = col.Keys
            For i As Integer = 0 To UBound(keys)
                Me.Add(keys(i), col(keys(i)))
            Next
        End If
    End Sub


    Public Function GetItemByKey(ByVal key As String) As Object
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim i As Integer = Me.IndexOfKey(key)
                If (i < 0) Then Return Nothing
                Return Me.Item(i)
            End SyncLock
        Else
            Dim i As Integer = Me.IndexOfKey(key)
            If (i < 0) Then Return Nothing
            Return Me.Item(i)
        End If
    End Function

    Public Sub SetItemByKey(ByVal key As String, ByVal value As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim i As Integer = Me.IndexOfKey(key)
                If (i < 0) Then
                    Me.Add(key, value)
                Else
                    Me(i) = value
                End If
            End SyncLock
        Else
            Dim i As Integer = Me.IndexOfKey(key)
            If (i < 0) Then
                Me.Add(key, value)
            Else
                Me(i) = value
            End If
        End If
    End Sub

    Protected Overrides Sub OnRemove(index As Integer, value As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim kI As Integer = -1
                For i As Integer = 0 To Me.Count - 1
                    If Me.m_Indicies(i) = index Then
                        kI = i
                        Exit For
                    End If
                Next
                Me.m_Keys = Arrays.RemoveAt(Me.m_Keys, kI, 1)
                For i As Integer = 0 To Arrays.Len(Me.m_Indicies) - 1
                    If Me.m_Indicies(i) >= index Then Me.m_Indicies(i) -= 1
                Next
                Me.m_Indicies = Arrays.RemoveAt(Me.m_Indicies, kI, 1)
                MyBase.OnRemove(index, value)
            End SyncLock
        Else
            Dim kI As Integer = -1
            For i As Integer = 0 To Me.Count - 1
                If Me.m_Indicies(i) = index Then
                    kI = i
                    Exit For
                End If
            Next
            Me.m_Keys = Arrays.RemoveAt(Me.m_Keys, kI, 1)
            For i As Integer = 0 To Arrays.Len(Me.m_Indicies) - 1
                If Me.m_Indicies(i) >= index Then Me.m_Indicies(i) -= 1
            Next
            Me.m_Indicies = Arrays.RemoveAt(Me.m_Indicies, kI, 1)
            MyBase.OnRemove(index, value)
        End If
#If DEBUGKEY Then
        If (Not Arrays.IsSorted(Me.m_Keys, m_KeyComparer)) Then
            Throw New ArgumentException("Array delle chiavi non ordinato")
        End If
#End If
    End Sub

    Protected Overrides Sub OnRemoveComplete(index As Integer, value As Object)
        MyBase.OnRemoveComplete(index, value)
    End Sub

    Protected Overrides Sub OnClear()
        MyBase.OnClear()
    End Sub

    Protected Overrides Sub OnClearComplete()
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Me.m_Keys = {}
                Me.m_Indicies = {}
                MyBase.OnClearComplete()
            End SyncLock
        Else
            Me.m_Keys = {}
            Me.m_Indicies = {}
            MyBase.OnClearComplete()
        End If
    End Sub

    Protected Overrides Sub OnInsertComplete(index As Integer, value As Object)
        MyBase.OnInsertComplete(index, value)
#If DEBUGKEY Then
        If (Not Arrays.IsSorted(Me.m_Keys, m_KeyComparer)) Then
            Throw New ArgumentException("Array delle chiavi non ordinato")
        End If
#End If
    End Sub

    Public Shadows Sub Add(ByVal key As String, ByVal value As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim p As Integer = Arrays.GetInsertPosition(Me.m_Keys, key, 0, Me.Count, m_KeyComparer)
                Me.m_Keys = Arrays.Insert(Me.m_Keys, key, p)
                Me.m_Indicies = Arrays.Insert(Me.m_Indicies, Me.Count, p)
                MyBase.Add(value)
            End SyncLock
        Else
            Dim p As Integer = Arrays.GetInsertPosition(Me.m_Keys, key, 0, Me.Count, m_KeyComparer)
            Me.m_Keys = Arrays.Insert(Me.m_Keys, key, p)
            Me.m_Indicies = Arrays.Insert(Me.m_Indicies, Me.Count, p)
            MyBase.Add(value)
        End If

    End Sub

    'Private Sub SortKeys()
    '    Me.m_KeysChanged = False
    '    Arrays.Sort(Me.m_Keys, Me.m_Indicies, 0, Me.Count, m_KeyComparer)
    'End Sub

    Public Shadows Sub Insert(ByVal index As Integer, ByVal key As String, ByVal value As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim p As Integer = Arrays.GetInsertPosition(Me.m_Keys, key, 0, Me.Count, m_KeyComparer)
                Me.m_Keys = Arrays.Insert(Me.m_Keys, key, p)
                For i As Integer = 0 To Arrays.Len(Me.m_Indicies) - 1
                    If (Me.m_Indicies(i) >= index) Then Me.m_Indicies(i) += 1
                Next
                Me.m_Indicies = Arrays.Insert(Me.m_Indicies, index, p)
#If DEBUGKEY Then
                If (Not Arrays.IsSorted(Me.m_Keys, m_KeyComparer)) Then
                    Throw New ArgumentException("Array delle chiavi non ordinato")
                End If
#End If
                MyBase.Insert(index, value)
            End SyncLock
        Else
            Dim p As Integer = Arrays.GetInsertPosition(Me.m_Keys, key, 0, Me.Count, m_KeyComparer)
            Me.m_Keys = Arrays.Insert(Me.m_Keys, key, p)
            For i As Integer = 0 To Arrays.Len(Me.m_Indicies) - 1
                If (Me.m_Indicies(i) >= index) Then Me.m_Indicies(i) += 1
            Next
            Me.m_Indicies = Arrays.Insert(Me.m_Indicies, index, p)
#If DEBUGKEY Then
            If (Not Arrays.IsSorted(Me.m_Keys, m_KeyComparer)) Then
                Throw New ArgumentException("Array delle chiavi non ordinato")
            End If
#End If
            MyBase.Insert(index, value)
        End If
    End Sub

    Default Public Overloads Property Item(ByVal key As String) As Object
        Get
            If Me.IsSynchronized Then
                SyncLock Me.SyncRoot
                    Dim i As Integer = Me.IndexOfKey(key)
                    If (i < 0 OrElse i >= Me.Count) Then Throw New ArgumentOutOfRangeException("Chiave non valida: [" & key & "]")
                    Return MyBase.Item(i)
                End SyncLock
            Else
                Dim i As Integer = Me.IndexOfKey(key)
                If (i < 0 OrElse i >= Me.Count) Then Throw New ArgumentOutOfRangeException("Chiave non valida: [" & key & "]")
                Return MyBase.Item(i)
            End If
        End Get
        Set(value As Object)
            If Me.IsSynchronized Then
                SyncLock Me.SyncRoot
                    Dim i As Integer = Me.IndexOfKey(key)
                    If (i < 0 OrElse i >= Me.Count) Then Throw New ArgumentOutOfRangeException("Chiave non valida: [" & key & "]")
                    MyBase.Item(i) = value
                End SyncLock
            Else
                Dim i As Integer = Me.IndexOfKey(key)
                If (i < 0 OrElse i >= Me.Count) Then Throw New ArgumentOutOfRangeException("Chiave non valida: [" & key & "]")
                MyBase.Item(i) = value
            End If
        End Set
    End Property

    Public Shadows Function IndexOf(ByVal item As Object) As Integer
        Return MyBase.IndexOf(item)
    End Function

    Public Shadows Function Contains(ByVal item As Object) As Boolean
        Return Me.IndexOf(item) >= 0
    End Function

    Public Shadows Sub Remove(ByVal item As Object)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Me.RemoveAt(Me.IndexOf(item))
            End SyncLock
        Else
            Me.RemoveAt(Me.IndexOf(item))
        End If
    End Sub

    Public Overrides Sub RemoveAt(ByVal index As Integer)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                MyBase.RemoveAt(index)
            End SyncLock
        Else
            MyBase.RemoveAt(index)
        End If
    End Sub

    Public Function IndexOfKey(ByVal key As String) As Integer
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim i As Integer = -1
                If (Me.m_Keys IsNot Nothing) Then i = Array.BinarySearch(Me.m_Keys, key, m_KeyComparer) ' Arrays.BinarySearch(Me.m_Keys, 0, Me.Count, key, m_KeyComparer)
                If (i >= 0) Then Return Me.m_Indicies(i)
                Return -1
            End SyncLock
        Else
            Dim i As Integer = -1
#If DEBUGKEY Then
            If (Not Arrays.IsSorted(Me.m_Keys, m_KeyComparer)) Then
                Throw New ArgumentException("Array delle chiavi non ordinato")
            End If
#End If
            If (Me.m_Keys IsNot Nothing) Then i = Array.BinarySearch(Me.m_Keys, key, m_KeyComparer) ' Arrays.BinarySearch(Me.m_Keys, 0, Me.Count, key, m_KeyComparer)
                If (i >= 0) Then Return Me.m_Indicies(i)
                Return -1
            End If
    End Function

    Public Function ContainsKey(ByVal key As String) As Boolean
        Return (Me.IndexOfKey(key) >= 0)
    End Function

    Sub RemoveByKey(ByVal Key As String)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                Dim i As Integer = Me.IndexOfKey(Key)
                If (i < 0) Then Throw New ArgumentOutOfRangeException("Chiave non valida: [" & Key & "]")
                Me.RemoveAt(i)
            End SyncLock
        Else
            Dim i As Integer = Me.IndexOfKey(Key)
            If (i < 0) Then Throw New ArgumentOutOfRangeException("Chiave non valida: [" & Key & "]")
            Me.RemoveAt(i)
        End If
    End Sub

    Public ReadOnly Property Keys As String()
        Get
            If Me.IsSynchronized Then
                SyncLock Me.SyncRoot
                    If (Me.m_Keys Is Nothing) Then
                        Return New String() {}
                    Else
                        Dim ret As String() = Array.CreateInstance(GetType(String), Me.m_Keys.Length)
                        For i As Integer = 0 To Me.m_Keys.Length - 1
                            ret(i) = Me.m_Keys(i)
                        Next
                        Return ret
                    End If
                End SyncLock
            Else
                If (Me.m_Keys Is Nothing) Then
                    Return New String() {}
                Else
                    Dim ret As String() = Array.CreateInstance(GetType(String), Me.m_Keys.Length)
                    For i As Integer = 0 To Me.m_Keys.Length - 1
                        ret(i) = Me.m_Keys(i)
                    Next
                    Return ret
                End If
            End If
        End Get
    End Property

    Friend Structure KeyValue
        Implements DMD.XML.IDMDXMLSerializable
        Public Key As String
        Public Value As Object

        Public Overrides Function ToString() As String
            Return Me.Key & "/" & Me.Value.ToString
        End Function

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Key" : Me.Key = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TypeCode" : Me.Value = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Value" : Me.Value = Types.CastTo(fieldValue, CInt(Me.Value))
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Key", Me.Key)
            writer.WriteAttribute("TypeCode", Types.GetTypeCode(Me.Value))
            writer.WriteAttribute("Value", CStr(Types.CastTo(Me.Value, TypeCode.String)))
        End Sub
    End Structure

    Friend Class KeyValueComparer
        Implements IComparer

        Public c As CKeyCollection

        Public Sub New(ByVal c As CKeyCollection)
            Me.c = c
        End Sub

        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim a As KeyValue = x
            Dim b As KeyValue = y
            Return c.Compare(a.Value, b.Value)
        End Function
    End Class

    Public Overrides Sub Sort()
        Dim e As New System.EventArgs
        If (Me.IsSynchronized) Then
            SyncLock Me.SyncRoot
                Me.OnBeforeSort(e)
                If Me.Count > 0 Then
                    Dim items() As KeyValue
                    Dim oldV As Boolean = Me.m_Sorted
                    Me.m_Sorted = False
                    Dim i As Integer
                    ReDim items(Me.Count - 1)
                    For i = 0 To Me.Count - 1
                        With items(i)
                            .Key = Me.m_Keys(i)
                            .Value = MyBase.Item(Me.m_Indicies(i))
                        End With
                    Next
                    Arrays.Sort(items, 0, Me.Count, New KeyValueComparer(Me))
                    Me.Clear()
                    For i = 0 To UBound(items)
                        Me.Add(items(i).Key, items(i).Value)
                    Next
                    Me.m_Sorted = oldV
                End If
                Me.OnAfterSort(e)
            End SyncLock
        Else
            Me.OnBeforeSort(e)
            If Me.Count > 0 Then
                Dim items() As KeyValue
                Dim oldV As Boolean = Me.m_Sorted
                Me.m_Sorted = False
                Dim i As Integer
                ReDim items(Me.Count - 1)
                For i = 0 To Me.Count - 1
                    With items(i)
                        .Key = Me.m_Keys(i)
                        .Value = MyBase.Item(Me.m_Indicies(i))
                    End With
                Next
                Arrays.Sort(items, 0, Me.Count, New KeyValueComparer(Me))
                Me.Clear()
                For i = 0 To UBound(items)
                    Me.Add(items(i).Key, items(i).Value)
                Next
                Me.m_Sorted = oldV
            End If
            Me.OnAfterSort(e)
        End If
    End Sub

    Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
        If Me.IsSynchronized Then
            SyncLock Me.SyncRoot
                MyBase.XMLSerialize(writer)
                writer.BeginTag("keys")
                If Me.Count > 0 Then writer.Write(Me.m_Keys)
                writer.EndTag()
                writer.BeginTag("Indicies")
                If Me.Count > 0 Then writer.Write(Me.m_Indicies)
                writer.EndTag()
            End SyncLock
        Else
            MyBase.XMLSerialize(writer)
            writer.BeginTag("keys")
            If Me.Count > 0 Then writer.Write(Me.m_Keys)
            writer.EndTag()
            writer.BeginTag("Indicies")
            If Me.Count > 0 Then writer.Write(Me.m_Indicies)
            writer.EndTag()
        End If
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case "keys" : Me.m_Keys = Arrays.Convert(Of String)(fieldValue)
            Case "Indicies"
                Me.m_Indicies = Arrays.Convert(Of Integer)(fieldValue)
                If (Me.Count() <> Arrays.Len(Me.m_Keys)) Then
                    Throw New Exception("XML Deserialization Execption")
                End If
                If (Me.Count() <> Arrays.Len(Me.m_Indicies)) Then
                    Throw New Exception("XML Deserialization Execption")
                End If
            Case "items"
                MyBase.SetFieldInternal(fieldName, fieldValue)

            Case Else
                MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select


    End Sub


End Class
