Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Sistema
Imports DMD.Databases
Imports System.IO
Imports DMD

Partial Class Office



    <Serializable>
    Public Class MailFolder
        Inherits DBObjectPO
        Implements IComparable

        Private m_Name As String
        Private m_ParentID As Integer
        <NonSerialized> Private m_Parent As MailFolder
        <NonSerialized> Private m_Childs As MailFolderChilds
        Private m_TotalMessages As Integer
        Private m_TotalUnread As Integer
        Private m_Flags As Integer
        Private m_Attributi As CKeyCollection
        <NonSerialized> Private m_Utente As CUser
        Private m_IDUtente As Integer
        Private m_ApplicationID As Integer
        Private m_Application As MailApplication

        Public Sub New()
            Me.m_Name = ""
            Me.m_ParentID = 0
            Me.m_Parent = Nothing
            Me.m_Childs = Nothing
            Me.m_TotalMessages = 0
            Me.m_TotalUnread = 0
            Me.m_Flags = 0
            Me.m_Attributi = Nothing
            Me.m_ApplicationID = 0
            Me.m_Application = Nothing
        End Sub

        Public Sub New(ByVal name As String)
            Me.New()
            Me.m_Name = Strings.Trim(name)
        End Sub

        Public Property ApplicationID As Integer
            Get
                Return GetID(Me.m_Application, Me.m_ApplicationID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ApplicationID
                If (oldValue = value) Then Return
                Me.m_ApplicationID = value
                Me.m_Application = Nothing
                Me.DoChanged("ApplicationID", value, oldValue)
            End Set
        End Property

        Public Property Application As MailApplication
            Get
                If (Me.m_Application Is Nothing) Then Me.m_Application = Office.Mails.Applications.GetItemById(Me.m_ApplicationID)
                Return Me.m_Application
            End Get
            Set(value As MailApplication)
                Dim oldValue As MailApplication = Me.Application
                If (oldValue Is value) Then Return
                Me.m_Application = value
                Me.m_ApplicationID = GetID(value)
                Me.DoChanged("Application", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetApplication(ByVal app As MailApplication)
            Me.m_Application = app
            Me.m_ApplicationID = GetID(app)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il proprietario della cartella
        ''' </summary>
        ''' <returns></returns>
        Public Property Utente As CUser
            Get
                If (Me.m_Utente Is Nothing) Then Me.m_Utente = Sistema.Users.GetItemById(Me.m_IDUtente)
                Return Me.m_Utente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Utente
                If (oldValue = value) Then Return
                Me.m_Utente = value
                Me.m_IDUtente = GetID(value)
                Me.DoChanged("Utente", value, oldValue)
            End Set
        End Property

        Public Property IDUtente As Integer
            Get
                Return GetID(Me.m_Utente, Me.m_IDUtente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUtente
                If (oldValue = value) Then Return
                Me.m_IDUtente = value
                Me.m_Utente = Nothing
                Me.DoChanged("IDUtente", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetUtente(ByVal value As CUser)
            Me.m_Utente = value
            Me.m_IDUtente = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il numero totale di messaggio memorizzati nella cartella (questo valore viene aggiornato dall'applicazione e potrebbe essere non sincronizzato)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TotalMessages As Integer
            Get
                Return Me.m_TotalMessages
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TotalMessages
                If (oldValue = value) Then Exit Property
                Me.m_TotalMessages = value
                Me.DoChanged("TotalMessages", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero totale di messaggi non letti (aggiornato dall'applicazione)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TotalUnread As Integer
            Get
                Return Me.m_TotalUnread
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TotalUnread
                If (oldValue = value) Then Exit Property
                Me.m_TotalUnread = value
                Me.DoChanged("TotalUnread", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una collezione di attributi aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della cartella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del contenitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParentID As Integer
            Get
                Return GetID(Me.m_Parent, Me.m_ParentID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ParentID
                If (oldValue = value) Then Exit Property
                Me.m_ParentID = value
                Me.m_Parent = Nothing
                Me.DoChanged("ParentID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il contenitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Parent As MailFolder
            Get
                If (Me.m_Parent Is Nothing) Then Me.m_Parent = Office.Mails.Folders.GetItemById(Me.m_ParentID)
                Return Me.m_Parent
            End Get
            Set(value As MailFolder)
                Dim oldValue As MailFolder = Me.Parent
                If (oldValue Is value) Then Exit Property
                Me.m_Parent = value
                Me.m_ParentID = GetID(value)
                Me.DoChanged("Parent", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetParent(ByVal value As MailFolder)
            Me.m_Parent = value
            Me.m_ParentID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce la collezione delle sottocartelle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Childs As MailFolderChilds
            Get
                If (Me.m_Childs Is Nothing) Then Me.m_Childs = New MailFolderChilds(Me)
                Return Me.m_Childs
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Name
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Mails.Folders.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_eMailFolders"
        End Function

        Public Sub AggiornaConteggi()
            Dim cursor As MailMessageCursor = Nothing
            Try
                Me.m_TotalMessages = 0
                Me.m_TotalUnread = 0
                If (GetID(Me) <> 0) Then
                    cursor = New MailMessageCursor
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IgnoreRights = True
                    cursor.FolderID.Value = GetID(Me)
                    cursor.PageSize = 1
                    Me.m_TotalMessages = cursor.Count
                    cursor.Reset1()
                    cursor.ReadDate.Value = Nothing
                    Me.m_TotalUnread = cursor.Count

                End If
                Me.Save(True)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_ParentID = reader.Read("ParentID", Me.m_ParentID)
            Me.m_TotalMessages = reader.Read("TotalMessages", Me.m_TotalMessages)
            Me.m_TotalUnread = reader.Read("TotalUnread", Me.m_TotalUnread)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDUtente = reader.Read("IDUtente", Me.m_IDUtente)
            Me.m_ApplicationID = reader.Read("ApplicationID", Me.m_ApplicationID)
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("ParentID", Me.ParentID)
            writer.Write("TotalMessages", Me.m_TotalMessages)
            writer.Write("TotalUnread", Me.m_TotalUnread)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("IDUtente", Me.IDUtente)
            writer.Write("ApplicationID", Me.ApplicationID)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("ParentID", Me.ParentID)
            writer.WriteAttribute("TotalMessages", Me.m_TotalMessages)
            writer.WriteAttribute("TotalUnread", Me.m_TotalUnread)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDUtente", Me.IDUtente)
            writer.WriteAttribute("ApplicationID", Me.ApplicationID)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ParentID" : Me.m_ParentID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TotalMessages" : Me.m_TotalMessages = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TotalUnread" : Me.m_TotalUnread = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attributi" : Me.m_Attributi = fieldValue
                Case "IDUtente" : Me.m_IDUtente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ApplicationID" : Me.m_ApplicationID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Private Function getPOS(ByVal root As MailRootFolder, ByVal o As MailFolder) As Integer
            If (root Is Nothing) Then Return -1
            If GetID(root) = GetID(o) Then Return -10
            If GetID(root.Inbox) = GetID(o) Then Return -9
            If GetID(root.Sent) = GetID(o) Then Return -8
            If GetID(root.Drafts) = GetID(o) Then Return -7
            If GetID(root.Spam) = GetID(o) Then Return -6
            If GetID(root.TrashBin) = GetID(o) Then Return -5
            If GetID(root.Archive) = GetID(o) Then Return -4
            If GetID(root.FindFolder) = GetID(o) Then Return -3
            Return -1
        End Function

        Public Function CompareTo(ByVal o As MailFolder) As Integer
            Dim root As MailRootFolder = Nothing
            If (Me.Application IsNot Nothing) Then root = Me.Application.Root
            Dim a1 As Integer = Me.getPOS(root, Me)
            Dim a2 As Integer = Me.getPOS(root, o)
            Dim ret As Integer = a1.CompareTo(a2)
            If (ret = 0) Then ret = Strings.Compare(Me.Name, o.Name, CompareMethod.Text)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            If (Me.Application IsNot Nothing) Then Me.Application.UpdateFolder(Me)
        End Sub

        Protected Friend Overridable Function UpdateFolder(ByVal folder As MailFolder) As MailFolder
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            If (GetID(folder) = 0) Then Return Nothing
            Dim o As MailFolder = Me.Childs.GetItemById(GetID(folder))
            If (o Is Nothing) Then
                For Each f As MailFolder In Me.Childs
                    o = f.UpdateFolder(folder)
                    If (o IsNot Nothing) Then Return o
                Next
            Else
                Dim i As Integer = Me.Childs.IndexOf(o)
                Me.Childs(i) = folder
            End If
            Return o
        End Function

    End Class

End Class