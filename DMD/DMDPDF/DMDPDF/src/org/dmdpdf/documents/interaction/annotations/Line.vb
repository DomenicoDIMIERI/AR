'/*
'  Copyright 2008 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

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

'  You should have received a copy Of the GNU Lesser General Public License along With this
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  this list Of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary> Line annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks> It displays displays a Single straight line On the page.
    '  When opened, it displays a pop-up window containing the text of the associated note.</remarks>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class Line
        Inherits Annotation

#Region "types"
        '/**
        '  <summary> Line ending style [PDF:1.6:8.4.5].</summary>
        '*/
        Public Enum LineEndStyleEnum
            '/**
            '  Square.
            '*/
            Square
            '/**
            '  Circle.
            '*/
            Circle
            '/**
            '  Diamond.
            '*/
            Diamond
            '/**
            '  Open arrow.
            '*/
            OpenArrow
            '/**
            '  Closed arrow.
            '*/
            ClosedArrow
            '/**
            '  None.
            '*/
            None
            '/**
            '  Butt.
            '*/
            Butt
            '/**
            '  Reverse open arrow.
            '*/
            ReverseOpenArrow
            '/**
            '  Reverse closed arrow.
            '*/
            ReverseClosedArrow
            '/**
            '  Slash.
            '*/
            Slash
        End Enum
#End Region

#Region "shared"
#Region "fields"

        Private Shared ReadOnly _DefaultLeaderLineExtensionLength As Double = 0
        Private Shared ReadOnly _DefaultLeaderLineLength As Double = 0
        Private Shared ReadOnly _DefaultLineEndStyle As LineEndStyleEnum = LineEndStyleEnum.None

        Private Shared ReadOnly _LineEndStyleEnumCodes As Dictionary(Of LineEndStyleEnum, PdfName)
#End Region

#Region "constructors"
        Shared Sub New()
            _LineEndStyleEnumCodes = New Dictionary(Of LineEndStyleEnum, PdfName)()
            _LineEndStyleEnumCodes(LineEndStyleEnum.Square) = PdfName.Square
            _LineEndStyleEnumCodes(LineEndStyleEnum.Circle) = PdfName.Circle
            _LineEndStyleEnumCodes(LineEndStyleEnum.Diamond) = PdfName.Diamond
            _LineEndStyleEnumCodes(LineEndStyleEnum.OpenArrow) = PdfName.OpenArrow
            _LineEndStyleEnumCodes(LineEndStyleEnum.ClosedArrow) = PdfName.ClosedArrow
            _LineEndStyleEnumCodes(LineEndStyleEnum.None) = PdfName.None
            _LineEndStyleEnumCodes(LineEndStyleEnum.Butt) = PdfName.Butt
            _LineEndStyleEnumCodes(LineEndStyleEnum.ReverseOpenArrow) = PdfName.ROpenArrow
            _LineEndStyleEnumCodes(LineEndStyleEnum.ReverseClosedArrow) = PdfName.RClosedArrow
            _LineEndStyleEnumCodes(LineEndStyleEnum.Slash) = PdfName.Slash
        End Sub

#End Region

#Region "interface"
#Region "private"
        '/**
        '  <summary> Gets the code corresponding To the given value.</summary>
        '*/
        Private Shared Function ToCode(ByVal value As LineEndStyleEnum) As PdfName
            Return _LineEndStyleEnumCodes(value)
        End Function

        '/**
        '  <summary> Gets the line ending style corresponding To the given value.</summary>
        '*/
        Private Shared Function ToLineEndStyleEnum(ByVal value As PdfName) As LineEndStyleEnum
            For Each style As KeyValuePair(Of LineEndStyleEnum, PdfName) In _LineEndStyleEnumCodes
                If (style.Value.Equals(value)) Then
                    Return style.Key
                End If
            Next
            Return _DefaultLineEndStyle
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal startPoint As PointF, ByVal endPoint As PointF, ByVal text As String)
            MyBase.New(
                        page,
                        PdfName.Line,
                        New RectangleF(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y),
                        text
                        )

            Me.BaseDataObject(PdfName.L) = New PdfArray(New PdfDirectObject() {PdfReal.Get(0), PdfReal.Get(0), PdfReal.Get(0), PdfReal.Get(0)})
            Me.startPoint = startPoint
            Me.endPoint = endPoint
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets/Sets whether the contents should be shown As a caption.</summary>
        '*/
        Public Property CaptionVisible As Boolean
            Get
                Dim captionVisibleObject As PdfBoolean = CType(BaseDataObject(PdfName.Cap), PdfBoolean)
                If (captionVisibleObject IsNot Nothing) Then
                    Return captionVisibleObject.BooleanValue
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                BaseDataObject(PdfName.Cap) = PdfBoolean.Get(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the ending coordinates.</summary>
        '*/
        Public Property EndPoint As PointF
            Get
                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
                Return New PointF(CSng(CType(coordinatesObject(2), IPdfNumber).RawValue), CSng(CType(coordinatesObject(3), IPdfNumber).RawValue))
            End Get
            Set(ByVal value As PointF)
                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
                coordinatesObject(2) = PdfReal.Get(value.X)
                coordinatesObject(3) = PdfReal.Get(Page.Box.Height - value.Y)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the style Of the ending line ending.</summary>
        '*/
        Public Property EndStyle As LineEndStyleEnum
            Get
                Dim endstylesObject As PdfArray = CType(BaseDataObject(PdfName.LE), PdfArray)
                If (endstylesObject IsNot Nothing) Then
                    Return ToLineEndStyleEnum(CType(endstylesObject(1), PdfName))
                Else
                    Return _DefaultLineEndStyle
                End If
            End Get
            Set(ByVal value As LineEndStyleEnum)
                EnsureLineEndStylesObject()(1) = ToCode(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the color With which To fill the interior Of the annotation's line endings.</summary>
        '*/
        Public Property FillColor As DeviceRGBColor
            Get
                Dim fillColorObject As PdfArray = CType(BaseDataObject(PdfName.IC), PdfArray)
                If (fillColorObject Is Nothing) Then Return Nothing
                'TODO:use BaseObject constructor!!!
                Return New DeviceRGBColor(CType(fillColorObject(0), IPdfNumber).RawValue, CType(fillColorObject(1), IPdfNumber).RawValue, CType(fillColorObject(2), IPdfNumber).RawValue)
            End Get
            Set(ByVal value As DeviceRGBColor)
                BaseDataObject(PdfName.IC) = CType(value.BaseDataObject, PdfDirectObject)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the length Of leader line extensions that extend
        '  in the opposite direction from the leader lines.</summary>
        '*/
        Public Property LeaderLineExtensionLength As Double
            Get
                Dim leaderLineExtensionLengthObject As IPdfNumber = CType(BaseDataObject(PdfName.LLE), IPdfNumber)
                If (leaderLineExtensionLengthObject IsNot Nothing) Then
                    Return leaderLineExtensionLengthObject.RawValue
                Else
                    Return _DefaultLeaderLineExtensionLength
                End If
            End Get
            Set(ByVal value As Double)
                BaseDataObject(PdfName.LLE) = PdfReal.Get(value)
                '/*
                '  NOTE: If leader Then line extension entry Is present, leader line MUST be too.
                '*/
                If (Not BaseDataObject.ContainsKey(PdfName.LL)) Then
                    LeaderLineLength = _DefaultLeaderLineLength
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the length Of leader lines that extend from Each endpoint
        '  of the line perpendicular to the line itself.</summary>
        '  <remarks> A positive value means that the leader lines appear In the direction
        '  that Is clockwise when traversing the line from its starting point
        '  to its ending point; a negative value indicates the opposite direction.</remarks>
        '*/
        Public Property LeaderLineLength As Double
            Get
                Dim leaderLineLengthObject As IPdfNumber = CType(BaseDataObject(PdfName.LL), IPdfNumber)
                If (leaderLineLengthObject IsNot Nothing) Then
                    Return -leaderLineLengthObject.RawValue
                Else
                    Return _DefaultLeaderLineLength
                End If
            End Get
            Set(ByVal value As Double)
                BaseDataObject(PdfName.LL) = PdfReal.Get(-value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the starting coordinates.</summary>
        '*/
        Public Property StartPoint As PointF
            Get
                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
                Return New PointF(CSng(CType(coordinatesObject(0), IPdfNumber).RawValue), CSng(CType(coordinatesObject(1), IPdfNumber).RawValue))
            End Get
            Set(ByVal value As PointF)
                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
                coordinatesObject(0) = PdfReal.Get(value.X)
                coordinatesObject(1) = PdfReal.Get(Page.Box.Height - value.Y)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the style Of the starting line ending.</summary>
        '*/
        Public Property StartStyle As LineEndStyleEnum
            Get
                Dim endstylesObject As PdfArray = CType(BaseDataObject(PdfName.LE), PdfArray)
                If (endstylesObject IsNot Nothing) Then
                    Return ToLineEndStyleEnum(CType(endstylesObject(0), PdfName))
                Else
                    Return _DefaultLineEndStyle
                End If
            End Get
            Set(ByVal value As LineEndStyleEnum)
                EnsureLineEndStylesObject()(0) = ToCode(value)
            End Set
        End Property

#End Region

#Region "Private"

        Private Function EnsureLineEndStylesObject() As PdfArray
            Dim endStylesObject As PdfArray = CType(BaseDataObject(PdfName.LE), PdfArray)
            If (endStylesObject Is Nothing) Then
                endStylesObject = New PdfArray(New PdfDirectObject() {ToCode(_DefaultLineEndStyle), ToCode(_DefaultLineEndStyle)})
                BaseDataObject(PdfName.LE) = endStylesObject
            End If
            Return endStylesObject
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace

''/*
''  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

''  This file should be part of the source code distribution of "PDF Clown library" (the
''  Program): see the accompanying README files for more info.

''  This Program is free software; you can redistribute it and/or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 of the License, or (at your option) any later version.

''  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy of the GNU Lesser General Public License along with this
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  this list of conditions.
''*/

'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.documents
'Imports DMD.org.dmdpdf.documents.contents.colorSpaces
'Imports DMD.org.dmdpdf.objects

'Imports System
'Imports System.Collections.Generic
'Imports System.Drawing

'Namespace DMD.org.dmdpdf.documents.interaction.annotations

'    '/**
'    '  <summary>Line annotation [PDF:1.6:8.4.5].</summary>
'    '  <remarks>It displays displays a single straight line on the page.
'    '  When opened, it displays a pop-up window containing the text of the associated note.</remarks>
'    '*/
'    <PDF(VersionEnum.PDF13)>
'    Public NotInheritable Class Line
'        Inherits Annotation

'#Region "types"
'        '/**
'        '  <summary>Line ending style [PDF:1.6:8.4.5].</summary>
'        '*/
'        Public Enum LineEndStyleEnum

'            '  /**
'            '  Square.
'            '*/
'            Square
'            '/**
'            '  Circle.
'            '*/
'            Circle
'            '/**
'            '  Diamond.
'            '*/
'            Diamond
'            '/**
'            '  Open arrow.
'            '*/
'            OpenArrow
'            '/**
'            '  Closed arrow.
'            '*/
'            ClosedArrow
'            '/**
'            '  None.
'            '*/
'            None
'            '/**
'            '  Butt.
'            '*/
'            Butt
'            '/**
'            '  Reverse open arrow.
'            '*/
'            ReverseOpenArrow
'            '/**
'            '  Reverse closed arrow.
'            '*/
'            ReverseClosedArrow
'            '/**
'            '  Slash.
'            '*/
'            Slash
'        End Enum

'#End Region

'#Region "shared"
'#Region "fields"

'        Private Shared ReadOnly _DefaultLeaderLineExtensionLength As Double = 0
'        Private Shared ReadOnly _DefaultLeaderLineLength As Double = 0
'        Private Shared ReadOnly _DefaultLineEndStyle As LineEndStyleEnum = LineEndStyleEnum.None

'        Private Shared ReadOnly _LineEndStyleEnumCodes As Dictionary(Of LineEndStyleEnum, PdfName)

'#End Region

'#Region "constructors"

'        Shared Sub New()
'            _LineEndStyleEnumCodes = New Dictionary(Of LineEndStyleEnum, PdfName)
'            _LineEndStyleEnumCodes(LineEndStyleEnum.Square) = PdfName.Square
'            _LineEndStyleEnumCodes(LineEndStyleEnum.Circle) = PdfName.Circle
'            _LineEndStyleEnumCodes(LineEndStyleEnum.Diamond) = PdfName.Diamond
'            _LineEndStyleEnumCodes(LineEndStyleEnum.OpenArrow) = PdfName.OpenArrow
'            _LineEndStyleEnumCodes(LineEndStyleEnum.ClosedArrow) = PdfName.ClosedArrow
'            _LineEndStyleEnumCodes(LineEndStyleEnum.None) = PdfName.None
'            _LineEndStyleEnumCodes(LineEndStyleEnum.Butt) = PdfName.Butt
'            _LineEndStyleEnumCodes(LineEndStyleEnum.ReverseOpenArrow) = PdfName.ROpenArrow
'            _LineEndStyleEnumCodes(LineEndStyleEnum.ReverseClosedArrow) = PdfName.RClosedArrow
'            _LineEndStyleEnumCodes(LineEndStyleEnum.Slash) = PdfName.Slash
'        End Sub

'#End Region

'#Region "interface"
'#Region "private"

'        '/**
'        '  <summary>Gets the code corresponding To the given value.</summary>
'        '*/
'        Private Shared Function ToCode(ByVal value As LineEndStyleEnum) As PdfName
'            Return _LineEndStyleEnumCodes(value)
'        End Function

'        '/**
'        '  <summary>Gets the line ending style corresponding to the given value.</summary>
'        '*/
'        Private Shared Function ToLineEndStyleEnum(ByVal value As PdfName) As LineEndStyleEnum
'            For Each style As KeyValuePair(Of LineEndStyleEnum, PdfName) In _LineEndStyleEnumCodes
'                If (style.Value.Equals(value)) Then
'                    Return style.Key
'                End If
'            Next
'            Return _DefaultLineEndStyle
'        End Function

'#End Region
'#End Region
'#End Region

'#Region "dynamic"
'#Region "constructors"

'        Public Sub New(ByVal page As Page, ByVal startPoint As PointF, ByVal endPoint As PointF, ByVal text As String)
'            MyBase.New(page, PdfName.Line, New RectangleF(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y), text)
'            BaseDataObject(PdfName.L) = New PdfArray(New PdfDirectObject() {PdfReal.Get(0), PdfReal.Get(0), PdfReal.Get(0), PdfReal.Get(0)})
'            startPoint = startPoint
'            endPoint = endPoint
'        End Sub

'        Friend Sub New(ByVal baseObject As PdfDirectObject)
'            MyBase.New(baseObject)
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"

'        '/**
'        '  <summary>Gets/Sets whether the contents should be shown as a caption.</summary>
'        '*/
'        Public Property CaptionVisible As Boolean
'            Get
'                Dim captionVisibleObject As PdfBoolean = CType(BaseDataObject(PdfName.Cap), PdfBoolean)
'                If (captionVisibleObject IsNot Nothing) Then
'                    Return captionVisibleObject.BooleanValue
'                Else
'                    Return False
'                End If
'            End Get
'            Set(ByVal value As Boolean)
'                BaseDataObject(PdfName.Cap) = PdfBoolean.Get(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the ending coordinates.</summary>
'        '*/
'        Public Property EndPoint As PointF
'            Get
'                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
'                Return New PointF(
'                                CSng(CType(coordinatesObject(2), IPdfNumber).RawValue),
'                                CSng(CType(coordinatesObject(3), IPdfNumber).RawValue)
'                                )
'            End Get
'            Set(ByVal value As PointF)
'                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
'                coordinatesObject(2) = PdfReal.Get(value.X)
'                coordinatesObject(3) = PdfReal.Get(Page.Box.Height - value.Y)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the style Of the ending line ending.</summary>
'        '*/
'        Public Property EndStyle As LineEndStyleEnum
'            Get
'                Dim endstylesObject As PdfArray = CType(BaseDataObject(PdfName.LE), PdfArray)
'                If (endstylesObject IsNot Nothing) Then
'                    Return ToLineEndStyleEnum(CType(endstylesObject(1), PdfName))
'                Else
'                    Return _DefaultLineEndStyle
'                End If
'            End Get
'            Set(ByVal value As LineEndStyleEnum)
'                EnsureLineEndStylesObject()(1) = ToCode(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the color With which To fill the interior Of the annotation's line endings.</summary>
'        '*/
'        Public Property FillColor As DeviceRGBColor
'            Get
'                Dim fillColorObject As PdfArray = CType(BaseDataObject(PdfName.IC), PdfArray)
'                If (fillColorObject Is Nothing) Then Return Nothing
'                'TODO: use BaseObject constructor!!!
'                Return New DeviceRGBColor(
'                              CType(fillColorObject(0), IPdfNumber).RawValue,
'                              CType(fillColorObject(1), IPdfNumber).RawValue,
'                              CType(fillColorObject(2), IPdfNumber).RawValue
'                              )
'            End Get
'            Set(ByVal value As DeviceRGBColor)
'                BaseDataObject(PdfName.IC) = CType(value.BaseDataObject, PdfDirectObject)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the length Of leader line extensions that extend
'        '  in the opposite direction from the leader lines.</summary>
'        '*/
'        Public Property LeaderLineExtensionLength As Double
'            Get
'                Dim leaderLineExtensionLengthObject As IPdfNumber = CType(BaseDataObject(PdfName.LLE), IPdfNumber)
'                If (leaderLineExtensionLengthObject IsNot Nothing) Then
'                    Return leaderLineExtensionLengthObject.RawValue
'                Else
'                    Return _DefaultLeaderLineExtensionLength
'                End If
'            End Get
'            Set(ByVal value As Double)
'                BaseDataObject(PdfName.LLE) = PdfReal.Get(value)
'                '/*
'                '  NOTE: If leader Then line extension entry Is present, leader line MUST be too.
'                '*/
'                If (Not BaseDataObject.ContainsKey(PdfName.LL)) Then
'                    Me.LeaderLineLength = _DefaultLeaderLineLength
'                End If
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the length Of leader lines that extend from Each endpoint
'        '  of the line perpendicular to the line itself.</summary>
'        '  <remarks>A positive value means that the leader lines appear In the direction
'        '  that Is clockwise when traversing the line from its starting point
'        '  to its ending point; a negative value indicates the opposite direction.</remarks>
'        '*/
'        Public Property LeaderLineLength As Double
'            Get
'                Dim leaderLineLengthObject As IPdfNumber = CType(BaseDataObject(PdfName.LL), IPdfNumber)
'                If (leaderLineLengthObject IsNot Nothing) Then
'                    Return -leaderLineLengthObject.RawValue
'                Else
'                    Return _DefaultLeaderLineLength
'                End If
'            End Get
'            Set(ByVal value As Double)
'                BaseDataObject(PdfName.LL) = PdfReal.Get(-value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the starting coordinates.</summary>
'        '*/
'        Public Property StartPoint As PointF
'            Get
'                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
'                Return New PointF(
'                        CSng(CType(coordinatesObject(0), IPdfNumber).RawValue),
'                        CSng(CType(coordinatesObject(1), IPdfNumber).RawValue)
'                            )
'            End Get
'            Set(ByVal value As PointF)
'                Dim coordinatesObject As PdfArray = CType(BaseDataObject(PdfName.L), PdfArray)
'                coordinatesObject(0) = PdfReal.Get(value.X)
'                coordinatesObject(1) = PdfReal.Get(Page.Box.Height - value.Y)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the style Of the starting line ending.</summary>
'        '*/
'        Public Property StartStyle As LineEndStyleEnum
'            Get
'                Dim endstylesObject As PdfArray = CType(BaseDataObject(PdfName.LE), PdfArray)
'                If (endstylesObject IsNot Nothing) Then
'                    Return ToLineEndStyleEnum(CType(endstylesObject(0), PdfName))
'                Else
'                    Return _DefaultLineEndStyle
'                End If
'            End Get
'            Set(ByVal value As LineEndStyleEnum)
'                EnsureLineEndStylesObject()(0) = ToCode(value)
'            End Set
'        End Property

'#End Region

'#Region "Private"

'        Private Function EnsureLineEndStylesObject() As PdfArray
'            Dim endStylesObject As PdfArray = CType(BaseDataObject(PdfName.LE), PdfArray)
'            If (endStylesObject Is Nothing) Then
'                endStylesObject = New PdfArray(
'                                           New PdfDirectObject() {
'                                                        ToCode(_DefaultLineEndStyle),
'                                                        ToCode(_DefaultLineEndStyle)
'                                                        }
'                                               )
'                BaseDataObject(PdfName.LE) = endStylesObject
'            End If
'            Return endStylesObject
'        End Function

'#End Region
'#End Region
'#End Region

'    End Class


'End Namespace