Namespace Drawings

    Public Class BufferedImage
        Private m_Image As System.Drawing.Bitmap

        Public Const TYPE_INT_ARGB = 1
        Private _p1 As Integer
        Private _p2 As Integer
        Private _tYPE_INT_ARGB As Integer
        Private _colorModel As ColorModel
        Private _raster As WritableRaster
        Private _p3 As Boolean
        Private _p4 As Object

        Public Sub New(ByVal fileName As String)
            Me.m_Image = New System.Drawing.Bitmap(fileName)
        End Sub

        Public Sub New(ByVal img As System.Drawing.Image)
            If (img Is Nothing) Then Throw New ArgumentNullException
            Me.m_Image = img
        End Sub

        Sub New(p1 As Integer, p2 As Integer, TYPE_INT_ARGB As Integer)
            ' TODO: Complete member initialization 
            _p1 = p1
            _p2 = p2
            _tYPE_INT_ARGB = TYPE_INT_ARGB
        End Sub

        Sub New(colorModel As ColorModel, raster As WritableRaster, p3 As Boolean, p4 As Object)
            ' TODO: Complete member initialization 
            _colorModel = colorModel
            _raster = raster
            _p3 = p3
            _p4 = p4
        End Sub

        Function getWidth() As Integer
            Return Me.m_Image.Width
        End Function

        Function getHeight() As Integer
            Return Me.m_Image.Height
        End Function

        Function getRGB(x As Integer, y As Integer) As Integer
            Return Me.m_Image.GetPixel(x, y).ToArgb
        End Function

        Function setRGB(x As Integer, y As Integer, p3 As Integer) As Integer
            Dim c As Integer = &HFF000000 Or p3
            Me.m_Image.SetPixel(x, y, Color.FromArgb(c))
            Return c
        End Function

        Function getScaledInstance(newWidth As Integer, newHeight As Integer, p3 As Object) As BufferedImage
            Throw New NotImplementedException
        End Function


        Public Shared Widening Operator CType(ByVal img As System.Drawing.Image) As BufferedImage
            Return New BufferedImage(img)
        End Operator

        Public Shared Narrowing Operator CType(ByVal img As BufferedImage) As System.Drawing.Image
            Return img.m_Image
        End Operator

        Function getGraphics() As Graphics2D
            Throw New NotImplementedException
        End Function

        Function getColorModel() As ColorModel
            Throw New NotImplementedException
        End Function

        Function getRaster() As WritableRaster
            Throw New NotImplementedException
        End Function

        Function SCALE_SMOOTH() As Object
            Throw New NotImplementedException
        End Function

        Function getData() As Object
            Throw New NotImplementedException
        End Function

        Shared Function TYPE_USHORT_565_RGB() As Integer
            Throw New NotImplementedException
        End Function

        Shared Function TYPE_INT_RGB() As Object
            Throw New NotImplementedException
        End Function

        Sub setData(raster As WritableRaster)
            Throw New NotImplementedException
        End Sub

        Function createGraphics() As Graphics2D
            Throw New NotImplementedException
        End Function

        Shared Function TYPE_3BYTE_BGR() As Object
            Throw New NotImplementedException
        End Function

        Function getAlphaRaster() As WritableRaster
            Throw New NotImplementedException
        End Function

        Shared Function TYPE_BYTE_BINARY() As Object
            Throw New NotImplementedException
        End Function

    End Class

End Namespace