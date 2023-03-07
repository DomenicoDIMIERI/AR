Imports System.Drawing.Imaging
Imports System.Drawing
Imports nQuant

<Serializable> _
Public Class ScreenShot
    Implements IDisposable

    Public Const jpgQuality As Long = 50L
    Public Shared jpgEncoder As System.Drawing.Imaging.ImageCodecInfo
    Public Shared myEncoderParameters As System.Drawing.Imaging.EncoderParameters

    Shared Sub New()
        jpgEncoder = GetEncoder(ImageFormat.Jpeg)
        ' Create an Encoder object based on the GUID
        ' for the Quality parameter category.
        Dim myEncoder As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality

        ' Create an EncoderParameters object.
        ' An EncoderParameters object has an array of EncoderParameter
        ' objects. In this case, there is only one
        ' EncoderParameter object in the array.
        myEncoderParameters = New EncoderParameters(1)

        Dim myEncoderParameter As New EncoderParameter(myEncoder, jpgQuality)
        myEncoderParameters.Param(0) = myEncoderParameter
        'bmp1.Save(@"c:\TestPhotoQualityFifty.jpg", jpgEncoder, myEncoderParameters);
    End Sub


    Private Shared Function GetEncoder(ByVal format As ImageFormat) As ImageCodecInfo
        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageDecoders()
        For Each codec As ImageCodecInfo In codecs
            If (codec.FormatID = format.Guid) Then
                Return codec
            End If
        Next
        Return Nothing
    End Function


    Public Time As Date
    Public Name As String
    Public IsFullScreen As Boolean
    Public Bounds As System.Drawing.Rectangle
    Public Content As System.Drawing.Image

    <NonSerialized> Private ms As System.IO.MemoryStream

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.Time = Nothing
        Me.Name = ""
        Me.IsFullScreen = False
        Me.Bounds = Nothing
        Me.Content = Nothing
    End Sub

    Public Sub New(ByVal name As String, ByVal isFullScreen As Boolean, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal content As System.Drawing.Bitmap)
        Me.New
        Me.Time = Now
        Me.Name = name
        Me.IsFullScreen = isFullScreen
        Me.Bounds = New Drawing.Rectangle(x, y, width, height)
        Dim quantizer As New WuQuantizer()
        Dim alphaTransparency As Integer = 1
        Dim alphaFader As Integer = 1
        Dim targetPath As String = System.IO.Path.GetTempFileName
        'Using quantized As System.Drawing.Image = quantizer.QuantizeImage(content, alphaTransparency, alphaFader)
        'quantized.Save(targetPath, ImageFormat.Jpeg)
        'End Using
        Me.Content = quantizer.QuantizeImage(content, alphaTransparency, alphaFader) 'New Bitmap(targetPath) 'Me.jpegCompress(content)
        'System.IO.File.Delete(targetPath)
    End Sub

    Private Function jpegCompress(ByVal img As System.Drawing.Bitmap) As System.Drawing.Bitmap
        Me.ms = New System.IO.MemoryStream
        'Dim fName As String = System.IO.Path.GetTempFileName
        img.Save(ms, jpgEncoder, myEncoderParameters)
        img.Dispose()
        ms.Position = 0

        img = New Bitmap(ms)

        Return img
    End Function


    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        If Me.Content IsNot Nothing Then Me.Content.Dispose() : Me.Content = Nothing
        If Me.ms IsNot Nothing Then Me.ms.Dispose() : Me.ms = Nothing
        Me.Name = vbNullString
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
