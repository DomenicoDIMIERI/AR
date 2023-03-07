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
    Public Class PdfCheckBoxField
        Inherits PdfButtonField

        Private OnState As PdfName ' the Name object for the checked state, unchecked is always /Off
        Private Shared OffState As New PdfName("/Off")
        Private Shared ASName As New PdfName("/AS")

        ''' <summary>
        ''' Initializes a new instance of PdfCheckBoxField with the specified object number, generation number,
        ''' and field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        Public Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary)
            MyBase.New(objNumber, generationNumber, fieldDictionary)
            Me.SetOnState()
        End Sub

        Private Sub SetOnState()
            If (Me.FieldDictionary.Dictionary.ContainsKey(APName) AndAlso Me.FieldDictionary.Dictionary(APName).GetType() Is GetType(PdfDictionary)) Then
                Dim appearance As PdfDictionary = Me.FieldDictionary.Dictionary(APName)
                If (appearance.Dictionary.ContainsKey(NName) AndAlso appearance.Dictionary(NName).GetType() Is GetType(PdfDictionary)) Then
                    Dim subAppearance As PdfDictionary = appearance.Dictionary(NName)
                    For Each name As PdfName In subAppearance.Dictionary.Keys
                        If (Not name.Equals(OffState)) Then
                            Me.OnState = name
                            Exit For
                        End If
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating whether the CheckBox is checked.
        ''' </summary>
        ''' <value>true is the CheckBox is checked.</value>
        Public Property Checked As Boolean
			get
                Return Me.FieldDictionary.Dictionary.ContainsKey(VName) AndAlso Me.FieldDictionary.Dictionary(VName).Equals(Me.OnState)
            End Get
            Set(value As Boolean)
                Dim name As PdfName = IIf(value, Me.OnState, OffState)
                Me.FieldDictionary.SetElement(VName, name)
                Me.FieldDictionary.SetElement(ASName, name)
            End Set
        End Property


    End Class


End Namespace