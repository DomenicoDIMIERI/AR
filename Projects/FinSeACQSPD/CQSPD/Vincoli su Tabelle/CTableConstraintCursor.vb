Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD
 
  

    Public MustInherit Class CTableConstraintCursor
        Inherits DBObjectCursor(Of CTableConstraint)

        Private m_Espressione As New CCursorFieldObj(Of String)("Espressione")
        Private m_TipoValore As New CCursorField(Of System.TypeCode)("TipoValore")
        Private m_TipoVincolo As New CCursorField(Of TableContraints)("TipoVincolo")

        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public MustOverride Overrides Function GetTableName() As String

        Public ReadOnly Property Espressione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Espressione
            End Get
        End Property

        Public ReadOnly Property TipoValore As CCursorField(Of System.TypeCode)
            Get
                Return Me.m_TipoValore
            End Get
        End Property

        Public ReadOnly Property TipoVincolo As CCursorField(Of TableContraints)
            Get
                Return Me.m_TipoVincolo
            End Get
        End Property

    End Class
 
End Class
