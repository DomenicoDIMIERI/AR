Namespace org.fontbox.afm

    '/**
    ' * This class represents a piece of track kerning data.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class TrackKern

        Private degree As Integer
        Private minPointSize As Single
        Private minKern As Single
        Private maxPointSize As Single
        Private maxKern As Single

        '/** Getter for property degree.
        ' * @return Value of property degree.
        ' */
        Public Function getDegree() As Integer
            Return degree
        End Function

        '/** Setter for property degree.
        ' * @param degreeValue New value of property degree.
        ' */
        Public Sub setDegree(ByVal degreeValue As Integer)
            degree = degreeValue
        End Sub

        '/** Getter for property maxKern.
        ' * @return Value of property maxKern.
        ' */
        Public Function getMaxKern() As Single
            Return maxKern
        End Function

        '/** Setter for property maxKern.
        ' * @param maxKernValue New value of property maxKern.
        ' */
        Public Sub setMaxKern(ByVal maxKernValue As Single)
            maxKern = maxKernValue
        End Sub

        '/** Getter for property maxPointSize.
        ' * @return Value of property maxPointSize.
        ' */
        Public Function getMaxPointSize() As Single
            Return maxPointSize
        End Function

        '/** Setter for property maxPointSize.
        ' * @param maxPointSizeValue New value of property maxPointSize.
        ' */
        Public Sub setMaxPointSize(ByVal maxPointSizeValue As Single)
            maxPointSize = maxPointSizeValue
        End Sub

        '/** Getter for property minKern.
        ' * @return Value of property minKern.
        ' */
        Public Function getMinKern() As Single
            Return minKern
        End Function

        '/** Setter for property minKern.
        ' * @param minKernValue New value of property minKern.
        ' */
        Public Sub setMinKern(ByVal minKernValue As Single)
            minKern = minKernValue
        End Sub

        '/** Getter for property minPointSize.
        ' * @return Value of property minPointSize.
        ' */
        Public Function getMinPointSize() As Single
            Return minPointSize
        End Function

        '/** Setter for property minPointSize.
        ' * @param minPointSizeValue New value of property minPointSize.
        ' */
        Public Sub setMinPointSize(ByVal minPointSizeValue As Single)
            minPointSize = minPointSizeValue
        End Sub

    End Class

End Namespace