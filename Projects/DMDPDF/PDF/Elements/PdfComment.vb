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
    Public Class PdfComment
        Inherits PDFObject

        Private m_comment As String

        ''' <summary>
        ''' Initializes a new instance of PdfComment.
        ''' </summary>
        ''' <param name="comment">The comment from which to initialize the object (without the leading '%' character and the trailing newline).</param>
        Public Sub New(ByVal comment As String)
            Me.m_comment = comment
        End Sub

        ''' <summary>
        ''' Returns the string representation of the PdfComment object.
        ''' </summary>
        ''' <returns>The string representation of the PdfComment object.</returns>
        Public Overrides Function ToString() As String
            Return "%" & Me.m_comment & vbCr
        End Function

        ''' <summary>
        ''' Gets or sets the value of this PdfComment object.
        ''' </summary>
        Public Property Comment As String
			get
                Return Me.m_comment
            End Get
            Set(value As String)
                Me.m_comment = value
            End Set
        End Property

      
    End Class


End Namespace