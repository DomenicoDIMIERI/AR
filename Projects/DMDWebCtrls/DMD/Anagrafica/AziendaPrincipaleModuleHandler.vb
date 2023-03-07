Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.XML

Namespace Forms

    Public Class AziendaPrincipaleModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return Nothing
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Anagrafica.Aziende.AziendaPrincipale
        End Function

        Public Function getAziendaPrincipale() As String
            Return XML.Utils.Serializer.Serialize(Anagrafica.Aziende.AziendaPrincipale, XMLSerializeMethod.Document)
        End Function

        Public Overrides Function list(ByVal renderer As Object) As String
            Return Me.InternalEdit(renderer, Anagrafica.Aziende.AziendaPrincipale)
        End Function

    End Class


End Namespace