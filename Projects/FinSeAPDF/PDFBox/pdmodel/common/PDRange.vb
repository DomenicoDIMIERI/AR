Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * This class will be used to signify a range.  a(min) <= a* <= a(max)
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDRange
        Implements COSObjectable

        Private rangeArray As COSArray
        Private startingIndex As Integer

        '/**
        ' * Constructor with an initial range of 0..1.
        ' */
        Public Sub New()
            rangeArray = New COSArray()
            rangeArray.add(New COSFloat(0.0F))
            rangeArray.add(New COSFloat(1.0F))
            startingIndex = 0
        End Sub

        '/**
        ' * Constructor assumes a starting index of 0.
        ' *
        ' * @param range The array that describes the range.
        ' */
        Public Sub New(ByVal range As COSArray)
            rangeArray = range
        End Sub

        '/**
        ' * Constructor with an index into an array.  Because some arrays specify
        ' * multiple ranges ie [ 0,1,  0,2,  2,3 ] It is convenient for this
        ' * class to take an index into an array.  So if you want this range to
        ' * represent 0,2 in the above example then you would say <code>new PDRange( array, 1 )</code>.
        ' *
        ' * @param range The array that describes the index
        ' * @param index The range index into the array for the start of the range.
        ' */
        Public Sub New(ByVal range As COSArray, ByVal index As Integer)
            rangeArray = range
            startingIndex = index
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return rangeArray
        End Function

        '/**
        ' * This will get the underlying array value.
        ' *
        ' * @return The cos object that this object wraps.
        ' */
        Public Function getCOSArray() As COSArray
            Return rangeArray
        End Function

        '/**
        ' * This will get the minimum value of the range.
        ' *
        ' * @return The min value.
        ' */
        Public Function getMin() As Single
            Dim min As COSNumber = rangeArray.getObject(startingIndex * 2)
            Return min.floatValue()
        End Function

        '/**
        ' * This will set the minimum value for the range.
        ' *
        ' * @param min The new minimum for the range.
        ' */
        Public Sub setMin(ByVal min As Single)
            rangeArray.set(startingIndex * 2, New COSFloat(min))
        End Sub

        '/**
        ' * This will get the maximum value of the range.
        ' *
        ' * @return The max value.
        ' */
        Public Function getMax() As Single
            Dim max As COSNumber = rangeArray.getObject(startingIndex * 2 + 1)
            Return max.floatValue()
        End Function

        '/**
        ' * This will set the maximum value for the range.
        ' *
        ' * @param max The new maximum for the range.
        ' */
        Public Sub setMax(ByVal max As Single)
            rangeArray.set(startingIndex * 2 + 1, New COSFloat(max))
        End Sub

    End Class

End Namespace