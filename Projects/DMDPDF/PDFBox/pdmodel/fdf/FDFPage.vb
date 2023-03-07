Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.fdf

    '/**
    ' * This represents an FDF page that is part of the FDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class FDFPage
        Implements COSObjectable

        Private page As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            page = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param p The FDF page.
        ' */
        Public Sub New(ByVal p As COSDictionary)
            page = p
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return page
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return page
        End Function

        '/**
        ' * This will get a list of FDFTemplage objects that describe the named pages
        ' * that serve as templates.
        ' *
        ' * @return A list of templates.
        ' */
        Public Function getTemplates() As List
            Dim retval As List = Nothing
            Dim array As COSArray = page.getDictionaryObject("Templates")
            If (array IsNot Nothing) Then
                Dim objects As List = New ArrayList()
                For i As Integer = 0 To array.size() - 1
                    objects.add(New FDFTemplate(array.getObject(i)))
                Next
                retval = New COSArrayList(objects, array)
            End If
            Return retval
        End Function

        '/**
        ' * A list of FDFTemplate objects.
        ' *
        ' * @param templates A list of templates for this Page.
        ' */
        Public Sub setTemplates(ByVal templates As List)
            page.setItem("Templates", COSArrayList.converterToCOSArray(templates))
        End Sub

        '/**
        ' * This will get the FDF page info object.
        ' *
        ' * @return The Page info.
        ' */
        Public Function getPageInfo() As FDFPageInfo
            Dim retval As FDFPageInfo = Nothing
            Dim dict As COSDictionary = page.getDictionaryObject("Info")
            If (dict IsNot Nothing) Then
                retval = New FDFPageInfo(dict)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the page info.
        ' *
        ' * @param info The new page info dictionary.
        ' */
        Public Sub setPageInfo(ByVal info As FDFPageInfo)
            page.setItem("Info", info)
        End Sub

    End Class

End Namespace