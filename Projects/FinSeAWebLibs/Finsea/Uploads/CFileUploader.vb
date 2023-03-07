Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.Threading
Imports System.Web.UI.HtmlControls
Imports System.IO

Partial Class WebSite



    Public MustInherit Class CFileUploader
        Inherits DBObjectBase
        Implements IDisposable

        Public Event UploadBegin(ByVal sender As Object, ByVal e As UploadEventArgs)
        Public Event UploadProgress(ByVal sender As Object, ByVal e As UploadEventArgs)
        Public Event UploadComplete(ByVal sender As Object, ByVal e As UploadEventArgs)
        Public Event UploadError(ByVal sender As Object, ByVal e As UploadErrorEventArgs)

 
        Private m_Key As String
        Private m_BasePath As String
        Private m_FileName As String
        Private m_StartTime As Date
        Private m_EndTime As Date
        Private m_Async As Boolean
        Private m_DestinationFile As String
        Private m_DestURL As String

        ''' <summary>
        ''' Data e ora dell'ultima lettura completa (reader)
        ''' </summary>
        ''' <remarks></remarks>
        Protected m_LastRead As Date

        ''' <summary>
        ''' Numero totale di bytes da ricevere
        ''' </summary>
        ''' <remarks></remarks>
        Protected m_BytesToReceive As Long

        ''' <summary>
        ''' Conteggio dei bytes ricevuti
        ''' </summary>
        ''' <remarks></remarks>
        Protected m_BytesReceived As Long

        ''' <summary>
        ''' Eventuale eccezione durante l'uload
        ''' </summary>
        ''' <remarks></remarks>
        Private m_Exception As Exception

        ''' <summary>
        ''' Se vero indica che l'utente ha richiesto l'annullamento del download .Abort()
        ''' </summary>
        ''' <remarks></remarks>
        Private m_Cancel As Boolean


        
        ''' <summary>
        ''' Semaforo per le operazioni asincrone
        ''' </summary>
        ''' <remarks></remarks>
        Protected m_ManualReset As ManualResetEvent

        ''' <summary>
        ''' Dimensione del buffer
        ''' </summary>
        ''' <remarks></remarks>
        Private m_BufferSize As Integer

        ''' <summary>
        ''' Buffer utilizzato per lo scambio tra il reader ed il consumer
        ''' </summary>
        ''' <remarks></remarks>
        Protected buffer() As Byte

        Private Delegate Sub UploadDelegate()

        Private m_AsyncCall As UploadDelegate
        Private m_AsyncRes As IAsyncResult

        Public Sub New()
            Me.m_Key = ""
            Me.m_BasePath = WebSite.Instance.Configuration.PublicURL
            Me.m_Async = False
            'ReDim m_Buffer(BUFFERSIZE - 1)
            Me.m_ManualReset = New ManualResetEvent(False)
            Me.m_Cancel = False
            Me.m_DestinationFile = ""
            Me.m_BufferSize = Math.Max(512, WebSite.Instance.Configuration.UploadBufferSize)
            ReDim Me.buffer(Me.m_BufferSize - 1)
            Me.m_AsyncCall = AddressOf Me.InternalUploadSync
            Me.m_AsyncRes = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la dimensione del buffer utilizzato per l'upload
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BufferSize As Integer
            Get
                Return Me.m_BufferSize
            End Get
            Set(value As Integer)
                Me.m_BufferSize = Math.Max(512, value)
                ReDim buffer(Me.m_BufferSize - 1)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce vero se è stato richiesto alla classe di abortire un upoload
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsAborting() As Boolean
            Return Me.m_Cancel
        End Function

        ''' <summary>
        ''' Restituisce vero se la procedura è terminata
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function IsCompleted() As Boolean
            Return (Me.Exception Is Nothing) AndAlso (Me.UploadedBytes >= Me.TotalBytes)
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se utilizzare il codice asincrono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Async As Boolean
            Get
                Return Me.m_Async
            End Get
            Set(value As Boolean)
                Me.m_Async = value
            End Set
        End Property

        Public ReadOnly Property Exception As Exception
            Get
                Return Me.m_Exception
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' Annulla il download
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Abort()
            If (Me.IsCompleted = False) Then
                Me.m_Cancel = True
                Me.FinalizeUpload()
            End If
        End Sub

        ''' <summary>
        ''' Restituisce la chiave univoca associata quando viene creato il download
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Key As String
            Get
                Return Me.m_Key
            End Get
        End Property

        ''' <summary>
        ''' Imposta la chiave univoca che identifica il download
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Friend Sub SetKey(ByVal value As String)
            Me.m_Key = value
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome ed il percorso del file da salvare sul server.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Se questo valore viene lasciato vuoto verrà generato un nome automaticamente in base al contenuto caricato</remarks>
        Public Property FileName As String
            Get
                Return Me.m_FileName
            End Get
            Set(value As String)
                Me.m_FileName = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del percorso base di caricamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BasePath As String
            Get
                Return Me.m_BasePath
            End Get
            Set(value As String)
                Me.m_BasePath = Trim(value)
                If Right(Me.m_BasePath, 1) = "/" Then Me.m_BasePath = Left(Me.m_BasePath, Len(Me.m_BasePath) - 1)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce il percorso fisico in cui è stato salvato il file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetFileName As String
            Get
                Return Me.m_DestinationFile
            End Get
            Set(value As String)
                Me.m_DestinationFile = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la URL del file salvato sul server
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DestinationUrl As String
            Get
                Return Me.m_DestURL
            End Get
            Set(value As String)
                Me.m_DestURL = Strings.Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la percentuale di caricamento del file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Percentage As Double
            Get
                Return 100 * CDbl(Me.m_BytesReceived) / Me.TotalBytes
            End Get
        End Property

        ''' <summary>
        ''' Restituisce i bytes finora ricevuti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UploadedBytes As Long
            Get
                Return Me.m_BytesReceived
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il numero totale di bytes inviati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TotalBytes As Long
            Get
                Return Me.m_BytesToReceive
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la data e ora di inizio del caricamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StartTime As Date
            Get
                Return Me.m_StartTime
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la data e l'ora di fine caricamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EndTime As Date
            Get
                Return Me.m_EndTime
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_FileUploads"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.LOGConn
        End Function

        Protected Overridable Sub OnBeforeBeginUpload(ByVal e As UploadEventArgs)
            RaiseEvent UploadBegin(Me, e)
            Uploader.FireBeginUpload(e)
        End Sub

        Protected MustOverride Sub PrepareUpload()

        Protected MustOverride Function GetTotalBytesToReceive() As Long



        Public Sub Upload()
            Try
                Me.m_StartTime = Now
                Me.m_BytesReceived = 0
                Me.m_BytesToReceive = Me.GetTotalBytesToReceive
                Me.m_Cancel = False

                Me.PrepareUpload()

                Dim e As New UploadEventArgs(Me)
                Me.OnBeforeBeginUpload(e)

                If (Me.Async) Then
                    Me.m_AsyncRes = Me.m_AsyncCall.BeginInvoke(AddressOf doEndAsync, Me)
                    Me.m_AsyncCall.EndInvoke(Me.m_AsyncRes)
                Else
                    Me.InternalUploadSync()
                End If

                Me.FinalizeUpload()

                Me.m_EndTime = Now
                Me.OnAfterUpload(e)
                'End If

            Catch ex As Exception
                Me.FinalizeUpload()
                Me.OnUploadError(New UploadErrorEventArgs(Me, ex))
            End Try

        End Sub

        Private Sub doEndAsync(ByVal o As IAsyncResult)
            'Try

            'Catch ex As Exception
            '    Sistema.Events.NotifyUnhandledException(ex)
            '    '
            'End Try
        End Sub

        'Protected Overridable Sub InternalUploadAsync()
        '    Me.InternalUploadSync()
        'End Sub

        Protected Overridable Sub InternalUploadSync()
            Me.m_LastRead = Now
            While Not Me.IsCompleted AndAlso Not Me.m_Cancel
                If (Me.m_Cancel) Then
                    Throw New UploadCalcelledException
                Else
                    'Leggiamo
                    Dim nRead As Integer

                    nRead = Me.DoReader(Me.buffer)
                    If (nRead <= 0) Then
                        WebSite.Instance.AppContext.Log(Formats.FormatUserDate(Calendar.Now()) & " - Lettura di dimensione 0 caricando il file " & Me.m_DestURL & " (" & Me.m_BytesReceived & " / " & Me.m_BytesToReceive & ")")
                        Exit While
                    End If
                    Me.m_BytesReceived += nRead

                    'Scriviamo
                    Me.DoConsumer(Me.buffer, nRead)
                    'Me.m_BytesReceived += Me.m_BytesRead

                    'Se sono impostati dei limiti di velocità applichiamoli
                    Me.LimitateSpeed(nRead)

                    Me.m_LastRead = Now

                    Dim e As New UploadEventArgs(Me)
                    Me.OnUploadProgress(e)
                End If
            End While
        End Sub

        Protected Overridable Sub OnUploadProgress(ByVal e As UploadEventArgs)
            RaiseEvent UploadProgress(Me, e)
            Uploader.FireUploadProgress(e)
        End Sub

        ''' <summary>
        ''' Questo metodo viene richiamato per rilasciare le risorse allocate per l'upload
        ''' </summary>
        ''' <remarks></remarks>
        Protected MustOverride Sub FinalizeUpload()

        Protected Overridable Sub OnAfterUpload(ByVal e As UploadEventArgs)
            RaiseEvent UploadComplete(Me, e)
            Uploader.FireEndUpload(e)
        End Sub

        Protected Overridable Sub OnUploadError(ByVal e As UploadErrorEventArgs)
            Me.m_Exception = e.Cause
            RaiseEvent UploadError(Me, e)
            Uploader.FireUploadError(e)
        End Sub

        
        ''' <summary>
        ''' Legge i dati dalla sorgente nel buffer e restituisce il numero di bytes letti
        ''' </summary>
        ''' <param name="buffer">[out] buffer (di dimensione BufferSize) in cui vengono caricati i dati letti</param>
        ''' <returns>Restituisce il numero di byres letti e caricati nel buffer</returns>
        ''' <remarks></remarks>
        Protected Overridable Function DoReader(ByVal buffer As Byte()) As Integer
            Return 0
        End Function

        ''' <summary>
        ''' Scrive il numero di bytes indicati da bytes nella destinazione
        ''' </summary>
        ''' <param name="buffer">[in] Buffer di dimensione BufferSize in cui si trovano i dati</param>
        ''' <param name="nBytes">[in] Numero di bytes validi all'interno del buffer</param>
        ''' <remarks></remarks>
        Protected Overridable Sub DoConsumer(ByVal buffer As Byte(), ByVal nBytes As Integer)

        End Sub

        Protected Overridable Sub LimitateSpeed(ByVal nRead As Integer)
            If (Not Me.IsCompleted) AndAlso (WebSite.Instance.Configuration.UploadSpeedLimit > 0) Then
                Dim t As TimeSpan = Now - Me.m_LastRead
                Dim speed As Double = 0
                Dim delay As Integer
                If (t.TotalMilliseconds > 0) Then
                    speed = CDbl(nRead) * 1000 / t.TotalMilliseconds
                    delay = speed / WebSite.Instance.Configuration.UploadSpeedLimit
                Else
                    delay = Math.Floor(Me.BufferSize * 1000 / WebSite.Instance.Configuration.UploadSpeedLimit)
                End If
                If (delay > 0) Then Thread.Sleep(Math.Min(delay, 2000))
            End If
        End Sub



        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Me.FinalizeUpload()
            If (Me.m_ManualReset IsNot Nothing) Then Me.m_ManualReset.Dispose() : Me.m_ManualReset = Nothing
            Me.m_Key = vbNullString
            Me.m_BasePath = vbNullString
            Me.m_FileName = vbNullString
            Me.m_DestinationFile = vbNullString
            Me.m_DestURL = vbNullString
            Me.m_Exception = Nothing
            If (Me.buffer IsNot Nothing) Then Erase Me.buffer : Me.buffer = Nothing
        End Sub


        Public Overrides Function ToString() As String
            Return "{" & Me.Key & ", " & Me.FileName & "}"
        End Function
    End Class


End Class