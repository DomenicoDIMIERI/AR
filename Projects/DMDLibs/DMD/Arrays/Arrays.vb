Imports DMD
Imports DMD.Sistema
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CArraysClass
        Private m_DefaultComparer As IComparer

        Friend Sub New()
        End Sub

        Public Function Clone(Of T)(ByVal items() As T) As T()
            Dim ret(UBound(items)) As T
            For i As Integer = 0 To UBound(items)
                ret(i) = items(i)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un'istanza del comparatore predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefaultComparer As Object
            Get
                If m_DefaultComparer Is Nothing Then m_DefaultComparer = New CDefaultComparer
                Return m_DefaultComparer
            End Get
        End Property

        ''' <summary>
        ''' Assegna a target il valore o il riferimento (se oggetto) di source
        ''' </summary>
        ''' <param name="target"></param>
        ''' <param name="source"></param>
        ''' <remarks></remarks>
        Public Sub Assign(Of T)(ByRef target As T, ByVal source As T)
            target = source
        End Sub

        ''' <summary>
        ''' Scambia i valori (o i riferimenti) di a e b
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="a"></param>
        ''' <param name="b"></param>
        ''' <remarks></remarks>
        Public Sub SwapValues(Of T)(ByRef a As T, ByRef b As T)
            Dim tmp As T = a
            a = b
            b = tmp
        End Sub

        Public Function Compare(Of T)(ByRef a As T, ByRef b As T, ByVal comparer As Object) As Integer
            Return DirectCast(comparer, IComparer).Compare(a, b)
        End Function

        Public Function Compare(Of T)(ByRef a As T, ByRef b As T) As Integer
            Return Compare(a, b, DefaultComparer)
        End Function

        ''' <summary>
        ''' Copia gli elementi dell'array source nell'array dest
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="dest"></param>
        ''' <param name="source"></param>
        ''' <remarks></remarks>
        Public Sub Copy(Of T)(ByRef dest() As T, ByRef source() As T)
            source.CopyTo(dest, 0)
        End Sub

        Public Function RemoveAt(Of T)(ByRef items() As T, ByVal position As Integer, Optional ByVal numItems As Integer = 1) As T()
            For i As Integer = position To UBound(items) - numItems
                items(i) = items(i + numItems)
            Next
            If (UBound(items) - numItems >= 0) Then
                ReDim Preserve items(UBound(items) - numItems)
            Else
                items = {}
            End If

            Return items
        End Function

        Public Function GetInsertPosition(ByVal items As System.Collections.ArrayList, ByVal item As Object, ByVal fromIndex As Integer, ByVal arrayLen As Integer) As Integer
            Return GetInsertPosition(items, item, fromIndex, arrayLen, DefaultComparer)
        End Function

        Public Function GetInsertPosition(ByVal items As System.Collections.ArrayList, ByVal item As Object, ByVal fromIndex As Integer, ByVal arrayLen As Integer, ByVal comparer As Object) As Integer
            If (arrayLen = 0) Then Return fromIndex
            Dim p As Integer
            p = Arrays.Compare(item, items(fromIndex), comparer)
            If (p < 0) Then Return fromIndex
            p = Arrays.Compare(item, items(fromIndex + arrayLen - 1), comparer)
            If (p >= 0) Then Return fromIndex + arrayLen
            Dim m As Integer = Math.Floor(arrayLen / 2)
            p = Arrays.Compare(item, items(fromIndex + m), comparer)
            If (p < 0) Then
                Return GetInsertPosition(items, item, fromIndex, m, comparer)
            ElseIf (p > 0) Then
                Return GetInsertPosition(items, item, fromIndex + m + 1, arrayLen - m - 1, comparer)
            Else
                Return fromIndex + m
            End If
            'For i As Integer = fromIndex To fromIndex + arrayLen - 1
            '    If Compare(items(i), item, comparer) >= 0 Then Return i
            'Next
            'Return fromIndex + arrayLen
        End Function

        Public Function GetInsertPosition(Of T)(
                                    ByVal items() As T,
                                    ByVal item As T,
                                    ByVal fromIndex As Integer,
                                    ByVal arrayLen As Integer
                                    ) As Integer
            Return GetInsertPosition(items, item, fromIndex, arrayLen, DefaultComparer)
        End Function

        Public Function GetInsertPosition(Of T)(
                                    ByVal items() As T,
                                    ByVal item As T,
                                    ByVal fromIndex As Integer,
                                    ByVal arrayLen As Integer,
                                    ByVal comparer As Object
                                    ) As Integer
            If (arrayLen = 0) Then Return fromIndex
            Dim p As Integer
            p = Arrays.Compare(item, items(fromIndex), comparer)
            If (p < 0) Then Return fromIndex
            p = Arrays.Compare(item, items(fromIndex + arrayLen - 1), comparer)
            If (p >= 0) Then Return fromIndex + arrayLen
            Dim m As Integer = Math.Floor(arrayLen / 2)
            p = Arrays.Compare(item, items(fromIndex + m), comparer)
            If (p < 0) Then
                Return GetInsertPosition(items, item, fromIndex, m, comparer)
            ElseIf (p > 0) Then
                Return GetInsertPosition(items, item, fromIndex + m + 1, arrayLen - m - 1, comparer)
            Else
                Return fromIndex + m
            End If
            'For i As Integer = fromIndex To fromIndex + arrayLen - 1
            '    If Compare(items(i), item, comparer) >= 0 Then Return i
            'Next
            'Return fromIndex + arrayLen
        End Function

        Public Function Insert(Of T)(ByRef items() As T, ByVal fromIndex As Integer, ByVal arrayLen As Integer, ByVal item As T, ByVal insertIndex As Integer) As T()
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


        Public Function Insert(Of T)(ByVal items() As T, ByVal item As T, ByVal insertIndex As Integer) As T()
            Return Me.Insert(items, 0, Arrays.Len(items), item, insertIndex)
        End Function


        'Public Function BinarySearch( _
        '           ByVal items As System.Collections.ArrayList, _
        '           ByVal left As Integer, _
        '           ByVal count As Integer, _
        '           ByRef item As Object _
        '           ) As Integer
        '    Return BinarySearch(items, left, count, item, DefaultComparer)
        'End Function

        'Public Function BinarySearch( _
        '            ByVal items As System.Collections.ArrayList, _
        '            ByVal left As Integer, _
        '            ByVal count As Integer, _
        '            ByRef item As Object, _
        '            ByVal comparer As Object _
        '            ) As Integer
        '    Return BinarySearch1(items, left, left + count - 1, item, comparer)
        'End Function

        'Private Function BinarySearch1(Of T)( _
        '            ByVal items As System.Collections.ArrayList, _
        '            ByVal left As Integer, _
        '            ByVal right As Integer, _
        '            ByRef item As T, _
        '            ByVal comparer As IComparer _
        '            ) As Integer
        '    If (left = right) AndAlso (right >= 0) Then
        '        Return IIf(Compare(Of Object)(item, items(left), comparer) = 0, left, -1)
        '    ElseIf (left < right) Then
        '        Dim m, c As Integer
        '        m = Fix((left + right) / 2)
        '        c = Compare(Of Object)(item, items(m), comparer)
        '        If (c < 0) Then Return BinarySearch1(items, left, m - 1, item, comparer)
        '        If (c > 0) Then Return BinarySearch1(items, m + 1, right, item, comparer)
        '        Return m
        '    Else
        '        Return -1
        '    End If
        '    'Return Array.BinarySearch(items, left, right - left + 1, item, comparer)
        'End Function

        Public Function BinarySearch(Of T)(
                   ByVal items() As T,
                   ByRef item As T
                   ) As Integer
            If (items Is Nothing) Then Return -1
            Return BinarySearch(items, 0, items.Length, item, DefaultComparer)
        End Function
        Public Function BinarySearch(Of T)(
                    ByVal items() As T,
                    ByVal left As Integer,
                    ByVal count As Integer,
                    ByRef item As T
                    ) As Integer
            Return BinarySearch(items, left, count, item, DefaultComparer)
        End Function

        Public Function BinarySearch(Of T)(
                    ByVal items() As T,
                    ByVal left As Integer,
                    ByVal count As Integer,
                    ByRef item As T,
                    ByVal comparer As Object
                    ) As Integer
            If (items Is Nothing OrElse Arrays.Len(items) <= 0) Then Return -1
            Return BinarySearch1(items, left, left + count - 1, item, comparer)
        End Function

        Private Function BinarySearch1(Of T)(
                    ByRef items() As T,
                    ByVal left As Integer,
                    ByVal right As Integer,
                    ByRef item As T,
                    ByVal comparer As IComparer
                    ) As Integer
            'If (left = right) Then
            '    Return IIf(Compare(item, items(left), comparer) = 0, left, -1)
            'ElseIf (left < right) Then
            '    Dim m, c As Integer
            '    m = Fix((left + right) / 2)
            '    c = Compare(item, items(m), comparer)
            '    If (c < 0) Then Return BinarySearch1(items, left, m - 1, item, comparer)
            '    If (c > 0) Then Return BinarySearch1(items, m + 1, right, item, comparer)
            '    Return m
            'Else
            '    Return -1
            'End If
            If (items Is Nothing OrElse Arrays.Len(items) <= 0 OrElse left > right) Then Return -1
            Return Array.BinarySearch(items, left, right - left + 1, item, comparer)
        End Function

        Public Function Append(Of T)(ByRef srcArray() As T, ByRef item As T) As T()
            Dim l As Integer
            l = Len(srcArray)
            If (l = 0) Then
                ReDim srcArray(0)
            Else
                ReDim Preserve srcArray(l)
            End If
            srcArray(UBound(srcArray)) = item
            Return srcArray
        End Function

        Public Function Merge(Of T)(ByVal arr1() As T, ByVal arr2() As T) As T()
            Return Me.Merge(arr1, 0, Arrays.Len(arr1), arr2, 0, Arrays.Len(arr2))
        End Function

        Public Function Merge(Of T)(ByVal arr1() As T, ByVal arr2() As T, ByVal comparer As Object) As T()
            Return Me.Merge(arr1, 0, Arrays.Len(arr1), arr2, 0, Arrays.Len(arr2), comparer)
        End Function

        Public Function Merge(Of T)(
                ByVal arr1() As T,
                ByVal s1 As Integer,
                ByVal l1 As Integer,
                ByVal arr2() As T,
                ByVal s2 As Integer,
                ByVal l2 As Integer
                ) As T()
            Return Merge(arr1, s1, l1, arr2, s2, l2, DefaultComparer)
        End Function

        Public Function Merge(Of T)(
                ByVal arr1() As T,
                ByVal s1 As Integer,
                ByVal l1 As Integer,
                ByVal arr2() As T,
                ByVal s2 As Integer,
                ByVal l2 As Integer,
                ByVal comparer As Object
                ) As T()
            Dim arr() As T
            Dim i, i1, i2 As Integer
            ReDim arr(0)
            If (l1 + l2 > 0) Then
                ReDim arr(l1 + l2 - 1)
                i = 0 : i1 = 0 : i2 = 0
                While (i1 < l1 And i2 < l2)
                    If Compare(arr1(s1 + i1), arr2(s2 + i2), comparer) >= 0 Then
                        Call Assign(arr(i), arr1(s1 + i1))
                        i1 = i1 + 1
                    Else
                        Call Assign(arr(i), arr2(s2 + i2))
                        i2 = i2 + 1
                    End If
                    i = i + 1
                End While
                While (i1 < l1)
                    Call Assign(arr(i), arr1(s1 + i1))
                    i = i + 1 : i1 = i1 + 1
                End While
                While (i2 < l2)
                    Call Assign(arr(i), arr2(s2 + i2))
                    i = i + 1 : i2 = i2 + 1
                End While
            Else
                Erase arr
            End If
            Return arr
        End Function

        Public Function IndexOf(Of T)(ByVal items() As T, ByVal item As T) As Integer
            Return IndexOf(items, 0, Len(items), item)
        End Function

        Public Function IndexOf(Of T)(ByVal items() As T, ByVal startIndex As Integer, ByVal arrayLen As Integer, ByVal item As T, ByVal comparer As Object) As Integer
            Dim i As Integer
            For i = startIndex To startIndex + arrayLen - 1
                If Compare(item, items(i), comparer) = 0 Then Return i
            Next
            Return -1
        End Function

        Public Sub LinSpace(ByRef items() As Integer, ByVal count As Integer, Optional ByVal [step] As Integer = 1)
            ReDim items(count - 1)
            For i As Integer = 0 To UBound(items)
                items(i) = i * [step]
            Next
        End Sub


        Public Function IndexOf(Of T)(
                    ByRef items() As T,
                    ByVal startIndex As Integer,
                    ByVal arrayLen As Integer,
                    ByRef item As T
                    ) As Integer
            Return IndexOf(items, startIndex, arrayLen, item, DefaultComparer)
        End Function

        Public Sub Shuffle(Of T)(ByRef items() As T, ByVal indexes() As Integer, ByVal fromIndex As Integer, ByVal count As Integer)
            Dim cc As Integer = fromIndex + Fix(count / 2)
            Dim top As Integer = fromIndex + count - 1
            For i As Integer = fromIndex To cc
                SwapValues(items(indexes(i)), items(indexes(top - i)))
            Next
        End Sub

        Private Class Sort2ArrItem(Of T, T1)
            Implements IComparable

            Public Key As T
            Public Value As T1
            Public Comparer As IComparer

            Public Sub New(ByVal key As T, ByVal value As T1)
                Me.Key = key
                Me.Value = value
                Me.Comparer = Arrays.DefaultComparer
            End Sub

            Public Sub New(ByVal key As T, ByVal value As T1, ByVal comparer As Object)
                Me.Key = key
                Me.Value = value
                Me.Comparer = comparer
            End Sub

            Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
                Dim other As Sort2ArrItem(Of T, T1) = obj
                Return Me.Comparer.Compare(Me.Key, other.Key)
            End Function
        End Class

        Public Sub Sort2Arrays(Of T, T1)(ByVal keyArray() As T, ByVal valueArray() As T1, ByVal fromIndex As Integer, ByVal count As Integer)
            If (count < 0) Then Exit Sub
            Dim tmpArr As Sort2ArrItem(Of T, T1)()
            ReDim tmpArr(count - 1)
            For i As Integer = 0 To count - 1
                tmpArr(i) = New Sort2ArrItem(Of T, T1)(keyArray(fromIndex + i), valueArray(fromIndex + i))
            Next
            Me.Sort(tmpArr)
            For i As Integer = 0 To count - 1
                With tmpArr(i)
                    keyArray(fromIndex + i) = .Key
                    valueArray(fromIndex + i) = .Value
                End With
            Next
            'Arrays.LinSpace(indexes, 1 + UBound(keyArray))
            'Sort(keyArray, indexes, fromIndex, count)
            'Shuffle(valueArray, indexes, fromIndex, count)
        End Sub

        Public Sub Sort2Arrays(Of T, T1)(ByVal keyArray() As T, ByVal valueArray() As T1, ByVal fromIndex As Integer, ByVal count As Integer, ByVal comparer As Object)
            'Dim indexes() As Integer = Nothing
            'Arrays.LinSpace(indexes, 1 + UBound(keyArray))
            'Sort(keyArray, indexes, fromIndex, count, comparer)
            'Shuffle(valueArray, indexes, fromIndex, count)
            If (count < 0) Then Exit Sub
            Dim tmpArr As Sort2ArrItem(Of T, T1)()
            ReDim tmpArr(count - 1)
            For i As Integer = 0 To count - 1
                tmpArr(i) = New Sort2ArrItem(Of T, T1)(keyArray(fromIndex + i), valueArray(fromIndex + i), comparer)
            Next
            Me.Sort(tmpArr)
            For i As Integer = 0 To count - 1
                With tmpArr(i)
                    keyArray(fromIndex + i) = .Key
                    valueArray(fromIndex + i) = .Value
                End With
            Next
        End Sub

        Public Sub Sort(Of T)(ByVal items() As T)
            If (Arrays.Len(items) <= 0) Then Return
            QuickSort(items, 0, Arrays.Len(items), DefaultComparer)
        End Sub

        Public Sub Sort(Of T)(
                ByVal items() As T,
                ByVal fromIndex As Integer,
                ByVal count As Integer
                )
            QuickSort(items, fromIndex, count, DefaultComparer)
        End Sub




        'Public  Sub Sort( _
        '       ByRef items As Array, _
        '       ByVal fromIndex As Integer, _
        '       ByVal count As Integer _
        '       )
        '    QuickSort(items, fromIndex, count, DefaultComparer)
        'End Sub

        Public Sub Sort(Of T)(
                ByRef items() As T,
                ByRef indexes() As Integer,
                ByVal fromIndex As Integer,
                ByVal count As Integer
                )
            QuickSortIndexes(items, indexes, fromIndex, count, DefaultComparer)
        End Sub

        'Public  Sub Sort( _
        '        ByRef items As Array, _
        '        ByRef indexes() As Integer, _
        '        ByVal fromIndex As Integer, _
        '        ByVal count As Integer _
        '        )
        '    QuickSortIndexes(items, indexes, fromIndex, count, DefaultComparer)
        'End Sub

        Public Sub Sort(Of T)(
                    ByRef items() As T,
                    ByVal fromIndex As Integer,
                    ByVal count As Integer,
                    ByVal comparer As Object
                    )
            QuickSort(items, fromIndex, count, comparer)
        End Sub

        'Public  Sub Sort(Of T)( _
        '            ByRef items As Array, _
        '            ByVal fromIndex As Integer, _
        '            ByVal count As Integer, _
        '            ByVal comparer As Object _
        '            )
        '    QuickSort(items, fromIndex, count, comparer)
        'End Sub

        Public Sub Sort(Of T)(
                   ByRef items() As T,
                   ByRef indexes() As Integer,
                   ByVal fromIndex As Integer,
                   ByVal count As Integer,
                   ByVal comparer As Object
                   )
            QuickSortIndexes(items, indexes, fromIndex, count, comparer)
        End Sub

        Public Sub SelectSort(Of T)(
                        ByRef items() As T,
                        ByVal fromIndex As Integer,
                        ByVal count As Integer
                        )
            SelectSort(items, fromIndex, count, DefaultComparer)
        End Sub

        Public Sub SelectSort(Of T)(
                        ByRef items() As T,
                        ByVal fromIndex As Integer,
                        ByVal count As Integer,
                        ByVal comparer As Object
                        )
            Dim i, j As Integer
            'If comparer Is Nothing Then comparer = DefaultComparer
            For i = fromIndex To fromIndex + count - 1
                For j = i + 1 To fromIndex + count - 1
                    If Compare(items(j), items(i), comparer) < 0 Then
                        Call SwapValues(items(i), items(j))
                    End If
                Next
            Next
        End Sub


        Public Sub QuickSort(Of T)(
            ByRef items() As T,
            ByVal Low As Integer,
            ByVal count As Integer
            )
            QuickSort(items, Low, count, DefaultComparer)
        End Sub

        'Public  Sub QuickSort( _
        '   ByRef items As Array, _
        '   ByVal Low As Integer, _
        '   ByVal count As Integer _
        '   )
        '    QuickSort(items, Low, count, DefaultComparer)
        'End Sub

        Public Sub QuickSort(Of T)(
                ByRef items() As T,
                ByVal Low As Integer,
                ByVal count As Integer,
                ByVal comparer As Object
                )
            QuickSort1(items, Low, Low + count - 1, comparer)
        End Sub

        'Public  Sub QuickSort( _
        '        ByRef items As Array, _
        '        ByVal Low As Integer, _
        '        ByVal count As Integer, _
        '        ByVal comparer As Object _
        '        )
        '    QuickSort1(items, Low, Low + count - 1, comparer)
        'End Sub

        Private Sub QuickSort1(Of T)(
                        ByRef arr() As T,
                        ByVal left As Integer,
                        ByVal right As Integer,
                        ByVal comparer As Object
                        )
            'Dim i, j As Integer
            'Dim pivot As T

            'If (right <= left) Then Exit Sub

            'i = left : j = right
            'pivot = arr((left + right) / 2)
            ''partition 
            'While (i <= j)
            '    While (Compare(arr(i), pivot, comparer) < 0)
            '        i = i + 1
            '        If (i > right) Then
            '            j = left
            '            Exit While
            '        End If
            '    End While
            '    While Compare(arr(j), pivot, comparer) > 0
            '        j = j - 1
            '    End While
            '    If (i <= j) Then
            '        SwapValues(arr(i), arr(j))
            '        i = i + 1 : j = j - 1
            '    End If
            'End While
            ''recursion 
            'If (left < j) Then Call QuickSort1(arr, left, j, comparer)
            'If (i < right) Then Call QuickSort1(arr, i, right, comparer)
            System.Array.Sort(arr, left, right - left + 1, comparer)
        End Sub

        'Private  Sub QuickSort1( _
        '               ByRef arr As Array, _
        '               ByVal left As Integer, _
        '               ByVal right As Integer, _
        '               ByVal comparer As Object _
        '               )
        '    Dim i, j As Integer
        '    Dim pivot As Object
        '    i = left : j = right
        '    pivot = arr((left + right) / 2)
        '    'partition 
        '    While (i <= j)
        '        While Compare(arr(i), pivot, comparer) < 0
        '            i = i + 1
        '        End While
        '        While Compare(arr(j), pivot, comparer) > 0
        '            j = j - 1
        '        End While
        '        If (i <= j) Then
        '            SwapValues(arr(i), arr(j))
        '            i = i + 1 : j = j - 1
        '        End If
        '    End While
        '    'recursion 
        '    If (left < j) Then Call QuickSort1(arr, left, j, comparer)
        '    If (i < right) Then Call QuickSort1(arr, i, right, comparer)
        'End Sub

        Private Sub QuickSortIndexes(Of T)(
                    ByRef arr() As T,
                    ByRef indexes() As Integer,
                    ByVal startIndex As Integer,
                    ByVal count As Integer,
                    ByVal comparer As Object
                    )
            QuickSortIndexes1(arr, indexes, startIndex, startIndex + count - 1, comparer)
        End Sub

        'Private  Sub QuickSortIndexes( _
        '            ByRef arr As Array, _
        '            ByRef indexes() As Integer, _
        '            ByVal startIndex As Integer, _
        '            ByVal count As Integer, _
        '            ByVal comparer As Object _
        '            )
        '    QuickSortIndexes1(arr, indexes, startIndex, startIndex + count - 1, comparer)
        'End Sub

        Private Sub QuickSortIndexes1(Of T)(
                    ByRef arr() As T,
                    ByRef indexes() As Integer,
                    ByVal left As Integer,
                    ByVal right As Integer,
                    ByVal comparer As Object
                    )
            Dim i, j As Integer
            Dim pivot As T

            If (right <= left) Then Exit Sub

            i = left : j = right
            pivot = arr((left + right) / 2)
            'partition 
            While (i <= j)
                While Compare(arr(i), pivot, comparer) < 0
                    i = i + 1
                End While
                While Compare(arr(j), pivot, comparer) > 0
                    j = j - 1
                End While
                If (i <= j) Then
                    SwapValues(arr(i), arr(j))
                    SwapValues(indexes(i), indexes(j))
                    i = i + 1 : j = j - 1
                End If
            End While
            'recursion 
            If (left < j) Then Call QuickSortIndexes1(arr, indexes, left, j, comparer)
            If (i < right) Then Call QuickSortIndexes1(arr, indexes, i, right, comparer)
        End Sub

        'Private  Sub QuickSortIndexes1( _
        '            ByRef arr As Array, _
        '            ByRef indexes() As Integer, _
        '            ByVal left As Integer, _
        '            ByVal right As Integer, _
        '            ByVal comparer As Object _
        '            )
        '    Dim i, j As Integer
        '    Dim pivot As Object
        '    i = left : j = right
        '    pivot = arr((left + right) / 2)
        '    'partition 
        '    While (i <= j)
        '        While Compare(arr(i), pivot, comparer) < 0
        '            i = i + 1
        '        End While
        '        While Compare(arr(j), pivot, comparer) > 0
        '            j = j - 1
        '        End While
        '        If (i <= j) Then
        '            SwapValues(arr(i), arr(j))
        '            SwapValues(indexes(i), indexes(j))
        '            i = i + 1 : j = j - 1
        '        End If
        '    End While
        '    'recursion 
        '    If (left < j) Then Call QuickSortIndexes1(arr, indexes, left, j, comparer)
        '    If (i < right) Then Call QuickSortIndexes1(arr, indexes, i, right, comparer)
        'End Sub

        ''' <summary>
        ''' Restituisce una stringa rappresentante gli elementi non vuoti dell'array separati dalla stringa separator
        ''' </summary>
        ''' <param name="items"></param>
        ''' <param name="fromIndex"></param>
        ''' <param name="arrayLen"></param>
        ''' <param name="separator"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function JoinNonEmptyString(
                            ByVal items() As String,
                            ByVal fromIndex As Integer,
                            ByVal arrayLen As Integer,
                            ByVal separator As String
                            ) As String
            Dim i As Integer
            Dim ret As String
            ret = ""
            For i = fromIndex To fromIndex + arrayLen - 1
                If ret <> "" Then ret &= separator
                ret &= items(i).ToString
            Next
            Return ret
        End Function

        Public Function Len(ByVal items As Array) As Integer
            If (items Is Nothing) Then Return 0
            Return items.Length
        End Function

        Public Function Len(Of T)(ByVal items() As T) As Integer
            If (items Is Nothing) Then Return 0
            Return 1 + UBound(items)
        End Function

        Public Function Push(Of T)(ByRef items() As T, ByVal value As T) As T()
            Return Append(items, value)
        End Function

        Function AreEquals(ByVal a As Object, ByVal b As Object) As Boolean
            If (Object.ReferenceEquals(a, b)) Then Return True
            If (IsArray(a)) Then
                If (IsArray(b)) Then
                    Dim arr1 As Array = a
                    Dim arr2 As Array = b
                    If (arr1.Length <> arr2.Length) Then Return False
                    For i As Integer = 0 To arr1.Length - 1
                        If Not AreEquals(arr1.GetValue(i), arr2.GetValue(i)) Then Return False
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return Me.Compare(a, b) = 0
            End If
        End Function

        Public Shadows Function ToString(ByVal item As Array) As String
            Dim ret As New System.Text.StringBuilder
            Dim i As Boolean = False
            For Each o As Object In item
                If (i) Then ret.Append(",")
                If TypeOf (o) Is String Then
                    ret.Append(Chr(34) & o.ToString & Chr(34))
                ElseIf TypeOf (o) Is Object Then
                    ret.Append("{ " & o.ToString & " }")
                ElseIf (o Is Nothing) Then
                    ret.Append("{ NULL }")
                Else
                    ret.Append(o.ToString)
                End If
            Next
            Return ret.ToString
        End Function

        Function hashCode(p1 As Integer()) As Integer
            Throw New NotImplementedException
        End Function



        ''' <summary>
        ''' Restituisce un array delle dimensioni specificate
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="len"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateInstance(Of T)(ByVal len As Integer) As T()
            Return Array.CreateInstance(GetType(T), len)
        End Function

        Public Sub fill(Of T)(ByVal arr As T(), ByVal value As T)
            For i As Integer = 0 To UBound(arr)
                arr(i) = value
            Next
        End Sub

        Public Function Convert(Of T)(ByVal value As Object) As Array
            Dim ret() As T = Nothing
            If (value IsNot Nothing) Then
                If (TypeOf (value) Is Array) Then
                    Dim arr As Array = value
                    ret = CreateInstance(Of T)(arr.Length)
                    For i As Integer = 0 To arr.Length - 1
                        ret(i) = arr.GetValue(i)
                    Next
                Else
                    ReDim ret(0)
                    ret(0) = value
                End If
            Else
                'ReDim ret(0)
                'ret(0) = value
            End If
            Return ret
        End Function

        Public Function Join(Of T)(ByVal arr1 As T(), ByVal arr2 As T()) As T()
            Return Me.Join(arr1, arr2, DefaultComparer)
        End Function

        Public Function Join(Of T)(ByVal arr1 As T(), ByVal arr2 As T(), ByVal comparer As Object) As T()
            Dim ret As T() = Nothing
            Dim len1 As Integer = Arrays.Len(arr1)
            Dim len2 As Integer = Arrays.Len(arr2)
            Dim len As Integer = Math.Min(len1, len2)
            If (len > 0) Then
                ReDim ret(len - 1)
                Dim i, j, k As Integer
                i = 0 : j = 0 : k = 0
                While (i < len1 AndAlso j < len2)
                    Dim c As Integer = DirectCast(comparer, IComparer).Compare(arr1(i), arr2(j))
                    If (c = 0) Then
                        ret(k) = arr1(i)
                        k += 1
                        i += 1
                        j += 1
                    ElseIf (c < 0) Then
                        i += 1
                    Else
                        j += 1
                    End If
                End While

                If (k > 0) Then
                    ReDim Preserve ret(k - 1)
                Else
                    ret = Nothing
                End If
            End If
            Return ret
        End Function

        Function IntegersToBytes(value As Integer()) As Byte()
            If value Is Nothing Then Return Nothing
            Dim ms As New System.IO.MemoryStream
            Dim w As New System.IO.BinaryWriter(ms)
            For i As Integer = 0 To UBound(value)
                w.Write(value(i))
            Next
            Dim ret As Byte() = ms.ToArray
            ms.Dispose()
            Return ret
        End Function

        Function BytesToIntegers(ByVal value As Byte()) As Integer()
            If value Is Nothing Then Return Nothing
            Dim ms As New System.IO.MemoryStream(value)
            Dim r As New System.IO.BinaryReader(ms)
            Dim ret As New System.Collections.ArrayList
            While ms.Position < ms.Length
                ret.Add(r.ReadInt32)
            End While
            ms.Dispose()
            Return ret.ToArray(GetType(Integer))
        End Function

        Public Function InsertSorted(Of T)(ByVal items As T(), ByVal item As T) As T()
            Return Me.InsertSorted(items, item, 0, Len(items), DefaultComparer)
        End Function

        Public Function InsertSorted(Of T)(ByVal items As T(), ByVal item As T, ByVal fromIndex As Integer, ByVal arrLen As Integer, ByVal comparer As Object) As T()
            Dim p As Integer = Me.GetInsertPosition(items, item, fromIndex, arrLen, comparer)
            If (p <= 0) Then
                items = Me.Insert(items, item, 0)
            ElseIf (p >= Me.Len(items)) Then
                items = Me.Push(items, item)
            Else
                items = Me.Insert(items, item, p)
            End If
            Return items
        End Function


        Private Class LineSimilarityComparer
            Implements System.Collections.IComparer

            Public w As LineSimilarity

            Public Sub New(ByVal w As LineSimilarity)
                Me.w = w
            End Sub

            Public Function Compare(ByVal a As LineSimilarity, ByVal b As LineSimilarity) As Integer
                Dim delta1 As LineSimilarity = (a - Me.w)
                Dim delta2 As LineSimilarity = (b - Me.w)
                Dim s1 As Single = delta1.Similitudine
                Dim s2 As Single = delta2.Similitudine
                Return -s1.CompareTo(s2)
            End Function

            Public Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
                Return Me.Compare(CType(a, LineSimilarity), CType(b, LineSimilarity))
            End Function
        End Class



        Public Function FindBySimilarity(ByVal arr As String(), ByVal textToFind As String) As LineSimilarity()
            Dim w1 As New LineSimilarity(textToFind)

            Dim list As New System.Collections.ArrayList(arr.Length)
            For Each s As String In arr
                Dim w As New LineSimilarity(s)
                list.Add(w - w1)
            Next


            'Dim comparer As New LineSimilarityComparer(w1)
            'list.Sort(CType(comparer, System.Collections.IComparer))
            list.Sort()

            Return list.ToArray(GetType(LineSimilarity))
        End Function

        Public Function FindBySimilarity(ByVal arr As String(), ByVal textToFind As String, ByVal textComparer As Object) As LineSimilarity()
            Dim w1 As New LineSimilarity(textToFind, textComparer, False)

            Dim list As New System.Collections.ArrayList(arr.Length)
            For Each s As String In arr
                Dim w As New LineSimilarity(s, textComparer)
                list.Add(w - w1)
            Next

            'Dim comparer As New LineSimilarityComparer(w1)
            'list.Sort(CType(comparer, System.Collections.IComparer))
            list.Sort()

            Return list.ToArray(GetType(LineSimilarity))
        End Function

#Region "IsSorted"

        Public Function IsSorted(Of T)(ByVal arr As T(), ByVal comparer As Object) As Boolean
                Return Me.IsSorted(arr, 0, Len(arr), comparer)
            End Function

            Public Function IsSorted(Of T)(ByVal arr As T(), ByVal fromIndex As Integer, ByVal arrLen As Integer, ByVal comparer As Object) As Boolean
                For i As Integer = fromIndex To fromIndex + arrLen - 2
                    If (Compare(arr(i), arr(i + 1), comparer) > 0) Then
                        Return False
                    End If
                Next
                Return True
            End Function

#End Region

        End Class

End Namespace

Partial Class Sistema
    Public Class WordSimilarity
        Implements IComparable(Of WordSimilarity), ICloneable

        Public word As String
        Public FullMatch As Boolean
        Private comparer As IComparer
        Friend count As Integer

        Public Sub New(ByVal w As String)
            Me.New(w, Strings.DefaultComparerIgnoreCase)
        End Sub

        Public Sub New(ByVal w As String, ByVal comparer As Object)
            Me.New(w, comparer, True)
        End Sub

        Public Sub New(ByVal w As String, ByVal comparer As Object, ByVal fullMath As Boolean)
            Me.comparer = CType(comparer, IComparer)
            Me.word = Strings.UCase(Strings.Trim(w))
            Me.count = 1
            Me.FullMatch = fullMath
        End Sub

        Public Function CompareTo(ByVal other As WordSimilarity) As Integer Implements IComparable(Of WordSimilarity).CompareTo
            Return Me.comparer.Compare(Me.word, other.word) ' String.Compare(Me.word, other.word, True)
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone()
        End Function

        Public Overrides Function ToString() As String
            Return Me.word
        End Function
    End Class

    Public Class LineSimilarity
        Implements IComparable(Of LineSimilarity), IComparable

        Public text As String = ""
        Private arr As WordSimilarity() = New WordSimilarity() {}
        Private comparer As IComparer = Strings.DefaultComparerIgnoreCase
        Public fullMatch As Boolean = True

        Public Sub New()

        End Sub

        Public Sub New(ByVal text As String)
            Me.New(text, Strings.DefaultComparer)
        End Sub

        Public Sub New(ByVal text As String, ByVal comparer As Object)
            Me.New(text, comparer, True)
        End Sub

        Public Sub New(ByVal text As String, ByVal comparer As Object, ByVal fullMatch As Boolean)
            Me.comparer = comparer
            Me.text = text
            Me.fullMatch = fullMatch
            Dim tmp As String() = text.Split(" "c)
            Dim lastW As WordSimilarity = Nothing
            For Each s As String In tmp
                If (Not String.IsNullOrWhiteSpace(s)) Then
                    Dim w As New WordSimilarity(s, comparer)
                    Dim j As Integer = Array.BinarySearch(arr, w)
                    If (j < 0) Then
                        Me.Push(w)
                    Else
                        Me.arr(j).count += 1
                    End If
                    lastW = w
                End If
            Next
            If (lastW IsNot Nothing) Then lastW.FullMatch = fullMatch
        End Sub

        Public Overrides Function ToString() As String
            Return Me.text
        End Function

        Private Sub Push(ByVal w As WordSimilarity)
            Dim tmp1 As WordSimilarity() = CType(Array.CreateInstance(GetType(WordSimilarity), Me.arr.Length + 1), WordSimilarity())
            Dim i As Integer, j As Integer = 0
            For i = 0 To Me.arr.Length - 1   'i++, j++)
                If (arr(i).CompareTo(w) <= 0) Then
                    tmp1(j) = Me.arr(i)
                    j += 1
                Else
                    Exit For
                End If
            Next
            tmp1(j) = w : j += 1
            While i < Me.arr.Length
                tmp1(j) = Me.arr(i) : i += 1 : j += 1
            End While
            Me.arr = tmp1
        End Sub

        Public ReadOnly Property Similitudine As Single
            Get
                Dim cnt As Integer = 0
                Dim len As Integer = Me.arr.Length
                If (len = 0) Then Return 1
                For Each w As WordSimilarity In Me.arr
                    If (w.count > 0) Then
                        cnt += 1
                    ElseIf (w.count < 0) Then
                        Return 0
                    End If
                Next
                Return 1 - CSng(cnt) / len
            End Get
        End Property

        Public Function CompareTo(ByVal other As LineSimilarity) As Integer Implements IComparable(Of LineSimilarity).CompareTo
            Return -Me.Similitudine.CompareTo(other.Similitudine)
        End Function

        Private Function CompareTo(ByVal other As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(other, LineSimilarity))
        End Function

        Public Shared Operator -(ByVal a As LineSimilarity, ByVal b As LineSimilarity) As LineSimilarity
            Dim ret As LineSimilarity = New LineSimilarity()
            ret.text = a.text
            ret.comparer = a.comparer
            For Each s As WordSimilarity In a.arr
                Dim w As WordSimilarity = CType(s.Clone(), WordSimilarity)
                ret.Push(w)
            Next
            For Each s As WordSimilarity In b.arr
                Dim w As WordSimilarity = CType(s.Clone(), WordSimilarity)
                If (w.FullMatch) Then
                    Dim j As Integer = Array.BinarySearch(ret.arr, w)
                    If (j >= 0) Then
                        ret.arr(j).count -= w.count
                    Else
                        w.count = -w.count
                        ret.Push(w)
                    End If
                Else
                    Dim found As Boolean = False
                    For Each w1 As WordSimilarity In ret.arr
                        If (w1.word.StartsWith(w.word, StringComparison.InvariantCultureIgnoreCase)) Then
                            If (w1.count > 0) Then
                                w1.count = 0
                                found = True
                            End If
                        End If
                    Next
                    If (Not found) Then
                        w.count = -1
                        ret.Push(w)
                    End If
                End If

            Next
            Return ret
        End Operator

    End Class


    Private Shared m_Arrays As CArraysClass = Nothing


    Public Shared ReadOnly Property Arrays As CArraysClass
        Get
            If (m_Arrays Is Nothing) Then m_Arrays = New CArraysClass
            Return m_Arrays
        End Get
    End Property

    


End Class