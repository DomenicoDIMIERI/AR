Namespace org.apache.fontbox.afm

    '/**
    ' * This class represents composite character data.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class Composite

        Private name As String
        Private parts As List(Of CompositePart) = New ArrayList(Of CompositePart)()

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
            Me.name = nameValue
        End Sub

        '/**
        ' * This will add a composite part.
        ' *
        ' * @param part The composite part to add.
        ' */
        Public Sub addPart(ByVal part As CompositePart)
            parts.add(part)
        End Sub

        '/** Getter for property parts.
        ' * @return Value of property parts.
        ' */
        Public Function getParts() As List(Of CompositePart)
            Return parts
        End Function

        '/** Setter for property parts.
        ' * @param partsList New value of property parts.
        ' */
        Public Sub setParts(ByVal partsList As List(Of CompositePart))
            Me.parts = partsList
        End Sub

    End Class


End Namespace