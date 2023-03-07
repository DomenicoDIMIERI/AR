'/*
'  Copyright 2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.contents.layers

    '/**
    '  <summary>List mode specifying which layers should be displayed to the user.</summary>
    '*/
    Public Enum ListModeEnum

        '/**
        '  <summary>All the layers are displayed.</summary>
        '*/
        AllPages
        '/**
        '  <summary>Only the layers referenced by one or more visible pages are displayed.</summary>
        '*/
        VisiblePages
    End Enum

    Module ListModeEnumExtension

        Private ReadOnly codes As BiDictionary(Of ListModeEnum, PdfName)

        Sub New()
            codes = New BiDictionary(Of ListModeEnum, PdfName)
            codes(ListModeEnum.AllPages) = PdfName.AllPages
            codes(ListModeEnum.VisiblePages) = PdfName.VisiblePages
        End Sub

        Public Function [Get](ByVal name As PdfName) As ListModeEnum
            If (name Is Nothing) Then Return ListModeEnum.AllPages
            Dim listMode As ListModeEnum? = codes.GetKey(name)
            If (Not listMode.HasValue) Then Throw New NotSupportedException("List mode unknown: " & name.ToString)
            Return listMode.Value
        End Function

        <Extension>
        Public Function GetName(ByVal listMode As ListModeEnum) As PdfName
            Return codes(listMode)
        End Function
    End Module

End Namespace