'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Namespace DMD.org.dmdpdf.objects

    ''' <summary>
    ''' PDF number interface.
    ''' </summary>
    Public Interface IPdfNumber
        Inherits IPdfSimpleObject(Of Double)

        ''' <summary>
        ''' Gets the double-precision floating-point representation of the value.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property DoubleValue As Double

        ''' <summary>
        ''' Gets the floating-point representation of the value.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property FloatValue As Single

        ''' <summary>
        ''' Gets the integer representation of the value.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IntValue As Integer

    End Interface


    Friend Class PdfNumber

        Public Shared Function Compare(ByVal obj1 As Object, ByVal obj2 As Object) As Integer
            If (Not TypeOf (obj1) Is IPdfNumber) Then Throw New ArgumentException("obj1 MUST implement IPdfNumber")
            If (Not TypeOf (obj2) Is IPdfNumber) Then Throw New ArgumentException("obj2 MUST implement IPdfNumber")
            Return CType(obj1, IPdfNumber).RawValue.CompareTo(CType(obj2, IPdfNumber).RawValue)
        End Function

        Public Shared Function Equal(ByVal obj1 As Object, ByVal obj2 As Object) As Boolean
            If (Not TypeOf (obj1) Is IPdfNumber) Then Throw New ArgumentException("obj1 MUST implement IPdfNumber")
            If (Not TypeOf (obj2) Is IPdfNumber) Then Throw New ArgumentException("obj2 MUST implement IPdfNumber")
            Return CType(obj1, IPdfNumber).RawValue.Equals(CType(obj2, IPdfNumber).RawValue)
        End Function

        Public Shared Shadows Function GetHashCode(ByVal obj As Object) As Integer
            If (Not TypeOf (obj) Is IPdfNumber) Then Throw New ArgumentException("obj MUST implement IPdfNumber")
            Dim value As Double = CType(obj, IPdfNumber).RawValue
            Dim intValue As Integer = CInt(value)
            If (value = intValue) Then
                Return intValue.GetHashCode()
            Else
                Return value.GetHashCode()
            End If
        End Function

    End Class

End Namespace

