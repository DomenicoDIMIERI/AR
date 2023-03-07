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

Imports DMD.org.dmdpdf.objects

Imports System
Imports System.IO

Namespace DMD.org.dmdpdf.documents.files

    '/**
    '  <summary>Simple reference to the contents of another file [PDF:1.6:3.10.2].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class SimpleFileSpecification
        Inherits FileSpecification

#Region "dynamic"
#Region "constructors"

        Friend Sub New(ByVal context As Document, ByVal path As String)
            MyBase.new(context, New PdfString(path))
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides ReadOnly Property Path As String
            Get
                Return CStr(CType(Me.BaseDataObject, PdfString).Value)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
