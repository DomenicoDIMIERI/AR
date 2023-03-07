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

Namespace DMD.org.dmdpdf.documents.multimedia

    'TODO: this is just a stub.
    '/**
    '  <summary>Sound object [PDF:1.6:9.2].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class Sound
        Inherits PdfObjectWrapper(Of PdfStream)

#Region "dynamic"
#Region "constructors"
        '/**
        '  <summary>Creates a new sound within the given document context.</summary>
        '*/
        Public Sub New(ByVal context As Document, ByVal stream As IInputStream)
            MyBase.New(context, New PdfStream(New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.Sound})))
            Throw New NotImplementedException("Process the sound stream!")
        End Sub

        Public Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#End Region
#End Region
    End Class

End Namespace