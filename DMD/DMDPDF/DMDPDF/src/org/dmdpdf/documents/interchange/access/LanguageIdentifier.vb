'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.interaction
'Imports DMD.actions = org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interchange.access

    '/**
    '  <summary>Language identifier [PDF:1.7:10.8.1][RFC 3066].</summary>
    '  <remarks>
    '    <para>Language identifiers can be based on codes defined by the International Organization for
    '    Standardization in ISO 639 (language code) and ISO 3166 (country code) or registered with the
    '    Internet Assigned Numbers Authority (<a href="http://iana.org">IANA</a>), or they can include
    '    codes created for private use.</para>
    '    <para>A language identifier consists of a primary code optionally followed by one or more
    '    subcodes (each preceded by a hyphen).</para>
    '  </remarks>
    '*/
    <PDF(VersionEnum.PDF14)>
    Public NotInheritable Class LanguageIdentifier
        Inherits PdfObjectWrapper(Of PdfTextString)

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary>Wraps a language identifier base object into a language identifier object.</summary>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As LanguageIdentifier
            If (baseObject Is Nothing) Then Return Nothing
            If (TypeOf (baseObject.Resolve()) Is PdfTextString) Then
                Return New LanguageIdentifier(baseObject)
            Else
                Throw New ArgumentException("It doesn't represent a valid language identifier object.", "baseObject")
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ParamArray components As String())
            Me.New(New List(Of String)(components))
        End Sub

        Public Sub New(ByVal components As IList(Of String))
            Me.Components = components
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the identifier components:
        '    <list type="number">
        '      <item>the first one is the primary code. It can be any of the following:
        '        <list type="bullet">
        '          <item>a 2-character ISO 639 language code (e.g., <code>en</code> for English);</item>
        '          <item>the letter <code>i</code>, designating an IANA-registered identifier;</item>
        '          <item>the letter <code>x</code>, for private use;</item>
        '        </list>
        '      </item>
        '      <item>the second one is the first subcode. It can be any of the following:
        '        <list type="bullet">
        '          <item>a 2-character ISO 3166 country code (e.g., <code>en-US</code>);</item>
        '          <item>a 3-to-8-character subcode registered with IANA (e.g., <code>en-cockney</code>)</item>
        '          <item>private non-registered subcodes;</item>
        '        </list>
        '      </item>
        '      <item>subcodes beyond the first can be any that have been registered with IANA.</item>
        '    </list>
        '  </summary>
        '*/
        Public Property Components As IList(Of String)
            Get
                Return New List(Of String)(BaseDataObject.StringValue.Split("-"c))
            End Get
            Set(ByVal value As IList(Of String))
                Me.BaseObject = New PdfTextString(String.Join("-", value))
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace