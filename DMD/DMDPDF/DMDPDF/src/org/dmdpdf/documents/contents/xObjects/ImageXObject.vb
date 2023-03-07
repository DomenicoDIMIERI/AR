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

Imports DMD.org.dmdpdf
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.xObjects

    '/**
    '  <summary>Image external object [PDF:1.6:4.8.4].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class ImageXObject
        Inherits XObject

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Shadows Function Wrap(ByVal baseObject As PdfDirectObject) As ImageXObject
            If (baseObject IsNot Nothing) Then
                Return New ImageXObject(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document, ByVal baseDataObject As PdfStream)
            MyBase.New(context, baseDataObject)
            '/*
            '  NOTE: It's caller responsability to adequately populate the stream
            '  header and body in order to instantiate a valid object; header entries like
            '  'Width', 'Height', 'ColorSpace', 'BitsPerComponent' MUST be defined
            '  appropriately.
            '*/
            baseDataObject.Header(PdfName.Subtype) = PdfName.Image
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the number of bits per color component.</summary>
        '*/
        Public ReadOnly Property BitsPerComponent As Integer
            Get
                Return CType(BaseDataObject.Header(PdfName.BitsPerComponent), PdfInteger).RawValue
            End Get
        End Property

        '/**
        '  <summary>Gets the color space in which samples are specified.</summary>
        '*/
        Public ReadOnly Property ColorSpace As String
            Get
                Return CType(BaseDataObject.Header(PdfName.ColorSpace), PdfName).RawValue
            End Get
        End Property

        Public Overrides Property Matrix As Matrix
            Get
                Dim size As SizeF = Me.Size
                '/*
                '  NOTE: Image-space-to-user-space matrix is [1/w 0 0 1/h 0 0],
                '  where w and h are the width and height of the image in samples [PDF:1.6:4.8.3].
                '*/
                Return New Matrix(
                                1.0F / size.Width, 0, 0,
                                1.0F / size.Height, 0, 0
                                )
            End Get
            Set(ByVal value As Matrix)
                ' NOOP. 
            End Set
        End Property

        '/**
        '  <summary>Gets the size of the image (in samples).</summary>
        '*/
        Public Overrides Property Size As SizeF
            Get
                Dim header As PdfDictionary = BaseDataObject.Header
                Return New SizeF(
                                  CType(header(PdfName.Width), PdfInteger).RawValue,
                                  CType(header(PdfName.Height), PdfInteger).RawValue
                                  )
            End Get
            Set(ByVal value As SizeF)
                Throw New NotSupportedException()
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace