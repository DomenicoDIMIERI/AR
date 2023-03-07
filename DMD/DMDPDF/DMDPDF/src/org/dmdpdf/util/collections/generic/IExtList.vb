'/*
'  Copyright 2006 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.util.collections.generic

    '/**
    '  <summary>Extension list interface.</summary>
    '*/
    Public Interface IExtList(Of T)
        Inherits IExtCollection(Of T)

        '/**
        '  <summary>Creates a shallow copy of a range of items in the source list.</summary>
        '  <param name="index">Lower item index (inclusive) of the copy.</param>
        '  <param name="count">Number of items to copy.</param>
        '  <returns>Shallow copy of the specified range of items.</returns>
        '*/
        Function GetRange(ByVal index As Integer, ByVal count As Integer) As IList(Of T)

        '/**
        '  <summary>Creates a shallow copy of a slice of items in the source list.</summary>
        '  <param name="fromIndex">Lower item (inclusive) of the copy.</param>
        '  <param name="toIndex">Higher item (exclusive) of the copy.</param>
        '  <returns>Shallow copy of the specified slice of items.</returns>
        '*/
        Function GetSlice(ByVal fromIndex As Integer, ByVal toIndex As Integer) As IList(Of T)

        '/**
        '  <summary>Inserts all of the specified-collection's items at the specified position
        '  within the list.</summary>
        '  <param name="index">Insertion position.</param>
        '  <param name="items">Collection of items to insert.</param>
        '*/
        Sub InsertAll(Of TVar As T)(ByVal index As Integer, ByVal items As ICollection(Of TVar))

    End Interface

End Namespace
