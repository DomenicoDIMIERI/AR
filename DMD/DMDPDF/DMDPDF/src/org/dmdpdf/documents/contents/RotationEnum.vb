'/*
'  Copyright 2010-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Rotation (clockwise) [PDF:1.6:3.6.2].</summary>
    '*/
    Public Enum RotationEnum
        '{
        '  /**
        '    Downward (0° clockwise).
        '  */
        Downward = 0
        '/**
        '  Leftward (90° clockwise).
        '*/
        Leftward = 90
        '/**
        '  Upward (180° clockwise).
        '*/
        Upward = 180
        '/**
        '  Rightward (270° clockwise).
        '*/
        Rightward = 270
    End Enum


    Module RotationEnumExtension 'Static 

        '/**
        '  <summary>Gets the direction corresponding to the given value.</summary>
        '*/
        Public Function [Get](ByVal value As PdfInteger) As RotationEnum
            If (value Is Nothing) Then Return RotationEnum.Downward
            Dim normalizedValue As Integer = CInt(Math.Round(value.RawValue / 90D) Mod 4) * 90
            If (normalizedValue < 0) Then
                normalizedValue += 360 * CInt(Math.Ceiling(-normalizedValue / 360D))
            End If
            Return CType(normalizedValue, RotationEnum)
        End Function

        <Extension>
        Public Function Transform(ByVal rotation As RotationEnum, ByVal size As SizeF) As SizeF  'this 
            If (CInt(rotation) Mod 180 = 0) Then
                Return New SizeF(size.Width, size.Height)
            Else
                Return New SizeF(size.Height, size.Width)
            End If
        End Function

    End Module

End Namespace
