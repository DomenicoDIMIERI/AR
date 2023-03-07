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

Imports DMD.org.dmdpdf
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.interaction
'Using actions = org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interchange.access
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Rendition [PDF:1.7:9.1.2].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public MustInherit Class Rendition
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IPdfNamedObjectWrapper

#Region "types"

        '/**
        '  <summary>Rendition viability [PDF:1.7:9.1.2].</summary>
        '*/
        Public Class Viability
            Inherits PdfObjectWrapper(Of PdfDictionary)

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

            '/**
            '  <summary> Gets the minimum system's bandwidth (in bits per second).</summary>
            '  <remarks> Equivalent To SMIL's systemBitrate attribute.</remarks>
            '*/
            Public ReadOnly Property Bandwidth As Integer?
                Get
                    Return CType(PdfInteger.GetValue(MediaCriteria(PdfName.R)), Integer?)
                End Get
            End Property

            '/**
            '  <summary> Gets the minimum screen color depth (In bits per pixel).</summary>
            '  <remarks> Equivalent To SMIL's systemScreenDepth attribute.</remarks>
            '*/
            Public ReadOnly Property ScreenDepth As Integer?
                Get
                    Dim screenDepthObject As PdfDictionary = CType(MediaCriteria(PdfName.D), PdfDictionary)
                    If (screenDepthObject IsNot Nothing) Then
                        Return CType(screenDepthObject(PdfName.V), PdfInteger).IntValue
                    Else
                        Return Nothing
                    End If
                End Get
            End Property

            '/**
            '  <summary> Gets the minimum screen size (In pixels).</summary>
            '  <remarks> Equivalent To SMIL's systemScreenSize attribute.</remarks>
            '*/
            Public ReadOnly Property ScreenSize As Size?
                Get
                    Dim screenSizeObject As PdfDictionary = CType(MediaCriteria(PdfName.Z), PdfDictionary)
                    If (screenSizeObject Is Nothing) Then Return Nothing
                    Dim screenSizeValueObject As PdfArray = CType(screenSizeObject(PdfName.V), PdfArray)
                    If (screenSizeValueObject IsNot Nothing) Then
                        Return New Size(
                                CType(screenSizeValueObject(0), PdfInteger).IntValue,
                                CType(screenSizeValueObject(1), PdfInteger).IntValue
                                )
                    Else
                        Return Nothing
                    End If
                End Get
            End Property

            '/**
            '  <summary> Gets the list Of supported viewer applications.</summary>
            '*/
            Public ReadOnly Property Renderers As Array(Of SoftwareIdentifier)
                Get
                    Return Array(Of SoftwareIdentifier).Wrap(Of SoftwareIdentifier)(MediaCriteria.Get(Of PdfArray)(PdfName.V))
                End Get
            End Property

            '/**
            '  <summary> Gets the PDF version range supported by the viewer application.</summary>
            '*/
            Public ReadOnly Property Version As Interval(Of Version)
                Get
                    Dim pdfVersionArray As PdfArray = CType(MediaCriteria(PdfName.P), PdfArray)
                    If (pdfVersionArray IsNot Nothing AndAlso pdfVersionArray.Count > 0) Then
                        If (pdfVersionArray.Count > 1) Then
                            Return New Interval(Of Version)(
                                        org.dmdpdf.Version.Get(CType(pdfVersionArray(0), PdfName)),
                                        org.dmdpdf.Version.Get(CType(pdfVersionArray(1), PdfName))
                                         )
                        Else
                            Return New Interval(Of Version)(
                                        org.dmdpdf.Version.Get(CType(pdfVersionArray(0), PdfName)),
                                        Nothing
                                        )

                        End If
                    Else
                        Return Nothing
                    End If
                End Get
            End Property

            '/**
            '  <summary> Gets the list Of supported languages.</summary>
            '  <remarks> Equivalent To SMIL's systemLanguage attribute.</remarks>
            '*/
            Public ReadOnly Property Languages As IList(Of LanguageIdentifier)
                Get
                    Dim _languages As IList(Of LanguageIdentifier) = New List(Of LanguageIdentifier)()
                    Dim languagesObject As PdfArray = CType(MediaCriteria(PdfName.L), PdfArray)
                    If (languagesObject IsNot Nothing) Then
                        For Each languageObject As PdfDirectObject In languagesObject
                            _languages.Add(LanguageIdentifier.Wrap(languageObject))
                        Next
                    End If
                    Return _languages
                End Get
            End Property

            '/**
            '  <summary> Gets whether To hear audio descriptions.</summary>
            '  <remarks> Equivalent To SMIL's systemAudioDesc attribute.</remarks>
            '*/
            Public ReadOnly Property AudioDescriptionEnabled As Boolean
                Get
                    Return CBool(PdfBoolean.GetValue(MediaCriteria(PdfName.A)))
                End Get
            End Property

            '/**
            '  <summary> Gets whether To hear audio overdubs.</summary>
            '*/
            Public ReadOnly Property AudioOverdubEnabled As Boolean
                Get
                    Return CBool(PdfBoolean.GetValue(MediaCriteria(PdfName.O)))
                End Get
            End Property

            '/**
            '  <summary> Gets whether To see subtitles.</summary>
            '*/
            Public ReadOnly Property SubtitleEnabled As Boolean
                Get
                    Return CBool(PdfBoolean.GetValue(MediaCriteria(PdfName.S)))
                End Get
            End Property

            '/**
            '  <summary> Gets whether To see text captions.</summary>
            '  <remarks> Equivalent To SMIL's systemCaptions attribute.</remarks>
            '*/
            Public ReadOnly Property TextCaptionEnabled As Boolean
                Get
                    Return CBool(PdfBoolean.GetValue(MediaCriteria(PdfName.C)))
                End Get
            End Property

            Private ReadOnly Property MediaCriteria As PdfDictionary
                Get
                    Return BaseDataObject.Resolve(Of PdfDictionary)(PdfName.C)
                End Get
            End Property

            'TODOsetters!
        End Class

#End Region

#Region "static"
#Region "interface"
#Region "public"

        '    /**
        '  <summary> Wraps a rendition base Object into a rendition Object.</summary>
        '  <param name = "baseObject" > Rendition base Object.</param>
        '  <returns> Rendition Object associated To the base Object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Rendition
            If (baseObject Is Nothing) Then Return Nothing
            Dim subtype As PdfName = CType(CType(baseObject.Resolve(), PdfDictionary)(PdfName.S), PdfName)
            If (PdfName.MR.Equals(subtype)) Then
                Return New MediaRendition(baseObject)
            ElseIf (PdfName.SR.Equals(subtype)) Then
                Return New SelectorRendition(baseObject)
            Else
                Throw New ArgumentException("It doesn't represent a valid clip object.", "baseObject")
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal subtype As PdfName)
            MyBase.New(
                    context, New PdfDictionary(
                                New PdfName() {PdfName.Type, PdfName.S},
                                New PdfDirectObject() {PdfName.Rendition, subtype}
                                        )
                       )
        End Sub


        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the preferred options the renderer should attempt To honor without affecting
        '  its viability [PDF:1.7:9.1.1].</summary>
        '*/
        Public Property Preferences As Viability
            Get
                Return New Viability(Me.BaseDataObject.Get(Of PdfDictionary)(PdfName.BE))
            End Get
            Set(ByVal value As Viability)
                Me.BaseDataObject(PdfName.BE) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the minimum requirements the renderer must honor In order To be considered
        '  viable [PDF:1.7:9.1.1].</summary>
        '*/
        Public Property Requirements As Viability
            Get
                Return New Viability(BaseDataObject.Get(Of PdfDictionary)(PdfName.MH))
            End Get
            Set(ByVal value As Viability)
                Me.BaseDataObject(PdfName.MH) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

#Region "IPdfNamedObjectWrapper"

        Public ReadOnly Property Name As PdfString Implements IPdfNamedObjectWrapper.Name
            Get
                Return Me.RetrieveName()
            End Get
        End Property

        Public ReadOnly Property NamedBaseObject As PdfDirectObject Implements IPdfNamedObjectWrapper.NamedBaseObject
            Get
                Return Me.RetrieveNamedBaseObject()
            End Get
        End Property

#End Region
#End Region

#Region "Protected"

        Protected Overrides Function RetrieveName() As PdfString
            '/*
            '  NOTE: A rendition dictionary is not required to have a name tree entry. When it does, the
            '  viewer application should ensure that the name specified in the tree is kept the same as the
            '  value of the N entry (for example, if the user interface allows the name to be changed).
            '*/
            Return CType(BaseDataObject(PdfName.N), PdfString)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace