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
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Adobe standard glyph mapping (unicode-encoding against glyph-naming)
    '  [PDF:1.6:D;AGL:2.0].</summary>
    '*/
    Friend Class GlyphMapping

        Private Shared codes As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)()

        Shared Sub New()
            Load()
        End Sub

        Public Shared Function NameToCode(ByVal name As String) As Integer?
            Dim code As Integer = 0
            If (codes.TryGetValue(name, code)) Then
                Return code
            Else
                Return Nothing
            End If
        End Function

        '/**
        '  <summary>Loads the glyph list mapping character names to character codes (unicode
        '  encoding).</summary>
        '*/
        Private Shared Sub Load()
            Dim glyphListStream As StreamReader = Nothing
            Try
                '// Open the glyph list!
                '/*
                '  NOTE: The Adobe Glyph List [AGL:2.0] represents the reference name-to-unicode map
                '  for consumer applications.
                '*/
                'glyphListStream = New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("fonts.AGL20"))
                glyphListStream = New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("AGL20.scsv"))

                ' Parsing the glyph list...
                Dim line As String
                Dim linePattern As Regex = New Regex("^(\w+);([A-F0-9]+)$")
                line = glyphListStream.ReadLine()
                While (line IsNot Nothing)
                    Dim lineMatches As MatchCollection = linePattern.Matches(line)
                    If (lineMatches.Count < 1) Then
                        line = glyphListStream.ReadLine()
                        Continue While
                    End If

                    Dim lineMatch As Match = lineMatches(0)

                    Dim name As String = lineMatch.Groups(1).Value
                    Dim code As Integer = Int32.Parse(lineMatch.Groups(2).Value, NumberStyles.HexNumber)

                    ' Associate the character name with its corresponding character code!
                    codes(name) = code
                    line = glyphListStream.ReadLine()
                End While
            Finally
                If (glyphListStream IsNot Nothing) Then glyphListStream.Close() : glyphListStream = Nothing
            End Try
        End Sub

    End Class

End Namespace
