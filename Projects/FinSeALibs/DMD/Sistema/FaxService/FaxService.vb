Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema


    Public NotInheritable Class CFaxServiceClass

        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione di questo oggetto
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConfigurationChanged(ByVal e As System.EventArgs)




        ''' <summary>
        ''' Evento generato quando si verifica un errore nell'invio di un fax
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event FaxFailed(ByVal sender As Object, ByVal e As FaxJobEventArgs)

        ''' <summary>
        ''' Evento generato quando un Fax viene inviato correttamente
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event FaxDelivered(ByVal sender As Object, ByVal e As FaxDeliverEventArgs)

        ''' <summary>
        ''' Evento generato quando viene ricevuto un Fax
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event FaxReceived(ByVal sender As Object, ByVal e As FaxReceivedEventArgs)


        Private m_InstalledDrivers As New CKeyCollection(Of BaseFaxDriver)
        Private m_DefualtDriver As BaseFaxDriver
        Private m_Config As CFaxConfig


        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Dim nd As New DMD.Drivers.NullFaxDriver
            Me.m_InstalledDrivers.Add(nd.GetUniqueID, nd)
        End Sub


        Public ReadOnly Property Config As CFaxConfig
            Get
                If m_Config Is Nothing Then
                    m_Config = New CFaxConfig
                    If APPConn.IsOpen Then m_Config.Load()
                End If
                Return m_Config
            End Get
        End Property

        Protected Friend Sub SetConfig(ByVal value As CFaxConfig)
            Me.m_Config = value
            Me.m_DefualtDriver = Nothing
            Me.doConfigChanged(New System.EventArgs)
        End Sub


        Friend Sub doConfigChanged(ByVal e As System.EventArgs)
            RaiseEvent ConfigurationChanged(e)
        End Sub

        Public Function GetInstalledDrivers() As CKeyCollection(Of BaseFaxDriver)
            SyncLock Me
                Return New CKeyCollection(Of BaseFaxDriver)(Me.m_InstalledDrivers)
            End SyncLock
        End Function

        Protected Friend Sub UpdateDriver(ByVal drv As BaseFaxDriver)
            SyncLock Me
                Dim i As Integer = Me.m_InstalledDrivers.IndexOfKey(drv.GetUniqueID)
                If (i >= 0) Then
                    Dim isConnected As Boolean = Me.m_InstalledDrivers(i).IsConnected
                    If (isConnected) Then Me.m_InstalledDrivers(i).Disconnect()
                    Me.m_InstalledDrivers(i) = drv
                    If (isConnected) Then Me.m_InstalledDrivers(i).Connect()
                Else
                    Me.m_InstalledDrivers.Add(drv.GetUniqueID, drv)
                    drv.Connect()
                End If
            End SyncLock
        End Sub

        ''' <summary>
        ''' Restitusice vero se la stringa passata come argomento rappresenta un numero di fax valido
        ''' </summary>
        ''' <param name="number"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValidNumber(ByVal number As String) As Boolean
            Return Formats.ParsePhoneNumber(number) <> ""
        End Function

        Public Sub InstallDriver(ByVal driver As BaseFaxDriver)
            SyncLock Me
                If (m_InstalledDrivers.ContainsKey(driver.GetUniqueID)) Then
                    Throw New ArgumentException("Il driver " & driver.GetUniqueID & " è già installato")
                End If
                m_InstalledDrivers.Add(driver.GetUniqueID, driver)
            End SyncLock
        End Sub

        Public Sub RemoveDriver(ByVal driver As BaseFaxDriver)
            SyncLock Me
                m_InstalledDrivers.RemoveByKey(driver.GetUniqueID)
            End SyncLock
        End Sub


        ''' <summary>
        ''' Restituisce o imposta il driver predefinito utilizzato per l'invio degli SMS
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefaultDriver As BaseFaxDriver
            Get
                SyncLock Me
                    'If (m_DefualtDriver Is Nothing) Then
                    '    Dim dName As String = Trim([Module].Settings.GetValueString("DefaultFaxDriverName", ""))
                    '    If (dName <> "") Then m_DefualtDriver = m_InstalledDrivers.GetItemByKey(dName)
                    'End If
                    If (m_DefualtDriver Is Nothing AndAlso m_InstalledDrivers.Count > 0) Then
                        m_DefualtDriver = Me.m_InstalledDrivers.GetItemByKey(Me.Config.DefaultDriverName)
                        If (Me.m_DefualtDriver Is Nothing) Then Me.m_DefualtDriver = m_InstalledDrivers(0)
                    End If
                    Return Me.m_DefualtDriver
                End SyncLock
            End Get
            'Set(value As BaseFaxDriver)
            '    'If (value IsNot Nothing) Then
            '    '    My.Settings.FAXDefaultDriver = value.GetUniqueID
            '    'Else
            '    '    [Module].Settings.SetValueString("DefaultFaxDriverName", "")
            '    'End If
            '    m_DefualtDriver = value
            'End Set
        End Property

        ''' <summary>
        ''' Invia al numero specificato un fax (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
        ''' </summary>
        ''' <param name="destNumber">[in] Numero di fax</param>
        ''' <param name="fileName">[in] Nome del file da inviare come fax</param>
        ''' <remarks></remarks>
        Public Function Send(ByVal destNumber As String, ByVal fileName As String) As FaxJob
            Return Send(DefaultDriver, destNumber, fileName)
        End Function

        Public Function Send(ByVal destNumber As String, ByVal fileName As String, ByVal options As FaxDriverOptions) As FaxJob
            Return Send(DefaultDriver, destNumber, fileName, options)
        End Function

        Public Function Send(ByVal driver As BaseFaxDriver, ByVal destNumber As String, ByVal fileName As String) As FaxJob
            Return Send(driver, destNumber, fileName, driver.GetDefaultOptions)
        End Function

       

        ''' <summary>
        ''' Invia al numero specificato un fax (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
        ''' </summary>
        ''' <param name="destNumber">[in] Numero di fax</param>
        ''' <param name="fileName">[in] Nome del file da inviare come FAX</param>
        ''' <remarks></remarks>
        Public Function Send(ByVal driver As BaseFaxDriver, ByVal destNumber As String, ByVal fileName As String, ByVal options As FaxDriverOptions) As FaxJob
            If (driver Is Nothing) Then Throw New ArgumentNullException("driver")
            If (Not System.IO.File.Exists(fileName)) Then Throw New System.IO.FileNotFoundException(fileName)
            If (options Is Nothing) Then Throw New ArgumentNullException("options")
            Dim ret As FaxJob = Me.NewJob
            options.TargetNumber = destNumber
            options.FileName = fileName
            ret.SetDriver(driver)
            ret.SetOptions(options)
            Me.Send(ret)
            Return ret
        End Function

        ''' <summary>
        ''' Invia al numero specificato un fax (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
        ''' </summary>
        ''' <param name="job">[in] Fax da inviare</param>
        ''' <remarks></remarks>
        Public Function Send(ByVal driver As BaseFaxDriver, ByVal job As FaxJob) As String
            If (driver Is Nothing) Then Throw New ArgumentNullException("driver")
            job.SetDriver(driver)
            Return Me.Send(job)
        End Function

        Public Function Send(ByVal job As FaxJob) As String
            job.Send()
            Return job.JobID
        End Function

        Friend Sub doFaxReceived(ByVal e As FaxReceivedEventArgs)
            RaiseEvent FaxReceived(Nothing, e)
        End Sub

        Friend Sub doFaxDelivered(ByVal e As FaxDeliverEventArgs)
            RaiseEvent FaxDelivered(Nothing, e)
        End Sub

        Friend Sub doFaxFailed(ByVal e As FaxJobEventArgs)
            RaiseEvent FaxFailed(Nothing, e)
        End Sub

        Function GetDriver(ByVal driverId As String) As BaseFaxDriver
            SyncLock Me
                Return Me.m_InstalledDrivers.GetItemByKey(driverId)
            End SyncLock
        End Function

        Public Function NewJob() As FaxJob
            Dim job As New FaxJob
            job.SetDriver(Me.DefaultDriver)
            job.SetJobStatus(FaxJobStatus.PREPARING)
            job.SetOptions(Me.DefaultDriver.GetDefaultOptions)
            Return job
        End Function

        Public Function GetDriverConfiguration(ByVal drvName As String) As FaxDriverOptions
            Dim driver As BaseFaxDriver = GetDriver(drvName)
            Return driver.GetDefaultOptions
        End Function

        Public Sub SetDriverConfiguration(ByVal drvName As String, config As FaxDriverOptions)
            Dim driver As BaseFaxDriver = GetDriver(drvName)
            driver.SetConfig(config)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Private Shared m_FaxService As CFaxServiceClass = Nothing

    Public Shared ReadOnly Property FaxService As CFaxServiceClass
        Get
            If (m_FaxService Is Nothing) Then m_FaxService = New CFaxServiceClass
            Return m_FaxService
        End Get
    End Property

End Class