Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.util.operator

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class AppendRectangleToPath
        Inherits OperatorProcessor


        '/**
        ' * process : re : append rectangle to path.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' */
        Public Overrides Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase))
            Dim drawer As pdfviewer.PageDrawer = context

            Dim x As COSNumber = arguments.get(0)
            Dim y As COSNumber = arguments.get(1)
            Dim w As COSNumber = arguments.get(2)
            Dim h As COSNumber = arguments.get(3)

            Dim x1 As Double = x.doubleValue()
            Dim y1 As Double = y.doubleValue()
            ' create a pair of coordinates for the transformation 
            Dim x2 As Double = w.doubleValue() + x1
            Dim y2 As Double = h.doubleValue() + y1

            Dim startCoords As PointF = drawer.transformedPoint(x1, y1)
            Dim endCoords As PointF = drawer.transformedPoint(x2, y2)

            Dim width As Single = (endCoords.X - startCoords.X)
            Dim height As Single = (endCoords.Y - startCoords.Y)
            Dim xStart As Single = startCoords.X
            Dim yStart As Single = startCoords.Y

            ' To ensure that the path is created in the right direction,
            ' we have to create it by combining single lines instead of
            ' creating a simple rectangle
            Dim path As GeneralPath = drawer.getLinePath()
            path.moveTo(xStart, yStart)
            path.lineTo(xStart + width, yStart)
            path.lineTo(xStart + width, yStart + height)
            path.lineTo(xStart, yStart + height)
            path.lineTo(xStart, yStart)
        End Sub

    End Class

End Namespace
