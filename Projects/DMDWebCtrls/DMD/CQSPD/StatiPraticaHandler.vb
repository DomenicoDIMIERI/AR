Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class StatiPraticaHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CStatoPraticaCursor
        End Function


        Public Function GetDefault(ByVal renderer As Object) As String
            If (DMD.CQSPD.StatiPratica.GetDefault Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(DMD.CQSPD.StatiPratica.GetDefault, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function GetStatiAttivi(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.Serialize(DMD.CQSPD.StatiPratica.GetStatiAttivi, XMLSerializeMethod.Document)
        End Function

        Public Function GetItemByCompatibleID(ByVal renderer As Object) As String
            Dim ms As StatoPraticaEnum = RPC.n2int(GetParameter(renderer, "ms", ""))
            Dim ret As CStatoPratica = DMD.CQSPD.StatiPratica.GetItemByCompatibleID(ms)
            If (ret Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function GetStatiPraticaRules(ByVal renderer As Object) As String
            Dim sid As Integer = RPC.n2int(GetParameter(renderer, "sid", ""))
            Dim stato As CStatoPratica = CQSPD.StatiPratica.GetItemById(sid)
            Dim items As New CStatoPratRulesCollection(stato)
            Return XML.Utils.Serializer.Serialize(items)
        End Function

    End Class


End Namespace