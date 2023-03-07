Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents an Indexed color space.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDIndexed
        Inherits PDColorSpace

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "Indexed"

        ''' <summary>
        ''' The abbreviated name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ABBREVIATED_NAME As String = "I"

        ' Private array As COSArray

        Private baseColorspace As PDColorSpace = Nothing
        Private baseColorModel As ColorModel = Nothing

        ''' <summary>
        ''' The lookup data as byte array.
        ''' </summary>
        ''' <remarks></remarks>
        Private lookupData() As Byte

        Private indexedColorValues As Byte()
        Private indexNumOfComponents As Integer
        Private maxIndex As Integer

        ''' <summary>
        ''' Indexed color values are always 8bit based.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INDEXED_BPC As Integer = 8

        '/**
        ' * Constructor, default DeviceRGB, hival 255.
        ' */
        Public Sub New()
            array = New COSArray()
            array.add(COSName.INDEXED)
            array.add(COSName.DEVICERGB)
            array.add(COSInteger.get(255))
            array.add(org.apache.pdfbox.cos.COSNull.NULL)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param indexedArray The array containing the indexed parameters
        ' */
        Public Sub New(ByVal indexedArray As COSArray)
            array = indexedArray
        End Sub

        '/**
        ' * This will return the number of color components. This will return the number of color components in the base
        ' * color.
        ' * 
        ' * @return The number of components in this color space.
        ' * 
        ' * @throws IOException If there is an error getting the number of color components.
        ' */
        Public Overrides Function getNumberOfComponents() As Integer ' throws IOException
            Return getBaseColorSpace().getNumberOfComponents()
        End Function

        '/**
        ' * This will return the name of the color space.
        ' * 
        ' * @return The name of the color space.
        ' */
        Public Overrides Function getName() As String
            Return NAME
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' * 
        ' * @return A color space that can be used for Java AWT operations.
        ' * 
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace ' throws IOException
            Return getBaseColorSpace().getJavaColorSpace()
        End Function

        '/**
        ' * Create a Java color model for this colorspace.
        ' * 
        ' * @param bpc The number of bits per component.
        ' * 
        ' * @return A color model that can be used for Java AWT operations.
        ' * 
        ' * @throws IOException If there is an error creating the color model.
        ' */
        Public Overrides Function createColorModel(ByVal bpc As Integer) As ColorModel ' throws IOException
            Return createColorModel(bpc, -1)
        End Function

        '/**
        ' * Create a Java color model for this colorspace including the given mask value.
        ' * 
        ' * @param bpc The number of bits per component of the indexed color model.
        ' * @param mask the mask value, -1 indicates no mask
        ' * 
        ' * @return A color model that can be used for Java AWT operations.
        ' * 
        ' * @throws IOException If there is an error creating the color model.
        ' */
        Public Overloads Function createColorModel(ByVal bpc As Integer, ByVal mask As Integer) As ColorModel ' throws IOException
            Dim colorModel As ColorModel = getBaseColorModel(INDEXED_BPC)
            calculateIndexedColorValues(colorModel, bpc)
            If (mask > -1) Then
                Return New IndexColorModel(bpc, maxIndex + 1, indexedColorValues, 0, colorModel.hasAlpha(), mask)
            Else
                Return New IndexColorModel(bpc, maxIndex + 1, indexedColorValues, 0, colorModel.hasAlpha())
            End If
        End Function

        '/**
        ' * This will get the color space that acts as the index for this color space.
        ' * 
        ' * @return The base color space.
        ' * 
        ' * @throws IOException If there is error creating the base color space.
        ' */
        Public Function getBaseColorSpace() As PDColorSpace ' throws IOException
            If (baseColorspace Is Nothing) Then
                Dim base As COSBase = array.getObject(1)
                baseColorspace = PDColorSpaceFactory.createColorSpace(base)
            End If
            Return baseColorspace
        End Function

        '/**
        ' * This will set the base color space.
        ' * 
        ' * @param base The base color space to use as the index.
        ' */
        Public Sub setBaseColorSpace(ByVal base As PDColorSpace)
            array.set(1, base.getCOSObject())
            baseColorspace = base
        End Sub

        '/**
        ' * Get the highest value for the lookup.
        ' * 
        ' * @return The hival entry.
        ' */
        Public Function getHighValue() As Integer
            Return DirectCast(array.getObject(2), COSNumber).intValue()
        End Function

        '/**
        ' * This will set the highest value that is allowed. This cannot be higher than 255.
        ' * 
        ' * @param high The highest value for the lookup table.
        ' */
        Public Sub setHighValue(ByVal high As Integer)
            array.set(2, high)
        End Sub

        '/**
        ' * This will perform a lookup into the color lookup table.
        ' * 
        ' * @param lookupIndex The zero-based index into the table, should not exceed the high value.
        ' * @param componentNumber The component number, probably 1,2,3,3.
        ' * 
        ' * @return The value that was from the lookup table.
        ' * 
        ' * @throws IOException If there is an error looking up the color.
        ' */
        Public Function lookupColor(ByVal lookupIndex As Integer, ByVal componentNumber As Integer) As Integer ' throws IOException
            Dim baseColor As PDColorSpace = getBaseColorSpace()
            Dim data As Byte() = getLookupData()
            Dim numberOfComponents As Integer = baseColor.getNumberOfComponents()
            Return (data(lookupIndex * numberOfComponents + componentNumber) + 256) Mod 256
        End Function

        '/**
        ' * Get the lookup data table.
        ' * 
        ' * @return a byte array containing the the lookup data.
        ' * @throws IOException if an error occurs.
        ' */
        Public Function getLookupData() As Byte() ' throws IOException
            Dim lookupTable As COSBase = array.getObject(3)
            If (lookupData Is Nothing) Then
                If (TypeOf (lookupTable) Is COSString) Then
                    lookupData = DirectCast(lookupTable, COSString).getBytes()
                ElseIf (TypeOf (lookupTable) Is COSStream) Then
                    ' Data will be small so just load the whole thing into memory for
                    ' easier processing
                    Dim lookupStream As COSStream = lookupTable
                    Dim input As InputStream = lookupStream.getUnfilteredStream()
                    Dim output As ByteArrayOutputStream = New ByteArrayOutputStream(1024)
                    Dim buffer(1024 - 1) As Byte '= new byte[1024];
                    Dim amountRead As Integer
                    amountRead = input.read(buffer, 0, buffer.Length)
                    While (amountRead > 0)
                        output.Write(buffer, 0, amountRead)
                        amountRead = input.read(buffer, 0, buffer.Length)
                    End While
                    lookupData = output.toByteArray()
                    output.Dispose()
                ElseIf (lookupTable Is Nothing) Then
                    lookupData = {}
                End If
            Else
                Throw New IOException("Error: Unknown type for lookup table " & lookupTable.toString)
            End If
            Return lookupData
        End Function

        '/**
        ' * This will set a color in the color lookup table.
        ' * 
        ' * @param lookupIndex The zero-based index into the table, should not exceed the high value.
        ' * @param componentNumber The component number, probably 1,2,3,3.
        ' * @param color The color that will go into the table.
        ' * 
        ' * @throws IOException If there is an error looking up the color.
        ' */
        Public Sub setLookupColor(ByVal lookupIndex As Integer, ByVal componentNumber As Integer, ByVal color As Integer) 'throws IOException
            Dim baseColor As PDColorSpace = getBaseColorSpace()
            Dim numberOfComponents As Integer = baseColor.getNumberOfComponents()
            Dim data As Byte() = getLookupData()
            data(lookupIndex * numberOfComponents + componentNumber) = color
            Dim [string] As COSString = New COSString(data)
            array.set(3, [string])
        End Sub

        '/**
        ' * Returns the components of the color for the given index.
        ' * 
        ' * @param index the index of the color value
        ' * @return COSArray with the color components
        ' * @throws IOException If the tint function is not supported
        ' */
        Public Function calculateColorValues(ByVal index As Integer) As Single() ' throws IOException
            ' TODO bpc != 8 ??
            calculateIndexedColorValues(getBaseColorModel(INDEXED_BPC), 8)
            Dim colorValues As Single() = Nothing
            If (index < maxIndex) Then
                Dim bufferIndex As Integer = index * indexNumOfComponents
                colorValues = System.Array.CreateInstance(GetType(Single), indexNumOfComponents)
                For i As Integer = 0 To indexNumOfComponents - 1
                    colorValues(i) = indexedColorValues(bufferIndex + i)
                Next
            End If
            Return colorValues
        End Function

        Private Function getBaseColorModel(ByVal bpc As Integer) As ColorModel '  throws IOException
            If (baseColorModel Is Nothing) Then
                baseColorModel = getBaseColorSpace().createColorModel(bpc)
                If (baseColorModel.getTransferType() <> DataBuffer.TYPE_BYTE) Then
                    Throw New NotImplementedException ' IOException("Not implemented");
                End If
            End If
            Return baseColorModel
        End Function

        Private Sub calculateIndexedColorValues(ByVal colorModel As ColorModel, ByVal bpc As Integer) ' throws IOException
            If (indexedColorValues Is Nothing) Then
                ' number of possible color values in the target color space
                Dim numberOfColorValues As Integer = 1 << bpc
                ' number of indexed color values
                Dim highValue As Integer = getHighValue()
                ' choose the correct size, sometimes there are more indexed values than needed
                ' and sometimes there are fewer indexed value than possible
                maxIndex = Math.Min(numberOfColorValues - 1, highValue)
                Dim index As Byte() = getLookupData()
                ' despite all definitions there may be less values within the lookup data
                Dim numberOfColorValuesFromIndex As Integer = (index.Length / baseColorModel.getNumComponents()) - 1
                maxIndex = Math.Min(maxIndex, numberOfColorValuesFromIndex)
                ' does the colorspace have an alpha channel?
                Dim hasAlpha As Boolean = baseColorModel.hasAlpha()
                indexNumOfComponents = 3 + IIf(hasAlpha, 1, 0)
                Dim buffersize As Integer = (maxIndex + 1) * indexNumOfComponents
                indexedColorValues = System.Array.CreateInstance(GetType(Byte), buffersize)
                Dim inData() As Byte = System.Array.CreateInstance(GetType(Byte), baseColorModel.getNumComponents())
                Dim bufferIndex As Integer = 0
                For i As Integer = 0 To maxIndex
                    System.Array.Copy(index, i * inData.Length, inData, 0, inData.Length)
                    ' calculate RGB values
                    indexedColorValues(bufferIndex) = colorModel.getRed(inData)
                    indexedColorValues(bufferIndex + 1) = colorModel.getGreen(inData)
                    indexedColorValues(bufferIndex + 2) = colorModel.getBlue(inData)
                    If (hasAlpha) Then
                        indexedColorValues(bufferIndex + 3) = colorModel.getAlpha(inData)
                    End If
                    bufferIndex += indexNumOfComponents
                Next
            End If
        End Sub

    End Class

End Namespace
