'/*
'  Copyright 2009-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
    '  <summary>Specialized math operations.</summary>
    '*/
    Public NotInheritable Class OperationUtils

#Region "static"
#Region "fields"

        ''' <summary>
        ''' Double-precision floating-point exponent bias
        ''' </summary>
        Private Const DoubleExponentBias As Integer = 1023

        ''' <summary>
        ''' Double-precision floating-point exponent field bit mask.
        ''' </summary>
        Private Const DoubleExponentBitMask As Long = &H7FF0000000000000L
        '/**
        '  <summary>Double-precision floating-point significand bit count, excluding the implicit one.
        '  </summary>
        '*/
        Private Const DoubleSignificandBitCount As Integer = 52

        '/**
        '  <summary>Default relative floating-point precision error tolerance.</summary>
        '*/
        Private Const Epsilon As Double = 0.000001

#End Region

#Region "interface"
        '/**
        '  <summary>Compares double-precision floating-point numbers applying the default error tolerance.
        '  </summary>
        '  <param name="value1">First argument to compare.</param>
        '  <param name="value2">Second argument to compare.</param>
        '  <returns>How the first argument compares to the second:
        '    <list type="bullet">
        '      <item>-1, smaller;</item>
        '      <item>0, equal;</item>
        '      <item>1, greater.</item>
        '    </list>
        '  </returns>
        '*/
        Public Shared Function Compare(ByVal value1 As Double, ByVal value2 As Double) As Integer
            Return Compare(value1, value2, Epsilon)
        End Function

        '/**
        '  <summary>Compares double-precision floating-point numbers applying the specified error tolerance.
        '  </summary>
        '  <param name="value1">First argument to compare.</param>
        '  <param name="value2">Second argument to compare.</param>
        '  <param name="epsilon">Relative error tolerance.</param>
        '  <returns>How the first argument compares to the second:
        '    <list type="bullet">
        '      <item>-1, smaller;</item>
        '      <item>0, equal;</item>
        '      <item>1, greater.</item>
        '    </list>
        '  </returns>
        '*/
        Public Shared Function Compare(ByVal value1 As Double, ByVal value2 As Double, ByVal epsilon As Double) As Integer
            Dim exponent As Integer = GetExponent(System.Math.Max(value1, value2))
            Dim delta As Double = epsilon * System.Math.Pow(2, exponent)
            Dim difference As Double = value1 - value2
            If (difference > delta) Then
                Return 1
            ElseIf (difference < -delta) Then
                Return -1
            Else
                Return 0
            End If
        End Function


        '/**
        '  <summary>Compares big-endian byte arrays.</summary>
        '  <param name="data1">First argument to compare.</param>
        '  <param name="data2">Second argument to compare.</param>
        '  <returns>How the first argument compares to the second:
        '    <list type="bullet">
        '      <item>-1, smaller;</item>
        '      <item>0, equal;</item>
        '      <item>1, greater.</item>
        '    </list>
        '  </returns>
        '*/
        Public Shared Function Compare(ByVal data1 As Byte(), ByVal data2 As Byte()) As Integer

            Dim length As Integer = data1.Length
            For index As Integer = 0 To length - 1
                Select Case (CInt(System.Math.Sign((data1(index) And &HFF) - (data2(index) And &HFF))))
                    Case -1 : Return -1
                    Case 1 : Return 1
                End Select
            Next
            Return 0
        End Function

        '/**
        '  <summary>Increments a big-endian byte array.</summary>
        '*/
        Public Shared Sub Increment(ByVal data As Byte())
            Increment(data, data.Length - 1)
        End Sub

        '/**
        '  <summary>Increments a big-endian byte array at the specified position.</summary>
        '*/
        Public Shared Sub Increment(ByVal data As Byte(), ByVal position As Integer)
            If ((data(position) And &HFF) = 255) Then
                data(position) = 0
                Increment(data, position - 1)
            Else
                data(position) = CByte(data(position) + 1)
            End If
        End Sub

        '/**
        '  <summary>Gets the unbiased exponent of the specified argument.</summary>
        '*/
        Private Shared Function GetExponent(ByVal value As Double) As Integer
            Return CInt(((BitConverter.DoubleToInt64Bits(value) And DoubleExponentBitMask) >> (DoubleSignificandBitCount)) - DoubleExponentBias)
        End Function
#End Region
#End Region
    End Class

End Namespace
