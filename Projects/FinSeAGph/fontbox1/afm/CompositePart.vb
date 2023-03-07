Namespace org.fontbox.afm

    '/**
    ' * This class represents a part of composite character data.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class CompositePart

        Private name As String
        Private xDisplacement As Integer
        Private yDisplacement As Integer

        '/** Getter for property name.
        ' * @return Value of property name.
        ' */
        Public Function getName() As String
            Return name
        End Function

        '/** Setter for property name.
        ' * @param nameValue New value of property name.
        ' */
        Public Sub setName(ByVal nameValue As String)
            name = nameValue
        End Sub

        '/** Getter for property xDisplacement.
        ' * @return Value of property xDisplacement.
        ' */
        Public Function getXDisplacement() As Integer
            Return xDisplacement
        End Function

        '/** Setter for property xDisplacement.
        ' * @param xDisp New value of property xDisplacement.
        ' */
        Public Sub setXDisplacement(ByVal xDisp As Integer)
            xDisplacement = xDisp
        End Sub

        '/** Getter for property yDisplacement.
        ' * @return Value of property yDisplacement.
        ' */
        Public Function getYDisplacement() As Integer
            Return yDisplacement
        End Function

        '/** Setter for property yDisplacement.
        ' * @param yDisp New value of property yDisplacement.
        ' */
        Public Sub setYDisplacement(ByVal yDisp As Integer)
            yDisplacement = yDisp
        End Sub

    End Class

End Namespace