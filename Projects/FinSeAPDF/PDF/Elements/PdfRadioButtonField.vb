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
    Public Class PdfRadioButtonField
        Inherits PdfButtonField

        ''' <summary>
        ''' Initializes a new instance of PdfRadioButtonField with the specified object number, generation number,
        ''' and field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        Public Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary)
            MyBase.New(objNumber, generationNumber, fieldDictionary)
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating the selected RadioButton.
        ''' </summary>
        ''' <value>The name of the selected RadioButton (without the leading '/').</value>
        Public Property SelectedItem As String
			get
                If (Me.FieldDictionary.Dictionary.ContainsKey(VName) AndAlso Me.FieldDictionary.Dictionary(VName).GetType() Is GetType(PdfName)) Then
                    Dim selected As PdfName = Me.FieldDictionary.Dictionary(VName)
                    Return selected.ToString().Substring(1)
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                Me.FieldDictionary.SetElement(VName, New PdfName("/" + value))
            End Set
        End Property

                ''' <summary>
                ''' Gets or sets a value indicating if it is possible that no RadioButton is selected.
                ''' </summary>
                ''' <value>true if it is not possible that no RadioButton is selected.</value>
        Public Property NoToggleToOff As Boolean
			get
                Return Me.GetBit(15)
            End Get
            Set(value As Boolean)
                Me.SetBit(15, value)
            End Set
        End Property
         
    End Class


End Namespace