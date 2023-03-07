Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.common.function


    '/**
    ' * This class represents a type 3 function in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDFunctionType3
        Inherits PDFunction

        Private functions As COSArray = Nothing
        Private encode As COSArray = Nothing
        Private bounds As COSArray = Nothing

        '/**
        ' * Constructor.
        ' *
        ' * @param functionStream The function .
        ' */
        Public Sub New(ByVal functionStream As COSBase)
            MyBase.New(functionStream)
        End Sub

        Public Overrides Function getFunctionType() As Integer
            Return 3
        End Function

        Public Overrides Function eval(ByVal input() As Single) As Single() ' throws IOException
            'This function is known as a "stitching" function. Based on the input, it decides which child function to call.
            ' All functions in the array are 1-value-input functions
            'See PDF Reference section 3.9.3.
            Dim [function] As PDFunction = Nothing
            Dim x As Single = input(0)
            Dim domain As PDRange = getDomainForInput(0)
            ' clip input value to domain
            x = clipToRange(x, domain.getMin(), domain.getMax())

            Dim functionsArray As COSArray = getFunctions()
            Dim numberOfFunctions As Integer = functionsArray.size()
            ' This doesn't make sense but it may happen ...
            If (numberOfFunctions = 1) Then
                [function] = PDFunction.create(functionsArray.get(0))
                Dim encRange As PDRange = getEncodeForParameter(0)
                x = interpolate(x, domain.getMin(), domain.getMax(), encRange.getMin(), encRange.getMax())
            Else
                Dim boundsValues() As Single = getBounds().toFloatArray()
                Dim boundsSize As Integer = boundsValues.length
                ' create a combined array containing the domain and the bounds values
                ' domain.min, bounds(0), bounds(1), ...., bounds[boundsSize-1], domain.max
                Dim partitionValues() As Single = Array.CreateInstance(GetType(Single), boundsSize + 2)
                Dim partitionValuesSize As Integer = partitionValues.length
                partitionValues(0) = domain.getMin()
                partitionValues(partitionValuesSize - 1) = domain.getMax()
                Array.Copy(boundsValues, 0, partitionValues, 1, boundsSize)
                ' find the partition 
                For i As Integer = 0 To partitionValuesSize - 1 - 1
                    If (x >= partitionValues(i) AndAlso (x < partitionValues(i + 1) OrElse (i = partitionValuesSize - 2 AndAlso x = partitionValues(i + 1)))) Then
                        [function] = PDFunction.create(functionsArray.get(i))
                        Dim encRange As PDRange = getEncodeForParameter(i)
                        x = interpolate(x, partitionValues(i), partitionValues(i + 1), encRange.getMin(), encRange.getMax())
                        Exit For
                    End If
                Next
            End If
            Dim functionValues() As Single = {x}
            ' calculate the output values using the chosen function
            Dim functionResult() As Single = [function].eval(functionValues)
            ' clip to range if available
            Return clipToRange(functionResult)
        End Function

        '/**
        ' * Returns all functions values as COSArray.
        ' * 
        ' * @return the functions array. 
        ' */
        Public Function getFunctions() As COSArray
            If (functions Is Nothing) Then
                functions = (getDictionary().getDictionaryObject(COSName.FUNCTIONS))
            End If
            Return functions
        End Function

        '/**
        ' * Returns all bounds values as COSArray.
        ' * 
        ' * @return the bounds array. 
        ' */
        Public Function getBounds() As COSArray
            If (bounds Is Nothing) Then
                bounds = (getDictionary().getDictionaryObject(COSName.BOUNDS))
            End If
            Return bounds
        End Function

        '/**
        ' * Returns all encode values as COSArray.
        ' * 
        ' * @return the encode array. 
        ' */
        Public Function getEncode() As COSArray
            If (encode Is Nothing) Then
                encode = (getDictionary().getDictionaryObject(COSName.ENCODE))
            End If
            Return encode
        End Function

        '/**
        ' * Get the encode for the input parameter.
        ' *
        ' * @param paramNum The function parameter number.
        ' *
        ' * @return The encode parameter range or null if none is set.
        ' */
        Private Function getEncodeForParameter(ByVal n As Integer) As PDRange
            Dim encodeValues As COSArray = getEncode()
            Return New PDRange(encodeValues, n)
        End Function

    End Class

End Namespace