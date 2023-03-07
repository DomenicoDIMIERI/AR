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
    Public Class PdfForm
        Inherits  PdfField

        Private Shared NAName As New PdfName("/NeedAppearances")

        ''' <summary>
        ''' Initializes a new instance of PdfForm with the specified object number, generation number,
        ''' and field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        Public Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary)
            MyBase.New(objNumber, generationNumber, fieldDictionary)
            ' set NeedAppearances key so the viewer application regenerates the appearance streams
            ' for the form fields.
            Me.FieldDictionary.SetElement(NAName, New PdfBool(True))
        End Sub
    End Class


End Namespace