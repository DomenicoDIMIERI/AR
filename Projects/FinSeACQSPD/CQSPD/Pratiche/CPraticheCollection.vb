Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CPraticheCollection
        Inherits CCollection(Of CRapportino)

        Public Sub New()
        End Sub

        Public Sub New(ByVal tm As CTeamManager)
            Me.New()
            If (tm Is Nothing) Then Throw New ArgumentNullException("tm")
            If (GetID(tm) <> 0) Then
                Dim cursor As New CRapportiniCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDCommerciale.Value = GetID(tm)
                cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If
        End Sub

        Public Function ContaValide() As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CRapportino = Me(i)
                If (Item.StatoAttuale.MacroStato.HasValue) Then
                    Select Case Item.StatoAttuale.MacroStato
                        Case StatoPraticaEnum.STATO_ANNULLATA, _
                             StatoPraticaEnum.STATO_RICHIESTADELIBERA, _
                             StatoPraticaEnum.STATO_DELIBERATA, _
                             StatoPraticaEnum.STATO_LIQUIDATA
                            cnt += 1
                    End Select
                End If
            Next
            Return cnt
        End Function

        Public Function ContaInLavorazione() As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CRapportino = Me(i)
                If (Item.StatoAttuale.MacroStato.HasValue) Then
                    Select Case Item.StatoAttuale.MacroStato
                        Case StatoPraticaEnum.STATO_RICHIESTADELIBERA, _
                             StatoPraticaEnum.STATO_DELIBERATA
                            cnt += 1
                    End Select
                End If
            Next
            Return cnt
        End Function

        Public Function ContaNonPagate() As Integer
            Return -1
        End Function

        ''' <summary>
        ''' Crea una nuova pratica
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add() As CRapportino
            Dim item As New CRapportino
            MyBase.Add(item)
            Return item
        End Function

    End Class



End Class
