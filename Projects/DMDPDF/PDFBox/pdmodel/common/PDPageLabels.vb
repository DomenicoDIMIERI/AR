Imports System.IO
Imports FinSeA.Io
Imports System.Text
Imports FinSeA.Text

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * Represents the page label dictionary of a document.
    ' * 
    ' * @author <a href="mailto:igor.podolskiy@ievvwi.uni-stuttgart.de">Igor
    ' *         Podolskiy</a>
    ' * @version $Revision$
    ' */
    Public Class PDPageLabels
        Implements COSObjectable

        Private labels As SortedMap(Of NInteger, PDPageLabelRange)

        Private doc As PDDocument

        '/**
        ' * Creates an empty page label dictionary for the given document.
        ' * 
        ' * <p>
        ' * Note that the page label dictionary won't be automatically added to the
        ' * document; you will still need to do it manually (see
        ' * {@link PDDocumentCatalog#setPageLabels(PDPageLabels)}.
        ' * </p>
        ' * 
        ' * @param document
        ' *            The document the page label dictionary is created for.
        ' * @see PDDocumentCatalog#setPageLabels(PDPageLabels)
        ' */
        Public Sub New(ByVal document As PDDocument)
            labels = New TreeMap(Of NInteger, PDPageLabelRange)
            Me.doc = document
            Dim defaultRange As PDPageLabelRange = New PDPageLabelRange()
            defaultRange.setStyle(PDPageLabelRange.STYLE_DECIMAL)
            labels.put(0, defaultRange)
        End Sub

        '/**
        ' * Creates an page label dictionary for a document using the information in
        ' * the given COS dictionary.
        ' * 
        ' * <p>
        ' * Note that the page label dictionary won't be automatically added to the
        ' * document; you will still need to do it manually (see
        ' * {@link PDDocumentCatalog#setPageLabels(PDPageLabels)}.
        ' * </p>
        ' * 
        ' * @param document
        ' *            The document the page label dictionary is created for.
        ' * @param dict
        ' *            an existing page label dictionary
        ' * @see PDDocumentCatalog#setPageLabels(PDPageLabels)
        ' * @throws IOException
        ' *             If something goes wrong during the number tree conversion.
        ' */
        Public Sub New(ByVal document As PDDocument, ByVal dict As COSDictionary)  'throws IOException
            Me.New(document)
            If (dict Is Nothing) Then
                Return
            End If
            Dim root As PDNumberTreeNode = New PDNumberTreeNode(dict, GetType(COSDictionary))
            findLabels(root)
        End Sub

        Private Sub findLabels(ByVal node As PDNumberTreeNode) 'throws IOException 
            If (node.getKids() IsNot Nothing) Then
                Dim kids As List(Of PDNumberTreeNode) = node.getKids()
                For Each kid As PDNumberTreeNode In kids
                    findLabels(kid)
                Next
            ElseIf (node.getNumbers() IsNot Nothing) Then
                Dim numbers As Map(Of NInteger, COSObjectable) = node.getNumbers()
                For Each i As Map.Entry(Of NInteger, COSObjectable) In numbers.entrySet()
                    If (i.Key >= 0) Then
                        labels.put(i.Key, New PDPageLabelRange(i.Value))
                    End If
                Next
            End If
        End Sub


        '/**
        ' * Returns the number of page label ranges.
        ' * 
        ' * <p>
        ' * This will be always &gt;= 1, as the required default entry for the page
        ' * range starting at the first page is added automatically by this
        ' * implementation (see PDF32000-1:2008, p. 375).
        ' * </p>
        ' * 
        ' * @return the number of page label ranges.
        ' */
        Public Function getPageRangeCount() As Integer
            Return labels.size()
        End Function

        '/**
        ' * Returns the page label range starting at the given page, or {@code null}
        ' * if no such range is defined.
        ' * 
        ' * @param startPage
        ' *            the 0-based page index representing the start page of the page
        ' *            range the item is defined for.
        ' * @return the page label range or {@code null} if no label range is defined
        ' *         for the given start page.
        ' */
        Public Function getPageLabelRange(ByVal startPage As Integer) As PDPageLabelRange
            Return labels.get(startPage)
        End Function

        '/**
        ' * Sets the page label range beginning at the specified start page.
        ' * 
        ' * @param startPage
        ' *            the 0-based index of the page representing the start of the
        ' *            page label range.
        ' * @param item
        ' *            the page label item to set.
        ' */
        Public Sub setLabelItem(ByVal startPage As Integer, ByVal item As PDPageLabelRange)
            labels.put(startPage, item)
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Dim dict As COSDictionary = New COSDictionary()
            Dim arr As COSArray = New COSArray()
            For Each i As Map.Entry(Of NInteger, PDPageLabelRange) In labels.entrySet()
                arr.add(COSInteger.get(i.Key))
                arr.add(i.Value)
            Next
            dict.setItem(COSName.NUMS, arr)
            Return dict
        End Function

        '/**
        ' * Returns a mapping with computed page labels as keys and corresponding
        ' * 0-based page indices as values. The returned map will contain at most as
        ' * much entries as the document has pages.
        ' * 
        ' * <p>
        ' * <strong>NOTE:</strong> If the document contains duplicate page labels,
        ' * the returned map will contain <em>less</em> entries than the document has
        ' * pages. The page index returned in this case is the <em>highest</em> index
        ' * among all pages sharing the same label.
        ' * </p>
        ' * 
        ' * @return a mapping from labels to 0-based page indices.
        ' */
        Public Function getPageIndicesByLabels() As Map(Of String, NInteger)
            Dim labelMap As Map(Of String, NInteger) = New HashMap(Of String, NInteger)(doc.getNumberOfPages())
            computeLabels(New LblHandler1(labelMap)) ' new LabelHandler()
            '{
            '    public void newLabel(int pageIndex, String label)
            '    {
            '        labelMap.put(label, pageIndex);
            '    }
            '});
            Return labelMap
        End Function

        Private Class LblHandler1
            Implements LabelHandler

            Public labelMap As Map(Of String, NInteger)

            Public Sub New(ByVal m As Map(Of String, NInteger))
                Me.labelMap = m
            End Sub

            Private Sub newLabel(ByVal pageIndex As Integer, ByVal label As String) Implements LabelHandler.newLabel
                labelMap.put(label, pageIndex)
            End Sub

        End Class

        '/**
        ' * Returns a mapping with 0-based page indices as keys and corresponding
        ' * page labels as values as an array. The array will have exactly as much
        ' * entries as the document has pages.
        ' * 
        ' * @return an array mapping from 0-based page indices to labels.
        ' */
        Public Function getLabelsByPageIndices() As String()
            Dim map() As String = Array.CreateInstance(GetType(String), doc.getNumberOfPages())
            computeLabels(New LblHandler2(doc, map)) 'new LabelHandler()
            '{
            '    public void newLabel(int pageIndex, String label)
            '    {
            '    If (pageIndex < doc.getNumberOfPages()) Then
            '        { 
            '            map[pageIndex] = label;
            '        }
            '    }
            '});
            Return map
        End Function

        Private Class LblHandler2
            Implements LabelHandler

            Public doc As PDDocument
            Public map() As String

            Public Sub New(ByVal doc As PDDocument, ByVal m() As String)
                Me.map = m
            End Sub

            Private Sub newLabel(ByVal pageIndex As Integer, ByVal label As String) Implements LabelHandler.newLabel
                If (pageIndex < doc.getNumberOfPages()) Then
                    map(pageIndex) = label
                End If
            End Sub

        End Class



        '/**
        ' * Internal interface for the control flow support.
        ' * 
        ' * @author Igor Podolskiy
        ' */
        Private Interface LabelHandler
            Sub newLabel(ByVal pageIndex As Integer, ByVal label As String)
        End Interface

        Private Sub computeLabels(ByVal handler As LabelHandler)
            Dim iterator As Global.System.Collections.Generic.IEnumerator(Of Map.Entry(Of NInteger, PDPageLabelRange)) = labels.entrySet().iterator() ', PDPageLabelRange)
            If (Not iterator.MoveNext) Then Return
            Dim pageIndex As Integer = 0
            Dim lastEntry As Map.Entry(Of NInteger, PDPageLabelRange) = iterator.Current
            Dim gen As LabelGenerator

            For Each entry As Map.Entry(Of NInteger, PDPageLabelRange) In labels.entrySet ', 'While (iterator.hasNext())
                'Dim entry As Map.Entry(Of NInteger, PDPageLabelRange) = iterator.next()
                Dim numPages As Integer = entry.Key - lastEntry.Key
                gen = New LabelGenerator(lastEntry.Value, numPages)
                'For Each item As Object In gen ''While (gen.hasNext())
                While (gen.hasNext)
                    handler.newLabel(pageIndex, gen.next) 'gen.next())
                    pageIndex += 1
                End While
                lastEntry = entry
            Next 'End While
            gen = New LabelGenerator(lastEntry.Value, doc.getNumberOfPages() - lastEntry.Key)
            While (gen.hasNext)
                handler.newLabel(pageIndex, gen.next())
                pageIndex += 1
            End While
        End Sub

        '/**
        ' * Generates the labels in a page range.
        ' * 
        ' * @author Igor Podolskiy
        ' * 
        ' */
        Private Class LabelGenerator
            Implements Global.System.Collections.Generic.IEnumerator(Of String) ' Iterator(Of String)

            Private labelInfo As PDPageLabelRange
            Private numPages As Integer
            Private currentPage As Integer

            Public Sub New(ByVal label As PDPageLabelRange, ByVal pages As Integer)
                Me.labelInfo = label
                Me.numPages = pages
                Me.currentPage = 0
            End Sub

            Public Function hasNext() As Boolean
                Return currentPage < numPages
            End Function

            Public Function [next]() As String
                If (Not hasNext()) Then
                    Throw New Global.System.Collections.Generic.KeyNotFoundException()
                End If
                Dim buf As StringBuilder = New StringBuilder()
                If (labelInfo.getPrefix() IsNot Nothing) Then
                    Dim label As String = labelInfo.getPrefix()
                    ' there may be some labels with some null bytes at the end
                    ' which will lead to an incomplete output, see PDFBOX-1047
                    While (label.LastIndexOf(0) <> -1)
                        label = label.Substring(0, label.Length() - 1)
                    End While
                    buf.Append(label)
                End If
                If (labelInfo.getStyle() IsNot Nothing) Then
                    buf.Append(getNumber(labelInfo.getStart() + currentPage, labelInfo.getStyle()))
                End If
                currentPage += 1
                Return buf.ToString()
            End Function

            Private Function getNumber(ByVal pageIndex As Integer, ByVal style As String) As String
                If (PDPageLabelRange.STYLE_DECIMAL.Equals(style)) Then
                    Return CStr(pageIndex) ' Integer.ToString(pageIndex)
                ElseIf (PDPageLabelRange.STYLE_LETTERS_LOWER.Equals(style)) Then
                    Return makeLetterLabel(pageIndex)
                ElseIf (PDPageLabelRange.STYLE_LETTERS_UPPER.Equals(style)) Then
                    Return makeLetterLabel(pageIndex).ToUpper()
                ElseIf (PDPageLabelRange.STYLE_ROMAN_LOWER.Equals(style)) Then
                    Return makeRomanLabel(pageIndex)
                ElseIf (PDPageLabelRange.STYLE_ROMAN_UPPER.Equals(style)) Then
                    Return makeRomanLabel(pageIndex).ToUpper()
                Else
                    ' Fall back to decimals.
                    Return CStr(pageIndex) ' Integer.ToString(pageIndex)
                End If
            End Function

            '/**
            ' * Lookup table used by the {@link #makeRomanLabel(int)} method.
            ' */
            Private Shared ROMANS(,) As String = { _
                    {"", "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix"}, _
                    {"", "x", "xx", "xxx", "xl", "l", "lx", "lxx", "lxxx", "xc"}, _
                    {"", "c", "cc", "ccc", "cd", "d", "dc", "dcc", "dccc", "cm"} _
                 }

            Private Shared Function makeRomanLabel(ByVal pageIndex As Integer) As String
                Dim buf As StringBuilder = New StringBuilder()
                Dim power As Integer = 0
                While (power < 3 AndAlso pageIndex > 0)
                    buf.Insert(0, ROMANS(power, pageIndex Mod 10))
                    pageIndex = pageIndex / 10
                    power += 1
                End While
                '// Prepend as many m as there are thousands (which is
                '// incorrect by the roman numeral rules for numbers > 3999,
                '// but is unbounded and Adobe Acrobat does it this way).
                '// This code is somewhat inefficient for really big numbers,
                '// but those don't occur too often (and the numbers in those cases
                '// would be incomprehensible even if we and Adobe
                '// used strict Roman rules).
                For i As Integer = 0 To pageIndex - 1
                    buf.Insert(0, "m")
                Next
                Return buf.ToString()
            End Function

            '/**
            ' * A..Z, AA..ZZ, AAA..ZZZ ... labeling as described in PDF32000-1:2008,
            ' * Table 159, Page 375.
            ' */
            Private Shared Function makeLetterLabel(ByVal num As Integer) As String
                Dim buf As StringBuffer = New StringBuffer()
                Dim numLetters As Integer = num / 26 + Math.Sign(num Mod 26)
                Dim letter As Integer = num Mod 26 + 26 * (1 - Math.Sign(num Mod 26)) + 64
                For i As Integer = 0 To numLetters - 1
                    buf.appendCodePoint(letter)
                Next
                Return buf.ToString()
            End Function

            Public Sub remove()
                ' This is a generator, no removing allowed.
                Throw New NotSupportedException ' UnsupportedOperationException()
            End Sub

            Private ReadOnly Property Current As String Implements Global.System.Collections.Generic.IEnumerator(Of String).Current
                Get
                    Return Nothing
                End Get
            End Property

            Private ReadOnly Property Current1 As Object Implements Global.System.Collections.IEnumerator.Current
                Get
                    Return Nothing
                End Get
            End Property

            Private Function MoveNext() As Boolean Implements Global.System.Collections.IEnumerator.MoveNext
                Return False
            End Function

            Private Sub Reset() Implements Global.System.Collections.IEnumerator.Reset

            End Sub

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: dispose managed state (managed objects).
                    End If

                    ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                    ' TODO: set large fields to null.
                End If
                Me.disposedValue = True
            End Sub

            ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
            'Protected Overrides Sub Finalize()
            '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class

    End Class

End Namespace