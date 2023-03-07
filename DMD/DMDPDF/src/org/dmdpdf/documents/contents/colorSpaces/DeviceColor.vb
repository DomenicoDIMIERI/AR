'/*
'  Copyright 2006-2010 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
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

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Device color value [PDF:1.6:4.5.3].</summary>
    '*/
    Public MustInherit Class DeviceColor
        Inherits LeveledColor

#Region "static"
#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets the color corresponding to the specified components.</summary>
        '  <param name="components">Color components to convert.</param>
        ' */
        Public Shared Function [Get](ByVal components As PdfArray) As DeviceColor
            If (components Is Nothing) Then Return Nothing
            Select Case (components.Count)
                Case 1 : Return DeviceGrayColor.Get(components)
                Case 3 : Return DeviceRGBColor.Get(components)
                Case 4 : Return DeviceCMYKColor.Get(components)
                Case Else : Return Nothing
            End Select
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal colorSpace As DeviceColorSpace, ByVal baseObject As PdfDirectObject)
            MyBase.New(colorSpace, baseObject)
        End Sub

#End Region
#End Region

    End Class

End Namespace