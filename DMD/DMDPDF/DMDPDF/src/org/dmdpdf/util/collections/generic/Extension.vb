'/*
'  Copyright 2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.util.collections.generic

    Public Module Extension 'Static 

        <Extension>
        Public Sub AddAll(Of T)(ByVal collection As ICollection(Of T), ByVal enumerable As IEnumerable(Of T)) 'this collection
            For Each item As T In enumerable
                collection.Add(item)
            Next
        End Sub


        'Public Sub RemoveAll(of T)( this ICollection<T> collection, IEnumerable<T> enumerable )
        <Extension>
        Public Sub RemoveAll(Of T)(ByVal collection As ICollection(Of T), enumerable As IEnumerable(Of T))
            For Each item As T In enumerable
                collection.Remove(item)
            Next
        End Sub

        '/**
        '  <summary>Sets all the specified entries into this dictionary.</summary>
        '  <remarks>The effect of this call is equivalent to that of calling the indexer on this dictionary
        '  once for each entry in the specified enumerable.</remarks>
        '*/
        'Public shared sub  SetAll(of TKey,TValue) (this IDictionary<TKey,TValue> dictionary, IEnumerable<KeyValuePair<TKey,TValue>> enumerable )
        <Extension>
        Public Sub SetAll(Of TKey, TValue)(ByVal dictionary As IDictionary(Of TKey, TValue), ByVal enumerable As IEnumerable(Of KeyValuePair(Of TKey, TValue)))
            For Each entry As KeyValuePair(Of TKey, TValue) In enumerable
                dictionary(entry.Key) = entry.Value
            Next
        End Sub

    End Module

End Namespace


