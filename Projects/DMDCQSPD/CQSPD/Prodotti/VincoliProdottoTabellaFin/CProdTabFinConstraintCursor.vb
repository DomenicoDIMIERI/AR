Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD


    Public Class CProdTabFinConstraintCursor
        Inherits CTableConstraintCursor

        Private m_OwnerID As New CCursorField(Of Integer)("Owner")

        Public Sub New()
        End Sub

        Public ReadOnly Property OwnerID As CCursorField(Of Integer)
            Get
                Return Me.m_OwnerID
            End Get
        End Property

        Public Shadows Property Item As CProdTabFinConstraint
            Get
                Return MyBase.Item
            End Get
            Set(value As CProdTabFinConstraint)
                MyBase.Item = value
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabFinConstr"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProdTabFinConstraint
        End Function

    End Class

End Class