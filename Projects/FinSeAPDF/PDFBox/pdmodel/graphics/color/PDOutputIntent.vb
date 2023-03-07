Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.color

    Public Class PDOutputIntent
        Implements COSObjectable

        Private dictionary As COSDictionary

        Public Sub New(ByVal doc As PDDocument, ByVal colorProfile As InputStream) 'throws Exception{ 
            dictionary = New COSDictionary()
            dictionary.setItem(COSName.TYPE, COSName.OUTPUT_INTENT)
            dictionary.setItem(COSName.S, COSName.GTS_PDFA1)
            Dim destOutputIntent As PDStream = configureOutputProfile(doc, colorProfile)
            dictionary.setItem(COSName.DEST_OUTPUT_PROFILE, destOutputIntent)
        End Sub

        Public Sub New(ByVal dictionary As COSDictionary)
            Me.dictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dictionary
        End Function

        Public Function getDestOutputIntent() As COSStream
            Return dictionary.getItem(COSName.DEST_OUTPUT_PROFILE)
        End Function

        Public Function getInfo() As String
            Return dictionary.getString(COSName.INFO)
        End Function

        Public Sub setInfo(ByVal value As String)
            dictionary.setString(COSName.INFO, value)
        End Sub

        Public Function getOutputCondition() As String
            Return dictionary.getString(COSName.OUTPUT_CONDITION)
        End Function

        Public Sub setOutputCondition(ByVal value As String)
            dictionary.setString(COSName.OUTPUT_CONDITION, value)
        End Sub

        Public Function getOutputConditionIdentifier() As String
            Return dictionary.getString(COSName.OUTPUT_CONDITION_IDENTIFIER)
        End Function

        Public Sub setOutputConditionIdentifier(ByVal value As String)
            dictionary.setString(COSName.OUTPUT_CONDITION_IDENTIFIER, value)
        End Sub

        Public Function getRegistryName() As String
            Return dictionary.getString(COSName.REGISTRY_NAME)
        End Function

        Public Sub setRegistryName(ByVal value As String)
            dictionary.setString(COSName.REGISTRY_NAME, value)
        End Sub

        Private Function configureOutputProfile(ByVal doc As PDDocument, ByVal colorProfile As InputStream) As PDStream
            Dim stream As PDStream = New PDStream(doc, colorProfile, False)
            stream.getStream().setFilters(COSName.FLATE_DECODE)
            stream.getStream().setInt(COSName.LENGTH, stream.getByteArray().Length)
            stream.getStream().setInt(COSName.N, 3)
            stream.addCompression()
            Return stream
        End Function


    End Class

End Namespace