Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms

    Public Class CQSPDRichiesteDerogheHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
        End Sub


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.RichiesteDeroghe.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CQSPD.CRichiestaDerogaCursor
        End Function

        Public Function Invia(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("inviare") Then Throw New PermissionDeniedException(Me.Module, "inviare")
            Dim ric As CRichiestaDeroga = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            ric.Invia()
            Return XML.Utils.Serializer.Serialize(ric)
        End Function

        Public Function Ricevi(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("ricevere") Then Throw New PermissionDeniedException(Me.Module, "ricevere")
            Dim ric As CRichiestaDeroga = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            ric.Ricevi()
            Return XML.Utils.Serializer.Serialize(ric)
        End Function


    End Class




End Namespace