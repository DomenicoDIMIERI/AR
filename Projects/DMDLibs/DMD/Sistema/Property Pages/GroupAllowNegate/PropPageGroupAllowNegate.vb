Imports DMD
Imports DMD.Sistema


Public partial Class Sistema

    <Serializable>
    Public Class PropPageGroupAllowNegate
        Inherits GroupAllowNegate(Of CRegisteredPropertyPage)

        Public Sub New()
        End Sub


        Protected Friend Shadows Sub SetItem(ByVal c As CRegisteredPropertyPage)
            MyBase.SetItem(c)
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "IDPropPage"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PropPagesXGruppo"
        End Function



        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            Sistema.PropertyPages.InvalidateGroupAuth()
        End Sub
    End Class

End Class

