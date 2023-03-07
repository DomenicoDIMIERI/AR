Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD

   Public Class CollaboratoriFonteProvider
        Implements IFonteProvider

        Public Function GetItemById(ByVal nome As String, id As Integer) As IFonte Implements IFonteProvider.GetItemById
            Return CQSPD.Collaboratori.GetItemById(id)
        End Function

        Public Function GetItemByName(ByVal tipo As String, ByVal value As String) As IFonte Implements IFonteProvider.GetItemByName
            Return CQSPD.Collaboratori.GetItemByName(value)
        End Function


        Public Function GetItemsAsArray(ByVal nome As String, Optional onlyValid As Boolean = True) As IFonte() Implements IFonteProvider.GetItemsAsArray
            Dim ret As New CCollection(Of IFonte)
            Dim cursor As New CCollaboratoriCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OnlyValid = onlyValid
            cursor.NomePersona.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            'ret.Sort()
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret.ToArray
        End Function


        Public Function GetSupportedNames() As String() Implements IFonteProvider.GetSupportedNames
            Return New String() {"Collaboratori"}
        End Function
    End Class

End Class