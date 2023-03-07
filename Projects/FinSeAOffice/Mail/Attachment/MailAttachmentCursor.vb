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

    Public Class MailAttachmentCursor
        Inherits DBObjectCursor(Of MailAttachment)

        Private m_MessageID As New CCursorField(Of Integer)("MessageID")
        Private m_FileName As New CCursorFieldObj(Of String)("FileName")
        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_ContentID As New CCursorFieldObj(Of String)("ContentID")
        Private m_ContentType As New CCursorFieldObj(Of String)("ContentType")
        Private m_ContentDisposition As New CCursorFieldObj(Of String)("ContentDisposition")
        Private m_FileSize As New CCursorField(Of Integer)("FileSize")

        Public Sub New()
        End Sub

        Public ReadOnly Property MessageID As CCursorField(Of Integer)
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Public ReadOnly Property FileName As CCursorFieldObj(Of String)
            Get
                Return Me.m_FileName
            End Get
        End Property

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property ContentID As CCursorFieldObj(Of String)
            Get
                Return Me.m_ContentID
            End Get
        End Property

        Public ReadOnly Property ContentType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ContentType
            End Get
        End Property

        Public ReadOnly Property ContentDisposition As CCursorFieldObj(Of String)
            Get
                Return Me.m_ContentDisposition
            End Get
        End Property

        Public ReadOnly Property FileSize As CCursorField(Of Integer)
            Get
                Return Me.m_FileSize
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_eMailAttachments"
        End Function

        Protected Overrides Function GetModule() As Sistema.CModule
            Return Office.Mails.Attachments.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function





    End Class

End Class