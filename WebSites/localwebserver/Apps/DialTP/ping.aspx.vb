Imports System.IO


Partial Class ping
    Inherits System.Web.UI.Page 'Inherits FinSeA.WebPage

    
    Private Function Puro(ByVal kName As String) As String
        Dim chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_"
        Dim ret As String = ""
        For i As Integer = 1 To Len(kName)
            Dim ch As String = Mid(kName, i, 1)
            If (InStr(chars, ch) <= 0) Then
                ret &= "_"
            Else
                ret &= ch
            End If
        Next
        Return ret
    End Function

    Private Function DataPath() As String
        Dim d As Date = Now
        Return Right("0000" & d.Year, 4) & Right("00" & d.Month, 2) & Right("00" & d.Day, 2) & Right("00" & d.Hour, 2) & Right("00" & d.Minute, 2) & Right("00" & d.Second, 2)
    End Function

    Private Function MakeFileName(ByVal kName As String) As String
        Return "/App_data/DialTP/Pings/" & Me.Puro(kName) & "/" & Me.Puro(kName) & "_" & Me.DataPath & ".dtp"
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Server.ScriptTimeout = 120 * 10
        If (Request.Files.Count = 0) Then Exit Sub

        Dim objFile As HttpPostedFile = Request.Files(0)
        Dim fileName As String = Nothing


        'Prepariamo il percorso destinazione
        Dim kname As String = Request.QueryString("kname")
        If (kname = "") Then kname = Request.ServerVariables("REMOTE_ADDRESS")
        Dim kURL As String = Me.MakeFileName(kname)
        Dim kFile As String = Server.MapPath(kURL)
        Dim kDir As String = GetFolderName(kFile)
        CreateFolder(kDir)

        'Me.CurrentUpload = FinSeA.Web.Uploader.CreateUploader(Request.QueryString("rk"), Me.m_DestFile, False)
        objFile.SaveAs(kFile)
    End Sub

    ''' <summary>
    ''' Estrae il percorso del folder che contiene il file o la cartella
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFolderName(ByVal path As String) As String
        Dim i As Integer
        Dim ret As String
        i = InStrRev(path, "\")
        If (i > 0) Then
            ret = Left(path, i - 1)
        Else
            ret = path
        End If
        Return ret
    End Function

    ''' <summary>
    ''' Crea il percorso (se il percorso esiste genera errore)
    ''' </summary>
    ''' <param name="path"></param>
    ''' <remarks></remarks>
    Public Sub CreateFolder(ByVal path As String)
        System.IO.Directory.CreateDirectory(path)
    End Sub

#Region " A Function To Cleanup The Directories "
    Private Sub CleanupDirectories()
        Dim strDirectories() As String = Directory.GetDirectories(Server.MapPath("Upload"))
        Dim strFiles() As String = Nothing
        Dim fiInfo As FileInfo = Nothing
        Dim diInfo As DirectoryInfo = Nothing

        For i As Integer = 0 To strDirectories.Length - 1
            diInfo = New DirectoryInfo(strDirectories(i))
            strFiles = Directory.GetFiles(strDirectories(i))

            If diInfo.Name.Equals(Request.QueryString("id")) Then
                For j As Integer = 0 To strFiles.Length - 1
                    File.Delete(strFiles(j))
                Next
            ElseIf diInfo.CreationTime.AddHours(1) < DateTime.Now Then
                For j As Integer = 0 To strFiles.Length - 1
                    fiInfo = New FileInfo(strFiles(j))
                    If fiInfo.CreationTime.AddHours(1) < DateTime.Now Then
                        File.Delete(strFiles(j))
                    End If
                Next

                diInfo.Attributes = FileAttributes.Hidden
            End If
        Next
    End Sub
#End Region

End Class
