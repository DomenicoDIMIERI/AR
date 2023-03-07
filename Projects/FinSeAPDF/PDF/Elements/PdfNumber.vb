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
    Public Class PdfNumber
        Inherits PDFObject

        Private m_number As Double = 0.0
        Private Shared ReadOnly numberFormat As NumberFormatInfo = New CultureInfo("en-US").NumberFormat

        ''' <summary>
        ''' Initializes a new instance of PdfNumber.
        ''' </summary>
        ''' <param name="num">The string from which to parse the PdfNumber</param>
        Public Sub New(ByVal num As String)
            Me.m_number = Double.Parse(num, numberFormat)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of PdfNumber.
        ''' </summary>
        ''' <param name="num">The number from which to initialize the PdfNumber.</param>
        Public Sub New(ByVal num As Double)
            Me.m_number = num
        End Sub

        ''' <summary>
        ''' Returns the string representation of the PdfNumber object.
        ''' </summary>
        ''' <returns>The string representation of the PdfNumber object.</returns>
        Public Overrides Function ToString() As String
            Return Me.m_number.ToString(numberFormat)
        End Function

        ''' <summary>
        ''' Gets or sets the value of this PdfNumber object.
        ''' </summary>
        Public Property Number As Double
            Get
                Return Me.m_number
            End Get
            Set(value As Double)
                Me.m_number = value
            End Set
        End Property


    End Class


End Namespace