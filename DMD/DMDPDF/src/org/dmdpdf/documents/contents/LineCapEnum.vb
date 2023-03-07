'/*
'  Copyright 2006-2010 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Shape to be used at the ends of stroked open subpaths
    '  [PDF:1.6:4.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public Enum LineCapEnum

        '/**
        '  <summary>Truncated line cap.</summary>
        '*/
        Butt = 0
        '/**
        '  <summary>Rounded line cap.</summary>
        '*/
        Round = 1
        '/**
        '  <summary>Squared-off line cap.</summary>
        '*/
        Square = 2
    End Enum

    Module LineCapEnumExtension 'Static 

        <Extension>
        Public Function ToGdi(ByVal value As LineCapEnum) As LineCap  'this
            Select Case (value)
                Case LineCapEnum.Butt : Return LineCap.Flat
                Case LineCapEnum.Round : Return LineCap.Round
                Case LineCapEnum.Square : Return LineCap.Square
                Case Else : Throw New NotSupportedException(value.ToString & " convertion not supported.")
            End Select
        End Function

        <Extension>
        Public Function ToDashCap(ByVal lineCap As LineCap) As DashCap
            Select Case (lineCap)
                Case LineCap.Round,
                     LineCap.RoundAnchor
                    Return DashCap.Round
                Case LineCap.Triangle
                    Return DashCap.Triangle
                Case Else
                    Return DashCap.Flat
            End Select
        End Function

    End Module

End Namespace
