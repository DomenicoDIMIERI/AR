'/*
'  Copyright 2007 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports System.Text

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary> Content objects scanner.</summary>
    '  <remarks>
    '  <para> It wraps the <see cref="Contents">content objects collection</see> To scan its graphics state
    '    through a forward cursor.</para>
    '    <para> Scanning Is performed at an arbitrary deepness, according To the content objects nesting:
    '    each depth level corresponds to a scan level so that at any time it's possible to seamlessly
    '    navigate across the levels (see <see cref="ParentLevel"/>, <see cref="ChildLevel"/>).</para>
    '  </remarks>
    '*/
    Public NotInheritable Class ContentScanner

#Region "delegates"
        '/**
        '  <summary>Handles the scan start notification.</summary>
        '  <param name = "scanner" > Content scanner started.</param>
        '*/
        Public Delegate Sub StartEventHandler(ByVal scanner As ContentScanner)

#End Region

#Region "events"

        '    /**
        '  <summary> Notifies the scan start.</summary>
        '*/
        Public Event OnStart(ByVal scanner As ContentScanner) 'OnStartEventHandler()

#End Region

#Region "types"

        '/**
        '  <summary> Graphics state [PDF:1.6:4.3].</summary>
        '*/
        Public NotInheritable Class GraphicsState
            Implements ICloneable

#Region "dynamic"
#Region "fields"

            Private _blendMode As IList(Of BlendModeEnum)
            Private _charSpace As Double
            Private _ctm As Matrix
            Private _fillColor As colorSpaces.Color
            Private _fillColorSpace As ColorSpace
            Private _font As fonts.Font
            Private _fontSize As Double
            Private _lead As Double
            Private _lineCap As LineCapEnum
            Private _lineDash As LineDash
            Private _lineJoin As LineJoinEnum
            Private _lineWidth As Double
            Private _miterLimit As Double
            Private _renderMode As TextRenderModeEnum
            Private _rise As Double
            Private _scale As Double
            Private _strokeColor As colorSpaces.Color
            Private _strokeColorSpace As colorSpaces.ColorSpace
            Private _tlm As Matrix
            Private _tm As Matrix
            Private _wordSpace As Double

            Private _scanner As ContentScanner

#End Region

#Region "constructors"

            Friend Sub New(ByVal scanner As ContentScanner)
                Me._scanner = scanner
                Initialize()
            End Sub

#End Region


#If DEBUG Then

            Public Sub PrintDebugState()
                System.Diagnostics.Debug.Print("_blendMode: " & Me._blendMode.ToString())
                System.Diagnostics.Debug.Print("_charSpace: " & Me._charSpace.ToString())
                System.Diagnostics.Debug.Print("_ctm: " & Me._ctm.ToString())
                System.Diagnostics.Debug.Print("_fillColor: " & Me._fillColor.ToString())
                System.Diagnostics.Debug.Print("_fillColorSpace: " & Me._fillColorSpace.ToString())
                System.Diagnostics.Debug.Print("_font: " & Me._font.ToString())
                System.Diagnostics.Debug.Print("_fontSize: " & Me._fontSize.ToString())
                System.Diagnostics.Debug.Print("_lead: " & Me._lead.ToString())
                System.Diagnostics.Debug.Print("_lineCap: " & Me._lineCap.ToString())
                System.Diagnostics.Debug.Print("_lineDash: " & Me._lineDash.ToString())
                System.Diagnostics.Debug.Print("_lineJoin: " & Me._lineJoin.ToString())
                System.Diagnostics.Debug.Print("_lineWidth: " & Me._lineWidth.ToString())
                System.Diagnostics.Debug.Print("_miterLimit: " & Me._miterLimit.ToString())
                System.Diagnostics.Debug.Print("_renderMode: " & Me._renderMode.ToString())
                System.Diagnostics.Debug.Print("_rise: " & Me._rise.ToString())
                System.Diagnostics.Debug.Print("_scale: " & Me._scale.ToString())
                System.Diagnostics.Debug.Print("_strokeColor: " & Me._strokeColor.ToString())
                System.Diagnostics.Debug.Print("_strokeColorSpace: " & Me._strokeColorSpace.ToString())
                System.Diagnostics.Debug.Print("_tlm: " & Me._tlm.ToString())
                System.Diagnostics.Debug.Print("_tm: " & Me._tm.ToString())
                System.Diagnostics.Debug.Print("_wordSpace: " & Me._wordSpace.ToString())
                System.Diagnostics.Debug.Print("_scanner: " & Me._scanner.ToString())
                System.Diagnostics.Debug.Print(New String("-"c, 80))
            End Sub

#End If

#Region "interface"
#Region "public"
            '/**
            '  <summary> Gets a deep copy Of the graphics state Object.</summary>
            '*/
            Public Function Clone() As Object Implements ICloneable.Clone
                Dim _clone As GraphicsState
                '{
                ' Shallow copy.
                _clone = CType(Me.MemberwiseClone(), GraphicsState)

                ' Deep copy.
                '* NOTE: Mutable objects are To be cloned. */
                _clone._ctm = CType(_ctm.Clone(), Matrix)
                _clone._tlm = CType(_tlm.Clone(), Matrix)
                _clone._tm = CType(_tm.Clone(), Matrix)
                '}


                Return _clone
            End Function

            '/**
            '  <summary> Copies Me graphics state into the specified one.</summary>
            '  <param name = "state" > Target graphics state Object.</param>
            '*/
            Public Sub CopyTo(ByVal state As GraphicsState)
                state._blendMode = _blendMode
                state._charSpace = _charSpace
                state._ctm = CType(_ctm.Clone(), Matrix)
                state._fillColor = _fillColor
                state._fillColorSpace = _fillColorSpace
                state._font = _font
                state._fontSize = _fontSize
                state._lead = _lead
                state._lineCap = _lineCap
                state._lineDash = _lineDash
                state._lineJoin = _lineJoin
                state._lineWidth = _lineWidth
                state._miterLimit = _miterLimit
                state._renderMode = _renderMode
                state._rise = _rise
                state._scale = _scale
                state._strokeColor = _strokeColor
                state._strokeColorSpace = _strokeColorSpace
                'TODO:temporary hack(define TextState For textual parameters!)...
                If (TypeOf (state._scanner.Parent) Is Text) Then
                    state._tlm = CType(_tlm.Clone(), Matrix)
                    state._tm = CType(_tm.Clone(), Matrix)
                Else
                    state._tlm = New Matrix()
                    state._tm = New Matrix()
                End If
                state._wordSpace = _wordSpace
            End Sub

            '/**
            '  <summary> Gets/Sets the current blend mode To be used In the transparent imaging model
            '  [PDF:1.6:5.2.1].</summary>
            '  <remarks> The application should use the first blend mode In the list that it recognizes.
            '  </remarks>
            '*/
            Public Property BlendMode As IList(Of BlendModeEnum)
                Get
                    Return Me._blendMode
                End Get
                Set(ByVal value As IList(Of BlendModeEnum))
                    Me._blendMode = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current character spacing [PDF:1.6:5.2.1].</summary>
            '*/
            Public Property CharSpace As Double
                Get
                    Return _charSpace
                End Get
                Set(ByVal value As Double)
                    _charSpace = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current transformation matrix.</summary>
            '*/
            Public Property Ctm As Matrix
                Get
                    Return _ctm
                End Get
                Set(ByVal value As Matrix)
                    _ctm = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current color For nonstroking operations [PDF:1.6:4.5.1].</summary>
            '*/
            Public Property FillColor As colorSpaces.Color
                Get
                    Return _fillColor
                End Get
                Set(ByVal value As colorSpaces.Color)
                    _fillColor = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current color space For nonstroking operations [PDF:1.6:4.5.1].</summary>
            '*/
            Public Property FillColorSpace As colorSpaces.ColorSpace
                Get
                    Return _fillColorSpace
                End Get
                Set(ByVal value As colorSpaces.ColorSpace)
                    _fillColorSpace = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current font [PDF:1.6:5.2].</summary>
            '*/
            Public Property Font As fonts.Font
                Get
                    Return Me._font
                End Get
                Set(ByVal value As fonts.Font)
                    System.Diagnostics.Debug.Assert(value IsNot Nothing)
                    Me._font = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current font size [PDF:1.6:5.2].</summary>
            '*/
            Public Property FontSize As Double
                Get
                    Return _fontSize
                End Get
                Set(ByVal value As Double)
                    _fontSize = value
                End Set
            End Property

            '/**
            '  <summary> Gets the initial current transformation matrix.</summary>
            '*/
            Public Function GetInitialCtm() As Matrix
                Dim initialCtm As Matrix
                If (scanner.RenderContext Is Nothing) Then ' Device - independent.
                    initialCtm = New Matrix() ' Identity.
                Else ' Device-dependent.
                    Dim contentContext As IContentContext = scanner.ContentContext
                    Dim canvasSize As SizeF = scanner.CanvasSize

                    ' Axes orientation.
                    Dim rotation As RotationEnum = contentContext.Rotation
                    Select Case (rotation)
                        Case RotationEnum.Downward : initialCtm = New Matrix(1, 0, 0, -1, 0, canvasSize.Height) 'break;
                        Case RotationEnum.Leftward : initialCtm = New Matrix(0, 1, 1, 0, 0, 0) ' break;
                        Case RotationEnum.Upward : initialCtm = New Matrix(-1, 0, 0, 1, canvasSize.Width, 0)' break;
                        Case RotationEnum.Rightward : initialCtm = New Matrix(0, -1, -1, 0, canvasSize.Width, canvasSize.Height) 'break;
                        Case Else
                            Throw New NotImplementedException()
                    End Select

                    ' Scaling.
                    Dim contentBox As RectangleF = contentContext.Box
                    Dim rotatedCanvasSize As SizeF = rotation.Transform(canvasSize)
                    initialCtm.Scale(rotatedCanvasSize.Width / contentBox.Width, rotatedCanvasSize.Height / contentBox.Height)

                    ' Origin alignment.
                    initialCtm.Translate(-contentBox.Left, -contentBox.Top) 'TODO: verify minimum coordinates!
                End If
                Return initialCtm
            End Function

            '/**
            '  <summary> Gets/Sets the current leading [PDF:1.6:5.2.4].</summary>
            '*/
            Public Property Lead As Double
                Get
                    Return _lead
                End Get
                Set(ByVal value As Double)
                    _lead = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current line cap style [PDF:1.6:4.3.2].</summary>
            '*/
            Public Property LineCap As LineCapEnum
                Get
                    Return _lineCap
                End Get
                Set(ByVal value As LineCapEnum)
                    _lineCap = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current line dash pattern [PDF:1.6:4.3.2].</summary>
            '*/
            Public Property LineDash As LineDash
                Get
                    Return _lineDash
                End Get
                Set(ByVal value As LineDash)
                    _lineDash = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current line join style [PDF:1.6:4.3.2].</summary>
            '*/
            Public Property LineJoin As LineJoinEnum
                Get
                    Return _lineJoin
                End Get
                Set(ByVal value As LineJoinEnum)
                    _lineJoin = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current line width [PDF:1.6:4.3.2].</summary>
            '*/
            Public Property LineWidth As Double
                Get
                    Return _lineWidth
                End Get
                Set(ByVal value As Double)
                    _lineWidth = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current miter limit [PDF:1.6:4.3.2].</summary>
            '*/
            Public Property MiterLimit As Double
                Get
                    Return _miterLimit
                End Get
                Set(ByVal value As Double)
                    _miterLimit = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current text rendering mode [PDF:1.6:5.2.5].</summary>
            '*/
            Public Property RenderMode As TextRenderModeEnum
                Get
                    Return _renderMode
                End Get
                Set(ByVal value As TextRenderModeEnum)
                    _renderMode = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current text rise [PDF:1.6:5.2.6].</summary>
            '*/
            Public Property Rise As Double
                Get
                    Return _rise
                End Get
                Set(ByVal value As Double)
                    _rise = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current horizontal scaling [PDF:1.6:5.2.3].</summary>
            '*/
            Public Property Scale As Double
                Get
                    Return _scale
                End Get
                Set(ByVal value As Double)
                    _scale = value
                End Set
            End Property

            '/**
            '  <summary> Gets the scanner associated To Me state.</summary>
            '*/
            Public ReadOnly Property Scanner As ContentScanner
                Get
                    Return _scanner
                End Get
            End Property

            '/**
            '  <summary> Gets/Sets the current color For stroking operations [PDF:1.6:4.5.1].</summary>
            '*/
            Public Property StrokeColor As colorSpaces.Color
                Get
                    Return _strokeColor
                End Get
                Set(ByVal value As colorSpaces.Color)
                    _strokeColor = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current color space For stroking operations [PDF:1.6:4.5.1].</summary>
            '*/
            Public Property StrokeColorSpace As colorSpaces.ColorSpace
                Get
                    Return _strokeColorSpace
                End Get
                Set(ByVal value As colorSpaces.ColorSpace)
                    _strokeColorSpace = value
                End Set
            End Property

            '/**
            '  <summary> Resolves the given text-space point To its equivalent device-space one [PDF:1.6:5.3.3],
            '  expressed in standard PDF coordinate system (lower-left origin).</summary>
            '  <param name = "point" > Point To transform.</param>
            '*/
            Public Function TextToDeviceSpace(ByVal point As PointF) As PointF
                Return TextToDeviceSpace(point, False)
            End Function

            '/**
            '  <summary> Resolves the given text-space point To its equivalent device-space one [PDF:1.6:5.3.3].</summary>
            '  <param name = "point" > Point To transform.</param>
            '  <param name = "topDown" > Whether the y-axis orientation has To be adjusted To common top-down orientation
            '  rather than standard PDF coordinate system (bottom-up).</param>
            '*/
            Public Function TextToDeviceSpace(ByVal point As PointF, ByVal topDown As Boolean) As PointF
                '/*
                '  NOTE:         The Text rendering matrix (trm) Is obtained from the concatenation
                '  of the current transformation matrix (ctm) And the text matrix (tm).
                '*/
                Dim trm As Matrix
                If (topDown) Then
                    trm = New Matrix(1, 0, 0, -1, 0, _scanner.CanvasSize.Height)
                Else
                    trm = New Matrix()
                End If
                trm.Multiply(_ctm)
                trm.Multiply(_tm)
                Dim points As PointF() = New PointF() {point}
                trm.TransformPoints(points)
                Return points(0)
            End Function

            '/**
            '  <summary> Gets/Sets the current text line matrix [PDF:1.6:5.3].</summary>
            '*/
            Public Property Tlm As Matrix
                Get
                    Return _tlm
                End Get
                Set(ByVal value As Matrix)
                    _tlm = value
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the current text matrix [PDF:1.6:5.3].</summary>
            '*/
            Public Property Tm As Matrix
                Get
                    Return _tm
                End Get
                Set(ByVal value As Matrix)
                    _tm = value
                End Set
            End Property

            '/**
            '  <summary> Resolves the given user-space point To its equivalent device-space one [PDF:1.6:4.2.3],
            '  expressed in standard PDF coordinate system (lower-left origin).</summary>
            '  <param name = "point" > Point To transform.</param>
            '*/
            Public Function UserToDeviceSpace(ByVal point As PointF) As PointF
                Dim points As PointF() = New PointF() {point}
                _ctm.TransformPoints(points)
                Return points(0)
            End Function

            '/**
            '  <summary> Gets/Sets the current word spacing [PDF:1.6:5.2.2].</summary>
            '*/
            Public Property WordSpace As Double
                Get
                    Return _wordSpace
                End Get
                Set(ByVal value As Double)
                    _wordSpace = value
                End Set
            End Property

#End Region

#Region "internal"

            Friend Function Clone(ByVal scanner As ContentScanner) As GraphicsState
                Dim state As GraphicsState = CType(Me.Clone(), GraphicsState)
                state._scanner = scanner
                Return state
            End Function

            Friend Sub Initialize()
                ' State parameters initialization.
                Me._blendMode = ExtGState.DefaultBlendMode
                Me._charSpace = 0
                Me._ctm = Me.GetInitialCtm()
                Me._fillColor = colorSpaces.DeviceGrayColor.Default
                Me._fillColorSpace = colorSpaces.DeviceGrayColorSpace.Default
                Me._font = Nothing
                Me._fontSize = 0
                Me._lead = 0
                Me._lineCap = LineCapEnum.Butt
                Me._lineDash = New LineDash()
                Me._lineJoin = LineJoinEnum.Miter
                Me._lineWidth = 1
                Me._miterLimit = 10
                Me._renderMode = TextRenderModeEnum.Fill
                Me._rise = 0
                Me._scale = 100
                Me._strokeColor = colorSpaces.DeviceGrayColor.Default
                Me._strokeColorSpace = colorSpaces.DeviceGrayColorSpace.Default
                Me._tlm = New Matrix()
                Me._tm = New Matrix()
                Me._wordSpace = 0

                ' Rendering context initialization.
                Dim renderContext As Graphics = Me._scanner.RenderContext
                If (renderContext IsNot Nothing) Then
                    renderContext.Transform = Me._ctm
                End If
            End Sub
#End Region
#End Region
#End Region
        End Class


        Public MustInherit Class GraphicsObjectWrapper

#Region "Static"

            Friend Shared Function [Get](ByVal scanner As ContentScanner) As GraphicsObjectWrapper
                Dim obj As ContentObject = scanner.Current
                If (TypeOf (obj) Is ShowText) Then
                    Return New TextStringWrapper(scanner)
                ElseIf (TypeOf (obj) Is Text) Then
                    Return New TextWrapper(scanner)
                ElseIf (TypeOf (obj) Is objects.XObject) Then
                    Return New XObjectWrapper(scanner)
                ElseIf (TypeOf (obj) Is InlineImage) Then
                    Return New InlineImageWrapper(scanner)
                Else
                    Return Nothing
                End If
            End Function

#End Region

#Region "dynamic"
#Region "fields"

            Protected _box As RectangleF?

#End Region

#Region "interface"
#Region "public"

            '/**
            '  <summary>Gets the Object's bounding box.</summary>
            '*/
            Public Overridable ReadOnly Property Box As RectangleF?
                Get
                    Return _box
                End Get
            End Property

#End Region
#End Region
#End Region
        End Class


        '/**
        '  <summary>Object information.</summary>
        '  <remarks>
        '    <para>This Class provides derivative (higher-level) information
        '    about The currently scanned object.</para>
        '  </remarks>
        '*/
        Public MustInherit Class GraphicsObjectWrapper(Of TDataObject As ContentObject)
            Inherits GraphicsObjectWrapper

#Region "dynamic"
#Region "fields"
            Private _baseDataObject As TDataObject
#End Region

#Region "constructors"

            Protected Sub New(ByVal baseDataObject As TDataObject)
                Me._baseDataObject = baseDataObject
            End Sub

#End Region

#Region "interface"
#Region "public"

            '/**
            '  <summary>Gets the underlying data Object.</summary>
            '*/
            Public ReadOnly Property BaseDataObject As TDataObject
                Get
                    Return _baseDataObject
                End Get
            End Property

#End Region
#End Region
#End Region
        End Class

        '/**
        '  <summary>Inline image information.</summary>
        '*/
        Public NotInheritable Class InlineImageWrapper
            Inherits GraphicsObjectWrapper(Of InlineImage)

            Friend Sub New(ByVal scanner As ContentScanner)
                MyBase.New(CType(scanner.Current, InlineImage))
                Dim ctm As Matrix = scanner.State.Ctm
                Me._box = New RectangleF(ctm.Elements(4), scanner.ContentContext.Box.Height - ctm.Elements(5), ctm.Elements(0), Math.Abs(ctm.Elements(3)))
            End Sub

            '/**
            '  <summary> Gets the inline image.</summary>
            '*/
            Public ReadOnly Property InlineImage As InlineImage
                Get
                    Return BaseDataObject
                End Get
            End Property

        End Class


        '/**
        '  <summary> Text information.</summary>
        '*/
        Public NotInheritable Class TextWrapper
            Inherits GraphicsObjectWrapper(Of Text)

            Private _textStrings As List(Of TextStringWrapper)

            Friend Sub New(ByVal scanner As ContentScanner)
                MyBase.New(CType(scanner.Current, Text))
                _textStrings = New List(Of TextStringWrapper)()
                Extract(scanner.ChildLevel)
            End Sub

            Public Overrides ReadOnly Property Box As RectangleF?
                Get
                    If (_box Is Nothing) Then
                        For Each textString As TextStringWrapper In _textStrings
                            If (Not _box.HasValue) Then
                                _box = textString.Box
                            Else
                                _box = RectangleF.Union(_box.Value, textString.Box.Value)
                            End If
                        Next
                    End If
                    Return _box
                End Get
            End Property

            '/**
            '  <summary> Gets the text strings.</summary>
            '*/
            Public ReadOnly Property TextStrings As List(Of TextStringWrapper)
                Get
                    Return _textStrings
                End Get
            End Property

            Private Sub Extract(ByVal level As ContentScanner)
                If (level Is Nothing) Then Return

                While (level.MoveNext())
                    Dim content As ContentObject = level.Current
                    If (TypeOf (content) Is ShowText) Then
                        _textStrings.Add(CType(level.CurrentWrapper, TextStringWrapper))
                    ElseIf (TypeOf (content) Is ContainerObject) Then
                        Extract(level.ChildLevel)
                    End If
                End While
            End Sub
        End Class

        '/**
        '  <summary> Text String information.</summary>
        '*/
        Public NotInheritable Class TextStringWrapper
            Inherits GraphicsObjectWrapper(Of ShowText)
            Implements ITextString

            Private Class ShowTextScanner
                Implements ShowText.IScanner

                Public _wrapper As TextStringWrapper

                Friend Sub New(ByVal wrapper As TextStringWrapper)
                    Me._wrapper = wrapper
                End Sub

                Public Sub ScanChar(ByVal textChar As Char, ByVal textCharBox As RectangleF) Implements ShowText.IScanner.ScanChar
                    _wrapper._textChars.Add(New TextChar(textChar, textCharBox, _wrapper._style, False))
                End Sub
            End Class



            Private _style As TextStyle
            Private _textChars As List(Of TextChar)

            Friend Sub New(ByVal scanner As ContentScanner)
                MyBase.New(CType(scanner.Current, ShowText))
                _textChars = New List(Of TextChar)()
                '{
                Dim state As GraphicsState = scanner.State
                _style = New TextStyle(
                                    state.Font,
                                    state.FontSize * state.Tm.Elements(3),
                                    state.RenderMode,
                                    state.StrokeColor,
                                    state.StrokeColorSpace,
                                    state.FillColor,
                                    state.FillColorSpace
                                    )
                BaseDataObject.Scan(state, New ShowTextScanner(Me))
            End Sub


            Public Overrides ReadOnly Property Box As RectangleF? Implements ITextString.Box
                Get
                    If (_box Is Nothing) Then
                        For Each textChar As TextChar In _textChars
                            If (Not _box.HasValue) Then
                                _box = textChar.Box
                            Else
                                _box = RectangleF.Union(_box.Value, textChar.Box)
                            End If
                        Next
                    End If
                    Return _box
                End Get
            End Property

            '/**
            '  <summary> Gets the text style.</summary>
            '*/
            Public ReadOnly Property Style As TextStyle
                Get
                    Return _style
                End Get
            End Property

            Public ReadOnly Property Text As String Implements ITextString.Text
                Get
                    Dim textBuilder As StringBuilder = New StringBuilder()
                    For Each textChar As TextChar In _textChars
                        textBuilder.Append(textChar)
                    Next
                    Return textBuilder.ToString()
                End Get
            End Property

            Public ReadOnly Property TextChars As List(Of TextChar) Implements ITextString.TextChars
                Get
                    Return _textChars
                End Get
            End Property

        End Class

        '/**
        '  <summary> External Object information.</summary>
        '*/
        Public NotInheritable Class XObjectWrapper
            Inherits GraphicsObjectWrapper(Of objects.XObject)

            Private _name As PdfName
            Private _xObject As xObjects.XObject

            Friend Sub New(ByVal scanner As ContentScanner)
                MyBase.New(CType(scanner.Current, objects.XObject))
                Dim context As IContentContext = scanner.ContentContext
                Dim ctm As Matrix = scanner.State.Ctm
                Me._box = New RectangleF(ctm.Elements(4), context.Box.Height - ctm.Elements(5), ctm.Elements(0), Math.Abs(ctm.Elements(3)))
                Me._name = BaseDataObject.Name
                Me._xObject = BaseDataObject.GetResource(context)
            End Sub

            '/**
            '  <summary> Gets the corresponding resource key.</summary>
            '*/
            Public ReadOnly Property Name As PdfName
                Get
                    Return _name
                End Get
            End Property

            '/**
            '  <summary> Gets the external Object.</summary>
            '*/
            Public ReadOnly Property XObject As xObjects.XObject
                Get
                    Return _xObject
                End Get
            End Property

        End Class

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly _StartIndex As Integer = -1

#End Region
#End Region

#Region "dynamic"
#Region "fields"
        '/**
        '  Child level.
        '*/
        Private _childLevel As ContentScanner
        '/**
        '  Content objects collection.
        '*/
        Private _contents As Contents
        '/**
        '  Current object index at Me level.
        '*/
        Private _index As Integer = 0
        '/**
        '  Object collection at Me level.
        '*/
        Private _objects As IList(Of ContentObject)
        '/**
        '  Parent level.
        '*/
        Private _parentLevel As ContentScanner
        '/**
        '  Current graphics state.
        '*/
        Private _state As GraphicsState

        '/**
        '  Rendering context.
        '*/
        Private _renderContext As Graphics
        '/**
        '  Rendering object.
        '*/
        Private _renderObject As GraphicsPath
        '/**
        '  Device-space size of the rendering canvas.
        '*/
        Private _renderSize As SizeF?
#End Region

#Region "constructors"
        '/**
        '  <summary> Instantiates a top-level content scanner.</summary>
        '  <param name = "contents" > content objects collection To scan.</param>
        '*/
        Public Sub New(ByVal contents As Contents)
            Me._parentLevel = Nothing
            Me._contents = contents
            Me._objects = Me._contents

            MoveStart()
        End Sub

        '/**
        '  <summary> Instantiates a top-level content scanner.</summary>
        '  <param name = "contentContext" > content context containing the content objects collection To scan.</param>
        '*/
        Public Sub New(ByVal contentContext As IContentContext)
            Me.New(contentContext.Contents)
        End Sub

        '/**
        '  <summary> Instantiates a child-level content scanner For <see cref="org.dmdpdf.documents.contents.xObjects.FormXObject">external form</see>.</summary>
        '  <param name = "formXObject" > External form.</param>
        '  <param name = "parentLevel" > Parent scan level.</param>
        '*/
        Public Sub New(ByVal formXObject As xObjects.FormXObject, ByVal parentLevel As ContentScanner)
            Me._parentLevel = parentLevel
            Me._contents = formXObject.Contents
            Me._objects = Me._contents

            Dim handler As New OnStartHandler(Me, formXObject, parentLevel)
            AddHandler Me.OnStart, AddressOf handler.handleStart

            MoveStart()
        End Sub

        Private Class OnStartHandler
            Public o As ContentScanner
            Public formXObject As xObjects.FormXObject
            Public parentLevel As ContentScanner

            Public Sub New(ByVal o As ContentScanner, ByVal formXObject As xObjects.FormXObject, ByVal parentLevel As ContentScanner)
                Me.o = o
                Me.formXObject = formXObject
                Me.parentLevel = parentLevel
            End Sub


            Public Sub handleStart(ByVal scanner As ContentScanner)
                ' Adjust the initial graphics state to the external form context!
                scanner.State.Ctm.Multiply(formXObject.Matrix)
                '/*
                '  TODO: On rendering, clip according to the form dictionary's BBox entry!
                '*/
            End Sub
        End Class


        '/**
        '  <summary> Instantiates a child-level content scanner.</summary>
        '  <param name = "parentLevel" > Parent scan level.</param>
        '*/
        Private Sub New(ByVal parentLevel As ContentScanner)
            Me._parentLevel = parentLevel
            Me._contents = parentLevel._contents
            Me._objects = CType(parentLevel.Current, CompositeObject).Objects

            MoveStart()
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets the size Of the current imageable area.</summary>
        '  <remarks> It can be either the user-space area (dry scanning)
        '  Or the device-space area (wet scanning).</remarks>
        '*/
        Public ReadOnly Property CanvasSize As SizeF
            Get
                If (_renderSize.HasValue) Then
                    Return _renderSize.Value ' Device - dependent(device - Space()) area.
                Else
                    Return ContentContext.Box.Size ' Device-independent (user-space) area.
                End If
            End Get
        End Property

        '/**
        '  <summary> Gets the current child scan level.</summary>
        '*/
        Public ReadOnly Property ChildLevel As ContentScanner
            Get
                Return _childLevel
            End Get
        End Property

        '/**
        '  <summary> Gets the content context associated To the content objects collection.</summary>
        '*/
        Public ReadOnly Property ContentContext As IContentContext
            Get
                Return _contents.ContentContext
            End Get
        End Property

        '/**
        '  <summary> Gets the content objects collection Me scanner Is inspecting.</summary>
        '*/
        Public ReadOnly Property Contents As Contents
            Get
                Return _contents
            End Get
        End Property

        '/**
        '  <summary> Gets/Sets the current content Object.</summary>
        '*/
        Public Property Current As ContentObject
            Get
                If (_index < 0 OrElse _index >= _objects.Count) Then Return Nothing
                Return _objects(_index)
            End Get
            Set(ByVal value As ContentObject)
                _objects(_index) = value
                Refresh()
            End Set
        End Property

        '/**
        '  <summary> Gets the current content Object's information.</summary>
        '*/
        Public ReadOnly Property CurrentWrapper As GraphicsObjectWrapper
            Get
                Return GraphicsObjectWrapper.Get(Me)
            End Get
        End Property

        '/**
        '  <summary> Gets the current position.</summary>
        '*/
        Public ReadOnly Property Index As Integer
            Get
                Return _index
            End Get
        End Property

        '/**
        '  <summary> Inserts a content Object at the current position.</summary>
        '*/
        Public Sub Insert(ByVal obj As ContentObject)
            If (_index = -1) Then
                _index = 0
            End If

            _objects.Insert(_index, obj)
            Refresh()
        End Sub

        '/**
        '  <summary> Inserts content objects at the current position.</summary>
        '  <remarks> After the insertion Is complete, the lastly-inserted content Object Is at the current position.</remarks>
        '*/
        Public Sub Insert(Of T As ContentObject)(ByVal objects As ICollection(Of T))
            Dim index As Integer = 0
            Dim count As Integer = objects.Count
            For Each obj As ContentObject In objects
                Insert(obj)
                index += 1
                If (index < count) Then
                    MoveNext()
                End If
            Next
        End Sub

        '/**
        '  <summary> Gets whether Me level Is the root Of the hierarchy.</summary>
        '*/
        Public Function IsRootLevel() As Boolean
            Return _parentLevel Is Nothing
        End Function

        '/**
        '  <summary> Moves To the Object at the given position.</summary>
        '  <param name = "index" > New position.</param>
        '  <returns> Whether the Object was successfully reached.</returns>
        '*/
        Public Function Move(ByVal index As Integer)
            If (Me._index > index) Then
                MoveStart()
            End If

            While (Me._index < index AndAlso MoveNext())
            End While

            Return Current IsNot Nothing
        End Function

        '/**
        '  <summary> Moves after the last Object.</summary>
        '*/
        Public Sub MoveEnd()
            MoveLast()
            MoveNext()
        End Sub

        '/**
        '  <summary> Moves To the first Object.</summary>
        '  <returns> Whether the first Object was successfully reached.</returns>
        '*/
        Public Function MoveFirst() As Boolean
            MoveStart()
            Return MoveNext()
        End Function

        '/**
        '  <summary> Moves To the last Object.</summary>
        '  <returns> Whether the last Object was successfully reached.</returns>
        '*/
        Public Function MoveLast() As Boolean
            Dim lastIndex As Integer = _objects.Count - 1
            While (_index < lastIndex)
                MoveNext()
            End While

            Return Current IsNot Nothing
        End Function

        '/**
        '  <summary> Moves To the Next Object.</summary>
        '  <returns> Whether the Next Object was successfully reached.</returns>
        '*/
        Public Function MoveNext() As Boolean
            ' Scanning the current graphics state...
            Dim currentObject As ContentObject = Current
            If (currentObject IsNot Nothing) Then
                currentObject.Scan(_state)
            End If

            ' Moving to the next object...
            If (_index < _objects.Count) Then
                _index += 1
                Refresh()
            End If

            Return Current IsNot Nothing
        End Function

        '/**
        '  <summary> Moves before the first Object.</summary>
        '*/
        Public Sub MoveStart()
            _index = _StartIndex
            If (_state Is Nothing) Then
                If (_parentLevel Is Nothing) Then
                    _state = New GraphicsState(Me)
                Else
                    _state = _parentLevel._state.Clone(Me)
                End If
            Else
                If (_parentLevel Is Nothing) Then
                    _state.Initialize()
                Else
                    _parentLevel._state.CopyTo(_state)
                End If
            End If

            NotifyStart()

            Refresh()
        End Sub

        '/**
        '  <summary> Gets the current parent Object.</summary>
        '*/
        Public ReadOnly Property Parent As CompositeObject
            Get
                If (_parentLevel Is Nothing) Then
                    Return Nothing
                Else
                    Return CType(_parentLevel.Current, CompositeObject)
                End If
            End Get
        End Property

        '/**
        '  <summary> Gets the parent scan level.</summary>
        '*/
        Public ReadOnly Property ParentLevel As ContentScanner
            Get
                Return _parentLevel
            End Get
        End Property

        '/**
        '  <summary> Removes the content Object at the current position.</summary>
        '  <returns> Removed Object.</returns>
        '*/
        Public Function Remove() As ContentObject
            Dim removedObject As ContentObject = Current
            _objects.RemoveAt(_index)
            Refresh()

            Return removedObject
        End Function

        '/**
        '  <summary> Renders the contents into the specified context.</summary>
        '  <param name = "renderContext" > Rendering context.</param>
        '  <param name = "renderSize" > Rendering canvas size.</param>
        '*/
        Public Sub Render(ByVal renderContext As Graphics, ByVal renderSize As SizeF)
            Render(renderContext, renderSize, Nothing)
        End Sub

        '/**
        '  <summary> Renders the contents into the specified Object.</summary>
        '  <param name = "renderContext" > Rendering context.</param>
        '  <param name = "renderSize" > Rendering canvas size.</param>
        '  <param name = "renderObject" > Rendering Object.</param>
        '*/
        Public Sub Render(ByVal renderContext As Graphics, ByVal renderSize As SizeF, ByVal renderObject As GraphicsPath)
            If (IsRootLevel()) Then
                ' Initialize the context!
                renderContext.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
                renderContext.SmoothingMode = SmoothingMode.HighQuality

                ' Paint the canvas background!
                renderContext.Clear(System.Drawing.Color.White)
            End If

            Try
                Me._renderContext = renderContext
                Me._renderSize = renderSize
                Me._renderObject = renderObject

                ' Scan Me level for rendering!
                MoveStart()
                While (MoveNext())

                End While
            Finally
                Me._renderContext = Nothing
                Me._renderSize = Nothing
                Me._renderObject = Nothing
            End Try
        End Sub

        '/**
        '  <summary> Gets the rendering context.</summary>
        '  <returns> <code>Nothing</code> In Case Of dry scanning.</returns>
        '*/
        Public ReadOnly Property RenderContext As Graphics
            Get
                Return _renderContext
            End Get
        End Property

        '/**
        '  <summary> Gets the rendering Object.</summary>
        '  <returns> <code>Nothing</code> In Case Of scanning outside a shape.</returns>
        '*/
        Public ReadOnly Property RenderObject As GraphicsPath
            Get
                Return _renderObject
            End Get
        End Property

        '/**
        '  <summary> Gets the root scan level.</summary>
        '*/
        Public ReadOnly Property RootLevel As ContentScanner
            Get
                Dim level As ContentScanner = Me
                While (True)
                    Dim parentLevel As ContentScanner = level.ParentLevel
                    If (parentLevel Is Nothing) Then Return level
                    level = parentLevel
                End While
            End Get
        End Property

        '/**
        '  <summary> Gets the current graphics state applied To the current content Object.</summary>
        '*/
        Public ReadOnly Property State As GraphicsState
            Get
                Return _state
            End Get
        End Property

#End Region

#Region "protected"
        '#pragma warning disable 0628
        '/**
        '  <summary> Notifies the scan start To listeners.</summary>
        '*/
        Protected Sub NotifyStart()
            'If (OnStart!= Nothing)
            '{OnStart(Me);}
            RaiseEvent OnStart(Me)
        End Sub

        '#pragma warning restore 0628
#End Region

#Region "private"
        '/**
        '  <summary> Synchronizes the scanner state.</summary>
        '*/
        Private Sub Refresh()
            If (TypeOf (Current) Is CompositeObject) Then
                _childLevel = New ContentScanner(Me)
            Else
                _childLevel = Nothing
            End If
        End Sub

#End Region
#End Region
#End Region
    End Class

End Namespace

''/*
''  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

''  You should have received a copy of the GNU Lesser General Public License along with Me
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  Me list of conditions.
''*/

'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.documents.contents.colorSpaces
'Imports DMD.org.dmdpdf.documents.contents.fonts
'Imports DMD.org.dmdpdf.documents.contents.objects
'Imports DMD.org.dmdpdf.documents.contents.xObjects
'Imports DMD.org.dmdpdf.files
'Imports DMD.org.dmdpdf.objects

'Imports System
'Imports System.Collections
'Imports System.Collections.Generic
'Imports System.Drawing
'Imports System.Drawing.Drawing2D
'Imports System.Reflection
'Imports System.Text

'Namespace DMD.org.dmdpdf.documents.contents

'    '/**
'    '  <summary>Content objects scanner.</summary>
'    '  <remarks>
'    '    <para>It wraps the <see cref="Contents">content objects collection</see> to scan its graphics state
'    '    through a forward cursor.</para>
'    '    <para>Scanning is performed at an arbitrary deepness, according to the content objects nesting:
'    '    each depth level corresponds to a scan level so that at any time it's possible to seamlessly
'    '    navigate across the levels (see <see cref="ParentLevel"/>, <see cref="ChildLevel"/>).</para>
'    '  </remarks>
'    '*/
'    Public NotInheritable Class ContentScanner

'#Region "delegates"
'        '/**
'        '  <summary>Handles the scan start notification.</summary>
'        '  <param name="scanner">Content scanner started.</param>
'        '*/
'        Public Delegate Sub StartEventHandler(ByVal scanner As ContentScanner)

'#End Region

'#Region "events"

'        '/**
'        '  <summary>Notifies the scan start.</summary>
'        '*/
'        Public Event OnStart(ByVal scanner As ContentScanner)

'#End Region

'#Region "types"

'        '/**
'        '  <summary>Graphics state [PDF:1.6:4.3].</summary>
'        '*/
'        Public NotInheritable Class GraphicsState
'            Implements ICloneable

'#Region "dynamic"
'#Region "fields"

'            Private _blendMode As IList(Of BlendModeEnum)
'            Private _charSpace As Double
'            Private _ctm As Matrix
'            Private _fillColor As colorSpaces.Color
'            Private _fillColorSpace As colorSpaces.ColorSpace
'            Private _font As fonts.Font
'            Private _fontSize As Double
'            Private _lead As Double
'            Private _lineCap As LineCapEnum
'            Private _lineDash As LineDash
'            Private _lineJoin As LineJoinEnum
'            Private _lineWidth As Double
'            Private _miterLimit As Double
'            Private _renderMode As TextRenderModeEnum
'            Private _rise As Double
'            Private _scale As Double
'            Private _strokeColor As colorSpaces.Color
'            Private _strokeColorSpace As colorSpaces.ColorSpace
'            Private _tlm As Matrix
'            Private _tm As Matrix
'            Private _wordSpace As Double

'            Private _scanner As ContentScanner

'#End Region

'#If DEBUG Then

'            Public Sub PrintDebugState()
'                System.Diagnostics.Debug.Print("_blendMode: " & Me._blendMode.ToString())
'                System.Diagnostics.Debug.Print("_charSpace: " & Me._charSpace.ToString())
'                System.Diagnostics.Debug.Print("_ctm: " & Me._ctm.ToString())
'                System.Diagnostics.Debug.Print("_fillColor: " & Me._fillColor.ToString())
'                System.Diagnostics.Debug.Print("_fillColorSpace: " & Me._fillColorSpace.ToString())
'                System.Diagnostics.Debug.Print("_font: " & Me._font.ToString())
'                System.Diagnostics.Debug.Print("_fontSize: " & Me._fontSize.ToString())
'                System.Diagnostics.Debug.Print("_lead: " & Me._lead.ToString())
'                System.Diagnostics.Debug.Print("_lineCap: " & Me._lineCap.ToString())
'                System.Diagnostics.Debug.Print("_lineDash: " & Me._lineDash.ToString())
'                System.Diagnostics.Debug.Print("_lineJoin: " & Me._lineJoin.ToString())
'                System.Diagnostics.Debug.Print("_lineWidth: " & Me._lineWidth.ToString())
'                System.Diagnostics.Debug.Print("_miterLimit: " & Me._miterLimit.ToString())
'                System.Diagnostics.Debug.Print("_renderMode: " & Me._renderMode.ToString())
'                System.Diagnostics.Debug.Print("_rise: " & Me._rise.ToString())
'                System.Diagnostics.Debug.Print("_scale: " & Me._scale.ToString())
'                System.Diagnostics.Debug.Print("_strokeColor: " & Me._strokeColor.ToString())
'                System.Diagnostics.Debug.Print("_strokeColorSpace: " & Me._strokeColorSpace.ToString())
'                System.Diagnostics.Debug.Print("_tlm: " & Me._tlm.ToString())
'                System.Diagnostics.Debug.Print("_tm: " & Me._tm.ToString())
'                System.Diagnostics.Debug.Print("_wordSpace: " & Me._wordSpace.ToString())
'                System.Diagnostics.Debug.Print("_scanner: " & Me._scanner.ToString())
'                System.Diagnostics.Debug.Print(New String("-"c, 80))
'            End Sub

'#End If

'#Region "constructors"

'            Friend Sub New(ByVal scanner As ContentScanner)
'                Me._scanner = scanner
'                Initialize()
'            End Sub

'#End Region

'#Region "interface"
'#Region "public"

'            '/**
'            '  <summary> Gets a deep copy Of the graphics state Object.</summary>
'            '*/
'            Public Function Clone() As Object Implements ICloneable.Clone
'                Dim _clone As GraphicsState
'                ' Shallow copy.
'                _clone = CType(Me.MemberwiseClone(), GraphicsState)

'                '// Deep copy.
'                '/* NOTE: Mutable objects are To be cloned. */
'                _clone._ctm = CType(_ctm.Clone(), Matrix)
'                _clone._tlm = CType(_tlm.Clone(), Matrix)
'                _clone._tm = CType(_tm.Clone(), Matrix)

'                Return _clone
'            End Function

'            '/**
'            '  <summary> Copies Me graphics state into the specified one.</summary>
'            '  <param name = "state" > Target graphics state Object.</param>
'            '*/
'            Public Sub CopyTo(ByVal state As GraphicsState)
'                state._blendMode = _blendMode
'                state._charSpace = _charSpace
'                state._ctm = CType(_ctm.Clone(), Matrix)
'                state._fillColor = _fillColor
'                state._fillColorSpace = _fillColorSpace
'                state._font = _font
'                state._fontSize = _fontSize
'                state._lead = _lead
'                state._lineCap = _lineCap
'                state._lineDash = _lineDash
'                state._lineJoin = _lineJoin
'                state._lineWidth = _lineWidth
'                state._miterLimit = _miterLimit
'                state._renderMode = _renderMode
'                state._rise = _rise
'                state._scale = _scale
'                state._strokeColor = _strokeColor
'                state._strokeColorSpace = _strokeColorSpace
'                'TODO:temporary hack(define TextState For textual parameters!)...
'                If (TypeOf (state._scanner.Parent) Is Text) Then
'                    state._tlm = CType(_tlm.Clone(), Matrix)
'                    state._tm = CType(_tm.Clone(), Matrix)
'                Else
'                    state._tlm = New Matrix()
'                    state._tm = New Matrix()
'                End If
'                state._wordSpace = _wordSpace
'            End Sub

'            '/**
'            '  <summary> Gets/Sets the current blend mode To be used In the transparent imaging model
'            '  [PDF:1.6:5.2.1].</summary>
'            '  <remarks> The application should use the first blend mode In the list that it recognizes.
'            '  </remarks>
'            '*/
'            Public Property BlendMode As IList(Of BlendModeEnum)
'                Get
'                    Return _blendMode
'                End Get
'                Set(ByVal value As IList(Of BlendModeEnum))
'                    _blendMode = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current character spacing [PDF:1.6:5.2.1].</summary>
'            '*/
'            Public Property CharSpace As Double
'                Get
'                    Return _charSpace
'                End Get
'                Set(ByVal value As Double)
'                    _charSpace = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current transformation matrix.</summary>
'            '*/
'            Public Property Ctm As Matrix
'                Get
'                    Return _ctm
'                End Get
'                Set(ByVal value As Matrix)
'                    _ctm = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current color For nonstroking operations [PDF:1.6:4.5.1].</summary>
'            '*/
'            Public Property FillColor As colorSpaces.Color
'                Get
'                    Return _fillColor
'                End Get
'                Set(ByVal value As colorSpaces.Color)
'                    Me._fillColor = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current color space For nonstroking operations [PDF:1.6:4.5.1].</summary>
'            '*/
'            Public Property FillColorSpace As colorSpaces.ColorSpace
'                Get
'                    Return _fillColorSpace
'                End Get
'                Set(ByVal value As colorSpaces.ColorSpace)
'                    _fillColorSpace = value
'                End Set
'            End Property


'            '/**
'            '  <summary> Gets/Sets the current font [PDF:1.6:5.2].</summary>
'            '*/
'            Public Property Font As fonts.Font
'                Get
'                    Return Me._font
'                End Get
'                Set(ByVal value As fonts.Font)
'                    Me._font = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current font size [PDF:1.6:5.2].</summary>
'            '*/
'            Public Property FontSize As Double
'                Get
'                    Return _fontSize
'                End Get
'                Set(ByVal value As Double)
'                    _fontSize = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets the initial current transformation matrix.</summary>
'            '*/
'            Public Function GetInitialCtm() As Matrix
'                Dim initialCtm As Matrix
'                If (Scanner.RenderContext Is Nothing) Then ' Device - independent.
'                    initialCtm = New Matrix() ' Identity.
'                Else ' Device-dependent.
'                    Dim contentContext As IContentContext = Scanner.ContentContext
'                    Dim canvasSize As SizeF = Scanner.CanvasSize

'                    ' Axes orientation.
'                    Dim rotation As RotationEnum = contentContext.Rotation
'                    Select Case (rotation)
'                        Case RotationEnum.Downward : initialCtm = New Matrix(1, 0, 0, -1, 0, canvasSize.Height)
'                        Case RotationEnum.Leftward : initialCtm = New Matrix(0, 1, 1, 0, 0, 0)
'                        Case RotationEnum.Upward : initialCtm = New Matrix(-1, 0, 0, 1, canvasSize.Width, 0)
'                        Case RotationEnum.Rightward : initialCtm = New Matrix(0, -1, -1, 0, canvasSize.Width, canvasSize.Height)
'                        Case Else : Throw New NotImplementedException()
'                    End Select

'                    ' Scaling.
'                    Dim contentBox As RectangleF = contentContext.Box
'                    Dim rotatedCanvasSize As SizeF = rotation.Transform(canvasSize)
'                    initialCtm.Scale(
'                                    rotatedCanvasSize.Width / contentBox.Width,
'                                    rotatedCanvasSize.Height / contentBox.Height
'                                    )

'                    ' Origin alignment.
'                    initialCtm.Translate(-contentBox.Left, -contentBox.Top) 'TODO: verify minimum coordinates!
'                End If
'                Return initialCtm
'            End Function

'            '/**
'            '  <summary> Gets/Sets the current leading [PDF:1.6:5.2.4].</summary>
'            '*/
'            Public Property Lead As Double
'                Get
'                    Return _lead
'                End Get
'                Set(ByVal value As Double)
'                    _lead = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current line cap style [PDF:1.6:4.3.2].</summary>
'            '*/
'            Public Property LineCap As LineCapEnum
'                Get
'                    Return _lineCap
'                End Get
'                Set(ByVal value As LineCapEnum)
'                    _lineCap = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current line dash pattern [PDF:1.6:4.3.2].</summary>
'            '*/
'            Public Property LineDash As LineDash
'                Get
'                    Return _lineDash
'                End Get
'                Set(ByVal value As LineDash)
'                    _lineDash = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current line join style [PDF:1.6:4.3.2].</summary>
'            '*/
'            Public Property LineJoin As LineJoinEnum
'                Get
'                    Return _lineJoin
'                End Get
'                Set(ByVal value As LineJoinEnum)
'                    _lineJoin = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current line width [PDF:1.6:4.3.2].</summary>
'            '*/
'            Public Property LineWidth As Double
'                Get
'                    Return _lineWidth
'                End Get
'                Set(ByVal value As Double)
'                    _lineWidth = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current miter limit [PDF:1.6:4.3.2].</summary>
'            '*/
'            Public Property MiterLimit As Double
'                Get
'                    Return _miterLimit
'                End Get
'                Set(ByVal value As Double)
'                    _miterLimit = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current text rendering mode [PDF:1.6:5.2.5].</summary>
'            '*/
'            Public Property RenderMode As TextRenderModeEnum
'                Get
'                    Return _renderMode
'                End Get
'                Set(ByVal value As TextRenderModeEnum)
'                    _renderMode = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current text rise [PDF:1.6:5.2.6].</summary>
'            '*/
'            Public Property Rise As Double
'                Get
'                    Return _rise
'                End Get
'                Set(ByVal value As Double)
'                    _rise = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current horizontal scaling [PDF:1.6:5.2.3].</summary>
'            '*/
'            Public Property Scale As Double
'                Get
'                    Return _scale
'                End Get
'                Set(ByVal value As Double)
'                    _scale = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets the scanner associated To Me state.</summary>
'            '*/
'            Public ReadOnly Property Scanner As ContentScanner
'                Get
'                    Return _scanner
'                End Get
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current color For stroking operations [PDF:1.6:4.5.1].</summary>
'            '*/
'            Public Property StrokeColor As colorSpaces.Color
'                Get
'                    Return _strokeColor
'                End Get
'                Set(ByVal value As colorSpaces.Color)
'                    _strokeColor = value
'                End Set
'            End Property

'            '      /**
'            '  <summary> Gets/Sets the current color space For stroking operations [PDF:1.6:4.5.1].</summary>
'            '*/
'            Public Property StrokeColorSpace As colorSpaces.ColorSpace
'                Get
'                    Return _strokeColorSpace
'                End Get
'                Set(ByVal value As colorSpaces.ColorSpace)
'                    _strokeColorSpace = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Resolves the given text-space point To its equivalent device-space one [PDF:1.6:5.3.3],
'            '  expressed in standard PDF coordinate system (lower-left origin).</summary>
'            '  <param name = "point" > Point To transform.</param>
'            '*/
'            Public Function TextToDeviceSpace(ByVal Point As PointF) As PointF
'                Return TextToDeviceSpace(Point, False)
'            End Function

'            '/**
'            '  <summary> Resolves the given text-space point To its equivalent device-space one [PDF:1.6:5.3.3].</summary>
'            '  <param name = "point" > Point To transform.</param>
'            '  <param name = "topDown" > Whether the y-axis orientation has To be adjusted To common top-down orientation
'            '  rather than standard PDF coordinate system (bottom-up).</param>
'            '*/
'            Public Function TextToDeviceSpace(ByVal Point As PointF, ByVal topDown As Boolean) As PointF
'                '/*
'                '  NOTE:         The Text rendering matrix (trm) Is obtained from the concatenation
'                '  of the current transformation matrix (ctm) And the text matrix (tm).
'                '*/
'                Dim trm As Matrix
'                If (topDown) Then
'                    trm = New Matrix(1, 0, 0, -1, 0, _scanner.CanvasSize.Height)
'                Else
'                    trm = New Matrix()
'                End If

'                trm.Multiply(_ctm)
'                trm.Multiply(_tm)
'                Dim points As PointF() = New PointF() {Point}
'                trm.TransformPoints(points)
'                Return points(0)
'            End Function

'            '/**
'            '  <summary> Gets/Sets the current text line matrix [PDF:1.6:5.3].</summary>
'            '*/
'            Public Property Tlm As Matrix
'                Get
'                    Return _tlm
'                End Get
'                Set(ByVal value As Matrix)
'                    _tlm = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Gets/Sets the current text matrix [PDF:1.6:5.3].</summary>
'            '*/
'            Public Property Tm As Matrix
'                Get
'                    Return _tm
'                End Get
'                Set(ByVal value As Matrix)
'                    _tm = value
'                End Set
'            End Property

'            '/**
'            '  <summary> Resolves the given user-space point To its equivalent device-space one [PDF:1.6:4.2.3],
'            '  expressed in standard PDF coordinate system (lower-left origin).</summary>
'            '  <param name = "point" > Point To transform.</param>
'            '*/
'            Public Function UserToDeviceSpace(ByVal Point As PointF) As PointF
'                Dim points As PointF() = New PointF() {Point}
'                _ctm.TransformPoints(points)
'                Return points(0)
'            End Function

'            '/**
'            '  <summary> Gets/Sets the current word spacing [PDF:1.6:5.2.2].</summary>
'            '*/
'            Public Property WordSpace As Double
'                Get
'                    Return _wordSpace
'                End Get
'                Set(ByVal value As Double)
'                    _wordSpace = value
'                End Set
'            End Property

'#End Region

'#Region "internal"


'            Friend Function Clone(ByVal scanner As ContentScanner) As GraphicsState
'                Dim state As GraphicsState = CType(Me.Clone(), GraphicsState)
'                state._scanner = scanner
'                Return state
'            End Function

'            Friend Sub Initialize()
'                ' State parameters initialization.
'                _blendMode = ExtGState.DefaultBlendMode
'                _charSpace = 0
'                _ctm = GetInitialCtm()
'                _fillColor = colorSpaces.DeviceGrayColor.Default
'                _fillColorSpace = colorSpaces.DeviceGrayColorSpace.Default
'                _font = Nothing
'                _fontSize = 0
'                _lead = 0
'                _lineCap = LineCapEnum.Butt
'                _lineDash = New LineDash()
'                _lineJoin = LineJoinEnum.Miter
'                _lineWidth = 1
'                _miterLimit = 10
'                _renderMode = TextRenderModeEnum.Fill
'                _rise = 0
'                _scale = 100
'                _strokeColor = colorSpaces.DeviceGrayColor.Default
'                _strokeColorSpace = colorSpaces.DeviceGrayColorSpace.Default
'                _tlm = New Matrix()
'                _tm = New Matrix()
'                _wordSpace = 0

'                ' Rendering context initialization.
'                Dim renderContext As Graphics = Scanner.RenderContext
'                If (renderContext IsNot Nothing) Then
'                    renderContext.Transform = _ctm
'                End If
'            End Sub
'#End Region
'#End Region
'#End Region
'        End Class


'        Public MustInherit Class GraphicsObjectWrapper

'#Region "Static"

'            Friend Shared Function [Get](ByVal scanner As ContentScanner) As GraphicsObjectWrapper
'                Dim obj As ContentObject = scanner.Current
'                If (TypeOf (obj) Is ShowText) Then
'                    Return New TextStringWrapper(scanner)
'                ElseIf (TypeOf (obj) Is Text) Then
'                    Return New TextWrapper(scanner)
'                ElseIf (TypeOf (obj) Is objects.XObject) Then
'                    Return New XObjectWrapper(scanner)
'                ElseIf (TypeOf (obj) Is InlineImage) Then
'                    Return New InlineImageWrapper(scanner)
'                Else
'                    Return Nothing
'                End If
'            End Function

'#End Region

'#Region "dynamic"
'#Region "fields"

'            Protected _box As RectangleF?

'#End Region

'#Region "interface"
'#Region "public"

'            '/**
'            '  <summary> Gets the Object's bounding box.</summary>
'            '*/
'            Public Overridable ReadOnly Property Box As RectangleF?
'                Get
'                    Return _box
'                End Get
'            End Property

'#End Region
'#End Region
'#End Region
'        End Class


'        '/**
'        '  <summary>Object information.</summary>
'        '  <remarks>
'        '        <para> This Class provides derivative (higher-level) information
'        '    about The currently scanned Object.</para>
'        '  </remarks>
'        '*/
'        Public MustInherit Class GraphicsObjectWrapper(Of TDataObject As ContentObject)
'            Inherits GraphicsObjectWrapper

'#Region "dynamic"
'#Region "fields"

'            Private _baseDataObject As TDataObject

'#End Region

'#Region "constructors"

'            Protected Sub New(ByVal baseDataObject As TDataObject)
'                Me._baseDataObject = baseDataObject
'            End Sub

'#End Region

'#Region "interface"
'#Region "public"

'            '/**
'            '  <summary> Gets the underlying data Object.</summary>
'            '*/
'            Public ReadOnly Property BaseDataObject As TDataObject
'                Get
'                    Return Me._baseDataObject
'                End Get
'            End Property

'#End Region
'#End Region
'#End Region
'        End Class

'        '/**
'        '  <summary> Inline image information.</summary>
'        '*/
'        Public NotInheritable Class InlineImageWrapper
'            Inherits GraphicsObjectWrapper(Of InlineImage)

'            Friend Sub New(ByVal scanner As ContentScanner)
'                MyBase.New(CType(scanner.Current, InlineImage))
'                Dim ctm As Matrix = scanner.State.Ctm
'                Me._box = New RectangleF(
'                                ctm.Elements(4),
'                                scanner.ContentContext.Box.Height - ctm.Elements(5),
'                                ctm.Elements(0),
'                                Math.Abs(ctm.Elements(3))
'                            )
'            End Sub

'            '/**
'            '  <summary> Gets the inline image.</summary>
'            '*/
'            Public ReadOnly Property InlineImage As InlineImage
'                Get
'                    Return BaseDataObject
'                End Get
'            End Property
'        End Class

'        '/**
'        '  <summary> Text information.</summary>
'        '*/
'        Public NotInheritable Class TextWrapper
'            Inherits GraphicsObjectWrapper(Of Text)

'            Private _textStrings As List(Of TextStringWrapper)

'            Friend Sub New(ByVal scanner As ContentScanner)
'                MyBase.New(CType(scanner.Current, Text))
'                Me._textStrings = New List(Of TextStringWrapper)()
'                Extract(scanner.ChildLevel)
'            End Sub

'            Public Overrides ReadOnly Property Box As RectangleF?
'                Get
'                    If (_box Is Nothing) Then
'                        For Each textString As TextStringWrapper In _textStrings
'                            If (Not _box.HasValue) Then
'                                _box = textString.Box
'                            Else
'                                _box = RectangleF.Union(_box.Value, textString.Box.Value)
'                            End If
'                        Next
'                    End If
'                    Return _box
'                End Get
'            End Property

'            '/**
'            '  <summary> Gets the text strings.</summary>
'            '*/
'            Public ReadOnly Property TextStrings As List(Of TextStringWrapper)
'                Get
'                    Return _textStrings
'                End Get
'            End Property

'            Private Sub Extract(ByVal level As ContentScanner)
'                If (level Is Nothing) Then Return
'                While (level.MoveNext())
'                    Dim content As ContentObject = level.Current
'                    If (TypeOf (content) Is ShowText) Then
'                        _textStrings.Add(CType(level.CurrentWrapper, TextStringWrapper))
'                    ElseIf (TypeOf (content) Is ContainerObject) Then
'                        Extract(level.ChildLevel)
'                    End If
'                End While
'            End Sub
'        End Class

'        '/**
'        '  <summary> Text String information.</summary>
'        '*/
'        Public NotInheritable Class TextStringWrapper
'            Inherits GraphicsObjectWrapper(Of ShowText)
'            Implements ITextString

'            Private Class ShowTextScanner
'                Implements ShowText.IScanner

'                Dim _wrapper As TextStringWrapper

'                Friend Sub New(ByVal wrapper As TextStringWrapper)
'                    Me._wrapper = wrapper
'                End Sub

'                Public Sub ScanChar(ByVal textChar As Char, ByVal textCharBox As RectangleF) Implements ShowText.IScanner.ScanChar
'                    _wrapper._textChars.Add(
'                                    New TextChar(
'                                                textChar,
'                                                textCharBox,
'                                                _wrapper._style,
'                                                False
'                                                  )
'                                             )
'                End Sub
'            End Class

'            Private _style As TextStyle
'            Private _textChars As List(Of TextChar)

'            Friend Sub New(ByVal scanner As ContentScanner)
'                MyBase.New(CType(scanner.Current, ShowText))
'                Me._textChars = New List(Of TextChar)()
'                Dim state As GraphicsState = scanner.State
'                _style = New TextStyle(
'                                state.Font,
'                                state.FontSize * state.Tm.Elements(3),
'                                state.RenderMode,
'                                state.StrokeColor,
'                                state.StrokeColorSpace,
'                                state.FillColor,
'                                state.FillColorSpace
'                                )
'                BaseDataObject.Scan(state, New ShowTextScanner(Me))

'            End Sub

'            Public Overrides ReadOnly Property Box As RectangleF? Implements ITextString.Box
'                Get
'                    If (_box Is Nothing) Then
'                        For Each textChar As TextChar In _textChars
'                            If (Not _box.HasValue) Then
'                                _box = textChar.Box
'                            Else
'                                _box = RectangleF.Union(_box.Value, textChar.Box)
'                            End If
'                        Next
'                    End If
'                    Return _box
'                End Get
'            End Property

'            '/**
'            '  <summary> Gets the text style.</summary>
'            '*/
'            Public ReadOnly Property Style As TextStyle
'                Get
'                    Return _style
'                End Get
'            End Property

'            Public ReadOnly Property Text As String Implements ITextString.Text
'                Get
'                    Dim textBuilder As StringBuilder = New StringBuilder()
'                    For Each textChar As TextChar In _textChars
'                        textBuilder.Append(textChar)
'                    Next
'                    Return textBuilder.ToString()
'                End Get
'            End Property

'            Public ReadOnly Property TextChars As List(Of TextChar) Implements ITextString.TextChars
'                Get
'                    Return _textChars
'                End Get
'            End Property

'        End Class

'        '/**
'        '  <summary> External Object information.</summary>
'        '*/
'        Public NotInheritable Class XObjectWrapper
'            Inherits GraphicsObjectWrapper(Of objects.XObject)

'            Private _name As PdfName
'            Private _xObject As xObjects.XObject

'            Friend Sub New(ByVal scanner As ContentScanner)
'                MyBase.New(CType(scanner.Current, objects.XObject))
'                Dim context As IContentContext = scanner.ContentContext
'                Dim ctm As Matrix = scanner.State.Ctm
'                Me._box = New RectangleF(
'                                    ctm.Elements(4),
'                                    context.Box.Height - ctm.Elements(5),
'                                    ctm.Elements(0),
'                                    Math.Abs(ctm.Elements(3))
'                                    )
'                Me._name = BaseDataObject.Name
'                Me._xObject = BaseDataObject.GetResource(context)
'            End Sub

'            '/**
'            '  <summary> Gets the corresponding resource key.</summary>
'            '*/
'            Public ReadOnly Property Name As PdfName
'                Get
'                    Return _name
'                End Get
'            End Property

'            '/**
'            '  <summary>Gets the external object.</summary>
'            '*/
'            Public ReadOnly Property XObject As xObjects.XObject
'                Get
'                    Return _xObject
'                End Get
'            End Property


'        End Class

'#End Region

'#Region "Static"
'#Region "fields"

'        Private Shared ReadOnly _StartIndex As Integer = -1

'#End Region
'#End Region

'#Region "dynamic"
'#Region "fields"
'        '/**
'        '  Child level.
'        '*/
'        Private _childLevel As ContentScanner

'        '/**
'        '  Content objects collection.
'        '*/
'        Private _contents As Contents
'        '/**
'        '  Current object index at Me level.
'        '*/
'        Private _index As Integer = 0
'        '/**
'        '  Object collection at Me level.
'        '*/
'        Private _objects As IList(Of ContentObject)
'        '/**
'        '  Parent level.
'        '*/
'        Private _parentLevel As ContentScanner
'        '/**
'        '  Current graphics state.
'        '*/
'        Private _state As GraphicsState

'        '/**
'        '  Rendering context.
'        '*/
'        Private _renderContext As Graphics
'        '/**
'        '  Rendering object.
'        '*/
'        Private _renderObject As GraphicsPath
'        '/**
'        '  Device-space size of the rendering canvas.
'        '*/
'        Private _renderSize As SizeF?

'#End Region

'#Region "constructors"
'        '/**
'        '  <summary>Instantiates a top-level content scanner.</summary>
'        '                                                                    <param name="contents">Content objects collection to scan.</param>
'        '*/
'        Public Sub New(ByVal contents As Contents)
'            Me._parentLevel = Nothing
'            Me._contents = contents
'            Me._objects = Me._contents
'            MoveStart()
'        End Sub

'        '/**
'        '  <summary>Instantiates a top-level content scanner.</summary>
'        '                                                                    <param name="contentContext">Content context containing the content objects collection to scan.</param>
'        '*/
'        Public Sub New(ByVal contentContext As IContentContext)
'            Me.New(contentContext.Contents)
'        End Sub

'        '/**
'        '  <summary>Instantiates a child-level content scanner for <see cref="org.dmdpdf.documents.contents.xObjects.FormXObject">external form</see>.</summary>
'        '                                                                    <param name="formXObject">External form.</param>
'        '                                                                    <param name="parentLevel">Parent scan level.</param>
'        '*/
'        Public Sub New(ByVal formXObject As xObjects.FormXObject, ByVal parentLevel As ContentScanner)
'            Me._parentLevel = parentLevel
'            Me._contents = formXObject.Contents
'            Me._objects = Me._contents
'            Dim h As New OnStartHandler(formXObject)
'            AddHandler OnStart, AddressOf h.Handle
'            MoveStart()
'        End Sub

'        Private Class OnStartHandler

'            Public formXObject As xObjects.FormXObject

'            Public Sub New(ByVal o As xObjects.FormXObject)
'                Me.formXObject = o
'            End Sub

'            Public Sub Handle(ByVal scanner As ContentScanner)
'                ' Adjust the initial graphics state to the external form context!
'                scanner.State.Ctm.Multiply(FormXObject.Matrix)
'                '/*
'                '  TODO: On rendering, clip according to the form dictionary's BBox entry!
'                '*/
'            End Sub
'        End Class



'        '/**
'        '  <summary>Instantiates a child-level content scanner.</summary>
'        '                                                                    <param name="parentLevel">Parent scan level.</param>
'        '*/
'        Private Sub New(ByVal parentLevel As ContentScanner)
'            Me._parentLevel = parentLevel
'            Me._contents = parentLevel._contents
'            Me._objects = CType(parentLevel.Current, CompositeObject).Objects
'            MoveStart()
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"

'        '/**
'        '  <summary>Gets the size of the current imageable area.</summary>
'        '                                                                    <remarks>It can be either the user-space area (dry scanning)
'        '  or the device-space area (wet scanning).</remarks>
'        '*/
'        Public ReadOnly Property CanvasSize As SizeF
'            Get
'                If (_renderSize.HasValue) Then
'                    Return _renderSize.Value ' Device-dependent (device-space) area
'                Else
'                    Return ContentContext.Box.Size ' Device-independent (user-space) area.
'                End If
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the current child scan level.</summary>
'        '*/
'        Public ReadOnly Property ChildLevel As ContentScanner
'            Get
'                Return _childLevel
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the content context associated to the content objects collection.</summary>
'        '*/
'        Public ReadOnly Property ContentContext As IContentContext
'            Get
'                Return _contents.ContentContext
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the content objects collection Me scanner is inspecting.</summary>
'        '*/
'        Public ReadOnly Property Contents As Contents
'            Get
'                Return _contents
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets/Sets the current content object.</summary>
'        '*/
'        Public Property Current As ContentObject
'            Get
'                If (_index < 0 OrElse _index >= _objects.Count) Then Return Nothing
'                Return _objects(_index)
'            End Get
'            Set(ByVal value As ContentObject)
'                _objects(_index) = value
'                Refresh()
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets the current content object's information.</summary>
'        '*/
'        Public ReadOnly Property CurrentWrapper As GraphicsObjectWrapper
'            Get
'                Return GraphicsObjectWrapper.Get(Me)
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the current position.</summary>
'        '*/
'        Public ReadOnly Property Index As Integer
'            Get
'                Return _index
'            End Get
'        End Property

'        '/**
'        '  <summary>Inserts a content object at the current position.</summary>
'        '*/
'        Public Sub Insert(ByVal obj As ContentObject)
'            If (_index = -1) Then
'                _index = 0
'            End If
'            _objects.Insert(_index, obj)
'            Refresh()
'        End Sub

'        '/**
'        '  <summary>Inserts content objects at the current position.</summary>
'        '                                                                        <remarks>After the insertion is complete, the lastly-inserted content object is at the current position.</remarks>
'        '*/
'        Public Sub Insert(Of T As ContentObject)(ByVal objects As ICollection(Of T))
'            Dim index As Integer = 0
'            Dim count As Integer = objects.Count
'            For Each obj As ContentObject In objects
'                Insert(obj)
'                index += 1
'                If (index < count) Then
'                    MoveNext()
'                End If
'            Next
'        End Sub

'        '/**
'        '  <summary>Gets whether Me level Is the root Of the hierarchy.</summary>
'        '*/
'        Public Function IsRootLevel() As Boolean
'            Return _parentLevel Is Nothing
'        End Function

'        '/**
'        '  <summary>Moves To the Object at the given position.</summary>
'        '  <param name = "index" > New position.</param>
'        '  <returns>Whether the Object was successfully reached.</returns>
'        '*/
'        Public Function Move(ByVal Index As Integer)
'            If (Me._index > Index) Then
'                MoveStart()
'            End If
'            While (Me._index < Index AndAlso MoveNext())

'            End While
'            Return (Current IsNot Nothing)
'        End Function

'        '/**
'        '  <summary>Moves after the last Object.</summary>
'        '*/
'        Public Sub MoveEnd()
'            MoveLast()
'            MoveNext()
'        End Sub

'        '/**
'        '  <summary>Moves To the first Object.</summary>
'        '  <returns>Whether the first Object was successfully reached.</returns>
'        '*/
'        Public Function MoveFirst()
'            MoveStart()
'            Return MoveNext()
'        End Function

'        '/**
'        '  <summary>Moves To the last Object.</summary>
'        '  <returns>Whether the last Object was successfully reached.</returns>
'        '*/
'        Public Function MoveLast() As Boolean
'            Dim lastIndex As Integer = _objects.Count - 1
'            While (_index < lastIndex)
'                MoveNext()
'            End While
'            Return Current IsNot Nothing
'        End Function

'        '/**
'        '  <summary>Moves To the Next Object.</summary>
'        '  <returns>Whether the Next Object was successfully reached.</returns>
'        '*/
'        Public Function MoveNext() As Boolean
'            ' Scanning the current graphics state...
'            Dim currentObject As ContentObject = Me.Current
'            If (currentObject IsNot Nothing) Then
'                currentObject.Scan(_state)
'            End If

'            ' Moving to the next object...
'            If (_index < _objects.Count) Then
'                _index += 1
'                Refresh()
'            End If

'            Return Current IsNot Nothing
'        End Function

'        '/**
'        '  <summary>Moves before the first Object.</summary>
'        '*/
'        Public Sub MoveStart()
'            _index = _StartIndex
'            If (_state Is Nothing) Then
'                If (_parentLevel Is Nothing) Then
'                    _state = New GraphicsState(Me)
'                Else
'                    _state = _parentLevel._state.Clone(Me)
'                End If
'            Else
'                If (_parentLevel Is Nothing) Then
'                    _state.Initialize()
'                Else
'                    _parentLevel._state.CopyTo(_state)
'                End If
'            End If

'            NotifyStart()
'            Refresh()
'        End Sub

'        '/**
'        '  <summary>Gets the current parent Object.</summary>
'        '*/
'        Public ReadOnly Property Parent As CompositeObject
'            Get
'                If (_parentLevel Is Nothing) Then
'                    Return Nothing
'                Else
'                    Return CType(_parentLevel.Current, CompositeObject)
'                End If
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the parent scan level.</summary>
'        '*/
'        Public ReadOnly Property ParentLevel As ContentScanner
'            Get
'                Return _parentLevel
'            End Get
'        End Property

'        '/**
'        '  <summary>Removes the content Object at the current position.</summary>
'        '  <returns>Removed Object.</returns>
'        '*/
'        Public Function Remove() As ContentObject
'            Dim removedObject As ContentObject = Current
'            _objects.RemoveAt(_index)
'            Refresh()
'            Return removedObject
'        End Function

'        '/**
'        '  <summary>Renders the contents into the specified context.</summary>
'        '  <param name = "renderContext" > Rendering context.</param>
'        '  <param name = "renderSize" > Rendering canvas size.</param>
'        '*/
'        Public Sub Render(ByVal renderContext As Graphics, ByVal renderSize As SizeF)
'            Render(renderContext, renderSize, Nothing)
'        End Sub

'        '/**
'        '  <summary>Renders the contents into the specified Object.</summary>
'        '  <param name = "renderContext" > Rendering context.</param>
'        '  <param name = "renderSize" > Rendering canvas size.</param>
'        '  <param name = "renderObject" > Rendering Object.</param>
'        '*/
'        Public Sub Render(ByVal renderContext As Graphics, ByVal renderSize As SizeF, ByVal renderObject As GraphicsPath)
'            If (IsRootLevel()) Then
'                ' Initialize the context!
'                renderContext.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
'                renderContext.SmoothingMode = SmoothingMode.HighQuality

'                ' Paint the canvas background!
'                renderContext.Clear(System.Drawing.Color.White)
'            End If

'            Try
'                Me._renderContext = renderContext
'                Me._renderSize = renderSize
'                Me._renderObject = renderObject

'                ' Scan Me level for rendering!
'                MoveStart()
'                While (MoveNext())

'                End While
'            Finally
'                Me._renderContext = Nothing
'                Me._renderSize = Nothing
'                Me._renderObject = Nothing
'            End Try
'        End Sub

'        '/**
'        '  <summary>Gets the rendering context.</summary>
'        '  <returns><code>Nothing</code> In Case Of dry scanning.</returns>
'        '*/
'        Public ReadOnly Property RenderContext As Graphics
'            Get
'                Return _renderContext
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the rendering Object.</summary>
'        '  <returns><code>Nothing</code> In Case Of scanning outside a shape.</returns>
'        '*/
'        Public ReadOnly Property RenderObject As GraphicsPath
'            Get
'                Return _renderObject
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the root scan level.</summary>
'        '*/
'        Public ReadOnly Property RootLevel As ContentScanner
'            Get
'                Dim level As ContentScanner = Me
'                While (True)
'                    Dim parentLevel As ContentScanner = level.ParentLevel
'                    If (parentLevel Is Nothing) Then Return level
'                    level = parentLevel
'                End While
'                Return Nothing  'never happens but compiler dosn't know :-)
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the current graphics state applied To the current content Object.</summary>
'        '*/
'        Public ReadOnly Property State As GraphicsState
'            Get
'                Return _state
'            End Get
'        End Property

'#End Region

'#Region "protected"
'        '#pragma warning disable 0628
'        '/**
'        '  <summary>Notifies the scan start To listeners.</summary>
'        '*/
'        Protected Sub NotifyStart()
'            '{
'            '        If (OnStart!= Nothing) Then
'            RaiseEvent OnStart(Me)
'        End Sub

'        '#pragma warning restore 0628
'#End Region

'#Region "private"

'        '    /**
'        '  <summary>Synchronizes the scanner state.</summary>
'        '*/
'        Private Sub Refresh()
'            If (TypeOf (Current) Is CompositeObject) Then
'                _childLevel = New ContentScanner(Me)
'            Else
'                _childLevel = Nothing
'            End If
'        End Sub

'#End Region
'#End Region
'#End Region

'    End Class

'End Namespace