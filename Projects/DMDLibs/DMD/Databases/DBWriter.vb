Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public NotInheritable Class DBWriter
        Implements IDisposable

        Private m_Connection As CDBConnection
        Private m_dbRis As System.Data.DataRow
        Private m_OldValues As New CKeyCollection
        Private m_Prefix As String = vbNullString
        Private m_Schema As CDBEntity

        Public Sub New(ByVal schema As CDBEntity, ByVal dbRis As System.Data.DataRow, Optional ByVal prefix As String = vbNullString)
            DMD.DMDObject.IncreaseCounter(Me)
            If (schema Is Nothing) Then Throw New ArgumentNullException("schema")
            If (dbRis Is Nothing) Then Throw New ArgumentNullException("dbRis")
            Me.m_Connection = schema.Connection
            Me.m_Schema = schema
            Me.m_dbRis = dbRis
            Me.m_Prefix = Trim(prefix)
            If (Me.Schema.TrackChanges) Then
                For Each c As System.Data.DataColumn In Me.m_dbRis.Table.Columns
                    Me.m_OldValues.Add(prefix & c.ColumnName, Me.m_dbRis.Item(prefix & c.ColumnName))
                Next
            End If
        End Sub

        Public ReadOnly Property Connection As CDBConnection
            Get
                Return Me.m_Connection
            End Get
        End Property

        Public ReadOnly Property Schema As CDBEntity
            Get
                Return Me.m_Schema
            End Get
        End Property

        Public Property Prefix As String
            Get
                Return Me.m_Prefix
            End Get
            Set(value As String)
                Me.m_Prefix = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Informa l'oggetto che il record è stato salvato
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Update(ByVal id As Integer)
            If (id = 0) Then Throw New ArgumentOutOfRangeException("id")
            If (Me.Schema IsNot Nothing) AndAlso (Me.Schema.TrackChanges) Then
                Dim data As Date = Now
                Dim changesFields As CCollection(Of PropertyChangedEventArgs) = Me.GetChangedFields
                Dim cmd As System.Data.IDbCommand = LOGConn.Tables("tbl_ChangesHistory").GetInsertCommand
                Dim userID As Integer = GetID(Users.CurrentUser)
                If (Me.CheckEquality(Me.m_dbRis("ID"), id) = False) Then
                    DirectCast(cmd.Parameters("@Utente"), System.Data.IDbDataParameter).Value = userID
                    DirectCast(cmd.Parameters("@DataModifica"), System.Data.IDbDataParameter).Value = data
                    DirectCast(cmd.Parameters("@Tabella"), System.Data.IDbDataParameter).Value = Me.m_Schema.Name
                    DirectCast(cmd.Parameters("@IDOggetto"), System.Data.IDbDataParameter).Value = id
                    DirectCast(cmd.Parameters("@NomeCampo"), System.Data.IDbDataParameter).Value = "ID"
                    DirectCast(cmd.Parameters("@OldValue"), System.Data.IDbDataParameter).Value = Me.m_dbRis("ID")
                    DirectCast(cmd.Parameters("@NewValue"), System.Data.IDbDataParameter).Value = id
                    cmd.ExecuteNonQuery()
                End If
                For Each p As PropertyChangedEventArgs In changesFields
                    DirectCast(cmd.Parameters("@Utente"), System.Data.IDbDataParameter).Value = userID
                    DirectCast(cmd.Parameters("@DataModifica"), System.Data.IDbDataParameter).Value = data
                    DirectCast(cmd.Parameters("@Tabella"), System.Data.IDbDataParameter).Value = Me.m_Schema.Name
                    DirectCast(cmd.Parameters("@IDOggetto"), System.Data.IDbDataParameter).Value = id
                    DirectCast(cmd.Parameters("@NomeCampo"), System.Data.IDbDataParameter).Value = p.PropertyName
                    DirectCast(cmd.Parameters("@OldValue"), System.Data.IDbDataParameter).Value = "" & Formats.ToString(p.OldValue)
                    DirectCast(cmd.Parameters("@NewValue"), System.Data.IDbDataParameter).Value = "" & Formats.ToString(p.NewValue)
                    cmd.ExecuteNonQuery()
                Next
                Me.m_OldValues.Clear()
                For Each c As System.Data.DataColumn In Me.m_dbRis.Table.Columns
                    Me.m_OldValues.Add(Me.Prefix & c.ColumnName, Me.m_dbRis.Item(Me.Prefix & c.ColumnName))
                Next
            End If
        End Sub

#Region "Write"

        Public Sub Write(ByVal fieldName As String, ByVal value As String)
            If Me.m_Schema IsNot Nothing Then
                Dim f As CDBEntityField = Me.m_Schema.Fields.GetItemByKey(fieldName)
                If (f Is Nothing) Then Throw New MissingFieldException(Me.m_Schema.Name, fieldName)
                If (value = vbNullString) Then
                    If f.AllowDBNull = False Then Throw New InvalidConstraintException("Il campo [" & Me.m_Schema.Name & "." & fieldName & "] non può essere NULL")
                    Me.m_dbRis(Me.m_Prefix & fieldName) = DBNull.Value
                Else
                    If f.MaxLength > 0 AndAlso Len(value) > f.MaxLength Then
                        Throw New ArgumentOutOfRangeException("Il campo [" & Me.m_Schema.Name & "." & fieldName & "] non può superare " & f.MaxLength & " caratteri")
                    End If
                End If
            End If
            Me.m_dbRis(Me.m_Prefix & fieldName) = value
        End Sub

        Public Sub Write(Of T)(ByVal fieldName As String, ByVal value As T)
            Me.m_dbRis(Me.m_Prefix & fieldName) = value
        End Sub

        Public Sub Write(ByVal fieldName As String, ByVal value As Integer())
            If value IsNot Nothing Then
                Me.m_dbRis(Me.m_Prefix & fieldName) = Arrays.IntegersToBytes(value)
            Else
                Me.m_dbRis(Me.m_Prefix & fieldName) = DBNull.Value
            End If
        End Sub

        Public Sub Write(Of T As Structure)(ByVal fieldName As String, ByVal value As Nullable(Of T))
            If value.HasValue Then
                Me.m_dbRis(Me.m_Prefix & fieldName) = value
            Else
                Me.m_dbRis(Me.m_Prefix & fieldName) = DBNull.Value
            End If
        End Sub

        Public Sub Write(ByVal fieldName As String, ByVal value As Array)
            If value Is Nothing Then
                Me.m_dbRis(Me.m_Prefix & fieldName) = DBNull.Value
            Else
                Me.m_dbRis(Me.m_Prefix & fieldName) = value
            End If
        End Sub

#End Region

        Public Function GetChangedFields() As CCollection(Of PropertyChangedEventArgs)
            Dim ret As New CCollection(Of PropertyChangedEventArgs)
            If (Me.Schema IsNot Nothing) AndAlso (Me.Schema.TrackChanges) Then
                For Each c As System.Data.DataColumn In Me.m_dbRis.Table.Columns
                    Dim value As Object = Me.m_dbRis(Me.m_Prefix & c.ColumnName)
                    Dim oldValue As Object = Me.m_OldValues(Me.m_Prefix & c.ColumnName)
                    If Me.CheckEquality(value, oldValue) = False Then ret.Add(New PropertyChangedEventArgs(Me.m_Prefix & c.ColumnName, value, oldValue))
                Next
            End If
            Return ret
        End Function

        Protected Function CheckEquality(ByVal a As Object, ByVal b As Object) As Boolean
            If TypeOf (a) Is DBNull Then
                If (TypeOf (b) Is DBNull) Then
                    Return True
                Else
                    Return False
                End If
            Else
                If (TypeOf (b) Is DBNull) Then
                    Return False
                Else
                    If TypeOf (a) Is ValueType Then
                        If TypeOf (b) Is ValueType Then
                            Return a.Equals(b)
                        Else
                            Return False
                        End If
                    Else
                        If TypeOf (b) Is ValueType Then
                            Return False
                        Else
                            If TypeOf (a) Is String AndAlso TypeOf (b) Is String Then
                                Return Strings.Compare(a, b, CompareMethod.Binary) = 0
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
            End If
        End Function

        ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.m_Connection = Nothing
            Me.m_dbRis = Nothing
            Me.m_OldValues = Nothing
            Me.m_Prefix = vbNullString
            Me.m_Schema = Nothing


        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class

