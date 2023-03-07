Imports DMD.S300.CKT_DLL
Imports DMD.Internals
Imports System.Runtime.InteropServices


Namespace S300

    Public Enum S300UserType As Integer
        NormalUser = 0
        Administrator = 1
    End Enum

    ''' <summary>
    ''' Classe che rappresenta una persona censita sul dispositivo
    ''' </summary>
    Public Class S300PersonInfo
        Private m_Device As S300Device

        Private m_PersonID As Integer
        Private m_Password As String
        Private m_CardNo As Integer
        Private m_Name As String
        Private m_Dept As Integer '²¿ÃÅ
        Private m_Group As Integer '²¿ÃÅ
        Private m_KQOption As Integer '¿¼ÇÚÄ£Ê½
        Private m_FPMark As Integer
        Private m_Other As Integer 'ÌØÊâÐÅÏ¢ =0 ÆÕÍ¨ÈËÔ±, =1 ¹ÜÀíÔ±
        Private m_FingerPrints As S300FingerPrintsCollection

        Public Sub New()
            Me.m_Device = Nothing
            Me.m_PersonID = 0
            Me.m_Password = ""
            Me.m_CardNo = 0
            Me.m_Name = ""
            Me.m_Dept = 0
            Me.m_Group = 0
            Me.m_KQOption = 0
            Me.m_FPMark = 0
            Me.m_Other = 0
            Me.m_FingerPrints = Nothing

        End Sub

        Friend Sub New(ByVal info As CKT_DLL.PERSONINFO)
            Me.New
            Me.m_PersonID = info.PersonID
            Me.m_Password = System.Text.Encoding.ASCII.GetString(info.Password)
            Me.m_CardNo = info.CardNo
            Me.m_Name = System.Text.Encoding.ASCII.GetString(info.Name)
            Me.m_Dept = info.Dept
            Me.m_Group = info.Group
            Me.m_KQOption = info.KQOption
            Me.m_FPMark = info.FPMark
            Me.m_Other = info.Other
        End Sub

        Friend Sub New(ByVal device As S300Device, ByVal uid As Integer, ByVal userName As String)
            Me.New
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
            Me.m_PersonID = uid
            Me.m_Name = userName
        End Sub

        Public Property PersonID As Integer
            Get
                Return Me.m_PersonID
            End Get
            Set(value As Integer)
                If Me.m_PersonID <> 0 Then Throw New InvalidOperationException("Impossibile modificare l'ID della persona dopo averla salvata")
                Me.m_PersonID = value
            End Set
        End Property

        Public Property Password As String
            Get
                Return Me.m_Password
            End Get
            Set(value As String)
                Me.m_Password = value
            End Set
        End Property

        Public Property CardNo As Integer
            Get
                Return Me.m_CardNo
            End Get
            Set(value As Integer)
                Me.m_CardNo = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                Me.m_Name = value
            End Set
        End Property

        Public Property Department As Integer '²¿ÃÅ
            Get
                Return Me.m_Dept
            End Get
            Set(value As Integer)
                Me.m_Dept = value
            End Set
        End Property

        Public Property Group As Integer '²¿ÃÅ
            Get
                Return Me.m_Group
            End Get
            Set(value As Integer)
                Me.m_Group = value
            End Set
        End Property

        Public Property KQOption As Integer 'modalità di frequenza
            Get
                Return Me.m_KQOption
            End Get
            Set(value As Integer)
                Me.m_KQOption = value
            End Set
        End Property

        Public Property FPMark As Integer
            Get
                Return Me.m_FPMark
            End Get
            Set(value As Integer)
                Me.m_FPMark = value
            End Set
        End Property

        Public Property UserType As S300UserType 'ÌØÊâÐÅÏ¢ =0 ÆÕÍ¨ÈËÔ±, =1 ¹ÜÀíÔ±
            Get
                Return Me.m_Other
            End Get
            Set(value As S300UserType)
                Me.m_Other = value
            End Set
        End Property

        Public ReadOnly Property Device As S300Device
            Get
                Return Me.m_Device
            End Get
        End Property

        Protected Friend Sub SetDevice(ByVal device As S300Device)
            Me.m_Device = device
        End Sub

        ''' <summary>
        ''' Salva le modifiche fatte sulla periferica
        ''' </summary>
        Public Sub Save()
            If Me.Device Is Nothing Then Throw New ArgumentNullException("Device è NULL")
            If Not Me.Device.IsConnected Then Throw New ArgumentNullException("Dispositivo non connesso")


            Dim person As CKT_DLL.PERSONINFO = New CKT_DLL.PERSONINFO()
            person.CardNo = Me.CardNo
            person.Name = System.Text.Encoding.ASCII.GetBytes(Me.Name) : Array.Resize(person.Name, 12)
            person.Password = System.Text.Encoding.ASCII.GetBytes(Me.Password) : Array.Resize(person.Password, 8)
            person.PersonID = Me.m_PersonID
            person.Dept = Me.m_Dept
            person.Group = Me.m_Group
            person.KQOption = Me.m_KQOption
            person.FPMark = Me.m_FPMark
            person.Other = Me.m_Other


            Dim mpiRet As Integer = CKT_DLL.CKT_ModifyPersonInfo(Me.Device.DeviceID, person)
            If (mpiRet = CKT_DLL.CKT_RESULT_ADDOK) Then
                Return '("Edit OK!")
            ElseIf (mpiRet = CKT_DLL.CKT_ERROR_MEMORYFULL) Then
                Throw New OutOfMemoryException("Errore in CKT_ModifyPersonInfo: Memoria piena") 'MessageBox.Show("MEMORY FUL")
            Else
                'MessageBox.Show("Edit ERROR!")
                Throw New OutOfMemoryException("Errore in CKT_ModifyPersonInfo: " & Marshal.GetLastWin32Error)
            End If
        End Sub

        ''' <summary>
        ''' Elimina l'utente sul dispositivo
        ''' </summary>
        Public Sub Delete()
            'SyncLock Me.Device
            If Me.Device Is Nothing Then Throw New ArgumentNullException("Device è NULL")
            If Not Me.Device.IsConnected Then Throw New ArgumentNullException("Dispositivo non connesso")

            Dim dpiRet As Integer = 0
            dpiRet = CKT_DLL.CKT_DeletePersonInfo(Me.Device.DeviceID, Me.PersonID, Me.PersonID) '&H4
            If (dpiRet = CKT_DLL.CKT_RESULT_OK) Then
                Me.Device.Users.NotifyDelete(Me)
                Return '("Delete OK!")
            ElseIf (dpiRet = CKT_DLL.CKT_ERROR_NOTHISPERSON) Then
                Throw New KeyNotFoundException("Nessun utente con questo ID sul dispositivo")
            Else
                Throw New Exception("Errore in CKT_DeletePersonInfo: " & Marshal.GetLastWin32Error)
            End If
            ' End SyncLock
        End Sub

        ''' <summary>
        ''' Restituisce la collezione delle impronte digitali registrate per questo utente
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FingerPrints As S300FingerPrintsCollection
            Get
                SyncLock Me
                    If (Me.m_FingerPrints Is Nothing) Then Me.m_FingerPrints = New S300FingerPrintsCollection(Me)
                    Return Me.m_FingerPrints
                End SyncLock
            End Get
        End Property

    End Class


End Namespace
