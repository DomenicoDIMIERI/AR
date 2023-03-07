Imports System.IO

Namespace org.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class OS2WindowsMetricsTable
        Inherits TTFTable

        ''' <summary>
        ''' Weight class constant.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WEIGHT_CLASS_THIN = 100
        Public Const WEIGHT_CLASS_ULTRA_LIGHT = 200
        Public Const WEIGHT_CLASS_LIGHT = 300
        Public Const WEIGHT_CLASS_NORMAL = 400
        Public Const WEIGHT_CLASS_MEDIUM = 500
        Public Const WEIGHT_CLASS_SEMI_BOLD = 600
        Public Const WEIGHT_CLASS_BOLD = 700
        Public Const WEIGHT_CLASS_EXTRA_BOLD = 800
        Public Const WEIGHT_CLASS_BLACK = 900

        ''' <summary>
        ''' Width class constant.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WIDTH_CLASS_ULTRA_CONDENSED = 1
        Public Const WIDTH_CLASS_EXTRA_CONDENSED = 2
        Public Const WIDTH_CLASS_CONDENSED = 3
        Public Const WIDTH_CLASS_SEMI_CONDENSED = 4
        Public Const WIDTH_CLASS_MEDIUM = 5
        Public Const WIDTH_CLASS_SEMI_EXPANDED = 6
        Public Const WIDTH_CLASS_EXPANDED = 7
        Public Const WIDTH_CLASS_EXTRA_EXPANDED = 8
        Public Const WIDTH_CLASS_ULTRA_EXPANDED = 9

        ''' <summary>
        ''' Family class constant.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FAMILY_CLASS_NO_CLASSIFICATION = 0
        Public Const FAMILY_CLASS_OLDSTYLE_SERIFS = 1
        Public Const FAMILY_CLASS_TRANSITIONAL_SERIFS = 2
        Public Const FAMILY_CLASS_MODERN_SERIFS = 3
        Public Const FAMILY_CLASS_CLAREDON_SERIFS = 4
        Public Const FAMILY_CLASS_SLAB_SERIFS = 5
        Public Const FAMILY_CLASS_FREEFORM_SERIFS = 7
        Public Const FAMILY_CLASS_SANS_SERIF = 8
        Public Const FAMILY_CLASS_ORNAMENTALS = 9
        Public Const FAMILY_CLASS_SCRIPTS = 10
        Public Const FAMILY_CLASS_SYMBOLIC = 12

        '/**
        ' * @return Returns the achVendId.
        ' */
        Public Function getAchVendId() As String
            Return achVendId
        End Function

        '/**
        ' * @param achVendIdValue The achVendId to set.
        ' */
        Public Sub setAchVendId(ByVal achVendIdValue As String)
            Me.achVendId = achVendIdValue
        End Sub

        '/**
        ' * @return Returns the averageCharWidth.
        ' */
        Public Function getAverageCharWidth() As Short
            Return averageCharWidth
        End Function

        '/**
        ' * @param averageCharWidthValue The averageCharWidth to set.
        ' */
        Public Sub setAverageCharWidth(ByVal averageCharWidthValue As Short)
            Me.averageCharWidth = averageCharWidthValue
        End Sub

        '/**
        ' * @return Returns the codePageRange1.
        ' */
        Public Function getCodePageRange1() As Long
            Return codePageRange1
        End Function

        '/**
        ' * @param codePageRange1Value The codePageRange1 to set.
        ' */
        Public Sub setCodePageRange1(ByVal codePageRange1Value As Long)
            Me.codePageRange1 = codePageRange1Value
        End Sub

        '/**
        ' * @return Returns the codePageRange2.
        ' */
        Public Function getCodePageRange2() As Long
            Return codePageRange2
        End Function

        '/**
        ' * @param codePageRange2Value The codePageRange2 to set.
        ' */
        Public Sub setCodePageRange2(ByVal codePageRange2Value As Long)
            Me.codePageRange2 = codePageRange2Value
        End Sub

        '/**
        ' * @return Returns the familyClass.
        ' */
        Public Function getFamilyClass() As Short
            Return familyClass
        End Function

        '/**
        ' * @param familyClassValue The familyClass to set.
        ' */
        Public Sub setFamilyClass(ByVal familyClassValue As Short)
            Me.familyClass = familyClassValue
        End Sub

        '/**
        ' * @return Returns the firstCharIndex.
        ' */
        Public Function getFirstCharIndex() As Integer
            Return firstCharIndex
        End Function

        '/**
        ' * @param firstCharIndexValue The firstCharIndex to set.
        ' */
        Public Sub setFirstCharIndex(ByVal firstCharIndexValue As Integer)
            Me.firstCharIndex = firstCharIndexValue
        End Sub

        '/**
        ' * @return Returns the fsSelection.
        ' */
        Public Function getFsSelection() As Integer
            Return fsSelection
        End Function

        '/**
        ' * @param fsSelectionValue The fsSelection to set.
        ' */
        Public Sub setFsSelection(ByVal fsSelectionValue As Integer)
            Me.fsSelection = fsSelectionValue
        End Sub

        '/**
        ' * @return Returns the fsType.
        ' */
        Public Function getFsType() As Short
            Return fsType
        End Function

        '/**
        ' * @param fsTypeValue The fsType to set.
        ' */
        Public Sub setFsType(ByVal fsTypeValue As Short)
            Me.fsType = fsTypeValue
        End Sub

        '/**
        ' * @return Returns the lastCharIndex.
        ' */
        Public Function getLastCharIndex() As Integer
            Return lastCharIndex
        End Function

        '/**
        ' * @param lastCharIndexValue The lastCharIndex to set.
        ' */
        Public Sub setLastCharIndex(ByVal lastCharIndexValue As Integer)
            Me.lastCharIndex = lastCharIndexValue
        End Sub

        '/**
        ' * @return Returns the panose.
        ' */
        Public Function getPanose() As Byte()
            Return panose
        End Function

        '/**
        ' * @param panoseValue The panose to set.
        ' */
        Public Sub setPanose(ByVal panoseValue() As Byte)
            Me.panose = panoseValue
        End Sub

        '/**
        ' * @return Returns the strikeoutPosition.
        ' */
        Public Function getStrikeoutPosition() As Short
            Return strikeoutPosition
        End Function

        '/**
        ' * @param strikeoutPositionValue The strikeoutPosition to set.
        ' */
        Public Sub setStrikeoutPosition(ByVal strikeoutPositionValue As Short)
            Me.strikeoutPosition = strikeoutPositionValue
        End Sub

        '/**
        ' * @return Returns the strikeoutSize.
        ' */
        Public Function getStrikeoutSize() As Short
            Return strikeoutSize
        End Function

        '/**
        ' * @param strikeoutSizeValue The strikeoutSize to set.
        ' */
        Public Sub setStrikeoutSize(ByVal strikeoutSizeValue As Short)
            Me.strikeoutSize = strikeoutSizeValue
        End Sub

        '/**
        ' * @return Returns the subscriptXOffset.
        ' */
        Public Function getSubscriptXOffset() As Short
            Return subscriptXOffset
        End Function

        '/**
        ' * @param subscriptXOffsetValue The subscriptXOffset to set.
        ' */
        Public Sub setSubscriptXOffset(ByVal subscriptXOffsetValue As Short)
            Me.subscriptXOffset = subscriptXOffsetValue
        End Sub

        '/**
        ' * @return Returns the subscriptXSize.
        ' */
        Public Function getSubscriptXSize() As Short
            Return subscriptXSize
        End Function

        '/**
        ' * @param subscriptXSizeValue The subscriptXSize to set.
        ' */
        Public Sub setSubscriptXSize(ByVal subscriptXSizeValue As Short)
            Me.subscriptXSize = subscriptXSizeValue
        End Sub

        '/**
        ' * @return Returns the subscriptYOffset.
        ' */
        Public Function getSubscriptYOffset() As Short
            Return subscriptYOffset
        End Function

        '/**
        ' * @param subscriptYOffsetValue The subscriptYOffset to set.
        ' */
        Public Sub setSubscriptYOffset(ByVal subscriptYOffsetValue As Short)
            Me.subscriptYOffset = subscriptYOffsetValue
        End Sub

        '/**
        ' * @return Returns the subscriptYSize.
        ' */
        Public Function getSubscriptYSize() As Short
            Return subscriptYSize
        End Function

        '/**
        ' * @param subscriptYSizeValue The subscriptYSize to set.
        ' */
        Public Sub setSubscriptYSize(ByVal subscriptYSizeValue As Short)
            Me.subscriptYSize = subscriptYSizeValue
        End Sub

        '/**
        ' * @return Returns the superscriptXOffset.
        ' */
        Public Function getSuperscriptXOffset() As Short
            Return superscriptXOffset
        End Function

        '/**
        ' * @param superscriptXOffsetValue The superscriptXOffset to set.
        ' */
        Public Sub setSuperscriptXOffset(ByVal superscriptXOffsetValue As Short)
            Me.superscriptXOffset = superscriptXOffsetValue
        End Sub

        '/**
        ' * @return Returns the superscriptXSize.
        ' */
        Public Function getSuperscriptXSize() As Short
            Return superscriptXSize
        End Function

        '/**
        ' * @param superscriptXSizeValue The superscriptXSize to set.
        ' */
        Public Sub setSuperscriptXSize(ByVal superscriptXSizeValue As Short)
            Me.superscriptXSize = superscriptXSizeValue
        End Sub

        '/**
        ' * @return Returns the superscriptYOffset.
        ' */
        Public Function getSuperscriptYOffset() As Short
            Return superscriptYOffset
        End Function

        '/**
        ' * @param superscriptYOffsetValue The superscriptYOffset to set.
        ' */
        Public Sub setSuperscriptYOffset(ByVal superscriptYOffsetValue As Short)
            Me.superscriptYOffset = superscriptYOffsetValue
        End Sub

        '/**
        ' * @return Returns the superscriptYSize.
        ' */
        Public Function getSuperscriptYSize() As Short
            Return superscriptYSize
        End Function

        '/**
        ' * @param superscriptYSizeValue The superscriptYSize to set.
        ' */
        Public Sub setSuperscriptYSize(ByVal superscriptYSizeValue As Short)
            Me.superscriptYSize = superscriptYSizeValue
        End Sub

        '/**
        ' * @return Returns the typeLineGap.
        ' */
        Public Function getTypeLineGap() As Integer
            Return typeLineGap
        End Function

        '/**
        ' * @param typeLineGapValue The typeLineGap to set.
        ' */
        Public Sub setTypeLineGap(ByVal typeLineGapValue As Integer)
            Me.typeLineGap = typeLineGapValue
        End Sub

        '/**
        ' * @return Returns the typoAscender.
        ' */
        Public Function getTypoAscender() As Integer
            Return typoAscender
        End Function

        '/**
        ' * @param typoAscenderValue The typoAscender to set.
        ' */
        Public Sub setTypoAscender(ByVal typoAscenderValue As Integer)
            Me.typoAscender = typoAscenderValue
        End Sub

        '/**
        ' * @return Returns the typoDescender.
        ' */
        Public Function getTypoDescender() As Integer
            Return typoDescender
        End Function

        '/**
        ' * @param typoDescenderValue The typoDescender to set.
        ' */
        Public Sub setTypoDescender(ByVal typoDescenderValue As Integer)
            Me.typoDescender = typoDescenderValue
        End Sub

        '/**
        ' * @return Returns the unicodeRange1.
        ' */
        Public Function getUnicodeRange1() As Long
            Return unicodeRange1
        End Function

        '/**
        ' * @param unicodeRange1Value The unicodeRange1 to set.
        ' */
        Public Sub setUnicodeRange1(ByVal unicodeRange1Value As Long)
            Me.unicodeRange1 = unicodeRange1Value
        End Sub

        '/**
        ' * @return Returns the unicodeRange2.
        ' */
        Public Function getUnicodeRange2() As Long
            Return unicodeRange2
        End Function

        '/**
        ' * @param unicodeRange2Value The unicodeRange2 to set.
        ' */
        Public Sub setUnicodeRange2(ByVal unicodeRange2Value As Long)
            Me.unicodeRange2 = unicodeRange2Value
        End Sub

        '/**
        ' * @return Returns the unicodeRange3.
        ' */
        Public Function getUnicodeRange3() As Long
            Return unicodeRange3
        End Function

        '/**
        ' * @param unicodeRange3Value The unicodeRange3 to set.
        ' */
        Public Sub setUnicodeRange3(ByVal unicodeRange3Value As Long)
            Me.unicodeRange3 = unicodeRange3Value
        End Sub

        '/**
        ' * @return Returns the unicodeRange4.
        ' */
        Public Function getUnicodeRange4() As Long
            Return unicodeRange4
        End Function

        '/**
        ' * @param unicodeRange4Value The unicodeRange4 to set.
        ' */
        Public Sub setUnicodeRange4(ByVal unicodeRange4Value As Long)
            Me.unicodeRange4 = unicodeRange4Value
        End Sub

        '/**
        ' * @return Returns the version.
        ' */
        Public Function getVersion() As Integer
            Return Version
        End Function

        '/**
        ' * @param versionValue The version to set.
        ' */
        Public Sub setVersion(ByVal versionValue As Integer)
            Me.version = versionValue
        End Sub

        '/**
        ' * @return Returns the weightClass.
        ' */
        Public Function getWeightClass() As Integer
            Return weightClass
        End Function

        '/**
        ' * @param weightClassValue The weightClass to set.
        ' */
        Public Sub setWeightClass(ByVal weightClassValue As Integer)
            Me.weightClass = weightClassValue
        End Sub

        '/**
        ' * @return Returns the widthClass.
        ' */
        Public Function getWidthClass() As Integer
            Return widthClass
        End Function

        '/**
        ' * @param widthClassValue The widthClass to set.
        ' */
        Public Sub setWidthClass(ByVal widthClassValue As Integer)
            Me.widthClass = widthClassValue
        End Sub

        '/**
        ' * @return Returns the winAscent.
        ' */
        Public Function getWinAscent() As Integer
            Return winAscent
        End Function

        '/**
        ' * @param winAscentValue The winAscent to set.
        ' */
        Public Sub setWinAscent(ByVal winAscentValue As Integer)
            Me.winAscent = winAscentValue
        End Sub

        '/**
        ' * @return Returns the winDescent.
        ' */
        Public Function getWinDescent() As Integer
            Return winDescent
        End Function

        '/**
        ' * @param winDescentValue The winDescent to set.
        ' */
        Public Sub setWinDescent(ByVal winDescentValue As Integer)
            Me.winDescent = winDescentValue
        End Sub

        Private version As Integer
        Private averageCharWidth As Short
        Private weightClass As Integer
        Private widthClass As Integer
        Private fsType As Short
        Private subscriptXSize As Short
        Private subscriptYSize As Short
        Private subscriptXOffset As Short
        Private subscriptYOffset As Short
        Private superscriptXSize As Short
        Private superscriptYSize As Short
        Private superscriptXOffset As Short
        Private superscriptYOffset As Short
        Private strikeoutSize As Short
        Private strikeoutPosition As Short
        Private familyClass As Short
        Private panose() As Byte = Array.CreateInstance(GetType(Byte), 10)
        Private unicodeRange1 As Long
        Private unicodeRange2 As Long
        Private unicodeRange3 As Long
        Private unicodeRange4 As Long
        Private achVendId As String
        Private fsSelection As Integer
        Private firstCharIndex As Integer
        Private lastCharIndex As Integer
        Private typoAscender As Integer
        Private typoDescender As Integer
        Private typeLineGap As Integer
        Private winAscent As Integer
        Private winDescent As Integer
        Private codePageRange1 As Long = -1
        Private codePageRange2 As Long = -1

        ''' <summary>
        ''' A tag that identifies Me table type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "OS/2"

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            version = data.readUnsignedShort()
            averageCharWidth = data.readSignedShort()
            weightClass = data.readUnsignedShort()
            widthClass = data.readUnsignedShort()
            fsType = data.readSignedShort()
            subscriptXSize = data.readSignedShort()
            subscriptYSize = data.readSignedShort()
            subscriptXOffset = data.readSignedShort()
            subscriptYOffset = data.readSignedShort()
            superscriptXSize = data.readSignedShort()
            superscriptYSize = data.readSignedShort()
            superscriptXOffset = data.readSignedShort()
            superscriptYOffset = data.readSignedShort()
            strikeoutSize = data.readSignedShort()
            strikeoutPosition = data.readSignedShort()
            familyClass = data.readSignedShort()
            panose = data.read(10)
            unicodeRange1 = data.readUnsignedInt()
            unicodeRange2 = data.readUnsignedInt()
            unicodeRange3 = data.readUnsignedInt()
            unicodeRange4 = data.readUnsignedInt()
            achVendId = data.readString(4)
            fsSelection = data.readUnsignedShort()
            firstCharIndex = data.readUnsignedShort()
            lastCharIndex = data.readUnsignedShort()
            typoAscender = data.readSignedShort()
            typoDescender = data.readSignedShort()
            typeLineGap = data.readSignedShort()
            winAscent = data.readUnsignedShort()
            winDescent = data.readUnsignedShort()
            If (Version >= 1) Then
                codePageRange1 = data.readUnsignedInt()
                codePageRange2 = data.readUnsignedInt()
            End If
        End Sub


    End Class

End Namespace