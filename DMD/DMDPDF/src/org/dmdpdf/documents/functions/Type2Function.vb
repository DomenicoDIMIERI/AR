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

Namespace DMD.org.dmdpdf.documents.functions

    '/**
    '  <summary>Exponential interpolation of one input value and <code>n</code> output values
    '  [PDF:1.6:3.9.2].</summary>
    '  <remarks>Each input value <code>x</code> will return <code>n</code> values, given by <code>
    '  y[j] = C0[j] + x^N × (C1[j] − C0[j])</code>, for <code>0 ≤ j < n</code>, where <code>C0</code>
    '  and <code>C1</code> are the {@link #getBoundOutputValues() function results} when, respectively,
    '  <code>x = 0</code> and <code>x = 1</code>, and <code>N</code> is the {@link #getExponent()
    '  interpolation exponent}.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class Type2Function
        Inherits [Function]


#Region "dynamic"
#Region "constructors"
        'TODO:implement function creation!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Calculate(ByVal inputs As Double()) As Double()
            ' FIXME: Auto-generated method stub
            Return Nothing
        End Function

        '/**
        '  <summary>Gets the output value pairs <code>(C0,C1)</code> For lower (<code>0.0</code>)
        '  And higher (<code>1.0</code>) input values.</summary>
        '*/
        Public ReadOnly Property BoundOutputValues As IList(Of Double())
            Get
                Dim outputBounds As IList(Of Double())
                '{
                Dim lowOutputBoundsObject As PdfArray = CType(Dictionary(PdfName.C0), PdfArray)
                Dim highOutputBoundsObject As PdfArray = CType(Dictionary(PdfName.C1), PdfArray)
                If (lowOutputBoundsObject Is Nothing) Then
                    outputBounds = New List(Of Double())
                    outputBounds.Add(New Double() {0, 1})
                Else
                    outputBounds = New List(Of Double())
                    Dim lowOutputBoundsObjectIterator As IEnumerator(Of PdfDirectObject) = lowOutputBoundsObject.GetEnumerator()
                    Dim highOutputBoundsObjectIterator As IEnumerator(Of PdfDirectObject) = highOutputBoundsObject.GetEnumerator()
                    While (lowOutputBoundsObjectIterator.MoveNext() AndAlso highOutputBoundsObjectIterator.MoveNext())
                        outputBounds.Add(
                                        New Double() {
                                                CType(lowOutputBoundsObjectIterator.Current, IPdfNumber).RawValue,
                                                CType(highOutputBoundsObjectIterator.Current, IPdfNumber).RawValue
                                            }
                                    )
                    End While
                End If
                '}
                Return outputBounds
            End Get
        End Property

        '/**
        '  <summary> Gets the interpolation exponent.</summary>
        '*/
        Public ReadOnly Property Exponent As Double
            Get
                Return CType(Dictionary(PdfName.N), IPdfNumber).RawValue
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace