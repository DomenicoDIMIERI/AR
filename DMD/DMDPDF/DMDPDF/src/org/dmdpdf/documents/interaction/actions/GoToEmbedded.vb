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
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Change the view to a specified destination in a PDF file embedded in another PDF file'
    '  action [PDF:1.6:8.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class GoToEmbedded
        Inherits GotoNonLocal(Of Destination)

#Region "types"
        '/**
        '  <summary>Path information to the target document [PDF:1.6:8.5.3].</summary>
        '*/
        Public Class PathElement
            Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
            '/**
            '  <summary>Relationship between the target and the current document [PDF:1.6:8.5.3].</summary>
            '*/
            Public Enum RelationEnum

                '    /**
                '  <summary>Parent.</summary>
                '*/
                Parent
                '/**
                '  <summary>Child.</summary>
                '*/
                Child
            End Enum

#End Region

#Region "Static"
#Region "fields"

            Private Shared ReadOnly _RelationEnumCodes As Dictionary(Of RelationEnum, PdfName)

#End Region

#Region "constructors"

            Shared Sub New()
                _RelationEnumCodes = New Dictionary(Of RelationEnum, PdfName)()
                _RelationEnumCodes(RelationEnum.Parent) = PdfName.P
                _RelationEnumCodes(RelationEnum.Child) = PdfName.C
            End Sub

#End Region

#Region "interface"
#Region "private"

            '/**
            '  <summary> Gets the code corresponding To the given value.</summary>
            '*/
            Private Shared Function ToCode(ByVal value As RelationEnum) As PdfName
                Return _RelationEnumCodes(value)
            End Function

            '/**
            '  <summary> Gets the relation corresponding To the given value.</summary>
            '*/
            Private Shared Function ToRelationEnum(ByVal value As PdfName) As RelationEnum
                For Each relation As KeyValuePair(Of RelationEnum, PdfName) In _RelationEnumCodes
                    If (relation.Value.Equals(value)) Then
                        Return relation.Key
                    End If
                Next
                Throw New Exception("'" & value.Value.ToString & "' doesn't represent a valid relation.")
            End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

            '/**
            '  <summary>Creates a New path element representing the parent Of the document.</summary>
            '*/
            Public Sub New(ByVal context As Document, ByVal [Next] As PathElement)
                Me.New(context, RelationEnum.Parent, Nothing, Nothing, Nothing, [Next])
            End Sub

            '/**
            '  <summary>Creates a New path element located In the embedded files collection Of the document.</summary>
            '*/
            Public Sub New(ByVal context As Document, ByVal embeddedFileName As String, ByVal [Next] As PathElement)
                Me.New(context, RelationEnum.Child, embeddedFileName, Nothing, Nothing, [Next])
            End Sub

            '/**
            '  <summary>Creates a New path element associated With a file attachment annotation.</summary>
            '*/
            Public Sub New(ByVal context As Document, ByVal annotationPageRef As Object, ByVal annotationRef As Object, ByVal [Next] As PathElement)
                Me.New(context, RelationEnum.Child, Nothing, annotationPageRef, annotationRef, [Next])
            End Sub

            '/**
            '  <summary>Creates a New path element.</summary>
            '*/
            Private Sub New(ByVal context As Document, ByVal relation As RelationEnum, ByVal embeddedFileName As String, ByVal annotationPageRef As Object, ByVal annotationRef As Object, ByVal [next] As PathElement)
                MyBase.New(context, New PdfDictionary())
                Me.relation = relation
                Me.embeddedFileName = embeddedFileName
                Me.annotationPageRef = annotationPageRef
                Me.annotationRef = annotationRef
                Me.Next = [next]
            End Sub

            '/**
            '  <summary>Instantiates an existing path element.</summary>
            '*/
            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

#End Region

#Region "Interface"
#Region "Public"

            Public Overrides Function Clone(ByVal context As Document) As Object
                Throw New NotImplementedException()
            End Function

            '/**
            '  <summary>Gets/Sets the page reference To the file attachment annotation.</summary>
            '  <returns>Either the (zero-based) number Of the page In the current document containing the file attachment annotation,
            '  Or the name of a destination in the current document that provides the page number of the file attachment annotation.</returns>
            '*/
            Public Property AnnotationPageRef As Object
                Get
                    Dim pageRefObject As PdfDirectObject = BaseDataObject(PdfName.P)
                    If (pageRefObject Is Nothing) Then Return Nothing
                    If (TypeOf (pageRefObject) Is PdfInteger) Then
                        Return CType(pageRefObject, PdfInteger).Value
                    Else
                        Return CType(pageRefObject, PdfString).Value
                    End If
                End Get
                Set(ByVal value As Object)
                    If (value Is Nothing) Then
                        BaseDataObject.Remove(PdfName.P)
                    Else
                        Dim pageRefObject As PdfDirectObject
                        If (TypeOf (value) Is Integer) Then
                            pageRefObject = PdfInteger.Get(CInt(value))
                        ElseIf (TypeOf (value) Is String) Then
                            pageRefObject = New PdfString(CStr(value))
                        Else
                            Throw New ArgumentException("Wrong argument type: it MUST be either a page number Integer or a named destination String.")
                        End If
                        BaseDataObject(PdfName.P) = pageRefObject
                    End If
                End Set
            End Property


            '/**
            '  <summary>Gets/Sets the reference To the file attachment annotation.</summary>
            '  <returns>Either the (zero-based) index Of the annotation In the list Of annotations
            '  associated to the page specified by the annotationPageRef property, Or the name of the annotation.</returns>
            '*/
            Public Property AnnotationRef As Object
                Get
                    Dim annotationRefObject As PdfDirectObject = BaseDataObject(PdfName.A)
                    If (annotationRefObject Is Nothing) Then Return Nothing
                    If (TypeOf (annotationRefObject) Is PdfInteger) Then
                        Return CType(annotationRefObject, PdfInteger).Value
                    Else
                        Return CType(annotationRefObject, PdfTextString).Value
                    End If
                End Get
                Set(ByVal value As Object)
                    If (value Is Nothing) Then
                        BaseDataObject.Remove(PdfName.A)
                    Else
                        Dim annotationRefObject As PdfDirectObject
                        If (TypeOf (value) Is Integer) Then
                            annotationRefObject = PdfInteger.Get(CInt(value))
                        ElseIf (TypeOf (value) Is String) Then
                            annotationRefObject = New PdfTextString(CStr(value))
                        Else
                            Throw New ArgumentException("Wrong argument type: it MUST be either an annotation index Integer or an annotation name String.")
                        End If
                        BaseDataObject(PdfName.A) = annotationRefObject
                    End If
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the embedded file name.</summary>
            '*/
            Public Property EmbeddedFileName As String
                Get
                    Dim fileNameObject As PdfString = CType(BaseDataObject(PdfName.N), PdfString)
                    If (fileNameObject IsNot Nothing) Then
                        Return CStr(fileNameObject.Value)
                    Else
                        Return Nothing
                    End If
                End Get
                Set(ByVal value As String)
                    If (value Is Nothing) Then
                        BaseDataObject.Remove(PdfName.N)
                    Else
                        BaseDataObject(PdfName.N) = New PdfString(value)
                    End If
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the relationship between the target And the current document.</summary>
            '*/
            Public Property Relation As RelationEnum
                Get
                    Return ToRelationEnum(CType(BaseDataObject(PdfName.R), PdfName))
                End Get
                Set(ByVal value As RelationEnum)
                    BaseDataObject(PdfName.R) = ToCode(value)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets a further path information To the target document.</summary>
            '*/
            Public Property [Next] As PathElement
                Get
                    Dim targetObject As PdfDirectObject = BaseDataObject(PdfName.T)
                    If (targetObject IsNot Nothing) Then
                        Return New PathElement(targetObject)
                    Else
                        Return Nothing
                    End If
                End Get
                Set(ByVal value As PathElement)
                    If (value Is Nothing) Then
                        BaseDataObject.Remove(PdfName.T)
                    Else
                        BaseDataObject(PdfName.T) = value.BaseObject
                    End If
                End Set
            End Property

#End Region
#End Region
#End Region
        End Class

#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a New instance within the specified document context, pointing To a
        '  Destination within an embedded document.</summary>
        '  <param name = "context" > Document context.</param>
        '  <param name = "destinationPath" > Path information To the target document within the destination
        '  File.</param>
        '  <param name = "destination" > Destination within the target document.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal destinationPath As PathElement, ByVal Destination As Destination)
            Me.New(context, Nothing, destinationPath, Destination)
        End Sub

        '/**
        '  <summary>Creates a New instance within the specified document context, pointing To a
        '  Destination within another document.</summary>
        '  <param name = "context" > Document context.</param>
        '  <param name = "destinationFile" > File In which the destination Is located.</param>
        '  <param name = "destination" > Destination within the target document.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal destinationFile As FileSpecification, ByVal Destination As Destination)
            Me.New(context, destinationFile, Nothing, Destination)
        End Sub

        '/**
        '  <summary>Creates a New instance within the specified document context.</summary>
        '  <param name = "context" > Document context.</param>
        '  <param name = "destinationFile" > File In which the destination Is located.</param>
        '  <param name = "destinationPath" > Path information To the target document within the destination
        '  File.</param>
        '  <param name = "destination" > Destination within the target document.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal destinationFile As FileSpecification, ByVal destinationPath As PathElement, ByVal destination As Destination)
            MyBase.New(context, PdfName.GoToE, destinationFile, destination)
            Me.DestinationPath = destinationPath
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the path information To the target document.</summary>
        '*/
        Public Property DestinationPath As PathElement
            Get
                Dim targetObject As PdfDirectObject = Me.BaseDataObject(PdfName.T)
                If (targetObject IsNot Nothing) Then
                    Return New PathElement(targetObject)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As PathElement)
                If (value Is Nothing) Then
                    BaseDataObject.Remove(PdfName.T)
                Else
                    BaseDataObject(PdfName.T) = value.BaseObject
                End If
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace