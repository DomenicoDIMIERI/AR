Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema


    <Serializable> _
    Public NotInheritable Class CGroup
        Inherits DBObject
        Implements IComparable, ISupportsSingleNotes

        Private m_GroupName As String 'Nome del gruppo utente
        Private m_Description As String
        Private m_IsBuiltIn As Boolean
        <NonSerialized>  Private m_Members As CGroupMembersCollection
        <NonSerialized>  Private m_Authorizations As CGroupAuthorizationCollection
        <NonSerialized>  Private m_Modules As CModuleXGroupCollection

        Public Sub New()
            Me.m_GroupName = ""
            Me.m_Description = ""
            Me.m_IsBuiltIn = False
            Me.m_Members = Nothing
        End Sub

        Public Sub New(ByVal groupName As String)
            Me.New()
            Me.SetGroupName(Trim(groupName))
        End Sub


        ''' <summary>
        ''' Restituisce la collezione delle azioni consentite o negate esplicitamente al gruppo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Authorizations As CGroupAuthorizationCollection
            Get
                SyncLock Me
                    If (Me.m_Authorizations Is Nothing) Then Me.m_Authorizations = New CGroupAuthorizationCollection(Me)
                    Return Me.m_Authorizations
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei moduli esplicitamente autorizzati o negati per il gruppo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Modules As CModuleXGroupCollection
            Get
                If (Me.m_Modules Is Nothing) Then Me.m_Modules = New CModuleXGroupCollection(Me)
                Return Me.m_Modules
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return Sistema.Groups.Module
        End Function

        Public ReadOnly Property GroupName As String
            Get
                Return Me.m_GroupName
            End Get
        End Property
        Protected Friend Sub SetGroupName(ByVal value As String)
            Me.m_GroupName = Trim(value)
        End Sub

        Public Property Description As String Implements ISupportsSingleNotes.Notes
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

        Public Property IsBuiltIn() As Boolean
            Get
                Return Me.m_IsBuiltIn
            End Get
            Friend Set(value As Boolean)
                If (Me.m_IsBuiltIn = value) Then Exit Property
                Me.m_IsBuiltIn = value
                Me.DoChanged("IsBuiltIn", value, Not value)
            End Set
        End Property

        Public ReadOnly Property Members As CGroupMembersCollection
            Get
                SyncLock Me
                    If (Me.m_Members Is Nothing) Then Me.m_Members = New CGroupMembersCollection(Me)
                    Return Me.m_Members
                End SyncLock
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Gruppi"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_GroupName = reader.Read("GroupName", Me.m_GroupName)
            Me.m_Description = reader.Read("Description", Me.m_Description)
            Me.m_IsBuiltIn = reader.Read("BuiltIn", Me.m_IsBuiltIn)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("GroupName", Me.m_GroupName)
            writer.Write("Description", Me.m_Description)
            writer.Write("BuiltIn", Me.m_IsBuiltIn)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("GroupName", Me.m_GroupName)
            writer.WriteAttribute("IsBuiltIn", Me.m_IsBuiltIn)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Description", Me.m_Description)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "GroupName" : Me.m_GroupName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Description" : Me.m_Description = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IsBuiltIn" : Me.m_IsBuiltIn = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_GroupName
        End Function

        Public Function CompareTo(obj As CGroup) As Integer
            Return Strings.Compare(Me.GroupName, obj.GroupName, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
        End Sub


        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Sistema.Groups.UpdateCached(Me)
        End Sub

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Authorizations = Nothing
            Me.m_Members = Nothing
            Me.m_Modules = Nothing
        End Sub

    End Class

End Class
