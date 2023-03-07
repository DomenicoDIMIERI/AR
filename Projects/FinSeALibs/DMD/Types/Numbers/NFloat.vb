''' <summary>
''' Rappresenta un numero intero che può assumere un valore NULL.
''' A differenza di Nullable(of Single) per questo oggetto sono definite le operazioni aritmetiche standard
''' </summary>
''' <remarks></remarks>
Public Structure NFloat
    Implements IComparable(Of NFloat), Number

    Private m_Value As Single
    Private m_HasValue As Boolean

    Public Sub New(ByVal value As Single)
        Me.m_Value = value
        Me.m_HasValue = True
    End Sub

    Public Function HasValue() As Boolean
        Return Me.m_HasValue
    End Function

    Public Property Value As Single
        Get
            Return Me.m_Value
        End Get
        Set(value As Single)
            Me.m_Value = value
            Me.m_HasValue = True
        End Set
    End Property

    Public Shared Widening Operator CType(ByVal value As Single) As NFloat
        Return New NFloat(value)
    End Operator

    Public Shared Narrowing Operator CType(ByVal value As NFloat) As Single
        Return value.Value
    End Operator

    Public Shared Operator -(ByVal value As NFloat) As NFloat
        If (value.HasValue) Then
            Return -value.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator &(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value & b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator *(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value * b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator /(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value / b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator \(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value \ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator ^(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value ^ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator +(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value + b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <(ByVal a As NFloat, ByVal b As NFloat) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value < b.Value
        Else
            Return Nothing
        End If
    End Operator


    Public Shared Operator <=(ByVal a As NFloat, ByVal b As NFloat) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <>(ByVal a As NFloat, ByVal b As NFloat) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <> b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator =(ByVal a As NFloat, ByVal b As NFloat) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value = b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >(ByVal a As NFloat, ByVal b As NFloat) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value > b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >=(ByVal a As NFloat, ByVal b As NFloat) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value >= b.Value
        Else
            Return Nothing
        End If
    End Operator



    Public Shared Operator And(ByVal a As NFloat, ByVal b As NFloat) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value And b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsFalse(ByVal a As NFloat) As Boolean
        If (a.HasValue) Then
            Return a.Value = 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsTrue(ByVal a As NFloat) As Boolean
        If (a.HasValue) Then
            Return a.Value <> 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Like(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Like b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Mod(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Mod b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Not(ByVal a As NFloat) As NFloat
        If (a.HasValue) Then
            Return Not a.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Or(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Or b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Xor(ByVal a As NFloat, ByVal b As NFloat) As NFloat
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Xor b.Value
        Else
            Return Nothing
        End If
    End Operator

    'Public Shared Operator |(ByVal a As NFloat, ByVal b As NFloat) As NFloat
    '    If (a.HasValue AndAlso b.HasValue) Then
    '        Return a.Value | b.Value
    '    Else
    '        Return Nothing
    '    End If
    'End Operator

    Public Function CompareTo(other As NFloat) As Integer Implements IComparable(Of NFloat).CompareTo
        If (Me.m_HasValue AndAlso other.m_HasValue) Then
            If (Me.m_Value < other.m_Value) Then
                Return -1
            ElseIf (Me.m_Value > other.m_Value) Then
                Return 1
            Else
                Return 0
            End If
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
        Return Me.m_Value
    End Function

    Public Function intValue() As Integer Implements Number.intValue
        Return CSng(Me.m_Value)
    End Function

    Public Function isInteger() As Object Implements Number.isInteger
        Return False
    End Function

    Public Function longValue() As Long Implements Number.longValue
        Return CLng(Me.m_Value)
    End Function

    Public Function shortValue() As Short Implements Number.shortValue
        Return CShort(Me.m_Value)
    End Function

#End Region
End Structure
