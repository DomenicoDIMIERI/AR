Imports FinSeA.Drawings
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.pattern
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.shading
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.pattern

    '/**
    ' * This represents the resources for a shading pattern.
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDShadingPatternResources
        Inherits PDPatternResources

        Private extendedGraphicsState As PDExtendedGraphicsState
        Private shading As PDShadingResources
        Private matrix As COSArray = Nothing

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDShadingPatternResources.class);

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getCOSDictionary().setInt(COSName.PATTERN_TYPE, PDPatternResources.SHADING_PATTERN)
        End Sub

        '/**
        ' * Prepopulated pattern resources.
        ' *
        ' * @param resourceDictionary The COSDictionary for this pattern resource.
        ' */
        Public Sub New(ByVal resourceDictionary As COSDictionary)
            MyBase.New(resourceDictionary)
        End Sub

        Public Overrides Function getPatternType() As Integer
            Return PDPatternResources.SHADING_PATTERN
        End Function

        '/**
        ' * This will get the optional Matrix of a Pattern.
        ' * It maps the form space into the user space
        ' * @return the form matrix
        ' */
        Public Function getMatrix() As Matrix
            Dim returnMatrix As Matrix = Nothing
            If (matrix Is Nothing) Then
                matrix = getCOSDictionary().getDictionaryObject(COSName.MATRIX)
            End If
            If (matrix IsNot Nothing) Then
                returnMatrix = New Matrix()
                returnMatrix.setValue(0, 0, DirectCast(matrix.get(0), COSNumber).floatValue())
                returnMatrix.setValue(0, 1, DirectCast(matrix.get(1), COSNumber).floatValue())
                returnMatrix.setValue(1, 0, DirectCast(matrix.get(2), COSNumber).floatValue())
                returnMatrix.setValue(1, 1, DirectCast(matrix.get(3), COSNumber).floatValue())
                returnMatrix.setValue(2, 0, DirectCast(matrix.get(4), COSNumber).floatValue())
                returnMatrix.setValue(2, 1, DirectCast(matrix.get(5), COSNumber).floatValue())
            End If
            Return returnMatrix
        End Function

        '/**
        ' * Sets the optional Matrix entry for the Pattern.
        ' * @param transform the transformation matrix
        ' */
        Public Sub setMatrix(ByVal transform As AffineTransform)
            matrix = New COSArray()
            Dim values(6 - 1) As Double '= new double[6];
            transform.getMatrix(values)
            For Each v As Double In values
                matrix.add(New COSFloat(CSng(v)))
            Next
            getCOSDictionary().setItem(COSName.MATRIX, matrix)
        End Sub

        '/**
        ' * This will get the extended graphics state for this pattern.
        ' *
        ' * @return The extended graphics state for this pattern.
        ' */
        Public Function getExtendedGraphicsState() As PDExtendedGraphicsState
            If (extendedGraphicsState Is Nothing) Then
                Dim dictionary As COSDictionary = getCOSDictionary().getDictionaryObject(COSName.EXT_G_STATE)
                If (dictionary IsNot Nothing) Then
                    extendedGraphicsState = New PDExtendedGraphicsState(dictionary)
                End If
            End If
            Return extendedGraphicsState
        End Function

        '/**
        ' * This will set the extended graphics state for this pattern.
        ' *
        ' * @param extendedGraphicsState The new extended graphics state for this pattern.
        ' */
        Public Sub setExtendedGraphicsState(ByVal extendedGraphicsState As PDExtendedGraphicsState)
            Me.extendedGraphicsState = extendedGraphicsState
            If (extendedGraphicsState IsNot Nothing) Then
                getCOSDictionary().setItem(COSName.EXT_G_STATE, extendedGraphicsState)
            Else
                getCOSDictionary().removeItem(COSName.EXT_G_STATE)
            End If
        End Sub

        '/**
        ' * This will get the shading resources for this pattern.
        ' *
        ' * @return The shading resourcesfor this pattern.
        ' * 
        ' * @throws IOException if something went wrong
        ' */
        Public Function getShading() As PDShadingResources ' throws IOException
            If (shading Is Nothing) Then
                Dim dictionary As COSDictionary = getCOSDictionary().getDictionaryObject(COSName.SHADING)
                If (Dictionary IsNot Nothing) Then
                    shading = PDShadingResources.create(dictionary)
                End If
            End If
            Return shading
        End Function

        '/**
        ' * This will set the shading resources for this pattern.
        ' *
        ' * @param shadingResources The new shading resources for this pattern.
        ' */
        Public Sub setShading(ByVal shadingResources As PDShadingResources)
            shading = shadingResources
            If (shadingResources IsNot Nothing) Then
                getCOSDictionary().setItem(COSName.SHADING, shadingResources)
            Else
                getCOSDictionary().removeItem(COSName.SHADING)
            End If
        End Sub

   
        Public Overrides Function getPaint(ByVal pageHeight As Integer) As Paint 'throws IOException
            Dim paint As Paint = Nothing
            Dim shadingResources As PDShadingResources = getShading()
            Dim shadingType As Integer = 0
            If (shadingResources IsNot Nothing) Then shadingType = shadingResources.getShadingType()

            Select Case (shadingType)
                Case PDShadingResources.SHADING_TYPE2
                    paint = New AxialShadingPaint(getShading(), Nothing, pageHeight)
                Case PDShadingResources.SHADING_TYPE3
                    paint = New RadialShadingPaint(getShading(), Nothing, pageHeight)

                Case PDShadingResources.SHADING_TYPE1, _
                     PDShadingResources.SHADING_TYPE4, _
                     PDShadingResources.SHADING_TYPE5, _
                     PDShadingResources.SHADING_TYPE6, _
                     PDShadingResources.SHADING_TYPE7
                    LOG.debug("Error: Unsupported shading type " & shadingType)
                Case Else
                    Throw New IOException("Error: Unknown shading type " & shadingType)
            End Select
            Return paint
        End Function

    End Class

End Namespace