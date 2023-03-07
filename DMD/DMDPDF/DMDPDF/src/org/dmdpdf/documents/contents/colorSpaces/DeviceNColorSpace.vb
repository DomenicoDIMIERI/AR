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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Special color space that can contain an arbitrary number of color components [PDF:1.6:4.5.5].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class DeviceNColorSpace
        Inherits SpecialDeviceColorSpace

#Region "dynamic"
#Region "constructors"
        'TODO:IMPL new element constructor!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides ReadOnly Property ComponentCount As Integer
            Get
                Return CType(CType(BaseDataObject, PdfArray)(1), PdfArray).Count
            End Get
        End Property

        Public Overrides ReadOnly Property ComponentNames As IList(Of String)
            Get
                Dim _componentNames As IList(Of String) = New List(Of String)
                '{
                Dim namesObject As PdfArray = CType(CType(BaseDataObject, PdfArray)(1), PdfArray)
                For Each nameObject As PdfDirectObject In namesObject
                    _componentNames.Add(CStr(CType(nameObject, PdfName).Value))
                Next
                '}
                Return _componentNames
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultColor As Color
            Get
                Dim components As Double() = New Double(ComponentCount - 1) {}
                Dim length As Integer = components.Length
                For index As Integer = 0 To length - 1
                    components(index) = 1
                Next
                Return New DeviceNColor(components)
            End Get
        End Property

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            Return New DeviceNColor(components)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace