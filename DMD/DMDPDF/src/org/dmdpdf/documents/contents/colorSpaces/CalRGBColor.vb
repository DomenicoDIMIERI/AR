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
    '  <summary>CIE-based red-green-blue color value [PDF:1.6:4.5.4].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class CalRGBColor
        Inherits LeveledColor

#Region "Static"
#Region "fields"

        Public Shared ReadOnly Black As CalRGBColor = New CalRGBColor(0, 0, 0)
        Public Shared ReadOnly White As CalRGBColor = New CalRGBColor(1, 1, 1)

        Public Shared ReadOnly [Default] As CalRGBColor = Black

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal r As Double, ByVal g As Double, ByVal b As Double)
            Me.New(
                    New List(Of PdfDirectObject)(
                            New PdfDirectObject() {
                                PdfReal.Get(NormalizeComponent(r)),
                                PdfReal.Get(NormalizeComponent(g)),
                                PdfReal.Get(NormalizeComponent(b))
                                }
                        )
                )
        End Sub

        Friend Sub New(ByVal components As IList(Of PdfDirectObject))
            MyBase.New(Nothing, New PdfArray(components))
            '//TODO:colorspace?
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the blue component.</summary>
        '*/
        Public Property B As Double
            Get
                Return GetComponentValue(2)
            End Get
            Set(ByVal value As Double)
                SetComponentValue(2, value)
            End Set
        End Property

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        '/**
        '  <summary>Gets/Sets the green component.</summary>
        '*/
        Public Property G As Double
            Get
                Return GetComponentValue(1)
            End Get
            Set(ByVal value As Double)
                SetComponentValue(1, value)
            End Set
        End Property

        '/**
        '  <summary>Gets the red component.</summary>
        '*/
        Public Property R As Double
            Get
                Return GetComponentValue(0)
            End Get
            Set(ByVal value As Double)
                SetComponentValue(0, value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace