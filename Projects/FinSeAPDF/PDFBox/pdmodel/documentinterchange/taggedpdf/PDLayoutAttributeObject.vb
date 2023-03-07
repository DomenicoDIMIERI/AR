Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf


    '/**
    ' * A Layout attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDLayoutAttributeObject
        Inherits PDStandardAttributeObject

        '/**
        ' * standard attribute owner: Layout
        ' */
        Public Const OWNER_LAYOUT As String = "Layout"

        Private Const PLACEMENT As String = "Placement"
        Private Const WRITING_MODE = "WritingMode"
        Private Const BACKGROUND_COLOR = "BackgroundColor"
        Private Const BORDER_COLOR = "BorderColor"
        Private Const BORDER_STYLE = "BorderStyle"
        Private Const BORDER_THICKNESS = "BorderThickness"
        Private Const PADDING = "Padding"
        Private Const COLOR = "Color"
        Private Const SPACE_BEFORE = "SpaceBefore"
        Private Const SPACE_AFTER = "SpaceAfter"
        Private Const START_INDENT = "StartIndent"
        Private Const END_INDENT = "EndIndent"
        Private Const TEXT_INDENT = "TextIndent"
        Private Const TEXT_ALIGN = "TextAlign"
        Private Const BBOX = "BBox"
        Private Const WIDTH = "Width"
        Private Const HEIGHT = "Height"
        Private Const BLOCK_ALIGN = "BlockAlign"
        Private Const INLINE_ALIGN = "InlineAlign"
        Private Const T_BORDER_STYLE = "TBorderStyle"
        Private Const T_PADDING = "TPadding"
        Private Const BASELINE_SHIFT = "BaselineShift"
        Private Const LINE_HEIGHT = "LineHeight"
        Private Const TEXT_DECORATION_COLOR = "TextDecorationColor"
        Private Const TEXT_DECORATION_THICKNESS = "TextDecorationThickness"
        Private Const TEXT_DECORATION_TYPE = "TextDecorationType"
        Private Const RUBY_ALIGN = "RubyAlign"
        Private Const RUBY_POSITION = "RubyPosition"
        Private Const GLYPH_ORIENTATION_VERTICAL = "GlyphOrientationVertical"
        Private Const COLUMN_COUNT = "ColumnCount"
        Private Const COLUMN_GAP = "ColumnGap"
        Private Const COLUMN_WIDTHS = "ColumnWidths"

        ''' <summary>
        ''' Placement: Block: Stacked in the block-progression direction within an enclosing reference area or parent BLSE.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLACEMENT_BLOCK = "Block"

        ''' <summary>
        ''' Placement: Inline: Packed in the inline-progression direction within an enclosing BLSE.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLACEMENT_INLINE = "Inline"

        ''' <summary>
        ''' Placement: Before: Placed so that the before edge of the element’s allocation rectangle coincides 
        ''' with that of the nearest enclosing reference area. The element may Single, if necessary, to achieve the
        ''' specified placement. The element shall be treated as a block occupying
        ''' the full extent of the enclosing reference area in the inline direction.
        ''' Other content shall be stacked so as to begin at the after edge of the
        ''' element’s allocation rectangle.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLACEMENT_BEFORE = "Before"

        ''' <summary>
        ''' Placement: Start: Placed so that the start edge of the element’s
        ''' allocation rectangle coincides with that of the nearest enclosing
        ''' reference area. The element may Single, if necessary, to achieve the
        ''' specified placement. Other content that would intrude into the element’s
        ''' allocation rectangle shall be laid out as a runaround.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLACEMENT_START = "Start"

        ''' <summary>
        ''' Placement: End: Placed so that the end edge of the element’s allocation
        ''' rectangle coincides with that of the nearest enclosing reference area.
        ''' The element may Single, if necessary, to achieve the specified placement.
        ''' Other content that would intrude into the element’s allocation rectangle
        ''' shall be laid out as a runaround.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLACEMENT_END = "End"

        ''' <summary>
        ''' WritingMode: LrTb: Inline progression from left to right; block
        ''' progression from top to bottom. Me is the typical writing mode for
        ''' Western writing systems.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WRITING_MODE_LRTB = "LrTb"

        ''' <summary>
        ''' WritingMode: RlTb: Inline progression from right to left; block
        ''' progression from top to bottom. Me is the typical writing mode for
        ''' Arabic and Hebrew writing systems.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WRITING_MODE_RLTB = "RlTb"

        ''' <summary>
        ''' WritingMode: TbRl: Inline progression from top to bottom; block
        ''' progression from right to left. Me is the typical writing mode for
        ''' Chinese and Japanese writing systems.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WRITING_MODE_TBRL = "TbRl"

        ''' <summary>
        ''' BorderStyle: None: No border. Forces the computed value of
        ''' BorderThickness to be 0.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_NONE = "None"

        ''' <summary>
        ''' BorderStyle: Hidden: Same as {@link #BORDER_STYLE_NONE}, except in terms of border conflict resolution for table elements.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_HIDDEN = "Hidden"

        ''' <summary>
        ''' BorderStyle: Dotted: The border is a series of dots.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_DOTTED = "Dotted"

        ''' <summary>
        ''' BorderStyle: Dashed: The border is a series of short line segments.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_DASHED = "Dashed"

        ''' <summary>
        ''' BorderStyle: Solid: The border is a single line segment.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_SOLID = "Solid"

        ''' <summary>
        ''' BorderStyle: Double: The border is two solid lines. The sum of the two lines and the space between them equals the value of BorderThickness.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_DOUBLE = "Double"

        ''' <summary>
        ''' BorderStyle: Groove: The border looks as though it were carved into the canvas.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_GROOVE = "Groove"

        ''' <summary>
        ''' BorderStyle: Ridge: The border looks as though it were coming out of the canvas (the opposite of {@link #BORDER_STYLE_GROOVE}).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_RIDGE = "Ridge"

        ''' <summary>
        ''' BorderStyle: Inset: The border makes the entire box look as though it were embedded in the canvas.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_INSET = "Inset"

        ''' <summary>
        ''' BorderStyle: Outset: The border makes the entire box look as though it were coming out of the canvas (the opposite of {@link #BORDER_STYLE_INSET}.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BORDER_STYLE_OUTSET = "Outset"

        ''' <summary>
        ''' TextAlign: Start: Aligned with the start edge.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_ALIGN_START = "Start"

        ''' <summary>
        ''' TextAlign: Center: Centered between the start and end edges.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_ALIGN_CENTER = "Center"

        ''' <summary>
        ''' TextAlign: End: Aligned with the end edge.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_ALIGN_END = "End"

        ''' <summary>
        ''' TextAlign: Justify: Aligned with both the start and end edges, with internal spacing 
        ''' within each line expanded, if necessary, to achieve such
        ''' alignment. The last (or only) line shall be aligned with the start edge
        ''' only.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_ALIGN_JUSTIFY = "Justify"

        ''' <summary>
        ''' Width: Auto
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WIDTH_AUTO = "Auto"

        ''' <summary>
        ''' Height: Auto
        ''' </summary>
        ''' <remarks></remarks>
        Public Const HEIGHT_AUTO = "Auto"

        ''' <summary>
        ''' BlockAlign: Before: Before edge of the first child’s allocation rectangle
        ''' aligned with that of the table cell’s content rectangle.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BLOCK_ALIGN_BEFORE = "Before"

        ''' <summary>
        ''' BlockAlign: Middle: Children centered within the table cell. The distance
        ''' between the before edge of the first child’s allocation rectangle and
        ''' that of the table cell’s content rectangle shall be the same as the
        ''' distance between the after edge of the last child’s allocation rectangle
        ''' and that of the table cell’s content rectangle.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BLOCK_ALIGN_MIDDLE = "Middle"

        ''' <summary>
        ''' BlockAlign: After: After edge of the last child’s allocation rectangle aligned with that of the table cell’s content rectangle.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BLOCK_ALIGN_AFTER = "After"

        ''' <summary>
        ''' BlockAlign: Justify: Children aligned with both the before and after
        ''' edges of the table cell’s content rectangle. The first child shall be
        ''' placed as described for {@link #BLOCK_ALIGN_BEFORE} and the last child as
        ''' described for {@link #BLOCK_ALIGN_AFTER}, with equal spacing between the
        ''' children. If there is only one child, it shall be aligned with the before
        ''' edge only, as for {@link #BLOCK_ALIGN_BEFORE}.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BLOCK_ALIGN_JUSTIFY = "Justify"

        ''' <summary>
        ''' InlineAlign: Start: Start edge of each child’s allocation rectangle aligned with that of the table cell’s content rectangle.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const INLINE_ALIGN_START = "Start"

        ''' <summary>
        ''' InlineAlign: Center: Each child centered within the table cell. The
        ''' distance between the start edges of the child’s allocation rectangle and
        ''' the table cell’s content rectangle shall be the same as the distance
        ''' between their end edges.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const INLINE_ALIGN_CENTER = "Center"

        ''' <summary>
        ''' InlineAlign: End: End edge of each child’s allocation rectangle aligned with that of the table cell’s content rectangle.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const INLINE_ALIGN_END = "End"

        ''' <summary>
        ''' LineHeight: NormalAdjust the line height to include any nonzero value specified for BaselineShift.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LINE_HEIGHT_NORMAL = "Normal"

        ''' <summary>
        ''' LineHeight: Auto: Adjustment for the value of BaselineShift shall not be made.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LINE_HEIGHT_AUTO = "Auto"

        ''' <summary>
        ''' TextDecorationType: None: No text decoration
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_DECORATION_TYPE_NONE = "None"

        ''' <summary>
        ''' TextDecorationType: Underline: A line below the text
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_DECORATION_TYPE_UNDERLINE = "Underline"

        ''' <summary>
        ''' TextDecorationType: Overline: A line above the text
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_DECORATION_TYPE_OVERLINE = "Overline"

        ''' <summary>
        ''' TextDecorationType: LineThrough: A line through the middle of the text
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TEXT_DECORATION_TYPE_LINE_THROUGH = "LineThrough"

        ''' <summary>
        ''' RubyAlign: Start: The content shall be aligned on the start edge in the inline-progression direction.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_ALIGN_START = "Start"

        ''' <summary>
        ''' RubyAlign: Center: The content shall be centered in the inline-progression direction.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_ALIGN_CENTER = "Center"

        ''' <summary>
        ''' RubyAlign: End: The content shall be aligned on the end edge in the inline-progression direction.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_ALIGN_END = "End"

        ''' <summary>
        ''' RubyAlign: Justify:  The content shall be expanded to fill the available width in the inline-progression direction.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_ALIGN_JUSTIFY = "Justify"

        ''' <summary>
        ''' RubyAlign: Distribute: The content shall be expanded to fill the
        ''' available width in the inline-progression direction. However, space shall
        ''' also be inserted at the start edge and end edge of the text. The spacing
        ''' shall be distributed using a 1:2:1 (start:infix:end) ratio. It shall be
        ''' changed to a 0:1:1 ratio if the ruby appears at the start of a text line
        ''' or to a 1:1:0 ratio if the ruby appears at the end of the text line.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_ALIGN_DISTRIBUTE = "Distribute"

        ''' <summary>
        ''' RubyPosition: Before: The RT content shall be aligned along the before edge of the element.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_POSITION_BEFORE = "Before"

        ''' <summary>
        ''' RubyPosition: After: The RT content shall be aligned along the after edge of the element.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_POSITION_AFTER = "After"

        ''' <summary>
        ''' RubyPosition: Warichu: The RT and associated RP elements shall be formatted as a warichu, following the RB element.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_POSITION_WARICHU = "Warichu"

        ''' <summary>
        ''' RubyPosition: Inline: The RT and associated RP elements shall be formatted as a parenthesis comment, following the RB element.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY_POSITION_INLINE = "Inline"

        ''' <summary>
        ''' GlyphOrientationVertical: Auto
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_AUTO = "Auto"

        ''' <summary>
        ''' GlyphOrientationVertical: -180°
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_MINUS_180_DEGREES = "-180"

        ''' <summary>
        ''' GlyphOrientationVertical: -90°
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_MINUS_90_DEGREES = "-90"

        ''' <summary>
        ''' GlyphOrientationVertical: 0°
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_ZERO_DEGREES = "0"

        ''' <summary>
        ''' GlyphOrientationVertical: 90°
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_90_DEGREES = "90"

        ''' <summary>
        ''' GlyphOrientationVertical: 180°
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_180_DEGREES = "180"

        ''' <summary>
        ''' GlyphOrientationVertical: 270°
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_270_DEGREES = "270"

        ''' <summary>
        ''' GlyphOrientationVertical: 360°
        ''' </summary>
        ''' <remarks></remarks>
        Public Const GLYPH_ORIENTATION_VERTICAL_360_DEGREES = "360"


        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            Me.setOwner(OWNER_LAYOUT)
        End Sub

        '/**
        ' * Creates a new Layout attribute object with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Gets the positioning of the element with respect to the enclosing
        ' * reference area and other content (Placement). The default value is
        ' * {@link #PLACEMENT_INLINE}.
        ' * 
        ' * @return the placement
        ' */
        Public Function getPlacement() As String
            Return Me.getName(PLACEMENT, PLACEMENT_INLINE)
        End Function

        '/**
        ' * Sets the positioning of the element with respect to the enclosing
        ' * reference area and other content (Placement). The value should be one of:
        ' * <ul>
        ' *   <li>{@link #PLACEMENT_BLOCK},</li>
        ' *   <li>{@link #PLACEMENT_INLINE},</li>
        ' *   <li>{@link #PLACEMENT_BEFORE},</li>
        ' *   <li>{@link #PLACEMENT_START},</li>
        ' *   <li>{@link #PLACEMENT_END}.</li>
        ' * <ul>
        ' * 
        ' * @param placement the placement
        ' */
        Public Sub setPlacement(ByVal placement As String)
            Me.setName(placement, placement)
        End Sub

        '/**
        ' * Gets the writing mode (WritingMode). The default value is
        ' * {@link #WRITING_MODE_LRTB}.
        ' * 
        ' * @return the writing mode
        ' */
        Public Function getWritingMode() As String
            Return Me.getName(WRITING_MODE, WRITING_MODE_LRTB)
        End Function

        '/**
        ' * Sets the writing mode (WritingMode). The value should be one of:
        ' * <ul>
        ' *   <li>{@link #WRITING_MODE_LRTB},</li>
        ' *   <li>{@link #WRITING_MODE_RLTB},</li>
        ' *   <li>{@link #WRITING_MODE_TBRL}.</li>
        ' * </ul>
        ' * 
        ' * @param writingMode the writing mode
        ' */
        Public Sub setWritingMode(ByVal writingMode As String)
            Me.setName(WRITING_MODE, writingMode)
        End Sub

        '/**
        ' * Gets the background colour (BackgroundColor).
        ' * 
        ' * @return the background colour
        ' */
        Public Function getBackgroundColor() As PDGamma
            Return Me.getColor(BACKGROUND_COLOR)
        End Function

        '/**
        ' * Sets the background colour (BackgroundColor).
        ' * 
        ' * @param backgroundColor the background colour
        ' */
        Public Sub setBackgroundColor(ByVal backgroundColor As PDGamma)
            Me.setColor(BACKGROUND_COLOR, backgroundColor)
        End Sub

        '/**
        ' * Gets the border colour (BorderColor).
        ' * 
        ' * @return a single border colour ({@link PDGamma}) or four border colours
        ' *  ({@link PDFourColours})
        ' */
        Public Function getBorderColors() As Object
            Return Me.getColorOrFourColors(BORDER_COLOR)
        End Function

        '/**
        ' * Sets the same border colour for all four sides (BorderColor).
        ' * 
        ' * @param borderColor the border colour
        ' */
        Public Sub setAllBorderColors(ByVal borderColor As PDGamma)
            Me.setColor(BORDER_COLOR, borderColor)
        End Sub

        '/**
        ' * Sets the border colours for four sides separately (BorderColor).
        ' * 
        ' * @param borderColors the border colours
        ' */
        Public Sub setBorderColors(ByVal borderColors As PDFourColours)
            Me.setFourColors(BORDER_COLOR, borderColors)
        End Sub

        '/**
        ' * Gets the border style (BorderStyle). The default value is
        ' * {@link #BORDER_STYLE_NONE}.
        ' * 
        ' * @return the border styles (a String or an array of four Strings)
        ' */
        Public Function getBorderStyle() As Object
            Return Me.getNameOrArrayOfName(BORDER_STYLE, BORDER_STYLE_NONE)
        End Function

        '/**
        ' * Sets the same border style for all four sides (BorderStyle). The value
        ' * should be one of:
        ' * <ul>
        ' *   <li>{@link #BORDER_STYLE_NONE},</li>
        ' *   <li>{@link #BORDER_STYLE_HIDDEN},</li>
        ' *   <li>{@link #BORDER_STYLE_DOTTED},</li>
        ' *   <li>{@link #BORDER_STYLE_DASHED},</li>
        ' *   <li>{@link #BORDER_STYLE_SOLID},</li>
        ' *   <li>{@link #BORDER_STYLE_DOUBLE},</li>
        ' *   <li>{@link #BORDER_STYLE_GROOVE},</li>
        ' *   <li>{@link #BORDER_STYLE_RIDGE},</li>
        ' *   <li>{@link #BORDER_STYLE_INSET},</li>
        ' *   <li>{@link #BORDER_STYLE_OUTSET}.</li>
        ' * </ul>
        ' * 
        ' * @param borderStyle the border style
        ' */
        Public Sub setAllBorderStyles(ByVal borderStyle As String)
            Me.setName(BORDER_STYLE, borderStyle)
        End Sub

        '/**
        ' * Sets the border styles for four sides separately (BorderStyle). The
        ' * values should be of:
        ' * <ul>
        ' *   <li>{@link #BORDER_STYLE_NONE},</li>
        ' *   <li>{@link #BORDER_STYLE_HIDDEN},</li>
        ' *   <li>{@link #BORDER_STYLE_DOTTED},</li>
        ' *   <li>{@link #BORDER_STYLE_DASHED},</li>
        ' *   <li>{@link #BORDER_STYLE_SOLID},</li>
        ' *   <li>{@link #BORDER_STYLE_DOUBLE},</li>
        ' *   <li>{@link #BORDER_STYLE_GROOVE},</li>
        ' *   <li>{@link #BORDER_STYLE_RIDGE},</li>
        ' *   <li>{@link #BORDER_STYLE_INSET},</li>
        ' *   <li>{@link #BORDER_STYLE_OUTSET}.</li>
        ' * </ul>
        ' * 
        ' * @param borderStyles the border styles (an array of four Strings)
        ' */
        Public Sub setBorderStyles(ByVal borderStyles() As String)
            Me.setArrayOfName(BORDER_STYLE, borderStyles)
        End Sub

        '/**
        ' * Gets the border thickness (BorderThickness).
        ' * 
        ' * @return the border thickness (a NFloat or an array of four floats)
        ' */
        Public Function getBorderThickness() As Object
            Return Me.getNumberOrArrayOfNumber(BORDER_THICKNESS, UNSPECIFIED)
        End Function

        '/**
        ' * Sets the same border thickness for all four sides (BorderThickness).
        ' * 
        ' * @param borderThickness the border thickness
        ' */
        Public Sub setAllBorderThicknesses(ByVal borderThickness As Single)
            Me.setNumber(BORDER_THICKNESS, borderThickness)
        End Sub

        '/**
        ' * Sets the same border thickness for all four sides (BorderThickness).
        ' * 
        ' * @param borderThickness the border thickness
        ' */
        Public Sub setAllBorderThicknesses(ByVal borderThickness As Integer)
            Me.setNumber(BORDER_THICKNESS, borderThickness)
        End Sub

        '/**
        ' * Sets the border thicknesses for four sides separately (BorderThickness).
        ' * 
        ' * @param borderThicknesses the border thickness (an array of four floats)
        ' */
        Public Sub setBorderThicknesses(ByVal borderThicknesses() As Single)
            Me.setArrayOfNumber(BORDER_THICKNESS, borderThicknesses)
        End Sub

        '/**
        ' * Gets the padding (Padding). The default value is 0.
        ' * 
        ' * @return the padding (a NFloat or an array of Single)
        ' */
        Public Function getPadding() As Object
            Return Me.getNumberOrArrayOfNumber(PADDING, 0.0F)
        End Function

        '/**
        ' * Sets the same padding for all four sides (Padding).
        ' * 
        ' * @param padding the padding
        ' */
        Public Sub setAllPaddings(ByVal padding As Single)
            Me.setNumber(padding.ToString, padding)
        End Sub

        '/**
        ' * Sets the same padding for all four sides (Padding).
        ' * 
        ' * @param padding the padding
        ' */
        Public Sub setAllPaddings(ByVal padding As Integer)
            Me.setNumber(padding.ToString, padding)
        End Sub

        '/**
        ' * Sets the paddings for four sides separately (Padding).
        ' * 
        ' * @param paddings the paddings (an array of four floats)
        ' */
        Public Sub setPaddings(ByVal paddings() As Single)
            Me.setArrayOfNumber(PADDING, paddings)
        End Sub

        '/**
        ' * Gets the color to be used for drawing text and the default value for the
        ' * colour of table borders and text decorations (Color).
        ' * 
        ' * @return the colour
        ' */
        Public Overloads Function getColor() As PDGamma
            Return Me.getColor(PDLayoutAttributeObject.COLOR)
        End Function

        '/**
        ' * Sets the color to be used for drawing text and the default value for the
        ' * colour of table borders and text decorations (Color).
        ' * 
        ' * @param color the colour
        ' */
        Public Overloads Sub setColor(ByVal color As PDGamma)
            Me.setColor(color.ToString, color)
        End Sub

        '/**
        ' * Gets the amount of extra space preceding the before edge of the BLSE in
        ' * the block-progression direction (SpaceBefore). The default value is 0.
        ' * 
        ' * @return the space before
        ' */
        Public Function getSpaceBefore() As Single
            Return Me.getNumber(SPACE_BEFORE, 0.0F)
        End Function

        '/**
        ' * Sets the amount of extra space preceding the before edge of the BLSE in
        ' * the block-progression direction (SpaceBefore).
        ' * 
        ' * @param spaceBefore the space before
        ' */
        Public Sub setSpaceBefore(ByVal spaceBefore As Single)
            Me.setNumber(SPACE_BEFORE, spaceBefore)
        End Sub

        '/**
        ' * Sets the amount of extra space preceding the before edge of the BLSE in
        ' * the block-progression direction (SpaceBefore).
        ' * 
        ' * @param spaceBefore the space before
        ' */
        Public Sub setSpaceBefore(ByVal spaceBefore As Integer)
            Me.setNumber(SPACE_BEFORE, spaceBefore)
        End Sub

        '/**
        ' * Gets the amount of extra space following the after edge of the BLSE in
        ' * the block-progression direction (SpaceAfter). The default value is 0.
        ' * 
        ' * @return the space after
        ' */
        Public Function getSpaceAfter() As Single
            Return Me.getNumber(SPACE_AFTER, 0.0F)
        End Function

        '/**
        ' * Sets the amount of extra space following the after edge of the BLSE in
        ' * the block-progression direction (SpaceAfter).
        ' * 
        ' * @param spaceAfter the space after
        ' */
        Public Sub setSpaceAfter(ByVal spaceAfter As Single)
            Me.setNumber(SPACE_AFTER, spaceAfter)
        End Sub

        '/**
        ' * Sets the amount of extra space following the after edge of the BLSE in
        ' * the block-progression direction (SpaceAfter).
        ' * 
        ' * @param spaceAfter the space after
        ' */
        Public Sub setSpaceAfter(ByVal spaceAfter As Integer)
            Me.setNumber(SPACE_AFTER, spaceAfter)
        End Sub

        '/**
        ' * Gets the distance from the start edge of the reference area to that of
        ' * the BLSE in the inline-progression direction (StartIndent). The default value is 0.
        ' * 
        ' * @return the start indent
        ' */
        Public Function getStartIndent() As Single
            Return Me.getNumber(START_INDENT, 0.0F)
        End Function

        '/**
        ' * Sets the distance from the start edge of the reference area to that of
        ' * the BLSE in the inline-progression direction (StartIndent).
        ' * 
        ' * @param startIndent the start indent
        ' */
        Public Sub setStartIndent(ByVal startIndent As Single)
            Me.setNumber(START_INDENT, startIndent)
        End Sub

        '/**
        ' * Sets the distance from the start edge of the reference area to that of
        ' * the BLSE in the inline-progression direction (StartIndent).
        ' * 
        ' * @param startIndent the start indent
        ' */
        Public Sub setStartIndent(ByVal startIndent As Integer)
            Me.setNumber(START_INDENT, startIndent)
        End Sub

        '/**
        ' * Gets the distance from the end edge of the BLSE to that of the reference
        ' * area in the inline-progression direction (EndIndent). The default value
        ' * is 0.
        ' * 
        ' * @return the end indent
        ' */
        Public Function getEndIndent() As Single
            Return Me.getNumber(END_INDENT, 0.0F)
        End Function

        '/**
        ' * Sets the distance from the end edge of the BLSE to that of the reference
        ' * area in the inline-progression direction (EndIndent).
        ' * 
        ' * @param endIndent the end indent
        ' */
        Public Sub setEndIndent(ByVal endIndent As Single)
            Me.setNumber(END_INDENT, endIndent)
        End Sub

        '/**
        ' * Sets the distance from the end edge of the BLSE to that of the reference
        ' * area in the inline-progression direction (EndIndent).
        ' * 
        ' * @param endIndent the end indent
        ' */
        Public Sub setEndIndent(ByVal endIndent As Integer)
            Me.setNumber(END_INDENT, endIndent)
        End Sub

        '/**
        ' * Gets the additional distance in the inline-progression direction from the
        ' * start edge of the BLSE, as specified by StartIndent, to that of the first
        ' * line of text (TextIndent). The default value is 0.
        ' * 
        ' * @return the text indent
        ' */
        Public Function getTextIndent() As Single
            Return Me.getNumber(TEXT_INDENT, 0.0F)
        End Function

        '/**
        ' * Sets the additional distance in the inline-progression direction from the
        ' * start edge of the BLSE, as specified by StartIndent, to that of the first
        ' * line of text (TextIndent).
        ' * 
        ' * @param textIndent the text indent
        ' */
        Public Sub setTextIndent(ByVal textIndent As Single)
            Me.setNumber(TEXT_INDENT, textIndent)
        End Sub

        '/**
        ' * Sets the additional distance in the inline-progression direction from the
        ' * start edge of the BLSE, as specified by StartIndent, to that of the first
        ' * line of text (TextIndent).
        ' * 
        ' * @param textIndent the text indent
        ' */
        Public Sub setTextIndent(ByVal textIndent As Integer)
            Me.setNumber(TEXT_INDENT, textIndent)
        End Sub

        '/**
        ' * Gets the alignment, in the inline-progression direction, of text and
        ' * other content within lines of the BLSE (TextAlign). The default value is
        ' * {@link #TEXT_ALIGN_START}.
        ' * 
        ' * @return the text alignment
        ' */
        Public Function getTextAlign() As String
            Return Me.getName(TEXT_ALIGN, TEXT_ALIGN_START)
        End Function

        '/**
        ' * Sets the alignment, in the inline-progression direction, of text and
        ' * other content within lines of the BLSE (TextAlign). The value should be
        ' * one of:
        ' * <ul>
        ' *   <li>{@link #TEXT_ALIGN_START},</li>
        ' *   <li>{@link #TEXT_ALIGN_CENTER},</li>
        ' *   <li>{@link #TEXT_ALIGN_END},</li>
        ' *   <li>{@link #TEXT_ALIGN_JUSTIFY}.</li>
        ' * </ul>
        ' * 
        ' * @param textIndent the text alignment
        ' */
        Public Sub setTextAlign(ByVal textIndent As String)
            Me.setName(TEXT_ALIGN, textIndent)
        End Sub

        '/**
        ' * Gets the bounding box.
        ' * 
        ' * @return the bounding box.
        ' */
        Public Function getBBox() As PDRectangle
            Dim array As COSArray = Me.getCOSDictionary().getDictionaryObject(BBOX)
            If (array IsNot Nothing) Then
                Return New PDRectangle(array)
            End If
            Return Nothing
        End Function

        '/**
        ' * Sets the bounding box.
        ' * 
        ' * @param bbox the bounding box
        ' */
        Public Sub setBBox(ByVal bbox As PDRectangle)
            Dim name As String = bbox.toString
            Dim oldValue As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setItem(name, bbox)
            Dim newValue As COSBase
            If (bbox Is Nothing) Then
                newValue = Nothing
            Else
                newValue = bbox.getCOSObject()
            End If
            Me.potentiallyNotifyChanged(oldValue, newValue)
        End Sub

        '/**
        ' * Gets the width of the element’s content rectangle in the
        ' * inline-progression direction (Width). The default value is
        ' * {@link #WIDTH_AUTO}.
        ' * 
        ' * @return the width (a NFloat or a String)
        ' */
        Public Function getWidth() As Object
            Return Me.getNumberOrName(WIDTH, WIDTH_AUTO)
        End Function

        '/**
        ' * Sets the width of the element’s content rectangle in the
        ' * inline-progression direction (Width) to {@link #WIDTH_AUTO}.
        ' */
        Public Sub setWidthAuto()
            Me.setName(WIDTH, WIDTH_AUTO)
        End Sub

        '/**
        ' * Sets the width of the element’s content rectangle in the
        ' * inline-progression direction (Width).
        ' * 
        ' * @param width the width
        ' */
        Public Sub setWidth(ByVal width As Single)
            Me.setNumber(width.ToString, width)
        End Sub

        '/**
        ' * Sets the width of the element’s content rectangle in the
        ' * inline-progression direction (Width).
        ' * 
        ' * @param width the width
        ' */
        Public Sub setWidth(ByVal width As Integer)
            Me.setNumber(width.ToString, width)
        End Sub

        '/**
        ' * Gets the height of the element’s content rectangle in the
        ' * block-progression direction (Height). The default value is
        ' * {@link #HEIGHT_AUTO}.
        ' * 
        ' * @return the height (a NFloat or a String)
        ' */
        Public Function getHeight() As Object
            Return Me.getNumberOrName(HEIGHT, HEIGHT_AUTO)
        End Function

        '/**
        ' * Sets the height of the element’s content rectangle in the
        ' * block-progression direction (Height) to {@link #HEIGHT_AUTO}.
        ' */
        Public Sub setHeightAuto()
            Me.setName(HEIGHT, HEIGHT_AUTO)
        End Sub

        '/**
        ' * Sets the height of the element’s content rectangle in the
        ' * block-progression direction (Height).
        ' * 
        ' * @param height the height
        ' */
        Public Sub setHeight(ByVal height As Single)
            Me.setNumber(height.ToString, height)
        End Sub

        '/**
        ' * Sets the height of the element’s content rectangle in the
        ' * block-progression direction (Height).
        ' * 
        ' * @param height the height
        ' */
        Public Sub setHeight(ByVal height As Integer)
            Me.setNumber(height.ToString, height)
        End Sub

        '/**
        ' * Gets the alignment, in the block-progression direction, of content within
        ' * the table cell (BlockAlign). The default value is
        ' * {@link #BLOCK_ALIGN_BEFORE}.
        ' * 
        ' * @return the block alignment
        ' */
        Public Function getBlockAlign() As String
            Return Me.getName(BLOCK_ALIGN, BLOCK_ALIGN_BEFORE)
        End Function

        '/**
        ' * Sets the alignment, in the block-progression direction, of content within
        ' * the table cell (BlockAlign). The value should be one of:
        ' * <ul>
        ' *   <li>{@link #BLOCK_ALIGN_BEFORE},</li>
        ' *   <li>{@link #BLOCK_ALIGN_MIDDLE},</li>
        ' *   <li>{@link #BLOCK_ALIGN_AFTER},</li>
        ' *   <li>{@link #BLOCK_ALIGN_JUSTIFY}.</li>
        ' * </ul>
        ' * 
        ' * @param blockAlign the block alignment
        ' */
        Public Sub setBlockAlign(ByVal blockAlign As String)
            Me.setName(BLOCK_ALIGN, blockAlign)
        End Sub

        '/**
        ' * Gets the alignment, in the inline-progression direction, of content
        ' * within the table cell (InlineAlign). The default value is
        ' * {@link #INLINE_ALIGN_START}.
        ' * 
        ' * @return the inline alignment
        ' */
        Public Function getInlineAlign() As String
            Return Me.getName(INLINE_ALIGN, INLINE_ALIGN_START)
        End Function

        '/**
        ' * Sets the alignment, in the inline-progression direction, of content
        ' * within the table cell (InlineAlign). The value should be one of
        ' * <ul>
        ' *   <li>{@link #INLINE_ALIGN_START},</li>
        ' *   <li>{@link #INLINE_ALIGN_CENTER},</li>
        ' *   <li>{@link #INLINE_ALIGN_END}.</li>
        ' * </ul>
        ' * 
        ' * @param inlineAlign the inline alignment
        ' */
        Public Sub setInlineAlign(ByVal inlineAlign As String)
            Me.setName(INLINE_ALIGN, inlineAlign)
        End Sub

        '/**
        ' * Gets the style of the border drawn on each edge of a table cell
        ' * (TBorderStyle).
        ' * 
        ' * @return
        ' */
        Public Function getTBorderStyle() As Object
            Return Me.getNameOrArrayOfName(T_BORDER_STYLE, BORDER_STYLE_NONE)
        End Function

        '/**
        ' * Sets the same table border style for all four sides (TBorderStyle). The
        ' * value should be one of:
        ' * <ul>
        ' *   <li>{@link #BORDER_STYLE_NONE},</li>
        ' *   <li>{@link #BORDER_STYLE_HIDDEN},</li>
        ' *   <li>{@link #BORDER_STYLE_DOTTED},</li>
        ' *   <li>{@link #BORDER_STYLE_DASHED},</li>
        ' *   <li>{@link #BORDER_STYLE_SOLID},</li>
        ' *   <li>{@link #BORDER_STYLE_DOUBLE},</li>
        ' *   <li>{@link #BORDER_STYLE_GROOVE},</li>
        ' *   <li>{@link #BORDER_STYLE_RIDGE},</li>
        ' *   <li>{@link #BORDER_STYLE_INSET},</li>
        ' *   <li>{@link #BORDER_STYLE_OUTSET}.</li>
        ' * </ul>
        ' * 
        ' * @param tBorderStyle the table border style
        ' */
        Public Sub setAllTBorderStyles(ByVal tBorderStyle As String)
            Me.setName(T_BORDER_STYLE, tBorderStyle)
        End Sub

        '/**
        ' * Sets the style of the border drawn on each edge of a table cell
        ' * (TBorderStyle). The values should be of:
        ' * <ul>
        ' *   <li>{@link #BORDER_STYLE_NONE},</li>
        ' *   <li>{@link #BORDER_STYLE_HIDDEN},</li>
        ' *   <li>{@link #BORDER_STYLE_DOTTED},</li>
        ' *   <li>{@link #BORDER_STYLE_DASHED},</li>
        ' *   <li>{@link #BORDER_STYLE_SOLID},</li>
        ' *   <li>{@link #BORDER_STYLE_DOUBLE},</li>
        ' *   <li>{@link #BORDER_STYLE_GROOVE},</li>
        ' *   <li>{@link #BORDER_STYLE_RIDGE},</li>
        ' *   <li>{@link #BORDER_STYLE_INSET},</li>
        ' *   <li>{@link #BORDER_STYLE_OUTSET}.</li>
        ' * </ul>
        ' * 
        ' * @param tBorderStyles
        ' */
        Public Sub setTBorderStyles(ByVal tBorderStyles() As String)
            Me.setArrayOfName(T_BORDER_STYLE, tBorderStyles)
        End Sub

        '/**
        ' * Gets the offset to account for the separation between the table cell’s
        ' * content rectangle and the surrounding border (TPadding). The default
        ' * value is 0.
        ' * 
        ' * @return the table padding (a NFloat or an array of Single)
        ' */
        Public Function getTPadding() As Object
            Return Me.getNumberOrArrayOfNumber(T_PADDING, 0.0F)
        End Function

        '/**
        ' * Sets the same table padding for all four sides (TPadding).
        ' * 
        ' * @param tPadding the table padding
        ' */
        Public Sub setAllTPaddings(ByVal tPadding As Single)
            Me.setNumber(T_PADDING, tPadding)
        End Sub

        '/**
        ' * Sets the same table padding for all four sides (TPadding).
        ' * 
        ' * @param tPadding the table padding
        ' */
        Public Sub setAllTPaddings(ByVal tPadding As Integer)
            Me.setNumber(T_PADDING, tPadding)
        End Sub

        '/**
        ' * Sets the table paddings for four sides separately (TPadding).
        ' * 
        ' * @param tPaddings the table paddings (an array of four floats)
        ' */
        Public Sub setTPaddings(ByVal tPaddings() As Single)
            Me.setArrayOfNumber(T_PADDING, tPaddings)
        End Sub

        '/**
        ' * Gets the distance by which the element’s baseline shall be shifted
        ' * relative to that of its parent element (BaselineShift). The default value
        ' * is 0.
        ' * 
        ' * @return the baseline shift
        ' */
        Public Function getBaselineShift() As Single
            Return Me.getNumber(BASELINE_SHIFT, 0.0F)
        End Function

        '/**
        ' * Sets the distance by which the element’s baseline shall be shifted
        ' * relative to that of its parent element (BaselineShift).
        ' * 
        ' * @param baselineShift the baseline shift
        ' */
        Public Sub setBaselineShift(ByVal baselineShift As Single)
            Me.setNumber(BASELINE_SHIFT, baselineShift)
        End Sub

        '/**
        ' * Sets the distance by which the element’s baseline shall be shifted
        ' * relative to that of its parent element (BaselineShift).
        ' * 
        ' * @param baselineShift the baseline shift
        ' */
        Public Sub setBaselineShift(ByVal baselineShift As Integer)
            Me.setNumber(BASELINE_SHIFT, baselineShift)
        End Sub

        '/**
        ' * Gets the element’s preferred height in the block-progression direction
        ' * (LineHeight). The default value is {@link #LINE_HEIGHT_NORMAL}.
        ' * 
        ' * @return the line height (a NFloat or a String)
        ' */
        Public Function getLineHeight() As Object
            Return Me.getNumberOrName(LINE_HEIGHT, LINE_HEIGHT_NORMAL)
        End Function

        '/**
        ' * Sets the element’s preferred height in the block-progression direction
        ' * (LineHeight) to {@link #LINE_HEIGHT_NORMAL}.
        ' */
        Public Sub setLineHeightNormal()
            Me.setName(LINE_HEIGHT, LINE_HEIGHT_NORMAL)
        End Sub

        '/**
        ' * Sets the element’s preferred height in the block-progression direction
        ' * (LineHeight) to {@link #LINE_HEIGHT_AUTO}.
        ' */
        Public Sub setLineHeightAuto()
            Me.setName(LINE_HEIGHT, LINE_HEIGHT_AUTO)
        End Sub

        '/**
        ' * Sets the element’s preferred height in the block-progression direction
        ' * (LineHeight).
        ' * 
        ' * @param lineHeight the line height
        ' */
        Public Sub setLineHeight(ByVal lineHeight As Single)
            Me.setNumber(LINE_HEIGHT, lineHeight)
        End Sub

        '/**
        ' * Sets the element’s preferred height in the block-progression direction
        ' * (LineHeight).
        ' * 
        ' * @param lineHeight the line height
        ' */
        Public Sub setLineHeight(ByVal lineHeight As Integer)
            Me.setNumber(LINE_HEIGHT, lineHeight)
        End Sub

        '/**
        ' * Gets the colour to be used for drawing text decorations
        ' * (TextDecorationColor).
        ' * 
        ' * @return the text decoration colour
        ' */
        Public Function getTextDecorationColor() As PDGamma
            Return Me.getColor(TEXT_DECORATION_COLOR)
        End Function

        '/**
        ' * Sets the colour to be used for drawing text decorations
        ' * (TextDecorationColor).
        ' * 
        ' * @param textDecorationColor the text decoration colour
        ' */
        Public Sub setTextDecorationColor(ByVal textDecorationColor As PDGamma)
            Me.setColor(TEXT_DECORATION_COLOR, textDecorationColor)
        End Sub

        '/**
        ' * Gets the thickness of each line drawn as part of the text decoration
        ' * (TextDecorationThickness).
        ' * 
        ' * @return the text decoration thickness
        ' */
        Public Function getTextDecorationThickness() As Single
            Return Me.getNumber(TEXT_DECORATION_THICKNESS)
        End Function

        '/**
        ' * Sets the thickness of each line drawn as part of the text decoration
        ' * (TextDecorationThickness).
        ' * 
        ' * @param textDecorationThickness the text decoration thickness
        ' */
        Public Sub setTextDecorationThickness(ByVal textDecorationThickness As Single)
            Me.setNumber(TEXT_DECORATION_THICKNESS, textDecorationThickness)
        End Sub

        '/**
        ' * Sets the thickness of each line drawn as part of the text decoration
        ' * (TextDecorationThickness).
        ' * 
        ' * @param textDecorationThickness the text decoration thickness
        ' */
        Public Sub setTextDecorationThickness(ByVal textDecorationThickness As Integer)
            Me.setNumber(TEXT_DECORATION_THICKNESS, textDecorationThickness)
        End Sub

        '/**
        ' * Gets the type of text decoration (TextDecorationType). The default value
        ' * is {@link #TEXT_DECORATION_TYPE_NONE}.
        ' * 
        ' * @return the type of text decoration
        ' */
        Public Function getTextDecorationType() As String
            Return Me.getName(TEXT_DECORATION_TYPE, TEXT_DECORATION_TYPE_NONE)
        End Function

        '/**
        ' * Sets the type of text decoration (TextDecorationType). The value should
        ' * be one of:
        ' * <ul>
        ' *   <li>{@link #TEXT_DECORATION_TYPE_NONE},</li>
        ' *   <li>{@link #TEXT_DECORATION_TYPE_UNDERLINE},</li>
        ' *   <li>{@link #TEXT_DECORATION_TYPE_OVERLINE},</li>
        ' *   <li>{@link #TEXT_DECORATION_TYPE_LINE_THROUGH}.</li>
        ' * </ul>
        ' * 
        ' * @param textDecorationType the type of text decoration
        ' */
        Public Sub setTextDecorationType(ByVal textDecorationType As String)
            Me.setName(TEXT_DECORATION_TYPE, textDecorationType)
        End Sub

        '/**
        ' * Gets the justification of the lines within a ruby assembly (RubyAlign).
        ' * The default value is {@link #RUBY_ALIGN_DISTRIBUTE}.
        ' * 
        ' * @return the ruby alignment
        ' */
        Public Function getRubyAlign() As String
            Return Me.getName(RUBY_ALIGN, RUBY_ALIGN_DISTRIBUTE)
        End Function

        '/**
        ' * Sets the justification of the lines within a ruby assembly (RubyAlign).
        ' * The value should be one of:
        ' * <ul>
        ' *   <li>{@link #RUBY_ALIGN_START},</li>
        ' *   <li>{@link #RUBY_ALIGN_CENTER},</li>
        ' *   <li>{@link #RUBY_ALIGN_END},</li>
        ' *   <li>{@link #RUBY_ALIGN_JUSTIFY},</li>
        ' *   <li>{@link #RUBY_ALIGN_DISTRIBUTE},</li>
        ' * </ul>
        ' * 
        ' * @param rubyAlign the ruby alignment
        ' */
        Public Sub setRubyAlign(ByVal rubyAlign As String)
            Me.setName(RUBY_ALIGN, rubyAlign)
        End Sub

        '/**
        ' * Gets the placement of the RT structure element relative to the RB element
        ' * in a ruby assembly (RubyPosition). The default value is
        ' * {@link #RUBY_POSITION_BEFORE}.
        ' * 
        ' * @return the ruby position
        ' */
        Public Function getRubyPosition() As String
            Return Me.getName(RUBY_POSITION, RUBY_POSITION_BEFORE)
        End Function

        '/**
        ' * Sets the placement of the RT structure element relative to the RB element
        ' * in a ruby assembly (RubyPosition). The value should be one of:
        ' * <ul>
        ' *   <li>{@link #RUBY_POSITION_BEFORE},</li>
        ' *   <li>{@link #RUBY_POSITION_AFTER},</li>
        ' *   <li>{@link #RUBY_POSITION_WARICHU},</li>
        ' *   <li>{@link #RUBY_POSITION_INLINE}.</li>
        ' * </ul>
        ' * 
        ' * @param rubyPosition the ruby position
        ' */
        Public Sub setRubyPosition(ByVal rubyPosition As String)
            Me.setName(RUBY_POSITION, rubyPosition)
        End Sub

        '/**
        ' * Gets the orientation of glyphs when the inline-progression direction is
        ' * top to bottom or bottom to top (GlyphOrientationVertical). The default
        ' * value is {@link #GLYPH_ORIENTATION_VERTICAL_AUTO}.
        ' * 
        ' * @return the vertical glyph orientation
        ' */
        Public Function getGlyphOrientationVertical() As String
            Return Me.getName(GLYPH_ORIENTATION_VERTICAL, GLYPH_ORIENTATION_VERTICAL_AUTO)
        End Function

        '/**
        ' * Sets the orientation of glyphs when the inline-progression direction is
        ' * top to bottom or bottom to top (GlyphOrientationVertical). The value
        ' * should be one of:
        ' * <ul>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_AUTO},</li>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_MINUS_180_DEGREES},</li>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_MINUS_90_DEGREES},</li>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_ZERO_DEGREES},</li>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_90_DEGREES},</li>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_180_DEGREES},</li>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_270_DEGREES},</li>
        ' *   <li>{@link #GLYPH_ORIENTATION_VERTICAL_360_DEGREES}.</li>
        ' * </ul>
        ' * 
        ' * @param glyphOrientationVertical the vertical glyph orientation
        ' */
        Public Sub setGlyphOrientationVertical(ByVal glyphOrientationVertical As String)
            Me.setName(GLYPH_ORIENTATION_VERTICAL, glyphOrientationVertical)
        End Sub

        '/**
        ' * Gets the number of columns in the content of the grouping element
        ' * (ColumnCount). The default value is 1.
        ' * 
        ' * @return the column count
        ' */
        Public Function getColumnCount() As Integer
            Return Me.getInteger(COLUMN_COUNT, 1)
        End Function

        '/**
        ' * Sets the number of columns in the content of the grouping element
        ' * (ColumnCount).
        ' * 
        ' * @param columnCount the column count
        ' */
        Public Sub setColumnCount(ByVal columnCount As Integer)
            Me.setInteger(COLUMN_COUNT, columnCount)
        End Sub

        '/**
        ' * Gets the desired space between adjacent columns in the inline-progression
        ' * direction (ColumnGap).
        ' * 
        ' * @return the column gap (FLoat or array of floats)
        ' */
        Public Function getColumnGap() As Object
            Return Me.getNumberOrArrayOfNumber(COLUMN_GAP, UNSPECIFIED)
        End Function

        '/**
        ' * Sets the desired space between all columns in the inline-progression
        ' * direction (ColumnGap).
        ' * 
        ' * @param columnGap the column gap
        ' */
        Public Sub setColumnGap(ByVal columnGap As Single)
            Me.setNumber(COLUMN_GAP, columnGap)
        End Sub

        '/**
        ' * Sets the desired space between all columns in the inline-progression
        ' * direction (ColumnGap).
        ' * 
        ' * @param columnGap the column gap
        ' */
        Public Sub setColumnGap(ByVal columnGap As Integer)
            Me.setNumber(COLUMN_GAP, columnGap)
        End Sub

        '/**
        ' * Sets the desired space between adjacent columns in the inline-progression
        ' * direction (ColumnGap), the first element specifying the space between the
        ' * first and second columns, the second specifying the space between the
        ' * second and third columns, and so on.
        ' * 
        ' * @param columnGaps the column gaps
        ' */
        Public Sub setColumnGaps(ByVal columnGaps() As Single)
            Me.setArrayOfNumber(COLUMN_GAP, columnGaps)
        End Sub

        '/**
        ' * Gets the desired width of the columns, measured in default user space
        ' * units in the inline-progression direction (ColumnWidths).
        ' * 
        ' * @return the column widths (NFloat or array of floats)
        ' */
        Public Function getColumnWidths() As Object
            Return Me.getNumberOrArrayOfNumber(COLUMN_WIDTHS, UNSPECIFIED)
        End Function

        '/**
        ' * Sets the same column width for all columns (ColumnWidths).
        ' * 
        ' * @param columnWidth the column width
        ' */
        Public Sub setAllColumnWidths(ByVal columnWidth As Single)
            Me.setNumber(COLUMN_WIDTHS, columnWidth)
        End Sub

        '/**
        ' * Sets the same column width for all columns (ColumnWidths).
        ' * 
        ' * @param columnWidth the column width
        ' */
        Public Sub setAllColumnWidths(ByVal columnWidth As Integer)
            Me.setNumber(COLUMN_WIDTHS, columnWidth)
        End Sub

        '/**
        ' * Sets the column widths for the columns separately (ColumnWidths).
        ' * 
        ' * @param columnWidths the column widths
        ' */
        Public Sub setColumnWidths(ByVal columnWidths() As Single)
            Me.setArrayOfNumber(COLUMN_WIDTHS, columnWidths)
        End Sub

        Public Overrides Function toString() As String
            Dim sb As New StringBuilder()
            sb.Append(MyBase.toString())
            If (Me.isSpecified(PLACEMENT)) Then
                sb.Append(", Placement=").Append(Me.getPlacement())
            End If
            If (Me.isSpecified(WRITING_MODE)) Then
                sb.Append(", WritingMode=").Append(Me.getWritingMode())
            End If
            If (Me.isSpecified(BACKGROUND_COLOR)) Then
                sb.Append(", BackgroundColor=").Append(Me.getBackgroundColor())
            End If
            If (Me.isSpecified(BORDER_COLOR)) Then
                sb.Append(", BorderColor=").Append(Me.getBorderColors())
            End If
            If (Me.isSpecified(BORDER_STYLE)) Then
                Dim borderStyle As Object = Me.getBorderStyle()
                sb.Append(", BorderStyle=")
                If (TypeOf (borderStyle) Is String()) Then
                    sb.Append(arrayToString(borderStyle))
                Else
                    sb.Append(borderStyle)
                End If
            End If
            If (Me.isSpecified(BORDER_THICKNESS)) Then
                Dim borderThickness As Object = Me.getBorderThickness()
                sb.Append(", BorderThickness=")
                If (TypeOf (borderThickness) Is Single()) Then
                    sb.Append(arrayToString(borderThickness))
                Else
                    sb.Append(CStr(borderThickness))
                End If
            End If
            If (Me.isSpecified(PADDING)) Then
                Dim padding As Object = Me.getPadding()
                sb.Append(", Padding=")
                If (TypeOf (PADDING) Is Single()) Then
                    sb.Append(arrayToString(PADDING))
                Else
                    sb.Append(CStr(PADDING))
                End If
            End If
            If (Me.isSpecified(COLOR)) Then
                sb.Append(", Color=").Append(Me.getColor())
            End If
            If (Me.isSpecified(SPACE_BEFORE)) Then
                sb.Append(", SpaceBefore=").Append(CStr(Me.getSpaceBefore()))
            End If
            If (Me.isSpecified(SPACE_AFTER)) Then
                sb.Append(", SpaceAfter=").Append(CStr(Me.getSpaceAfter()))
            End If
            If (Me.isSpecified(START_INDENT)) Then
                sb.Append(", StartIndent=").Append(CStr(Me.getStartIndent()))
            End If
            If (Me.isSpecified(END_INDENT)) Then
                sb.Append(", EndIndent=").Append(CStr(Me.getEndIndent()))
            End If
            If (Me.isSpecified(TEXT_INDENT)) Then
                sb.Append(", TextIndent=").Append(CStr(Me.getTextIndent()))
            End If
            If (Me.isSpecified(TEXT_ALIGN)) Then
                sb.Append(", TextAlign=").Append(Me.getTextAlign())
            End If
            If (Me.isSpecified(BBOX)) Then
                sb.Append(", BBox=").Append(Me.getBBox())
            End If
            If (Me.isSpecified(WIDTH)) Then
                Dim width As Object = Me.getWidth()
                sb.Append(", Width=")
                If (TypeOf (WIDTH) Is Single) Then
                    sb.Append(CStr(WIDTH))
                Else
                    sb.Append(WIDTH)
                End If
            End If
            If (Me.isSpecified(HEIGHT)) Then
                Dim height As Object = Me.getHeight()
                sb.Append(", Height=")
                If (TypeOf (height) Is Single) Then
                    sb.Append(CStr(height))
                Else
                    sb.Append(height)
                End If
            End If
            If (Me.isSpecified(BLOCK_ALIGN)) Then
                sb.Append(", BlockAlign=").Append(Me.getBlockAlign())
            End If
            If (Me.isSpecified(INLINE_ALIGN)) Then
                sb.Append(", InlineAlign=").Append(Me.getInlineAlign())
            End If
            If (Me.isSpecified(T_BORDER_STYLE)) Then
                Dim tBorderStyle As Object = Me.getTBorderStyle()
                sb.Append(", TBorderStyle=")
                If (TypeOf (tBorderStyle) Is String()) Then
                    sb.Append(arrayToString(tBorderStyle))
                Else
                    sb.Append(tBorderStyle)
                End If
            End If
            If (Me.isSpecified(T_PADDING)) Then
                Dim tPadding As Object = Me.getTPadding()
                sb.Append(", TPadding=")
                If (TypeOf (tPadding) Is Single()) Then
                    sb.Append(arrayToString(tPadding))
                Else
                    sb.Append(CStr(tPadding))
                End If
            End If
            If (Me.isSpecified(BASELINE_SHIFT)) Then
                sb.Append(", BaselineShift=").Append(CStr(Me.getBaselineShift()))
            End If
            If (Me.isSpecified(LINE_HEIGHT)) Then
                Dim lineHeight As Object = Me.getLineHeight()
                sb.Append(", LineHeight=")
                If (TypeOf (lineHeight) Is Single) Then
                    sb.Append(CStr(lineHeight))
                Else
                    sb.Append(lineHeight)
                End If
            End If
            If (Me.isSpecified(TEXT_DECORATION_COLOR)) Then
                sb.Append(", TextDecorationColor=").Append(Me.getTextDecorationColor())
            End If
            If (Me.isSpecified(TEXT_DECORATION_THICKNESS)) Then
                sb.Append(", TextDecorationThickness=").Append(CStr(Me.getTextDecorationThickness()))
            End If
            If (Me.isSpecified(TEXT_DECORATION_TYPE)) Then
                sb.Append(", TextDecorationType=").Append(Me.getTextDecorationType())
            End If
            If (Me.isSpecified(RUBY_ALIGN)) Then
                sb.Append(", RubyAlign=").Append(Me.getRubyAlign())
            End If
            If (Me.isSpecified(RUBY_POSITION)) Then
                sb.Append(", RubyPosition=").Append(Me.getRubyPosition())
            End If
            If (Me.isSpecified(GLYPH_ORIENTATION_VERTICAL)) Then
                sb.Append(", GlyphOrientationVertical=").Append(Me.getGlyphOrientationVertical())
            End If
            If (Me.isSpecified(COLUMN_COUNT)) Then
                sb.Append(", ColumnCount=").Append(CStr(Me.getColumnCount()))
            End If
            If (Me.isSpecified(COLUMN_GAP)) Then
                Dim columnGap As Object = Me.getColumnGap()
                sb.Append(", ColumnGap=")
                If (TypeOf (columnGap) Is Single()) Then
                    sb.Append(arrayToString(columnGap))
                Else
                    sb.Append(CStr(columnGap))
                End If
            End If
            If (Me.isSpecified(COLUMN_WIDTHS)) Then
                Dim columnWidth As Object = Me.getColumnWidths()
                sb.Append(", ColumnWidths=")
                If (TypeOf (columnWidth) Is Single()) Then
                    sb.Append(arrayToString(columnWidth))
                Else
                    sb.Append(CStr(columnWidth))
                End If
            End If
            Return sb.ToString()
        End Function

    End Class

End Namespace
