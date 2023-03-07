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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Type 1 font parser.</summary>
    '*/
    Friend NotInheritable Class PfbParser

#Region "dynamic"
#Region "fields"

        Private _stream As IInputStream

#End Region

#Region "constructors"

        Friend Sub New(ByVal stream As IInputStream)
            Me._stream = stream
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Parses the character-code-To-unicode mapping [PDF:1.6:5.9.1].</summary>
        '*/
        Public Function Parse() As Dictionary(Of ByteArray, Integer)
            Dim codes As Dictionary(Of ByteArray, Integer) = New Dictionary(Of ByteArray, Integer)()

            Dim line As String
            Dim linePattern As Regex = New Regex("(\S+)\s+(.+)")
            line = _stream.ReadLine()
            While (line IsNot Nothing) 'line = _stream.ReadLine()
                Dim lineMatches As MatchCollection = linePattern.Matches(line)
                If (lineMatches.Count < 1) Then Continue While
                Dim lineMatch As Match = lineMatches(0)
                Dim key As String = lineMatch.Groups(1).Value
                If (key.Equals("/Encoding")) Then
                    ' Skip to the encoding array entries!
                    _stream.ReadLine()
                    Dim encodingLine As String
                    Dim encodingLinePattern As Regex = New Regex("dup (\S+) (\S+) put")
                    encodingLine = _stream.ReadLine()
                    'While ((encodingLine = _stream.ReadLine())!= null)
                    While (encodingLine IsNot Nothing)
                        Dim encodingLineMatches As MatchCollection = encodingLinePattern.Matches(encodingLine)
                        If (encodingLineMatches.Count < 1) Then Exit While
                        Dim encodingLineMatch As Match = encodingLineMatches(0)
                        Dim inputCode As Byte() = New Byte() {CByte(Int32.Parse(encodingLineMatch.Groups(1).Value))}
                        Dim name As String = encodingLineMatch.Groups(2).Value.Substring(1)
                        codes(New ByteArray(inputCode)) = GlyphMapping.NameToCode(name).Value
                        encodingLine = _stream.ReadLine()
                    End While
                    Exit While
                End If
                line = _stream.ReadLine()
            End While

            Return codes
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace