'/*
'  Copyright 2008-2010 Stefano Chizzolini. http://www.dmdpdf.org

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
    '  <summary>'Begin marked-content sequence' operation [PDF:1.6:10.5].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class BeginMarkedContent
        Inherits ContentMarker

#Region "Static"
#Region "fields"

        Public Shared ReadOnly PropertyListOperatorKeyword As String = "BDC"
        Public Shared ReadOnly SimpleOperatorKeyword As String = "BMC"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal tag As PdfName)
            MyBase.New(tag)
        End Sub

        Public Sub New(ByVal tag As PdfName, ByVal properties As PdfDirectObject)
            MyBase.New(tag, properties)
        End Sub

        Friend Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(operator_, operands)
        End Sub

#End Region

#Region "Interface"
#Region "Protected"

        Protected Overrides ReadOnly Property PropertyListOperator As String
            Get
                Return PropertyListOperatorKeyword
            End Get
        End Property

        Protected Overrides ReadOnly Property SimpleOperator As String
            Get
                Return SimpleOperatorKeyword
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace