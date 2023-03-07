
Public Interface ISupportInitializeFrom

    Sub InitializeFrom(ByVal value As Object)

End Interface

<Serializable>
Public Class DMDObject
    Implements ISupportInitializeFrom

    Public Class ObjectInfo
        Implements IComparable

        Public TypeName As String
        Public NewCount As Integer
        Public FinCount As Integer
        Public TotaleMemory As Integer
        Public LastNew As Date?
        Public LastFin As Date?

        Public Sub New(ByVal tn As String)
            Me.TypeName = tn
            Me.NewCount = 0
            Me.FinCount = 0
            Me.LastNew = Nothing
            Me.LastFin = Nothing
            Me.TotaleMemory = 0
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return System.String.Compare(Me.TypeName, DirectCast(obj, ObjectInfo).TypeName)
        End Function

        Public Sub IncNew()
            Me.LastNew = Now
            Me.NewCount += 1
        End Sub

        Public Sub IncFin()
            Me.LastFin = Now
            Me.FinCount += 1
        End Sub

        Public Function Count() As Integer
            Return Me.NewCount - Me.FinCount
        End Function

    End Class

    Private Shared ObjectCounters() As ObjectInfo = {}

    Private Shared Function Compare(ByVal a As ObjectInfo, ByVal b As ObjectInfo) As Integer
        Return a.CompareTo(b)
    End Function



    Private Shared Function GetInsertPosition(
                                ByVal items() As ObjectInfo,
                                ByVal item As ObjectInfo,
                                ByVal fromIndex As Integer,
                                ByVal arrayLen As Integer
                                ) As Integer
        If (arrayLen = 0) Then Return fromIndex
        Dim p As Integer
        p = Compare(item, items(fromIndex))
        If (p < 0) Then Return fromIndex
        p = Compare(item, items(fromIndex + arrayLen - 1))
        If (p >= 0) Then Return fromIndex + arrayLen
        Dim m As Integer = Math.Floor(arrayLen / 2)
        p = Compare(item, items(fromIndex + m))
        If (p < 0) Then
            Return GetInsertPosition(items, item, fromIndex, m)
        ElseIf (p > 0) Then
            Return GetInsertPosition(items, item, fromIndex + m + 1, arrayLen - m - 1)
        Else
            Return m
        End If
        'For i As Integer = fromIndex To fromIndex + arrayLen - 1
        '    If Compare(items(i), item, comparer) >= 0 Then Return i
        'Next
        'Return fromIndex + arrayLen
    End Function

    Private Shared Function Insert(ByVal items() As ObjectInfo, ByVal fromIndex As Integer, ByVal arrayLen As Integer, ByVal item As ObjectInfo, ByVal insertIndex As Integer) As ObjectInfo()
        Dim i As Integer
        If items Is Nothing Then
            ReDim items(0)
        Else
            ReDim Preserve items(1 + UBound(items))
        End If
        For i = fromIndex + arrayLen To insertIndex + 1 Step -1
            items(i) = items(i - 1)
        Next
        items(insertIndex) = item

        Return items
    End Function


    Public Shared Property EnableTracking As Boolean = False

    Public Shared Sub IncreaseCounter(ByVal obj As Object)
        If EnableTracking = False Then Return

        SyncLock ObjectCounters
            Dim o As New ObjectInfo(obj.GetType.FullName)
            Dim i As Integer = Array.BinarySearch(ObjectCounters, o)
            If (i < 0) Then
                i = GetInsertPosition(ObjectCounters, o, 0, ObjectCounters.Length)
                ObjectCounters = Insert(ObjectCounters, 0, ObjectCounters.Length, o, i)
            End If
            With ObjectCounters(i)
                .IncNew()
                '.TotaleMemory += GuessMemorySize(obj)
            End With
        End SyncLock
    End Sub

    Public Shared Sub DecreaseCounter(ByVal obj As Object)
        If EnableTracking = False Then Return

        SyncLock ObjectCounters
            Dim o As New ObjectInfo(obj.GetType.FullName)
            Dim i As Integer = Array.BinarySearch(ObjectCounters, o)
            With ObjectCounters(i)
                .IncFin()
                '.TotaleMemory -= GuessMemorySize(obj)
            End With
        End SyncLock
    End Sub

    'Private Shared Function GuessMemorySize(ByVal obj As Object) As Integer
    '    Dim m() As System.Runtime.
    'End Function

    Public Shared Function GetCounters() As ObjectInfo()
        SyncLock ObjectCounters
            Return ObjectCounters.Clone
        End SyncLock
    End Function

    Public Sub New()
        IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DecreaseCounter(Me)
    End Sub

    ''' <summary>
    ''' Copia tutti i valori delle proprietà
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Overridable Sub InitializeFrom(value As Object) Implements ISupportInitializeFrom.InitializeFrom
        Dim f1() As System.Reflection.FieldInfo = GetAllFields(Me.GetType)
        For Each f As System.Reflection.FieldInfo In f1
            If Not f.IsInitOnly Then
                f.SetValue(Me, f.GetValue(value))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Copia tutti i valori delle proprietà 
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Overridable Sub CopyFrom(value As Object)
        Dim f1() As System.Reflection.FieldInfo = GetAllFields(Me.GetType)
        For Each f As System.Reflection.FieldInfo In f1
            If Not f.IsInitOnly Then
                f.SetValue(Me, f.GetValue(value))
            End If
        Next
    End Sub

    Private Class CTypeInfo
        Implements IComparable

        Public t As System.Type
        Public DeclaringTypeName As String
        Public arr As System.Reflection.FieldInfo()

        Public Sub New()
            Me.DeclaringTypeName = vbNullString
            Me.arr = {}
            Me.t = Nothing
        End Sub

        Public Sub New(ByVal t As System.Type)
            Me.DeclaringTypeName = t.FullName
            Me.arr = {}
            Me.t = t
        End Sub

        Public Sub Init()
            Dim comparer As New TComparer
            Dim t1 As System.Type = Me.t
            While (t1 IsNot Nothing)
                Dim tmp As System.Reflection.FieldInfo() = t1.GetFields(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.DeclaredOnly)
                Me.arr = Merge(Me.arr, 0, Me.arr.Length, tmp, 0, tmp.Length, comparer)
                t1 = t1.BaseType
            End While
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim o As CTypeInfo = DirectCast(obj, CTypeInfo)
            Return String.Compare(Me.DeclaringTypeName, o.DeclaringTypeName, False)
        End Function
    End Class

    Private Shared mTypesBuff As CTypeInfo() = {}

    ''' <summary>
    ''' Restituisce tutti i campi definiti per il tipo e per le sue superclassi
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAllFields(ByVal t As System.Type) As System.Reflection.FieldInfo()
        SyncLock mTypesBuff
            If (t Is Nothing) Then Throw New ArgumentNullException("t")
            Dim o As New CTypeInfo(t)
            Dim i As Integer = Array.BinarySearch(mTypesBuff, o)
            If (i < 0) Then
                i = GetInsertPosition(mTypesBuff, o, 0, mTypesBuff.Length)
                o.Init
                mTypesBuff = Insert(mTypesBuff, 0, mTypesBuff.Length, o, i)
            End If
            Return mTypesBuff(i).arr
        End SyncLock

    End Function

    Private Class TComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim f1 As System.Reflection.FieldInfo = x
            Dim f2 As System.Reflection.FieldInfo = y
            Return String.Compare(f1.DeclaringType.FullName & "." & f1.Name, f2.DeclaringType.FullName & "." & f2.Name, False)
        End Function
    End Class

    Private Shared Function GetInsertPosition(
                               ByVal items() As CTypeInfo,
                               ByVal item As CTypeInfo,
                               ByVal fromIndex As Integer,
                               ByVal arrayLen As Integer
                               ) As Integer
        If (arrayLen = 0) Then Return fromIndex
        Dim p As Integer
        p = item.CompareTo(items(fromIndex))
        If (p < 0) Then Return fromIndex
        p = item.CompareTo(items(fromIndex + arrayLen - 1))
        If (p >= 0) Then Return fromIndex + arrayLen
        Dim m As Integer = Math.Floor(arrayLen / 2)
        p = item.CompareTo(items(fromIndex + m))
        If (p < 0) Then
            Return GetInsertPosition(items, item, fromIndex, m)
        ElseIf (p > 0) Then
            Return GetInsertPosition(items, item, fromIndex + m + 1, arrayLen - m - 1)
        Else
            Return m
        End If
    End Function

    Private Shared Function Insert(ByVal items() As CTypeInfo, ByVal fromIndex As Integer, ByVal arrayLen As Integer, ByVal item As CTypeInfo, ByVal insertIndex As Integer) As CTypeInfo()
        Dim i As Integer
        If items Is Nothing Then
            ReDim items(0)
        Else
            ReDim Preserve items(1 + UBound(items))
        End If
        For i = fromIndex + arrayLen To insertIndex + 1 Step -1
            items(i) = items(i - 1)
        Next
        items(insertIndex) = item

        Return items
    End Function

    Private Shared Function Merge(Of T)(
               ByVal arr1() As T,
               ByVal s1 As Integer,
               ByVal l1 As Integer,
               ByVal arr2() As T,
               ByVal s2 As Integer,
               ByVal l2 As Integer,
               ByVal comparer As IComparer
               ) As T()
        Dim arr() As T
        Dim i, i1, i2 As Integer
        ReDim arr(0)
        If (l1 + l2 > 0) Then
            ReDim arr(l1 + l2 - 1)
            i = 0 : i1 = 0 : i2 = 0
            While (i1 < l1 And i2 < l2)
                If comparer.Compare(arr1(s1 + i1), arr2(s2 + i2)) >= 0 Then
                    arr(i) = arr1(s1 + i1)
                    i1 = i1 + 1
                Else
                    arr(i) = arr2(s2 + i2)
                    i2 = i2 + 1
                End If
                i = i + 1
            End While
            While (i1 < l1)
                arr(i) = arr1(s1 + i1)
                i = i + 1 : i1 = i1 + 1
            End While
            While (i2 < l2)
                arr(i) = arr2(s2 + i2)
                i = i + 1 : i2 = i2 + 1
            End While
        Else
            arr = {}
        End If

        Return arr
    End Function
End Class