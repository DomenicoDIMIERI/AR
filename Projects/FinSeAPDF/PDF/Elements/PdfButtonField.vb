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
    Public Class PdfButtonField
        Inherits PdfField

        Private Shared ReadOnly btnRegex As New Regex("/Ff\s+(\d+)", RegexOptions.Singleline)

        ''' <summary>
        ''' Initializes a new instance of PdfButtonField with the specified object number, generation number,
        ''' and field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        Protected Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary)
            MyBase.New(objNumber, generationNumber, fieldDictionary)
        End Sub

        ''' <summary>
        ''' Creates either a <see cref="PdfPushButtonField"/>, <see cref="PdfRadioButtonField"/>, or a
        ''' <see cref="PdfCheckBoxField"/> according to the parsed field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        ''' <returns></returns>
        Public Shared Function GetButtonField(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary) As PdfButtonField
            Dim flags As Integer = 0

            Dim num As PdfNumber = fieldDictionary.GetElement(FFName)
            If (num IsNot Nothing) Then
                flags = num.Number
            End If

            If (Math.TestBit(flags, 16)) Then
                Return New PdfPushButtonField(objNumber, generationNumber, fieldDictionary)
            ElseIf (Math.TestBit(flags, 15)) Then
                Return New PdfRadioButtonField(objNumber, generationNumber, fieldDictionary)
            Else
                Return New PdfCheckBoxField(objNumber, generationNumber, fieldDictionary)
            End If
        End Function


        ''' <summary>
        ''' Determines whether the object specified is a RadioButton.
        ''' </summary>
        ''' <param name="fieldDictionary">The object's field dictionary.</param>
        ''' <returns>true if the object specified is a RadioButton, false otherwise.</returns>
        Public Shared Function IsRadioButton(ByVal fieldDictionary As PdfDictionary) As Boolean
            If (Not IsButton(fieldDictionary)) Then Return False
			
            Dim num As PdfNumber = fieldDictionary.GetElement(FFName)
            If (num IsNot Nothing) Then
                Dim flags As Integer = num.Number
                Return Math.TestBit(flags, 15)
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Determines whether the object specified is a Button.
        ''' </summary>
        ''' <param name="fieldDictionary">The object's field dictionary.</param>
        ''' <returns>true if the object specified is a Button, false otherwise.</returns>
        Protected Shared Function IsButton(ByVal fieldDictionary As PdfDictionary) As Boolean
            Dim type As PdfName = fieldDictionary.GetElement(FTName)
            If (type IsNot Nothing) Then
                If (Not type.Equals(ButtonName)) Then
                    Return False
                End If
			Else
                Return False
            End If
            Return True
        End Function

    End Class


End Namespace