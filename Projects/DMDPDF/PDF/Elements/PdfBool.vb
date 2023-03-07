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
    Public Class PdfBool
        Inherits PDFObject
        Private val As Boolean

        ''' <summary>
        ''' Initializes a new PdfBool instance.
        ''' </summary>
        ''' <param name="truthValue">The initial value of the PdfBool object.</param>
        Public Sub New(ByVal truthValue As Boolean)
            Me.val = truthValue
        End Sub

        ''' <summary>
        ''' Returns the string representation of this object.
        ''' </summary>
        ''' <returns>Either "true" or "false".</returns>
        Public Overrides Function ToString() As String
            Return IIf(Me.val, "true", "false")
        End Function

    End Class


End Namespace