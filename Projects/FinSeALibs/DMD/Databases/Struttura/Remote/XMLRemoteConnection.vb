Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class XMLRemoteConnection
        Inherits CDBConnection

        Private m_Entry As String

        Public Sub New()
            Me.m_Entry = ""
        End Sub

        Public Sub New(ByVal entry As String)
            Me.m_Entry = entry
        End Sub

        Public Overrides Function GetItemById(m As CModule, id As Integer) As Object
            If (id = 0) Then Return Nothing
            If (m Is Nothing) Then Throw New ArgumentNullException("m")
            If (m.ModuleName = "") Then Throw New ArgumentNullException("module name")
            Dim tmp As String = RPC.InvokeMethod(Me.m_Entry & "/websvc/?_m=" & m.ModuleName & "&_a=GetItemById", "id", RPC.int2n(id))
            Dim ret As Object = Nothing
            If (tmp <> "") Then ret = XML.Utils.Serializer.Deserialize(tmp)
            Return ret
        End Function

        Protected Friend Overrides Sub CreateField(field As CDBEntityField)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub CreateFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub CreateInternal()
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub CreateTable(table As CDBTable)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub CreateTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub CreateView(view As CDBQuery)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub DropField(field As CDBEntityField)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub DropFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub DropTable(table As CDBTable)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub DropTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub DropView(view As CDBQuery)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub UpdateField(field As CDBEntityField)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub UpdateFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub UpdateTable(table As CDBTable)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub UpdateTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException()
        End Sub

        Protected Friend Overrides Sub UpdateView(vuew As CDBQuery)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Function CreateConnection() As IDbConnection
            Throw New NotImplementedException()
        End Function

        Public Overrides Function GetInsertCommand(obj As Object, idFieldName As String, dr As DataRow, ByRef Optional maxID As Integer = 0) As IDbCommand
            Throw New NotImplementedException()
        End Function

        Public Overrides Function GetSqlDataType(field As CDBEntityField) As String
            Throw New NotImplementedException()
        End Function

        Public Overrides Function GetUpdateCommand(obj As Object, idFieldName As String, idValue As Integer, dr As DataRow, changedValues As CKeyCollection(Of Boolean)) As IDbCommand
            Throw New NotImplementedException()
        End Function

        Protected Overrides Function GetTablesArray() As CDBTable()
            Throw New NotImplementedException()
        End Function

        Protected Friend Overrides Function CreateAdapterInternal(selectCommand As String) As IDataAdapter
            Throw New NotImplementedException()
        End Function

        Protected Friend Overrides Function CreateCommandInternal(sql As String) As IDbCommand
            Throw New NotImplementedException()
        End Function

        Public Overrides Sub SaveObject(obj As Object, force As Boolean)
            SyncLock Me
                Dim o As DBObjectBase = obj
                Dim isNew As Boolean = (Databases.GetID(o) = 0)
                Dim value As String = XML.Utils.Serializer.Serialize(o)
                Dim Type As String = Types.vbTypeName(o)
                Dim ret As String = RPC.InvokeMethod("/websvc/?_m=Sistema&_a=SaveObject", "type", RPC.str2n(Type), "value", RPC.str2n(value))
                Dim tmp As Integer = RPC.ParseID(Strings.Left(ret, 8))
                If (tmp <> 0) Then
                    If (isNew) Then
                        DBUtils.SetID(o, tmp)
                    End If
                Else
                    Throw New Exception(ret)
                End If
                o.SetChanged(False)
            End SyncLock
        End Sub

        Public Overrides Sub DeleteObject(obj As Object, force As Boolean)
            SyncLock Me

            End SyncLock
        End Sub

        Public Overrides Function IsRemote() As Boolean
            Return True
        End Function

        Public Overrides Function InvokeMethodArray([module] As CModule, methodName As String, args() As Object) As String
            If ([module] Is Nothing) Then Throw New ArgumentNullException("module")
            If ([module].ModuleName = "") Then Throw New ArgumentNullException("module name")
            Return RPC.InvokeMethod1(Me.m_Entry & "/websvc/?_m=" & [module].ModuleName & "&_a=" & methodName, args)
        End Function

        Public Overrides Function InvokeMethodArrayAsync([module] As CModule, methodName As String, handler As Object, args() As Object) As AsyncState
            If ([module] Is Nothing) Then Throw New ArgumentNullException("module")
            If ([module].ModuleName = "") Then Throw New ArgumentNullException("module name")
            Return RPC.InvokeMethodArrayAsync(Me.m_Entry & "/websvc/?_m=" & [module].ModuleName & "&_a=" & methodName, handler, args)
        End Function
    End Class


End Class


