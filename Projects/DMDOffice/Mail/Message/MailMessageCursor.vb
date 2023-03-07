Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Net
Imports DMD
Imports DMD.XML

Partial Class Office

    Public Class MailMessageCursor
        Inherits DBObjectCursorPO(Of MailMessage)

        Private m_ApplicationID As New CCursorField(Of Integer)("ApplicationID")
        Private m_FolderID As New CCursorField(Of Integer)("FolderID")
        Private m_AccountID As New CCursorField(Of Integer)("AccountID")
        Private m_Bcc As New CCursorFieldObj(Of String)("Bcc")
        Private m_Cc As New CCursorFieldObj(Of String)("Cc")
        Private m_From As New CCursorFieldObj(Of String)("From")
        Private m_DeliveredTo As New CCursorFieldObj(Of String)("DeliveredTo")
        Private m_ReplyTo As New CCursorFieldObj(Of String)("ReplayTo")
        Private m_Subject As New CCursorFieldObj(Of String)("Subject")
        Private m_To As New CCursorFieldObj(Of String)("To")
        Private m_Body As New CCursorFieldObj(Of String)("Body")
        Private m_IsBodyHtml As New CCursorField(Of Boolean)("BodyHtml")
        Private m_DeliveryDate As New CCursorField(Of Date)("DeliveryDate")
        Private m_DownloadDate As New CCursorField(Of Date)("DownloadDate")
        Private m_ReadDate As New CCursorField(Of Date)("ReadDate")
        Private m_Flags As New CCursorField(Of MailFlags)("Falgs")
        Private m_MessageID As New CCursorFieldObj(Of String)("MessageId")
        Private m_OnlyHeaders As Boolean = False

        Public Sub New()
        End Sub

        Public Property OnlyHeaders As Boolean
            Get
                Return Me.m_OnlyHeaders
            End Get
            Set(value As Boolean)
                Me.m_OnlyHeaders = value
            End Set
        End Property

        Public ReadOnly Property Flags As CCursorField(Of MailFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property DeliveredTo As CCursorFieldObj(Of String)
            Get
                Return Me.m_DeliveredTo
            End Get
        End Property

        Public ReadOnly Property MessageID As CCursorFieldObj(Of String)
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Public ReadOnly Property AccountID As CCursorField(Of Integer)
            Get
                Return Me.m_AccountID
            End Get
        End Property

        Public ReadOnly Property ApplicationID As CCursorField(Of Integer)
            Get
                Return Me.m_ApplicationID
            End Get
        End Property

        Public ReadOnly Property FolderID As CCursorField(Of Integer)
            Get
                Return Me.m_FolderID
            End Get
        End Property

        'Me.MessageId = Formats.ToString(dbRis("MessageId")))
        Public ReadOnly Property Bcc As CCursorFieldObj(Of String)
            Get
                Return Me.m_Bcc
            End Get
        End Property

        Public ReadOnly Property Cc As CCursorFieldObj(Of String)
            Get
                Return Me.m_Cc
            End Get
        End Property

        Public ReadOnly Property From As CCursorFieldObj(Of String)
            Get
                Return Me.m_From
            End Get
        End Property

        Public ReadOnly Property ReplyTo As CCursorFieldObj(Of String)
            Get
                Return Me.m_ReplyTo
            End Get
        End Property

        Public ReadOnly Property Subject As CCursorFieldObj(Of String)
            Get
                Return Me.m_Subject
            End Get
        End Property

        Public ReadOnly Property [To] As CCursorFieldObj(Of String)
            Get
                Return Me.m_To
            End Get
        End Property

        Public ReadOnly Property Body As CCursorFieldObj(Of String)
            Get
                Return Me.m_Body
            End Get
        End Property

        Public ReadOnly Property IsBodyHtml As CCursorField(Of Boolean)
            Get
                Return Me.m_IsBodyHtml
            End Get
        End Property


        Public ReadOnly Property DeliveryDate As CCursorField(Of Date)
            Get
                Return Me.m_DeliveryDate
            End Get
        End Property

        Public ReadOnly Property ReadDate As CCursorField(Of Date)
            Get
                Return Me.m_ReadDate
            End Get
        End Property

        Public ReadOnly Property DownloadDate As CCursorField(Of Date)
            Get
                Return Me.m_DownloadDate
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_EmailMessages"
        End Function

        Protected Overrides Function GetModule() As Sistema.CModule
            Return Office.Mails.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

        Protected Overrides Sub OnInitialize(item As Object)
            MyBase.OnInitialize(item)
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("OnlyHeaders", Me.m_OnlyHeaders)
            writer.Settings.SetValueBool("OnlyHeaders", Me.m_OnlyHeaders)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyHeaders" : Me.m_OnlyHeaders = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub SyncPage()
            MyBase.SyncPage()

            Dim msgsid() As Integer = {}
            Dim items As Object = Me.GetItemsArray
            For Each m As MailMessage In items
                If (m IsNot Nothing AndAlso m.m_Attachements Is Nothing) Then
                    msgsid = DMD.Sistema.Arrays.InsertSorted(msgsid, GetID(m))
                End If
            Next

            If (msgsid.Length > 0) Then
                Dim col As New CCollection(Of MailAttachment)
                Dim cursor As New MailAttachmentCursor
                cursor.MessageID.ValueIn(msgsid)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.ID.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    col.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                For Each m As MailMessage In items
                    If (m IsNot Nothing AndAlso m.m_Attachements Is Nothing) Then
                        Dim atts As New MailAttachmentCollection
                        atts.SetOwner(m)
                        For Each a As MailAttachment In col
                            If (a.MessageID = GetID(m)) Then
                                atts.Add(a)
                            End If
                        Next
                        m.m_Attachements = atts
                    End If
                Next
            End If


            '-----------------
            msgsid = {}
            For Each m As MailMessage In items
                If (m IsNot Nothing AndAlso m.m_OriginalAddresses Is Nothing) Then
                    msgsid = DMD.Sistema.Arrays.InsertSorted(msgsid, GetID(m))
                End If
            Next
            If (msgsid.Length > 0) Then
                Dim col1 As New CCollection(Of MailAddress)
                Dim cursor1 As New MailAddressCursor
                cursor1.IgnoreRights = True
                cursor1.MessageID.ValueIn(msgsid)
                cursor1.ID.SortOrder = SortEnum.SORT_ASC
                While Not cursor1.EOF
                    Dim add As MailAddress = cursor1.Item
                    col1.Add(add)
                    cursor1.MoveNext()
                End While
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : 
                cursor1 = Nothing

                For Each m As MailMessage In items
                    If (m IsNot Nothing AndAlso m.m_OriginalAddresses Is Nothing) Then
                        Dim lst As New System.Collections.ArrayList
                        For Each add As MailAddress In col1
                            If (add.MessageID = GetID(m)) Then
                                add.SetApplication(m.Application)
                                add.SetMessage(m)
                                lst.Add(add)
                            End If
                        Next
                        m.SetOriginalAddressList(lst)
                    End If
                Next
            End If

        End Sub
    End Class

End Class