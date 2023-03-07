'import java.util.Random;

Namespace org.apache.pdfbox.pdmodel.graphics.predictor

    '/**
    ' * Implements different PNG predictor algorithms that is used in PDF files.
    ' *
    ' * @author xylifyx@yahoo.co.uk
    ' * @version $Revision: 1.4 $
    ' * @see <a href="http://www.w3.org/TR/PNG-Filters.html">PNG Filters</a>
    ' */
    Public MustInherit Class PredictorAlgorithm

        Private width As Integer

        Private height As Integer

        Private bpp As Integer

        '/**
        ' * check that buffer sizes matches width,height,bpp. This implementation is
        ' * used by most of the filters, but not Uptimum.
        ' *
        ' * @param src The source buffer.
        ' * @param dest The destination buffer.
        ' */
        Public Overridable Sub checkBufsiz(ByVal src() As Byte, ByVal dest() As Byte)
            If (src.Length <> dest.Length) Then
                Throw New ArgumentException("src.length != dest.length") 'IllegalArgumentException
            End If
            If (src.Length <> getWidth() * getHeight() * getBpp()) Then
                Throw New ArgumentException("src.length != width * height * bpp")
            End If
        End Sub

        '/**
        ' * encode line of pixel data in src from srcOffset and width*bpp bytes
        ' * forward, put the decoded bytes into dest.
        ' *
        ' * @param src
        ' *            raw image data
        ' * @param dest
        ' *            encoded data
        ' * @param srcDy
        ' *            byte offset between lines
        ' * @param srcOffset
        ' *            beginning of line data
        ' * @param destDy
        ' *            byte offset between lines
        ' * @param destOffset
        ' *            beginning of line data
        ' */
        Public MustOverride Sub encodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)

        '/**
        ' * decode line of pixel data in src from src_offset and width*bpp bytes
        ' * forward, put the decoded bytes into dest.
        ' *
        ' * @param src
        ' *            encoded image data
        ' * @param dest
        ' *            raw data
        ' * @param srcDy
        ' *            byte offset between lines
        ' * @param srcOffset
        ' *            beginning of line data
        ' * @param destDy
        ' *            byte offset between lines
        ' * @param destOffset
        ' *            beginning of line data
        ' */
        Public MustOverride Sub decodeLine(ByVal src() As Byte, ByVal dest() As Byte, ByVal srcDy As Integer, ByVal srcOffset As Integer, ByVal destDy As Integer, ByVal destOffset As Integer)

        '    '/**
        '    ' * Simple command line program to test the algorithm.
        '    ' *
        '    ' * @param args The command line arguments.
        '    ' */
        '    Public Shared Sub main(ByVal args() As String)
        '    Random rnd = new Random();
        '    int width = 5;
        '    int height = 5;
        '    int bpp = 3;
        '    byte[] raw = new byte[width * height * bpp];
        '    rnd.nextBytes(raw);
        '    System.out.println("raw:   ");
        '    dump(raw);
        '    for (int i = 10; i < 15; i++)
        '    {
        '        byte[] decoded = new byte[width * height * bpp];
        '        byte[] encoded = new byte[width * height * bpp];

        '        PredictorAlgorithm filter = PredictorAlgorithm.getFilter(i);
        '        filter.setWidth(width);
        '        filter.setHeight(height);
        '        filter.setBpp(bpp);
        '        filter.encode(raw, encoded);
        '        filter.decode(encoded, decoded);
        '        System.out.println(filter.getClass().getName());
        '        dump(decoded);
        '    }
        '}

        '/**
        ' * Get the left pixel from the buffer.
        ' *
        ' * @param buf The buffer.
        ' * @param offset The offset into the buffer.
        ' * @param dy The dy value.
        ' * @param x The x value.
        ' *
        ' * @return The left pixel.
        ' */
        Public Function leftPixel(ByVal buf() As Byte, ByVal offset As Integer, ByVal dy As Integer, ByVal x As Integer)
            If (x >= getBpp()) Then
                Return buf(offset + x - getBpp())
            Else
                Return 0
            End If
        End Function

        '/**
        ' * Get the above pixel from the buffer.
        ' *
        ' * @param buf The buffer.
        ' * @param offset The offset into the buffer.
        ' * @param dy The dy value.
        ' * @param x The x value.
        ' *
        ' * @return The above pixel.
        ' */
        Public Function abovePixel(ByVal buf() As Byte, ByVal offset As Integer, ByVal dy As Integer, ByVal x As Integer)
            If (offset >= dy) Then
                Return buf(offset + x - dy)
            Else
                Return 0
            End If
        End Function

        '/**
        ' * Get the above-left pixel from the buffer.
        ' *
        ' * @param buf The buffer.
        ' * @param offset The offset into the buffer.
        ' * @param dy The dy value.
        ' * @param x The x value.
        ' *
        ' * @return The above-left pixel.
        ' */
        Public Function aboveLeftPixel(ByVal buf() As Byte, ByVal offset As Integer, ByVal dy As Integer, ByVal x As Integer) As Integer
            If (offset >= dy AndAlso x >= getBpp()) Then
                Return buf(offset + x - dy - getBpp())
            Else
                Return 0
            End If
        End Function


        '/**
        ' * Simple helper to print out a buffer.
        ' *
        ' * @param raw The bytes to print out.
        ' */
        Private Shared Sub dump(ByVal raw() As Byte)
            For i As Integer = 0 To raw.Length - 1
                Debug.Write(raw(i) & " ")
            Next
            Debug.Print("")
        End Sub

        '/**
        ' * @return Returns the bpp.
        ' */
        Public Function getBpp() As Integer
            Return bpp
        End Function

        '/**
        ' * @param newBpp
        ' *            The bpp to set.
        ' */
        Public Overridable Sub setBpp(ByVal newBpp As Integer)
            bpp = newBpp
        End Sub

        '/**
        ' * @return Returns the height.
        ' */
        Public Overridable Function getHeight() As Integer
            Return height
        End Function

        '/**
        ' * @param newHeight
        ' *            The height to set.
        ' */
        Public Overridable Sub setHeight(ByVal newHeight As Integer)
            height = newHeight
        End Sub

        '/**
        ' * @return Returns the width.
        ' */
        Public Function getWidth() As Integer
            Return width
        End Function

        '/**
        ' * @param newWidth
        ' *            The width to set.
        ' */
        Public Overridable Sub setWidth(ByVal newWidth As Integer)
            Me.width = newWidth
        End Sub


        '/**
        ' * encode a byte array full of image data using the filter that this object
        ' * implements.
        ' *
        ' * @param src
        ' *            buffer
        ' * @param dest
        ' *            buffer
        ' */
        Public Overridable Sub encode(ByVal src() As Byte, ByVal dest() As Byte)
            checkBufsiz(dest, src)
            Dim dy As Integer = getWidth() * getBpp()
            For y As Integer = 0 To height - 1
                Dim yoffset As Integer = y * dy
                encodeLine(src, dest, dy, yoffset, dy, yoffset)
            Next
        End Sub

        '/**
        ' * decode a byte array full of image data using the filter that this object
        ' * implements.
        ' *
        ' * @param src
        ' *            buffer
        ' * @param dest
        ' *            buffer
        ' */
        Public Overridable Sub decode(ByVal src() As Byte, ByVal dest() As Byte)
            checkBufsiz(src, dest)
            Dim dy As Integer = width * bpp
            For y As Integer = 0 To height - 1
                Dim yoffset As Integer = y * dy
                decodeLine(src, dest, dy, yoffset, dy, yoffset)
            Next
        End Sub


        Public Enum FilterEnum As Integer
            None = 0
            TIFF = 2

            ''' <summary>
            ''' PNG prediction (on encoding, PNG None on all rows)
            ''' </summary>
            ''' <remarks></remarks>
            PNG_NoneAllRows = 10

            ''' <summary>
            ''' PNG prediction (on encoding, PNG Sub on all rows)
            ''' </summary>
            ''' <remarks></remarks>
            PNG_SubAllRows = 11

            ''' <summary>
            ''' PNG prediction (on encoding, PNG Up on all rows)
            ''' </summary>
            ''' <remarks></remarks>
            PNG_UpAllRows = 12

            ''' <summary>
            ''' PNG prediction (on encoding, PNG Average on all rows)
            ''' </summary>
            ''' <remarks></remarks>
            PNG_AverageAllRows = 13

            ''' <summary>
            ''' PNG prediction (on encoding, PNG Paeth on all rows)
            ''' </summary>
            ''' <remarks></remarks>
            PNG_PaethAllRows = 14

            ''' <summary>
            ''' 15 PNG prediction (on encoding, PNG optimum)
            ''' </summary>
            ''' <remarks></remarks>
            PNG_Optimum = 15
        End Enum

        '/**
        ' * @param predictor
        ' *            <ul>
        ' *            <li>1 No prediction (the default value)
        ' *            <li>2 TIFF Predictor 2
        ' *            <li>10 PNG prediction (on encoding, PNG None on all rows)
        ' *            <li>11 PNG prediction (on encoding, PNG Sub on all rows)
        ' *            <li>12 PNG prediction (on encoding, PNG Up on all rows)
        ' *            <li>13 PNG prediction (on encoding, PNG Average on all rows)
        ' *            <li>14 PNG prediction (on encoding, PNG Paeth on all rows)
        ' *            <li>15 PNG prediction (on encoding, PNG optimum)
        ' *            </ul>
        ' *
        ' * @return The predictor class based on the predictor code.
        ' */
        Public Shared Function getFilter(ByVal predictor As FilterEnum) As PredictorAlgorithm
            Dim filter As PredictorAlgorithm
            Select Case (predictor)
                Case 10 : filter = New None()
                Case 11 : filter = New [Sub]()
                Case 12 : filter = New Up()
                Case 13 : filter = New Average()
                Case 14 : filter = New Paeth()
                Case 15 : filter = New Optimum()
                Case Else : filter = New None()
            End Select
            Return filter
        End Function

    End Class

End Namespace