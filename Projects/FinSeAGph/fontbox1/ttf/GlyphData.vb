Imports FinSeA.org.fontbox.util

Namespace org.fontbox.ttf

    '/**
    ' * A glyph data record in the glyf table.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class GlyphData

        Private Const FLAG_ON_CURVE = 1
        Private Const FLAG_SHORT_X = 1 << 1
        Private Const FLAG_SHORT_Y = 1 << 2
        Private Const FLAG_X_MAGIC = 1 << 3
        Private Const FLAG_Y_MAGIC = 1 << 4

        Private boundingBox As BoundingBox = New BoundingBox()
        Private numberOfContours As Short
        Private endPointsOfContours As Integer()
        Private instructions As Byte()
        Private flags As Integer()
        Private xCoordinates As Short()
        Private yCoordinates As Short()

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            numberOfContours = data.readSignedShort()
            boundingBox.setLowerLeftX(data.readSignedShort())
            boundingBox.setLowerLeftY(data.readSignedShort())
            boundingBox.setUpperRightX(data.readSignedShort())
            boundingBox.setUpperRightY(data.readSignedShort())
            '/**if( numberOfContours > 0 )
            '{
            '    endPointsOfContours = new int[ numberOfContours ];
            '    for( int i=0; i<numberOfContours; i++ )
            '    {
            '        endPointsOfContours[i] = data.readUnsignedShort();
            '    }
            '    int instructionLength = data.readUnsignedShort();
            '    instructions = data.read( instructionLength );

            '    //BJL It is possible to read some more information here but PDFBox
            '    //does not need it at this time so just ignore it.

            '    //not sure if the length of the flags is the number of contours??
            '    //flags = new int[numberOfContours];
            '    //first read the flags, and just so the TTF can save a couples bytes
            '    //we need to check some bit masks to see if there are more bytes or not.
            '    //int currentFlagIndex = 0;
            '    //int currentFlag = 


            '}*/
        End Sub

        '/**
        ' * @return Returns the boundingBox.
        ' */
        Public Function getBoundingBox() As BoundingBox
            Return boundingBox
        End Function

        '/**
        ' * @param boundingBoxValue The boundingBox to set.
        ' */
        Public Sub setBoundingBox(ByVal boundingBoxValue As BoundingBox)
            Me.boundingBox = boundingBoxValue
        End Sub

        '/**
        ' * @return Returns the numberOfContours.
        ' */
        Public Function getNumberOfContours() As Short
            Return numberOfContours
        End Function

        '/**
        ' * @param numberOfContoursValue The numberOfContours to set.
        ' */
        Public Sub setNumberOfContours(ByVal numberOfContoursValue As Short)
            Me.numberOfContours = numberOfContoursValue
        End Sub

    End Class

End Namespace
