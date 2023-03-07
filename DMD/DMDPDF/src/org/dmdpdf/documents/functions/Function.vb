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
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.functions

    '/**
    '  <summary>Function [PDF:1.6:3.9].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public MustInherit Class [Function]
        Inherits PdfObjectWrapper(Of PdfDataObject)

#Region "types"

        '/**
        '  <summary>Default intervals callback.</summary>
        '*/
        Protected Delegate Function DefaultIntervalsCallback(Of T As IComparable(Of T))(ByVal intervals As IList(Of Interval(Of T))) As IList(Of Interval(Of T))

#End Region

#Region "Static"
#Region "fields"

        Private Const FunctionType0 As Integer = 0
        Private Const FunctionType2 As Integer = 2
        Private Const FunctionType3 As Integer = 3
        Private Const FunctionType4 As Integer = 4

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Wraps a Function base Object into a Function Object.</summary>
        '  <param name = "baseObject" > Function() base Object.</param>
        '  <returns> Function object() associated To the base Object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As [Function]
            If (baseObject Is Nothing) Then Return Nothing
            Dim dataObject As PdfDataObject = baseObject.Resolve()
            Dim Dictionary As PdfDictionary = GetDictionary(dataObject)
            Dim functionType As Integer = CType(Dictionary(PdfName.FunctionType), PdfInteger).RawValue
            Select Case (functionType)
                Case FunctionType0 : Return New Type0Function(baseObject)
                Case FunctionType2 : Return New Type2Function(baseObject)
                Case FunctionType3 : Return New Type3Function(baseObject)
                Case FunctionType4 : Return New Type4Function(baseObject)
                Case Else : Throw New NotSupportedException("Function type " & functionType & " unknown.")
            End Select
        End Function

#End Region

#Region "private"

        '/**
        '  <summary>Gets a Function's dictionary.</summary>
        '  <param name = "functionDataObject" > Function() Data Object.</param>
        '*/
        Private Shared Function GetDictionary(ByVal functionDataObject As PdfDataObject) As PdfDictionary
            If (TypeOf (functionDataObject) Is PdfDictionary) Then
                Return CType(functionDataObject, PdfDictionary)
            Else ' MUST be PdfStream.
                Return CType(functionDataObject, PdfStream).Header
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal baseDataObject As PdfDataObject)
            MyBase.New(context, baseDataObject)
        End Sub


        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the result Of the calculation applied by this Function
        '  to the specified input values.</summary>
        '  <param name = "inputs" > Input values.</param>
        ' */
        Public MustOverride Function Calculate(ByVal inputs As Double()) As Double()

        '/**
        '  <summary>Gets the result Of the calculation applied by this Function
        '  to the specified input values.</summary>
        '  <param name = "inputs" > Input values.</param>
        ' */
        Public Function Calculate(ByVal inputs As IList(Of PdfDirectObject)) As IList(Of PdfDirectObject)
            Dim outputs As IList(Of PdfDirectObject) = New List(Of PdfDirectObject)()
            '{
            Dim inputValues As Double() = New Double(inputs.Count - 1) {}
            Dim length As Integer = inputValues.Length
            For index As Integer = 0 To length - 1
                inputValues(index) = CType(inputs(index), IPdfNumber).RawValue
            Next
            Dim outputValues As Double() = Calculate(inputValues)
            length = outputValues.Length
            For index As Integer = 0 To length - 1
                outputs.Add(PdfReal.Get(outputValues(index)))
            Next
            '}
            Return outputs
        End Function

        '/**
        '  <summary>Gets the (inclusive) domains Of the input values.</summary>
        '  <remarks>Input values outside the declared domains are clipped To the nearest boundary value.</remarks>
        '*/
        Public ReadOnly Property Domains As IList(Of Interval(Of Double))
            Get
                Return GetIntervals(Of Double)(PdfName.Domain, Nothing)
            End Get
        End Property

        '/**
        '  <summary> Gets the number Of input values (parameters) Of this Function.</summary>
        '*/
        Public ReadOnly Property InputCount As Integer
            Get
                Return CInt(CType(Dictionary(PdfName.Domain), PdfArray).Count / 2)
            End Get
        End Property

        '/**
        '  <summary> Gets the number Of output values (results) Of this Function.</summary>
        '*/
        Public ReadOnly Property OutputCount As Integer
            Get
                Dim rangesObject As PdfArray = CType(Dictionary(PdfName.Range), PdfArray)
                If (rangesObject Is Nothing) Then
                    Return 1
                Else
                    Return CInt(rangesObject.Count / 2)
                End If
            End Get
        End Property

        '/**
        '  <summary> Gets the (inclusive) ranges Of the output values.</summary>
        '  <remarks> Output values outside the declared ranges are clipped To the nearest boundary value;
        '  If this Then entry Is absent, no clipping Is done.</remarks>
        '  <returns> <code>Nothing</code> In Case Of unbounded ranges.</returns>
        '*/
        Public ReadOnly Property Ranges As IList(Of Interval(Of Double))
            Get
                Return GetIntervals(Of Double)(PdfName.Range, Nothing)
            End Get
        End Property

#End Region

#Region "protected"

        '/**
        '  <summary>Gets this Function's dictionary.</summary>
        '*/
        Protected ReadOnly Property Dictionary As PdfDictionary
            Get
                Return GetDictionary(BaseDataObject)
            End Get
        End Property

        '/**
        '  <summary>Gets the intervals corresponding To the specified key.</summary>
        '*/
        Protected Function GetIntervals(Of T As IComparable(Of T))(ByVal key As PdfName, ByVal defaultIntervalsCallback As DefaultIntervalsCallback(Of T)) As IList(Of Interval(Of T))
            Dim intervals As IList(Of Interval(Of T))
            '{
            Dim intervalsObject As PdfArray = CType(Dictionary(key), PdfArray)
            If (intervalsObject Is Nothing) Then
                If (defaultIntervalsCallback Is Nothing) Then
                    intervals = Nothing
                Else
                    intervals = defaultIntervalsCallback(New List(Of Interval(Of T))())
                End If
            Else
                intervals = New List(Of Interval(Of T))()
                Dim length As Integer = intervalsObject.Count
                For index As Integer = 0 To length - 1 Step 2
                    intervals.Add(
                                  New Interval(Of T)(
                                    CType(CType(intervalsObject(index), IPdfNumber).Value, T),
                                    CType(CType(intervalsObject(index + 1), IPdfNumber).Value, T)
                                    )
                                )
                Next
            End If
            '}
            Return intervals
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace