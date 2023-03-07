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
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>Name tree [PDF:1.7:3.8.5].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class NameTree(Of TValue As PdfObjectWrapper)
        Inherits Tree(Of PdfString, TValue)

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document)
            MyBase.New(context)
        End Sub


        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overrides ReadOnly Property PairsKey As PdfName
            Get
                Return PdfName.Names
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace