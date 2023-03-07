Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class CCodiciPerArticolo
        Inherits CKeyCollection(Of CodiceArticolo)

        Private m_Articolo As Articolo

        Public Sub New()
            Me.m_Articolo = Nothing
        End Sub

        Public Sub New(ByVal articolo As Articolo)
            Me.New()
            Me.Load(articolo)
        End Sub

        Protected Friend Overridable Sub SetArticolo(ByVal articolo As Articolo)
            Me.m_Articolo = articolo
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Articolo IsNot Nothing) Then
                With DirectCast(newValue, CodiceArticolo)
                    .SetArticolo(Me.m_Articolo)
                    .SetOrdine(index)
                End With
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Articolo IsNot Nothing) Then
                With DirectCast(value, CodiceArticolo)
                    .SetArticolo(Me.m_Articolo)
                    .SetOrdine(index)
                End With
            End If
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Overridable Sub Load(ByVal articolo As Articolo)
            If (articolo Is Nothing) Then Throw New ArgumentNullException("articolo")
            Me.Clear()
            Me.m_Articolo = articolo
            If (GetID(articolo) = 0) Then Exit Sub
            Dim cursor As New CodiceArticoloCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDArticolo.Value = GetID(articolo)
            cursor.Ordine.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                Me.Add(cursor.Item.Nome, cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub

    End Class

End Class


