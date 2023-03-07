Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD

Partial Class Office

    Public Class MailFolderCursor
        Inherits DBObjectCursorPO(Of MailFolder)

        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_ParentID As New CCursorField(Of Integer)("ParentID")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDUtente As New CCursorField(Of Integer)("IDUtente")
        Private m_ApplicationID As New CCursorField(Of Integer)("ApplicationID")

        Public Sub New()
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_eMailFolders"
        End Function

        Public ReadOnly Property ApplicationID As CCursorField(Of Integer)
            Get
                Return Me.m_ApplicationID
            End Get
        End Property

        Public ReadOnly Property ParentID As CCursorField(Of Integer)
            Get
                Return Me.m_ParentID
            End Get
        End Property

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property IDutente As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtente
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As Sistema.CModule
            Return Office.Mails.Folders.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function
    End Class

End Class