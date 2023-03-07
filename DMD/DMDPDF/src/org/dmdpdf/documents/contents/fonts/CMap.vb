'/*
'  Copyright 2010-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Reflection

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>Character map [PDF:1.6:5.6.4].</summary>
    '*/
    Friend NotInheritable Class CMap

#Region "static"
#Region "interface"
        '/**
        '  <summary>Gets the character map extracted from the given data.</summary>
        '  <param name="stream">Character map data.</param>
        '*/
        Public Shared Function [Get](ByVal stream As bytes.IInputStream) As IDictionary(Of ByteArray, Integer)
            Dim parser As CMapParser = New CMapParser(stream)
            Return parser.Parse()
        End Function

        '/**
        '  <summary>Gets the character map extracted from the given encoding Object.</summary>
        '  <param name = "encodingObject" > Encoding Object.</param>
        '*/
        Public Shared Function [Get](ByVal encodingObject As PdfDataObject) As IDictionary(Of ByteArray, Integer)
            If (encodingObject Is Nothing) Then Return Nothing
            If (TypeOf (encodingObject) Is PdfName) Then ' Predefined CMap.
                Return [Get](CType(encodingObject, PdfName))
            ElseIf (TypeOf (encodingObject) Is PdfStream) Then ' Embedded Then CMap file.
                Return [Get](CType(encodingObject, PdfStream))
            Else
                Throw New NotSupportedException("Unknown encoding object type: " + encodingObject.GetType().Name)
            End If
        End Function

        '/**
        '  <summary>Gets the character map extracted from the given data.</summary>
        '  <param name = "stream" > Character map data.</param>
        '*/
        Public Shared Function [Get](ByVal Stream As PdfStream) As IDictionary(Of ByteArray, Integer)
            Return [Get](Stream.Body)
        End Function

        '/**
        '  <summary>Gets the character map corresponding To the given name.</summary>
        '  <param name = "name" > Predefined character map name.</param>
        '  <returns>Nothing, in Case no name matching occurs.</returns>
        '*/
        Public Shared Function [Get](ByVal name As PdfName) As IDictionary(Of ByteArray, Integer)
            Return [Get](CStr(name.Value))
        End Function

        '/**
        '  <summary>Gets the character map corresponding To the given name.</summary>
        '  <param name = "name" > Predefined character map name.</param>
        '  <returns>Nothing, in Case no name matching occurs.</returns>
        '*/
        Public Shared Function [Get](ByVal name As String) As IDictionary(Of ByteArray, Integer)
            Dim CMap As IDictionary(Of ByteArray, Integer)
            '{
            'Dim cmapResourceStream As System.IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("fonts.cmap." & name)
            Dim cmapResourceStream As System.IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)
            If (cmapResourceStream Is Nothing) Then
                Return Nothing
            End If

            CMap = [Get](New bytes.Buffer(cmapResourceStream))
            '}
            Return CMap
        End Function

#End Region
#End Region

#Region "constructors"

        Private Sub New()
        End Sub

#End Region

    End Class

End Namespace