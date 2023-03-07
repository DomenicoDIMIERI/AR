Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class HeaderTable
        Inherits TTFTable

        ''' <summary>
        ''' Tag to identify Me table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "head"

        Private version As Single
        Private fontRevision As Single
        Private checkSumAdjustment As Long
        Private magicNumber As Long
        Private flags As Integer
        Private unitsPerEm As Integer
        Private created As NDate
        Private modified As NDate
        Private xMin As Short
        Private yMin As Short
        Private xMax As Short
        Private yMax As Short
        Private macStyle As Integer
        Private lowestRecPPEM As Integer
        Private fontDirectionHint As Short
        Private indexToLocFormat As Short
        Private glyphDataFormat As Short

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            version = data.read32Fixed()
            fontRevision = data.read32Fixed()
            checkSumAdjustment = data.readUnsignedInt()
            magicNumber = data.readUnsignedInt()
            flags = data.readUnsignedShort()
            unitsPerEm = data.readUnsignedShort()
            created = data.readInternationalDate()
            modified = data.readInternationalDate()
            xMin = data.readSignedShort()
            yMin = data.readSignedShort()
            xMax = data.readSignedShort()
            yMax = data.readSignedShort()
            macStyle = data.readUnsignedShort()
            lowestRecPPEM = data.readUnsignedShort()
            fontDirectionHint = data.readSignedShort()
            indexToLocFormat = data.readSignedShort()
            glyphDataFormat = data.readSignedShort()
        End Sub

        '/**
        ' * @return Returns the checkSumAdjustment.
        ' */
        Public Function getCheckSumAdjustment() As Long
            Return checkSumAdjustment
        End Function

        '/**
        ' * @param checkSumAdjustmentValue The checkSumAdjustment to set.
        ' */
        Public Sub setCheckSumAdjustment(ByVal checkSumAdjustmentValue As Long)
            Me.checkSumAdjustment = checkSumAdjustmentValue
        End Sub

        '/**
        ' * @return Returns the created.
        ' */
        Public Function getCreated() As NDate 'Calendar 
            Return created
        End Function

        '/**
        ' * @param createdValue The created to set.
        ' */
        Public Sub setCreated(ByVal createdValue As NDate) 'Calendar 
            Me.created = createdValue
        End Sub

        '/**
        ' * @return Returns the flags.
        ' */
        Public Function getFlags() As Integer
            Return flags
        End Function

        '/**
        ' * @param flagsValue The flags to set.
        ' */
        Public Sub setFlags(ByVal flagsValue As Integer)
            Me.flags = flagsValue
        End Sub

        '/**
        ' * @return Returns the fontDirectionHint.
        ' */
        Public Function getFontDirectionHint() As Short
            Return fontDirectionHint
        End Function

        '/**
        ' * @param fontDirectionHintValue The fontDirectionHint to set.
        ' */
        Public Sub setFontDirectionHint(ByVal fontDirectionHintValue As Short)
            Me.fontDirectionHint = fontDirectionHintValue
        End Sub

        '/**
        ' * @return Returns the fontRevision.
        ' */
        Public Function getFontRevision() As Single
            Return fontRevision
        End Function

        '/**
        ' * @param fontRevisionValue The fontRevision to set.
        ' */
        Public Sub setFontRevision(ByVal fontRevisionValue As Single)
            Me.fontRevision = fontRevisionValue
        End Sub

        '/**
        ' * @return Returns the glyphDataFormat.
        ' */
        Public Function getGlyphDataFormat() As Short
            Return glyphDataFormat
        End Function

        '/**
        ' * @param glyphDataFormatValue The glyphDataFormat to set.
        ' */
        Public Sub setGlyphDataFormat(ByVal glyphDataFormatValue As Short)
            Me.glyphDataFormat = glyphDataFormatValue
        End Sub

        '/**
        ' * @return Returns the indexToLocFormat.
        ' */
        Public Function getIndexToLocFormat() As Short
            Return indexToLocFormat
        End Function

        '/**
        ' * @param indexToLocFormatValue The indexToLocFormat to set.
        ' */
        Public Sub setIndexToLocFormat(ByVal indexToLocFormatValue As Short)
            Me.indexToLocFormat = indexToLocFormatValue
        End Sub

        '/**
        ' * @return Returns the lowestRecPPEM.
        ' */
        Public Function getLowestRecPPEM() As Integer
            Return lowestRecPPEM
        End Function

        '/**
        ' * @param lowestRecPPEMValue The lowestRecPPEM to set.
        ' */
        Public Sub setLowestRecPPEM(ByVal lowestRecPPEMValue As Integer)
            Me.lowestRecPPEM = lowestRecPPEMValue
        End Sub

        '/**
        ' * @return Returns the macStyle.
        ' */
        Public Function getMacStyle() As Integer
            Return macStyle
        End Function

        '/**
        ' * @param macStyleValue The macStyle to set.
        ' */
        Public Sub setMacStyle(ByVal macStyleValue As Integer)
            Me.macStyle = macStyleValue
        End Sub

        '/**
        ' * @return Returns the magicNumber.
        ' */
        Public Function getMagicNumber() As Long
            Return magicNumber
        End Function

        '/**
        ' * @param magicNumberValue The magicNumber to set.
        ' */
        Public Sub setMagicNumber(ByVal magicNumberValue As Long)
            Me.magicNumber = magicNumberValue
        End Sub

        '/**
        ' * @return Returns the modified.
        ' */
        Public Function getModified() As NDate 'Calendar 
            Return modified
        End Function

        '/**
        ' * @param modifiedValue The modified to set.
        ' */
        Public Sub setModified(ByVal modifiedValue As NDate) 'Calendar 
            Me.modified = modifiedValue
        End Sub

        '/**
        ' * @return Returns the unitsPerEm.
        ' */
        Public Function getUnitsPerEm() As Integer
            Return unitsPerEm
        End Function

        '/**
        ' * @param unitsPerEmValue The unitsPerEm to set.
        ' */
        Public Sub setUnitsPerEm(ByVal unitsPerEmValue As Integer)
            Me.unitsPerEm = unitsPerEmValue
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
        ' * @return Returns the xMax.
        ' */
        Public Function getXMax() As Short
            Return xMax
        End Function

        '/**
        ' * @param maxValue The xMax to set.
        ' */
        Public Sub setXMax(ByVal maxValue As Short)
            xMax = maxValue
        End Sub

        '/**
        ' * @return Returns the xMin.
        ' */
        Public Function getXMin() As Short
            Return xMin
        End Function

        '/**
        ' * @param minValue The xMin to set.
        ' */
        Public Sub setXMin(ByVal minValue As Short)
            xMin = minValue
        End Sub

        '/**
        ' * @return Returns the yMax.
        ' */
        Public Function getYMax() As Short
            Return yMax
        End Function

        '/**
        ' * @param maxValue The yMax to set.
        ' */
        Public Sub setYMax(ByVal maxValue As Short)
            yMax = maxValue
        End Sub

        '/**
        ' * @return Returns the yMin.
        ' */
        Public Function getYMin() As Short
            Return yMin
        End Function

        '/**
        ' * @param minValue The yMin to set.
        ' */
        Public Sub setYMin(ByVal minValue As Short)
            yMin = minValue
        End Sub

    End Class

End Namespace
