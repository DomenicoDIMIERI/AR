Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This represents the Paint of an radial shading.
    ' * 
    ' * @author lehmi
    ' * @version $Revision: $
    ' * 
    ' */
    Public Class RadialShadingPaint
        Implements Paint

        Private shading As PDShadingType3
        Private currentTransformationMatrix As Matrix
        Private pageHeight As Integer

        '/**
        ' * Constructor.
        ' * 
        ' * @param shadingType3 the shading resources
        ' * @param ctm current transformation matrix
        ' * @param pageSizeValue size of the current page
        ' */
        Public Sub New(ByVal shadingType3 As PDShadingType3, ByVal ctm As Matrix, ByVal pageHeightValue As Integer)
            shading = shadingType3
            currentTransformationMatrix = ctm
            pageHeight = pageHeightValue
        End Sub

        Public Function getTransparency() As Integer
            Return 0
        End Function

        Public Function createContext(ByVal cm As ColorModel, ByVal deviceBounds As Rectangle, ByVal userBounds As RectangleF, ByVal xform As AffineTransform, ByVal hints As RenderingHints) As PaintContext
            Return New RadialShadingContext(shading, cm, xform, currentTransformationMatrix, pageHeight)
        End Function

    End Class

End Namespace
