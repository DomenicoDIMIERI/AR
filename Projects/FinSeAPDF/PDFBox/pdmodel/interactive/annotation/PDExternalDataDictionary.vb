Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common


Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This class represents an external data dictionary.
    ' * 
    ' * @version $Revision: 1.0 $
    ' * 
    ' */
    Public Class PDExternalDataDictionary
        Implements COSObjectable

        Private dataDictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Me.dataDictionary = New COSDictionary()
            Me.dataDictionary.setName(COSName.TYPE, "ExData")
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' *  @param dictionary Dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            Me.dataDictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.dataDictionary
        End Function

        '/**
        ' * returns the dictionary.
        ' *
        ' * @return the dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return Me.dataDictionary
        End Function

        '/**
        ' * returns the type of the external data dictionary.
        ' * It must be "ExData", if present
        ' * @return the type of the external data dictionary
        ' */
        Public Function getDataType() As String
            Return Me.getDictionary().getNameAsString(COSName.TYPE, "ExData")
        End Function

        '/**
        ' * returns the subtype of the external data dictionary.
        ' * @return the subtype of the external data dictionary
        ' */
        Public Function getSubtype() As String
            Return Me.getDictionary().getNameAsString(COSName.SUBTYPE)
        End Function

        '/**
        ' * This will set the subtype of the external data dictionary.
        ' * @param subtype the subtype of the external data dictionary
        ' */
        Public Sub setSubtype(ByVal subtype As String)
            Me.getDictionary().setName(COSName.SUBTYPE, subtype)
        End Sub

    End Class

End Namespace