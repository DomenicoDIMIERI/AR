Module Utils

    Public Function IIF(Of T)(ByVal cond As Boolean, ByVal a As T, ByVal b As T) As T
        If (cond) Then
            Return a
        Else
            Return b
        End If
    End Function

    Public Function NewT(Of T)() As T
        Dim type As System.Type = GetType(T)
        Dim c As System.Reflection.ConstructorInfo = type.GetConstructor({})
        If (c IsNot Nothing) Then
            Return CType(c.Invoke({}), T)
        Else
            Throw New ArgumentNullException
        End If
    End Function


    Friend Class mEnumerator(Of T)
        Implements IEnumerator(Of T)


        Public o As ICollection(Of T)
        Private index As Integer
        Private length As Integer

        Public Sub New(ByVal o As ICollection(Of T))
            Me.o = o
            Me.Reset()
        End Sub

        Public ReadOnly Property Current As T Implements IEnumerator(Of T).Current
            Get
                Return Me.o(Me.index)
            End Get
        End Property

        Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
            Get
                Return Me.Current
            End Get
        End Property

        Public Sub Reset() Implements IEnumerator.Reset
            Me.index = -1
            Me.length = Me.o.Count
        End Sub

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            If (Me.index + 1 < Me.length) Then
                Me.index += 1
                Return True
            Else
                Return False
            End If
        End Function



        ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.o = Nothing

            ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
            ' GC.SuppressFinalize(Me)
        End Sub

    End Class

    Friend Function Clip(r As Integer, min As Integer, max As Integer) As Integer
        If (r > max) Then
            r = max
        ElseIf (r < min) Then
            r = min
        End If
        Return r
    End Function

End Module

