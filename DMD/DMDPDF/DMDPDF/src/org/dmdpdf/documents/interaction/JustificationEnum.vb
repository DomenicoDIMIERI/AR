'/*
'  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.interaction
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.interaction

    '/**
    '  <summary>Text justification [PDF:1.6:8.4.5,8.6.2].</summary>
    '*/
    Public Enum JustificationEnum As Integer

        '/**
        '  <summary>Left.</summary>
        '*/
        Left
        '/**
        '  <summary>Center.</summary>
        '*/
        Center
        '/**
        '  <summary>Right.</summary>
        '*/
        Right
    End Enum


    <Extension>
    Module JustificationEnumExtension 'Static 

        Private ReadOnly codes As BiDictionary(Of JustificationEnum, PdfInteger)

        Sub New()

            codes = New BiDictionary(Of JustificationEnum, PdfInteger)
            codes(JustificationEnum.Left) = PdfInteger.Get(0)
            codes(JustificationEnum.Center) = PdfInteger.Get(1)
            codes(JustificationEnum.Right) = PdfInteger.Get(2)
        End Sub

        '/**
        '  <summary>Gets the justification corresponding to the given value.</summary>
        '*/

        Public Function [Get](ByVal value As PdfInteger) As JustificationEnum
            If (value Is Nothing) Then Return JustificationEnum.Left
            Dim justification As JustificationEnum? = codes.GetKey(value)
            If (Not justification.HasValue) Then Throw New NotSupportedException("Justification unknown: " & value.ToString)
            Return justification.Value
        End Function

        '/**
        '  <summary>Gets the code corresponding to the given value.</summary>
        '*/
        <Extension()>
        Public Function GetCode(ByVal value As JustificationEnum) As PdfInteger  'this
            Return codes(value)
        End Function

        <Extension()>
        Public Function ToXAlignment(ByVal value As JustificationEnum) As XAlignmentEnum  'this 
            Select Case (value)
                Case JustificationEnum.Left : Return XAlignmentEnum.Left
                Case JustificationEnum.Center : Return XAlignmentEnum.Center
                Case JustificationEnum.Right : Return XAlignmentEnum.Right
                Case Else : Throw New NotSupportedException()
            End Select
        End Function

    End Module

End Namespace
