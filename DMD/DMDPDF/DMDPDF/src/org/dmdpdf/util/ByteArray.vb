'/*
'  Copyright 2009-2010 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports System.Linq
Imports System.Text

Namespace DMD.org.dmdpdf.util

    '/**
    '  <summary>Byte array.</summary>
    '*/
    '/*
    '  NOTE: This class is useful when applied as key for dictionaries using the default IEqualityComparer.
    '*/
    Public Structure ByteArray

        Public ReadOnly Data As Byte() ' //TODO: yes, I know it's risky (temporary simplification)...

        Public Sub New(ByVal data As Byte())
            Me.Data = New Byte(data.Length - 1) {}
            Array.Copy(data, Me.Data, data.Length)
        End Sub

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return TypeOf (obj) Is ByteArray AndAlso Me.Data.SequenceEqual(CType(obj, ByteArray).Data)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Dim hashCode As Integer = 0
            'For (int index = 0, length = Data.Length; index < length; index++)
            Dim length As Integer = Me.Data.Length
            For index As Integer = 0 To length - 1
                hashCode = hashCode Xor Me.Data(index) << (8 * (index Mod 4))
            Next
            Return hashCode
        End Function

        Public Overrides Function ToString() As String
            Dim builder As New StringBuilder("[")
            For Each datum As Byte In Me.Data
                If (builder.Length > 1) Then builder.Append(",")
                builder.Append(datum And &HFF)
            Next
            builder.Append("]")
            Return builder.ToString()
        End Function

    End Structure


End Namespace

