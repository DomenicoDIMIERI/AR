Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.optionalcontent

    '/**
    ' * This class represents an optional content group (OCG).
    ' *
    ' * @since PDF 1.5
    ' * @version $Revision$
    ' */
    Public Class PDOptionalContentGroup
        Implements COSObjectable

        Private ocg As COSDictionary

        '/**
        ' * Creates a new optional content group (OCG).
        ' * @param name the name of the content group
        ' */
        Public Sub New(ByVal name As String)
            Me.ocg = New COSDictionary()
            Me.ocg.setItem(COSName.TYPE, COSName.OCG)
            setName(name)
        End Sub

        '/**
        ' * Creates a new instance based on a given {@link COSDictionary}.
        ' * @param dict the dictionary
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            If (Not dict.getItem(COSName.TYPE).Equals(COSName.OCG)) Then
                Throw New ArgumentException("Provided dictionary is not of type '" & COSName.OCG.toString & "'") 'IllegalArgumentException
            End If
            Me.ocg = dict
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.ocg
        End Function

        '/**
        ' * Returns the name of the optional content group.
        ' * @return the name
        ' */
        Public Function getName() As String
            Return Me.ocg.getString(COSName.NAME)
        End Function

        '/**
        ' * Sets the name of the optional content group.
        ' * @param name the name
        ' */
        Public Sub setName(ByVal name As String)
            Me.ocg.setString(COSName.NAME, name)
        End Sub

        ' //TODO Add support for "Intent" and "Usage"

        Public Overrides Function toString() As String
            Return MyBase.ToString() & " (" & getName() & ")"
        End Function

    End Class

End Namespace
