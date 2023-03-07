'Imports Microsoft.VisualBasic.Compatibility.VB6;
Imports System
Imports System.Collections
Imports System.Data
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Namespace S300

    Public Class CKT_DLL

        ' Consts

        Public Const CKT_ERROR_INVPARAM As Short = -1
        Public Const CKT_ERROR_NETDAEMONREADY As Short = -1
        Public Const CKT_ERROR_CHECKSUMERR As Short = -2
        Public Const CKT_ERROR_MEMORYFULL As Short = -1
        Public Const CKT_ERROR_INVFILENAME As Short = -3
        Public Const CKT_ERROR_FILECANNOTOPEN As Short = -4
        Public Const CKT_ERROR_FILECONTENTBAD As Short = -5
        Public Const CKT_ERROR_FILECANNOTCREATED As Short = -2
        Public Const CKT_ERROR_NOTHISPERSON As Short = -1

        Public Const CKT_RESULT_OK As Short = 1
        Public Const CKT_RESULT_ADDOK As Short = 1
        Public Const CKT_RESULT_HASMORECONTENT As Short = 2


        'Public Const PERSONINFOSIZE As Short = 44
        Public Shared CLOCKINGRECORDSIZE As Short

        Shared Sub New()
            Dim clocking As CKT_DLL.CLOCKINGRECORD = New CKT_DLL.CLOCKINGRECORD()
            CKT_DLL.CLOCKINGRECORDSIZE = Convert.ToInt16(Marshal.SizeOf(clocking))

        End Sub

        ' Types


        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure NETINFO
            <MarshalAs(UnmanagedType.I4)> Public ID As Integer
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public IP As Byte()
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public Mask As Byte()
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public Gateway As Byte()
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public ServerIP As Byte()
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=6)> Public MAC As Byte()
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure DATETIMEINFO
            Public ID As Integer
            Public Year_Renamed As Short
            Public Month_Renamed As Byte
            Public Day_Renamed As Byte
            Public Hour_Renamed As Byte
            Public Minute_Renamed As Byte
            Public Second_Renamed As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure PERSONINFO
            <MarshalAs(UnmanagedType.I4)> Public PersonID As Integer
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> Public Password As Byte()
            <MarshalAs(UnmanagedType.I4)> Public CardNo As Integer
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=12)> Public Name As Byte()
            <MarshalAs(UnmanagedType.I4)> Public Dept As Integer '²¿ÃÅ
            <MarshalAs(UnmanagedType.I4)> Public Group As Integer '²¿ÃÅ
            <MarshalAs(UnmanagedType.I4)> Public KQOption As Integer '¿¼ÇÚÄ£Ê½
            <MarshalAs(UnmanagedType.I4)> Public FPMark As Integer
            <MarshalAs(UnmanagedType.I4)> Public Other As Integer '0 = Normal User, 1 = Administrator
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure CLOCKINGRECORD
            <MarshalAs(UnmanagedType.I4)> Public ID As Integer
            <MarshalAs(UnmanagedType.I4)> Public PersonID As Integer
            <MarshalAs(UnmanagedType.I4)> Public Stat As Integer
            <MarshalAs(UnmanagedType.I4)> Public BackupCode As Integer
            <MarshalAs(UnmanagedType.I4)> Public WorkTyte As Integer
            '<VBFixedString(20), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=20)> _
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=20)> Public Time As Byte()
        End Structure

#If 0 Then
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure DEVICEINFO
            Public ID As Integer
            Public MajorVersion As Integer
            Public MinorVersion As Integer
            Public SpeakerVolume As Integer
            Public AdminPassword As Integer
            '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> Public AdminPassword As Byte()
            'Public AdminPassword As IntPtr
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=28)> Public Padding1 As Byte()

            Public DoorLockDelay As Integer
            Public Parameter As Integer
            Public DefaultAuth As Integer
            Public Capacity As Integer

            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=128)> Public Padding2 As Byte()
        End Structure

#End If



        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure DEVICEINFO
            Public ID As Integer ' Device serial number
            Public MajorVersion As Integer ' Device firmware version
            Public MinorVersion As Integer ' Device firmware version
            Public SpeakerVolume As Integer ' Speaker volume
            Public Parameter As Integer '
            Public DefaultAuth As Integer ''Public DoorLockDelay As Integer ' Lock control delay
            Public FixWGHead As Integer 'Fixed wiegand area code 
            Public WGOption As Integer 'wiegand Option
            Public AutoUpdateAllow As Integer 'Allow To update the fingerprint template intelligently
            Public KQRepeatTime As Integer 'Options For the repeated clocking
            Public RealTimeAllow As Integer 'Allow To transfer data real-time.
            Public RingAllow As Integer 'Allow To make ringing
            Public LockDelayTime As Integer 'Lock delay time
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> Public AdminPassword As Byte()
            Public Capacity As Integer ' Capacity For the staffer information





        End Structure

        Public Enum WeekDaysEnum As Integer
            SUN = 1
            MON = 2
            TUE = 4
            WED = 8
            THU = 16
            FRI = 32
            SAT = 64
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure RINGTIME


            <MarshalAs(UnmanagedType.I4)> Public hour As Integer
            <MarshalAs(UnmanagedType.I4)> Public minute As Integer
            <MarshalAs(UnmanagedType.I4)> Public week As Integer

            Public Function TestWeekDay(ByVal wd As WeekDaysEnum) As Boolean
                Return (Me.week And wd) = wd
            End Function

            Public Sub SetWeekDay(ByVal wd As WeekDaysEnum, ByVal value As Boolean)
                If (value) Then
                    Me.week = Me.week Or wd
                Else
                    Me.week = Me.week And Not CInt(wd)
                End If
            End Sub

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure TIMESECT
            Public bHour As Byte
            Public bMinute As Byte
            Public eHour As Byte
            Public eMinute As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure CKT_MessageInfo
            <MarshalAs(UnmanagedType.I4)> Public PersonID As Integer
            <MarshalAs(UnmanagedType.I4)> Public sYear As Integer
            <MarshalAs(UnmanagedType.I4)> Public sMon As Integer
            <MarshalAs(UnmanagedType.I4)> Public sDay As Integer
            <MarshalAs(UnmanagedType.I4)> Public eYear As Integer
            <MarshalAs(UnmanagedType.I4)> Public eMon As Integer
            <MarshalAs(UnmanagedType.I4)> Public eDay As Integer
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=48)> Public msg As Byte()
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure CKT_MessageHead
            <MarshalAs(UnmanagedType.I4)> Public PersonID As Integer
            <MarshalAs(UnmanagedType.I4)> Public sYear As Integer
            <MarshalAs(UnmanagedType.I4)> Public sMon As Integer
            <MarshalAs(UnmanagedType.I4)> Public sDay As Integer
            <MarshalAs(UnmanagedType.I4)> Public eYear As Integer
            <MarshalAs(UnmanagedType.I4)> Public eMon As Integer
            <MarshalAs(UnmanagedType.I4)> Public eDay As Integer
        End Structure

        ' Routines

        <DllImport("tc400.dll", EntryPoint:="CKT_FreeMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_FreeMemory(ByVal memory As IntPtr) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_RegisterSno", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_RegisterSno(ByVal Sno As Integer, ByVal ComPort As Integer) As Integer

        End Function

        'Public Declare Function CKT_RegisterNet Lib "tc400.dll" (ByVal Sno As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal Addr As String) As Integer
        <DllImport("tc400.dll", EntryPoint:="CKT_RegisterNet", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_RegisterNet(ByVal Sno As Integer, ByVal Addr As String) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_UnregisterSnoNet", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub CKT_UnregisterSnoNet(ByVal Sno As Integer)

        End Sub

        <DllImport("tc400.dll", EntryPoint:="CKT_NetDaemon", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_NetDaemon() As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ComDaemon", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ComDaemon() As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_Disconnect", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub CKT_Disconnect()

        End Sub

        <DllImport("tc400.dll", EntryPoint:="CKT_ReportConnections", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ReportConnections(ByRef ppSno As IntPtr) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetDeviceNetInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetDeviceNetInfo(ByVal Sno As Integer, ByRef pNetInfo As NETINFO) As Integer

        End Function

        'Public Declare Function CKT_SetDeviceIPAddr Lib "tc400.dll" (ByVal Sno As Integer, ByRef IP As Byte) As Integer
        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceIPAddr", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceIPAddr(ByVal Sno As Integer, ByVal IP As Byte()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceMask", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceMask(ByVal Sno As Integer, ByRef Mask As Byte) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceMask", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceMask(ByVal Sno As Integer, ByVal Mask As Byte()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceGateway", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceGateway(ByVal Sno As Integer, ByRef Gate As Byte) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceGateway", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceGateway(ByVal Sno As Integer, ByVal Gate As Byte()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceServerIPAddr", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceServerIPAddr(ByVal Sno As Integer, ByRef Svr As Byte) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceServerIPAddr", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceServerIPAddr(ByVal Sno As Integer, ByVal Svr As Byte()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceMAC", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceMAC(ByVal Sno As Integer, ByRef MAC As Byte) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceMAC", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceMAC(ByVal Sno As Integer, ByVal MAC As Byte()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetDeviceClock", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetDeviceClock(ByVal Sno As Integer, ByRef pDateTimeInfo As DATETIMEINFO) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceDate", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceDate(ByVal Sno As Integer, ByVal Year_Renamed As Short, ByVal Month_Renamed As Byte, ByVal Day_Renamed As Byte) As Integer

        End Function


        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceTime", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceTime(ByVal Sno As Integer, ByVal Hour_Renamed As Byte, ByVal Minute_Renamed As Byte, ByVal Second_Renamed As Byte) As Integer

        End Function


        <DllImport("tc400.dll", EntryPoint:="CKT_GetFPTemplate", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetFPTemplate(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByRef pFPData As IntPtr, ByRef FPDataLen As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetFPTemplate", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetFPTemplate(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByVal pFPData As Byte(), ByRef FPDataLen As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_PutFPTemplate", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_PutFPTemplate(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByVal pFPData As Byte(), ByVal FPDataLen As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetFPTemplateSaveFile", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetFPTemplateSaveFile(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByVal FPDataFilename As String) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_PutFPTemplateLoadFile", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_PutFPTemplateLoadFile(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByVal FPDataFilename As String) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetFPRawData", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetFPRawData(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByRef FPRawData As Byte) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_PutFPRawData", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_PutFPRawData(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByRef FPRawData As Byte) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetFPRawDataSaveFile", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetFPRawDataSaveFile(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByVal FPDataFilename As String) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_PutFPRawDataLoadFile", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_PutFPRawDataLoadFile(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal FPID As Integer, ByVal FPDataFilename As String) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ListPersonInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ListPersonInfo(ByVal Sno As Integer, ByRef pRecordCount As Integer, ByRef ppPersons As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ListPersonInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ListPersonInfo(ByVal Sno As Integer, ByRef pRecordCount As Integer, ByRef ppPersons As CKT_DLL.PERSONINFO) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ModifyPersonInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ModifyPersonInfo(ByVal Sno As Integer, ByRef person As PERSONINFO) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_DeletePersonInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_DeletePersonInfo(ByVal Sno As Integer, ByVal PersonID As Integer, ByVal backupID As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_DeleteAllPersonInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_DeleteAllPersonInfo(ByVal Sno As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ListPersonInfoEx", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ListPersonInfoEx(ByVal Sno As Integer, ByRef ppLongRun As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ListPersonProgress", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ListPersonProgress(ByVal pLongRun As Integer, ByRef pRecCount As Integer, ByRef pRetCount As Integer, ByRef ppPersons As IntPtr) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetCounts", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetCounts(ByVal Sno As Integer, ByRef pPersonCount As Integer, ByRef pFPCount As Integer, ByRef pClockingsCount As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ClearClockingRecord", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ClearClockingRecord(ByVal Sno As Integer, ByVal type As Integer, ByVal count As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetClockingRecordEx", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetClockingRecordEx(ByVal Sno As Integer, ByRef ppLongRun As IntPtr) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetClockingNewRecordEx", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetClockingNewRecordEx(ByVal Sno As Integer, ByRef ppLongRun As IntPtr) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetClockingRecordProgress", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetClockingRecordProgress(ByVal pLongRun As Integer, ByRef pRecCount As Integer, ByRef pRetCount As Integer, ByRef ppPersons As IntPtr) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ResetDevice", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ResetDevice(ByVal Sno As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetDeviceInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetDeviceInfo(ByVal Sno As Integer, ByRef devinfo As DEVICEINFO) As Integer

        End Function



        <DllImport("tc400.dll", EntryPoint:="CKT_SetDefaultAuth", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDefaultAuth(ByVal Sno As Integer, ByVal Auth As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDoor", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDoor(ByVal Sno As Integer, ByVal Second_Renamed As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetSpeakerVolume", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetSpeakerVolume(ByVal Sno As Integer, ByVal Volume As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetDeviceAdminPassword", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetDeviceAdminPassword(ByVal Sno As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal Password As String) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetRealtimeMode", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetRealtimeMode(ByVal Sno As Integer, ByVal RealMode As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetFixWGHead", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetFixWGHead(ByVal Sno As Integer, ByVal WGHead As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetWG", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetWG(ByVal Sno As Integer, ByVal WGMode As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetRingAllow", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetRingAllow(ByVal Sno As Integer, ByVal type As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetRepeatKQ", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetRepeatKQ(ByVal Sno As Integer, ByVal time As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetAutoUpdate", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetAutoUpdate(ByVal Sno As Integer, ByVal AutoUpdate As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ForceOpenLock", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ForceOpenLock(ByVal Sno As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_ReadRealtimeClocking", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_ReadRealtimeClocking(ByRef ppClockings As Integer) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetTimeSection", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetTimeSection(ByVal Sno As Integer, ByVal ord As Integer, <Out> ByVal ts As TIMESECT()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetTimeSection", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetTimeSection(ByVal Sno As Integer, ByVal ord As Integer, <[In]> ByVal ts As TIMESECT()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetGroup", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetGroup(ByVal Sno As Integer, ByVal ord As Integer, <Out> ByVal grp As Integer()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetGroup", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetGroup(ByVal Sno As Integer, ByVal ord As Integer, ByVal grp As Integer()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetHitRingInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetHitRingInfo(ByVal Sno As Integer, <Out> ByVal array As RINGTIME()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_SetHitRingInfo", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_SetHitRingInfo(ByVal Sno As Integer, ByVal ord As Integer, ByRef ring As RINGTIME) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_GetMessageByIndex", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetMessageByIndex(ByVal Sno As Integer, ByVal idx As Integer, ByRef msg As CKT_MessageInfo) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_AddMessage", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_AddMessage(ByVal Sno As Integer, ByRef msg As CKT_MessageInfo) As Integer

        End Function

        'Public Declare Function CKT_GetAllMessageHead Lib "tc400.dll" (ByVal Sno As Integer, <[In](), Out()> ByVal mh As CKT_MessageHead()) As Integer
        <DllImport("tc400.dll", EntryPoint:="CKT_GetAllMessageHead", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_GetAllMessageHead(ByVal Sno As Integer, <Out> ByVal mh As CKT_MessageHead()) As Integer

        End Function

        <DllImport("tc400.dll", EntryPoint:="CKT_DelMessageByIndex", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Public Shared Function CKT_DelMessageByIndex(ByVal Sno As Integer, ByVal idx As Integer) As Integer

        End Function

    End Class


End Namespace
