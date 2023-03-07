Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica

Partial Public Class Databases
    Public Const DBCURSORPAGESIZE As Integer = 25
    Public Shared ReadOnly NULL As DBNull = DBNull.Value
    
    


#Region "Contants"

    Public Enum ObjectStatus As Integer
        OBJECT_TEMP = 0
        OBJECT_VALID = 1
        OBJECT_DELETED = 3
    End Enum

    Public Enum DBTypesEnum As Integer
        DBUNSUPPORTED_TYPE = 0
        DBNUMERIC_TYPE = 1
        DBDATETIME_TYPE = 2
        DBBOOLEAN_TYPE = 3
        DBTEXT_TYPE = 4
        DBBINARY_TYPE = 5
    End Enum

    Public Enum AttachmentStatus As Integer
        STATUS_NOTVALIDATED = 0   'Documento non validato
        STATUS_VALIDATED = 1      'Documento validato
        STATUS_NOTREADABLE = 2    'Documento non leggibile
        STATUS_INVALID = 4        'Documento non valido
    End Enum

#End Region

#Region "ADO Constants"


    ' File di costanti ADODB 
    '
    '   Autore		: 	Domenico Di Mieri <info@domenicodimieri.it>
    '	Data		:	2009/06/16
    '	Versione	:	1.00
    '	Copyright	:	consentita la copia, l'utilizzo e la modifica purchè si lascino
    '                   invariate queste note di copyright
    ' 	

    ' CursorOptionEnum
    Public Enum adCusrorOptionEnum As Integer
        adHoldRecords = &H100
        adMovePrevious = &H200
        adAddNew = &H1000400
        adDelete = &H1000800
        adUpdate = &H1008000
        adBookmark = &H2000
        adApproxPosition = &H4000
        adUpdateBatch = &H10000
        adResync = &H20000
        adNotify = &H40000
        adFind = &H80000
        adSeek = &H400000
        adIndex = &H800000
    End Enum

    ' LockTypeEnum
    Public Enum adLockTypeEnum As Integer
        adLockUnspecified = -1    'Unspecified type of lock. Clones inherits lock type from the original Recordset.
        adLockReadOnly = 1 'Read-only records
        adLockPessimistic = 2 'Pessimistic locking, record by record. The provider lock records immediately after editing
        adLockOptimistic = 3 'Optimistic locking, record by record. The provider lock records only when calling update
        adLockBatchOptimistic = 4 'Optimistic batch updates. Required for batch update mode
    End Enum

    ' ExecuteOptionEnum
    Public Enum adExecuteOptionEnum As Integer
        adAsyncExecute = &H10
        adAsyncFetch = &H20
        adAsyncFetchNonBlocking = &H40
        adExecuteNoRecords = &H80
    End Enum

    ' ConnectOptionEnum
    Public Enum adConnectOptionEnum As Integer
        adAsyncConnect = &H10
    End Enum

    ' ObjectStateEnum
    Public Enum adObjectStateEnum As Integer
        adStateClosed = &H0
        adStateOpen = &H1
        adStateConnecting = &H2
        adStateExecuting = &H4
        adStateFetching = &H8
    End Enum

    ' CursorLocationEnum
    Public Enum adCursorLocationEnum As Integer
        adUseNone = 1    'OBSOLETE (appears only for backward compatibility). Does not use cursor services
        adUseServer = 2   'Default. Uses a server-side cursor
        adUseClient = 3   'Uses a client-side cursor supplied by a local cursor library. For backward compatibility, the synonym adUseClientBatch is also supported
    End Enum

    ' CursorTypes
    Public Enum adCursorTypes As Integer
        adOpenUnspecified = -1 'Does not specify the type of cursor.
        adOpenForwardOnly = 0  'Default. Uses a forward-only cursor. Identical to a static cursor, except that you can only scroll forward through records. This improves performance when you need to make only one pass through a Recordset.
        adOpenKeyset = 1  'Uses a keyset cursor. Like a dynamic cursor, except that you can't see records that other users add, although records that other users delete are inaccessible from your Recordset. Data changes by other users are still visible.
        adOpenDynamic = 2  'Uses a dynamic cursor. Additions, changes, and deletions by other users are visible, and all types of movement through the Recordset are allowed, except for bookmarks, if the provider doesn't support them.
        adOpenStatic = 3  'Uses a static cursor. A static copy of a set of records that you can use to find data or generate reports. Additions, changes, or deletions by other users are not visible.
    End Enum

    ' DataTypeEnum
    Public Enum adDataTypeEnum As Integer
        adEmpty = 0
        adTinyInt = 16
        adSmallInt = 2
        adInteger = 3
        adBigInt = 20
        adUnsignedTinyInt = 17
        adUnsignedSmallInt = 18
        adUnsignedInt = 19
        adUnsignedBigInt = 21
        adSingle = 4
        adDouble = 5
        adCurrency = 6
        adDecimal = 14
        adNumeric = 131
        adBoolean = 11
        adError = 10
        adUserDefined = 132
        adVariant = 12
        adIDispatch = 9
        adIUnknown = 13
        adGUID = 72
        adDate = 7
        adDBDate = 133
        adDBTime = 134
        adDBTimeStamp = 135
        adBSTR = 8
        adChar = 129
        adVarChar = 200
        adLongVarChar = 201
        adWChar = 130
        adVarWChar = 202
        adLongVarWChar = 203
        adBinary = 128
        adVarBinary = 204
        adLongVarBinary = 205
        adChapter = 136
        adFileTime = 64
        adPropVariant = 138
        adVarNumeric = 139
        adArray = &H2000
    End Enum

    ' FieldAttributeEnum
    Public Enum adFieldAttributeEnum As Integer
        adFldMayDefer = &H2
        adFldUpdatable = &H4
        adFldUnknownUpdatable = &H8
        adFldFixed = &H10
        adFldIsNullable = &H20
        adFldMayBeNull = &H40
        adFldLong = &H80
        adFldRowID = &H100
        adFldRowVersion = &H200
        adFldCacheDeferred = &H1000
        adFldIsChapter = &H2000
        adFldNegativeScale = &H4000
        adFldKeyColumn = &H8000
        adFldIsRowURL = &H10000
        adFldIsDefaultStream = &H20000
        adFldIsCollection = &H40000
    End Enum

    ' EditModeEnum
    Public Enum asEditModeEnum As Integer
        adEditNone = &H0
        adEditInProgress = &H1
        adEditAdd = &H2
        adEditDelete = &H4
    End Enum

    ' RecordStatusEnum
    Public Enum adRecordStatusEnum As Integer
        adRecOK = &H0
        adRecNew = &H1
        adRecModified = &H2
        adRecDeleted = &H4
        adRecUnmodified = &H8
        adRecInvalid = &H10
        adRecMultipleChanges = &H40
        adRecPendingChanges = &H80
        adRecCanceled = &H100
        adRecCantRelease = &H400
        adRecConcurrencyViolation = &H800
        adRecIntegrityViolation = &H1000
        adRecMaxChangesExceeded = &H2000
        adRecObjectOpen = &H4000
        adRecOutOfMemory = &H8000
        adRecPermissionDenied = &H10000
        adRecSchemaViolation = &H20000
        adRecDBDeleted = &H40000
    End Enum

    ' GetRowsOptionEnum
    Public Enum adGetRowsOptionEnum As Integer
        adGetRowsRest = -1
    End Enum

    ' PositionEnum
    Public Enum adPositionEnum As Integer
        adPosUnknown = -1
        adPosBOF = -2
        adPosEOF = -3
    End Enum

    ' BookmarkEnum
    Public Enum adBookmarkEnum As Integer
        adBookmarkCurrent = 0
        adBookmarkFirst = 1
        adBookmarkLast = 2
    End Enum

    ' MarshalOptionsEnum
    Public Enum adMarshalOptionsEnum As Integer
        adMarshalAll = 0
        adMarshalModifiedOnly = 1
    End Enum

    ' AffectEnum
    Public Enum adAffectEnum As Integer
        adAffectCurrent = 1
        adAffectGroup = 2
        adAffectAllChapters = 4
    End Enum

    ' ResyncEnum
    Public Enum adResyncEnum As Integer
        adResyncUnderlyingValues = 1
        adResyncAllValues = 2
    End Enum

    ' CompareEnum
    Public Enum adCompareEnum As Integer
        adCompareLessThan = 0
        adCompareEqual = 1
        adCompareGreaterThan = 2
        adCompareNotEqual = 3
        adCompareNotComparable = 4
    End Enum

    ' FilterGroupEnum
    Public Enum adFilterGroupEnum As Integer
        adFilterNone = 0
        adFilterPendingRecords = 1
        adFilterAffectedRecords = 2
        adFilterFetchedRecords = 3
        adFilterConflictingRecords = 5
    End Enum

    ' SearchDirectionEnum
    Public Enum adSearchDirectionEnum As Integer
        adSearchForward = 1
        adSearchBackward = -1
    End Enum

    ' PersistFormatEnum
    Public Enum adPersistFormatEnum As Integer
        adPersistADTG = 0
        adPersistXML = 1
    End Enum

    ' StringFormatEnum
    Public Enum adStringFormatEnum As Integer
        adClipString = 2
    End Enum

    ' ConnectPromptEnum
    Public Enum adConnectPromptEnum As Integer
        adPromptAlways = 1
        adPromptComplete = 2
        adPromptCompleteRequired = 3
        adPromptNever = 4
    End Enum

    Public Enum adConnectModeEnum As Integer
        adModeUnknown = 0
        adModeRead = 1
        adModeWrite = 2
        adModeReadWrite = 3
        adModeShareDenyRead = 4
        adModeShareDenyWrite = 8
        adModeShareExclusive = &HC
        adModeShareDenyNone = &H10
        adModeRecursive = &H400000
    End Enum

    Public Enum adRecordCreateOptionsEnum As Integer
        adCreateCollection = &H2000
        adCreateStructDoc = &H80000000
        adCreateNonCollection = &H0
        adOpenIfExists = &H2000000
        adCreateOverwrite = &H4000000
        adFailIfNotExists = -1
    End Enum


    Public Enum adRecordOpenOptionsEnum As Integer
        adOpenRecordUnspecified = -1
        adOpenSource = &H800000
        adOpenAsync = &H1000
        adDelayFetchStream = &H4000
        adDelayFetchFields = &H8000
    End Enum

    Public Enum adIsolationLevelEnum As Integer
        adXactUnspecified = &HFFFFFFFF
        adXactChaos = &H10
        adXactReadUncommitted = &H100
        adXactBrowse = &H100
        adXactCursorStability = &H1000
        adXactReadCommitted = &H1000
        adXactRepeatableRead = &H10000
        adXactSerializable = &H100000
        adXactIsolated = &H100000
    End Enum

    Public Enum adXactAttributeEnum As Integer
        adXactCommitRetaining = &H20000
        adXactAbortRetaining = &H40000
    End Enum

    Public Enum adPropertyAttributesEnum As Integer
        adPropNotSupported = &H0
        adPropRequired = &H1
        adPropOptional = &H2
        adPropRead = &H200
        adPropWrite = &H400
    End Enum

    Public Enum adErrorValueEnum As Integer
        adErrProviderFailed = &HBB8
        adErrInvalidArgument = &HBB9
        adErrOpeningFile = &HBBA
        adErrReadFile = &HBBB
        adErrWriteFile = &HBBC
        adErrNoCurrentRecord = &HBCD
        adErrIllegalOperation = &HC93
        adErrCantChangeProvider = &HC94
        adErrInTransaction = &HCAE
        adErrFeatureNotAvailable = &HCB3
        adErrItemNotFound = &HCC1
        adErrObjectInCollection = &HD27
        adErrObjectNotSet = &HD5C
        adErrDataConversion = &HD5D
        adErrObjectClosed = &HE78
        adErrObjectOpen = &HE79
        adErrProviderNotFound = &HE7A
        adErrBoundToCommand = &HE7B
        adErrInvalidParamInfo = &HE7C
        adErrInvalidConnection = &HE7D
        adErrNotReentrant = &HE7E
        adErrStillExecuting = &HE7F
        adErrOperationCancelled = &HE80
        adErrStillConnecting = &HE81
        adErrInvalidTransaction = &HE82
        adErrUnsafeOperation = &HE84
        adwrnSecurityDialog = &HE85
        adwrnSecurityDialogHeader = &HE86
        adErrIntegrityViolation = &HE87
        adErrPermissionDenied = &HE88
        adErrDataOverflow = &HE89
        adErrSchemaViolation = &HE8A
        adErrSignMismatch = &HE8B
        adErrCantConvertvalue = &HE8C
        adErrCantCreate = &HE8D
        adErrColumnNotOnThisRow = &HE8E
        adErrURLIntegrViolSetColumns = &HE8F
        adErrURLDoesNotExist = &HE8F
        adErrTreePermissionDenied = &HE90
        adErrInvalidURL = &HE91
        adErrResourceLocked = &HE92
        adErrResourceExists = &HE93
        adErrCannotComplete = &HE94
        adErrVolumeNotFound = &HE95
        adErrOutOfSpace = &HE96
        adErrResourceOutOfScope = &HE97
        adErrUnavailable = &HE98
        adErrURLNamedRowDoesNotExist = &HE99
        adErrDelResOutOfScope = &HE9A
        adErrPropInvalidColumn = &HE9B
        adErrPropInvalidOption = &HE9C
        adErrPropInvalidValue = &HE9D
        adErrPropConflicting = &HE9E
        adErrPropNotAllSettable = &HE9F
        adErrPropNotSet = &HEA0
        adErrPropNotSettable = &HEA1
        adErrPropNotSupported = &HEA2
        adErrCatalogNotSet = &HEA3
        adErrCantChangeConnection = &HEA4
        adErrFieldsUpdateFailed = &HEA5
        adErrDenyNotSupported = &HEA6
        adErrDenyTypeNotSupported = &HEA7
    End Enum

    Public Enum adParameterAttributesEnum As Integer
        adParamSigned = &H10
        adParamNullable = &H40
        adParamLong = &H80
    End Enum

    Public Enum adParameterDirectionEnum As Integer
        adParamUnknown = &H0
        adParamInput = &H1
        adParamOutput = &H2
        adParamInputOutput = &H3
        adParamReturnValue = &H4
    End Enum

    Public Enum adCommandTypeEnum As Integer
        adCmdUnknown = &H8
        adCmdText = &H1
        adCmdTable = &H2
        adCmdStoredProc = &H4
        adCmdFile = &H100
        adCmdTableDirect = &H200
    End Enum

    Public Enum adEventStatusEnum As Integer
        adStatusOK = &H1
        adStatusErrorsOccurred = &H2
        adStatusCantDeny = &H3
        adStatusCancel = &H4
        adStatusUnwantedEvent = &H5
    End Enum

    Public Enum adEventReasonEnum As Integer
        adRsnAddNew = 1
        adRsnDelete = 2
        adRsnUpdate = 3
        adRsnUndoUpdate = 4
        adRsnUndoAddNew = 5
        adRsnUndoDelete = 6
        adRsnRequery = 7
        adRsnResynch = 8
        adRsnClose = 9
        adRsnMove = 10
        adRsnFirstChange = 11
        adRsnMoveFirst = 12
        adRsnMoveNext = 13
        adRsnMovePrevious = 14
        adRsnMoveLast = 15
    End Enum

    Public Enum adSchemaEnum As Integer
        adSchemaProviderSpecific = -1
        adSchemaAsserts = 0
        adSchemaCatalogs = 1
        adSchemaCharacterSets = 2
        adSchemaCollations = 3
        adSchemaColumns = 4
        adSchemaCheckConstraints = 5
        adSchemaConstraintColumnUsage = 6
        adSchemaConstraintTableUsage = 7
        adSchemaKeyColumnUsage = 8
        adSchemaReferentialConstraints = 9
        adSchemaTableConstraints = 10
        adSchemaColumnsDomainUsage = 11
        adSchemaIndexes = 12
        adSchemaColumnPrivileges = 13
        adSchemaTablePrivileges = 14
        adSchemaUsagePrivileges = 15
        adSchemaProcedures = 16
        adSchemaSchemata = 17
        adSchemaSQLLanguages = 18
        adSchemaStatistics = 19
        adSchemaTables = 20
        adSchemaTranslations = 21
        adSchemaProviderTypes = 22
        adSchemaViews = 23
        adSchemaViewColumnUsage = 24
        adSchemaViewTableUsage = 25
        adSchemaProcedureParameters = 26
        adSchemaForeignKeys = 27
        adSchemaPrimaryKeys = 28
        adSchemaProcedureColumns = 29
        adSchemaDBInfoKeywords = 30
        adSchemaDBInfoLiterals = 31
        adSchemaCubes = 32
        adSchemaDimensions = 33
        adSchemaHierarchies = 34
        adSchemaLevels = 35
        adSchemaMeasures = 36
        adSchemaProperties = 37
        adSchemaMembers = 38
        adSchemaTrustees = 39
    End Enum

    Public Enum adFieldStatusEnum As Integer
        adFieldOK = 0
        adFieldCantConvertValue = 2
        adFieldIsNull = 3
        adFieldTruncated = 4
        adFieldSignMismatch = 5
        adFieldDataOverflow = 6
        adFieldCantCreate = 7
        adFieldUnavailable = 8
        adFieldPermissionDenied = 9
        adFieldIntegrityViolation = 10
        adFieldSchemaViolation = 11
        adFieldBadStatus = 12
        adFieldDefault = 13
        adFieldIgnore = 15
        adFieldDoesNotExist = 16
        adFieldInvalidURL = 17
        adFieldResourceLocked = 18
        adFieldResourceExists = 19
        adFieldCannotComplete = 20
        adFieldVolumeNotFound = 21
        adFieldOutOfSpace = 22
        adFieldCannotDeleteSource = 23
        adFieldReadOnly = 24
        adFieldResourceOutOfScope = 25
        adFieldAlreadyExists = 26
        adFieldPendingInsert = &H10000
        adFieldPendingDelete = &H20000
        adFieldPendingChange = &H40000
        adFieldPendingUnknown = &H80000
        adFieldPendingUnknownDelete = &H100000
    End Enum

    Public Enum adSeekEnum As Integer
        adSeekFirstEQ = &H1
        adSeekLastEQ = &H2
        adSeekAfterEQ = &H4
        adSeekAfter = &H8
        adSeekBeforeEQ = &H10
        adSeekBefore = &H20
    End Enum

    Public Enum adADCPROP_UPDATECRITERIA_ENUM As Integer
        adCriteriaKey = 0
        adCriteriaAllCols = 1
        adCriteriaUpdCols = 2
        adCriteriaTimeStamp = 3
    End Enum

    Public Enum adADCPROP_ASYNCTHREADPRIORITY_ENUM As Integer
        adPriorityLowest = 1
        adPriorityBelowNormal = 2
        adPriorityNormal = 3
        adPriorityAboveNormal = 4
        adPriorityHighest = 5
    End Enum

    Public Enum adADCPROP_AUTORECALC_ENUM As Integer
        adRecalcUpFront = 0
        adRecalcAlways = 1
    End Enum

    'Public Enum ad ADCPROP_UPDATERESYNC_ENUM

    ' ADCPROP_UPDATERESYNC_ENUM

    Public Enum adMoveRecordOptionsEnum As Integer
        adMoveUnspecified = -1
        adMoveOverWrite = 1
        adMoveDontUpdateLinks = 2
        adMoveAllowEmulation = 4
    End Enum

    Public Enum adCopyRecordOptionsEnum As Integer
        adCopyUnspecified = -1
        adCopyOverWrite = 1
        adCopyAllowEmulation = 4
        adCopyNonRecursive = 2
    End Enum

    Public Enum adStreamTypeEnum As Integer
        adTypeBinary = 1
        adTypeText = 2
    End Enum

    Public Enum adLineSeparatorEnum As Integer
        adLF = 10
        adCR = 13
        adCRLF = -1
    End Enum

    Public Enum adStreamOpenOptionsEnum As Integer
        adOpenStreamUnspecified = -1
        adOpenStreamAsync = 1
        adOpenStreamFromRecord = 4
    End Enum

    Public Enum adStreamWriteEnum As Integer
        adWriteChar = 0
        adWriteLine = 1
    End Enum

    Public Enum adSaveOptionsEnum As Integer
        adSaveCreateNotExist = 1
        adSaveCreateOverWrite = 2
    End Enum

    Public Enum adFieldEnum As Integer
        adDefaultStream = -1
        adRecordURL = -2
    End Enum

    Public Enum adStreamReadEnum As Integer
        adReadAll = -1
        adReadLine = -2
    End Enum

    Public Enum adRecordTypeEnum As Integer
        adSimpleRecord = 0
        adCollectionRecord = 1
        adStructDoc = 2
    End Enum

#End Region

#Region "Interfaces"

    Public Interface ICopyObject

        Function CopyFrom(ByVal source As Object) As Object

    End Interface

    Public Interface IDBMinObject

        Function SaveToDatabase(ByVal dbConn As CDBConnection, ByVal force As Boolean) As Boolean

        Function DropFromDatabase(ByVal dbConn As CDBConnection, ByVal force As Boolean) As Boolean

    End Interface

   

    Public Interface IDBObjectBase
        Inherits IDBMinObject

        ReadOnly Property ID As Integer

        Sub SetID(ByVal newID As Integer)

        Sub ResetID()



        Function SaveToRecordset(ByVal writer As DBWriter) As Boolean

        Function LoadFromRecordset(ByVal reader As DBReader) As Boolean

        Function GetTableName() As String

    End Interface

    Public Interface IDBObjectCollection
        Inherits IEnumerable, IDBMinObject

    End Interface

    Public Interface IDBObject
        Inherits IDBObjectBase

        ReadOnly Property CreatoDa As CUser
        ReadOnly Property CreatoDaId As Integer
        ReadOnly Property CreatoIl As Date
        ReadOnly Property ModificatoDa As CUser
        ReadOnly Property ModificatoDaId As Integer
        ReadOnly Property ModificatoIl As Date
        Property Stato As ObjectStatus

        Sub ForceUser(ByVal user As CUser)

    End Interface

    Public Interface IDBPOObject
        Property IDPuntoOperativo As Integer
        Property PuntoOperativo As CUfficio
        Property NomePuntoOperativo As String
    End Interface

#End Region

#Region "Classes"

    <Serializable> _
    Public Class DBException
        Inherits System.Exception

        Private m_SQL As String = ""

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String, ByVal sql As String, ByVal inner As Exception)
            MyBase.New(message, inner)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_SQL = sql
        End Sub

        Public ReadOnly Property SQL As String
            Get
                Return Me.m_SQL
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append(Me.Message)
            If (Me.m_SQL <> "") Then ret.Append(vbNewLine & "SQL: " & Me.m_SQL)
            If (Me.InnerException IsNot Nothing) Then ret.Append(vbNewLine & "Inner: " & Me.InnerException.ToString)
            Return ret.ToString
        End Function

    End Class

    Public NotInheritable Class PropertyChangedEventArgs
        Inherits System.EventArgs
        Implements DMD.XML.IDMDXMLSerializable

        Private m_PropertyName As String
        Private m_NewValue As Object
        Private m_OldValue As Object
        Private m_TypeName As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_PropertyName = vbNullString
            Me.m_NewValue = Nothing
            Me.m_OldValue = Nothing
            Me.m_TypeName = Nothing
        End Sub

        Public Sub New(ByVal propName As String, Optional ByVal newVal As Object = Nothing, Optional ByVal oldVal As Object = Nothing)
            Me.New
            Me.m_PropertyName = Trim(propName)
            Me.m_OldValue = oldVal
            Me.m_NewValue = newVal
        End Sub

        Public ReadOnly Property PropertyName As String
            Get
                Return Me.m_PropertyName
            End Get
        End Property

        Public ReadOnly Property OldValue As Object
            Get
                Return Me.m_OldValue
            End Get
        End Property

        Public ReadOnly Property NewValue As Object
            Get
                Return Me.m_NewValue
            End Get
        End Property

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Name" : Me.m_PropertyName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NewVal" : Me.m_NewValue = Me.GetValue(fieldValue, Me.m_TypeName)
                Case "OldVal" : Me.m_OldValue = Me.GetValue(fieldValue, Me.m_TypeName)
                Case "TypeName" : Me.m_TypeName = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Name", Me.m_PropertyName)
            writer.WriteAttribute("TypeName", TypeName(Me.m_NewValue))
            Me.WriteValue(writer, "NewVal", Me.m_NewValue)
            Me.WriteValue(writer, "OldVal", Me.m_NewValue)
        End Sub

        Private Sub WriteValue(ByVal writer As XML.XMLWriter, ByVal tag As String, ByVal value As Object)
            Select Case TypeName(value)
                Case "Byte", "SByte"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Byte)(value))
                Case "Short", "UShort", "Int16", "UInt16"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Short)(value))
                Case "Integer", "UInteger", "Int32", "UInt32"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Short)(value))
                Case "Long", "ULong", "Int16", "UInt64"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Short)(value))
                Case "Single"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Single)(value))
                Case "Double"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Double)(value))
                Case "Decimal"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Decimal)(value))
                Case "Date", "DateTime"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Date)(value))
                Case "Boolean"
                    writer.WriteAttribute(tag, Me.MakeValue(Of Boolean)(value))
                Case "String"
                    writer.WriteAttribute(tag, CStr(value))
                Case Else
                    Debug.Print(TypeName(value))
            End Select
        End Sub

        Private Function MakeValue(Of T As Structure)(ByVal v As Object) As Nullable(Of T)
            Return v
        End Function

        Private Function GetValue(ByVal v As Object, ByVal tName As String) As Object
            Select Case tName
                Case "Byte", "SByte"
                    Return XML.Utils.Serializer.DeserializeInteger(v)
                Case "Short", "UShort", "Int16", "UInt16"
                    Return XML.Utils.Serializer.DeserializeInteger(v)
                Case "Integer", "UInteger", "Int32", "UInt32"
                    Return XML.Utils.Serializer.DeserializeInteger(v)
                Case "Long", "ULong", "Int16", "UInt64"
                    Return XML.Utils.Serializer.DeserializeLong(v)
                Case "Single", "Double"
                    Return XML.Utils.Serializer.DeserializeDouble(v)
                Case "Decimal"
                    Return XML.Utils.Serializer.DeserializeDouble(v)
                Case "Date", "DateTime"
                    Return XML.Utils.Serializer.DeserializeDate(v)
                Case "Boolean"
                    Return XML.Utils.Serializer.DeserializeBoolean(v)
                Case "String"
                    Return CStr(v)
                Case Else
                    Return Nothing
            End Select
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class








#End Region

    Private Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub






    Public Shared Function GetID(ByVal value As IDBObjectBase, Optional ByVal defaultID As Integer = 0) As Integer
        If (value Is Nothing) Then Return defaultID
        Return DirectCast(value, IDBObjectBase).ID
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class

