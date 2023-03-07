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
    Public Class PdfArray
        Inherits PDFObject

        Private array As New System.Collections.ArrayList()
        Private Shared ReadOnly endRegex As New Regex("^\s*\]", RegexOptions.Singleline)

        ''' <summary>
        ''' Initializes a new PdfArray object.
        ''' </summary>
        ''' <param name="input">The input string to be parsed. 
        ''' Must not contain the leading '['. Must contain the trailing ']'. 
        ''' The PdfArray is consumed from the input.</param>
        Public Sub New(ByRef input As String)
            Dim match As Boolean

            input = input.TrimStart()
            match = input.StartsWith("]")
            While (Not match AndAlso input.Length > 0)
                array.Add(PDFObject.GetPdfObject(input))
                input = input.TrimStart()
                match = input.StartsWith("]")
            End While

            If (match) Then
                input = input.Substring(1)
            End If
        End Sub

        ''' <summary>
        ''' Initializes a new PdfArray object.
        ''' </summary>
        ''' <param name="items">The array of PdfObjects from which to construct the new PdfArray.</param>
        Public Sub New(Optional ByVal items() As PDFObject = Nothing)
            If (items IsNot Nothing) Then
                Me.array.AddRange(items)
            End If
        End Sub

        ''' <summary>
        ''' Returns the string representation of this object.
        ''' </summary>
        ''' <returns>The string representation of this object.</returns>
        Public Overrides Function ToString() As String
            Dim s As New System.Text.StringBuilder
            s.Append("[ ")

            For Each element As PDFObject In Me.array
                s.Append(element.ToString())
                s.Append(" ")
            Next

            s.Append("]")

            Return s.ToString
        End Function

        ''' <summary>
        ''' Gets the elements of the PdfArray.
        ''' </summary>
        ''' <value>The elements of the PdfArray.</value>
        Public ReadOnly Property Elements As System.Collections.ArrayList
            Get
                Return Me.array
            End Get
        End Property

        ''' <summary>
        ''' Indexer for the PdfArray.
        ''' </summary>
        Default Public Property Item(ByVal index As Integer) As PDFObject
			get
                Return DirectCast(Me.array(index), PDFObject)
            End Get
            Set(value As PDFObject)
                If (value Is Nothing) Then Throw New ArgumentNullException
                Me.array(index) = value
            End Set
        End Property


    End Class


End Namespace