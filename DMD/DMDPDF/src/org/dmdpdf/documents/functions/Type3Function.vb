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

Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.functions

    '/**
    '  <summary>Stitching function producing a single new 1-input function from the combination of the
    '  subdomains of <see cref="Functions">several 1-input functions</see> [PDF:1.6:3.9.3].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class Type3Function
        Inherits [Function]

#Region "dynamic"
#Region "constructors"

        'TODO: implement function creation!

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Calculate(inputs() As Double) As Double()
            ' FIXME: Auto-generated method stub
            Return Nothing
        End Function

        '/**
        '  <summary> Gets the <see cref="Domains">domain</see> partition bounds whose resulting intervals
        '  are respectively applied To Each <see cref="Functions">Function</see>.</summary>
        '*/
        Public ReadOnly Property DomainBounds As IList(Of Double)
            Get
                Dim _domainBounds As IList(Of Double) = New List(Of Double)()
                '{
                Dim domainBoundsObject As PdfArray = CType(Dictionary.Resolve(PdfName.Bounds), PdfArray)
                For Each domainBoundObject As PdfDirectObject In domainBoundsObject
                    _domainBounds.Add(CType(domainBoundObject, IPdfNumber).RawValue)
                Next
                '}
                Return _domainBounds
            End Get
        End Property

        '/**
        '  <summary> Gets the mapping Of Each <see cref="DomainBounds">subdomain</see> into the domain
        '  of the corresponding <see cref="Functions">function</see>.</summary>
        '*/
        Public ReadOnly Property DomainEncodes As IList(Of Interval(Of Double))
            Get
                Return GetIntervals(Of Double)(PdfName.Encode, Nothing)
            End Get
        End Property

        '/**
        '  <summary> Gets the 1-input functions making up this stitching Function.</summary>
        '  <remarks> The output dimensionality Of all functions must be the same.</remarks>
        '*/
        Public ReadOnly Property Functions As functions
            Get
                Return New functions(Dictionary(PdfName.Functions), Me)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace