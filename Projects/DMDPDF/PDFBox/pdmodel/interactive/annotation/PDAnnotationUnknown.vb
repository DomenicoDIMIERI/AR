Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents an arbitary Unknown Annotation type.
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDAnnotationUnknown
        Inherits PDAnnotation

        '/**
        '  * Creates an arbitary annotation from a COSDictionary, expected to be
        '  * a correct object definition for some sort of annotation.
        '  *
        '  * @param dic The dictionary which represents this Annotation.
        '  */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic)
        End Sub

    End Class

End Namespace
