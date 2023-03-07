Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases


    <Serializable>
    Public Class CDBEntityFields
        Inherits CKeyCollection(Of CDBEntityField)

        <NonSerialized> Private m_Owner As CDBEntity

        Public Sub New()
            Me.Comparer = New CStringComparer(CompareMethod.Text)
        End Sub

        Public Sub New(ByVal entity As CDBEntity)
            Me.New()
            Me.GetFields(entity)
        End Sub

        Public ReadOnly Property Owner As CDBEntity
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetOwner(ByVal value As CDBEntity)
            Me.m_Owner = value
            For Each f As CDBEntityField In Me
                f.SetOwner(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CDBEntityField).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CDBEntityField).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub GetFields(ByVal entity As CDBEntity)
            If (entity Is Nothing) Then Throw New ArgumentNullException("entity")

            MyBase.Clear()

            Me.m_Owner = entity
            If (entity.Connection Is Nothing OrElse Not entity.IsCreated) Then Exit Sub

            Dim ds As System.Data.DataSet
            Dim dt As New System.Data.DataTable
            Dim da As System.Data.IDataAdapter


            da = entity.Connection.CreateAdapter("SELECT * FROM " & entity.InternalName & " WHERE (0<>0)")
            ds = New System.Data.DataSet()
            da.FillSchema(ds, SchemaType.Source)

            If ds.Tables.Count > 0 Then
                dt = ds.Tables(0)

                For Each dc As System.Data.DataColumn In dt.Columns
                    Dim field As New CDBEntityField
                    field.DataType = dc.DataType
                    field.Name = dc.ColumnName
                    field.MaxLength = dc.MaxLength
                    field.AutoIncrement = dc.AutoIncrement
                    field.AutoIncrementSeed = dc.AutoIncrementSeed
                    field.AutoIncrementStep = dc.AutoIncrementStep
                    field.AllowDBNull = dc.AllowDBNull
                    field.Caption = dc.Caption
                    field.ColumnMapping = dc.ColumnMapping
                    field.DefaultValue = dc.DefaultValue
                    field.Expression = dc.Expression
                    field.Namespace = dc.Namespace
                    field.SetOrdinal(dc.Ordinal)
                    field.Prefix = dc.Prefix
                    field.ReadOnly = dc.ReadOnly
                    field.Unique = dc.Unique
                    field.SetChanged(False)
                    field.SetCreated(True)
                    MyBase.Add(field.Name, field)
                Next

                dt.Dispose()
            End If

            ds.Dispose()

        End Sub

        Public Shadows Sub Add(ByVal item As CDBEntityField)
            MyBase.Add(item.Name, item)
        End Sub

        Public Shadows Function Add(ByVal fieldName As String, ByVal fieldType As System.Type) As CDBEntityField
            Dim field As New CDBEntityField(fieldName, fieldType)
            Me.Add(field)
            Return field
        End Function

        Public Shadows Function Add(ByVal fieldName As String, ByVal fieldType As System.TypeCode) As CDBEntityField
            Dim field As New CDBEntityField(fieldName, fieldType)
            Me.Add(field)
            Return field
        End Function

    End Class


End Class


