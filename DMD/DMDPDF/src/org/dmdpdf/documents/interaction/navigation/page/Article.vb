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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.interchange.metadata
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.navigation.page

    '/**
    '  <summary>Article thread [PDF:1.7:8.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class Article
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Article
            If (baseObject IsNot Nothing) Then
                Return New Article(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As documents.Document)
            MyBase.New(
                        context,
                        New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.Thread})
                        )
            context.Articles.Add(Me)
        End Sub


        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Deletes Me thread removing also its reference In the document's collection.</summary>
        '*/
        Public Overrides Function Delete() As Boolean
            ' Shallow removal (references):
            ' * reference in document
            Document.Articles.Remove(Me)
            ' Deep removal (indirect object).
            Return MyBase.Delete()
        End Function

        '/**
        '  <summary> Gets the beads associated To Me thread.</summary>
        '*/
        Public ReadOnly Property Elements As ArticleElements
            Get
                Return ArticleElements.Wrap(BaseObject)
            End Get
        End Property

        '/**
        '  <summary> Gets/Sets common article metadata.</summary>
        '*/
        Public Property Information As Information
            Get
                Return Information.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.I))
            End Get
            Set(ByVal value As Information)
                BaseDataObject(PdfName.I) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace