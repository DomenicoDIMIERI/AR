Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents an FDF JavaScript dictionary that is part of the FDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class FDFJavaScript
        Implements COSObjectable

        Private js As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            js = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param javaScript The FDF java script.
        ' */
        Public Sub New(ByVal javaScript As COSDictionary)
            js = javaScript
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return js
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return js
        End Function

        '/**
        ' * This will get the javascript that is executed before the import.
        ' *
        ' * @return Some javascript code.
        ' */
        Public Function getBefore() As PDTextStream
            Return PDTextStream.createTextStream(js.getDictionaryObject("Before"))
        End Function

        '/**
        ' * This will set the javascript code the will get execute before the import.
        ' *
        ' * @param before A reference to some javascript code.
        ' */
        Public Sub setBefore(ByVal before As PDTextStream)
            js.setItem("Before", before)
        End Sub

        '/**
        ' * This will get the javascript that is executed after the import.
        ' *
        ' * @return Some javascript code.
        ' */
        Public Function getAfter() As PDTextStream
            Return PDTextStream.createTextStream(js.getDictionaryObject("After"))
        End Function

        '/**
        ' * This will set the javascript code the will get execute after the import.
        ' *
        ' * @param after A reference to some javascript code.
        ' */
        Public Sub setAfter(ByVal after As PDTextStream)
            js.setItem("After", after)
        End Sub

        '/**
        ' * This will return a list of PDNamedTextStream objects.  This is the "Doc"
        ' * entry of the pdf document.  These will be added to the PDF documents
        ' * javascript name tree.  This will not return null.
        ' *
        ' * @return A list of all named javascript entries.
        ' */
        Public Function getNamedJavaScripts() As List
            Dim array As COSArray = js.getDictionaryObject("Doc")
            Dim namedStreams As List = New ArrayList()
            If (array Is Nothing) Then
                array = New COSArray()
                js.setItem("Doc", array)
            End If
            For i As Integer = 0 To array.size() - 1
                Dim name As COSName = array.get(i)
                i += 1
                Dim stream As COSBase = array.get(i)
                Dim namedStream As PDNamedTextStream = New PDNamedTextStream(name, stream)
                namedStreams.add(namedStream)
            Next
            Return New COSArrayList(namedStreams, array)
        End Function

        '/**
        ' * This should be a list of PDNamedTextStream objects.
        ' *
        ' * @param namedStreams The named streams.
        ' */
        Public Sub setNamedJavaScripts(ByVal namedStreams As List)
            Dim array As COSArray = COSArrayList.converterToCOSArray(namedStreams)
            js.setItem("Doc", array)
        End Sub

    End Class

End Namespace
