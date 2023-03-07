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
    Public Class PdfReference
        Inherits PDFObject

        Private m_objNumber As Integer
        Private m_generationNumber As Integer

        ''' <summary>
        ''' Initializes a new PdfReference object.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        Public Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer)
            Me.m_objNumber = objNumber
            Me.m_generationNumber = generationNumber
        End Sub

        ''' <summary>
        ''' Returns the string representation of this object.
        ''' </summary>
        ''' <returns>"o g R" where o is the object number and g is the generation number.</returns>
        Public Overrides Function ToString() As String
            Return Me.m_objNumber.ToString(CultureInfo.InvariantCulture) & " " & Me.m_generationNumber & " R"
        End Function

        ''' <summary>
        ''' Gets or sets a value indicating the object number.
        ''' </summary>
        Public Property ObjectNumber As Integer
            Get
                Return Me.m_objNumber
            End Get
            Set(value As Integer)
                Me.m_objNumber = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating the generation number.
        ''' </summary>
        Public Property GenerationNumber As Integer
            Get
                Return Me.m_generationNumber
            End Get
            Set(value As Integer)
                Me.m_generationNumber = value
            End Set
        End Property



    End Class


End Namespace