Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure

    '/**
    ' * The MarkInfo provides additional information relevant to specialized
    ' * uses of structured documents.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDMarkInfo
        Implements COSObjectable

        Private dictionary As COSDictionary

        '/**
        ' * Default Constructor.
        ' *
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor for an existing MarkInfo element.
        ' *
        ' * @param dic The existing dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            dictionary = dic
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dictionary
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * Tells if this is a tagged PDF.
        ' *
        ' * @return true If this is a tagged PDF.
        ' */
        Public Function isMarked() As Boolean
            Return dictionary.getBoolean("Marked", False)
        End Function

        '/**
        ' * Set if this is a tagged PDF.
        ' *
        ' * @param value The new marked value.
        ' */
        Public Sub setMarked(ByVal value As Boolean)
            dictionary.setBoolean("Marked", value)
        End Sub

        '/**
        ' * Tells if structure elements use user properties.
        ' *
        ' * @return A boolean telling if the structure elements use user properties.
        ' */
        Public Function usesUserProperties() As Boolean
            Return dictionary.getBoolean("UserProperties", False)
        End Function

        '/**
        ' * Set if the structure elements contain user properties.
        ' *
        ' * @param userProps The new value for this property.
        ' */
        Public Sub setUserProperties(ByVal userProps As Boolean)
            dictionary.setBoolean("UserProperties", userProps)
        End Sub

        '/**
        ' * Tells if this PDF contain 'suspect' tags.  See PDF Reference 1.6
        ' * section 10.6 "Logical Structure" for more information about this property.
        ' *
        ' * @return true if the suspect flag has been set.
        ' */
        Public Function isSuspect() As Boolean
            Return dictionary.getBoolean("Suspects", False)
        End Function

        '/**
        ' * Set the value of the suspects property.  See PDF Reference 1.6
        ' * section 10.6 "Logical Structure" for more information about this
        ' * property.
        ' *
        ' * @param suspect The new "Suspects" value.
        ' */
        Public Sub setSuspect(ByVal suspect As Boolean)
            dictionary.setBoolean("Suspects", False)
        End Sub
    End Class

End Namespace
