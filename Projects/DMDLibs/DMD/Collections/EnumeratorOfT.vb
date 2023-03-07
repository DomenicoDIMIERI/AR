Public Class Enumerator(Of T)
    Implements IEnumerator(Of T)

    Private m_Base As IEnumerator

    Public Sub New(ByVal base As IEnumerator)
        DMD.DMDObject.IncreaseCounter(Me)
        Me.m_Base = base
    End Sub

    Public ReadOnly Property Current As T Implements IEnumerator(Of T).Current
        Get
            Return Me.m_Base.Current
        End Get
    End Property

    Private ReadOnly Property Current1 As Object Implements IEnumerator.Current
        Get
            Return Me.m_Base.Current
        End Get
    End Property

    Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        Return Me.m_Base.MoveNext
    End Function

    Public Sub Reset() Implements IEnumerator.Reset
        Me.m_Base.Reset()
    End Sub


    Public Sub Dispose() Implements IDisposable.Dispose
        If (TypeOf (Me.m_Base) Is IDisposable) Then
            DirectCast(Me.m_Base, IDisposable).Dispose()
        Else
            Me.m_Base.Reset()
        End If
        Me.m_Base = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
