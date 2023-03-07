Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Pagina di proprietà registrata per un oggetto
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CRegisteredPropertyPage
        Inherits DBObjectBase
        Implements IComparable


        Private m_ClassName As String
        Private m_TabPageClass As String
        Private m_TabPageType As System.Type
        Private m_Context As String
        Private m_Priority As Integer
        <NonSerialized> Private m_UsersAuth As PropPageUserAllowNegateCollection
        <NonSerialized> Private m_GroupAuth As PropPageGroupAllowNegateCollection

        Public Sub New()
            Me.m_ClassName = ""
            Me.m_TabPageClass = ""
            Me.m_Context = ""
            Me.m_Priority = 0
            Me.m_UsersAuth = Nothing
            Me.m_GroupAuth = Nothing
        End Sub

        ''' <summary>
        ''' Assegna le autorizzazione utente per la pagina 
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="allow"></param>
        ''' <param name="negate"></param>
        ''' <returns></returns>
        Public Function SetUserAllowNegate(ByVal user As CUser, ByVal allow As Boolean, ByVal negate As Boolean) As PropPageUserAllowNegate
            SyncLock Me
                Dim item As PropPageUserAllowNegate = Nothing
                Dim i As Integer = 0
                While (i < Me.UserAuth.Count) AndAlso (item Is Nothing)
                    If Me.UserAuth(i).UserID = GetID(user) Then
                        item = Me.UserAuth(i)
                    Else
                        i += 1
                    End If
                End While
                If (allow = negate) Then
                    If (item IsNot Nothing) Then
                        item.Delete()
                        Me.UserAuth.Remove(item)
                    End If
                Else
                    If (item Is Nothing) Then
                        item = New PropPageUserAllowNegate()
                        Me.UserAuth.Add(item)
                    End If
                    item.Item = Me
                    item.User = user
                    item.Allow = allow
                    item.Negate = negate
                    item.Save()
                End If

                Return item
            End SyncLock
        End Function

        Public Function SetGroupAllowNegate(ByVal group As CGroup, ByVal allow As Boolean, ByVal negate As Boolean) As PropPageGroupAllowNegate
            'SyncLock Me
            Dim item As PropPageGroupAllowNegate = Nothing
            Dim i As Integer = 0
            While (i < Me.GroupAuth.Count) AndAlso (item Is Nothing)
                If Me.GroupAuth(i).GroupID = GetID(group) Then
                    item = Me.GroupAuth(i)
                Else
                    i += 1
                End If
            End While
            If (allow = negate) Then
                If (item IsNot Nothing) Then
                    item.Delete()
                    Me.GroupAuth.Remove(item)
                End If
            Else
                If (item Is Nothing) Then
                    item = New PropPageGroupAllowNegate()
                    Me.GroupAuth.Add(item)
                End If
                item.Item = Me
                item.Group = group
                item.Allow = allow
                item.Negate = negate
                item.Save()
            End If

            Return item
            'End SyncLock
        End Function

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_GroupAuth = Nothing
            Me.m_UsersAuth = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsVisibleToUser(ByVal user As CUser) As Boolean
            SyncLock Me
                Dim a = False, n As Boolean = False
                Dim i As Integer = 0
                Dim ua As PropPageUserAllowNegate
                Dim ga As PropPageGroupAllowNegate

                While (Not n AndAlso i < Me.UserAuth.Count)
                    ua = Me.UserAuth(i)
                    If (ua.UserID = GetID(user)) Then
                        a = ua.Allow
                        n = ua.Negate
                    End If
                    i += 1
                End While

                i = 0
                While (Not n AndAlso i < Me.GroupAuth.Count)
                    ga = Me.GroupAuth(i)
                    For Each gp As CGroup In user.Groups
                        If (ga.GroupID = GetID(gp)) Then
                            a = ga.Allow
                            n = ga.Negate
                            Exit For
                        End If
                    Next
                    i += 1
                End While

                Return a And Not n
            End SyncLock
        End Function

        ''' <summary>
        ''' Restituisce vero se l'utente è autorizzato a vedere questo link
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsVisibleToUser(ByVal userID As Integer) As Boolean
            Return Me.IsVisibleToUser(Sistema.Users.GetItemById(userID))
        End Function

        ''' <summary>
        ''' Restituisce la collezione degli oggetti autorizzazione/negazione definiti per gli utenti specifici
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UserAuth As PropPageUserAllowNegateCollection
            Get
                SyncLock Me
                    If (Me.m_UsersAuth Is Nothing) Then Me.m_UsersAuth = New PropPageUserAllowNegateCollection(Me)
                    Return Me.m_UsersAuth
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione degli oggetti autorizzazione/negazione definiti per i gruppi specifici
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GroupAuth As PropPageGroupAllowNegateCollection
            Get
                SyncLock Me
                    If (Me.m_GroupAuth Is Nothing) Then Me.m_GroupAuth = New PropPageGroupAllowNegateCollection(Me)
                    Return Me.m_GroupAuth
                End SyncLock
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return Sistema.PropertyPages.Module
        End Function

        Public Property ClassName As String
            Get
                Return Me.m_ClassName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ClassName
                If (oldValue = value) Then Exit Property
                Me.m_ClassName = value
                Me.DoChanged("ClassName", value, oldValue)
            End Set
        End Property

        Public Property TabPageClass As String
            Get
                Return Me.m_TabPageClass
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TabPageClass
                If (oldValue = value) Then Exit Property
                Me.m_TabPageClass = value
                Me.DoChanged("TabPageClass", value, oldValue)
            End Set
        End Property

        Public Property TabPageType As System.Type
            Get
                If Me.m_TabPageType Is Nothing Then Me.m_TabPageType = Types.GetType(Me.m_TabPageClass)
                Return Me.m_TabPageType
            End Get
            Set(value As System.Type)
                Dim oldValue As System.Type = Me.m_TabPageType
                If (oldValue Is value) Then Exit Property
                Me.m_TabPageType = value
                Me.m_TabPageClass = value.Name
                Me.DoChanged("TabPageType", value, oldValue)
            End Set
        End Property

        Public Property Context As String
            Get
                Return Me.m_Context
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Context
                If (oldValue = value) Then Exit Property
                Me.m_Context = value
                Me.DoChanged("Context", value, oldValue)
            End Set
        End Property

        Public Property Priority As Integer
            Get
                Return Me.m_Priority
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Priority
                If (oldValue = value) Then Exit Property
                Me.m_Priority = value
                Me.DoChanged("Priority", value, oldValue)
            End Set
        End Property

        
        Public Overrides Function GetTableName() As String
            Return "tbl_RegisteredTabPages"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_ClassName = reader.Read("ClassName", Me.m_ClassName)
            Me.m_TabPageClass = reader.Read("TabPageClass", Me.m_TabPageClass)
            Me.m_Context = reader.Read("Context", Me.m_Context)
            Me.m_Priority = reader.Read("Priority", Me.m_Priority)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("ClassName", Me.m_ClassName)
            writer.Write("TabPageClass", Me.m_TabPageClass)
            writer.Write("Context", Me.m_Context)
            writer.Write("Priority", Me.m_Priority)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_ClassName", Me.m_ClassName)
            writer.WriteAttribute("m_TabPageClass", Me.m_TabPageClass)
            writer.WriteAttribute("m_Context", Me.m_Context)
            writer.WriteAttribute("m_Priority", Me.m_Priority)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("UserAuth", Me.UserAuth)
            writer.WriteTag("GroupAuth", Me.GroupAuth)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "m_ClassName" : Me.m_ClassName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_TabPageClass" : Me.m_TabPageClass = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Context" : Me.m_Context = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Priority" : Me.m_Priority = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserAuth" : Me.m_UsersAuth = fieldValue : Me.m_UsersAuth.SetPropPage(Me)
                Case "GroupAuth" : Me.m_GroupAuth = fieldValue : Me.m_GroupAuth.SetPropPage(Me)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_ClassName & "/" & Me.m_TabPageClass & "(" & Me.m_Context & ", " & Me.m_Priority & ")"
        End Function

        Public Function CompareTo(ByVal value As CRegisteredPropertyPage) As Integer
            Dim ret As Integer
            ret = Strings.Compare(Me.Context, value.Context, CompareMethod.Text)
            If ret <> 0 Then Return ret
            ret = Strings.Compare(Me.ClassName, value.ClassName, CompareMethod.Text)
            If ret <> 0 Then Return ret
            ret = (Me.Priority - value.Priority)
            If ret <> 0 Then Return ret
            ret = Strings.Compare(Me.TabPageClass, value.TabPageClass, CompareMethod.Text)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Sistema.PropertyPages.UpdateCached(Me)
        End Sub


        Protected Friend Sub InvalidateUserAuth()
            Me.m_UsersAuth = Nothing
        End Sub

        Protected Friend Sub InvalidateGroupAuth()
            Me.m_GroupAuth = Nothing
        End Sub

    End Class



End Class