Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Anagrafica

    Public Class CTipoRapportoCursor
        Inherits DBObjectCursorBase(Of CTipoRapporto)

        Private m_Descrizione As New CCursorFieldObj(Of String)("descrizione")
        Private m_IdTipoRapporto As New CCursorFieldObj(Of String)("IdTipoRapporto")
        Private m_Flags As New CCursorField(Of TipoRapportoFlags)("Flags")

        Public Sub New()
        End Sub

        Public ReadOnly Property Flags As CCursorField(Of TipoRapportoFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IdTipoRapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_IdTipoRapporto
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return TipiRapporto.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "Tiporapporto"
        End Function


    End Class


End Class
