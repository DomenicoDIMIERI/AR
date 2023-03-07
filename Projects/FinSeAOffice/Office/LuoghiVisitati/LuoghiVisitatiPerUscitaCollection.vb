Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    <Serializable> _
    Public Class LuoghiVisitatiPerUscitaCollection
        Inherits CCollection(Of LuogoVisitato)

        Private m_Uscita As Uscita

        Public Sub New()
            Me.m_Uscita = Nothing
        End Sub

        Public Sub New(ByVal uscita As Uscita)
            If (uscita Is Nothing) Then Throw New ArgumentNullException("uscita")
            Me.Load(uscita)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Uscita IsNot Nothing) Then DirectCast(value, LuogoVisitato).SetUscita(Me.m_Uscita)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Uscita IsNot Nothing) Then DirectCast(newValue, LuogoVisitato).SetUscita(Me.m_Uscita)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Load(ByVal uscita As Uscita)
            If (uscita Is Nothing) Then Throw New ArgumentNullException("uscita")
            Me.Clear()
            Me.m_Uscita = uscita
            If (GetID(uscita) = 0) Then Exit Sub
            Dim cursor As New LuoghiVisitatiCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.OraArrivo.SortOrder = SortEnum.SORT_ASC
                cursor.IDUscita.Value = GetID(uscita)
                cursor.Progressivo.SortOrder = SortEnum.SORT_ASC
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

    End Class



End Class