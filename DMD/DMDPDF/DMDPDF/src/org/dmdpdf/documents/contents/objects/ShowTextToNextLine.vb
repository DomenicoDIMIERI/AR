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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Move to the next line and show a text string' operation [PDF:1.6:5.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class ShowTextToNextLine
        Inherits ShowText

#Region "static"
#Region "fields"
        '/**
        '  <summary>Specifies no text state parameter
        '  (just uses the current settings).</summary>
        '*/
        Public Shared ReadOnly SimpleOperatorKeyword As String = "'"
        '/**
        '  <summary>Specifies the word spacing and the character spacing
        '  (setting the corresponding parameters in the text state).</summary>
        '*/
        Public Shared ReadOnly SpaceOperatorKeyword As String = "''"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <param name = "text" > Text encoded Using current font's encoding.</param>
        '*/
        Public Sub New(ByVal text As Byte())
            MyBase.New(SimpleOperatorKeyword, New PdfString(text))
        End Sub

        '/**
        '  <param name = "text" > text encoded Using current font's encoding.</param>
        '  <param name = "wordSpace" > Word spacing.</param>
        '  <param name = "charSpace" > Character spacing.</param>
        '*/
        Public Sub New(ByVal text As Byte(), ByVal wordSpace As Double, ByVal charSpace As Double)
            MyBase.New(SpaceOperatorKeyword, PdfReal.Get(wordSpace), PdfReal.Get(charSpace), New PdfString(text))
        End Sub

        Public Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(operator_, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the character spacing.</summary>
        '*/
        Public Property CharSpace As Double?
            Get
                If (Me._operator_.Equals(SimpleOperatorKeyword)) Then
                    Return Nothing
                Else
                    Return CType(Me._operands(1), IPdfNumber).RawValue
                End If
            End Get
            Set(ByVal value As Double?)
                EnsureSpaceOperation()
                Me._operands(1) = PdfReal.Get(value.Value)
            End Set
        End Property

        Public Overrides Property Text As Byte()
            Get
                If (Me._operator_.Equals(SimpleOperatorKeyword)) Then
                    Return CType(Me._operands(0), PdfString).RawValue
                Else
                    Return CType(Me._operands(2), PdfString).RawValue
                End If
            End Get
            Set(ByVal value As Byte())
                If (Me._operator_.Equals(SimpleOperatorKeyword)) Then
                    Me._operands(0) = New PdfString(value)
                Else
                    Me._operands(2) = New PdfString(value)
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the word spacing.</summary>
        '*/
        Public Property WordSpace As Double?
            Get
                If (Me._operator_.Equals(SimpleOperatorKeyword)) Then
                    Return Nothing
                Else
                    Return CType(Me._operands(0), IPdfNumber).RawValue
                End If
            End Get
            Set(ByVal value As Double?)
                EnsureSpaceOperation()
                Me._operands(0) = PdfReal.Get(value.Value)
            End Set
        End Property

#End Region

#Region "Private"

        Private Sub EnsureSpaceOperation()
            If (Me._operator_.Equals(SimpleOperatorKeyword)) Then
                Me._operator_ = SpaceOperatorKeyword
                Me._operands.Insert(0, PdfReal.Get(0))
                Me._operands.Insert(1, PdfReal.Get(0))
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace