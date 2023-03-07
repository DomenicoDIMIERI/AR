Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class MaximumProfileTable
        Inherits TTFTable

        ''' <summary>
        ''' A tag that identifies Me table type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "maxp"

        Private version As Single
        Private numGlyphs As Integer
        Private maxPoints As Integer
        Private maxContours As Integer
        Private maxCompositePoints As Integer
        Private maxCompositeContours As Integer
        Private maxZones As Integer
        Private maxTwilightPoints As Integer
        Private maxStorage As Integer
        Private maxFunctionDefs As Integer
        Private maxInstructionDefs As Integer
        Private maxStackElements As Integer
        Private maxSizeOfInstructions As Integer
        Private maxComponentElements As Integer
        Private maxComponentDepth As Integer

        '/**
        ' * @return Returns the maxComponentDepth.
        ' */
        Public Function getMaxComponentDepth() As Integer
            Return maxComponentDepth
        End Function

        '/**
        '    * @param maxComponentDepthValue The maxComponentDepth to set.
        '    */
        Public Sub setMaxComponentDepth(ByVal maxComponentDepthValue As Integer)
            Me.maxComponentDepth = maxComponentDepthValue
        End Sub

        '/**
        ' * @return Returns the maxComponentElements.
        ' */
        Public Function getMaxComponentElements() As Integer
            Return maxComponentElements
        End Function

        '/**
        ' * @param maxComponentElementsValue The maxComponentElements to set.
        ' */
        Public Sub setMaxComponentElements(ByVal maxComponentElementsValue As Integer)
            Me.maxComponentElements = maxComponentElementsValue
        End Sub

        '/**
        ' * @return Returns the maxCompositeContours.
        ' */
        Public Function getMaxCompositeContours() As Integer
            Return maxCompositeContours
        End Function

        '/**
        ' * @param maxCompositeContoursValue The maxCompositeContours to set.
        ' */
        Public Sub setMaxCompositeContours(ByVal maxCompositeContoursValue As Integer)
            Me.maxCompositeContours = maxCompositeContoursValue
        End Sub

        '/**
        ' * @return Returns the maxCompositePoints.
        ' */
        Public Function getMaxCompositePoints() As Integer
            Return maxCompositePoints
        End Function

        '/**
        '    * @param maxCompositePointsValue The maxCompositePoints to set.
        '    */
        Public Sub setMaxCompositePoints(ByVal maxCompositePointsValue As Integer)
            Me.maxCompositePoints = maxCompositePointsValue
        End Sub

        '/**
        ' * @return Returns the maxContours.
        ' */
        Public Function getMaxContours() As Integer
            Return maxContours
        End Function

        '/**
        ' * @param maxContoursValue The maxContours to set.
        ' */
        Public Sub setMaxContours(ByVal maxContoursValue As Integer)
            Me.maxContours = maxContoursValue
        End Sub

        '/**
        ' * @return Returns the maxFunctionDefs.
        ' */
        Public Function getMaxFunctionDefs() As Integer
            Return maxFunctionDefs
        End Function

        '/**
        ' * @param maxFunctionDefsValue The maxFunctionDefs to set.
        ' */
        Public Sub setMaxFunctionDefs(ByVal maxFunctionDefsValue As Integer)
            Me.maxFunctionDefs = maxFunctionDefsValue
        End Sub

        '/**
        ' * @return Returns the maxInstructionDefs.
        ' */
        Public Function getMaxInstructionDefs() As Integer
            Return maxInstructionDefs
        End Function

        '/**
        ' * @param maxInstructionDefsValue The maxInstructionDefs to set.
        ' */
        Public Sub setMaxInstructionDefs(ByVal maxInstructionDefsValue As Integer)
            Me.maxInstructionDefs = maxInstructionDefsValue
        End Sub

        '/**
        ' * @return Returns the maxPoints.
        ' */
        Public Function getMaxPoints() As Integer
            Return maxPoints
        End Function

        '/**
        ' * @param maxPointsValue The maxPoints to set.
        ' */
        Public Sub setMaxPoints(ByVal maxPointsValue As Integer)
            Me.maxPoints = maxPointsValue
        End Sub

        '/**
        ' * @return Returns the maxSizeOfInstructions.
        ' */
        Public Function getMaxSizeOfInstructions() As Integer
            Return maxSizeOfInstructions
        End Function

        '/**
        ' * @param maxSizeOfInstructionsValue The maxSizeOfInstructions to set.
        ' */
        Public Sub setMaxSizeOfInstructions(ByVal maxSizeOfInstructionsValue As Integer)
            Me.maxSizeOfInstructions = maxSizeOfInstructionsValue
        End Sub

        '/**
        ' * @return Returns the maxStackElements.
        ' */
        Public Function getMaxStackElements() As Integer
            Return maxStackElements
        End Function

        ' * @param maxStackElementsValue The maxStackElements to set.
        '*/
        Public Sub setMaxStackElements(ByVal maxStackElementsValue As Integer)
            Me.maxStackElements = maxStackElementsValue
        End Sub

        '/**
        ' * @return Returns the maxStorage.
        ' */
        Public Function getMaxStorage() As Integer
            Return maxStorage
        End Function

        '/**
        ' * @param maxStorageValue The maxStorage to set.
        ' */
        Public Sub setMaxStorage(ByVal maxStorageValue As Integer)
            Me.maxStorage = maxStorageValue
        End Sub

        '/**
        ' * @return Returns the maxTwilightPoints.
        ' */
        Public Function getMaxTwilightPoints() As Integer
            Return maxTwilightPoints
        End Function

        '/**
        ' * @param maxTwilightPointsValue The maxTwilightPoints to set.
        ' */
        Public Sub setMaxTwilightPoints(ByVal maxTwilightPointsValue As Integer)
            Me.maxTwilightPoints = maxTwilightPointsValue
        End Sub

        '/**
        ' * @return Returns the maxZones.
        ' */
        Public Function getMaxZones() As Integer
            Return maxZones
        End Function

        '/**
        ' * @param maxZonesValue The maxZones to set.
        ' */
        Public Sub setMaxZones(ByVal maxZonesValue As Integer)
            Me.maxZones = maxZonesValue
        End Sub

        '/**
        ' * @return Returns the numGlyphs.
        ' */
        Public Function getNumGlyphs() As Integer
            Return numGlyphs
        End Function

        '/**
        ' * @param numGlyphsValue The numGlyphs to set.
        ' */
        Public Sub setNumGlyphs(ByVal numGlyphsValue As Integer)
            Me.numGlyphs = numGlyphsValue
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
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            version = data.read32Fixed()
            numGlyphs = data.readUnsignedShort()
            maxPoints = data.readUnsignedShort()
            maxContours = data.readUnsignedShort()
            maxCompositePoints = data.readUnsignedShort()
            maxCompositeContours = data.readUnsignedShort()
            maxZones = data.readUnsignedShort()
            maxTwilightPoints = data.readUnsignedShort()
            maxStorage = data.readUnsignedShort()
            maxFunctionDefs = data.readUnsignedShort()
            maxInstructionDefs = data.readUnsignedShort()
            maxStackElements = data.readUnsignedShort()
            maxSizeOfInstructions = data.readUnsignedShort()
            maxComponentElements = data.readUnsignedShort()
            maxComponentDepth = data.readUnsignedShort()
        End Sub

    End Class

End Namespace
