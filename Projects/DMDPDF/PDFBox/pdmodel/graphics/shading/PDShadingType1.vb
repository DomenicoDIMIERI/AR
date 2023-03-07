Imports FinSeA.Drawings
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.shading


    '/**
    ' * This represents resources for a function based shading.
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDShadingType1
        Inherits PDShadingResources

        Private domain As COSArray = Nothing
        Private [function] As PDFunction = Nothing

        '/**
        ' * Constructor using the given shading dictionary.
        ' *
        ' * @param shadingDictionary The dictionary for this shading.
        ' */
        Public Sub New(ByVal shadingDictionary As COSDictionary)
            MyBase.New(shadingDictionary)
        End Sub

        Public Overrides Function getShadingType() As Integer
            Return PDShadingResources.SHADING_TYPE1
        End Function

        '/**
        ' * This will get the optional Matrix of a function based shading.
        ' * 
        ' * @return the matrix
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
        ' * Sets the optional Matrix entry for the function based shading.
        ' * 
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

        '/**
        ' * This will get the optional Domain values of a function based shading.
        ' * 
        ' * @return the domain values
        ' */
        Public Function getDomain() As COSArray
            If (domain Is Nothing) Then
                domain = getCOSDictionary().getDictionaryObject(COSName.DOMAIN)
            End If
            Return domain
        End Function

        '/**
        ' * Sets the optional Domain entry for the function based shading.
        ' * 
        ' * @param newDomain the domain array
        ' */
        Public Sub setDomain(ByVal newDomain As COSArray)
            domain = newDomain
            getCOSDictionary().setItem(COSName.DOMAIN, newDomain)
        End Sub

        '/**
        ' * This will set the function for the color conversion.
        ' *
        ' * @param newFunction The new function.
        ' */
        Public Sub setFunction(ByVal newFunction As PDFunction)
            [function] = newFunction
            getCOSDictionary().setItem(COSName.FUNCTION, newFunction)
        End Sub

        '/**
        ' * This will return the function used to convert the color values.
        ' *
        ' * @return The function
        ' * @exception IOException If we are unable to create the PDFunction object. 
        ' */
        Public Function getFunction() As PDFunction ' throws IOException
            If ([function] Is Nothing) Then
                [function] = PDFunction.create(getCOSDictionary().getDictionaryObject(COSName.FUNCTION))
            End If
            Return [function]
        End Function

    End Class

End Namespace
