'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  Me file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  Me Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  Me Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.objects

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Inline image data (anonymous) operation [PDF:1.6:4.8.6].</summary>
    '  <remarks>Me is a figurative operation necessary to constrain the inline image data section
    '  within the content stream model.</remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class InlineImageBody
        Inherits Operation

#Region "Static"
#Region "fields"

        Private _value As IBuffer

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal value As IBuffer)
            MyBase.New(Nothing)
            Me._value = value
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property Value As IBuffer
            Get
                Return Me._value
            End Get
            Set(ByVal value As IBuffer)
                Me._value = value
            End Set
        End Property

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)
            stream.Write(_value)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace