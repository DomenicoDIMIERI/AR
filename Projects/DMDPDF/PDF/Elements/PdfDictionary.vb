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
    Public Class PdfDictionary
        Inherits PDFObject

        Private m_dictionary As New System.Collections.Hashtable()
        Private Shared ReadOnly endRegex As New Regex("^>>", RegexOptions.Singleline)
        Private Shared ReadOnly keyRegex As New Regex("^(/[^\s()<>{}/%[\]]+)", RegexOptions.Singleline)

        ''' <summary>
        ''' Initializes a new instance of PdfDictionary.
        ''' </summary>
        ''' <param name="input">The input string from which to parse the PdfDictionary.
        ''' Must not contain the leading "&lt;&lt;". Must contain the trailing "&gt;&gt;". 
        ''' Consumes the PdfDictionary from the input.</param>
        Public Sub New(ByRef input As String)
            Dim match As Boolean

            input = input.TrimStart()
            match = input.StartsWith(">>")
            While (Not match AndAlso input.Length > 0)
                Dim keyMatch As Match = keyRegex.Match(input)
                If (keyMatch.Success) Then
                    input = input.Substring(keyMatch.Index + keyMatch.Length)
                    Me.m_dictionary.Add(New PdfName(keyMatch.Groups(1).Value), PDFObject.GetPdfObject(input))
                ElseIf (ParseComment(input) Is Nothing) Then ' maybe there is a comment
                    Throw New Exception("cannot parse PDF dictionary from '" & input & "'")
                End If

                input = input.TrimStart()
                match = input.StartsWith(">>")
            End While

            If (match) Then
                input = input.Substring(2)
            End If
        End Sub

        ''' <summary>
        ''' Initializes a new PdfDictionary object.
        ''' </summary>
        ''' <param name="dictionary">The Hashtable from which to initialize the PdfDictionary.</param>
        Public Sub New(ByVal dictionary As System.Collections.Hashtable)
            Me.m_dictionary = dictionary
        End Sub

        ''' <summary>
        ''' Gets the Hashtable for the PdfDictionary.
        ''' </summary>
        Public ReadOnly Property Dictionary As System.Collections.Hashtable
            Get
                Return Me.m_dictionary
            End Get
        End Property

        ''' <summary>
        ''' Returns the string representation of this PdfDictionary.
        ''' </summary>
        ''' <returns>The string representation of this PdfDictionary.</returns>
        Public Overrides Function ToString() As String
            Dim s As New System.Text.StringBuilder
            s.Append("<<" & vbLf)
            For Each key As PdfName In dictionary.Keys
                s.Append(key.ToString)
                s.Append(" ")
                s.Append(Me.m_dictionary(key))
                s.Append(vbLf)
            Next
            s.Append(" >>")
            Return s.ToString
        End Function

        ''' <summary>
        ''' Sets an element in the PDF dictionary to the specified value.
        ''' </summary>
        ''' <remarks>
        ''' If there is no element with the specified key in the dictionary, it will be added.
        ''' </remarks>
        ''' <param name="key">The key.</param>
        ''' <param name="objValue">The value.</param>
        Public Sub SetElement(ByVal key As PdfName, ByVal objValue As PDFObject)
            If (key Is Nothing) Then Throw New ArgumentNullException("key")
            If (objValue Is Nothing) Then Throw New ArgumentNullException("objValue")
            If (Me.m_dictionary.ContainsKey(key)) Then
                Me.m_dictionary(key) = objValue
            Else
                Me.m_dictionary.Add(key, objValue)
            End If
        End Sub

        ''' <summary>
        ''' Returns the value in the PdfDictionary for the specified key.
        ''' </summary>
        ''' <param name="key">The key.</param>
        ''' <returns>A PdfObject or null if there is no value in the PdfDictionary with the specified key.</returns>
        Public Function GetElement(ByVal key As PdfName) As PDFObject
            If (Me.m_dictionary.ContainsKey(key)) Then Return Me.m_dictionary(key)
            Return Nothing
        End Function

        ''' <summary>
        ''' Indexer for the PdfDictionary.
        ''' </summary>
        Default Public Property Item(ByVal key As String) As PDFObject
            Get
                Return Me.GetElement(New PdfName(key))
            End Get
            Set(value As PDFObject)
                Me.SetElement(New PdfName(key), value)
            End Set
        End Property

    End Class


End Namespace