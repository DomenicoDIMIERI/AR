'/*
'  Copyright 2007 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it):
'      - enhancement of [MG]'s line alignment implementation.
'    * Manuel Guilbault (code contributor, manuel.guilbault@gmail.com):
'      - line alignment.

'  Me file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  Me Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  Me Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With Me
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  Me list Of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.contents.composition

    '/**
    '  <summary>
    '  <para> Content block composer.</para>
    '    <para> It provides content positioning functionalities For page typesetting.</para>
    '  </summary>
    '*/
    '/*
    '  TODO: Manage all the graphics parameters (especially
    '  those text-related, Like horizontal scaling etc.) Using ContentScanner -- see PDF:1.6:5.2-3!!!
    '*/
    Public NotInheritable Class BlockComposer

#Region "types"
        Private NotInheritable Class ContentPlaceholder
            Inherits Operation

            Public _objects As List(Of ContentObject) = New List(Of ContentObject)()

            Public Sub New()
                MyBase.New(Nothing)
            End Sub

            Public ReadOnly Property Objects As List(Of ContentObject)
                Get
                    Return Me._objects
                End Get
            End Property

            Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)
                For Each obj As ContentObject In Me._objects
                    obj.WriteTo(stream, context)
                Next
            End Sub

        End Class

        Private NotInheritable Class Row

            '      /**
            '  <summary> Row MyBase line.</summary>
            '*/
            Public _BaseLine As Double
            '/**
            '  <summary> Row 's graphics objects container.</summary>
            '*/
            Public _Container As ContentPlaceholder
            Public _Height As Double
            '/**
            '  <summary> Row 's objects.</summary>
            '*/
            Public _Objects As List(Of RowObject) = New List(Of RowObject)()
            '/**
            '  <summary> Number Of space characters.</summary>
            '*/
            Public _SpaceCount As Integer = 0
            Public _Width As Double
            '/**
            '  <summary> Vertical location relative To the block frame.</summary>
            '*/
            Public _Y As Double

            Friend Sub New(ByVal container As ContentPlaceholder, ByVal y As Double)
                Me._Container = container
                Me._Y = y
            End Sub
        End Class

        Private NotInheritable Class RowObject

            Public Enum TypeEnum
                Text
                XObject
            End Enum

            '/**
            '  <summary> Base line.</summary>
            '*/
            Public _BaseLine As Double
            '/**
            '  <summary> Graphics objects container associated To Me Object.</summary>
            '*/
            Public _Container As ContainerObject
            Public _Height As Double
            '/**
            '  <summary> Line alignment (can be either LineAlignmentEnum Or Double).</summary>
            '*/
            Public _LineAlignment As Object
            Public _SpaceCount As Integer
            Public _Type As TypeEnum
            Public _Width As Double

            Friend Sub New(ByVal type As TypeEnum, ByVal container As ContainerObject, ByVal height As Double, ByVal width As Double, ByVal spaceCount As Integer, ByVal lineAlignment As Object, ByVal baseLine As Double)
                Me._Type = type
                Me._Container = container
                Me._Height = height
                Me._Width = width
                Me._SpaceCount = spaceCount
                Me._LineAlignment = lineAlignment
                Me._BaseLine = baseLine
            End Sub

        End Class

#End Region

#Region "dynamic"
        '    /*
        '      NOTE: In order to provide fine-grained alignment,
        '      there are 2 postproduction state levels
        '        1- row level (see EndRow());
        '        2- block level (see End()).

        '      NOTE:     Graphics instructions ' layout follows Me scheme (XS-BNF syntax):
        '                block = {beginLocalState translation parameters rows endLocalState }
        '                beginLocalState {"q\r"}
        '        translation = {"1 0 0 1 " number ' ' number "cm\r" }
        '                parameters = { ... } // Graphics state parameters.
        '        rows = {Row * }
        '                Row = {Object * }
        '                Object = {parameters beginLocalState translation content endLocalState }
        '                content = { ... } // Text, image (And so On) showing operators.
        '        endLocalState = {"Q\r"}
        'NOTE:           all the graphics state parameters within a block are block-level ones,
        '      i.e.they can't be represented inside row's or row object's local state, in order to
        '                facilitate parameter reuse within the same block.
        '    */
#Region "fields"
        Private ReadOnly _baseComposer As PrimitiveComposer
        Private ReadOnly _scanner As ContentScanner

        Private _hyphenation As Boolean
        Private _hyphenationCharacter As Char = "-"c
        Private _lineAlignment As LineAlignmentEnum = LineAlignmentEnum.BaseLine
        Private _lineSpace As Length = New Length(0, Length.UnitModeEnum.Relative)
        Private _xAlignment As XAlignmentEnum
        Private _yAlignment As YAlignmentEnum

        '/** <summary>Area available for the block contents.</summary> */
        Private _frame As RectangleF
        '/** <summary>Actual area occupied by the block contents.</summary> */
        Private _boundBox As RectangleF

        Private _currentRow As Row
        Private _rowEnded As Boolean

        Private _container As LocalGraphicsState

        Private _lastFontSize As Double
#End Region

#Region "constructors"

        Public Sub New(ByVal baseComposer As PrimitiveComposer)
            Me._baseComposer = baseComposer
            Me._scanner = baseComposer.Scanner
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the MyBase composer.</summary>
        '*/
        Public ReadOnly Property BaseComposer As PrimitiveComposer
            Get
                Return Me._baseComposer
            End Get
        End Property

        '/**
        '  <summary> Begins a content block.</summary>
        '  <param name = "frame" > Block boundaries.</param>
        '  <param name = "xAlignment" > Horizontal alignment.</param>
        '  <param name = "yAlignment" > Vertical alignment.</param>
        '*/
        Public Sub Begin(ByVal frame As RectangleF, ByVal xAlignment As XAlignmentEnum, ByVal yAlignment As YAlignmentEnum)
            Me._frame = frame
            Me._xAlignment = xAlignment
            Me._yAlignment = yAlignment
            _lastFontSize = 0

            '// Open the block local state!
            '/*
            '  NOTE: Me device allows a fine-grained control over the block representation.
            '  It MUST be coupled With a closing statement On block End.
            '*/
            _container = _baseComposer.BeginLocalState()

            _boundBox = New RectangleF(frame.X, frame.Y, frame.Width, 0)

            BeginRow()
        End Sub

        '/**
        '  <summary> Gets the area occupied by the already-placed block contents.</summary>
        '*/
        Public ReadOnly Property BoundBox As RectangleF
            Get
                Return _boundBox
            End Get
        End Property

        '/**
        '  <summary> Ends the content block.</summary>
        '*/
        Public Sub [End]()
            ' End last row!
            EndRow(True)

            ' Block translation.
            _container.Objects.Insert(
                                0,
                                New ModifyCTM(
                                  1, 0, 0, 1,
                                  _boundBox.X,
                                  -_boundBox.Y
                                  )
                                )

            ' Close the block local state!
            _baseComposer.End()
        End Sub

        '/**
        '  <summary> Gets the area where To place the block contents.</summary>
        '*/
        Public ReadOnly Property Frame As RectangleF
            Get
                Return _frame
            End Get
        End Property

        '/**
        '  <summary> Gets/Sets whether the hyphenation algorithm has To be applied.</summary>
        '  <remarks> Initial value: <code>False</code>.</remarks>
        '*/
        Public Property Hyphenation As Boolean
            Get
                Return _hyphenation
            End Get
            Set(ByVal value As Boolean)
                _hyphenation = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the character shown at the End Of the line before a hyphenation break.
        '  </summary>
        '  <remarks> Initial value: hyphen symbol(U + 2D, i.e. '-').</remarks>
        '*/
        Public Property HyphenationCharacter As Char
            Get
                Return _hyphenationCharacter
            End Get
            Set(ByVal value As Char)
                _hyphenationCharacter = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the Default line alignment.</summary>
        '  <remarks> Initial value: <see cref = "LineAlignmentEnum.BaseLine" /> .</remarks>
        '*/
        Public Property LineAlignment As LineAlignmentEnum
            Get
                Return _lineAlignment
            End Get
            Set(ByVal value As LineAlignmentEnum)
                _lineAlignment = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the text interline spacing.</summary>
        '  <remarks> Initial value: 0.</remarks>
        '*/
        Public Property LineSpace As Length
            Get
                Return _lineSpace
            End Get
            Set(ByVal value As Length)
                _lineSpace = value
            End Set
        End Property

        '/**
        '  <summary> Gets the content scanner.</summary>
        '*/
        Public ReadOnly Property Scanner As ContentScanner
            Get
                Return _scanner
            End Get
        End Property

        '/**
        '  <summary> Ends current paragraph.</summary>
        '*/
        Public Sub ShowBreak()
            EndRow(True)
            BeginRow()
        End Sub

        '/**
        '  <summary> Ends current paragraph, specifying the offset Of the Next one.</summary>
        '  <remarks> Me functionality allows higher-level features such As paragraph indentation
        '  And margin.</remarks>
        '  <param name = "offset" > Relative location Of the Next paragraph.</param>
        '*/
        Public Sub ShowBreak(ByVal offset As SizeF)
            ShowBreak()

            _currentRow._Y += offset.Height
            _currentRow._Width = offset.Width
        End Sub

        '/**
        '  <summary> Ends current paragraph, specifying the alignment Of the Next one.</summary>
        '  <remarks> Me functionality allows higher-level features such As paragraph indentation And margin.</remarks>
        '  <param name = "xAlignment" > Horizontal alignment.</param>
        '*/
        Public Sub ShowBreak(ByVal xAlignment As XAlignmentEnum)
            ShowBreak()

            Me._xAlignment = xAlignment
        End Sub

        '/**
        '  <summary> Ends current paragraph, specifying the offset And alignment Of the Next one.</summary>
        '  <remarks> Me functionality allows higher-level features such As paragraph indentation And margin.</remarks>
        '  <param name = "offset" > Relative location Of the Next paragraph.</param>
        '  <param name = "xAlignment" > Horizontal alignment.</param>
        '*/
        Public Sub ShowBreak(ByVal offset As SizeF, ByVal xAlignment As XAlignmentEnum)
            ShowBreak(offset)

            Me._xAlignment = xAlignment
        End Sub

        '/**
        '  <summary> Shows text.</summary>
        '  <remarks> Default line alignment Is applied.</remarks>
        '  <param name = "text" > Text To show.</param>
        '  <returns> Last shown character index.</returns>
        '*/
        Public Function ShowText(ByVal text As String) As Integer
            Return ShowText(text, _lineAlignment)
        End Function

        '/**
        '  <summary> Shows text.</summary>
        '  <param name = "text" > text To show.</param>
        '  <param name = "lineAlignment" > line alignment. It can be
        '    <List type = "bullet" >
        '      <item><see cref="LineAlignmentEnum"/></item>
        '    <item> <see cref = "Length" >: arbitrary super -/ Sub() - script, depending On whether the value Is
        '      positive Or Not .</item>
        '    </list>
        '  </param>
        '  <returns> Last shown character index.</returns>
        '*/
        Public Function ShowText(ByVal text As String, ByVal lineAlignment As Object) As Integer
            If (text = "Welcome") Then
                Debug.Print("Oops")
            End If
            If (_currentRow Is Nothing OrElse text Is Nothing) Then Return 0

            Dim state As ContentScanner.GraphicsState = _baseComposer.State
            Dim font As fonts.Font = state.Font
            Dim fontSize As Double = state.FontSize
            Dim lineHeight As Double = font.GetLineHeight(fontSize)
            Dim baseLine As Double = font.GetAscent(fontSize)
            lineAlignment = ResolveLineAlignment(lineAlignment)

            Dim textFitter As TextFitter = New TextFitter(text, 0, font, fontSize, _hyphenation, _hyphenationCharacter)
            Dim textLength As Integer = text.Length
            Dim index As Integer = 0

            While (True)
                If (_currentRow._Width = 0) Then ' Current Then Row has just begun.
                    ' Removing leading space...
                    While (True)
                        If (index = textLength) Then ' text Then End reached.
                            GoTo endTextShowing
                        ElseIf (text(index) <> " "c) Then ' No more leading spaces.
                            Exit While
                        End If
                        index += 1
                    End While
                End If

                If (OperationUtils.Compare(_currentRow._Y + lineHeight, _frame.Height) = 1) Then '// text's height exceeds block's remaining vertical space.
                    ' Terminate the current row And exit!
                    EndRow(False)
                    GoTo endTextShowing
                End If

                ' Does the text fit?
                If (textFitter.Fit(index, _frame.Width - _currentRow._Width, _currentRow._SpaceCount = 0)) Then
                    ' Get the fitting text!
                    Dim textChunk As String = textFitter.FittedText
                    Dim textChunkWidth As Double = textFitter.FittedWidth
                    Dim textChunkLocation As PointF = New PointF(CSng(_currentRow._Width), CSng(_currentRow._Y))

                    ' Insert the fitting text!
                    Dim obj As RowObject
                    '{
                    obj = New RowObject(
                              RowObject.TypeEnum.Text,
                              _baseComposer.BeginLocalState(),
                              lineHeight,
                              textChunkWidth,
                              CountOccurrence(" "c, textChunk),
                              lineAlignment,
                              baseLine
                              )
                    _baseComposer.ShowText(textChunk, textChunkLocation)
                    _baseComposer.End() ' Closes the row Object's local state.
                    '}
                    AddRowObject(obj, lineAlignment)

                    index = textFitter.EndIndex
                End If

                ' Evaluating trailing text...
                While (True)
                    If (index = textLength) Then ' text Then End reached.
                        GoTo endTextShowing
                    End If

                    Select Case (text(index))
                        Case ControlChars.Cr ' '\r':
                            'break;
                        Case ControlChars.Lf ' '\n':
                            ' New paragraph!
                            index += 1
                            ShowBreak()
                            GoTo endTrailParsing
                        Case Else
                            ' New row (within the same paragraph)!
                            EndRow(False)
                            BeginRow()
                            GoTo endTrailParsing
                    End Select

                    index += 1
                End While
endTrailParsing:
            End While
endTextShowing:
            If (index >= 0 AndAlso lineAlignment.Equals(LineAlignmentEnum.BaseLine)) Then
                _lastFontSize = fontSize
            End If

            Return index
        End Function

        '/**
        '  <summary> Shows the specified external Object.</summary>
        '  <remarks> Default line alignment Is applied.</remarks>
        '  <param name = "xObject" > External Object.</param>
        '  <param name = "size" > Size Of the external Object.</param>
        '  <returns> Whether the external Object was successfully shown.</returns>
        '*/
        Public Function ShowXObject(ByVal xObject As xObjects.XObject, ByVal size As SizeF?) As Boolean
            Return ShowXObject(xObject, size, _lineAlignment)
        End Function

        '/**
        '  <summary> Shows the specified external Object.</summary>
        '  <param name = "xObject" > External Object.</param>
        '  <param name = "size" > size Of the external Object.</param>
        '  <param name = "lineAlignment" > line alignment. It can be
        '    <List type = "bullet" >
        '      <item><see cref="LineAlignmentEnum"/></item>
        '    <item> <see cref = "Length" >: arbitrary super -/ Sub() - script, depending On whether the value Is
        '      positive Or Not .</item>
        '    </list>
        '  </param>
        '  <returns> Whether the external Object was successfully shown.</returns>
        '*/
        Public Function ShowXObject(ByVal xObject As xObjects.XObject, ByVal size As SizeF?, ByVal lineAlignment As Object) As Boolean
            If (_currentRow Is Nothing OrElse xObject Is Nothing) Then Return False

            If (Not size.HasValue) Then
                size = xObject.Size
            End If
            lineAlignment = ResolveLineAlignment(lineAlignment)

            While (True)
                If (OperationUtils.Compare(_currentRow._Y + size.Value.Height, _frame.Height) = 1) Then ' Object 's height exceeds block's remaining vertical space.
                    ' Terminate current row And exit!
                    EndRow(False)
                    Return False
                ElseIf (OperationUtils.Compare(_currentRow._Width + size.Value.Width, _frame.Width) < 1) Then ' There 's room for the object in the current row.
                    Dim location As PointF = New PointF(CSng(_currentRow._Width), CSng(_currentRow._Y))
                    Dim obj As RowObject
                    '{
                    obj = New RowObject(
                                  RowObject.TypeEnum.XObject,
                                  _baseComposer.BeginLocalState(),
                                  size.Value.Height,
                                  size.Value.Width,
                                  0,
                                  lineAlignment,
                                  size.Value.Height
                                  )
                    _baseComposer.ShowXObject(xObject, location, size)
                    _baseComposer.End() ' Closes the row Object's local state.
                    '}
                    AddRowObject(obj, lineAlignment)

                    Return True
                Else ' There's NOT enough room for the object in the current row.
                    ' Go to next row!
                    EndRow(False)
                    BeginRow()
                End If
            End While
        End Function

        '/**
        '  <summary> Gets the horizontal alignment applied To the current content block.</summary>
        '*/
        Public ReadOnly Property XAlignment As XAlignmentEnum
            Get
                Return _xAlignment
            End Get
        End Property

        '/**
        '  <summary> Gets the vertical alignment applied To the current content block.</summary>
        '*/
        Public ReadOnly Property YAlignment As YAlignmentEnum
            Get
                Return _yAlignment
            End Get
        End Property

#End Region

#Region "private"
        '/**
        '  <summary> Adds an Object To the current row.</summary>
        '  <param name = "obj" > Object To add.</param>
        '  <param name = "lineAlignment" > Object 's line alignment.</param>
        '*/
        Private Sub AddRowObject(ByVal obj As RowObject, ByVal lineAlignment As Object)
            _currentRow._Objects.Add(obj)
            _currentRow._SpaceCount += obj._SpaceCount
            _currentRow._Width += obj._Width

            If (TypeOf (lineAlignment) Is Double OrElse lineAlignment.Equals(LineAlignmentEnum.BaseLine)) Then
                Dim gap As Double = 0
                If (TypeOf (lineAlignment) Is Double) Then gap = CDbl(lineAlignment)
                Dim superGap As Double = obj._BaseLine + gap - _currentRow._BaseLine
                If (superGap > 0) Then
                    _currentRow._Height += superGap
                    _currentRow._BaseLine += superGap
                End If
                Dim subGap As Double = _currentRow._BaseLine + (obj._Height - obj._BaseLine) - gap - _currentRow._Height
                If (subGap > 0) Then
                    _currentRow._Height += subGap
                End If
            ElseIf (obj._Height > _currentRow._Height) Then
                _currentRow._Height = obj._Height
            End If
        End Sub

        '/**
        '  <summary> Begins a content row.</summary>
        '*/
        Private Sub BeginRow()
            _rowEnded = False

            Dim rowY As Double = _boundBox.Height
            If (rowY > 0) Then
                Dim state As ContentScanner.GraphicsState = _baseComposer.State
                rowY += _lineSpace.GetValue(state.Font.GetLineHeight(state.FontSize))
            End If
            _currentRow = New Row(CType(_baseComposer.Add(New ContentPlaceholder()), ContentPlaceholder), rowY)
        End Sub

        Private Function CountOccurrence(ByVal value As Char, ByVal text As String) As Integer
            Dim count As Integer = 0
            Dim fromIndex As Integer = 0
            Do
                Dim foundIndex As Integer = text.IndexOf(value, fromIndex)
                If (foundIndex = -1) Then Return count
                count += 1
                fromIndex = foundIndex + 1
            Loop While (True)
            Return -1
        End Function

        '/**
        '  <summary> Ends the content row.</summary>
        '  <param name = "broken" > Indicates whether Me Is the End Of a paragraph.</param>
        '*/
        Private Sub EndRow(ByVal broken As Boolean)
            If (_rowEnded) Then Return

            _rowEnded = True

            Dim objectXOffsets As Double() = New Double(_currentRow._Objects.Count - 1) {} ' // Horizontal Object displacements.
            Dim wordSpace As Double = 0 ' Exceeding space among words.
            Dim rowXOffset As Double = 0 ' Horizontal row offset.

            Dim objects As List(Of RowObject) = _currentRow._Objects

            ' Horizontal alignment.
            Dim xAlignment As XAlignmentEnum = Me._xAlignment
            Select Case (xAlignment)
                Case XAlignmentEnum.Left 'break;
                Case XAlignmentEnum.Right : rowXOffset = _frame.Width - _currentRow._Width 'break;
                Case XAlignmentEnum.Center : rowXOffset = (_frame.Width - _currentRow._Width) / 2 'break;
                Case XAlignmentEnum.Justify
                    ' Are there NO spaces?
                    If (_currentRow._SpaceCount = 0 OrElse broken) Then ' NO spaces.
                        ' NOTE: Me situation equals a simple left alignment. */
                        xAlignment = XAlignmentEnum.Left
                    Else ' Spaces exist.
                        ' Calculate the exceeding spacing among the words!
                        wordSpace = (_frame.Width - _currentRow._Width) / _currentRow._SpaceCount

                        ' Define the horizontal offsets for justified alignment.
                        Dim count As Integer = objects.Count
                        For index As Integer = 1 To count - 1
                            '/*
                            '  NOTE: The offset represents the horizontal justification gap inserted
                            '  at the left side Of Each Object.
                            '*/
                            objectXOffsets(index) = objectXOffsets(index - 1) + objects(index - 1)._SpaceCount * wordSpace
                        Next
                    End If
                    'break;
            End Select

            Dim wordSpaceOperation As SetWordSpace = New SetWordSpace(wordSpace)

            ' Vertical alignment And translation.
            For index As Integer = objects.Count - 1 To 0 Step -1
                Dim obj As RowObject = objects(index)

                ' Vertical alignment.
                Dim objectYOffset As Double = 0
                '{
                Dim LineAlignment As LineAlignmentEnum
                Dim lineRise As Double
                '{
                Dim objectLineAlignment As Object = obj._LineAlignment
                If (TypeOf (objectLineAlignment) Is Double) Then
                    LineAlignment = LineAlignmentEnum.BaseLine
                    lineRise = CDbl(objectLineAlignment)
                Else
                    LineAlignment = CType(objectLineAlignment, LineAlignmentEnum)
                    lineRise = 0
                End If
                '}
                Select Case (LineAlignment)
                    Case LineAlignmentEnum.Top        '/* NOOP */ break;
                    Case LineAlignmentEnum.Middle : objectYOffset = -(_currentRow._Height - obj._Height) / 2 '                  break;
                    Case LineAlignmentEnum.BaseLine : objectYOffset = -(_currentRow._BaseLine - obj._BaseLine - lineRise) ' break;
                    Case LineAlignmentEnum.Bottom : objectYOffset = -(_currentRow._Height - obj._Height) 'break;
                    Case Else : Throw New NotImplementedException("Line alignment " & LineAlignment & " unknown.")
                End Select
                '}

                Dim containedGraphics As IList(Of ContentObject) = obj._Container.Objects
                ' Word spacing.
                containedGraphics.Insert(0, wordSpaceOperation)
                ' Translation.
                containedGraphics.Insert(
                                      0,
                                      New ModifyCTM(
                                        1, 0, 0, 1,
                                        objectXOffsets(index) + rowXOffset,
                                        objectYOffset
                                        )
                                      )
            Next

            ' Update the actual block height!
            _boundBox.Height = CSng(_currentRow._Y + _currentRow._Height)

            ' Update the actual block vertical location!
            Dim yOffset As Double
            Select Case (_yAlignment)
                Case YAlignmentEnum.Bottom : yOffset = _frame.Height - _boundBox.Height '          break;
                Case YAlignmentEnum.Middle : yOffset = (_frame.Height - _boundBox.Height) / 2  'break;
                    'Case YAlignmentEnum.Top
                Case Else
                    yOffset = 0
                    'break;
            End Select
            _boundBox.Y = CSng(_frame.Y + yOffset)

            ' Discard the current row!
            _currentRow = Nothing
        End Sub

        Private Function ResolveLineAlignment(ByVal lineAlignment As Object) As Object
            If (Not (TypeOf (lineAlignment) Is LineAlignmentEnum OrElse TypeOf (lineAlignment) Is Length)) Then Throw New ArgumentException("MUST be either LineAlignmentEnum or Length.", "lineAlignment")

            If (LineAlignment.Equals(LineAlignmentEnum.Super)) Then
                lineAlignment = New Length(0.33, Length.UnitModeEnum.Relative)
            ElseIf (LineAlignment.Equals(LineAlignmentEnum.Sub)) Then
                lineAlignment = New Length(-0.33, Length.UnitModeEnum.Relative)
            End If
            If (TypeOf (lineAlignment) Is Length) Then
                If (_lastFontSize = 0) Then
                    _lastFontSize = _baseComposer.State.FontSize
                End If
                lineAlignment = CType(lineAlignment, Length).GetValue(_lastFontSize)
            End If

            Return lineAlignment
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace

''/*
''  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it):
''      - enhancement of [MG]'s line alignment implementation.
''    * Manuel Guilbault (code contributor, manuel.guilbault@gmail.com):
''      - line alignment.

''  Me file should be part of the source code distribution of "PDF Clown library" (the
''  Program): see the accompanying README files for more info.

''  Me Program is free software; you can redistribute it and/or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 of the License, or (at your option) any later version.

''  Me Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy of the GNU Lesser General Public License along with Me
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  Me list of conditions.
''*/

'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.documents.contents.fonts
'Imports DMD.org.dmdpdf.documents.contents.objects
'Imports DMD.org.dmdpdf.documents.contents.xObjects
'Imports DMD.org.dmdpdf.objects
'Imports DMD.org.dmdpdf.util.math

'Imports System
'Imports System.Collections.Generic
'Imports System.Drawing

'Namespace DMD.org.dmdpdf.documents.contents.composition

'    '/**
'    '  <summary>
'    '    <para>Content block composer.</para>
'    '    <para>It provides content positioning functionalities for page typesetting.</para>
'    '  </summary>
'    '*/
'    '/*
'    '  TODO: Manage all the graphics parameters (especially
'    '  those text-related, like horizontal scaling etc.) using ContentScanner -- see PDF:1.6:5.2-3!!!
'    '*/
'    Public NotInheritable Class BlockComposer

'#Region "types"

'        Private NotInheritable Class ContentPlaceholder
'            Inherits Operation

'            Public _objects As List(Of ContentObject) = New List(Of ContentObject)()

'            Public Sub New()
'                MyBase.New(Nothing)
'            End Sub

'            Public ReadOnly Property Objects As List(Of ContentObject)
'                Get
'                    Return Me._objects
'                End Get
'            End Property

'            Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)
'                For Each obj As ContentObject In _objects
'                    obj.WriteTo(stream, context)
'                Next
'            End Sub
'        End Class


'        Private NotInheritable Class Row

'            '      /**
'            '  <summary> Row MyBase line.</summary>
'            '*/
'            Public BaseLine As Double
'            '/**
'            '  <summary> Row 's graphics objects container.</summary>
'            '*/
'            Public Container As ContentPlaceholder
'            Public Height As Double
'            '/**
'            '  <summary> Row 's objects.</summary>
'            '*/
'            Public Objects As List(Of RowObject) = New List(Of RowObject)()
'            '/**
'            '  <summary> Number Of space characters.</summary>
'            '*/
'            Public SpaceCount As Integer = 0
'            Public Width As Double
'            '/**
'            '  <summary> Vertical location relative To the block frame.</summary>
'            '*/
'            Public Y As Double

'            Friend Sub New(ByVal container As ContentPlaceholder, ByVal y As Double)
'                Me.Container = container
'                Me.Y = y
'            End Sub
'        End Class

'        Private NotInheritable Class RowObject

'            Public Enum TypeEnum
'                Text
'                XObject
'            End Enum

'            '/**
'            '  <summary> Base line.</summary>
'            '*/
'            Public BaseLine As Double
'            '/**
'            '  <summary> Graphics objects container associated To Me Object.</summary>
'            '*/
'            Public Container As ContainerObject
'            Public Height As Double
'            '/**
'            '  <summary> Line alignment (can be either LineAlignmentEnum Or Double).</summary>
'            '*/
'            Public LineAlignment As Object
'            Public SpaceCount As Integer
'            Public Type As TypeEnum
'            Public Width As Double

'            Friend Sub New(ByVal type As TypeEnum, ByVal container As ContainerObject, ByVal height As Double,
'                            ByVal width As Double, ByVal spaceCount As Integer, ByVal lineAlignment As Object,
'                            ByVal baseLine As Double
'                      )
'                Me.Type = type
'                Me.Container = container
'                Me.Height = height
'                Me.Width = width
'                Me.SpaceCount = spaceCount
'                Me.LineAlignment = lineAlignment
'                Me.BaseLine = baseLine
'            End Sub
'        End Class

'#End Region

'#Region "dynamic"
'        '/*
'        '  NOTE: In order to provide fine-grained alignment,
'        '  there are 2 postproduction state levels
'        '    1- row level (see EndRow());
'        '    2- block level (see End()).

'        '  NOTE: Graphics instructions ' layout follows Me scheme (XS-BNF syntax):
'        '    block = { beginLocalState translation parameters rows endLocalState }
'        '    beginLocalState { "q\r" }
'        '    translation = { "1 0 0 1 " number ' ' number "cm\r" }
'        '    parameters = { ... } // Graphics state parameters.
'        '    rows = { row* }
'        '    row = { object* }
'        '    Object = {parameters beginLocalState translation content endLocalState }
'        '    content = { ... } // Text, image (And so on) showing operators.
'        '    endLocalState = { "Q\r" }
'        '  NOTE: all the graphics state parameters within a block are block-level ones,
'        '  i.e. they can't be represented inside row's or row object's local state, in order to
'        '  facilitate parameter reuse within the same block.
'        '*/
'#Region "fields"

'        Private ReadOnly _baseComposer As PrimitiveComposer
'        Private ReadOnly _scanner As ContentScanner

'        Private _hyphenation As Boolean
'        Private _hyphenationCharacter As Char = "-"c
'        Private _lineAlignment As LineAlignmentEnum = LineAlignmentEnum.BaseLine
'        Private _lineSpace As Length = New Length(0, Length.UnitModeEnum.Relative)
'        Private _xAlignment As XAlignmentEnum
'        Private _yAlignment As YAlignmentEnum

'        '/** <summary>Area available for the block contents.</summary> */
'        Private _frame As RectangleF
'        '** <summary>Actual area occupied by the block contents.</summary> */
'        Private _boundBox As RectangleF

'        Private _currentRow As Row
'        Private _rowEnded As Boolean

'        Private _container As LocalGraphicsState

'        Private _lastFontSize As Double
'#End Region

'#Region "constructors"

'        Public Sub New(ByVal baseComposer As PrimitiveComposer)
'            Me._baseComposer = baseComposer
'            Me._scanner = baseComposer.Scanner
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"

'        '/**
'        '  <summary> Gets the MyBase composer.</summary>
'        '*/
'        Public ReadOnly Property BaseComposer As PrimitiveComposer
'            Get
'                Return Me._baseComposer
'            End Get
'        End Property

'        '/**
'        '  <summary> Begins a content block.</summary>
'        '  <param name = "frame" > Block boundaries.</param>
'        '  <param name = "xAlignment" > Horizontal alignment.</param>
'        '  <param name = "yAlignment" > Vertical alignment.</param>
'        '*/
'        Public Sub Begin(ByVal frame As RectangleF, ByVal xAlignment As XAlignmentEnum, ByVal yAlignment As YAlignmentEnum)
'            Me._frame = frame
'            Me._xAlignment = xAlignment
'            Me._yAlignment = yAlignment
'            _lastFontSize = 0

'            '// Open the block local state!
'            '/*
'            '  NOTE: Me device allows a fine-grained control over the block representation.
'            '  It MUST be coupled With a closing statement On block End.
'            '*/
'            _container = _baseComposer.BeginLocalState()

'            _boundBox = New RectangleF(
'                            frame.X,
'                            frame.Y,
'                            frame.Width,
'                            0
'                            )

'            BeginRow()
'        End Sub

'        '/**
'        '  <summary> Gets the area occupied by the already-placed block contents.</summary>
'        '*/
'        Public ReadOnly Property BoundBox As RectangleF
'            Get
'                Return _boundBox
'            End Get
'        End Property

'        '/**
'        '  <summary> Ends the content block.</summary>
'        '*/
'        Public Sub [End]()
'            ' End last row!
'            EndRow(True)

'            ' Block translation.
'            '_container.Objects.Insert(
'            '                    0,
'            '                    New ModifyCTM(
'            '                      1, 0, 0, 1,
'            '                      _boundBox.X, // Horizontal translation.
'            '                      -_boundBox.Y // Vertical translation.
'            '                      )
'            '                    )
'            _container.Objects.Insert(
'                                0,
'                                New ModifyCTM(
'                                  1, 0, 0, 1,
'                                  _boundBox.X,
'                                  -_boundBox.Y
'                                  )
'                                )

'            ' Close the block local state!
'            _baseComposer.End()
'        End Sub

'        '/**
'        '  <summary> Gets the area where To place the block contents.</summary>
'        '*/
'        Public ReadOnly Property Frame As RectangleF
'            Get
'                Return Me._frame
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets/Sets whether the hyphenation algorithm has To be applied.</summary>
'        '  <remarks> Initial value: <code>False</code>.</remarks>
'        '*/
'        Public Property Hyphenation As Boolean
'            Get
'                Return Me._hyphenation
'            End Get
'            Set(ByVal value As Boolean)
'                Me._hyphenation = value
'            End Set
'        End Property

'        '/**
'        '  <summary> Gets/Sets the character shown at the End Of the line before a hyphenation break.
'        '  </summary>
'        '  <remarks> Initial value: hyphen symbol(U + 2D, i.e. '-').</remarks>
'        '*/
'        Public Property HyphenationCharacter As Char
'            Get
'                Return Me._hyphenationCharacter
'            End Get
'            Set(ByVal value As Char)
'                Me._hyphenationCharacter = value
'            End Set
'        End Property

'        '/**
'        '  <summary> Gets/Sets the Default line alignment.</summary>
'        '  <remarks> Initial value: <see cref = "LineAlignmentEnum.BaseLine" /> .</remarks>
'        '*/
'        Public Property LineAlignment As LineAlignmentEnum
'            Get
'                Return Me._lineAlignment
'            End Get
'            Set(ByVal value As LineAlignmentEnum)
'                Me._lineAlignment = value
'            End Set
'        End Property

'        '/**
'        '  <summary> Gets/Sets the text interline spacing.</summary>
'        '  <remarks> Initial value: 0.</remarks>
'        '*/
'        Public Property LineSpace As Length
'            Get
'                Return Me._lineSpace
'            End Get
'            Set(ByVal value As Length)
'                Me._lineSpace = value
'            End Set
'        End Property

'        '/**
'        '  <summary> Gets the content scanner.</summary>
'        '*/
'        Public ReadOnly Property Scanner As ContentScanner
'            Get
'                Return _scanner
'            End Get
'        End Property

'        '/**
'        '  <summary> Ends current paragraph.</summary>
'        '*/
'        Public Sub ShowBreak()
'            EndRow(True)
'            BeginRow()
'        End Sub

'        '/**
'        '  <summary> Ends current paragraph, specifying the offset Of the Next one.</summary>
'        '  <remarks> Me functionality allows higher-level features such As paragraph indentation
'        '  And margin.</remarks>
'        '  <param name = "offset" > Relative location Of the Next paragraph.</param>
'        '*/
'        Public Sub ShowBreak(ByVal offset As SizeF)
'            ShowBreak()

'            _currentRow.Y += offset.Height
'            _currentRow.Width = offset.Width
'        End Sub

'        '/**
'        '  <summary> Ends current paragraph, specifying the alignment Of the Next one.</summary>
'        '  <remarks> Me functionality allows higher-level features such As paragraph indentation And margin.</remarks>
'        '  <param name = "xAlignment" > Horizontal alignment.</param>
'        '*/
'        Public Sub ShowBreak(ByVal xAlignment As XAlignmentEnum)
'            ShowBreak()

'            Me._xAlignment = xAlignment
'        End Sub

'        '/**
'        '  <summary> Ends current paragraph, specifying the offset And alignment Of the Next one.</summary>
'        '  <remarks> Me functionality allows higher-level features such As paragraph indentation And margin.</remarks>
'        '  <param name = "offset" > Relative location Of the Next paragraph.</param>
'        '  <param name = "xAlignment" > Horizontal alignment.</param>
'        '*/
'        Public Sub ShowBreak(ByVal offset As SizeF, ByVal xAlignment As XAlignmentEnum)
'            ShowBreak(offset)

'            Me._xAlignment = xAlignment
'        End Sub

'        '/**
'        '  <summary> Shows text.</summary>
'        '  <remarks> Default line alignment Is applied.</remarks>
'        '  <param name = "text" > Text To show.</param>
'        '  <returns> Last shown character index.</returns>
'        '*/
'        Public Function ShowText(ByVal Text As String) As Integer
'            Return ShowText(Text, _lineAlignment)
'        End Function

'        '/**
'        '  <summary> Shows text.</summary>
'        '  <param name = "text" > Text To show.</param>
'        '  <param name = "lineAlignment" > line alignment. It can be
'        '    <List type = "bullet" >
'        '      <item><see cref="LineAlignmentEnum"/></item>
'        '    <item> <see cref = "Length" >: arbitrary super -/ Sub() - script, depending On whether the value Is
'        '      positive Or Not .</item>
'        '    </list>
'        '  </param>
'        '  <returns> Last shown character index.</returns>
'        '*/
'        Public Function ShowText(ByVal Text As String, ByVal lineAlignment As Object) As Integer
'            If (_currentRow Is Nothing OrElse Text Is Nothing) Then Return 0

'            Dim state As ContentScanner.GraphicsState = _baseComposer.State
'            Dim font As fonts.Font = state.Font
'            Dim fontSize As Double = state.FontSize
'            Dim lineHeight As Double = Font.GetLineHeight(fontSize)
'            Dim baseLine As Double = Font.GetAscent(fontSize)
'            lineAlignment = ResolveLineAlignment(lineAlignment)

'            Dim TextFitter As TextFitter = New TextFitter(
'                                                    Text,
'                                                    0,
'                                                    Font,
'                                                    fontSize,
'                                                    _hyphenation,
'                                                    _hyphenationCharacter
'                                                    )
'            Dim textLength As Integer = Text.Length
'            Dim Index As Integer = 0

'            While (True)
'                If (_currentRow.Width = 0) Then ' Current Then row has just begun.
'                    ' Removing leading space...
'                    While (True)
'                        If (Index = textLength) Then ' Text Then End reached.
'                            GoTo endTextShowing
'                        ElseIf (Text(Index) <> " "c) Then ' No more leading spaces.
'                            Exit While
'                        End If
'                        Index += 1
'                    End While
'                End If

'                If (OperationUtils.Compare(_currentRow.Y + lineHeight, _frame.Height) = 1) Then '// Text 's height exceeds block's remaining vertical space.
'                    ' Terminate the current row And exit!
'                    EndRow(False)
'                    GoTo endTextShowing
'                End If

'                ' Does the text fit?
'                If (TextFitter.Fit(Index, _frame.Width - _currentRow.Width, _currentRow.SpaceCount = 0)) Then '// Remaining row width.
'                    ' Get the fitting text!
'                    Dim textChunk As String = TextFitter.FittedText
'                    Dim textChunkWidth As Double = TextFitter.FittedWidth
'                    Dim textChunkLocation As PointF = New PointF(
'                                                    CSng(_currentRow.Width),
'                                                    CSng(_currentRow.Y)
'                                                    )

'                    ' Insert the fitting text!
'                    Dim obj As RowObject
'                    '{ // Opens the row object's local state.
'                    obj = New RowObject(
'                                    RowObject.TypeEnum.Text,
'                                    _baseComposer.BeginLocalState(),
'                                      lineHeight,
'                                      textChunkWidth,
'                                      CountOccurrence(" "c, textChunk),
'                                      lineAlignment,
'                                      baseLine
'                                      )
'                    _baseComposer.ShowText(textChunk, textChunkLocation)
'                    _baseComposer.End() ' Closes the row Object's local state.
'                    '}
'                    AddRowObject(obj, lineAlignment)

'                    Index = TextFitter.EndIndex
'                End If

'                ' Evaluating trailing text...
'                While (True)
'                    If (Index = textLength) Then ' Text Then End reached.
'                        GoTo endTextShowing
'                    End If

'                    Select Case (Text(Index))
'                        Case ControlChars.Cr
'                            'break;
'                        Case ControlChars.Lf
'                            ' New paragraph!
'                            Index += 1
'                            ShowBreak()
'                            GoTo endTrailParsing
'                        Case Else
'                            ' New row (within the same paragraph)!
'                            EndRow(False)
'                            BeginRow()
'                            GoTo endTrailParsing
'                    End Select

'                    Index += 1
'                End While
'endTrailParsing:
'            End While
'endTextShowing:

'            If (Index >= 0 AndAlso
'                lineAlignment.Equals(LineAlignmentEnum.BaseLine)) Then
'                _lastFontSize = fontSize
'            End If

'            Return Index
'        End Function

'        '/**
'        '  <summary> Shows the specified external Object.</summary>
'        '  <remarks> Default line alignment Is applied.</remarks>
'        '  <param name = "xObject" > External Object.</param>
'        '  <param name = "size" > Size Of the external Object.</param>
'        '  <returns> Whether the external Object was successfully shown.</returns>
'        '*/
'        Public Function ShowXObject(ByVal xObject As xObjects.XObject, ByVal size As SizeF?) As Boolean
'            Return ShowXObject(xObject, size, _lineAlignment)
'        End Function

'        '/**
'        '  <summary> Shows the specified external Object.</summary>
'        '  <param name = "xObject" > External Object.</param>
'        '  <param name = "size" > size Of the external Object.</param>
'        '  <param name = "lineAlignment" > line alignment. It can be
'        '    <List type = "bullet" >
'        '      <item><see cref="LineAlignmentEnum"/></item>
'        '    <item> <see cref = "Length" >: arbitrary super -/ Sub() - script, depending On whether the value Is
'        '      positive Or Not .</item>
'        '    </list>
'        '  </param>
'        '  <returns> Whether the external Object was successfully shown.</returns>
'        '*/
'        Public Function ShowXObject(ByVal xObject As xObjects.XObject, ByVal size As SizeF?, ByVal lineAlignment As Object) As Boolean
'            If (_currentRow Is Nothing OrElse xObject Is Nothing) Then Return False

'            If (Not size.HasValue) Then
'                size = xObject.Size
'            End If

'            lineAlignment = ResolveLineAlignment(lineAlignment)

'            While (True)
'                If (OperationUtils.Compare(_currentRow.Y + size.Value.Height, _frame.Height) = 1) Then ' Object Then 's height exceeds block's remaining vertical space.
'                    ' Terminate current row And exit!
'                    EndRow(False)
'                    Return False
'                ElseIf (OperationUtils.Compare(_currentRow.Width + size.Value.Width, _frame.Width) < 1) Then ' There Then 's room for the object in the current row.
'                    Dim location As PointF = New PointF(
'                                                CSng(_currentRow.Width),
'                                                CSng(_currentRow.Y)
'                                                )
'                    Dim obj As RowObject
'                    '{
'                    'obj = New RowObject(
'                    '                RowObject.TypeEnum.XObject,
'                    '                _baseComposer.BeginLocalState(), // Opens the row object's local state.
'                    '                size.Value.Height,
'                    '                size.Value.Width,
'                    '                0,
'                    '                lineAlignment,
'                    '                size.Value.Height
'                    '                )
'                    obj = New RowObject(
'                                    RowObject.TypeEnum.XObject,
'                                    _baseComposer.BeginLocalState(),
'                                    size.Value.Height,
'                                    size.Value.Width,
'                                    0,
'                                    lineAlignment,
'                                    size.Value.Height
'                                    )
'                    _baseComposer.ShowXObject(xObject, location, size)
'                    _baseComposer.End() ' Closes the row Object's local state.
'                    '}
'                    AddRowObject(obj, lineAlignment)

'                    Return True
'                Else ' There's NOT enough room for the object in the current row.
'                    ' Go to next row!
'                    EndRow(False)
'                    BeginRow()
'                End If
'            End While

'            Return False 'Should never happen
'        End Function

'        '/**
'        '  <summary> Gets the horizontal alignment applied To the current content block.</summary>
'        '*/
'        Public ReadOnly Property XAlignment As XAlignmentEnum
'            Get
'                Return _xAlignment
'            End Get
'        End Property

'        '/**
'        '  <summary> Gets the vertical alignment applied To the current content block.</summary>
'        '*/
'        Public ReadOnly Property YAlignment As YAlignmentEnum
'            Get
'                Return _yAlignment
'            End Get
'        End Property

'#End Region

'#Region "private"

'        '/**
'        '  <summary> Adds an Object To the current row.</summary>
'        '  <param name = "obj" > Object To add.</param>
'        '  <param name = "lineAlignment" > Object 's line alignment.</param>
'        '*/
'        Private Sub AddRowObject(ByVal obj As RowObject, ByVal lineAlignment As Object)
'            _currentRow.Objects.Add(obj)
'            _currentRow.SpaceCount += obj.SpaceCount
'            _currentRow.Width += obj.Width

'            If (TypeOf (lineAlignment) Is Double OrElse lineAlignment.Equals(LineAlignmentEnum.BaseLine)) Then
'                Dim gap As Double
'                If (TypeOf (lineAlignment) Is Double) Then
'                    gap = CDbl(lineAlignment)
'                Else
'                    gap = 0
'                End If
'                Dim superGap As Double = obj.BaseLine + gap - _currentRow.BaseLine
'                If (superGap > 0) Then
'                    _currentRow.Height += superGap
'                    _currentRow.BaseLine += superGap
'                End If
'                Dim subGap As Double = _currentRow.BaseLine + (obj.Height - obj.BaseLine) - gap - _currentRow.Height
'                If (subGap > 0) Then
'                    _currentRow.Height += subGap
'                End If

'            ElseIf (obj.Height > _currentRow.Height) Then
'                _currentRow.Height = obj.Height
'            End If
'        End Sub

'        '/**
'        '  <summary> Begins a content row.</summary>
'        '*/
'        Private Sub BeginRow()
'            _rowEnded = False

'            Dim rowY As Double = _boundBox.Height
'            If (rowY > 0) Then
'                Dim state As ContentScanner.GraphicsState = _baseComposer.State
'                rowY += _lineSpace.GetValue(state.Font.GetLineHeight(state.FontSize))
'            End If
'            _currentRow = New Row(CType(_baseComposer.Add(New ContentPlaceholder()), ContentPlaceholder), rowY)
'        End Sub

'        Private Function CountOccurrence(ByVal value As Char, ByVal text As String) As Integer
'            Dim count As Integer = 0
'            Dim fromIndex As Integer = 0
'            Do
'                Dim foundIndex As Integer = text.IndexOf(value, fromIndex)
'                If (foundIndex = -1) Then Return count
'                count += 1
'                fromIndex = foundIndex + 1
'            Loop While (True)

'            Return -1 'Should never happen
'        End Function

'        '/**
'        '  <summary> Ends the content row.</summary>
'        '  <param name = "broken" > Indicates whether Me Is the End Of a paragraph.</param>
'        '*/
'        Private Sub EndRow(ByVal broken As Boolean)
'            If (_rowEnded) Then Return
'            _rowEnded = True

'            Dim objectXOffsets As Double() = New Double(_currentRow.Objects.Count - 1) {}  ' Horizontal Object displacements.
'            Dim wordSpace As Double = 0 ' Exceeding space among words.
'            Dim rowXOffset As Double = 0 ' Horizontal row offset.

'            Dim objects As List(Of RowObject) = _currentRow.Objects

'            ' Horizontal alignment.
'            Dim xAlignment As XAlignmentEnum = Me._xAlignment
'            Select Case (xAlignment)
'                Case XAlignmentEnum.Left
'     'break;
'                Case XAlignmentEnum.Right
'                    rowXOffset = _frame.Width - _currentRow.Width
'          'break;
'                Case XAlignmentEnum.Center
'                    rowXOffset = (_frame.Width - _currentRow.Width) / 2
'                    'break;
'                Case XAlignmentEnum.Justify
'                    ' Are there NO spaces?
'                    If (_currentRow.SpaceCount = 0 OrElse broken) Then ' NO spaces.
'                        '  /* NOTE: Me situation equals a simple left alignment. */
'                        xAlignment = XAlignmentEnum.Left
'                    Else ' Spaces exist.
'                        ' Calculate the exceeding spacing among the words!
'                        wordSpace = (_frame.Width - _currentRow.Width) / _currentRow.SpaceCount

'                        ' Define the horizontal offsets for justified alignment.
'                        Dim count As Integer = objects.Count
'                        For index As Integer = 1 To count - 1
'                            '/*
'                            '  NOTE:       The offset represents the horizontal justification gap inserted
'                            '  at the left side Of Each Object.
'                            '*/
'                            objectXOffsets(index) = objectXOffsets(index - 1) + objects(index - 1).SpaceCount * wordSpace
'                        Next
'                    End If
'                    'break;
'            End Select

'            Dim wordSpaceOperation As SetWordSpace = New SetWordSpace(wordSpace)

'            ' Vertical alignment And translation.
'            '    For (
'            'Integer index = objects.Count - 1;
'            'index >= 0;
'            'index--
'            ')
'            For index As Integer = objects.Count - 1 To 0 Step -1
'                Dim obj As RowObject = objects(index)

'                ' Vertical alignment.
'                Dim objectYOffset As Double = 0
'                '{
'                Dim lineAlignment As LineAlignmentEnum
'                Dim lineRise As Double
'                '{
'                Dim objectLineAlignment As Object = obj.LineAlignment
'                If (TypeOf (objectLineAlignment) Is Double) Then
'                    lineAlignment = LineAlignmentEnum.BaseLine
'                    lineRise = CDbl(objectLineAlignment)
'                Else
'                    lineAlignment = CType(objectLineAlignment, LineAlignmentEnum)
'                    lineRise = 0
'                End If
'                '}

'                Select Case (lineAlignment)
'                    Case LineAlignmentEnum.Top
'                    '/* NOOP */
'                    'break;
'                    Case LineAlignmentEnum.Middle
'                        objectYOffset = -(_currentRow.Height - obj.Height) / 2
'                        'break;
'                    Case LineAlignmentEnum.BaseLine
'                        objectYOffset = -(_currentRow.BaseLine - obj.BaseLine - lineRise)
'                        'break;
'                    Case LineAlignmentEnum.Bottom
'                        objectYOffset = -(_currentRow.Height - obj.Height)
'                        'break;
'                    Case Else
'                        Throw New NotImplementedException("Line alignment " & lineAlignment & " unknown.")
'                End Select
'                '}

'                Dim containedGraphics As IList(Of ContentObject) = obj.Container.Objects
'                ' Word spacing.
'                containedGraphics.Insert(0, wordSpaceOperation)
'                ' Translation.
'                '      containedGraphics.Insert(
'                '          0,
'                'New ModifyCTM(
'                '  1, 0, 0, 1,
'                '  objectXOffsets(index) + rowXOffset, // Horizontal alignment.
'                '  objectYOffset // Vertical alignment.
'                '  )
'                ');
'                containedGraphics.Insert(
'                                        0,
'                                        New ModifyCTM(
'                                            1, 0, 0, 1,
'                                            objectXOffsets(index) + rowXOffset,
'                                            objectYOffset
'                                            )
'                                            )
'            Next

'            ' Update the actual block height!
'            _boundBox.Height = CSng(_currentRow.Y + _currentRow.Height)

'            ' Update the actual block vertical location!
'            Dim yOffset As Double
'            Select Case (_yAlignment)
'                Case YAlignmentEnum.Bottom
'                    yOffset = _frame.Height - _boundBox.Height
'                    'break;
'                Case YAlignmentEnum.Middle
'                    yOffset = (_frame.Height - _boundBox.Height) / 2
'                    'break;
'                Case YAlignmentEnum.Top
'                Case Else
'                    yOffset = 0
'                    'break
'            End Select
'            _boundBox.Y = CSng(_frame.Y + yOffset)

'            ' Discard the current row!
'            _currentRow = Nothing
'        End Sub

'        Private Function ResolveLineAlignment(ByVal lineAlignment As Object) As Object
'            If (Not (TypeOf (lineAlignment) Is LineAlignmentEnum OrElse TypeOf (lineAlignment) Is Length)) Then
'                Throw New ArgumentException("MUST be either LineAlignmentEnum or Length.", "lineAlignment")
'            End If


'            If (lineAlignment.Equals(LineAlignmentEnum.Super)) Then
'                lineAlignment = New Length(0.33, Length.UnitModeEnum.Relative)
'            ElseIf (lineAlignment.Equals(LineAlignmentEnum.Sub)) Then
'                lineAlignment = New Length(-0.33, Length.UnitModeEnum.Relative)
'            End If


'            If (TypeOf (lineAlignment) Is Length) Then
'                If (_lastFontSize = 0) Then
'                    _lastFontSize = _baseComposer.State.FontSize
'                End If
'                lineAlignment = CType(lineAlignment, Length).GetValue(_lastFontSize)
'            End If

'            Return lineAlignment
'        End Function

'#End Region
'#End Region
'#End Region
'    End Class

'End Namespace