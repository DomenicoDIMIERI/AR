'Filename    : PhoneListBox.vb
'Part of     : Phone Navigator VB.NET example
'Description : Implementation of phone navigator's phone list box
'Version     : 3.2
'
'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.
'
'Copyright (c) 2005-2007 Nokia Corporation.
'
'This material, including but not limited to documentation and any related
'computer programs, is protected by intellectual property rights of Nokia
'Corporation and/or its licensors.
'All rights are reserved. Reproducing, modifying, translating, or
'distributing any or all of this material requires the prior written consent
'of Nokia Corporation. Nokia Corporation retains the right to make changes
'to this material at any time without notice. A copyright license is hereby
'granted to download and print a copy of this material for personal use only.
'No other license to any other intellectual property rights is granted. The
'material is provided "as is" without warranty of any kind, either express or
'implied, including without limitation, any warranty of non-infringement,
'merchantability and fitness for a particular purpose. In no event shall
'Nokia Corporation be liable for any direct, indirect, special, incidental,
'or consequential loss or damages, including but not limited to, lost profits
'or revenue,loss of use, cost of substitute program, or loss of data or
'equipment arising out of the use or inability to use the material, even if
'Nokia Corporation has been advised of the likelihood of such damages occurring.

Imports System
Imports System.Runtime.InteropServices
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.IO

'===================================================================
' PhoneListBox
Public Class PhoneListBox1
    Inherits System.Windows.Forms.ListBox
    Private bDisposed As Boolean = False



    '===================================================================
    ' Constructor
    '
    '===================================================================
    Public Sub New()
        
        
    End Sub
 

     

    '===================================================================
    ' ListAllPhones
    '
    ' Adds all connected phones to list box
    ' 
    '===================================================================
    Public Sub Refill()
        ResetContent()
        For Each dev As DMD.Nokia.NokiaDevice In DMD.Nokia.Devices
            ' Querying count of connected devices
            Me.Items.Add(dev)
        Next
    End Sub

    '===================================================================
    ' ResetContent
    '
    ' Clear contents of list box
    ' 
    '===================================================================
    Public Sub ResetContent()
        Me.Items.Clear()
    End Sub

    
    Public Function GetCurrentDevice() As DMD.Nokia.NokiaDevice
        If TypeOf (Me.SelectedItem) Is DMD.Nokia.NokiaDevice Then Return Me.SelectedItem
        Return Nothing
    End Function
End Class
