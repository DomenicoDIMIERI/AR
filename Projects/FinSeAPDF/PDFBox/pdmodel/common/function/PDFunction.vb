Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.common.function


    '/**
    ' * This class represents a function in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public MustInherit Class PDFunction
        Implements COSObjectable

        Private functionStream As PDStream = Nothing
        Private functionDictionary As COSDictionary = Nothing
        Private domain As COSArray = Nothing
        Private range As COSArray = Nothing
        Private numberOfInputValues As Integer = -1
        Private numberOfOutputValues As Integer = -1

        '/**
        ' * Constructor.
        ' *
        ' * @param function The function stream.
        ' * 
        ' */
        Public Sub New(ByVal [function] As COSBase)
            If (TypeOf ([function]) Is COSStream) Then
                functionStream = New PDStream([function])
                functionStream.getStream().setItem(COSName.TYPE, COSName.FUNCTION)
            ElseIf (TypeOf ([function]) Is COSDictionary) Then
                functionDictionary = [function]
            End If
        End Sub

        '/**
        ' * Returns the function type.
        ' * 
        ' * Possible values are:
        ' * 
        ' * 0 - Sampled function
        ' * 2 - Exponential interpolation function
        ' * 3 - Stitching function
        ' * 4 - PostScript calculator function
        ' * 
        ' * @return the function type.
        ' */
        Public MustOverride Function getFunctionType() As Integer

        '/**
        ' * Returns the COSObject.
        ' *
        ' * {@inheritDoc}
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            If (functionStream IsNot Nothing) Then
                Return functionStream.getCOSObject()
            Else
                Return functionDictionary
            End If
        End Function

        '/**
        ' * Returns the stream.
        ' * @return The stream for this object.
        ' */
        Public Function getDictionary() As COSDictionary
            If (functionStream IsNot Nothing) Then
                Return functionStream.getStream()
            Else
                Return functionDictionary
            End If
        End Function

        '/**
        ' * Returns the underlying PDStream.
        ' * @return The stream.
        ' */
        Protected Function getPDStream() As PDStream
            Return functionStream
        End Function

        '/**
        ' * Create the correct PD Model function based on the COS base function.
        ' *
        ' * @param function The COS function dictionary.
        ' *
        ' * @return The PDModel Function object.
        ' *
        ' * @throws IOException If we are unable to create the PDFunction object.
        ' */
        Public Shared Function create(ByVal [function] As COSBase) As PDFunction 'throws IOException
            Dim retval As PDFunction = Nothing
            If (TypeOf ([function]) Is COSObject) Then
                [function] = DirectCast([function], COSObject).getObject()
            End If
            Dim functionDictionary As COSDictionary = [function]
            Dim functionType As Integer = functionDictionary.getInt(COSName.FUNCTION_TYPE)
            If (functionType = 0) Then
                retval = New PDFunctionType0(functionDictionary)
            ElseIf (functionType = 2) Then
                retval = New PDFunctionType2(functionDictionary)
            ElseIf (functionType = 3) Then
                retval = New PDFunctionType3(functionDictionary)
            ElseIf (functionType = 4) Then
                retval = New PDFunctionType4(functionDictionary)
            Else
                Throw New IOException("Error: Unknown function type " & functionType)
            End If
            Return retval
        End Function

        '/**
        ' * This will get the number of output parameters that
        ' * have a range specified.  A range for output parameters
        ' * is optional so this may return zero for a function
        ' * that does have output parameters, this will simply return the
        ' * number that have the rnage specified.
        ' *
        ' * @return The number of input parameters that have a range
        ' * specified.
        ' */
        Public Function getNumberOfOutputParameters() As Integer
            If (numberOfOutputValues = -1) Then
                Dim rangeValues As COSArray = getRangeValues()
                numberOfOutputValues = rangeValues.size() / 2
            End If
            Return numberOfOutputValues
        End Function

        '/**
        ' * This will get the range for a certain output parameters.  This is will never
        ' * return null.  If it is not present then the range 0 to 0 will
        ' * be returned.
        ' *
        ' * @param n The output parameter number to get the range for.
        ' *
        ' * @return The range for this component.
        ' */
        Public Function getRangeForOutput(ByVal n As Integer) As PDRange
            Dim rangeValues As COSArray = getRangeValues()
            Return New PDRange(rangeValues, n)
        End Function

        '/**
        ' * This will set the range values.
        ' *
        ' * @param rangeValues The new range values.
        ' */
        Public Sub setRangeValues(ByVal rangeValues As COSArray)
            range = rangeValues
            getDictionary().setItem(COSName.RANGE, rangeValues)
        End Sub

        '/**
        ' * This will get the number of input parameters that
        ' * have a domain specified.
        ' *
        ' * @return The number of input parameters that have a domain
        ' * specified.
        ' */
        Public Function getNumberOfInputParameters() As Integer
            If (numberOfInputValues = -1) Then
                Dim array As COSArray = getDomainValues()
                numberOfInputValues = array.size() / 2
            End If
            Return numberOfInputValues
        End Function

        '/**
        ' * This will get the range for a certain input parameter.  This is will never
        ' * return null.  If it is not present then the range 0 to 0 will
        ' * be returned.
        ' *
        ' * @param n The parameter number to get the domain for.
        ' *
        ' * @return The domain range for this component.
        ' */
        Public Function getDomainForInput(ByVal n As Integer) As PDRange
            Dim domainValues As COSArray = getDomainValues()
            Return New PDRange(domainValues, n)
        End Function

        '/**
        ' * This will set the domain values.
        ' *
        ' * @param domainValues The new domain values.
        ' */
        Public Sub setDomainValues(ByVal domainValues As COSArray)
            domain = domainValues
            getDictionary().setItem(COSName.DOMAIN, domainValues)
        End Sub


        '/**
        ' * Evaluates the function at the given input.
        ' * ReturnValue = f(input)
        ' *
        ' * @param input The COSArray of input values for the function. 
        ' * In many cases will be an array of a single value, but not always.
        ' * 
        ' * @return The of outputs the function returns based on those inputs. 
        ' * In many cases will be an COSArray of a single value, but not always.
        ' * 
        ' * @throws IOException an IOExcpetion is thrown if something went wrong processing the function.
        ' * 
        ' */
        Public Function eval(ByVal input As COSArray) As COSArray 'throws IOException
            ' TODO should we mark this method as deprecated? 
            Dim outputValues() As Single = eval(input.toFloatArray())
            Dim array As COSArray = New COSArray()
            array.setFloatArray(outputValues)
            Return array
        End Function

        '/**
        ' * Evaluates the function at the given input.
        ' * ReturnValue = f(input)
        ' *
        ' * @param input The array of input values for the function. 
        ' * In many cases will be an array of a single value, but not always.
        ' * 
        ' * @return The of outputs the function returns based on those inputs. 
        ' * In many cases will be an array of a single value, but not always.
        ' * 
        ' * @throws IOException an IOExcpetion is thrown if something went wrong processing the function.  
        ' */
        Public MustOverride Function eval(ByVal input() As Single) As Single() 'Single()  throws IOException;

        '/**
        ' * Returns all ranges for the output values as COSArray .
        ' * Required for type 0 and type 4 functions
        ' * @return the ranges array. 
        ' */
        Protected Function getRangeValues() As COSArray
            If (range Is Nothing) Then
                range = getDictionary().getDictionaryObject(COSName.RANGE)
            End If
            Return range
        End Function

        '/**
        ' * Returns all domains for the input values as COSArray.
        ' * Required for all function types.
        ' * @return the domains array. 
        ' */
        Private Function getDomainValues() As COSArray
            If (domain Is Nothing) Then
                domain = getDictionary().getDictionaryObject(COSName.DOMAIN)
            End If
            Return domain
        End Function

        '/**
        ' * Clip the given input values to the ranges.
        ' * 
        ' * @param inputArray the input values
        ' * @return the clipped values
        ' */
        Protected Function clipToRange(ByVal inputValues() As Single) As Single() 'Single() '
            Dim rangesArray As COSArray = getRangeValues()
            Dim result() As Single = Nothing
            If (rangesArray IsNot Nothing) Then
                Dim rangeValues() As Single = rangesArray.toFloatArray()
                Dim numberOfRanges As Integer = rangeValues.Length / 2
                result = Array.CreateInstance(GetType(Single), numberOfRanges)
                For i As Integer = 0 To numberOfRanges - 1
                    result(i) = clipToRange(inputValues(i), rangeValues(2 * i), rangeValues(2 * i + 1))
                Next
            Else
                result = inputValues
            End If
            Return result
        End Function

        '/**
        ' * Clip the given input value to the given range.
        ' * 
        ' * @param x the input value
        ' * @param rangeMin the min value of the range
        ' * @param rangeMax the max value of the range

        ' * @return the clipped value
        ' */
        Protected Function clipToRange(ByVal x As Single, ByVal rangeMin As Single, ByVal rangeMax As Single) As Single
            Return Math.Min(Math.Max(x, rangeMin), rangeMax)
        End Function

        '/**
        ' * For a given value of x, interpolate calculates the y value 
        ' * on the line defined by the two points (xRangeMin , xRangeMax ) 
        ' * and (yRangeMin , yRangeMax ).
        ' * 
        ' * @param x the to be interpolated value.
        ' * @param xRangeMin the min value of the x range
        ' * @param xRangeMax the max value of the x range
        ' * @param yRangeMin the min value of the y range
        ' * @param yRangeMax the max value of the y range
        ' * @return the interpolated y value
        ' */
        Protected Function interpolate(ByVal x As Single, ByVal xRangeMin As Single, ByVal xRangeMax As Single, ByVal yRangeMin As Single, ByVal yRangeMax As Single) As Single
            Return yRangeMin + ((x - xRangeMin) * (yRangeMax - yRangeMin) / (xRangeMax - xRangeMin))
        End Function

    End Class

End Namespace
