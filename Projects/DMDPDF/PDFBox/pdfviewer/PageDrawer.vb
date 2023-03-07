Imports System.IO

Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.pdmodel
'Imports FinSeA.org.apache.fontbox.util
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.shading
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.pdmodel.text

Namespace org.apache.pdfbox.pdfviewer


    '/**
    ' * This will paint a page in a PDF document to a graphics context.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.22 $
    ' */
    Public Class PageDrawer
        Inherits PDFStreamEngine

        Implements IDisposable

        Private graphics As Graphics2D

        ''' <summary>
        ''' clipping winding rule used for the clipping path.
        ''' </summary>
        ''' <remarks></remarks>
        Private clippingWindingRule As Integer = -1

        ''' <summary>
        ''' Size of the page.
        ''' </summary>
        ''' <remarks></remarks>
        Protected pageSize As System.Drawing.SizeF

        ''' <summary>
        ''' Current page to be rendered.
        ''' </summary>
        ''' <remarks></remarks>
        Protected page As PDPage

        Private linePath As GeneralPath = New GeneralPath()

        '/**
        ' * Default constructor, loads properties from file.
        ' *
        ' * @throws IOException If there is an error loading properties from the file.
        ' */
        Public Sub New() 'throws IOException
            MyBase.New(ResourceLoader.loadProperties("org/apache/pdfbox/resources/PageDrawer.properties", True))
        End Sub

        '/**
        ' * This will draw the page to the requested context.
        ' *
        ' * @param g The graphics context to draw onto.
        ' * @param p The page to draw.
        ' * @param pageDimension The size of the page to draw.
        ' *
        ' * @throws IOException If there is an IO error while drawing the page.
        ' */
        Public Sub drawPage(ByVal g As Graphics2D, ByVal p As PDPage, ByVal pageDimension As System.Drawing.SizeF) 'throws IOException
            graphics = g
            page = p
            pageSize = pageDimension
            graphics.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON)
            graphics.setRenderingHint(RenderingHints.KEY_FRACTIONALMETRICS, RenderingHints.VALUE_FRACTIONALMETRICS_ON)
            ' Only if there is some content, we have to process it. 
            ' Otherwise we are done here and we will produce an empty page
            If (page.getContents() IsNot Nothing) Then
                Dim resources As PDResources = page.findResources()
                processStream(page, resources, page.getContents().getStream())
            End If
            Dim annotations As List(Of PDAnnotation) = page.getAnnotations()
            For i As Integer = 0 To annotations.size() - 1
                Dim annot As PDAnnotation = annotations.get(i)
                Dim rect As PDRectangle = annot.getRectangle()
                Dim appearanceName As String = annot.getAppearanceStream()
                Dim appearDictionary As PDAppearanceDictionary = annot.getAppearance()
                If (appearDictionary IsNot Nothing) Then
                    If (appearanceName IsNot Nothing) Then
                        appearanceName = "default"
                    End If
                    Dim appearanceMap As Map(Of String, PDAppearanceStream) = appearDictionary.getNormalAppearance()
                    If (appearanceMap IsNot Nothing) Then
                        Dim appearance As PDAppearanceStream = appearanceMap.get(appearanceName)
                        If (appearance IsNot Nothing) Then
                            Dim point As New System.Drawing.PointF(rect.getLowerLeftX(), rect.getLowerLeftY())
                            Dim matrix As Matrix = appearance.getMatrix()
                            If (matrix IsNot Nothing) Then
                                ' transform the rectangle using the given matrix 
                                Dim at As AffineTransform = matrix.createAffineTransform()
                                at.transform(point, point)
                            End If
                            g.translate(CInt(point.X), -CInt(point.Y))
                            processSubStream(page, appearance.getResources(), appearance.getStream())
                            g.translate(-CInt(point.X), CInt(point.Y))
                        End If
                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' Remove all cached resources.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub dispose() Implements IDisposable.Dispose
            graphics = Nothing
            linePath = Nothing
            page = Nothing
            pageSize = Nothing
        End Sub

        '/**
        ' * You should override this method if you want to perform an action when a
        ' * text is being processed.
        ' *
        ' * @param text The text to process
        ' */
        Protected Overrides Sub processTextPosition(ByVal text As TextPosition)
            Try
                Dim graphicsState As PDGraphicsState = getGraphicsState()
                Dim composite As Composite
                Dim paint As Paint
                Select Case graphicsState.getTextState().getRenderingMode()
                    Case PDTextState.RENDERING_MODE_FILL_TEXT
                        composite = graphicsState.getNonStrokeJavaComposite()
                        paint = graphicsState.getNonStrokingColor().getJavaColor()
                        If (paint Is Nothing) Then
                            paint = graphicsState.getNonStrokingColor().getPaint(pageSize.Height)
                        End If
                    Case PDTextState.RENDERING_MODE_STROKE_TEXT
                        composite = graphicsState.getStrokeJavaComposite()
                        paint = graphicsState.getStrokingColor().getJavaColor()
                        If (paint Is Nothing) Then
                            paint = graphicsState.getStrokingColor().getPaint(pageSize.Height)
                        End If
                    Case PDTextState.RENDERING_MODE_NEITHER_FILL_NOR_STROKE_TEXT
                        'basic support for text rendering mode "invisible"
                        Dim nsc As JColor = graphicsState.getStrokingColor().getJavaColor()
                        Dim components() As Single = {System.Drawing.Color.Black.R / 255, System.Drawing.Color.Black.G / 255, System.Drawing.Color.Black.B / 255}
                        paint = New JColor(nsc.getColorSpace(), components, 0.0F)
                        composite = graphicsState.getStrokeJavaComposite()
                    Case Else
                        ' TODO : need to implement....
                        LOG.debug("Unsupported RenderingMode " & Me.getGraphicsState().getTextState().getRenderingMode() & " in PageDrawer.processTextPosition(). Using RenderingMode " & PDTextState.RENDERING_MODE_FILL_TEXT & " instead")
                        composite = graphicsState.getNonStrokeJavaComposite()
                        paint = graphicsState.getNonStrokingColor().getJavaColor()
                End Select
                graphics.setComposite(composite)
                graphics.setPaint(paint)

                Dim font As PDFont = text.getFont()
                Dim textPos As Matrix = text.getTextPos().copy()
                Dim x As Single = textPos.getXPosition()
                ' the 0,0-reference has to be moved from the lower left (PDF) to the upper left (AWT-graphics)
                Dim y As Single = pageSize.Height - textPos.getYPosition()
                ' Set translation to 0,0. We only need the scaling and shearing
                textPos.setValue(2, 0, 0)
                textPos.setValue(2, 1, 0)
                ' because of the moved 0,0-reference, we have to shear in the opposite direction
                textPos.setValue(0, 1, (-1) * textPos.getValue(0, 1))
                textPos.setValue(1, 0, (-1) * textPos.getValue(1, 0))
                Dim at As AffineTransform = textPos.createAffineTransform()
                Dim fontMatrix As PDMatrix = font.getFontMatrix()
                at.scale(fontMatrix.getValue(0, 0) * 1000.0F, fontMatrix.getValue(1, 1) * 1000.0F)
                'TODO setClip() is a massive performance hot spot. Investigate optimization possibilities
                graphics.SetClip(graphicsState.getCurrentClippingPath())
                ' the fontSize is no longer needed as it is already part of the transformation
                ' we should remove it from the parameter list in the long run
                font.drawString(text.getCharacter(), text.getCodePoints(), graphics, 1, at, x, y)
            Catch io As IOException
                LOG.debug(io.ToString)
            End Try
        End Sub


        ''' <summary>
        ''' Get the graphics that we are currently drawing on.
        ''' </summary>
        ''' <returns>The graphics we are drawing on.</returns>
        ''' <remarks></remarks>
        Public Function getGraphics() As Graphics2D
            Return graphics
        End Function


        ''' <summary>
        ''' Get the page that is currently being drawn.
        ''' </summary>
        ''' <returns>The page that is being drawn.</returns>
        ''' <remarks></remarks>
        Public Function getPage() As PDPage
            Return page
        End Function


        ''' <summary>
        ''' Get the size of the page that is currently being drawn.
        ''' </summary>
        ''' <returns>The size of the page that is being drawn.</returns>
        ''' <remarks></remarks>
        Public Function getPageSize() As System.Drawing.SizeF 'Dimension 
            Return pageSize
        End Function


        ''' <summary>
        ''' Fix the y coordinate.
        ''' </summary>
        ''' <param name="y">The y coordinate.</param>
        ''' <returns>The updated y coordinate.</returns>
        ''' <remarks></remarks>
        Public Function fixY(ByVal y As Double) As Double
            Return pageSize.Height - y
        End Function

        '/**
        ' * 
        ' *
        ' * @return The current line path to be drawn.
        ' */

        ''' <summary>
        ''' Get the current line path to be drawn.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getLinePath() As GeneralPath
            Return linePath
        End Function


        ''' <summary>
        ''' Set the line path to draw.
        ''' </summary>
        ''' <param name="newLinePath">Set the line path to draw.</param>
        ''' <remarks></remarks>
        Public Sub setLinePath(ByVal newLinePath As GeneralPath)
            If (linePath Is Nothing OrElse linePath.getCurrentPoint().IsEmpty) Then
                linePath = newLinePath
            Else
                linePath.append(newLinePath, False)
            End If
        End Sub


        '/**
        ' * 
        ' *
        ' * @param windingRule 
        ' * 
        ' * @throws 
        ' */

        ''' <summary>
        ''' Fill the path.
        ''' </summary>
        ''' <param name="windingRule">The winding rule this path will use.</param>
        ''' <remarks></remarks>
        ''' <exception cref="IOException">If there is an IO error while filling the path.</exception>
        Public Sub fillPath(ByVal windingRule As Integer) 'throws IOException
            graphics.setComposite(getGraphicsState().getNonStrokeJavaComposite())
            Dim nonStrokingPaint As Paint = getGraphicsState().getNonStrokingColor().getJavaColor()
            If (nonStrokingPaint Is Nothing) Then
                nonStrokingPaint = getGraphicsState().getNonStrokingColor().getPaint(pageSize.Height)
            End If
            If (nonStrokingPaint Is Nothing) Then
                LOG.info("ColorSpace " & getGraphicsState().getNonStrokingColor().getColorSpace().getName() & " doesn't provide a non-stroking color, using white instead!")
                nonStrokingPaint = JColor.White
            End If
            graphics.setPaint(nonStrokingPaint)
            getLinePath().setWindingRule(windingRule)
            graphics.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_OFF)
            graphics.SetClip(getGraphicsState().getCurrentClippingPath())
            graphics.fill(getLinePath())
            getLinePath().reset()
        End Sub


        '/**
        ' * This will set the current stroke.
        ' *
        ' * @param newStroke The current stroke.
        ' * 
        ' */
        Public Sub setStroke(ByVal newStroke As BasicStroke)
            getGraphics().setStroke(newStroke)
        End Sub


        ''' <summary>
        ''' This will return the current stroke.
        ''' </summary>
        ''' <returns>The current stroke.</returns>
        ''' <remarks></remarks>
        Public Function getStroke() As BasicStroke
            Return getGraphics().getStroke()
        End Function

        '/**
        ' * 
        ' *
        ' * @throws 
        ' */

        ''' <summary>
        ''' Stroke the path.
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref="IOException">If there is an IO error while stroking the path.</exception>
        Public Sub strokePath() ' throws IOException
            graphics.setComposite(getGraphicsState().getStrokeJavaComposite())
            Dim strokingPaint As Paint = getGraphicsState().getStrokingColor().getJavaColor()
            If (strokingPaint Is Nothing) Then
                strokingPaint = getGraphicsState().getStrokingColor().getPaint(pageSize.Height)
            End If
            If (strokingPaint Is Nothing) Then
                LOG.info("ColorSpace " & getGraphicsState().getStrokingColor().getColorSpace().getName() & " doesn't provide a stroking color, using white instead!")
                strokingPaint = JColor.White
            End If
            graphics.setPaint(strokingPaint)
            graphics.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_OFF)
            graphics.SetClip(getGraphicsState().getCurrentClippingPath())
            Dim path As GeneralPath = getLinePath()
            graphics.draw(path)
            path.reset()
        End Sub


        ''' <summary>
        ''' Called when the color changed.
        ''' </summary>
        ''' <param name="bStroking">true for the stroking color, false for the non-stroking color</param>
        ''' <remarks></remarks>
        ''' 		<exception cref="IOException">if an I/O error occurs</exception>
        <Obsolete("")> _
        Public Sub colorChanged(ByVal bStroking As Boolean) 'throws IOException
            '//logger().info("changing " + (bStroking ? "" : "non") + "stroking color");
        End Sub

        '//This code generalizes the code Jim Lynch wrote for AppendRectangleToPath
        '/**
        ' * use the current transformation matrix to transform a single point.
        ' * @param x x-coordinate of the point to be transform
        ' * @param y y-coordinate of the point to be transform
        ' * @return the transformed coordinates as Point2D.Double
        ' */
        Public Function transformedPoint(ByVal x As Double, ByVal y As Double) As System.Drawing.PointF 'java.awt.geom.Point2D.Double
            Dim position() As Double = {x, y}
            getGraphicsState().getCurrentTransformationMatrix().createAffineTransform().transform(position, 0, position, 0, 1)
            position(1) = fixY(position(1))
            Return New System.Drawing.PointF(position(0), position(1))
        End Function

        '/**
        ' * Set the clipping Path.
        ' *
        ' * @param windingRule The winding rule this path will use.
        ' * 
        ' * @deprecated use {@link #setClippingWindingRule(int)} instead
        ' * 
        ' */
        Public Sub setClippingPath(ByVal windingRule As Integer)
            setClippingWindingRule(windingRule)
        End Sub

        '/**
        ' * Set the clipping winding rule.
        ' *
        ' * @param windingRule The winding rule which will be used for clipping.
        ' * 
        ' */
        Public Sub setClippingWindingRule(ByVal windingRule As Integer)
            clippingWindingRule = windingRule
        End Sub

        '/**
        ' * Set the clipping Path.
        ' *
        ' */
        Public Sub endPath()
            If (clippingWindingRule > -1) Then
                Dim graphicsState As PDGraphicsState = getGraphicsState()
                Dim clippingPath As GeneralPath = getLinePath().clone()
                clippingPath.setWindingRule(clippingWindingRule)
                ' If there is already set a clipping path, we have to intersect the new with the existing one
                If (graphicsState.getCurrentClippingPath() IsNot Nothing) Then
                    Dim currentArea As New Area(getGraphicsState().getCurrentClippingPath())
                    Dim newArea As New Area(clippingPath)
                    currentArea.intersect(newArea)
                    graphicsState.setCurrentClippingPath(currentArea)
                Else
                    graphicsState.setCurrentClippingPath(clippingPath)
                End If
                clippingWindingRule = -1
            End If
            getLinePath().reset()
        End Sub
        '/**
        ' * Draw the AWT image. Called by Invoke.
        ' * Moved into PageDrawer so that Invoke doesn't have to reach in here for Graphics as that breaks extensibility.
        ' *
        ' * @param awtImage The image to draw.
        ' * @param at The transformation to use when drawing.
        ' * 
        ' */
        Public Sub drawImage(ByVal awtImage As BufferedImage, ByVal at As AffineTransform)
            graphics.setComposite(getGraphicsState().getStrokeJavaComposite())
            graphics.SetClip(getGraphicsState().getCurrentClippingPath())
            graphics.drawImage(awtImage, at, Nothing)
        End Sub

        ''/**
        '' * Fill with Shading.  Called by SHFill operator.
        '' *
        '' * @param ShadingName  The name of the Shading Dictionary to use for this fill instruction.
        '' *
        '' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        '' * 
        '' * @deprecated use {@link #shFill(COSName)) instead.
        '' */
        'Public Sub SHFill(ByVal ShadingName As COSName) 'throws IOException
        '    SHFill(ShadingName)
        'End Sub

        '/**
        ' * Fill with Shading.  Called by SHFill operator.
        ' *
        ' * @param shadingName  The name of the Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the clipping area.
        ' */
        Public Sub shFill(ByVal shadingName As COSName) 'throws IOException
            Dim shading As PDShadingResources = getResources().getShadings().get(shadingName.getName())
            LOG.debug("Shading = " & shading.toString())
            Dim shadingType As Integer = shading.getShadingType()
            Dim ctm As Matrix = getGraphicsState().getCurrentTransformationMatrix()
            Dim paint As Paint = Nothing
            Select Case shadingType
                Case 1
                    ' TODO
                    LOG.debug("Function based shading not yet supported")
                Case 2
                    paint = New AxialShadingPaint(shading, ctm, pageSize.Height)
                Case 3
                    paint = New RadialShadingPaint(shading, ctm, pageSize.Height)
                Case 4, 5, 6, 7
                    ' TODO
                    LOG.debug("Shading type " & shadingType & " not yet supported")
                Case Else
                    Throw New IOException("Invalid ShadingType " & shadingType.ToString & " for Shading " & shadingName.toString)
            End Select
            graphics.setComposite(getGraphicsState().getNonStrokeJavaComposite())
            graphics.setPaint(paint)
            graphics.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_OFF)
            graphics.fill(getGraphicsState().getCurrentClippingPath())
        End Sub

        '/**
        ' * Fill with a Function-based gradient / shading.  
        ' * If extending the class, override this and its siblings, not the public SHFill method.
        ' *
        ' * @param Shading  The Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        ' */
        Protected Sub SHFill_Function(ByVal Shading As PDShading) 'throws IOException
            Throw New NotImplementedException("Not Implemented")
        End Sub

        '/**
        ' * Fill with an Axial Shading.  
        ' * If extending the class, override this and its siblings, not the public SHFill method.
        ' *
        ' * @param Shading  The Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        ' */
        Protected Sub SHFill_Axial(ByVal Shading As PDShading) 'throws IOException
            Throw New NotImplementedException("Not Implemented")
        End Sub

        '/**
        ' * Fill with a Radial gradient / shading.  
        ' * If extending the class, override this and its siblings, not the public SHFill method.
        ' *
        ' * @param Shading  The Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        ' */
        Protected Sub SHFill_Radial(ByVal Shading As PDShading) 'throws IOException
            Throw New NotImplementedException("Not Implemented")
        End Sub

        '/**
        ' * Fill with a Free-form Gourad-shaded triangle mesh.
        ' * If extending the class, override this and its siblings, not the public SHFill method.
        ' *
        ' * @param Shading  The Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        ' */
        Protected Sub SHFill_FreeGourad(ByVal Shading As PDShading) 'throws IOException
            Throw New NotImplementedException("Not Implemented")
        End Sub

        '/**
        ' * Fill with a Lattice-form Gourad-shaded triangle mesh.
        ' * If extending the class, override this and its siblings, not the public SHFill method.
        ' *
        ' * @param Shading  The Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        ' */
        Protected Sub SHFill_LatticeGourad(ByVal Shading As PDShading) 'throws IOException
            Throw New NotImplementedException("Not Implemented")
        End Sub

        '/**
        ' * Fill with a Coons patch mesh
        ' * If extending the class, override this and its siblings, not the public SHFill method.
        ' *
        ' * @param Shading  The Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        ' */
        Protected Sub SHFill_CoonsPatch(ByVal Shading As PDShading) 'throws IOException
            Throw New NotImplementedException("Not Implemented")
        End Sub

        '/**
        ' * Fill with a Tensor-product patch mesh.
        ' * If extending the class, override this and its siblings, not the public SHFill method.
        ' *
        ' * @param Shading  The Shading Dictionary to use for this fill instruction.
        ' *
        ' * @throws IOException If there is an IO error while shade-filling the path/clipping area.
        ' */
        Protected Sub SHFill_TensorPatch(ByVal Shading As PDShading) 'throws IOException
            Throw New IOException("Not Implemented")
        End Sub

       

    End Class

End Namespace
