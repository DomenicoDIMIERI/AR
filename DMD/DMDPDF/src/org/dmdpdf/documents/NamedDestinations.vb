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
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary>Named destinations [PDF:1.6:3.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class NamedDestinations
        Inherits NameTree(Of Destination)

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.new(context)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overrides Function WrapValue(ByVal baseObject As PdfDirectObject) As Destination
            '/*
            '  NOTE: A named destination may be either an array defining the destination,
            '  or a dictionary with a D entry whose value is such an array [PDF:1.6:8.2.1].
            '*/
            Dim destinationObject As PdfDirectObject
            Dim baseDataObject As PdfDataObject = PdfObject.Resolve(baseObject)
            If (TypeOf (baseDataObject) Is PdfDictionary) Then
                destinationObject = CType(baseDataObject, PdfDictionary)(PdfName.D)
            Else
                destinationObject = baseObject
            End If

            Return Destination.Wrap(destinationObject)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
