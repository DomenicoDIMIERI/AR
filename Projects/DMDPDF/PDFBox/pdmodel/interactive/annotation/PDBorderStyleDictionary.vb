Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This class represents a PDF /BS entry the border style dictionary.
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDBorderStyleDictionary
        Implements COSObjectable

        '/*
        ' * The various values of the style for the border as defined in the PDF 1.6
        ' * reference Table 8.13
        ' */

        ''' <summary>
        ''' Constant for the name of a solid style.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_SOLID = "S"

        ''' <summary>
        ''' Constant for the name of a dashed style.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_DASHED = "D"

        ''' <summary>
        ''' Constant for the name of a beveled style.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_BEVELED = "B"

        ''' <summary>
        ''' Constant for the name of a inset style.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_INSET = "I"

        ''' <summary>
        ''' Constant for the name of a underline style.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_UNDERLINE = "U"

        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict
        ' *            a border style dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
        End Sub

        '/**
        ' * returns the dictionary.
        ' *
        ' * @return the dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * returns the dictionary.
        ' *
        ' * @return the dictionary
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dictionary
        End Function

        '/**
        ' * This will set the border width in points, 0 = no border.
        ' *
        ' * @param w
        ' *            Single the width in points
        ' */
        Public Sub setWidth(ByVal w As Single)
            getDictionary().setFloat("W", w)
        End Sub

        '/**
        ' * This will retrieve the border width in points, 0 = no border.
        ' *
        ' * @return flaot the width of the border in points
        ' */
        Public Function getWidth() As Single
            Return getDictionary().getFloat("W", 1)
        End Function

        '/**
        ' * This will set the border style, see the STYLE_* constants for valid values.
        ' *
        ' * @param s
        ' *            the border style to use
        ' */
        Public Sub setStyle(ByVal s As String)
            getDictionary().setName("S", s)
        End Sub

        '/**
        ' * This will retrieve the border style, see the STYLE_* constants for valid
        ' * values.
        ' *
        ' * @return the style of the border
        ' */
        Public Function getStyle() As String
            Return getDictionary().getNameAsString("S", STYLE_SOLID)
        End Function

        '/**
        ' * This will set the dash style used for drawing the border.
        ' *
        ' * @param d
        ' *            the dash style to use
        ' */
        Public Sub setDashStyle(ByVal d As PDLineDashPattern)
            Dim array As COSArray = Nothing
            If (d IsNot Nothing) Then
                array = d.getCOSDashPattern()
            End If
            getDictionary().setItem("D", array)
        End Sub

        '/**
        ' * This will retrieve the dash style used for drawing the border.
        ' *
        ' * @return the dash style of the border
        ' */
        Public Function getDashStyle() As PDLineDashPattern
            Dim d As COSArray = getDictionary().getDictionaryObject("D")
            If (d Is Nothing) Then
                d = New COSArray()
                d.add(COSInteger.THREE)
                getDictionary().setItem("D", d)
            End If
            Return New PDLineDashPattern(d, 0)
        End Function

    End Class

End Namespace
