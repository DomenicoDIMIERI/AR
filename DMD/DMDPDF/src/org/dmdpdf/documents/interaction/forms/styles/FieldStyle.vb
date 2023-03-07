'/*
'  Copyright 2008-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.objects

Namespace DMD.org.dmdpdf.documents.interaction.forms.styles

    '/**
    '  <summary>Abstract field appearance style.</summary>
    '  <remarks>It automates the definition of field appearance, applying a common look.</remarks>
    '*/
    Public MustInherit Class FieldStyle

#Region "dynamic"
#Region "fields"

        Private _backColor As Color = DeviceRGBColor.White
        Private _checkSymbol As Char = Chr(52) '
        Private _fontSize As Double = 10
        Private _foreColor As Color = DeviceRGBColor.Black
        Private _graphicsVisibile As Boolean = False
        Private _radioSymbol As Char = Chr(108)
#End Region

#Region "constructors"

        Protected Sub New()
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public MustOverride Sub Apply(ByVal field As Field)

        Public Property BackColor As Color
            Get
                Return Me._backColor
            End Get
            Set(ByVal value As Color)
                Me._backColor = value
            End Set
        End Property

        Public Property CheckSymbol As Char
            Get
                Return Me._checkSymbol
            End Get
            Set(ByVal value As Char)
                Me._checkSymbol = value
            End Set
        End Property

        Public Property FontSize As Double
            Get
                Return Me._fontSize
            End Get
            Set(ByVal value As Double)
                Me._fontSize = value
            End Set
        End Property

        Public Property ForeColor As Color
            Get
                Return Me._foreColor
            End Get
            Set(ByVal value As Color)
                Me._foreColor = value
            End Set
        End Property

        Public Property GraphicsVisibile As Boolean
            Get
                Return Me._graphicsVisibile
            End Get
            Set(ByVal value As Boolean)
                Me._graphicsVisibile = value
            End Set
        End Property

        Public Property RadioSymbol As Char
            Get
                Return Me._radioSymbol
            End Get
            Set(ByVal value As Char)
                Me._radioSymbol = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
