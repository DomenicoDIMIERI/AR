'/*
'  Copyright 2008 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With this
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  this list Of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.xObjects

Imports System
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.entities

    '/**
    '  <see href = "http://en.wikipedia.org/wiki/EAN13" > EAN - 13 Bar Code</see> Object [GS1:7.1:5.1.1.3.1].

    '  <para> The EAN-13 Bar Code Symbol shall be made up As follows, reading from left To right:</para>
    '  <List type = "number" >
    '    <item>A left Quiet Zone</item>
    '  <item> A normal Guard Bar Pattern (Left Guard)</item>
    '    <item> Six symbol characters from number sets A And B (Left Half)</item>
    '    <item> A center Guard Bar Pattern (Center Guard)</item>
    '    <item> Six symbol characters from number Set C (Right Half)</item>
    '    <item> A normal Guard Bar Pattern (Right Guard)</item>
    '    <item> A right Quiet Zone</item>
    '  </list>
    '  <para> The rightmost symbol character shall encode the Check Digit.</para>
    '*/
    Public NotInheritable Class EAN13Barcode
        Inherits Barcode

        '    /*
        '  NOTE: Conventional terms used within this implementation:
        '    * module: smallest encoding unit (either a bar (dark Module) Or a space (light Module);
        '    * element: sequence of omogeneous modules (either all bars Or all spaces);
        '    * symbol character: code digit, whose encoding Is made up Of 4 elements encompassing 7 modules;
        '    * number set: symbol character encoding, representing the codomain Of the digit domain
        '      (i.e. [0-9]).
        '*/
#Region "static"
#Region "fields"
        '/**
        '  Symbol Character Encodation (Number Set A, odd parity) [GS1:7.1:5.1.1.2.1].
        '  NOTE: Number Set B uses the same patterns (though at inverted parity, i.e. even),
        '  whilst Number Set C (even parity) mirrors Number Set B.
        '*/
        Private Shared ReadOnly _DigitElementWidths As Integer()() = {
                                              New Integer() {3, 2, 1, 1},
                                              New Integer() {2, 2, 2, 1},
                                              New Integer() {2, 1, 2, 2},
                                              New Integer() {1, 4, 1, 1},
                                              New Integer() {1, 1, 3, 2},
                                              New Integer() {1, 2, 3, 1},
                                              New Integer() {1, 1, 1, 4},
                                              New Integer() {1, 3, 1, 2},
                                              New Integer() {1, 2, 1, 3},
                                              New Integer() {3, 1, 1, 2}
                                            }

        '/** Bar elements count. */
        Private Shared _ElementCount As Integer

        '/** Digit box height. */
        Private Shared ReadOnly _DigitHeight As Integer
        '/** Digit box width. */
        Private Shared ReadOnly _DigitWidth As Integer

        '/** Bar full height. */
        Private Shared ReadOnly _BarHeight As Integer

        '/** Digit glyph width. */
        Private Shared ReadOnly _DigitGlyphWidth As Integer
        '/** Digit glyph horizontal positions. */
        Private Shared ReadOnly _DigitGlyphXs As Double()

        '/** Guard bar index positions. */
        Private Shared _GuardBarIndexes As Integer() = {
                                      0, 2,
                                      28, 30,
                                      56, 58
                                    }

        Private Shared ReadOnly _NumberSet_A As Integer = 0
        Private Shared ReadOnly _NumberSet_B As Integer = 1
        '/**
        '  Left Half Of an EAN-13 Bar Code Symbol.
        '  Since the EAN-13 Bar Code Symbol comprises only 12 symbol characters
        '  but encodes 13 digits Of data (including the Check Digit),
        '  the value Of the additional digit (leading digit, implicitly encoded),
        '  which Is the character in the leftmost position in the data string,
        '  shall be encoded by the variable parity mix Of number sets A And B
        '  For the six symbol characters in the left half of the symbol.
        '*/
        Private Shared ReadOnly _LeftHalfNumberSets As Integer()() =
                                    {
                                      New Integer() {_NumberSet_A, _NumberSet_A, _NumberSet_A, _NumberSet_A, _NumberSet_A, _NumberSet_A},
                                      New Integer() {_NumberSet_A, _NumberSet_A, _NumberSet_B, _NumberSet_A, _NumberSet_B, _NumberSet_B},
                                      New Integer() {_NumberSet_A, _NumberSet_A, _NumberSet_B, _NumberSet_B, _NumberSet_A, _NumberSet_B},
                                      New Integer() {_NumberSet_A, _NumberSet_A, _NumberSet_B, _NumberSet_B, _NumberSet_B, _NumberSet_A},
                                      New Integer() {_NumberSet_A, _NumberSet_B, _NumberSet_A, _NumberSet_A, _NumberSet_B, _NumberSet_B},
                                      New Integer() {_NumberSet_A, _NumberSet_B, _NumberSet_B, _NumberSet_A, _NumberSet_A, _NumberSet_B},
                                      New Integer() {_NumberSet_A, _NumberSet_B, _NumberSet_B, _NumberSet_B, _NumberSet_A, _NumberSet_A},
                                      New Integer() {_NumberSet_A, _NumberSet_B, _NumberSet_A, _NumberSet_B, _NumberSet_A, _NumberSet_B},
                                      New Integer() {_NumberSet_A, _NumberSet_B, _NumberSet_A, _NumberSet_B, _NumberSet_B, _NumberSet_A},
                                      New Integer() {_NumberSet_A, _NumberSet_B, _NumberSet_B, _NumberSet_A, _NumberSet_B, _NumberSet_A}
                                    }
#End Region

#Region "constructors"

        Shared Sub New()
            '/*
            '  Digit metrics.
            '*/
            '{
            Dim digitElementWidths As Integer() = _DigitElementWidths(0)

            _ElementCount = 3 + digitElementWidths.Length * 6 + 5 + digitElementWidths.Length * 6 + 3

            Dim digitWidth As Integer = 0
            For Each digitElementWidth As Integer In digitElementWidths
                digitWidth += digitElementWidth
            Next
            _DigitWidth = digitWidth
            _DigitHeight = _DigitWidth + 2
            _DigitGlyphWidth = _DigitWidth - 1
            _BarHeight = _DigitHeight * 4
            '}

            '/*
            '  Digit glyph horizontal positions.
            '*/
            '{
            Dim elementWidths As Double() =
                                    {
                                      _DigitWidth,
                                      3,
                                      _DigitWidth, _DigitWidth, _DigitWidth, _DigitWidth, _DigitWidth, _DigitWidth,
                                      5,
                                      _DigitWidth, _DigitWidth, _DigitWidth, _DigitWidth, _DigitWidth, _DigitWidth,
                                      3
                                    }
            Dim digitIndexes As Integer() = {0, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14}
            _DigitGlyphXs = New Double(13 - 1) {}
            Dim digitXIndex As Integer = 0
            Dim length As Integer = elementWidths.Length
            For index As Integer = 0 To length - 1
                If (index < digitIndexes(digitXIndex)) Then
                    _DigitGlyphXs(digitXIndex) += elementWidths(index)
                Else
                    _DigitGlyphXs(digitXIndex) += elementWidths(index) / 2
                    digitXIndex += 1
                    If (digitXIndex >= _DigitGlyphXs.Length) Then
                        Exit For
                    End If
                    _DigitGlyphXs(digitXIndex) = _DigitGlyphXs(digitXIndex - 1) + elementWidths(index) / 2
                End If
            Next
            '}
        End Sub

#End Region
#End Region

#Region "dynamic"
#Region "fields"
#End Region

#Region "constructors"

        Public Sub New(ByVal code As String)
            MyBase.New(code)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function ToInlineObject(ByVal composer As PrimitiveComposer) As ContentObject
            Dim barcodeObject As ContentObject = composer.BeginLocalState()
            '{
            Dim font As fonts.Font = New StandardType1Font(composer.Scanner.Contents.Document, StandardType1Font.FamilyEnum.Helvetica, False, False)
            Dim fontSize As Double = (_DigitGlyphWidth / font.GetWidth(Code.Substring(0, 1), 1))

            '// 1. Bars.
            '{
            Dim elementX As Double = _DigitWidth
            Dim elementWidths As Integer() = GetElementWidths()

            Dim guardBarIndentY As Double = _DigitHeight / 2
            Dim isBar As Boolean = True
            For elementIndex As Integer = 0 To elementWidths.Length - 1
                Dim elementWidth As Double = elementWidths(elementIndex)
                '// Dark element?
                '/*
                '  NOTE:         EAN symbol elements alternate bars To spaces.
                '*/
                If (isBar) Then
                    If (Array.BinarySearch(Of Integer)(_GuardBarIndexes, elementIndex) >= 0) Then
                        composer.DrawRectangle(New RectangleF(CSng(elementX), 0, CSng(elementWidth), CSng(_BarHeight + guardBarIndentY)))
                    Else
                        composer.DrawRectangle(New RectangleF(CSng(elementX), 0, CSng(elementWidth), CSng(_BarHeight)))
                    End If
                End If
                elementX += elementWidth
                isBar = Not isBar
            Next
            composer.Fill()
            '}

            ' 2. Digits.
            '{
            composer.SetFont(font, fontSize)
            Dim digitY As Double = _BarHeight + (_DigitHeight - (font.GetAscent(fontSize))) / 2
            ' Showing the digits...
            For digitIndex As Integer = 0 To 13 - 1
                Dim digit As String = Code.Substring(digitIndex, 1)
                Dim pX As Double = _DigitGlyphXs(digitIndex) - font.GetWidth(digit, fontSize) / 2 '// Digit position. Centering.
                ' Show the current digit!
                composer.ShowText(digit, New PointF(CSng(pX), CSng(digitY)))
            Next
            '}
            composer.End()
            '}
            Return barcodeObject
        End Function

        Public Overrides Function ToXObject(ByVal context As Document) As xObjects.XObject
            Dim XObject As FormXObject = New FormXObject(context, Size)
            Dim composer As New PrimitiveComposer(XObject)
            Me.ToInlineObject(composer)
            composer.Flush()
            Return XObject
        End Function

#End Region

#Region "private"
        '/**
        '  <summary> Gets the code elements widths.</summary>
        '*/
        Private Function GetElementWidths() As Integer()
            '// 1. Digit-codes-to-digit-IDs transformation.
            '/* NOTE: Leveraging the ASCII charset sequence. */
            Dim digits As Integer() = New Integer(Me._code.Length - 1) {}
            For index As Integer = 0 To digits.Length - 1
                digits(index) = Asc(Me._code(index)) - Asc("0"c)
            Next

            ' 2. Element widths calculation.
            Dim elementWidths As Integer() = New Integer(_ElementCount - 1) {}
            Dim elementIndex As Integer = 0

            ' Left Guard Bar Pattern (3 elements).
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1

            Dim digitIndex As Integer = 0

            ' Left Half (6 digits, 4 elements each).
            Dim leftHalfNumberSets As Integer() = _LeftHalfNumberSets(digits(digitIndex)) : digitIndex += 1 ' Gets the left-half number Set encoding sequence based On the leading digit.
            Do
                Dim digitElementWidths As Integer() = _DigitElementWidths(digits(digitIndex))
                ' Number Set A encoding to apply?
                If (leftHalfNumberSets(digitIndex - 1) = _NumberSet_A) Then ' Number Set A encoding.
                    elementWidths(elementIndex) = digitElementWidths(0) : elementIndex += 1
                    elementWidths(elementIndex) = digitElementWidths(1) : elementIndex += 1
                    elementWidths(elementIndex) = digitElementWidths(2) : elementIndex += 1
                    elementWidths(elementIndex) = digitElementWidths(3) : elementIndex += 1
                Else ' Number Set B encoding.
                    elementWidths(elementIndex) = digitElementWidths(3) : elementIndex += 1
                    elementWidths(elementIndex) = digitElementWidths(2) : elementIndex += 1
                    elementWidths(elementIndex) = digitElementWidths(1) : elementIndex += 1
                    elementWidths(elementIndex) = digitElementWidths(0) : elementIndex += 1
                End If
                digitIndex += 1
            Loop While (digitIndex - 1 < leftHalfNumberSets.Length)

            ' Center Guard Bar Pattern (5 elements).
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1

            ' Right Half (6 digits, 4 elements each).
            Do
                Dim digitElementWidths As Integer() = _DigitElementWidths(digits(digitIndex))
                ' NOTE: Number Set C encoding.
                elementWidths(elementIndex) = digitElementWidths(0) : elementIndex += 1
                elementWidths(elementIndex) = digitElementWidths(1) : elementIndex += 1
                elementWidths(elementIndex) = digitElementWidths(2) : elementIndex += 1
                elementWidths(elementIndex) = digitElementWidths(3) : elementIndex += 1
                digitIndex += 1
            Loop While (digitIndex - 1 < 12)

            ' Right Guard Bar Pattern (3 elements).
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1
            elementWidths(elementIndex) = 1 : elementIndex += 1

            Return elementWidths
        End Function

        '/**
        '  Gets the barcode's graphical size.
        '*/
        Private ReadOnly Property Size As Size
            Get
                Return New Size(_DigitWidth * 13 + 3 * 2 + 5, _BarHeight + _DigitHeight)
            End Get

        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace