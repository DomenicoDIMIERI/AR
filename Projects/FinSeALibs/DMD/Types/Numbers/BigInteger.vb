''' <summary>
''' Rappresenta un numero intero che può assumere un valore NULL.
''' A differenza di Nullable(of Long) per questo oggetto sono definite le operazioni aritmetiche standard
''' </summary>
''' <remarks></remarks>
Public Structure BigInteger
    Implements IComparable(Of BigInteger), Number

    Private m_Value As Long

    Public Sub New(ByVal value As Long)
        Me.m_Value = value
    End Sub

    Public Property Value As Long
        Get
            Return Me.m_Value
        End Get
        Set(value As Long)
            Me.m_Value = value
        End Set
    End Property

    Public Shared Widening Operator CType(ByVal value As Long) As BigInteger
        Return New BigInteger(value)
    End Operator

    Public Shared Narrowing Operator CType(ByVal value As BigInteger) As Long
        Return value.Value
    End Operator

    Public Shared Operator -(ByVal value As BigInteger) As BigInteger
        Return -value.Value
    End Operator

    Public Shared Operator &(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value & b.Value
    End Operator

    Public Shared Operator *(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value * b.Value
    End Operator

    Public Shared Operator /(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value / b.Value
    End Operator

    Public Shared Operator \(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value \ b.Value
    End Operator

    Public Shared Operator ^(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value ^ b.Value
    End Operator

    Public Shared Operator +(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value + b.Value
    End Operator

    Public Shared Operator -(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value - b.Value
    End Operator

    Public Shared Operator <(ByVal a As BigInteger, ByVal b As BigInteger) As Boolean
        Return a.Value < b.Value
    End Operator

    Public Shared Operator <<(ByVal a As BigInteger, ByVal b As Integer) As BigInteger
        Return a.Value << b
    End Operator

    Public Shared Operator <=(ByVal a As BigInteger, ByVal b As BigInteger) As Boolean
        Return a.Value <= b.Value
    End Operator

    Public Shared Operator <>(ByVal a As BigInteger, ByVal b As BigInteger) As Boolean
        Return a.Value <> b.Value
    End Operator

    Public Shared Operator =(ByVal a As BigInteger, ByVal b As BigInteger) As Boolean
        Return a.Value = b.Value
    End Operator

    Public Shared Operator >(ByVal a As BigInteger, ByVal b As BigInteger) As Boolean
        Return a.Value > b.Value
    End Operator

    Public Shared Operator >=(ByVal a As BigInteger, ByVal b As BigInteger) As Boolean
        Return a.Value >= b.Value
    End Operator

    Public Shared Operator >>(ByVal a As BigInteger, ByVal b As Integer) As BigInteger
        Return a.Value >> b
    End Operator

    Public Shared Operator And(ByVal a As BigInteger, ByVal b As BigInteger) As Boolean
        Return a.Value And b.Value
    End Operator

    Public Shared Operator IsFalse(ByVal a As BigInteger) As Boolean
        Return a.Value = 0
    End Operator

    Public Shared Operator IsTrue(ByVal a As BigInteger) As Boolean
        Return a.Value <> 0
    End Operator

    Public Shared Operator Like(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value Like b.Value
    End Operator

    Public Shared Operator Mod(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value Mod b.Value
    End Operator

    Public Shared Operator Not(ByVal a As BigInteger) As BigInteger
        Return Not a.Value
    End Operator

    Public Shared Operator Or(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value Or b.Value
    End Operator

    Public Shared Operator Xor(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
        Return a.Value Xor b.Value
    End Operator

    'Public Shared Operator |(ByVal a As BigInteger, ByVal b As BigInteger) As BigInteger
    '    If (a.HasValue AndAlso b.HasValue) Then
    '        Return a.Value | b.Value
    '    Else
    '        Return Nothing
    '    End If
    'End Operator

    Public Function CompareTo(other As BigInteger) As Integer Implements IComparable(Of BigInteger).CompareTo
        Return Me.m_Value - other.m_Value
    End Function

    Public Function ToByteArray() As Byte()
        Dim ret(7) As Byte
        Dim tmp As ULong = Convert.ToUInt64(Me.m_Value)
        For i = 0 To 7
            ret(i) = tmp And &HFF
            tmp = tmp >> 8
        Next
        Return ret
    End Function


#Region "Number"

    Private Function byteValue() As Byte Implements Number.byteValue
        Return CByte(Me.m_Value)
    End Function

    Private Function doubleValue() As Double Implements Number.doubleValue
        Return CDbl(Me.m_Value)
    End Function

    Private Function floatValue() As Single Implements Number.floatValue
        Return Me.m_Value
    End Function

    Private Function intValue() As Integer Implements Number.intValue
        Return CSng(Me.m_Value)
    End Function

    Private Function isInteger() As Object Implements Number.isInteger
        Return True
    End Function

    Private Function longValue() As Long Implements Number.longValue
        Return CLng(Me.m_Value)
    End Function

    Private Function shortValue() As Short Implements Number.shortValue
        Return CShort(Me.m_Value)
    End Function

#End Region
End Structure
