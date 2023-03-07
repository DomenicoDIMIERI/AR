Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading


Public Class Mouse

    Private Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As MouseProcDelegate, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
    Private Declare Function CallNextHookEx Lib "user32" (ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer
    Private Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hHook As Integer) As Integer

    Private Delegate Function MouseProcDelegate(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer

    Private Structure MSLLHOOKSTRUCT
        Public pt As Point
        Public mouseData As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure

    Public Enum Wheel_Direction
        WheelUp
        WheelDown
    End Enum

    Private Const HC_ACTION As Integer = 0
    Private Const WH_MOUSE_LL As Integer = 14

    Private Const WM_MOUSEMOVE As Integer = &H200
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_LBUTTONUP As Integer = &H202
    Private Const WM_LBUTTONDBLCLK As Integer = &H203
    Private Const WM_RBUTTONDOWN As Integer = &H204
    Private Const WM_RBUTTONUP As Integer = &H205
    Private Const WM_RBUTTONDBLCLK As Integer = &H206
    Private Const WM_MBUTTONDOWN As Integer = &H207
    Private Const WM_MBUTTONUP As Integer = &H208
    Private Const WM_MBUTTONDBLCLK As Integer = &H209
    Private Const WM_MOUSEWHEEL As Integer = &H20A

    Public Shared Event Mouse_Move(ByVal ptLocat As Point)
    Public Shared Event Mouse_Left_Down(ByVal ptLocat As Point)
    Public Shared Event Mouse_Left_Up(ByVal ptLocat As Point)
    Public Shared Event Mouse_Left_DoubleClick(ByVal ptLocat As Point)
    Public Shared Event Mouse_Right_Down(ByVal ptLocat As Point)
    Public Shared Event Mouse_Right_Up(ByVal ptLocat As Point)
    Public Shared Event Mouse_Right_DoubleClick(ByVal ptLocat As Point)
    Public Shared Event Mouse_Middle_Down(ByVal ptLocat As Point)
    Public Shared Event Mouse_Middle_Up(ByVal ptLocat As Point)
    Public Shared Event Mouse_Middle_DoubleClick(ByVal ptLocat As Point)
    Public Shared Event Mouse_Wheel(ByVal ptLocat As Point, ByVal Direction As Wheel_Direction)


    Private Shared MouseHook As Integer
    Private Shared MouseHookDelegate As MouseProcDelegate
    Private Shared lock As New Object
    Private Shared m_LastPosition As Point


    'Public Shared Sub Hook()
    '    MouseHookDelegate = New MouseProcDelegate(AddressOf MouseProc)
    '    MouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0)
    '    If (MouseHook = 0) Then
    '        DIALTPLib.Log.GetCurrSession.Append("Mouse hook failed: " & Err.LastDllError)
    '    Else
    '        DIALTPLib.Log.GetCurrSession.Append("Mouse Hook: " & MouseHook.ToString)
    '    End If
    'End Sub

    Public Shared Sub Hook(ByVal hModule As Integer)
        If (IsHooked) Then Return

        MouseHookDelegate = New MouseProcDelegate(AddressOf MouseProc)
        MouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, hModule, 0)
        If (MouseHook = 0) Then
            DIALTPLib.Log.LogMessage("Mouse hook failed: " & Err.LastDllError)
        Else
            DIALTPLib.Log.LogMessage("Mouse Hook: " & MouseHook.ToString)
        End If
    End Sub

    Public Shared Sub UnHook()
        If (IsHooked()) Then
            UnhookWindowsHookEx(MouseHook)
            DIALTPLib.Log.LogMessage("Mouse UnHook: " & MouseHook.ToString)
        End If
        MouseHook = 0
    End Sub

    Public Shared ReadOnly Property Position As Point
        Get
            SyncLock lock
                Return m_LastPosition
            End SyncLock
        End Get
    End Property

    Private Shared Function MouseProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer
        If (nCode >= HC_ACTION) Then
            SyncLock lock
                m_LastPosition = lParam.pt
            End SyncLock

            Select Case wParam
                Case WM_MOUSEMOVE
                    RaiseEvent Mouse_Move(lParam.pt)
                Case WM_LBUTTONDOWN
                    'DIALTP.Log.CheckWindow()
                    'DIALTP.Log.TakeScreenShot()
                    RaiseEvent Mouse_Left_Down(lParam.pt)
                Case WM_LBUTTONUP
                    'DIALTP.Log.CheckWindow()

                    Log.TakeMouseScreenShot()
                    RaiseEvent Mouse_Left_Up(lParam.pt)
                Case WM_LBUTTONDBLCLK

                    Log.TakeMouseScreenShot()
                    RaiseEvent Mouse_Left_DoubleClick(lParam.pt)
                Case WM_RBUTTONDOWN

                    Log.TakeMouseScreenShot()
                    RaiseEvent Mouse_Right_Down(lParam.pt)
                Case WM_RBUTTONUP
                    Log.TakeMouseScreenShot()

                    RaiseEvent Mouse_Right_Up(lParam.pt)
                Case WM_RBUTTONDBLCLK

                    Log.TakeMouseScreenShot()
                    RaiseEvent Mouse_Right_DoubleClick(lParam.pt)
                Case WM_MBUTTONDOWN

                    RaiseEvent Mouse_Middle_Down(lParam.pt)
                Case WM_MBUTTONUP
                    Log.TakeMouseScreenShot()

                    RaiseEvent Mouse_Middle_Up(lParam.pt)
                Case WM_MBUTTONDBLCLK

                    Log.TakeMouseScreenShot()
                    RaiseEvent Mouse_Middle_DoubleClick(lParam.pt)

                Case WM_MOUSEWHEEL
                    Dim wDirection As Wheel_Direction
                    If lParam.mouseData < 0 Then
                        wDirection = Wheel_Direction.WheelDown
                    Else
                        wDirection = Wheel_Direction.WheelUp
                    End If
                    RaiseEvent Mouse_Wheel(lParam.pt, wDirection)
            End Select
        End If

        Return CallNextHookEx(MouseHook, nCode, wParam, lParam)
    End Function

    Shared Function IsHooked() As Boolean
        Return MouseHook <> 0
    End Function



End Class

