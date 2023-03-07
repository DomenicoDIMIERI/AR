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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.interaction.navigation.document

    '/**
    '  <summary>Interaction target [PDF:1.6:8.2.1].</summary>
    '  <remarks>
    '    It represents a particular view of a document, consisting of the following items:
    '    <list type="bullet">
    '      <item>the page of the document to be displayed;</item>
    '      <item>the location of the document window on that page;</item>
    '      <item>the magnification (zoom) factor to use when displaying the page.</item>
    '    </list>
    '  </remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class Destination
        Inherits PdfObjectWrapper(Of PdfArray)
        Implements IPdfNamedObjectWrapper

#Region "types"

        '/**
        '  <summary>Destination mode [PDF:1.6:8.2.1].</summary>
        '*/
        Public Enum ModeEnum

            '  /**
            '  <summary>Display the page at the given upper-left position,
            '  applying the given magnification.</summary>
            '  <remarks>
            '    View parameters:
            '    <list type="number">
            '      <item>left coordinate</item>
            '      <item>top coordinate</item>
            '      <item>zoom</item>
            '    </list>
            '  </remarks>
            '*/
            XYZ
            '/**
            '  <summary>Display the page with its contents magnified just enough to fit
            '  the entire page within the window both horizontally and vertically.</summary>
            '  <remarks>No view parameters.</remarks>
            '*/
            Fit
            '/**
            '  <summary>Display the page with the vertical coordinate <code>top</code> positioned
            '  at the top edge of the window and the contents of the page magnified
            '  just enough to fit the entire width of the page within the window.</summary>
            '  <remarks>
            '    View parameters:
            '    <list type="number">
            '      <item>top coordinate</item>
            '    </list>
            '  </remarks>
            '*
            FitHorizontal
            '/**
            '  <summary>Display the page with the horizontal coordinate <code>left</code> positioned
            '  at the left edge of the window and the contents of the page magnified
            '  just enough to fit the entire height of the page within the window.</summary>
            '  <remarks>
            '    View parameters:
            '    <list type="number">
            '      <item>left coordinate</item>
            '    </list>
            '  </remarks>
            '*/
            FitVertical
            '/**
            '  <summary>Display the page with its contents magnified just enough to fit
            '  the rectangle specified by the given coordinates entirely
            '  within the window both horizontally and vertically.</summary>
            '  <remarks>
            '    View parameters:
            '    <list type="number">
            '      <item>left coordinate</item>
            '      <item>bottom coordinate</item>
            '      <item>right coordinate</item>
            '      <item>top coordinate</item>
            '    </list>
            '  </remarks>
            '*/
            FitRectangle
            '/**
            '  <summary>Display the page with its contents magnified just enough to fit
            '  its bounding box entirely within the window both horizontally and vertically.</summary>
            '  <remarks>No view parameters.</remarks>
            '*/
            FitBoundingBox
            '/**
            '  <summary>Display the page with the vertical coordinate <code>top</code> positioned
            '  at the top edge of the window and the contents of the page magnified
            '  just enough to fit the entire width of its bounding box within the window.</summary>
            '  <remarks>
            '    View parameters:
            '    <list type="number">
            '      <item>top coordinate</item>
            '    </list>
            '  </remarks>
            '*/
            FitBoundingBoxHorizontal
            '/**
            '  <summary>Display the page with the horizontal coordinate <code>left</code> positioned
            '  at the left edge of the window and the contents of the page magnified
            '  just enough to fit the entire height of its bounding box within the window.</summary>
            '  <remarks>
            '    View parameters:
            '    <list type="number">
            '      <item>left coordinate</item>
            '    </list>
            '  </remarks>
            '*/
            FitBoundingBoxVertical
        End Enum

#End Region

#Region "static"
#Region "interface"
#Region "public"

        '    /**
        '  <summary>Wraps a destination base object into a destination object.</summary>
        '  <param name="baseObject">Destination base object.</param>
        '  <returns>Destination object associated to the base object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Destination
            If (baseObject Is Nothing) Then Return Nothing
            Dim dataObject As PdfArray = CType(baseObject.Resolve(), PdfArray)
            Dim pageObject As PdfDirectObject = dataObject(0)
            If (TypeOf (pageObject) Is PdfReference) Then
                Return New LocalDestination(baseObject)
            ElseIf (TypeOf (pageObject) Is PdfInteger) Then
                Return New RemoteDestination(baseObject)
            Else
                Throw New ArgumentException("Not a valid destination object.", "baseObject")
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '    /**
        '  <summary>Creates a new destination within the given document context.</summary>
        '  <param name="context">Document context.</param>
        '  <param name="page">Page reference. It may be either a <see cref="Page"/> or a page index (int).
        '  </param>
        '  <param name="mode">Destination mode.</param>
        '  <param name="location">Destination location.</param>
        '  <param name="zoom">Magnification factor to use when displaying the page.</param>
        '*/
        Protected Sub New(ByVal context As documents.Document, ByVal page As Object, ByVal mode As ModeEnum, ByVal location As Object, ByVal zoom As Double?)
            MyBase.New(context, New PdfArray(Nothing, Nothing))
            Me.Page = page
            Me.Mode = mode
            Me.Location = location
            Me.Zoom = zoom
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the page location.</summary>
        '*/
        Public Property Location As Object
            Get
                Select Case (Me.Mode)
                    Case ModeEnum.FitBoundingBoxHorizontal,
                         ModeEnum.FitBoundingBoxVertical,
                         ModeEnum.FitHorizontal,
                         ModeEnum.FitVertical
                        Return PdfSimpleObject(Of Object).GetValue(BaseDataObject(2), Double.NaN)
                    Case ModeEnum.FitRectangle
                        Dim Left As Single = CSng(PdfSimpleObject(Of Object).GetValue(BaseDataObject(2), Double.NaN))
                        Dim top As Single = CSng(PdfSimpleObject(Of Object).GetValue(BaseDataObject(5), Double.NaN))
                        Dim width As Single = CSng(PdfSimpleObject(Of Object).GetValue(BaseDataObject(4), Double.NaN)) - Left
                        Dim height As Single = CSng(PdfSimpleObject(Of Object).GetValue(BaseDataObject(3), Double.NaN)) - top
                        Return New RectangleF(Left, top, width, height)
                    Case ModeEnum.XYZ
                        Return New PointF(
                                    CSng(PdfSimpleObject(Of Object).GetValue(BaseDataObject(2), Double.NaN)),
                                    CSng(PdfSimpleObject(Of Object).GetValue(BaseDataObject(3), Double.NaN))
                                    )
                    Case Else
                        Return Nothing
                End Select
            End Get
            Set(ByVal value As Object)
                Dim baseDataObject As PdfArray = Me.BaseDataObject
                Select Case (Me.Mode)
                    Case ModeEnum.FitBoundingBoxHorizontal,
                         ModeEnum.FitBoundingBoxVertical,
                         ModeEnum.FitHorizontal,
                         ModeEnum.FitVertical
                        baseDataObject(2) = PdfReal.Get(CType(Convert.ToDouble(value), Double?))
                        'break;
                    Case ModeEnum.FitRectangle
                        Dim rectangle As RectangleF = CType(value, RectangleF)
                        baseDataObject(2) = PdfReal.Get(rectangle.X)
                        baseDataObject(3) = PdfReal.Get(rectangle.Y)
                        baseDataObject(4) = PdfReal.Get(rectangle.Right)
                        baseDataObject(5) = PdfReal.Get(rectangle.Bottom)
                        'break;
                    Case ModeEnum.XYZ
                        Dim point As PointF = CType(value, PointF)
                        baseDataObject(2) = PdfReal.Get(point.X)
                        baseDataObject(3) = PdfReal.Get(point.Y)
                        'break;
                    Case Else
                        ' NOOP */

                End Select
            End Set
        End Property

        '/**
        '  <summary>Gets the destination mode.</summary>
        '*/
        Public Property Mode As ModeEnum
            Get
                Return ModeEnumExtension.Get(CType(Me.BaseDataObject(1), PdfName)).Value
            End Get
            Set(ByVal value As ModeEnum)
                Dim baseDataObject As PdfArray = Me.BaseDataObject
                baseDataObject(1) = value.GetName()
                ' Adjusting parameter list...
                Dim parametersCount As Integer
                Select Case (value)
                    Case ModeEnum.Fit,
                        ModeEnum.FitBoundingBox
                        parametersCount = 2
                        'break;
                    Case ModeEnum.FitBoundingBoxHorizontal,
                         ModeEnum.FitBoundingBoxVertical,
                         ModeEnum.FitHorizontal,
                         ModeEnum.FitVertical
                        parametersCount = 3
                        'break;
                    Case ModeEnum.XYZ
                        parametersCount = 5
                        'break;
                    Case ModeEnum.FitRectangle
                        parametersCount = 6
                        'break;
                    Case Else
                        Throw New NotSupportedException("Mode unknown: " & value)
                End Select
                While (baseDataObject.Count < parametersCount)
                    baseDataObject.Add(Nothing)
                End While

                While (baseDataObject.Count > parametersCount)
                    baseDataObject.RemoveAt(baseDataObject.Count - 1)
                End While
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the target page reference.</summary>
        '*/
        Public MustOverride Property Page As Object

        '/**
        '  <summary>Gets the magnification factor To use When displaying the page.</summary>
        '*/
        Public Property Zoom As Double?
            Get
                Select Case (Me.Mode)
                    Case ModeEnum.XYZ
                        Return CType(PdfSimpleObject(Of Object).GetValue(BaseDataObject(4)), Double?)
                    Case Else
                        Return Nothing
                End Select
            End Get
            Set(ByVal value As Double?)
                Select Case (Me.Mode)
                    Case ModeEnum.XYZ
                        BaseDataObject(4) = PdfReal.Get(value)
                        'break;
                    Case Else
                        ' NOOP */

                End Select
            End Set
        End Property

#Region "IPdfNamedObjectWrapper"

        Public ReadOnly Property Name As PdfString Implements IPdfNamedObjectWrapper.Name
            Get
                Return RetrieveName()
            End Get
        End Property

        Public ReadOnly Property NamedBaseObject As PdfDirectObject Implements IPdfNamedObjectWrapper.NamedBaseObject
            Get
                Return RetrieveNamedBaseObject()
            End Get
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class


    Module ModeEnumExtension 'Static 

        Private ReadOnly codes As BiDictionary(Of Destination.ModeEnum, PdfName)

        Sub New()
            codes = New BiDictionary(Of Destination.ModeEnum, PdfName)
            codes(Destination.ModeEnum.Fit) = PdfName.Fit
            codes(Destination.ModeEnum.FitBoundingBox) = PdfName.FitB
            codes(Destination.ModeEnum.FitBoundingBoxHorizontal) = PdfName.FitBH
            codes(Destination.ModeEnum.FitBoundingBoxVertical) = PdfName.FitBV
            codes(Destination.ModeEnum.FitHorizontal) = PdfName.FitH
            codes(Destination.ModeEnum.FitRectangle) = PdfName.FitR
            codes(Destination.ModeEnum.FitVertical) = PdfName.FitV
            codes(Destination.ModeEnum.XYZ) = PdfName.XYZ
        End Sub

        Public Function [Get](ByVal name As PdfName) As Destination.ModeEnum?
            If (name Is Nothing) Then Return Nothing
            Dim mode As Destination.ModeEnum? = codes.GetKey(name)
            If (Not mode.HasValue) Then Throw New NotSupportedException("Mode unknown: " & name.ToString)
            Return mode
        End Function

        <Extension>
        Public Function GetName(ByVal mode As Destination.ModeEnum) As PdfName  'this 
            Return codes(mode)
        End Function

    End Module

End Namespace