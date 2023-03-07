Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.markedcontent
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject

Namespace org.apache.pdfbox.util

    '/**
    ' * This is an stream engine to extract the marked content of a pdf.
    ' * @author koch
    ' * @version $Revision$
    ' */
    Public Class PDFMarkedContentExtractor
        Inherits PDFStreamEngine

        Private suppressDuplicateOverlappingText As Boolean = True
        Private markedContents As List(Of PDMarkedContent) = New ArrayList(Of PDMarkedContent)()
        Private currentMarkedContents As Stack(Of PDMarkedContent) = New Stack(Of PDMarkedContent)()

        Private characterListMapping As Map(Of String, List(Of TextPosition)) = New HashMap(Of String, List(Of TextPosition))()

        ''' <summary>
        ''' encoding that text will be written in (or null).
        ''' </summary>
        ''' <remarks></remarks>
        Protected outputEncoding As String

        '/**
        ' * The normalizer is used to remove text ligatures/presentation forms
        ' * and to correct the direction of right to left text, such as Arabic and Hebrew.
        ' */
        Private normalize As TextNormalize = Nothing

        '/**
        ' * Instantiate a new PDFTextStripper object. This object will load
        ' * properties from PDFMarkedContentExtractor.properties and will not
        ' * do anything special to convert the text to a more encoding-specific
        ' * output.
        ' *
        ' * @throws IOException If there is an error loading the properties.
        ' */
        Public Sub New() 'throws IOException
            MyBase.New(ResourceLoader.loadProperties("org/apache/pdfbox/resources/PDFMarkedContentExtractor.properties", True))
            Me.outputEncoding = Nothing
            Me.normalize = New TextNormalize(Me.outputEncoding)
        End Sub


        '/**
        ' * Instantiate a new PDFTextStripper object.  Loading all of the operator mappings
        ' * from the properties object that is passed in.  Does not convert the text
        ' * to more encoding-specific output.
        ' *
        ' * @param props The properties containing the mapping of operators to PDFOperator
        ' * classes.
        ' *
        ' * @throws IOException If there is an error reading the properties.
        ' */
        Public Sub New(ByVal props As Properties) ' throws IOException
            MyBase.New(props)
            Me.outputEncoding = Nothing
            Me.normalize = New TextNormalize(Me.outputEncoding)
        End Sub

        '/**
        ' * Instantiate a new PDFTextStripper object. This object will load
        ' * properties from PDFMarkedContentExtractor.properties and will apply
        ' * encoding-specific conversions to the output text.
        ' *
        ' * @param encoding The encoding that the output will be written in.
        ' * @throws IOException If there is an error reading the properties.
        ' */
        Public Sub New(ByVal encoding As String)  'throws IOException
            MyBase.New(ResourceLoader.loadProperties("org/apache/pdfbox/resources/PDFMarkedContentExtractor.properties", True))
            Me.outputEncoding = encoding
            Me.normalize = New TextNormalize(Me.outputEncoding)
        End Sub


        '/**
        ' * This will determine of two floating point numbers are within a specified variance.
        ' *
        ' * @param first The first number to compare to.
        ' * @param second The second number to compare to.
        ' * @param variance The allowed variance.
        ' */
        Private Function within(ByVal first As Single, ByVal second As Single, ByVal variance As Single) As Boolean
            Return second > first - variance AndAlso second < first + variance
        End Function


        Public Sub beginMarkedContentSequence(ByVal tag As COSName, ByVal properties As COSDictionary)
            Dim markedContent As PDMarkedContent = PDMarkedContent.create(tag, properties)
            If (Me.currentMarkedContents.isEmpty()) Then
                Me.markedContents.add(markedContent)
            Else
                Dim currentMarkedContent As PDMarkedContent = Me.currentMarkedContents.peek()
                If (currentMarkedContent IsNot Nothing) Then
                    currentMarkedContent.addMarkedContent(markedContent)
                End If
            End If
            Me.currentMarkedContents.push(markedContent)
        End Sub

        Public Sub endMarkedContentSequence()
            If (Not Me.currentMarkedContents.isEmpty()) Then
                Me.currentMarkedContents.pop()
            End If
        End Sub

        Public Sub xobject(ByVal xobject As PDXObject)
            If (Not Me.currentMarkedContents.isEmpty()) Then
                Me.currentMarkedContents.peek().addXObject(xobject)
            End If
        End Sub


        '/**
        ' * This will process a TextPosition object and add the
        ' * text to the list of characters on a page.  It takes care of
        ' * overlapping text.
        ' *
        ' * @param text The text to process.
        ' */
        Protected Overrides Sub processTextPosition(ByVal text As TextPosition)
            Dim showCharacter As Boolean = True
            If (Me.suppressDuplicateOverlappingText) Then
                showCharacter = False
                Dim textCharacter As String = text.getCharacter()
                Dim textX As Single = text.getX()
                Dim textY As Single = text.getY()
                Dim sameTextCharacters As List(Of TextPosition) = Me.characterListMapping.get(textCharacter)
                If (sameTextCharacters Is Nothing) Then
                    sameTextCharacters = New ArrayList(Of TextPosition)()
                    Me.characterListMapping.put(textCharacter, sameTextCharacters)
                End If

                '// RDD - Here we compute the value that represents the end of the rendered
                '// text.  This value is used to determine whether subsequent text rendered
                '// on the same line overwrites the current text.
                '//
                '// We subtract any positive padding to handle cases where extreme amounts
                '// of padding are applied, then backed off (not sure why this is done, but there
                '// are cases where the padding is on the order of 10x the character width, and
                '// the TJ just backs up to compensate after each character).  Also, we subtract
                '// an amount to allow for kerning (a percentage of the width of the last
                '// character).
                '//
                Dim suppressCharacter As Boolean = False
                Dim tolerance As Single = (text.getWidth() / textCharacter.Length()) / 3.0F
                For i As Integer = 0 To sameTextCharacters.size() - 1
                    Dim character As TextPosition = sameTextCharacters.get(i)
                    Dim charCharacter As String = character.getCharacter()
                    Dim charX As Single = character.getX()
                    Dim charY As Single = character.getY()
                    'only want to suppress

                    '//charCharacter.equals( textCharacter ) 
                    If (charCharacter IsNot Nothing AndAlso within(charX, textX, tolerance) AndAlso within(charY, textY, tolerance)) Then
                        suppressCharacter = True
                        Exit For
                    End If
                Next
                If (Not suppressCharacter) Then
                    sameTextCharacters.add(text)
                    showCharacter = True
                End If
            End If

            If (showCharacter) Then
                Dim textList As List(Of TextPosition) = New ArrayList(Of TextPosition)()

                '/* In the wild, some PDF encoded documents put diacritics (accents on
                ' * top of characters) into a separate Tj element.  When displaying them
                ' * graphically, the two chunks get overlayed.  With text output though,
                ' * we need to do the overlay. This code recombines the diacritic with
                ' * its associated character if the two are consecutive.
                ' */ 
                If (textList.isEmpty()) Then
                    textList.add(text)
                Else
                    '/* test if we overlap the previous entry.  
                    ' * Note that we are making an assumption that we need to only look back
                    ' * one TextPosition to find what we are overlapping.  
                    ' * This may not always be true. */
                    Dim previousTextPosition As TextPosition = textList.get(textList.size() - 1)
                    If (text.isDiacritic() AndAlso previousTextPosition.contains(text)) Then
                        previousTextPosition.mergeDiacritic(text, Me.normalize)

                        '/* If the previous TextPosition was the diacritic, merge it into this
                        ' * one and remove it from the list. */
                    ElseIf (previousTextPosition.isDiacritic() AndAlso text.contains(previousTextPosition)) Then
                        text.mergeDiacritic(previousTextPosition, Me.normalize)
                        textList.remove(textList.size() - 1)
                        textList.add(text)
                    Else
                        textList.add(text)
                    End If
                End If
                If (Not Me.currentMarkedContents.isEmpty()) Then
                    Me.currentMarkedContents.peek().addText(text)
                End If
            End If
        End Sub


        Public Function getMarkedContents() As List(Of PDMarkedContent)
            Return Me.markedContents
        End Function

    End Class

End Namespace
