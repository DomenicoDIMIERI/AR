Namespace Drawings

    Public Class BasicStroke

        Private _p1 As Object
        Private _lineCapStyle As Integer
        Private _p3 As Object
        Private _p4 As Object
        Private _p5 As Object
        Private _p6 As Object
        Private _p11 As Integer
        Private _lineWidth As Single
        Private _p2 As Integer

        Sub New(p1 As Object, lineCapStyle As Integer, p3 As Object, p4 As Object, p5 As Object, p6 As Object)
            ' TODO: Complete member initialization 
            _p1 = p1
            _lineCapStyle = lineCapStyle
            _p3 = p3
            _p4 = p4
            _p5 = p5
            _p6 = p6
        End Sub

        Sub New(p1 As Integer, lineCapStyle As Integer, p3 As Object)
            ' TODO: Complete member initialization 
            _p11 = p1
            _lineCapStyle = lineCapStyle
            _p3 = p3
        End Sub

        Sub New(lineWidth As Single)
            ' TODO: Complete member initialization 
            _lineWidth = lineWidth
        End Sub

        Sub New(p1 As Object, p2 As Integer, p3 As Object, p4 As Object)
            ' TODO: Complete member initialization 
            _p1 = p1
            _p2 = p2
            _p3 = p3
            _p4 = p4
        End Sub

        Function getLineWidth() As Object
            Throw New NotImplementedException
        End Function

        Function getLineJoin() As Object
            Throw New NotImplementedException
        End Function

        Function getMiterLimit() As Object
            Throw New NotImplementedException
        End Function

        Function getDashArray() As Object
            Throw New NotImplementedException
        End Function

        Function getDashPhase() As Object
            Throw New NotImplementedException
        End Function

        Public Const JOIN_MITER As Integer = 0

        Function getEndCap() As Integer
            Throw New NotImplementedException
        End Function

        Shared Function CAP_SQUARE() As Integer
            Throw New NotImplementedException
        End Function


    End Class

End Namespace