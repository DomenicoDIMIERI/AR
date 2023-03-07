Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * A wrapper for a COS dictionary including Type information.
    ' *
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' *
    ' */
    Public Class PDTypedDictionaryWrapper
        Inherits PDDictionaryWrapper

        '/**
        ' * Creates a new instance with a given type.
        ' * 
        ' * @param type the type (Type)
        ' */
        Public Sub New(ByVal type As String)
            MyBase.New()
            Me.getCOSDictionary().setName(COSName.TYPE, type)
        End Sub

        '/**
        ' * Creates a new instance with a given COS dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Gets the type.
        ' * 
        ' * @return the type
        ' */
        Public Function getDictType() As String
            Return Me.getCOSDictionary().getNameAsString(COSName.TYPE)
        End Function

        ' There is no setType(String) method because changing the Type would most
        ' probably also change the type of PD object.
    End Class

End Namespace
