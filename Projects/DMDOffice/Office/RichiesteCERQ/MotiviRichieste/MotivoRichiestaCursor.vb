Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei motivi delle richieste
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MotivoRichiestaCursor
        Inherits DBObjectCursor(Of MotivoRichiesta)

        Private m_Motivo As New CCursorFieldObj(Of String)("Motivo")
        Private m_Flags As New CCursorField(Of MotivoRichiestaFlags)("Flags")
        Private m_HandlerName As New CCursorFieldObj(Of String)("NomeHandler")

        Public ReadOnly Property Motivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Motivo
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of MotivoRichiestaFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property HandlerName As CCursorFieldObj(Of String)
            Get
                Return Me.m_HandlerName
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.RichiesteCERQ.MotiviRichieste.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRichiesteM"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class