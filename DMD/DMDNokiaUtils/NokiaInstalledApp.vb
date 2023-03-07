Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS
Imports DMD.Internals

Partial Class Nokia

    Public Enum CONA_APPLICATION_TYPE As Integer
        SIS = CONA_APPLICATION_TYPE_SIS ' Use when struct type is CONAPI_APPLICATION_SIS
        JAVA = CONA_APPLICATION_TYPE_JAVA ' Use when struct type is CONA_APPLICATION_TYPE_JAVA
        THEMES = CONA_APPLICATION_TYPE_THEMES   ' Use when struct type is CONAPI_APPLICATION_THEMES
        UNKNOWN = CONA_APPLICATION_TYPE_UNKNOWN ' Use only in CONAPI_APPLICATION_INFO struct, unknown application type.
    End Enum

    ''' <summary>
    ''' Rappresenta un'applicazione installata sul telefonino
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NokiaInstalledApp
        Inherits CBaseItem

        Private m_Name As String ' Application name. Always exist.
        Private m_Description As String 'Application description. If not available, pointer is NULL.
        Private m_Vendor As String 'Application vendor. If not available, pointer is NULL.
        Private m_Version As String ' Application version. If not available, pointer is NULL.
        Private m_ParentAppNam As String ' Parent application name. This is available if application is augmentation for some other application. 
        Private m_ApplicationSize As Integer                                    ' Application size in bytes. If not available, value is -1 (0xFFFFFFFF).
        Private m_ApplicationType As CONA_APPLICATION_TYPE                      ' Application type possible values:
        Private m_ApplicationID As String ' Application UID string, used with CONAUninstallApplication function.
        Private m_Options As Integer                                            ' Reserved for future use. Value is zero.
        Private m_Value As String               ' Reserved for future use. Pointer is NULL.

        Public Sub New()
            Me.m_Name = ""
            Me.m_Description = ""
            Me.m_Vendor = ""
            Me.m_Version = ""
            Me.m_ParentAppNam = ""
            Me.m_ApplicationSize = 0
            Me.m_ApplicationType = CONA_APPLICATION_TYPE.UNKNOWN
            Me.m_ApplicationID = ""
            Me.m_Options = 0
            Me.m_Value = ""
        End Sub

        Friend Sub New(ByVal device As NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.SetDevice(device)
        End Sub

        Public ReadOnly Property Name As String
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property Description As String
            Get
                Return Me.m_Description
            End Get
        End Property

        Public ReadOnly Property Vendor As String
            Get
                Return Me.m_Vendor
            End Get
        End Property

        Public ReadOnly Property Version As String
            Get
                Return Me.m_Version
            End Get
        End Property

        Public ReadOnly Property ParentAppNam As String
            Get
                Return Me.m_ParentAppNam
            End Get
        End Property

        Public ReadOnly Property ApplicationSize As Integer
            Get
                Return Me.m_ApplicationSize
            End Get
        End Property

        Public ReadOnly Property ApplicationType As CONA_APPLICATION_TYPE
            Get
                Return Me.m_ApplicationType
            End Get
        End Property

        Public ReadOnly Property ApplicationID As String
            Get
                Return Me.m_ApplicationID
            End Get
        End Property

        Public ReadOnly Property ApplicationTypeEx As String
            Get
                Select Case Me.m_ApplicationType
                    Case CONA_APPLICATION_TYPE.JAVA : Return "Java"
                    Case CONA_APPLICATION_TYPE.SIS : Return "SIS"
                    Case CONA_APPLICATION_TYPE.THEMES : Return "THEMES"
                    Case Else : Return "Unknown"
                End Select
            End Get
        End Property

        Protected Overrides Sub InternalDelete()
            Me.Uninstall()
        End Sub

            

        Friend Sub FromInfo(ByVal info As CONAPI_APPLICATION_INFO)
            Me.m_Name = info.pstrName
            Me.m_Description = info.pstrDescription
            Me.m_ApplicationID = info.pstrUID
            Me.m_ApplicationSize = info.dwApplicationSize
            Me.m_ApplicationType = info.dwApplicationType
            Me.m_Options = info.dwOptions
            Me.m_ParentAppNam = info.pstrParentAppNam
            Me.m_Value = info.pstrValue
            Me.m_Vendor = info.pstrVendor
            Me.m_Version = info.pstrVersion
        End Sub

        Public Sub Uninstall()
            ' Create FS connection
            Dim iResult As Integer = CONAUninstallApplication(Me.Device.FileSystem.GetHandle, CONA_SILENT_UNINSTALLATION, Me.Name, Me.ApplicationID)
            If iResult <> CONA_OK Then ShowErrorMessage("CONAUninstallApplication", iResult)

            Me.Device.InstalledApplications.Remove(Me)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Name & " (" & Me.m_ApplicationID & ")"
        End Function
    End Class

End Class