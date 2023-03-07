Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This represents resources for a radial shading.
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDShadingType3
        Inherits PDShadingType2

        '/**
        ' * Constructor using the given shading dictionary.
        ' *
        ' * @param shadingDictionary The dictionary for this shading.
        ' */
        Public Sub New(ByVal shadingDictionary As COSDictionary)
            MyBase.New(shadingDictionary)
        End Sub

        Public Overrides Function getShadingType() As Integer
            Return PDShadingResources.SHADING_TYPE3
        End Function

    End Class

End Namespace
