Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML
Imports DIALTPLib
Imports DMD.CallManagers
Imports DMD.CallManagers.Events

Public NotInheritable Class ChiamateCursor
    Inherits DBObjectCursorBase(Of Chiamata)

    Private m_ServerIP As New CCursorFieldObj(Of String)("ServerIP")
    Private m_ServerName As New CCursorFieldObj(Of String)("ServerName")
    Private m_Channel As New CCursorFieldObj(Of String)("Channel")
    Private m_SourceNumber As New CCursorFieldObj(Of String)("SourceNumber")
    Private m_TargetNumber As New CCursorFieldObj(Of String)("TargetNumber")
    Private m_StartTime As New CCursorField(Of DateTime)("StartTime")
    Private m_AnswerTime As New CCursorField(Of DateTime)("AnswerTime")
    Private m_EndTime As New CCursorField(Of DateTime)("EndTime")
    Private m_StatoChiamata As New CCursorField(Of StatoChiamata)("StatoChiamata")
    Private m_Direzione As New CCursorField(Of Integer)("Direzione")
    Private m_Flags As New CCursorField(Of Integer)("Flags")
    Private m_IDTelefonata As New CCursorField(Of Integer)("IDTelefonata")

    Public Sub New()

    End Sub

    Public ReadOnly Property IDTelefonata As CCursorField(Of Integer)
        Get
            Return Me.m_IDTelefonata
        End Get
    End Property

    Public ReadOnly Property ServerIP As CCursorFieldObj(Of String)
        Get
            Return Me.m_ServerIP
        End Get
    End Property

    Public ReadOnly Property ServerName As CCursorFieldObj(Of String)
        Get
            Return Me.m_ServerName
        End Get
    End Property

    Public ReadOnly Property Channel As CCursorFieldObj(Of String)
        Get
            Return Me.m_Channel
        End Get
    End Property

    Public ReadOnly Property SourceNumber As CCursorFieldObj(Of String)
        Get
            Return Me.m_SourceNumber
        End Get
    End Property

    Public ReadOnly Property TargetNumber As CCursorFieldObj(Of String)
        Get
            Return Me.m_TargetNumber
        End Get
    End Property

    Public ReadOnly Property StartTime As CCursorField(Of DateTime)
        Get
            Return Me.m_StartTime
        End Get
    End Property

    Public ReadOnly Property AnswerTime As CCursorField(Of DateTime)
        Get
            Return Me.m_AnswerTime
        End Get
    End Property

    Public ReadOnly Property EndTime As CCursorField(Of DateTime)
        Get
            Return Me.m_EndTime
        End Get
    End Property

    Public ReadOnly Property StatoChiamata As CCursorField(Of StatoChiamata)
        Get
            Return Me.m_StatoChiamata
        End Get
    End Property

    Public ReadOnly Property Direzione As CCursorField(Of Integer)
        Get
            Return Me.m_Direzione
        End Get
    End Property

    Public ReadOnly Property Flags As CCursorField(Of Integer)
        Get
            Return Me.m_Flags
        End Get
    End Property

    Public Overrides Function GetTableName() As String
        Return "tbl_Telefonate"
    End Function

    Protected Overrides Function GetModule() As CModule
        Return Nothing
    End Function

    Protected Overrides Function GetConnection() As CDBConnection
        Return Chiamate.Database
    End Function
End Class
