Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Cursore sulla tabella delle visite
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTemplatesCursor
        Inherits DBObjectCursor(Of CTemplate)


        Private m_Flags As New CCursorField(Of TemplateFlags)("Flags")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Scopo As New CCursorFieldObj(Of String)("Scopo")
        Private m_TipoContatto As New CCursorFieldObj(Of String)("TipoContatto")
        Private m_Testo As New CCursorFieldObj(Of String)("Testo")


        Public Sub New()
        End Sub

        Public ReadOnly Property Flags As CCursorField(Of TemplateFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Scopo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Scopo
            End Get
        End Property

        Public ReadOnly Property TipoContatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContatto
            End Get
        End Property

        Public ReadOnly Property Testo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Testo
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CustomerCalls.Templates.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMTemplates"
        End Function
    End Class


End Class

