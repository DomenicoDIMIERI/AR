Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common


Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    '* This class represents a PDF /BE entry the border effect dictionary.
    '*
    '* @author Paul King
    '* @version $Revision: 1.1 $
    '*/
    Public Class PDBorderEffectDictionary
        Implements COSObjectable

        '/*
        ' * The various values of the effect applied to the border as defined in the
        ' * PDF 1.6 reference Table 8.14
        ' */

        ''' <summary>
        ''' Constant for the name for no effect.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_SOLID As String = "S"

        ''' <summary>
        '''  Constant for the name of a cloudy effect.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_CLOUDY As String = "C"

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
        ' * This will set the intensity of the applied effect.
        ' *
        ' * @param i
        ' *            the intensity of the effect values 0 to 2
        ' */
        Public Sub setIntensity(ByVal i As Single)
            getDictionary().setFloat("I", i)
        End Sub

        '/**
        ' * This will retrieve the intensity of the applied effect.
        ' *
        ' * @return the intensity value 0 to 2
        ' */
        Public Function getIntensity() As Single
            Return getDictionary().getFloat("I", 0)
        End Function

        '/**
        ' * This will set the border effect, see the STYLE_* constants for valid values.
        ' *
        ' * @param s
        ' *            the border effect to use
        ' */
        Public Sub setStyle(ByVal s As String)
            getDictionary().setName("S", s)
        End Sub

        '/**
        ' * This will retrieve the border effect, see the STYLE_* constants for valid
        ' * values.
        ' *
        ' * @return the effect of the border
        ' */
        Public Function getStyle() As String
            Return getDictionary().getNameAsString("S", STYLE_SOLID)
        End Function

    End Class

End Namespace
