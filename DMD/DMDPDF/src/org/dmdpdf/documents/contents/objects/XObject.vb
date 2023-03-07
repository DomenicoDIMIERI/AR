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
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>External object shown in a content stream context [PDF:1.6:4.7].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class XObject
        Inherits GraphicsObject
        Implements IResourceReference(Of xObjects.XObject)

#Region "Static"
#Region "fields"

        Public Shared ReadOnly BeginOperatorKeyword As String = PaintXObject.OperatorKeyword
        Public Shared ReadOnly EndOperatorKeyword As String = BeginOperatorKeyword

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal operation As PaintXObject)
            MyBase.new(operation)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the scanner For this Object's contents.</summary>
        '  <param name = "context" > Scanning context.</param>
        '*/
        Public Function GetScanner(ByVal context As ContentScanner) As ContentScanner
            Return Operation.GetScanner(context)
        End Function

#Region "IResourceReference"

        Public Function GetResource(ByVal context As IContentContext) As xObjects.XObject Implements IResourceReference(Of xObjects.XObject).GetResource
            Return Operation.GetResource(context)
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of xObjects.XObject).Name
            Get
                Return Operation.Name
            End Get
            Set(ByVal value As PdfName)
                Operation.Name = value
            End Set
        End Property

#End Region
#End Region

#Region "Private"

        Private ReadOnly Property Operation As PaintXObject
            Get
                Return CType(objects(0), PaintXObject)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
