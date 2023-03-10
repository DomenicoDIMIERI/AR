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
    Public Class PdfTXField
        Inherits PdfField

        ''' <summary>
        ''' Initializes a new instance of PdfTXField with the specified object number, generation number,
        ''' and field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        Public Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary)
            MyBase.New(objNumber, generationNumber, fieldDictionary)
            Me.FieldDictionary.Dictionary.Remove(APName) ' remove the appearance element, so it gets regenerated by the viewer
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating the text that is displayed in the field.
        ''' </summary>
        Public Property Text As String
			get
                If (Me.FieldDictionary.Dictionary.ContainsKey(VName) AndAlso Me.FieldDictionary.Dictionary(VName).GetType() Is GetType(PdfString)) Then
                    Dim val As PdfString = Me.FieldDictionary.Dictionary(VName)
                    Return val.Text
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                Dim val As PdfString
                Dim input As String = value & ")"
                val = New PdfString(False, input)
                Me.FieldDictionary.SetElement(VName, val)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether the field allows more than one line of input.
        ''' </summary>
        ''' <value>true if more than one line of input is allowed.</value>
        Public Property MultiLine As Boolean
			get
                Return Me.GetBit(13)
            End Get
            Set(value As Boolean)
                Me.SetBit(13, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether the text should be visibly echoed
        ''' or instead rendered in a non-readable form such as asterisks or bullets.
        ''' </summary>
        ''' <value>true if the text should not be visibly echoed.</value>
        Public Property Password As Boolean
			get
                Return Me.GetBit(14)
            End Get
            Set(value As Boolean)
                Me.SetBit(14, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether this field represents the pathname of
        ''' a file whose contents are to be submitted as the value of the field.
        ''' </summary>
        ''' <value>true if this field represents the pathname of a file.</value>
        Public Property FileSelect As Boolean
			get
                Return Me.GetBit(21)
            End Get
            Set(value As Boolean)
                Me.SetBit(21, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether spellchecking should be enabled.
        ''' </summary>
        ''' <value>true if no spell checking is performed.</value>
        Public Property DoNotSpellCheck As Boolean
			get
                Return Me.GetBit(23)
            End Get
            Set(value As Boolean)
                Me.SetBit(23, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating if scrolling is allowed.
        ''' </summary>
        ''' <value>true if this field should not allow scrolling.</value>
        Public Property DoNotScroll As Boolean
			get
                Return Me.GetBit(24)
            End Get
            Set(value As Boolean)
                Me.SetBit(24, value)
            End Set
        End Property

    End Class


End Namespace