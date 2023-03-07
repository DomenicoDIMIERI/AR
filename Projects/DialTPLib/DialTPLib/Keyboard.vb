Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading


Public Class Keyboard
    '' Low-Level Keyboard Constants
    Private Const HC_ACTION As Integer = 0
    Private Const WH_KEYBOARD_LL As Integer = 13&

    Public Shared Event KeyPressed(ByVal sender As Object, ByVal e As KeyboardEventArgs)
    Public Shared Event KeyReleased(ByVal sender As Object, ByVal e As KeyboardEventArgs)

    Private Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hHook As Integer) As Integer
    Private Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As KeyboardHookDelegate, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As VirtualKeys) As Integer
    Private Declare Function CallNextHookEx Lib "user32" (ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="keys"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("User32")> _
    Private Shared Function GetKeyboardState(ByVal keys() As Byte) As Integer

    End Function

    ''' <summary>
    ''' Translates the specified virtual-key code and keyboard state to the corresponding character or characters. The function translates the code using the input language and physical keyboard layout identified by the keyboard layout handle.
    ''' To specify a handle to the keyboard layout to use to translate the specified code, use the ToAsciiEx function.
    ''' </summary>
    ''' <param name="uVirtKey">The virtual-key code to be translated. See Virtual-Key Codes.</param>
    ''' <param name="uScanCode">The hardware scan code of the key to be translated. The high-order bit of this value is set if the key is up (not pressed). </param>
    ''' <param name="lpKeyState">A pointer to a 256-byte array that contains the current keyboard state. Each element (byte) in the array contains the state of one key. If the high-order bit of a byte is set, the key is down (pressed).
    ''' The low bit, if set, indicates that the key is toggled on. In this function, only the toggle bit of the CAPS LOCK key is relevant. The toggle state of the NUM LOCK and SCROLL LOCK keys is ignored.</param>
    ''' <param name="lpChar">The buffer that receives the translated character or characters. </param>
    ''' <param name="uFlags">This parameter must be 1 if a menu is active, or 0 otherwise. </param>
    ''' <returns>
    ''' 0  The specified virtual key has no translation for the current state of the keyboard.
    ''' 1  One character was copied to the buffer.
    ''' 2  Two characters were copied to the buffer. This usually happens when a dead-key character (accent or diacritic) stored in the keyboard layout cannot be composed with the specified virtual key to form a single character.</returns>
    ''' <remarks>The parameters supplied to the ToAscii function might not be sufficient to translate the virtual-key code, because a previous dead key is stored in the keyboard layout.
    ''' Typically, ToAscii performs the translation based on the virtual-key code. In some cases, however, bit 15 of the uScanCode parameter may be used to distinguish between a key press and a key release. The scan code is used for translating ALT+ number key combinations.
    ''' Although NUM LOCK is a toggle key that affects keyboard behavior, ToAscii ignores the toggle setting (the low bit) of lpKeyState (VK_NUMLOCK) because the uVirtKey parameter alone is sufficient to distinguish the cursor movement keys (VK_HOME, VK_INSERT, and so on) from the numeric keys (VK_DECIMAL, VK_NUMPAD0 - VK_NUMPAD9).</remarks>
    <DllImport("User32")> _
    Private Shared Function ToAscii(ByVal uVirtKey As VirtualKeys, ByVal uScanCode As Integer, ByVal lpKeyState As Byte(), ByVal lpChar As System.Text.StringBuilder, ByVal uFlags As Integer) As Integer

    End Function


    Private Shared m_Keys As Byte() = Nothing
    <MarshalAs(UnmanagedType.FunctionPtr)> Private Shared callback As KeyboardHookDelegate
    Private Shared KeyboardHandle As Integer

    Private Shared Function GetKeyboardState() As Byte()
        If (m_Keys Is Nothing) Then ReDim m_Keys(255)
        GetKeyboardState(m_Keys)
        Return m_Keys
    End Function

    
    <Flags> _
    Public Enum KBDLLFlags As Integer
        LLKHF_EXTENDED = &H1
        LLKHF_INJECTED = &H10
        LLKHF_ALTDOWN = &H20
        LLKHF_UP = &H80
    End Enum

    ' Virtual Keys
    Public Enum VirtualKeys As Integer
        'VK_TAB = &H9
        'VK_CONTROL = &H11
        'VK_ESCAPE = &H1B
        'VK_DELETE = &H2E
        VK_LBUTTON = &H1       ' Left mouse button
        VK_RBUTTON = &H2       ' Right mouse button
        VK_CANCEL = &H3       ' Control-break processing
        VK_MBUTTON = &H4       ' Middle mouse button (three-button mouse)
        VK_XBUTTON1 = &H5       ' X1 mouse button
        VK_XBUTTON2 = &H6       ' X2 mouse button
        VK_BACK = &H8       ' BACKSPACE key
        VK_TAB = &H9       ' TAB key
        VK_CLEAR = &HC       ' CLEAR key
        VK_RETURN = &HD       ' ENTER key
        VK_SHIFT = &H10      ' SHIFT key
        VK_CONTROL = &H11      ' CTRL key
        VK_MENU = &H12      ' ALT key
        VK_PAUSE = &H13      ' PAUSE key
        VK_CAPITAL = &H14      ' CAPS LOCK key
        VK_KANA = &H15      ' IME Kana mode
        VK_JUNJA = &H17      ' IME Junja mode
        VK_FINAL = &H18      ' IME final mode
        VK_HANJA = &H19      ' IME Hanja mode
        VK_ESCAPE = &H1B      ' ESC key
        VK_CONVERT = &H1C      ' IME convert
        VK_NONCONVERT = &H1D      ' IME nonconvert
        VK_ACCEPT = &H1E      ' IME accept
        VK_MODECHANGE = &H1F      ' IME mode change request
        VK_SPACE = &H20      ' SPACEBAR
        VK_PRIOR = &H21      ' PAGE UP key
        VK_NEXT = &H22      ' PAGE DOWN key
        VK_END = &H23      ' END key
        VK_HOME = &H24      ' HOME key
        VK_LEFT = &H25      ' LEFT ARROW key
        VK_UP = &H26      ' UP ARROW key
        VK_RIGHT = &H27      ' RIGHT ARROW key
        VK_DOWN = &H28      ' DOWN ARROW key
        VK_SELECT = &H29      ' SELECT key
        VK_PRINT = &H2A      ' PRINT key
        VK_EXECUTE = &H2B      ' EXECUTE key
        VK_SNAPSHOT = &H2C      ' PRINT SCREEN key
        VK_INSERT = &H2D      ' INS key
        VK_DELETE = &H2E      ' DEL key
        VK_HELP = &H2F      ' HELP key
        VK_LWIN = &H5B      ' Left Windows key (Natural keyboard)
        VK_RWIN = &H5C      ' Right Windows key (Natural keyboard)
        VK_APPS = &H5D      ' Applications key (Natural keyboard)
        VK_SLEEP = &H5F      ' Computer Sleep key
        VK_NUMPAD0 = &H60      ' Numeric keypad 0 key
        VK_NUMPAD1 = &H61      ' Numeric keypad 1 key
        VK_NUMPAD2 = &H62      ' Numeric keypad 2 key
        VK_NUMPAD3 = &H63      ' Numeric keypad 3 key
        VK_NUMPAD4 = &H64      ' Numeric keypad 4 key
        VK_NUMPAD5 = &H65      ' Numeric keypad 5 key
        VK_NUMPAD6 = &H66      ' Numeric keypad 6 key
        VK_NUMPAD7 = &H67      ' Numeric keypad 7 key
        VK_NUMPAD8 = &H68      ' Numeric keypad 8 key
        VK_NUMPAD9 = &H69      ' Numeric keypad 9 key
        VK_MULTIPLY = &H6A      ' Multiply key
        VK_ADD = &H6B      ' Add key
        VK_SEPARATOR = &H6C      ' Separator key
        VK_SUBTRACT = &H6D      ' Subtract key
        VK_DECIMAL = &H6E      ' Decimal key
        VK_DIVIDE = &H6F      ' Divide key
        VK_F1 = &H70      ' F1 key
        VK_F2 = &H71      ' F2 key
        VK_F3 = &H72      ' F3 key
        VK_F4 = &H73      ' F4 key
        VK_F5 = &H74      ' F5 key
        VK_F6 = &H75      ' F6 key
        VK_F7 = &H76      ' F7 key
        VK_F8 = &H77      ' F8 key
        VK_F9 = &H78      ' F9 key
        VK_F10 = &H79      ' F10 key
        VK_F11 = &H7A      ' F11 key
        VK_F12 = &H7B      ' F12 key
        VK_F13 = &H7C      ' F13 key
        VK_F14 = &H7D      ' F14 key
        VK_F15 = &H7E      ' F15 key
        VK_F16 = &H7F      ' F16 key
        VK_F17 = &H80      ' F17 key
        VK_F18 = &H81      ' F18 key
        VK_F19 = &H82      ' F19 key
        VK_F20 = &H83      ' F20 key
        VK_F21 = &H84      ' F21 key
        VK_F22 = &H85      ' F22 key
        VK_F23 = &H86      ' F23 key
        VK_F24 = &H87      ' F24 key
        VK_NUMLOCK = &H90      ' NUM LOCK key
        VK_SCROLL = &H91      ' SCROLL LOCK key
        VK_LSHIFT = &HA0      ' Left SHIFT key
        VK_RSHIFT = &HA1      ' Right SHIFT key
        VK_LCONTROL = &HA2      ' Left CONTROL key
        VK_RCONTROL = &HA3      ' Right CONTROL key
        VK_LMENU = &HA4      ' Left MENU key
        VK_RMENU = &HA5      ' Right MENU key
        VK_BROWSER_BACK = &HA6      ' Browser Back key
        VK_BROWSER_FORWARD = &HA7      ' Browser Forward key
        VK_BROWSER_REFRESH = &HA8      ' Browser Refresh key
        VK_BROWSER_STOP = &HA9      ' Browser Stop key
        VK_BROWSER_SEARCH = &HAA      ' Browser Search key
        VK_BROWSER_FAVORITES = &HAB      ' Browser Favorites key
        VK_BROWSER_HOME = &HAC      ' Browser Start and Home key
        VK_VOLUME_MUTE = &HAD      ' Volume Mute key
        VK_VOLUME_DOWN = &HAE      ' Volume Down key
        VK_VOLUME_UP = &HAF      ' Volume Up key
        VK_MEDIA_NEXT_TRACK = &HB0      ' Next Track key
        VK_MEDIA_PREV_TRACK = &HB1      ' Previous Track key
        VK_MEDIA_STOP = &HB2      ' Stop Media key
        VK_MEDIA_PLAY_PAUSE = &HB3      ' Play/Pause Media key
        VK_LAUNCH_MAIL = &HB4      ' Start Mail key
        VK_LAUNCH_MEDIA_SELECT = &HB5      ' Select Media key
        VK_LAUNCH_APP1 = &HB6      ' Start Application 1 key
        VK_LAUNCH_APP2 = &HB7      ' Start Application 2 key
        VK_OEM_1 = &HBA      ' Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ';:' key
        VK_OEM_PLUS = &HBB      ' For any country/region, the '+' key
        VK_OEM_COMMA = &HBC      ' For any country/region, the ',' key
        VK_OEM_MINUS = &HBD      ' For any country/region, the '-' key
        VK_OEM_PERIOD = &HBE      ' For any country/region, the '.' key
        VK_OEM_2 = &HBF      ' Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '/?' key
        VK_OEM_3 = &HC0      ' Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '~' key
        VK_OEM_4 = &HDB      ' Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '[{' key
        VK_OEM_5 = &HDC      ' Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '\|' key
        VK_OEM_6 = &HDD      ' Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ']}' key
        VK_OEM_7 = &HDE      ' Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the 'single-quote/double-quote' key
        VK_OEM_8 = &HDF      ' Used for miscellaneous characters; it can vary by keyboard.
        VK_OEM_102 = &HE2      ' Either the angle bracket key or the backslash key on the RT 102-key keyboard
        VK_PROCESSKEY = &HE5      ' IME PROCESS key
        VK_PACKET = &HE7      ' Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
        VK_ATTN = &HF6      ' Attn key
        VK_CRSEL = &HF7      ' CrSel key
        VK_EXSEL = &HF8      ' ExSel key
        VK_EREOF = &HF9      ' Erase EOF key
        VK_PLAY = &HFA      ' Play key
        VK_ZOOM = &HFB      ' Zoom key
        VK_NONAME = &HFC      ' Reserved
        VK_PA1 = &HFD      ' PA1 key
        VK_OEM_CLEAR = &HFE      ' Clear key
    End Enum

    Private Structure KBDLLHOOKSTRUCT
        Public vkCode As VirtualKeys
        Public scanCode As Integer
        Public flags As KBDLLFlags
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure


    

    ' Implement this function to block as many key combinations as you'd like
    Private Shared Function handleHooked(ByRef Hookstruct As KBDLLHOOKSTRUCT) As Boolean
        'Debug.WriteLine("Hookstruct.vkCode: " & Hookstruct.vkCode)
        'Debug.WriteLine(Hookstruct.vkCode = VK_ESCAPE)
        'Debug.WriteLine(Hookstruct.vkCode = VK_TAB)

        'If (Hookstruct.vkCode = VK_ESCAPE) And _
        '  CBool(GetAsyncKeyState(VK_CONTROL) _
        '  And &H8000) Then

        '    Call HookedState("Ctrl + Esc blocked")
        '    Return True
        'End If

        'If (Hookstruct.vkCode = VK_TAB) And CBool(Hookstruct.flags And LLKHF_ALTDOWN) Then
        '    Call HookedState("Alt + Tab blockd")
        '    Return True
        'End If

        'If (Hookstruct.vkCode = VK_ESCAPE) And _
        '  CBool(Hookstruct.flags And LLKHF_ALTDOWN) Then

        '    Call HookedState("Alt + Escape blocked")
        '    Return True
        'End If

        Return False
    End Function

    Private Shared Sub HookedState(ByVal Text As String)
        Debug.WriteLine(Text)
    End Sub

    Public Shared Function GetKeyName(ByVal code As Integer, ByVal scanCode As Integer, ByVal flags As Integer, ByVal modifiers As Integer) As String
        Dim name As String = [Enum].GetName(GetType(VirtualKeys), code)
        If (name = "") Then
            'If (code >= 65 AndAlso code <= 65 + 26) Then
            name = Chr(code)
            'ElseIf (code >= Asc("0") AndAlso code <= 65 + 26) Then
            '   name = Chr(code)
            'Else
            '   name = "0x" & Right("00" & Hex(code), 2)
            'End If
        End If
        Return name
    End Function

    Public Shared Function GetKeyChar(ByVal code As VirtualKeys, ByVal scanCode As Integer, ByVal flags As KBDLLFlags, ByVal modifiers As Integer) As Char
        If (code = VirtualKeys.VK_RETURN) Then Return Chr(13)
        'If (code = VirtualKeys.VK_RETURN) Then Return vbCr

        Dim buffer As New System.Text.StringBuilder
        buffer.Capacity = 32
        Dim ret As Integer = ToAscii(code, scanCode, GetKeyboardState, buffer, 0)
        If (ret > 0) Then
            Return CType(buffer.ToString(0, ret), Char)
        Else
            Return " "c
        End If
    End Function

    Private Shared Function KeyboardCallback(ByVal Code As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer

        Try
            If (Code >= HC_ACTION) Then
                Dim e As New KeyboardEventArgs(lParam.vkCode, lParam.scanCode, lParam.flags, lParam.dwExtraInfo, GetKeyChar(lParam.vkCode, lParam.scanCode, lParam.flags, lParam.dwExtraInfo))
                DIALTPLib.Log.LogKey(e)
                If (e.IsKeyDown) Then
                    RaiseEvent KeyPressed(Nothing, e)
                Else
                    RaiseEvent KeyReleased(Nothing, e)
                End If

                'DIALTP.Log.Append("Calling IsHooked")
                If (handleHooked(lParam)) Then
                    Return 1
                Else
                    Return CallNextHookEx(KeyboardHandle, Code, wParam, lParam)
                End If
            Else
                Return CallNextHookEx(KeyboardHandle, Code, wParam, lParam)
            End If
        Catch ex As Exception
            Debug.Print(ex.Message)
            Return CallNextHookEx(KeyboardHandle, Code, wParam, lParam)
        End Try


    End Function


    Private Delegate Function KeyboardHookDelegate(ByVal Code As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer


    'Public Shared Sub HookKeyboard(ByVal handle As Integer)
    '    If IsHooked() Then Return
    '    callback = New KeyboardHookDelegate(AddressOf KeyboardCallback)
    '    KeyboardHandle = SetWindowsHookEx(WH_KEYBOARD_LL, callback, Marshal.GetHINSTANCE(GetType(Keyboard).Assembly.GetModules()(0)).ToInt32, 0)
    '    If (IsHooked()) Then
    '        DIALTPLib.Log.LogMessage("Keyboard hooked")
    '    Else
    '        DIALTPLib.Log.LogMessage("Keyboard hook failed: " & Err.LastDllError)
    '    End If
    'End Sub

    Public Shared Sub HookKeyboard(ByVal hInstance As Integer)
        If IsHooked Then Return
        callback = New KeyboardHookDelegate(AddressOf KeyboardCallback)
        KeyboardHandle = SetWindowsHookEx(WH_KEYBOARD_LL, callback, hInstance, 0)
        If (IsHooked()) Then
            DIALTPLib.Log.LogMessage("Keyboard hooked: " & KeyboardHandle)
        Else
            DIALTPLib.Log.LogMessage("Keyboard hook failed: " & Err.LastDllError)
        End If
    End Sub

    

    Public Shared Function IsHooked() As Boolean
        Return (KeyboardHandle <> 0)
    End Function

    Public Shared Sub UnhookKeyboard()
        If (IsHooked()) Then
            UnhookWindowsHookEx(KeyboardHandle)
        End If
        KeyboardHandle = 0
    End Sub



End Class

