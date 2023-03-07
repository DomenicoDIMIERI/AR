Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

   Public Class CCollaboratoreCollection
        Inherits CCollection(Of CCollaboratore)
        ' Implements IComparer

        Public Sub New()
        End Sub

        Public Sub New(ByVal tm As CTeamManager)
            Me.New()
            If (tm Is Nothing) Then Throw New ArgumentNullException("tm")
            If GetID(tm) <> 0 Then
                Dim cursor As New CCollaboratoriCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ReferenteID.Value = GetID(tm)
                cursor.NomePersona.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
        End Sub

        Public Function ContaValidi() As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To Me.Count - 1
                Dim item As CCollaboratore = Me(i)
                If item.IsValid Then cnt += 1
            Next
            Return cnt
        End Function

        Protected Overrides Function Compare(a As Object, b As Object) As Integer
            Dim n1, n2 As String
            n1 = LCase(DirectCast(a, CCollaboratore).NomePersona)
            n2 = LCase(DirectCast(b, CCollaboratore).NomePersona)
            Return Strings.Compare(n1, n2, CompareMethod.Text)
        End Function

    End Class

End Class