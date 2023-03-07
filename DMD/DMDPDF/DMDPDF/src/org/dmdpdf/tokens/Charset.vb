Imports System
Imports System.Text

Namespace DMD.org.dmdpdf.tokens

    Friend Class Charset 'Static 

        Public Shared ReadOnly ISO88591 As System.Text.Encoding = System.Text.Encoding.GetEncoding("ISO-8859-1")
        Public Shared ReadOnly UTF16BE As System.Text.Encoding = System.Text.Encoding.BigEndianUnicode
        Public Shared ReadOnly UTF16LE As System.Text.Encoding = System.Text.Encoding.Unicode

    End Class

End Namespace

