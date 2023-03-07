Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.measurement

    '/**
    ' * This class represents a measure dictionary.
    ' * 
    ' * @version $Revision: 1.0 $
    ' *
    ' */
    Public Class PDMeasureDictionary
        Implements COSObjectable

        ''' <summary>
        ''' The type of the dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TYPE = "Measure"

        Private measureDictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Protected Sub New()
            Me.measureDictionary = New COSDictionary()
            Me.getDictionary().setName(COSName.TYPE, TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param dictionary the corresponding dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            Me.measureDictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.measureDictionary
        End Function

        '/**
        ' * This will return the corresponding dictionary.
        ' * 
        ' * @return the measure dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return Me.measureDictionary
        End Function

        '/**
        ' * This will return the type of the measure dictionary.
        ' * It must be "Measure"
        ' * 
        ' * @return the type
        ' */
        Public Function getMeasType() As String
            Return TYPE
        End Function

        '/**
        ' * returns the subtype of the measure dictionary.
        ' * @return the subtype of the measure data dictionary
        ' */

        Public Function getSubtype() As String
            Return Me.getDictionary().getNameAsString(COSName.SUBTYPE, PDRectlinearMeasureDictionary.SUBTYPE)
        End Function

        '/**
        ' * This will set the subtype of the measure dictionary.
        ' * @param subtype the subtype of the measure dictionary
        ' */
        Protected Sub setSubtype(ByVal subtype As String)
            Me.getDictionary().setName(COSName.SUBTYPE, subtype)
        End Sub

    End Class

End Namespace
