Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    ''' <summary>
    ''' Rappresenta le informazioni registrate nel DB relativamente ad un upload
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CUploadedFilesClass
        Inherits CGeneralClass(Of CUploadedFile)


        Friend Sub New()
            MyBase.New("modUploadedFiles", GetType(CUploadedFileCursor), 0)
        End Sub

        Public Function GetItemByKey(ByVal k As String) As CUploadedFile
            k = Strings.Trim(k)
            If (k = "") Then Return Nothing

            Dim cursor As New CUploadedFileCursor
            Dim ret As CUploadedFile
            cursor.IgnoreRights = True
            cursor.PageSize = 1
            cursor.Key.Value = k
            ret = cursor.Item
            cursor.Dispose()

            Return ret
        End Function



    End Class

    Private Shared m_UploadedFiles As CUploadedFilesClass = Nothing

    Public Shared ReadOnly Property Uploads As CUploadedFilesClass
        Get
            If (m_UploadedFiles Is Nothing) Then m_UploadedFiles = New CUploadedFilesClass
            Return m_UploadedFiles
        End Get
    End Property

End Class
