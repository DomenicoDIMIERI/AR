Namespace org.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class HorizontalMetricsTable
        Inherits TTFTable

        ''' <summary>
        ''' A tag that identifies this table type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "hmtx"

        Private advanceWidth() As Integer
        Private leftSideBearing() As Short
        Private nonHorizontalLeftSideBearing() As Short

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            Dim hHeader As HorizontalHeaderTable = ttf.getHorizontalHeader()
            Dim maxp As MaximumProfileTable = ttf.getMaximumProfile()
            Dim numHMetrics As Integer = hHeader.getNumberOfHMetrics()
            Dim numGlyphs As Integer = maxp.getNumGlyphs()

            advanceWidth = Array.CreateInstance(GetType(Integer), numHMetrics)
            leftSideBearing = Array.CreateInstance(GetType(Short), numHMetrics)
            For i As Integer = 0 To numHMetrics - 1
                advanceWidth(i) = data.readUnsignedShort()
                leftSideBearing(i) = data.readSignedShort()
            Next

            Dim numberNonHorizontal As Integer = numGlyphs - numHMetrics
            nonHorizontalLeftSideBearing = Array.CreateInstance(GetType(Short), numberNonHorizontal)
            For i As Integer = 0 To numberNonHorizontal - 1
                nonHorizontalLeftSideBearing(i) = data.readSignedShort()
            Next
        End Sub

        '/**
        ' * @return Returns the advanceWidth.
        ' */
        Public Function getAdvanceWidth() As Integer()
            Return advanceWidth
        End Function

        '/**
        ' * @param advanceWidthValue The advanceWidth to set.
        ' */
        Public Sub setAdvanceWidth(ByVal advanceWidthValue() As Integer)
            Me.advanceWidth = advanceWidthValue
        End Sub

    End Class

End Namespace
