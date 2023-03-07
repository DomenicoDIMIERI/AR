Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD

    Public Class CPratichePerCollabCollection
        Inherits CPraticheCollection

        Private m_Collaboratore As CCollaboratore

        Public Sub New()
        End Sub

        Public ReadOnly Property Collaboratore As CCollaboratore
            Get
                Return Me.m_Collaboratore
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            'If (Me.m_Collaboratore IsNot Nothing) Then DirectCast(value, CRapportino).colla.Info.Produttore = Me.m_Collaboratore
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Initialize(ByVal value As CCollaboratore) As Boolean
            Dim cursor As New CRapportiniCursor
            MyBase.Clear()
            Me.m_Collaboratore = value
            'cursor.IDProduttore.Value = GetID(value)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.CreatoIl.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return True
        End Function

    End Class

End Class