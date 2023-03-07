Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    

    <Serializable> _
    Public Class CQSPDSoluzioniXStudioDiFattibilita
        Inherits CCollection(Of CQSPDConsulenza)

        Private m_StudioDiFattibilita As CQSPDStudioDiFattibilita

        Public Sub New()
            Me.m_StudioDiFattibilita = Nothing
        End Sub

        Public Sub New(ByVal gruppo As CQSPDStudioDiFattibilita)
            Me.New()
            Me.Load(gruppo)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_StudioDiFattibilita IsNot Nothing) Then DirectCast(value, CQSPDConsulenza).SetStudioDiFattibilita(Me.m_StudioDiFattibilita)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_StudioDiFattibilita IsNot Nothing) Then DirectCast(newValue, CQSPDConsulenza).SetStudioDiFattibilita(Me.m_StudioDiFattibilita)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub SetStudioDiFattibilita(ByVal value As CQSPDStudioDiFattibilita)
            If (value Is Nothing) Then Throw New ArgumentNullException("value")
            Me.m_StudioDiFattibilita = value
            For i As Integer = 0 To Me.Count - 1
                Dim c As CQSPDConsulenza = Me(i)
                c.SetStudioDiFattibilita(value)
            Next
        End Sub

        Private Sub Load(ByVal studio As CQSPDStudioDiFattibilita)
            If (studio Is Nothing) Then Throw New ArgumentNullException("gruppo")
            Me.Clear()
            Me.m_StudioDiFattibilita = studio
            If (GetID(studio) = 0) Then Exit Sub
            Dim cursor As New CQSPDConsulenzaCursor
            Try
                cursor.IDStudioDiFattibilita.Value = GetID(studio)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                'cursor.ID.SortOrder = SortEnum.SORT_DESC
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try

            Me.Sort()
        End Sub
    End Class

End Class
