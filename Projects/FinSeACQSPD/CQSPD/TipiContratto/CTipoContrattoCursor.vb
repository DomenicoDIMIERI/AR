Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CTipoContrattoCursor
        Inherits DBObjectCursorBase(Of CTipoContratto)

        Private m_IdTipoContratto As New CCursorFieldObj(Of String)("IdTipoContratto")
        Private m_Descrizione As New CCursorFieldObj(Of String)("descrizione")

        Public Sub New()
        End Sub


        Public ReadOnly Property IdTipoContratto As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDTipoContratto
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return TipiContratto.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "Tipocontratto"
        End Function

    End Class


End Class