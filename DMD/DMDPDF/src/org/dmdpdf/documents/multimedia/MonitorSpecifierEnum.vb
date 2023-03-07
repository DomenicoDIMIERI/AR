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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.interaction
'Imports DMD.actions = org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Monitor specifier [PDF:1.7:9.1.6].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public Enum MonitorSpecifierEnum As Integer

        '/**
        '  <summary>The monitor containing the largest section of the document window.</summary>
        '*/
        LargestDocumentWindowSection
        '/**
        '  <summary>The monitor containing the smallest section of the document window.</summary>
        '*/
        SmallestDocumentWindowSection
        '/**
        '  <summary>Primary monitor, otherwise the monitor containing the largest section of the document
        '  window.</summary>
        '*/
        Primary
        '/**
        '  <summary>The monitor with the greatest color depth (in bits).</summary>
        '*/
        GreatestColorDepth
        '/**
        '  <summary>The monitor with the greatest area (in pixels squared).</summary>
        '*/
        GreatestArea
        '/**
        '  <summary>The monitor with the greatest height (in pixels).</summary>
        '*/
        GreatestHeight
        '/**
        '  <summary>The monitor with the greatest width (in pixels).</summary>
        '*/
        GreatestWidth
    End Enum

    Module MonitorSpecifierEnumExtension ' Static 

        Private ReadOnly codes As BiDictionary(Of MonitorSpecifierEnum, PdfInteger)

        Sub New()
            codes = New BiDictionary(Of MonitorSpecifierEnum, PdfInteger)()
            codes(MonitorSpecifierEnum.LargestDocumentWindowSection) = New PdfInteger(0)
            codes(MonitorSpecifierEnum.SmallestDocumentWindowSection) = New PdfInteger(1)
            codes(MonitorSpecifierEnum.Primary) = New PdfInteger(2)
            codes(MonitorSpecifierEnum.GreatestColorDepth) = New PdfInteger(3)
            codes(MonitorSpecifierEnum.GreatestArea) = New PdfInteger(4)
            codes(MonitorSpecifierEnum.GreatestHeight) = New PdfInteger(5)
            codes(MonitorSpecifierEnum.GreatestWidth) = New PdfInteger(6)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As MonitorSpecifierEnum?
            If (code Is Nothing) Then Return MonitorSpecifierEnum.LargestDocumentWindowSection
            Dim monitorSpecifier As MonitorSpecifierEnum? = codes.GetKey(code)
            If (Not monitorSpecifier.HasValue) Then Throw New NotSupportedException("Monitor specifier unknown: " & code.ToString())
            Return monitorSpecifier
        End Function

        <Extension>
        Public Function GetCode(ByVal monitorSpecifier As MonitorSpecifierEnum) As PdfInteger  'this 
            Return codes(monitorSpecifier)
        End Function

    End Module

End Namespace