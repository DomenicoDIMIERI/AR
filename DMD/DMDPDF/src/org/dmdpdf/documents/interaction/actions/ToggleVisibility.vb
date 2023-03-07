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
Imports DMD.org.dmdpdf.documents.interaction.forms
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Toggle the visibility of one or more annotations on the screen' action [PDF:1.6:8.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class ToggleVisibility
        Inherits Action

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new action within the given document context.</summary>
        '*/
        Public Sub New(ByVal context As Document, ByVal objects As ICollection(Of PdfObjectWrapper), ByVal visible As Boolean)
            MyBase.New(context, PdfName.Hide)
            Me.objects = objects
            Me.visible = visible
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the annotations (Or associated form fields) To be affected.</summary>
        '*/
        Public Property Objects As ICollection(Of PdfObjectWrapper)
            Get
                Dim _objects As List(Of PdfObjectWrapper) = New List(Of PdfObjectWrapper)()
                '{
                Dim objectsObject As PdfDirectObject = BaseDataObject(PdfName.T)
                FillObjects(objectsObject, _objects)
                '}
                Return _objects
            End Get
            Set(ByVal value As ICollection(Of PdfObjectWrapper))
                Dim objectsDataObject As PdfArray = New PdfArray()
                For Each item As PdfObjectWrapper In value
                    If (TypeOf (item) Is Annotation) Then
                        objectsDataObject.Add(item.BaseObject)
                    ElseIf (TypeOf (item) Is Field) Then
                        objectsDataObject.Add(New PdfTextString(CType(item, Field).FullName))
                    Else
                        Throw New ArgumentException("Invalid 'Hide' action target type (" & item.GetType().Name & ")." & vbLf & "It MUST be either an annotation or a form field.")
                    End If
                Next
                BaseDataObject(PdfName.T) = objectsDataObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether To show the annotations.</summary>
        '*/
        Public Property Visible As Boolean
            Get
                Dim hideObject As PdfBoolean = CType(BaseDataObject(PdfName.H), PdfBoolean)
                If (hideObject IsNot Nothing) Then
                    Return Not hideObject.BooleanValue
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.H) = PdfBoolean.Get(Not value)
            End Set
        End Property

#End Region

#Region "Private"

        Private Sub FillObjects(ByVal objectObject As PdfDataObject, ByVal objects As ICollection(Of PdfObjectWrapper))
            Dim objectDataObject As PdfDataObject = PdfObject.Resolve(objectObject)
            If (TypeOf (objectDataObject) Is PdfArray) Then ' MultipleThen objects.
                For Each itemObject As PdfDirectObject In CType(objectDataObject, PdfArray)
                    FillObjects(itemObject, objects)
                Next
            Else ' Single object.
                If (TypeOf (objectDataObject) Is PdfDictionary) Then ' Annotation.
                    objects.Add(Annotation.Wrap(CType(objectObject, PdfReference)))
                ElseIf (TypeOf (objectDataObject) Is PdfTextString) Then ' Form Then field (associated To widget annotations).
                    objects.Add(Document.Form.Fields(CStr(CType(objectDataObject, PdfTextString).Value)))
                Else ' Invalid object type.
                    Throw New System.Exception("Invalid 'Hide' action target type (" & objectDataObject.GetType().Name & ")." & vbLf & "It should be either an annotation or a form field.")
                End If
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace