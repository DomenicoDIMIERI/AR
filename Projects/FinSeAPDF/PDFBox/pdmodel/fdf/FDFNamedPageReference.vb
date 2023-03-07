Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents an FDF named page reference that is part of the FDF field.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class FDFNamedPageReference
        Implements COSObjectable

        Private ref As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            ref = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param r The FDF named page reference dictionary.
        ' */
        Public Sub New(ByVal r As COSDictionary)
            ref = r
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return ref
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return ref
        End Function

        '/**
        ' * This will get the name of the referenced page.  A required parameter.
        ' *
        ' * @return The name of the referenced page.
        ' */
        Public Function getName() As String
            Return ref.getString("Name")
        End Function

        '/**
        ' * This will set the name of the referenced page.
        ' *
        ' * @param name The referenced page name.
        ' */
        Public Sub setName(ByVal name As String)
            ref.setString("Name", name)
        End Sub

        '/**
        ' * This will get the file specification of this reference.  An optional parameter.
        ' *
        ' * @return The F entry for this dictionary.
        ' *
        ' * @throws IOException If there is an error creating the file spec.
        ' */
        Public Function getFileSpecification() As PDFileSpecification 'throws IOException
            Return PDFileSpecification.createFS(ref.getDictionaryObject("F"))
        End Function

        '/**
        ' * This will set the file specification for this named page reference.
        ' *
        ' * @param fs The file specification to set.
        ' */
        Public Sub setFileSpecification(ByVal fs As PDFileSpecification)
            ref.setItem("F", fs)
        End Sub


    End Class

End Namespace
