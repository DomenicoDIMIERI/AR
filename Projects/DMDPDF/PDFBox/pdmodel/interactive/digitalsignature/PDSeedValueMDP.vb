Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    '/**
    ' * <p>This MDP dictionary is a part of the seed value dictionary and define
    ' * if a author signature or a certification signature should be use.</p>
    ' *
    ' * <p>For more informations, consider the spare documented chapter in the seed
    ' * value dictionary in the ISO 32000 specification.</p>
    ' *
    ' * @author Thomas Chojecki
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDSeedValueMDP


        Private dictionary As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            dictionary.setDirect(True)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The signature dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
            dictionary.setDirect(True)
        End Sub


        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase
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
        ' * Return the P value.
        ' * 
        ' * @return the P value
        ' */
        Public Function getP() As Integer
            Return dictionary.getInt(COSName.P)
        End Function

        '/**
        ' * Set the P value.
        ' * 
        ' * @param p the value to be set as P
        ' */
        Public Sub setP(ByVal p As Integer)
            If (p < 0 OrElse p > 3) Then
                Throw New ArgumentOutOfRangeException("Only values between 0 and 3 nare allowed.")
            End If
            dictionary.setInt(COSName.P, p)
        End Sub

    End Class

End Namespace
