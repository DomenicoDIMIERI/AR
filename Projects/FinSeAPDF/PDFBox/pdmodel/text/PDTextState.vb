Imports FinSeA.org.apache.pdfbox.pdmodel.font

Namespace org.apache.pdfbox.pdmodel.text

    '/**
    ' * This class will hold the current state of the text parameters when executing a
    ' * content stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDTextState
        Implements ICloneable

        ''' <summary>
        ''' See PDF Reference 1.5 Table 5.3.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RENDERING_MODE_FILL_TEXT = 0
        Public Const RENDERING_MODE_STROKE_TEXT = 1
        Public Const RENDERING_MODE_FILL_THEN_STROKE_TEXT = 2
        Public Const RENDERING_MODE_NEITHER_FILL_NOR_STROKE_TEXT = 3
        Public Const RENDERING_MODE_FILL_TEXT_AND_ADD_TO_PATH_FOR_CLIPPING = 4
        Public Const RENDERING_MODE_STROKE_TEXT_AND_ADD_TO_PATH_FOR_CLIPPING = 5
        Public Const RENDERING_MODE_FILL_THEN_STROKE_TEXT_AND_ADD_TO_PATH_FOR_CLIPPING = 6
        Public Const RENDERING_MODE_ADD_TEXT_TO_PATH_FOR_CLIPPING = 7


        'these are set default according to PDF Reference 1.5 section 5.2
        Private characterSpacing As Single = 0
        Private wordSpacing As Single = 0
        Private horizontalScaling As Single = 100
        Private leading As Single = 0
        Private font As PDFont
        Private fontSize As Single
        Private renderingMode As Integer = 0
        Private rise As Single = 0
        Private knockout As Boolean = True

        '/**
        ' * Get the value of the characterSpacing.
        ' *
        ' * @return The current characterSpacing.
        ' */
        Public Function getCharacterSpacing() As Single
            Return characterSpacing
        End Function

        '/**
        ' * Set the value of the characterSpacing.
        ' *
        ' * @param value The characterSpacing.
        ' */
        Public Sub setCharacterSpacing(ByVal value As Single)
            characterSpacing = value
        End Sub

        '/**
        ' * Get the value of the wordSpacing.
        ' *
        ' * @return The wordSpacing.
        ' */
        Public Function getWordSpacing() As Single
            Return wordSpacing
        End Function

        '/**
        ' * Set the value of the wordSpacing.
        ' *
        ' * @param value The wordSpacing.
        ' */
        Public Sub setWordSpacing(ByVal value As Single)
            wordSpacing = value
        End Sub

        '/**
        ' * Get the value of the horizontalScaling.  The default is 100.  This value
        ' * is the percentage value 0-100 and not 0-1.  So for mathematical operations
        ' * you will probably need to divide by 100 first.
        ' *
        ' * @return The horizontalScaling.
        ' */
        Public Function getHorizontalScalingPercent() As Single
            Return horizontalScaling
        End Function

        '/**
        ' * Set the value of the horizontalScaling.
        ' *
        ' * @param value The horizontalScaling.
        ' */
        Public Sub setHorizontalScalingPercent(ByVal value As Single)
            horizontalScaling = value
        End Sub

        '/**
        ' * Get the value of the leading.
        ' *
        ' * @return The leading.
        ' */
        Public Function getLeading() As Single
            Return leading
        End Function

        '/**
        ' * Set the value of the leading.
        ' *
        ' * @param value The leading.
        ' */
        Public Sub setLeading(ByVal value As Single)
            leading = value
        End Sub

        '/**
        ' * Get the value of the font.
        ' *
        ' * @return The font.
        ' */
        Public Function getFont() As PDFont
            Return font
        End Function

        '/**
        ' * Set the value of the font.
        ' *
        ' * @param value The font.
        ' */
        Public Sub setFont(ByVal value As PDFont)
            font = value
        End Sub

        '/**
        ' * Get the value of the fontSize.
        ' *
        ' * @return The fontSize.
        ' */
        Public Function getFontSize() As Single
            Return fontSize
        End Function

        '/**
        ' * Set the value of the fontSize.
        ' *
        ' * @param value The fontSize.
        ' */
        Public Sub setFontSize(ByVal value As Single)
            fontSize = value
        End Sub

        '/**
        ' * Get the value of the renderingMode.
        ' *
        ' * @return The renderingMode.
        ' */
        Public Function getRenderingMode() As Integer
            Return renderingMode
        End Function

        '/**
        ' * Set the value of the renderingMode.
        ' *
        ' * @param value The renderingMode.
        ' */
        Public Sub setRenderingMode(ByVal value As Integer)
            renderingMode = value
        End Sub

        '/**
        ' * Get the value of the rise.
        ' *
        ' * @return The rise.
        ' */
        Public Function getRise() As Single
            Return rise
        End Function

        '/**
        ' * Set the value of the rise.
        ' *
        ' * @param value The rise.
        ' */
        Public Sub setRise(ByVal value As Single)
            rise = value
        End Sub

        '/**
        ' * Get the value of the knockout.
        ' *
        ' * @return The knockout.
        ' */
        Public Function getKnockoutFlag() As Boolean
            Return knockout
        End Function

        '/**
        ' * Set the value of the knockout.
        ' *
        ' * @param value The knockout.
        ' */
        Public Sub setKnockoutFlag(ByVal value As Boolean)
            knockout = value
        End Sub

        Public Function clone() As Object Implements ICloneable.Clone
            ' Try
            Return MyBase.MemberwiseClone
            'Catch ignore As CloneNotSupportedException
            '    'ignore
            'End Try
            'Return Nothing
        End Function

    End Class

End Namespace
