'/*
'  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Set the text matrix' operation [PDF:1.6:5.3.1].</summary>
    '  <remarks>The specified matrix is not concatenated onto the current text matrix,
    '  but replaces it.</remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class SetTextMatrix
        Inherits Operation

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "Tm"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal value As Matrix)
            Me.New(
                    value.Elements(0),
                    value.Elements(1),
                    value.Elements(2),
                    value.Elements(3),
                    value.Elements(4),
                    value.Elements(5)
                    )
        End Sub

        Public Sub New(ByVal a As Double,
                        ByVal b As Double,
                        ByVal c As Double,
                        ByVal d As Double,
                        ByVal e As Double,
                        ByVal f As Double
                        )
            MyBase.New(
                    OperatorKeyword,
                    PdfReal.Get(a),
                    PdfReal.Get(b),
                    PdfReal.Get(c),
                    PdfReal.Get(d),
                    PdfReal.Get(e),
                    PdfReal.Get(f)
                    )
        End Sub


        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            state.Tlm = Me.Value
            state.Tm = state.Tlm.Clone()
        End Sub

        Public ReadOnly Property Value As Matrix
            Get
                Return New Matrix(
                          CType(Me._operands(0), IPdfNumber).FloatValue,
                          CType(Me._operands(1), IPdfNumber).FloatValue,
                          CType(Me._operands(2), IPdfNumber).FloatValue,
                          CType(Me._operands(3), IPdfNumber).FloatValue,
                          CType(Me._operands(4), IPdfNumber).FloatValue,
                          CType(Me._operands(5), IPdfNumber).FloatValue
                          )
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace