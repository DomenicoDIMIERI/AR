Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils

Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms

    'Handler del module Utenti
    Public Class CUsersModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            'Return MyBase.GetItemById(id)
            Return Sistema.Users.GetItemById(id)
        End Function


        'Public Function LogIn(ByVal renderer As Object) As String
        '    'If Users.CurrentUser.IsLogged Then Return ""
        '    Dim uName As String = Me.GetParameter(renderer, "u", "")
        '    Dim pWd As String = Me.GetParameter(renderer, "p", "")
        '    Dim user As CUser = Users.LogIn(uName, pWd)
        '    Return XML.Utils.Serializer.Serialize(user)
        'End Function


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CUserCursor
            cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
            Return cursor
        End Function

        Public Overrides Function CanList() As Boolean
            Return MyBase.CanList()
        End Function

        Public Overrides ReadOnly Property SupportsCreate As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsDelete As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsEdit As Boolean
            Get
                Return True
            End Get
        End Property

    End Class


End Namespace