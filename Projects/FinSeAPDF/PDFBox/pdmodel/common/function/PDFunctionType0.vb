Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.common.function


    '/**
    ' * This class represents a type 0 function in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDFunctionType0
        Inherits PDFunction

        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(PDFunctionType0.class);

        '/**
        ' * An array of 2 x m numbers specifying the linear mapping of input values 
        ' * into the domain of the function's sample table. 
        ' * Default value: [ 0 (Size0 - 1) 0 (Size1 - 1) ...].
        ' */
        Private encode As COSArray = Nothing
        '/**
        ' * An array of 2 x n numbers specifying the linear mapping of sample values 
        ' * into the range appropriate for the function's output values. 
        ' * Default value: same as the value of Range
        ' */
        Private decode As COSArray = Nothing
        '/**
        ' * An array of m positive integers specifying the number of samples in each 
        ' * input dimension of the sample table.
        ' */
        Private size As COSArray = Nothing
        '/**
        ' * The samples of the function.
        ' */
        Private samples(,) As Integer = Nothing

        '/**
        ' * Constructor.
        ' *
        ' * @param functionStream The function .
        ' */
        Public Sub New(ByVal [function] As COSBase)
            MyBase.New([function])
        End Sub

        Public Overrides Function getFunctionType() As Integer
            Return 0
        End Function

        '/**
        ' * The "Size" entry, which is the number of samples in
        ' * each input dimension of the sample table.
        ' *
        ' * @return A List of java.lang.Integer objects.
        ' */
        Public Function getSize() As COSArray
            If (size Is Nothing) Then
                size = getDictionary().getDictionaryObject(COSName.SIZE)
            End If
            Return size
        End Function

        '/**
        ' * Get all sample values of this function.
        ' * 
        ' * @return an array with all samples.
        ' */
        Public Function getSamples() As Integer(,)
            If (samples Is Nothing) Then
                Dim arraySize As Integer = 1
                Dim numberOfInputValues As Integer = getNumberOfInputParameters()
                Dim numberOfOutputValues As Integer = getNumberOfOutputParameters()
                Dim sizes As COSArray = getSize()
                For i As Integer = 0 To numberOfInputValues - 1
                    arraySize *= sizes.getInt(i)
                Next
                samples = Array.CreateInstance(GetType(Integer), arraySize, getNumberOfOutputParameters())
                Dim bitsPerSample As Integer = getBitsPerSample()
                Dim index As Integer = 0
                Dim arrayIndex As Integer = 0
                Try
                    Dim samplesArray() As Byte = getPDStream().getByteArray()
                    For i As Integer = 0 To numberOfInputValues - 1
                        Dim sizeInputValues As Integer = sizes.getInt(i)
                        For j As Integer = 0 To sizeInputValues - 1
                            Dim bitsLeft As Integer = 0
                            Dim bitsToRead As Integer = bitsPerSample
                            Dim currentValue As Integer = 0
                            For k As Integer = 0 To numberOfOutputValues - 1
                                If (bitsLeft = 0) Then
                                    currentValue = (samplesArray(arrayIndex) + 256) Mod 256 : arrayIndex += 1
                                    bitsLeft = 8
                                End If
                                Dim value As Integer = 0
                                While (bitsToRead > 0)
                                    Dim bits As Integer = Math.Min(bitsToRead, bitsLeft)
                                    value = value << bits
                                    Dim valueToAdd As Integer = currentValue >> (8 - bits)
                                    value = value Or valueToAdd
                                    bitsToRead -= bits
                                    bitsLeft -= bits
                                    If (bitsLeft = 0 AndAlso bitsToRead > 0) Then
                                        currentValue = (samplesArray(arrayIndex) + 256) Mod 256 : arrayIndex += 1
                                        bitsLeft = 8
                                    End If
                                End While
                                samples(index, k) = value
                                bitsToRead = bitsPerSample
                            Next
                            index += 1
                        Next
                    Next
                Catch exception As IOException
                    LOG.error("IOException while reading the sample values of this function.")
                End Try
            End If
            Return samples
        End Function

        '/**
        ' * Get the number of bits that the output value will take up.  
        ' * 
        ' * Valid values are 1,2,4,8,12,16,24,32.
        ' *
        ' * @return Number of bits for each output value.
        ' */
        Public Function getBitsPerSample() As Integer
            Return getDictionary().getInt(COSName.BITS_PER_SAMPLE)
        End Function

        '/**
        ' * Set the number of bits that the output value will take up.  Valid values
        ' * are 1,2,4,8,12,16,24,32.
        ' *
        ' * @param bps The number of bits for each output value.
        ' */
        Public Sub setBitsPerSample(ByVal bps As Integer)
            getDictionary().setInt(COSName.BITS_PER_SAMPLE, bps)
        End Sub


        '/**
        ' * Returns all encode values as COSArray.
        ' * 
        ' * @return the encode array. 
        ' */
        Private Function getEncodeValues() As COSArray
            If (encode Is Nothing) Then
                encode = getDictionary().getDictionaryObject(COSName.ENCODE)
                ' the default value is [0 (size(0)-1) 0 (size(1)-1) ...]
                If (encode Is Nothing) Then
                    encode = New COSArray()
                    Dim sizeValues As COSArray = getSize()
                    Dim sizeValuesSize As Integer = sizeValues.size()
                    For i As Integer = 0 To sizeValuesSize - 1
                        encode.add(COSInteger.ZERO)
                        encode.add(COSInteger.get(sizeValues.getInt(i) - 1))
                    Next
                End If
            End If
            Return encode
        End Function

        '/**
        ' * Returns all decode values as COSArray.
        ' * 
        ' * @return the decode array. 
        ' */
        Private Function getDecodeValues() As COSArray
            If (decode Is Nothing) Then
                decode = getDictionary().getDictionaryObject(COSName.DECODE)
                ' if decode is null, the default values are the range values
                If (decode Is Nothing) Then
                    decode = getRangeValues()
                End If
            End If
            Return decode
        End Function

        '/**
        ' * Get the encode for the input parameter.
        ' *
        ' * @param paramNum The function parameter number.
        ' *
        ' * @return The encode parameter range or null if none is set.
        ' */
        Public Function getEncodeForParameter(ByVal paramNum As Integer) As PDRange
            Dim retval As PDRange = Nothing
            Dim encodeValues As COSArray = getEncodeValues()
            If (encodeValues IsNot Nothing AndAlso encodeValues.size() >= paramNum * 2 + 1) Then
                retval = New PDRange(encodeValues, paramNum)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the encode values.
        ' *
        ' * @param range The new encode values.
        ' */
        Public Sub setEncodeValues(ByVal encodeValues As COSArray)
            encode = encodeValues
            getDictionary().setItem(COSName.ENCODE, encodeValues)
        End Sub

        '/**
        ' * Get the decode for the input parameter.
        ' *
        ' * @param paramNum The function parameter number.
        ' *
        ' * @return The decode parameter range or null if none is set.
        ' */
        Public Function getDecodeForParameter(ByVal paramNum As Integer) As PDRange
            Dim retval As PDRange = Nothing
            Dim decodeValues As COSArray = getDecodeValues()
            If (decodeValues IsNot Nothing AndAlso decodeValues.size() >= paramNum * 2 + 1) Then
                retval = New PDRange(decodeValues, paramNum)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the decode values.
        ' *
        ' * @param range The new decode values.
        ' */
        Public Sub setDecodeValues(ByVal decodeValues As COSArray)
            decode = decodeValues
            getDictionary().setItem(COSName.DECODE, decodeValues)
        End Sub

        '/**
        '* {@inheritDoc}
        '*/
        Public Overrides Function eval(ByVal input() As Single) As Single() 'Single()  throws IOException
            'This involves linear interpolation based on a set of sample points.
            'Theoretically it's not that difficult ... see section 3.9.1 of the PDF Reference.
            Dim sizeValues() As Single = getSize().toFloatArray()
            Dim bitsPerSample As Integer = getBitsPerSample()
            Dim numberOfInputValues As Integer = input.Length
            Dim numberOfOutputValues As Integer = getNumberOfOutputParameters()
            Dim intInputValuesPrevious As Integer = 0
            Dim intInputValuesNext As Integer = 0
            For i As Integer = 0 To numberOfInputValues - 1
                Dim domain As PDRange = getDomainForInput(i)
                Dim encode As PDRange = getEncodeForParameter(i)
                input(i) = clipToRange(input(i), domain.getMin(), domain.getMax())
                input(i) = interpolate(input(i), domain.getMin(), domain.getMax(), encode.getMin(), encode.getMax())
                input(i) = clipToRange(input(i), 0, sizeValues(i) - 1)
                intInputValuesPrevious += Math.Floor(input(i))
                intInputValuesNext += Math.Ceiling(input(i))
            Next

            Dim outputValuesPrevious() As Single = Nothing
            Dim outputValuesNext() As Single = Nothing
            outputValuesPrevious = getSample(intInputValuesPrevious)
            outputValuesNext = getSample(intInputValuesNext)
            Dim outputValues() As Single = Array.CreateInstance(GetType(Single), numberOfOutputValues)
            For i As Integer = 0 To numberOfOutputValues
                Dim range As PDRange = getRangeForOutput(i)
                Dim decode As PDRange = getDecodeForParameter(i)
                ' TODO using only a linear interpolation. 
                ' See "Order" entry in table 3.36 of the PDF reference
                outputValues(i) = (outputValuesPrevious(i) + outputValuesNext(i)) / 2
                outputValues(i) = interpolate(outputValues(i), 0, Math.Pow(2, bitsPerSample), decode.getMin(), decode.getMax())
                outputValues(i) = clipToRange(outputValues(i), range.getMin(), range.getMax())
            Next

            Return outputValues
        End Function

        '/**
        ' * Get the samples for the given input values.
        ' * 
        ' * @param indexValue the index into the sample values array
        ' * @return an array with the corresponding samples
        ' */
        Private Function getSample(ByVal indexValue As Integer) As Single()
            Dim sampleValues(,) As Integer = getSamples() '(indexValue)
            Dim numberOfValues As Integer = sampleValues.length
            Dim result() As Single = Array.CreateInstance(GetType(Single), numberOfValues)
            For i As Integer = 0 To numberOfValues - 1
                result(i) = sampleValues(indexValue, i)
            Next
            Return result
        End Function

    End Class

End Namespace
