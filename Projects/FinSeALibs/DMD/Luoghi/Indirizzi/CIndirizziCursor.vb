Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public Class CIndirizziCursor
        Inherits DBObjectCursor(Of CIndirizzo)

        Private m_PersonaID As CCursorField(Of Integer) 'ID della persona associata
        Private m_Nome As CCursorFieldObj(Of String) 'Nome dell'indirizzo
        Private m_Citta As CCursorFieldObj(Of String) 'Nome della città
        Private m_Provincia As CCursorFieldObj(Of String) 'Nome della provincia
        Private m_CAP As CCursorFieldObj(Of String) 'Codice di avviamento postale
        Private m_Toponimo As CCursorFieldObj(Of String)
        Private m_Via As CCursorFieldObj(Of String) 'Via o piazza
        Private m_Civico As CCursorFieldObj(Of String) 'Numero civico
        Private m_Note As CCursorFieldObj(Of String) 'Note

        Public Sub New()
            Me.m_PersonaID = New CCursorField(Of Integer)("Persona")
            Me.m_Nome = New CCursorFieldObj(Of String)("Nome")
            Me.m_Citta = New CCursorFieldObj(Of String)("Citta")
            Me.m_Provincia = New CCursorFieldObj(Of String)("Provincia")
            Me.m_CAP = New CCursorFieldObj(Of String)("CAP")
            Me.m_Toponimo = New CCursorFieldObj(Of String)("Toponimo")
            Me.m_Via = New CCursorFieldObj(Of String)("Via")
            Me.m_Civico = New CCursorFieldObj(Of String)("Civico")
            Me.m_Note = New CCursorFieldObj(Of String)("Note")
        End Sub

        Public ReadOnly Property PersonaID As CCursorField(Of Integer)
            Get
                Return Me.m_PersonaID
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_Citta
            End Get
        End Property

        Public ReadOnly Property Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_Provincia
            End Get
        End Property

        Public ReadOnly Property CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_CAP
            End Get
        End Property

        Public ReadOnly Property Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_Via
            End Get
        End Property

        Public ReadOnly Property Civico As CCursorFieldObj(Of String)
            Get
                Return Me.m_Civico
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CIndirizzo
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Indirizzi"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Luoghi.Indirizzi.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class

End Class