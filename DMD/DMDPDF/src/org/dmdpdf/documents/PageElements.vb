'/*
'  Copyright 2012-2013 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.collections.generic

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary>Page elements.</summary>
    '*/
    Public MustInherit Class PageElements(Of TItem As PdfObjectWrapper(Of PdfDictionary))
        Inherits Array(Of TItem)

#Region "dynamic"
#Region "fields"
        Private _page As Page
#End Region

#Region "constructors"

        Friend Sub New(ByVal baseObject As PdfDirectObject, ByVal page As Page)
            MyBase.new(baseObject)
            Me._page = page
        End Sub

#End Region

#Region "Interface"
#Region "Public"
        Public Overrides Sub Add(ByVal [object] As TItem)
            DoAdd([object])
            MyBase.Add([object])
        End Sub

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotSupportedException()
        End Function

        Public Overrides Sub Insert(ByVal Index As Integer, ByVal [object] As TItem)
            DoAdd([object])
            MyBase.Insert(Index, [object])
        End Sub

        '/**
        '  <summary> Gets the page associated To these elements.</summary>
        '*/
        Public ReadOnly Property Page As Page
            Get
                Return Me._page
            End Get
        End Property

        Public Overrides Sub RemoveAt(ByVal Index As Integer)
            Dim [Object] As TItem = Me(Index)
            MyBase.RemoveAt(Index)
            DoRemove([Object])
        End Sub

        Public Overrides Function Remove(ByVal [Object] As TItem) As Boolean
            If (Not MyBase.Remove([Object])) Then Return False
            DoRemove(CType([Object], TItem))
            Return True
        End Function

#End Region

#Region "Private"

        Private Sub DoAdd(ByVal [Object] As TItem)
            ' Link the element to its page!
            [Object].BaseDataObject(PdfName.P) = _page.BaseObject
        End Sub

        Private Sub DoRemove(ByVal [Object] As TItem)
            ' Unlink the element from its page!
            [Object].BaseDataObject.Remove(PdfName.P)
        End Sub

#End Region
#End Region
#End Region

    End Class


End Namespace
