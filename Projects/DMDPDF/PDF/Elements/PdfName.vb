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
    Public Class PdfName
        Inherits PDFObject

        Private name As String
        Private Shared ReadOnly hexRegex As New Regex("#([A-Fa-f0-9]{2})", RegexOptions.Singleline)
		private shared readonly specialRegex as new Regex("[\x00-\x20\x7f-\xff\s/%\(\)<>\{\}\[\]#]", RegexOptions.Singleline) ' PDF special characters, to be encoded as #xx

        Private Function HexMatchEvaluator(ByVal match As Match)
            Dim b As Byte = Byte.Parse(match.Groups(1).Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture)
            Return "" & Convert.ToChar(b)
        End Function

        Private Function SpecialMatchEvaluator(ByVal match As Match)
            Dim b As Byte = Convert.ToByte(match.Groups(0).Value(0))
            Return "#" & b.ToString("x2", CultureInfo.InvariantCulture)
        End Function

        ''' <summary>
        ''' Initializes a new PdfName instance.
        ''' </summary>
        ''' <param name="name">The string to be parsed into a PdfName, e.g. "/AP".</param>
        Public Sub New(ByVal name As String)
            If (Not name.StartsWith("/")) Then Throw New Exception("PdfName does not start with '/': '" & name & "'")
            Me.name = hexRegex.Replace(name.Substring(1), New MatchEvaluator(AddressOf HexMatchEvaluator))
        End Sub

        ''' <summary>
        ''' Returns the string representation of this object.
        ''' </summary>
        ''' <returns>The string representation of this object.</returns>
        Public Overrides Function ToString() As String
            Return "/" & specialRegex.Replace(Me.name, New MatchEvaluator(AddressOf SpecialMatchEvaluator))
        End Function

        ''' <summary>
        ''' Determines whether the specified object's value is equal to the current object.
        ''' </summary>
        ''' <param name="obj">The object to compare with the current object.</param>
        ''' <returns></returns>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return (TypeOf (obj) Is PdfName) AndAlso (DirectCast(obj, PdfName).name.Equals(Me.name))
        End Function

        ''' <summary>
        ''' Returns a hashcode that is determined by the string representation of the PdfName object.
        ''' </summary>
        ''' <returns>A hashcode for the PdfName object.</returns>
        Public Overrides Function GetHashCode() As Integer
            Return Me.name.GetHashCode()
        End Function

    End Class


End Namespace