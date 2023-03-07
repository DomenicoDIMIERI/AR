Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD


    ''' <summary>
    ''' Insieme di vincoli definiti per una tabella finanziaria
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CVincoliProdottoTabellaFin
        Inherits CCollection(Of CProdTabFinConstraint)

        Private m_Owner As CProdottoXTabellaFin

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As CProdottoXTabellaFin)
            Me.New()
            Me.Initialize(owner)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CProdTabFinConstraint).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Initialize(ByVal owner As CProdottoXTabellaFin) As Boolean
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.Clear()
            Me.m_Owner = owner
            If (GetID(owner) <> 0) Then
                Dim cursor As New CProdTabFinConstraintCursor
                cursor.OwnerID.Value = GetID(owner)
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Return True
        End Function

        Public Function Check(ByVal offerta As COffertaCQS) As Boolean
            For i As Integer = 0 To Me.Count - 1
                Dim c As CProdTabFinConstraint = Me(i)
                If (c.Check(offerta) = False) Then Return False
            Next
            Return True
        End Function

    End Class

End Class