'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Combo box [PDF:1.6:8.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ComboBox
        Inherits ChoiceField

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new combobox within the given document context.</summary>
        '*/
        Public Sub New(ByVal name As String, ByVal widget As Widget)
            MyBase.new(name, widget)
            Me.Flags = EnumUtils.Mask(Flags, FlagsEnum.Combo, True)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets whether the text is editable.</summary>
        '*/
        Public Property Editable As Boolean
            Get
                Return (Me.Flags And FlagsEnum.Edit) = FlagsEnum.Edit
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.Edit, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether the edited text is spell checked.</summary>
        '*/
        Public Property SpellChecked As Boolean
            Get
                Return (Me.Flags And FlagsEnum.DoNotSpellCheck) <> FlagsEnum.DoNotSpellCheck
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.DoNotSpellCheck, Not value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace