Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This is the document metadata.  Each getXXX method will return the entry if
    ' * it exists or null if it does not exist.  If you pass in null for the setXXX
    ' * method then it will clear the value.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.12 $
    ' */
    Public Class PDDocumentInformation
        Implements COSObjectable

        Private info As COSDictionary

        '/**
        ' * Default Constructor.
        ' */
        Public Sub New()
            info = New COSDictionary()
        End Sub

        '/**
        ' * Constructor that is used for a preexisting dictionary.
        ' *
        ' * @param dic The underlying dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            info = dic
        End Sub

        '/**
        ' * This will get the underlying dictionary that this object wraps.
        ' *
        ' * @return The underlying info dictionary.
        ' */
        Public Function getDictionary() As COSDictionary
            Return info
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return info
        End Function

        '/**
        ' * This will get the title of the document.  This will return null if no title exists.
        ' *
        ' * @return The title of the document.
        ' */
        Public Function getTitle() As String
            Return info.getString(COSName.TITLE)
        End Function

        '/**
        ' * This will set the title of the document.
        ' *
        ' * @param title The new title for the document.
        ' */
        Public Sub setTitle(ByVal title As String)
            info.setString(COSName.TITLE, title)
        End Sub

        '/**
        ' * This will get the author of the document.  This will return null if no author exists.
        ' *
        ' * @return The author of the document.
        ' */
        Public Function getAuthor() As String
            Return info.getString(COSName.AUTHOR)
        End Function

        '/**
        ' * This will set the author of the document.
        ' *
        ' * @param author The new author for the document.
        ' */
        Public Sub setAuthor(ByVal author As String)
            info.setString(COSName.AUTHOR, author)
        End Sub

        '/**
        ' * This will get the subject of the document.  This will return null if no subject exists.
        ' *
        ' * @return The subject of the document.
        ' */
        Public Function getSubject() As String
            Return info.getString(COSName.SUBJECT)
        End Function

        '/**
        ' * This will set the subject of the document.
        ' *
        ' * @param subject The new subject for the document.
        ' */
        Public Sub setSubject(ByVal subject As String)
            info.setString(COSName.SUBJECT, subject)
        End Sub

        '/**
        ' * This will get the keywords of the document.  This will return null if no keywords exists.
        ' *
        ' * @return The keywords of the document.
        ' */
        Public Function getKeywords() As String
            Return info.getString(COSName.KEYWORDS)
        End Function

        '/**
        ' * This will set the keywords of the document.
        ' *
        ' * @param keywords The new keywords for the document.
        ' */
        Public Sub setKeywords(ByVal keywords As String)
            info.setString(COSName.KEYWORDS, keywords)
        End Sub

        '/**
        ' * This will get the creator of the document.  This will return null if no creator exists.
        ' *
        ' * @return The creator of the document.
        ' */
        Public Function getCreator() As String
            Return info.getString(COSName.CREATOR)
        End Function

        '/**
        ' * This will set the creator of the document.
        ' *
        ' * @param creator The new creator for the document.
        ' */
        Public Sub setCreator(ByVal creator As String)
            info.setString(COSName.CREATOR, creator)
        End Sub

        '/**
        ' * This will get the producer of the document.  This will return null if no producer exists.
        ' *
        ' * @return The producer of the document.
        ' */
        Public Function getProducer() As String
            Return info.getString(COSName.PRODUCER)
        End Function

        '/**
        ' * This will set the producer of the document.
        ' *
        ' * @param producer The new producer for the document.
        ' */
        Public Sub setProducer(ByVal producer As String)
            info.setString(COSName.PRODUCER, producer)
        End Sub

        '/**
        ' * This will get the creation date of the document.  This will return null if no creation date exists.
        ' *
        ' * @return The creation date of the document.
        ' *
        ' * @throws IOException If there is an error creating the date.
        ' */
        Public Function getCreationDate() As NDate ' throws IOException
            Return info.getDate(COSName.CREATION_DATE)
        End Function

        '/**
        ' * This will set the creation date of the document.
        ' *
        ' * @param date The new creation date for the document.
        ' */
        Public Sub setCreationDate(ByVal [date] As NDate)
            info.setDate(COSName.CREATION_DATE, [date])
        End Sub

        '/**
        ' * This will get the modification date of the document.  This will return null if no modification date exists.
        ' *
        ' * @return The modification date of the document.
        ' *
        ' * @throws IOException If there is an error creating the date.
        ' */
        Public Function getModificationDate() As NDate ' throws IOException
            Return info.getDate(COSName.MOD_DATE)
        End Function

        '/**
        ' * This will set the modification date of the document.
        ' *
        ' * @param date The new modification date for the document.
        ' */
        Public Sub setModificationDate(ByVal [date] As NDate)
            info.setDate(COSName.MOD_DATE, [date])
        End Sub

        '/**
        ' * This will get the trapped value for the document.
        ' * This will return null if one is not found.
        ' *
        ' * @return The trapped value for the document.
        ' */
        Public Function getTrapped() As String
            Return info.getNameAsString(COSName.TRAPPED)
        End Function

        '/**
        ' * This will get the keys of all metadata information fields for the document.
        ' *
        ' * @return all metadata key strings.
        ' * @since Apache PDFBox 1.3.0
        ' */
        Public Function getMetadataKeys() As [Set](Of String)
            Dim keys As [Set](Of String) = New TreeSet(Of String)()
            For Each key As COSName In info.keySet()
                keys.add(key.getName())
            Next
            Return keys
        End Function

        '/**
        ' *  This will get the value of a custom metadata information field for the document.
        ' *  This will return null if one is not found.
        ' *
        ' * @param fieldName Name of custom metadata field from pdf document.
        ' *
        ' * @return String Value of metadata field
        ' *
        ' * @author  Gerardo Ortiz
        ' */
        Public Function getCustomMetadataValue(ByVal fieldName As String) As String
            Return info.getString(fieldName)
        End Function

        '/**
        ' * Set the custom metadata value.
        ' *
        ' * @param fieldName The name of the custom metadata field.
        ' * @param fieldValue The value to the custom metadata field.
        ' */
        Public Sub setCustomMetadataValue(ByVal fieldName As String, ByVal fieldValue As String)
            info.setString(fieldName, fieldValue)
        End Sub

        '/**
        ' * This will set the trapped of the document.  This will be
        ' * 'True', 'False', or 'Unknown'.
        ' *
        ' * @param value The new trapped value for the document.
        ' */
        Public Sub setTrapped(ByVal value As String)
            If (value IsNot Nothing AndAlso Not value.Equals("True") AndAlso Not value.Equals("False") AndAlso Not value.Equals("Unknown")) Then
                Throw New ArgumentOutOfRangeException("Valid values for trapped are 'True', 'False', or 'Unknown'")
            End If
            info.setName(COSName.TRAPPED, value)
        End Sub

    End Class

End Namespace
