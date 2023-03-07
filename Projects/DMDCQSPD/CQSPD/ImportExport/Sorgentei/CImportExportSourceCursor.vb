Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

     

    ''' <summary>
    ''' Rappresenta una sorgente di importazione/esportazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CImportExportSourceCursor
        Inherits DBObjectCursor(Of CImportExportSource)

        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_DisplayName As New CCursorFieldObj(Of String)("DisplayName")
        Private m_Flags As New CCursorField(Of ImportExportSourceFlags)("Flags")
        Private m_Password As New CCursorFieldObj(Of String)("Password")
        Private m_RemoteURL As New CCursorFieldObj(Of String)("RemoteURL")

        Public Sub New()
        End Sub

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property DisplayName As CCursorFieldObj(Of String)
            Get
                Return Me.m_DisplayName
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ImportExportSourceFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Password As CCursorFieldObj(Of String)
            Get
                Return Me.m_Password
            End Get
        End Property

        Public ReadOnly Property RemoteURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_RemoteURL
            End Get
        End Property
          

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.ImportExportSources.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDImportExportS"
        End Function

    End Class





End Class