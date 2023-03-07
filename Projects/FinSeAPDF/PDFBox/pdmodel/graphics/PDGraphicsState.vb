Imports FinSeA.Drawings
Imports System.Drawing
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.text
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics

    '/**
    ' * This class will hold the current state of the graphics parameters when executing a
    ' * content stream.
    ' *
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDGraphicsState
        Implements ICloneable

        Private currentTransformationMatrix As Matrix = New Matrix()

        'Here are some attributes of the Graphics state, but have not been created yet.
        '
        'clippingPath
        Private strokingColor As PDColorState = New PDColorState()
        Private nonStrokingColor As PDColorState = New PDColorState()
        Private textState As PDTextState = New PDTextState()
        Private lineWidth As Double = 0
        Private lineCap As Integer = 0
        Private lineJoin As Integer = 0
        Private miterLimit As Double = 0
        Private lineDashPattern As PDLineDashPattern
        Private renderingIntent As String
        Private strokeAdjustment As Boolean = False
        'blend mode
        'soft mask
        Private alphaConstants As Double = 1.0
        Private nonStrokingAlphaConstants As Double = 1.0
        Private alphaSource As Boolean = False

        'DEVICE DEPENDENT parameters
        Private overprint As Boolean = False
        Private overprintMode As Double = 0
        'black generation
        'undercolor removal
        'transfer
        'halftone
        Private flatness As Double = 1.0
        Private smoothness As Double = 0

        Private currentClippingPath As GeneralPath

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Constructor with a given pagesize to initialize the clipping path.
        ' * @param page the size of the page
        ' */
        Public Sub New(ByVal page As PDRectangle)
            Dim s As SizeF = page.createDimension()
            currentClippingPath = New GeneralPath(New Rectangle(0, 0, s.Width, s.Height))
            If (page.getLowerLeftX() <> 0 OrElse page.getLowerLeftY() <> 0) Then
                'Compensate for offset
                Me.currentTransformationMatrix = Me.currentTransformationMatrix.multiply(Matrix.getTranslatingInstance(-page.getLowerLeftX(), -page.getLowerLeftY()))
            End If
        End Sub

        '/**
        ' * Get the value of the CTM.
        ' *
        ' * @return The current transformation matrix.
        ' */
        Public Function getCurrentTransformationMatrix() As Matrix
            Return currentTransformationMatrix
        End Function

        '/**
        ' * Set the value of the CTM.
        ' *
        ' * @param value The current transformation matrix.
        ' */
        Public Sub setCurrentTransformationMatrix(ByVal value As Matrix)
            currentTransformationMatrix = value
        End Sub

        '/**
        ' * Get the value of the line width.
        ' *
        ' * @return The current line width.
        ' */
        Public Function getLineWidth() As Double
            Return lineWidth
        End Function

        '/**
        ' * set the value of the line width.
        ' *
        ' * @param value The current line width.
        ' */
        Public Sub setLineWidth(ByVal value As Double)
            lineWidth = value
        End Sub

        '/**
        ' * Get the value of the line cap.
        ' *
        ' * @return The current line cap.
        ' */
        Public Function getLineCap() As Integer
            Return lineCap
        End Function

        '/**
        ' * set the value of the line cap.
        ' *
        ' * @param value The current line cap.
        ' */
        Public Sub setLineCap(ByVal value As Integer)
            lineCap = value
        End Sub

        '/**
        ' * Get the value of the line join.
        ' *
        ' * @return The current line join value.
        ' */
        Public Function getLineJoin() As Integer
            Return lineJoin
        End Function

        '/**
        ' * Get the value of the line join.
        ' *
        ' * @param value The current line join
        ' */
        Public Sub setLineJoin(ByVal value As Integer)
            lineJoin = value
        End Sub

        '/**
        ' * Get the value of the miter limit.
        ' *
        ' * @return The current miter limit.
        ' */
        Public Function getMiterLimit() As Double
            Return miterLimit
        End Function

        '/**
        ' * set the value of the miter limit.
        ' *
        ' * @param value The current miter limit.
        ' */
        Public Sub setMiterLimit(ByVal value As Double)
            miterLimit = value
        End Sub

        '/**
        ' * Get the value of the stroke adjustment parameter.
        ' *
        ' * @return The current stroke adjustment.
        ' */
        Public Function isStrokeAdjustment() As Boolean
            Return strokeAdjustment
        End Function

        '/**
        ' * set the value of the stroke adjustment.
        ' *
        ' * @param value The value of the stroke adjustment parameter.
        ' */
        Public Sub setStrokeAdjustment(ByVal value As Boolean)
            strokeAdjustment = value
        End Sub

        '/**
        ' * Get the value of the stroke alpha constants property.
        ' *
        ' * @return The value of the stroke alpha constants parameter.
        ' */
        Public Function getAlphaConstants() As Double
            Return alphaConstants
        End Function

        '/**
        ' * set the value of the stroke alpha constants property.
        ' *
        ' * @param value The value of the stroke alpha constants parameter.
        ' */
        Public Sub setAlphaConstants(ByVal value As Double)
            alphaConstants = value
        End Sub

        '/**
        ' * Get the value of the non-stroke alpha constants property.
        ' *
        ' * @return The value of the non-stroke alpha constants parameter.
        ' */
        Public Function getNonStrokeAlphaConstants() As Double
            Return nonStrokingAlphaConstants
        End Function

        '/**
        ' * set the value of the non-stroke alpha constants property.
        ' *
        ' * @param value The value of the non-stroke alpha constants parameter.
        ' */
        Public Sub setNonStrokeAlphaConstants(ByVal value As Double)
            nonStrokingAlphaConstants = value
        End Sub

        '/**
        ' * get the value of the stroke alpha source property.
        ' *
        ' * @return The value of the stroke alpha source parameter.
        ' */
        Public Function isAlphaSource() As Boolean
            Return alphaSource
        End Function

        '/**
        ' * set the value of the alpha source property.
        ' *
        ' * @param value The value of the alpha source parameter.
        ' */
        Public Sub setAlphaSource(ByVal value As Boolean)
            alphaSource = value
        End Sub

        '/**
        ' * get the value of the overprint property.
        ' *
        ' * @return The value of the overprint parameter.
        ' */
        Public Function isOverprint() As Boolean
            Return overprint
        End Function

        '/**
        ' * set the value of the overprint property.
        ' *
        ' * @param value The value of the overprint parameter.
        ' */
        Public Sub setOverprint(ByVal value As Boolean)
            overprint = value
        End Sub

        '/**
        ' * get the value of the overprint mode property.
        ' *
        ' * @return The value of the overprint mode parameter.
        ' */
        Public Function getOverprintMode() As Double
            Return overprintMode
        End Function

        '/**
        ' * set the value of the overprint mode property.
        ' *
        ' * @param value The value of the overprint mode parameter.
        ' */
        Public Sub setOverprintMode(ByVal value As Double)
            overprintMode = value
        End Sub

        '/**
        ' * get the value of the flatness property.
        ' *
        ' * @return The value of the flatness parameter.
        ' */
        Public Function getFlatness() As Double
            Return flatness
        End Function

        '/**
        ' * set the value of the flatness property.
        ' *
        ' * @param value The value of the flatness parameter.
        ' */
        Public Sub setFlatness(ByVal value As Double)
            flatness = value
        End Sub

        '/**
        ' * get the value of the smoothness property.
        ' *
        ' * @return The value of the smoothness parameter.
        ' */
        Public Function getSmoothness() As Double
            Return smoothness
        End Function

        '/**
        ' * set the value of the smoothness property.
        ' *
        ' * @param value The value of the smoothness parameter.
        ' */
        Public Sub setSmoothness(ByVal value As Double)
            smoothness = value
        End Sub

        '/**
        ' * This will get the graphics text state.
        ' *
        ' * @return The graphics text state.
        ' */
        Public Function getTextState() As PDTextState
            Return textState
        End Function

        '/**
        ' * This will set the graphics text state.
        ' *
        ' * @param value The graphics text state.
        ' */
        Public Sub setTextState(ByVal value As PDTextState)
            textState = value
        End Sub

        '/**
        ' * This will get the current line dash pattern.
        ' *
        ' * @return The line dash pattern.
        ' */
        Public Function getLineDashPattern() As PDLineDashPattern
            Return lineDashPattern
        End Function

        '/**
        ' * This will set the current line dash pattern.
        ' *
        ' * @param value The new line dash pattern.
        ' */
        Public Sub setLineDashPattern(ByVal value As PDLineDashPattern)
            lineDashPattern = value
        End Sub

        '/**
        ' * This will get the rendering intent.
        ' *
        ' * @see PDExtendedGraphicsState
        ' *
        ' * @return The rendering intent
        ' */
        Public Function getRenderingIntent() As String
            Return renderingIntent
        End Function

        '/**
        ' * This will set the rendering intent.
        ' *
        ' * @param value The new rendering intent.
        ' */
        Public Sub setRenderingIntent(ByVal value As String)
            renderingIntent = value
        End Sub

        Public Function clone() As Object Implements ICloneable.Clone
            Dim ret As PDGraphicsState = Nothing
            'Try
            ret = MyBase.MemberwiseClone
            ret.setTextState(textState.clone())
            ret.setCurrentTransformationMatrix(currentTransformationMatrix.copy())
            ret.strokingColor = strokingColor.clone()
            ret.nonStrokingColor = nonStrokingColor.clone()
            If (lineDashPattern IsNot Nothing) Then
                ret.setLineDashPattern(lineDashPattern.clone())
            End If
            If (currentClippingPath IsNot Nothing) Then
                ret.setCurrentClippingPath(currentClippingPath.clone())
            End If
            'Catch e As CloneNotSupportedException
            '    e.printStackTrace()
            'End Try
            Return ret
        End Function

        '/**
        ' * Returns the stroking color state.
        ' *
        ' * @return stroking color state
        ' */
        Public Function getStrokingColor() As PDColorState
            Return strokingColor
        End Function

        '/**
        ' * Returns the non-stroking color state.
        ' *
        ' * @return non-stroking color state
        ' */
        Public Function getNonStrokingColor() As PDColorState
            Return nonStrokingColor
        End Function

        '/**
        ' * This will set the current clipping path.
        ' *
        ' * @param pCurrentClippingPath The current clipping path.
        ' *
        ' */
        Public Sub setCurrentClippingPath(ByVal pCurrentClippingPath As Shape)
            If (pCurrentClippingPath IsNot Nothing) Then
                If (TypeOf (pCurrentClippingPath) Is GeneralPath) Then
                    currentClippingPath = pCurrentClippingPath
                Else
                    currentClippingPath = New GeneralPath()
                    currentClippingPath.append(pCurrentClippingPath, False)
                End If
            Else
                currentClippingPath = Nothing
            End If
        End Sub

        '/**
        ' * This will get the current clipping path.
        ' *
        ' * @return The current clipping path.
        ' */
        Public Function getCurrentClippingPath() As Shape
            Return currentClippingPath
        End Function

        Public Function getStrokeJavaComposite() As Composite
            Return AlphaComposite.getInstance(AlphaComposite.SRC_OVER, alphaConstants)
        End Function

        Public Function getNonStrokeJavaComposite() As Composite
            Return AlphaComposite.getInstance(AlphaComposite.SRC_OVER, nonStrokingAlphaConstants)
        End Function

    End Class

End Namespace
