'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace DMD.org.dmdpdf.util.metadata

    '/**
    '  <summary>Version utility.</summary>
    '*/
    Public Class VersionUtils 'static 

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function CompareTo(ByVal version1 As IVersion, ByVal version2 As IVersion) As Integer
            Dim comparison As Integer = 0
            Dim version1Numbers As IList(Of Integer) = version1.Numbers
            Dim version2Numbers As IList(Of Integer) = version2.Numbers
            Dim length As Integer = System.Math.Min(version1Numbers.Count, version2Numbers.Count)
            For index As Integer = 0 To length - 1
                comparison = version1Numbers(index) - version2Numbers(index)
                If (comparison <> 0) Then Exit For
            Next
            If (comparison = 0) Then
                comparison = version1Numbers.Count - version2Numbers.Count
            End If
            Return System.Math.Sign(comparison)
        End Function

        Public Shared Shadows Function ToString(ByVal version As IVersion) As String
            Dim versionStringBuilder As New StringBuilder()
            'foreach(int number in version.Numbers)
            For Each number As Integer In version.Numbers
                If (versionStringBuilder.Length > 0) Then versionStringBuilder.Append("."c)
                versionStringBuilder.Append(number)
            Next
            Return versionStringBuilder.ToString()
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
