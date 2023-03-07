Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CDBConstraintsCollection
        Inherits CKeyCollection(Of CDBTableConstraint)

        Private m_Owner As CDBEntity

        Public Sub New()
        End Sub
        Public Sub New(ByVal owner As CDBEntity)
            Me.New()
            Me.Initialize(owner)
        End Sub

        Public ReadOnly Property Owner As CDBEntity
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetOwner(ByVal value As CDBEntity)
            Me.m_Owner = value
            For Each f As CDBTableConstraint In Me
                f.SetOwner(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CDBTableConstraint).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CDBTableConstraint).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Initialize(ByVal owner As CDBEntity)
            MyBase.Clear()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")

            Me.m_Owner = owner

            If (owner.Connection Is Nothing OrElse owner.IsCreated = False) Then Exit Sub
            'Dim ds As System.Data.DataSet
            'Dim dt As New System.Data.DataTable
            'Dim da As System.Data.IDataAdapter

            'da = owner.Connection.CreateAdapter("SELECT * FROM [" & owner.Connection.GetTableName(owner.Name) & "] WHERE (0<>0)")
            'ds = New System.Data.DataSet()
            'da.FillSchema(ds, SchemaType.Source)
            'dt = ds.Tables(0)
            'For Each dc As System.Data.Constraint In dt.Constraints
            '    Dim c As New CDBConstraint
            '    c.Name = dc.ConstraintName
            '    MyBase.Add(c.Name, c)
            'Next

            'dt.Dispose()
            'ds.Dispose()
            Dim restrictions() As String = {Nothing, Nothing, Nothing, Nothing, Me.m_Owner.Name}
            Dim indexDetails As DataTable = DirectCast(Me.m_Owner.Connection.GetConnection, System.Data.OleDb.OleDbConnection).GetSchema("INDEXES", restrictions)
            For Each row As DataRow In indexDetails.Rows
                Dim c As CDBTableConstraint
                Dim i As Integer = Me.IndexOfKey(Formats.ToString(row.Item("INDEX_NAME")))
                If (i >= 0) Then
                    c = Me(i)
                Else
                    c = New CDBTableConstraint
                    c.Name = Formats.ToString(row.Item("INDEX_NAME"))
                    Me.Add(c.Name, c)
                End If
                c.Columns.Add(Me.m_Owner.Fields(Formats.ToString(row.Item("COLUMN_NAME"))))
            Next

            'Dim columnsIWant = From row In indexDetails.AsEnumerable()
            'Select
            '    TableName = row.Field(Of String)("TABLE_NAME"),
            '    IndexName = row.Field(Of String)("INDEX_NAME"),
            '    ColumnOrdinal = row.Field(Of Int64)("ORDINAL_POSITION"),
            '    ColumnName = row.Field(Of String)("COLUMN_NAME")
            '    Order By TableName, IndexName, ColumnOrdinal, ColumnName
            indexDetails.Dispose()
        End Sub

    End Class

End Class


