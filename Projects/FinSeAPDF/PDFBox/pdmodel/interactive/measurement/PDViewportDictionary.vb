Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.measurement

    '/**
    ' * This class represents a viewport dictionary.
    ' * 
    ' * @version $Revision: 1.0 $
    ' *
    ' */
    Public Class PDViewportDictionary
        Implements COSObjectable

        ''' <summary>
        ''' The type of this annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TYPE = "Viewport"

        Private viewportDictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Me.viewportDictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            Me.viewportDictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.viewportDictionary
        End Function

        '/**
        ' * This will return the corresponding dictionary.
        ' * 
        ' * @return the viewport dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return Me.viewportDictionary
        End Function

        '/**
        ' * Returns the type of the viewport dictionary.
        ' * It must be "Viewport"
        ' * @return the type of the external data dictionary
        ' */

        Public Function getVPType() As String
            Return TYPE
        End Function

        '/**
        ' * This will retrieve the rectangle specifying the location of the viewport.
        ' * 
        ' * @return the location
        ' */
        Public Function getBBox() As PDRectangle
            Dim bbox As COSArray = Me.getDictionary().getDictionaryObject("BBox")
            If (bbox IsNot Nothing) Then
                Return New PDRectangle(bbox)
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the rectangle specifying the location of the viewport.
        ' * 
        ' * @param rectangle the rectangle specifying the location.
        ' */
        Public Sub setBBox(ByVal rectangle As PDRectangle)
            Me.getDictionary().setItem("BBox", rectangle)
        End Sub

        '/**
        ' * This will retrieve the name of the viewport.
        ' * 
        ' * @return the name of the viewport
        ' */
        Public Function getName() As String
            Return Me.getDictionary().getNameAsString(COSName.NAME)
        End Function

        '/**
        ' * This will set the name of the viewport.
        ' *  
        ' * @param name the name of the viewport
        ' */
        Public Sub setName(ByVal name As String)
            Me.getDictionary().setName(COSName.NAME, name)
        End Sub

        '/**
        ' * This will retrieve the measure dictionary.
        ' * 
        ' * @return the measure dictionary
        ' */
        Public Function getMeasure() As PDMeasureDictionary
            Dim measure As COSDictionary = Me.getDictionary().getDictionaryObject("Measure")
            If (measure IsNot Nothing) Then
                Return New PDMeasureDictionary(measure)
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the measure dictionary.
        ' * 
        ' * @param measure the measure dictionary
        ' */
        Public Sub setMeasure(ByVal measure As PDMeasureDictionary)
            Me.getDictionary().setItem("Measure", measure)
        End Sub

    End Class

End Namespace