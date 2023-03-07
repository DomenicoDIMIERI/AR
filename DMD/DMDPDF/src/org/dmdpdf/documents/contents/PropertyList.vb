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

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary>Private information meaningful to the program (application or plugin extension)
    '  creating the marked content [PDF:1.6:10.5.1].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public Class PropertyList
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "static"
#Region "interface"
#Region "public"
        '/**
        '  <summary>Wraps the specified base object into a property list object.</summary>
        '  <param name="baseObject">Base object of a property list object.</param>
        '  <returns>Property list object corresponding to the base object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As PropertyList
            If (baseObject Is Nothing) Then Return Nothing
            Dim type As PdfName = CType(CType(baseObject.Resolve(), PdfDictionary)(PdfName.Type), PdfName)
            If (Layer.TypeName.Equals(type)) Then
                Return Layer.Wrap(baseObject)
            ElseIf (LayerMembership.TypeName.Equals(type)) Then
                Return LayerMembership.Wrap(baseObject)
            Else
                Return New PropertyList(baseObject)
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document, ByVal baseDataObject As PdfDictionary)
            MyBase.New(context, baseDataObject)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#End Region
#End Region

    End Class

End Namespace