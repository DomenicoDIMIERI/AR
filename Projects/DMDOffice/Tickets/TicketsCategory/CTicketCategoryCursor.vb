Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Office

    ''' <summary>
    ''' Cursore sulla tabelle delle segnalazioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTicketCategoryCursor
        Inherits DBObjectCursor(Of CTicketCategory)

        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_Sottocategoria As New CCursorFieldObj(Of String)("Sottocategoria")

        Public Sub New()
        End Sub

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Sottocategoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sottocategoria
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_SupportTicketsCat"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return TicketCategories.Module
        End Function
    End Class


End Class