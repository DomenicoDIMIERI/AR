Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
  
    Public Class CQSPDMotiviScontoPraticaHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CMotivoScontoPraticaCursor
            Return cursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.MotiviSconto.GetItemById(id)
        End Function

        Private Function GetItemByName(ByVal name As String) As String
            Dim ret As CMotivoScontoPratica = CQSPD.MotiviSconto.GetItemByName(name)
            If (ret Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(ret)
            End If
        End Function

        Public Overridable Function GetItemByName(ByVal renderer As Object) As String
            Dim nm As String = RPC.n2str(GetParameter(renderer, "nm", ""))
            Return Me.GetItemByName(nm)
        End Function



    End Class




End Namespace