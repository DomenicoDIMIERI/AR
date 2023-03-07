Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function

'/**
' * This represents resources for an axial shading.
' *
' * @version $Revision: 1.0 $
' */
Namespace org.apache.pdfbox.pdmodel.graphics.shading

    Public Class PDShadingType2
        Inherits PDShadingResources

        Private coords As COSArray = Nothing
        Private domain As COSArray = Nothing
        Private extend As COSArray = Nothing
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
            Return PDShadingResources.SHADING_TYPE2
        End Function

        '/**
        ' * This will get the optional Extend values for this shading.
        ' * 
        ' * @return the extend values
        ' */
        Public Function getExtend() As COSArray
            If (extend Is Nothing) Then
                extend = getCOSDictionary().getDictionaryObject(COSName.EXTEND)
            End If
            Return extend
        End Function

        '/**
        ' * Sets the optional Extend entry for this shading.
        ' * 
        ' * @param newExtend the extend array
        ' */
        Public Sub setExtend(ByVal newExtend As COSArray)
            extend = newExtend
            If (newExtend Is Nothing) Then
                getCOSDictionary().removeItem(COSName.EXTEND)
            Else
                getCOSDictionary().setItem(COSName.EXTEND, newExtend)
            End If
        End Sub

        '/**
        ' * This will get the optional Domain values for this shading.
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
        ' * Sets the optional Domain entry for this shading.
        ' * 
        ' * @param newDomain the domain array
        ' */
        Public Sub setDomain(ByVal newDomain As COSArray)
            domain = newDomain
            If (newDomain Is Nothing) Then
                getCOSDictionary().removeItem(COSName.DOMAIN)
            Else
                getCOSDictionary().setItem(COSName.DOMAIN, newDomain)
            End If
        End Sub

        '/**
        ' * This will get the Coords values for this shading.
        ' * 
        ' * @return the coords values
        ' */
        Public Function getCoords() As COSArray
            If (coords Is Nothing) Then
                coords = getCOSDictionary().getDictionaryObject(COSName.COORDS)
            End If
            Return coords
        End Function

        '/**
        ' * Sets the Coords entry for this shading.
        ' * 
        ' * @param newCoords the coords array
        ' */
        Public Sub setCoords(ByVal newCoords As COSArray)
            coords = newCoords
            If (newCoords Is Nothing) Then
                getCOSDictionary().removeItem(COSName.COORDS)
            Else
                getCOSDictionary().setItem(COSName.COORDS, newCoords)
            End If
        End Sub

        '/**
        ' * This will set the function for the color conversion.
        ' *
        ' * @param newFunction The new function.
        ' */
        Public Sub setFunction(ByVal newFunction As PDFunction)
            [function] = newFunction
            If (newFunction Is Nothing) Then
                getCOSDictionary().removeItem(COSName.FUNCTION)
            Else
                getCOSDictionary().setItem(COSName.FUNCTION, newFunction)
            End If
        End Sub

        '/**
        ' * This will return the function used to convert the color values.
        ' *
        ' * @return The function
        ' * 
        ' * @exception IOException If we are unable to create the PDFunction object. 
        ' * 
        ' */
        Public Function getFunction() As PDFunction ' throws IOException
            If ([function] Is Nothing) Then
                [function] = PDFunction.create(getCOSDictionary().getDictionaryObject(COSName.FUNCTION))
            End If
            Return [function]
        End Function

    End Class

End Namespace
