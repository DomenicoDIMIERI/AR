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
    Public Class PdfCrossReferenceEntry

        Private m_objectNumber As Integer
        Private m_generationNumber As Integer
        Private m_offset As Integer
        Private m_active As Boolean

        ''' <summary>
        ''' The object number.
        ''' </summary>
        Public Property ObjectNumber As Integer
            Get
                Return Me.m_objectNumber
            End Get
            Set(value As Integer)
                Me.m_objectNumber = value
            End Set
        End Property

        ''' <summary>
        ''' The generation number.
        ''' </summary>
        Public Property GenerationNumber As Integer
            Get
                Return Me.m_generationNumber
            End Get
            Set(value As Integer)
                Me.m_generationNumber = value
            End Set
        End Property

        ''' <summary>
        ''' The byte offset of the object within the document.
        ''' </summary>
        Public Property Offset As Integer
            Get
                Return Me.m_offset
            End Get
            Set(value As Integer)
                Me.m_offset = value
            End Set
        End Property

        ''' <summary>
        ''' true if the object is not free, false otherwise.
        ''' </summary>
        Public Property Active As Boolean
            Get
                Return Me.m_active
            End Get
            Set(value As Boolean)
                Me.m_active = value
            End Set
        End Property

        ''' <summary>
        ''' Initializes a new PdfCrossReferenceEntry object.
        ''' </summary>
        ''' <param name="objNumber">The object number</param>
        ''' <param name="generationNumber">The generation number</param>
        ''' <param name="offset">The byte offset within the PDF file</param>
        ''' <param name="active">true if the object is not free, false otherwise</param>
        Public Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal offset As Integer, ByVal active As Boolean)
            Me.m_objectNumber = objNumber
            Me.m_generationNumber = generationNumber
            Me.m_offset = offset
            Me.m_active = active
        End Sub

    End Class


End Namespace