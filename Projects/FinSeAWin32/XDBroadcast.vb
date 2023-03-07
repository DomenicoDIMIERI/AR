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
Imports System.Collections.Generic
Imports System.Text
Imports DMD.Utils
Imports DMD.Native
Imports DMD.Native.Win32

Namespace Net.Messaging

    ''' <summary>
    ''' Class used to broadcast messages to other applications listening
    ''' on a particular channel.
    ''' </summary>
    Public Class XDBroadcast
        Private Sub New()
        End Sub

        'Public Shared Sub SendToChannel(ByVal channel As String, ByVal message As String)

        ''' <summary>
        ''' The API used to broadcast messages to a channel, and other applications that
        ''' may be listening.
        ''' </summary>
        ''' <param name="channel">The channel name to broadcast on.</param>
        ''' <param name="message">The string message data.</param>
        Public Shared Sub SendToChannel(ByVal channel As String, ByVal message As Object)
            ' create a DataGram instance
            Dim dataGram As DataGram = New DataGram(channel, message)
            ' Allocate the DataGram to a memory address contained in COPYDATASTRUCT
            Dim dataStruct As Win32.COPYDATASTRUCT = dataGram.ToStruct()
            '// Use a filter with the EnumWindows class to get a list of windows containing
            '// a property name that matches the destination channel. These are the listening
            '// applications.
            Dim filter As WindowEnumFilter = New WindowEnumFilter(XDListener.GetChannelKey(channel))
            Dim winEnum As WindowsEnum = New WindowsEnum(AddressOf filter.WindowFilterHandler)
            For Each hWnd As IntPtr In winEnum.Enumerate(Win32.GetDesktopWindow())
                Dim outPtr As IntPtr = IntPtr.Zero
                ' For each listening window, send the message data. Return if hang or unresponsive within 1 sec.
                Win32.SendMessageTimeout(hWnd, Win32.WM_COPYDATA, IntPtr.Zero.ToInt32, dataStruct, Win32.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, outPtr)
            Next
            ' free the memory
            dataGram.Dispose()
        End Sub
    End Class

End Namespace
