Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This class holds all of the name trees that are available at the document level.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDDocumentNameDictionary
        Implements COSObjectable

        Private nameDictionary As COSDictionary
        Private catalog As PDDocumentCatalog

        '/**
        ' * Constructor.
        ' *
        ' * @param cat The document catalog that this dictionary is part of.
        ' */
        Public Sub New(ByVal cat As PDDocumentCatalog)
            Dim names As COSBase = cat.getCOSDictionary().getDictionaryObject(COSName.NAMES)
            If (names IsNot Nothing) Then
                nameDictionary = names
            Else
                nameDictionary = New COSDictionary()
                cat.getCOSDictionary().setItem(COSName.NAMES, nameDictionary)
            End If
            catalog = cat
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param cat The document that this dictionary is part of.
        ' * @param names The names dictionary.
        ' */
        Public Sub New(ByVal cat As PDDocumentCatalog, ByVal names As COSDictionary)
            catalog = cat
            nameDictionary = names
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return nameDictionary
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos dictionary for this object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return nameDictionary
        End Function

        '/**
        ' * Get the destination named tree node.  The value in this name tree will be PDDestination
        ' * objects.
        ' *
        ' * @return The destination name tree node.
        ' */
        Public Function getDests() As PDDestinationNameTreeNode
            Dim dests As PDDestinationNameTreeNode = Nothing

            Dim dic As COSDictionary = nameDictionary.getDictionaryObject(COSName.DESTS)

            'The document catalog also contains the Dests entry sometimes
            'so check there as well.
            If (dic Is Nothing) Then
                dic = catalog.getCOSDictionary().getDictionaryObject(COSName.DESTS)
            End If

            If (dic IsNot Nothing) Then
                dests = New PDDestinationNameTreeNode(dic)
            End If

            Return dests
        End Function

        '/**
        ' * Set the named destinations that are associated with this document.
        ' *
        ' * @param dests The destination names.
        ' */
        Public Sub setDests(ByVal dests As PDDestinationNameTreeNode)
            nameDictionary.setItem(COSName.DESTS, dests)
            '//The dests can either be in the document catalog or in the
            '//names dictionary, PDFBox will just maintain the one in the
            '//names dictionary for now unless there is a reason to do
            '//something else.
            '//clear the potentially out of date Dests reference.
            catalog.getCOSDictionary().setItem(COSName.DESTS, DirectCast(Nothing, COSObjectable))
        End Sub

        '/**
        ' * Get the embedded files named tree node.  The value in this name tree will be PDComplexFileSpecification
        ' * objects.
        ' *
        ' * @return The embedded files name tree node.
        ' */
        Public Function getEmbeddedFiles() As PDEmbeddedFilesNameTreeNode
            Dim retval As PDEmbeddedFilesNameTreeNode = Nothing

            Dim dic As COSDictionary = nameDictionary.getDictionaryObject(COSName.EMBEDDED_FILES)

            If (dic IsNot Nothing) Then
                retval = New PDEmbeddedFilesNameTreeNode(dic)
            End If

            Return retval
        End Function

        '/**
        ' * Set the named embedded files that are associated with this document.
        ' *
        ' * @param ef The new embedded files
        ' */
        Public Sub setEmbeddedFiles(ByVal ef As PDEmbeddedFilesNameTreeNode)
            nameDictionary.setItem(COSName.EMBEDDED_FILES, ef)
        End Sub

        '/**
        ' * Get the document level javascript entries.  The value in this name tree will be PDTextStream.
        ' *
        ' * @return The document level named javascript.
        ' */
        Public Function getJavaScript() As PDJavascriptNameTreeNode
            Dim retval As PDJavascriptNameTreeNode = Nothing

            Dim dic As COSDictionary = nameDictionary.getDictionaryObject(COSName.JAVA_SCRIPT)

            If (dic IsNot Nothing) Then
                retval = New PDJavascriptNameTreeNode(dic)
            End If

            Return retval
        End Function

        '/**
        ' * Set the named javascript entries that are associated with this document.
        ' *
        ' * @param js The new Javascript entries.
        ' */
        Public Sub setJavascript(ByVal js As PDJavascriptNameTreeNode)
            nameDictionary.setItem(COSName.JAVA_SCRIPT, js)
        End Sub

    End Class

End Namespace
