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

Imports System

Namespace DMD.org.dmdpdf.util.math

    '/**
    '  <summary>An interval of comparable objects.</summary>
    '*/
    Public NotInheritable Class Interval(Of T As IComparable(Of T))
        Private _high As T = Nothing
        Private _highInclusive As Boolean
        Private _low As T = Nothing 'default(T);
        Private _lowInclusive As Boolean

        Public Sub New(ByVal _low As T, ByVal _high As T)
            Me.New(_low, _high, True, True)
        End Sub

        Public Sub New(ByVal _low As T, ByVal _high As T, ByVal _lowInclusive As Boolean, ByVal _highInclusive As Boolean)
            Me._low = _low
            Me._high = _high
            Me._lowInclusive = _lowInclusive
            Me._highInclusive = _highInclusive
        End Sub

        '/**
        '  <summary>Gets whether the specified value is contained within this interval.</summary>
        '  <param name="value">Value to check for containment.</param>
        '*/
        Public Function Contains(ByVal value As T) As Boolean
            Dim lowCompare As Integer = -1 : If (_low IsNot Nothing) Then lowCompare = _low.CompareTo(value)
            Dim highCompare As Integer = 1 : If (_high IsNot Nothing) Then highCompare = _high.CompareTo(value)
            Return (lowCompare < 0 OrElse (lowCompare = 0 AndAlso _lowInclusive)) AndAlso
                   (highCompare > 0 OrElse (highCompare = 0 AndAlso _highInclusive))
        End Function

        '/**
        '  <summary>Gets/Sets the higher interval endpoint.</summary>
        '*/
        Public Property High As T
            Get
                Return Me._high
            End Get
            Set(ByVal value As T)
                Me._high = value
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether the higher endpoint Is inclusive.</summary>
        '*/
        Public Property HighInclusive As Boolean
            Get
                Return Me._highInclusive
            End Get
            Set(ByVal value As Boolean)
                Me._highInclusive = value
            End Set
        End Property

        '    /**
        '  <summary>Gets/Sets the lower interval endpoint.</summary>
        '*/
        Public Property Low As T
            Get
                Return Me._low
            End Get
            Set(ByVal value As T)
                Me._low = value
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether the lower endpoint is inclusive.</summary>
        '*/
        Public Property LowInclusive As Boolean
            Get
                Return Me._lowInclusive
            End Get
            Set(ByVal value As Boolean)
                Me._lowInclusive = value
            End Set
        End Property

    End Class

End Namespace

