Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD



    <Serializable>
    Public Class NotaCollaboratoreCursor
        Inherits DBObjectCursor(Of NotaCollaboratore)

        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_IDCollaboratore As New CCursorField(Of Integer)("IDCollaboratore")
        Private m_IDClienteXCollaboratore As New CCursorField(Of Integer)("IDCliXCollab")
        Private m_Data As New CCursorField(Of DateTime)("Data")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Indirizzo As New CCursorFieldObj(Of String)("Indirizzo")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_Esito As New CCursorField(Of Integer)("Esito")
        Private m_Scopo As New CCursorFieldObj(Of String)("Scopo")
        Private m_Nota As New CCursorFieldObj(Of String)("Nota")

        Public Sub New()

        End Sub


        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property


        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property


        Public ReadOnly Property IDClienteXCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDClienteXCollaboratore
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property


        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Indirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo
            End Get
        End Property


        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDNoteCliXCollab"
        End Function




    End Class

End Class