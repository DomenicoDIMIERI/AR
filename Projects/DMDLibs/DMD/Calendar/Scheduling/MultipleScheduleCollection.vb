Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema

    <Serializable>
    Public Class MultipleScheduleCollection
        Inherits CCollection(Of CalendarSchedule)

        <NonSerialized> Private m_Owner As ISchedulable

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As Object)
            Me.New()
            Me.m_Owner = owner
            Me.Reload()
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CalendarSchedule).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then
                DirectCast(newValue, CalendarSchedule).SetOwner(Me.m_Owner)
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub


        Public Sub Reload()
            Dim cursor As CalendarScheduleCursor = Nothing
            Try
                Me.Clear()
                If (GetID(Me.m_Owner) = 0) Then Exit Sub

                cursor = New CalendarScheduleCursor
                cursor.OwnerType.Value = TypeName(Me.m_Owner)
                cursor.OwnerID.Value = GetID(Me.m_Owner)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Public Overloads Function Add(ByVal tipo As ScheduleType, ByVal dataInizio As Date, Optional ByVal intervallo As Single = 1, Optional ByVal ripetizioni As Integer = 0) As CalendarSchedule
            Dim schedule As New CalendarSchedule(tipo, dataInizio, intervallo, ripetizioni)
            schedule.Stato = ObjectStatus.OBJECT_VALID
            Me.Add(schedule)
            Return schedule
        End Function


        Public Function GetNextSchedule() As CalendarSchedule
            Dim ret As Nullable(Of Date) = Nothing
            Dim s As CalendarSchedule = Nothing
            For Each item As CalendarSchedule In Me
                Dim u As Nullable(Of Date) = item.CalcolaProssimaEsecuzione()
                If (u.HasValue) Then
                    If (ret.HasValue) Then
                        If (u.Value < ret.Value) Then
                            ret = u
                            s = item
                        End If
                    Else
                        ret = u
                        s = item
                    End If
                End If
            Next
            Return s
        End Function

        Public Overrides Function ToString() As String
            Dim ret As String = ""
            For Each Item As CalendarSchedule In Me
                If (ret <> "") Then ret &= vbCrLf
                ret &= Item.ToString
            Next
            Return ret
        End Function

        Public Sub SetOwner(ByVal value As Object)
            Me.m_Owner = value
            For Each Item As CalendarSchedule In Me
                Item.SetOwner(value)
            Next
        End Sub

    End Class



End Class