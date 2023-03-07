Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office


    Public Class PeriodoLavoratoCursor
        Inherits DBObjectCursorPO(Of PeriodoLavorato)

        Private m_Periodo As New CCursorField(Of Date)("Periodo")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")

        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")

        Private m_IDTurno As New CCursorField(Of Integer)("IDTurno")
        Private m_NomeTurno As New CCursorFieldObj(Of String)("NomeTurno")

        Private m_DeltaIngresso As New CCursorField(Of Double)("DeltaIngresso")
        Private m_DeltaUscita As New CCursorField(Of Double)("DeltaUscita")

        Private m_OreLavorateTurno As New CCursorField(Of Double)("OreLavorateTurno")
        Private m_OreLavorateEffettive As New CCursorField(Of Double)("OreLavorateEffettive")

        Private m_Flags As New CCursorField(Of Integer)("Flags")

        Private m_RetribuzioneCalcolata As New CCursorField(Of Decimal)("RetribuzioneCalcolata")
        Private m_RetribuzioneErogabile As New CCursorField(Of Decimal)("RetribuzioneErogabile")
        Private m_RetribuzioneErogata As New CCursorField(Of Decimal)("RetribuzioneErogata")

        Private m_DataVerifica As New CCursorField(Of Date)("DataVerifica")
        Private m_IDVerificatoDa As New CCursorField(Of Integer)("IDVerificatoDa")
        Private m_NomeVerificatoDa As New CCursorFieldObj(Of String)("NomeVerificatoDa")
        Private m_NoteVerifica As New CCursorFieldObj(Of String)("NoteVerifica")

        Public Sub New()
        End Sub

        Public ReadOnly Property Periodo As CCursorField(Of Date)
            Get
                Return Me.m_Periodo
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

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property IDTurno As CCursorField(Of Integer)
            Get
                Return Me.m_IDTurno
            End Get
        End Property

        Public ReadOnly Property NomeTurno As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeTurno
            End Get
        End Property

        Public ReadOnly Property DeltaIngresso As CCursorField(Of Double)
            Get
                Return Me.m_DeltaIngresso
            End Get
        End Property

        Public ReadOnly Property DeltaUscita As CCursorField(Of Double)
            Get
                Return Me.m_DeltaUscita
            End Get
        End Property

        Public ReadOnly Property OreLavorateTurno As CCursorField(Of Double)
            Get
                Return Me.m_OreLavorateTurno
            End Get
        End Property

        Public ReadOnly Property OreLavorateEffettive As CCursorField(Of Double)
            Get
                Return Me.m_OreLavorateEffettive
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property RetribuzioneCalcolata As CCursorField(Of Decimal)
            Get
                Return Me.m_RetribuzioneCalcolata
            End Get
        End Property

        Public ReadOnly Property RetribuzioneErogabile As CCursorField(Of Decimal)
            Get
                Return Me.m_RetribuzioneErogabile
            End Get
        End Property

        Public ReadOnly Property RetribuzioneErogata As CCursorField(Of Decimal)
            Get
                Return Me.m_RetribuzioneErogata
            End Get
        End Property

        Public ReadOnly Property DataVerifica As CCursorField(Of Date)
            Get
                Return Me.m_DataVerifica
            End Get
        End Property

        Public ReadOnly Property IDVerificatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDVerificatoDa
            End Get
        End Property

        Public ReadOnly Property NomeVerificatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeVerificatoDa
            End Get
        End Property

        Public ReadOnly Property NoteVerifica As CCursorFieldObj(Of String)
            Get
                Return Me.m_NoteVerifica
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficePeriodiLavorati"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.PeriodiLavorati.Module
        End Function

    End Class


End Class