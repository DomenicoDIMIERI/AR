Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.Threading
Imports System.Web.UI.HtmlControls
Imports System.IO

Partial Class WebSite



    Public Class CNetFileUploader
        Inherits CFileUploader

   
        Private m_OutputStream As System.IO.Stream
        Private m_InputStream As System.IO.Stream
        Private m_Field As System.Web.UI.HtmlControls.HtmlInputFile
        ' Private ascb As New AsyncCallback(AddressOf HandleReceive)

        'Private ascb As New AsyncCallback(AddressOf HandleReceive)

        Public Sub New()
        End Sub

        Public Overrides Sub Dispose()
            MyBase.Dispose()
            If (Me.m_OutputStream IsNot Nothing) Then Me.m_OutputStream.Dispose() : Me.m_OutputStream = Nothing
            If (Me.m_InputStream IsNot Nothing) Then Me.m_InputStream = Nothing
            Me.m_Field = Nothing
            ' Me.ascb = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce il nome del campo del form da cui è stato caricato il file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property FieldName As String
            Get
                Return Me.m_Field.ClientID
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il percorso del file sul PC remoto (da cui è stato caricato)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property FieldValue As String
            Get
                Return Me.m_Field.Value
            End Get
        End Property


        Protected Overrides Sub PrepareUpload()
            'If Me.FileName = vbNullString Then
            '    Do
            '        Me.TargetFileName = ASPSecurity.GetRandomKey(8) & "." & FileSystem.GetExtensionName(Me.FieldValue)
            '    Loop While FileSystem.FileExists(Me.TargetFileName)
            'End If

            Me.m_InputStream = Me.m_Field.PostedFile.InputStream

            'DMD.Sistema.FileSystem.CreateRecursiveFolder(DMD.Sistema.FileSystem.GetFolderName (Me.BasePath))

            Me.m_OutputStream = New System.IO.FileStream(Me.TargetFileName, FileMode.CreateNew)
        End Sub

        Protected Overrides Function GetTotalBytesToReceive() As Long
            Return Me.m_Field.PostedFile.ContentLength
        End Function

        ''' <summary>
        ''' Avvia il caricamento
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetFields(ByVal filed As HtmlInputFile, ByVal destURL As String, ByVal destFile As String, ByVal async As Boolean)
            Me.m_Field = filed
            Me.DestinationUrl = destURL
            Me.TargetFileName = destFile
            Me.Async = async
        End Sub

        'Protected Overrides Sub InternalUploadAsync()
        '    Me.Receive(ascb)
        '    Dim ret As Boolean
        '    If (WebSite.Instance.Configuration.UploadTimeOut > 0) Then
        '        ret = Me.m_ManualReset.WaitOne(WebSite.Instance.Configuration.UploadTimeOut * 1000)
        '    Else
        '        ret = Me.m_ManualReset.WaitOne()
        '    End If
        '    If (ret = False) Then
        '        Throw New TimeoutException
        '    End If
        'End Sub

        'Private Sub Receive(ByVal cb As AsyncCallback)
        '    'Debug.Print("CFileUploade>Receive")
        '    Me.m_InputStream.BeginRead(Me.buffer, 0, Me.BufferSize, cb, Nothing)
        'End Sub

        '''' <summary>
        '''' Gets the single line response callback.
        '''' </summary>
        '''' <param name="ar">The ar.</param>
        'Protected Sub HandleReceive(ByVal ar As IAsyncResult)
        '    Try
        '        If (Me.IsAborting) Then Throw New UploadCalcelledException("Annullato dall'utente")

        '        Dim nRead As Integer = Me.m_InputStream.EndRead(ar)
        '        Me.m_BytesReceived += nRead
        '        Me.DoConsumer(Me.buffer, nRead)
        '        Uploader.FireUploadProgress(New UploadEventArgs(Me))

        '        If (Me.UploadedBytes >= Me.TotalBytes) Then
        '            Me.FinalizeUpload()
        '            Uploader.FireEndUpload(New UploadEventArgs(Me))
        '            Me.m_ManualReset.Set()
        '        Else
        '            Me.LimitateSpeed(nRead)
        '            Me.m_LastRead = Now
        '            Me.Receive(ascb)
        '        End If


        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Me.FinalizeUpload()
        '        Me.OnUploadError(New UploadErrorEventArgs(Me, ex))
        '        Me.m_ManualReset.Set()
        '    End Try

        'End Sub


        Protected Overrides Sub FinalizeUpload()
            If (Me.m_OutputStream IsNot Nothing) Then Me.m_OutputStream.Dispose() : Me.m_OutputStream = Nothing
            'If (Me.m_InputStream IsNot Nothing) Then Me.m_InputStream.Dispose()

        End Sub

        ''' <summary>
        ''' Legge i dati dalla sorgente nel buffer e restituisce il numero di bytes letti
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Function DoReader(ByVal buffer() As Byte) As Integer
            Return Me.m_InputStream.Read(buffer, 0, Me.BufferSize)
        End Function

        ''' <summary>
        ''' Scrive il numero di bytes indicati da bytes nella destinazione
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub DoConsumer(ByVal buffer() As Byte, ByVal nBytes As Integer)
            Me.m_OutputStream.Write(buffer, 0, nBytes)
            MyBase.DoConsumer(buffer, nBytes)
        End Sub

         
    End Class


End Class