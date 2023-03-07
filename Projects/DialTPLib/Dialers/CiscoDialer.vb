Imports System.IO
Imports System.Threading.Thread

Public Class CiscoDialer
    Inherits DialerBaseClass

    Private m_Thread As System.Threading.Thread
    Private m_DeviceIP As String = ""
    Private m_DeviceName As String = ""

    Public Sub New()
        Me.m_Thread = Nothing
    End Sub

    Public Sub New(ByVal deviceIP As String, ByVal deviceName As String)
        Me.New
        deviceIP = Trim(deviceIP)
        If (deviceIP = "") Then Throw New ArgumentNullException("deviceIP")
        Me.m_DeviceIP = deviceIP
        Me.m_DeviceName = deviceName
    End Sub

    ''' <summary>
    ''' Restituisce o imposta l'IP del dispositivo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeviceIP As String
        Get
            Return Me.m_DeviceIP
        End Get
        Set(value As String)
            value = Trim(value)
            If (value = "") Then Throw New ArgumentNullException("DeviceIP")
            Me.m_DeviceIP = value
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta il nome del dispositivo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeviceName As String
        Get
            Return Me.m_DeviceName
        End Get
        Set(value As String)
            Me.m_DeviceName = value
        End Set
    End Property

    Function PrepareNumber(ByVal number As String) As String
        Return Trim(number)
    End Function

    Public Overrides Sub Dial(number As String)
        If Not Me.IsInstalled Then Return
        Me.HangUp()
        If (Len(number) <= 1) Then Return

        Me.m_Thread = New System.Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf thread))
        Me.m_Thread.Start(number)
    End Sub

    Private Sub thread(ByVal o As Object)
        Dim Number As String = CStr(o)

        Dim e As New DialEventArgs(Number)
        Me.OnBegidDial(e)

        Const sleeptime As Integer = CInt(0.5 * 1000)
        Dim telnet As New DMD.Net.Telnet.TelnetClient
        telnet.Host = Me.m_DeviceIP
        telnet.Port = 23
        telnet.Connect()

        Dim password As String = "cisco" ' cisco phone password
        Dim mute As Integer = 0 ' mute on a dial 0/1
        Dim prompt As String = ">"

        'telnet = new Net::Telnet ( Timeout=>3, Errmode=>'die');
        ' connecting
        telnet.WaitFor("Password :")
        telnet.Println(password)
        telnet.WaitFor(prompt)

        'telnet.Print("test close")

        telnet.Println("test open")
        telnet.WaitFor(prompt)
        Sleep(sleeptime)

        telnet.Println("test key spkr")
        telnet.WaitFor(prompt)
        Sleep(sleeptime)

        If (mute <> 0) Then
            telnet.Println("test key mute")
            telnet.WaitFor(prompt)
            Sleep(sleeptime)
        End If

        telnet.Println("test key " & Number)
        telnet.WaitFor(prompt)
        Sleep((Number.Length) * sleeptime)

        telnet.Println("test close")
        telnet.WaitFor(prompt)
        Sleep(sleeptime)

        telnet.Close()

        Me.OnEndDial(e)

    End Sub

    Public Overrides Function IsInstalled() As Boolean
        Return Me.m_DeviceIP <> ""
    End Function

    
    Public Overrides ReadOnly Property Name As String
        Get
            Return "Cisco " & Me.m_DeviceName
        End Get
    End Property

    Public Overrides Sub HangUp()
        If (Me.m_Thread IsNot Nothing) Then
            Me.m_Thread.Abort()
            Me.m_Thread = Nothing
        End If
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is C3CXDialer) Then Return False
        Return MyBase.Equals(obj) AndAlso DirectCast(obj, CiscoDialer).DeviceIP = Me.DeviceIP AndAlso DirectCast(obj, CiscoDialer).DeviceName = Me.DeviceName
    End Function

End Class
