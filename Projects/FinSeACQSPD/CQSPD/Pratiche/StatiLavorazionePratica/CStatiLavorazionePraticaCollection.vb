Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Collezione ordinata per data degli stati di lavorazione di una pratica
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CStatiLavorazionePraticaCollection
        Inherits CCollection(Of CStatoLavorazionePratica)

        Private m_Pratica As CRapportino

        Public Sub New()
            Me.m_Pratica = Nothing
        End Sub

        Public Sub New(ByVal pratica As CRapportino)
            Me.New()
            Me.Load(pratica)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Pratica IsNot Nothing) Then DirectCast(value, CStatoLavorazionePratica).SetPratica(Me.m_Pratica)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Pratica IsNot Nothing) Then DirectCast(newValue, CStatoLavorazionePratica).SetPratica(Me.m_Pratica)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Load(ByVal pratica As CRapportino)
            If (pratica Is Nothing) Then Throw New ArgumentException("pratica")
            Me.Clear()
            Me.m_Pratica = pratica
            If (GetID(pratica) = 0) Then Return
            Dim cursor As New CStatiLavorazionePraticaCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Data.SortOrder = SortEnum.SORT_ASC
                cursor.ID.SortOrder = SortEnum.SORT_ASC
                cursor.IDPratica.Value = GetID(pratica)
                cursor.IgnoreRights = True
                cursor.ID.SortPriority = 99
                cursor.Data.SortPriority = 100
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
            Me.Sort()
        End Sub

        Public Function GetItemByMacroStato(ByVal ms As StatoPraticaEnum) As CStatoLavorazionePratica
            For i As Integer = 0 To Me.Count - 1
                Dim c As CStatoLavorazionePratica = Me(i)
                If c.MacroStato = ms Then Return c
            Next
            Return Nothing
        End Function

        Protected Friend Sub SetPratica(ByVal p As CRapportino)
            Me.m_Pratica = p
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CStatoLavorazionePratica = Me(i)
                Item.SetPratica(p)
            Next
        End Sub

    End Class

End Class