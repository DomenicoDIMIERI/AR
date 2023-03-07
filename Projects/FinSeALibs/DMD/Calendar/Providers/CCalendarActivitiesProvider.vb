Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Namespace Internals


    ''' <summary>
    ''' Classe base del provider di attività del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCalendarActivitiesProvider
        Inherits BaseCalendarActivitiesProvider

        Public Sub New()
        End Sub

        Protected Overrides Function GetModule() As CModule
            Return Calendar.Module
        End Function

        Public Overrides Function InstantiateNewItem() As Object
            Return New CCalendarActivity
        End Function

        Public Overrides Function GetCreateCommand() As String
            Return "/calendar/activities/?_a=create"
        End Function

        Public Overrides Function GetShortDescription() As String
            Return "Attività"
        End Function


        Public Overrides Function GetSupportedTypes() As Type()
            Return {GetType(CCalendarActivity)}
        End Function

        Public Overrides Function GetActivePersons(ByVal nomeLista As String, fromDate As Date?, toDate As Date?, Optional ufficio As Integer = 0, Optional operatore As Integer = 0) As CCollection(Of CActivePerson)
            Return New CCollection(Of CActivePerson)
        End Function

        Public Overrides Function GetToDoList(user As CUser) As CCollection(Of ICalendarActivity)
            If (user Is Nothing) Then Throw New ArgumentNullException("user")

            Dim ret As New CCollection(Of ICalendarActivity)
            If (GetID(user) = 0) Then Return ret

            Dim cursor As New CCalendarActivityCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDAssegnatoA.Value = GetID(user)
            cursor.StatoAttivita.Value = StatoAttivita.CONCLUSA
            cursor.StatoAttivita.Operator = OP.OP_NE
            cursor.IgnoreRights = True
            cursor.ProviderName.Value = Me.UniqueName
            cursor.ProviderName.IncludeNulls = True
            While Not cursor.EOF
                Dim a As ICalendarActivity = cursor.Item
                a.SetProvider(Me)
                a.Flags = a.Flags Or CalendarActivityFlags.CanDelete Or CalendarActivityFlags.CanEdit
                ret.Add(a)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

        Public Overrides Function GetScadenze(fromDate As Date?, toDate As Date?) As CCollection(Of ICalendarActivity)
            Dim dbSQL, wherePart, tmpSQL As String
            Dim ret As New CCollection(Of ICalendarActivity)
            Dim item As CCalendarActivity
            wherePart = "([Stato]=" & ObjectStatus.OBJECT_VALID & ")"
            If Not Types.IsNull(fromDate) Then wherePart = Strings.Combine(wherePart, "[DataInizio]>=" & DBUtils.DBDate(fromDate) & "", " AND ")
            If Not Types.IsNull(toDate) Then wherePart = Strings.Combine(wherePart, "[DataFine]>=" & DBUtils.DBDate(toDate) & "", " AND ")
            If Not Me.Module.UserCanDoAction("list") Then
                tmpSQL = "(0<>0)"
                If Me.Module.UserCanDoAction("list_own") Then tmpSQL = Strings.Combine(tmpSQL, "([Operatore]=" & GetID(Users.CurrentUser) & ")", " Or ")
                'If m_CanListGroup Then tmpSQL = Strings.Combine(tmpSQL, "([Operatore] In (SELECT [User]=" & Users.CurrentUser.ID & ")", "Or")
                wherePart = Strings.Combine(wherePart, "(" & tmpSQL & ")", " Or ")
            End If

            dbSQL = "SELECT * FROM [tbl_CalendarActivities] WHERE " & wherePart & " ORDER BY [DataInizio] DESC"
            ret.Comparer = Calendar.DefaultComparer
            ret.Sorted = False
            Dim reader As New DBReader(APPConn.Tables("tbl_CalendarActivities"), dbSQL)
            While reader.Read
                item = Me.InstantiateNewItem
                If APPConn.Load(item, reader) Then Call ret.Add(item)
            End While
            reader.Dispose()

            ret.Sorted = True
            Return ret
        End Function

        Public Overrides Function GetPendingActivities() As CCollection(Of ICalendarActivity)
            Dim dbSQL, wherePart, tmpSQL As String
            Dim items As New CCollection(Of ICalendarActivity)
            Dim item As CCalendarActivity
            wherePart = "([Stato]=1) And ([StatoAttivita]<=0)"
            If Not Me.Module.UserCanDoAction("list") Then
                tmpSQL = "(0<>0)"
                If Me.Module.UserCanDoAction("list_own") Then tmpSQL = Strings.Combine(tmpSQL, "([Operatore]=" & Users.CurrentUser.ID & ")", " Or ")
                'If m_CanListGroup Then tmpSQL = Strings.Combine(wherePart, "([Operatore] In (SELECT [User]=" & Users.CurrentUser.ID & ")", " Or ")
                wherePart = Strings.Combine(wherePart, "(" & tmpSQL & ")", " Or ")
            End If
            dbSQL = "SELECT * FROM [tbl_CalendarActivities] WHERE " & wherePart & " ORDER BY [DataInizio] DESC"
            Dim reader As New DBReader(APPConn.Tables("tbl_CalendarActivities"), dbSQL)
            While reader.Read
                item = Me.InstantiateNewItem()
                If APPConn.Load(item, reader) Then items.Add(item)
            End While
            reader.Dispose()
            Return items
        End Function


        Public Overrides ReadOnly Property UniqueName As String
            Get
                Return "DEFCALPROV"
            End Get
        End Property


        Public Overrides Sub DeleteActivity(item As ICalendarActivity, Optional force As Boolean = False)
            Dim c As CCalendarActivity = item
            c.OldDelete(force)
        End Sub

        Public Overrides Sub SaveActivity(item As ICalendarActivity, Optional force As Boolean = False)
            Dim c As CCalendarActivity = item
            c.OldSave(force)
        End Sub
    End Class


End Namespace