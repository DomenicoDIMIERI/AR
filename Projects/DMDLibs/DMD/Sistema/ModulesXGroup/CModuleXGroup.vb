Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Rappresenta l'assegnazioni di un modulo ad un gruppo di utenti
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CModuleXGroup
        Inherits DBObjectBase

        Private m_ModuleID As Integer
        Private m_Module As CModule
        Private m_GroupID As Integer
        Private m_Group As CGroup
        Private m_Allow As Boolean
        Private m_Negate As Boolean

        Public Sub New()
            Me.m_ModuleID = 0
            Me.m_Module = Nothing
            Me.m_GroupID = 0
            Me.m_Group = Nothing
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
        ''' Restituisce o imposta l'ID del gruppo associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GroupID As Integer
            Get
                Return GetID(Me.m_Group, Me.m_GroupID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.GroupID
                If (oldValue = value) Then Exit Property
                Me.m_GroupID = value
                Me.m_Group = Nothing
                Me.DoChanged("GroupID", value, oldValue)
            End Set
        End Property

        Public Property Group As CGroup
            Get
                If (Me.m_Group Is Nothing) Then Me.m_Group = Groups.GetItemById(Me.m_GroupID)
                Return Me.m_Group
            End Get
            Set(value As CGroup)
                If (Me.m_Group Is value) Then Exit Property
                Me.m_Group = value
                Me.m_GroupID = GetID(value)
                Me.DoChanged("Group", value)
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
            Return "tbl_ModulesXGroup"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Group", Me.m_GroupID)
            reader.Read("Module", Me.m_ModuleID)
            reader.Read("Allow", Me.m_Allow)
            reader.Read("Negate", Me.m_Negate)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Group", Me.GroupID)
            writer.Write("Module", Me.ModuleID)
            writer.Write("Allow", Me.m_Allow)
            writer.Write("Negate", Me.m_Negate)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Group", Me.GroupID)
            writer.WriteAttribute("Module", Me.ModuleID)
            writer.WriteAttribute("Allow", Me.m_Allow)
            writer.WriteAttribute("Negate", Me.m_Negate)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Group" : Me.m_GroupID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Module" : Me.m_ModuleID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Allow" : Me.m_Allow = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Negate" : Me.m_Negate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class


End Class