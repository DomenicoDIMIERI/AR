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

Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Device Cyan-Magenta-Yellow-Key color value [PDF:1.6:4.5.3].</summary>
    '  <remarks>The 'Key' component is renamed 'Black' to avoid semantic
    '  ambiguities.</remarks>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class DeviceCMYKColor
        Inherits DeviceColor

#Region "Static"
#Region "fields"

        Public Shared ReadOnly Black As DeviceCMYKColor = New DeviceCMYKColor(0, 0, 0, 1)
        Public Shared ReadOnly White As DeviceCMYKColor = New DeviceCMYKColor(0, 0, 0, 0)

        Public Shared ReadOnly [Default] As DeviceCMYKColor = Black

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets the color corresponding to the specified components.</summary>
        '  <param name="components">Color components to convert.</param>
        '*/
        Public Shared Shadows Function [Get](ByVal components As PdfArray) As DeviceCMYKColor
            If (components IsNot Nothing) Then
                Return New DeviceCMYKColor(components)
            Else
                Return [Default]
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(
                        ByVal c As Double,
                        ByVal m As Double,
                        ByVal y As Double,
                        ByVal k As Double
                    )
            Me.New(
                New List(Of PdfDirectObject)(
                        New PdfDirectObject() {
                            PdfReal.Get(NormalizeComponent(c)),
                            PdfReal.Get(NormalizeComponent(m)),
                            PdfReal.Get(NormalizeComponent(y)),
                            PdfReal.Get(NormalizeComponent(k))
                            }
                    )
            )
        End Sub

        Friend Sub New(ByVal components As IList(Of PdfDirectObject))
            MyBase.New(DeviceCMYKColorSpace.Default, New PdfArray(components))
        End Sub

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary>Gets/Sets the cyan component.</summary>
        '*/
        Public Property C As Double
            Get
                Return GetComponentValue(0)
            End Get
            Set(ByVal value As Double)
                SetComponentValue(0, value)
            End Set
        End Property

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        '/**
        '  <summary>Gets/Sets the black (key) component.</summary>
        '*/
        Public Property K As Double
            Get
                Return GetComponentValue(3)
            End Get
            Set(ByVal value As Double)
                SetComponentValue(3, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the magenta component.</summary>
        '*/
        Public Property M As Double
            Get
                Return GetComponentValue(1)
            End Get
            Set(ByVal value As Double)
                SetComponentValue(1, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the yellow component.</summary>
        '*/
        Public Property Y As Double
            Get
                Return GetComponentValue(2)
            End Get
            Set(ByVal value As Double)
                SetComponentValue(2, value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace