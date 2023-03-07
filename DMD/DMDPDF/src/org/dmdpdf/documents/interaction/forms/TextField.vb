'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

'imports bytes = org.dmdpdf.bytes;
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.tokens
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Text field [PDF:1.6:8.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class TextField
        Inherits Field

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new text field within the given document context.</summary>
        '*/
        Public Sub New(ByVal name As String, ByVal widget As Widget, ByVal value As String)
            MyBase.New(PdfName.Tx, name, widget)
            Me.Value = value
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets/Sets whether the field can contain multiple lines of text.</summary>
        '*/
        Public Property IsMultiline As Boolean
            Get
                Return (Me.Flags And FlagsEnum.Multiline) = FlagsEnum.Multiline
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.Multiline, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether the field is intended for entering a secure password.</summary>
        '*/
        Public Property IsPassword As Boolean
            Get
                Return (Me.Flags And FlagsEnum.Password) = FlagsEnum.Password
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Flags, FlagsEnum.Password, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the justification to be used in displaying this field's text.</summary>
        '*/
        Public Property Justification As JustificationEnum
            Get
                Return JustificationEnumExtension.Get(CType(Me.BaseDataObject(PdfName.Q), PdfInteger))
            End Get
            Set(ByVal value As JustificationEnum)
                Me.BaseDataObject(PdfName.Q) = value.GetCode()
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the maximum length of the field's text, in characters.</summary>
        '  <remarks>It corresponds to the maximum integer value in case no explicit limit is defined.</remarks>
        '*/
        Public Property MaxLength As Integer
            Get
                Dim maxLengthObject As PdfInteger = CType(PdfObject.Resolve(GetInheritableAttribute(PdfName.MaxLen)), PdfInteger)
                If (maxLengthObject IsNot Nothing) Then
                    Return maxLengthObject.IntValue
                Else
                    Return Int32.MaxValue
                End If
            End Get
            Set(ByVal value As Integer)
                If (value <> Int32.MaxValue) Then
                    Me.BaseDataObject(PdfName.MaxLen) = PdfInteger.Get(value)
                Else
                    Me.BaseDataObject(PdfName.MaxLen) = Nothing
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether text entered in the field is spell-checked.</summary>
        '*/
        Public Property SpellChecked As Boolean
            Get
                Return (Me.Flags And FlagsEnum.DoNotSpellCheck) <> FlagsEnum.DoNotSpellCheck
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.DoNotSpellCheck, Not value)
            End Set
        End Property

        Public Overrides Property Value As Object
            Get
                Return MyBase.Value
            End Get
            Set(ByVal value As Object)
                Dim stringValue As String = CStr(value)
                If (stringValue IsNot Nothing) Then
                    Dim maxLength As Integer = Me.MaxLength
                    If (stringValue.Length > maxLength) Then
                        stringValue = stringValue.Remove(maxLength)
                    End If
                End If
                BaseDataObject(PdfName.V) = New PdfTextString(stringValue)
                RefreshAppearance()
            End Set
        End Property

#End Region

#Region "Private"

        Private Sub RefreshAppearance()
            Dim widget As Widget = Me.Widgets(0)
            Debug.Print(widget.BaseDataObject.ToString)
            Dim normalAppearance As FormXObject
            Dim normalAppearances As AppearanceStates = widget.Appearance.Normal
            normalAppearance = normalAppearances(Nothing)
            If (normalAppearance Is Nothing) Then
                normalAppearance = New FormXObject(Document, widget.Box.Size)
                normalAppearances(Nothing) = normalAppearance
            End If
            Dim fontName As PdfName = Nothing
            Dim fontSize As Double = 0

            Dim defaultAppearanceState As PdfString = Me.DefaultAppearanceState
            If (defaultAppearanceState Is Nothing) Then
                ' Retrieving the font to define the default appearance...
                Dim defaultFont As fonts.Font = Nothing
                Dim defaultFontName As PdfName = Nothing
                '{
                ' Field fonts.
                Dim normalAppearanceFonts As FontResources = normalAppearance.Resources.Fonts
                For Each entry As KeyValuePair(Of PdfName, fonts.Font) In normalAppearanceFonts
                    If (Not entry.Value.Symbolic) Then
                        defaultFont = entry.Value
                        defaultFontName = entry.Key
                        Exit For
                    End If
                Next
                If (defaultFontName Is Nothing) Then
                    'Common fonts.
                    Dim formFonts As FontResources = Document.Form.Resources.Fonts
                    For Each entry As KeyValuePair(Of PdfName, fonts.Font) In formFonts
                        If (Not entry.Value.Symbolic) Then
                            defaultFont = entry.Value
                            defaultFontName = entry.Key
                            Exit For
                        End If
                    Next
                    If (defaultFontName Is Nothing) Then
                        'TODO:manage Name collision!
                        defaultFont = New fonts.StandardType1Font(Document, fonts.StandardType1Font.FamilyEnum.Helvetica, False, False)
                        defaultFontName = New PdfName("default")
                        formFonts(defaultFontName) = defaultFont
                    End If
                    normalAppearanceFonts(defaultFontName) = defaultFont
                End If
                Dim Buffer As New bytes.Buffer()
                Dim tmp As SetFont
                If (Me.IsMultiline) Then
                    tmp = New SetFont(defaultFontName, 10)
                Else
                    tmp = New SetFont(defaultFontName, 0)
                End If
                tmp.WriteTo(Buffer, Document)
                defaultAppearanceState = New PdfString(Buffer.ToByteArray())
                widget.BaseDataObject(PdfName.DA) = defaultAppearanceState
            End If

            ' Retrieving the font to use...
            Dim parser As ContentParser = New ContentParser(defaultAppearanceState.ToByteArray())
            For Each content As ContentObject In parser.ParseContentObjects()
                If (TypeOf (content) Is SetFont) Then
                    Dim setFontOperation As SetFont = CType(content, SetFont)
                    fontName = setFontOperation.Name
                    fontSize = setFontOperation.Size
                    Exit For
                End If
            Next
            normalAppearance.Resources.Fonts(fontName) = Document.Form.Resources.Fonts(fontName)

            '// Refreshing the field appearance...
            '/*
            ' * TODO: Resources MUST be resolved both through the apperance stream resource dictionary And
            ' * from the DR-entry acroform resource dictionary
            ' */
            Dim baseComposer As PrimitiveComposer = New PrimitiveComposer(normalAppearance)
            Dim composer As BlockComposer = New BlockComposer(baseComposer)
            Dim currentLevel As ContentScanner = composer.Scanner
            Dim textShown As Boolean = False
            While (currentLevel IsNot Nothing)
                If (Not currentLevel.MoveNext()) Then
                    currentLevel = currentLevel.ParentLevel
                    Continue While
                End If

                Dim content As ContentObject = currentLevel.Current
                If (TypeOf (content) Is MarkedContent) Then
                    Dim markedContent As MarkedContent = CType(content, MarkedContent)
                    If (PdfName.Tx.Equals(CType(markedContent.Header, BeginMarkedContent).Tag)) Then
                        ' Remove old text representation!
                        markedContent.Objects.Clear()
                        ' Add New text representation!
                        baseComposer.Scanner = currentLevel.ChildLevel ' Ensures the composer places New contents within the marked content block.
                        ShowText(composer, fontName, fontSize)
                        textShown = True
                    End If
                ElseIf (TypeOf (content) Is Text) Then
                    currentLevel.Remove()
                ElseIf (currentLevel.ChildLevel IsNot Nothing) Then
                    currentLevel = currentLevel.ChildLevel
                End If
            End While
            If (Not textShown) Then
                baseComposer.BeginMarkedContent(PdfName.Tx)
                ShowText(composer, fontName, fontSize)
                baseComposer.End()
            End If
            baseComposer.Flush()
        End Sub

        Private Sub ShowText(ByVal composer As BlockComposer, ByVal fontName As PdfName, ByVal fontSize As Double)
            Dim baseComposer As PrimitiveComposer = composer.BaseComposer
            Dim scanner As ContentScanner = baseComposer.Scanner
            Dim textBox As RectangleF = scanner.ContentContext.Box
            If (scanner.State.Font Is Nothing) Then
                '/*
                '  NOTE: A zero value for size means that the font is to be auto-sized: its size is computed as
                '  a function of the height of the annotation rectangle.
                '*/
                If (fontSize = 0) Then
                    fontSize = textBox.Height * 0.65
                End If
                baseComposer.SetFont(fontName, fontSize)
            End If

            Dim text As String = CStr(Value)

            Dim flags As FlagsEnum = flags
            If ((flags And FlagsEnum.Comb) = FlagsEnum.Comb AndAlso
                (flags And FlagsEnum.FileSelect) = 0 AndAlso
                (flags And FlagsEnum.Multiline) = 0 AndAlso
                (flags And FlagsEnum.Password) = 0) Then
                Dim maxLength As Integer = Me.MaxLength
                If (maxLength > 0) Then
                    textBox.Width /= maxLength
                    Dim length As Integer = text.Length
                    For index As Integer = 0 To length - 1
                        composer.Begin(textBox, XAlignmentEnum.Center, YAlignmentEnum.Middle)
                        composer.ShowText(text(index).ToString())
                        composer.End()
                        textBox.X += textBox.Width
                    Next
                    Return
                End If
            End If

            textBox.X += 2
            textBox.Width -= 4
            Dim yAlignment As YAlignmentEnum
            If ((flags And FlagsEnum.Multiline) = FlagsEnum.Multiline) Then
                yAlignment = YAlignmentEnum.Top
                textBox.Y += CSng(fontSize * 0.35)
                textBox.Height -= CSng(fontSize * 0.7)
            Else
                yAlignment = YAlignmentEnum.Middle
            End If
            composer.Begin(textBox, Justification.ToXAlignment(), yAlignment)
            composer.ShowText(text)
            composer.End()
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace