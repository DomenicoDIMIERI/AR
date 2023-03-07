
Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class TicketCategoriesHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTicketCategoryCursor
        End Function


        Public Function GetArraySottocategorie(ByVal renderer As Object) As String
            Dim cat As String = Trim(RPC.n2str(Me.GetParameter(renderer, "cat", "")))
            Dim items As String() = TicketCategories.GetArraySottocategorie(cat)
            If (items.Length > 0) Then
                Return XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

    End Class


End Namespace
