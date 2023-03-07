Namespace PDF

    Public Class PDFImage
        Private m_File As cImage
        Public n As Integer
        Public i As Integer
        Public w As Single 'width
        Public h As Single 'height
        Public cs As String 'Colorspace
        Public bpc As Byte 'bits per channel
        Public f As String '"DCTDecode
        Public data As String 'data buffer
        Public size As Integer 'size of the image in bytes
        Public parms As String
        Public trns() As String
        Public pal As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal w As Single, ByVal h As Single, ByVal cs As String, ByVal bpc As Byte, ByVal f As String, ByVal data As String, ByVal size As Integer)
            Me.w = w
            Me.h = h
            Me.cs = cs
            Me.bpc = bpc
            Me.f = f
            Me.data = data
            Me.size = size
        End Sub

        Public Sub New(ByVal fileName As String)
            Dim xa As New cImage(fileName)
            'if(!xa)Me.Error("Missing or incorrect image file: " + xfile);
            'If (xa.id <> 2) Then
            '    Throw New ArgumentException("Not a JPEG file: " & fileName)
            'End If
            Dim xcolspace As String
            If (xa.channels = 0 Or xa.channels = 3) Then
                xcolspace = "DeviceRGB"
            ElseIf (xa.channels = 4) Then
                xcolspace = "DeviceCMYK"
            Else
                xcolspace = "DeviceGray"
            End If
            Dim xbpc As Integer = IIf(xa.bits > 0, xa.bits, 8)
            Dim xdata As String = xa.GetBuffer()
            Dim Size As Integer = xa.size
            Me.w = xa.width
            Me.h = xa.height
            Me.cs = xcolspace
            Me.bpc = xbpc
            Me.f = "DCTDecode"
            Me.data = xdata
            Me.size = Size
        End Sub

        Public Sub New(ByVal img As System.Drawing.Image)
            Dim xa As New cImage(img)
            'if(!xa)Me.Error("Missing or incorrect image file: " + xfile);
            'If (xa.id <> 2) Then
            '    Throw New ArgumentException("Not a JPEG file")
            'End If
            Dim xcolspace As String
            If (xa.channels = 0 Or xa.channels = 3) Then
                xcolspace = "DeviceRGB"
            ElseIf (xa.channels = 4) Then
                xcolspace = "DeviceCMYK"
            Else
                xcolspace = "DeviceGray"
            End If
            Dim xbpc As Integer = IIf(xa.bits > 0, xa.bits, 8)
            Dim xdata As String = xa.GetBuffer()
            Dim Size As Integer = xa.Size
            Me.w = xa.width
            Me.h = xa.height
            Me.cs = xcolspace
            Me.bpc = xbpc
            Me.f = "DCTDecode"
            Me.data = xdata
            Me.size = Size
        End Sub

    End Class

End Namespace