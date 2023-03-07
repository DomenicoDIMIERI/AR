Imports FinSeA.Anagrafica
Imports FinSeA.Databases
Imports FinSeA.Sistema


Partial Class Office

      
    Public Class DistribuzioneMaterialeCursor
        Inherits DBObjectCursorPO(Of DistribuzioneMateriale)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
        Private m_DistanzaStimataMin As New CCursorField(Of Single)("DistanzaStimataMin")
        Private m_DistanzaStimataMax As New CCursorField(Of Single)("DistanzaStimataMax")
        Private m_TempoStimatoMin As New CCursorField(Of Integer)("TempoStimatoMin")
        Private m_TempoStimatoMax As New CCursorField(Of Integer)("TempoStimatoMax")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

        Public ReadOnly Property DistanzaStimataMin As CCursorField(Of Single)
            Get
                Return Me.m_DistanzaStimataMin
            End Get
        End Property

        Public ReadOnly Property DistanzaStimataMax As CCursorField(Of Single)
            Get
                Return Me.m_DistanzaStimataMax
            End Get
        End Property

        Public ReadOnly Property TempoStimatoMin As CCursorField(Of Integer)
            Get
                Return Me.m_TempoStimatoMin
            End Get
        End Property

        Public ReadOnly Property TempoStimatoMax As CCursorField(Of Integer)
            Get
                Return Me.m_TempoStimatoMax
            End Get
        End Property


        Protected Overrides Function GetTableName() As String
            Return "tbl_OfficeDefPercorsi"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.PercorsiDefiniti.Module
        End Function

    End Class


End Class