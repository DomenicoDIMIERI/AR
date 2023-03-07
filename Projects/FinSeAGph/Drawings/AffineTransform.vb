Namespace Drawings

    Public Class AffineTransform

        Private _width As Single
        Private _p2 As Integer
        Private _p3 As Integer
        Private _height As Single
        Private _x As Single
        Private _y As Single

        Sub New(width As Single, p2 As Integer, p3 As Integer, height As Single, x As Single, y As Single)
            ' TODO: Complete member initialization 
            _width = width
            _p2 = p2
            _p3 = p3
            _height = height
            _x = x
            _y = y
        End Sub

        Sub New()
            ' TODO: Complete member initialization 
        End Sub

        Sub transform(coordsTemp As Single(), p2 As Integer, coords As Single(), p4 As Integer, p5 As Integer)
            Throw New NotImplementedException
        End Sub

        Sub transform(coordsTemp As Double(), p2 As Integer, coords As Double(), p4 As Integer, p5 As Integer)
            Throw New NotImplementedException
        End Sub

        Function getTranslateY() As Single
            Throw New NotImplementedException
        End Function

        Sub getMatrix(values As Double())
            Throw New NotImplementedException
        End Sub

        Function getShearX() As Object
            Throw New NotImplementedException
        End Function

        Function getScaleX() As Object
            Throw New NotImplementedException
        End Function

        Sub scale(p1 As Single, p2 As Single)
            Throw New NotImplementedException
        End Sub

        Function getShearY() As Single
            Throw New NotImplementedException
        End Function

        Function getScaleY() As Single
            Throw New NotImplementedException
        End Function

        Function getTranslateX() As Single
            Throw New NotImplementedException
        End Function

        Sub transform(point As PointF, point1 As PointF)
            Throw New NotImplementedException
        End Sub

        Function createInverse() As AffineTransform
            Throw New NotImplementedException
        End Function

        Function isIdentity() As Boolean
            Throw New NotImplementedException
        End Function

        Sub translate(p1 As Single, p2 As Single)
            Throw New NotImplementedException
        End Sub

        Sub rotate(p1 As Double)
            Throw New NotImplementedException
        End Sub


    End Class

End Namespace