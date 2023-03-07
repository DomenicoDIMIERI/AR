Namespace org.fontbox.ttf


    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class HorizontalHeaderTable
        Inherits TTFTable

        ''' <summary>
        ''' A tag that identifies Me table type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "hhea"

        Private version As Single
        Private ascender As Short
        Private descender As Short
        Private lineGap As Short
        Private advanceWidthMax As Integer
        Private minLeftSideBearing As Short
        Private minRightSideBearing As Short
        Private xMaxExtent As Short
        Private caretSlopeRise As Short
        Private caretSlopeRun As Short
        Private reserved1 As Short
        Private reserved2 As Short
        Private reserved3 As Short
        Private reserved4 As Short
        Private reserved5 As Short
        Private metricDataFormat As Short
        Private numberOfHMetrics As Integer

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            version = data.read32Fixed()
            ascender = data.readSignedShort()
            descender = data.readSignedShort()
            lineGap = data.readSignedShort()
            advanceWidthMax = data.readUnsignedShort()
            minLeftSideBearing = data.readSignedShort()
            minRightSideBearing = data.readSignedShort()
            xMaxExtent = data.readSignedShort()
            caretSlopeRise = data.readSignedShort()
            caretSlopeRun = data.readSignedShort()
            reserved1 = data.readSignedShort()
            reserved2 = data.readSignedShort()
            reserved3 = data.readSignedShort()
            reserved4 = data.readSignedShort()
            reserved5 = data.readSignedShort()
            metricDataFormat = data.readSignedShort()
            numberOfHMetrics = data.readUnsignedShort()
        End Sub

        '/**
        ' * @return Returns the advanceWidthMax.
        ' */
        Public Function getAdvanceWidthMax() As Integer
            Return advanceWidthMax
        End Function

        '/**
        ' * @param advanceWidthMaxValue The advanceWidthMax to set.
        ' */
        Public Sub setAdvanceWidthMax(ByVal advanceWidthMaxValue As Integer)
            Me.advanceWidthMax = advanceWidthMaxValue
        End Sub

        '/**
        ' * @return Returns the ascender.
        ' */
        Public Function getAscender() As Short
            Return ascender
        End Function

        '/**
        ' * @param ascenderValue The ascender to set.
        ' */
        Public Sub setAscender(ByVal ascenderValue As Short)
            Me.ascender = ascenderValue
        End Sub

        '/**
        ' * @return Returns the caretSlopeRise.
        ' */
        Public Function getCaretSlopeRise() As Short
            Return caretSlopeRise
        End Function

        '/**
        ' * @param caretSlopeRiseValue The caretSlopeRise to set.
        ' */
        Public Sub setCaretSlopeRise(ByVal caretSlopeRiseValue As Short)
            Me.caretSlopeRise = caretSlopeRiseValue
        End Sub

        '/**
        ' * @return Returns the caretSlopeRun.
        ' */
        Public Function getCaretSlopeRun() As Short
            Return caretSlopeRun
        End Function

        '/**
        ' * @param caretSlopeRunValue The caretSlopeRun to set.
        ' */
        Public Sub setCaretSlopeRun(ByVal caretSlopeRunValue As Short)
            Me.caretSlopeRun = caretSlopeRunValue
        End Sub

        '/**
        ' * @return Returns the descender.
        ' */
        Public Function getDescender() As Short
            Return descender
        End Function

        '/**
        ' * @param descenderValue The descender to set.
        ' */
        Public Sub setDescender(ByVal descenderValue As Short)
            Me.descender = descenderValue
        End Sub

        '/**
        ' * @return Returns the lineGap.
        ' */
        Public Function getLineGap() As Short
            Return lineGap
        End Function

        '/**
        ' * @param lineGapValue The lineGap to set.
        ' */
        Public Sub setLineGap(ByVal lineGapValue As Short)
            Me.lineGap = lineGapValue
        End Sub

        '/**
        ' * @return Returns the metricDataFormat.
        ' */
        Public Function getMetricDataFormat() As Short
            Return metricDataFormat
        End Function

        '/**
        ' * @param metricDataFormatValue The metricDataFormat to set.
        ' */
        Public Sub setMetricDataFormat(ByVal metricDataFormatValue As Short)
            Me.metricDataFormat = metricDataFormatValue
        End Sub

        '/**
        ' * @return Returns the minLeftSideBearing.
        ' */
        Public Function getMinLeftSideBearing() As Short
            Return minLeftSideBearing
        End Function

        '/**
        ' * @param minLeftSideBearingValue The minLeftSideBearing to set.
        ' */
        Public Sub setMinLeftSideBearing(ByVal minLeftSideBearingValue As Short)
            Me.minLeftSideBearing = minLeftSideBearingValue
        End Sub

        '/**
        ' * @return Returns the minRightSideBearing.
        ' */
        Public Function getMinRightSideBearing() As Short
            Return minRightSideBearing
        End Function

        '/**
        ' * @param minRightSideBearingValue The minRightSideBearing to set.
        ' */
        Public Sub setMinRightSideBearing(ByVal minRightSideBearingValue As Short)
            Me.minRightSideBearing = minRightSideBearingValue
        End Sub

        '/**
        ' * @return Returns the numberOfHMetrics.
        ' */
        Public Function getNumberOfHMetrics() As Integer
            Return numberOfHMetrics
        End Function

        '/**
        ' * @param numberOfHMetricsValue The numberOfHMetrics to set.
        ' */
        Public Sub setNumberOfHMetrics(ByVal numberOfHMetricsValue As Integer)
            Me.numberOfHMetrics = numberOfHMetricsValue
        End Sub

        '/**
        ' * @return Returns the reserved1.
        ' */
        Public Function getReserved1() As Short
            Return reserved1
        End Function

        '/**
        ' * @param reserved1Value The reserved1 to set.
        ' */
        Public Sub setReserved1(ByVal reserved1Value As Short)
            Me.reserved1 = reserved1Value
        End Sub

        '/**
        ' * @return Returns the reserved2.
        ' */
        Public Function getReserved2() As Short
            Return reserved2
        End Function

        '/**
        ' * @param reserved2Value The reserved2 to set.
        ' */
        Public Sub setReserved2(ByVal reserved2Value As Short)
            Me.reserved2 = reserved2Value
        End Sub

        '/**
        ' * @return Returns the reserved3.
        ' */
        Public Function getReserved3() As Short
            Return reserved3
        End Function

        '/**
        '    * @param reserved3Value The reserved3 to set.
        '    */
        Public Sub setReserved3(ByVal reserved3Value As Short)
            Me.reserved3 = reserved3Value
        End Sub

        '/**
        ' * @return Returns the reserved4.
        ' */
        Public Function getReserved4() As Short
            Return reserved4
        End Function

        '/**
        ' * @param reserved4Value The reserved4 to set.
        ' */
        Public Sub setReserved4(ByVal reserved4Value As Short)
            Me.reserved4 = reserved4Value
        End Sub

        '/**
        ' * @return Returns the reserved5.
        ' */
        Public Function getReserved5() As Short
            Return reserved5
        End Function

        '/**
        ' * @param reserved5Value The reserved5 to set.
        ' */
        Public Sub setReserved5(ByVal reserved5Value As Short)
            Me.reserved5 = reserved5Value
        End Sub

        '/**
        ' * @return Returns the version.
        ' */
        Public Function getVersion() As Single
            Return version
        End Function

        '/**
        ' * @param versionValue The version to set.
        ' */
        Public Sub setVersion(ByVal versionValue As Single)
            Me.version = versionValue
        End Sub

        '/**
        ' * @return Returns the xMaxExtent.
        ' */
        Public Function getXMaxExtent() As Short
            Return xMaxExtent
        End Function

        '/**
        ' * @param maxExtentValue The xMaxExtent to set.
        ' */
        Public Sub setXMaxExtent(ByVal maxExtentValue As Short)
            xMaxExtent = maxExtentValue
        End Sub

    End Class

End Namespace
