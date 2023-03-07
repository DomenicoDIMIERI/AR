Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports DMD.Internals

Namespace Internals


    Public NotInheritable Class CSMSServiceClass
        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione di questo oggetto
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConfigurationChanged(ByVal e As System.EventArgs)

        ''' <summary>
        ''' Evento generato quando un SMS inviato cambia stato
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event SMSStatusChanged(ByVal sender As Object, ByVal e As SMSStatusEventArgs)


        ''' <summary>
        ''' Evento generato quando un SMS inviato cambia stato
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event SMSDeliver(ByVal sender As Object, ByVal e As SMSDeliverEventArgs)

        ''' <summary>
        ''' Evento generato quando viene ricevuto un SMS
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event SMSReceived(ByVal sender As Object, ByVal e As SMSReceivedEventArgs)

        ''' <summary>
        ''' Evento generato per segnalare un evento generico del driver
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event SMSEvent(ByVal sender As Object, ByVal e As SMSEventArgs)

        Private m_InstalledDrivers As New CKeyCollection(Of BasicSMSDriver)
        Private m_DefualtDriver As BasicSMSDriver
        Private m_Config As CSMSConfig


        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Dim nd As New DMD.Drivers.NullSMSDriver
            Me.m_InstalledDrivers.Add(nd.GetUniqueID, nd)
        End Sub


        Public ReadOnly Property Config As CSMSConfig
            Get
                If m_Config Is Nothing Then
                    m_Config = New CSMSConfig
                    m_Config.Load()
                End If
                Return m_Config
            End Get
        End Property

        Protected Friend Sub SetConfig(ByVal value As CSMSConfig)
            Me.m_Config = value
            Me.m_DefualtDriver = Nothing
            Me.doConfigChanged(New System.EventArgs)
        End Sub


        Friend Sub doConfigChanged(ByVal e As System.EventArgs)
            RaiseEvent ConfigurationChanged(e)
        End Sub

        Public Function GetInstalledDrivers() As CKeyCollection(Of BasicSMSDriver)
            Return New CKeyCollection(Of BasicSMSDriver)(Me.m_InstalledDrivers)
        End Function


        Public Sub InstallDriver(ByVal driver As BasicSMSDriver)
            If (m_InstalledDrivers.ContainsKey(driver.GetUniqueID)) Then
                Throw New ArgumentException("Il driver " & driver.GetUniqueID & " è già installato")
            End If
            m_InstalledDrivers.Add(driver.GetUniqueID, driver)
        End Sub

        Public Sub RemoveDriver(ByVal driver As BasicSMSDriver)
            m_InstalledDrivers.RemoveByKey(driver.GetUniqueID)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il driver predefinito utilizzato per l'invio degli SMS
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefaultDriver As BasicSMSDriver
            Get
                ''If (m_DefualtDriver Is Nothing) Then
                ''    Dim dName As String = Trim([Module].Settings.GetValueString("DefaultSMSDriverName", ""))
                ''    If (dName <> "") Then m_DefualtDriver = m_InstalledDrivers.GetItemByKey(dName)
                ''End If
                'If (m_DefualtDriver Is Nothing AndAlso m_InstalledDrivers.Count > 0) Then
                '    m_DefualtDriver = m_InstalledDrivers(0)
                'End If
                'Return m_DefualtDriver
                If (m_DefualtDriver Is Nothing AndAlso m_InstalledDrivers.Count > 0) Then
                    m_DefualtDriver = Me.m_InstalledDrivers.GetItemByKey(Me.Config.DefaultDriverName)
                    If (Me.m_DefualtDriver Is Nothing) Then Me.m_DefualtDriver = m_InstalledDrivers(0)
                End If
                Return Me.m_DefualtDriver
            End Get
            'Set(value As BasicSMSDriver)
            '    'If (value IsNot Nothing) Then
            '    '    [Module].Settings.SetValueString("DefaultSMSDriverName", value.GetUniqueID)
            '    'Else
            '    '    [Module].Settings.SetValueString("DefaultSMSDriverName", "")
            '    'End If
            '    m_DefualtDriver = value
            'End Set
        End Property

        ''' <summary>
        ''' Invia un SMS utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal targetNumber As String, ByVal message As String) As String
            Return Send(DefaultDriver, targetNumber, message, DefaultDriver.GetDefaultOptions)
        End Function

        ''' <summary>
        ''' Invia un SMS utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal targetNumber As String, ByVal message As String, ByVal options As SMSDriverOptions) As String
            Return Send(DefaultDriver, targetNumber, message, options)
        End Function

        ''' <summary>
        ''' Invia un SMS utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal driver As BasicSMSDriver, ByVal targetNumber As String, ByVal message As String) As String
            If driver Is Nothing Then Throw New ArgumentNullException("driver")
            Return Send(driver, targetNumber, message, driver.GetDefaultOptions)
        End Function

        ''' <summary>
        ''' Invia un SMS utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal driver As BasicSMSDriver, ByVal targetNumber As String, ByVal message As String, ByVal options As SMSDriverOptions) As String
            If driver Is Nothing Then Throw New ArgumentNullException("driver")
            If options Is Nothing Then Throw New ArgumentNullException("options")
            Return driver.Send(targetNumber, message, options)
        End Function

        Function GetDriver(ByVal driverId As String) As BasicSMSDriver
            SyncLock Me
                Return Me.m_InstalledDrivers.GetItemByKey(driverId)
            End SyncLock
        End Function



        Public Function GetStatus(ByVal messageID As String) As MessageStatus
            Return GetStatus(DefaultDriver, messageID)
        End Function

        Public Function GetStatus(ByVal driver As BasicSMSDriver, ByVal messageID As String) As MessageStatus
            If (driver Is Nothing) Then Throw New ArgumentNullException("driver")
            Return driver.GetStatus(messageID)
        End Function

        Public Function IsValidNumber(ByVal value As String) As Boolean
            Return IsValidNumber(DefaultDriver, value)
        End Function

        Public Function IsValidNumber(ByVal driver As BasicSMSDriver, ByVal value As String) As Boolean
            If (driver Is Nothing) Then Throw New ArgumentNullException("driver")
            Return driver.IsValidNumber(value)
        End Function

        Friend Sub doSMSReceived(ByVal e As SMSReceivedEventArgs)
            RaiseEvent SMSReceived(Nothing, e)
        End Sub

        Friend Sub doSMSStatusChanged(ByVal e As SMSStatusEventArgs)
            RaiseEvent SMSStatusChanged(Nothing, e)
        End Sub


        Friend Sub doSMSEvent(ByVal e As SMSEventArgs)
            RaiseEvent SMSEvent(Nothing, e)
        End Sub

        Friend Sub doSMSDelivered(ByVal e As SMSDeliverEventArgs)
            RaiseEvent SMSDeliver(Nothing, e)
        End Sub

        Public Function GetDriverConfiguration(drvName As String) As SMSDriverOptions
            Dim driver As BasicSMSDriver = Me.GetDriver(drvName)
            Return driver.GetDefaultOptions
        End Function

        Public Sub SetDriverConfiguration(drvName As String, config As SMSDriverOptions)
            Dim driver As BasicSMSDriver = Me.GetDriver(drvName)
            driver.SetDefaultOptions(config)
        End Sub

        Protected Friend Sub UpdateDriver(ByVal drv As BasicSMSDriver)
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

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace

Partial Public Class Sistema


    Private Shared m_SMSService As CSMSServiceClass = Nothing

    Public Shared ReadOnly Property SMSService As CSMSServiceClass
        Get
            If (m_SMSService Is Nothing) Then m_SMSService = New CSMSServiceClass
            Return m_SMSService
        End Get
    End Property


End Class