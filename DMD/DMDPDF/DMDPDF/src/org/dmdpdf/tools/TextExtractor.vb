'/*
'  Copyright 2009 - 2011 Stefano Chizzolini. http: //www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text

Namespace DMD.org.dmdpdf.tools

    '/**
    '  <summary> Tool For extracting text from <see cref="IContentContext">content contexts</see>.</summary>
    '*/
    Public NotInheritable Class TextExtractor

#Region "types"
        '/**
        '  <summary> Text-To-area matching mode.</summary>
        '*/
        Public Enum AreaModeEnum

            '  /**
            '  <summary> Text String must be contained by the area.</summary>
            '*/
            Containment
            '/**
            '  <summary> Text String must intersect the area.</summary>
            '*/
            Intersection
        End Enum

        '/**
        '  <summary> Text filter by interval.</summary>
        '  <remarks> Iterated intervals MUST be ordered.</remarks>
        '*/
        Public Interface IIntervalFilter
            Inherits IEnumerator(Of Interval(Of Integer))
            '/**
            '  <summary> Notifies current matching.</summary>
            '  <param name = "interval" > Current interval.</param>
            '  <param name = "match" > Text String matching the current interval.</param>
            '*/
            Sub Process(ByVal interval As Interval(Of Integer), ByVal match As ITextString)
        End Interface

        Private Class IntervalFilter
            Implements IIntervalFilter

            Private m_intervals As IList(Of Interval(Of Integer))

            Private m_textStrings As IList(Of ITextString) = New List(Of ITextString)
            Private m_index As Integer = 0

            Public Sub New(ByVal intervals As IList(Of Interval(Of Integer)))
                Me.m_intervals = intervals
            End Sub


            Public ReadOnly Property Current As Interval(Of Integer) Implements IEnumerator(Of Interval(Of Integer)).Current
                Get
                    Return Me.m_intervals(Me.m_index)
                End Get
            End Property

            Private ReadOnly Property Current_ As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Dispose() Implements IDisposable.Dispose
                ' NOOP 
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                Me.m_index += 1
                Return Me.m_index < m_intervals.Count
            End Function


            Public Sub Process(ByVal interval As Interval(Of Integer), ByVal match As ITextString) Implements IIntervalFilter.Process
                Me.m_textStrings.Add(match)
            End Sub


            Public Sub Reset() Implements IEnumerator.Reset
                Throw New NotSupportedException()
            End Sub

            Public ReadOnly Property TextStrings As IList(Of ITextString)
                Get
                    Return Me.m_textStrings
                End Get
            End Property
        End Class

        '/**
        '  <summary> Text String.</summary>
        '  <remarks> This Is typically used To assemble contiguous raw text strings
        '  laying on the same line.</remarks>
        '*/
        Private Class TextString
            Implements ITextString

            Private m_textChars As List(Of TextChar) = New List(Of TextChar)

            Public ReadOnly Property Box As RectangleF? Implements ITextString.Box
                Get
                    Dim box_ As RectangleF? = Nothing
                    For Each textChar As TextChar In Me.m_textChars
                        If (Not box_.HasValue) Then
                            box_ = CType(textChar.Box, RectangleF?)
                        Else
                            box_ = RectangleF.Union(box_.Value, textChar.Box)
                        End If
                    Next
                    Return box_
                End Get
            End Property

            Public ReadOnly Property Text As String Implements ITextString.Text
                Get
                    Dim textBuilder As StringBuilder = New StringBuilder()
                    For Each textChar As TextChar In Me.m_textChars
                        textBuilder.Append(textChar)
                    Next
                    Return textBuilder.ToString()
                End Get
            End Property

            Public ReadOnly Property TextChars As List(Of TextChar) Implements ITextString.TextChars
                Get
                    Return Me.m_textChars
                End Get
            End Property

        End Class


        '/**
        '  <summary> Text String position comparer.</summary>
        '*/
        Private Class TextStringPositionComparer(Of T As ITextString)
            Implements IComparer(Of T)

#Region "static"
            '/**
            '  <summary> Gets whether the specified boxes lay On the same text line.</summary>
            '*/
            Public Shared Function IsOnTheSameLine(ByVal box1 As RectangleF, ByVal box2 As RectangleF) As Boolean
                '/*
                '  NOTE: In order to consider the two boxes being on the same line,
                '  we apply a simple rule Of thumb: at least 25% Of a box's height MUST
                '  lay on the horizontal projection of the other one.
                '*/
                Dim minHeight As Double = Math.Min(box1.Height, box2.Height)
                Dim yThreshold As Double = minHeight * 0.75
                Return ((box1.Y > box2.Y - yThreshold AndAlso box1.Y < box2.Bottom + yThreshold - minHeight) OrElse
                        (box2.Y > box1.Y - yThreshold AndAlso box2.Y < box1.Bottom + yThreshold - minHeight))
            End Function

#End Region

#Region "dynamic"
#Region "IComparer"

            Public Function Compare(ByVal textString1 As T, ByVal textString2 As T) As Integer Implements IComparer(Of T).Compare
                Dim box1 As RectangleF = textString1.Box.Value
                Dim box2 As RectangleF = textString2.Box.Value
                If (IsOnTheSameLine(box1, box2)) Then
                    If (box1.X < box2.X) Then
                        Return -1
                    ElseIf (box1.X > box2.X) Then
                        Return 1
                    Else
                        Return 0
                    End If
                ElseIf (box1.Y < box2.Y) Then
                    Return -1
                Else
                    Return 1
                End If
            End Function

#End Region
#End Region
        End Class

#End Region

#Region "Static"
#Region "fields"

        Public Shared ReadOnly DefaultArea As RectangleF = New RectangleF(0, 0, 0, 0)
#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Converts text information into plain text.</summary>
        '  <param name = "textStrings" > Text information To convert.</param>
        '  <returns> Plain text.</returns>
        '*/
        Public Overloads Shared Function ToString(ByVal textStrings As IDictionary(Of RectangleF?, IList(Of ITextString))) As String
            Return ToString(textStrings, "", "")
        End Function

        '/**
        '  <summary> Converts text information into plain text.</summary>
        '  <param name = "textStrings" > Text information To convert.</param>
        '  <param name = "lineSeparator" > Separator To apply On line break.</param>
        '  <param name = "areaSeparator" > Separator To apply On area break.</param>
        '  <returns> Plain text.</returns>
        '*/
        Public Overloads Shared Function ToString(ByVal textStrings As IDictionary(Of RectangleF?, IList(Of ITextString)), ByVal lineSeparator As String, ByVal areaSeparator As String) As String
            Dim textBuilder As StringBuilder = New StringBuilder()
            For Each areaTextStrings As IList(Of ITextString) In textStrings.Values
                If (textBuilder.Length > 0) Then
                    textBuilder.Append(areaSeparator)
                End If

                For Each textString As ITextString In areaTextStrings
                    textBuilder.Append(textString.Text).Append(lineSeparator)
                Next
            Next
            Return textBuilder.ToString()
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private m_areaMode As AreaModeEnum = AreaModeEnum.Containment
        Private m_areas As List(Of RectangleF)
        Private m_areaTolerance As Single = 0
        Private m_dehyphenated As Boolean
        Private m_sorted As Boolean
#End Region

#Region "constructors"

        Public Sub New()
            Me.New(True, False)
        End Sub

        Public Sub New(ByVal sorted As Boolean, ByVal dehyphenated As Boolean)
            Me.New(Nothing, sorted, dehyphenated)
        End Sub

        Public Sub New(ByVal areas As IList(Of RectangleF), ByVal sorted As Boolean, ByVal dehyphenated As Boolean)
            Me.Areas = areas
            Me.Dehyphenated = dehyphenated
            Me.Sorted = sorted
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the text-To-area matching mode.</summary>
        '*/
        Public Property AreaMode As AreaModeEnum
            Get
                Return Me.m_areaMode
            End Get
            Set(ByVal value As AreaModeEnum)
                Me.m_areaMode = value
            End Set
        End Property

        '/**
        '  <summary> Gets the graphic areas whose text has To be extracted.</summary>
        '*/
        Public Property Areas As IList(Of RectangleF)
            Get
                Return Me.m_areas
            End Get
            Set(ByVal value As IList(Of RectangleF))
                If (value Is Nothing) Then
                    Me.m_areas = New List(Of RectangleF)
                Else
                    Me.m_areas = New List(Of RectangleF)(value)
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets the admitted outer area (In points) For containment matching purposes.</summary>
        '  <remarks> This measure Is useful To ensure that text whose boxes overlap With the area bounds
        '  Is Not excluded from the match.</remarks>
        '*/
        Public Property AreaTolerance As Single
            Get
                Return Me.m_areaTolerance
            End Get
            Set(ByVal value As Single)
                Me.m_areaTolerance = value
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets whether the text strings have To be dehyphenated.</summary>
        '*/
        Public Property Dehyphenated As Boolean
            Get
                Return Me.m_dehyphenated
            End Get
            Set(ByVal value As Boolean)
                Me.m_dehyphenated = value
                If (Me.m_dehyphenated) Then
                    Me.Sorted = True
                End If
            End Set
        End Property

        '/**
        '  <summary> Extracts text strings from the specified content context.</summary>
        '  <param name = "contentContext" > Source content context.</param>
        '*/
        Public Function Extract(ByVal contentContext As IContentContext) As IDictionary(Of RectangleF?, IList(Of ITextString))
            Dim extractedTextStrings As IDictionary(Of RectangleF?, IList(Of ITextString))
            Dim textStrings As List(Of ITextString) = New List(Of ITextString)()
            ' 1. Extract the source text strings!
            Dim rawTextStrings As List(Of ContentScanner.TextStringWrapper) = New List(Of ContentScanner.TextStringWrapper)()
            Extract(New ContentScanner(contentContext), rawTextStrings)

            ' 2. Sort the target text strings!
            If (Me.m_sorted) Then
                Sort(rawTextStrings, textStrings)
            Else
                For Each rawTextString As ContentScanner.TextStringWrapper In rawTextStrings
                    textStrings.Add(rawTextString)
                Next
            End If

            ' 3. Filter the target text strings!
            If (m_areas.Count = 0) Then
                extractedTextStrings = New Dictionary(Of RectangleF?, IList(Of ITextString))()
                extractedTextStrings(DefaultArea) = textStrings
            Else
                extractedTextStrings = Filter(textStrings, m_areas.ToArray())
            End If

            Return extractedTextStrings
        End Function

        '/**
        '  <summary> Extracts text strings from the specified contents.</summary>
        '                  <param name = "contents" > Source contents.</param>
        '*/
        Public Function Extract(ByVal contents As Contents) As IDictionary(Of RectangleF?, IList(Of ITextString))
            Return Extract(contents.ContentContext)
        End Function

        '/**
        '  <summary> Gets the text strings matching the specified intervals.</summary>
        '                  <param name = "textStrings" > Text strings To filter.</param>
        '                  <param name = "intervals" > Text intervals To match. They MUST be ordered And Not overlapping.</param>
        '                  <returns> A list Of text strings corresponding To the specified intervals.</returns>
        '*/
        Public Function Filter(ByVal textStrings As IDictionary(Of RectangleF?, IList(Of ITextString)), ByVal intervals As IList(Of Interval(Of Integer))) As IList(Of ITextString)
            Dim _Filter As IntervalFilter = New IntervalFilter(intervals)
            Filter(textStrings, _Filter)
            Return _Filter.TextStrings
        End Function

        '/**
        '  <summary> Processes the text strings matching the specified filter.</summary>
        '                      <param name = "textStrings" > Text strings To filter.</param>
        '                      <param name = "filter" > Matching processor.</param>
        '*/
        Public Sub Filter(ByVal textStrings As IDictionary(Of RectangleF?, IList(Of ITextString)), ByVal filter As IIntervalFilter)
            Dim textStringsIterator As IEnumerator(Of IList(Of ITextString)) = textStrings.Values.GetEnumerator()
            If (Not textStringsIterator.MoveNext()) Then
                Return
            End If

            Dim areaTextStringsIterator As IEnumerator(Of ITextString) = textStringsIterator.Current.GetEnumerator()
            If (Not areaTextStringsIterator.MoveNext()) Then
                Return
            End If
            Dim textChars As IList(Of TextChar) = areaTextStringsIterator.Current.TextChars
            Dim baseTextCharIndex As Integer = 0
            Dim textCharIndex As Integer = 0
            While (filter.MoveNext())
                Dim interval As Interval(Of Integer) = filter.Current
                Dim match As TextString = New TextString()
                Dim matchStartIndex As Integer = interval.Low
                Dim matchEndIndex As Integer = interval.High
                While (matchStartIndex > baseTextCharIndex + textChars.Count)
                    baseTextCharIndex += textChars.Count
                    If (Not areaTextStringsIterator.MoveNext()) Then
                        areaTextStringsIterator = textStringsIterator.Current.GetEnumerator()
                        areaTextStringsIterator.MoveNext()
                    End If
                    textChars = areaTextStringsIterator.Current.TextChars
                End While
                textCharIndex = matchStartIndex - baseTextCharIndex
                While (baseTextCharIndex + textCharIndex < matchEndIndex)
                    If (textCharIndex = textChars.Count) Then
                        baseTextCharIndex += textChars.Count
                        If (Not areaTextStringsIterator.MoveNext()) Then
                            areaTextStringsIterator = textStringsIterator.Current.GetEnumerator()
                            areaTextStringsIterator.MoveNext()
                        End If
                        textChars = areaTextStringsIterator.Current.TextChars
                        textCharIndex = 0
                    End If
                    match.TextChars.Add(textChars(textCharIndex)) : textCharIndex += 1
                End While

                filter.Process(interval, match)
            End While
        End Sub

        '/**
        '  <summary> Gets the text strings matching the specified area.</summary>
        '  <param name = "textStrings" > Text strings To filter, grouped by source area.</param>
        '  <param name = "area" > Graphic area which text strings have To be matched To.</param>
        '*/
        Public Function Filter(ByVal textStrings As IDictionary(Of RectangleF?, IList(Of ITextString)), ByVal area As RectangleF) As IList(Of ITextString)
            Return Filter(textStrings, New RectangleF() {area})(area)
        End Function

        '/**
        '  <summary> Gets the text strings matching the specified areas.</summary>
        '                 <param name = "textStrings" > Text strings To filter, grouped by source area.</param>
        '                 <param name = "areas" > Graphic areas which text strings have To be matched To.</param>
        '*/
        Public Function Filter(ByVal textStrings As IDictionary(Of RectangleF?, IList(Of ITextString)), ParamArray areas As RectangleF()) As IDictionary(Of RectangleF?, IList(Of ITextString))
            Dim filteredTextStrings As IDictionary(Of RectangleF?, IList(Of ITextString)) = Nothing
            For Each areaTextStrings As IList(Of ITextString) In textStrings.Values
                Dim filteredAreasTextStrings As IDictionary(Of RectangleF?, IList(Of ITextString)) = Filter(areaTextStrings, areas)
                If (filteredTextStrings Is Nothing) Then
                    filteredTextStrings = filteredAreasTextStrings
                Else
                    For Each filteredAreaTextStringsEntry As KeyValuePair(Of RectangleF?, IList(Of ITextString)) In filteredAreasTextStrings
                        Dim filteredTextStringsList As IList(Of ITextString) = filteredTextStrings(filteredAreaTextStringsEntry.Key)
                        For Each filteredAreaTextString As ITextString In filteredAreaTextStringsEntry.Value
                            filteredTextStringsList.Add(filteredAreaTextString)
                        Next
                    Next
                End If
            Next
            Return filteredTextStrings
        End Function


        '/**
        '  <summary> Gets the text strings matching the specified area.</summary>
        '                                     <param name = "textStrings" > Text strings To filter.</param>
        '                                     <param name = "area" > Graphic area which text strings have To be matched To.</param>
        '*/
        Public Function Filter(ByVal textStrings As IList(Of ITextString), ByVal area As RectangleF) As IList(Of ITextString)
            Return Filter(textStrings, New RectangleF() {area})(area)
        End Function

        '/**
        '  <summary> Gets the text strings matching the specified areas.</summary>
        '                                     <param name = "textStrings" > Text strings To filter.</param>
        '                                     <param name = "areas" > Graphic areas which text strings have To be matched To.</param>
        '*/
        Public Function Filter(ByVal textStrings As IList(Of ITextString), ParamArray areas As RectangleF()) As IDictionary(Of RectangleF?, IList(Of ITextString))
            Dim filteredAreasTextStrings As IDictionary(Of RectangleF?, IList(Of ITextString)) = New Dictionary(Of RectangleF?, IList(Of ITextString))()
            For Each area As RectangleF In areas
                Dim filteredAreaTextStrings As IList(Of ITextString) = New List(Of ITextString)()
                filteredAreasTextStrings(area) = filteredAreaTextStrings
                Dim toleratedArea As RectangleF
                If (Me.m_areaTolerance <> 0) Then
                    toleratedArea = New RectangleF(area.X - m_areaTolerance, area.Y - m_areaTolerance, area.Width + m_areaTolerance * 2, area.Height + m_areaTolerance * 2)
                Else
                    toleratedArea = area
                End If
                For Each textString As ITextString In textStrings
                    Dim textStringBox As RectangleF? = textString.Box
                    If (toleratedArea.IntersectsWith(textStringBox.Value)) Then
                        Dim filteredTextString As TextString = New TextString()
                        Dim filteredTextStringChars As List(Of TextChar) = filteredTextString.TextChars
                        For Each textChar As TextChar In textString.TextChars
                            Dim textCharBox As RectangleF = textChar.Box
                            If (
                                (Me.m_areaMode = AreaModeEnum.Containment AndAlso toleratedArea.Contains(textCharBox)) OrElse
                                (Me.m_areaMode = AreaModeEnum.Intersection AndAlso toleratedArea.IntersectsWith(textCharBox))
                                ) Then
                                filteredTextStringChars.Add(textChar)
                            End If
                        Next
                        filteredAreaTextStrings.Add(filteredTextString)
                    End If
                Next
            Next
            Return filteredAreasTextStrings
        End Function

        '/**
        '  <summary> Gets/Sets whether the text strings have To be sorted.</summary>
        '*/
        Public Property Sorted As Boolean
            Get
                Return Me.m_sorted
            End Get
            Set(ByVal value As Boolean)
                Me.m_sorted = value
                If (Not Me.m_sorted) Then
                    Me.Dehyphenated = False
                End If
            End Set
        End Property


#End Region

#Region "private"
        '/**
        '  <summary> Scans a content level looking For text.</summary>
        '*/
        Private Sub Extract(ByVal level As ContentScanner, ByVal extractedTextStrings As IList(Of ContentScanner.TextStringWrapper))
            If (level Is Nothing) Then Return

            While (level.MoveNext())
                Dim content As ContentObject = level.Current
                If (TypeOf (content) Is Text) Then
                    ' Collect the text strings!
                    For Each textString As ContentScanner.TextStringWrapper In CType(level.CurrentWrapper, ContentScanner.TextWrapper).TextStrings
                        extractedTextStrings.Add(textString)
                    Next
                ElseIf (TypeOf (content) Is XObject) Then
                    ' Scan the external level!
                    Extract(CType(content, XObject).GetScanner(level), extractedTextStrings)
                ElseIf (TypeOf (content) Is ContainerObject) Then
                    ' Scan the inner level!
                    Extract(level.ChildLevel, extractedTextStrings)
                End If
            End While
        End Sub

        '/**
        '  <summary> Sorts the extracted text strings.</summary>
        '                <remarks> Sorting implies text position ordering, integration And aggregation.</remarks>
        '                <param name="rawTextStrings"> Source(lower - level) text strings.</param>
        '                <param name="textStrings"> Target(higher - level) text strings.</param>
        '*/
        Private Sub Sort(ByVal rawTextStrings As List(Of ContentScanner.TextStringWrapper), ByVal textStrings As List(Of ITextString))
            ' Sorting the source text strings...
            Dim positionComparator As TextStringPositionComparer(Of ContentScanner.TextStringWrapper) = New TextStringPositionComparer(Of ContentScanner.TextStringWrapper)()
            rawTextStrings.Sort(positionComparator)

            ' Aggregating And integrating the source text strings into the target ones...
            Dim textString As TextString = Nothing
            Dim textStyle As TextStyle = Nothing
            Dim previousTextChar As TextChar = Nothing
            Dim dehyphenating As Boolean = False
            For Each rawTextString As ContentScanner.TextStringWrapper In rawTextStrings
                '/*
                '  NOTE: Contents on the same line are grouped together within the same text string.
                '*/
                ' Add a New text string in case of New line!
                If (
                    textString IsNot Nothing AndAlso
                    textString.TextChars.Count > 0 AndAlso
                    Not TextStringPositionComparer(Of ITextString).IsOnTheSameLine(textString.Box.Value, rawTextString.Box.Value)
                ) Then
                    If (Me.m_dehyphenated AndAlso previousTextChar.Value = "-"c) Then ' Hyphened word.
                        textString.TextChars.Remove(previousTextChar)
                        dehyphenating = True
                    Else ' Full word.
                        ' Add synthesized space character!
                        textString.TextChars.Add(New TextChar(" "c, New RectangleF(previousTextChar.Box.Right, previousTextChar.Box.Top, 0, previousTextChar.Box.Height), textStyle, True))
                        textString = Nothing
                        dehyphenating = False
                    End If
                    previousTextChar = Nothing
                End If

                If (textString Is Nothing) Then
                    textString = New TextString()
                    textStrings.Add(textString)
                End If

                textStyle = rawTextString.Style
                Dim spaceWidth As Double = textStyle.Font.GetWidth(" "c, textStyle.FontSize)
                If (spaceWidth = 0) Then '// NOTE: as a rule of thumb, space width Is estimated according to the font size.
                    spaceWidth = textStyle.FontSize * 0.25F
                End If
                For Each textChar As TextChar In rawTextString.TextChars
                    If (previousTextChar IsNot Nothing) Then
                        '/*
                        '  NOTE: PDF files may have text contents omitting space characters,
                        '  so they must be inferred And synthesized, marking them As virtual
                        '  in order to allow the user to distinguish between original contents
                        '  And augmented ones.
                        '*/
                        Dim characterSpace As Single = TextChar.Box.X - previousTextChar.Box.Right
                        If (characterSpace >= spaceWidth) Then
                            ' Add synthesized space character!
                            previousTextChar = New TextChar(" "c, New RectangleF(previousTextChar.Box.Right, textChar.Box.Y, characterSpace, textChar.Box.Height), textStyle, True)
                            textString.TextChars.Add(previousTextChar)
                        End If
                        If (dehyphenating AndAlso previousTextChar.Value = " "c) Then
                            textString = New TextString()
                            textStrings.Add(textString)
                            dehyphenating = False
                        End If
                    End If
                    previousTextChar = textChar
                    textString.TextChars.Add(previousTextChar)
                Next
            Next
        End Sub

#End Region
#End Region
#End Region

    End Class


End Namespace

