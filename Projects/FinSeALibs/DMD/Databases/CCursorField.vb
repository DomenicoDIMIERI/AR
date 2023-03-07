Imports DMD.Sistema

Public partial class Databases



    Public Enum OP As Integer
        OP_NE = -3        'Not equal
        OP_LT = -2        'Less than
        OP_LE = -1        'Less than or equal
        OP_EQ = 0         'Equal
        OP_GE = 1         'Greater than or equal
        OP_GT = 2         'Greater than
        OP_LIKE = 3       'Like
        OP_NOTLIKE = 5      'Not Like
        OP_BETWEEN = 4    'Compreso tra
        OP_IN = 6
        OP_NOTIN = 7

        ''' <summary>
        ''' Confronto binario ( FieldValue BAND Value ) != 0
        ''' </summary>
        ''' <remarks></remarks>
        OP_ANYBITAND = 8

        ''' <summary>
        ''' Confronto binario ( FieldValue BAND Value ) == Value
        ''' </summary>
        ''' <remarks></remarks>
        OP_ALLBITAND = 9

        ''' <summary>
        ''' Confronto binario ( FieldValue BOR Value ) != 0
        ''' </summary>
        ''' <remarks></remarks>
        OP_ANYBITOR = 10

        ''' <summary>
        ''' Confronto binario ( FieldValue BOR Value ) == Value
        ''' </summary>
        ''' <remarks></remarks>
        OP_ALLBITOR = 11
    End Enum

    Public Enum SortEnum As Integer
        SORT_NOTSET = 0   'La colonna non fa parte dei criteri di ordinamento
        SORT_ASC = 1      'Colonna ordinata in senso crescente
        SORT_DESC = 2     'Colonna ordinata in senso decrescente
    End Enum

    Public Class CCursorField
        Implements ICopyObject, XML.IDMDXMLSerializable

        Private m_IsSet As Boolean
        Private m_Values() As Object
        Private m_FieldName As String
        Private m_DataType As adDataTypeEnum
        Private m_Operator As OP
        Private m_IncludeNulls As Boolean
        Private m_SortOrder As SortEnum
        Private m_SortPriority As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Operator = OP.OP_EQ
            Me.m_DataType = 0
            Me.m_Values = {DBNull.Value, DBNull.Value}
            Me.m_IsSet = False
            Me.m_IncludeNulls = False
            Me.m_SortPriority = -1
            Me.m_SortOrder = SortEnum.SORT_NOTSET
        End Sub

        Public Sub New(ByVal name As String, ByVal tType As adDataTypeEnum, Optional ByVal op As OP = OP.OP_EQ, Optional ByVal nullable As Boolean = False)
            Me.New()
            Me.m_FieldName = name
            Me.m_DataType = tType
            Me.m_Operator = op
            Me.m_IncludeNulls = nullable
        End Sub

        Public Property Value As Object
            Get
                Return Me.m_Values(0)
            End Get
            Set(value As Object)
                Me.m_Values(0) = Me.Convert(value)
                Me.m_IsSet = True
            End Set
        End Property

        Public Property Value1 As Object
            Get
                Return Me.m_Values(1)
            End Get
            Set(value As Object)
                Me.m_Values(1) = Me.Convert(value)
                Me.m_IsSet = True
            End Set
        End Property

        Public Sub ValueIn(ByVal values() As Object)
            If (values Is Nothing OrElse UBound(values) < 0) Then Throw New ArgumentNullException("values")
            If (UBound(values) = 0) Then
                Me.Operator = OP.OP_EQ
                Me.Value = values(0)
            Else
                Me.Operator = OP.OP_IN
                Me.m_Values = values
            End If
            Me.m_IsSet = True
        End Sub


        Private Function Convert(ByVal value As Object) As Object
            If (TypeOf (value) Is DBNull OrElse value Is Nothing) Then
                Return DBNull.Value
            End If
            Select Case Me.m_DataType
                Case adDataTypeEnum.adArray
                    If (TypeOf (value) Is Array) Then
                        Return value
                    Else
                        Return {value}
                    End If
                Case adDataTypeEnum.adBoolean : Return CBool(value)
                Case adDataTypeEnum.adChar, adDataTypeEnum.adWChar, adDataTypeEnum.adVarChar, adDataTypeEnum.adVarWChar : Return CStr(value)
                Case adDataTypeEnum.adBigInt : Return CLng(value)
                Case adDataTypeEnum.adCurrency, adDataTypeEnum.adDecimal : Return CDec(value)
                Case adDataTypeEnum.adDate, adDataTypeEnum.adDBDate, adDataTypeEnum.adDBTime, adDataTypeEnum.adDBTimeStamp : Return CDate(value)
                Case adDataTypeEnum.adDouble : Return CDbl(value)
                Case adDataTypeEnum.adInteger : Return CInt(value)
                Case adDataTypeEnum.adSingle : Return CSng(value)
                Case adDataTypeEnum.adSmallInt : Return CShort(value)
                Case adDataTypeEnum.adTinyInt : Return CSByte(value)
                Case adDataTypeEnum.adUnsignedBigInt : Return CULng(value)
                Case adDataTypeEnum.adUnsignedInt : Return CUInt(value)
                Case adDataTypeEnum.adUnsignedSmallInt : Return CUShort(value)
                Case adDataTypeEnum.adUnsignedTinyInt : Return CByte(value)
                Case Else : Return value
            End Select
        End Function

        Public Sub Between(ByVal a As Object, ByVal b As Object)
            If (Sistema.Types.IsNull(a)) Then
                If (Sistema.Types.IsNull(b)) Then
                    ReDim Me.m_Values(0)
                    Me.m_Values(0) = Nothing
                    Me.m_Operator = OP.OP_EQ
                Else
                    ReDim Me.m_Values(0)
                    Me.m_Values(0) = b
                    Me.m_Operator = OP.OP_LE
                End If
            ElseIf (Sistema.Types.IsNull(b)) Then
                ReDim Me.m_Values(0)
                Me.m_Values(0) = a
                Me.m_Operator = OP.OP_GE
            Else
                Me.m_Operator = OP.OP_BETWEEN
                Me.m_Values(0) = a
                Me.m_Values(1) = b
            End If
            Me.m_IsSet = True
        End Sub

        Public Function IsSet() As Boolean
            Return Me.m_IsSet
        End Function

        ''' <summary>
        ''' Restituisce vero se lo stato del campo è diverso da quello predefinito
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsChanged() As Boolean
            Return Me.IsSet OrElse Me.m_SortOrder <> SortEnum.SORT_NOTSET OrElse Me.m_SortPriority <> -1 OrElse Me.m_Operator <> OP.OP_EQ OrElse Me.m_IncludeNulls <> False
        End Function

        Public Function IsExpression(ByVal text As String) As Boolean
            Const chars As String = "([.])"
            For i As Integer = 1 To Len(chars)
                If (InStr(text, Mid(chars, i, 1)) > 0) Then Return True
            Next
            Return False
        End Function

        Public Sub Clear()
            Me.m_Values = New Object() {DBNull.Value, DBNull.Value}
            Me.m_IsSet = False
        End Sub

        Public Property [Operator] As OP
            Get
                Return Me.m_Operator
            End Get
            Set(value As OP)
                Me.m_Operator = value
            End Set
        End Property

        Public Property IncludeNulls As Boolean
            Get
                Return Me.m_IncludeNulls
            End Get
            Set(value As Boolean)
                Me.m_IncludeNulls = value
            End Set
        End Property

        Public Property DataType As adDataTypeEnum
            Get
                Return Me.m_DataType
            End Get
            Set(value As adDataTypeEnum)
                Me.m_DataType = value
            End Set
        End Property

        ReadOnly Property FieldName As String
            Get
                Return Me.m_FieldName
            End Get
        End Property

        Private Function GetOperatorString() As String
            Select Case Me.Operator
                Case OP.OP_NE : Return "<>"
                Case OP.OP_LT : Return "<"
                Case OP.OP_LE : Return "<="
                Case OP.OP_EQ : Return "="
                Case OP.OP_GE : Return ">="
                Case OP.OP_GT : Return ">"
                Case OP.OP_LIKE : Return "Like"
                Case OP.OP_NOTLIKE : Return "Not Like"
                Case OP.OP_BETWEEN : Return "Between"
                Case OP.OP_IN : Return "In"
                Case OP.OP_NOTIN : Return "Not In"
                Case OP.OP_ALLBITAND : Return " BAND "
                Case OP.OP_ANYBITAND : Return " BAND "
                Case OP.OP_ANYBITOR : Return " BOR "
                Case OP.OP_ALLBITOR : Return " BOR "
                Case Else : Return ""
            End Select
        End Function

        Private Function GetFieldNameInternal(ByVal name As String) As String
            If Me.IsExpression(name) Then
                Return name
            Else
                Return "[" & name & "]"
            End If
        End Function

        Public Property SortOrder As SortEnum
            Get
                Return Me.m_SortOrder
            End Get
            Set(value As SortEnum)
                Me.m_SortOrder = value
            End Set
        End Property

        Public Property SortPriority As Integer
            Get
                Return Me.m_SortPriority
            End Get
            Set(value As Integer)
                Me.m_SortPriority = value
            End Set
        End Property



        Public Overridable Function GetSQL() As String
            Return Me.GetSQL(Me.FieldName)
        End Function

        Public Overridable Function GetSQL(ByVal nomeCampoOverride As String) As String
            Dim opStr, ret As String
            Dim nomeCampo As String = Me.GetFieldNameInternal(nomeCampoOverride)
            ret = nomeCampo
            If (InStr(ret, "%OP%") > 0) And (InStr(ret, "%VALUE%") > 0) Then
                If Types.IsNull(Me.m_Values(0)) Then
                    Select Case (Me.m_Operator)
                        Case OP.OP_EQ : ret = nomeCampo & " Is Null"
                        Case OP.OP_NE : ret = "Not (" & nomeCampo & " Is Null)"
                        Case Else
                            Throw New InvalidOperationException("CCursorField: Operatore non valido su NULL")
                    End Select
                Else
                    opStr = Me.GetOperatorString
                    Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                        Case DBTypesEnum.DBNUMERIC_TYPE
                            If opStr = "Like" Then opStr = "="
                            If opStr = "Not Like" Then opStr = "<>"
                            ret = ""
                            ret = Replace(ret, "%OP%", opStr)
                            ret = Replace(ret, "%VALUE%", DBUtils.DBNumber(Me.m_Values(0)))
                        Case DBTypesEnum.DBDATETIME_TYPE
                            If opStr = "Between" Then
                                ret = Replace(ret, "%OP%", opStr)
                                ret = Replace(ret, "%VALUE%", DBUtils.DBDate(Me.m_Values(0)) & " AND " & DBUtils.DBDate(Me.m_Values(1)))
                            Else
                                If opStr = "Like" Then opStr = "="
                                If opStr = "Not Like" Then opStr = "<>"
                                ret = Replace(ret, "%OP%", opStr)
                                ret = Replace(ret, "%VALUE%", DBUtils.DBDate(Me.m_Values(0)))
                            End If
                        Case DBTypesEnum.DBBOOLEAN_TYPE
                            If opStr = "Like" Then opStr = "="
                            If opStr = "Not Like" Then opStr = "<>"
                            ret = Replace(ret, "%OP%", opStr)
                            ret = Replace(ret, "%VALUE%", DBUtils.DBBool(Me.m_Values(0)))
                        Case DBTypesEnum.DBTEXT_TYPE
                            If opStr = "Like" Then
                                ret = Replace(ret, "%OP%", opStr)
                                ret = Replace(ret, "%VALUE%", "'" & Replace(Me.m_Values(0), "'", "''") & "'")
                            ElseIf opStr = "Not Like" Then
                                ret = Replace(ret, "%OP%", "Like")
                                ret = "Not (" & Replace(ret, "%VALUE%", "'" & Replace(Me.m_Values(0), "'", "''") & "'") & ")"
                            Else
                                ret = Replace(ret, "%OP%", opStr)
                                ret = Replace(ret, "%VALUE%", DBUtils.DBString(Me.m_Values(0)))
                            End If
                        Case DBTypesEnum.DBBINARY_TYPE
                            Throw New InvalidOperationException("CCursorField: Tipo di dato binario non implementato nel cursore")
                        Case Else
                            Throw New InvalidOperationException("CCursorField: Tipo di dato non supportato nel cursore")
                    End Select
                    If Me.m_IncludeNulls Then ret &= " Or (" & Replace(Replace(nomeCampo, "%OP%", " Is "), "%VALUE%", " Null") & ")"
                End If
            Else
                Select Case Me.Operator
                    Case OP.OP_BETWEEN
                        Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                            'Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBBool(Me.m_Values(0)) & " AND " & DBUtils.DBBool(Me.m_Values(1))
                            Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " >= " & DBUtils.DBBool(Me.m_Values(0)) & " AND " & nomeCampo & " <= " & DBUtils.DBBool(Me.m_Values(1))
                                'Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBDate(Me.m_Values(0)) & " AND " & DBUtils.DBDate(Me.m_Values(1))
                            Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " >= " & DBUtils.DBDate(Me.m_Values(0)) & " AND " & nomeCampo & " <= " & DBUtils.DBDate(Me.m_Values(1))
                                'Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBNumber(Me.m_Values(0)) & " AND " & DBUtils.DBNumber(Me.m_Values(1))
                            Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " >= " & DBUtils.DBNumber(Me.m_Values(0)) & " AND " & nomeCampo & " <= " & DBUtils.DBNumber(Me.m_Values(1))
                                'Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBString(Me.m_Values(0)) & " AND " & DBUtils.DBString(Me.m_Values(1))
                            Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " >= " & DBUtils.DBString(Me.m_Values(0)) & " AND " & nomeCampo & " <= " & DBUtils.DBString(Me.m_Values(1))
                            Case Else
                                Throw New ArgumentException("Tipo di dato non supportato")
                        End Select
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_EQ
                        If Types.IsNull(Me.m_Values(0)) Then
                            ret = nomeCampo & " Is Null"
                        Else
                            Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                                Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " =" & DBUtils.DBBool(Me.m_Values(0))
                                Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " =" & DBUtils.DBDate(Me.m_Values(0))
                                Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " =" & DBUtils.DBNumber(Me.m_Values(0))
                                Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " =" & DBUtils.DBString(Me.m_Values(0))
                                Case Else
                                    Throw New ArgumentException("Tipo di dato non supportato")
                            End Select
                            If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                        End If
                    Case OP.OP_GE
                        Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                            Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " >=" & DBUtils.DBBool(Me.m_Values(0))
                            Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " >=" & DBUtils.DBDate(Me.m_Values(0))
                            Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " >=" & DBUtils.DBNumber(Me.m_Values(0))
                            Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " >=" & DBUtils.DBString(Me.m_Values(0))
                            Case Else
                                Throw New ArgumentException("Tipo di dato non supportato")
                        End Select
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_GT
                        Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                            Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " >" & DBUtils.DBBool(Me.m_Values(0))
                            Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " >" & DBUtils.DBDate(Me.m_Values(0))
                            Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " >" & DBUtils.DBNumber(Me.m_Values(0))
                            Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " >" & DBUtils.DBString(Me.m_Values(0))
                            Case Else
                                Throw New ArgumentException("Tipo di dato non supportato")
                        End Select
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_IN
                        Dim buffer As New System.Text.StringBuilder
                        buffer.Append(nomeCampo & " In (")

                        For i As Integer = 0 To UBound(Me.m_Values)
                            If (i > 0) Then buffer.Append(",")
                            Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                                Case DBTypesEnum.DBBOOLEAN_TYPE : buffer.Append(DBUtils.DBBool(Me.m_Values(i)))
                                Case DBTypesEnum.DBDATETIME_TYPE : buffer.Append(DBUtils.DBDate(Me.m_Values(i)))
                                Case DBTypesEnum.DBNUMERIC_TYPE : buffer.Append(DBUtils.DBNumber(Me.m_Values(i)))
                                Case DBTypesEnum.DBTEXT_TYPE : buffer.Append(DBUtils.DBString(Me.m_Values(i)))
                                Case Else
                                    Throw New ArgumentException("Tipo di dato non supportato")
                            End Select
                        Next
                        buffer.Append(")")
                        If (Me.m_IncludeNulls) Then buffer.Append(" Or " & nomeCampo & " Is Null")
                        ret = buffer.ToString
                    Case OP.OP_LE
                        Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                            Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " <=" & DBUtils.DBBool(Me.m_Values(0))
                            Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " <=" & DBUtils.DBDate(Me.m_Values(0))
                            Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " <=" & DBUtils.DBNumber(Me.m_Values(0))
                            Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " <=" & DBUtils.DBString(Me.m_Values(0))
                            Case Else
                                Throw New ArgumentException("Tipo di dato non supportato")
                        End Select
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_LIKE
                        Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                            Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " =" & DBUtils.DBBool(Me.m_Values(0))
                            Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " =" & DBUtils.DBDate(Me.m_Values(0))
                            Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " =" & DBUtils.DBNumber(Me.m_Values(0))
                            Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " LIKE " & DBUtils.DBString(Me.m_Values(0))
                            Case Else
                                Throw New ArgumentException("Tipo di dato non supportato")
                        End Select
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_LT
                        Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                            Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " <" & DBUtils.DBBool(Me.m_Values(0))
                            Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " <" & DBUtils.DBDate(Me.m_Values(0))
                            Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " <" & DBUtils.DBNumber(Me.m_Values(0))
                            Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " <" & DBUtils.DBString(Me.m_Values(0))
                            Case Else
                                Throw New ArgumentException("Tipo di dato non supportato")
                        End Select
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_NE
                        If Types.IsNull(Me.m_Values(0)) Then
                            ret = nomeCampo & " Is Not Null"
                        Else
                            Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                                Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " <>" & DBUtils.DBBool(Me.m_Values(0))
                                Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " <>" & DBUtils.DBDate(Me.m_Values(0))
                                Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " <>" & DBUtils.DBNumber(Me.m_Values(0))
                                Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " <>" & DBUtils.DBString(Me.m_Values(0))
                                Case Else
                                    Throw New ArgumentException("Tipo di dato non supportato")
                            End Select
                        End If
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_NOTIN
                        ret = nomeCampo & " Not In ("
                        For i As Integer = 0 To UBound(Me.m_Values)
                            If (i > 0) Then ret &= ","
                            Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                                Case DBTypesEnum.DBBOOLEAN_TYPE : ret &= DBUtils.DBBool(Me.m_Values(i))
                                Case DBTypesEnum.DBDATETIME_TYPE : ret &= DBUtils.DBDate(Me.m_Values(i))
                                Case DBTypesEnum.DBNUMERIC_TYPE : ret &= DBUtils.DBNumber(Me.m_Values(i))
                                Case DBTypesEnum.DBTEXT_TYPE : ret &= DBUtils.DBString(Me.m_Values(i))
                                Case Else
                                    Throw New ArgumentException("Tipo di dato non supportato")
                            End Select
                        Next
                        ret &= ")"
                        If (Me.m_IncludeNulls) Then ret &= " Or " & nomeCampo & " Is Null"
                    Case OP.OP_NOTLIKE
                        Select Case DBUtils.GetDataTypeFamily(Me.m_DataType)
                            Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " <>" & DBUtils.DBBool(Me.m_Values(0))
                            Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " <>" & DBUtils.DBDate(Me.m_Values(0))
                            Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " <>" & DBUtils.DBNumber(Me.m_Values(0))
                            Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " Not LIKE " & DBUtils.DBString(Me.m_Values(0))
                            Case Else
                                Throw New ArgumentException("Tipo di dato non supportato")
                        End Select
                        If (Me.m_IncludeNulls) Then ret = ret & " Or " & nomeCampo & " Is Null"
                    Case OP.OP_ANYBITAND
                        ret = "((" & nomeCampo & " BAND " & DBUtils.DBNumber(Me.m_Values(0)) & ") <> 0)"
                        If (Me.m_IncludeNulls) Then ret = ret & " Or (" & nomeCampo & " Is Null)"
                    Case OP.OP_ALLBITAND
                        ret = "((" & nomeCampo & " BAND " & DBUtils.DBNumber(Me.m_Values(0)) & ") = " & DBUtils.DBNumber(Me.m_Values(0)) & ")"
                        If (Me.m_IncludeNulls) Then ret = ret & " Or (" & nomeCampo & " Is Null)"
                    Case OP.OP_ANYBITOR
                        ret = "((" & nomeCampo & " BOR " & DBUtils.DBNumber(Me.m_Values(0)) & ") <> 0)"
                        If (Me.m_IncludeNulls) Then ret = ret & " Or (" & nomeCampo & " Is Null)"
                    Case OP.OP_ALLBITOR
                        ret = "((" & nomeCampo & " BOR " & DBUtils.DBNumber(Me.m_Values(0)) & ") = " & DBUtils.DBNumber(Me.m_Values(0)) & ")"
                        If (Me.m_IncludeNulls) Then ret = ret & " Or (" & nomeCampo & " Is Null)"
                    Case Else
                        Throw New ArgumentOutOfRangeException("Operator")
                End Select
            End If


            Return ret
        End Function

        Friend Function CopyFrom(ByVal field As Object) As Object Implements ICopyObject.CopyFrom
            With DirectCast(field, CCursorField)
                Me.m_FieldName = .FieldName
                Me.m_DataType = .DataType
                Me.m_Operator = .Operator
                Me.m_IncludeNulls = .IncludeNulls
                Me.m_SortOrder = .SortOrder
                Me.m_SortPriority = .SortPriority
                Me.m_Values = .m_Values.Clone
                '                Me.m_Value1 = .Value1
                Me.m_IsSet = .IsSet
            End With
            Return Me
        End Function

        Public Function InitFrom(ByVal field As Object) As Object
            With DirectCast(field, CCursorField)
                Me.m_DataType = .DataType
                Me.m_Operator = .Operator
                Me.m_IncludeNulls = .IncludeNulls
                Me.m_SortOrder = .SortOrder
                Me.m_SortPriority = .SortPriority
                Me.m_Values = .m_Values.Clone
                '                Me.m_Value1 = .Value1
                Me.m_IsSet = .IsSet
            End With
            Return Me
        End Function


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IsSet" : Me.m_IsSet = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "FieldName" : Me.m_FieldName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataType" : Me.m_DataType = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Operator" : Me.m_Operator = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IncludeNulls" : Me.m_IncludeNulls = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "SortOrder" : Me.m_SortOrder = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SortPriority" : Me.m_SortPriority = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Values" : Me.m_Values = Me.parseFieldValues(fieldValue)

            End Select
        End Sub

        Protected Overridable Function parseFieldValues(ByVal value As Object) As Object()
            If (Not IsArray(value)) Then
                value = {value}
            End If
            Dim ret As Object() = value

            Select Case Me.m_DataType
                Case adDataTypeEnum.adBoolean
                    For i As Integer = 0 To UBound(ret)
                        ret(i) = Formats.ParseBool(ret(i))
                    Next
                Case adDataTypeEnum.adInteger, adDataTypeEnum.adBigInt, adDataTypeEnum.adSmallInt, adDataTypeEnum.adTinyInt
                    For i As Integer = 0 To UBound(ret)
                        ret(i) = Formats.ParseInteger(ret(i))
                    Next
                Case adDataTypeEnum.adDouble, adDataTypeEnum.adSingle
                    For i As Integer = 0 To UBound(ret)
                        ret(i) = Formats.ParseDouble(ret(i))
                    Next
                Case adDataTypeEnum.adDate
                    For i As Integer = 0 To UBound(ret)
                        ret(i) = Formats.ParseDate(ret(i))
                    Next
                Case adDataTypeEnum.adWChar, adDataTypeEnum.adChar, adDataTypeEnum.adVarChar, adDataTypeEnum.adVarWChar
                    For i As Integer = 0 To UBound(ret)
                        ret(i) = Formats.ToString(ret(i))
                    Next
            End Select
            Return ret
        End Function

        Protected Overridable Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IsSet", Me.m_IsSet)
            writer.WriteAttribute("FieldName", Me.m_FieldName)
            writer.WriteAttribute("DataType", Me.m_DataType)
            writer.WriteAttribute("Operator", Me.m_Operator)
            writer.WriteAttribute("IncludeNulls", Me.m_IncludeNulls)
            writer.WriteAttribute("SortOrder", Me.m_SortOrder)
            writer.WriteAttribute("SortPriority", Me.m_SortPriority)
            writer.WriteTag("Values", Me.m_Values)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class
