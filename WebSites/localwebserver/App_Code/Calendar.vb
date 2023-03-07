Imports FinSeA

Namespace FinSeA

    Partial Class Calendar

        Public Shared Function Compare(ByVal d1 As Nullable(Of Date), d2 As Nullable(Of Date)) As Integer
            If d1.HasValue Then
                If d2.HasValue Then
                    Return Date.Compare(d1, d2)
                Else
                    Return -1
                End If
            ElseIf d2.HasValue Then
                Return 1
            Else
                Return 0
            End If
        End Function


    End Class

End Namespace