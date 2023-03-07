Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms

    Public Class OfferteModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCQSPDOfferteCursor
        End Function

        Public Function getOfferteByPersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim offerte As CCollection(Of COffertaCQS) = DMD.CQSPD.Offerte.GetOfferteByPersona(pid)
            If (offerte.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(offerte.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function getOfferteByPratica(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim offerte As CCollection(Of COffertaCQS) = DMD.CQSPD.Offerte.GetOfferteByPratica(pid)
            If (offerte.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(offerte.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function Calc(ByVal renderer As Object) As String
            Dim params As String = RPC.n2str(Me.GetParameter(renderer, "params", ""))
            Dim offerta As COffertaCQS = XML.Utils.Serializer.Deserialize(params, "COffertaCQS")
            offerta.Calcola()
            Return XML.Utils.Serializer.Serialize(offerta, XMLSerializeMethod.Document)
        End Function


    End Class


End Namespace