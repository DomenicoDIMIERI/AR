Namespace Drawings

  
    Public Class ComponentColorModel
        Inherits ColorModel

        Private _colorSpace As ColorSpace
        Private _nbBits As Integer()
        Private _p3 As Boolean
        Private _p4 As Boolean
        Private _mode As Transparency.Mode
        Private _p6 As Object
        Private _cs As ColorSpace
        Private _p2 As Boolean
        Private _p5 As Object

        Sub New(colorSpace As ColorSpace, nbBits As Integer(), p3 As Boolean, p4 As Boolean, mode As Transparency.Mode, p6 As Object)
            MyBase.New(0)
        End Sub

        Sub New(cs As ColorSpace, p2 As Boolean, p3 As Boolean, mode As Transparency.Mode, p5 As Object)
            MyBase.New(0)
        End Sub


        Public Overloads Overrides Function getAlpha(pixel As Integer) As Integer
            Throw New NotImplementedException
        End Function
    End Class

End Namespace