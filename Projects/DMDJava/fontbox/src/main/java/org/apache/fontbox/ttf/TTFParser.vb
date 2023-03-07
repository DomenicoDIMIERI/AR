Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A true type font file parser.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class TTFParser
        Inherits AbstractTTFParser

        Public Sub New()
            MyBase.New(False)
        End Sub

        Public Sub New(ByVal isEmbedded As Boolean)
            MyBase.New(isEmbedded)
        End Sub

        '/**
        ' * A simple command line program to test parsing of a TTF file. <br/>
        ' * usage: java org.pdfbox.ttf.TTFParser &lt;ttf-file&gt;
        ' * 
        ' * @param args The command line arguments.
        ' * 
        ' * @throws IOException If there is an error while parsing the font file.
        ' */
        'public static void main( String[] args ) throws IOException
        '{
        '    if( args.length != 1 )
        '    {
        '        System.err.println( "usage: java org.pdfbox.ttf.TTFParser <ttf-file>" );
        '        System.exit( -1 );
        '    }
        '    TTFParser parser = new TTFParser();
        '    TrueTypeFont font = parser.parseTTF( args(0) );
        '    System.out.println( "Font:" &  font );
        '}

        Protected Overrides Sub parseTables(ByVal font As TrueTypeFont, ByVal raf As TTFDataStream)
            MyBase.parseTables(font, raf)

            ' check others mandatory tables
            If (font.getCMAP() Is Nothing) Then
                Throw New IOException("cmap is mandatory")
            End If
        End Sub

    End Class

End Namespace