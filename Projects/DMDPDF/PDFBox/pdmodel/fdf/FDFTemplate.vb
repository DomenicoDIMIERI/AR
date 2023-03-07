Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents an FDF template that is part of the FDF page.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class FDFTemplate
        Implements COSObjectable

        Private template As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            template = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param t The FDF page template.
        ' */
        Public Sub New(ByVal t As COSDictionary)
            template = t
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return template
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return template
        End Function

        '/**
        ' * This is the template reference.
        ' *
        ' * @return The template reference.
        ' */
        Public Function getTemplateReference() As FDFNamedPageReference
            Dim retval As FDFNamedPageReference = Nothing
            Dim dict As COSDictionary = template.getDictionaryObject("TRef")
            If (dict IsNot Nothing) Then
                retval = New FDFNamedPageReference(dict)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the template reference.
        ' *
        ' * @param tRef The template reference.
        ' */
        Public Sub setTemplateReference(ByVal tRef As FDFNamedPageReference)
            template.setItem("TRef", tRef)
        End Sub

        '/**
        ' * This will get a list of fields that are part of this template.
        ' *
        ' * @return A list of fields.
        ' */
        Public Function getFields() As List
            Dim retval As List = Nothing
            Dim array As COSArray = template.getDictionaryObject("Fields")
            If (array IsNot Nothing) Then
                Dim fields As List = New ArrayList()
                For i As Integer = 0 To array.size() - 1
                    fields.add(New FDFField(array.getObject(i)))
                Next
                retval = New COSArrayList(fields, array)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a list of fields for this template.
        ' *
        ' * @param fields The list of fields to set for this template.
        ' */
        Public Sub setFields(ByVal fields As List)
            template.setItem("Fields", COSArrayList.converterToCOSArray(fields))
        End Sub

        '/**
        ' * A flag telling if the fields imported from the template may be renamed if there are conflicts.
        ' *
        ' * @return A flag telling if the fields can be renamed.
        ' */
        Public Function shouldRename() As Boolean
            Return template.getBoolean("Rename", False)
        End Function

        '/**
        ' * This will set if the fields can be renamed.
        ' *
        ' * @param value The flag value.
        ' */
        Public Sub setRename(ByVal value As Boolean)
            template.setBoolean("Rename", value)
        End Sub

    End Class

End Namespace
