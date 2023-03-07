'/*
'  Copyright 2009-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>AFM file format parser [AFM:4.1].</summary>
    '*/
    Public NotInheritable Class AfmParser

#Region "types"
        '/**
        '  <summary>Font header (Global font information).</summary>
        '*/
        Public NotInheritable Class FontMetrics
            Public IsCustomEncoding As Boolean
            Public FontName As String
            Public Weight As String
            Public ItalicAngle As Single
            Public IsFixedPitch As Boolean
            Public XMin As Short
            Public YMin As Short
            Public XMax As Short
            Public YMax As Short
            Public UnderlinePosition As Short
            Public UnderlineThickness As Short
            Public CapHeight As Short
            Public XHeight As Short
            Public Ascender As Short
            Public Descender As Short
            Public StemH As Short
            Public StemV As Short
        End Class

#End Region

#Region "dynamic"
#Region "fields"

        Public Metrics As FontMetrics

        Public GlyphIndexes As Dictionary(Of Integer, Integer)
        Public GlyphKernings As Dictionary(Of Integer, Integer)
        Public GlyphWidths As Dictionary(Of Integer, Integer)

        Public FontData As IInputStream

#End Region

#Region "constructors"

        Friend Sub New(ByVal fontData As IInputStream)
            Me.FontData = fontData
            Load()
        End Sub

#End Region

#Region "Interface"
#Region "Private"

        Private Sub Load()
            Metrics = New FontMetrics()
            LoadFontHeader()
            LoadCharMetrics()
            LoadKerningData()
        End Sub

        '/**
        '  <summary> Loads the font header [AFM:4.1:3,4,4.1,4.2].</summary>
        '*/
        Private Sub LoadFontHeader()
            Dim line As String
            Dim linePattern As Regex = New Regex("(\S+)\s+(.+)")
            line = FontData.ReadLine()
            While (line IsNot Nothing)
                Dim lineMatches As MatchCollection = linePattern.Matches(line)
                If (lineMatches.Count < 1) Then
                    line = FontData.ReadLine()
                    Continue While
                End If
                Dim lineMatch As Match = lineMatches(0)
                Dim key As String = lineMatch.Groups(1).Value
                Select Case (key)
                    Case "FontName" : Metrics.FontName = lineMatch.Groups(2).Value 'break;
                    Case "Weight" : Metrics.Weight = lineMatch.Groups(2).Value 'break;
                    Case "ItalicAngle" : Metrics.ItalicAngle = Single.Parse(lineMatch.Groups(2).Value)' break;
                    Case "IsFixedPitch" : Metrics.IsFixedPitch = lineMatch.Groups(2).Value.Equals("true") 'break;
                    Case "FontBBox"
                        Dim coordinates As String() = Regex.Split(lineMatch.Groups(2).Value, "\s+")
                        Metrics.XMin = Int16.Parse(coordinates(0))
                        Metrics.YMin = Int16.Parse(coordinates(1))
                        Metrics.XMax = Int16.Parse(coordinates(2))
                        Metrics.YMax = Int16.Parse(coordinates(3))
            ' break;
                    Case "UnderlinePosition"
                        Metrics.UnderlinePosition = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "UnderlineThickness"
                        Metrics.UnderlineThickness = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "EncodingScheme"
                        Metrics.IsCustomEncoding = lineMatch.Groups(2).Value.Equals("FontSpecific")' break;
                    Case "CapHeight"
                        Metrics.CapHeight = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "XHeight"
                        Metrics.XHeight = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "Ascender"
                        Metrics.Ascender = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "Descender"
                        Metrics.Descender = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "StdHW"
                        Metrics.StemH = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "StdVW"
                        Metrics.StemV = Int16.Parse(lineMatch.Groups(2).Value)' break;
                    Case "StartCharMetrics"
                        GoTo endParsing
                End Select
                line = FontData.ReadLine()
            End While
endParsing:
            If (Metrics.Ascender = 0) Then
                Metrics.Ascender = Metrics.YMax
            End If
            If (Metrics.Descender = 0) Then
                Metrics.Descender = Metrics.YMin
            End If
        End Sub

        '/**
        '  <summary> Loads individual character metrics [AFM:4.1:3,4,4.4,8].</summary>
        '*/
        Private Sub LoadCharMetrics()
            GlyphIndexes = New Dictionary(Of Integer, Integer)()
            GlyphWidths = New Dictionary(Of Integer, Integer)()

            Dim line As String
            Dim linePattern As Regex = New Regex("C (\S+) ; WX (\S+) ; N (\S+)")
            Dim implicitCharCode As Integer = Short.MaxValue
            line = FontData.ReadLine()
            While (line IsNot Nothing)
                Dim lineMatches As MatchCollection = linePattern.Matches(line)
                If (lineMatches.Count < 1) Then
                    If (line.Equals("EndCharMetrics")) Then
                        Exit While
                    End If
                    line = FontData.ReadLine()
                    Continue While
                End If

                Dim lineMatch As Match = lineMatches(0)
                Dim charCode As Integer = Int32.Parse(lineMatch.Groups(1).Value)
                Dim width As Integer = Int32.Parse(lineMatch.Groups(2).Value)
                Dim charName As String = lineMatch.Groups(3).Value
                If (charCode < 0) Then
                    If (charName Is Nothing) Then Continue While
                    implicitCharCode += 1
                    charCode = implicitCharCode
                End If
                Dim code As Integer
                If (charName Is Nothing OrElse Metrics.IsCustomEncoding) Then
                    code = charCode
                Else
                    code = GlyphMapping.NameToCode(charName).Value
                End If

                GlyphIndexes(code) = charCode
                GlyphWidths(charCode) = width
                line = FontData.ReadLine()
            End While
        End Sub

        '/**
        '  <summary> Loads kerning data [AFM:4.1:3,4,4.5,9].</summary>
        '*/
        Private Sub LoadKerningData()
            GlyphKernings = New Dictionary(Of Integer, Integer)()

            Dim line As String
            line = FontData.ReadLine()
            While (line IsNot Nothing)
                If (line.StartsWith("StartKernPairs")) Then Exit While
                line = FontData.ReadLine()
            End While

            Dim linePattern As Regex = New Regex("KPX (\S+) (\S+) (\S+)")
            line = FontData.ReadLine()
            While (line IsNot Nothing)
                Dim lineMatches As MatchCollection = linePattern.Matches(line)
                If (lineMatches.Count < 1) Then
                    If (line.Equals("EndKernPairs")) Then
                        Exit While
                    End If
                    line = FontData.ReadLine()
                    Continue While
                End If
                Dim lineMatch As Match = lineMatches(0)
                Dim code1 As Integer = GlyphMapping.NameToCode(lineMatch.Groups(1).Value).Value
                Dim code2 As Integer = GlyphMapping.NameToCode(lineMatch.Groups(2).Value).Value
                Dim pair As Integer = code1 << 16 + code2
                Dim value As Integer = Int32.Parse(lineMatch.Groups(3).Value)
                GlyphKernings(pair) = value
                line = FontData.ReadLine()
            End While
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace