Imports DMD
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.WebSite

Namespace Internals


    Public NotInheritable Class CUploaderClass
        Public Event UploadBegin(ByVal e As UploadEventArgs)
        Public Event UploadError(ByVal e As UploadErrorEventArgs)
        Public Event UploadCompleted(ByVal e As UploadEventArgs)
        Public Event UploadProgress(ByVal e As UploadEventArgs)

        Public ReadOnly Property lock As New Object

        Private m_Uploads As New CCollection(Of CFileUploader)


        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Public ReadOnly Property UploadedFiles As CCollection(Of CFileUploader)
            Get
                SyncLock Me.lock
                    Return New CCollection(Of CFileUploader)(Me.m_Uploads)
                End SyncLock
            End Get
        End Property

        Public Function IsUploading(ByVal key As String) As Boolean
            Dim u As CFileUploader = Me.GetUpload(key)
            If (u Is Nothing) Then Return False
            Return Not u.IsCompleted
        End Function

        Friend Sub FireBeginUpload(ByVal e As UploadEventArgs)
            RaiseEvent UploadBegin(e)
        End Sub

        Friend Sub FireEndUpload(ByVal e As UploadEventArgs)
            RaiseEvent UploadCompleted(e)
        End Sub

        Friend Sub FireUploadError(ByVal e As UploadEventArgs)
            RaiseEvent UploadError(e)
        End Sub

        Friend Sub FireUploadProgress(ByVal e As UploadEventArgs)
            RaiseEvent UploadProgress(e)
        End Sub

        Public Function CreateUploader(Of T As CFileUploader)() As CFileUploader
            SyncLock Me.lock
                Dim limit As Integer = WebSite.Instance.Configuration.NumberOfUploadsLimit
                If (limit > 0) AndAlso (Me.TotalNumberOfUploads > limit) Then
                    Throw New UploadCalcelledException("Superato il numero massimo di upload consentiti")
                End If
                Dim u As CFileUploader = Sistema.Types.CreateInstance(GetType(T))
                Dim key As String = Sistema.ASPSecurity.GetRandomKey(25)
                While (Me.GetUpload(key) IsNot Nothing)
                    key = Sistema.ASPSecurity.GetRandomKey(25)
                End While
                u.SetKey(key)
                m_Uploads.Add(u)
                Return u
            End SyncLock
        End Function

        Public Sub RemoveUploader(ByVal u As CFileUploader)
            If (u IsNot Nothing) Then m_Uploads.Remove(u)
        End Sub

        ''' <summary>
        ''' Restituisce il numero totale di uploads in corso nell'ambito dell'applicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TotalNumberOfUploads As Integer
            Get
                SyncLock Me.lock
                    Dim timeOut As Integer = WebSite.Instance.Configuration.UploadTimeOut
                    Dim i, cnt As Integer
                    i = 0 : cnt = 0
                    While (i < Me.m_Uploads.Count)
                        Dim u As CFileUploader = Me.m_Uploads(i)
                        If (u.IsCompleted) Then
                            'Me.m_Uploads.RemoveByKey(u.Key)
                        Else
                            If (timeOut > 0) AndAlso Calendar.DateDiff(DateInterval.Second, u.StartTime, Now) > timeOut + 10 Then
                                u.Abort()
                            Else
                                cnt += 1
                            End If
                        End If
                        i += 1
                    End While
                    Return cnt
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'oggetto Upload con l'ID specifico
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUpload(ByVal key As String) As CFileUploader
            SyncLock Me.lock
                For Each u As CFileUploader In Me.m_Uploads
                    If (u.Key = key) Then Return u
                Next
                Return Nothing
            End SyncLock
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace

Partial Class WebSite


    Private Shared m_Uploader As CUploaderClass = Nothing

    Public Shared ReadOnly Property Uploader As CUploaderClass
        Get
            If (m_Uploader Is Nothing) Then m_Uploader = New CUploaderClass
            Return m_Uploader
        End Get
    End Property


End Class