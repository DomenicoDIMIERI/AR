Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.IO


Namespace Net.Mime


    Public NotInheritable Class MIME
        Friend Sub New()
        End Sub

        Public Shared Function GetMimeTypeFromExtension(ByVal extensionName As String) As String
            Dim fileType, mimeType As String
            fileType = FileSystem.GetExtensionName(extensionName)
            Select Case LCase(Trim(fileType))
                Case "asf" : mimeType = "video/x-ms-asf"
                Case "avi" : mimeType = "video/avi"
                Case "bmp" : mimeType = "image/bmp"
                Case "doc" : mimeType = "application/msword"
                Case "zip" : mimeType = "application/zip"
                Case "xls" : mimeType = "application/vnd.ms-excel"
                Case "gif" : mimeType = "image/gif"
                Case "jpg", "jpeg" : mimeType = "image/jpeg"
                Case "wav" : mimeType = "audio/wav"
                Case "mp3" : mimeType = "audio/mpeg3"
                Case "mpg", "mpeg" : mimeType = "video/mpeg"
                Case "rtf" : mimeType = "application/rtf"
                Case "htm", "html" : mimeType = "text/html"
                Case "asp" : mimeType = "text/asp"
                Case "pdf" : mimeType = "application/pdf"
                Case "mdb" : mimeType = "application/msaccess, application/x-msaccess, application/vnd.msaccess, application/vnd.ms-access, application/mdb, application/x-mdb, zz-application/zz-winassoc-mdb"
                Case Else : mimeType = "application/octet-stream"
            End Select

            Return mimeType
        End Function

        Public Shared Function GetDefaultContentDispositionExtension(ByVal extensionName As String) As String
            'Dim fileType, disposition As String
            'fileType = FileSystem.GetExtensionName(extensionName)
            Dim filetype As String = extensionName
            Dim disposition As String
            Select Case LCase(Trim(filetype))
                Case "asf" : disposition = "inline"
                Case "avi" : disposition = "inline"
                Case "bmp" : disposition = "inline"
                Case "doc" : disposition = "attachment"
                Case "zip" : disposition = "attachment"
                Case "xls" : disposition = "attachment"
                Case "gif" : disposition = "inline"
                Case "jpg", "jpeg" : disposition = "inline"
                Case "wav" : disposition = "attachment"
                Case "mp3" : disposition = "attachment"
                Case "mpg", "mpeg" : disposition = "inline"
                Case "rtf" : disposition = "attachment"
                Case "htm", "html" : disposition = "inline"
                Case "asp" : disposition = "inline"
                Case "pdf" : disposition = "inline"
                Case "mdb" : disposition = "attachment"
                Case Else : disposition = "attachment"
            End Select

            Return disposition
        End Function

    End Class

End Namespace