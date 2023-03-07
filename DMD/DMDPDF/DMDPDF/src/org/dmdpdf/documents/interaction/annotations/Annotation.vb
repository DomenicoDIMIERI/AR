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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Annotation [PDF:1.6:8.4].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public Class Annotation
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements ILayerable

#Region "types"

        '/**
        '  <summary> Field flags [PDF:1.6:8.4.2].</summary>
        '*/
        <Flags>
        Public Enum FlagsEnum

            '/**
            '  <summary> Hide the annotation, both On screen And On print,
            '  If it does Not belong To one Of the standard annotation types
            '  And no annotation handler Is available.</summary>
            '*/
            Invisible = &H1
            '/**
            '  <summary> Hide the annotation, both On screen And On print
            '  (regardless of its annotation type Or whether an annotation handler Is available).</summary>
            '*/
            Hidden = &H2
            '/**
            '  <summary> Print the annotation When the page Is printed.</summary>
            '*/
            Print = &H4
            '/**
            '  <summary>Do Not scale the annotation's appearance to match the magnification of the page.</summary>
            '*/
            NoZoom = &H8
            '/**
            '  <summary>Do Not rotate the annotation's appearance to match the rotation of the page.</summary>
            '*/
            NoRotate = &H10
            '/**
            '  <summary> Hide the annotation On the screen.</summary>
            '*/
            NoView = &H20
            '/**
            '  <summary>Do Not allow the annotation To interact With the user.</summary>
            '*/
            [ReadOnly] = &H40
            '/**
            '  <summary>Do Not allow the annotation To be deleted Or its properties To be modified by the user.</summary>
            '*/
            Locked = &H80
            '/**
            '  <summary> Invert the interpretation Of the NoView flag.</summary>
            '*/
            ToggleNoView = &H100
        End Enum

#End Region

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary> Wraps an annotation base Object into an annotation Object.</summary>
        '  <param name = "baseObject" > Annotation base Object.</param>
        '  <returns> Annotation Object associated To the base Object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Annotation
            If (baseObject Is Nothing) Then Return Nothing
            Dim annotationType As PdfName = CType(CType(baseObject.Resolve(), PdfDictionary)(PdfName.Subtype), PdfName)
            If (annotationType.Equals(PdfName.Text)) Then
                Return New Note(baseObject)
            ElseIf (annotationType.Equals(PdfName.Link)) Then
                Return New Link(baseObject)
            ElseIf (annotationType.Equals(PdfName.FreeText)) Then
                Return New CalloutNote(baseObject)
            ElseIf (annotationType.Equals(PdfName.Line)) Then
                Return New Line(baseObject)
            ElseIf (annotationType.Equals(PdfName.Square)) Then
                Return New Rectangle(baseObject)
            ElseIf (annotationType.Equals(PdfName.Circle)) Then
                Return New Ellipse(baseObject)
            ElseIf (annotationType.Equals(PdfName.Polygon)) Then
                Return New Polygon(baseObject)
            ElseIf (annotationType.Equals(PdfName.PolyLine)) Then
                Return New Polyline(baseObject)
            ElseIf (annotationType.Equals(PdfName.Highlight) OrElse
                    annotationType.Equals(PdfName.Underline) OrElse
                    annotationType.Equals(PdfName.Squiggly) OrElse
                    annotationType.Equals(PdfName.StrikeOut)) Then
                Return New TextMarkup(baseObject)
            ElseIf (annotationType.Equals(PdfName.Stamp)) Then
                Return New RubberStamp(baseObject)
            ElseIf (annotationType.Equals(PdfName.Caret)) Then
                Return New Caret(baseObject)
            ElseIf (annotationType.Equals(PdfName.Ink)) Then
                Return New Scribble(baseObject)
            ElseIf (annotationType.Equals(PdfName.Popup)) Then
                Return New Popup(baseObject)
            ElseIf (annotationType.Equals(PdfName.FileAttachment)) Then
                Return New FileAttachment(baseObject)
            ElseIf (annotationType.Equals(PdfName.Sound)) Then
                Return New Sound(baseObject)
            ElseIf (annotationType.Equals(PdfName.Movie)) Then
                Return New Movie(baseObject)
            ElseIf (annotationType.Equals(PdfName.Widget)) Then
                Return New Widget(baseObject)
            ElseIf (annotationType.Equals(PdfName.Screen)) Then
                Return New Screen(baseObject)
                '//TODO
                '//     else if(annotationType.Equals(PdfName.PrinterMark)) return New PrinterMark(baseObject);
                '//     else if(annotationType.Equals(PdfName.TrapNet)) return New TrapNet(baseObject);
                '//     else if(annotationType.Equals(PdfName.Watermark)) return New Watermark(baseObject);
                '//     else if(annotationType.Equals(PdfName.3DAnnotation)) return New 3DAnnotation(baseObject);
            Else ' Other annotation type.
                Return New Annotation(baseObject)
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '' NOTE: Hide border by Default.
        Protected Sub New(ByVal page As Page, ByVal subtype As PdfName, ByVal box As RectangleF, ByVal text As String)
            MyBase.New(page.Document,
                       New PdfDictionary(
                                New PdfName() {PdfName.Type, PdfName.Subtype, PdfName.Border},
                                New PdfDirectObject() {
                                            PdfName.Annot,
                                            subtype,
                                            New PdfArray(
                                                New PdfDirectObject() {
                                                    PdfInteger.Default,
                                                    PdfInteger.Default,
                                                    PdfInteger.Default}
                                                 )
                                            }
                                      )
                                    )
            page.Annotations.Add(Me)
            Me.Box = box
            Me.Text = text
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets action To be performed When the annotation Is activated.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Overridable Property Action As actions.Action
            Get
                Return interaction.actions.Action.Wrap(BaseDataObject(PdfName.A))
            End Get
            Set(ByVal value As actions.Action)
                Me.BaseDataObject(PdfName.A) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the annotation's behavior in response to various trigger events.</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Overridable Property Actions As AnnotationActions
            Get
                Return New AnnotationActions(Me, BaseDataObject.Get(Of PdfDictionary)(PdfName.AA))
            End Get
            Set(ByVal value As AnnotationActions)
                Me.BaseDataObject(PdfName.AA) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the appearance specifying how the annotation Is presented visually On the page.</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Property Appearance As Appearance
            Get
                Return Appearance.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.AP))
            End Get
            Set(ByVal value As Appearance)
                Me.BaseDataObject(PdfName.AP) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the border style.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Border As Border
            Get
                Return New Border(BaseDataObject.Get(Of PdfDictionary)(PdfName.BS))
            End Get
            Set(ByVal value As Border)
                Me.BaseDataObject(PdfName.BS) = PdfObjectWrapper.GetBaseObject(value)
                If (value IsNot Nothing) Then
                    Me.BaseDataObject.Remove(PdfName.Border)
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the location Of the annotation On the page In Default user space units.
        '  </summary>
        '*/
        Public Property Box As RectangleF
            Get
                Dim _box As org.dmdpdf.objects.Rectangle = org.dmdpdf.objects.Rectangle.Wrap(BaseDataObject(PdfName.Rect))
                Return New RectangleF(
                                  CSng(_box.Left),
                                  CSng(GetPageHeight() - _box.Top),
                                  CSng(_box.Width),
                                  CSng(_box.Height)
                                  )
            End Get
            Set(ByVal value As RectangleF)
                Me.BaseDataObject(PdfName.Rect) = New org.dmdpdf.objects.Rectangle(
                                                                        value.X,
                                                                        GetPageHeight() - value.Y,
                                                                        value.Width,
                                                                        value.Height
                                                                        ).BaseDataObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the annotation color.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Color As DeviceColor
            Get
                Return DeviceColor.Get(CType(BaseDataObject(PdfName.C), PdfArray))
            End Get
            Set(ByVal value As DeviceColor)
                Me.BaseDataObject(PdfName.C) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Deletes Me annotation removing also its reference On the page.</summary>
        '*/
        Public Overrides Function Delete() As Boolean
            '// Shallow removal (references):
            '// * reference on page
            Me.Page.Annotations.Remove(Me)
            ' Deep removal (indirect object).
            Return MyBase.Delete()
        End Function

        '/**
        '  <summary> Gets/Sets the annotation flags.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Flags As FlagsEnum
            Get
                Dim flagsObject As PdfInteger = CType(Me.BaseDataObject(PdfName.F), PdfInteger)
                If (flagsObject Is Nothing) Then
                    Return 0
                Else
                    Return CType([Enum].ToObject(GetType(FlagsEnum), flagsObject.RawValue), FlagsEnum)
                End If
            End Get
            Set(ByVal value As FlagsEnum)
                Me.BaseDataObject(PdfName.F) = PdfInteger.Get(CInt(value))
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the Date And time When the annotation was most recently modified.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property ModificationDate As DateTime?
            Get
                '/*
                '  NOTE: Despite PDF Date being the preferred format, loose formats are tolerated by the spec.
                '*/
                Dim modificationDateObject As PdfDirectObject = BaseDataObject(PdfName.M)
                If (TypeOf (modificationDateObject) Is PdfDate) Then
                    Return CType(CType(modificationDateObject, PdfDate).Value, DateTime?)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As DateTime?)
                Me.BaseDataObject(PdfName.M) = PdfDate.Get(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the annotation name.</summary>
        '  <remarks> The annotation name uniquely identifies the annotation among all the annotations On its page.</remarks>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property Name As String
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(Me.BaseDataObject(PdfName.NM)))
            End Get
            Set(ByVal value As String)
                Me.BaseDataObject(PdfName.NM) = PdfTextString.Get(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the associated page.</summary>
        '*/
        <PDF(VersionEnum.PDF13)>
        Public ReadOnly Property Page As Page
            Get
                Return Page.Wrap(Me.BaseDataObject(PdfName.P))
            End Get
        End Property

        '/**
        '  <summary> Gets/Sets whether To print the annotation When the page Is printed.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Printable As Boolean
            Get
                Return (Me.Flags And FlagsEnum.Print) = FlagsEnum.Print
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.Print, value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the annotation text.</summary>
        '  <remarks> Depending On the annotation type, the text may be either directly displayed
        '  Or (in case of non-textual annotations) used as alternate description.</remarks>
        '*/
        Public Property Text As String
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(Me.BaseDataObject(PdfName.Contents)))
            End Get
            Set(ByVal value As String)
                If (value Is Nothing) Then
                    Me.BaseDataObject.Remove(PdfName.Contents)
                Else
                    Me.BaseDataObject(PdfName.Contents) = PdfTextString.Get(value)
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets whether the annotation Is visible.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Visible As Boolean
            Get
                Return (Me.Flags And FlagsEnum.Hidden) <> FlagsEnum.Hidden
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.Hidden, Not value)
            End Set
        End Property

#Region "ILayerable"

        <PDF(VersionEnum.PDF15)>
        Public Property Layer As LayerEntity Implements ILayerable.Layer
            Get
                Return CType(PropertyList.Wrap(Me.BaseDataObject(PdfName.OC)), LayerEntity)
            End Get
            Set(ByVal value As LayerEntity)
                Me.BaseDataObject(PdfName.OC) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

#End Region
#End Region

#Region "Private"

        Private Function GetPageHeight() As Single
            Dim page As Page = Me.Page
            If (page IsNot Nothing) Then
                Return page.Box.Height
            Else
                Return Me.Document.GetSize().Height
            End If
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace