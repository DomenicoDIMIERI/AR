Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    '/**
    ' * <p>This represents a pdf signature build dictionary as specified in
    ' * <a href="http://partners.adobe.com/public/developer/en/acrobat/Acrobat_Signature_BuildDict.pdf">
    ' * http://partners.adobe.com/public/developer/en/acrobat/Acrobat_Signature_BuildDict.pdf</a></p>
    ' *
    ' * <p>The signature build properties dictionary provides signature properties for the software
    ' * application that was used to create the signature.</p>
    ' *
    ' * @author Thomas Chojecki
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDPropBuild
        Implements COSObjectable

        Private dictionary As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            dictionary.setDirect(True) ' the specification claim to use direct objects
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The signature dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
            dictionary.setDirect(True) ' the specification claim to use direct objects
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
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
        ' * A build data dictionary for the signature handler that was
        ' * used to create the parent signature.
        ' *
        ' * @return the Filter as PDPropBuildFilter object
        ' */
        Public Function getFilter() As PDPropBuildDataDict
            Dim filter As PDPropBuildDataDict = Nothing
            Dim filterDic As COSDictionary = dictionary.getDictionaryObject(COSName.FILTER)
            If (filterDic IsNot Nothing) Then
                filter = New PDPropBuildDataDict(filterDic)
            End If
            Return filter
        End Function

        '/**
        ' * Set the build data dictionary for the signature handler.
        ' * This entry is optional but is highly recommended for the signatures.
        ' *
        ' * @param filter is the PDPropBuildFilter
        ' */
        Public Sub setPDPropBuildFilter(ByVal filter As PDPropBuildDataDict)
            dictionary.setItem(COSName.FILTER, filter)
        End Sub

        '/**
        ' * A build data dictionary for the PubSec software module
        ' * that was used to create the parent signature.
        ' *
        ' * @return the PubSec as PDPropBuildPubSec object
        ' */
        Public Function getPubSec() As PDPropBuildDataDict
            Dim pubSec As PDPropBuildDataDict = Nothing
            Dim pubSecDic As COSDictionary = dictionary.getDictionaryObject(COSName.PUB_SEC)
            If (pubSecDic IsNot Nothing) Then
                pubSec = New PDPropBuildDataDict(pubSecDic)
            End If
            Return pubSec
        End Function

        '/**
        ' * Set the build data dictionary for the PubSec Software module.
        ' *
        ' * @param pubSec is the PDPropBuildPubSec
        ' */
        Public Sub setPDPropBuildPubSec(ByVal pubSec As PDPropBuildDataDict)
            dictionary.setItem(COSName.PUB_SEC, pubSec)
        End Sub

        '/**
        ' * A build data dictionary for the viewing application software
        ' * module that was used to create the parent signature.
        ' *
        ' * @return the App as PDPropBuildApp object
        ' */
        Public Function getApp() As PDPropBuildDataDict
            Dim app As PDPropBuildDataDict = Nothing
            Dim appDic As COSDictionary = dictionary.getDictionaryObject(COSName.APP)
            If (appDic IsNot Nothing) Then
                app = New PDPropBuildDataDict(appDic)
            End If
            Return app
        End Function

        '/**
        ' * Set the build data dictionary for the viewing application
        ' * software module.
        ' *
        ' * @param app is the PDPropBuildApp
        ' */
        Public Sub setPDPropBuildApp(ByVal app As PDPropBuildDataDict)
            dictionary.setItem(COSName.APP, app)
        End Sub

    End Class

End Namespace
