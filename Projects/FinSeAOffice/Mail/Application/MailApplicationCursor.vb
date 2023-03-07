Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD

Partial Class Office

    <Serializable>
    Public Class MailApplicationCursor
        Inherits DBObjectCursorPO(Of MailApplication)

        Private m_UserID As New CCursorField(Of Integer)("UserID")
        Private m_UserName As New CCursorFieldObj(Of String)("UserName")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_RootID As New CCursorField(Of Integer)("RootID")

        Public Sub New()
        End Sub

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property UserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property RootID As CCursorField(Of Integer)
            Get
                Return Me.m_RootID
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.Mails.Applications.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_MailApps"
        End Function





    End Class

End Class