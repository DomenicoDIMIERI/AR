Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This class holds all of the name trees that are available at the document level.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDDestinationNameTreeNode
        Inherits PDNameTreeNode

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New(GetType(PDPageDestination))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dic The COS dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic, GetType(PDPageDestination))
        End Sub

        Protected Overrides Function convertCOSToPD(ByVal base As COSBase) As COSObjectable 'throws IOException
            Dim destination As COSBase = base
            If (TypeOf (base) Is COSDictionary) Then
                'the destination is sometimes stored in the D dictionary
                'entry instead of being directly an array, so just dereference
                'it for now
                destination = DirectCast(base, COSDictionary).getDictionaryObject(COSName.D)
            End If
            Return PDDestination.create(destination)
        End Function

  
        Protected Overrides Function createChildNode(ByVal dic As COSDictionary) As PDNameTreeNode
            Return New PDDestinationNameTreeNode(dic)
        End Function

    End Class

End Namespace
