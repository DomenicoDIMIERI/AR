''' <summary>
''' Rappresenta un numero intero che può assumere un valore NULL.
''' A differenza di Nullable(of Integer) per questo oggetto sono definite le operazioni aritmetiche standard
''' </summary>
''' <remarks></remarks>
Public Class NInteger
    Implements IComparable(Of NInteger), Number

    Private m_Value As Integer
    Private m_HasValue As Boolean

    Public Sub New(ByVal value As Nullable(Of Integer))
        If (value.HasValue) Then
            Me.m_Value = value
            Me.m_HasValue = True
        Else
            Me.m_HasValue = False
        End If
    End Sub

    Public Sub New(ByVal value As String)
        value = Trim(value)
        If (value = "") Then
            Me.m_HasValue = False
        Else
            Me.m_Value = Integer.Parse(value)
            Me.m_HasValue = True
        End If
    End Sub

    Public Function HasValue() As Boolean
        Return Me.m_HasValue
    End Function

    Public Property Value As Integer
        Get
            Return Me.m_Value
        End Get
        Set(value As Integer)
            Me.m_Value = value
            Me.m_HasValue = True
        End Set
    End Property

    Public Shared Widening Operator CType(ByVal value As Integer) As NInteger
        Return New NInteger(value)
    End Operator

    Public Shared Widening Operator CType(ByVal value As Nullable(Of Integer)) As NInteger
        Return New NInteger(value)
    End Operator

    Public Shared Narrowing Operator CType(ByVal value As NInteger) As Integer
        Return value.Value
    End Operator

    Public Shared Narrowing Operator CType(ByVal value As NInteger) As Nullable(Of Integer)
        If (value.HasValue) Then
            Return value.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator -(ByVal value As NInteger) As NInteger
        If (value.HasValue) Then
            Return -value.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator &(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value & b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator *(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value * b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator /(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value / b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator \(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value \ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator ^(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value ^ b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator +(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value + b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator -(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value - b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <(ByVal a As NInteger, ByVal b As NInteger) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value < b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <<(ByVal a As NInteger, ByVal b As Integer) As NInteger
        If (a.HasValue) Then
            Return a.Value << b
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <=(ByVal a As NInteger, ByVal b As NInteger) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <>(ByVal a As NInteger, ByVal b As NInteger) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <> b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator =(ByVal a As NInteger, ByVal b As NInteger) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value = b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >(ByVal a As NInteger, ByVal b As NInteger) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value > b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >=(ByVal a As NInteger, ByVal b As NInteger) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value >= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >>(ByVal a As NInteger, ByVal b As Integer) As NInteger
        If (a.HasValue) Then
            Return a.Value >> b
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator And(ByVal a As NInteger, ByVal b As NInteger) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value And b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsFalse(ByVal a As NInteger) As Boolean
        If (a.HasValue) Then
            Return a.Value = 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator IsTrue(ByVal a As NInteger) As Boolean
        If (a.HasValue) Then
            Return a.Value <> 0
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Like(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Like b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Mod(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Mod b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Not(ByVal a As NInteger) As NInteger
        If (a.HasValue) Then
            Return Not a.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Or(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Or b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator Xor(ByVal a As NInteger, ByVal b As NInteger) As NInteger
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value Xor b.Value
        Else
            Return Nothing
        End If
    End Operator

    'Public Shared Operator |(ByVal a As NInteger, ByVal b As NInteger) As NInteger
    '    If (a.HasValue AndAlso b.HasValue) Then
    '        Return a.Value | b.Value
    '    Else
    '        Return Nothing
    '    End If
    'End Operator

    Public Function CompareTo(other As NInteger) As Integer Implements IComparable(Of NInteger).CompareTo
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

    Shared Function valueOf(p1 As Integer) As NInteger
        Return New NInteger(p1)
    End Function

    Shared Function toHexString(ByVal value As Integer) As String
        Return Hex(value)
    End Function

    Shared Function Parse(ByVal value As String, ByVal base As Integer) As Integer
        Select Case base
            Case 16 : Return Val("&H" & value)
            Case 10 : Return Integer.Parse(value)
            Case Else
                Throw New ArgumentOutOfRangeException("base")
        End Select
    End Function

    Shared Function highestOneBit(nTables As Integer) As Integer
        Throw New NotImplementedException
    End Function

End Class
