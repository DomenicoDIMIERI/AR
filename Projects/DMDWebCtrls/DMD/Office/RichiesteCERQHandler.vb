
Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class RichiesteCERQHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New RichiestaCERQCursor
        End Function


        Public Function Richiedi(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", "0"))
            Dim ric As RichiestaCERQ = Office.RichiesteCERQ.GetItemById(id)
            If Not Me.CanEdit(ric) Then Throw New PermissionDeniedException(Me.Module, "edit")
            ric.Richiedi()
            Return ""
        End Function

        Public Function Ritira(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", "0"))
            Dim ric As RichiestaCERQ = Office.RichiesteCERQ.GetItemById(id)
            If Not Me.CanEdit(ric) Then Throw New PermissionDeniedException(Me.Module, "edit")
            ric.Ritira()
            Return ""
        End Function

        Public Function Annulla(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", "0"))
            Dim ric As RichiestaCERQ = Office.RichiesteCERQ.GetItemById(id)
            If Not Me.CanEdit(ric) Then Throw New PermissionDeniedException(Me.Module, "edit")
            ric.Annulla()
            Return ""
        End Function

        Public Function Rifiuta(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", "0"))
            Dim ric As RichiestaCERQ = Office.RichiesteCERQ.GetItemById(id)
            If Not Me.CanEdit(ric) Then Throw New PermissionDeniedException(Me.Module, "edit")
            ric.Rifiuta()
            Return ""
        End Function

    End Class


End Namespace
