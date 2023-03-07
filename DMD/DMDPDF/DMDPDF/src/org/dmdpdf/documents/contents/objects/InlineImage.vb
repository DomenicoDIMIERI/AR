'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Inline image object [PDF:1.6:4.8.6].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class InlineImage
        Inherits GraphicsObject

#Region "Static"
#Region "fields"

        Public Shared ReadOnly BeginOperatorKeyword As String = BeginInlineImage.OperatorKeyword
        Public Shared ReadOnly EndOperatorKeyword As String = EndInlineImage.OperatorKeyword

        Private Shared ReadOnly DataOperatorKeyword As String = "ID"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal header As InlineImageHeader, ByVal body As InlineImageBody)
            Me._objects.Add(header)
            Me._objects.Add(body)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the image body.</summary>
        '*/
        Public ReadOnly Property Body As Operation
            Get
                Return CType(Me._objects(1), Operation)
            End Get
        End Property

        ''' <summary>
        ''' Gets the image header.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Property Header As Operation
            Get
                Return CType(Me._objects(0), Operation)
            End Get
            Set(value As Operation)
                MyBase.Header = value
            End Set
        End Property



        '/**
        '  <summary>Gets the image size.</summary>
        '*/
        Public ReadOnly Property Size As Size
            Get
                Dim header As InlineImageHeader = CType(Me.Header, InlineImageHeader)
                Dim w As PdfName
                Dim h As PdfName
                If (header.ContainsKey(PdfName.W)) Then
                    w = PdfName.W
                Else
                    w = PdfName.Width
                End If
                If (header.ContainsKey(PdfName.H)) Then
                    h = PdfName.H
                Else
                    h = PdfName.Height
                End If
                Return New Size(
                            CInt(CType(header(w), IPdfNumber).Value),
                            CInt(CType(header(h), IPdfNumber).Value)
                            )
            End Get
        End Property

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)
            stream.Write(BeginOperatorKeyword) : stream.Write(vbLf)
            Header.WriteTo(stream, context)
            stream.Write(DataOperatorKeyword) : stream.Write(vbLf)
            Body.WriteTo(stream, context) : stream.Write(vbLf)
            stream.Write(EndOperatorKeyword)
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace