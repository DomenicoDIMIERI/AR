Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms

    Public Class CQSPDRichiesteApprovazioneHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CQSPD.CRichiestaApprovazioneCursor
        End Function


        Public Function GetRichiesteByOggetto(ByVal renderer As Object) As String
            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", ""))
            Dim otp As String = RPC.n2str(GetParameter(renderer, "otp", ""))
            Dim o As Object = Sistema.Types.CreateInstance(otp)
            DBUtils.SetID(o, oid)
            Dim ret As CCollection(Of CRichiestaApprovazione) = CQSPD.RichiesteApprovazione.GetRichiesteByOggetto(o)
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray)
            Else
                Return ""
            End If
        End Function
    End Class

    


End Namespace