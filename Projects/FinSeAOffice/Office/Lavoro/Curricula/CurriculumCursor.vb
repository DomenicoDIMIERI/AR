Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella dei curricula
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CurriculumCursor
        Inherits DBObjectCursorPO(Of Curriculum)

        Private m_DataPresentazione As New CCursorField(Of Date)("DataPresentazione")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IDAllegato As New CCursorField(Of Integer)("IDAllegato")

        Public Sub New()
        End Sub

        Public ReadOnly Property DataPresentazione As CCursorField(Of Date)
            Get
                Return Me.m_DataPresentazione
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IDAllegato As CCursorField(Of Integer)
            Get
                Return Me.m_IDAllegato
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCurricula"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.Curricula.Module
        End Function

    End Class


End Class