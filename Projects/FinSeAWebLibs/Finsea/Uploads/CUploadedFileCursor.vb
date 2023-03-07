Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    ''' <summary>
    ''' Rappresenta le informazioni registrate nel DB relativamente ad un upload
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CUploadedFileCursor
        Inherits DBObjectCursorBase(Of CUploadedFile)

        Private m_Key As New CCursorFieldObj(Of String)("Key")
        Private m_UserID As New CCursorField(Of Integer)("UserID")
        Private m_SourceFile As New CCursorFieldObj(Of String)("SourceFile")
        Private m_TargetURL As New CCursorFieldObj(Of String)("TargetURL")
        Private m_UploadTime As New CCursorField(Of Date)("UploadTime")
        Private m_FileSize As New CCursorField(Of Integer)("FileSize")
        
        Public Sub New()
        End Sub

        Protected Overrides Function GetModule() As CModule
            Return Uploads.Module
        End Function

        Public ReadOnly Property Key As CCursorFieldObj(Of String)
            Get
                Return Me.m_Key
            End Get
        End Property

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property SourceFile As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceFile
            End Get
        End Property

        Public ReadOnly Property TargetURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_TargetURL
            End Get
        End Property

        Public ReadOnly Property UploadTime As CCursorField(Of Date)
            Get
                Return Me.m_UploadTime
            End Get
        End Property

        Public ReadOnly Property FileSize As CCursorField(Of Integer)
            Get
                Return Me.m_FileSize
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Uploads"
        End Function
         
        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CUploadedFile
        End Function

    End Class


End Class