Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Public partial Class Sistema

    <Serializable>
    Public Class PropPageUserAllowNegate
        Inherits UserAllowNegate(Of CRegisteredPropertyPage)

        Public Sub New()
        End Sub


        Protected Friend Shadows Sub SetItem(ByVal c As CRegisteredPropertyPage)
            MyBase.SetItem(c)
        End Sub


        Protected Overrides Function GetItemFieldName() As String
            Return "IDPropPage"
        End Function

        Protected Overrides Function GetUserFieldName() As String
            Return "IDUser"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PropPageXUtente"
        End Function

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            Sistema.PropertyPages.InvalidateUserAuth()
        End Sub
    End Class

End Class

