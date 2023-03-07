'Filename    : FileOperationListener.vb
'Part of     : Phone Navigator
'Description : Implementation of phone navigators file operation listener
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

Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    '===================================================================
    ' FileOperationListener
    Public Class FileOperation
        Public Type As Nokia.FileOperationTypes
        Public SourceName As String
        Public TargetName As String
        Public Percentage As Integer

        Public Sub New(ByVal type As FileOperationTypes, ByVal SourceName As String, Optional ByVal TargetName As String = vbNullString)
            Me.Type = type
            Me.SourceName = SourceName
            Me.TargetName = TargetName
            Me.Percentage = 0
        End Sub
         
    End Class

End Namespace