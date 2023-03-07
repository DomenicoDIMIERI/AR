'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.multimedia
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary>Named renditions [PDF:1.6:3.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class NamedRenditions
        Inherits NameTree(Of Rendition)

#Region "dynamic"
#Region "constructors"
        Public Sub New(ByVal context As Document)
            MyBase.new(context)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Remove(ByVal key As PdfString) As Boolean
            Dim oldValue As Rendition = Me(key)
            Dim removed As Boolean = MyBase.Remove(key)
            UpdateName(oldValue, Nothing)
            Return removed
        End Function

        Default Public Overrides Property Item(ByVal key As PdfString) As Rendition
            Get
                Return MyBase.Item(key)
            End Get
            Set(ByVal value As Rendition)
                Dim oldValue As Rendition = MyBase.Item(key)
                MyBase.Item(key) = value
                UpdateName(oldValue, Nothing)
                UpdateName(value, key)
            End Set
        End Property

#End Region

#Region "Protected"

        Protected Overrides Function WrapValue(ByVal baseObject As PdfDirectObject) As Rendition
            Return Rendition.Wrap(baseObject)
        End Function

#End Region

#Region "private"

        '/**
        '  <summary>Ensures name reference synchronization for the specified rendition [PDF:1.7:9.1.2].
        '  </summary>
        '*/
        Private Sub UpdateName(ByVal rendition As Rendition, ByVal name As PdfString)
            If (rendition Is Nothing) Then Return

            rendition.BaseDataObject(PdfName.N) = name
        End Sub

#End Region
#End Region
#End Region
    End Class

End Namespace
