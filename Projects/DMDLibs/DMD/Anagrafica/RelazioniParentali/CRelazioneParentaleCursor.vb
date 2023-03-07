Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable>
    Public Class CRelazioneParentaleCursor
        Inherits DBObjectCursor(Of CRelazioneParentale)

        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_NomeRelazione As New CCursorFieldObj(Of String)("NomeRelazione")
        Private m_IDPersona1 As New CCursorField(Of Integer)("IDPersona1")
        Private m_NomePersona1 As New CCursorFieldObj(Of String)("NomePersona1")
        Private m_IDPersona2 As New CCursorField(Of Integer)("IDPersona2")
        Private m_NomePersona2 As New CCursorFieldObj(Of String)("NomePersona2")
        Private m_Ordine1 As New CCursorField(Of Integer)("Ordine1")
        Private m_Descrizione1 As New CCursorFieldObj(Of String)("Descrizione1")
        Private m_Ordine2 As New CCursorField(Of Integer)("Ordine2")
        Private m_Descrizione2 As New CCursorFieldObj(Of String)("Descrizione2")

        Public Sub New()
        End Sub

        Public ReadOnly Property Ordine1 As CCursorField(Of Integer)
            Get
                Return Me.m_Ordine1
            End Get
        End Property

        Public ReadOnly Property Descrizione1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione1
            End Get
        End Property

        Public ReadOnly Property Ordine2 As CCursorField(Of Integer)
            Get
                Return Me.m_Ordine2
            End Get
        End Property

        Public ReadOnly Property Descrizione2 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione2
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property NomeRelazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRelazione
            End Get
        End Property

        Public ReadOnly Property IDPersona1 As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona1
            End Get
        End Property

        Public ReadOnly Property NomePersona1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona1
            End Get
        End Property

        Public ReadOnly Property IDPersona2 As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona2
            End Get
        End Property

        Public ReadOnly Property NomePersona2 As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona2
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_PersoneRelazioni"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.RelazioniParentali.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

    End Class


End Class