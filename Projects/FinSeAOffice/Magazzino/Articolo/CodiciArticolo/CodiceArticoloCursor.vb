Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class CodiceArticoloCursor
        Inherits DBObjectCursor(Of CodiceArticolo)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Valore As New CCursorFieldObj(Of String)("Valore")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDArticolo As New CCursorField(Of Integer)("IDArticolo")
        Private m_Ordine As New CCursorField(Of Integer)("Ordine")
        
        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Valore As CCursorFieldObj(Of String)
            Get
                Return Me.m_Valore
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

        Public ReadOnly Property Ordine As CCursorField(Of Integer)
            Get
                Return Me.m_Ordine
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCodiciArticolo"
        End Function


    End Class

End Class


