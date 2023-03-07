Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms

    Public Class CQSPDRichiesteConteggiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CQSPD.CRichiestaConteggioCursor
        End Function

        Public Function Segnala(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ric As CRichiestaConteggio = Me.GetInternalItemById(id)
            If Not Me.Module.UserCanDoAction("segnalare") Then Throw New PermissionDeniedException(Me.Module, "segnalare")
            ric.Segnala()
            Return ""
        End Function

        Public Function PrendiInCarico(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ric As CRichiestaConteggio = Me.GetInternalItemById(id)
            If Not Me.Module.UserCanDoAction("prendereincarico") Then Throw New PermissionDeniedException(Me.Module, "prendereincarico")
            ric.PrendiInCarico()
            Return ""
        End Function


    End Class

    


End Namespace