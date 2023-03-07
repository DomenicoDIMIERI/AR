Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports Ionic.Zip

Partial Public Class Sistema

    Public Class CBackupCursor
        Inherits DBObjectCursor(Of CBackup)

        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_FileName As New CCursorFieldObj(Of String)("FileName")
        Private m_FileDate As New CCursorField(Of Date)("FileDate")
        Private m_FileSize As New CCursorField(Of Long)("FileSize")
        Private m_LogMessages As New CCursorFieldObj(Of String)("LogMessages")
        Private m_CompressionLevel As New CCursorField(Of CompressionLevels)("CompressionLevel")
        Private m_CompressionMethod As New CCursorField(Of CompressionMethods)("CompressionMethod")

        Public Sub New()
        End Sub

        Public ReadOnly Property CompressionLevel As CCursorField(Of CompressionLevels)
            Get
                Return Me.m_CompressionLevel
            End Get
        End Property

        Public ReadOnly Property CompressionMethod As CCursorField(Of CompressionMethods)
            Get
                Return Me.m_CompressionMethod
            End Get
        End Property


        Public ReadOnly Property LogMessages As CCursorFieldObj(Of String)
            Get
                Return Me.m_LogMessages
            End Get
        End Property

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property FileName As CCursorFieldObj(Of String)
            Get
                Return Me.m_FileName
            End Get
        End Property

        Public ReadOnly Property FileDate As CCursorField(Of Date)
            Get
                Return Me.m_FileDate
            End Get
        End Property

        Public ReadOnly Property FileSize As CCursorField(Of Long)
            Get
                Return Me.m_FileSize
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Backups.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Backups"
        End Function

    End Class


End Class

 