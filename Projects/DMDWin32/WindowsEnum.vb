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
Imports DMD.Native
Imports DMD.Native.Win32

Namespace Utils

    ''' <summary>
    ''' A utility class built ontop of the native windows API. This is used to 
    ''' enumerate child windows of a parent. The class defines a delegate for the 
    ''' the logic as to whether a window should or should not be included during
    ''' the enumeration.
    ''' </summary>
    Friend NotInheritable Class WindowsEnum

        ''' <summary>
        ''' The delegate used for processing the windows enumeration results.
        ''' </summary>
        ''' <param name="hWnd">The window being enumerated.</param>
        ''' <param name="include">A reference to a bool, which may be set to true/false in 
        ''' order to determine whether it should be included in the enumeration output.</param>
        Public Delegate Sub WindowFilterHandler(ByVal hWnd As IntPtr, ByRef include As Boolean)

        ''' <summary>
        ''' A list used to store the windows pointers during enumeration.
        ''' </summary>
        Private winEnumList As List(Of IntPtr)

        ''' <summary>
        ''' The delegate allocated to the instance for processing the enumerated windows.
        ''' </summary>
        Private filterHandler As WindowFilterHandler

        ''' <summary>
        ''' The constructor which takes a filter delegate for filtering enumeration results.
        ''' </summary>
        ''' <param name="filterHandler">A delegate which may filter the results.</param>
        Public Sub New(ByVal filterHandler As WindowFilterHandler)
            Me.New()
            Me.filterHandler = filterHandler
        End Sub

        ''' <summary>
        ''' A constructor used when there is no requirement to filter the enumeration results. 
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Enumerates the child windows of a parent and returns a list of pointers. If a filter
        ''' delegate is specified Me is used to determine whether the windows are included in 
        ''' the resultant list.
        ''' </summary>
        ''' <param name="parent">The parent window.</param>
        ''' <returns>A filtered list of child windows.</returns>
        Public Function Enumerate(ByVal parent As IntPtr) As List(Of IntPtr)
            Me.winEnumList = New List(Of IntPtr)()
            Win32.EnumChildWindows(parent, AddressOf OnWindowEnum, IntPtr.Zero)
            Return Me.winEnumList
        End Function

        ''' <summary>
        ''' A delegate used by the native API to process the enumerated windows from the 
        ''' Enumerate method call.
        ''' </summary>
        ''' <param name="hWnd">The window being enumerated</param>
        ''' <param name="lParam">The lParam passed by the windows API.</param>
        ''' <returns></returns>
        Private Function OnWindowEnum(ByVal hWnd As IntPtr, ByVal lParam As Integer) As Integer
            Dim include As Boolean = True
            If (filterHandler IsNot null) Then
                filterHandler(hWnd, include)
            End If
            If (include) Then
                Me.winEnumList.Add(hWnd)
            End If
            Return 1
        End Function

    End Class

End Namespace

