Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public NotInheritable Class CListeRicontattoItemsClass
        Inherits CGeneralClass(Of ListaRicontattoItem)

        Friend Sub New()
            MyBase.New("modListeRicontattoItem", GetType(ListaRicontattoItemCursor), 0)
        End Sub


        Public Function GetRicontattoBySource(ByVal source As Object) As ListaRicontattoItem
            Return GetRicontattoBySource(TypeName(source), GetID(source))
        End Function

        Public Function GetRicontattoBySource(ByVal sourceName As String, ByVal param As String) As ListaRicontattoItem
            Dim cursor As ListaRicontattoItemCursor = Nothing
            Try
                sourceName = Trim(sourceName)
                param = Trim(param)
                If (sourceName = vbNullString AndAlso param = vbNullString) Then Return Nothing

                cursor = New ListaRicontattoItemCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.SourceName.Value = sourceName
                cursor.SourceParam.Value = param
                cursor.ID.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                Return cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

    End Class

End Namespace

 