Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger

    Public Class CMessagesCursor
        Inherits DBObjectCursor(Of CMessage)

        'Private m_Time As New CCursorField(Of Date)("Time")
        Private m_Time As New CCursorFieldDBDate("TimeStr")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        Private m_SourceName As New CCursorFieldObj(Of String)("SourceName")
        Private m_SourceDescription As New CCursorFieldObj(Of String)("SourceDescription")
        Private m_TargetID As New CCursorField(Of Integer)("TargetID")
        Private m_TargetName As New CCursorFieldObj(Of String)("TargetName")
        Private m_Message As New CCursorFieldObj(Of String)("Message")
        Private m_DeliveryTime As New CCursorField(Of Date)("DeliveryTime")
        Private m_ReadTime As New CCursorField(Of Date)("ReadTime")
        Private m_SourceSession As New CCursorField(Of Integer)("SourceSession")
        Private m_TargetSession As New CCursorField(Of Integer)("TargetSession")
        Private m_Stanza As New CCursorFieldObj(Of String)("Stanza")
        Private m_StatoMessaggio As New CCursorField(Of StatoMessaggio)("StatoMessaggio")

        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Messages.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Messenger"
        End Function

        'Public ReadOnly Property Time As CCursorField(Of Date)
        '    Get
        '        Return Me.m_Time
        '    End Get
        'End Property

        Public ReadOnly Property Time As CCursorFieldDBDate
            Get
                Return Me.m_Time
            End Get
        End Property

        Public ReadOnly Property SourceID As CCursorField(Of Integer)
            Get
                Return Me.m_SourceID
            End Get
        End Property

        Public ReadOnly Property SourceName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceName
            End Get
        End Property

        Public ReadOnly Property SourceDescription As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceDescription
            End Get
        End Property

        Public ReadOnly Property TargetID As CCursorField(Of Integer)
            Get
                Return Me.m_TargetID
            End Get
        End Property

        Public ReadOnly Property TargetName As CCursorFieldObj(Of String)
            Get
                Return Me.m_TargetName
            End Get
        End Property

        Public ReadOnly Property Message As CCursorFieldObj(Of String)
            Get
                Return Me.m_Message
            End Get
        End Property

        Public ReadOnly Property DeliveryTime As CCursorField(Of Date)
            Get
                Return Me.m_DeliveryTime
            End Get
        End Property

        Public ReadOnly Property ReadTime As CCursorField(Of Date)
            Get
                Return Me.m_ReadTime
            End Get
        End Property

        Public ReadOnly Property SourceSession As CCursorField(Of Integer)
            Get
                Return Me.m_SourceSession
            End Get
        End Property

        Public ReadOnly Property TargetSession As CCursorField(Of Integer)
            Get
                Return Me.m_TargetSession
            End Get
        End Property

        Public ReadOnly Property Stanza As CCursorFieldObj(Of String)
            Get
                Return Me.m_Stanza
            End Get
        End Property

        Public ReadOnly Property StatoMessaggio As CCursorField(Of StatoMessaggio)
            Get
                Return Me.m_StatoMessaggio
            End Get
        End Property

        Public Overrides Function GetWherePartLimit() As String
            Dim tmpSQL As String
            tmpSQL = ""
            If Not Me.Module.UserCanDoAction("list") Then
                tmpSQL = ""
                If Me.Module.UserCanDoAction("list_own") Then
                    tmpSQL = Strings.Combine(tmpSQL, "([TargetID]=" & GetID(Users.CurrentUser) & ")", " OR ")
                    tmpSQL = Strings.Combine(tmpSQL, "([SourceID]=" & GetID(Users.CurrentUser) & ")", " OR ")
                End If
                If tmpSQL = "" Then tmpSQL = "(0<>0)"
            End If
            Return tmpSQL
        End Function

    End Class

End Class
