Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Rappresenta una autorizzazione o una negazione esplicita di un'azione ad uno specifico utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CUserAuthorization
        Inherits DBObjectBase

        Private m_ActionID As Integer
        Private m_Action As CModuleAction
        Private m_UserID As Integer
        Private m_User As CUser
        Private m_Allow As Boolean
        Private m_Negate As Boolean

        Public Sub New()
            Me.m_ActionID = 0
            Me.m_Action = Nothing
            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_Allow = False
            Me.m_Negate = False
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'azione associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActionID As Integer
            Get
                Return GetID(Me.m_Action, Me.m_ActionID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ActionID
                If (oldValue = value) Then Exit Property
                Me.m_ActionID = value
                Me.m_Action = Nothing
                Me.DoChanged("ActionID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'azione associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Action As CModuleAction
            Get
                If (Me.m_Action Is Nothing) Then Me.m_Action = Modules.DefinedActions.GetItemById(Me.m_ActionID)
                Return Me.m_Action
            End Get
            Set(value As CModuleAction)
                Dim oldValue As CModuleAction = Me.Action
                If (value Is oldValue) Then Exit Property
                Me.m_Action = value
                Me.m_ActionID = GetID(value)
                Me.DoChanged("Action", value)
            End Set
        End Property

        Protected Friend Sub SetAction(ByVal value As CModuleAction)
            Me.m_Action = value
            Me.m_ActionID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If (oldValue = value) Then Exit Property
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        Public Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue Is value) Then Exit Property
                Me.m_User = value
                Me.m_UserID = GetID(value)
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetUser(ByVal user As CUser)
            Me.m_User = user
            Me.m_UserID = GetID(user)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che consente esplicitamente l'azione all'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Allow As Boolean
            Get
                Return Me.m_Allow
            End Get
            Set(value As Boolean)
                If (Me.m_Allow = value) Then Exit Property
                Me.m_Allow = value
                Me.DoChanged("Allow", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che nega esplicitamente l'azione all'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Negate As Boolean
            Get
                Return Me.m_Negate
            End Get
            Set(value As Boolean)
                If (Me.m_Negate = value) Then Exit Property
                Me.m_Negate = value
                Me.DoChanged("Negate", value, Not value)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_UserAuthorizations"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_UserID = reader.Read("User", Me.m_UserID)
            Me.m_ActionID = reader.Read("Action", Me.m_ActionID)
            Me.m_Allow = reader.Read("Allow", Me.m_Allow)
            Me.m_Negate = reader.Read("Negate", Me.m_Negate)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("User", Me.UserID)
            writer.Write("Action", Me.ActionID)
            writer.Write("Allow", Me.m_Allow)
            writer.Write("Negate", Me.m_Negate)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("User", Me.UserID)
            writer.WriteAttribute("Action", Me.ActionID)
            writer.WriteAttribute("Allow", Me.m_Allow)
            writer.WriteAttribute("Negate", Me.m_Negate)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "User" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Action" : Me.m_ActionID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Allow" : Me.m_Allow = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Negate" : Me.m_Negate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class


End Class