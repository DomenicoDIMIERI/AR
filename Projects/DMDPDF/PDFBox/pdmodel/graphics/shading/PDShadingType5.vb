Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This represents resources for a shading type 5 (Lattice-Form Gouraud-Shaded Triangle Meshes).
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDShadingType5
        Inherits PDShadingResources

        Private [function] As PDFunction = Nothing
        '/**
        ' * An array of 2 × n numbers specifying the linear mapping of sample values 
        ' * into the range appropriate for the function’s output values. 
        ' * Default value: same as the value of Range
        ' */
        Private decode As COSArray = Nothing

        '/**
        ' * Constructor using the given shading dictionary.
        ' *
        ' * @param shadingDictionary The dictionary for this shading.
        ' */
        Public Sub New(ByVal shadingDictionary As COSDictionary)
            MyBase.New(shadingDictionary)
        End Sub

        Public Overrides Function getShadingType() As Integer
            Return PDShadingResources.SHADING_TYPE5
        End Function

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

        '/**
        ' * The bits per component of this shading.  
        ' * This will return -1 if one has not been set.
        ' *
        ' * @return The number of bits per component.
        ' */
        Public Function getBitsPerComponent() As Integer
            Return getCOSDictionary().getInt(COSName.BITS_PER_COMPONENT, -1)
        End Function

        '/**
        ' * Set the number of bits per component.
        ' *
        ' * @param bpc The number of bits per component.
        ' */
        Public Sub setBitsPerComponent(ByVal bpc As Integer)
            getCOSDictionary().setInt(COSName.BITS_PER_COMPONENT, bpc)
        End Sub

        '/**
        ' * The bits per coordinate of this shading.  
        ' * This will return -1 if one has not been set.
        ' *
        ' * @return The number of bits per coordinate.
        ' */
        Public Function getBitsPerCoordinate() As Integer
            Return getCOSDictionary().getInt(COSName.BITS_PER_COORDINATE, -1)
        End Function

        '/**
        ' * Set the number of bits per coordinate.
        ' *
        ' * @param bpc The number of bits per coordinate.
        ' */
        Public Sub setBitsPerCoordinate(ByVal bpc As Integer)
            getCOSDictionary().setInt(COSName.BITS_PER_COORDINATE, bpc)
        End Sub

        '/**
        ' * The vertices per row of this shading.  
        ' * This will return -1 if one has not been set.
        ' *
        ' * @return The number of vertices per row.
        ' */
        Public Function getVerticesPerRow() As Integer
            Return getCOSDictionary().getInt(COSName.VERTICES_PER_ROW, -1)
        End Function

        '/**
        ' * Set the number of vertices per row.
        ' *
        ' * @param vpr The number of vertices per row.
        ' */
        Public Sub setVerticesPerRow(ByVal vpr As Integer)
            getCOSDictionary().setInt(COSName.VERTICES_PER_ROW, vpr)
        End Sub

        '/**
        ' * Returns all decode values as COSArray.
        ' * 
        ' * @return the decode array. 
        ' */
        Private Function getDecodeValues() As COSArray
            If (decode Is Nothing) Then
                decode = getCOSDictionary().getDictionaryObject(COSName.DECODE)
            End If
            Return decode
        End Function

        '/**
        ' * This will set the decode values.
        ' *
        ' * @param decodeValues The new decode values.
        ' */
        Public Sub setDecodeValues(ByVal decodeValues As COSArray)
            decode = decodeValues
            getCOSDictionary().setItem(COSName.DECODE, decodeValues)
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

    End Class

End Namespace