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

Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Paint that consists of a repeating graphical figure or a smoothly varying color gradient
    '  instead of a simple color [PDF:1.6:4.6].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public MustInherit Class Pattern
        Inherits Color

#Region "static"
#Region "fields"

        'TODO: verify!
        Public Shared ReadOnly [Default] As Pattern = New TilingPattern(Nothing)

        Private Const PatternType1 As Integer = 1
        Private Const PatternType2 As Integer = 2

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Wraps the specified base Object into a pattern Object.</summary>
        '  <param name = "baseObject" > Base Object Of a pattern Object.</param>
        '  <returns> Pattern Object corresponding To the base Object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Pattern
            If (baseObject Is Nothing) Then Return Nothing

            Dim dataObject As PdfDataObject = baseObject.Resolve()
            Dim dictionary As PdfDictionary = GetDictionary(dataObject)
            Dim patternType As Integer = CType(dictionary(PdfName.PatternType), PdfInteger).RawValue
            Select Case (patternType)
                Case PatternType1 : Return New TilingPattern(baseObject)
                Case PatternType2 : Return New ShadingPattern(baseObject)
                Case Else : Throw New NotSupportedException("Pattern type " & patternType.ToString & " unknown.")
            End Select
        End Function

#End Region

#Region "private"

        '    /**
        '  <summary> Gets a pattern's dictionary.</summary>
        '  <param name = "patternDataObject" > Pattern data Object.</param>
        '*/
        Private Shared Function GetDictionary(ByVal patternDataObject As PdfDataObject) As PdfDictionary
            If (TypeOf (patternDataObject) Is PdfDictionary) Then
                Return CType(patternDataObject, PdfDictionary)
            Else ' MUST be PdfStream.
                Return CType(patternDataObject, PdfStream).Header
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"
        'TODO verify (colorspace Is available Or may be implicit?)

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

        'TODO verify (colorspace Is available Or may be implicit?)
        Protected Sub New(ByVal colorSpace As PatternColorSpace, ByVal baseObject As PdfDirectObject)
            MyBase.New(colorSpace, baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides ReadOnly Property Components As IList(Of PdfDirectObject)
            Get
                Return New List(Of PdfDirectObject)() 'TODO:verify (see SetFillColor/SetStrokeColor -- name!)!
            End Get
        End Property

        '/**
        '  <summary> Gets the pattern matrix, a transformation matrix that maps the pattern's
        '  internal coordinate system To the Default coordinate system Of the pattern's
        '  parent content stream (the content stream In which the pattern Is defined As a resource).</summary>
        '  <remarks> The concatenation Of the pattern matrix With that Of the parent content stream establishes
        '  the pattern coordinate space, within which all graphics objects In the pattern are interpreted.</remarks>
        '*/
        Public ReadOnly Property Matrix As Double()
            Get
                '/*
                '  NOTE: Pattern-space-to-user-space matrix Is identity [1 0 0 1 0 0] by default.
                '*/
                Dim _matrix As PdfArray = CType(Dictionary(PdfName.Matrix), PdfArray)
                If (_matrix Is Nothing) Then
                    '        Return New Double() {
                    '  1, // a.
                    '  0, // b.
                    '  0, // c.
                    '  1, // d.
                    '  0, // e.
                    '  0 // f.
                    '}
                    Return New Double() {1, 0, 0, 1, 0, 0}
                Else
                    '        Return New Double[]
                    '{
                    '  ((IPdfNumber)matrix[0]).RawValue, // a.
                    '  ((IPdfNumber)matrix[1]).RawValue, // b.
                    '  ((IPdfNumber)matrix[2]).RawValue, // c.
                    '  ((IPdfNumber)matrix[3]).RawValue, // d.
                    '  ((IPdfNumber)matrix[4]).RawValue, // e.
                    '  ((IPdfNumber)matrix[5]).RawValue // f.
                    '};
                    Return New Double() {
                          CType(_matrix(0), IPdfNumber).RawValue,
                          CType(_matrix(1), IPdfNumber).RawValue,
                          CType(_matrix(2), IPdfNumber).RawValue,
                          CType(_matrix(3), IPdfNumber).RawValue,
                          CType(_matrix(4), IPdfNumber).RawValue,
                          CType(_matrix(5), IPdfNumber).RawValue
                        }
                End If
            End Get
        End Property

#End Region

#Region "Protected"

        '    /**
        '  <summary> Gets this pattern's dictionary.</summary>
        '*/
        Protected ReadOnly Property Dictionary As PdfDictionary
            Get
                Return GetDictionary(BaseDataObject)
            End Get
        End Property

#End Region
#End Region
#End Region
    End Class

End Namespace