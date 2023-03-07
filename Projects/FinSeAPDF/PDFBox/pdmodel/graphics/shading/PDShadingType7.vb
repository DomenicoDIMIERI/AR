Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This represents resources for a shading type 7 (Tensor-Product Patch Meshes).
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDShadingType7
        Inherits PDShadingType4

        '/**
        ' * Constructor using the given shading dictionary.
        ' *
        ' * @param shadingDictionary The dictionary for this shading.
        ' */
        Public Sub New(ByVal shadingDictionary As COSDictionary)
            MyBase.New(shadingDictionary)
        End Sub

        Public Overrides Function getShadingType() As Integer
            Return PDShadingResources.SHADING_TYPE6
        End Function

    End Class

End Namespace
