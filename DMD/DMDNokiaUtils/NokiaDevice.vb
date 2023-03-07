Imports DMD.Internals
Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS


Partial Class Nokia

    Public Enum NokiaDeviceCaps As Integer
        'FSPublic Const CONAPI_FS_NOT_SUPPORTED As Integer = &H0               ' Device is not support file system.

        ''' <summary>
        ''' Device is support file system.
        ''' </summary>
        ''' <remarks></remarks>
        SupportsFileSystem

        ''' <summary>
        ''' Device is supporting Java MIDlet installation.
        ''' </summary>
        ''' <remarks></remarks>
        SupportsJavaAppInstallation

        ''' <summary>
        ''' Device is supporting SIS applications installation. 
        ''' </summary>
        ''' <remarks></remarks>
        SupportsSISAppInstallation

        ''' <summary>
        ''' Device supports SISX applications' installation. 
        ''' </summary>
        ''' <remarks></remarks>
        SupportsSISXAppInstallation

        ''' <summary>
        ''' Device is supporting file conversion.
        ''' </summary>
        ''' <remarks></remarks>
        SupportsFileConversion

        ''' <summary>
        ''' Device supports installed applications' listing.
        ''' </summary>
        ''' <remarks></remarks>
        CanListInstalledApplications

        ''' <summary>
        ''' Device supports installed applications' uninstallation.
        ''' </summary>
        ''' <remarks></remarks>
        SupportsUninstallApplication

        ''' <summary>
        ''' Device supports extended File System operations (e.g. Copy folder).
        ''' </summary>
        ''' <remarks></remarks>
        SupportsExtendedFSOperations


        SupportsTheme 'CONAPI_SERIES40_DEVICE

        SupportsSynchronization

        SupportsSADSSynchronization


        SupportsSADMSynchronization

        SupportsCIDSSynchronization

    End Enum

    Public Enum DeviceType As Integer
        Serie40 = CONAPI_SERIES40_DEVICE
        Serie60_2ed = CONAPI_SERIES60_2ED_DEVICE
        Serie60_3ed = CONAPI_SERIES60_3ED_DEVICE
        Serie80 = CONAPI_SERIES80_DEVICE
    End Enum

    ''' <summary>
    ''' Rappresenta un telefonino o una periferica Nokia connessa al PC
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NokiaDevice
        Implements IDisposable

        ''' <summary>
        ''' Evento quando sulla periferica è rischiesto l'intervento dell'utente per confermare un'operazione
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RequireUserAction(ByVal sender As Object, ByVal e As System.EventArgs)


        Private m_Manufacturer As String
        Private m_FriendlyName As String
        Private m_Model As String
        Private m_SerialNumber As String

        Private m_Contacts As CContacts
        Private m_SMS As CSMS
        Private m_MMS As CMMS
        Private m_BookMarks As CBookMarks
        Private m_Calendar As CCalendar
        Private m_FileSystem As CFileSystem
        Private m_InstalledApplications As NokiaInstalledApps

        Friend pCANotifyCallBack As CANotifyCallbackDelegate
        Friend pCAOperationCallback As CAOperationCallbackDelegate

        Private m_Info As CONAPI_DEVICE_GEN_INFO
        Private m_SoftwareName As String
        Private m_SoftwareVersion As String
        Private m_UsedLanguage As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Contacts = Nothing
            Me.m_SMS = Nothing
            Me.m_MMS = Nothing
            Me.m_BookMarks = Nothing
            Me.m_Calendar = Nothing
            Me.pCANotifyCallBack = AddressOf CANotifyCallBack
            Me.pCAOperationCallback = AddressOf CAOperationCallback
            Me.m_FileSystem = Nothing
            Me.m_InstalledApplications = Nothing
        End Sub


        ''' <summary>
        ''' Accede alle applicazioni installate sul telefonino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property InstalledApplications As NokiaInstalledApps
            Get
                SyncLock Me
                    If (Me.m_InstalledApplications Is Nothing) Then Me.m_InstalledApplications = New NokiaInstalledApps(Me)
                    Return Me.m_InstalledApplications
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il nome del produttore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Manifacturer As String
            Get
                Return Me.m_Manufacturer
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un nome "amichevole" per la periferica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FriendlyName As String
            Get
                Return Me.m_FriendlyName
            End Get
            Set(value As String)
                If (Me.m_FriendlyName = value) Then Exit Property
                Dim iResult As Integer = CONARenameFriendlyName(DMD.Nokia.m_hDMHandle, Me.SerialNumber, value)
                If iResult <> CONA_OK Then ShowErrorMessage("CONARenameFriendlyName failed!", iResult)
                Me.m_FriendlyName = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Model As String
            Get
                Return Me.m_Model
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il numero seriale del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SerialNumber As String
            Get
                Return Me.m_SerialNumber
            End Get
        End Property

        ''' <summary>
        ''' Inizializza l'oggetto dal puntatore
        ''' </summary>
        ''' <param name="ptr"></param>
        ''' <remarks></remarks>
        Protected Friend Sub InitializeFromPtr(ByVal ptr As System.IntPtr)
            Dim info As CONAPI_DEVICE = CType(Marshal.PtrToStructure(ptr, GetType(CONAPI_DEVICE)), CONAPI_DEVICE)
            Me.FromInfo(info)
        End Sub

        ''' <summary>
        ''' Restituisce la collezione dei contatti memorizzati nella periferica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Contacts As CContacts
            Get
                SyncLock Me
                    If (Me.m_Contacts Is Nothing) Then Me.m_Contacts = New CContacts(Me)
                    Return Me.m_Contacts
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Accede alle informazioni sugli SMS
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SMS As CSMS
            Get
                SyncLock Me
                    If (Me.m_SMS Is Nothing) Then Me.m_SMS = New CSMS(Me)
                    Return Me.m_SMS
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property MMS As CMMS
            Get
                SyncLock Me
                    If (Me.m_MMS Is Nothing) Then Me.m_MMS = New CMMS(Me)
                    Return Me.m_MMS
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property BookMarks As CBookMarks
            Get
                SyncLock Me
                    If (Me.m_BookMarks Is Nothing) Then Me.m_BookMarks = New CBookMarks(Me)
                    Return Me.m_BookMarks
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Calendar As CCalendar
            Get
                SyncLock Me
                    If (Me.m_Calendar Is Nothing) Then Me.m_Calendar = New CCalendar(Me)
                    Return Me.m_Calendar
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property DeviceType As DeviceType
            Get
                Me.CheckCaps()
                Return DirectCast(Me.m_Info.iType, DeviceType)
            End Get
        End Property

        Public ReadOnly Property DeviceTypeEx As String
            Get
                If ((Me.m_Info.iType And DeviceType.Serie80) = DeviceType.Serie80) Then Return "Serie 80"
                If ((Me.m_Info.iType And DeviceType.Serie60_3ed) = DeviceType.Serie60_3ed) Then Return "Serie 60 Thirt Edition"
                If ((Me.m_Info.iType And DeviceType.Serie60_2ed) = DeviceType.Serie60_2ed) Then Return "Serie 60 Second Edition"
                If ((Me.m_Info.iType And DeviceType.Serie40) = DeviceType.Serie40) Then Return "Serie 40"
                Return "Unknown"
            End Get
        End Property

        'GetMMSFolders(itemX.Tag, itemX)
        'GetCalendarFolder(itemX.Tag, itemX)
        'GetBookmarkFolder(itemX.Tag, itemX)

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If

            If (Me.m_BookMarks IsNot Nothing) Then Me.m_BookMarks.Dispose() : Me.m_BookMarks = Nothing
            If (Me.m_Calendar IsNot Nothing) Then Me.m_Calendar.Dispose() : Me.m_Calendar = Nothing
            If (Me.m_Contacts IsNot Nothing) Then Me.m_Contacts.Dispose() : Me.m_Contacts = Nothing
            If (Me.m_FileSystem IsNot Nothing) Then Me.m_FileSystem.Dispose() : Me.m_FileSystem = Nothing
            If (Me.m_MMS IsNot Nothing) Then Me.m_MMS.Dispose() : Me.m_MMS = Nothing
            If (Me.m_SMS IsNot Nothing) Then Me.m_SMS.Dispose() : Me.m_SMS = Nothing

            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        '===================================================================
        ' CANotifyCallBack
        '
        ' Callback function for CA notifications
        '
        '===================================================================
        Friend Function CANotifyCallBack(ByVal hCAHandle As Integer, ByVal iReason As Integer, ByVal iParam As Integer, ByVal pItemID As IntPtr) As Integer

            'If iReason = CA_REASON_ENUMERATING Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_ENUMERATING")
            'ElseIf iReason = CA_REASON_ITEM_ADDED Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_ITEM_ADDED")
            'ElseIf iReason = CA_REASON_ITEM_DELETED Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_ITEM_DELETED")
            'ElseIf iReason = CA_REASON_ITEM_UPDATED Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_ITEM_UPDATED")
            'ElseIf iReason = CA_REASON_ITEM_MOVED Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_ITEM_MOVED")
            'ElseIf iReason = CA_REASON_ITEM_REPLACED Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_ITEM_REPLACED")
            'ElseIf iReason = CA_REASON_CONNECTION_LOST Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_CONNECTION_LOST")
            'ElseIf iReason = CA_REASON_MSG_DELIVERY Then
            '    ShowNotification("CANotifyCallBack: CA_REASON_MSG_DELIVERY")
            'End If

            Return CONA_OK
        End Function

        '===================================================================
        ' CAOperationCallback
        '
        ' Callback function for CA operation notifications
        '
        '===================================================================
        Friend Function CAOperationCallback(ByVal hOperHandle As Integer, ByVal iOperation As Integer, ByVal iCurrent As Integer, ByVal iTotalAmount As Integer, ByVal iStatus As Integer, ByVal pItemID As IntPtr) As Integer
            Dim strStatus As String
            strStatus = String.Format("CAOperationCallback: Operation({0}), progress({0}), total({0}), status({0})", iOperation, iCurrent, iTotalAmount, iStatus)
            'ShowNotification(strStatus)
            Return CONA_OK
        End Function

        Friend Sub FromInfo(ByVal info As CONAPI_DEVICE)
            Me.m_Manufacturer = info.pstrManufacturer
            Me.m_FriendlyName = info.pstrFriendlyName
            Me.m_Model = info.pstrModel
            Me.m_SerialNumber = info.pstrSerialNumber
        End Sub


        Friend Sub FromSerialNumber(ByVal serial As String)
            Dim info As CONAPI_DEVICE
            Dim iRet As Integer = CONAGetDevice(Nokia.m_hDMHandle, serial, info)
            Me.FromInfo(info)
            'CONAFreeDeviceInfoStructure(CONAPI_DEVICE_PRODUCT_INFO, ptr)
            'CONAFreeDeviceStructure 
        End Sub

        Public ReadOnly Property FileSystem As CFileSystem
            Get
                SyncLock Me
                    If (Me.m_FileSystem Is Nothing) Then Me.m_FileSystem = New CFileSystem(Me)
                    Return Me.m_FileSystem
                End SyncLock
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.FriendlyName & " (" & Me.SerialNumber & ")"
        End Function

        Private Sub CheckCaps()
            If Me.m_Info.iSize = 0 Then
                Dim ptr As IntPtr
                ' Query device Info
                Dim iResult As Integer = CONAGetDeviceInfo(DMD.Nokia.m_hDMHandle, Me.SerialNumber, CONAPI_DEVICE_GENERAL_INFO, ptr)
                If iResult <> CONA_OK Then ShowErrorMessage("CONAGetDeviceInfo", iResult)
                Me.m_Info = CType(Marshal.PtrToStructure(ptr, GetType(CONAPI_DEVICE_GEN_INFO)), CONAPI_DEVICE_GEN_INFO)
                Me.m_SoftwareName = Me.m_Info.pstrTypeName
                Me.m_SoftwareVersion = Me.m_Info.pstrSWVersion
                Me.m_UsedLanguage = Me.m_Info.pstrUsedLanguage
                iResult = CONAFreeDeviceInfoStructure(CONAPI_DEVICE_GENERAL_INFO, ptr)
                If iResult <> CONA_OK Then ShowErrorMessage("CONAFreeDeviceInfoStructure", iResult)
            End If
        End Sub

        Public ReadOnly Property SoftwareName As String
            Get
                Me.CheckCaps()
                Return Me.m_SoftwareName
            End Get
        End Property

        Public ReadOnly Property SoftwareVersion As String
            Get
                Me.CheckCaps()
                Return Me.m_SoftwareVersion
            End Get
        End Property

        Public ReadOnly Property UsedLanguage As String
            Get
                Me.CheckCaps()
                Return Me.m_UsedLanguage
            End Get
        End Property


        Public Function GetDeviceCAPS(ByVal cap As NokiaDeviceCaps) As Object
            Me.CheckCaps()

            'CONAPI_FS_NOT_SUPPORTED As Integer = &H0               ' Device is not support file system.
            Select Case cap
                Case NokiaDeviceCaps.SupportsFileSystem : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_SUPPORTED) = CONAPI_FS_SUPPORTED
                Case NokiaDeviceCaps.SupportsJavaAppInstallation : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_INSTALL_JAVA_APPLICATIONS) = CONAPI_FS_INSTALL_JAVA_APPLICATIONS
                Case NokiaDeviceCaps.SupportsSISAppInstallation : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_INSTALL_SIS_APPLICATIONS) = CONAPI_FS_INSTALL_SIS_APPLICATIONS
                Case NokiaDeviceCaps.SupportsSISXAppInstallation : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_INSTALL_SISX_APPLICATIONS) = CONAPI_FS_INSTALL_SISX_APPLICATIONS
                Case NokiaDeviceCaps.SupportsFileConversion : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_FILE_CONVERSION) = CONAPI_FS_FILE_CONVERSION
                Case NokiaDeviceCaps.CanListInstalledApplications : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_LIST_APPLICATIONS) = CONAPI_FS_LIST_APPLICATIONS
                Case NokiaDeviceCaps.SupportsUninstallApplication : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_UNINSTALL_APPLICATIONS) = CONAPI_FS_UNINSTALL_APPLICATIONS
                Case NokiaDeviceCaps.SupportsExtendedFSOperations : Return (Me.m_Info.iFileSystemSupport And CONAPI_FS_EXTENDED_OPERATIONS) = CONAPI_FS_EXTENDED_OPERATIONS
                Case NokiaDeviceCaps.SupportsTheme : Return (Me.m_Info.iType And CONAPI_SERIES40_DEVICE) = CONAPI_SERIES40_DEVICE
                Case NokiaDeviceCaps.SupportsSADSSynchronization : Return (Me.m_Info.iSyncSupport And CONAPI_SYNC_SA_DS) = CONAPI_SYNC_SA_DS
                Case NokiaDeviceCaps.SupportsSADMSynchronization : Return (Me.m_Info.iSyncSupport And CONAPI_SYNC_SA_DM) = CONAPI_SYNC_SA_DM
                Case NokiaDeviceCaps.SupportsCIDSSynchronization : Return (Me.m_Info.iSyncSupport And CONAPI_SYNC_CI_DS) = CONAPI_SYNC_CI_DS
                Case NokiaDeviceCaps.SupportsSynchronization : Return Me.m_Info.iSyncSupport <> 0
                Case Else : Return 0
            End Select

        End Function

        Friend Overridable Sub OnRequireUserAction(ByVal e As EventArgs)
            RaiseEvent RequireUserAction(Me, e)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class