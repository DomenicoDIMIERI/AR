'<summary>
'----------------------------------------------

'----------------------------------------------
' Comment: Http Util - Utility to perform the cleanup of the temporary files which have been uploaded to the server.
' Author: Amit Champaneri
' Date Created : 14th March 2007
'----------------------------------------------
'</summary>

#Region " All The Imports "
Imports System.IO
#End Region

#Region " The [HttpUtil] Class "
Partial Class HttpUtil1
    Inherits System.Web.UI.Page

#Region " Page Events "

#Region " The [Page Load] Event "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        Dim objFile As HttpPostedFile = Request.Files(0)
        Dim fileName As String = Nothing

        If Not Directory.Exists(Server.MapPath("Upload")) Then
            Directory.CreateDirectory(Server.MapPath("Upload"))
        End If

        If Not Directory.Exists(Server.MapPath("Upload\" + Request.QueryString("id"))) Then
            Directory.CreateDirectory(Server.MapPath("Upload\" + Request.QueryString("id")))
        End If

        fileName = objFile.FileName.Substring(objFile.FileName.LastIndexOf("\") + 1)

        objFile.SaveAs(Server.MapPath("Upload") + "\" + Request.QueryString("id") + "\" + fileName)
        Response.End()

    End Sub
#End Region

#End Region

    'Created - Function to cleanup the directory.
    'By Kirtan Gor
    '14th March 2007

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
#End Region