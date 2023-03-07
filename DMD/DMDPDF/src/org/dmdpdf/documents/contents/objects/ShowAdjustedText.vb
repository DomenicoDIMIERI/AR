'/*
'  Copyright 2009-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Redistribution and use, with or without modification, are permitted provided that such
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.IO

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Show one or more text strings, allowing individual glyph positioning'
    '  operation [PDF:1.6:5.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class ShowAdjustedText
        Inherits ShowText

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "TJ"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <param name = "value" >Each element can be either a byte array (encoded text) Or a number.
        '    If the element Is a Byte array (encoded text), this Operator shows the text glyphs.
        '    If it Is a number (glyph adjustment), the Operator adjusts the Next glyph position by that amount.</param>
        '*/
        Public Sub New(ByVal value As IList(Of Object))
            MyBase.New(OperatorKeyword)
            Me.Value = value
        End Sub


        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "Interface"
#Region "Public"


        Public Overrides Property Text As Byte()
            Get
                Dim textStream As MemoryStream = New MemoryStream()
                For Each element As PdfDirectObject In CType(Me._operands(0), PdfArray)
                    If (TypeOf (element) Is PdfString) Then
                        Dim elementValue As Byte() = CType(element, PdfString).RawValue
                        textStream.Write(elementValue, 0, elementValue.Length)
                    End If
                Next
                Return textStream.ToArray()
            End Get
            Set(ByVal value As Byte())
                Me.Value = New List(Of Object)({CType(value, Object)})
            End Set
        End Property

        Public Overrides Property Value As IList(Of Object)
            Get
                Dim _value As IList(Of Object) = New List(Of Object)
                For Each element As PdfDirectObject In CType(Me._operands(0), PdfArray)
                    'TODO:horrible workaround To the lack Of generic covariance...
                    If (TypeOf (element) Is IPdfNumber) Then
                        _value.Add(CType(element, IPdfNumber).RawValue)
                    ElseIf (TypeOf (element) Is PdfString) Then
                        _value.Add(CType(element, PdfString).RawValue)
                    Else
                        Throw New NotSupportedException("Element type " & element.GetType().Name & " not supported.")
                    End If
                Next
                Return _value
            End Get
            Set(ByVal value As IList(Of Object))
                Dim elements As PdfArray = New PdfArray()
                Me._operands(0) = elements
                Dim textItemExpected As Boolean = True
                For Each valueItem As Object In value
                    Dim element As PdfDirectObject
                    If (textItemExpected) Then
                        element = New PdfString(CType(valueItem, Byte()))
                    Else
                        element = PdfReal.Get(CType(valueItem, Double))
                    End If
                    elements.Add(element)
                    textItemExpected = Not textItemExpected
                Next
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace