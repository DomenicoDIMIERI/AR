Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections
Imports System.Diagnostics
Imports System.Globalization
Imports System.Security.Permissions
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace PDF.Elements

    <Serializable> _
    Public Class PdfNull
        Inherits PDFObject

        ''' <summary>
        ''' Returns "null".
        ''' </summary>
        ''' <returns>The string "null".</returns>
        Public Overrides Function ToString() As String
            Return "null"
        End Function


    End Class


End Namespace