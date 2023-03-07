Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.outline

    '/**
    ' * This represents an outline in a pdf document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDDocumentOutline
        Inherits PDOutlineNode

        '/**
        ' * Default Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            node.setName("Type", "Outlines")
        End Sub

        '/**
        ' * Constructor for an existing document outline.
        ' *
        ' * @param dic The storage dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic)
        End Sub

    End Class


End Namespace
