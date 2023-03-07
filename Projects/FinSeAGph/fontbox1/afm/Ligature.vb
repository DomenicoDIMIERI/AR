Namespace org.fontbox.afm

    '/**
    ' * This class represents a ligature, which is an entry of the CharMetrics.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class Ligature

        Private successor As String
        Private ligature As String

        '/** Getter for property ligature.
        ' * @return Value of property ligature.
        ' */
        Public Function getLigature() As String
            Return ligature
        End Function

        '/** Setter for property ligature.
        ' * @param lig New value of property ligature.
        ' */
        Public Sub setLigature(ByVal lig As String)
            ligature = lig
        End Sub

        '/** Getter for property successor.
        ' * @return Value of property successor.
        ' */
        Public Function getSuccessor() As String
            Return successor
        End Function

        '/** Setter for property successor.
        ' * @param successorValue New value of property successor.
        ' */
        Public Sub setSuccessor(ByVal successorValue As String)
            successor = successorValue
        End Sub

    End Class

End Namespace