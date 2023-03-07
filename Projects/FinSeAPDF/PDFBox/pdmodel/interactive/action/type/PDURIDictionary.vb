Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.action.type


    '/**
    ' * This is the implementation of an URI dictionary.
    ' *
    ' * @version $Revision: 1.0 $
    ' *
    ' */
    Public Class PDURIDictionary
        Implements COSObjectable

        Private uriDictionary As COSDictionary

        '/**
        ' * Constructor.
        ' * 
        ' */
        Public Sub New()
            Me.uriDictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param dictionary the corresponding dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            Me.uriDictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.uriDictionary
        End Function

        '/**
        ' * Returns the corresponding dictionary.
        ' * @return the dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return Me.uriDictionary
        End Function

        '/**
        ' * This will get the base URI to be used in resolving relative URI references.
        ' * URI actions within the document may specify URIs in partial form, to be interpreted
        ' * relative to this base address. If no base URI is specified, such partial URIs
        ' * will be interpreted relative to the location of the document itself.
        ' * The use of this entry is parallel to that of the body element &lt;BASE&gt;, as described
        ' * in the HTML 4.01 Specification.
        ' *
        ' * @return The URI entry of the specific URI dictionary.
        ' */
        Public Function getBase() As String
            Return Me.getDictionary().getString("Base")
        End Function

        '/**
        ' * This will set the base URI to be used in resolving relative URI references.
        ' * URI actions within the document may specify URIs in partial form, to be interpreted
        ' * relative to this base address. If no base URI is specified, such partial URIs
        ' * will be interpreted relative to the location of the document itself.
        ' * The use of this entry is parallel to that of the body element &lt;BASE&gt;, as described
        ' * in the HTML 4.01 Specification.
        ' *
        ' * @param base The the base URI to be used.
        ' */
        Public Sub setBase(ByVal base As String)
            Me.getDictionary().setString("Base", base)
        End Sub

    End Class

End Namespace
