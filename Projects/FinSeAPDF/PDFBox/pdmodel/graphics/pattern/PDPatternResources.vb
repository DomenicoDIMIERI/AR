Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.pattern

Namespace org.apache.pdfbox.pdmodel.graphics.pattern

    '/**
    ' * This represents the resources for a pattern colorspace.
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public MustInherit Class PDPatternResources
        Implements COSObjectable

        Private patternDictionary As COSDictionary

        Public Const TILING_PATTERN As Integer = 1
        Public Const SHADING_PATTERN As Integer = 2

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            patternDictionary = New COSDictionary()
            patternDictionary.setName(COSName.TYPE, COSName.PATTERN.getName())
        End Sub

        '/**
        ' * Prepopulated pattern resources.
        ' *
        ' * @param resourceDictionary The COSDictionary for this pattern resource.
        ' */
        Public Sub New(ByVal resourceDictionary As COSDictionary)
            patternDictionary = resourceDictionary
        End Sub

        '/**
        ' * This will get the underlying dictionary.
        ' *
        ' * @return The dictionary for these pattern resources.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return patternDictionary
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return patternDictionary
        End Function

        '/**
        ' * Sets the filter entry of the encryption dictionary.
        ' *
        ' * @param filter The filter name.
        ' */
        Public Sub setFilter(ByVal filter As String)
            patternDictionary.setItem(COSName.FILTER, COSName.getPDFName(filter))
        End Sub

        '/**
        ' * Get the name of the filter.
        ' *
        ' * @return The filter name contained in this encryption dictionary.
        ' */
        Public Function getFilter() As String
            Return patternDictionary.getNameAsString(COSName.FILTER)
        End Function

        '/**
        ' * This will set the length of the content stream.
        ' *
        ' * @param length The new stream length.
        ' */
        Public Overridable Sub setLength(ByVal length As Integer)
            patternDictionary.setInt(COSName.LENGTH, length)
        End Sub

        '/**
        ' * This will return the length of the content stream.
        ' *
        ' * @return The length of the content stream
        ' */
        Public Overridable Function getLength() As Integer
            Return patternDictionary.getInt(COSName.LENGTH, 0)
        End Function

        '/**
        ' * This will set the paint type.
        ' *
        ' * @param paintType The new paint type.
        ' */
        Public Overridable Sub setPaintType(ByVal paintType As Integer)
            patternDictionary.setInt(COSName.PAINT_TYPE, paintType)
        End Sub

        '/**
        ' * This will return the paint type.
        ' *
        ' * @return The type of object that this is.
        ' */
        Public Function getObjectType() As String
            Return COSName.PATTERN.getName()
        End Function

        '/**
        ' * This will set the pattern type.
        ' *
        ' * @param patternType The new pattern type.
        ' */
        Public Sub setPatternType(ByVal patternType As Integer)
            patternDictionary.setInt(COSName.PATTERN_TYPE, patternType)
        End Sub

        '/**
        ' * This will return the pattern type.
        ' *
        ' * @return The pattern type
        ' */
        Public MustOverride Function getPatternType() As Integer

        '/**
        ' * Create the correct PD Model pattern based on the COS base pattern.
        ' * 
        ' * @param resourceDictionary the COS pattern dictionary
        ' * 
        ' * @return the newly created pattern resources object
        ' * 
        ' * @throws IOException If we are unable to create the PDPattern object.
        ' */
        Public Shared Function create(ByVal resourceDictionary As COSDictionary) As PDPatternResources
            Dim pattern As PDPatternResources = Nothing
            Dim patternType As Integer = resourceDictionary.getInt(COSName.PATTERN_TYPE, 0)
            Select Case (patternType)
                Case TILING_PATTERN
                    pattern = New PDTilingPatternResources(resourceDictionary)
                Case SHADING_PATTERN
                    pattern = New PDShadingPatternResources(resourceDictionary)
                Case Else
                    Throw New IOException("Error: Unknown pattern type " & patternType.ToString)
            End Select
            Return pattern
        End Function

        '/**
        ' * This will return the paint of the pattern.
        ' * 
        ' * @param the height of the current page
        ' * 
        ' * @return the paint of the pattern
        ' */
        Public MustOverride Function getPaint(ByVal pageHeight As Integer) As Paint ' throws IOException;

    End Class

End Namespace
