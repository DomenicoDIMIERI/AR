'/*
'  Copyright 2010 Stefano Chizzolini. http: //www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With Me
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  Me list Of conditions.
'*/

Imports DMD.org.dmdpdf.objects

Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary> Text character.</summary>
    '  <remarks> It describes a text element extracted from content streams.</remarks>
    '*/
    Public NotInheritable Class TextChar

#Region "dynamic"
#Region "fields"
        Private ReadOnly m_box As RectangleF
        Private ReadOnly m_style As TextStyle
        Private ReadOnly m_value As Char
        Private ReadOnly m_virtual_ As Boolean
#End Region

#Region "constructors"
        Public Sub New(ByVal value As Char, ByVal box As RectangleF, ByVal style As TextStyle, ByVal virtual_ As Boolean)
            Me.m_value = value
            Me.m_box = box
            Me.m_style = style
            Me.m_virtual_ = virtual_
        End Sub
#End Region

#Region "Interface"
#Region "Public"
        Public ReadOnly Property Box As RectangleF
            Get
                Return Me.m_box
            End Get
        End Property

        Public ReadOnly Property Style As TextStyle
            Get
                Return Me.m_style
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.Value.ToString()
        End Function


        Public ReadOnly Property Value As Char
            Get
                Return Me.m_value
            End Get
        End Property

        Public ReadOnly Property Virtual As Boolean
            Get
                Return Me.m_virtual_
            End Get
        End Property
#End Region
#End Region
#End Region

    End Class

End Namespace