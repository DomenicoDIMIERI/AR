'/*=============================================================================
'*
'*	(C) Copyright 2007, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
'*
'*   http://www.TheCodeKing.co.uk
'*  
'*	All rights reserved.
'*	The code and information is provided "as-is" without waranty of any kind,
'*	either expresed or implied.
'*
'*-----------------------------------------------------------------------------
'*	History:
'*		11/02/2007	Michael Carlisle				Version 1.0
'*       08/09/2007  Michael Carlisle                Version 1.1
'*=============================================================================
'*/
Imports System
Imports System.Runtime.InteropServices

Namespace Native

    '''' <summary>
    '''' The native Win32 APIs used by the library.
    '''' </summary>
    Public NotInheritable Class Win32

        Public Const NULL As Object = Nothing

        ''' <summary>
        ''' WM_COPYDATA constant.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WM_COPYDATA As Integer = &H4A

        ''' <summary>
        ''' The WM_CHILD window style constant.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WS_CHILD As UInteger = &H40000000

        ''' <summary>
        ''' The struct used to marshal data between applications using the windows messaging API.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure COPYDATASTRUCT
            Public dwData As IntPtr
            Public cbData As Integer
            Public lpData As IntPtr
        End Structure


        ''' <summary>
        ''' Specifies how to send the message. This parameter can be one or more of the following values.
        ''' </summary>
        ''' <remarks></remarks>
        <Flags> _
        Public Enum SendMessageTimeoutFlags As UInteger
            ''' <summary>
            ''' The calling thread is not prevented from processing other requests while waiting for the function to return.
            ''' </summary>
            SMTO_NORMAL = &H0

            ''' <summary>
            ''' Prevents the calling thread from processing any other
            ''' requests until the function returns.
            ''' </summary>
            SMTO_BLOCK = &H1

            ''' <summary>
            ''' Returns without waiting for the time-out period to elapse 
            ''' if the receiving thread appears to not respond or "hangs."
            ''' </summary>
            SMTO_ABORTIFHUNG = &H2

            ''' <summary>
            ''' Microsoft Windows 2000/Windows XP: Do not enforce the time-out 
            ''' period as long as the receiving thread is processing messages.
            ''' </summary>
            SMTO_NOTIMEOUTIFNOTHUNG = &H8
        End Enum

        ''' <summary>
        ''' Returns a pointer to the Desktop window.
        ''' </summary>
        ''' <returns>Pointer to the desktop window.</returns>
        <DllImport("user32.dll", EntryPoint:="GetDesktopWindow")> _
        Public Shared Function GetDesktopWindow() As IntPtr

        End Function

        ''' <summary>
        ''' Sends a native windows message to a specified window.
        ''' </summary>
        ''' <param name="hwnd">The window to which the message should be sent.</param>
        ''' <param name="wMsg">The native windows message type.</param>
        ''' <param name="wParam">A pointer to the wPAram data.</param>
        ''' <param name="lParam">The struct containing lParam data</param>
        ''' <param name="flags">The timeout flags.</param>
        ''' <param name="timeout">The timeout value in miliseconds.</param>
        ''' <param name="result">The result.</param>
        ''' <returns></returns>
        <DllImport("user32", CharSet:=CharSet.Auto)> _
        Public Shared Function SendMessageTimeout( _
                    ByVal hwnd As IntPtr, _
                    ByVal wMsg As UInteger, ByVal wParam As Integer, _
                    ByRef lParam As COPYDATASTRUCT, _
                    ByVal flags As SendMessageTimeoutFlags,
                    ByVal timeout As UInteger, _
                    ByRef result As IntPtr) As Integer

        End Function

        ''' <summary>
        ''' A delegate used by the EnumChildWindows windows API to enumerate windows.
        ''' </summary>
        ''' <param name="hwnd">A pointer to a window that was found.</param>
        ''' <param name="lParam">The lParam passed by the EnumChildWindows API.</param>
        ''' <returns></returns>
        Public Delegate Function EnumWindowsProc(ByVal hwnd As IntPtr, ByVal lParam As Integer) As Integer

        ''' <summary>
        ''' The API used to enumerate child windows of a given parent window.
        ''' </summary>
        ''' <param name="hwndParent">The parent window.</param>
        ''' <param name="lpEnumFunc">The delegate called when a window is located.</param>
        ''' <param name="lParam">The lParam passed to the deleage.</param>
        ''' <returns></returns>
        <DllImport("user32.dll")> _
        Public Shared Function EnumChildWindows(ByVal hwndParent As IntPtr, ByVal lpEnumFunc As EnumWindowsProc, ByVal lParam As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        End Function


        ''' <summary>
        ''' Gets a named window property for a given window address. 
        ''' This returns zero if not found.
        ''' </summary>
        ''' <param name="hwnd">The window containing the property.</param>
        ''' <param name="lpString">The property name to lookup.</param>
        ''' <returns>The property data, or 0 if not found.</returns>
        <DllImport("user32", CharSet:=CharSet.Auto)> _
        Public Shared Function GetProp(ByVal hwnd As IntPtr, ByVal lpString As String) As Integer

        End Function

        ''' <summary>
        ''' Sets a window proerty value.
        ''' </summary>
        ''' <param name="hwnd">The window on which to attach the property.</param>
        ''' <param name="lpString">The property name.</param>
        ''' <param name="hData">The property value.</param>
        ''' <returns>A value indicating whether the function succeeded.</returns>
        <DllImport("user32", CharSet:=CharSet.Auto)> _
        Public Shared Function SetProp(ByVal hwnd As IntPtr, ByVal lpString As String, ByVal hData As Integer) As Integer

        End Function

        ''' <summary>
        ''' Removes a named property from a given window.
        ''' </summary>
        ''' <param name="hwnd">The window whose property should be removed.</param>
        ''' <param name="lpString">The property name.</param>
        ''' <returns>A value indicating whether the function succeeded.</returns>
        <DllImport("user32", CharSet:=CharSet.Auto)> _
        Public Shared Function RemoveProp(ByVal hwnd As IntPtr, ByVal lpString As String) As Integer

        End Function

    End Class

End Namespace
