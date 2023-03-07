Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema


    Public Class CalendarScheduleCursor
        Inherits DBObjectCursor(Of CalendarSchedule)

        Private m_ScheduleType As New CCursorField(Of ScheduleType)("ScheduleType")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Intervallo As New CCursorField(Of Single)("Itnervallo")
        Private m_Ripetizioni As New CCursorField(Of Integer)("Ripetizioni")
        Private m_OwnerType As New CCursorFieldObj(Of String)("OwnerType")
        Private m_OwnerID As New CCursorField(Of Integer)("OwnerID")

        Public Sub New()
        End Sub

        Public ReadOnly Property ScheduleType As CCursorField(Of ScheduleType)
            Get
                Return Me.m_ScheduleType
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

        Public ReadOnly Property Intervallo As CCursorField(Of Single)
            Get
                Return Me.m_Intervallo
            End Get
        End Property

        Public ReadOnly Property Ripetizioni As CCursorField(Of Integer)
            Get
                Return Me.m_Ripetizioni
            End Get
        End Property

        Public ReadOnly Property OwnerType As CCursorFieldObj(Of String)
            Get
                Return Me.m_OwnerType
            End Get
        End Property

        Public ReadOnly Property OwnerID As CCursorField(Of Integer)
            Get
                Return Me.m_OwnerID
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DateUtils.ScheduledTasks.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CalendarSchedules"
        End Function
    End Class



End Class