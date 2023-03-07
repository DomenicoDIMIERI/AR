Imports FinSeA.Drawings
Imports System.Text

Namespace org.apache.pdfbox.util

    '   /**
    '* This class will be used for matrix manipulation.
    '*
    '* @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    '* @version $Revision: 1.14 $
    '*/
    Public Class Matrix
        Implements ICloneable

        Public Shared ReadOnly DEFAULT_SINGLE() As Single = {1, 0, 0, 0, 1, 0, 0, 0, 1}

        Private [single]() As Single

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            'single = new Single[DEFAULT_SINGLE.length];
            ReDim Me.[single](UBound(DEFAULT_SINGLE))
            Me.reset()
        End Sub

        Public Sub New(ByVal items() As Single)
            If (UBound(items) <> 8) Then Throw New ArgumentException("L'array deve essere composto da 9 elementi")
            Me.single = items.Clone
        End Sub

        '/**
        ' * This method resets the numbers in this Matrix to the original values, which are
        ' * the values that a newly constructed Matrix would have.
        ' */
        Public Sub reset()
            Array.Copy(DEFAULT_SINGLE, 0, Me.single, 0, DEFAULT_SINGLE.Length)
        End Sub

        '/**
        ' * Create an affine transform from this matrix's values.
        ' *
        ' * @return An affine transform with this matrix's values.
        ' */
        Public Function createAffineTransform() As AffineTransform
            Dim retval As New AffineTransform(0, 1, 3, 4, 6, 7)
            Return retval
        End Function

        '/**
        ' * Set the values of the matrix from the AffineTransform.
        ' *
        ' * @param af The transform to get the values from.
        ' */
        Public Sub setFromAffineTransform(ByVal af As AffineTransform)
            Me.single(0) = af.getScaleX()
            Me.single(1) = af.getShearY()
            Me.single(3) = af.getShearX()
            Me.single(4) = af.getScaleY()
            Me.single(6) = af.getTranslateX()
            Me.single(7) = af.getTranslateY()
        End Sub

        '/**
        ' * This will get a matrix value at some point.
        ' *
        ' * @param row The row to get the value from.
        ' * @param column The column to get the value from.
        ' *
        ' * @return The value at the row/column position.
        ' */
        Public Function getValue(ByVal row As Integer, ByVal column As Integer) As Single
            Return Me.single(row * 3 + column)
        End Function

        '/**
        ' * This will set a value at a position.
        ' *
        ' * @param row The row to set the value at.
        ' * @param column the column to set the value at.
        ' * @param value The value to set at the position.
        ' */
        Public Sub setValue(ByVal row As Integer, ByVal column As Integer, ByVal value As Single)
            Me.single(row * 3 + column) = value
        End Sub

        '/**
        ' * Return a single dimension array of all values in the matrix.
        ' *
        ' * @return The values ot this matrix.
        ' */
        Public Function getValues() As Single(,)
            Dim retval(,) As Single
            ReDim retval(2, 2)
            retval(0, 0) = Me.single(0)
            retval(0, 1) = Me.single(1)
            retval(0, 2) = Me.single(2)
            retval(1, 0) = Me.single(3)
            retval(1, 1) = Me.single(4)
            retval(1, 2) = Me.single(5)
            retval(2, 0) = Me.single(6)
            retval(2, 1) = Me.single(7)
            retval(2, 2) = Me.single(8)
            Return retval
        End Function

        '/**
        ' * Return a single dimension array of all values in the matrix.
        ' *
        ' * @return The values ot this matrix.
        ' */
        Public Function getValuesAsDouble() As Double(,)
            Dim retval(,) As Double
            ReDim retval(2, 2)
            retval(0, 0) = Me.single(0)
            retval(0, 1) = Me.single(1)
            retval(0, 2) = Me.single(2)
            retval(1, 0) = Me.single(3)
            retval(1, 1) = Me.single(4)
            retval(1, 2) = Me.single(5)
            retval(2, 0) = Me.single(6)
            retval(2, 1) = Me.single(7)
            retval(2, 2) = Me.single(8)
            Return retval
        End Function

        '/**
        ' * This will take the current matrix and multipy it with a matrix that is passed in.
        ' *
        ' * @param b The matrix to multiply by.
        ' *
        ' * @return The result of the two multiplied matrices.
        ' */
        Public Function multiply(ByVal b As Matrix) As Matrix
            Return Me.multiply(b, New Matrix())
        End Function

        '/**
        ' * This method multiplies this Matrix with the specified other Matrix, storing the product in the specified
        ' * result Matrix. By reusing Matrix instances like this, multiplication chains can be executed without having
        ' * to create many temporary Matrix objects.
        ' * <p/>
        ' * It is allowed to have (other == this) or (result == this) or indeed (other == result) but if this is done,
        ' * the backing Single() matrix values may be copied in order to ensure a correct product.
        ' *
        ' * @param other the second operand Matrix in the multiplication
        ' * @param result the Matrix instance into which the result should be stored. If result is null, a new Matrix
        ' *               instance is created.
        ' * @return the product of the two matrices.
        ' */
        Public Function multiply(ByVal other As Matrix, ByVal result As Matrix) As Matrix
            If (result Is Nothing) Then result = New Matrix()
            If (other IsNot Nothing AndAlso other.single IsNot Nothing) Then
                ' the operands
                Dim thisOperand() As Single = Me.single
                Dim otherOperand() As Single = other.single

                '// We're multiplying 2 sets of floats together to produce a third, but we allow
                '// any of these Single() instances to be the same objects.
                '// There is the possibility then to overwrite one of the operands with result values
                '// and therefore corrupt the result.

                '// If either of these operands are the same Single() instance as the result, then
                '// they need to be copied.

                If (Me Is result) Then
                    Dim thisOrigVals() As Single = Me.single.Clone
                    'System.arraycopy(Me.single, 0, thisOrigVals, 0, Me.single.length);
                    thisOperand = thisOrigVals
                End If
                If (other Is result) Then
                    Dim otherOrigVals() As Single = other.single.Clone
                    'System.arraycopy(other.single, 0, otherOrigVals, 0, other.single.length);
                    otherOperand = otherOrigVals
                End If
                result.single(0) = thisOperand(0) * otherOperand(0) + thisOperand(1) * otherOperand(3) + thisOperand(2) * otherOperand(6)
                result.single(1) = thisOperand(0) * otherOperand(1) + thisOperand(1) * otherOperand(4) + thisOperand(2) * otherOperand(7)
                result.single(2) = thisOperand(0) * otherOperand(2) + thisOperand(1) * otherOperand(5) + thisOperand(2) * otherOperand(8)
                result.single(3) = thisOperand(3) * otherOperand(0) + thisOperand(4) * otherOperand(3) + thisOperand(5) * otherOperand(6)
                result.single(4) = thisOperand(3) * otherOperand(1) + thisOperand(4) * otherOperand(4) + thisOperand(5) * otherOperand(7)
                result.single(5) = thisOperand(3) * otherOperand(2) + thisOperand(4) * otherOperand(5) + thisOperand(5) * otherOperand(8)
                result.single(6) = thisOperand(6) * otherOperand(0) + thisOperand(7) * otherOperand(3) + thisOperand(8) * otherOperand(6)
                result.single(7) = thisOperand(6) * otherOperand(1) + thisOperand(7) * otherOperand(4) + thisOperand(8) * otherOperand(7)
                result.single(8) = thisOperand(6) * otherOperand(2) + thisOperand(7) * otherOperand(5) + thisOperand(8) * otherOperand(8)
            End If

            Return result
        End Function

        ''' <summary>
        '''Create a new matrix with just the scaling operators. 
        ''' </summary>
        ''' <returns>return A new matrix with just the scaling operators.</returns>
        ''' <remarks></remarks>
        Public Function extractScaling() As Matrix
            Dim retval As New Matrix()
            retval.single(0) = Me.single(0)
            retval.single(4) = Me.single(4)
            Return retval
        End Function

        '/**
        ' * Convenience method to create a scaled instance.
        ' *
        ' * @param x The xscale operator.
        ' * @param y The yscale operator.
        ' * @return A new matrix with just the x/y scaling
        ' */
        Public Shared Function getScaleInstance(ByVal x As Single, ByVal y As Single) As Matrix
            Dim retval As New Matrix()
            retval.single(0) = x
            retval.single(4) = y
            Return retval
        End Function

        '/**
        ' * Create a new matrix with just the translating operators.
        ' *
        ' * @return A new matrix with just the translating operators.
        ' */
        Public Function extractTranslating() As Matrix
            Dim retval As New Matrix()
            retval.single(6) = Me.single(6)
            retval.single(7) = Me.single(7)
            Return retval
        End Function

        '/**
        ' * Convenience method to create a translating instance.
        ' *
        ' * @param x The x translating operator.
        ' * @param y The y translating operator.
        ' * @return A new matrix with just the x/y translating.
        ' */
        Public Shared Function getTranslatingInstance(ByVal x As Single, ByVal y As Single) As Matrix
            Dim retval As New Matrix()
            retval.single(6) = x
            retval.single(7) = y
            Return retval
        End Function

        '/**
        ' * Clones this object.
        ' * @return cloned matrix as an object.
        ' */
        Public Function clone() As Object Implements ICloneable.Clone
            Return New Matrix(Me.single.Clone)
        End Function

        '/**
        ' * This will copy the text matrix data.
        ' *
        ' * @return a matrix that matches this one.
        ' */
        Public Function copy() As Matrix
            Return Me.clone()
        End Function

        '/**
        ' * This will return a string representation of the matrix.
        ' *
        ' * @return The matrix as a string.
        ' */
        Public Overrides Function toString() As String
            Dim result As New StringBuilder
            result.append("[[")
            result.Append(Me.single(0) & ",")
            result.Append(Me.single(1) & ",")
            result.Append(Me.single(2) & "][")
            result.Append(Me.single(3) & ",")
            result.Append(Me.single(4) & ",")
            result.Append(Me.single(5) & "][")
            result.Append(Me.single(6) & ",")
            result.Append(Me.single(7) & ",")
            result.Append(Me.single(8) & "]]")

            Return result.toString()
        End Function

        '/**
        ' * Get the xscaling factor of this matrix.
        ' * @return The x-scale.
        ' */
        Public Function getXScale() As Single
            Dim xScale As Single = Me.single(0)

            '/**
            ' * BM: if the trm is rotated, the calculation is a little more complicated
            ' *
            ' * The rotation matrix multiplied with the scaling matrix is:
            ' * (   x   0   0)    ( cos  sin  0)    ( x*cos x*sin   0)
            ' * (   0   y   0) *  (-sin  cos  0)  = (-y*sin y*cos   0)
            ' * (   0   0   1)    (   0    0  1)    (     0     0   1)
            ' *
            ' * So, if you want to deduce x from the matrix you take
            ' * M(0,0) = x*cos and M(0,1) = x*sin and use the theorem of Pythagoras
            ' *
            ' * sqrt(M(0,0)^2+M(0,1)^2) =
            ' * sqrt(x2*cos2+x2*sin2) =
            ' * sqrt(x2*(cos2+sin2)) = <- here is the trick cos2+sin2 is one
            ' * sqrt(x2) =
            ' * abs(x)
            ' */
            If (Not (Me.single(1) = 0.0F AndAlso Me.single(3) = 0.0F)) Then
                xScale = Math.Sqrt(Math.pow(Me.single(0), 2) + Math.pow(Me.single(1), 2))
            End If
            Return xScale
        End Function

        '/**
        ' * Get the y scaling factor of this matrix.
        ' * @return The y-scale factor.
        ' */
        Public Function getYScale() As Single
            Dim yScale As Single = Me.single(4)
            If (Not (Me.single(1) = 0.0F AndAlso Me.single(3) = 0.0F)) Then
                yScale = Math.Sqrt(Math.Pow(Me.single(3), 2) + Math.Pow(Me.single(4), 2))
            End If
            Return yScale
        End Function

        '/**
        ' * Get the x position in the matrix.
        ' * @return The x-position.
        ' */
        Public Function getXPosition() As Single
            Return Me.single(6)
        End Function

        '/**
        ' * Get the y position.
        ' * @return The y position.
        ' */
        Public Function getYPosition() As Single
            Return Me.single(7)
        End Function

    End Class

End Namespace
