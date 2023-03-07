Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

   
    ''' <summary>
    ''' Cursore sulla tabella dei documenti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDocumentiCursor
        Inherits DBObjectCursor(Of CDocumento)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_TemplatePath As New CCursorFieldObj(Of String)("Template")
        Private m_Uploadable As New CCursorField(Of Boolean)("Uploadable")
        Private m_ValiditaLimitata As New CCursorField(Of Boolean)("ValiditaLimitata")
        Private m_LegatoAlContesto As New CCursorField(Of Boolean)("LegatoAlContesto")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_Categoria As New CCursorFieldObj(Of String)("cat")
        Private m_SottoCategoria As New CCursorFieldObj(Of String)("sotto_cat")

        Public Sub New()
            
        End Sub

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property SottoCategoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_SottoCategoria
            End Get
        End Property



        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property TemplatePath As CCursorFieldObj(Of String)
            Get
                Return Me.m_TemplatePath
            End Get
        End Property

        Public ReadOnly Property Uploadable As CCursorField(Of Boolean)
            Get
                Return Me.m_Uploadable
            End Get
        End Property

        Public ReadOnly Property ValiditaLimitata As CCursorField(Of Boolean)
            Get
                Return Me.m_ValiditaLimitata
            End Get
        End Property

        Public ReadOnly Property LegatoAlContesto As CCursorField(Of Boolean)
            Get
                Return Me.m_LegatoAlContesto
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CDocumento
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Documenti"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return GDE.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class


End Class