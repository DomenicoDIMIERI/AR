Imports FinSeA.Io

Imports System.IO

Namespace org.apache.pdfbox.io.ccitt

    '/**
    ' * This filtering input stream does a fill order change required for certain TIFF images.
    ' * @version $Revision$
    ' */
    Public Class FillOrderChangeInputStream
        Inherits FilterInputStream

        '/**
        ' * Main constructor.
        ' * @param in the underlying input stream
        ' */
        Public Sub New(ByVal [in] As InputStream)
            MyBase.New([in])
        End Sub

        '/** {@inheritDoc} */
        Public Overrides Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer ' throws IOException
            Dim result As Integer = MyBase.read(b, off, len)
            If (result > 0) Then
                Dim endpos As Integer = off + result
                For i As Integer = off To endpos - 1
                    b(i) = FLIP_TABLE(b(i) And &HFF)
                Next
            End If
            Return result
        End Function

        '/** {@inheritDoc} */
        Public Overrides Function read() As Integer ' throws IOException
            Dim b As Integer = MyBase.read()
            If (b < 0) Then
                Return b
            Else
                Return FLIP_TABLE(b) And &HFF
            End If
        End Function

        '// Table to be used when fillOrder = 2, for flipping bytes.
        '// Copied from the TIFFFaxDecoder class
        Private Shared ReadOnly FLIP_TABLE() As Integer = { _
         0, -128, 64, -64, 32, -96, 96, -32, _
        16, -112, 80, -48, 48, -80, 112, -16, _
         8, -120, 72, -56, 40, -88, 104, -24, _
        24, -104, 88, -40, 56, -72, 120, -8, _
         4, -124, 68, -60, 36, -92, 100, -28, _
        20, -108, 84, -44, 52, -76, 116, -12, _
        12, -116, 76, -52, 44, -84, 108, -20, _
        28, -100, 92, -36, 60, -68, 124, -4, _
         2, -126, 66, -62, 34, -94, 98, -30, _
        18, -110, 82, -46, 50, -78, 114, -14, _
        10, -118, 74, -54, 42, -86, 106, -22, _
        26, -102, 90, -38, 58, -70, 122, -6, _
         6, -122, 70, -58, 38, -90, 102, -26, _
        22, -106, 86, -42, 54, -74, 118, -10, _
        14, -114, 78, -50, 46, -82, 110, -18, _
        30, -98, 94, -34, 62, -66, 126, -2, _
         1, -127, 65, -63, 33, -95, 97, -31, _
        17, -111, 81, -47, 49, -79, 113, -15, _
         9, -119, 73, -55, 41, -87, 105, -23, _
        25, -103, 89, -39, 57, -71, 121, -7, _
         5, -123, 69, -59, 37, -91, 101, -27, _
        21, -107, 85, -43, 53, -75, 117, -11, _
        13, -115, 77, -51, 45, -83, 109, -19, _
        29, -99, 93, -35, 61, -67, 125, -3, _
         3, -125, 67, -61, 35, -93, 99, -29, _
        19, -109, 83, -45, 51, -77, 115, -13, _
        11, -117, 75, -53, 43, -85, 107, -21, _
        27, -101, 91, -37, 59, -69, 123, -5, _
         7, -121, 71, -57, 39, -89, 103, -25, _
        23, -105, 87, -41, 55, -73, 119, -9, _
        15, -113, 79, -49, 47, -81, 111, -17, _
        31, -97, 95, -33, 63, -65, 127, -1 _
        }
        ' end
    End Class

End Namespace