Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common


Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents an FDF page info that is part of the FDF page.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class FDFPageInfo
        Implements COSObjectable

        Private pageInfo As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            pageInfo = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param p The FDF page.
        ' */
        Public Sub New(ByVal p As COSDictionary)
            pageInfo = p
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return pageInfo
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return pageInfo
        End Function

    End Class

End Namespace
