VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsgTicketUtil"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'-------------------------------------------------------------------------------------------
Dim WinHttpReq As WinHttp.WinHttpRequest
Const HTTPREQUEST_SETCREDENTIALS_FOR_SERVER = 0
Const HTTPREQUEST_SETCREDENTIALS_FOR_PROXY = 1
Const BOUNDARY = "Xu02=$"
Const HEADER = "--Xu02=$"
Const FOOTER = "--Xu02=$--"
'-------------------------------------------------------------------------------------------
Const vbCFText = 1              ' Plain Text
Const vbCFBitmap = 2            ' Bitmap
Const vbCFMetafile = 3          ' Metafile Picture Format
Const vbCFDIB = 8               ' Device Independent Bitmap FOrmat
Const vbCFPalette = 9           ' Color Palette
Const vbCFEMetafile = 14        ' Enhanced Metafile Format
Const vbCFFiles = 15            ' Files
Const vbCFLink = -16640         ' Link
Const vbCFRTF = -16639          ' Rich Text Format


' Clipboard routines.
Private Declare Function OpenClipboard Lib "USER32" (ByVal hWnd As Long) As Long
Private Declare Function CloseClipboard Lib "USER32" () As Long
Private Declare Function SetClipboardData Lib "USER32" (ByVal wFormat As Long, ByVal hMem As Long) As Long
Private Declare Function GetClipboardData Lib "USER32" (ByVal wFormat As Long) As Long

Private Declare Function DragQueryFile Lib "shell32.dll" Alias "DragQueryFileA" (ByVal drop_handle As Long, ByVal UINT As Long, ByVal lpStr As String, ByVal ch As Long) As Long

' File list clipboard format code.
Private Const CF_HDROP = 15

' DROPFILES data structure.
Private Type POINTAPI
    X As Long
    y As Long
End Type
Private Type DROPFILES
    pFiles As Long
    pt As POINTAPI
    fNC As Long
    fWide As Long
End Type

' Global memory routines.
Private Declare Function GlobalAlloc Lib "kernel32" (ByVal wFlags As Long, ByVal dwBytes As Long) As Long
Private Declare Function GlobalFree Lib "kernel32" (ByVal hMem As Long) As Long
Private Declare Function GlobalLock Lib "kernel32" (ByVal hMem As Long) As Long
Private Declare Function GlobalUnlock Lib "kernel32" (ByVal hMem As Long) As Long
Private Declare Sub CopyMem Lib "kernel32" Alias "RtlMoveMemory" (Destination As Any, Source As Any, ByVal Length As Long)

' Global Memory Flags
Private Const GMEM_MOVEABLE = &H2
Private Const GMEM_ZEROINIT = &H40
Private Const GHND = (GMEM_MOVEABLE Or GMEM_ZEROINIT)


'Function to check if file is valid or not.
Public Function isValidFile(strFileName As String) As Boolean
Dim arr() As String
Dim strExt As String

arr = Split(strFileName, ".")
strExt = arr(UBound(arr))

strExt = LCase(strExt)

If (strExt = "jpg" Or strExt = "bmp" Or strExt = "gif") Then
isValidFile = True
Else
isValidFile = False
End If

End Function
 

Private Function ClipboardGetFiles() As String()
Dim drop_handle As Long
Dim num_file_names As Long
Dim file_names() As String
Dim file_name As String * 1024
Dim i As Long

    ' Make sure there is file data.
    If Clipboard.GetFormat(vbCFFiles) Then
        ' File data exists. Get it.
        ' Open the clipboard.
        If OpenClipboard(0) Then
            ' The clipboard is open.

            ' Get the handle to the dropped list of files.
            drop_handle = GetClipboardData(CF_HDROP)

            ' Get the number of dropped files.
            num_file_names = DragQueryFile(drop_handle, -1, _
                vbNullString, 0)

            ' Get the file names.
            ReDim file_names(1 To num_file_names) As String
            For i = 1 To num_file_names
                ' Get the file name.
                DragQueryFile drop_handle, i - 1, _
                    file_name, Len(file_name)

                ' Truncate at the NULL character.
                file_names(i) = Left$(file_name, _
                    InStr(file_name, vbNullChar) - 1)
            Next

            ' Close the clipboard.
            CloseClipboard

            ' Assign the return value.
            ClipboardGetFiles = file_names
        End If
    End If
End Function

' Function To Create Target Dir
Private Function createTargetDir(strTargetDirPath) As Boolean
On Error GoTo LableError

If Dir(strTargetDirPath, vbDirectory) <> "" Then
    createTargetDir = True
    Exit Function
Else
    Dim a() As String
    Dim i As Integer
    a = Split(strTargetDirPath, "\")
    Dim strCreatePath As String
    strCreatePath = a(0)
    
    For i = 1 To UBound(a)
        strCreatePath = strCreatePath & "\" & a(i)
        If Dir(strCreatePath, vbDirectory) = "" Then
            MkDir strCreatePath
        End If
    Next i
    createTargetDir = True
    Exit Function
End If

LableError:
    createTargetDir = False
End Function

Public Function getCopiedImage(strDirPath As String) As String
Dim str() As String
Dim objPic As StdPicture
Dim strTargetFilePath As String
Dim strFileName As String

strFileName = Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now) & ".jpg"
    
    If Clipboard.GetFormat(vbCFFiles) Then
        str = ClipboardGetFiles()
        strTargetFilePath = str(1)
        If isValidFile(strTargetFilePath) = False Then
           strTargetFilePath = "ERR1" & strTargetFilePath
        End If
    ElseIf Clipboard.GetFormat(vbCFBitmap) Then
     
        strTargetFilePath = strDirPath & "\" & strFileName
        
        'Checking Target Dir Exits or Not,if not creating it.
        If createTargetDir(strDirPath) = False Then
           'This means,function was failed in creating Tartet Dir path.
           getCopiedImage = "ERR4"
           Exit Function
        End If
        
         Call SaveAsJPG.SaveJPG(Clipboard.GetData(vbCFBitmap), strTargetFilePath)
        
    Else
        strTargetFilePath = "ERR2"
    End If
    'Set getCopiedImage = objPic
    getCopiedImage = strTargetFilePath
End Function
Private Sub UserControl_Initialize()
Clipboard.Clear
End Sub

'---------------------------------------------------------------------------------------------------

Function UploadFiles(strFileName1 As String, strUrl As String, Optional postVar As String, _
                     Optional strUserName As String, Optional strPassword As String) As String

    Dim fname As String
    Dim strFile As String
    Dim strBody As String
    Dim aPostBody() As Byte
    Dim nFile As Integer
    
    Dim strFileName(0) As String
    strFileName(0) = strFileName1
        
    Set WinHttpReq = New WinHttpRequest
    
    
    ' Turn error trapping on
    On Error GoTo SaveErrHandler

    ' Assemble an HTTP request.
    strUrl = strUrl & "?slots=" & CStr(UBound(strFileName) + 1) & "&" & postVar
    WinHttpReq.Open "POST", strUrl, False
    Debug.Print strUrl
    
    If strUserName <> "" And strPassword <> "" Then
        ' Set the user name and password.
        WinHttpReq.SetCredentials strUserName, strPassword, _
        HTTPREQUEST_SETCREDENTIALS_FOR_SERVER
    End If
    
    '-------------------------- Becareful not to mingle too much here -----------------------------------
    
    ' Set the header
    WinHttpReq.SetRequestHeader "Content-Type", "multipart/form-data; boundary=" & BOUNDARY
       
    ' Assemble the body
    strBody = HEADER                        ' Starting tag
    
    For i = 0 To UBound(strFileName)
    
        ' Grap the name
        fname = strFileName(i)
        
        ' Grap the file
        strFile = getFile(fname)
        
            strBody = strBody & vbCrLf & "Content-Disposition: form-data; name=""" & "upload" & _
            (i + 1) & """; filename=""" & fname & """ " & vbCrLf & "Content-type: file" & _
            vbCrLf & vbCrLf & strFile & vbCrLf
        
        If i < UBound(strFileName) Then
            strBody = strBody & "--Xu02=$" ' This is boundary tag between two files
        End If
        strFile = ""
        
    Next i
    
    strBody = strBody & FOOTER             ' Ending tag
    '----------------------------------------------------------------------------------------------------
    
    ' Because of binary zeros, post body has to convert to byte array
    aPostBody = StrConv(strBody, vbFromUnicode)
    
    ' Send the HTTP Request.
    WinHttpReq.Send aPostBody
    
    ' Display the status code and response headers.
    'UploadFiles = WinHttpReq.GetAllResponseHeaders & "  " & WinHttpReq.ResponseText
    UploadFiles = WinHttpReq.ResponseText
    
    Set WinHttpReq = Nothing
    
    Exit Function
    
SaveErrHandler:
    UploadFiles = Err.Description
    
    Set WinHttpReq = Nothing
    
End Function
Function getFile(strFileName As String) As String

    Dim strFile As String
    
    ' Grap the file
    nFile = FreeFile
    Open strFileName For Binary As #nFile
    strFile = String(LOF(nFile), " ")
    Get #nFile, , strFile
    Close #nFile
    
    getFile = strFile
    
End Function

