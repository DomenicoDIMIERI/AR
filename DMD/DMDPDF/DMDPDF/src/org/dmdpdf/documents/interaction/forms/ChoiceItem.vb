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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Field option [PDF:1.6:8.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ChoiceItem
        Inherits PdfObjectWrapper(Of PdfDirectObject)

#Region "fields"

        Private _items As ChoiceItems

#End Region

#Region "constructors"

        Public Sub New(ByVal value As String)
            MyBase.New(New PdfTextString(value))
        End Sub

        Public Sub New(ByVal context As Document, ByVal value As String, ByVal text As String)
            MyBase.New(context, New PdfArray(New PdfDirectObject() {New PdfTextString(value), New PdfTextString(text)}))
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject, ByVal items As ChoiceItems)
            MyBase.New(baseObject)
            Me.Items = items
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException
        End Function

        '//TODO:make the class immutable (to avoid needing wiring it up to its collection...)!!!
        '/**
        '  <summary>Gets/Sets the displayed text.</summary>
        '*/
        Public Property Text As String
            Get
                Dim baseDataObject As PdfDirectObject = Me.BaseDataObject
                If (TypeOf (baseDataObject) Is PdfArray) Then ' <value,text> pair.
                    Return CStr(CType(CType(baseDataObject, PdfArray)(1), PdfTextString).Value)
                Else ' Single text string.
                    Return CStr(CType(baseDataObject, PdfTextString).Value)
                End If
            End Get
            Set(ByVal value As String)
                Dim baseDataObject As PdfDirectObject = Me.BaseDataObject
                If (TypeOf (baseDataObject) Is PdfTextString) Then
                    Dim oldBaseDataObject As PdfDirectObject = baseDataObject
                    baseDataObject = New PdfArray(New PdfDirectObject() {oldBaseDataObject})
                    BaseObject = baseDataObject
                    CType(baseDataObject, PdfArray).Add(PdfTextString.Default)
                    If (_items IsNot Nothing) Then
                        ' Force list update!
                        '/*
                        '  NOTE: This operation is necessary in order to substitute
                        '  the previous base object with the new one within the list.
                        '*/
                        Dim itemsObject As PdfArray = _items.BaseDataObject
                        itemsObject(itemsObject.IndexOf(oldBaseDataObject)) = baseDataObject
                    End If
                End If
                CType(baseDataObject, PdfArray)(1) = New PdfTextString(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the export value.</summary>
        '*/
        Public Property Value As String
            Get
                Dim baseDataObject As PdfDirectObject = Me.BaseDataObject
                If (TypeOf (baseDataObject) Is PdfArray) Then ' <value,text> pair.
                    Return CStr(CType(CType(baseDataObject, PdfArray)(0), PdfTextString).Value)
                Else ' Single text string.
                    Return CStr(CType(baseDataObject, PdfTextString).Value)
                End If
            End Get
            Set(ByVal value As String)
                Dim baseDataObject As PdfDirectObject = Me.BaseDataObject
                If (TypeOf (baseDataObject) Is PdfArray) Then '<value,text> pair.
                    CType(baseDataObject, PdfArray)(0) = New PdfTextString(value)
                Else ' Single text string.
                    BaseObject = New PdfTextString(value)
                End If
            End Set
        End Property

#End Region

#Region "internal"

        Friend WriteOnly Property Items As ChoiceItems
            Set
                If (_items IsNot Nothing) Then Throw New ArgumentException("Item already associated to another choice field.")
                _items = Value
            End Set
        End Property


#End Region
#End Region

    End Class

End Namespace