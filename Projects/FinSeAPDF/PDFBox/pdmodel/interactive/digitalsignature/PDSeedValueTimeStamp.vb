Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    '/**
    ' * If exist, it describe where the signature handler can request a rfc3161
    ' * timestamp and if it is a must have for the signature.
    ' *
    ' * @author Thomas Chojecki
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDSeedValueTimeStamp

        Private dictionary As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            dictionary.setDirect(True)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The signature dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
            dictionary.setDirect(True)
        End Sub


        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase
            Return getDictionary()
        End Function

        '/**
        ' * Convert this standard java object to a COS dictionary.
        ' *
        ' * @return The COS dictionary that matches this Java object.
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * Returns the URL.
        ' * 
        ' * @return the URL
        ' */
        Public Function getURL() As String
            Return dictionary.getString(COSName.URL)
        End Function

        '/**
        ' * Sets the URL.
        ' * @param url the URL to be set as URL
        ' */
        Public Sub setURL(ByVal url As String)
            dictionary.setString(COSName.URL, url)
        End Sub

        '/**
        ' * Indicates if a timestamp is required.
        ' * 
        ' * @return true if a timestamp is required
        ' */
        Public Function isTimestampRequired() As Boolean
            Return dictionary.getInt(COSName.FT, 0) <> 0
        End Function

        '/**
        ' * Sets if a timestamp is reuqired or not.
        ' * 
        ' * @param flag true if a timestamp is required
        ' */
        Public Sub setTimestampRequired(ByVal flag As Boolean)
            dictionary.setInt(COSName.FT, IIf(flag, 1, 0))
        End Sub

    End Class

End Namespace
