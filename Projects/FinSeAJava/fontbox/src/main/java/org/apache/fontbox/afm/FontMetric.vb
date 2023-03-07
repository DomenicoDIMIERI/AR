Imports System.IO
Imports FinSeA.org.apache.fontbox.util

Namespace org.apache.fontbox.afm

    '/**
    ' * This is the outermost AFM type.  This can be created by the afmparser with a valid
    ' * AFM document.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.3 $
    ' */
    Public Class FontMetric

        ''' <summary>
        ''' This is the version of the FontMetrics.
        ''' </summary>
        ''' <remarks></remarks>
        Private afmVersion As Single
        Private metricSets As Integer = 0
        Private fontName As String
        Private fullName As String
        Private familyName As String
        Private weight As String
        Private fontBBox As BoundingBox
        Private fontVersion As String
        Private notice As String
        Private encodingScheme As String
        Private mappingScheme As Integer
        Private escChar As Integer
        Private characterSet As String
        Private characters As Integer
        Private _isBaseFont As Boolean
        Private vVector() As Single
        Private _isFixedV As Boolean
        Private capHeight As Single
        Private xHeight As Single
        Private ascender As Single
        Private descender As Single
        Private comments As List(Of String) = New ArrayList(Of String)()

        Private underlinePosition As Single
        Private underlineThickness As Single
        Private italicAngle As Single
        Private charWidth As Single()
        Private _isFixedPitch As Boolean
        Private standardHorizontalWidth As Single
        Private standardVerticalWidth As Single

        Private charMetrics As List(Of CharMetric) = New ArrayList(Of CharMetric)()
        Private charMetricsMap As Map(Of String, CharMetric) = New HashMap(Of String, CharMetric)()
        Private trackKern As List(Of TrackKern) = New ArrayList(Of TrackKern)()
        Private composites As List(Of Composite) = New ArrayList(Of Composite)()
        Private kernPairs As List(Of KernPair) = New ArrayList(Of KernPair)()
        Private kernPairs0 As List(Of KernPair) = New ArrayList(Of KernPair)()
        Private kernPairs1 As List(Of KernPair) = New ArrayList(Of KernPair)()

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * This will get the width of a character.
        ' *
        ' * @param name The character to get the width for.
        ' *
        ' * @return The width of the character.
        ' *
        ' * @throws IOException If Me AFM file does not handle the character.
        ' */
        Public Function getCharacterWidth(ByVal name As String) As Single 'throws IOException
            Dim result As Single = 0
            Dim metric As CharMetric = charMetricsMap.get(name)
            If (metric Is Nothing) Then
                result = 0
                'don't throw an exception right away.
                'throw new IOException( "Unknown AFM(" &  getFullName()  & ") characer '" &  name  & "'" );
            Else
                result = metric.getWx()
            End If
            Return result
        End Function

        '/**
        ' * This will get the width of a character.
        ' *
        ' * @param name The character to get the width for.
        ' *
        ' * @return The width of the character.
        ' *
        ' * @throws IOException If Me AFM file does not handle the character.
        ' */
        Public Function getCharacterHeight(ByVal name As String) As Single ' throws IOException
            Dim result As Single = 0
            Dim metric As CharMetric = charMetricsMap.get(name)
            If (metric Is Nothing) Then
                result = 0
                'don't throw an exception right away.
                'throw new IOException( "Unknown AFM(" &  getFullName()  & ") characer '" &  name  & "'" );
            Else
                If (metric.getWy() = 0) Then
                    result = metric.getBoundingBox().getHeight()
                Else
                    result = metric.getWy()
                End If
            End If
            Return result
        End Function


        '/**
        ' * This will get the average width of a character.
        ' *
        ' * @return The width of the character.
        ' *
        ' * @throws IOException If Me AFM file does not handle the character.
        ' */
        Public Function getAverageCharacterWidth() As Single 'throws IOException
            Dim average As Single = 0
            Dim totalWidths As Single = 0
            Dim characterCount As Single = 0
            Dim iter As Iterator(Of CharMetric) = charMetricsMap.Values().iterator()
            While (iter.hasNext())
                Dim metric As CharMetric = iter.next()
                If (metric.getWx() > 0) Then
                    totalWidths += metric.getWx()
                    characterCount += 1
                End If
            End While
            If (characterCount > 0) Then
                average = totalWidths / characterCount
            End If

            Return average
        End Function

        '/**
        ' * This will add a new comment.
        ' *
        ' * @param comment The comment to add to Me metric.
        ' */
        Public Sub addComment(ByVal comment As String)
            comments.add(comment)
        End Sub

        '/**
        ' * This will get all comments.
        ' *
        ' * @return The list of all comments.
        ' */
        Public Function getComments() As List(Of String)
            Return comments
        End Function

        '/**
        ' * This will get the version of the AFM document.
        ' *
        ' * @return The version of the document.
        ' */
        Public Function getAFMVersion() As Single
            Return afmVersion
        End Function

        '/**
        ' * This will get the metricSets attribute.
        ' *
        ' * @return The value of the metric sets.
        ' */
        Public Function getMetricSets() As Integer
            Return metricSets
        End Function

        '/**
        ' * This will set the version of the AFM document.
        ' *
        ' * @param afmVersionValue The version of the document.
        ' */
        Public Sub setAFMVersion(ByVal afmVersionValue As Single)
            afmVersion = afmVersionValue
        End Sub

        '/**
        ' * This will set the metricSets attribute.  This value must be 0,1, or 2.
        ' *
        ' * @param metricSetsValue The new metric sets attribute.
        ' */
        Public Sub setMetricSets(ByVal metricSetsValue As Integer)
            If (metricSetsValue < 0 OrElse metricSetsValue > 2) Then
                Throw New RuntimeException("The metricSets attribute must be in the set {0,1,2} and not '" & metricSetsValue & "'")
            End If
            metricSets = metricSetsValue
        End Sub

        '/**
        ' * Getter for property fontName.
        ' *
        ' * @return Value of property fontName.
        ' */
        Public Function getFontName() As String
            Return fontName
        End Function

        '/**
        ' * Setter for property fontName.
        ' *
        ' * @param name New value of property fontName.
        ' */
        Public Sub setFontName(ByVal name As String)
            fontName = name
        End Sub

        '/**
        ' * Getter for property fullName.
        ' *
        ' * @return Value of property fullName.
        ' */
        Public Function getFullName() As String
            Return fullName
        End Function

        '/**
        ' * Setter for property fullName.
        ' *
        ' * @param fullNameValue New value of property fullName.
        ' */
        Public Sub setFullName(ByVal fullNameValue As String)
            fullName = fullNameValue
        End Sub

        '/**
        ' * Getter for property familyName.
        ' *
        ' * @return Value of property familyName.
        ' */
        Public Function getFamilyName() As String
            Return familyName
        End Function

        '/**
        ' * Setter for property familyName.
        ' *
        ' * @param familyNameValue New value of property familyName.
        ' */
        Public Sub setFamilyName(ByVal familyNameValue As String)
            familyName = familyNameValue
        End Sub

        '/**
        ' * Getter for property weight.
        ' *
        ' * @return Value of property weight.
        ' */
        Public Function getWeight() As String
            Return weight
        End Function

        '/**
        ' * Setter for property weight.
        ' *
        ' * @param weightValue New value of property weight.
        ' */
        Public Sub setWeight(ByVal weightValue As String)
            weight = weightValue
        End Sub

        '/**
        ' * Getter for property fontBBox.
        ' *
        ' * @return Value of property fontBBox.
        ' */
        Public Function getFontBBox() As BoundingBox
            Return fontBBox
        End Function

        '/**
        ' * Setter for property fontBBox.
        ' *
        ' * @param bBox New value of property fontBBox.
        ' */
        Public Sub setFontBBox(ByVal bBox As BoundingBox)
            Me.fontBBox = bBox
        End Sub

        '/**
        ' * Getter for property notice.
        ' *
        ' * @return Value of property notice.
        ' */
        Public Function getNotice() As String
            Return notice
        End Function

        '/**
        ' * Setter for property notice.
        ' *
        ' * @param noticeValue New value of property notice.
        ' */
        Public Sub setNotice(ByVal noticeValue As String)
            notice = noticeValue
        End Sub

        '/**
        ' * Getter for property encodingScheme.
        ' *
        ' * @return Value of property encodingScheme.
        ' */
        Public Function getEncodingScheme() As String
            Return encodingScheme
        End Function

        '/**
        ' * Setter for property encodingScheme.
        ' *
        ' * @param encodingSchemeValue New value of property encodingScheme.
        ' */
        Public Sub setEncodingScheme(ByVal encodingSchemeValue As String)
            encodingScheme = encodingSchemeValue
        End Sub

        '/**
        ' * Getter for property mappingScheme.
        ' *
        ' * @return Value of property mappingScheme.
        ' */
        Public Function getMappingScheme() As Integer
            Return mappingScheme
        End Function

        '/**
        ' * Setter for property mappingScheme.
        ' *
        ' * @param mappingSchemeValue New value of property mappingScheme.
        ' */
        Public Sub setMappingScheme(ByVal mappingSchemeValue As Integer)
            mappingScheme = mappingSchemeValue
        End Sub

        '/**
        ' * Getter for property escChar.
        ' *
        ' * @return Value of property escChar.
        ' */
        Public Function getEscChar() As Integer
            Return escChar
        End Function

        '/**
        ' * Setter for property escChar.
        ' *
        ' * @param escCharValue New value of property escChar.
        ' */
        Public Sub setEscChar(ByVal escCharValue As Integer)
            escChar = escCharValue
        End Sub

        '/**
        ' * Getter for property characterSet.
        ' *
        ' * @return Value of property characterSet.
        ' */
        Public Function getCharacterSet() As String
            Return characterSet
        End Function

        '/**
        ' * Setter for property characterSet.
        ' *
        ' * @param characterSetValue New value of property characterSet.
        ' */
        Public Sub setCharacterSet(ByVal characterSetValue As String)
            characterSet = characterSetValue
        End Sub

        '/**
        ' * Getter for property characters.
        ' *
        ' * @return Value of property characters.
        ' */
        Public Function getCharacters() As Integer
            Return characters
        End Function

        '/**
        ' * Setter for property characters.
        ' *
        ' * @param charactersValue New value of property characters.
        ' */
        Public Sub setCharacters(ByVal charactersValue As Integer)
            characters = charactersValue
        End Sub

        '/**
        ' * Getter for property _isBaseFont.
        ' *
        ' * @return Value of property _isBaseFont.
        ' */
        Public Function isBaseFont() As Boolean
            Return _isBaseFont
        End Function

        '/**
        ' * Setter for property _isBaseFont.
        ' *
        ' * @param isBaseFontValue New value of property _isBaseFont.
        ' */
        Public Sub setIsBaseFont(ByVal value As Boolean)
            _isBaseFont = value
        End Sub

        '/**
        ' * Getter for property vVector.
        ' *
        ' * @return Value of property vVector.
        ' */
        Public Function getVVector() As Single()
            Return Me.vVector
        End Function

        '/**
        ' * Setter for property vVector.
        ' *
        ' * @param vVectorValue New value of property vVector.
        ' */
        Public Sub setVVector(ByVal vVectorValue As Single())
            vVector = vVectorValue
        End Sub

        '/**
        ' * Getter for property _isFixedV.
        ' *
        ' * @return Value of property _isFixedV.
        ' */
        Public Function isFixedV() As Boolean
            Return _isFixedV
        End Function

        '/**
        ' * Setter for property _isFixedV.
        ' *
        ' * @param isFixedVValue New value of property _isFixedV.
        ' */
        Public Sub setIsFixedV(ByVal isFixedVValue As Boolean)
            _isFixedV = isFixedVValue
        End Sub

        '/**
        ' * Getter for property capHeight.
        ' *
        ' * @return Value of property capHeight.
        ' */
        Public Function getCapHeight() As Single
            Return capHeight
        End Function

        '/**
        ' * Setter for property capHeight.
        ' *
        ' * @param capHeightValue New value of property capHeight.
        ' */
        Public Sub setCapHeight(ByVal capHeightValue As Single)
            capHeight = capHeightValue
        End Sub

        '/**
        ' * Getter for property xHeight.
        ' *
        ' * @return Value of property xHeight.
        ' */
        Public Function getXHeight() As Single
            Return xHeight
        End Function

        '/**
        ' * Setter for property xHeight.
        ' *
        ' * @param xHeightValue New value of property xHeight.
        ' */
        Public Sub setXHeight(ByVal xHeightValue As Single)
            xHeight = xHeightValue
        End Sub

        '/**
        ' * Getter for property ascender.
        ' *
        ' * @return Value of property ascender.
        ' */
        Public Function getAscender() As Single
            Return ascender
        End Function

        '/**
        ' * Setter for property ascender.
        ' *
        ' * @param ascenderValue New value of property ascender.
        ' */
        Public Sub setAscender(ByVal ascenderValue As Single)
            ascender = ascenderValue
        End Sub

        '/**
        ' * Getter for property descender.
        ' *
        ' * @return Value of property descender.
        ' */
        Public Function getDescender() As Single
            Return descender
        End Function

        '/**
        ' * Setter for property descender.
        ' *
        ' * @param descenderValue New value of property descender.
        ' */
        Public Sub setDescender(ByVal descenderValue As Single)
            descender = descenderValue
        End Sub

        '/**
        ' * Getter for property fontVersion.
        ' *
        ' * @return Value of property fontVersion.
        ' */
        Public Function getFontVersion() As String
            Return fontVersion
        End Function

        '/**
        ' * Setter for property fontVersion.
        ' *
        ' * @param fontVersionValue New value of property fontVersion.
        ' */
        Public Sub setFontVersion(ByVal fontVersionValue As String)
            fontVersion = fontVersionValue
        End Sub

        '/**
        ' * Getter for property underlinePosition.
        ' *
        ' * @return Value of property underlinePosition.
        ' */
        Public Function getUnderlinePosition() As Single
            Return underlinePosition
        End Function

        '/**
        ' * Setter for property underlinePosition.
        ' *
        ' * @param underlinePositionValue New value of property underlinePosition.
        ' */
        Public Sub setUnderlinePosition(ByVal underlinePositionValue As Single)
            underlinePosition = underlinePositionValue
        End Sub

        '/**
        ' * Getter for property underlineThickness.
        ' *
        ' * @return Value of property underlineThickness.
        ' */
        Public Function getUnderlineThickness() As Single
            Return underlineThickness
        End Function

        '/**
        ' * Setter for property underlineThickness.
        ' *
        ' * @param underlineThicknessValue New value of property underlineThickness.
        ' */
        Public Sub setUnderlineThickness(ByVal underlineThicknessValue As Single)
            underlineThickness = underlineThicknessValue
        End Sub

        '/**
        ' * Getter for property italicAngle.
        ' *
        ' * @return Value of property italicAngle.
        ' */
        Public Function getItalicAngle() As Single
            Return italicAngle
        End Function

        '/**
        ' * Setter for property italicAngle.
        ' *
        ' * @param italicAngleValue New value of property italicAngle.
        ' */
        Public Sub setItalicAngle(ByVal italicAngleValue As Single)
            italicAngle = italicAngleValue
        End Sub

        '/**
        ' * Getter for property charWidth.
        ' *
        ' * @return Value of property charWidth.
        ' */
        Public Function getCharWidth() As Single()
            Return Me.charWidth
        End Function

        '/**
        ' * Setter for property charWidth.
        ' *
        ' * @param charWidthValue New value of property charWidth.
        ' */
        Public Sub setCharWidth(ByVal charWidthValue() As Single)
            charWidth = charWidthValue
        End Sub

        '/**
        ' * Getter for property _isFixedPitch.
        ' *
        ' * @return Value of property _isFixedPitch.
        ' */
        Public Function isFixedPitch() As Boolean
            Return _isFixedPitch
        End Function

        '/**
        ' * Setter for property _isFixedPitch.
        ' *
        ' * @param isFixedPitchValue New value of property _isFixedPitch.
        ' */
        Public Sub setFixedPitch(ByVal isFixedPitchValue As Boolean)
            _isFixedPitch = isFixedPitchValue
        End Sub

        '/** Getter for property charMetrics.
        ' * @return Value of property charMetrics.
        ' */
        Public Function getCharMetrics() As List(Of CharMetric)
            Return charMetrics
        End Function

        '/** Setter for property charMetrics.
        ' * @param charMetricsValue New value of property charMetrics.
        ' */
        Public Sub setCharMetrics(ByVal charMetricsValue As List(Of CharMetric))
            charMetrics = charMetricsValue
        End Sub

        '/**
        ' * This will add another character metric.
        ' *
        ' * @param metric The character metric to add.
        ' */
        Public Sub addCharMetric(ByVal metric As CharMetric)
            charMetrics.add(metric)
            charMetricsMap.put(metric.getName(), metric)
        End Sub

        '/** Getter for property trackKern.
        ' * @return Value of property trackKern.
        ' */
        Public Function getTrackKern() As List(Of TrackKern)
            Return trackKern
        End Function

        '/** Setter for property trackKern.
        ' * @param trackKernValue New value of property trackKern.
        ' */
        Public Sub setTrackKern(ByVal trackKernValue As List(Of TrackKern))
            trackKern = trackKernValue
        End Sub

        '/**
        ' * This will add another track kern.
        ' *
        ' * @param kern The track kerning data.
        ' */
        Public Sub addTrackKern(ByVal kern As TrackKern)
            trackKern.add(kern)
        End Sub

        '/** Getter for property composites.
        ' * @return Value of property composites.
        ' */
        Public Function getComposites() As List(Of Composite)
            Return composites
        End Function

        '/** Setter for property composites.
        ' * @param compositesList New value of property composites.
        ' */
        Public Sub setComposites(ByVal compositesList As List(Of Composite))
            composites = compositesList
        End Sub

        '/**
        ' * This will add a single composite part to the picture.
        ' *
        ' * @param composite The composite info to add.
        ' */
        Public Sub addComposite(ByVal composite As Composite)
            composites.add(composite)
        End Sub

        '/** Getter for property kernPairs.
        ' * @return Value of property kernPairs.
        ' */
        Public Function getKernPairs() As List(Of KernPair)
            Return kernPairs
        End Function

        '/**
        ' * This will add a kern pair.
        ' *
        ' * @param kernPair The kern pair to add.
        ' */
        Public Sub addKernPair(ByVal kernPair As KernPair)
            kernPairs.add(kernPair)
        End Sub

        '/** Setter for property kernPairs.
        ' * @param kernPairsList New value of property kernPairs.
        ' */
        Public Sub setKernPairs(ByVal kernPairsList As List(Of KernPair))
            kernPairs = kernPairsList
        End Sub

        '/** Getter for property kernPairs0.
        ' * @return Value of property kernPairs0.
        ' */
        Public Function getKernPairs0() As List(Of KernPair)
            Return kernPairs0
        End Function

        '/**
        ' * This will add a kern pair.
        ' *
        ' * @param kernPair The kern pair to add.
        ' */
        Public Sub addKernPair0(ByVal kernPair As KernPair)
            kernPairs0.add(kernPair)
        End Sub

        '/** Setter for property kernPairs0.
        ' * @param kernPairs0List New value of property kernPairs0.
        ' */
        Public Sub setKernPairs0(ByVal kernPairs0List As List(Of KernPair))
            kernPairs0 = kernPairs0List
        End Sub

        '/** Getter for property kernPairs1.
        ' * @return Value of property kernPairs1.
        ' */
        Public Function getKernPairs1() As List(Of KernPair)
            Return kernPairs1
        End Function

        '/**
        ' * This will add a kern pair.
        ' *
        ' * @param kernPair The kern pair to add.
        ' */
        Public Sub addKernPair1(ByVal kernPair As KernPair)
            kernPairs1.add(kernPair)
        End Sub

        '/** Setter for property kernPairs1.
        ' * @param kernPairs1List New value of property kernPairs1.
        ' */
        Public Sub setKernPairs1(ByVal kernPairs1List As List(Of KernPair))
            kernPairs1 = kernPairs1List
        End Sub

        '/** Getter for property standardHorizontalWidth.
        ' * @return Value of property standardHorizontalWidth.
        ' */
        Public Function getStandardHorizontalWidth() As Single
            Return standardHorizontalWidth
        End Function

        '/** Setter for property standardHorizontalWidth.
        ' * @param standardHorizontalWidthValue New value of property standardHorizontalWidth.
        ' */
        Public Sub setStandardHorizontalWidth(ByVal standardHorizontalWidthValue As Single)
            standardHorizontalWidth = standardHorizontalWidthValue
        End Sub

        '/** Getter for property standardVerticalWidth.
        ' * @return Value of property standardVerticalWidth.
        ' */
        Public Function getStandardVerticalWidth() As Single
            Return standardVerticalWidth
        End Function

        '/** Setter for property standardVerticalWidth.
        ' * @param standardVerticalWidthValue New value of property standardVerticalWidth.
        ' */
        Public Sub setStandardVerticalWidth(ByVal standardVerticalWidthValue As Single)
            standardVerticalWidth = standardVerticalWidthValue
        End Sub

    End Class

End Namespace