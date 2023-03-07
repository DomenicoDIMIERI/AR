Namespace org.apache.fontbox.afm

    '/**
    ' * This represents some kern pair data.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class KernPair

        Private firstKernCharacter As String
        Private secondKernCharacter As String
        Private x As Single
        Private y As Single

        '/** Getter for property firstKernCharacter.
        ' * @return Value of property firstKernCharacter.
        ' */
        Public Function getFirstKernCharacter() As String
            Return firstKernCharacter
        End Function

        '/** Setter for property firstKernCharacter.
        ' * @param firstKernCharacterValue New value of property firstKernCharacter.
        ' */
        Public Sub setFirstKernCharacter(ByVal firstKernCharacterValue As String)
            firstKernCharacter = firstKernCharacterValue
        End Sub

        '/** Getter for property secondKernCharacter.
        ' * @return Value of property secondKernCharacter.
        ' */
        Public Function getSecondKernCharacter() As String
            Return secondKernCharacter
        End Function

        '/** Setter for property secondKernCharacter.
        ' * @param secondKernCharacterValue New value of property secondKernCharacter.
        ' */
        Public Sub setSecondKernCharacter(ByVal secondKernCharacterValue As String)
            secondKernCharacter = secondKernCharacterValue
        End Sub

        '/** Getter for property x.
        ' * @return Value of property x.
        ' */
        Public Function getX() As Single
            Return x
        End Function

        '/** Setter for property x.
        ' * @param xValue New value of property x.
        ' */
        Public Sub setX(ByVal xValue As Single)
            x = xValue
        End Sub

        '/** Getter for property y.
        ' * @return Value of property y.
        ' */
        Public Function getY() As Single
            Return y
        End Function

        '/** Setter for property y.
        ' * @param yValue New value of property y.
        ' */
        Public Sub setY(ByVal yValue As Single)
            y = yValue
        End Sub

    End Class

End Namespace