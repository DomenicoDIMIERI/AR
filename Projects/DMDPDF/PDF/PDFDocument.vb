Imports System.Drawing
Imports FinSeA.PDF.Fonts

Namespace PDF



    Public Class PDFDocument

        Private Shared CoreFonts As CKeyCollection(Of PDFFont)
        Private m_Orientation As String
        Private m_Unit As String
        Private m_Format As String
        Private m_Pages As CCollection(Of PDFPage)

        Shared Sub New()
        End Sub

       

    End Class

End Namespace