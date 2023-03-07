Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.fdf

    '/**
    ' * This represents an object that can be used in a Field's Opt entry to represent
    ' * an available option and a default appearance string.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class FDFOptionElement
        Implements COSObjectable

        Private [option] As COSArray

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            [option] = New COSArray()
            [option].add(New COSString(""))
            [option].add(New COSString(""))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param o The option element.
        ' */
        Public Sub New(ByVal o As COSArray)
            [option] = o
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return [option]
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSArray() As COSArray
            Return [option]
        End Function

        '/**
        ' * This will get the string of one of the available options.  A required element.
        ' *
        ' * @return An available option.
        ' */
        Public Function getOption() As String
            Return DirectCast([option].getObject(0), COSString).getString()
        End Function

        '/**
        ' * This will set the string for an available option.
        ' *
        ' * @param opt One of the available options.
        ' */
        Public Sub setOption(ByVal opt As String)
            [option].set(0, New COSString(opt))
        End Sub

        '/**
        ' * This will get the string of default appearance string.  A required element.
        ' *
        ' * @return A default appearance string.
        ' */
        Public Function getDefaultAppearanceString() As String
            Return DirectCast([option].getObject(1), COSString).getString()
        End Function

        '/**
        ' * This will set the default appearance string.
        ' *
        ' * @param da The default appearance string.
        ' */
        Public Sub setDefaultAppearanceString(ByVal da As String)
            [option].set(1, New COSString(da))
        End Sub


    End Class

End Namespace