Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS
Imports DMD.Internals

Partial Class Nokia

    Public Enum MediaTypes As Integer
        IrDA = API_MEDIA_IRDA
        Serial = API_MEDIA_SERIAL
        Bluetooth = API_MEDIA_BLUETOOTH
        USB = API_MEDIA_USB
    End Enum

    ''' <summary>
    ''' Rappresenta una cartella che contiene SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CMemoryType
        Friend m_Name As String
        Friend m_MediaType As MediaTypes
        Friend m_FS As CFileSystem
        Friend m_iFreeMem As Int64
        Friend m_iUsedMem As Int64
        Friend m_iTotalMem As Int64


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_FS = Nothing
            Me.m_MediaType = MediaTypes.Serial
            Me.m_Name = ""
            Me.m_iFreeMem = -1
            Me.m_iUsedMem = -1
            Me.m_iTotalMem = -1
        End Sub

        Friend Sub New(ByVal fs As CFileSystem)
            Me.New()
            If (fs Is Nothing) Then Throw New ArgumentNullException("gs")
            Me.SetFileSystem(fs)
        End Sub

        Public ReadOnly Property FileSystem As CFileSystem
            Get
                Return Me.m_FS
            End Get
        End Property

        Public ReadOnly Property Device As NokiaDevice
            Get
                If (Me.m_FS Is Nothing) Then Return Nothing
                Return Me.m_FS.Device
            End Get
        End Property


        Protected Friend Sub SetFileSystem(ByVal fs As CFileSystem)
            Me.m_FS = fs
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property Name As String
            Get
                Return Me.m_Name
            End Get
        End Property


        Public ReadOnly Property MediaType As MediaTypes
            Get
                Return Me.m_MediaType
            End Get
        End Property

        Public ReadOnly Property MediaTypeEx As String
            Get
                Select Case Me.m_MediaType
                    Case MediaTypes.Bluetooth : Return "Bluetooth"
                    Case MediaTypes.IrDA : Return "IrDA"
                    Case MediaTypes.Serial : Return "Serial"
                    Case MediaTypes.USB : Return "USB"
                    Case Else : Return "Unknown media"
                End Select
            End Get
        End Property

        Public ReadOnly Property FreeMemory As Int64
            Get
                Return Me.m_iFreeMem
            End Get
        End Property

        Public ReadOnly Property UsedMemory As Int64
            Get
                Return Me.m_iUsedMem
            End Get
        End Property

        Public ReadOnly Property TotalMemory As Int64
            Get
                Return Me.m_iTotalMem
            End Get
        End Property

    End Class

End Class