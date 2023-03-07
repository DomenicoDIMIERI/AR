'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.navigation.document

    '/**
    '  <summary>Outline item [PDF:1.6:8.2.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Bookmark
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements ILink

#Region "types"
        '/**
        '  <summary>Bookmark flags [PDF:1.6:8.2.2].</summary>
        '*/
        <Flags>
        <PDF(VersionEnum.PDF14)>
        Public Enum FlagsEnum

            '/**
            '  <summary>Display the item in italic.</summary>
            '*/
            Italic = &H1
            '/**
            '  <summary>Display the item in bold.</summary>
            '*/
            Bold = &H2
        End Enum

#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As documents.Document, ByVal title As String)
            MyBase.New(context, New PdfDictionary())
            Me.Title = title
        End Sub

        Public Sub New(ByVal context As documents.Document, ByVal title As String, ByVal destination As LocalDestination)
            Me.New(context, title)
            Me.Destination = destination
        End Sub

        Public Sub New(ByVal context As documents.Document, ByVal title As String, ByVal action As actions.Action)
            Me.New(context, title)
            Me.Action = action
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the child bookmarks.</summary>
        '*/
        Public ReadOnly Property Bookmarks As Bookmarks
            Get
                Return Bookmarks.Wrap(BaseObject)
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the bookmark text color.</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property Color As DeviceRGBColor
            Get
                Return DeviceRGBColor.Get(CType(BaseDataObject(PdfName.C), PdfArray))
            End Get
            Set(ByVal value As DeviceRGBColor)
                If (value Is Nothing) Then
                    BaseDataObject.Remove(PdfName.C)
                Else
                    CheckCompatibility("Color")
                    BaseDataObject(PdfName.C) = value.BaseObject
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether this bookmark's children are displayed.</summary>
        '*/
        Public Property Expanded As Boolean
            Get
                Dim countObject As PdfInteger = CType(BaseDataObject(PdfName.Count), PdfInteger)
                Return (countObject Is Nothing) OrElse (countObject.RawValue >= 0)
            End Get
            Set(ByVal value As Boolean)
                Dim countObject As PdfInteger = CType(BaseDataObject(PdfName.Count), PdfInteger)
                If (countObject Is Nothing) Then Return
                '/*
                '  NOTE: Positive Count entry means open, negative Count entry means closed [PDF:1.6:8.2.2].
                '*/
                If (value) Then
                    BaseDataObject(PdfName.Count) = PdfInteger.Get(1 * Math.Abs(countObject.IntValue))
                Else
                    BaseDataObject(PdfName.Count) = PdfInteger.Get(-1 * Math.Abs(countObject.IntValue))
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the bookmark flags.</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property Flags As FlagsEnum
            Get
                Dim flagsObject As PdfInteger = CType(BaseDataObject(PdfName.F), PdfInteger)
                If (flagsObject Is Nothing) Then Return 0
                Return CType([Enum].ToObject(GetType(FlagsEnum), flagsObject.RawValue), FlagsEnum)
            End Get
            Set(ByVal value As FlagsEnum)
                If (value = 0) Then
                    BaseDataObject.Remove(PdfName.F)
                Else
                    CheckCompatibility(value)
                    BaseDataObject(PdfName.F) = PdfInteger.Get(CInt(value))
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets the parent bookmark.</summary>
        '*/
        Public ReadOnly Property Parent As Bookmark
            Get
                Dim reference As PdfReference = CType(BaseDataObject(PdfName.Parent), PdfReference)
                '// Is its parent a bookmark?
                '/*
                '  NOTE: the Title entry can be used as a flag to distinguish bookmark
                '  (outline item) dictionaries from outline (root) dictionaries.
                '*/
                If (CType(reference.DataObject, PdfDictionary).ContainsKey(PdfName.Title)) Then ' Bookmark.
                    Return New Bookmark(reference)
                Else ' Outline root.
                    Return Nothing ' NO parent bookmark.
                End If
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the text to be displayed for this bookmark.</summary>
        '*/
        Public Property Title As String
            Get
                Return CStr(CType(BaseDataObject(PdfName.Title), PdfTextString).Value)
            End Get
            Set(ByVal value As String)
                BaseDataObject(PdfName.Title) = New PdfTextString(value)
            End Set
        End Property

#Region "ILink"

        Public Property Target As PdfObjectWrapper Implements ILink.Target
            Get
                If (BaseDataObject.ContainsKey(PdfName.Dest)) Then
                    Return Me.Destination
                ElseIf (BaseDataObject.ContainsKey(PdfName.A)) Then
                    Return Me.Action
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As PdfObjectWrapper)
                If (TypeOf (value) Is Destination) Then
                    Me.Destination = CType(value, Destination)
                ElseIf (TypeOf (value) Is actions.Action) Then
                    Me.Action = CType(value, actions.Action)
                Else
                    Throw New ArgumentException("It MUST be either a Destination or an Action.")
                End If
            End Set
        End Property

#End Region
#End Region

#Region "Private"

        Private Property Action As actions.Action
            Get
                Return actions.Action.Wrap(BaseDataObject(PdfName.A))
            End Get
            Set(ByVal value As actions.Action)
                If (value Is Nothing) Then
                    BaseDataObject.Remove(PdfName.A)
                Else
                    '/*
                    '  NOTE:   This entry Is Not permitted in bookmarks if a 'Dest' entry already exists.
                    '*/
                    If (BaseDataObject.ContainsKey(PdfName.Dest)) Then
                        BaseDataObject.Remove(PdfName.Dest)
                    End If
                    BaseDataObject(PdfName.A) = value.BaseObject
                End If
            End Set
        End Property

        Private Property Destination As Destination
            Get
                Dim destinationObject As PdfDirectObject = BaseDataObject(PdfName.Dest)
                If (destinationObject IsNot Nothing) Then
                    Return Document.ResolveName(Of LocalDestination)(destinationObject)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Destination)
                If (value Is Nothing) Then
                    BaseDataObject.Remove(PdfName.Dest)
                Else
                    '/*
                    '  NOTE:   This entry Is Not permitted in bookmarks if an 'A' entry is present.
                    '*/
                    If (BaseDataObject.ContainsKey(PdfName.A)) Then
                        BaseDataObject.Remove(PdfName.A)
                    End If
                    BaseDataObject(PdfName.Dest) = value.NamedBaseObject
                End If
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
