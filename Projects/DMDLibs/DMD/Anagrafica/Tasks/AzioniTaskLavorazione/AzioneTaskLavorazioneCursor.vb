Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public Class AzioneTaskLavorazioneCursor
        Inherits DBObjectCursorPO(Of AzioneTaskLavorazione)

        Private m_ParametriAzione As New CCursorFieldObj(Of String)("ParametriAzione")
        Private m_RisultatoAzione As New CCursorFieldObj(Of String)("RisultatoAzione")
        Private m_IDTaskLavorazione As New CCursorField(Of Integer)("IDTaskLavorazione")
        Private m_DataEsecuzione As New CCursorField(Of Date)("DataEsecuzione")
        Private m_DataFineEsecuzione As New CCursorField(Of Date)("DataFineEsecuzione")
        Private m_IDRegolaApplicata As New CCursorField(Of Integer)("IDRegolaApplicata")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_NomeHandler As New CCursorFieldObj(Of String)("NomeHandler")

        Public Sub New()
        End Sub

        Public ReadOnly Property NomeHandler As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeHandler
            End Get
        End Property

        Public ReadOnly Property ParametriAzione As CCursorFieldObj(Of String)
            Get
                Return Me.m_ParametriAzione
            End Get
        End Property

        Public ReadOnly Property RisultatoAzione As CCursorFieldObj(Of String)
            Get
                Return Me.m_RisultatoAzione
            End Get
        End Property

        Public ReadOnly Property IDTaskLavorazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDTaskLavorazione
            End Get
        End Property

        Public ReadOnly Property DataEsecuzione As CCursorField(Of Date)
            Get
                Return Me.m_DataEsecuzione
            End Get
        End Property

        Public ReadOnly Property DataFineEsecuzione As CCursorField(Of Date)
            Get
                Return Me.m_DataFineEsecuzione
            End Get
        End Property

        Public ReadOnly Property IDRegolaApplicata As CCursorField(Of Integer)
            Get
                Return Me.m_IDRegolaApplicata
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazioneAzioni"
        End Function


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.AzioniTasksLavorazione.Module
        End Function
    End Class


End Class