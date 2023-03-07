Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class OggettoCollegatoCursor
        Inherits DBObjectCursor(Of OggettoCollegato)

        Private m_DataCollegamento As New CCursorField(Of Date)("DataCollegamento")
        Private m_DataScollegamento As New CCursorField(Of Date)("DataScollegamento")
        Private m_IDOggetto1 As New CCursorField(Of Integer)("IDOggetto1")
        Private m_IDOggetto2 As New CCursorField(Of Integer)("IDOggetto2")
        Private m_Relazione As New CCursorField(Of RelazioniOggettiCollegati)("Relazione")

        Public Sub New()
        End Sub

        Public ReadOnly Property DataCollegamento As CCursorField(Of Date)
            Get
                Return Me.m_DataCollegamento
            End Get
        End Property

        Public ReadOnly Property DataScollegamento As CCursorField(Of Date)
            Get
                Return Me.m_DataScollegamento
            End Get
        End Property

        Public ReadOnly Property IDOggetto1 As CCursorField(Of Integer)
            Get
                Return Me.m_IDOggetto1
            End Get
        End Property

        Public ReadOnly Property IDOggetto2 As CCursorField(Of Integer)
            Get
                Return Me.m_IDOggetto2
            End Get
        End Property

        Public ReadOnly Property Relazione As CCursorField(Of RelazioniOggettiCollegati)
            Get
                Return Me.m_Relazione
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOggettiCollegati"
        End Function


    End Class

End Class


