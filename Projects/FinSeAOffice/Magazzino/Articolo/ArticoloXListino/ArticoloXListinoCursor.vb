Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class ArticoloXListinoCursor
        Inherits DBObjectCursor(Of ArticoloXListino)

        Private m_TipoPrezzo As New CCursorField(Of TipoPrezzoListino)("TipoPrezzo")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDArticolo As New CCursorField(Of Integer)("IDArticolo")
        Private m_IDListino As New CCursorField(Of Integer)("IDListino")

        Public Sub New()
        End Sub

        Public ReadOnly Property TipoPrezzo As CCursorField(Of TipoPrezzoListino)
            Get
                Return Me.m_TipoPrezzo
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

        Public ReadOnly Property IDListino As CCursorField(Of Integer)
            Get
                Return Me.m_IDListino
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeArticoliListino"
        End Function


    End Class

End Class


