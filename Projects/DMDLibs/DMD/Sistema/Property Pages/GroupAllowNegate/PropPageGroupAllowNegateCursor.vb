Imports DMD
Imports DMD.Sistema


Public partial Class Sistema

    <Serializable>
    Public Class PropPageGroupAllowNegateCursor
        Inherits GroupAllowNegateCursor(Of CRegisteredPropertyPage)

        Public Sub New()
        End Sub


        Protected Overrides Function GetItemFieldName() As String
            Return "IDPropPage"
        End Function

        Protected Overrides Function GetGroupFieldName() As String
            Return "IDGroup"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PropPagesXGruppo"
        End Function

        'Protected Overrides Function GetConnection() As CDBConnection
        '    Return Me.Application.Sistema.PropertyPages.Database
        'End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        'Public Overrides Function InstantiateNewT(dbRis As DBReader) As GroupAllowNegate(Of CRegisteredPropertyPage)
        '    Return New PropPageGroupAllowNegate
        'End Function

        Public Overrides Function InstantiateNew(dbRis As Databases.DBReader) As Object
            Return New PropPageGroupAllowNegate
        End Function

        Public Shadows Property Item As PropPageGroupAllowNegate
            Get
                Return CType(MyBase.Item, PropPageGroupAllowNegate)
            End Get
            Set(value As PropPageGroupAllowNegate)
                MyBase.Item = value
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function
    End Class

End Class

