'/*
'  Copyright 2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Set the color to use for nonstroking operations in device gray color space'
    '  operation [PDF:1.6:4.5.7].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class SetDeviceGrayFillColor
        Inherits SetFillColor

#Region "Static"
#Region "fields"

        Public Shared Shadows ReadOnly OperatorKeyword As String = "g"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal value As DeviceGrayColor)
            MyBase.New(OperatorKeyword, value)
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            state.FillColorSpace = DeviceGrayColorSpace.Default
            MyBase.Scan(state)
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace