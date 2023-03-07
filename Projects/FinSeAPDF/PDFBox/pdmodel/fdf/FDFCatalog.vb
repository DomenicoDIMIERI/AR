Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature
'import org.w3c.dom.Element;

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents an FDF catalog that is part of the FDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class FDFCatalog
        Implements COSObjectable

        Private catalog As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            catalog = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param cat The FDF documents catalog.
        ' */
        Public Sub New(ByVal cat As COSDictionary)
            catalog = cat
        End Sub

        '/**
        ' * This will create an FDF catalog from an XFDF XML document.
        ' *
        ' * @param element The XML document that contains the XFDF data.
        ' * @throws IOException If there is an error reading from the dom.
        ' */
        Public Sub New(ByVal element As System.Xml.XmlElement) 'throws IOException
            Me.New()
            Dim fdfDict As FDFDictionary = New FDFDictionary(element)
            setFDF(fdfDict)
        End Sub

        '/**
        ' * This will write this element as an XML document.
        ' *
        ' * @param output The stream to write the xml to.
        ' *
        ' * @throws IOException If there is an error writing the XML.
        ' */
        Public Sub writeXML(ByVal output As Finsea.Io.Writer) 'throws IOException
            Dim fdf As FDFDictionary = getFDF()
            fdf.writeXML(output)
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return catalog
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return catalog
        End Function

        '/**
        ' * This will get the version that was specified in the catalog dictionary.
        ' *
        ' * @return The FDF version.
        ' */
        Public Function getVersion() As String
            Return catalog.getNameAsString("Version")
        End Function

        '/**
        ' * This will set the version of the FDF document.
        ' *
        ' * @param version The new version for the FDF document.
        ' */
        Public Sub setVersion(ByVal version As String)
            catalog.setName("Version", version)
        End Sub

        '/**
        ' * This will get the FDF dictionary.
        ' *
        ' * @return The FDF dictionary.
        ' */
        Public Function getFDF() As FDFDictionary
            Dim fdf As COSDictionary = catalog.getDictionaryObject("FDF")
            Dim retval As FDFDictionary = Nothing
            If (fdf IsNot Nothing) Then
                retval = New FDFDictionary(fdf)
            Else
                retval = New FDFDictionary()
                setFDF(retval)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the FDF document.
        ' *
        ' * @param fdf The new FDF dictionary.
        ' */
        Public Sub setFDF(ByVal fdf As FDFDictionary)
            catalog.setItem("FDF", fdf)
        End Sub

        '/**
        ' * This will get the signature or null if there is none.
        ' *
        ' * @return The signature.
        ' */
        Public Function getSignature() As PDSignature
            Dim signature As PDSignature = Nothing
            Dim sig As COSDictionary = catalog.getDictionaryObject("Sig")
            If (sig IsNot Nothing) Then
                signature = New PDSignature(sig)
            End If
            Return signature
        End Function

        '/**
        ' * This will set the signature that is associated with this catalog.
        ' *
        ' * @param sig The new signature.
        ' */
        Public Sub setSignature(ByVal sig As PDSignature)
            catalog.setItem("Sig", sig)
        End Sub
    End Class

End Namespace
