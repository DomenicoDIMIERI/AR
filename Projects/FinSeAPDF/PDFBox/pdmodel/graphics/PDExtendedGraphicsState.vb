Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics

    '/**
    ' * This class represents the graphics state dictionary that is stored in the PDF document.
    ' * The PDGraphicsStateValue holds the current runtime values as a stream is being executed.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDExtendedGraphicsState
        Implements COSObjectable

        ''' <summary>
        ''' Rendering intent constants, see PDF Reference 1.5 Section 4.5.4 Rendering Intents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RENDERING_INTENT_ABSOLUTE_COLORIMETRIC As String = "AbsoluteColorimetric"

        ''' <summary>
        ''' Rendering intent constants, see PDF Reference 1.5 Section 4.5.4 Rendering Intents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RENDERING_INTENT_RELATIVE_COLORIMETRIC As String = "RelativeColorimetric"

        ''' <summary>
        ''' Rendering intent constants, see PDF Reference 1.5 Section 4.5.4 Rendering Intents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RENDERING_INTENT_SATURATION As String = "Saturation"

        ''' <summary>
        ''' Rendering intent constants, see PDF Reference 1.5 Section 4.5.4 Rendering Intents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RENDERING_INTENT_PERCEPTUAL As String = "Perceptual"


        Private graphicsState As COSDictionary

        '/**
        ' * Default constructor, creates blank graphics state.
        ' */
        Public Sub New()
            graphicsState = New COSDictionary()
            graphicsState.setItem(COSName.TYPE, COSName.EXT_G_STATE)
        End Sub

        '/**
        ' * Create a graphics state from an existing dictionary.
        ' *
        ' * @param dictionary The existing graphics state.
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            graphicsState = dictionary
        End Sub

        '/**
        ' * This will implement the gs operator.
        ' *
        ' * @param gs The state to copy this dictionaries values into.
        ' *
        ' * @throws IOException If there is an error copying font information.
        ' */
        Public Sub copyIntoGraphicsState(ByVal gs As PDGraphicsState)  'throws IOException
            For Each key As COSName In graphicsState.keySet()
                If (key.equals(COSName.LW)) Then
                    gs.setLineWidth(getLineWidth().doubleValue())
                ElseIf (key.equals(COSName.LC)) Then
                    gs.setLineCap(getLineCapStyle())
                ElseIf (key.equals(COSName.LJ)) Then
                    gs.setLineJoin(getLineJoinStyle())
                ElseIf (key.equals(COSName.ML)) Then
                    gs.setMiterLimit(getMiterLimit().doubleValue())
                ElseIf (key.equals(COSName.D)) Then
                    gs.setLineDashPattern(getLineDashPattern())
                ElseIf (key.equals(COSName.RI)) Then
                    gs.setRenderingIntent(getRenderingIntent())
                ElseIf (key.equals(COSName.OPM)) Then
                    gs.setOverprintMode(getOverprintMode().doubleValue())
                ElseIf (key.equals(COSName.FONT)) Then
                    Dim setting As PDFontSetting = getFontSetting()
                    gs.getTextState().setFont(setting.getFont())
                    gs.getTextState().setFontSize(setting.getFontSize())
                ElseIf (key.equals(COSName.FL)) Then
                    gs.setFlatness(getFlatnessTolerance().floatValue())
                ElseIf (key.equals(COSName.SM)) Then
                    gs.setSmoothness(getSmoothnessTolerance().floatValue())
                ElseIf (key.equals(COSName.SA)) Then
                    gs.setStrokeAdjustment(getAutomaticStrokeAdjustment())
                ElseIf (key.equals(COSName.CA)) Then
                    gs.setAlphaConstants(getStrokingAlpaConstant().floatValue())
                ElseIf (key.equals(COSName.CA_NS)) Then
                    gs.setNonStrokeAlphaConstants(getNonStrokingAlpaConstant().floatValue())
                ElseIf (key.equals(COSName.AIS)) Then
                    gs.setAlphaSource(getAlphaSourceFlag())
                ElseIf (key.equals(COSName.TK)) Then
                    gs.getTextState().setKnockoutFlag(getTextKnockoutFlag())
                End If
            Next
        End Sub

        '/**
        ' * This will get the underlying dictionary that this class acts on.
        ' *
        ' * @return The underlying dictionary for this class.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return graphicsState
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return graphicsState
        End Function

        '/**
        ' * This will get the line width.  This will return null if there is no line width
        ' *
        ' * @return null or the LW value of the dictionary.
        ' */
        Public Function getLineWidth() As NFloat
            Return getFloatItem(COSName.LW)
        End Function

        '/**
        ' * This will set the line width.
        ' *
        ' * @param width The line width for the object.
        ' */
        Public Sub setLineWidth(ByVal width As NFloat)
            setFloatItem(COSName.LW, width)
        End Sub

        '/**
        ' * This will get the line cap style.
        ' *
        ' * @return null or the LC value of the dictionary.
        ' */
        Public Function getLineCapStyle() As Integer
            Return graphicsState.getInt(COSName.LC)
        End Function

        '/**
        ' * This will set the line cap style for the graphics state.
        ' *
        ' * @param style The new line cap style to set.
        ' */
        Public Sub setLineCapStyle(ByVal style As Integer)
            graphicsState.setInt(COSName.LC, style)
        End Sub

        '/**
        ' * This will get the line join style.
        ' *
        ' * @return null or the LJ value in the dictionary.
        ' */
        Public Function getLineJoinStyle() As Integer
            Return graphicsState.getInt(COSName.LJ)
        End Function

        '/**
        ' * This will set the line join style.
        ' *
        ' * @param style The new line join style.
        ' */
        Public Sub setLineJoinStyle(ByVal style As Integer)
            graphicsState.setInt(COSName.LJ, style)
        End Sub


        '/**
        ' * This will get the miter limit.
        ' *
        ' * @return null or the ML value in the dictionary.
        ' */
        Public Function getMiterLimit() As NFloat
            Return getFloatItem(COSName.ML)
        End Function

        '/**
        ' * This will set the miter limit for the graphics state.
        ' *
        ' * @param miterLimit The new miter limit value
        ' */
        Public Sub setMiterLimit(ByVal miterLimit As NFloat)
            setFloatItem(COSName.ML, miterLimit)
        End Sub

        '/**
        ' * This will get the dash pattern.
        ' *
        ' * @return null or the D value in the dictionary.
        ' */
        Public Function getLineDashPattern() As PDLineDashPattern
            Dim retval As PDLineDashPattern = Nothing
            Dim dp As COSArray = graphicsState.getDictionaryObject(COSName.D)
            If (dp IsNot Nothing) Then
                retval = New PDLineDashPattern(dp)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the dash pattern for the graphics state.
        ' *
        ' * @param dashPattern The dash pattern
        ' */
        Public Sub setLineDashPattern(ByVal dashPattern As PDLineDashPattern)
            graphicsState.setItem(COSName.D, dashPattern.getCOSObject())
        End Sub

        '/**
        ' * This will get the rendering intent.
        ' *
        ' * @return null or the RI value in the dictionary.
        ' */
        Public Function getRenderingIntent() As String
            Return graphicsState.getNameAsString("RI")
        End Function

        '/**
        ' * This will set the rendering intent for the graphics state.
        ' *
        ' * @param ri The new rendering intent
        ' */
        Public Sub setRenderingIntent(ByVal ri As String)
            graphicsState.setName("RI", ri)
        End Sub

        '/**
        ' * This will get the overprint control.
        ' *
        ' * @return The overprint control or null if one has not been set.
        ' */
        Public Function getStrokingOverprintControl() As Boolean
            Return graphicsState.getBoolean(COSName.OP, False)
        End Function

        '/**
        ' * This will get the overprint control(OP).
        ' *
        ' * @param op The overprint control.
        ' */
        Public Sub setStrokingOverprintControl(ByVal op As Boolean)
            graphicsState.setBoolean(COSName.OP, op)
        End Sub

        '/**
        ' * This will get the overprint control for non stroking operations.  If this
        ' * value is null then the regular overprint control value will be returned.
        ' *
        ' * @return The overprint control or null if one has not been set.
        ' */
        Public Function getNonStrokingOverprintControl() As Boolean
            Return graphicsState.getBoolean(COSName.OP_NS, getStrokingOverprintControl())
        End Function

        '/**
        ' * This will get the overprint control(OP).
        ' *
        ' * @param op The overprint control.
        ' */
        Public Sub setNonStrokingOverprintControl(ByVal op As Boolean)
            graphicsState.setBoolean(COSName.OP_NS, op)
        End Sub

        '/**
        ' * This will get the overprint control mode.
        ' *
        ' * @return The overprint control mode or null if one has not been set.
        ' */
        Public Function getOverprintMode() As NFloat
            Return getFloatItem(COSName.OPM)
        End Function

        '/**
        ' * This will get the overprint mode(OPM).
        ' *
        ' * @param overprintMode The overprint mode
        ' */
        Public Sub setOverprintMode(ByVal overprintMode As NFloat)
            setFloatItem(COSName.OPM, overprintMode)
        End Sub

        '/**
        ' * This will get the font setting of the graphics state.
        ' *
        ' * @return The font setting.
        ' */
        Public Function getFontSetting() As PDFontSetting
            Dim setting As PDFontSetting = Nothing
            Dim font As COSArray = graphicsState.getDictionaryObject(COSName.FONT)
            If (font IsNot Nothing) Then
                setting = New PDFontSetting(font)
            End If
            Return setting
        End Function

        '/**
        ' * This will set the font setting for this graphics state.
        ' *
        ' * @param fs The new font setting.
        ' */
        Public Sub setFontSetting(ByVal fs As PDFontSetting)
            graphicsState.setItem(COSName.FONT, fs)
        End Sub

        '/**
        ' * This will get the flatness tolerance.
        ' *
        ' * @return The flatness tolerance or null if one has not been set.
        ' */
        Public Function getFlatnessTolerance() As NFloat
            Return getFloatItem(COSName.FL)
        End Function

        '/**
        ' * This will get the flatness tolerance.
        ' *
        ' * @param flatness The new flatness tolerance
        ' */
        Public Sub setFlatnessTolerance(ByVal flatness As NFloat)
            setFloatItem(COSName.FL, flatness)
        End Sub

        '/**
        ' * This will get the smothness tolerance.
        ' *
        ' * @return The smothness tolerance or null if one has not been set.
        ' */
        Public Function getSmoothnessTolerance() As NFloat
            Return getFloatItem(COSName.SM)
        End Function

        '/**
        ' * This will get the smoothness tolerance.
        ' *
        ' * @param smoothness The new smoothness tolerance
        ' */
        Public Sub setSmoothnessTolerance(ByVal smoothness As NFloat)
            setFloatItem(COSName.SM, smoothness)
        End Sub

        '/**
        ' * This will get the automatic stroke adjustment flag.
        ' *
        ' * @return The automatic stroke adjustment flag or null if one has not been set.
        ' */
        Public Function getAutomaticStrokeAdjustment() As Boolean
            Return graphicsState.getBoolean(COSName.SA, False)
        End Function

        '/**
        ' * This will get the automatic stroke adjustment flag.
        ' *
        ' * @param sa The new automatic stroke adjustment flag.
        ' */
        Public Sub setAutomaticStrokeAdjustment(ByVal sa As Boolean)
            graphicsState.setBoolean(COSName.SA, sa)
        End Sub

        '/**
        ' * This will get the stroking alpha constant.
        ' *
        ' * @return The stroking alpha constant or null if one has not been set.
        ' */
        Public Function getStrokingAlpaConstant() As NFloat
            Return getFloatItem(COSName.CA)
        End Function

        '/**
        ' * This will get the stroking alpha constant.
        ' *
        ' * @param alpha The new stroking alpha constant.
        ' */
        Public Sub setStrokingAlphaConstant(ByVal alpha As NFloat)
            setFloatItem(COSName.CA, alpha)
        End Sub

        '/**
        ' * This will get the non stroking alpha constant.
        ' *
        ' * @return The non stroking alpha constant or null if one has not been set.
        ' */
        Public Function getNonStrokingAlpaConstant() As NFloat
            Return getFloatItem(COSName.CA_NS)
        End Function

        '/**
        ' * This will get the non stroking alpha constant.
        ' *
        ' * @param alpha The new non stroking alpha constant.
        ' */
        Public Sub setNonStrokingAlphaConstant(ByVal alpha As NFloat)
            setFloatItem(COSName.CA_NS, alpha)
        End Sub

        '/**
        ' * This will get the alpha source flag.
        ' *
        ' * @return The alpha source flag.
        ' */
        Public Function getAlphaSourceFlag() As Boolean
            Return graphicsState.getBoolean(COSName.AIS, False)
        End Function

        '/**
        ' * This will get the alpha source flag.
        ' *
        ' * @param alpha The alpha source flag.
        ' */
        Public Sub setAlphaSourceFlag(ByVal alpha As Boolean)
            graphicsState.setBoolean(COSName.AIS, alpha)
        End Sub

        '/**
        ' * This will get the text knockout flag.
        ' *
        ' * @return The text knockout flag.
        ' */
        Public Function getTextKnockoutFlag() As Boolean
            Return graphicsState.getBoolean(COSName.TK, True)
        End Function

        '/**
        ' * This will get the text knockout flag.
        ' *
        ' * @param tk The text knockout flag.
        ' */
        Public Sub setTextKnockoutFlag(ByVal tk As Boolean)
            graphicsState.setBoolean(COSName.TK, tk)
        End Sub

        '/**
        ' * This will get a Single item from the dictionary.
        ' *
        ' * @param key The key to the item.
        ' *
        ' * @return The value for that item.
        ' */
        Private Function getFloatItem(ByVal key As COSName) As NFloat
            Dim retval As NFloat = Nothing
            Dim value As COSNumber = graphicsState.getDictionaryObject(key)
            If (value IsNot Nothing) Then
                retval = value.floatValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will set a Single object.
        ' *
        ' * @param key The key to the data that we are setting.
        ' * @param value The value that we are setting.
        ' */
        Private Sub setFloatItem(ByVal key As COSName, ByVal value As NFloat)
            If (value.HasValue = False) Then
                graphicsState.removeItem(key)
            Else
                graphicsState.setItem(key, New COSFloat(value.Value))
            End If
        End Sub


    End Class


End Namespace
