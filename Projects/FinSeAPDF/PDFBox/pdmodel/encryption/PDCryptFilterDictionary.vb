Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.encryption


    '/**
    ' * This class is a specialized view of the crypt filter dictionary of a PDF document.
    ' * It contains a low level dictionary (COSDictionary) and provides the methods to
    ' * manage its fields.
    ' *
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDCryptFilterDictionary


        ''' <summary>
        ''' COS crypt filter dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Protected cryptFilterDictionary As COSDictionary = Nothing

        ''' <summary>
        ''' creates a new empty crypt filter dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            cryptFilterDictionary = New COSDictionary()
        End Sub

        ''' <summary>
        ''' creates a new crypt filter dictionary from the low level dictionary provided.
        ''' </summary>
        ''' <param name="d">the low level dictionary that will be managed by the newly created object</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal d As COSDictionary)
            cryptFilterDictionary = d
        End Sub

        '/**
        ' * This will get the dictionary associated with this crypt filter dictionary.
        ' *
        ' * @return The COS dictionary that this object wraps.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return cryptFilterDictionary
        End Function

        '/**
        ' * This will set the number of bits to use for the crypt filter algorithm.
        ' *
        ' * @param length The new key length.
        ' */
        Public Sub setLength(ByVal length As Integer)
            cryptFilterDictionary.setInt(COSName.LENGTH, length)
        End Sub

        '/**
        ' * This will return the Length entry of the crypt filter dictionary.<br /><br />
        ' * The length in <b>bits</b> for the crypt filter algorithm. This will return a multiple of 8.
        ' *
        ' * @return The length in bits for the encryption algorithm
        ' */
        Public Function getLength() As Integer
            Return cryptFilterDictionary.getInt(COSName.LENGTH, 40)
        End Function

        '/**
        '* This will set the crypt filter method. 
        '* Allowed values are: NONE, V2, AESV2
        '*
        '* @param cfm name of the crypt filter method.
        '*
        '* @throws IOException If there is an error setting the data.
        '*/
        Public Sub setCryptFilterMethod(ByVal cfm As COSName) 'throws IOException
            cryptFilterDictionary.setItem(COSName.CFM, cfm)
        End Sub

        '/**
        ' * This will return the crypt filter method. 
        ' * Allowed values are: NONE, V2, AESV2
        ' *
        ' * @return the name of the crypt filter method.
        ' *
        ' * @throws IOException If there is an error accessing the data.
        ' */
        Public Function getCryptFilterMethod() As COSName ' throws IOException
            Return cryptFilterDictionary.getDictionaryObject(COSName.CFM)
        End Function

    End Class

End Namespace
