Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils


Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class RelazioniParentaliHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRelazioneParentaleCursor
        End Function


        Public Function GetRelazioni(ByVal renderer As Object) As String
            Dim pID As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim items As CCollection(Of CRelazioneParentale) = Anagrafica.RelazioniParentali.GetRelazioni(pID)
            Return XML.Utils.Serializer.Serialize(items)
        End Function

    End Class



End Namespace