Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.common.function


    '/**
    ' * This class represents a type 2 function in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDFunctionType2
        Inherits PDFunction

        '/**
        ' * The C0 values of the exponential function.
        ' */
        Private C0 As COSArray
        '/**
        ' * The C1 values of the exponential function.
        ' */
        Private C1 As COSArray

        '/**
        ' * Constructor.
        ' *
        ' * @param functionStream The function .
        ' */
        Public Sub New(ByVal [function] As COSBase)
            MyBase.New([function])
        End Sub

        Public Overrides Function getFunctionType() As Integer
            Return 2
        End Function

        Public Overrides Function eval(ByVal input() As Single) As Single() ' throws IOException
            'This function performs exponential interpolation.
            'It uses only a single value as its input, but may produce a multi-valued output.
            'See PDF Reference section 3.9.2.

            Dim inputValue As Double = input(0)
            Dim exponent As Double = getN()
            Dim c0 As COSArray = getC0()
            Dim c1 As COSArray = getC1()
            Dim c0Size As Integer = C0.size()
            Dim functionResult() As Single = Array.CreateInstance(GetType(Single), c0Size)
            For j As Integer = 0 To c0Size - 1
                'y[j] = C0[j] + x^N*(C1[j] - C0[j])
                functionResult(j) = DirectCast(c0.get(j), COSNumber).floatValue() + Math.Pow(inputValue, exponent) * (DirectCast(c1.get(j), COSNumber).floatValue() - DirectCast(c0.get(j), COSNumber).floatValue())
            Next
            ' clip to range if available
            Return clipToRange(functionResult)
        End Function

        '/**
        ' * Returns the C0 values of the function, 0 if empty.
        ' * @return a COSArray with the C0 values
        ' */
        Public Function getC0() As COSArray
            If (C0 Is Nothing) Then
                C0 = getDictionary().getDictionaryObject(COSName.C0)
                If (C0 Is Nothing) Then
                    ' C0 is optional, default = 0
                    C0 = New COSArray()
                    C0.add(New COSFloat(0))
                End If
            End If
            Return C0
        End Function

        '/**
        ' * Returns the C1 values of the function, 1 if empty.
        ' * @return a COSArray with the C1 values
        ' */
        Public Function getC1() As COSArray
            If (C1 Is Nothing) Then
                C1 = getDictionary().getDictionaryObject(COSName.C1)
                If (C1 Is Nothing) Then
                    ' C1 is optional, default = 1
                    C1 = New COSArray()
                    C1.add(New COSFloat(1))
                End If
            End If
            Return C1
        End Function

        '/**
        ' * Returns the exponent of the function.
        ' * @return the Single value of the exponent
        ' */
        Public Function getN() As Single
            Return getDictionary().getFloat(COSName.N)
        End Function

    End Class

End Namespace
