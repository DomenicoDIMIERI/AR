Imports DMD.S300.CKT_DLL
Imports System.Runtime.InteropServices


Namespace S300

    Public Enum S300ClockingType As Integer
        [In] = 0
        [Out] = 1
    End Enum


    ''' <summary>
    ''' Oggetto che rappresenta una marcatura temporale di un utente
    ''' </summary>
    Public Class S300Clocking
        Implements IComparable

        Private m_Device As S300Device
        Private m_PersonID As Integer
        Private m_Person As S300PersonInfo
        Private m_Time As Date
        Private m_Type As S300ClockingType
        Private m_DeviceID As Integer

        'Dim item1 As New ListViewItem("item1", 0)
        'Dim item1 As New ListViewItem(ListView1.Items.Count)
        'item1.SubItems.Add(clocking.PersonID.ToString())
        'item1.SubItems.Add(Encoding.Default.GetString(clocking.Time))
        'item1.SubItems.Add(clocking.Stat.ToString())
        'item1.SubItems.Add(clocking.ID.ToString())

        Public Sub New()
            Me.m_Device = Nothing
            Me.m_PersonID = 0
            Me.m_Person = Nothing
            Me.m_Time = Nothing
            Me.m_Type = S300ClockingType.In
            Me.m_DeviceID = 0
        End Sub

        Friend Sub New(ByVal device As S300Device, ByVal info As CLOCKINGRECORD)
            Me.New
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
            Me.m_DeviceID = info.ID
            Me.m_PersonID = info.PersonID
            Me.m_Type = IIf((info.Stat And 1) = 1, S300ClockingType.Out, S300ClockingType.In)
            Me.m_Time = Me.ParseDate(System.Text.Encoding.ASCII.GetString(info.Time))
        End Sub

        Private Function ParseDate(ByVal value As String) As Date
            Dim dt() As String = Split(Trim(value), " ")
            If dt Is Nothing OrElse dt.Length <> 2 Then Throw New FormatException("Date format exception: " & value)
            Dim ymd As String() = Split(dt(0), "-")
            Dim hms As String() = Split(dt(1), ":")
            Return New Date(ymd(0), ymd(1), ymd(2), hms(0), hms(1), hms(2))
        End Function

        ''' <summary>
        ''' Restituisce l'ID del dispositivo su cui è registrata la marcatura
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DeviceID As Integer
            Get
                Return Me.m_DeviceID
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il dispositivo su cui è registrata la marcatura
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Device As S300Device
            Get
                Return Me.m_Device
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'ID della persona che ha effettuato la marcatura
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PersonID As Integer
            Get
                Return Me.m_PersonID
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la persona che ha effettuato la marcatura
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Person As S300PersonInfo
            Get
                If (Me.m_Person Is Nothing) Then Me.m_Person = Me.Device.Users.GetItemById(Me.m_PersonID)
                Return Me.m_Person
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il tipo di marcatura Ingresso/Uscita
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Type As S300ClockingType
            Get
                Return Me.m_Type
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora della marcatura
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Time As Date
            Get
                Return Me.m_Time
            End Get
        End Property

        Public ReadOnly Property TypeEx As String
            Get
                Select Case Me.m_Type
                    Case S300ClockingType.In : Return "In"
                    Case S300ClockingType.Out : Return "Out"
                    Case Else : Return "?"
                End Select
            End Get
        End Property


        Public Function CompareTo(ByVal obj As S300Clocking) As Integer
            Dim ret As Integer = Me.Time.CompareTo(obj.Time)
            If (ret = 0) Then
                Dim o1 As Integer = IIf(Me.Type = S300ClockingType.In, 0, 1)
                Dim o2 As Integer = IIf(obj.Type = S300ClockingType.In, 0, 1)
                ret = o1.CompareTo(o2)
            End If
            If (ret = 0) Then ret = Me.PersonID.CompareTo(obj.PersonID)
            If (ret = 0) Then ret = Me.DeviceID.CompareTo(obj.DeviceID)
            Return ret
        End Function


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

    End Class


End Namespace
