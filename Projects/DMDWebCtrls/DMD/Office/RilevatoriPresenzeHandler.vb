Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.Office



Namespace Forms


    Public Class RilevatoriPresenzeHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New RilevatoriPresenzeCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.RilevatoriPresenze.GetItemById(id)
        End Function

        Public Function SincronizzaOrario(ByVal renderer As Object) As String
            Dim item As RilevatorePresenze = Me.GetInternalItemById(RPC.n2int(GetParameter(renderer, "id", "")))
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If Not Me.CanConfigure() Then Throw New PermissionDeniedException(Me.Module, "configure")
            item.SincronizzaOrario()
            Return XML.Utils.Serializer.SerializeDate(DateUtils.Now)
        End Function

        Public Function ScaricaNuoveMarcature(ByVal renderer As Object) As String
            Dim item As RilevatorePresenze = Me.GetInternalItemById(RPC.n2int(GetParameter(renderer, "id", "")))
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If Not Me.CanConfigure() Then Throw New PermissionDeniedException(Me.Module, "configure")
            Dim col As CCollection(Of MarcaturaIngressoUscita) = item.ScaricaNuoveMarcature()
            Return XML.Utils.Serializer.Serialize(col)
        End Function

    End Class


End Namespace