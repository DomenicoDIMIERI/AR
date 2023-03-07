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

imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.objects

    ''' <summary>
    ''' Composite dictionary.
    ''' </summary>
    ''' <typeparam name="TKey"></typeparam>
    Public Interface ICompositeDictionary(Of TKey)

        ''' <summary>
        ''' Gets the value dictionary associated To the specified type.
        ''' </summary>
        ''' <param name="type"></param>
        ''' <returns></returns>
        Function [Get](ByVal type As Type) As PdfObjectWrapper

        ''' <summary>
        ''' Gets the value associated To the specified key For the specified type.
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="key"></param>
        ''' <returns></returns>
        Function [Get](ByVal type As Type, ByVal key As TKey) As PdfObjectWrapper

    End Interface

End Namespace
