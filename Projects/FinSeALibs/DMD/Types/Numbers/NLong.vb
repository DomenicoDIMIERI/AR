''' <summary>
''' Rappresenta un numero intero che può assumere un valore NULL.
''' A differenza di Nullable(of Long) per questo oggetto sono definite le operazioni aritmetiche standard
''' </summary>
''' <remarks></remarks>
Public Structure NLong
    Implements IComparable(Of NLong), Number

    Private m_Value As Long
    Private m_HasValue As Boolean

    Public Sub New(ByVal value As Long)
        Me.m_Value = value
        Me.m_HasValue = True
    End Sub

    Public Function HasValue() As Boolean
        Return Me.m_HasValue
    End Function

    Public Property Value As Long
        Get
            Return Me.m_Value
        End Get
        Set(value As Long)
            Me.m_Value = value
            Me.m_HasValue = True
        End Set
    End Property

    Public Shared Widening Operator CType(ByVal value As Long) As NLong
        Return New NLong(value)
    End Operator

    Public Shared Narrowing Operator CType(ByVal value As NLong) As Long
        Return value.Value
    End Operator

    Public Shared Operator -(ByVal value As NLong) As NLong
        If (value.HasValue) Then
            Return -value.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator &(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value & b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator *(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value * b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator /(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value / b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator \(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value \ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator ^(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value ^ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator +(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value + b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator -(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value - b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <(ByVal a As NLong, ByVal b As NLong) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value < b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <<(ByVal a As NLong, ByVal b As Integer) As NLong
        If (a.HasValue) Then
            Return a.Value << b
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <=(ByVal a As NLong, ByVal b As NLong) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <>(ByVal a As NLong, ByVal b As NLong) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <> b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator =(ByVal a As NLong, ByVal b As NLong) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value = b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >(ByVal a As NLong, ByVal b As NLong) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value > b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >=(ByVal a As NLong, ByVal b As NLong) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value >= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >>(ByVal a As NLong, ByVal b As Integer) As NLong
        If (a.HasValue) Then
            Return a.Value >> b
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator And(ByVal a As NLong, ByVal b As NLong) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value And b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsFalse(ByVal a As NLong) As Boolean
        If (a.HasValue) Then
            Return a.Value = 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsTrue(ByVal a As NLong) As Boolean
        If (a.HasValue) Then
            Return a.Value <> 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Like(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Like b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Mod(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Mod b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Not(ByVal a As NLong) As NLong
        If (a.HasValue) Then
            Return Not a.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Or(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Or b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Xor(ByVal a As NLong, ByVal b As NLong) As NLong
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Xor b.Value
        Else
            Return Nothing
        End If
    End Operator

    'Public Shared Operator |(ByVal a As NLong, ByVal b As NLong) As NLong
    '    If (a.HasValue AndAlso b.HasValue) Then
    '        Return a.Value | b.Value
    '    Else
    '        Return Nothing
    '    End If
    'End Operator

    Public Function CompareTo(other As NLong) As Integer Implements IComparable(Of NLong).CompareTo
        If (Me.m_HasValue AndAlso other.m_HasValue) Then
            Return Me.m_Value - other.m_Value
        Else
            Return 0
        End If
    End Function


#Region "Number"

    Public Function byteValue() As Byte Implements Number.byteValue
        Return CByte(Me.m_Value)
    End Function

    Public Function doubleValue() As Double Implements Number.doubleValue
        Return CDbl(Me.m_Value)
    End Function

    Public Function floatValue() As Single Implements Number.floatValue
        Return CSng(Me.m_Value)
    End Function

    Public Function intValue() As Integer Implements Number.intValue
        Return Me.m_Value
    End Function

    Public Function isInteger() As Object Implements Number.isInteger
        Return True
    End Function

    Public Function longValue() As Long Implements Number.longValue
        Return CLng(Me.m_Value)
    End Function

    Public Function shortValue() As Short Implements Number.shortValue
        Return CShort(Me.m_Value)
    End Function

#End Region
End Structure
