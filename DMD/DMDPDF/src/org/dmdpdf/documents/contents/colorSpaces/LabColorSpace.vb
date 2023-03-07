'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>CIE-based ABC double-transformation-stage color space, where A, B and C represent the
    '  L*, a* and b* components of a CIE 1976 L*a*b* space [PDF:1.6:4.5.4].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class LabColorSpace
        Inherits CIEBasedColorSpace

#Region "dynamic"
#Region "constructors"
        'TODO:IMPL new element constructor!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides ReadOnly Property ComponentCount As Integer
            Get
                Return 3
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultColor As Color
            Get
                Dim ranges As IList(Of Interval(Of Double)) = Me.Ranges
                Return New LabColor(ranges(0).Low, ranges(1).Low, ranges(2).Low)
            End Get
        End Property

        Public Overrides Function GetColor(ByVal components As IList(Of PdfDirectObject), ByVal context As IContentContext) As Color
            Return New LabColor(components)
        End Function

        Public Overrides Function GetPaint(ByVal color As Color) As Drawing.Brush
            ' FIXME: temporary hack
            Return New Drawing.SolidBrush(Drawing.Color.Black)
        End Function

        '/**
        '  <summary>Gets the (inclusive) ranges of the color components.</summary>
        '  <remarks>Component values falling outside the specified range are adjusted
        '  to the nearest valid value.</remarks>
        '*/
        '//TODO:generalize to all the color spaces!
        Public ReadOnly Property Ranges As IList(Of Interval(Of Double))
            Get
                Dim _ranges As IList(Of Interval(Of Double)) = New List(Of Interval(Of Double))()
                '{
                ' 1. L* component.
                _ranges.Add(New Interval(Of Double)(0D, 100D))

                Dim rangesObject As PdfArray = CType(Dictionary(PdfName.Range), PdfArray)
                If (rangesObject Is Nothing) Then
                    ' 2. a* component.
                    _ranges.Add(New Interval(Of Double)(-100D, 100D))
                    ' 3. b* component.
                    _ranges.Add(New Interval(Of Double)(-100D, 100D))
                Else
                    ' 2/3. a*/b* components.
                    Dim length As Integer = rangesObject.Count
                    For index As Integer = 0 To length - 1 Step 2
                        _ranges.Add(New Interval(Of Double)(CType(rangesObject(index), IPdfNumber).RawValue,
                                                                                   CType(rangesObject(index + 1), IPdfNumber).RawValue
                                                                                   )
                                                                                   )
                    Next
                End If
                '}
                Return _ranges
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace