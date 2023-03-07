Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML


Public NotInheritable Class ProxyProfileCursor
    Inherits DBObjectCursorBase(Of ProxyProfile)

    Private m_Flags As New CCursorField(Of ProxyFlags)("Flags")
    Private m_ProfileName As New CCursorFieldObj(Of String)("Name")

    Public Sub New()
    End Sub

    Public ReadOnly Property Flags As CCursorField(Of ProxyFlags)
        Get
            Return Me.m_Flags
        End Get
    End Property

    Public ReadOnly Property Name As CCursorFieldObj(Of String)
        Get
            Return Me.m_ProfileName
        End Get
    End Property

    Protected Overrides Function GetConnection() As CDBConnection
        Return DialTPApp.Database
    End Function

    Protected Overrides Function GetModule() As DMD.Sistema.CModule
        Return DialTPApp.ProxyProfiles.Module
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_ProxyProfiles"
    End Function


End Class
