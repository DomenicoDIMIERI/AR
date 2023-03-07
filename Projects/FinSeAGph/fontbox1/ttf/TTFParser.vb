Imports System.IO
Imports FinSeA.Io

Namespace org.fontbox.ttf

    '/**
    ' * A true type font file parser.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class TTFParser

        '    '/**
        '    ' * A simple command line program to test parsing of a TTF file. <br/>
        '    ' * usage: java org.pdfbox.ttf.TTFParser &lt;ttf-file&gt;
        '    ' * 
        '    ' * @param args The command line arguments.
        '    ' * 
        '    ' * @throws IOException If there is an error while parsing the font file.
        '    ' */
        'public Shared Sub main( String[] args ) throws IOException
        '{
        '    if( args.length != 1 )
        '    {
        '        System.err.println( "usage: java org.pdfbox.ttf.TTFParser <ttf-file>" );
        '        System.exit( -1 );
        '    }
        '    TTFParser parser = new TTFParser();
        '    TrueTypeFont font = parser.parseTTF( args[0] );
        '    System.out.println( "Font:" + font );
        '}

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
            Dim initialized As FinSeA.List = New FinSeA.ArrayList()
            'need to initialize a couple tables in a certain order
            Dim head As HeaderTable = font.getHeader()
            raf.seek(head.getOffset())
            head.initData(font, raf)
            initialized.add(head)


            Dim hh As HorizontalHeaderTable = font.getHorizontalHeader()
            raf.seek(hh.getOffset())
            hh.initData(font, raf)
            initialized.add(hh)

            Dim maxp As MaximumProfileTable = font.getMaximumProfile()
            raf.seek(maxp.getOffset())
            maxp.initData(font, raf)
            initialized.add(maxp)

            Dim post As PostScriptTable = font.getPostScript()
            raf.seek(post.getOffset())
            post.initData(font, raf)
            initialized.add(post)

            Dim loc As IndexToLocationTable = font.getIndexToLocation()
            raf.seek(loc.getOffset())
            loc.initData(font, raf)
            initialized.add(loc)

            'Iterator iter = font.getTables().iterator();
            '   While (iter.hasNext())
            '{
            '   TTFTable table = (TTFTable)iter.next();
            For Each table As TTFTable In font.getTables
                If (Not initialized.contains(table)) Then
                    raf.seek(table.getOffset())
                    table.initData(font, raf)
                End If
            Next
            Return font
        End Function

        Private Function readTableDirectory(ByVal raf As TTFDataStream) As TTFTable
            Dim retval As TTFTable = Nothing
            Dim tag As String = raf.readString(4)
            If (tag.equals(CMAPTable.TAG)) Then
                retval = New CMAPTable()
            ElseIf (tag.equals(GlyphTable.TAG)) Then
                retval = New GlyphTable()
            ElseIf (tag.Equals(HeaderTable.TAG)) Then
                retval = New HeaderTable()
            ElseIf (tag.Equals(HorizontalHeaderTable.TAG)) Then
                retval = New HorizontalHeaderTable()
            ElseIf (tag.Equals(HorizontalMetricsTable.TAG)) Then
                retval = New HorizontalMetricsTable()
            ElseIf (tag.Equals(IndexToLocationTable.TAG)) Then
                retval = New IndexToLocationTable()
            ElseIf (tag.Equals(MaximumProfileTable.TAG)) Then
                retval = New MaximumProfileTable()
            ElseIf (tag.Equals(NamingTable.TAG)) Then
                retval = New NamingTable()
            ElseIf (tag.Equals(OS2WindowsMetricsTable.TAG)) Then
                retval = New OS2WindowsMetricsTable()
            ElseIf (tag.Equals(PostScriptTable.TAG)) Then
                retval = New PostScriptTable()
            ElseIf (tag.Equals(GlyphTable.TAG)) Then
                retval = New GlyphTable()
            ElseIf (tag.Equals(GlyphTable.TAG)) Then
                retval = New GlyphTable()
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