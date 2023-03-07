Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms


    Public Class CTabelleSpeseModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTabellaSpeseCursor
        End Function

        Public Function GetTabelleByCessionario(ByVal renderer As Object) As String
            Dim cid As Integer = RPC.n2int(GetParameter(renderer, "cid", ""))
            Dim ov As Boolean = RPC.n2bool(GetParameter(renderer, "ov", ""))
            Dim col As CCollection(Of CTabellaSpese) = CQSPD.TabelleSpese.GetTabelleByCessionario(cid, ov)
            If (col.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(col.ToArray)
            Else
                Return ""
            End If
        End Function
    End Class




End Namespace