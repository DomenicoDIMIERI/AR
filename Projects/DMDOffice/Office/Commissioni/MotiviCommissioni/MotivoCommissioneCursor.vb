Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei motivi delle commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MotivoCommissioneCursor
        Inherits DBObjectCursor(Of MotivoCommissione)

        Private m_Motivo As New CCursorFieldObj(Of String)("Motivo")


        Public ReadOnly Property Motivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Motivo
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return MotiviCommissioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCommissioniM"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class