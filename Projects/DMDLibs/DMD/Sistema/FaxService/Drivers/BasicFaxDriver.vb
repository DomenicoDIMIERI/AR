Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public MustInherit Class BaseFaxDriver
        Inherits Drivers.Driver

        Public Event FaxJobFailed(ByVal sender As Object, ByVal e As FaxJobEventArgs)

        Public Event FaxReceived(ByVal sender As Object, ByVal e As FaxReceivedEventArgs)

        Public Event FaxDelivered(ByVal sender As Object, ByVal e As FaxDeliverEventArgs)

        Private m_Config As FaxDriverOptions
        'Private m_BaseFolder As String
        Private m_OutQueue As CSyncKeyCollection(Of FaxJob)
        'Private m_OutgoingQueue As CKeyCollection(Of FaxJob)
        'Private m_CompletedQueue As CKeyCollection(Of FaxJob)
        Private m_IncomingQueue As CKeyCollection(Of FaxJob)

        Private m_Modems As FaxDriverModems


        Protected outQueueLock As New Object
        Protected inqueueLock As New Object


        Public Sub New()
            Me.m_Config = Nothing
            Me.m_OutQueue = Nothing
            Me.m_IncomingQueue = Nothing
            Me.m_Modems = Nothing
        End Sub

        Protected Overrides Function GetDefaultSettingsFileName() As String
            Return Global.System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "Drivers\Fax\" & Me.GetUniqueID & "\config.cfg")
        End Function

        Public Overridable Function GetModem(ByVal modemName As String) As FaxDriverModem
            modemName = Strings.Trim(modemName)
            If (modemName <> "") Then
                For Each modem As FaxDriverModem In Me.Modems
                    If modem.Name = modemName Then
                        Return modem
                    End If
                Next
            End If
            If (Me.Modems.Count > 0) Then Return Me.Modems(0)
            Return Nothing
        End Function

        Public ReadOnly Property Modems As FaxDriverModems
            Get
                SyncLock Me
                    If (Me.m_Modems Is Nothing) Then
                        Dim str As String = Trim(Me.Config.GetValueString("__MODEMSCONFIG__", ""))
                        Try
                            If (str = "") Then
                                Me.m_Modems = New FaxDriverModems(Me)
                            Else
                                Me.m_Modems = XML.Utils.Serializer.Deserialize(str)
                            End If
                        Catch ex As Exception
                            Sistema.Events.NotifyUnhandledException(ex)
                            Me.m_Modems = New FaxDriverModems(Me)
                        End Try
                    End If
                    Return Me.m_Modems
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto che descrive la configurazione del driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Config As FaxDriverOptions
            Get
                SyncLock Me
                    If (Me.m_Config Is Nothing) Then Me.m_Config = Me.GetDefaultOptions
                    Return Me.m_Config
                End SyncLock
            End Get
        End Property

        Protected Friend Overridable Sub SetConfig(ByVal value As FaxDriverOptions)
            Me.m_Config = value
        End Sub

        ''' <summary>
        ''' Restituisce il percorso predefinito in cui vengono memorizzate le code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetBaseFolder() As String
            Return DMD.Sistema.FileSystem.GetFolderName(Me.GetDefaultSettingsFileName)
        End Function

        ''' <summary>
        ''' Restituisce il percorso predefinito in cui viene memorizzata la coda specifica
        ''' </summary>
        ''' <param name="queue"></param>
        ''' <param name="file"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetQueueFile(ByVal queue As String, ByVal file As String) As String
            Dim ret As String = System.IO.Path.Combine(Me.GetBaseFolder, queue)
            ret = System.IO.Path.Combine(ret, file)
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la coda dei fax in uscita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property OutQueue As CSyncKeyCollection(Of FaxJob)
            Get
                SyncLock Me.outQueueLock
                    If (Me.m_OutQueue Is Nothing) Then
                        Me.m_OutQueue = New CSyncKeyCollection(Of FaxJob)

                        Dim f As New FindFileCursor(Me.GetQueueFile("OutQueue", "*"))
                        While Not f.EOF
                            Dim p As String = f.Item
                            Try
                                Dim text As String = FileSystem.GetTextFileContents(p)
                                Dim job As FaxJob = XML.Utils.Serializer.Deserialize(text)
                                Me.SetDriver(job, Me)
                                Me.m_OutQueue.Add(job.JobID, job)
                            Catch ex As Exception
                                Sistema.Events.NotifyUnhandledException(ex)
                            End Try
                            f.MoveNext()
                        End While
                    End If
                    Return Me.m_OutQueue
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce una collezione contenente tutti  i fax ricevuti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IncomingQueue As CKeyCollection(Of FaxJob)
            Get
                SyncLock Me.inqueueLock
                    If (Me.m_IncomingQueue Is Nothing) Then
                        Me.m_IncomingQueue = New CKeyCollection(Of FaxJob)

                        Dim f As New FindFileCursor(Me.GetQueueFile("InQueue", "*"))
                        While Not f.EOF
                            Dim p As String = f.Item
                            Try
                                Dim text As String = FileSystem.GetTextFileContents(p)
                                Dim job As FaxJob = XML.Utils.Serializer.Deserialize(text)
                                Me.SetDriver(job, Me)
                                Me.m_IncomingQueue.Add(job.JobID, job)
                            Catch ex As Exception
                                Sistema.Events.NotifyUnhandledException(ex)
                            End Try
                            f.MoveNext()
                        End While

                    End If
                    Return Me.m_IncomingQueue
                End SyncLock
            End Get
        End Property

  
        ''' <summary>
        ''' Invia al numero specificato un fax (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
        ''' </summary>
        ''' <param name="job"></param>
        ''' <remarks></remarks>
        Protected Friend Function Send(ByVal job As FaxJob) As String
            Dim queue As CSyncKeyCollection(Of FaxJob) = Me.OutQueue
            Dim text As String
            Dim fName As String

            SyncLock Me.outQueueLock
                'Genera un ID che non sia già stato utilizzato per un fax in uscita o per uno già inviato
                fname = Me.GetUniqueID & "O" & ASPSecurity.GetRandomKey(12)
                While System.IO.File.Exists(Me.GetQueueFile("OutQueue", fName)) OrElse System.IO.File.Exists(Me.GetQueueFile("Sent", fName))
                    fName = Me.GetUniqueID & "O" & ASPSecurity.GetRandomKey(12)
                End While
                SetJobID(job, fName)
                'Accoda il fax in uscita e memorizza nel filesystem in modo da renderlo persistente
                SetStatus(job, FaxJobStatus.QUEUED)
                job.SetJobStatusMessage("Fax messo in coda")
                SetJobDate(job, DateUtils.Now())
                text = XML.Utils.Serializer.Serialize(job)
                DMD.Sistema.FileSystem.CreateTextFile(Me.GetQueueFile("OutQueue", fName), text)
                queue.Add(job.JobID, job)
            End SyncLock

            Try
                Me.InternalSend(job)
                'job.SetJobID (
            Catch ex As Exception
                job.SetJobStatus(FaxJobStatus.ERROR)
                job.SetJobStatusMessage(ex.Message)
                Me.doFaxError(job, "Driver error: " & ex.Message)
                'Sistema.FileSystem.SetTextFileContents(Me.GetQueueFile("OutQueue", fName), text)
                Return job.JobID
            End Try

            SyncLock Me.outQueueLock
                text = XML.Utils.Serializer.Serialize(job)
                DMD.Sistema.FileSystem.SetTextFileContents(Me.GetQueueFile("OutQueue", fName), text)
            End SyncLock

            Return job.JobID
        End Function

        ''' <summary>
        ''' Funziona che invia effettivamente il fax
        ''' </summary>
        ''' <param name="job"></param>
        ''' <remarks></remarks>
        Protected MustOverride Sub InternalSend(ByVal job As FaxJob)


        

        'Public Sub NotifyDeliveredFax(ByVal order_id As String, ByVal recipient As String, ByVal delivery_date As Nullable(Of Date))
        '    Dim e As New DMD.Sistema.FaxService.FaxDeliverEventArgs(order_id, recipient, delivery_date)

        'End Sub

        'Public Sub NotifyReceivedFax(ByVal order_id As String, ByVal sender As String, ByVal receiveDate As Date, ByVal message As Bitmap())
        '    Dim e As New DMD.Sistema.FaxService.FaxReceivedEventArgs(order_id, sender, receiveDate, message)
        '    FaxService.doFaxReceived(e)
        'End Sub

        Protected Friend Sub CancelJob(ByVal jobID As String)
            'Annulla l'invio (se supportato)
            Me.CancelJobInternal(jobID)

            SyncLock Me.outQueueLock
                'Rimuove dalla coda in uscita
                Me.OutQueue.RemoveByKey(jobID)
                DMD.Sistema.FileSystem.DeleteFile(Me.GetQueueFile("OutQueue", jobID))
            End SyncLock
        End Sub

        Protected MustOverride Sub CancelJobInternal(ByVal jobID As String)


        Protected Friend Sub SetJobFail(ByVal job As FaxJob, ByVal status As FaxJobStatus)
            job.SetJobStatus(status)
            Dim e As New FaxJobEventArgs(job)
            Me.OnFaxJobFail(e)
            FaxService.doFaxFailed(e)
        End Sub

        Protected Overridable Sub OnFaxJobFail(ByVal e As FaxJobEventArgs)
            RaiseEvent FaxJobFailed(Me, e)
        End Sub

        Protected Friend Sub doFaxChangeStatus(ByVal job As FaxJob, ByVal status As FaxJobStatus, ByVal message As String)
            Me.SetStatus(job, status)
            Me.SetStatusMessage(job, message)
            Dim src As String = Me.GetQueueFile("OutQueue", job.JobID)
            SyncLock Me.outQueueLock
                DMD.Sistema.FileSystem.DeleteFile(src)
                DMD.Sistema.FileSystem.CreateTextFile(src, XML.Utils.Serializer.Serialize(job))
            End SyncLock
        End Sub

        Protected Friend Sub setDeliveryDate(ByVal job As FaxJob, ByVal d As Date?)
            job.setDeliveryDate(d)
        End Sub

        Protected Friend Sub setFailDate(ByVal job As FaxJob, ByVal d As Date?)
            job.setFailDate(d)
        End Sub

        Protected Friend Sub setQueueDate(ByVal job As FaxJob, ByVal d As Date?)
            job.setQueueDate(d)
        End Sub

        Protected Friend Sub doFaxDelivered(ByVal job As FaxJob)
            job.SetJobStatus(FaxJobStatus.COMPLETED)
            job.SetJobStatusMessage("Inviato")

            SyncLock Me.outQueueLock
                'Dim fName As String = job.JobID
                Dim src As String = Me.GetQueueFile("OutQueue", job.JobID)
                Dim trg As String = Me.GetQueueFile("Sent", job.JobID)

                Me.OutQueue.RemoveByKey(job.JobID)

                DMD.Sistema.FileSystem.DeleteFile(src)
                DMD.Sistema.FileSystem.CreateTextFile(trg, XML.Utils.Serializer.Serialize(job))
            End SyncLock

            Dim e As New FaxDeliverEventArgs(job)
            Me.OnFaxDelivered(e)
            FaxService.doFaxDelivered(e)
        End Sub

        Protected Overridable Sub OnFaxDelivered(ByVal e As FaxDeliverEventArgs)
            RaiseEvent FaxDelivered(Me, e)
        End Sub

        Protected Overridable Sub doFaxError(ByVal job As FaxJob, ByVal message As String)
            job.SetJobStatus(FaxJobStatus.ERROR)
            job.SetJobStatusMessage(message)

            SyncLock Me.outQueueLock
                'Dim fName As String = job.JobID
                Dim src As String = Me.GetQueueFile("OutQueue", job.JobID)
                Dim trg As String = Me.GetQueueFile("Sent", job.JobID)

                Me.OutQueue.RemoveByKey(job.JobID)

                DMD.Sistema.FileSystem.DeleteFile(src)
                DMD.Sistema.FileSystem.CreateTextFile(trg, XML.Utils.Serializer.Serialize(job))
            End SyncLock

            Dim e As New FaxJobEventArgs(job)

            Me.OnFaxJobFail(e)

            FaxService.doFaxFailed(e)
        End Sub


        Protected Overridable Function NewJob() As FaxJob
            Dim ret As New FaxJob
            Me.SetDriver(ret, Me)
            Me.SetOptions(ret, Me.InstantiateNewOptions)
            Return ret
        End Function

        Protected Overridable Sub SetDriver(ByVal job As FaxJob, ByVal driver As BaseFaxDriver)
            job.SetDriver(driver)
        End Sub

        Protected Overridable Sub SetStatus(ByVal job As FaxJob, ByVal status As FaxJobStatus)
            job.SetJobStatus(status)
        End Sub

        Protected Overridable Sub SetJobDate(ByVal job As FaxJob, ByVal data As Date)
            job.SetDate(data)
        End Sub

        Protected Overridable Sub SetStatusMessage(ByVal job As FaxJob, ByVal message As String)
            job.SetJobStatusMessage(message)
        End Sub

        Protected Overridable Sub SetJobID(ByVal job As FaxJob, ByVal id As String)
            job.SetJobID(id)
        End Sub

        Protected Overridable Sub SetOptions(ByVal job As FaxJob, ByVal options As FaxDriverOptions)
            job.SetOptions(options)
        End Sub

        Protected Overridable Sub SetDate(ByVal job As FaxJob, ByVal value As Date)
            job.SetDate(value)
        End Sub

        

        Protected Sub doFaxReceived(ByVal f As FaxJob)
            Dim e As New FaxReceivedEventArgs(f)
            Me.OnFaxReceived(e)
            Sistema.FaxService.doFaxReceived(e)
        End Sub

        Protected Overridable Sub OnFaxReceived(ByVal e As FaxReceivedEventArgs)
            RaiseEvent FaxReceived(Me, e)
        End Sub
 
       
        Protected Overrides Function InstantiateNewOptions() As Drivers.DriverOptions
            Return New FaxDriverOptions
        End Function

        Protected Overrides Sub InternalConnect()
            DMD.Sistema.FileSystem.CreateRecursiveFolder(System.IO.Path.Combine(Me.GetBaseFolder, "OutQueue"))
            DMD.Sistema.FileSystem.CreateRecursiveFolder(System.IO.Path.Combine(Me.GetBaseFolder, "InQueue"))
            DMD.Sistema.FileSystem.CreateRecursiveFolder(System.IO.Path.Combine(Me.GetBaseFolder, "Sent"))
        End Sub

        Protected Overrides Sub SetFieldInternal(fielName As String, fieldValue As Object)
            Select Case fielName
                Case "Modems" : Me.m_Modems = fieldValue : Me.m_Modems.SetOwner(Me)
                Case "Configuration" : Me.m_Config = XML.Utils.Serializer.ToObject(fieldValue)
                Case Else : MyBase.SetFieldInternal(fielName, fieldValue)
            End Select
        End Sub


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Modems", Me.Modems)
            writer.WriteTag("Configuration", Me.Config)
        End Sub

        Public Overridable Sub SaveConfiguration()
            Me.Config.SetValueString("__MODEMSCONFIG__", XML.Utils.Serializer.Serialize(Me.Modems))
            Me.SetConfig(Me.Config)
            Dim path As String = Me.GetDefaultSettingsFileName 'Global.System.IO.Path.Combine(WebSite.Instance.AppContext.SystemDataFolder, "TrendooSMS\config.cfg")
            DMD.Sistema.FileSystem.CreateRecursiveFolder(Sistema.FileSystem.GetFolderName(path))
            Me.Config.SaveToFile(path)
            DMD.Sistema.FaxService.UpdateDriver(Me)
        End Sub

         
       
    End Class

       
 
End Class