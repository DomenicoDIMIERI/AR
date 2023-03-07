Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This class holds all of the name trees that are available at the document level.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDJavascriptNameTreeNode
        Inherits PDNameTreeNode

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New(GetType(PDTextStream))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dic The COS dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic, GetType(PDTextStream))
        End Sub

        Protected Overrides Function convertCOSToPD(ByVal base As COSBase) As COSObjectable
            Dim stream As PDTextStream = Nothing
            If (TypeOf (base) Is COSString) Then
                stream = New PDTextStream(DirectCast(base, COSString))
            ElseIf (TypeOf (base) Is COSStream) Then
                stream = New PDTextStream(DirectCast(base, COSStream))
            Else
                Throw New IOException("Error creating Javascript object, expected either COSString or COSStream and not " & base.ToString)
            End If
            Return stream
        End Function

        Protected Overrides Function createChildNode(ByVal dic As COSDictionary) As PDNameTreeNode
            Return New PDJavascriptNameTreeNode(dic)
        End Function


    End Class

End Namespace
