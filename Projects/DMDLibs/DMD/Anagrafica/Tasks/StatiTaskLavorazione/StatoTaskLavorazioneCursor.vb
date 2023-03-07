Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public Class StatoTaskLavorazioneCursor
        Inherits DBObjectCursor(Of StatoTaskLavorazione)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDStatoSuccessivoPredefinito As New CCursorField(Of Integer)("IDStatoSuccessivo")
        Private m_MacroStato As New CCursorField(Of MacroStatoLavorazione)("MacroStato")

        Public Sub New()
        End Sub

        Public ReadOnly Property MacroStato As CCursorField(Of MacroStatoLavorazione)
            Get
                Return Me.m_MacroStato
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDStatoSuccessivoPredefinito As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoSuccessivoPredefinito
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazioneStati"
        End Function


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.StatiTasksLavorazione.Module
        End Function
    End Class


End Class