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
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.functions

    '/**
    '  <summary>Sampled function using a sequence of sample values to provide an approximation for
    '  functions whose domains and ranges are bounded [PDF:1.6:3.9.1].</summary>
    '  <remarks>The samples are organized as an m-dimensional table in which each entry has n components.
    '  </remarks>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class Type0Function
        Inherits [Function]

#Region "types"

        Public Enum InterpolationOrderEnum

            '  /**
            '  Linear spline interpolation.
            '*/
            Linear = 1
            '/**
            '  Cubic spline interpolation.
            '*/
            Cubic = 3
        End Enum

#End Region

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

        Private Function _intervalsCallBack(ByVal intervals As IList(Of Interval(Of Integer))) As IList(Of Interval(Of Integer))
            For Each sampleCount As Integer In Me.SampleCounts
                intervals.Add(New Interval(Of Integer)(0, sampleCount - 1))
            Next
            Return intervals
        End Function

        '/**
        '  <summary>Gets the linear mapping of input values into the domain of the function's sample table.</summary>
        '*/
        Public ReadOnly Property DomainEncodes As IList(Of Interval(Of Integer))
            Get
                Return GetIntervals(Of Integer)(PdfName.Encode, AddressOf _intervalsCallBack)
            End Get
        End Property

        '/**
        '  <summary>Gets the order of interpolation between samples.</summary>
        '*/
        Public ReadOnly Property Order As InterpolationOrderEnum
            Get
                Dim interpolationOrderObject As PdfInteger = CType(Dictionary(PdfName.Order), PdfInteger)
                If (interpolationOrderObject Is Nothing) Then
                    Return InterpolationOrderEnum.Linear
                Else
                    Return CType(interpolationOrderObject.RawValue, InterpolationOrderEnum)
                End If
            End Get
        End Property

        '/**
        '  <summary>Gets the linear mapping of sample values into the ranges of the function's output values.</summary>
        '*/
        Public ReadOnly Property RangeDecodes As IList(Of Interval(Of Double))
            Get
                Return GetIntervals(Of Double)(PdfName.Decode, Nothing)
            End Get
        End Property

        '/**
        '  <summary>Gets the number of bits used to represent each sample.</summary>
        '*/
        Public ReadOnly Property SampleBitsCount As Integer
            Get
                Return CType(Dictionary(PdfName.BitsPerSample), PdfInteger).RawValue
            End Get
        End Property

        '/**
        '  <summary>Gets the number of samples in each input dimension of the sample table.</summary>
        '*/
        Public ReadOnly Property SampleCounts As IList(Of Integer)
            Get
                Dim _sampleCounts As List(Of Integer) = New List(Of Integer)
                '{
                Dim sampleCountsObject As PdfArray = CType(Dictionary(PdfName.Size), PdfArray)
                For Each sampleCountObject As PdfDirectObject In sampleCountsObject
                    _sampleCounts.Add(CType(sampleCountObject, PdfInteger).RawValue)
                Next
                '}
                Return _sampleCounts
            End Get

        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace