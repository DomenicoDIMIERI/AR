'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Abstract 'show a text string' operation [PDF:1.6:5.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class ShowText
        Inherits Operation

#Region "types"

        Public Interface IScanner

            '      /**
            '  <summary>Notifies the scanner about a text character.</summary>
            '  <param name="textChar">Scanned character.</param>
            '  <param name="textCharBox">Bounding box of the scanned character.</param>
            '*/
            Sub ScanChar(ByVal textChar As Char, ByVal textCharBox As System.Drawing.RectangleF)

        End Interface

#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal operator_ As String)
            MyBase.New(operator_)
        End Sub

        Protected Sub New(ByVal operator_ As String, ParamArray operands As PdfDirectObject())
            MyBase.New(operator_, operands)
        End Sub

        Protected Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(operator_, operands)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Scan(state, Nothing)
        End Sub


        '/**
        '  <summary> Executes scanning On Me operation.</summary>
        '  <param name = "state" > Graphics state context.</param>
        '  <param name = "textScanner" > Scanner To be notified about text contents.
        '  In case it's Nothing, the operation is applied to the graphics state context.</param>
        '*/
        Public Overloads Sub Scan(ByVal state As ContentScanner.GraphicsState, ByVal textScanner As IScanner)
            '/*
            '  TODO: I really dislike Me solution -- it's a temporary hack until the event-driven
            '      parsing mechanism Is implemented...
            '*/
            '/*
            '  TODO: support to vertical writing mode.
            '*/

            Dim context As IContentContext = state.Scanner.ContentContext
            Dim contextHeight As Double = context.Box.Height
            Dim font As fonts.Font = state.Font
            If (font Is Nothing) Then
                Debug.Print("opps 3")
            End If
            Dim fontSize As Double = state.FontSize
            Dim scale As Double = state.Scale / 100
            Dim scaledFactor As Double = fonts.Font.GetScalingFactor(fontSize) * scale
            Dim wordSpace As Double = state.WordSpace * scale
            Dim charSpace As Double = state.CharSpace * scale
            Dim ctm As Matrix = state.Ctm.Clone()
            Dim tm As Matrix = state.Tm
            If (TypeOf (Me) Is ShowTextToNextLine) Then
                Dim ShowTextToNextLine As ShowTextToNextLine = CType(Me, ShowTextToNextLine)
                Dim newWordSpace As Double? = ShowTextToNextLine.WordSpace
                If (newWordSpace IsNot Nothing) Then
                    If (textScanner Is Nothing) Then
                        state.WordSpace = newWordSpace.Value
                    End If
                    wordSpace = newWordSpace.Value * scale
                End If
                Dim newCharSpace As Double? = ShowTextToNextLine.CharSpace
                If (newCharSpace IsNot Nothing) Then
                    If (textScanner Is Nothing) Then
                        state.CharSpace = newCharSpace.Value
                    End If
                    charSpace = newCharSpace.Value * scale
                End If
                tm = state.Tlm.Clone()
                tm.Translate(0, CSng(state.Lead))
            Else
                tm = state.Tm.Clone()
            End If

            For Each textElement As Object In Value
                If (TypeOf (textElement) Is Byte()) Then ' Text string.
                    Dim textString As String = Font.Decode(CType(textElement, Byte()))
                    For Each textChar As Char In textString
                        Dim charWidth As Double = Font.GetWidth(textChar) * scaledFactor
                        If (textScanner IsNot Nothing) Then
                            '/*
                            '  NOTE:       The Text rendering matrix Is recomputed before Each glyph Is painted
                            '  during a text-showing operation.
                            '*/
                            Dim trm As Matrix = ctm.Clone()
                            trm.Multiply(tm)
                            Dim charHeight As Double = Font.GetHeight(textChar, fontSize)
                            Dim charBox As System.Drawing.RectangleF = New System.Drawing.RectangleF(
                                                        trm.Elements(4),
                                                        CSng(contextHeight - trm.Elements(5) - Font.GetAscent(fontSize) * trm.Elements(3)),
                                                        CSng(charWidth) * trm.Elements(0),
                                                        CSng(charHeight) * trm.Elements(3)
                                                        )
                            textScanner.ScanChar(textChar, charBox)
                        End If

                        '/*
                        '  NOTE:         After the glyph Is painted, the text matrix Is updated
                        '  according to the glyph displacement And any applicable spacing parameter.
                        '*/
                        If (textChar = " "c) Then
                            tm.Translate(CSng(charWidth + charSpace + wordSpace), 0)
                        Else
                            tm.Translate(CSng(charWidth + charSpace + 0), 0)
                        End If

                    Next
                Else ' Text position adjustment.
                    tm.Translate(CSng(-Convert.ToSingle(textElement) * scaledFactor), 0)
                End If
            Next

            If (textScanner Is Nothing) Then
                state.Tm = tm
                If (TypeOf (Me) Is ShowTextToNextLine) Then
                    state.Tlm = tm.Clone()
                End If
            End If
        End Sub

        '/**
        '  <summary> Gets/Sets the encoded text.</summary>
        '  <remarks> Text Is expressed In native encoding: to resolve it to Unicode, pass it
        '  to the decode method of the corresponding font.</remarks>
        '*/
        Public MustOverride Property Text As Byte()

        '/**
        '  <summary> Gets/Sets the encoded text elements along With their adjustments.</summary>
        '  <remarks> Text Is expressed In native encoding: to resolve it to Unicode, pass it
        '  to the decode method of the corresponding font.</remarks>
        '  <returns>Each element can be either a Byte array Or a number
        '    <List type = "bullet" >
        '      <item>if it's a byte array (encoded text), the operator shows text glyphs;</item>
        '    <item>If it's a number (glyph adjustment), the operator inversely adjusts the next glyph position
        '      by that amount (that Is: a positive value reduces the distance between consecutive glyphs).</item>
        '    </list>
        '  </returns>
        '*/
        Public Overridable Property Value As IList(Of Object)
            Get
                Return New List(Of Object)({Me.Text})
            End Get
            Set(ByVal value As IList(Of Object))
                Me.Text = CType(value(0), Byte())
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace