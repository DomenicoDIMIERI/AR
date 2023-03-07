'/*
'  Copyright 2009-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Set the specified graphics state parameters' operation [PDF:1.6:4.3.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ApplyExtGState
        Inherits Operation
        Implements IResourceReference(Of ExtGState)

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "gs"

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

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the <see cref="ExtGState">graphics state parameters</see> resource to be set.
        '  </summary>
        '  <param name="context">Content context.</param>
        '*/
        Public Function GetExtGState(ByVal context As IContentContext) As ExtGState
            Return GetResource(context)
        End Function

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim extGState As ExtGState = GetExtGState(state.Scanner.ContentContext)
            extGState.ApplyTo(state)
        End Sub

#Region "IResourceReference"

        Public Function GetResource(ByVal context As IContentContext) As ExtGState Implements IResourceReference(Of ExtGState).GetResource
            Return context.Resources.ExtGStates(Name)
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of ExtGState).Name
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
#End Region

    End Class
End Namespace