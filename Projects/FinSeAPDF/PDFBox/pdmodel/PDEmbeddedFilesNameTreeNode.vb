Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This class holds all of the name trees that are available at the document level.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDEmbeddedFilesNameTreeNode
        Inherits PDNameTreeNode

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New(GetType(PDComplexFileSpecification))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dic The COS dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic, GetType(PDComplexFileSpecification))
        End Sub

        Protected Overrides Function convertCOSToPD(ByVal base As COSBase) As COSObjectable
            Return New PDComplexFileSpecification(base)
        End Function


        Protected Overrides Function createChildNode(ByVal dic As COSDictionary) As PDNameTreeNode
            Return New PDEmbeddedFilesNameTreeNode(dic)
        End Function

    End Class

End Namespace
