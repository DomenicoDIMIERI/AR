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

Imports DMD.org.dmdpdf.tokens

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>Visitor interface.</summary>
    '  <remarks>Implementations are expected to be functional (traversal results are propagated through
    '  return values rather than side effects) and external (responsibility for traversing the
    '  hierarchical structure is assigned to the 'visit' methods rather than the 'accept' counterparts).
    '  </remarks>
    '*/
    Public Interface IVisitor

        '    /**
        '  <summary>Visits an object stream.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As ObjectStream, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits an object array.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfArray, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a boolean object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfBoolean, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a data object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfDataObject, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a date object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfDate, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits an object dictionary.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfDictionary, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits an indirect object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfIndirectObject, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits an integer-number object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfInteger, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a name object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfName, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a real-number object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfReal, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a reference object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfReference, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a stream object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfStream, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a string object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfString, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a text string object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As PdfTextString, ByVal data As Object) As PdfObject

        '/**
        '  <summary>Visits a cross-reference stream object.</summary>
        '  <param name="object">Visited object.</param>
        '  <param name="data">Supplemental data.</param>
        '  <returns>Result object.</returns>
        '*/
        Function Visit(ByVal obj As XRefStream, ByVal data As Object) As PdfObject

    End Interface

End Namespace
