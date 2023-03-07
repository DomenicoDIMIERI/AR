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
    Public Class PdfReader

        Private m_pdf As String
        Private m_fields() As PdfField ' = new PdfField(0);
        Private m_fieldsByName As New System.Collections.Hashtable()
        Private m_form As PdfForm
        Private Shared ReadOnly xrefRegex As New Regex("startxref\s*(\d+)\s*%%EOF", RegexOptions.Singleline)
        Private Shared ReadOnly trailerRegex As New Regex("trailer\s*<<(.*?)>>", RegexOptions.Singleline)
        Private Shared ReadOnly rootRegex As New Regex("/Root\s*(\d+)\s+(\d+)\s*R", RegexOptions.Singleline)
        Private Shared ReadOnly sizeRegex As New Regex("/Size\s*(\d+)", RegexOptions.Singleline)
        Private Shared ReadOnly nullRegex As New Regex("\sxref\s+0\s+\d+\s+(\d+)", RegexOptions.Singleline)
        Private Shared ReadOnly refRegex As New Regex("\s*((\d+)\s+(\d+))?\s*(\d{10})\s+(\d{5})\s+(n|f)", RegexOptions.Singleline)
        Private Shared ReadOnly objectRegex As New Regex("(\d+)\s+(\d+)\s+obj(.*?)endobj", RegexOptions.Singleline)
        Private Shared ReadOnly fieldRegex As New Regex("^\s*<<(.*?/FT\s+/(Btn|Tx|Ch).*>>)", RegexOptions.Singleline)
        Private Shared ReadOnly formRegex As New Regex("^\s*<<(.*?/Fields\s+\[.*>>)", RegexOptions.Singleline)
        Private Shared ReadOnly acroFormRegex As New Regex("/AcroForm\s+(\d+)\s+(\d+)", RegexOptions.Singleline)
        Private Shared ReadOnly trailRegex As New Regex("trailer\s+<<(.*>>)(\s+startxref\s+(\d+))?", RegexOptions.Singleline)
        Private Shared ReadOnly objRegex As New Regex("(\d+)\s+(\d+)\s+obj", RegexOptions.Singleline)
        Private Shared ReadOnly linearizedRegex As New Regex("/Linearized\s+1", RegexOptions.Singleline)

        Private m_previous As Integer = -1 ' location of the previous cross-reference table
        Private m_previousTrailer As PdfDictionary
        Private m_rootObjectNumber As Integer
        Private m_nullOffset As Integer
        Private m_linearized As Boolean = False

        Private m_objects As New System.Collections.Hashtable()
        Private m_offsets As New System.Collections.SortedList()

        Public Sub New()
            ReDim Me.m_fields(0)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of PdfReader with the specified Stream.
        ''' </summary>
        ''' <param name="stream">The Stream containing the PDF data.</param>
        Public Sub New(ByVal stream As Stream)
            'DOMConfigurator.ConfigureAndWatch(new FileInfo("PdfReader.exe.log4net"));
            Me.New()
            Dim buffer() As Byte
            ReDim buffer(stream.Length - 1)
            stream.Read(buffer, 0, stream.Length)

            Dim chars() As Char
            ReDim chars(buffer.Length - 1)

            For i As Integer = 0 To buffer.Length - 1
                chars(i) = Convert.ToChar(buffer(i))
            Next
            Me.m_pdf = New String(chars)

            Me.Parse()
        End Sub

        ''' <summary>
        ''' Returns the PDF object referenced by the specified PDF reference.
        ''' </summary>
        ''' <param name="reference">The reference to the object.</param>
        ''' <returns>The PDF object referenced.</returns>
        Public Function GetObjectForReference(ByVal reference As PdfReference) As PDFObject
            Dim PdfObject As PDFObject = Nothing

            ' is the object active?
            If (Me.m_objects.Contains(reference.ObjectNumber)) Then
                Dim entry As PdfCrossReferenceEntry = Me.m_objects(reference.ObjectNumber)
                If (entry.Active) Then
                    Dim start As Integer = entry.Offset
                    Dim [end] As Integer = GetEndOfObject(reference.ObjectNumber)

                    PdfObject = ParseObject(start, [end])
                End If
            End If

            Return PdfObject
        End Function

        ''' <summary>
        ''' Gets or sets a value indicating whether the file is linearized.
        ''' Refer to the PDF Reference, Appendix F.
        ''' </summary>
        Public Property Linearized As Boolean
			get
                Return Me.m_linearized
            End Get
            Set(value As Boolean)
                Me.m_linearized = value
            End Set
        End Property

        Private Sub Parse()
            Dim fieldsList As New System.Collections.ArrayList()
            Dim startTrailer As Integer
            Dim endPosition As Integer = Me.m_pdf.Length - 1
            Dim startxref As Integer
            Dim prevName As New PdfName("/Prev")
            Dim sizeName As New PdfName("/Size")
            Dim rootName As New PdfName("/Root")
            Dim match As Match

            ' hack to determine if PDF is linearized:
            ' check for the occurence of "/Linearized 1" before the first
            ' occurence of "endobj"
            match = linearizedRegex.Match(Me.m_pdf, 0, Me.m_pdf.IndexOf("endobj"))
            If (Match.Success) Then Me.m_linearized = True
            
            ' succesively parse all trailers starting from the end
            startTrailer = Me.m_pdf.LastIndexOf("trailer", endPosition)
            While (startTrailer >= 0)
                match = trailRegex.Match(Me.m_pdf, startTrailer, endPosition - startTrailer + 1)

                If (match.Success) Then
                    Dim trailerString As String = match.Groups(1).Value
                    Dim trailerDictionary As New PdfDictionary(trailerString)

                    startxref = -1
                    ' in a hybrid-reference file (PDF 1.5), the trailer doesn't seem to
                    ' always include a startxref.
                    If (match.Groups(2).Success) Then
                        startxref = Int32.Parse(match.Groups(3).Value, CultureInfo.InvariantCulture)
                    End If

                    If (Me.m_previous = -1) Then
                        Me.m_previous = startxref
                    End If

                    ' we don't believe the startxref field.
                    ' it can be bogus due to linearization.
                    ' we'll keep the "previous" value from above though, since
                    ' that's what taft@adobe.com says in a 1998 post on comp.text.pdf :-)
                    startxref = Me.m_pdf.LastIndexOf("xref", startTrailer)

                    If (Me.m_previousTrailer Is Nothing OrElse Me.m_linearized) Then
                        Me.m_previousTrailer = trailerDictionary
                    End If

                    ' parse the xref table that is referenced by this trailer
                    Me.ParseXRef(startxref)

                    ' the offset of the first object marks the start of the body and is therefore
                    ' the end of the previous trailer
                    If (Me.m_offsets.Count > 0) Then
                        endPosition = DirectCast(Me.m_offsets.GetByIndex(0), PdfCrossReferenceEntry).Offset
                    Else
                        ' if the xref table has no objects then the previous trailer must
                        ' be right in front of it
                        endPosition = startxref
                    End If
                End If
                startTrailer = Me.m_pdf.LastIndexOf("trailer", endPosition)
            End While

            Me.m_rootObjectNumber = DirectCast(Me.m_previousTrailer("/Root"), PdfReference).ObjectNumber

            Me.ParseAcroForm()

            If (Me.m_form IsNot Nothing) Then
                Dim fieldsObject As PDFObject = Me.m_form.FieldDictionary("/Fields")
                Dim fieldsArray As PdfArray = fieldsObject
                If (fieldsArray Is Nothing) Then
                    Dim fieldsReference As PdfReference = fieldsObject
                    fieldsArray = GetObjectForReference(fieldsReference)
                End If

                For Each fieldReference As PdfReference In fieldsArray.Elements
                    Dim fields() As PdfField = PdfField.GetPdfFields(fieldReference, Me, Nothing, "")

                    For Each field As PdfField In fields
                        Trace.Assert(field IsNot Nothing)

                        fieldsList.Add(field)

                        ' make field name unique
                        Dim fieldName As String = field.Name
                        Dim i As Integer = 0
                        While (FieldsByName.Contains(fieldName))
                            fieldName = String.Format(CultureInfo.InvariantCulture, "{0}[{1}]", field.Name, i)
                            i += 1
                        End While

                        FieldsByName.Add(fieldName, field)
                    Next
                Next

                Me.m_fields = fieldsList.ToArray(GetType(PdfField))
            End If
        End Sub


        Private Sub ParseXRef(ByVal startxref As Integer)
            Dim objNumber As Integer = 0
            Dim generationNumber As Integer
            Dim offset As Integer
            Dim entry As PdfCrossReferenceEntry
            Dim endOfXRef As Integer = Me.m_pdf.IndexOf("trailer", startxref + 4)
            Dim xRef As String = Me.m_pdf.Substring(startxref + 4, endOfXRef - (startxref + 4) + 1)
            Dim refMatches As MatchCollection = refRegex.Matches(xRef)

            For Each refMatch As Match In refMatches
                If (Not refMatch.Groups(2).Value.Equals("")) Then
                    objNumber = Int32.Parse(refMatch.Groups(2).Value, CultureInfo.InvariantCulture)
                End If

                offset = Int32.Parse(refMatch.Groups(4).Value, CultureInfo.InvariantCulture)
                generationNumber = Int32.Parse(refMatch.Groups(5).Value, CultureInfo.InvariantCulture)

                If (refMatch.Groups(6).Value.Equals("n")) Then
                    entry = New PdfCrossReferenceEntry(objNumber, generationNumber, offset, refMatch.Groups(6).Value.Equals("n"))

                    If (Not Me.m_objects.Contains(objNumber)) Then
                        Me.m_offsets.Add(offset, entry)
                        Me.m_objects.Add(objNumber, entry)
                    End If
                ElseIf (objNumber = 0) Then
                    ' special case:
                    ' in order to build a new xref table we need the first free object number
                    Me.m_nullOffset = offset
                End If

                objNumber += 1
            Next
        End Sub

        ''' <summary>
        ''' Gets the end offset of the specified object.
        ''' The offset is determined by the beginning offset of the object with next higher start offset.
        ''' If the object is the last object, -1 is returned.
        ''' If the document was updated, the offset may be after the xref table and trailer that
        ''' follow the specified object.
        ''' </summary>
        ''' <param name="objNumber">The object number to find the offset for.</param>
        ''' <returns>The end offset of the specified object or -1 if it is the last object</returns>
        Private Function GetEndOfObject(ByVal objNumber As Integer) As Integer
            Dim theObject As PdfCrossReferenceEntry = Me.m_objects(objNumber)
            Dim objectIndex As Integer = Me.m_offsets.IndexOfKey(theObject.Offset)

            If ((objectIndex + 1) < Me.m_offsets.Count) Then
                Dim nextObject As PdfCrossReferenceEntry = Me.m_offsets.GetByIndex(objectIndex + 1)
                Return nextObject.Offset
            Else
                Return -1
            End If
        End Function

        ''' <summary>
        ''' Parses the Interactive Form Dictionary referenced by the AcroForm entry in
        ''' the Document Catalog.
        ''' </summary>
        Private Sub ParseAcroForm()
            Dim documentCatalog As PdfCrossReferenceEntry = Me.m_objects(Me.m_rootObjectNumber)
            Dim documentCatalogStart As Integer = documentCatalog.Offset
            Dim documentCatalogEnd As Integer = GetEndOfObject(documentCatalog.ObjectNumber)

            Dim documentCatalogObject As PdfDictionary = ParseObject(documentCatalogStart, documentCatalogEnd)
            Dim acroFormObject As PDFObject = documentCatalogObject("/AcroForm")

            If (TypeOf (acroFormObject) Is PdfReference) Then
                Dim acroFormRef As PdfReference = acroFormObject

                ' extract the AcroForm object
                Dim acroNumber As Integer = acroFormRef.ObjectNumber
                Dim acroGeneration As Integer = acroFormRef.GenerationNumber
                Dim acroStart As Integer = DirectCast(Me.m_objects(acroNumber), PdfCrossReferenceEntry).Offset
                Dim acroEnd As Integer = GetEndOfObject(acroNumber)

                Dim formDictionary As PdfDictionary = ParseObject(acroStart, acroEnd)
                Me.m_form = New PdfForm(acroNumber, acroGeneration, formDictionary)
            End If
        End Sub

        Private Function ParseObject(ByVal start As Integer, ByVal [end] As Integer) As PDFObject
            Dim match As Match
            If ([end] < 0) Then
                match = objRegex.Match(Me.m_pdf, start)
			Else
                match = objRegex.Match(Me.m_pdf, start, [end] - start)
            End If

            If (match.Success) Then
                Dim objNumber As Integer = Int32.Parse(match.Groups(1).Value, CultureInfo.InvariantCulture)
                Dim generationNumber As Integer = Int32.Parse(match.Groups(2).Value, CultureInfo.InvariantCulture)
                Dim endOfMatch As Integer = match.Index + match.Length

                Dim afterObj As String
                If ([end] < 0) Then
                    afterObj = Me.m_pdf.Substring(endOfMatch)
                Else
                    afterObj = Me.m_pdf.Substring(endOfMatch, [end] - endOfMatch + 1)
                End If
                Return PDFObject.GetPdfObject(afterObj)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Writes the PDF out to the specified stream including all updates made after this object was created.
        ''' </summary>
        ''' <param name="stream">The <see cref="System.IO.Stream"/> the PDF will be written to.</param>
        Public Sub WritePdf(ByVal stream As Stream)
            Dim lines As String = Me.m_pdf & GetUpdate()

            Dim buffer() As Byte
            ReDim buffer(lines.Length - 1)

            For i As Integer = 0 To lines.Length - 1
                buffer(i) = Convert.ToByte(lines(i))
            Next

            stream.Write(buffer, 0, buffer.Length)
        End Sub

        Private Function GetUpdate() As String
            If (Me.m_form IsNot Nothing) Then
                Dim offset As Integer = Me.m_pdf.Length
                Dim update As String = ""
                Dim xref As String = "xref" & vbLf
                xref &= "0 1" & vbLf
                xref &= Me.m_nullOffset.ToString("0000000000", CultureInfo.InvariantCulture) & " 65535 f " & vbLf

                ' write the AcroForm object
                Dim formString As String = Me.m_form.ToString()
                update &= formString
                xref &= Me.m_form.ObjectNumber.ToString(CultureInfo.InvariantCulture) & " 1" & vbLf
                xref &= offset.ToString("0000000000", CultureInfo.InvariantCulture) & " " & Me.m_form.GenerationNumber.ToString("00000", CultureInfo.InvariantCulture) & " n " & vbLf
                offset &= formString.Length

                For Each field As PdfField In Me.m_fields
                    If (field.HasChanged()) Then
                        Dim fieldString As String = field.ToString()
                        update &= fieldString
                        xref &= field.ObjectNumber.ToString(CultureInfo.InvariantCulture) & " 1" & vbLf
                        xref &= offset.ToString("0000000000", CultureInfo.InvariantCulture) & " " & field.GenerationNumber.ToString("00000", CultureInfo.InvariantCulture) + " n " & vbLf
                        offset &= fieldString.Length
                    End If
                Next

                Dim trailer As String = GetTrailer(offset)

                Return update & xref & trailer
            Else
                Return ""
            End If
        End Function

        Private Function GetTrailer(ByVal xrefOffset As Integer) As String
            Dim trailerHash As New System.Collections.Hashtable()
            Dim prevName As New PdfName("/Prev")
            Dim rootName As New PdfName("/Root")
            Dim sizeName As New PdfName("/Size")

            trailerHash(prevName) = New PdfNumber(Me.m_previous)
            trailerHash(rootName) = Me.m_previousTrailer("/Root")
            trailerHash(sizeName) = Me.m_previousTrailer("/Size")

            Dim newTrailer As New PdfDictionary(trailerHash)

            Dim s As String = ""
            s &= "trailer" & vbLf
            s &= newTrailer.ToString & vbLf
            s &= "startxref" & vbLf
            s &= xrefOffset.ToString(CultureInfo.InvariantCulture) & vbLf
            s &= "%%EOF" & vbLf

            Return s
        End Function

        ''' <summary>
        ''' Gets a collection of all form fields.
        ''' </summary>
        Public ReadOnly Property Fields As PdfField()
            Get
                Return Me.m_fields
            End Get
        End Property

        ''' <summary>
        ''' Gets a Hashtable of all form fields keyed by their name.
        ''' </summary>
        Public ReadOnly Property FieldsByName As System.Collections.Hashtable
            Get
                Return Me.m_fieldsByName
            End Get
        End Property

        '''' <summary>
        '               ''' Reads the first file on the command line, parses it and writes it to the second file on the command line.
        '               ''' </summary>
        '               ''' <param name="args">Two filenames, the first must be a PDF file, the second will be written to.</param>
        '	public Shared Sub Main(string[] args)
        '	{
        '		if (args.Length != 2)
        '		{
        '			Console.Error.WriteLine("Usage: " + Environment.GetCommandLineArgs()(0) + " file1 file2");
        '			Console.Error.WriteLine("Reads file1, parses it as a PDF file, and writes to file2");
        '		}
        '           Else
        '		{
        '			PdfReader reader = PdfReader.GetPdfReader(args(0));

        '			foreach (PdfField field in reader.Fields)
        '			{
        '				Debug.WriteLine(field.FieldName);
        '			}

        '			FileStream fileStream = new FileStream(args(1), System.IO.FileMode.Create);
        '			reader.WritePdf(fileStream);
        '			fileStream.Close();
        '		}
        '	}
        '}

        ''' <summary>
        ''' Initializes a new instance of PdfReader with the specified file.
        ''' </summary>
        ''' <param name="name">The file containing the PDF data.</param>
        Public Shared Function GetPdfReader(ByVal name As String) As PdfReader
            Dim reader As PdfReader

            Using stream As New FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read)
                reader = New PdfReader(stream)
            End Using

            Return reader
        End Function

    End Class


End Namespace