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

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.colorSpaces

    '/**
    '  <summary>Color value defined by numeric-level components.</summary>
    '*/
    Public MustInherit Class LeveledColor
        Inherits Color

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal colorSpace As ColorSpace, ByVal baseObject As PdfDirectObject)
            MyBase.New(colorSpace, baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public NotOverridable Overrides ReadOnly Property Components As IList(Of PdfDirectObject)
            Get
                Return Me.BaseArray
            End Get
        End Property

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If (obj Is Nothing OrElse Not obj.GetType().Equals(Me.GetType())) Then Return False
            Dim objectIterator As IEnumerator(Of PdfDirectObject) = CType(obj, LeveledColor).BaseArray.GetEnumerator()
            Dim thisIterator As IEnumerator(Of PdfDirectObject) = Me.BaseArray.GetEnumerator()
            While (thisIterator.MoveNext())
                objectIterator.MoveNext()
                If (Not thisIterator.Current.Equals(objectIterator.Current)) Then
                    Return False
                End If
            End While
            Return True
        End Function

        Public NotOverridable Overrides Function GetHashCode() As Integer
            Dim hashCode As Integer = 0
            For Each component As PdfDirectObject In BaseArray
                hashCode = hashCode Xor component.GetHashCode()
            Next
            Return hashCode
        End Function

#End Region

#Region "protected"
        '/*
        '  NOTE: This Is a workaround to the horrible lack of covariance support in C# 3 which forced me
        '  to flatten type parameters at top hierarchy level (see Java implementation). Anyway, suggestions
        '  to overcome this issue are welcome!
        '*/
        Protected ReadOnly Property BaseArray As PdfArray
            Get
                Return CType(BaseDataObject, PdfArray)
            End Get
        End Property


        '/**
        '  <summary> Gets the specified color component.</summary>
        '  <param name = "index" > Component index.</param>
        '*/
        Protected Function GetComponentValue(ByVal index As Integer) As Double
            Return CType(Components(index), IPdfNumber).RawValue
        End Function

        Protected Sub SetComponentValue(ByVal index As Integer, ByVal value As Double)
            Components(index) = PdfReal.Get(NormalizeComponent(value))
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace