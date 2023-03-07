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
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Marked-content sequence [PDF:1.6:10.5].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class MarkedContent
        Inherits ContainerObject

#Region "Static"
#Region "fields"

        Public Shared ReadOnly EndOperatorKeyword As String = EndMarkedContent.OperatorKeyword

        Private Shared ReadOnly EndChunk As Byte() = Encoding.Pdf.Encode(EndOperatorKeyword + Symbol.LineFeed)

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _header As BeginMarkedContent

#End Region

#Region "constructors"

        Public Sub New(ByVal header As BeginMarkedContent)
            Me.New(header, New List(Of ContentObject))
        End Sub

        Public Sub New(ByVal header As BeginMarkedContent, ByVal objects As IList(Of ContentObject))
            MyBase.new(objects)
            Me._header = header
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets information about this marked-content sequence.</summary>
        '*/
        Public Overrides Property Header As Operation
            Get
                Return Me._header
            End Get
            Set(ByVal value As Operation)
                Me._header = CType(value, BeginMarkedContent)
            End Set
        End Property

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)
            Me._header.WriteTo(stream, context)
            MyBase.WriteTo(stream, context)
            stream.Write(EndChunk)
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace