''' <summary>
''' Rappresenta un numero intero che può assumere un valore NULL.
''' A differenza di Nullable(of Boolean) per questo oggetto sono definite le operazioni aritmetiche standard
''' </summary>
''' <remarks></remarks>
Public Structure NBoolean
    Implements IComparable(Of NBoolean)

    Public Shared ReadOnly [TRUE] As New NBoolean(True)
    Public Shared ReadOnly [FALSE] As New NBoolean(False)

    Private m_Value As Boolean
    Private m_HasValue As Boolean

    Public Sub New(ByVal value As Boolean)
        Me.m_Value = value
        Me.m_HasValue = True
    End Sub

    Public Function HasValue() As Boolean
        Return Me.m_HasValue
    End Function

    Public Property Value As Boolean
        Get
            Return Me.m_Value
        End Get
        Set(value As Boolean)
            Me.m_Value = value
            Me.m_HasValue = True
        End Set
    End Property

    Public Shared Widening Operator CType(ByVal value As Boolean) As NBoolean
        Return New NBoolean(value)
    End Operator

    Public Shared Narrowing Operator CType(ByVal value As NBoolean) As Boolean
        Return value.Value
    End Operator

    Public Shared Operator -(ByVal value As NBoolean) As NBoolean
        If (value.HasValue) Then
            Return -value.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator &(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value & b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator *(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value * b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator /(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value / b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator \(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value \ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator ^(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value ^ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator +(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value + b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <(ByVal a As NBoolean, ByVal b As NBoolean) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value < b.Value
        Else
            Return Nothing
        End If
    End Operator


    Public Shared Operator <=(ByVal a As NBoolean, ByVal b As NBoolean) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <>(ByVal a As NBoolean, ByVal b As NBoolean) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <> b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator =(ByVal a As NBoolean, ByVal b As NBoolean) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value = b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >(ByVal a As NBoolean, ByVal b As NBoolean) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value > b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >=(ByVal a As NBoolean, ByVal b As NBoolean) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value >= b.Value
        Else
            Return Nothing
        End If
    End Operator



    Public Shared Operator And(ByVal a As NBoolean, ByVal b As NBoolean) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value And b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsFalse(ByVal a As NBoolean) As Boolean
        If (a.HasValue) Then
            Return a.Value = 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsTrue(ByVal a As NBoolean) As Boolean
        If (a.HasValue) Then
            Return a.Value <> 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Like(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Like b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Mod(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Mod b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Not(ByVal a As NBoolean) As NBoolean
        If (a.HasValue) Then
            Return Not a.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Or(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Or b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Xor(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Xor b.Value
        Else
            Return Nothing
        End If
    End Operator

    'Public Shared Operator |(ByVal a As NBoolean, ByVal b As NBoolean) As NBoolean
    '    If (a.HasValue AndAlso b.HasValue) Then
    '        Return a.Value | b.Value
    '    Else
    '        Return Nothing
    '    End If
    'End Operator

    Public Shared Operator <<(ByVal a As NBoolean, ByVal b As Integer) As NBoolean
        If (a.HasValue) Then
            Return a.Value << b
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >>(ByVal a As NBoolean, ByVal b As Integer) As NBoolean
        If (a.HasValue) Then
            Return a.Value >> b
        Else
            Return Nothing
        End If
    End Operator

    Public Function CompareTo(other As NBoolean) As Integer Implements IComparable(Of NBoolean).CompareTo
        If (Me.m_HasValue AndAlso other.m_HasValue) Then
            If (Me.m_Value = False) And (other.m_Value = True) Then
                Return -1
            ElseIf (Me.m_Value = True) And (other.m_Value = False) Then
                Return 1
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

End Structure
