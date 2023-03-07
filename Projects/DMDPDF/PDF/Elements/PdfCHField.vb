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
    Public Class PdfCHField
        Inherits PdfField

        Private Shared IName As New PdfName("/I")

        ''' <summary>
        ''' Initializes a new instance of PdfCHField with the specified object number, generation number,
        ''' and field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        Public Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary)
            MyBase.New(objNumber, generationNumber, fieldDictionary)
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating whether this field should be rendered as a ComboBox or a ListBox.
        ''' </summary>
        ''' <value>true if the field should be rendered as ComboBox.</value>
        Public Property Combo As Boolean
			get
                Return Me.GetBit(18)
            End Get
            Set(value As Boolean)
                Me.SetBit(18, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether the ComboBox includes an editable text box as well as a drop list.
        ''' This property is meaningful only if the Combo property is set to true.
        ''' </summary>
        ''' <value>true if the ComboBox should include an editable text box.</value>
        Public Property Edit As Boolean
			get
                Return Me.GetBit(19)
            End Get
            Set(value As Boolean)
                Me.SetBit(19, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether the fields elements are sorted alphabetically.
        ''' </summary>
        ''' <value>true if the elements are sorted alphabetically.</value>
        Public Property Sort As Boolean
			get
                Return Me.GetBit(20)
            End Get
            Set(value As Boolean)
                Me.SetBit(20, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating if more than one element is selectable.
        ''' </summary>
        ''' <value>true if more than one element is selectable.</value>
        Public Property MultiSelect As Boolean
			get
                Return Me.GetBit(22)
            End Get
            Set(value As Boolean)
                Me.SetBit(22, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether spellchecking should be enabled.
        ''' This flag is meaningful only if the Combo and Edit flags are both set.
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
        ''' Sets the indexes of the items that should appear selected.
        ''' </summary>
        ''' <param name="indexes">The selected indexes.</param>
        Public Sub SetSelectedIndexes(Optional ByVal indexes() As Integer = Nothing)
            If (indexes Is Nothing OrElse indexes.Length <= 0) Then
                Me.FieldDictionary.SetElement(IName, New PdfNull())
            ElseIf (indexes.Length = 1) Then
                Dim val As New PdfNumber(indexes(0))
                Me.FieldDictionary.SetElement(IName, val)
            Else
                Dim items() As PdfNumber
                ReDim items(indexes.Length - 1)
                Dim i As Integer = 0
                For Each index As Integer In indexes
                    items(i) = New PdfNumber(index)
                    i += 1
                Next

                Dim val As New PdfArray(items)
                FieldDictionary.SetElement(IName, val)
            End If
        End Sub

        ''' <summary>
        ''' Gets the indexes of the items that should appear selected.
        ''' </summary>
        ''' <returns>An array of index that are selected.</returns>
        Public Function GetSelectedIndexes() As Integer()
            Dim iObject As PDFObject = FieldDictionary.GetElement(IName)

            If (iObject Is Nothing OrElse iObject.GetType() Is GetType(PdfNull)) Then
                Return New Integer() {}
            ElseIf (iObject.GetType() Is GetType(PdfNumber)) Then
                Return New Integer() {DirectCast(iObject, PdfNumber).Number}
			Else
                Dim array As PdfArray = iObject
                Dim indexes() As Integer
                ReDim indexes(array.Elements.Count - 1)
                For i As Integer = 0 To array.Elements.Count - 1
                    indexes(i) = DirectCast(array(i), PdfNumber).Number
                Next
                Return indexes
			End If
        End Function
    End Class


End Namespace