Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf


    '/**
    ' * An object for four colours.
    ' *
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDFourColours
        Implements COSObjectable

        Private array As COSArray

        Public Sub New()
            Me.array = New COSArray()
            Me.array.add(COSNull.NULL)
            Me.array.add(COSNull.NULL)
            Me.array.add(COSNull.NULL)
            Me.array.add(COSNull.NULL)
        End Sub

        Public Sub New(ByVal array As COSArray)
            Me.array = array
            ' ensure that array has 4 items
            While (Me.array.size() < 4)
                Me.array.add(COSNull.NULL)
            End While
        End Sub
    

        '/**
        ' * Gets the colour for the before edge.
        ' * 
        ' * @return the colour for the before edge
        ' */
        Public Function getBeforeColour() As PDGamma
            Return Me.getColourByIndex(0)
        End Function

        '/**
        ' * Sets the colour for the before edge.
        ' * 
        ' * @param colour the colour for the before edge
        ' */
        Public Sub setBeforeColour(ByVal colour As PDGamma)
            Me.setColourByIndex(0, colour)
        End Sub

        '/**
        ' * Gets the colour for the after edge.
        ' * 
        ' * @return the colour for the after edge
        ' */
        Public Function getAfterColour() As PDGamma
            Return Me.getColourByIndex(1)
        End Function

        '/**
        ' * Sets the colour for the after edge.
        ' * 
        ' * @param colour the colour for the after edge
        ' */
        Public Sub setAfterColour(ByVal colour As PDGamma)
            Me.setColourByIndex(1, colour)
        End Sub

        '/**
        ' * Gets the colour for the start edge.
        ' * 
        ' * @return the colour for the start edge
        ' */
        Public Function getStartColour() As PDGamma
            Return Me.getColourByIndex(2)
        End Function

        '/**
        ' * Sets the colour for the start edge.
        ' * 
        ' * @param colour the colour for the start edge
        ' */
        Public Sub setStartColour(ByVal colour As PDGamma)
            Me.setColourByIndex(2, colour)
        End Sub

        '/**
        ' * Gets the colour for the end edge.
        ' * 
        ' * @return the colour for the end edge
        ' */
        Public Function getEndColour() As PDGamma
            Return Me.getColourByIndex(3)
        End Function

        '/**
        ' * Sets the colour for the end edge.
        ' * 
        ' * @param colour the colour for the end edge
        ' */
        Public Sub setEndColour(ByVal colour As PDGamma)
            Me.setColourByIndex(3, colour)
        End Sub


        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.array
        End Function


        '/**
        ' * Gets the colour by edge index.
        ' * 
        ' * @param index edge index
        ' * @return the colour
        ' */
        Private Function getColourByIndex(ByVal index As Integer) As PDGamma
            Dim retval As PDGamma = Nothing
            Dim item As COSBase = Me.array.getObject(index)
            If (TypeOf (item) Is COSArray) Then
                retval = New PDGamma(item)
            End If
            Return retval
        End Function

        '/**
        ' * Sets the colour by edge index.
        ' * 
        ' * @param index the edge index
        ' * @param colour the colour
        ' */
        Private Sub setColourByIndex(ByVal index As Integer, ByVal colour As PDGamma)
            Dim base As COSBase
            If (colour Is Nothing) Then
                base = COSNull.NULL
            Else
                base = colour.getCOSArray()
            End If
            Me.array.set(index, base)
        End Sub

    End Class

End Namespace
