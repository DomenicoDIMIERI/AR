Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases
    Public Class CDBQueriesCollection
        Inherits CKeyCollection(Of CDBQuery)

        Private m_Owner As CDBConnection

        Public Sub New()
        End Sub

        Public Sub New(ByVal connection As CDBConnection)
            Me.New()
            Me.Initialize(connection)
        End Sub

        Public ReadOnly Property Connection As CDBConnection
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CDBQuery).SetConnection(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CDBQuery).SetConnection(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Initialize(ByVal conn As CDBConnection)
            If (conn Is Nothing) Then Throw New ArgumentNullException("conn")

            MyBase.Clear()

            Me.m_Owner = conn
            ' We only want user tables, not system tables
            Dim restrictions() As String = {vbNullString, vbNullString, vbNullString, "View"}
            Dim oleDBConn As System.Data.OleDb.OleDbConnection = conn.GetConnection
            Dim userTables As DataTable = oleDBConn.GetSchema("Tables", restrictions)
            For Each dr As DataRow In userTables.Rows
                'System.Console.WriteLine(userTables.Rows(i)(2).ToString())
                Dim table As New CDBQuery
                table.Catalog = Formats.ToString(dr("TABLE_CATALOG"))
                table.Schema = Formats.ToString(dr("TABLE_SCHEMA"))
                table.Name = Formats.ToString(dr("TABLE_NAME"))
                table.Type = Formats.ToString(dr("TABLE_TYPE"))
                table.Guid = Formats.ToString(dr("TABLE_GUID"))
                table.Description = Formats.ToString(dr("DESCRIPTION"))
                table.PropID = Formats.ToString(dr("TABLE_PROPID"))
                table.DateCreated = Formats.ToDate(dr("DATE_CREATED"))
                table.DateModified = Formats.ToDate(dr("DATE_MODIFIED"))
                MyBase.Add(table.Name, table)
            Next
            userTables.Dispose()
        End Sub


        Protected Friend Overridable Sub SetOwner(ByVal owner As CDBConnection)
            Me.m_Owner = owner
            For Each table As CDBQuery In Me
                table.SetConnection(owner)
            Next
        End Sub
    End Class


End Class


