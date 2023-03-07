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
    '  <summary>'Paint the specified XObject' operation [PDF:1.6:4.7].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class PaintXObject
        Inherits Operation
        Implements IResourceReference(Of xObjects.XObject)

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "Do"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal name As PdfName)
            MyBase.New(OperatorKeyword, name)
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region
#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the scanner for the contents of the painted external object.</summary>
        '  <param name="context">Scanning context.</param>
        '*/
        Public Function GetScanner(ByVal context As ContentScanner) As ContentScanner
            Dim xObject As xObjects.XObject = GetXObject(context.ContentContext)
            If (TypeOf (xObject) Is xObjects.FormXObject) Then
                Return New ContentScanner(CType(xObject, xObjects.FormXObject), context)
            Else
                Return Nothing
            End If
        End Function

        '/**
        '  <summary>Gets the <see cref="xObjects.XObject">external Object</see> resource To be painted.
        '  </summary>
        '  <param name = "context" > Content context.</param>
        '*/
        Public Function GetXObject(ByVal context As IContentContext) As xObjects.XObject
            Return GetResource(context)
        End Function

#Region "IResourceReference"

        Public Function GetResource(ByVal context As IContentContext) As xObjects.XObject Implements IResourceReference(Of xObjects.XObject).GetResource
            Return context.Resources.XObjects(Name)
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of xObjects.XObject).Name
            Get
                Return CType(Me._operands(0), PdfName)
            End Get
            Set(ByVal value As PdfName)
                Me._operands(0) = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace