Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

      
    Public Class StickyNotesCursor
        Inherits DBObjectCursorPO(Of StickyNote)

        Private m_Text As New CCursorFieldObj(Of String)("Text")
        Private m_Flags As New CCursorField(Of StickyNoteFlags)("Flags")

        Public ReadOnly Property Text As CCursorFieldObj(Of String)
            Get
                Return Me.m_Text
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of StickyNoteFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeStickyNotes"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.StickyNotes.Module
        End Function

    End Class


End Class