Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Internals

    Partial Public Class DateUtilsClass



        Public NotInheritable Class CScheduledTasksClass
            Inherits CGeneralClass(Of CalendarSchedule)

            Friend Sub New()
                MyBase.New("modScheduledTasks", GetType(CalendarScheduleCursor))
            End Sub


        End Class

        Private m_ScheduldTasks As CScheduledTasksClass = Nothing

        Public ReadOnly Property ScheduledTasks As CScheduledTasksClass
            Get
                If (Me.m_ScheduldTasks Is Nothing) Then Me.m_ScheduldTasks = New CScheduledTasksClass
                Return Me.m_ScheduldTasks
            End Get
        End Property

    End Class


End Namespace