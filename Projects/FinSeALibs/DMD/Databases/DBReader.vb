Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases
    
    Public NotInheritable Class DBReader
        Implements IDisposable

        Private m_Schema As CDBEntity
        Private m_dbRis As System.Data.IDataReader
        Private m_Dt As System.Data.DataTable
        Private m_Index As Integer = -1
        Private m_Prefix As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal dbRis As System.Data.IDataReader)
            Me.New(dbRis, Nothing, "")
        End Sub

        Public Sub New(ByVal dbRis As System.Data.IDataReader, ByVal schema As CDBEntity)
            Me.New(dbRis, schema, "")
        End Sub

        Public Sub New(ByVal dbRis As System.Data.IDataReader, ByVal schema As CDBEntity, ByVal prefix As String)
            DMD.DMDObject.IncreaseCounter(Me)
            If (dbRis Is Nothing) Then Throw New ArgumentNullException("dbRis")
            Me.m_Schema = schema
            Me.m_dbRis = dbRis
            Me.m_Prefix = Trim(prefix)
        End Sub

        Public Sub New(ByVal schema As CDBEntity, ByVal dt As System.Data.DataTable)
            DMD.DMDObject.IncreaseCounter(Me)
            If (schema Is Nothing) Then Throw New ArgumentNullException("schema")
            If (dt Is Nothing) Then Throw New ArgumentNullException("dt")
            Me.m_Schema = schema
            Me.m_dbRis = Nothing
            Me.m_Dt = dt
            Me.m_Index = -1
            Me.m_Prefix = Trim(Prefix)
        End Sub


        Public Sub New(ByVal schema As CDBEntity)
            Me.New(schema, "SELECT * FROM " & schema.InternalName)
        End Sub

        Public Sub New(ByVal schema As CDBEntity, ByVal sql As String)
            Me.New(schema, sql, "")
        End Sub

        Public Sub New(ByVal schema As CDBEntity, ByVal sql As String, ByVal prefix As String)
            DMD.DMDObject.IncreaseCounter(Me)
            If (schema Is Nothing) Then Throw New ArgumentNullException("schema")
            sql = Trim(sql)
            If (sql = "") Then Throw New ArgumentNullException("sql")
            Me.m_Schema = schema
            Me.m_dbRis = schema.Connection.ExecuteReader(sql)
            Me.m_Prefix = Trim(prefix)
        End Sub




        ''' <summary>
        ''' Restituisce lo schema sottostante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Schema As CDBEntity
            Get
                Return Me.m_Schema
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che viene anteposta al nome dei campi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prefix As String
            Get
                Return Me.m_Prefix
            End Get
            Set(value As String)
                Me.m_Prefix = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Se il cursore non si trova oltre la fine del file carica il record corrente e restituisce vero altrimenti restituisce falso
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Read() As Boolean
            If (Me.m_Dt IsNot Nothing) Then
                If (Me.m_Index < Me.m_Dt.Rows.Count - 1) Then
                    Me.m_Index += 1
                    Return True
                Else
                    Return False
                End If
            Else
                Return Me.m_dbRis.Read
            End If
        End Function

#Region "Read"

        'Public Sub Read(ByVal fieldName As String, ByRef value As Boolean)
        '    value = Formats.ToBool(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Boolean))
        '    value = Formats.ParseBool(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Byte)
        '    value = Formats.ToInteger(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Byte))
        '    value = Formats.ParseInteger(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As SByte)
        '    value = Formats.ToInteger(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of SByte))
        '    value = Formats.ParseInteger(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Short)
        '    value = Formats.ToInteger(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Short))
        '    value = Formats.ParseInteger(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Integer)
        '    value = Formats.ToInteger(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Integer))
        '    value = Formats.ParseInteger(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Long)
        '    value = Formats.ToInteger(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Long))
        '    value = Formats.ParseInteger(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Single)
        '    value = Formats.ToDouble(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Single))
        '    value = Formats.ParseDouble(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Double)
        '    value = Formats.ToDouble(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Double))
        '    value = Formats.ParseDouble(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Decimal)
        '    value = Formats.ToValuta(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Decimal))
        '    value = Formats.ToValuta(Me.GetValue(fieldName))
        'End Sub

        'Public Sub Read(ByVal fieldName As String, ByRef value As Date)
        '    value = Formats.ToDate(Me.GetValue(fieldName))
        'End Sub
        'Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Date))
        '    value = Formats.Par(Me.GetValue(fieldName))
        'End Sub

        Public Function Read(Of T)(ByVal fieldName As String, ByRef value As T, Optional ByVal defValue As Object = Nothing) As T
            Dim tmp As Object = Me.GetValue(fieldName)
            If TypeOf (tmp) Is DBNull Then
                value = defValue
            Else
                value = CType(Me.GetValue(fieldName), T)
            End If
            Return value
        End Function

        Public Function Read(Of T As Structure)(ByVal fieldName As String, ByRef value As Nullable(Of T)) As Nullable(Of T)
            Dim tmp As Object = Me.GetValue(fieldName)
            If TypeOf (tmp) Is DBNull Then
                value = Nothing
            Else
                value = CType(Me.GetValue(fieldName), T)
            End If
            Return value
        End Function

        Public Function Read(ByVal fieldName As String, ByRef value As String) As String
            Dim tmp As Object = Me.GetValue(fieldName)
            If (TypeOf (tmp) Is DBNull) Then
                value = vbNullString
            Else
                value = CStr(tmp)
            End If
            Return value
        End Function

        Public Sub Read(ByVal fieldName As String, ByRef value As Array)
            value = Me.GetValue(fieldName)
        End Sub

        Public Function Read(ByVal fieldName As String, ByVal value As Integer()) As Integer()
            value = Nothing
            Dim tmp As Object = Me.m_dbRis(Me.GetFieldName(fieldName))
            If (Not (TypeOf (tmp) Is DBNull) AndAlso tmp IsNot Nothing) Then value = Sistema.Arrays.BytesToIntegers(DirectCast(tmp, Byte()))
            Return value
        End Function

        'Public Function Read(Of T)(ByVal fieldName As String) As T
        '    Return CType(Me.GetValue(fieldName), T)
        'End Function

#End Region

#Region "IsNull"

        ''' <summary>
        ''' Restituisce vero se il campo specificato contiene il valore DBNull
        ''' </summary>
        ''' <param name="fieldName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsNull(ByVal fieldName As String) As Boolean
            Return TypeOf (Me.GetValue(fieldName)) Is DBNull
        End Function

#End Region

#Region "GetValue"

        Protected Function GetFieldName(ByVal value As String) As String
            value = value.Replace("."c, "#"c)
            If (Me.m_Prefix = vbNullString) Then Return value
            Dim ret As New System.Text.StringBuilder
            ret.Append(Me.m_Prefix)
            ret.Append(value)
            Return ret.ToString
        End Function

        Public Function GetValue(ByVal fieldName As String) As Object
            If (Me.m_Dt IsNot Nothing) Then
                Return Me.m_Dt.Rows(Me.m_Index).Item(Me.GetFieldName(fieldName))
            Else
                Return Me.m_dbRis(Me.GetFieldName(fieldName))
            End If
        End Function

        Public Function GetValue(Of T As Structure)(ByVal fieldName As String) As Nullable(Of T)
            Dim ret As Object = Me.GetValue(fieldName)
            If (TypeOf (ret) Is DBNull) Then Return Nothing
            Return CType(ret, T)
        End Function

        Public Function GetValue(Of T)(ByVal fieldName As String, Optional ByVal defValue As Object = Nothing) As T
            Dim ret As Object = Me.GetValue(fieldName)
            If (TypeOf (ret) Is DBNull) Then Return defValue
            Return CType(ret, T)
        End Function

        Public Function GetValue(ByVal fieldName As String, ByVal defValue As String) As String
            Dim ret As Object = Me.GetValue(fieldName)
            If (TypeOf (ret) Is DBNull) Then Return defValue
            Return CStr(ret)
        End Function

#End Region




        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            If (Me.m_dbRis IsNot Nothing) Then Me.m_dbRis.Dispose() : Me.m_dbRis = Nothing
            If (Me.m_Dt IsNot Nothing) Then Me.m_Dt.Dispose() : Me.m_Dt = Nothing
            'If (Me.m_Schema IsNot Nothing) Then Me.m_Schema.Dispose() 
            Me.m_Schema = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class

