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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Namespace DMD.org.dmdpdf.tools

    '/**
    '  <summary>Tool for content insertion into existing pages.</summary>
    '*/
    Public NotInheritable Class PageStamper

#Region "dynamic"
#Region "fields"
        Private _page As Page

        Private _background As PrimitiveComposer
        Private _foreground As PrimitiveComposer

#End Region

#Region "constructors"

        Public Sub New()
            Me.New(Nothing)
        End Sub


        Public Sub New(ByVal page As Page)
            Me.Page = page
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Sub Flush()
            '// Ensuring that there's room for the new content chunks inside the page's content stream...
            '/*
            '  NOTE: This specialized stamper Is optimized for content insertion without modifying
            '  existing content representations, leveraging the peculiar feature of page structures
            '  to express their content streams as arrays of data streams.
            '*/
            Dim streams As PdfArray
            Dim contentsObject As PdfDirectObject = _page.BaseDataObject(PdfName.Contents)
            Dim contentsDataObject As PdfDataObject = PdfObject.Resolve(contentsObject)
            ' Single data stream?
            If (TypeOf (contentsDataObject) Is PdfStream) Then
                '/*
                '  NOTE: Content stream MUST be expressed as an array of data streams in order to host
                '  background- And foreground - stamped contents.
                '*/
                streams = New PdfArray()
                streams.Add(contentsObject)
                _page.BaseDataObject(PdfName.Contents) = streams
            Else
                streams = CType(contentsDataObject, PdfArray)
            End If

            ' Background.
            ' Serialize the content!
            _background.Flush()
            ' Insert the serialized content into the page's content stream!
            streams.Insert(0, _background.Scanner.Contents.BaseObject)

            ' Foreground.
            ' Serialize the content!
            _foreground.Flush()
            ' Append the serialized content into the page's content stream!
            streams.Add(_foreground.Scanner.Contents.BaseObject)
        End Sub

        Public ReadOnly Property Background As PrimitiveComposer
            Get
                Return Me._background
            End Get
        End Property

        Public ReadOnly Property Foreground As PrimitiveComposer
            Get
                Return Me._foreground
            End Get
        End Property

        Public Property Page As Page
            Get
                Return Me._page
            End Get
            Set(value As Page)
                Me._page = value
                If (Me._page Is Nothing) Then
                    Me._background = Nothing
                    Me._foreground = Nothing
                Else
                    ' Background.
                    _background = CreateFilter()
                    ' Open the background local state!
                    _background.Add(SaveGraphicsState.Value)
                    ' Close the background local state!
                    _background.Add(RestoreGraphicsState.Value)
                    ' Open the middleground local state!
                    _background.Add(SaveGraphicsState.Value)
                    ' Move into the background!
                    _background.Scanner.Move(1)

                    ' Foregrond.
                    _foreground = CreateFilter()
                    ' Close the middleground local state!
                    _foreground.Add(RestoreGraphicsState.Value)
                End If
            End Set
        End Property

#End Region

#Region "Private"

        Private Function CreateFilter() As PrimitiveComposer
            Return New PrimitiveComposer(
                            New ContentScanner(
                                contents.Wrap(_page.File.Register(New PdfStream()), _page)
                                )
                        )
        End Function

#End Region
#End Region
#End Region

    End Class


End Namespace

