Public Class Module2
    '    VERSION 1.0 CLASS
    'BEGIN
    '  MultiUse = -1  'True
    '  Persistable = 0  'NotPersistable
    '  DataBindingBehavior = 0  'vbNone
    '  DataSourceBehavior  = 0  'vbNone
    '  MTSTransactionMode  = 0  'NotAnMTSObject
    'END
    'Attribute VB_Name = "vbPDFParser"
    'Attribute VB_GlobalNameSpace = False
    'Attribute VB_Creatable = True
    'Attribute VB_PredeclaredId = False
    'Attribute VB_Exposed = False
    '===================================================================
    ' Descrizione.....: Classe per il parsing di un file PDF
    '                   Class to parse a PDF file
    ' Nome dei File...: vbPDFParser.cls
    ' Data............: 20/09/2007
    ' Versione........: 1.1
    ' Sistema.........: Visual Basic 6.0 Pro - SP 6
    ' Testato su......: Windows XP Professional - SP 2
    ' Scritto da......: Luigi Micco
    ' E-Mail..........: l.micco(at)tiscali.it
    '===================================================================
    '===================================================================
    ' (C) 2007  - L'uso di questo software è consentito solo su espressa
    '             autorizzazione dell'autore. Non puo' essere copiato o
    '             ridistribuito, ne' integralmente, ne' parzialmente.
    '
    '             The use of this software is allowed only on express
    '             authorization of the author. It's cannot be copied or
    '             redistributed, neither integrally, neither partially.
    '===================================================================

    Public Structure Flate_CodesType
        Public Lenght As Integer
        Public Code As Integer
    End Structure

    Public Structure BufferString
        Public Buffer As String
        Public BufferLen As Integer
        Public Pointer As Integer
    End Structure

    Private mvarBufferDecodeStream As BufferString

    Private mvarImpFileNumber As Integer
    Private mvarFilename As String
    Private mvarPages() As Integer
    Private mvarObjOffset() As Integer
    Private mvarObjNewId() As Integer
    Private mvarDocVersion As Integer
    Private mvarPageCount As Integer
    Private mvarDocTitle As String
    Private mvarDocAuthor As String
    Private mvarDocProducer As String
    Private mvarDocCreator As String
    Private mvarDocSubject As String
    Private mvarDocKeywords As String
    Private mvarDocDate As String
    Private mvarProtection As Integer
    Private mvarEncrypt As Integer

    Private CodeBuffer As String
    Private CodeCursor As Integer

    Private Pwr2(16) As Integer

    Public Sub New()
        Dim x As Byte

        For x = 0 To 16
            Pwr2(x) = 2 ^ x
        Next
    End Sub

    '---------------------------------------------------------------------------------------
    ' Proprietà : Author
    ' Risultato : Restituisce l'autore del documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Author() As String
        Get
            Return Me.mvarDocAuthor
        End Get
    End Property


    '  '
    '  Private Sub Class_Terminate()
    'If mvarImpFileNumber <> 0 Then Close #mvarImpFileNumber
    '  End Sub

    '---------------------------------------------------------------------------------------
    ' Proprietà : CreationDate
    ' Risultato : Restituisce la data di creazione del documento (yyyymmdd)
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property CreationDate() As String
        Get
            Return Mid(mvarDocDate, 9, 2) & "/" & Mid(mvarDocDate, 7, 2) & "/" & Mid(mvarDocDate, 3, 4)
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : Creator
    ' Risultato : Restituisce il creatore del documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Creator() As String
        Get
            Return Me.mvarDocCreator
        End Get
    End Property

    Public Function GetInfo(Filename As String) As Boolean

        Dim lngPos As Integer
        Dim lngStart As Integer
        Dim lngEnd As Integer
        Dim strText As String
        Dim lngXREFPos As Integer
        Dim lngPageObj As Integer
        Dim ObjRoot As Integer
        Dim ObjInfo As Integer
        Dim blnEncrypted As Boolean

        mvarImpFileNumber = FreeFile()
        mvarFilename = Filename

        FileOpen(mvarImpFileNumber, Filename, OpenMode.Binary, OpenAccess.Read)

        ' Legge la versione del documento
        strText = Space(8)
        FileGet(mvarImpFileNumber, strText)
        mvarDocVersion = Val(Right(strText, 1))

        lngPos = LOF(mvarImpFileNumber)
        strText = Space(64)
        Seek(mvarImpFileNumber, lngPos - 64)
        FileGet(mvarImpFileNumber, strText)

        lngStart = InStr(strText, "startxref") + 9
        lngEnd = InStr(lngStart, strText, "%%EOF")
        lngXREFPos = Val(Mid(strText, lngStart, lngEnd - lngStart))
        ReDim mvarObjOffset(0)
        ReDim mvarObjNewId(0)
        blnEncrypted = zzPrsReadXREF(lngXREFPos, ObjRoot, ObjInfo)

        mvarProtection = 0
        If blnEncrypted Then
            ' Legge le informazione dell'oggetto Encrypt
            strText = zzPrsReadParam(mvarEncrypt)
            zzPrsInitBuffer(strText)
            If zzPrsSeekToken("V", 1) Then mvarProtection = zzPrsReadNum()
        Else
            ' Legge le informazione dell'oggetto INFO
            strText = zzPrsReadParam(ObjInfo)
            zzPrsInitBuffer(strText)
            If zzPrsSeekToken("Title", 1) Then mvarDocTitle = zzPrsReadValue()
            If zzPrsSeekToken("Author", 1) Then mvarDocAuthor = zzPrsReadValue()
            If zzPrsSeekToken("Creator", 1) Then mvarDocCreator = zzPrsReadValue()
            If zzPrsSeekToken("Producer", 1) Then mvarDocProducer = zzPrsReadValue()
            If zzPrsSeekToken("Subject", 1) Then mvarDocSubject = zzPrsReadValue()
            If zzPrsSeekToken("CreationDate", 1) Then mvarDocDate = zzPrsReadValue()
            If zzPrsSeekToken("Keywords", 1) Then mvarDocKeywords = zzPrsReadValue()
            If zzPrsSeekToken("CreationDate", 1) Then mvarDocDate = zzPrsReadValue()

            ' Legge le informazioni dell'oggetto ROOT
            strText = zzPrsReadParam(ObjRoot)
            zzPrsInitBuffer(strText)
            If zzPrsSeekToken("Pages", 1) Then
                lngPageObj = zzPrsReadRef()
                zzPrsReadPagesTree(lngPageObj)
            End If

        End If
        GetInfo = True

    End Function

    '---------------------------------------------------------------------------------------
    ' Sub       : GetObj
    ' Risultato : Restituisce l'oggetto
    '---------------------------------------------------------------------------------------
    Public Sub GetObj(ObjId As Integer, _
                      ByRef Param As String, _
                      ByRef Stream() As Byte, _
                      ByRef Filter As String)

        Dim blnStream As Boolean
        blnStream = True
        Me.zzPrsReadObj(ObjId, Param, blnStream, Stream, Filter)
        If Not blnStream Then ReDim Stream(0)
    End Sub

    '---------------------------------------------------------------------------------------
    ' Proprietà : Keywords
    ' Risultato : Restituisce la stringa delle Keywords del documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Keywords() As String
        Get
            Return Me.mvarDocKeywords
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : ObjCount
    ' Risultato : Restituisce il numero di oggetti del documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property ObjCount() As Integer
        Get
            Return UBound(Me.mvarObjOffset) - 1
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : Protection
    ' Risultato : Restituisce il tipo di protezione associato al documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Protection() As Integer
        Get
            Return Me.mvarProtection
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : ObjOffset
    ' Risultato : Restituisce l'offset di ogni singolo oggetto
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property ObjOffset(ByVal ObjIndex As Integer) As Integer
        Get
            Return Me.mvarObjOffset(ObjIndex)
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : PageCount
    ' Risultato : Restituisce il numero di pagine
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property PageCount() As Integer
        Get
            Return Me.mvarPageCount
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : PageObj
    ' Risultato : Restituisce il numero dell'oggetto che contiene la pagina
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property PageObj(ByVal PageNumber As Integer) As Integer
        Get
            Return Me.mvarPages(PageNumber)
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : Producer
    ' Risultato : Restituisce il nome dell'applicativo che ha generato il documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Producer() As String
        Get
            Return Me.mvarDocProducer
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : Subject
    ' Risultato : Restituisce l'oggetto del documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Subject() As String
        Get
            Return Me.mvarDocSubject
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : Title
    ' Risultato : Restituisce il titolo del documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Title() As String
        Get
            Return Me.mvarDocTitle
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    ' Proprietà : Version
    ' Risultato : Restituisce la versione del documento
    '---------------------------------------------------------------------------------------
    Public ReadOnly Property Version() As String
        Get
            Return "PDF 1." & Me.mvarDocVersion
        End Get
    End Property

    Private Sub zzPrsInitBuffer(ByRef Code As String)
        Me.CodeBuffer = Code
        Me.CodeCursor = 1
    End Sub

    Private Function zzPrsReadNum() As Object
        Dim strTemp As String = ""
        Me.zzPrsSkipDummy()
        While IsNumeric(Mid(Me.CodeBuffer, Me.CodeCursor, 1)) Or Mid(Me.CodeBuffer, Me.CodeCursor, 1) = "-" Or Mid(Me.CodeBuffer, Me.CodeCursor, 1) = "."
            strTemp = strTemp & Mid(Me.CodeBuffer, Me.CodeCursor, 1)
            Me.CodeCursor = Me.CodeCursor + 1
        End While
        Return Val(strTemp)
    End Function

    Private Sub zzPrsReadObj(ByVal ObjId As Integer, ByRef Param As String, ByRef Stream As Boolean, ByRef OutBuf() As Byte, ByRef Filter As String)
        Dim strBuffer As String = Space(1024)
        Dim blnFlag As Boolean
        Dim lngStream As Integer
        Dim lngLength As Integer
        Dim lngOffset As Integer

        Dim strTemp As String = ""
        Dim lngStartObj As Integer
        Dim lngEndObj As Integer

        Dim tmpCodeBuffer As String
        Dim tmpCodeCursor As Integer

        tmpCodeBuffer = Me.CodeBuffer
        tmpCodeCursor = Me.CodeCursor

        Seek(mvarImpFileNumber, mvarObjOffset(ObjId) + 1)
        blnFlag = True
        Do While blnFlag
            FileGet(mvarImpFileNumber, strBuffer)
            ' Trova l'inizio dell'oggetto
            If lngStartObj = 0 Then
                '' 1.1
                lngStartObj = InStr(strBuffer, "obj") + 4

                If Asc(Mid(strBuffer, lngStartObj, 1)) = 10 Then lngStartObj = lngStartObj + 1
                If Mid(strBuffer, lngStartObj, 2) = vbCrLf Then
                    lngStartObj = lngStartObj + 2
                ElseIf Mid(strBuffer, lngStartObj, 1) = vbCr Then
                    lngStartObj = lngStartObj + 1
                End If
                strBuffer = Mid(strBuffer, lngStartObj)
            End If

            ' Trova la fine dell'oggetto o l'inizio dello stream
            lngEndObj = InStr(strBuffer, "endobj" & Chr(13))
            If lngEndObj = 0 Then lngEndObj = InStr(strBuffer, "endobj" & Chr(10))
            lngStream = InStr(strBuffer, "stream" & Chr(10))
            If lngStream = 0 Then lngStream = InStr(strBuffer, "stream" & Chr(13))
            If (lngEndObj = 0) And (lngStream = 0) Then
                strTemp = strTemp & strBuffer
            Else
                If (lngEndObj = 0) Or ((lngStream <> 0) And (lngStream < lngEndObj)) Then
                    lngEndObj = lngStream
                End If
                blnFlag = False
                If Mid(strBuffer, lngEndObj - 2, 2) = vbCrLf Then
                    lngEndObj = lngEndObj - 2
                ElseIf Mid(strBuffer, lngEndObj - 1, 1) = vbCr Then
                    lngEndObj = lngEndObj - 1
                End If
                strTemp = strTemp & Left$(strBuffer, lngEndObj - 1) ' + 6)

            End If
        Loop

        Param = strTemp

        If Stream Then
            zzPrsInitBuffer(strTemp)
            If zzPrsSeekToken("Filter", 1) Then Filter = zzPrsReadValue()
            If zzPrsSeekToken("Length", 1) Then
                Stream = True
                lngStartObj = CodeCursor
                lngLength = Val(zzPrsReadRecurseRef)

                lngEndObj = CodeCursor
                strTemp = Left$(strTemp, lngStartObj) & " " & _
                          CStr(lngLength) & " " & _
                          Mid(strTemp, lngEndObj)
                Seek(mvarImpFileNumber, mvarObjOffset(ObjId) + 1)
                blnFlag = True
                Do While blnFlag
                    FileGet(Me.mvarImpFileNumber, strBuffer)
                    ' Trova l'inizio dello stream
                    lngStream = InStr(strBuffer, "stream")
                    If (lngStream <> 0) Then
                        If Mid(strBuffer, lngStream + 6, 1) = Chr(13) Then
                            lngOffset = lngStream + 7
                        Else
                            lngOffset = lngStream + 6
                        End If
                        blnFlag = False

                        ReDim OutBuf(lngLength - 1)
                        Seek(mvarImpFileNumber, mvarObjOffset(ObjId) + 1 + lngOffset)
                        FileGet(Me.mvarImpFileNumber, OutBuf)
                    Else
                        lngOffset = lngOffset + 1024
                    End If
                Loop
            Else
                Stream = False
            End If

        End If
        Me.CodeBuffer = tmpCodeBuffer
        Me.CodeCursor = tmpCodeCursor
    End Sub

    Private Sub zzPrsReadPagesTree(ByVal PagesObj As Integer)
        Dim strText As String
        Dim strType As String
        Dim Temp() As Integer = Nothing
        Dim i As Integer

        strText = zzPrsReadParam(PagesObj)
        zzPrsInitBuffer(strText)

        If zzPrsSeekToken("Kids", 1) Then
            Me.zzPrsReadRefArray(Temp)
            For i = 1 To UBound(Temp)
                strText = zzPrsReadParam(Temp(i))
                zzPrsInitBuffer(strText)
                If zzPrsSeekToken("Type", 1) Then
                    strType = zzPrsReadValue()
                    If strType = "/Page" Then
                        mvarPageCount = mvarPageCount + 1
                        ReDim Preserve mvarPages(mvarPageCount)
                        mvarPages(mvarPageCount) = Temp(i)
                    ElseIf strType = "/Pages" Then
                        zzPrsReadPagesTree(Temp(i))
                    End If
                End If
            Next
        End If

    End Sub

    Private Function zzPrsReadParam(Index As Integer) As String
        Dim Filter As String = ""
        Dim Stream() As Byte = Nothing
        Dim blnFlag As Boolean
        Dim strTemp As String = ""

        blnFlag = False
        Me.zzPrsReadObj(Index, strTemp, blnFlag, Stream, Filter)
        zzPrsReadParam = strTemp
    End Function

    ' Legge il valore a cui punta il riferimento indiretto
    Private Function zzPrsReadRecurseRef() As String
        Dim lngTemp As Integer
        Dim lngTempR As Integer
        Dim strText As String = ""


        lngTemp = zzPrsReadNum()
        zzPrsSkipDummy()
        If IsNumeric(Mid(CodeBuffer, CodeCursor, 1)) Then
            lngTempR = zzPrsReadNum()
            zzPrsSkipDummy()
            If Mid(CodeBuffer, CodeCursor, 1) = "R" Then strText = zzPrsReadParam(lngTemp)
        Else
            strText = CStr(lngTemp)
        End If

        strText = Replace(Replace(strText, vbCr, " "), vbLf, " ")
        Do While InStr(strText, "  ") <> 0
            strText = Replace(strText, "  ", " ")
        Loop
        zzPrsReadRecurseRef = strText

    End Function

    ' Legge l'ID dell'oggetto a cui si riferisce il riferimento indiretto
    Private Function zzPrsReadRef() As Integer
        Dim lngTemp As Integer
        Dim lngTempR As Integer
        Dim ret As Integer = 0

        lngTemp = Me.zzPrsReadNum()
        zzPrsSkipDummy()
        lngTempR = Me.zzPrsReadNum()
        Me.zzPrsSkipDummy()
        If Mid(Me.CodeBuffer, Me.CodeCursor, 1) = "R" Then
            ret = lngTemp
            Me.CodeCursor = Me.CodeCursor + 1
        End If
        Return ret
    End Function

    ' Legge un array di riferimenti indiretti
    Private Sub zzPrsReadRefArray(ByRef aObj() As Integer)
        Dim lngObjNum As Integer
        Dim i As Integer
        Dim lngEnd As Integer

        i = 0
        zzPrsSkipDummy()
        If Mid(CodeBuffer, CodeCursor, 1) = "[" Then
            CodeCursor = CodeCursor + 1
            lngEnd = InStr(CodeCursor, CodeBuffer, "]")
            While CodeCursor <= (lngEnd - 5)
                lngObjNum = zzPrsReadRef()
                If lngObjNum <> 0 Then
                    i = i + 1
                    ReDim Preserve aObj(i)
                    aObj(i) = lngObjNum
                End If
            End While
        Else
            lngObjNum = zzPrsReadRef()
            If lngObjNum <> 0 Then
                ReDim aObj(1)
                aObj(1) = lngObjNum
            End If
        End If

    End Sub

    ' Legge il valore presente alla posizione del cursore
    Private Function zzPrsReadValue(Optional Recursive As Boolean = False) As String
        Dim strTemp As String
        Dim strValue As String = ""
        Dim lngStart As Integer
        Dim lngEnd As Integer
        Dim i As Integer
        Dim strChar As String
        Dim lngTemp As Integer
        Dim strText As String = ""
        Dim lngCount As Integer
        Dim lngTempR As Integer

        zzPrsSkipDummy()
        If Mid(CodeBuffer, CodeCursor, 1) = "/" Then

            lngStart = CodeCursor
            CodeCursor = CodeCursor + 1
            Do While (Mid(CodeBuffer, CodeCursor, 1) <> "[") And _
               (Mid(CodeBuffer, CodeCursor, 1) <> " ") And _
               (Mid(CodeBuffer, CodeCursor, 1) <> "/") And _
               (Mid(CodeBuffer, CodeCursor, 1) <> "(") And _
               (Mid(CodeBuffer, CodeCursor, 1) <> "<") And _
               (Mid(CodeBuffer, CodeCursor, 1) <> ">") And _
               (Mid(CodeBuffer, CodeCursor, 1) <> vbCr) And _
               (Mid(CodeBuffer, CodeCursor, 1) <> vbLf)
                CodeCursor = CodeCursor + 1
            Loop
            zzPrsReadValue = Mid(CodeBuffer, lngStart, CodeCursor - lngStart)

        ElseIf Mid(CodeBuffer, CodeCursor, 1) = "[" Then
            ' Legge un array di valori
            zzPrsSkipDummy()
            If Mid(CodeBuffer, CodeCursor, 1) = "[" Then
                lngStart = CodeCursor
                lngCount = 1
                CodeCursor = CodeCursor + 1
                While lngCount > 0
                    If Mid(CodeBuffer, CodeCursor, 1) = "[" Then lngCount = lngCount + 1
                    If Mid(CodeBuffer, CodeCursor, 1) = "]" Then lngCount = lngCount - 1
                    CodeCursor = CodeCursor + 1
                End While
                strText = Mid(CodeBuffer, lngStart, CodeCursor - lngStart)
                strText = Replace(Replace(strText, vbCr, " "), vbLf, " ")
                Do While InStr(strText, "  ") <> 0
                    strText = Replace(strText, "  ", " ")
                Loop
            End If
            zzPrsReadValue = strText

        ElseIf Mid(CodeBuffer, CodeCursor, 2) = "<<" Then
            ' Legge un dizionario


            zzPrsSkipDummy()
            If IsNumeric(Mid(CodeBuffer, CodeCursor, 1)) Then
                lngTemp = zzPrsReadNum()
                zzPrsSkipDummy()
                lngTempR = zzPrsReadNum()
                zzPrsSkipDummy()
                If Mid(CodeBuffer, CodeCursor, 1) = "R" Then
                    strText = zzPrsReadParam(lngTemp)
                    lngStart = InStr(strText, "obj") + 3
                    lngEnd = InStr(strText, "endobj") - 1
                    strText = Mid(strText, lngStart, lngEnd - lngStart)
                End If
            ElseIf Mid(CodeBuffer, CodeCursor, 2) = "<<" Then
                lngStart = CodeCursor
                lngCount = 1
                CodeCursor = CodeCursor + 2
                While lngCount > 0
                    If Mid(CodeBuffer, CodeCursor, 2) = "<<" Then lngCount = lngCount + 1
                    If Mid(CodeBuffer, CodeCursor, 2) = ">>" Then lngCount = lngCount - 1
                    CodeCursor = CodeCursor + 1
                End While
                strText = Mid(CodeBuffer, lngStart, (CodeCursor + 1) - lngStart + 1)
            End If
            strText = Replace(Replace(strText, vbCr, " "), vbLf, " ")

            Do While InStr(strText, "  ") <> 0
                strText = Replace(strText, "  ", " ")
            Loop
            zzPrsReadValue = strText

        ElseIf (Mid(CodeBuffer, CodeCursor, 1) = "(") Or (Mid(CodeBuffer, CodeCursor, 1) = "<") Then

            If Mid(CodeBuffer, CodeCursor, 1) = "(" Then
                CodeCursor = CodeCursor + 1
                lngStart = CodeCursor
                While lngEnd = 0
                    If Mid(CodeBuffer, CodeCursor, 2) = "\\" Then CodeCursor = CodeCursor + 2
                    If Mid(CodeBuffer, CodeCursor, 2) = "\(" Then CodeCursor = CodeCursor + 2
                    If Mid(CodeBuffer, CodeCursor, 2) = "\)" Then CodeCursor = CodeCursor + 2
                    If Mid(CodeBuffer, CodeCursor, 1) = ")" Then lngEnd = CodeCursor - 1
                    CodeCursor = CodeCursor + 1
                End While
                strTemp = Mid(CodeBuffer, lngStart, lngEnd - lngStart + 1)
                zzPrsReadValue = Replace(Replace(Replace(strTemp, "\\", "\"), "\(", "("), "\)", ")")
            ElseIf Mid(CodeBuffer, CodeCursor, 1) = "<" Then
                CodeCursor = CodeCursor + 1
                lngStart = CodeCursor
                CodeCursor = InStr(lngStart, CodeBuffer, ">")
                strTemp = Mid(CodeBuffer, lngStart, CodeCursor - lngStart)
                CodeCursor = CodeCursor + 1
                For i = 1 To Len(strTemp) - 1 Step 2
                    strChar = Chr(Val("&H" & Mid(strTemp, i, 2)))
                    If (strChar >= " ") And (strChar <= "~") Then strValue = strValue & strChar
                Next
                zzPrsReadValue = strValue
            End If

        ElseIf IsNumeric(Mid(CodeBuffer, CodeCursor, 1)) Then
            If Recursive Then
                zzPrsReadValue = zzPrsReadRecurseRef()
            Else
                ' Legge un numero
                lngTemp = zzPrsReadNum()
                zzPrsSkipDummy()
                If IsNumeric(Mid(CodeBuffer, CodeCursor, 1)) Then
                    lngTempR = zzPrsReadNum()
                    zzPrsSkipDummy()
                    If Mid(CodeBuffer, CodeCursor, 1) = "R" Then
                        zzPrsReadValue = CStr(lngTemp) & " " & CStr(lngTempR) & " R"
                    End If
                Else
                    zzPrsReadValue = CStr(lngTemp)
                End If

            End If
            CodeCursor = CodeCursor + 1
        End If

    End Function

    ' Recupera le informazioni dalla tabella 'xref'
    Private Function zzPrsReadXREF(XREFStart As Integer, ByRef ObjRoot As Integer, ByRef ObjInfo As Integer) As Boolean
        Dim strTemp As String = ""
        Dim bytTemp As Byte
        Dim i As Integer
        Dim strDummy As String = Space(20)
        Dim lngStart As Integer
        Dim lngCount As Integer
        Dim lngSize As Integer

        Dim blnFlag As Boolean

        Seek(mvarImpFileNumber, (XREFStart + 1))

        For i = 1 To 4
            FileGet(mvarImpFileNumber, bytTemp)
            strTemp = strTemp & Chr(bytTemp)
        Next

        If strTemp = "xref" Then

            Do
                FileGet(mvarImpFileNumber, bytTemp)
            Loop While (bytTemp = 13) Or (bytTemp = 10) Or (bytTemp = 32)

            blnFlag = True
            Do While blnFlag
                strTemp = ""
                Do While IsNumeric(Chr(bytTemp))
                    strTemp = strTemp & Chr(bytTemp)
                    FileGet(mvarImpFileNumber, bytTemp)
                Loop
                lngStart = Val(strTemp)

                Do While (bytTemp = 32)
                    FileGet(mvarImpFileNumber, bytTemp)
                Loop

                strTemp = ""
                Do While IsNumeric(Chr(bytTemp))
                    strTemp = strTemp & Chr(bytTemp)
                    FileGet(mvarImpFileNumber, bytTemp)
                Loop
                lngCount = Val(strTemp)

                If (lngStart + lngCount - 1) > UBound(mvarObjOffset) Then
                    ReDim Preserve mvarObjOffset(lngStart + lngCount - 1)
                End If

                Do
                    FileGet(mvarImpFileNumber, bytTemp)
                Loop While (bytTemp = 13) Or (bytTemp = 10) Or (bytTemp = 32)
                Seek(mvarImpFileNumber, (Seek(mvarImpFileNumber) - 1))

                For i = lngStart To (lngStart + lngCount - 1)
                    FileGet(mvarImpFileNumber, strDummy)
                    If mvarObjOffset(i) = 0 Then mvarObjOffset(i) = Val(Left$(strDummy, 10))
                Next

                Do
                    FileGet(mvarImpFileNumber, bytTemp)
                Loop While (bytTemp = 13) Or (bytTemp = 10) Or (bytTemp = 32)

                blnFlag = False
                If IsNumeric(Chr(bytTemp)) Then
                    blnFlag = True
                ElseIf Chr(bytTemp) = "t" Then
                    strTemp = Space(1024)
                    FileGet(mvarImpFileNumber, strTemp)

                    ' Read the trailer object
                    zzPrsInitBuffer("t" & strTemp)

                    If zzPrsSeekToken("Encrypt", 1) Then
                        mvarEncrypt = zzPrsReadRef()
                        zzPrsReadXREF = True
                        Exit Function
                    End If


                    If zzPrsSeekToken("Size", 1) Then
                        lngSize = zzPrsReadNum()
                        If lngSize > UBound(mvarObjOffset) Then
                            ReDim Preserve mvarObjOffset(lngSize)
                        End If
                    End If

                    If (ObjInfo = 0) Then If zzPrsSeekToken("Info", 1) Then ObjInfo = zzPrsReadRef()
                    If (ObjRoot = 0) Then If zzPrsSeekToken("Root", 1) Then ObjRoot = zzPrsReadRef()
                    If zzPrsSeekToken("Prev", 1) Then zzPrsReadXREF = zzPrsReadXREF(zzPrsReadNum, ObjRoot, ObjInfo)
                End If
            Loop
        End If

    End Function

    ' Ricava ricorsivamente il valore del parametro
    ' risalendo l'abero attraverso "Parent"
    Private Function zzPrsRecurseValue(ObjText As String, Token As String) As String

        Dim tmpCodeBuffer As String
        Dim tmpCodeCursor As Integer

        tmpCodeBuffer = CodeBuffer
        tmpCodeCursor = CodeCursor

        zzPrsInitBuffer(ObjText)
        If zzPrsSeekToken(Token, 1) Then
            zzPrsRecurseValue = zzPrsReadValue(True)
        Else
            If zzPrsSeekToken("Parent", 1) Then
                zzPrsRecurseValue = zzPrsRecurseValue(zzPrsReadParam(zzPrsReadRef), Token)
            End If
        End If

        CodeBuffer = tmpCodeBuffer
        CodeCursor = tmpCodeCursor

    End Function

    ' Posiziona il cursore all'inizio del token
    Private Function zzPrsSeekToken(ByVal Token As String, _
                                     Optional StartPos As Integer = 0) As Boolean
        Dim lngTemp As Integer

        If StartPos = 0 Then StartPos = CodeCursor
        lngTemp = InStr(StartPos, CodeBuffer, "/" & Token)
        If lngTemp <> 0 Then
            CodeCursor = lngTemp + Len(Token) + 1
            zzPrsSeekToken = True
        End If

    End Function

    ' Salta i caratteri non significativi
    Private Sub zzPrsSkipDummy()
        While Mid(CodeBuffer, CodeCursor, 1) = " " Or _
              Mid(CodeBuffer, CodeCursor, 1) = vbCr Or _
              Mid(CodeBuffer, CodeCursor, 1) = vbLf Or _
              Mid(CodeBuffer, CodeCursor, 1) = vbTab
            CodeCursor = CodeCursor + 1
        End While
    End Sub

    Public Function FromFlateDecode(ByRef ByteArray() As Byte) As String

        Static blnInitFlate As Boolean
        Static LC(31) As Flate_CodesType
        Static DC(31) As Flate_CodesType
        Static LenOrder(18) As Integer

        Dim Inpos As Integer
        Dim BitNum As Integer
        Dim ByteBuff As Integer

        Dim Dist() As Flate_CodesType
        Dim LitLen() As Flate_CodesType
        Dim MinLLenght As Integer
        Dim MaxLLenght As Integer
        Dim MinDLenght As Integer
        Dim MaxDLenght As Integer

        Dim IsLastBlock As Boolean
        Dim CompType As Integer
        Dim DoInflate As Boolean
        Dim [Char] As Integer
        Dim NuBits As Integer
        Dim L1 As Integer
        Dim L2 As Integer
        Dim x As Integer
        Dim LenghtStatic(287) As Integer

        Dim Lenght() As Integer
        Dim Bl_Tree() As Flate_CodesType = Nothing
        Dim MinBL As Integer
        Dim MaxBL As Integer
        Dim NumLen As Integer
        Dim Numdis As Integer
        Dim NumCod As Integer
        Dim CharTree As Integer
        Dim NuBitsTree As Integer
        Dim LN As Integer
        Dim pos As Integer
        Dim blnFlag As Boolean
        Dim Temp()

        zzBStringInit(mvarBufferDecodeStream)
        Erase LitLen
        Erase Dist

        If Not blnInitFlate Then
            blnInitFlate = True
            Temp = {16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15}
            For x = 0 To UBound(Temp) : LenOrder(x) = Temp(x) : Next
            Temp = {3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 15, 17, 19, 23, 27, 31, 35, 43, 51, 59, 67, 83, 99, 115, 131, 163, 195, 227, 258}
            For x = 0 To UBound(Temp) : LC(x).Code = Temp(x) : Next
            Temp = {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0}
            For x = 0 To UBound(Temp) : LC(x).Lenght = Temp(x) : Next
            Temp = {1, 2, 3, 4, 5, 7, 9, 13, 17, 25, 33, 49, 65, 97, 129, 193, 257, 385, 513, 769, 1025, 1537, 2049, 3073, 4097, 6145, 8193, 12289, 16385, 24577, 32769, 49153}
            For x = 0 To UBound(Temp) : DC(x).Code = Temp(x) : Next
            Temp = {0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14}
            For x = 0 To UBound(Temp) : DC(x).Lenght = Temp(x) : Next
            For x = 0 To 16
                Pwr2(x) = 2 ^ x
            Next
        End If

        Inpos = 0
        ByteBuff = 0
        BitNum = 0

        Do
            IsLastBlock = (zzInFlate_GetBits(1, ByteArray, Inpos, BitNum, ByteBuff) = 1)
            CompType = zzInFlate_GetBits(2, ByteArray, Inpos, BitNum, ByteBuff)
            Select Case CompType
                Case 0
                    BitNum = 0
                    ByteBuff = 0
                    L1 = ByteArray(Inpos) + CInt(ByteArray(Inpos + 1)) * 256
                    Inpos = Inpos + 2
                    L2 = ByteArray(Inpos) + CInt(ByteArray(Inpos + 1)) * 256
                    Inpos = Inpos + 2
                    If L1 - (Not (L2) And &HFFFF&) Then FromFlateDecode = -2

                    For x = 1 To L1
                        zzBStringAddChar(mvarBufferDecodeStream, ByteArray(Inpos))
                        Inpos = Inpos + 1
                    Next
                    DoInflate = False
                Case 1

                    For x = 0 To 143 : LenghtStatic(x) = 8 : Next
                    For x = 144 To 255 : LenghtStatic(x) = 9 : Next
                    For x = 256 To 279 : LenghtStatic(x) = 7 : Next
                    For x = 280 To 287 : LenghtStatic(x) = 8 : Next
                    If zzInFlate_CreateCodes(LitLen, LenghtStatic, 287, MaxLLenght, MinLLenght) = 0 Then
                        For x = 0 To 31 : LenghtStatic(x) = 5 : Next
                        Call zzInFlate_CreateCodes(Dist, LenghtStatic, 31, MaxDLenght, MinDLenght)
                    End If
                    DoInflate = True
                Case 2

                    blnFlag = True

                    NumLen = zzInFlate_GetBits(5, ByteArray, Inpos, BitNum, ByteBuff) + 257
                    Numdis = zzInFlate_GetBits(5, ByteArray, Inpos, BitNum, ByteBuff) + 1
                    NumCod = zzInFlate_GetBits(4, ByteArray, Inpos, BitNum, ByteBuff) + 4
                    ReDim Lenght(18)

                    For x = 0 To NumCod - 1
                        Lenght(LenOrder(x)) = zzInFlate_GetBits(3, ByteArray, Inpos, BitNum, ByteBuff)
                    Next
                    For x = NumCod To 18
                        Lenght(LenOrder(x)) = 0
                    Next

                    If zzInFlate_CreateCodes(Bl_Tree, Lenght, 18, MaxBL, MinBL) = 0 Then
                        ReDim Lenght(NumLen + Numdis)
                        pos = 0
                        Do While (pos < NumLen + Numdis) And blnFlag
                            CharTree = zzInFlate_BitReverse(zzInFlate_GetBits(MinBL, ByteArray, Inpos, BitNum, ByteBuff), MinBL)
                            NuBitsTree = MinBL
                            Do While Bl_Tree(CharTree).Lenght <> NuBitsTree
                                CharTree = CharTree + CharTree + zzInFlate_GetBits(1, ByteArray, Inpos, BitNum, ByteBuff)
                                NuBitsTree = NuBitsTree + 1
                            Loop
                            CharTree = Bl_Tree(CharTree).Code
                            If CharTree < 16 Then
                                Lenght(pos) = CharTree
                                pos = pos + 1
                            Else
                                If CharTree = 16 Then
                                    If pos = 0 Then
                                        blnFlag = False
                                        Exit Do
                                    End If
                                    LN = Lenght(pos - 1)
                                    CharTree = 3 + zzInFlate_GetBits(2, ByteArray, Inpos, BitNum, ByteBuff)
                                ElseIf CharTree = 17 Then
                                    CharTree = 3 + zzInFlate_GetBits(3, ByteArray, Inpos, BitNum, ByteBuff)
                                    LN = 0
                                Else
                                    CharTree = 11 + zzInFlate_GetBits(7, ByteArray, Inpos, BitNum, ByteBuff)
                                    LN = 0
                                End If
                                If pos + CharTree > NumLen + Numdis Then
                                    blnFlag = False
                                    Exit Do
                                End If
                                Do While CharTree > 0
                                    CharTree = CharTree - 1
                                    Lenght(pos) = LN
                                    pos = pos + 1
                                Loop
                            End If
                        Loop
                        If (zzInFlate_CreateCodes(LitLen, Lenght, NumLen - 1, MaxLLenght, MinLLenght) = 0) And blnFlag Then
                            For x = 0 To Numdis
                                Lenght(x) = Lenght(x + NumLen)
                            Next
                            Call zzInFlate_CreateCodes(Dist, Lenght, Numdis - 1, MaxDLenght, MinDLenght)
                        End If
                    End If

                    DoInflate = True
                Case 3
                    FromFlateDecode = -1
                    DoInflate = False
            End Select
            If DoInflate Then
                Do
                    [Char] = zzInFlate_BitReverse(zzInFlate_GetBits(MinLLenght, ByteArray, Inpos, BitNum, ByteBuff), MinLLenght)
                    NuBits = MinLLenght
                    Do While LitLen([Char]).Lenght <> NuBits
                        [Char] = [Char] + [Char] + zzInFlate_GetBits(1, ByteArray, Inpos, BitNum, ByteBuff)
                        NuBits = NuBits + 1
                    Loop
                    [Char] = LitLen([Char]).Code
                    If [Char] < 256 Then
                        zzBStringAddChar(mvarBufferDecodeStream, CByte([Char]))
                    ElseIf [Char] > 256 Then
                        [Char] = [Char] - 257
                        L1 = LC([Char]).Code + zzInFlate_GetBits(LC([Char]).Lenght, ByteArray, Inpos, BitNum, ByteBuff)
                        '          If L1 = 258 And ZIP64 Then L1 = zzInFlate_GetBits(16, ByteArray, Inpos, BitNum, ByteBuff) + 3
                        [Char] = zzInFlate_BitReverse(zzInFlate_GetBits(MinDLenght, ByteArray, Inpos, BitNum, ByteBuff), MinDLenght)
                        NuBits = MinDLenght
                        Do While Dist([Char]).Lenght <> NuBits
                            [Char] = [Char] + [Char] + zzInFlate_GetBits(1, ByteArray, Inpos, BitNum, ByteBuff)
                            NuBits = NuBits + 1
                        Loop
                        [Char] = Dist([Char]).Code
                        L2 = DC([Char]).Code + zzInFlate_GetBits(DC([Char]).Lenght, ByteArray, Inpos, BitNum, ByteBuff)
                        For x = 1 To L1
                            zzBStringAdd(mvarBufferDecodeStream, Mid(zzBStringValue(mvarBufferDecodeStream), ((mvarBufferDecodeStream.Pointer + 1) / 2) - L2, 1))
                        Next
                    End If
                Loop While [Char] <> 256
            End If
        Loop While Not IsLastBlock
        Erase LitLen
        Erase Dist

        FromFlateDecode = zzBStringValue(mvarBufferDecodeStream)
    End Function

    Private Function zzInFlate_BitReverse(ByVal Value As Integer, ByVal Numbits As Integer) As Integer
        Dim ret As Integer = 0
        Do While Numbits > 0
            ret = ret * 2 + (Value And 1)
            Numbits = Numbits - 1
            Value = Fix(Value / 2)
        Loop
        Return ret
    End Function

    Private Function zzInFlate_CreateCodes(tree() As Flate_CodesType, Lenghts() As Integer, NumCodes As Integer, MaxBits As Integer, Minbits As Integer) As Integer
        Dim BITS(16) As Integer
        Dim next_code(16) As Integer
        Dim Code As Integer
        Dim LN As Integer
        Dim x As Integer

        Minbits = 16
        For x = 0 To NumCodes
            BITS(Lenghts(x)) = BITS(Lenghts(x)) + 1
            If Lenghts(x) > MaxBits Then MaxBits = Lenghts(x)
            If Lenghts(x) < Minbits And Lenghts(x) > 0 Then Minbits = Lenghts(x)
        Next

        LN = 1
        For x = 1 To MaxBits
            LN = LN + LN
            LN = LN - BITS(x)
            If LN < 0 Then zzInFlate_CreateCodes = LN : Exit Function 'Over subscribe, Return negative
        Next

        zzInFlate_CreateCodes = LN
        ReDim tree(2 ^ MaxBits - 1)
        Code = 0
        BITS(0) = 0
        For x = 1 To MaxBits
            Code = (Code + BITS(x - 1)) * 2
            next_code(x) = Code
        Next

        For x = 0 To NumCodes
            LN = Lenghts(x)
            If LN <> 0 Then
                tree(next_code(LN)).Lenght = LN
                tree(next_code(LN)).Code = x
                next_code(LN) = next_code(LN) + 1
            End If
        Next
    End Function

    Private Function zzInFlate_GetBits(Numbits As Integer, _
                                       ByRef InStream() As Byte, _
                                       ByRef Inpos As Integer, _
                                       ByRef BitNum As Integer, _
                                       ByRef ByteBuff As Integer) As Integer
        If BitNum < Numbits Then
            Do
                ByteBuff = ByteBuff + (InStream(Inpos) * Pwr2(BitNum))
                BitNum = BitNum + 8
                Inpos = Inpos + 1
            Loop While BitNum < Numbits
        End If
        zzInFlate_GetBits = ByteBuff And (Pwr2(Numbits) - 1)
        ByteBuff = Fix(ByteBuff / Pwr2(Numbits))
        BitNum = BitNum - Numbits
    End Function

    ' ----------------------------------------------------------------------------------------
    ' Funzioni per la gestione di stringhe bufferizzate
    ' ----------------------------------------------------------------------------------------
    '
    ' Aggiunge una stringa
    Private Sub zzBStringAdd(ByRef Item As BufferString, ByRef Value As String)
        Dim PointerNew As Integer

        PointerNew = Item.Pointer + Len(Value)

        If PointerNew > Item.BufferLen Then
            Item.Buffer = Item.Buffer & Space(PointerNew)
            Item.BufferLen = Item.BufferLen + PointerNew 'LenB(Item.Buffer)
        End If

        Mid(Item.Buffer, Item.Pointer) = Value
        Item.Pointer = PointerNew
    End Sub

    ' Aggiunge un carattere (sottoforma di byte)
    Private Sub zzBStringAddChar(ByRef Item As BufferString, ByRef Value As Byte)
        Dim PointerNew As Integer

        PointerNew = Item.Pointer + 2
        If PointerNew > Item.BufferLen Then
            Item.Buffer = Item.Buffer & Space(PointerNew)
            Item.BufferLen = Item.BufferLen + PointerNew
        End If

        Mid(Item.Buffer, Item.Pointer) = Chr(Value)
        Item.Pointer = PointerNew
    End Sub

    ' Inizializza il buffer
    Private Sub zzBStringInit(ByRef Item As BufferString)
        Item.Pointer = 1
        Item.BufferLen = 0
        Item.Buffer = vbNullString
    End Sub

    '' Restituisce la lunghezza della stringa
    'Private Function zzBStringLength(ByRef Item As BufferString) As Integer
    '  zzBStringLength = Item.Pointer \ 2
    'End Function

    ' Restituisce la stringa
    Private Function zzBStringValue(ByRef Item As BufferString) As String
        zzBStringValue = Left(Item.Buffer, Item.Pointer - 1)
    End Function
    ' ----------------------------------------------------------------------------------------


End Class
