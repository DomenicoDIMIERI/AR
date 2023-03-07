Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public NotInheritable Class RegoleTaskLavorazionePerStato
        Inherits CCollection(Of RegolaTaskLavorazione)

        Private m_Stato As StatoTaskLavorazione

        Public Sub New()
        End Sub

        Public Sub New(ByVal stato As StatoTaskLavorazione)
            Me.New()
            If (stato Is Nothing) Then Throw New ArgumentNullException("stato")
            Me.Load(stato)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Stato IsNot Nothing) Then DirectCast(value, RegolaTaskLavorazione).SetStatoSorgente(Me.m_Stato)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSetComplete(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Stato IsNot Nothing) Then DirectCast(newValue, RegolaTaskLavorazione).SetStatoSorgente(Me.m_Stato)
            MyBase.OnSetComplete(index, oldValue, newValue)
        End Sub

        Protected Sub Load(ByVal stato As StatoTaskLavorazione)
            Me.Clear()
            Me.m_Stato = stato
            If (GetID(stato) = 0) Then Exit Sub
            'Dim cursor As New RegolaTaskLavorazioneCursor
            'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            'cursor.IgnoreRights = True
            'cursor.IDStatoSorgente.Value = GetID(stato)
            'cursor.Nome.SortOrder = SortEnum.SORT_ASC
            'While Not cursor.EOF
            '    Me.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Dispose()
            SyncLock Anagrafica.RegoleTasksLavorazione
                For Each regola As RegolaTaskLavorazione In Anagrafica.RegoleTasksLavorazione.LoadAll
                    If regola.IDStatoSorgente = GetID(stato) Then
                        Me.Add(regola)
                    End If
                Next
            End SyncLock

            SyncLock Me
                Me.Sort()
            End SyncLock


        End Sub

    End Class



End Class