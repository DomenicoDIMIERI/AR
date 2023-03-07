Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Internals
Imports DMD.Databases

Namespace Internals


    Public NotInheritable Class CDBUtilsClass
        Public Event ConnectionOpened(ByVal sender As Object, ByVal e As DBEventArgs)
        Public Event ConnectionClosed(ByVal sender As Object, ByVal e As DBEventArgs)
        Public Event ConnectionError(ByVal sender As Object, ByVal e As DBEventArgs)
        Public Event CursorOpened(ByVal sender As Object, ByVal e As DBCursorEventArgs)
        Public Event CursorClosed(ByVal sender As Object, ByVal e As DBCursorEventArgs)

        Private lockObject As New Object
        Private m_OpenedConnections As New CCollection(Of CDBConnection)
        Private Shared m_StopStatistics As Boolean = True
        Private m_PendingQueries As New CCollection(Of StatsItem)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Property StopStatistics As Boolean
            Get
                Return m_StopStatistics
            End Get
            Set(value As Boolean)
                m_StopStatistics = value
            End Set
        End Property

        Function GetAllOpenedConnections() As CCollection(Of CDBConnection)
            SyncLock Me.lockObject
                Return New CCollection(Of CDBConnection)(Me.m_OpenedConnections)
            End SyncLock
        End Function


        Friend Sub doConnectionOpened(ByVal e As DBEventArgs)
            SyncLock Me.lockObject
                m_OpenedConnections.Add(e.Connection)
            End SyncLock
            RaiseEvent ConnectionOpened(Nothing, e)
        End Sub

        Friend Sub doConnectionClosed(ByVal e As DBEventArgs)
            SyncLock Me.lockObject
                m_OpenedConnections.Remove(e.Connection)
            End SyncLock
            RaiseEvent ConnectionClosed(Nothing, e)
        End Sub

        Friend Sub doConnectionError(ByVal e As DBEventArgs)
            RaiseEvent ConnectionError(Nothing, e)
        End Sub

        Friend Sub doCursorOpened(ByVal e As DBCursorEventArgs)
            RaiseEvent CursorOpened(Nothing, e)
        End Sub

        Friend Sub doCursorClosed(ByVal e As DBCursorEventArgs)
            RaiseEvent CursorClosed(Nothing, e)
        End Sub


#Region "Conversion"

        Public Function MakeArrayStr(ByVal items() As Integer, Optional ByVal separator As String = ",") As String
            Dim ret As New System.Text.StringBuilder
            For i = 0 To UBound(items)
                If (i > 0) Then ret.Append(separator)
                ret.Append(DBNumber(items(i)))
            Next
            Return ret.ToString
        End Function

        Public Function MakeArrayStr(ByVal items() As Double, Optional ByVal separator As String = ",") As String
            Dim ret As New System.Text.StringBuilder
            For i = 0 To UBound(items)
                If (i > 0) Then ret.Append(separator)
                ret.Append(DBNumber(items(i)))
            Next
            Return ret.ToString
        End Function

        Public Function MakeArrayStr(ByVal items() As String, Optional ByVal separator As String = ",") As String
            Dim ret As New System.Text.StringBuilder
            For i = 0 To UBound(items)
                If (i > 0) Then ret.Append(separator)
                ret.Append(DBString(items(i)))
            Next
            Return ret.ToString
        End Function

        Public Function MakeArrayStr(ByVal items() As Boolean, Optional ByVal separator As String = ",") As String
            Dim ret As New System.Text.StringBuilder
            For i = 0 To UBound(items)
                If (i > 0) Then ret.Append(separator)
                ret.Append(DBBool(items(i)))
            Next
            Return ret.ToString
        End Function

        Public Function MakeArrayStr(ByVal items() As Date, Optional ByVal separator As String = ",") As String
            Dim ret As New System.Text.StringBuilder
            For i = 0 To UBound(items)
                If (i > 0) Then ret.Append(separator)
                ret.Append(DBDate(items(i)))
            Next
            Return ret.ToString
        End Function

        ''' <summary>
        ''' Restituisce il valore dell'oggetto o DBNull se l'oggetto è vuoto
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToDB(Of T As Structure)(ByVal value As Nullable(Of T)) As Object
            If value.HasValue Then Return value.Value
            Return DBNull.Value
        End Function

        ''' <summary>
        ''' Restituisce il valore della stringa o DBNull se la stringa è nulla
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToDB(ByVal value As String) As Object
            Return value
        End Function

        Public Function FromDB(Of T As Structure)(ByVal value As Object) As Nullable(Of T)
            If TypeOf (value) Is DBNull Then Return Nothing
            Return CType(value, T)
        End Function

        Public Function FromDB(ByVal value As Object) As String
            If TypeOf (value) Is DBNull Then Return Nothing
            Return CStr(value)
        End Function

#End Region


#Region "DBDate"

        ''' <summary>
        ''' Restituisce la data nel formato MM/DD/YYYY HH.NN.SS
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DBDate(ByVal value As Object) As String
            If (TypeOf (value) Is DBNull OrElse value Is Nothing) Then
                Return "NULL"
            ElseIf (TypeOf (value) Is Nullable(Of Date)) AndAlso (DirectCast(value, Nullable(Of Date)).HasValue = False) Then
                Return "NULL"
            ElseIf (TypeOf (value) Is NDate) AndAlso (DirectCast(value, NDate).HasValue = False) Then
                Return "NULL"
            Else
                Dim d As Date = value
                Return Strings.JoinW("#", Month(d), "/", Day(d), "/", Year(d), " ", Hour(d), ":", Minute(d), ":", Second(d), "#")
            End If
        End Function

#End Region

        ''' <summary>
        '''  Restituisce il formato SQL del numero
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DBNumber(ByVal value As Object) As String
            If (TypeOf (value) Is DBNull) OrElse (value Is Nothing) Then
                Return "NULL"
            Else
                Return Trim(Replace(CStr(value), ",", "."))
            End If
        End Function



        ' ''' <summary>
        ' ''' Funzione utilizzata dagli oggetti per salvare i dati in una tabella
        ' ''' </summary>
        ' ''' <param name="dbConn"></param>
        ' ''' <param name="obj"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public  Function SaveToDatabase( _
        '                        ByVal dbConn As System.Data.IDbConnection, _
        '                        ByVal obj As Object, _
        '                        Optional ByVal idField As String = "ID" _
        '                        ) As Integer
        '    If TypeOf (obj) Is IDBObjectBase Then
        '        Return SaveToDatabaseObj(dbConn, obj, idField)
        '    ElseIf TypeOf (obj) Is IDBMinObject Then
        '        Return DirectCast(obj, IDBMinObject).SaveToDatabase(dbConn)
        '    Else
        '        Throw New ArgumentException("L'oggetto non implementa alcuna interfaccia valida")
        '        Return False
        '    End If
        'End Function








        Public Function PrepareWhereClause(ByVal fieldName As String, ByVal searchString As String, ByVal matchExact As String) As String
            ' Dim ret As String
            Dim i, j As Integer
            Dim operands() As String
            Dim operators() As String
            Dim status As Integer
            Dim ch As String

            searchString = Trim(searchString)

            i = 1
            ReDim operands(0)
            ReDim operators(0)
            status = 0

            operands(0) = ""
            operators(0) = ""

            While (i <= Len(searchString))
                ch = Mid(searchString, i, 1)

                Select Case status
                    Case 0
                        Select Case ch
                            Case Chr(34) 'Inizio stringa
                                status = 1
                                If (i > 1) Then
                                    operators(UBound(operators)) = "+"
                                    ReDim Preserve operators(1 + UBound(operators))
                                    ReDim Preserve operands(1 + UBound(operands))
                                    operands(UBound(operands)) = ""

                                End If
                            Case "&"    'Operatore logico AND
                                operators(UBound(operators)) = "*"
                                ReDim Preserve operators(1 + UBound(operators))
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ""

                            Case "|"
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ""

                            Case " "
                                status = 2

                            Case Else
                                operands(UBound(operands)) = operands(UBound(operands)) & ch
                        End Select
                    Case 1
                        If ch = Chr(34) Then
                            status = 0
                        Else
                            operands(UBound(operands)) = operands(UBound(operands)) & ch
                        End If
                    Case 2
                        Select Case ch
                            Case " "
                            Case "|"
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                status = 3
                            Case "&"
                                operators(UBound(operators)) = "*"
                                ReDim Preserve operators(1 + UBound(operators))
                                status = 3
                            Case Chr(34)
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                status = 1
                            Case Else
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ch
                                status = 0
                        End Select
                    Case 3
                        Select Case ch
                            Case " "
                            Case Else
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ch
                                status = 0
                        End Select
                End Select

                i = i + 1
            End While

            For i = 0 To UBound(operands)
                If matchExact Then
                    operands(i) = Strings.JoinW("[", fieldName, "]='", Replace(operands(i), "'", "''"), "'")
                Else
                    operands(i) = Strings.JoinW("[", fieldName, "] Like '%", Replace(operands(i), "'", "''"), "%'")
                End If
            Next

            i = UBound(operands)
            For j = UBound(operators) - 1 To 0 Step -1
                Select Case operators(j)
                    Case "+"
                        If UBound(operands) >= i Then
                            operands(i - 1) = Strings.JoinW("(", operands(i - 1), ") Or (", operands(i), ")")
                            i = i - 1
                        Else
                            operands(i - 1) = operands(i)
                        End If
                    Case "*"
                        If UBound(operands) >= i Then
                            operands(i - 1) = Strings.JoinW("(", operands(i - 1), ") And (", operands(i), ")")
                            i = i - 1
                        Else
                            operands(i - 1) = operands(i)
                        End If
                    Case Else
                End Select
            Next



            Return operands(0)
        End Function



        Public Sub ResetID(ByVal item As Object)
            DirectCast(item, IDBObjectBase).ResetID()
        End Sub

        Public Sub SetID(ByVal item As Object, ByVal id As Integer)
            DirectCast(item, IDBObjectBase).SetID(id)
        End Sub



        ''' <summary>
        ''' Restituisce una stringa utilizzabile in una istruzione SQL (sostituisce ' con '')
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DBString(ByVal value As Object) As String
            If (TypeOf (value) Is DBNull) OrElse (value Is Nothing) Then
                Return "NULL"
            Else
                Return Strings.JoinW("'", Replace(CStr(value), "'", "''"), "'")
            End If
        End Function



        ''' <summary>
        ''' Restituisce il formato SQL del valore booleano
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DBBool(ByVal value As Object) As String
            If TypeOf (value) Is DBNull Then Return "NULL"
            Return IIf(CBool(value), "True", "False")
        End Function




        Public Function OpenDatabase(ByVal dbType As String, ByVal dbPath As String, Optional ByVal userName As String = vbNullString, Optional ByVal password As String = vbNullString) As CDBConnection
            Dim ret As CDBConnection
            Select Case LCase(Trim(dbType))
                Case "mdb" : ret = New COleDBConnection
                Case "xls" : ret = New CXlsDBConnection
                Case Else : Throw New NotSupportedException
            End Select
            ret.Path = dbPath
            ret.SetCredentials(userName, password)
            ret.OpenDB()
            Return ret
        End Function



        Public Sub CompactDB(ByVal dbPath As String)
            ''jro = Nothing
            'Dim jro As JRO.JetEngine
            'Dim tmp As String = Sistema.FileSystem.GetFolderName(dbPath)
            'tmp & = "\tmpDB.tmp"
            'Sistema.FileSystem.DeleteFile(tmp, True)

            'jro = New JRO.JetEngine()

            'jro.CompactDatabase("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbPath, "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" 
            '& tmp &
            '";Jet OLEDB:Engine Type=5")
            'Sistema.FileSystem.DeleteFile(dbPath, True)
            'Sistema.FileSystem.MoveFile(tmp, dbPath)

        End Sub

        Public Sub CompactDB(ByVal dbPath As String, ByVal target As String)
            'Dim jro As JRO.JetEngine
            'jro = New JRO.JetEngine()

            'jro.CompactDatabase("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbPath, "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" '
            '& target 
            '& 
            '";Jet OLEDB:Engine Type=5")
        End Sub

        Public Function PrepareWhereClause(ByVal fieldName As String, ByVal searchString As String, ByVal matchExact As Boolean) As String
            Dim i, j As Integer
            Dim operands() As String
            Dim operators() As String
            Dim status As Integer
            Dim ch As String
            'Dim ret As String

            searchString = Trim(searchString)

            i = 1
            ReDim operands(0)
            ReDim operators(0)
            status = 0

            operands(0) = ""
            operators(0) = ""

            While (i <= Len(searchString))
                ch = Mid(searchString, i, 1)

                Select Case status
                    Case 0
                        Select Case ch
                            Case Chr(34) 'Inizio stringa
                                status = 1
                                If (i > 1) Then
                                    operators(UBound(operators)) = "+"
                                    ReDim Preserve operators(1 + UBound(operators))
                                    ReDim Preserve operands(1 + UBound(operands))
                                    operands(UBound(operands)) = ""

                                End If
                            Case "&"    'Operatore logico AND
                                operators(UBound(operators)) = "*"
                                ReDim Preserve operators(1 + UBound(operators))
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ""

                            Case "|"
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ""

                            Case " "
                                status = 2

                            Case Else
                                operands(UBound(operands)) = operands(UBound(operands)) & ch
                        End Select
                    Case 1
                        If ch = Chr(34) Then
                            status = 0
                        Else
                            operands(UBound(operands)) = operands(UBound(operands)) & ch
                        End If
                    Case 2
                        Select Case ch
                            Case " "
                            Case "|"
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                status = 3
                            Case "&"
                                operators(UBound(operators)) = "*"
                                ReDim Preserve operators(1 + UBound(operators))
                                status = 3
                            Case Chr(34)
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                status = 1
                            Case Else
                                operators(UBound(operators)) = "+"
                                ReDim Preserve operators(1 + UBound(operators))
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ch
                                status = 0
                        End Select
                    Case 3
                        Select Case ch
                            Case " "
                            Case Else
                                ReDim Preserve operands(1 + UBound(operands))
                                operands(UBound(operands)) = ch
                                status = 0
                        End Select
                End Select

                i = i + 1
            End While

            For i = 0 To UBound(operands)
                If matchExact Then
                    operands(i) = Strings.JoinW("[", fieldName, "]='", Replace(operands(i), "'", "''"), "'")
                Else
                    operands(i) = Strings.JoinW("[", fieldName, "] Like '%", Replace(operands(i), "'", "''"), "%'")
                End If
            Next

            i = UBound(operands)
            For j = UBound(operators) - 1 To 0 Step -1
                Select Case operators(j)
                    Case "+"
                        If UBound(operands) >= i Then
                            operands(i - 1) = Strings.JoinW("(", operands(i - 1), ") Or (", operands(i), ")")
                            i = i - 1
                        Else
                            operands(i - 1) = operands(i)
                        End If
                    Case "*"
                        If UBound(operands) >= i Then
                            operands(i - 1) = Strings.JoinW("(", operands(i - 1), ") And (", operands(i), ")")
                            i = i - 1
                        Else
                            operands(i - 1) = operands(i)
                        End If
                    Case Else
                End Select
            Next

            Return operands(0)
        End Function

        Public Function IsChanged(ByVal value As Object) As Boolean
            If TypeOf (value) Is IEnumerable Then
                For Each v As Object In value
                    If IsChanged(v) Then Return True
                Next
                Return False
            ElseIf TypeOf (value) Is DBObjectBase Then
                Return DirectCast(value, DBObjectBase).IsChanged
            Else
                Throw New ArgumentException("value non implementa alcuna interfaccia valida")
            End If
        End Function

        Public Sub SetChanged(ByVal obj As Object, ByVal value As Boolean)
            If TypeOf (obj) Is IEnumerable Then
                For Each v As Object In obj
                    DBUtils.SetChanged(v, value)
                Next
            ElseIf TypeOf (obj) Is DBObjectBase Then
                DirectCast(obj, DBObjectBase).SetChanged(value)
            Else
                Throw New ArgumentException("obj non implementa alcuna interfaccia valida")
            End If
        End Sub


        ''' <summary>
        ''' Restituisce un valore che indica se si tratta di un tipo data, stringa, numerico o altro
        ''' </summary>
        ''' <param name="rdt"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataTypeFamily(ByVal rdt As adDataTypeEnum) As DBTypesEnum
            Select Case rdt
                Case adDataTypeEnum.adTinyInt, adDataTypeEnum.adSmallInt, adDataTypeEnum.adInteger, adDataTypeEnum.adBigInt, adDataTypeEnum.adUnsignedTinyInt, adDataTypeEnum.adUnsignedSmallInt, adDataTypeEnum.adUnsignedInt, adDataTypeEnum.adUnsignedBigInt, adDataTypeEnum.adSingle, adDataTypeEnum.adDouble, adDataTypeEnum.adCurrency, adDataTypeEnum.adDecimal, adDataTypeEnum.adNumeric
                    Return DBTypesEnum.DBNUMERIC_TYPE
                Case adDataTypeEnum.adDate, adDataTypeEnum.adDBDate, adDataTypeEnum.adDBTime, adDataTypeEnum.adDBTimeStamp
                    Return DBTypesEnum.DBDATETIME_TYPE
                Case adDataTypeEnum.adBoolean
                    Return DBTypesEnum.DBBOOLEAN_TYPE
                Case adDataTypeEnum.adBSTR, adDataTypeEnum.adChar, adDataTypeEnum.adVarChar, adDataTypeEnum.adLongVarChar, adDataTypeEnum.adWChar, adDataTypeEnum.adVarWChar, adDataTypeEnum.adLongVarWChar
                    Return DBTypesEnum.DBTEXT_TYPE
                Case adDataTypeEnum.adBinary, adDataTypeEnum.adVarBinary, adDataTypeEnum.adLongVarBinary, adDataTypeEnum.adArray
                    Return DBTypesEnum.DBBINARY_TYPE
                Case Else
                    Return DBTypesEnum.DBUNSUPPORTED_TYPE
            End Select
        End Function

        Public Function GetADOType(ByVal t As System.Type) As adDataTypeEnum
            If t Is GetType(String) Then Return adDataTypeEnum.adWChar
            If t Is GetType(Boolean) Then Return adDataTypeEnum.adBoolean
            If t Is GetType(Byte) Then Return adDataTypeEnum.adUnsignedTinyInt
            If t Is GetType(SByte) Then Return adDataTypeEnum.adTinyInt
            If t Is GetType(Short) Then Return adDataTypeEnum.adSmallInt
            If t Is GetType(UShort) Then Return adDataTypeEnum.adUnsignedSmallInt
            If t Is GetType(Integer) Then Return adDataTypeEnum.adInteger
            If t Is GetType(UInteger) Then Return adDataTypeEnum.adUnsignedInt
            If t Is GetType(Long) Then Return adDataTypeEnum.adBigInt
            If t Is GetType(ULong) Then Return adDataTypeEnum.adUnsignedBigInt
            If t Is GetType(Single) Then Return adDataTypeEnum.adSingle
            If t Is GetType(Double) Then Return adDataTypeEnum.adDouble
            If t Is GetType(Date) Then Return adDataTypeEnum.adDate
            If t Is GetType(Decimal) Then Return adDataTypeEnum.adCurrency
            If t.IsArray Then Return adDataTypeEnum.adBinary
            If t.IsEnum Then Return adDataTypeEnum.adInteger
            ' Throw New NotSupportedException("GetADOType non riconosce il tipo " & t.FullName)
            Return adDataTypeEnum.adBinary ' adDataTypeEnum.adVariant
        End Function

        Public Function GetOleDbDataType(ByVal t As System.Type) As System.Data.OleDb.OleDbType
            If t Is GetType(String) Then Return OleDb.OleDbType.WChar
            If t Is GetType(Boolean) Then Return OleDb.OleDbType.Boolean
            If t Is GetType(Byte) Then Return OleDb.OleDbType.UnsignedTinyInt
            If t Is GetType(SByte) Then Return OleDb.OleDbType.TinyInt
            If t Is GetType(Short) Then Return OleDb.OleDbType.SmallInt
            If t Is GetType(UShort) Then Return OleDb.OleDbType.UnsignedSmallInt
            If t Is GetType(Integer) Then Return OleDb.OleDbType.Integer
            If t Is GetType(UInteger) Then Return OleDb.OleDbType.UnsignedInt
            If t Is GetType(Long) Then Return OleDb.OleDbType.BigInt
            If t Is GetType(ULong) Then Return OleDb.OleDbType.UnsignedBigInt
            If t Is GetType(Single) Then Return OleDb.OleDbType.Single
            If t Is GetType(Double) Then Return OleDb.OleDbType.Double
            If t Is GetType(Date) Then Return OleDb.OleDbType.Date
            If t Is GetType(Decimal) Then Return OleDb.OleDbType.Currency
            If t.IsArray Then Return OleDb.OleDbType.Binary
            If t.IsEnum Then Return OleDb.OleDbType.Integer
            '            Throw New NotSupportedException("GetADOType non riconosce il tipo " & t.FullName)
            Return OleDb.OleDbType.Binary ' OleDb.OleDbType.Variant
        End Function







        '-----------------------------------

        ''' <summary>
        ''' Restituisce la proprietà dell'oggetto prima delle modifiche
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="fieldName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetOriginalValue(ByVal obj As DBObjectBase, ByVal fieldName As String) As Object
            Return obj.GetOriginalFieldValue(fieldName)
        End Function

        ''' <summary>
        ''' Restituisce la proprietà dell'oggetto prima delle modifiche
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="fieldName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetOriginalValue(Of T As Structure)(ByVal obj As DBObjectBase, ByVal fieldName As String) As Nullable(Of T)
            Dim ret As Object = Me.GetOriginalValue(obj, fieldName)
            'If (ret Is Nothing) Then Return Nothing
            Return CType(ret, Nullable(Of T))
        End Function

        Public Function GetTableName(ByVal obj As Object) As String
            If (TypeOf (obj) Is DBObjectCursorBase) Then
                Return DirectCast(obj, DBObjectCursorBase).GetTableName
            Else
                Return DirectCast(obj, IDBObjectBase).GetTableName
            End If
        End Function

        Public Function GetConnection(ByVal obj As Object) As CDBConnection
            Return DirectCast(obj, DBObjectBase).GetConnection
        End Function

        Public ReadOnly Property PendingQueries As CCollection(Of StatsItem)
            Get
                Return m_PendingQueries
            End Get
        End Property

        Public Function ToDBDateStr(ByVal d As Date?) As String
            If (d.HasValue = False) Then Return vbNullString
            Dim ret As New System.Text.StringBuilder
            ret.Append(Right("0000" & d.Value.Year, 4))
            ret.Append(Right("00" & d.Value.Month, 2))
            ret.Append(Right("00" & d.Value.Day, 2))
            ret.Append(Right("00" & d.Value.Hour, 2))
            ret.Append(Right("00" & d.Value.Minute, 2))
            ret.Append(Right("00" & d.Value.Second, 2))
            Return ret.ToString
        End Function

        Public Function FromDBDateStr(ByVal str As String) As Date?
            str = Trim(str)
            If (str = "") Then Return Nothing
            Dim year As Integer = CInt(Mid(str, 1, 4))
            Dim month As Integer = CInt(Mid(str, 5, 2))
            Dim day As Integer = CInt(Mid(str, 7, 2))
            Dim hour As Integer = CInt(Mid(str, 9, 2))
            Dim minute As Integer = CInt(Mid(str, 11, 2))
            Dim second As Integer = CInt(Mid(str, 13, 2))
            Return Calendar.MakeDate(year, month, day, hour, minute, second)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace

Partial Public Class Databases

    Private Shared m_Utils As CDBUtilsClass = Nothing

    Public Shared ReadOnly Property DBUtils As CDBUtilsClass
        Get
            If (m_Utils Is Nothing) Then m_Utils = New CDBUtilsClass
            Return m_Utils
        End Get
    End Property



End Class

