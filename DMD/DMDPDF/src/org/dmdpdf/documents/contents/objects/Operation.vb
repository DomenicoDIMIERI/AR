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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Content stream instruction [PDF:1.6:3.7.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class Operation
        Inherits ContentObject

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets an operation.</summary>
        '  <param name="operator_">Operator.</param>
        '  <param name="operands">List of operands.</param>
        '*/
        Public Shared Function [Get](ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject)) As Operation
            If (operator_ Is Nothing) Then Return Nothing

            If (operator_.Equals(SaveGraphicsState.OperatorKeyword)) Then
                Return SaveGraphicsState.Value
            ElseIf (operator_.Equals(SetFont.OperatorKeyword)) Then
                Return New SetFont(operands)
            ElseIf (operator_.Equals(SetStrokeColor.OperatorKeyword) OrElse
                    operator_.Equals(SetStrokeColor.ExtendedOperatorKeyword)) Then
                Return New SetStrokeColor(operator_, operands)
            ElseIf (operator_.Equals(SetStrokeColorSpace.OperatorKeyword)) Then
                Return New SetStrokeColorSpace(operands)
            ElseIf (operator_.Equals(SetFillColor.OperatorKeyword) OrElse
                    operator_.Equals(SetFillColor.ExtendedOperatorKeyword)) Then
                Return New SetFillColor(operator_, operands)
            ElseIf (operator_.Equals(SetFillColorSpace.OperatorKeyword)) Then
                Return New SetFillColorSpace(operands)
            ElseIf (operator_.Equals(SetDeviceGrayStrokeColor.OperatorKeyword)) Then
                Return New SetDeviceGrayStrokeColor(operands)
            ElseIf (operator_.Equals(SetDeviceGrayFillColor.OperatorKeyword)) Then
                Return New SetDeviceGrayFillColor(operands)
            ElseIf (operator_.Equals(SetDeviceRGBStrokeColor.OperatorKeyword)) Then
                Return New SetDeviceRGBStrokeColor(operands)
            ElseIf (operator_.Equals(SetDeviceRGBFillColor.OperatorKeyword)) Then
                Return New SetDeviceRGBFillColor(operands)
            ElseIf (operator_.Equals(SetDeviceCMYKStrokeColor.OperatorKeyword)) Then
                Return New SetDeviceCMYKStrokeColor(operands)
            ElseIf (operator_.Equals(SetDeviceCMYKFillColor.OperatorKeyword)) Then
                Return New SetDeviceCMYKFillColor(operands)
            ElseIf (operator_.Equals(RestoreGraphicsState.OperatorKeyword)) Then
                Return RestoreGraphicsState.Value
            ElseIf (operator_.Equals(BeginSubpath.OperatorKeyword)) Then
                Return New BeginSubpath(operands)
            ElseIf (operator_.Equals(CloseSubpath.OperatorKeyword)) Then
                Return CloseSubpath.Value
            ElseIf (operator_.Equals(PaintPath.CloseStrokeOperatorKeyword)) Then
                Return PaintPath.CloseStroke
            ElseIf (operator_.Equals(PaintPath.FillOperatorKeyword) OrElse
                    operator_.Equals(PaintPath.FillObsoleteOperatorKeyword)) Then
                Return PaintPath.Fill
            ElseIf (operator_.Equals(PaintPath.FillEvenOddOperatorKeyword)) Then
                Return PaintPath.FillEvenOdd
            ElseIf (operator_.Equals(PaintPath.StrokeOperatorKeyword)) Then
                Return PaintPath.Stroke
            ElseIf (operator_.Equals(PaintPath.FillStrokeOperatorKeyword)) Then
                Return PaintPath.FillStroke
            ElseIf (operator_.Equals(PaintPath.FillStrokeEvenOddOperatorKeyword)) Then
                Return PaintPath.FillStrokeEvenOdd
            ElseIf (operator_.Equals(PaintPath.CloseFillStrokeOperatorKeyword)) Then
                Return PaintPath.CloseFillStroke
            ElseIf (operator_.Equals(PaintPath.CloseFillStrokeEvenOddOperatorKeyword)) Then
                Return PaintPath.CloseFillStrokeEvenOdd
            ElseIf (operator_.Equals(PaintPath.EndPathNoOpOperatorKeyword)) Then
                Return PaintPath.EndPathNoOp
            ElseIf (operator_.Equals(ModifyClipPath.NonZeroOperatorKeyword)) Then
                Return ModifyClipPath.NonZero
            ElseIf (operator_.Equals(ModifyClipPath.EvenOddOperatorKeyword)) Then
                Return ModifyClipPath.EvenOdd
            ElseIf (operator_.Equals(TranslateTextToNextLine.OperatorKeyword)) Then
                Return TranslateTextToNextLine.Value
            ElseIf (operator_.Equals(ShowSimpleText.OperatorKeyword)) Then
                Return New ShowSimpleText(operands)
            ElseIf (operator_.Equals(ShowTextToNextLine.SimpleOperatorKeyword) OrElse
                    operator_.Equals(ShowTextToNextLine.SpaceOperatorKeyword)) Then
                Return New ShowTextToNextLine(operator_, operands)
            ElseIf (operator_.Equals(ShowAdjustedText.OperatorKeyword)) Then
                Return New ShowAdjustedText(operands)
            ElseIf (operator_.Equals(TranslateTextRelative.SimpleOperatorKeyword) OrElse
                    operator_.Equals(TranslateTextRelative.LeadOperatorKeyword)) Then
                Return New TranslateTextRelative(operator_, operands)
            ElseIf (operator_.Equals(SetTextMatrix.OperatorKeyword)) Then
                Return New SetTextMatrix(operands)
            ElseIf (operator_.Equals(ModifyCTM.OperatorKeyword)) Then
                Return New ModifyCTM(operands)
            ElseIf (operator_.Equals(PaintXObject.OperatorKeyword)) Then
                Return New PaintXObject(operands)
            ElseIf (operator_.Equals(PaintShading.OperatorKeyword)) Then
                Return New PaintShading(operands)
            ElseIf (operator_.Equals(SetCharSpace.OperatorKeyword)) Then
                Return New SetCharSpace(operands)
            ElseIf (operator_.Equals(SetLineCap.OperatorKeyword)) Then
                Return New SetLineCap(operands)
            ElseIf (operator_.Equals(SetLineDash.OperatorKeyword)) Then
                Return New SetLineDash(operands)
            ElseIf (operator_.Equals(SetLineJoin.OperatorKeyword)) Then
                Return New SetLineJoin(operands)
            ElseIf (operator_.Equals(SetLineWidth.OperatorKeyword)) Then
                Return New SetLineWidth(operands)
            ElseIf (operator_.Equals(SetMiterLimit.OperatorKeyword)) Then
                Return New SetMiterLimit(operands)
            ElseIf (operator_.Equals(SetTextLead.OperatorKeyword)) Then
                Return New SetTextLead(operands)
            ElseIf (operator_.Equals(SetTextRise.OperatorKeyword)) Then
                Return New SetTextRise(operands)
            ElseIf (operator_.Equals(SetTextScale.OperatorKeyword)) Then
                Return New SetTextScale(operands)
            ElseIf (operator_.Equals(SetTextRenderMode.OperatorKeyword)) Then
                Return New SetTextRenderMode(operands)
            ElseIf (operator_.Equals(SetWordSpace.OperatorKeyword)) Then
                Return New SetWordSpace(operands)
            ElseIf (operator_.Equals(DrawLine.OperatorKeyword)) Then
                Return New DrawLine(operands)
            ElseIf (operator_.Equals(DrawRectangle.OperatorKeyword)) Then
                Return New DrawRectangle(operands)
            ElseIf (operator_.Equals(DrawCurve.FinalOperatorKeyword) OrElse
                    operator_.Equals(DrawCurve.FullOperatorKeyword) OrElse
                    operator_.Equals(DrawCurve.InitialOperatorKeyword)) Then
                Return New DrawCurve(operator_, operands)
            ElseIf (operator_.Equals(EndInlineImage.OperatorKeyword)) Then
                Return EndInlineImage.Value
            ElseIf (operator_.Equals(BeginText.OperatorKeyword)) Then
                Return BeginText.Value
            ElseIf (operator_.Equals(EndText.OperatorKeyword)) Then
                Return EndText.Value
            ElseIf (operator_.Equals(BeginMarkedContent.SimpleOperatorKeyword) OrElse
                    operator_.Equals(BeginMarkedContent.PropertyListOperatorKeyword)) Then
                Return New BeginMarkedContent(operator_, operands)
            ElseIf (operator_.Equals(EndMarkedContent.OperatorKeyword)) Then
                Return EndMarkedContent.Value
            ElseIf (operator_.Equals(MarkedContentPoint.SimpleOperatorKeyword) OrElse
                    operator_.Equals(MarkedContentPoint.PropertyListOperatorKeyword)) Then
                Return New MarkedContentPoint(operator_, operands)
            ElseIf (operator_.Equals(BeginInlineImage.OperatorKeyword)) Then
                Return BeginInlineImage.Value
            ElseIf (operator_.Equals(EndInlineImage.OperatorKeyword)) Then
                Return EndInlineImage.Value
            ElseIf (operator_.Equals(ApplyExtGState.OperatorKeyword)) Then
                Return New ApplyExtGState(operands)
            Else ' No explicit operation implementation available.
                Return New GenericOperation(operator_, operands)
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Protected _operator_ As String
        Protected _operands As IList(Of PdfDirectObject)

#End Region

#Region "onstructors"

        Protected Sub New(ByVal operator_ As String)
            Me._operator_ = operator_
        End Sub

        Protected Sub New(ByVal operator_ As String, ByVal operand As PdfDirectObject)
            Me._operator_ = operator_
            Me._operands = New List(Of PdfDirectObject)()
            Me._operands.Add(operand)
        End Sub

        Protected Sub New(ByVal operator_ As String, ParamArray operands As PdfDirectObject())
            Me._operator_ = operator_
            Me._operands = New List(Of PdfDirectObject)(operands)
        End Sub

        Protected Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            Me._operator_ = operator_
            Me._operands = operands
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public ReadOnly Property [Operator] As String
            Get
                Return Me._operator_
            End Get
        End Property

        Public ReadOnly Property Operands As IList(Of PdfDirectObject)
            Get
                Return Me._operands
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim buffer As New StringBuilder()
            ' Begin.
            buffer.Append("{")

            ' Operator.
            buffer.Append(_operator_)

            ' Operands.
            If (_operands IsNot Nothing) Then
                buffer.Append(" [")
                Dim count As Integer = _operands.Count
                For i As Integer = 0 To count - 1
                    If (i > 0) Then buffer.Append(", ")
                    buffer.Append(_operands(i).ToString())
                Next
                buffer.Append("]")
            End If

            ' End.
            buffer.Append("}")

            Return buffer.ToString()
        End Function

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)
            If (Me._operands IsNot Nothing) Then
                Dim fileContext As File = context.File
                For Each operand As PdfDirectObject In Me._operands
                    operand.WriteTo(stream, fileContext)
                    stream.Write(Chunk.Space)
                Next
            End If
            stream.Write(Me._operator_)
            stream.Write(Chunk.LineFeed)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace