'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Reflection

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Resources collection [PDF:1.6:3.7.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Resources
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements ICompositeDictionary(Of PdfName)

#Region "Static"
#Region "Interface"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Resources
            If (baseObject IsNot Nothing) Then
                Return New Resources(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property ColorSpaces As ColorSpaceResources
            Get
                Return New ColorSpaceResources(BaseDataObject.Get(Of PdfDictionary)(PdfName.ColorSpace))
            End Get
            Set(ByVal value As ColorSpaceResources)
                BaseDataObject(PdfName.ColorSpace) = value.BaseObject
            End Set
        End Property

        Public Property ExtGStates As ExtGStateResources
            Get
                Return New ExtGStateResources(BaseDataObject.Get(Of PdfDictionary)(PdfName.ExtGState))
            End Get
            Set(ByVal value As ExtGStateResources)
                BaseDataObject(PdfName.ExtGState) = value.BaseObject
            End Set
        End Property

        Public Property Fonts As FontResources
            Get
                Return New FontResources(BaseDataObject.Get(Of PdfDictionary)(PdfName.Font))
            End Get
            Set(ByVal value As FontResources)
                BaseDataObject(PdfName.Font) = value.BaseObject
            End Set
        End Property

        Public Property Patterns As PatternResources
            Get
                Return New PatternResources(BaseDataObject.Get(Of PdfDictionary)(PdfName.Pattern))
            End Get
            Set(ByVal value As PatternResources)
                BaseDataObject(PdfName.Pattern) = value.BaseObject
            End Set
        End Property

        <PDF(VersionEnum.PDF12)>
        Public Property PropertyLists As PropertyListResources
            Get
                Return New PropertyListResources(BaseDataObject.Get(Of PdfDictionary)(PdfName.Properties))
            End Get
            Set(ByVal value As PropertyListResources)
                CheckCompatibility("PropertyLists")
                BaseDataObject(PdfName.Properties) = value.BaseObject
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property Shadings As ShadingResources
            Get
                Return New ShadingResources(BaseDataObject.Get(Of PdfDictionary)(PdfName.Shading))
            End Get
            Set(ByVal value As ShadingResources)
                BaseDataObject(PdfName.Shading) = value.BaseObject
            End Set
        End Property

        Public Property XObjects As XObjectResources
            Get
                Return New XObjectResources(BaseDataObject.Get(Of PdfDictionary)(PdfName.XObject))
            End Get
            Set(ByVal value As XObjectResources)
                BaseDataObject(PdfName.XObject) = value.BaseObject
            End Set
        End Property

#Region "ICompositeDictionary"

        Public Function [Get](ByVal type As System.Type) As PdfObjectWrapper Implements ICompositeDictionary(Of PdfName).Get
            If (GetType(ColorSpace).IsAssignableFrom(type)) Then
                Return ColorSpaces
            ElseIf (GetType(ExtGState).IsAssignableFrom(type)) Then
                Return ExtGStates
            ElseIf (GetType(Font).IsAssignableFrom(type)) Then
                Return Fonts
            ElseIf (GetType(Pattern).IsAssignableFrom(type)) Then
                Return Patterns
            ElseIf (GetType(PropertyList).IsAssignableFrom(type)) Then
                Return PropertyLists
            ElseIf (GetType(Shading).IsAssignableFrom(type)) Then
                Return Shadings
            ElseIf (GetType(XObject).IsAssignableFrom(type)) Then
                Return XObjects
            Else
                Throw New ArgumentException(type.Name & " does NOT represent a valid resource class.")
            End If
        End Function

        Public Function [Get](ByVal type As Type, ByVal key As PdfName) As PdfObjectWrapper Implements ICompositeDictionary(Of PdfName).Get
            Return CType(type.GetMethod("get_Item", BindingFlags.Public Or BindingFlags.Instance).Invoke([Get](type), New Object() {key}), PdfObjectWrapper)
        End Function

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace
