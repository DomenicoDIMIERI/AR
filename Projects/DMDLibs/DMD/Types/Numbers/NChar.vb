''' <summary>
''' Rappresenta un numero intero che può assumere un valore NULL.
''' A differenza di Nullable(of Char) per questo oggetto sono definite le operazioni aritmetiche standard
''' </summary>
''' <remarks></remarks>
Public Structure NChar
    Implements IComparable(Of NChar)

    Private m_Value As Char
    Private m_HasValue As Boolean

    Public Sub New(ByVal value As Char)
        Me.m_Value = value
        Me.m_HasValue = True
    End Sub

    Public Function HasValue() As Boolean
        Return Me.m_HasValue
    End Function

    Public Property Value As Char
        Get
            Return Me.m_Value
        End Get
        Set(value As Char)
            Me.m_Value = value
            Me.m_HasValue = True
        End Set
    End Property

    Public Shared Widening Operator CType(ByVal value As Char) As NChar
        Return New NChar(value)
    End Operator

    Public Shared Narrowing Operator CType(ByVal value As NChar) As Char
        Return value.Value
    End Operator
 

    Public Shared Operator &(ByVal a As NChar, ByVal b As NChar) As NChar
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value & b.Value
        Else
            Return Nothing
        End If
    End Operator
     
 
 

    Public Shared Operator <(ByVal a As NChar, ByVal b As NChar) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value < b.Value
        Else
            Return Nothing
        End If
    End Operator

 

    Public Shared Operator <=(ByVal a As NChar, ByVal b As NChar) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <>(ByVal a As NChar, ByVal b As NChar) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <> b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator =(ByVal a As NChar, ByVal b As NChar) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value = b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >(ByVal a As NChar, ByVal b As NChar) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value > b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >=(ByVal a As NChar, ByVal b As NChar) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value >= b.Value
        Else
            Return Nothing
        End If
    End Operator

 
  

    
    'Public Shared Operator |(ByVal a As NChar, ByVal b As NChar) As NChar
    '    If (a.HasValue AndAlso b.HasValue) Then
    '        Return a.Value | b.Value
    '    Else
    '        Return Nothing
    '    End If
    'End Operator

    Public Function CompareTo(other As NChar) As Integer Implements IComparable(Of NChar).CompareTo
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

    Public Shared Function GetCharType(ByVal [char] As Char) As Integer
        Throw New NotImplementedException
    End Function

    Public Shared Function GetCharType(ByVal charCode As Integer) As Integer
        Throw New NotImplementedException
    End Function

    Public Const NON_SPACING_MARK = 0
    Public Const MODIFIER_SYMBOL = 1
    Public Const MODIFIER_LETTER = 2

    Shared Function isDigit(p1 As Char) As Boolean
        Throw New NotImplementedException
    End Function

    Shared Function DIRECTIONALITY_RIGHT_TO_LEFT() As Byte
        Throw New NotImplementedException
    End Function

    Shared Function getDirectionality(p1 As Char) As Byte
        Throw New NotImplementedException
    End Function

    Shared Function DIRECTIONALITY_LEFT_TO_RIGHT() As Byte
        Throw New NotImplementedException
    End Function

    Shared Function DIRECTIONALITY_LEFT_TO_RIGHT_EMBEDDING() As Byte
        Throw New NotImplementedException
    End Function

    Shared Function DIRECTIONALITY_RIGHT_TO_LEFT_ARABIC() As Byte
        Throw New NotImplementedException
    End Function

    Shared Function DIRECTIONALITY_RIGHT_TO_LEFT_OVERRIDE() As Byte
        Throw New NotImplementedException
    End Function

    Shared Function DIRECTIONALITY_LEFT_TO_RIGHT_OVERRIDE() As Byte
        Throw New NotImplementedException
    End Function

    Shared Function DIRECTIONALITY_RIGHT_TO_LEFT_EMBEDDING() As Byte
        Throw New NotImplementedException
    End Function

End Structure
