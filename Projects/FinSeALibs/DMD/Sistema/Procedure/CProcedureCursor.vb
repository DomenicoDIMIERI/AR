Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class Sistema


    <Serializable>
    Public Class CProcedureCursor
        Inherits DBObjectCursor(Of CProcedura)

        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Flags As New CCursorField(Of ProceduraFlags)("Flags")
        Private m_Priority As New CCursorField(Of PriorityEnum)("Priorita")

        Public Sub New()

        End Sub

        Public ReadOnly Property Priority As CCursorField(Of PriorityEnum)
            Get
                Return Me.m_Priority
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ProceduraFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return Sistema.Procedure.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CalendarProcs"
        End Function

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function
    End Class

End Class

