Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases

Partial Class Office

    <Serializable> _
    Public Class MailAttachmentCollection
        Inherits CCollection(Of MailAttachment)

        <NonSerialized> Private m_Message As MailMessage
        'Private m_Attachments As System.Net.Mail.AttachmentCollection

        Public Sub New()
            Me.m_Message = Nothing
        End Sub

        Protected Friend Sub New(ByVal message As MailMessage) ', ByVal under As System.Net.Mail.AttachmentCollection)
            If (message Is Nothing) Then Throw New ArgumentNullException("message")
            Me.SetOwner(message)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Message IsNot Nothing) Then DirectCast(value, MailAttachment).SetMessage(Me.m_Message)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Message IsNot Nothing) Then DirectCast(newValue, MailAttachment).SetMessage(Me.m_Message)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub SetOwner(ByVal message As MailMessage)
            Me.m_Message = message
            If (message IsNot Nothing) Then
                For Each a As MailAttachment In Me
                    a.SetMessage(message)
                Next
            End If
        End Sub

        Protected Friend Sub Load()
            Dim cursor As MailAttachmentCursor = Nothing
            Try
                Me.Clear()
                If (GetID(Me.m_Message) = 0) Then Exit Sub
                cursor = New MailAttachmentCursor
                cursor.MessageID.Value = GetID(Me.m_Message)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.ID.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

    End Class

End Class