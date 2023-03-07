Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Office
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.XML

Namespace Forms

    Public Class CGDEModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CDocumentiCursor
        End Function


        Public Function GetArrayTipiDocumento(ByVal renderer As Object) As String
            'Dim cursor As New CDocumentiCursor
            Dim ret As CCollection(Of CDocumento) = GDE.LoadAll

            'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            'cursor.Nome.SortOrder = SortEnum.SORT_ASC
            'cursor.IgnoreRights = True
            'While Not cursor.EOF
            '    ret.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Reset()

            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

    End Class





End Namespace