Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Public partial Class Sistema

    <Serializable>
    Public Class PropPageUserAllowNegateCursor
        Inherits UserAllowNegateCursor(Of CRegisteredPropertyPage)

        Public Sub New()
        End Sub


        Protected Overrides Function GetItemFieldName() As String
            Return "IDPropPage"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PropPageXUtente"
        End Function

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New PropPageUserAllowNegate
        End Function

        Public Shadows Property Item As PropPageUserAllowNegate
            Get
                Return CType(MyBase.Item, PropPageUserAllowNegate)
            End Get
            Set(value As PropPageUserAllowNegate)
                MyBase.Item = value
            End Set
        End Property

    End Class

End Class

