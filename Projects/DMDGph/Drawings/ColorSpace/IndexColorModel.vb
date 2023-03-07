Namespace Drawings

  
    Public Class IndexColorModel
        Inherits ColorModel


        Private _p1 As Integer
        Private _p2 As Integer
        Private _colors As Byte()
        Private _colors1 As Byte()
        Private _colors2 As Byte()
        Private _transparentColors As Byte()
        Private _map As Byte()
        Private _map1 As Byte()
        Private _map2 As Byte()
        Private _mode As Transparency.Mode

        Public Sub New(ByVal bpc As Integer, ByVal numColors As Integer, ByVal map() As Byte, ByVal offSet As Integer, ByVal hasAlfa As Boolean, ByVal alpha As Integer)
            MyBase.New(bpc)
        End Sub

        Public Sub New(ByVal bpc As Integer, ByVal numColors As Integer, ByVal map() As Byte, ByVal offSet As Integer, ByVal hasAlfa As Boolean)
            MyBase.New(bpc)
        End Sub

        Sub New(p1 As Integer, p2 As Integer, colors As Byte(), colors1 As Byte(), colors2 As Byte(), transparentColors As Byte())
            MyBase.New(0)
        End Sub

        Sub New(p1 As Integer, p2 As Integer, map As Byte(), map1 As Byte(), map2 As Byte(), mode As Transparency.Mode)
            MyBase.New(0)
        End Sub


        Public Overloads Overrides Function getAlpha(pixel As Integer) As Integer
            Throw New NotImplementedException
        End Function
    End Class

End Namespace