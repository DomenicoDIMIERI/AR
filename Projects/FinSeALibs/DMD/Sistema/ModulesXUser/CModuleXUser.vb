Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Rappresenta l'assegnazioni di un modulo ad un utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CModuleXUser
        Inherits DBObjectBase

        Private m_ModuleID As Integer
        Private m_Module As CModule
        Private m_UserID As Integer
        Private m_User As CUser
        Private m_Allow As Boolean
        Private m_Negate As Boolean

        Public Sub New()
            Me.m_ModuleID = 0
            Me.m_Module = Nothing
            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_Allow = False
            Me.m_Negate = False
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'ID del modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ModuleID As Integer
            Get
                Return GetID(Me.m_Module, Me.m_ModuleID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ModuleID
                If (oldValue = value) Then Exit Property
                Me.m_ModuleID = value
                Me.m_Module = Nothing
                Me.DoChanged("ModuleID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property [Module] As CModule
            Get
                If (Me.m_Module Is Nothing) Then Me.m_Module = Modules.GetItemById(Me.m_ModuleID)
                Return Me.m_Module
            End Get
            Set(value As CModule)
                If (value Is Me.m_Module) Then Exit Property
                Me.m_Module = value
                Me.m_ModuleID = GetID(value)
                Me.DoChanged("Module", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente
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
                If (Me.m_User Is value) Then Exit Property
                Me.m_User = value
                Me.m_UserID = GetID(value)
                Me.DoChanged("User", value)
            End Set
        End Property

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
            Return "tbl_ModulesXUser"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("User", Me.m_UserID)
            reader.Read("Module", Me.m_ModuleID)
            reader.Read("Allow", Me.m_Allow)
            reader.Read("Negate", Me.m_Negate)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("User", Me.UserID)
            writer.Write("Module", Me.ModuleID)
            writer.Write("Allow", Me.m_Allow)
            writer.Write("Negate", Me.m_Negate)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("User", Me.UserID)
            writer.WriteAttribute("Module", Me.ModuleID)
            writer.WriteAttribute("Allow", Me.m_Allow)
            writer.WriteAttribute("Negate", Me.m_Negate)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "User" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Module" : Me.m_ModuleID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Allow" : Me.m_Allow = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Negate" : Me.m_Negate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class


End Class