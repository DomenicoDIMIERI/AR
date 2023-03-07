'==============================================================================
' PC Connectivity API 3.2
'
' Filename    : PCCSTypeDefinitions.vb
' Description : PC Connectivity Solution Type Definitions for all APIs
' Version     : 3.2
'
' Copyright (c) 2007 Nokia Corporation.
' This software, including but not limited to documentation and any related 
' computer programs ("Software"), is protected by intellectual property rights 
' of Nokia Corporation and/or its licensors. All rights are reserved. By using 
' the Software you agree to the terms and conditions hereunder. If you do not 
' agree you must cease using the software immediately.
' Reproducing, disclosing, modifying, translating, or distributing any or all 
' of the Software requires the prior written consent of Nokia Corporation. 
' Nokia Corporation retains the right to make changes to the Software at any 
' time without notice.
'
' A copyright license is hereby granted to use of the Software to make, publish, 
' distribute, sub-license and/or sell new Software utilizing this Software. 
' The Software may not constitute the primary value of any new software utilizing 
' this software. No other license to any other intellectual property rights of 
' Nokia or a third party is granted. 
' 
' THIS SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS 
' OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF NON-INFRINGEMENT, 
' MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL
' NOKIA CORPORATION BE LIABLE FOR ANY DIRECT, INDIRECT, SPECIAL, INCIDENTAL, 
' OR CONSEQUENTIAL LOSS OR DAMAGES, INCLUDING BUT NOT LIMITED TO, LOST PROFITS 
' OR REVENUE, LOSS OF USE, COST OF SUBSTITUTE PROGRAM, OR LOSS OF DATA OR EQUIPMENT 
' ARISING OUT OF THE USE OR INABILITY TO USE THE MATERIAL, EVEN IF 
' NOKIA CORPORATION HAS BEEN ADVISED OF THE LIKELIHOOD OF SUCH DAMAGES OCCURRING.
' ==============================================================================
'
Imports System.Runtime.InteropServices

Partial Public Class Nokia

    Partial Public Class APIS

        'Public NotInheritable Class PCCSTypeDefinitions
        '    Private Sub New()
        '    End Sub

        ' Values used in API notification registeration methods
        Public Const API_REGISTER As Short = &H10
        Public Const API_UNREGISTER As Short = &H20

        ' Media types used in APIs 
        Public Const API_MEDIA_ALL As Short = &H1
        Public Const API_MEDIA_IRDA As Short = &H2
        Public Const API_MEDIA_SERIAL As Short = &H4
        Public Const API_MEDIA_BLUETOOTH As Short = &H8
        Public Const API_MEDIA_USB As Short = &H10

        ' Type definition for API_DATE_TIME
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)> _
        Public Structure API_DATE_TIME
            Dim iSize As Integer    ' Size of the structure. Must be sizeof(API_DATE_TIME).
            Dim wYear As UInt16
            Dim bMonth As Byte
            Dim bDay As Byte
            Dim bHour As Byte
            Dim bMinute As Byte
            Dim bSecond As Byte
            Dim lTimeZoneBias As Int32  ' Time zone bias in minutes (+120 for GMT+0200)
            Dim lBias As Int32          ' Daylight bias
        End Structure

    End Class

End Class