Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public Class RegolaTaskLavorazioneCursor
        Inherits DBObjectCursor(Of RegolaTaskLavorazione)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IDStatoSorgente As New CCursorField(Of Integer)("IDStatoSorgente")
        Private m_IDStatoDestinazione As New CCursorField(Of Integer)("IDStatoDestinazione")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_NomeHandler As New CCursorFieldObj(Of String)("NomeHandler")
        Private m_ContextType As New CCursorFieldObj(Of String)("ContextType")

        Public Sub New()
        End Sub

        Public ReadOnly Property ContextType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ContextType
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property NomeHandler As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeHandler
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IDStatoSorgente As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoSorgente
            End Get
        End Property

        Public ReadOnly Property IDStatoDestinazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoDestinazione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazioneRegole"
        End Function


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.RegoleTasksLavorazione.Module
        End Function
    End Class


End Class