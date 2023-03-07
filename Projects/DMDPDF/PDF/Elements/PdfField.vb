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
    Public Class PdfField
        Inherits PDFObject

        ''' <summary>
        ''' The object number of this field.
        ''' </summary>
        Private m_objectNumber As Integer
        ''' <summary>
        ''' The generation number of this field.
        ''' </summary>
        Private m_generationNumber As Integer
        ''' <summary>
        ''' The field dictionary of this field. See the PDF Reference 8.6.2 Field Dictionaries.
        ''' </summary>
        Private m_fieldDictionary As PdfDictionary
        ''' <summary>
        ''' The serialized form of the object after it was parsed.
        ''' </summary>
        Private m_original As String
        ''' <summary>
        ''' The /AP PDF Name object.
        ''' </summary>
        Protected Shared APName As New PdfName("/AP")
        ''' <summary>
        ''' The /N PDF Name object.
        ''' </summary>
        Protected Shared NName As New PdfName("/N")
        ''' <summary>
        ''' The /V PDF Name object.
        ''' </summary>
        Protected Shared VName As New PdfName("/V")
        ''' <summary>
        ''' The /T PDF Name object.
        ''' </summary>
        Protected Shared TName As New PdfName("/T")
        ''' <summary>
        ''' The /Ff PDF Name object.
        ''' </summary>
        Protected Shared FFName As New PdfName("/Ff")
        ''' <summary>
        ''' The /FT PDF Name object.
        ''' </summary>
        Protected Shared FTName As New PdfName("/FT")
        ''' <summary>
        ''' The /Tx PDF Name object.
        ''' </summary>
        Protected Shared TXName As New PdfName("/Tx")
        ''' <summary>
        ''' The /Ch PDF Name object.
        ''' </summary>
        Protected Shared CHName As New PdfName("/Ch")
        ''' <summary>
        ''' The /Btn PDF Name object.
        ''' </summary>
        Protected Shared ButtonName As New PdfName("/Btn")
        ''' <summary>
        ''' The /Kids PDF Name object.
        ''' </summary>
        Protected Shared KidsName As New PdfName("/Kids")
        ''' <summary>
        ''' The /AA PDF Name object.
        ''' </summary>
        Protected Shared AAName As New PdfName("/AA")
        ''' <summary>
        ''' The /TM PDF Name object.
        ''' </summary>
        Protected Shared TMName As New PdfName("/TM")
        ''' <summary>
        ''' The /TU PDF Name object.
        ''' </summary>
        Protected Shared TUName As New PdfName("/TU")
        ''' <summary>
        ''' The /Parent PDF Name object.
        ''' </summary>
        Protected Shared ParentName As New PdfName("/Parent")

        ''' <summary>
        ''' The object number of this field.
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
        ''' The generation number of this field.
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
        ''' The field dictionary of this field. See the PDF Reference 8.6.2 Field Dictionaries.
        ''' </summary>
        Public Property FieldDictionary As PdfDictionary
			get
                Return Me.m_fieldDictionary
            End Get
            Set(value As PdfDictionary)
                Me.m_fieldDictionary = value
            End Set
        End Property

        ''' <summary>
        ''' Initializes a new instance of PdfField with the specified object number, generation number,
        ''' and field dictionary.
        ''' </summary>
        ''' <param name="objNumber">The object number.</param>
        ''' <param name="generationNumber">The generation number.</param>
        ''' <param name="fieldDictionary">The field dictionary.</param>
        Protected Sub New(ByVal objNumber As Integer, ByVal generationNumber As Integer, ByVal fieldDictionary As PdfDictionary)
            Me.m_objectNumber = objNumber
            Me.m_generationNumber = generationNumber
            Me.m_fieldDictionary = fieldDictionary
            Me.m_original = Me.GetString()
        End Sub

        ''' <summary>
        ''' Factory method for PdfField objects. 
        ''' Initializes new objects according to the contents of the specified field dictionary.
        ''' Does not currently support signature fields.
        ''' In most cases, this will return just one object. However, fields are organized according
        ''' to trees, where intermediate field objects may have child fields. Only terminal field objects
        ''' are "real" fields, so this method may return more than one field.
        ''' </summary>
        ''' <param name="reference">The reference to the root node of the fields.</param>
        ''' <param name="reader">The PdfReader object from which to fetch additional child objects.</param>
        ''' <param name="parentDictionary">The accumulated field dictionary of the ancestor objects.
        ''' May be null for the root object.</param>
        ''' <param name="fieldName">The complete field name up to this object, e.g. "a.b.c".</param>
        ''' <returns>A new PdfField object.</returns>
        ''' <exception cref="Exception">Thrown when the field dictionary does not contain
        ''' a known field type.</exception>
        Public Shared Function GetPdfFields(ByVal reference As PdfReference, ByVal reader As PDFReader, ByVal parentDictionary As PdfDictionary, ByVal fieldName As String) As PdfField()
            Dim objNumber As Integer = reference.ObjectNumber
            Dim generationNumber As Integer = reference.GenerationNumber
            Dim fieldDictionary As PdfDictionary = reader.GetObjectForReference(reference)

            ' get the path name
            If (fieldDictionary("/T") IsNot Nothing) Then
                If (fieldName <> "") Then
                    fieldName &= "."
                End If

                fieldName &= DirectCast(fieldDictionary("/T"), PdfString).Text
            End If

            If (parentDictionary IsNot Nothing) Then
                ' add all ancestral objects that haven't been overwritten by this object
                For Each key As Object In parentDictionary.Dictionary.Keys
                    If (Not fieldDictionary.Dictionary.ContainsKey(key)) Then
                        fieldDictionary.Dictionary.Add(key, parentDictionary.Dictionary(key))
                    End If
                Next
            End If

            If (fieldDictionary("/Kids") Is Nothing OrElse PdfButtonField.IsRadioButton(fieldDictionary)) Then
                ' it's a terminal node

                Dim fieldType As PdfName = fieldDictionary.GetElement(FTName)
                Dim field As PdfField

                If (fieldType.Equals(TXName)) Then
                    field = New PdfTXField(objNumber, generationNumber, fieldDictionary)
                ElseIf (fieldType.Equals(CHName)) Then
                    field = New PdfCHField(objNumber, generationNumber, fieldDictionary)
                ElseIf (fieldType.Equals(ButtonName)) Then
                    field = PdfButtonField.GetButtonField(objNumber, generationNumber, fieldDictionary)
                Else
                    Throw New Exception("unsupported field type '" & fieldType.ToString() & "'")
                End If

                field.FieldName = fieldName

                Return New PdfField() {field}
            Else
                ' it's an intermediate node

                Dim fields As New System.Collections.ArrayList()
                Dim kids As PdfArray = fieldDictionary.GetElement(KidsName)

                For Each child As PdfReference In kids.Elements
                    Dim childDictionary As New PdfDictionary(New System.Collections.Hashtable(fieldDictionary.Dictionary))
                    ' these are not inheritable
                    childDictionary.Dictionary.Remove(KidsName)
                    childDictionary.Dictionary.Remove(TName)
                    childDictionary.Dictionary.Remove(AAName)
                    childDictionary.Dictionary.Remove(TMName)
                    childDictionary.Dictionary.Remove(TUName)
                    childDictionary.Dictionary.Remove(ParentName)
                    fields.AddRange(GetPdfFields(child, reader, childDictionary, fieldName))
                Next

                Return fields.ToArray(GetType(PdfField))
            End If
        End Function

        ''' <summary>
        ''' Returns true if changes were made to the field after it was created.
        ''' </summary>
        ''' <returns>true if there are changes.</returns>
        Public Function HasChanged() As Boolean
            Return Not Me.m_original.Equals(Me.ToString())
        End Function

        ''' <summary>
        ''' Returns the string representation of this form field.
        ''' </summary>
        ''' <returns>The string representation of this form field.</returns>
        Public Overrides Function ToString() As String
            Return Me.GetString()
        End Function

        Private Function GetString() As String
            Dim s As String = ObjectNumber.ToString(CultureInfo.InvariantCulture) & " " + Me.m_generationNumber & " obj" & vbLf
            s &= Me.m_fieldDictionary.ToString()
            s &= "\nendobj" & vbLf

            Return s
        End Function

        ''' <summary>
        ''' Gets or sets the name of this field. This is not the complete field name, just the local part,
        ''' e.g. "c" where the complete field name is "a.b.c".
        ''' </summary>
        ''' <value>The name of this form field.</value>
        Public Property Name As String
            Get
                If (Me.m_fieldDictionary.Dictionary.ContainsKey(TName) AndAlso Me.m_fieldDictionary.Dictionary(TName).GetType() Is GetType(PdfString)) Then
                    Dim xname As PdfString = Me.m_fieldDictionary.Dictionary(TName)
                    Return xname.Text
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                Dim s As String = "()"
                Dim aname As New PdfString(False, s)
                aname.Text = value
                Me.m_fieldDictionary.SetElement(TName, aname)
            End Set
        End Property

        ''' <summary>
        ''' The complete field name.
        ''' </summary>
        Private m_completeFieldName As String

                ''' <summary>
                ''' Gets or Sets the complete field name.
                ''' </summary>
        Public Property FieldName As String
			get
                Return Me.m_completeFieldName
            End Get
            Set(value As String)
                Me.m_completeFieldName = value
            End Set
        End Property

                ''' <summary>
                ''' Returns the bit at the specified position in the field flags of this field.
                ''' </summary>
                ''' <param name="bitPosition">The bit position starting at 1.</param>
                ''' <returns>true if the bit's value is 1.</returns>
        Public Function GetBit(ByVal bitPosition As Integer) As Boolean
            Dim bit As Boolean = False
            If (Me.m_fieldDictionary.Dictionary.ContainsKey(FFName) AndAlso Me.m_fieldDictionary.Dictionary(FFName).GetType() Is GetType(PdfNumber)) Then
                Dim number As PdfNumber = Me.m_fieldDictionary.Dictionary(FFName)
                bit = Math.TestBit(CInt(number.Number), bitPosition - 1) '((int)number.Number & (1 << (bitPosition - 1))) > 0;
            End If

            Return bit
        End Function

        ''' <summary>
        ''' Sets the bit at the specified position in the field flags of this field.
        ''' </summary>
        ''' <param name="bitPosition">The bit position starting at 1.</param>
        ''' <param name="bit">true to set the bit's value to 1.</param>
        Public Sub SetBit(ByVal bitPosition As Byte, ByVal bit As Boolean)
            Dim num As Integer = 0
            If (Me.m_fieldDictionary.Dictionary.ContainsKey(FFName) AndAlso Me.m_fieldDictionary.Dictionary(FFName).GetType() Is GetType(PdfNumber)) Then
                num = DirectCast(Me.m_fieldDictionary.Dictionary(FFName), PdfNumber).Number
            End If

            num = Math.SetBit(num, bitPosition - 1, bit) ' | (1 << (bitPosition - 1));
            '         If (bit) Then
            '             num = Math.SetBit(num, bitPosition - 1) ' | (1 << (bitPosition - 1));
            'Else
            '	num = (int)num & ~(1 << (bitPosition - 1));
            '}

            Me.m_fieldDictionary.SetElement(FFName, New PdfNumber(num.ToString(CultureInfo.InvariantCulture)))
        End Sub


    End Class


End Namespace