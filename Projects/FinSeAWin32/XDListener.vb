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
Imports System.Windows.Forms
Imports DMD.Native
Imports DMD.Native.Win32

Namespace Net.Messaging

    ''' <summary>
    ''' A class used to send and recieve cross AppDomain messages. Applications may
    ''' leverage this instance to register listeners on pseudo 'channels', and 
    ''' broadcast messages to other applications.
    ''' </summary>
    Public NotInheritable Class XDListener
        Inherits NativeWindow

        ''' <summary>
        ''' The delegate used for handling cross AppDomain communications.
        ''' </summary>
        ''' <param name="sender">The event sender.</param>
        ''' <param name="e">The event args containing the DataGram data.</param>
        Public Delegate Sub XDMessageHandler(ByVal sender As Object, ByVal e As XDMessageEventArgs)

        ''' <summary>
        ''' The event fired when messages are received.
        ''' </summary>
        Public Event MessageReceived As XDMessageHandler

        ''' <summary>
        ''' Default constructor. This creates a hidden native window used to 
        ''' listen for Windows Messages, and filter out the WM_COPYDATA commands.
        ''' </summary>
        Public Sub New()
            Dim p As CreateParams = New CreateParams()
            p.Width = 0
            p.Height = 0
            p.X = 0
            p.Y = 0
            p.Style = Win32.WS_CHILD
            p.Caption = Guid.NewGuid().ToString()
            p.Parent = Win32.GetDesktopWindow()
            MyBase.CreateHandle(p)
        End Sub

        ''' <summary>
        ''' Registers the instance to recieve messages from a named channel.
        ''' </summary>
        ''' <param name="channelName">The channel name to listen on.</param>
        Public Sub RegisterChannel(ByVal channelName As String)
            Win32.SetProp(Me.Handle, GetChannelKey(channelName), Me.Handle)
        End Sub

        ''' <summary>
        ''' Unregisters the channel name with the instance, so that messages from this 
        ''' channel will no longer be received.
        ''' </summary>
        ''' <param name="channelName">The channel name to stop listening for.</param>
        Public Sub UnRegisterChannel(ByVal channelName As String)
            Win32.RemoveProp(Me.Handle, GetChannelKey(channelName))
        End Sub

        ''' <summary>
        ''' The native window message filter used to catch our custom WM_COPYDATA
        ''' messages containing cross AppDomain messages. All other messages are ignored.
        ''' </summary>
        ''' <param name="msg">A representation of the native Windows Message.</param>
        Protected Overrides Sub WndProc(ByRef msg As Message)
            MyBase.WndProc(msg)
            If (msg.Msg = Win32.WM_COPYDATA) Then
                'If (MessageReceived Is NULL) Then Return
                Dim dataGram As DataGram = dataGram.FromPointer(msg.LParam)
                RaiseEvent MessageReceived(Me, New XDMessageEventArgs(dataGram))
            End If
        End Sub

        ''' <summary>
        ''' Gets a channel key string associated with the channel name. This is used as the 
        ''' property name attached to listening windows in order to identify them as
        ''' listeners. Using the key instead of user defined channel name avoids protential 
        ''' property name clashes. 
        ''' </summary>
        ''' <param name="channelName">The channel name for which a channel key is required.</param>
        ''' <returns>The string channel key.</returns>
        Friend Shared Function GetChannelKey(ByVal channelName As String) As String
            Return String.Format("DMD.IPC.{0}", channelName)
        End Function

    End Class


End Namespace
