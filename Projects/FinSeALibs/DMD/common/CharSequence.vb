Public Class CharSequence
    Implements IEnumerable(Of Char)

    Private m_Sb As New System.Text.StringBuilder

    Public Sub New()
    End Sub

    Public Sub New(ByVal sb As System.Text.StringBuilder)
        Me.m_Sb = sb
    End Sub

    Public Sub New(ByVal sb As String)
        Me.m_Sb = New System.Text.StringBuilder(sb)
    End Sub

    Public Shared Widening Operator CType(ByVal value As System.Text.StringBuilder) As CharSequence
        Return New CharSequence(value)
    End Operator

    Function length() As Integer
        Return Me.m_Sb.Length
    End Function

    Function charAt(index As Integer) As Char
        Return Me.m_Sb.Chars(index)
    End Function

    Public Shared Widening Operator CType(ByVal value As String) As CharSequence
        Return New CharSequence(value)
    End Operator

    Public Function GetEnumerator() As IEnumerator(Of Char) Implements IEnumerable(Of Char).GetEnumerator
        Return New CharEnumerator(Me)
    End Function

    Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator
    End Function

    Public Class CharEnumerator
        Implements IEnumerator(Of Char)

        Private m_Sb As CharSequence
        Private m_Index As Integer

        Public Sub New(ByVal c As CharSequence)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Sb = c
            Me.m_Index = -1
        End Sub


        Public ReadOnly Property Current As Char Implements IEnumerator(Of Char).Current
            Get
                If (Me.m_Index < Me.m_Sb.length) Then
                    Return Me.m_Sb.charAt(Me.m_Index)
                Else
                    Return vbNullChar
                End If
            End Get
        End Property

        Private ReadOnly Property _Current As Object Implements IEnumerator.Current
            Get
                Return Me.Current
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            Me.m_Index += 1
            Return Me.m_Index < Me.m_Sb.length
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            Me.m_Index = -1
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.m_Sb = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class
End Class
