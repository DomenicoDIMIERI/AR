Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization


Partial Public Class Databases

    <Serializable>
    Public Class CDBTablesCollection
        Inherits CKeyCollection(Of CDBTable)

        <NonSerialized> Private m_Owner As CDBConnection

        Public Sub New()
        End Sub

        Public Sub New(ByVal connection As CDBConnection)
            Me.New()
            Me.m_Owner = connection
        End Sub

        Public ReadOnly Property Connection As CDBConnection
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CDBTable).SetConnection(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CDBTable).SetConnection(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Shadows Sub Add(ByVal table As CDBTable)
            MyBase.Add(table.Name, table)
        End Sub

        Public Shadows Function Add(ByVal tableName As String) As CDBTable
            Dim item As New CDBTable(tableName)
            Me.Add(item)
            Return item
        End Function

        Protected Friend Overridable Sub SetOwner(ByVal owner As CDBConnection)
            Me.m_Owner = owner
            For Each table As CDBTable In Me
                table.SetConnection(owner)
            Next
        End Sub

    End Class

End Class


