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

    Public Class PreventiviModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPreventivoCursor
        End Function


        Public Function GetOfferteByPreventivo(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim preventivo As CPreventivo = CQSPD.Preventivi.GetItemById(pid)
            If (preventivo.Offerte.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(preventivo.Offerte.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function Calc(ByVal renderer As Object) As String
            Dim params As String = RPC.n2str(Me.GetParameter(renderer, "params", ""))
            Dim p As CPreventivo = XML.Utils.Serializer.Deserialize(params)
            p.Calcola()
            Return XML.Utils.Serializer.Serialize(p, XMLSerializeMethod.Document)
        End Function


    End Class


End Namespace