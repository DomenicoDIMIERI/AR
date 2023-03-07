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

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Set the text leading' operation [PDF:1.6:5.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class SetTextLead
        Inherits Operation

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "TL"

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal value As Double)
            MyBase.New(OperatorKeyword, PdfReal.Get(value))
        End Sub

        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(OperatorKeyword, operands)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            state.Lead = Me.Value
        End Sub

        '/**
        '  <summary>Gets/Sets the text leading, which is a number expressed in unscaled text space units.
        '  </summary>
        '*/
        Public Property Value As Double
            Get
                Return CType(Me._operands(0), IPdfNumber).RawValue
            End Get
            Set(ByVal value As Double)
                Me._operands(0) = PdfReal.Get(value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace