Imports FinSeA.Drawings
Imports System.Drawing
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.pattern

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents a color space and the color value for that colorspace.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDColorState
        Implements ICloneable

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDColorState.class);

        '/**
        ' * The default color that can be set to replace all colors in {@link ICC_ColorSpace ICC color spaces}.
        ' * 
        ' * @see #setIccOverrideColor(Color)
        ' */
        Private Shared iccOverrideColor As JColor = JColor.getColor(My.Settings.ICC_override_color) ' 'volatile 

        '/**
        ' * Sets the default color to replace all colors in {@link ICC_ColorSpace ICC color spaces}. This will work around a
        ' * potential JVM crash caused by broken native ICC color manipulation code in the Sun class libraries.
        ' * <p>
        ' * The default override can be specified by setting the color code in
        ' * <code>org.apache.pdfbox.ICC_override_color</code> system property (see {@link Color#getColor(String)}. If this
        ' * system property is not specified, then the override is not enabled unless this method is explicitly called.
        ' * 
        ' * @param color ICC override color, or <code>null</code> to disable the override
        ' * @see <a href="https://issues.apache.org/jira/browse/PDFBOX-511">PDFBOX-511</a>
        ' * @since Apache PDFBox 0.8.1
        ' */
        Public Shared Sub setIccOverrideColor(ByVal color As JColor)
            iccOverrideColor = color
        End Sub

        Private colorSpace As PDColorSpace = New PDDeviceGray()
        Private colorSpaceValue As COSArray = New COSArray()
        Private pattern As PDPatternResources = Nothing

        '/**
        ' * Cached Java AWT color based on the current color space and value. The value is cleared whenever the color space
        ' * or value is set.
        ' * 
        ' * @see #getJavaColor()
        ' */
        Private color As JColor = Nothing
        Private paint As Paint = Nothing

        '/**
        ' * Default constructor.
        ' * 
        ' */
        Public Sub New()
            setColorSpaceValue(New Single() {0})
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function clone() As Object Implements ICloneable.Clone
            Dim retval As PDColorState = New PDColorState()
            retval.colorSpace = colorSpace
            retval.colorSpaceValue.clear()
            retval.colorSpaceValue.addAll(colorSpaceValue)
            retval.setPattern(getPattern())
            Return retval
        End Function

        '/**
        ' * Returns the Java AWT color based on the current color space and value.
        ' * 
        ' * @return current Java AWT color
        ' * @throws IOException if the current color can not be created
        ' */
        Public Function getJavaColor() As JColor ' throws IOException
            If (color Is Nothing AndAlso colorSpaceValue.size() > 0) Then
                color = createColor()
            End If
            Return color
        End Function

        '/**
        ' * Returns the Java AWT paint based on the current pattern.
        ' * 
        ' * @param pageHeight the height of the current page
        ' * @return current Java AWT paint
        ' * 
        ' * @throws IOException if the current color can not be created
        ' */
        Public Function getPaint(ByVal pageHeight As Integer) As Paint ' throws IOException
            If (paint Is Nothing AndAlso pattern IsNot Nothing) Then
                paint = pattern.getPaint(pageHeight)
            End If
            Return paint
        End Function

        '/**
        ' * Create the current color from the colorspace and values.
        ' * 
        ' * @return The current awt color.
        ' * @throws IOException If there is an error creating the color.
        ' */
        Private Function createColor() As JColor ' throws IOException
            Dim components() As Single = colorSpaceValue.toFloatArray()
            Try
                Dim csName As String = colorSpace.getName()
                If (PDDeviceRGB.NAME.Equals(csName) AndAlso components.Length = 3) Then
                    ' for some reason, when using RGB and the RGB colorspace
                    ' the new Color doesn't maintain exactly the same values
                    ' I think some color conversion needs to take place first
                    ' for now we will just make rgb a special case.
                    Return New JColor(components(0), components(1), components(2))
                ElseIf (PDLab.NAME.Equals(csName)) Then
                    ' transform the color values from Lab- to RGB-space
                    Dim csComponents() As Single = colorSpace.getJavaColorSpace().toRGB(components)
                    Return New JColor(csComponents(0), csComponents(1), csComponents(2))
                Else
                    If (components.Length = 1) Then
                        If (PDSeparation.NAME.Equals(csName)) Then
                            ' Use that component as a single-integer RGB value
                            Return New JColor(CInt(components(0)))
                        End If
                        If (PDDeviceGray.NAME.Equals(csName)) Then
                            ' Handling DeviceGray as a special case as with JVM 1.5.0_15
                            ' and maybe others printing on Windows fails with an
                            ' ArrayIndexOutOfBoundsException when selecting colors
                            ' and strokes e.g. sun.awt.windows.WPrinterJob.setTextColor
                            Return New JColor(components(0), components(0), components(0))
                        End If
                    End If
                    Dim override As JColor = iccOverrideColor
                    Dim cs As ColorSpace = colorSpace.getJavaColorSpace()
                    If (TypeOf (cs) Is ICC_ColorSpace AndAlso override IsNot Nothing) Then
                        LOG.warn("Using an ICC override color to avoid a potential" + " JVM crash (see PDFBOX-511)")
                        Return override
                    Else
                        Return New JColor(cs, components, 1.0F)
                    End If
                End If

                ' Catch IOExceptions from PDColorSpace.getJavaColorSpace(), but
                ' possibly also IllegalArgumentExceptions or other RuntimeExceptions
                ' from the potentially complex color management code.
            Catch e As Exception
                Dim cGuess As JColor
                Dim sMsg As String = "Unable to create the color instance " & components.ToString & " in color space " & colorSpace.toString & "; guessing color ... "
                Try
                    Select Case (components.Length)
                        Case 1 ' Use that component as a single-integer RGB value
                            cGuess = New JColor(components(0))
                            sMsg &= vbLf & "Interpretating as single-integer RGB"
                        Case 3 ' RGB
                            cGuess = New JColor(components(0), components(1), components(2))
                            sMsg &= vbLf & "Interpretating as RGB"
                        Case 4 ' CMYK
                            ' do a rough conversion to RGB as I'm not getting the CMYK to work.
                            ' http://www.codeproject.com/KB/applications/xcmyk.aspx
                            Dim r, g, b, k As Single
                            k = components(2)

                            r = components(0) * (1.0F - k) + k
                            g = components(1) * (1.0F - k) + k
                            b = components(2) * (1.0F - k) + k

                            r = (1.0F - r)
                            g = (1.0F - g)
                            b = (1.0F - b)

                            cGuess = New JColor(r, g, b)
                            sMsg &= vbLf & "Interpretating as CMYK"
                        Case Else
                            sMsg &= vbLf & "Unable to guess using " & components.Length & " components; using black instead"
                            cGuess = JColor.Black
                    End Select
                Catch e2 As Exception
                    sMsg &= vbLf & "Color interpolation failed; using black instead" & vbLf
                    sMsg &= e2.ToString()
                    cGuess = JColor.Black
                End Try
                LOG.warn(sMsg, e)
                Return cGuess
            End Try
        End Function

        '/**
        ' * Constructor with an existing color set. Default colorspace is PDDeviceGray.
        ' * 
        ' * @param csValues The color space values.
        ' */
        Public Sub New(ByVal csValues As COSArray)
            colorSpaceValue = csValues
        End Sub

        '/**
        ' * This will get the current colorspace.
        ' * 
        ' * @return The current colorspace.
        ' */
        Public Function getColorSpace() As PDColorSpace
            Return colorSpace
        End Function

        '/**
        ' * This will set the current colorspace.
        ' * 
        ' * @param value The new colorspace.
        ' */
        Public Sub setColorSpace(ByVal value As PDColorSpace)
            colorSpace = value
            ' Clear color cache and current pattern
            color = Nothing
            pattern = Nothing
        End Sub

        '/**
        ' * This will get the color space values. Either 1 for gray or 3 for RGB.
        ' * 
        ' * @return The colorspace values.
        ' */
        Public Function getColorSpaceValue() As Single()
            Return colorSpaceValue.toFloatArray()
        End Function

        '/**
        ' * This will get the color space values. Either 1 for gray or 3 for RGB.
        ' * 
        ' * @return The colorspace values.
        ' */
        Public Function getCOSColorSpaceValue() As COSArray
            Return colorSpaceValue
        End Function

        '/**
        ' * This will update the colorspace values.
        ' * 
        ' * @param value The new colorspace values.
        ' */
        Public Sub setColorSpaceValue(ByVal value As Single())
            colorSpaceValue.setFloatArray(value)
            ' Clear color cache and current pattern
            color = Nothing
            pattern = Nothing
        End Sub

        '/**
        ' * This will get the current pattern.
        ' * 
        ' * @return The current pattern.
        ' */
        Public Function getPattern() As PDPatternResources
            Return pattern
        End Function

        '/**
        ' * This will update the current pattern.
        ' * 
        ' * @param patternValue The new pattern.
        ' */
        Public Sub setPattern(ByVal patternValue As PDPatternResources)
            pattern = patternValue
            ' Clear color cache
            color = Nothing
        End Sub

    End Class

End Namespace
