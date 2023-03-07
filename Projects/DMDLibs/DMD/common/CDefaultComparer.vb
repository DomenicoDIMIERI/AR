
Public Class CDefaultComparer
    Implements IComparer

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    ''' <summary>
    ''' Restituisce vero se il valore passato come argomento è un oggetto Nothing, un valore DBNull.Value oppure un NULLABLE senza valore
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function IsNull(ByVal value As Object) As Boolean
        If (TypeOf (value) Is DBNull) Then
            Return True
        ElseIf (value Is Nothing) Then
            Return True
        ElseIf IsNullableType(value.GetType) Then
            Return value.HasValue = False
        Else
            Return False
        End If
    End Function

    Protected Function IsNullableType(ByVal myType As System.Type) As Boolean
        Return (myType.IsGenericType) AndAlso (myType.GetGenericTypeDefinition() Is GetType(Nullable(Of )))
    End Function


    Public Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
        If (Me.IsNull(a)) Then
            If (Me.IsNull(b)) Then
                Return 0
            Else
                Return -1
            End If
        Else
            If (Me.IsNull(b)) Then
                Return 1
            Else
                If (TypeOf (a) Is IComparable) AndAlso (a.GetType Is b.GetType) Then
                    Return DirectCast(a, IComparable).CompareTo(b)
                    'ElseIf (TypeOf (a) Is ValueType) Then
                    '    If (a < b) Then Return -1
                    '    If (a > b) Then Return 1
                    '    'If (a Is b) Then Return 0
                    '    Return 0 '1
                ElseIf (TypeOf (a) Is String) Then
                    Return Strings.StrComp(CStr(a), CStr(b), CompareMethod.Binary)
                Else
                    If (a Is b) Then Return 0
                    Return 1
                End If
            End If
        End If
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
