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
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Form field actions [PDF:1.6:8.5.2].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class FieldActions
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets a JavaScript action to be performed to recalculate the value
        '  of this field when that of another field changes.</summary>
        '*/
        Public Property OnCalculate As JavaScript
            Get
                Return CType(actions.Action.Wrap(BaseDataObject(PdfName.C)), JavaScript)
            End Get
            Set(ByVal value As JavaScript)
                Me.BaseDataObject(PdfName.C) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets a JavaScript action to be performed when the user types a keystroke
        '  into a text field or combo box or modifies the selection in a scrollable list box.</summary>
        '*/
        Public Property OnChange As JavaScript
            Get
                Return CType(actions.Action.Wrap(BaseDataObject(PdfName.K)), JavaScript)
            End Get
            Set(ByVal value As JavaScript)
                Me.BaseDataObject(PdfName.K) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets a JavaScript action to be performed before the field is formatted
        '  to display its current value.</summary>
        '  <remarks>This action can modify the field's value before formatting.</remarks>
        '*/
        Public Property OnFormat As JavaScript
            Get
                Return CType(actions.Action.Wrap(BaseDataObject(PdfName.F)), JavaScript)
            End Get
            Set(ByVal value As JavaScript)
                Me.BaseDataObject(PdfName.F) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets a JavaScript action to be performed when the field's value is changed.</summary>
        '  <remarks>This action can check the new value for validity.</remarks>
        '*/
        Public Property OnValidate As JavaScript
            Get
                Return CType(actions.Action.Wrap(BaseDataObject(PdfName.V)), JavaScript)
            End Get
            Set(ByVal value As JavaScript)
                BaseDataObject(PdfName.V) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace