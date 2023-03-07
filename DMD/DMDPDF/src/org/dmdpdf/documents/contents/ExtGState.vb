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

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Graphics state parameters [PDF:1.6:4.3.4].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ExtGState
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "Static"
#Region "fields"

        Friend Shared ReadOnly DefaultBlendMode As IList(Of BlendModeEnum) = New BlendModeEnum() {}

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Wraps the specified base Object into a graphics state parameter dictionary Object.
        '  </summary>
        '  <param name = "baseObject" > Base Object Of a graphics state parameter dictionary Object.</param>
        '  <returns> Graphics state parameter dictionary Object corresponding To the base Object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As ExtGState
            If (baseObject IsNot Nothing) Then
                Return New ExtGState(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets whether the current soft mask And alpha constant are To be interpreted As
        '  shape values instead Of opacity values.</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property AlphaShape As Boolean
            Get
                Return CBool(PdfSimpleObject(Of Object).GetValue(BaseDataObject(PdfName.AIS), False))
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.AIS) = PdfBoolean.Get(value)
            End Set
        End Property

        Public Sub ApplyTo(ByVal state As ContentScanner.GraphicsState)
            For Each parameterName As PdfName In BaseDataObject.Keys
                If (parameterName.Equals(PdfName.Font)) Then
                    If (Me.Font Is Nothing) Then
                        Debug.Print("oops")
                    End If
                    state.Font = Me.Font
                    state.FontSize = Me.FontSize.Value
                ElseIf (parameterName.Equals(PdfName.LC)) Then
                    state.LineCap = Me.LineCap.Value
                ElseIf (parameterName.Equals(PdfName.D)) Then
                    state.LineDash = Me.LineDash
                ElseIf (parameterName.Equals(PdfName.LJ)) Then
                    state.LineJoin = Me.LineJoin.Value
                ElseIf (parameterName.Equals(PdfName.LW)) Then
                    state.LineWidth = Me.LineWidth.Value
                ElseIf (parameterName.Equals(PdfName.ML)) Then
                    state.MiterLimit = Me.MiterLimit.Value
                ElseIf (parameterName.Equals(PdfName.BM)) Then
                    state.BlendMode = Me.BlendMode
                    'TODO:extend supported parameters!!!
                End If
            Next
        End Sub

        '/**
        '  <summary> Gets/Sets the blend mode To be used In the transparent imaging model [PDF:1.7:7.2.4].
        '  </summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property BlendMode As IList(Of BlendModeEnum)
            Get
                Dim blendModeObject As PdfDirectObject = BaseDataObject(PdfName.BM)
                If (blendModeObject Is Nothing) Then
                    Return DefaultBlendMode
                End If

                Dim _blendMode As IList(Of BlendModeEnum) = New List(Of BlendModeEnum)()
                If (TypeOf (blendModeObject) Is PdfName) Then
                    _blendMode.Add(BlendModeEnumExtension.Get(CType(blendModeObject, PdfName)).Value)
                Else ' MUST be an array.
                    For Each alternateBlendModeObject As PdfDirectObject In CType(blendModeObject, PdfArray)
                        _blendMode.Add(BlendModeEnumExtension.Get(CType(alternateBlendModeObject, PdfName)).Value)
                    Next
                End If
                Return _blendMode
            End Get
            Set(ByVal value As IList(Of BlendModeEnum))
                Dim blendModeObject As PdfDirectObject
                If (value Is Nothing OrElse value.Count = 0) Then
                    blendModeObject = Nothing
                ElseIf (value.Count = 1) Then
                    blendModeObject = value(0).GetName()
                Else
                    Dim blendModeArray As PdfArray = New PdfArray()
                    For Each blendMode As BlendModeEnum In value
                        blendModeArray.Add(blendMode.GetName())
                    Next
                    blendModeObject = blendModeArray
                End If
                BaseDataObject(PdfName.BM) = blendModeObject
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the nonstroking alpha constant, specifying the constant shape Or constant
        '  opacity value To be used For nonstroking operations In the transparent imaging model
        '  [PDF:1.7:7.2.6].</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property FillAlpha As Double?
            Get
                Return CType(PdfSimpleObject(Of PdfObject).GetValue(BaseDataObject(PdfName.ca_)), Double)
            End Get
            Set(ByVal value As Double?)
                BaseDataObject(PdfName.ca_) = PdfReal.Get(value)
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property Font As Font
            Get
                Dim fontObject As PdfArray = CType(BaseDataObject(PdfName.Font), PdfArray)
                If (fontObject IsNot Nothing) Then
                    Return Font.Wrap(fontObject(0))
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Font)
                Dim fontObject As PdfArray = CType(BaseDataObject(PdfName.Font), PdfArray)
                If (fontObject Is Nothing) Then
                    fontObject = New PdfArray(PdfObjectWrapper.GetBaseObject(value), PdfInteger.Default)
                Else
                    fontObject(0) = PdfObjectWrapper.GetBaseObject(value)
                End If
                BaseDataObject(PdfName.Font) = fontObject
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property FontSize As Double?
            Get
                Dim fontObject As PdfArray = CType(BaseDataObject(PdfName.Font), PdfArray)
                If (fontObject IsNot Nothing) Then
                    Return CType(fontObject(1), IPdfNumber).RawValue
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Double?)
                Dim fontObject As PdfArray = CType(Me.BaseDataObject(PdfName.Font), PdfArray)
                If (fontObject Is Nothing) Then
                    fontObject = New PdfArray(Nothing, PdfReal.Get(value))
                Else
                    fontObject(1) = PdfReal.Get(value)
                End If
                Me.BaseDataObject(PdfName.Font) = fontObject
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property LineCap As LineCapEnum?
            Get
                Dim lineCapObject As PdfInteger = CType(BaseDataObject(PdfName.LC), PdfInteger)
                If (lineCapObject IsNot Nothing) Then
                    Return CType(lineCapObject.RawValue, LineCapEnum)
                Else
                    Return Nothing '(LineCapEnum?)null;
                End If
            End Get
            Set(ByVal value As LineCapEnum?)
                If (value.HasValue) Then
                    Me.BaseDataObject(PdfName.LC) = PdfInteger.Get(value.Value)
                Else
                    Me.BaseDataObject(PdfName.LC) = Nothing
                End If
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property LineDash As LineDash
            Get
                Dim lineDashObject As PdfArray = CType(BaseDataObject(PdfName.D), PdfArray)
                If (lineDashObject Is Nothing) Then Return Nothing
                Dim dashArray As Double()
                Dim baseDashArray As PdfArray = CType(lineDashObject(0), PdfArray)
                dashArray = New Double(baseDashArray.Count - 1) {}
                Dim length As Integer = dashArray.Length
                For index As Integer = 0 To length - 1
                    index = 1
                    dashArray(index) = CType(baseDashArray(index), IPdfNumber).RawValue
                Next
                Dim dashPhase As Double = CType(lineDashObject(1), IPdfNumber).RawValue
                Return New LineDash(dashArray, dashPhase)
            End Get
            Set(ByVal value As LineDash)
                Dim lineDashObject As PdfArray = New PdfArray()
                Dim dashArrayObject As PdfArray = New PdfArray()
                For Each dashArrayItem As Double In value.DashArray
                    dashArrayObject.Add(PdfReal.Get(dashArrayItem))
                Next
                lineDashObject.Add(dashArrayObject)
                lineDashObject.Add(PdfReal.Get(value.DashPhase))
                BaseDataObject(PdfName.D) = lineDashObject
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property LineJoin As LineJoinEnum?
            Get
                Dim lineJoinObject As PdfInteger = CType(BaseDataObject(PdfName.LJ), PdfInteger)
                If (lineJoinObject IsNot Nothing) Then
                    Return CType(lineJoinObject.RawValue, LineJoinEnum)
                Else
                    Return Nothing '(LineJoinEnum?)null;
                End If
            End Get
            Set(ByVal value As LineJoinEnum?)
                If (value.HasValue) Then
                    BaseDataObject(PdfName.LJ) = PdfInteger.Get(value.Value)
                Else
                    BaseDataObject(PdfName.LJ) = Nothing
                End If
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property LineWidth As Double?
            Get
                Dim lineWidthObject As IPdfNumber = CType(BaseDataObject(PdfName.LW), IPdfNumber)
                If (lineWidthObject IsNot Nothing) Then
                    Return lineWidthObject.RawValue
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Double?)
                BaseDataObject(PdfName.LW) = PdfReal.Get(value)
            End Set
        End Property

        <PDF(VersionEnum.PDF13)>
        Public Property MiterLimit As Double?
            Get
                Dim miterLimitObject As IPdfNumber = CType(BaseDataObject(PdfName.ML), IPdfNumber)
                If (miterLimitObject IsNot Nothing) Then
                    Return miterLimitObject.RawValue
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Double?)
                BaseDataObject(PdfName.ML) = PdfReal.Get(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the stroking alpha constant, specifying the constant shape Or constant
        '  opacity value To be used For stroking operations In the transparent imaging model
        '  [PDF:1.7:7.2.6].</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property StrokeAlpha As Double?
            Get
                Return CType(PdfSimpleObject(Of PdfObject).GetValue(BaseDataObject(PdfName.CA)), Double?)
            End Get
            Set(ByVal value As Double?)
                BaseDataObject(PdfName.CA) = PdfReal.Get(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace