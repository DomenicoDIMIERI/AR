''' <summary>
''' Rappresenta una data che può assumere un valore NULL.
''' A differenza di Nullable(of Date) per questo oggetto sono definite le operazioni aritmetiche standard
''' </summary>
''' <remarks></remarks>
Public Structure NDate
    Implements IComparable(Of NDate)

    Private m_Value As Date
    Private m_HasValue As Boolean

    Public Sub New(ByVal value As Nullable(Of Date))
        If (value.HasValue) Then
            Me.m_Value = value
            Me.m_HasValue = True
        Else
            Me.m_Value = Nothing
            Me.m_HasValue = False
        End If
    End Sub

    Public Sub New(ByVal year As Integer, ByVal month As Integer, ByVal day As Integer, Optional ByVal hour As Integer = 0, Optional ByVal minute As Integer = 0, Optional ByVal second As Integer = 0, Optional ByVal milli As Integer = 0)
        Me.New(New Date(year, month, day, hour, minute, second, milli))
    End Sub

    Public Function HasValue() As Boolean
        Return Me.m_HasValue
    End Function

    Public Property Value As Date
        Get
            Return Me.m_Value
        End Get
        Set(value As Date)
            Me.m_Value = value
            Me.m_HasValue = True
        End Set
    End Property

    Public Shared Narrowing Operator CType(ByVal value As Date) As NDate
        Return New NDate(value)
    End Operator

    Public Shared Widening Operator CType(ByVal value As NDate) As Date
        Return value.Value
    End Operator

    Public Shared Operator &(ByVal a As NDate, ByVal b As NDate) As NDate
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value & b.Value
        Else
            Return Nothing
        End If
    End Operator


    Public Shared Operator +(ByVal a As NDate, ByVal b As NDate) As NDate
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value + b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <(ByVal a As NDate, ByVal b As NDate) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value < b.Value
        Else
            Return Nothing
        End If
    End Operator


    Public Shared Operator <=(ByVal a As NDate, ByVal b As NDate) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <= b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator <>(ByVal a As NDate, ByVal b As NDate) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value <> b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator =(ByVal a As NDate, ByVal b As NDate) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value = b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >(ByVal a As NDate, ByVal b As NDate) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value > b.Value
        Else
            Return Nothing
        End If
    End Operator

    Public Shared Operator >=(ByVal a As NDate, ByVal b As NDate) As Boolean
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value >= b.Value
        Else
            Return Nothing
        End If
    End Operator



    Public Shared Operator IsFalse(ByVal a As NDate) As Boolean
        Return Not (a.HasValue)
    End Operator

    Public Shared Operator IsTrue(ByVal a As NDate) As Boolean
        Return (a.HasValue)
    End Operator



    'Public Shared Operator |(ByVal a As NDate, ByVal b As NDate) As NDate
    '    If (a.HasValue AndAlso b.HasValue) Then
    '        Return a.Value | b.Value
    '    Else
    '        Return Nothing
    '    End If
    'End Operator

    Public Function CompareTo(other As NDate) As Integer Implements IComparable(Of NDate).CompareTo
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

    Function getTimeInMillis() As Long
        If (Me.m_HasValue) Then
            Return Me.m_Value.Ticks
        Else
            Return 0
        End If
    End Function

    Sub setTimeInMillis(ByVal ticks As Long)
        Me.m_Value = New Date(ticks)
        Me.m_HasValue = True
    End Sub

    Sub setTimeZone(tz As TimeZone)
        Throw New NotImplementedException
    End Sub

    Function [get](p1 As Object) As Object
        Throw New NotImplementedException
    End Function

End Structure
