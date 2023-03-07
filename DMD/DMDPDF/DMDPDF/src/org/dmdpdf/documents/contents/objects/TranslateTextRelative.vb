'/*
'  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
    '  <summary>'Move to the start of the next line, offset from the start of the current line' operation
    '  [PDF:1.6:5.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class TranslateTextRelative
        Inherits Operation

#Region "static"
#Region "fields"

        '/**
        '  <summary>No side effect.</summary>
        '*/
        Public Shared ReadOnly SimpleOperatorKeyword As String = "Td"
        '/**
        '  <summary> Lead parameter setting.</summary>
        '*/
        Public Shared ReadOnly LeadOperatorKeyword As String = "TD"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Private Shared Function IIF(ByVal cond As Boolean, ByVal o1 As String, ByVal o2 As String) As String
            If (cond) Then
                Return o1
            Else
                Return o2
            End If
        End Function

        Public Sub New(ByVal offsetX As Double, ByVal offsetY As Double)
            Me.New(offsetX, offsetY, False)
        End Sub

        Public Sub New(ByVal offsetX As Double, ByVal offsetY As Double, ByVal leadSet As Boolean)
            MyBase.New(IIf(leadSet, LeadOperatorKeyword, SimpleOperatorKeyword), PdfReal.Get(offsetX), PdfReal.Get(offsetY))
        End Sub


        Public Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            MyBase.new(operator_, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary> Gets/Sets whether this operation, As a side effect, sets the leading parameter In the text state.</summary>
        '*/
        Public Property LeadSet As Boolean
            Get
                Return Me._operator_.Equals(LeadOperatorKeyword)
            End Get
            Set(ByVal value As Boolean)
                Me._operator_ = IIF(value, LeadOperatorKeyword, SimpleOperatorKeyword)
            End Set
        End Property

        Public Property OffsetX As Double
            Get
                Return CType(Me._operands(0), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(0) = PdfReal.Get(value)
            End Set
        End Property

        Public Property OffsetY As Double
            Get
                Return CType(Me._operands(1), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(1) = PdfReal.Get(value)
            End Set
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            state.Tlm.Translate(CSng(OffsetX), CSng(OffsetY))
            state.Tm = state.Tlm.Clone()
            If (Me.LeadSet) Then
                state.Lead = OffsetY
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace