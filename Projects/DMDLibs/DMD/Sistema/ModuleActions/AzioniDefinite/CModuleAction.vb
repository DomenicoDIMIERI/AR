Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Descrive un'azione definita su una risorsa.
    ''' All'azione possono essere associati uno o più handler ModuleActionHandler raccolti nella proprietà Handlers
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CModuleAction
        Inherits DBObjectBase

        Private m_ModuleName As String
        Private m_Module As CModule
        Private m_AuthorizationName As String
        Private m_AuthorizationDescription As String
        Private m_Visible As Boolean
        Private m_ClassHandler As String

        Public Sub New()
            Me.m_ModuleName = ""
            Me.m_Module = Nothing
            Me.m_AuthorizationName = ""
            Me.m_AuthorizationDescription = ""
            Me.m_Visible = False
            Me.m_ClassHandler = ""
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_AuthorizationName & " ( " & Me.m_ModuleName & ")"
        End Function

        ''' <summary>
        ''' Restituisce o imposta il nome del modulo su cui è definita l'azione
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
                Me.m_Module = Nothing
                Me.DoChanged("ModuleName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modulo su cui è definita l'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property [Module] As CModule
            Get
                If (Me.m_Module Is Nothing) Then Me.m_Module = Sistema.Modules.GetItemByName(Me.m_ModuleName)
                Return Me.m_Module
            End Get
            Set(value As CModule)
                Dim oldValue As CModule = Me.Module
                If (oldValue = value) Then Exit Property
                Me.m_Module = value
                If (value IsNot Nothing) Then
                    Me.m_ModuleName = value.ModuleName
                Else
                    Me.m_ModuleName = vbNullString
                End If
                Me.DoChanged("Module", value, oldValue)
            End Set
        End Property

        Friend Sub SetModule(ByVal value As CModule)
            Me.m_Module = value
            If (value IsNot Nothing) Then
                Me.m_ModuleName = value.ModuleName
            Else
                Me.m_ModuleName = vbNullString
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome dell'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationName As String
            Get
                Return Me.m_AuthorizationName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_AuthorizationName
                If (oldValue = value) Then Exit Property
                Me.m_AuthorizationName = value
                Me.DoChanged("AuthorizationName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringe che descrive l'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationDescription As String
            Get
                Return Me.m_AuthorizationDescription
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_AuthorizationDescription
                If (oldValue = value) Then Exit Property
                Me.m_AuthorizationDescription = value
                Me.DoChanged("AuthorizationDescription", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica specifica se si tratta di un'azione di interfaccia o nascota
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Nome della classe che esegue l'azione (non implementato)
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
                Me.m_ClassHandler = value
                Me.DoChanged("ClassHandler", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UserCanDoAction(ByVal user As CUser) As Boolean
            If GetID(user) = GetID(Sistema.Users.KnownUsers.GlobalAdmin) Then Return True
            Dim a, n As Boolean
            a = False
            n = False
            user.Authorizations.GetAllowNegate(Me, a, n)
            If (Not n) Then
                For Each g As CGroup In user.Groups
                    g.Authorizations.GetAllowNegate(Me, a, n)
                    If (n) Then Exit For
                Next
            End If
            Return a And Not n
            'Me.GetUserAllowNegate(GetID(user), a, n)
            'If (a = n) Then
            '    For Each grp As CGroup In user.Groups
            '        Me.GetGroupAllowNegate(GetID(grp), a, n)
            '        If (a <> n) Then Exit For
            '    Next
            'End If
            'Return a And Not n
        End Function

        Public Function UserCanDoAction(ByVal userID As Integer) As Boolean
            Return Me.UserCanDoAction(Sistema.Users.GetItemById(userID))
        End Function

        Public Function SetUserAllowNegate(ByVal user As CUser, ByVal allow As Boolean, ByVal negate As Boolean) As CUserAuthorization
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return user.Authorizations.SetAllowNegate(Me, allow, negate)
        End Function

        Public Function SetGroupAllowNegate(ByVal group As CGroup, ByVal allow As Boolean, ByVal negate As Boolean) As CGroupAuthorization
            If (group Is Nothing) Then Throw New ArgumentNullException("group")
            Return group.Authorizations.SetAllowNegate(Me, allow, negate)
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DefinedAuthorizations"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_ModuleName = reader.Read("Modulo", Me.m_ModuleName)
            Me.m_AuthorizationName = reader.Read("AuthorizationName", Me.m_AuthorizationName)
            Me.m_AuthorizationDescription = reader.Read("AuthorizationDescription", Me.m_AuthorizationDescription)
            Me.m_Visible = reader.Read("Visible", Me.m_Visible)
            Me.m_ClassHandler = reader.Read("ClassHandler", Me.m_ClassHandler)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Modulo", Me.m_ModuleName)
            writer.Write("AuthorizationName", Me.m_AuthorizationName)
            writer.Write("AuthorizationDescription", Me.m_AuthorizationDescription)
            writer.Write("Visible", Me.m_Visible)
            writer.Write("ClassHandler", Me.m_ClassHandler)
            Return MyBase.SaveToRecordset(writer)
        End Function
 
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("ModuleName", Me.m_ModuleName)
            writer.WriteAttribute("AuthorizationName", Me.m_AuthorizationName)
            writer.WriteAttribute("AuthorizationDescription", Me.m_AuthorizationDescription)
            writer.WriteAttribute("Visible", Me.m_Visible)
            writer.WriteAttribute("ClassHandler", Me.m_ClassHandler)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "ModuleName" : m_ModuleName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AuthorizationName" : m_AuthorizationName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AuthorizationDescription" : m_AuthorizationDescription = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Visible" : m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ClassHandler" : m_ClassHandler = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class


End Class