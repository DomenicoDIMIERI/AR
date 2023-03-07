Imports System.Drawing
Imports System.IO
Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.pattern
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.pattern

    '/**
    ' * This represents the resources for a tiling pattern.
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDTilingPatternResources
        Inherits PDPatternResources

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getCOSDictionary().setInt(COSName.PATTERN_TYPE, PDPatternResources.TILING_PATTERN)
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
            Return PDPatternResources.TILING_PATTERN
        End Function

        '/**
        ' * This will set the length of the content stream.
        ' *
        ' * @param length The new stream length.
        ' */
        Public Overrides Sub setLength(ByVal length As Integer)
            getCOSDictionary().setInt(COSName.LENGTH, length)
        End Sub

        '/**
        ' * This will return the length of the content stream.
        ' *
        ' * @return The length of the content stream
        ' */
        Public Overrides Function getLength() As Integer
            Return getCOSDictionary().getInt(COSName.LENGTH, 0)
        End Function

        '/**
        ' * This will set the paint type.
        ' *
        ' * @param paintType The new paint type.
        ' */
        Public Overrides Sub setPaintType(ByVal paintType As Integer)
            getCOSDictionary().setInt(COSName.PAINT_TYPE, paintType)
        End Sub

        '/**
        ' * This will return the paint type.
        ' *
        ' * @return The paint type
        ' */
        Public Function getPaintType() As Integer
            Return getCOSDictionary().getInt(COSName.PAINT_TYPE, 0)
        End Function

        '/**
        ' * This will set the tiling type.
        ' *
        ' * @param tilingType The new tiling type.
        ' */
        Public Sub setTilingType(ByVal tilingType As Integer)
            getCOSDictionary().setInt(COSName.TILING_TYPE, tilingType)
        End Sub

        '/**
        ' * This will return the tiling type.
        ' *
        ' * @return The tiling type
        ' */
        Public Function getTilingType() As Integer
            Return getCOSDictionary().getInt(COSName.TILING_TYPE, 0)
        End Function

        '/**
        ' * This will set the XStep value.
        ' *
        ' * @param xStep The new XStep value.
        ' */
        Public Sub setXStep(ByVal xStep As Integer)
            getCOSDictionary().setInt(COSName.X_STEP, xStep)
        End Sub

        '/**
        ' * This will return the XStep value.
        ' *
        ' * @return The XStep value
        ' */
        Public Function getXStep() As Integer
            Return getCOSDictionary().getInt(COSName.X_STEP, 0)
        End Function

        '/**
        ' * This will set the YStep value.
        ' *
        ' * @param yStep The new YStep value.
        ' */
        Public Sub setYStep(ByVal yStep As Integer)
            getCOSDictionary().setInt(COSName.Y_STEP, yStep)
        End Sub

        '/**
        ' * This will return the YStep value.
        ' *
        ' * @return The YStep value
        ' */
        Public Function getYStep() As Integer
            Return getCOSDictionary().getInt(COSName.Y_STEP, 0)
        End Function

        '/**
        ' * This will get the resources for this pattern.
        ' * This will return null if no resources are available at this level.
        ' *
        ' * @return The resources for this pattern.
        ' */
        Public Function getResources() As PDResources
            Dim retval As PDResources = Nothing
            Dim resources As COSDictionary = getCOSDictionary().getDictionaryObject(COSName.RESOURCES)
            If (resources IsNot Nothing) Then
                retval = New PDResources(resources)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the resources for this pattern.
        ' *
        ' * @param resources The new resources for this pattern.
        ' */
        Public Sub setResources(ByVal resources As PDResources)
            If (resources IsNot Nothing) Then
                getCOSDictionary().setItem(COSName.RESOURCES, resources)
            Else
                getCOSDictionary().removeItem(COSName.RESOURCES)
            End If
        End Sub

        '/**
        ' * An array of four numbers in the form coordinate system (see
        ' * below), giving the coordinates of the left, bottom, right, and top edges,
        ' * respectively, of the pattern's bounding box.
        ' *
        ' * @return The BBox of the form.
        ' */
        Public Function getBBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = getCOSDictionary().getDictionaryObject(COSName.BBOX)
            If (array IsNot Nothing) Then
                retval = New PDRectangle(array)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the BBox (bounding box) for this Pattern.
        ' *
        ' * @param bbox The new BBox for this Pattern.
        ' */
        Public Sub setBBox(ByVal bbox As PDRectangle)
            If (bbox Is Nothing) Then
                getCOSDictionary().removeItem(COSName.BBOX)
            Else
                getCOSDictionary().setItem(COSName.BBOX, bbox.getCOSArray())
            End If
        End Sub

        '/**
        ' * This will get the optional Matrix of a Pattern.
        ' * It maps the form space into the user space
        ' * @return the form matrix
        ' */
        Public Function getMatrix() As Matrix
            Dim retval As Matrix = Nothing
            Dim array As COSArray = getCOSDictionary().getDictionaryObject(COSName.MATRIX)
            If (array IsNot Nothing) Then
                retval = New Matrix()
                retval.setValue(0, 0, DirectCast(array.get(0), COSNumber).floatValue())
                retval.setValue(0, 1, DirectCast(array.get(1), COSNumber).floatValue())
                retval.setValue(1, 0, DirectCast(array.get(2), COSNumber).floatValue())
                retval.setValue(1, 1, DirectCast(array.get(3), COSNumber).floatValue())
                retval.setValue(2, 0, DirectCast(array.get(4), COSNumber).floatValue())
                retval.setValue(2, 1, DirectCast(array.get(5), COSNumber).floatValue())
            End If
            Return retval
        End Function

        '/**
        ' * Sets the optional Matrix entry for the Pattern.
        ' * @param transform the transformation matrix
        ' */
        Public Sub setMatrix(ByVal transform As AffineTransform)
            Dim matrix As COSArray = New COSArray()
            Dim values(6 - 1) As Double '= new double[6];
            transform.getMatrix(values)
            For Each v As Double In values
                matrix.add(New COSFloat(CSng(v)))
            Next
            getCOSDictionary().setItem(COSName.MATRIX, matrix)
        End Sub

        Public Overrides Function getPaint(ByVal pageHeight As Integer) As Paint 'throws IOException
            ' TODO Auto-generated method stub
            Return Nothing
        End Function

    End Class

End Namespace
