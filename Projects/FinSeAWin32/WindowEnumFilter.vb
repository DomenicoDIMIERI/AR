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
Imports DMD.Native
Imports DMD.Native.Win32

Namespace Net.Messaging

    ''' <summary>
    ''' A class used as a WindowFilterHandler for the WindowsEnum class. This 
    ''' filters the results of a windows enumeration based on whether the windows
    ''' contain a named property.
    ''' </summary>
    Friend NotInheritable Class WindowEnumFilter

        ''' <summary>
        ''' The property to search for when filtering the windows.
        ''' </summary>
        Private [property] As String

        ''' <summary>
        ''' The constructor which takes the property name used for filtering
        ''' results.
        ''' </summary>
        ''' <param name="property">The windows property name.</param>
        Public Sub New(ByVal [property] As String)
            Me.property = [property]
        End Sub

        ''' <summary>
        ''' The delegate used to filter windows during emuneration. Only windows 
        ''' that contain a named property are added to the enum.
        ''' </summary>
        ''' <param name="hWnd">The window being filtered.</param>
        ''' <param name="include">Indicates whether the window should be
        ''' inclused in the enumeration output.</param>
        Public Sub WindowFilterHandler(ByVal hWnd As IntPtr, ByRef include As Boolean)
            If (Win32.GetProp(hWnd, Me.property) = 0) Then
                include = False
            End If
        End Sub

    End Class

End Namespace
