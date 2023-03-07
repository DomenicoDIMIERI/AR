Imports DMD.Office
Imports DMD.Databases
Imports DMD.Web
Imports DMD.Sistema

Namespace Forms

    Public Class COfficeDefPercorsiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New PercorsiDefinitiCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.PercorsiDefiniti.GetItemById(id)
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            Return ret
        End Function

        Public Function GetPercorsiByPO(ByVal renderer As Object) As String
            Dim idPO As Integer = RPC.n2int(GetParameter(renderer, "po", ""))
            'Dim cursor As New PercorsiDefinitiCursor
            Try
                Dim ret As New CCollection(Of PercorsoDefinito)
                ' cursor.IgnoreRights = True
                'cursor.Nome.SortOrder = SortEnum.SORT_ASC
                'cursor.Attivo.Value = True
                'If (idPO <> 0) Then cursor.IDPuntoOperativo.Value = idPO
                'While Not cursor.EOF
                ' ret.Add(cursor.Item)
                'cursor.MoveNext()
                'End While
                For Each p As PercorsoDefinito In Office.PercorsiDefiniti.LoadAll
                    If (p.Stato = ObjectStatus.OBJECT_VALID AndAlso p.Attivo = True AndAlso (p.IDPuntoOperativo = 0 OrElse p.IDPuntoOperativo = idPO)) Then
                        ret.Add(p)
                    End If
                Next
                ret.Sort()
                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                'If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function
    End Class

End Namespace