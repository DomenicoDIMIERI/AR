Imports System.IO
Imports FinSeA.Io

Namespace org.apache.fontbox.ttf

    Public MustInherit Class AbstractTTFParser

        Protected isEmbedded As Boolean = False

        Public Sub New(ByVal isEmbedded As Boolean)
            Me.isEmbedded = isEmbedded
        End Sub

        '/**
        ' * Parse a file and get a true type font.
        ' * @param ttfFile The TTF file.
        ' * @return A true type font.
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Public Function parseTTF(ByVal ttfFile As String) As TrueTypeFont
            Dim raf As New RAFDataStream(ttfFile, "r")
            Return parseTTF(raf)
        End Function

        '/**
        ' * Parse a file and get a true type font.
        ' * @param ttfFile The TTF file.
        ' * @return A true type font.
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Public Function parseTTF(ByVal ttfFile As FinSeA.Io.File) As TrueTypeFont
            Dim raf As New RAFDataStream(ttfFile, "r")
            Return parseTTF(raf)
        End Function

        '/**
        ' * Parse a file and get a true type font.
        ' * @param ttfData The TTF data to parse.
        ' * @return A true type font.
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Public Function parseTTF(ByVal ttfData As InputStream) As TrueTypeFont
            Return parseTTF(New MemoryTTFDataStream(ttfData))
        End Function

        '/**
        ' * Parse a file and get a true type font.
        ' * @param raf The TTF file.
        ' * @return A true type font.
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Public Function parseTTF(ByVal raf As TTFDataStream) As TrueTypeFont
            Dim font As New TrueTypeFont(raf)
            font.setVersion(raf.read32Fixed())
            Dim numberOfTables As Integer = raf.readUnsignedShort()
            Dim searchRange As Integer = raf.readUnsignedShort()
            Dim entrySelector As Integer = raf.readUnsignedShort()
            Dim rangeShift As Integer = raf.readUnsignedShort()
            For i As Integer = 0 To numberOfTables - 1
                Dim table As TTFTable = readTableDirectory(raf)
                font.addTable(table)
            Next

            'need to initialize a couple tables in a certain order
            parseTables(font, raf)

            Return font
        End Function

        '/**
        ' * Parse all tables and check if all needed tables are present.
        ' * @param font the TrueTypeFont instance holding the parsed data.
        ' * @param raf the data stream of the to be parsed ttf font
        ' * @throws IOException If there is an error parsing the true type font.
        ' */
        Protected Overridable Sub parseTables(ByVal font As TrueTypeFont, ByVal raf As TTFDataStream)
            Dim initialized As List(Of TTFTable) = New ArrayList(Of TTFTable)()
            Dim head As HeaderTable = font.getHeader()
            If (head Is Nothing) Then
                Throw New IOException("head is mandatory")
            End If
            raf.seek(head.getOffset())
            head.initData(font, raf)
            initialized.add(head)

            Dim hh As HorizontalHeaderTable = font.getHorizontalHeader()
            If (hh Is Nothing) Then
                Throw New IOException("hhead is mandatory")
            End If
            raf.seek(hh.getOffset())
            hh.initData(font, raf)
            initialized.add(hh)

            Dim maxp As MaximumProfileTable = font.getMaximumProfile()
            If (maxp IsNot Nothing) Then
                raf.seek(maxp.getOffset())
                maxp.initData(font, raf)
                initialized.add(maxp)
            Else
                Throw New IOException("maxp is mandatory")
            End If

            Dim post As PostScriptTable = font.getPostScript()
            If (post IsNot Nothing) Then
                raf.seek(post.getOffset())
                post.initData(font, raf)
                initialized.add(post)
            ElseIf (Not isEmbedded) Then
                ' in an embedded font this table is optional
                Throw New IOException("post is mandatory")
            End If

            Dim loc As IndexToLocationTable = font.getIndexToLocation()
            If (loc Is Nothing) Then
                Throw New IOException("loca is mandatory")
            End If
            raf.seek(loc.getOffset())
            loc.initData(font, raf)
            initialized.add(loc)

            Dim cvt = False, prep = False, fpgm As Boolean = False
            Dim iter As Iterator(Of TTFTable) = font.getTables().iterator()
            While (iter.hasNext())
                Dim table As TTFTable = iter.next()
                If (Not initialized.Contains(table)) Then
                    raf.seek(table.getOffset())
                    table.initData(font, raf)
                End If
                If (table.getTag().StartsWith("cvt")) Then
                    cvt = True
                ElseIf ("prep".Equals(table.getTag())) Then
                    prep = True
                ElseIf ("fpgm".Equals(table.getTag())) Then
                    fpgm = True
                End If
            End While

            ' check others mandatory tables
            If (font.getGlyph() Is Nothing) Then
                Throw New IOException("glyf is mandatory")
            End If
            If (font.getNaming() Is Nothing AndAlso Not isEmbedded) Then
                Throw New IOException("name is mandatory")
            End If
            If (font.getHorizontalMetrics() Is Nothing) Then
                Throw New IOException("hmtx is mandatory")
            End If

            If (isEmbedded) Then
                ' in a embedded truetype font prep, cvt_ and fpgm tables 
                ' are mandatory
                If (Not fpgm) Then
                    Throw New IOException("fpgm is mandatory")
                End If
                If (Not prep) Then
                    Throw New IOException("prep is mandatory")
                End If
                If (Not cvt) Then
                    Throw New IOException("cvt_ is mandatory")
                End If
            End If
        End Sub

        Private Function readTableDirectory(ByVal raf As TTFDataStream) As TTFTable
            Dim retval As TTFTable = Nothing
            Dim tag As String = raf.readString(4)
            If (tag.equals(CMAPTable.TAG)) Then
                retval = New CMAPTable()
            ElseIf (tag.equals(GlyphTable.TAG)) Then
                retval = New GlyphTable()
            ElseIf (tag.equals(HeaderTable.TAG)) Then
                retval = New HeaderTable()
            ElseIf (tag.equals(HorizontalHeaderTable.TAG)) Then
                retval = New HorizontalHeaderTable()
            ElseIf (tag.equals(HorizontalMetricsTable.TAG)) Then
                retval = New HorizontalMetricsTable()
            ElseIf (tag.equals(IndexToLocationTable.TAG)) Then
                retval = New IndexToLocationTable()
            ElseIf (tag.equals(MaximumProfileTable.TAG)) Then
                retval = New MaximumProfileTable()
            ElseIf (tag.Equals(NamingTable.TAG)) Then
                retval = New NamingTable()
            ElseIf (tag.Equals(OS2WindowsMetricsTable.TAG)) Then
                retval = New OS2WindowsMetricsTable()
            ElseIf (tag.Equals(PostScriptTable.TAG)) Then
                retval = New PostScriptTable()
            ElseIf (tag.Equals(DigitalSignatureTable.TAG)) Then
                retval = New DigitalSignatureTable()
            Else
                'unknown table type but read it anyway.
                retval = New TTFTable()
            End If
            retval.setTag(tag)
            retval.setCheckSum(raf.readUnsignedInt())
            retval.setOffset(raf.readUnsignedInt())
            retval.setLength(raf.readUnsignedInt())
            Return retval
        End Function


    End Class

End Namespace