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
    Public Class MailRootFolder
        Inherits MailFolder

        Private m_Inbox As MailFolder
        Private m_TrashBin As MailFolder
        Private m_Drafts As MailFolder
        Private m_Sent As MailFolder
        Private m_Spam As MailFolder
        Private m_Archive As MailFolder
        Private m_FindFolder As MailFolder

        Public Sub New()
            Me.m_Inbox = Nothing
            Me.m_TrashBin = Nothing
            Me.m_Drafts = Nothing
            Me.m_Sent = Nothing
            Me.m_Spam = Nothing
            Me.m_Archive = Nothing
            Me.m_FindFolder = Nothing
        End Sub

        ''' <summary>
        ''' Cartella archivio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Archive As MailFolder
            Get
                If (Me.m_Archive Is Nothing) Then Me.m_Archive = Me.Childs.GetItemByName("/archive")
                If (Me.m_Archive Is Nothing) Then
                    Me.m_Archive = Me.Childs.Add("archive")
                    Me.m_Archive.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_Archive.Save()
                End If
                Return Me.m_Archive
            End Get
        End Property

        ''' <summary>
        ''' Cartelle ricerche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FindFolder As MailFolder
            Get
                If (Me.m_FindFolder Is Nothing) Then Me.m_FindFolder = Me.Childs.GetItemByName("/findfolder")
                If (Me.m_FindFolder Is Nothing) Then
                    Me.m_FindFolder = Me.Childs.Add("findfolder")
                    Me.m_FindFolder.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_FindFolder.Save()
                End If
                Return Me.m_FindFolder
            End Get
        End Property

        ''' <summary>
        ''' Cartella principale della posta in arrivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Inbox As MailFolder
            Get
                If (Me.m_Inbox Is Nothing) Then Me.m_Inbox = Me.Childs.GetItemByName("/inbox")
                If (Me.m_Inbox Is Nothing) Then
                    Me.m_Inbox = Me.Childs.Add("inbox")
                    Me.m_Inbox.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_Inbox.Save()
                End If
                Return Me.m_Inbox
            End Get
        End Property

        ''' <summary>
        ''' Cartella principale del cestino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TrashBin As MailFolder
            Get
                If (Me.m_TrashBin Is Nothing) Then Me.m_TrashBin = Me.Childs.GetItemByName("/recycler")
                If (Me.m_TrashBin Is Nothing) Then
                    Me.m_TrashBin = Me.Childs.Add("recycler")
                    Me.m_TrashBin.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_TrashBin.Save()
                End If
                Return Me.m_TrashBin
            End Get
        End Property

        ''' <summary>
        ''' Cartella delle bozze
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Drafts As MailFolder
            Get
                If (Me.m_Drafts Is Nothing) Then Me.m_Drafts = Me.Childs.GetItemByName("/drafts")
                If (Me.m_Drafts Is Nothing) Then
                    Me.m_Drafts = Me.Childs.Add("drafts")
                    Me.m_Drafts.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_Drafts.Save()
                End If
                Return Me.m_Drafts
            End Get
        End Property

        ''' <summary>
        ''' Cartella delle email inviate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Sent As MailFolder
            Get
                If (Me.m_Sent Is Nothing) Then Me.m_Sent = Me.Childs.GetItemByName("/sent")
                If (Me.m_Sent Is Nothing) Then
                    Me.m_Sent = Me.Childs.Add("sent")
                    Me.m_Sent.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_Sent.Save()
                End If
                Return Me.m_Sent
            End Get
        End Property

        ''' <summary>
        ''' Cartella delle email indesiderate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Spam As MailFolder
            Get
                If (Me.m_Spam Is Nothing) Then Me.m_Spam = Me.Childs.GetItemByName("/spam")
                If (Me.m_Spam Is Nothing) Then
                    Me.m_Spam = Me.Childs.Add("spam")
                    Me.m_Spam.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_Spam.Save()
                End If
                Return Me.m_Spam
            End Get
        End Property

        Protected Friend Overrides Function UpdateFolder(folder As MailFolder) As MailFolder
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            If (GetID(folder) = 0) Then Return Nothing
            If (GetID(folder) = GetID(Me.Inbox)) Then
                Me.m_Inbox = folder
            ElseIf (GetID(folder) = GetID(Me.m_Archive)) Then
                Me.m_Archive = folder
            ElseIf (GetID(folder) = GetID(Me.m_TrashBin)) Then
                Me.m_TrashBin = folder
            ElseIf (GetID(folder) = GetID(Me.m_Drafts)) Then
                Me.m_Drafts = folder
            ElseIf (GetID(folder) = GetID(Me.m_Sent)) Then
                Me.m_Sent = folder
            ElseIf (GetID(folder) = GetID(Me.m_Spam)) Then
                Me.m_Spam = folder
            ElseIf (GetID(folder) = GetID(Me.m_FindFolder)) Then
                Me.m_FindFolder = folder
            End If
            Return MyBase.UpdateFolder(folder)
        End Function


    End Class

End Class