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
    Public MustInherit Class PDFObject

        Public Sub New()
        End Sub

      
        Private Shared ReadOnly boolRegex As New Regex("^((true)|(false))", RegexOptions.Singleline)
        Private Shared ReadOnly nameRegex As New Regex("^(/[^\s()<>{}/%[\]]+)", RegexOptions.Singleline)
        Private Shared ReadOnly stringRegex As New Regex("^(<|\()", RegexOptions.Singleline)
        Private Shared ReadOnly arrayRegex As New Regex("^\[", RegexOptions.Singleline)
        Private Shared ReadOnly nullRegex As New Regex("^null", RegexOptions.Singleline)
        Private Shared ReadOnly referenceRegex As New Regex("^(\d+)\s+(\d+)\s+R", RegexOptions.Singleline)
        Private Shared ReadOnly dictionaryRegex As New Regex("^<<", RegexOptions.Singleline)
        Private Shared ReadOnly numberRegex As New Regex("^((-|\+)?\d*\.?\d*)", RegexOptions.Singleline)
        Private Shared ReadOnly commentRegex As New Regex("^%([^\u000a\u000d]*)", RegexOptions.Singleline)

        'private Shared readonly ILog log = LogManager.GetLogger(typeof(PdfObject));
        '#If 0 Then


        ''' <summary>
        ''' Factory method for PdfObjects. 
        ''' Tries to parse a given input string into a new instance of PdfObject.
        ''' </summary>
        ''' <param name="input">The input string to be parsed. Will be consumed.</param>
        ''' <returns>A PdfObject on success, null otherwise.</returns>
        Public Shared Function GetPdfObject(ByRef input As String) As PDFObject
            Dim match As Match

            input = input.TrimStart() ' remove white space from the start

            If (input.StartsWith("true")) Then
                input = input.Substring(4)
                'log.Info("Parsing PdfBool (true)")
                Return New PdfBool(True)
            ElseIf (input.StartsWith("false")) Then
                input = input.Substring(5)
                'log.Info("Parsing PdfBool (false)");
                Return New PdfBool(False)
            End If
            If input.StartsWith("/") Then
                match = nameRegex.Match(input)
                If (match.Success) Then
                    input = input.Substring(match.Index + match.Length)
                    'log.Info("Parsing PdfName (" + match.Groups(1).Value + ")");
                    Return New PdfName(match.Groups(1).Value)
                End If
            End If
            If (input.StartsWith("<<")) Then
                input = input.Substring(2)
                'log.Info("Parsing PdfDictionary");
                Return New PdfDictionary(input)
            End If
            If (input.Length > 0 AndAlso (Mid(input, 1, 1) = "<" OrElse Mid(input, 1, 1) = "(")) Then
                Dim hex As Boolean = Mid(input, 1, 1) = "<"
                input = input.Substring(1)
                'log.Info("Parsing PdfString (hex = " + hex + ")");
                Return New PdfString(hex, input)
            End If
            If (input.StartsWith("[")) Then
                input = input.Substring(1)
                'log.Info("Parsing PdfArray");
                Return New PdfArray(input)
            End If
            If (input.StartsWith("null")) Then
                input = input.Substring(4)
                'log.Info("Parsing PdfNull");
                Return New PdfNull()
            End If
            match = referenceRegex.Match(input)
            If (match.Success) Then
                input = input.Substring(match.Index + match.Length)
                Dim objNumber As Integer = Int32.Parse(match.Groups(1).Value, CultureInfo.InvariantCulture)
                Dim generationNumber As Integer = Int32.Parse(match.Groups(2).Value, CultureInfo.InvariantCulture)
                'log.Info(string.Format(CultureInfo.InvariantCulture, "Parsing PdfReference ({0}, {1})", objNumber, generationNumber));
                Return New PdfReference(objNumber, generationNumber)
            End If
            match = numberRegex.Match(input)
            If (match.Success) Then
                input = input.Substring(match.Index + match.Length)
                'log.Info("Parsing PdfNumber (" + match.Groups(1).Value + ")");
                Return New PdfNumber(match.Groups(1).Value)
            End If

            Dim comment As PDFObject = ParseComment(input)
            If (comment IsNot Nothing) Then
                'log.Info("Parsing PdfComment");
                Return comment
            End If

            Throw New Exception("unable to parse '" + input + "'")
        End Function

        ''' <summary>
        ''' Tries to parse a PDF comment from the input string.
        ''' </summary>
        ''' <param name="input">The input string to be parsed. Will be consumed.</param>
        ''' <returns>A PdfComment object on success, null otherwise.</returns>
        Public Shared Function ParseComment(ByRef input As String) As PdfComment
            Dim match As Match

            match = commentRegex.Match(input)

            If (match.Success) Then
                input = input.Substring(match.Index + match.Length)
                Return New PdfComment(match.Groups(1).Value)
            Else
                Return Nothing
            End If
        End Function

        '#End If

        Friend Overridable Sub Write(ByVal writer As PDFWriter)
            writer.WriteRawData(Me.ToString)
        End Sub


    End Class


End Namespace