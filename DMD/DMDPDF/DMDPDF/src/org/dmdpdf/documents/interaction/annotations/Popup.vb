'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Pop-up annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It displays text in a pop-up window for entry and editing.
    '  It typically does not appear alone but is associated with a markup annotation,
    '  its parent annotation, and is used for editing the parent's text.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class Popup
        Inherits Annotation

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String)
            MyBase.New(page, PdfName.Popup, box, text)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets whether the annotation should initially be displayed open.</summary>
        '*/
        Public Property IsOpen As Boolean
            Get
                Dim openObject As PdfBoolean = CType(BaseDataObject(PdfName.Open), PdfBoolean)
                If (openObject IsNot Nothing) Then
                    Return openObject.BooleanValue
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.Open) = PdfBoolean.Get(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the parent annotation.</summary>
        '*/
        Public Property Parent As Annotation
            Get
                Return Annotation.Wrap(BaseDataObject(PdfName.Parent))
            End Get
            Set(ByVal value As Annotation)
                BaseDataObject(PdfName.Parent) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace