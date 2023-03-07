Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This class represents an appearance for an annotation.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDAppearanceStream
        Implements COSObjectable

        Private stream As COSStream = Nothing

        '/**
        ' * Constructor.
        ' *
        ' * @param s The cos stream for this appearance.
        ' */
        Public Sub New(ByVal s As COSStream)
            stream = s
        End Sub

        '/**
        ' * This will return the underlying stream.
        ' *
        ' * @return The wrapped stream.
        ' */
        Public Function getStream() As COSStream
            Return stream
        End Function

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return stream
        End Function

        '/**
        ' * Get the bounding box for this appearance.  This may return null in which
        ' * case the Rectangle from the annotation should be used.
        ' *
        ' * @return The bounding box for this appearance.
        ' */
        Public Function getBoundingBox() As PDRectangle
            Dim box As PDRectangle = Nothing
            Dim bbox As COSArray = stream.getDictionaryObject(COSName.BBOX)
            If (bbox IsNot Nothing) Then
                box = New PDRectangle(bbox)
            End If
            Return box
        End Function

        '/**
        ' * This will set the bounding box for this appearance stream.
        ' *
        ' * @param rectangle The new bounding box.
        ' */
        Public Sub setBoundingBox(ByVal rectangle As PDRectangle)
            Dim array As COSArray = Nothing
            If (rectangle IsNot Nothing) Then
                array = rectangle.getCOSArray()
            End If
            stream.setItem(COSName.BBOX, array)
        End Sub

        '/**
        ' * This will get the resources for this appearance stream.
        ' *
        ' * @return The appearance stream resources.
        ' */
        Public Function getResources() As PDResources
            Dim retval As PDResources = Nothing
            Dim dict As COSDictionary = stream.getDictionaryObject(COSName.RESOURCES)
            If (dict IsNot Nothing) Then
                retval = New PDResources(dict)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the new resources.
        ' *
        ' * @param resources The new resources.
        ' */
        Public Sub setResources(ByVal resources As PDResources)
            Dim dict As COSDictionary = Nothing
            If (Resources IsNot Nothing) Then
                dict = resources.getCOSDictionary()
            End If
            stream.setItem(COSName.RESOURCES, dict)
        End Sub

        '/**
        ' * Gets the optional matrix for this appearance.  This may return null.
        ' *
        ' * @return The matrix of this appearance.
        ' */
        Public Function getMatrix() As Matrix
            Dim retval As Matrix = Nothing
            Dim array As COSArray = stream.getDictionaryObject(COSName.MATRIX)
            If (Array IsNot Nothing) Then
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
        ' * Sets the optional Matrix entry for this appearance.
        ' * @param transform the transformation matrix
        ' */
        Public Sub setMatrix(ByVal transform As AffineTransform)
            If (transform IsNot Nothing) Then
                Dim matrix As COSArray = New COSArray()
                Dim values(6 - 1) As Double '= new double[6];
                transform.getMatrix(values)
                For Each v As Double In values
                    matrix.add(New COSFloat(CSng(v)))
                Next
                stream.setItem(COSName.MATRIX, matrix)
            Else
                stream.removeItem(COSName.MATRIX)
            End If
        End Sub


    End Class

End Namespace