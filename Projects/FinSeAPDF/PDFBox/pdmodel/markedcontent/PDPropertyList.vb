Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.optionalcontent


Namespace org.apache.pdfbox.pdmodel.markedcontent

    '/**
    ' * This class represents a property list used for the marked content feature to map a resource name
    ' * to a dictionary.
    ' *
    ' * @since PDF 1.2
    ' * @version $Revision$
    ' */
    Public Class PDPropertyList
        Implements COSObjectable

        Private props As COSDictionary

        '/**
        ' * Creates a new property list.
        ' */
        Public Sub New()
            Me.props = New COSDictionary()
        End Sub

        '/**
        ' * Creates a new instance based on a given {@link COSDictionary}.
        ' * @param dict the dictionary
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            Me.props = dict
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.props
        End Function

        '/**
        ' * Returns the optional content group belonging to the given resource name.
        ' * @param name the resource name
        ' * @return the optional content group or null if the group was not found
        ' */
        Public Function getOptionalContentGroup(ByVal name As COSName) As PDOptionalContentGroup
            Dim dict As COSDictionary = props.getDictionaryObject(name)
            If (dict IsNot Nothing) Then
                If (COSName.OCG.equals(dict.getItem(COSName.TYPE))) Then
                    Return New PDOptionalContentGroup(dict)
                End If
            End If
            Return Nothing
        End Function

        '/**
        ' * Puts a mapping from a resource name to an optional content group.
        ' * @param name the resource name
        ' * @param ocg the optional content group
        ' */
        Public Sub putMapping(ByVal name As COSName, ByVal ocg As PDOptionalContentGroup)
            putMapping(name, ocg.getCOSObject())
        End Sub

        Private Sub putMapping(ByVal name As COSName, ByVal dict As COSDictionary)
            props.setItem(name, dict)
        End Sub

    End Class


End Namespace
