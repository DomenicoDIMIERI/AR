Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils




Namespace Forms


    Public Class CUfficiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Anagrafica.Uffici.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CUfficiCursor
        End Function

        Public Overrides Function CanCreate(item As Object) As Boolean
            Dim u As CUfficio = item
            If (GetID(u.Azienda) = GetID(Anagrafica.Aziende.AziendaPrincipale)) Then
                Return Sistema.Users.CurrentUser.IsAdministrator
            Else
                Dim m As IModuleHandler = Anagrafica.Aziende.Module.CreateHandler(Nothing)
                Return m.CanEdit(u.Azienda)
            End If
        End Function

        Public Overrides Function CanEdit(item As Object) As Boolean
            Dim u As CUfficio = item
            If (GetID(u.Azienda) = GetID(Anagrafica.Aziende.AziendaPrincipale)) Then
                Return Sistema.Users.CurrentUser.IsAdministrator
            Else
                Dim m As IModuleHandler = Anagrafica.Aziende.Module.CreateHandler(Nothing)
                Return m.CanEdit(u.Azienda)
            End If
        End Function

        Public Overrides Function CanDelete(item As Object) As Boolean
            Dim u As CUfficio = item
            If (GetID(u.Azienda) = GetID(Anagrafica.Aziende.AziendaPrincipale)) Then
                Return Sistema.Users.CurrentUser.IsAdministrator
            Else
                Dim m As IModuleHandler = Anagrafica.Aziende.Module.CreateHandler(Nothing)
                Return m.CanDelete(u.Azienda)
            End If
        End Function

    End Class


End Namespace