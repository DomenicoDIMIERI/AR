Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class ArticoloInMagazzinoCursor
        Inherits DBObjectCursor(Of ArticoloInMagazzino)

        Private m_Quantita As New CCursorField(Of Double)("Quantita")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDArticolo As New CCursorField(Of Integer)("IDArticolo")
        Private m_IDMagazzino As New CCursorField(Of Integer)("IDMagazzino")

        Public Sub New()
        End Sub

        Public ReadOnly Property Quantitia As CCursorField(Of Double)
            Get
                Return Me.m_Quantita
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDArticolo As CCursorField(Of Integer)
            Get
                Return Me.m_IDArticolo
            End Get
        End Property

        Public ReadOnly Property IDMagazzino As CCursorField(Of Integer)
            Get
                Return Me.m_IDMagazzino
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeArticoliMagazzino"
        End Function


    End Class

End Class


