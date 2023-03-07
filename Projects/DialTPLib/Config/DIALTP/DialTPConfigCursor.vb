Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

<Serializable>
Public NotInheritable Class DialTPConfigCursor
    Inherits DBObjectCursorBase(Of DialTPConfig)

    Private m_IDPostazione As New CCursorFieldObj(Of String)("IDPostazione")
    Private m_IDMacchina As New CCursorFieldObj(Of String)("IDMacchina")
    Private m_IDUtente As New CCursorFieldObj(Of String)("IDUtente")
    Private m_Attiva As New CCursorField(Of Boolean)("Attiva")
    Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
    Private m_DataFine As New CCursorField(Of Date)("DataFine")
    Private m_Flags As New CCursorField(Of Integer)("Flags")
    Private m_UploadServer As New CCursorFieldObj(Of String)("UploadServer")
    Private m_NotifyServer As New CCursorFieldObj(Of String)("NotifyServer")
    Private m_ServerName As New CCursorFieldObj(Of String)("ServerName")
    Private m_UserName As New CCursorFieldObj(Of String)("UserName")
    Private m_RemoveLogAfterNDays As New CCursorField(Of Integer)("RemoveLogAfterNDays")
    Private m_MinVersion As New CCursorFieldObj(Of String)("MinVersion")
    Private m_MaxVersion As New CCursorFieldObj(Of String)("MaxVersion")

    Public Sub New()
    End Sub

    Public ReadOnly Property MinVersion As CCursorFieldObj(Of String)
        Get
            Return Me.m_MinVersion
        End Get
    End Property

    Public ReadOnly Property MaxVersion As CCursorFieldObj(Of String)
        Get
            Return Me.m_MaxVersion
        End Get
    End Property

    Public ReadOnly Property RemoveLogAfterNDays As CCursorField(Of Integer)
        Get
            Return Me.m_RemoveLogAfterNDays
        End Get
    End Property

    Public ReadOnly Property ServerName As CCursorFieldObj(Of String)
        Get
            Return Me.m_ServerName
        End Get
    End Property

    Public ReadOnly Property UserName As CCursorFieldObj(Of String)
        Get
            Return Me.m_UserName
        End Get
    End Property

    Public ReadOnly Property UploadServer As CCursorFieldObj(Of String)
        Get
            Return Me.m_UploadServer
        End Get
    End Property

    Public ReadOnly Property NotifyServer As CCursorFieldObj(Of String)
        Get
            Return Me.m_NotifyServer
        End Get
    End Property

    Public ReadOnly Property IDPostazione As CCursorFieldObj(Of String)
        Get
            Return Me.m_IDPostazione
        End Get
    End Property

    Public ReadOnly Property IDMacchina As CCursorFieldObj(Of String)
        Get
            Return Me.m_IDMacchina
        End Get
    End Property

    Public ReadOnly Property IDUtente As CCursorFieldObj(Of String)
        Get
            Return Me.m_IDUtente
        End Get
    End Property

    Public ReadOnly Property Attiva As CCursorField(Of Boolean)
        Get
            Return Me.m_Attiva
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



    Public ReadOnly Property Flags As CCursorField(Of Integer)
        Get
            Return Me.m_Flags
        End Get
    End Property

    Protected Overrides Function GetConnection() As CDBConnection
        Return DialTPApp.Database
    End Function

    Protected Overrides Function GetModule() As DMD.Sistema.CModule
        Return DialTPApp.Configs.Module
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_DialTPConfig"
    End Function


End Class
