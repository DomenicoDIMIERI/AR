'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Shading object [PDF:1.6:4.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class Shading
        Inherits GraphicsObject
        Implements IResourceReference(Of colorSpaces.Shading)

#Region "shared "
#Region "fields"

        Public Shared ReadOnly BeginOperatorKeyword As String = PaintShading.OperatorKeyword
        Public Shared ReadOnly EndOperatorKeyword As String = BeginOperatorKeyword

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal operation As PaintShading)
            MyBase.New(operation)
        End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "IResourceReference"

        Public Function GetResource(ByVal context As IContentContext) As colorSpaces.Shading Implements IResourceReference(Of colorSpaces.Shading).GetResource
            Return Me.Operation.GetResource(context)
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of colorSpaces.Shading).Name
            Get
                Return Operation.Name
            End Get
            Set(ByVal value As PdfName)
                Me.Operation.Name = value
            End Set
        End Property

#End Region
#End Region

#Region "Private"

        Private ReadOnly Property Operation As PaintShading
            Get
                Return CType(Me.Objects(0), PaintShading)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace