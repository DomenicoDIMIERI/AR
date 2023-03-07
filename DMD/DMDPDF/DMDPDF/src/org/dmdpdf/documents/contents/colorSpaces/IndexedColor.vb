'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Indexed color value [PDF:1.6:4.5.5].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class IndexedColor
        Inherits Color

#Region "Static"
#Region "fields"

        Public Shared ReadOnly [Default] As IndexedColor = New IndexedColor(0)

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the color corresponding to the specified components.</summary>
        '  <param name="components">Color components to convert.</param>
        '*/
        Public Shared Function [Get](ByVal components As PdfArray) As IndexedColor
            If (components IsNot Nothing) Then
                Return New IndexedColor(components)
            Else
                Return [Default]
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal index As Integer)
            Me.New(New List(Of PdfDirectObject)(New PdfDirectObject() {PdfInteger.Get(index)}))
        End Sub

        Friend Sub New(ByVal components As IList(Of PdfDirectObject))
            MyBase.New(Nothing, New PdfArray(components))
            '//TODO:consider color space reference!
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides ReadOnly Property Components As IList(Of PdfDirectObject)
            Get
                Return CType(BaseDataObject, PdfArray)
            End Get
        End Property

        '/**
        '  <summary> Gets the color index.</summary>
        '*/
        Public Property Index As Integer
            Get
                Return CType(CType(BaseDataObject, PdfArray)(0), PdfInteger).IntValue
            End Get
            Set(ByVal value As Integer)
                CType(BaseDataObject, PdfArray)(0) = PdfInteger.Get(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace