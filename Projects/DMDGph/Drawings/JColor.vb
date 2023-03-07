Namespace Drawings

   
    <Serializable> _
    Public Class JColor

#Region "Colors"

        Public Shared ReadOnly Black As New JColor(0, 0, 0)
        Public Shared ReadOnly White As New JColor(1, 1, 1)

#End Region
        Private _cSpace As ColorSpace
        Private _components As Single()
        Private _p3 As Single
        Private _components1 As Single

        Public Sub New()
        End Sub

        Public Sub New(ByVal value As System.Drawing.Color)

        End Sub

        Public Sub New(ByVal r As Single, ByVal g As Single, ByVal b As Single)

        End Sub

        Sub New(cSpace As ColorSpace, components As Single(), p3 As Single)
            ' TODO: Complete member initialization 
            _cSpace = cSpace
            _components = components
            _p3 = p3
        End Sub

        Sub New(components As Single)
            ' TODO: Complete member initialization 
            _components1 = components
        End Sub

        Function getColorSpace() As ColorSpace
            Throw New NotImplementedException
        End Function

        Function getRed() As Single
            Throw New NotImplementedException
        End Function

        Function getGreen() As Single
            Throw New NotImplementedException
        End Function

        Function getBlue() As Single
            Throw New NotImplementedException
        End Function

        Sub getColorComponents(colorComponents As Single())
            Throw New NotImplementedException
        End Sub

        Shared Function getColor(p1 As Integer) As JColor
            Throw New NotImplementedException
        End Function


    End Class

End Namespace