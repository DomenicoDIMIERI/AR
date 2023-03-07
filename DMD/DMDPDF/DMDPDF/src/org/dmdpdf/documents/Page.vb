'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction.navigation.page
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports drawing = System.Drawing

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary>Document page [PDF:1.6:3.6.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public Class Page
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements IContentContext

        '/*
        '  NOTE: Inheritable attributes are NOT early-collected, as they are NOT part
        '  of the explicit representation of a page. They are retrieved every time
        '  clients call.
        '*/
#Region "types"
        '/**
        '  <summary>Annotations tab order [PDF:1.6:3.6.2].</summary>
        '*/
        <PDF(VersionEnum.PDF15)>
        Public Enum TabOrderEnum
            ''' <summary>
            ''' Row order.
            ''' </summary>
            Row

            ''' <summary>
            ''' Column order
            ''' </summary>
            Column

            ''' <summary>
            ''' Structure order
            ''' </summary>
            [Structure]
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Public Shared ReadOnly InheritableAttributeKeys As ISet(Of PdfName)

        Private Shared ReadOnly TabOrderEnumCodes As Dictionary(Of TabOrderEnum, PdfName)

#End Region

#Region "constructors"

        Shared Sub New() 'Page()
            InheritableAttributeKeys = New HashSet(Of PdfName)
            InheritableAttributeKeys.Add(PdfName.Resources)
            InheritableAttributeKeys.Add(PdfName.MediaBox)
            InheritableAttributeKeys.Add(PdfName.CropBox)
            InheritableAttributeKeys.Add(PdfName.Rotate)

            TabOrderEnumCodes = New Dictionary(Of TabOrderEnum, PdfName)
            TabOrderEnumCodes(TabOrderEnum.Row) = PdfName.R
            TabOrderEnumCodes(TabOrderEnum.Column) = PdfName.C
            TabOrderEnumCodes(TabOrderEnum.Structure) = PdfName.S
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets the attribute value corresponding to the specified key, possibly recurring to
        '  its ancestor nodes in the page tree.</summary>
        '  <param name="pageObject">Page object.</param>
        '  <param name="key">Attribute key.</param>
        '*/
        Public Shared Function GetInheritableAttribute(ByVal pageObject As PdfDictionary, ByVal key As PdfName) As PdfDirectObject
            '/*
            '  NOTE: It moves upward until it finds the inherited attribute.
            '*/
            Dim Dictionary As PdfDictionary = pageObject
            While (True)
                Dim entry As PdfDirectObject = Dictionary(key)
                If (entry IsNot Nothing) Then
                    Return entry
                End If

                Dictionary = CType(Dictionary.Resolve(PdfName.Parent), PdfDictionary)
                If (Dictionary Is Nothing) Then
                    ' Isn't the page attached to the page tree?
                    '/* NOTE: This condition Is illegal. */
                    If (pageObject(PdfName.Parent) Is Nothing) Then
                        Throw New Exception("Inheritable attributes unreachable: Page objects MUST be inserted into their document's Pages collection before being used.")
                    End If
                    Return Nothing
                End If
            End While

            Return Nothing 'Should never happen
        End Function

        Public Shared Function Wrap(ByVal BaseObject As PdfDirectObject) As Page
            If (BaseObject Is Nothing) Then
                Return Nothing
            Else
                Return New Page(BaseObject)
            End If
        End Function

#End Region

#Region "private"
        '/**
        '  <summary>Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As TabOrderEnum) As PdfName
            Return TabOrderEnumCodes(value)
        End Function

        '/**
        '  <summary>Gets the tab order corresponding To the given value.</summary>
        '*/
        Private Shared Function ToTabOrderEnum(ByVal value As PdfName) As TabOrderEnum
            For Each tabOrder As KeyValuePair(Of TabOrderEnum, PdfName) In TabOrderEnumCodes
                If (tabOrder.Value.Equals(value)) Then
                    Return tabOrder.Key
                End If
            Next
            Return TabOrderEnum.Row
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"
        '/**
        '  <summary>Creates a New page within the specified document context, Using the Default size.
        '  </summary>
        '  <param name = "context" > Document where To place this page.</param>
        '*/
        Public Sub New(ByVal context As Document)
            Me.New(context, Nothing)
        End Sub

        '/**
        '  <summary>Creates a New page within the specified document context.</summary>
        '  <param name = "context" > Document where To place this page.</param>
        '  <param name = "size" > Page size. In Case Of <code>null</code>, uses the Default size.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal size As System.Drawing.SizeF?)
            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type, PdfName.Contents}, New PdfDirectObject() {PdfName.Page, context.File.Register(New PdfStream())}))
            If (size.HasValue) Then
                size = size.Value
            End If
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets/Sets the page's behavior in response to trigger events.</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Property Actions As PageActions
            Get
                Return New PageActions(BaseDataObject.Get(Of PdfDictionary)(PdfName.AA))
            End Get
            Set(ByVal value As PageActions)
                BaseDataObject(PdfName.AA) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the annotations associated To the page.</summary>
        '*/
        Public Property Annotations As PageAnnotations
            Get
                Return New PageAnnotations(BaseDataObject.Get(Of PdfArray)(PdfName.Annots), Me)
            End Get
            Set(ByVal value As PageAnnotations)
                BaseDataObject(PdfName.Annots) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the extent Of the page's meaningful content (including potential white space)
        '  as intended by the page's creator [PDF:1.7:10.10.1].</summary>
        '  <seealso cref = "CropBox" />
        '*/
        <PDF(VersionEnum.PDF13)>
        Public Property ArtBox As System.Drawing.RectangleF
            Get
                '/*
                '  NOTE:                 The default value Is the page's crop box.
                '*/
                Dim artBoxObject As PdfDirectObject = GetInheritableAttribute(PdfName.ArtBox)
                If (artBoxObject IsNot Nothing) Then
                    Return Rectangle.Wrap(artBoxObject).ToRectangleF()
                Else
                    Return CropBox
                End If
            End Get
            Set(ByVal value As System.Drawing.RectangleF)
                BaseDataObject(PdfName.ArtBox) = New Rectangle(value).BaseDataObject
            End Set
        End Property

        '/**
        '  <summary>Gets the page article beads.</summary>
        '*/
        Public ReadOnly Property ArticleElements As PageArticleElements
            Get
                Return New PageArticleElements(BaseDataObject.Get(Of PdfArray)(PdfName.B), Me)
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the region To which the contents Of the page should be clipped When output
        '  in a production environment [PDF:1.7:10.10.1].</summary>
        '  <remarks>
        '    <para>This may include any extra bleed area needed To accommodate the physical limitations Of
        '    cutting, folding, And trimming equipment. The actual printed page may include printing marks
        '    that fall outside the bleed box.</para>
        '  </remarks>
        '  <seealso cref = "CropBox" />
        '*/
        <PDF(VersionEnum.PDF13)>
        Public Property BleedBox As System.Drawing.RectangleF
            Get
                '/*
                '  NOTE:                 The default value Is the page's crop box.
                '*/
                Dim bleedBoxObject As PdfDirectObject = GetInheritableAttribute(PdfName.BleedBox)
                If (bleedBoxObject IsNot Nothing) Then
                    Return Rectangle.Wrap(bleedBoxObject).ToRectangleF()
                Else
                    Return CropBox
                End If
            End Get
            Set(ByVal value As System.Drawing.RectangleF)
                BaseDataObject(PdfName.BleedBox) = New Rectangle(value).BaseDataObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the region To which the contents Of the page are To be clipped (cropped)
        '  when displayed Or printed [PDF:1.7:10.10.1].</summary>
        '  <remarks>
        '    <para>Unlike the other boxes, the crop box has no defined meaning In terms Of physical page
        '    geometry Or intended use; it merely imposes clipping On the page contents. However, in the
        '    absence of additional information, The crop box determines how the page's contents are to be
        '                            positioned on the output medium.</para>
        '  </remarks>
        '  <seealso cref = "Box" />
        '*/
        Public Property CropBox As System.Drawing.RectangleF
            Get
                '/*
                '  NOTE:                 The default value Is the page's media box.
                '*/
                Dim cropBoxObject As PdfDirectObject = GetInheritableAttribute(PdfName.CropBox)
                If (cropBoxObject IsNot Nothing) Then
                    Return Rectangle.Wrap(cropBoxObject).ToRectangleF()
                Else
                    Return Me.Box
                End If
            End Get
            Set(ByVal value As System.Drawing.RectangleF)
                BaseDataObject(PdfName.CropBox) = New Rectangle(value).BaseDataObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the page's display duration.</summary>
        '  <remarks>
        '    <para>The page's display duration (also called its advance timing)
        '    Is the maximum length of time, in seconds, that the page Is displayed
        '    during presentations before the viewer application automatically advances
        '    to the next page.</para>
        '    <para>By Default, the viewer does Not advance automatically.</para>
        '  </remarks>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Duration As Double
            Get
                Dim durationObject As IPdfNumber = CType(BaseDataObject(PdfName.Dur), IPdfNumber)
                If (durationObject Is Nothing) Then
                    Return 0
                Else
                    Return durationObject.RawValue
                End If
            End Get
            Set(ByVal value As Double)
                If (value = 0) Then
                    BaseDataObject(PdfName.Dur) = Nothing
                Else
                    BaseDataObject(PdfName.Dur) = PdfReal.Get(value)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets the index Of the page.</summary>
        '*/
        Public ReadOnly Property Index As Integer
            Get
                '/*
                '  NOTE:                 We 'll scan sequentially each page-tree level above this page object
                '                        collecting Page counts. At each level we'll scan the kids array from the
                '                        lower-indexed item to the ancestor of this page object at that level.
                '*/
                Dim ancestorKidReference As PdfReference = CType(BaseObject, PdfReference)
                Dim parentReference As PdfReference = CType(BaseDataObject(PdfName.Parent), PdfReference)
                Dim parent As PdfDictionary = CType(parentReference.DataObject, PdfDictionary)
                Dim kids As PdfArray = CType(parent.Resolve(PdfName.Kids), PdfArray)
                Dim _Index As Integer = 0
                Dim i As Integer = 0
                While (True)
                    Dim kidReference As PdfReference = CType(kids(i), PdfReference)
                    '// Is the current-level counting complete?
                    '// NOTE: It 's complete when it reaches the ancestor at this level.
                    If (kidReference.Equals(ancestorKidReference)) Then ' Ancestor Then node.
                        ' Does the current level correspond to the page-tree root node?
                        If (Not parent.ContainsKey(PdfName.Parent)) Then
                            ' We reached the top: counting 's finished.
                            Return _Index
                        End If
                        ' Set the ancestor at the next level!
                        ancestorKidReference = parentReference
                        ' Move up one level!
                        parentReference = CType(parent(PdfName.Parent), PdfReference)
                        parent = CType(parentReference.DataObject, PdfDictionary)
                        kids = CType(parent.Resolve(PdfName.Kids), PdfArray)
                        i = -1
                    Else ' Intermediate node.
                        Dim kid As PdfDictionary = CType(kidReference.DataObject, PdfDictionary)
                        If (kid(PdfName.Type).Equals(PdfName.Page)) Then
                            _Index += 1
                        Else
                            _Index += CType(kid(PdfName.Count), PdfInteger).RawValue
                        End If
                    End If
                    i += 1
                End While
                Return -1 'non accade mai
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the page size.</summary>
        '*/
        Public Property Size As System.Drawing.SizeF
            Get
                Return Me.Box.Size
            End Get
            Set(ByVal value As System.Drawing.SizeF)
                Dim box As System.Drawing.RectangleF
                Try
                    box = Me.Box
                Catch ex As System.Exception
                    box = New System.Drawing.RectangleF(0, 0, 0, 0)
                End Try
                box.Size = value
                Me.Box = box
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the tab order To be used For annotations On the page.</summary>
        '*/
        <PDF(VersionEnum.PDF15)>
        Public Property TabOrder As TabOrderEnum
            Get
                Return ToTabOrderEnum(CType(BaseDataObject(PdfName.Tabs), PdfName))
            End Get
            Set(ByVal value As TabOrderEnum)
                BaseDataObject(PdfName.Tabs) = ToCode(value)
            End Set
        End Property

        '/**
        '  <summary>Gets the transition effect To be used
        '  when displaying the page during presentations.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Transition As Transition
            Get
                Return Transition.Wrap(BaseDataObject(PdfName.Trans))
            End Get
            Set(ByVal value As Transition)
                BaseDataObject(PdfName.Trans) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the intended dimensions Of the finished page after trimming
        '  [PDF:1.7:10.10.1].</summary>
        '  <remarks>
        '    <para>It may be smaller than the media box To allow For production-related content, such As
        '    printing instructions, cut marks, Or color bars.</para>
        '  </remarks>
        '  <seealso cref = "CropBox" />
        '*/
        <PDF(VersionEnum.PDF13)>
        Public Property TrimBox As System.Drawing.RectangleF
            Get
                '/*
                '  NOTE: The default value Is the page's crop box.
                '*/
                Dim trimBoxObject As PdfDirectObject = GetInheritableAttribute(PdfName.TrimBox)
                If (trimBoxObject IsNot Nothing) Then
                    Return Rectangle.Wrap(trimBoxObject).ToRectangleF()
                Else
                    Return Me.CropBox
                End If
            End Get
            Set(ByVal value As System.Drawing.RectangleF)
                BaseDataObject(PdfName.TrimBox) = New Rectangle(value).BaseDataObject
            End Set
        End Property

#Region "IContentContext"

        Public Property Box As System.Drawing.RectangleF Implements IContentContext.Box
            Get
                Return Rectangle.Wrap(GetInheritableAttribute(PdfName.MediaBox)).ToRectangleF()
            End Get
            Set(ByVal value As System.Drawing.RectangleF)
                BaseDataObject(PdfName.MediaBox) = New Rectangle(value).BaseDataObject
            End Set
        End Property

        Public ReadOnly Property Contents As contents.Contents Implements IContentContext.Contents
            Get
                Dim contentsObject As PdfDirectObject = BaseDataObject(PdfName.Contents)
                If (contentsObject Is Nothing) Then
                    contentsObject = File.Register(New PdfStream())
                    BaseDataObject(PdfName.Contents) = contentsObject
                End If
                Return documents.contents.Contents.Wrap(contentsObject, Me)
            End Get
        End Property

        Public Sub Render(ByVal context As System.Drawing.Graphics, ByVal size As System.Drawing.SizeF) Implements IContentContext.Render
            Dim scanner As ContentScanner = New ContentScanner(Contents)
            scanner.Render(context, size)
        End Sub

        Public ReadOnly Property Resources As Resources Implements IContentContext.Resources
            Get
                Dim _resources As Resources = Resources.Wrap(GetInheritableAttribute(PdfName.Resources))
                If (_resources IsNot Nothing) Then
                    Return _resources
                Else
                    Return Resources.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.Resources))
                End If
            End Get
        End Property

        Public Property Rotation As RotationEnum Implements IContentContext.Rotation
            Get
                Return RotationEnumExtension.Get(CType(GetInheritableAttribute(PdfName.Rotate), PdfInteger))
            End Get
            Set(ByVal value As RotationEnum)
                BaseDataObject(PdfName.Rotate) = PdfInteger.Get(CInt(value))
            End Set
        End Property

#Region "IContentEntity"

        Public Function ToInlineObject(ByVal composer As PrimitiveComposer) As ContentObject Implements IContentEntity.ToInlineObject
            Throw New NotImplementedException()
        End Function

        Public Function ToXObject(ByVal context As Document) As xObjects.XObject Implements IContentEntity.ToXObject
            Dim form As xObjects.FormXObject
            form = New xObjects.FormXObject(context, Box)

            If (context.Equals(Document)) Then
                form.Resources = CType(Me.Resources, Resources) ' Same document: reuses The existing resources.
            Else
                form.Resources = CType(Me.Resources.Clone(context), Resources) ' Alien document: clones The resources.
            End If

            ' Body (contents).

            Dim formBody As IBuffer = form.BaseDataObject.Body
            Dim contentsDataObject As PdfDataObject = BaseDataObject.Resolve(PdfName.Contents)
            If (TypeOf (contentsDataObject) Is PdfStream) Then
                formBody.Append(CType(contentsDataObject, PdfStream).Body)
            Else
                For Each contentStreamObject As PdfDirectObject In CType(contentsDataObject, PdfArray)
                    formBody.Append(CType(contentStreamObject.Resolve(), PdfStream).Body)
                Next
            End If


            Return form
        End Function

#End Region
#End Region
#End Region

#Region "Private"

        Private Function GetInheritableAttribute(ByVal key As PdfName) As PdfDirectObject
            Return GetInheritableAttribute(BaseDataObject, key)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
