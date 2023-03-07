Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema

Partial Public Class Sistema

    <Serializable> _
    Public NotInheritable Class CModule
        Inherits DBObject
        Implements IComparable, ISettingsOwner

        Private m_DisplayName As String
        Private m_ModuleName As String 'Nome del modulo
        Private m_ModulePath As String 'Percorso del modulo		
        Private m_Description As String 'Descrizione del modulo
        Private m_IsBuiltIn As Boolean 'Vero se il modulo appartiene alla libreria standard
        Private m_Visible As Boolean 'Se vero il modulo è visibile
        Private m_Posizione As Integer
        Private m_ShowInMainPage As Boolean 'Se vero il modulo deve essere visualizzato in prima pagina
        Private m_ClassHandler As String 'Nome della classe che verrà utilizzata per eseguire il modulo
        'Private m_Handler As IModuleHandler = Nothing
        Private m_EncriptURL As Boolean 'Se vero la URL viene nascota da un collegamento valido solo all'interno della sessione corrente
        Private m_Target As String 'Finstra di destinazione del collegamento
        <NonSerialized> _
        Private m_Childs As CModulesCollection  'Collezione dei moduli contenuti
        Private m_IconPath As String 'Percorso di una icona che rappresenta il modulo
        Private m_ParentID As Integer
        <NonSerialized> _
        Private m_Parent As CModule
        <NonSerialized> _
        Private m_DefinedActions As CModuleActions
        <NonSerialized> _
        Private m_Settings As CModuleSettings

        Public Sub New()
            Me.m_DisplayName = ""
            Me.m_ModuleName = ""
            Me.m_ModulePath = ""
            Me.m_Description = ""
            Me.m_IsBuiltIn = False
            Me.m_Visible = True
            Me.m_Posizione = 0
            Me.m_ShowInMainPage = False
            Me.m_ClassHandler = ""
            'Me.m_Handler = Nothing
            Me.m_EncriptURL = True
            Me.m_Target = ""
            Me.m_Childs = Nothing
            Me.m_IconPath = ""
            Me.m_ParentID = 0
            Me.m_Parent = Nothing
            'Me.m_Configuration = Nothing
            Me.m_DefinedActions = Nothing
            Me.m_Settings = Nothing
        End Sub

        Public Sub New(ByVal moduleName As String)
            Me.New()
            moduleName = Strings.Trim(moduleName)
            If (moduleName = "") Then Throw New ArgumentNullException("moduleName")
            Me.m_ModuleName = moduleName
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Sistema.Modules.Module
        End Function

        ''' <summary>
        ''' Restituisce le impostazioni aggiuntive del modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Settings As CModuleSettings
            Get
                If Me.m_Settings Is Nothing Then Me.m_Settings = New CModuleSettings(Me)
                Return Me.m_Settings
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il nome del modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ModuleName As String
            Get
                Return Me.m_ModuleName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ModuleName
                If (oldValue = value) Then Exit Property
                Me.m_ModuleName = value
                Me.DoChanged("ModuleName", value, oldValue)
            End Set
        End Property

        Public Property Posizione As Integer
            Get
                Return Me.m_Posizione
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Posizione
                If (oldValue = value) Then Exit Property
                Me.m_Posizione = value
                Me.DoChanged("Posizione", value, oldValue)
            End Set
        End Property

        Public Property ModulePath As String
            Get
                Return Me.m_ModulePath
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ModulePath
                If (oldValue = value) Then Exit Property
                Me.m_ModulePath = value
                Me.DoChanged("ModulePath", value, oldValue)
            End Set
        End Property

        Public Property Description As String
            Get
                Return Me.m_Description
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Description
                If (oldValue = value) Then Exit Property
                Me.m_Description = value
                Me.DoChanged("Description", value, oldValue)
            End Set
        End Property

        Public Function IsBuiltIn() As Boolean
            Return Me.m_IsBuiltIn
        End Function

        Public Property Visible As Boolean
            Get
                Return Me.m_Visible
            End Get
            Set(value As Boolean)
                If (Me.m_Visible = value) Then Exit Property
                Me.m_Visible = value
                Me.DoChanged("Visible", value, Not value)
            End Set
        End Property

        Public Property IconPath As String
            Get
                Return Me.m_IconPath
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IconPath
                If (oldValue = value) Then Exit Property
                Me.m_IconPath = value
                Me.DoChanged("IconPath", value, oldValue)
            End Set
        End Property

        Public Property DisplayName As String
            Get
                Return Me.m_DisplayName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DisplayName
                If (oldValue = value) Then Exit Property
                Me.m_DisplayName = value
                Me.DoChanged("DisplayName", value, oldValue)
            End Set
        End Property

        Public Sub InitializeStandardActions()
            Me.DefinedActions.RegisterAction("create")
            Me.DefinedActions.RegisterAction("list")
            Me.DefinedActions.RegisterAction("edit")
            Me.DefinedActions.RegisterAction("delete")
            Me.DefinedActions.RegisterAction("list_own")
            Me.DefinedActions.RegisterAction("edit_own")
            Me.DefinedActions.RegisterAction("delete_own")

            Me.DefinedActions.RegisterAction("list_office")
            Me.DefinedActions.RegisterAction("edit_office")
            Me.DefinedActions.RegisterAction("delete_office")
        End Sub

        Public Property ParentID As Integer
            Get
                Return GetID(Me.m_Parent, Me.m_ParentID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ParentID
                If oldValue = value Then Exit Property
                Me.m_ParentID = value
                If (Me.m_Parent IsNot Nothing) Then Me.m_Parent.ResetChilds()
                Me.m_Parent = Sistema.Modules.GetItemById(Me.m_ParentID)
                If (Me.m_Parent IsNot Nothing) Then Me.m_Parent.ResetChilds()
                Me.DoChanged("ParentID", value, oldValue)
            End Set
        End Property

        Public Property Parent As CModule
            Get
                If (Me.m_Parent Is Nothing) Then Me.m_Parent = Sistema.Modules.GetItemById(Me.m_ParentID)
                Return Me.m_Parent
            End Get
            Set(value As CModule)
                Dim oldValue As CModule = Me.Parent
                If (oldValue = value) Then Exit Property
                If (Me.m_Parent IsNot Nothing) Then Me.m_Parent.ResetChilds()
                Me.m_Parent = value
                Me.m_ParentID = GetID(value)
                If (Me.m_Parent IsNot Nothing) Then Me.m_Parent.ResetChilds()
                Me.DoChanged("Parent", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetParent(ByVal value As CModule)
            Me.m_Parent = value
            Me.m_ParentID = GetID(value)
        End Sub


        ''' <summary>
        ''' Restituisce la collezione delle azioni definite per il modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefinedActions As CModuleActions
            Get
                SyncLock Modules.actionsLock
                    If (Me.m_DefinedActions Is Nothing) Then
                        Me.m_DefinedActions = New CModuleActions
                        Me.m_DefinedActions.Load(Me)
                    End If
                    Return Me.m_DefinedActions
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della classe che gestisce il modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClassHandler As String
            Get
                Return Me.m_ClassHandler
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ClassHandler
                If (oldValue = value) Then Exit Property
                Me.m_ClassHandler = Trim(value)
                'Me.m_Handler = Nothing
                Me.DoChanged("ClassHandler", value, oldValue)
            End Set
        End Property

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Modules.OnModuleCreated(New ModuleEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Modules.OnModuleDeleted(New ModuleEventArgs(Me))
        End Sub

        Public Sub DispatchEvent(ByVal e As EventDescription)
            e.SetModule(Me)
            Events.DispatchEvent(e)
        End Sub

        ''' <summary>
        ''' Restituisce la collezione dei moduli "figlio"
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Childs As CModulesCollection
            Get
                SyncLock Me
                    If (Me.m_Childs Is Nothing) Then Me.m_Childs = New CModulesCollection(Me)
                    Return Me.m_Childs
                End SyncLock
            End Get
        End Property


        Public Function UserCanDoAction(ByVal action As CModuleAction) As Boolean
            Return action.UserCanDoAction(Users.CurrentUser)
        End Function

        Public Function UserCanDoAction(ByVal actionName As String) As Boolean
            Dim a As CModuleAction = Me.DefinedActions.GetItemByKey(actionName)
            If (a Is Nothing) Then Return False ' Throw New ArgumentOutOfRangeException("Il modulo non implementa l'azione [" & actionName & "]")
            Return Me.UserCanDoAction(a)
        End Function

        Public Function UserCanDoAction(ByVal actionID As Integer) As Boolean
            Return Me.UserCanDoAction(Me.DefinedActions.GetItemById(actionID))
        End Function

        Public Function IsVisibleToUser(ByVal user As CUser) As Boolean
            'Return Me.IsVisibleToUser(GetID(user))
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Dim a, n As Boolean
            a = False
            n = False
            user.Modules.GetAllowNegate(Me, a, n)
            If Not n Then
                For Each grp As CGroup In user.Groups
                    grp.Modules.GetAllowNegate(Me, a, n)
                    If (n) Then Exit For
                Next
            End If
            Return a And Not n
        End Function

        Private m_handler As IModuleHandler = Nothing

        Public Function CreateHandler(ByVal context As Object) As Object
            'If (Me.m_Handler Is Nothing AndAlso Me.m_ClassHandler <> vbNullString) Then
            '    Me.m_Handler = Types.CreateInstance(Me.m_ClassHandler)
            '    Me.m_Handler.SetModule(Me)
            'End If
            'If (m_Handler IsNot Nothing) Then Me.m_Handler.Context = context
            'Return Me.m_Handler
            If (Me.m_handler Is Nothing AndAlso Me.m_ClassHandler <> vbNullString) Then
                Me.m_handler = Types.CreateInstance(Me.m_ClassHandler)
                Me.m_handler.SetModule(Me)
            End If

            Return Me.m_handler
        End Function

        Public Function ExecuteAction(ByVal actionName As String) As String
            Return Me.ExecuteAction(actionName, Nothing)
        End Function

        'Public Function ExecuteAction1(ByVal actionName As String) As MethodResults
        '    Return Me.ExecuteAction1(actionName, Nothing)
        'End Function

        Public Function ExecuteAction(ByVal actionName As String, ByVal context As Object) As String
            If actionName = "" Then actionName = "list"
            Dim handler As IModuleHandler = Me.CreateHandler(context)
            Return handler.ExecuteAction(context, actionName)
        End Function

        'Public Function ExecuteAction1(ByVal actionName As String, ByVal context As Object) As MethodResults
        '    If actionName = "" Then actionName = "list"
        '    Dim handler As IModuleHandler = Me.CreateHandler(context)
        '    Return handler.ExecuteAction1(context, actionName)
        'End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_Modules"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_ModuleName = reader.Read("ModuleName", Me.m_ModuleName)
            Me.m_ModulePath = reader.Read("ModulePath", Me.m_ModulePath)
            Me.m_DisplayName = reader.Read("DisplayName", Me.m_DisplayName)
            Me.m_Description = reader.Read("Description", Me.m_Description)
            Me.m_IsBuiltIn = reader.Read("BuiltIn", Me.m_IsBuiltIn)
            Me.m_Visible = reader.Read("Visible", Me.m_Visible)
            Me.m_IconPath = reader.Read("IconPath", Me.m_IconPath)
            Me.m_ParentID = reader.Read("Parent", Me.m_ParentID)
            Me.m_ShowInMainPage = reader.Read("ShowInMainPage", Me.m_ShowInMainPage)
            Me.m_ClassHandler = reader.Read("ClassHandler", Me.m_ClassHandler)
            Me.m_EncriptURL = reader.Read("EncriptURL", Me.m_EncriptURL)
            Me.m_Target = reader.Read("Target", Me.m_Target)
            Me.m_Posizione = reader.Read("Posizione", Me.m_Posizione)
            'm_ConfigClass = Formats.ToString(dbRis("ConfigClass"))
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            'dbRis("ConfigClass", Me.m_ConfigClass)
            writer.Write("ModuleName", Me.m_ModuleName)
            writer.Write("ModulePath", Me.m_ModulePath)
            writer.Write("Description", Me.m_Description)
            writer.Write("BuiltIn", Me.m_IsBuiltIn)
            writer.Write("Visible", Me.m_Visible)
            writer.Write("IconPath", Me.m_IconPath)
            writer.Write("Parent", GetID(m_Parent, m_ParentID))
            writer.Write("Posizione", Me.m_Posizione)
            writer.Write("DisplayName", Me.m_DisplayName)
            writer.Write("ShowInMainPage", Me.m_ShowInMainPage)
            writer.Write("ClassHandler", Me.m_ClassHandler)
            writer.Write("EncriptURL", Me.m_EncriptURL)
            writer.Write("Target", Me.m_Target)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_IsBuiltIn", Me.m_IsBuiltIn)
            writer.WriteAttribute("m_Visible", Me.m_Visible)
            writer.WriteAttribute("m_Posizione", Me.m_Posizione)
            writer.WriteAttribute("m_ShowInMainPage", Me.m_ShowInMainPage)
            writer.WriteAttribute("m_ClassHandler", Me.m_ClassHandler)
            writer.WriteAttribute("m_EncriptURL", Me.m_EncriptURL)
            writer.WriteAttribute("m_Target", Me.m_Target)
            writer.WriteAttribute("m_IconPath", Me.m_IconPath)
            writer.WriteAttribute("m_DisplayName", Me.m_DisplayName)
            writer.WriteAttribute("m_ModuleName", Me.m_ModuleName)
            writer.WriteAttribute("m_ModulePath", Me.m_ModulePath)
            writer.WriteAttribute("m_ParentID", Me.ParentID)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("m_Description", Me.m_Description)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "m_DisplayName" : Me.m_DisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_ModuleName" : Me.m_ModuleName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_ModulePath" : Me.m_ModulePath = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Description" : Me.m_Description = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_IsBuiltIn" : Me.m_IsBuiltIn = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_Posizione" : Me.m_Posizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_ShowInMainPage" : Me.m_ShowInMainPage = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_ClassHandler" : Me.m_ClassHandler = XML.Utils.Serializer.DeserializeString(fieldValue)
                    'ret &= "<m_Handler
                Case "m_EncriptURL" : Me.m_EncriptURL = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_Target" : Me.m_Target = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Childs"
                    'Me.Childs.Clear
                Case "m_IconPath" : Me.m_IconPath = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_ParentID" : Me.m_ParentID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'ret &= "<m_ConfigClass
                    'ret &= "<m_Configuration
                Case "m_DefinedActions"
                    If Not TypeOf (fieldValue) Is String Then
                        If Not (IsArray(fieldValue)) Then fieldValue = {fieldValue}
                        Call Me.DefinedActions.Clear()
                        Call Me.DefinedActions.AddRange(fieldValue)
                    End If
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Sistema.Modules.UpdateCached(Me)
            Return ret
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_ModuleName
        End Function

        Public Function CompareTo(ByVal b As CModule) As Integer
            Dim ret As Integer = (Me.m_Posizione - b.m_Posizione)
            If (ret = 0) Then ret = Strings.Compare(Me.m_DisplayName, b.m_DisplayName)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Private ReadOnly Property _Settings As CSettings Implements ISettingsOwner.Settings
            Get
                Return Me.Settings
            End Get
        End Property

        Friend Sub ResetChilds()
            SyncLock Me
                Me.m_Childs = Nothing
            End SyncLock
        End Sub

    End Class


End Class